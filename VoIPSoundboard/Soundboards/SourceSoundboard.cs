using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using NAudio.Wave;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace HiT.VoIPSoundboard.Soundboards
{
    public enum SourcePluginMessage
    {
        PlaySound = 0x00,
        StopPlaying = 0x01,
        StartRecording = 0x02,
        StopRecording = 0x03
    }
    public class SourceSoundboard : ISoundboard
    {
        [DllImport("user32.dll")] private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("kernel32.dll")] private static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")] private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll")] private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll")] private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll")] private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
        [DllImport("kernel32.dll")] private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")] private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")] private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        static readonly string SOURCEPLUGIN_PATH = Application.StartupPath + "\\VoIPSoundboard_SourcePlugin.dll";
        WaveFormat gameVoiceSampleFormat;
        IntPtr gameMainWindowHandle;
        IntPtr loadLibraryMethodPtr;
        List<TTSBankData> ttsBanks;
        BinaryWriter messageSender;
        Thread messageReaderThread;
        Process sourceGameProcess;
        string gameRootDirectory;
        TcpClient messageClient;
        int selectedVoiceIndex;
        int selectedBankIndex;
        NotifyIcon trayIcon;
        MainForm mainForm;
        bool ttsEnabled;
        public SourceSoundboard(MainForm mainForm)
        {
            string newVoiceCode;
            string newVoiceName;
            string newCountryCode;
            bool readFirstOption = false;
            string lastCountryCode = null;
            this.ttsBanks = new List<TTSBankData>();
            using (WebClient webClient = new WebClient())
            {
                foreach (var currLine in webClient.DownloadString("http://www.voicerss.org/api/demo.aspx").Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (currLine.EndsWith("</option>"))
                    {
                        if (readFirstOption)
                        {
                            newVoiceName = null;
                            newVoiceCode = currLine.Substring(43, 5);
                            newCountryCode = currLine.Substring(43, 2);
                            for (int currLineChar = currLine.Length - 10; currLineChar >= 0; currLineChar--)
                            {
                                if (currLine[currLineChar] == '>')
                                {
                                    newVoiceName = currLine.Substring(currLineChar + 1, currLine.Length - currLineChar - 10);
                                    break;
                                }
                            }
                            if (newVoiceName != null)
                            {
                                if (newCountryCode != lastCountryCode)
                                {
                                    this.ttsBanks.Add(new TTSBankData(CultureInfo.GetCultureInfo(newCountryCode).EnglishName));
                                }
                                this.ttsBanks[this.ttsBanks.Count - 1].AddVoice(new VoiceInfo(newVoiceName, newVoiceCode));
                            }
                            lastCountryCode = newCountryCode;
                        }
                        else
                        {
                            readFirstOption = true;
                        }
                    }
                    else if (currLine.EndsWith("</select>"))
                    {
                        break;
                    }
                }
            }
            this.loadLibraryMethodPtr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            this.trayIcon = mainForm.TrayIcon;
            this.mainForm = mainForm;
            this.ttsEnabled = true;
        }
        public SourceSoundboard(MainForm mainForm, RegistryKey appRegistryKey) : this(mainForm)
        {
            object registryValue = appRegistryKey.GetValue("TTSVoiceName");
            string lastSessionVoice;
            if (registryValue != null)
            {
                lastSessionVoice = registryValue.ToString();
                if (!SelectSessionVoice(lastSessionVoice))
                {
                    SelectSessionVoice("English (United States)");
                }
            }
            else
            {
                SelectSessionVoice("English (United States)");
            }
        }
        private bool SelectSessionVoice(string sessionVoice)
        {
            for (int currBankIndex = 0; currBankIndex < this.ttsBanks.Count; currBankIndex++)
            {
                for (int currVoiceIndex = 0; currVoiceIndex < this.ttsBanks[currBankIndex].VoicesCount; currVoiceIndex++)
                {
                    if (this.ttsBanks[currBankIndex].GetVoice(currVoiceIndex).VoiceName == sessionVoice)
                    {
                        this.selectedVoiceIndex = currVoiceIndex;
                        this.selectedBankIndex = currBankIndex;
                        return true;
                    }
                }
            }
            return false;
        }
        public Process SourceGameProcess
        {
            get
            {
                return sourceGameProcess;
            }
            set
            {
                string readLine;
                sourceGameProcess = value;
                gameMainWindowHandle = IntPtr.Zero;
                gameRootDirectory = Path.GetDirectoryName(value.MainModule.FileName);
                using (StreamReader appIDReader = new StreamReader(gameRootDirectory + "\\steam_appid.txt"))
                {
                    readLine = appIDReader.ReadLine();
                    switch (readLine)
                    {
                        case "10": //Counter - Strike
                        case "20": //Team Fortress Classic
                        case "30": //Day of Defeat
                        case "40": //Deathmatch Classic
                        case "50": //Opposing Force
                        case "80": //Condition Zero
                        case "130": //Half - Life: Blue Shift
                            gameVoiceSampleFormat = new WaveFormat(8000, 16, 1);
                        break;
                        case "240": //Counter-Strike: Source
                        case "260": //Counter-Strike: Source Beta
                        case "300": //Day of Defeat: Source
                        case "320": //Half - Life 2: Deathmatch
                        case "360": //Half - Life Deathmatch: Source
                        case "440": //Team Fortress 2
                        case "500": //Left 4 Dead
                        case "550": //Left 4 Dead 2
                        case "570": //Dota 2 (Beta?)
                        case "620": //Portal 2
                        case "630": //Alien Swarm
                        case "2400": //The Ship
                        case "4000": //Garry's Mod
                        case "17510": //Age of Chivalry
                            gameVoiceSampleFormat = new WaveFormat(11025, 16, 1);
                        break;
                        case "730":
                        case "1800": //+^ Counter - Strike: Global Offensive
                            gameVoiceSampleFormat = new WaveFormat(22050, 16, 1);
                        break;
                        default:
                            trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - Unknown Source game", "VoIPSoundboard was unable to detect the source game with AppID: " + readLine + ".", ToolTipIcon.Warning);
                        break;
                    }
                }
            }
        }
        public bool TTSEnabled
        {
            get
            {
                return ttsEnabled;
            }
            set
            {
                ttsEnabled = value;
            }
        }
        public int SelectedBankIndex
        {
            get
            {
                return selectedBankIndex;
            }
            set
            {
                selectedBankIndex = value;
            }
        }
        public int SelectedVoiceIndex
        {
            get
            {
                return selectedVoiceIndex;
            }
            set
            {
                selectedVoiceIndex = value;
            }
        }
        public string SelectedTTSVoice
        {
            get
            {
                return ttsBanks[selectedBankIndex].GetVoice(selectedVoiceIndex).VoiceName;
            }
        }
        public int TTSBanksCount
        {
            get
            {
                return ttsBanks.Count;
            }
        }
        public TTSBankData GetTTSBank(int index)
        {
            return ttsBanks[index];
        }
        public void SoundboardService()
        {
            if (sourceGameProcess != null)
            {
                if (!sourceGameProcess.HasExited)
                {
                    if (messageClient == null || !messageClient.Connected || messageSender == null)
                    {
                        messageClient = new TcpClient();
                        try
                        {
                            messageClient.Connect("127.0.0.1", 60873);
                        }
                        catch (SocketException) { }
                        catch (Exception ex)
                        {
                            Utils.ShowUnhandledException(ex, "SourceSoundboard->SoundboardService");
                        }
                        if (!messageClient.Connected)
                        {
                            InjectPlugin();
                        }
                        else
                        {
                            NotifyInjection();
                        }
                    }
                    else if (gameMainWindowHandle == IntPtr.Zero)
                    {
                        gameMainWindowHandle = FindWindow("Valve001", null);
                    }
                }
                else
                {
                    if (messageSender != null)
                    {
                        messageSender.Dispose();
                    }
                    sourceGameProcess.Dispose();
                    if (messageClient.Connected)
                    {
                        messageClient.Close();
                    }
                    sourceGameProcess = null;
                    messageSender = null;
                    messageClient = null;
                }
            }
        }
        private void InjectPlugin()
        {
            const int PROCESS_VM_WRITE = 0x20;
            const int PROCESS_VM_OPERATION = 0x08;
            const int PROCESS_CREATE_THREAD = 0x02;
            const int PROCESS_QUERY_INFORMATION = 0x0400;
            IntPtr processHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION, 1, (uint)sourceGameProcess.Id);
            if (processHandle != IntPtr.Zero)
            {
                const int MEM_COMMIT = 0x1000;
                const int MEM_RELEASE = 0x800;
                const int MEM_RESERVE = 0x2000;
                const int PAGE_EXECUTE_READWRITE = 0x40;
                IntPtr parameterAddress = VirtualAllocEx(processHandle, IntPtr.Zero, (uint)SOURCEPLUGIN_PATH.Length, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
                if (parameterAddress != IntPtr.Zero)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(SOURCEPLUGIN_PATH);
                    if (WriteProcessMemory(processHandle, parameterAddress, bytes, (uint)bytes.Length, 0) != 0)
                    {
                        if (CreateRemoteThread(processHandle, IntPtr.Zero, IntPtr.Zero, loadLibraryMethodPtr, parameterAddress, 0, IntPtr.Zero) != IntPtr.Zero)
                        {
                            Thread.Sleep(1250);
                            while (messageSender == null && sourceGameProcess != null && !sourceGameProcess.HasExited)
                            {
                                messageClient = new TcpClient();
                                try
                                {
                                    messageClient.Connect("127.0.0.1", 60873);
                                }
                                catch (SocketException) { }
                                catch (Exception ex)
                                {
                                    Utils.ShowUnhandledException(ex, "SourceSoundboard->InjectPlugin");
                                }
                                if (messageClient.Connected)
                                {
                                    NotifyInjection();
                                }
                                Thread.Sleep(1250);
                            }
                        }
                    }
                    VirtualFreeEx(processHandle, parameterAddress, (uint)SOURCEPLUGIN_PATH.Length, MEM_RELEASE);
                }
                CloseHandle(processHandle);
            }
        }
        private void NotifyInjection()
        {
            NetworkStream networkStream = messageClient.GetStream();
            trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - Injection successful", "VoIPSounboard was able to inject itself to the Source Game successfully.", ToolTipIcon.Info);
            messageReaderThread = new Thread( () => ReadMessagesService(new BinaryReader(networkStream)) );
            messageSender = new BinaryWriter(networkStream);
            messageReaderThread.Start();
        }
        private void ReadMessagesService(BinaryReader messageReader)
        {
            int messageLength;
            byte[] messageBytes;
            MemoryStream ttsMP3Stream;
            while (!mainForm.IsClosed && messageClient != null && messageClient.Connected)
            {
                try
                {
                    messageLength = messageReader.ReadInt32();
                    messageBytes = messageReader.ReadBytes(messageLength);
                    if (ttsEnabled && mainForm.SBEnabled)
                    {
                        ttsMP3Stream = RequestTTSFile(Encoding.ASCII.GetString(messageBytes));
                        if (ttsMP3Stream != null)
                        {
                            using (Mp3FileReader mp3FileReader = new Mp3FileReader(ttsMP3Stream))
                            {
                                PlayFile(null, mp3FileReader);
                                mainForm.WaitForSoundEnd(mp3FileReader.TotalTime);
                                StopPlaying();
                            }
                        }
                    }
                }
                catch (IOException) { }
                catch (Exception ex)
                {
                    Utils.ShowUnhandledException(ex, "SourceSoundboard->ReadMessagesService");
                }
            }
            messageReader.Dispose();
        }
        public void StopReadMessagesService()
        {
            if (messageClient != null && messageClient.Connected)
            {
                messageClient.Close();
                messageSender.Close();
            }
        }
        public MemoryStream RequestTTSFile(string message)
        {
            StringBuilder requestURL = new StringBuilder();
            VoiceInfo selectedVoice = this.ttsBanks[selectedBankIndex].GetVoice(selectedVoiceIndex);
            requestURL.Append("http://api.voicerss.org/?key=3a00956d09304edaab2559f5a0672b40");
            requestURL.Append("&hl=" + selectedVoice.VoiceCode + "&src=" + HttpUtility.HtmlEncode(message));
            requestURL.Append("&f=32khz_16bit_mono");
            using (WebClient webClient = new WebClient())
            {
                return new MemoryStream( webClient.DownloadData(requestURL.ToString()) );
            }
        }
        public void PlayFile(string filePath, WaveStream waveStream)
        {
            using (WaveChannel32 waveChannel = new WaveChannel32(waveStream))
            {
                waveChannel.PadWithZeroes = false;
                using (MediaFoundationResampler waveResampler = new MediaFoundationResampler(waveChannel, gameVoiceSampleFormat))
                {
                    waveResampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(gameRootDirectory + "\\voice_input.wav", waveResampler);
                }
            }
            messageSender.Write((byte)SourcePluginMessage.PlaySound);
        }
        public void StopPlaying()
        {
            messageSender.Write((byte)SourcePluginMessage.StopPlaying);
        }
        public void StartRecording()
        {
            messageSender.Write((byte)SourcePluginMessage.StartRecording);
        }
        public void StopRecording()
        {
            messageSender.Write((byte)SourcePluginMessage.StopRecording);
        }
        public bool IsReady()
        {
            return sourceGameProcess != null && !sourceGameProcess.HasExited && Utils.GetForegroundWindow() == gameMainWindowHandle &&
                   messageClient != null && messageClient.Connected && messageSender != null;
        }
    }
}
