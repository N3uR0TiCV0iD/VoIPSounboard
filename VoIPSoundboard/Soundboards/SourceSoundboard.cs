using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using NAudio.Wave;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Windows.Forms;
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
        [DllImport("kernel32.dll", SetLastError = true)] private static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        static readonly string SOURCEPLUGIN_PATH = Application.StartupPath + "\\VoIPSoundboard_SourcePlugin.dll";
        static readonly string TTSTMP_PATH = Path.GetTempPath() + "\\tts_tmp.mp3";
        WaveFormat gameVoiceSampleFormat;
        IntPtr loadLibraryMethodPtr;
        BinaryWriter messageSender;
        Thread messageReaderThread;
        Process sourceGameProcess;
        string gameRootDirectory;
        TcpClient messageClient;
        NotifyIcon trayIcon;
        MainForm mainForm;
        public SourceSoundboard(MainForm mainForm)
        {
            this.loadLibraryMethodPtr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            this.trayIcon = mainForm.TrayIcon;
            this.mainForm = mainForm;
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
                }
                else
                {
                    if (messageSender != null)
                    {
                        messageSender.Dispose();
                    }
                    sourceGameProcess.Dispose();
                    sourceGameProcess = null;
                    messageSender = null;
                    messageClient = null;
                }
            }
        }
        private void InjectPlugin()
        {
            IntPtr processHandle = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 1, (uint)sourceGameProcess.Id);
            if (processHandle != IntPtr.Zero)
            {
                IntPtr parameterAddress = VirtualAllocEx(processHandle, IntPtr.Zero, (uint)SOURCEPLUGIN_PATH.Length, (0x1000 | 0x2000), 0X40);
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
            string readLine;
            bool foundFileURL;
            int messageLength;
            string ttsMessage;
            byte[] messageBytes;
            HttpWebRequest ttsRequest;
            byte[] requestContentBytes;
            HttpWebResponse ttsResponse;
            StringBuilder requestContent = new StringBuilder();
            while (messageClient != null && messageClient.Connected)
            {
                try
                {
                    messageLength = messageReader.ReadInt32();
                    messageBytes = messageReader.ReadBytes(messageLength);
                    ttsMessage = HttpUtility.HtmlEncode(Encoding.ASCII.GetString(messageBytes));
                    ttsRequest = WebRequest.CreateHttp("http://www.acapela-group.com/demo-tts/DemoHTML5Form_V2.php");
                    requestContent.Append("MyLanguages=sonid10&0=Leila&1=Laia&2=Eliska&3=Mette&4=Zoe&5=Jasmijn&6=Tyler&7=Deepa&8=Rhona&9=Rachel&");
                    requestContent.Append("MySelectedVoice=WillLittleCreature+%28emotive+voice%29&");
                    requestContent.Append("11=Hanna&12=Sanna&13=Manon-be&14=Louise&15=Manon&16=Claudia&17=Dimitris&18=Fabiana&19=Sakura&20=Minji&21=Lulu&22=Bente&23=Monika&24=Marcia&25=Celia&26=Alyona&27=Biera&28=Ines&29=Rodrigo&30=Elin&31=Samuel&32=Kal&33=Mia&34=Ipek&");
                    requestContent.Append("MyTextForTTS=" + ttsMessage + "&t=1&SendToVaaS=");
                    requestContentBytes = Encoding.ASCII.GetBytes(requestContent.ToString());
                    ttsRequest.ContentType = "application/x-www-form-urlencoded";
                    ttsRequest.ContentLength = requestContentBytes.Length;
                    ttsRequest.Method = "POST";
                    using (Stream requestStream = ttsRequest.GetRequestStream())
                    {
                        requestStream.Write(requestContentBytes, 0, requestContentBytes.Length);
                    }
                    foundFileURL = false;
                    ttsResponse = (HttpWebResponse)ttsRequest.GetResponse();
                    using (StreamReader responseReader = new StreamReader(ttsResponse.GetResponseStream()))
                    {
                        while (!foundFileURL && !responseReader.EndOfStream)
                        {
                            readLine = responseReader.ReadLine();
                            if (readLine.StartsWith("            var myPhpVar"))
                            {
                                //MessageBox.Show("<" + readLine + ">");
                                using (WebClient webClient = new WebClient())
                                {
                                    messageBytes = webClient.DownloadData(readLine.Substring(28, readLine.Length - 30));  //Reusing variable...
                                    using (MemoryStream ttsMP3Stream = new MemoryStream(messageBytes))
                                    {
                                        using (Mp3FileReader mp3FileReader = new Mp3FileReader(ttsMP3Stream))
                                        {
                                            PlayFile(null, mp3FileReader);
                                            mainForm.WaitForSoundEnd(mp3FileReader.TotalTime);
                                            StopPlaying();
                                        }
                                    }
                                }
                                foundFileURL = true;
                            }
                        }
                    }
                    requestContent.Clear();
                }
                catch (IOException)
                {
                    //DO NOTHING...
                }
                catch (Exception ex)
                {
                    Utils.ShowUnhandledException(ex, "SourceSoundboard->ReadMessagesService");
                }
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
            return sourceGameProcess != null && !sourceGameProcess.HasExited && /*Utils.GetForegroundWindow() == sourceGameProcess.MainWindowHandle && */
                   messageClient != null && messageClient.Connected && messageSender != null;
        }
    }
}
