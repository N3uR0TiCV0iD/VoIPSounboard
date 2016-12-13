using System;
using System.IO;
using NAudio.Wave;
using SKYPE4COMLib;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using HiT.VoIPSoundboard.Soundboards;
using System.Runtime.InteropServices;
using Application = System.Windows.Forms.Application;
namespace HiT.VoIPSoundboard
{
    public partial class MainForm : Form //TODO: Make a new soundgroup editor with online capabilities | Add sound group hotkey (i.e. make it switch instantly to said group) | Make it so that sounds can share a key in a group
    {
        const int GLOBAL_HOTKEYS = 8;
        [DllImport("user32.dll")] private static extern short GetAsyncKeyState(Keys vKey);
        Dictionary<string, List<SoundData>> sounds;
        DiscordSoundboard discordSoundboard;
        SourceSoundboard sourceSoundboard;
        SkypeSoundboard skypeSoundboard;
        SettingsForm openSettingsForm;
        SoundBoardMode soundboardMode;
        ISoundboard activeSoundboard;
        ToastInfoForm toastInfoForm;
        List<string> soundGroups;
        Stopwatch playerTimer;
        Thread serviceThread;
        Keys[] globalHotkeys;
        string currGroupName;
        int lastHotkeyAction;
        bool acceptsVACRisk;
        bool requestedExit;
        bool updateHotkey;
        int selectedGroup;
        bool isRecording;
        bool sbEnabled;
        bool startup;
        public MainForm()
        {
            string lastUsingVersion;
            object registryValue;
            this.startup = true;
            this.sbEnabled = true;
            InitializeComponent();
            this.lastHotkeyAction = -1;
            this.moveToBox.SelectedIndex = 0;
            this.playerTimer = new Stopwatch();
            this.soundGroups = new List<string>();
            this.globalHotkeys = new Keys[GLOBAL_HOTKEYS];
            this.sounds = new Dictionary<string, List<SoundData>>();
            if (!Directory.Exists("soundboard\\"))
            {
                Directory.CreateDirectory("soundboard\\");
            }
            using (RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run"))
            {
                windowsStartupBox.Checked = startupKey.GetValue("HiTVoIPSoundboard") != null;
            }
            using (RegistryKey appRegistryKey = Registry.CurrentUser.OpenSubKey("Software\\HiT\\VoIPSoundboard"))
            {
                if (appRegistryKey != null)
                {
                    bool checkFullScreen;
                    registryValue = appRegistryKey.GetValue("AcceptsVACRisk");
                    acceptsVACRisk = registryValue != null && (int)registryValue == 1;
                    registryValue = appRegistryKey.GetValue("CheckFullScreen");
                    checkFullScreen = registryValue != null && (int)registryValue == 1;
                    registryValue = appRegistryKey.GetValue("FullScreenStatus");
                    this.skypeSoundboard = new SkypeSoundboard(trayIcon, checkFullScreen, registryValue != null ? (TUserStatus)registryValue : TUserStatus.cusDoNotDisturb);
                    this.sourceSoundboard = new SourceSoundboard(this, appRegistryKey);
                    this.discordSoundboard = new DiscordSoundboard(appRegistryKey);
                    registryValue = appRegistryKey.GetValue("SoundboardMode");
                    if (registryValue != null)
                    {
                        switch (Convert.ToInt32(registryValue))
                        {
                            case 0:
                                this.sourceGameMenuItem.PerformClick();
                            break;
                            case 1:
                                this.discordMenuItem.PerformClick();
                            break;
                            case 2:
                                this.skypeMenuItem.PerformClick();
                            break;
                        }
                    }
                    else
                    {
                        this.discordMenuItem.PerformClick();
                    }
                    registryValue = appRegistryKey.GetValue("UsingVersion");
                    lastUsingVersion = registryValue != null ? registryValue.ToString() : null;
                    registryValue = appRegistryKey.GetValue("EnableSBKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[0] = (Keys)registryValue;
                    }
                    registryValue = appRegistryKey.GetValue("PriorGroupKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[1] = (Keys)registryValue;
                    }
                    registryValue = appRegistryKey.GetValue("NextGroupKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[2] = (Keys)registryValue;
                    }
                    registryValue = appRegistryKey.GetValue("RecordKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[3] = (Keys)registryValue;
                    }
                    registryValue = appRegistryKey.GetValue("MuteMicrophoneKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[4] = (Keys)registryValue;
                    }
                    registryValue = appRegistryKey.GetValue("DeafenSkypeKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[5] = (Keys)registryValue;
                    }
                    registryValue = appRegistryKey.GetValue("EnableTTSKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[6] = (Keys)registryValue;
                    }
                    registryValue = appRegistryKey.GetValue("StopSoundKey");
                    if (registryValue != null)
                    {
                        globalHotkeys[GLOBAL_HOTKEYS - 1] = (Keys)registryValue;
                    }
                    enableSBHKButton.Text = globalHotkeys[0].ToString();
                    priorGroupHKButton.Text = globalHotkeys[1].ToString();
                    nextGroupHKButton.Text = globalHotkeys[2].ToString();
                    stopHKButton.Text = globalHotkeys[GLOBAL_HOTKEYS - 1].ToString();
                }
                else
                {
                    this.skypeSoundboard = new SkypeSoundboard(trayIcon);
                    this.sourceSoundboard = new SourceSoundboard(this);
                    this.discordSoundboard = new DiscordSoundboard();
                    this.discordMenuItem.PerformClick();
                    lastUsingVersion = null;
                }
            }
            #if !DEBUG
                if (lastUsingVersion != Program.VERSION)
                {
                    ShowWhatsNewForm();
                }
            #else
                if (WhatsNewForm.DEBUG_SHOWFORM)
                {
                    ShowWhatsNewForm();
                }
            #endif
            this.startup = false;
        }
        private void ShowWhatsNewForm()
        {
            using (WhatsNewForm whatsNewForm = new WhatsNewForm())
            {
                whatsNewForm.ShowDialog();
            }
        }
        public DiscordSoundboard DiscordSoundboard
        {
            get
            {
                return discordSoundboard;
            }
        }
        public SourceSoundboard SourceSoundboard
        {
            get
            {
                return sourceSoundboard;
            }
        }
        public SkypeSoundboard SkypeSoundboard
        {
            get
            {
                return skypeSoundboard;
            }
        }
        public NotifyIcon TrayIcon
        {
            get
            {
                return trayIcon;
            }
        }
        public bool SBEnabled
        {
            get
            {
                return sbEnabled;
            }
        }
        public bool IsClosed
        {
            get
            {
                return requestedExit;
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.serviceThread = new Thread(() => InitializeService());
            this.serviceThread.Start();
        }
        private void InitializeService()
        {
            int groupsCount;
            int soundsCount;
            string readGroupName;
            if (File.Exists("soundboard\\sounds.hdb"))
            {
                using (FileStream soundDBFileStream = new FileStream("soundboard\\sounds.hdb", FileMode.Open))
                {
                    using (BinaryReader soundDataReader = new BinaryReader(soundDBFileStream))
                    {
                        groupsCount = soundDataReader.ReadInt32();
                        this.Invoke((MethodInvoker)delegate
                        {
                            for (int currGroup = 0; currGroup < groupsCount; currGroup++)
                            {
                                readGroupName = soundDataReader.ReadString();
                                sounds.Add(readGroupName, new List<SoundData>());
                                soundsCount = soundDataReader.ReadInt32();
                                for (int currSound = 0; currSound < soundsCount; currSound++)
                                {
                                    sounds[readGroupName].Add(new SoundData(soundDataReader.ReadString(), soundDataReader.ReadString(),
                                                                            (Keys)soundDataReader.ReadInt32(), soundDataReader.ReadInt32()));
                                }
                                soundGroupBox.Items.Add(readGroupName);
                                moveToBox.Items.Add(readGroupName);
                                soundGroups.Add(readGroupName);
                            }
                            if (soundGroups.Count != 0)
                            {
                                soundGroupBox.SelectedIndex = 0;
                                soundGroupBox.Enabled = true;
                            }
                        });
                    }
                }
            }            
            ProvideService();
        }
        private void ProvideService()
        {
            SoundData currSound;
            List<SoundData> groupSounds;
            while (!requestedExit && !Environment.HasShutdownStarted)
            {
                foreach (var currProcess in Process.GetProcesses())
                {
                    switch (currProcess.ProcessName) //TODO: Do some research on the missing source game processes
                    {
                        case "hl":
                        case "hl2":
                        case "csgo":
                        case "dota2":
                            if (sourceSoundboard.SourceGameProcess == null && !currProcess.HasExited && (DateTime.Now - currProcess.StartTime).TotalSeconds >= 3)
                            {
                                sourceSoundboard.SourceGameProcess = currProcess;
                            }
                        break;
                        case "Skype":
                            if (skypeSoundboard.SkypeProcess == null)
                            {
                                skypeSoundboard.SkypeProcess = currProcess;
                            }
                        break;
                    }
                }
                activeSoundboard.SoundboardService();
                for (int currHotkey = 0; currHotkey < GLOBAL_HOTKEYS - 1; currHotkey++)
                {
                    if (IsKeyDown(globalHotkeys[currHotkey]))
                    {
                        if (lastHotkeyAction == -1)
                        {
                            switch (currHotkey)
                            {
                                case 0:
                                    sbEnabled = !sbEnabled;
                                    NotifyViaToast("Soundboard is now: " + (sbEnabled ? "Enabled" : "Disabled"));
                                break;
                                case 1:
                                    if (selectedGroup != 0)
                                    {
                                        selectedGroup--;
                                        NotifyViaToast("The \"" + soundGroups[selectedGroup] + "\" sound group is now selected.");
                                    }
                                break;
                                case 2:
                                    if (selectedGroup != soundGroups.Count - 1 && soundGroups.Count != 0)
                                    {
                                        selectedGroup++;
                                        NotifyViaToast("The \"" + soundGroups[selectedGroup] + "\" sound group is now selected.");
                                    }
                                break;
                                case 3:
                                    if (!isRecording)
                                    {
                                        using (SaveFileDialog recordFileDialog = new SaveFileDialog())
                                        {
                                            recordFileDialog.Filter = "WAV Files (*.wav)|*.wav";
                                            if (recordFileDialog.ShowDialog() == DialogResult.OK)
                                            {
                                                isRecording = true;
                                                activeSoundboard.StartRecording();
                                                MessageBox.Show("Press the record key again to stop recording.", "INFO: Recording started.",
                                                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NotifyViaToast("Recording has stopped.");
                                        activeSoundboard.StopRecording();
                                        isRecording = false;
                                    }
                                break;
                                case 4:
                                    if (soundboardMode == SoundBoardMode.Skype && skypeSoundboard.HasProcess())
                                    {
                                        NotifyViaToast("Microphone has been " + (skypeSoundboard.ToggleMicMute() ? "muted." : "unmuted."));
                                    }
                                break;
                                case 5:
                                    if (soundboardMode == SoundBoardMode.Skype && skypeSoundboard.HasProcess())
                                    {
                                        object returnObject = VolumeMixer.GetApplicationMute(skypeSoundboard.SkypeProcess.Id);
                                        if (returnObject != null)
                                        {
                                            bool isMuted = Convert.ToBoolean(returnObject);
                                            VolumeMixer.SetApplicationMute(skypeSoundboard.SkypeProcess.Id, !isMuted);
                                            NotifyViaToast("Skype has been " + (!isMuted ? "deafened." : "undeafened."));
                                        }
                                    }
                                break;
                                case 6:
                                    sourceSoundboard.TTSEnabled = !sourceSoundboard.TTSEnabled;
                                    NotifyViaToast("TTS is now: " + (sourceSoundboard.TTSEnabled ? "Enabled" : "Disabled"));
                                break;
                            }
                            lastHotkeyAction = currHotkey;
                        }
                    }
                    else if (currHotkey == lastHotkeyAction)
                    {
                        lastHotkeyAction = -1;
                    }
                }
                if (sbEnabled && activeSoundboard.IsReady() && sounds.Count != 0)
                {
                    groupSounds = sounds[soundGroups[selectedGroup]];
                    for (int currSoundIndex = 0; currSoundIndex < groupSounds.Count; currSoundIndex++)
                    {
                        currSound = groupSounds[currSoundIndex];
                        if (currSound.Hotkey != Keys.None && IsKeyDown(currSound.Hotkey))
                        {
                            currSound.TimesPlayed++;
                            this.Invoke((MethodInvoker)delegate
                            {
                                if (selectedGroup == soundGroupBox.SelectedIndex &&
                                    currSoundIndex == soundboardList.SelectedIndex)
                                {
                                    soundTimesLabel.Text = currSound.TimesPlayed + " times";
                                }
                            });
                            PlaySoundFile(currSound.Path);
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }       
        private void NotifyViaToast(string message)
        {
            if (toastInfoForm == null || toastInfoForm.Opacity == 0)
            {
                toastInfoForm = new ToastInfoForm(message);
                this.Invoke((MethodInvoker)delegate
                {
                    toastInfoForm.Show();
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    toastInfoForm.Message = message;
                });
            }
        }
        private void PlaySoundFile(string soundFilePath)
        {
            using (WaveFileReader waveFileReader = new WaveFileReader(soundFilePath))
            {
                activeSoundboard.PlayFile(Application.StartupPath + "\\" + soundFilePath, waveFileReader);
                WaitForSoundEnd(waveFileReader.TotalTime);
            }
            activeSoundboard.StopPlaying();
        }
        public void WaitForSoundEnd(TimeSpan soundLength)
        {
            bool stopped = false;
            playerTimer.Restart();
            while (!stopped && playerTimer.Elapsed <= soundLength && !Environment.HasShutdownStarted)
            {
                stopped = IsKeyDown(globalHotkeys[GLOBAL_HOTKEYS - 1]);
                Thread.Sleep(50);
            }
        }
        private bool IsKeyDown(Keys key)
        {
            return GetAsyncKeyState(key) == short.MinValue;
        }
        private void soundGroupBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            soundboardList.Items.Clear();
            currGroupName = soundGroupBox.SelectedItem.ToString();
            foreach (var currSound in sounds[currGroupName])
            {
                soundboardList.Items.Add(currSound.Name);
            }
            removeGroupButton.Enabled = true;
            moveDownButton.Enabled = false;
            addSoundButton.Enabled = true;
            moveUpButton.Enabled = false;
            DisableSoundControls();
        }
        private void addGroupButton_Click(object sender, EventArgs e)
        {
            using (NewGroupDialog newGroupDialog = new NewGroupDialog(this))
            {
                if (newGroupDialog.ShowDialog() == DialogResult.OK)
                {
                    string soundGroupPath = "soundboard\\" + newGroupDialog.GroupName;
                    sounds.Add(newGroupDialog.GroupName, new List<SoundData>());
                    soundGroupBox.Items.Add(newGroupDialog.GroupName);
                    moveToBox.Items.Add(newGroupDialog.GroupName);
                    soundGroups.Add(newGroupDialog.GroupName);
                    if (!Directory.Exists(soundGroupPath))
                    {
                        Directory.CreateDirectory(soundGroupPath);
                    }
                    soundGroupBox.SelectedIndex = soundGroupBox.Items.Count - 1;
                    soundGroupBox.Enabled = true;
                }
            }
        }
        public bool ContainsGroup(string groupName)
        {
            return soundGroups.Contains(groupName);
        }
        private void removeGroupButton_Click(object sender, EventArgs e)
        {
            if (sounds[currGroupName].Count == 0 || MessageBox.Show("WARNING: Are you sure you wish to delete the group and all its sounds?",
                                                                    "WARNING: Group deletion.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string directoryPath = "soundboard\\" + currGroupName;
                int deletingIndex = soundGroupBox.SelectedIndex;
                if (Directory.Exists(directoryPath))
                {
                    try
                    {
                        Directory.Delete(directoryPath, true);
                    }
                    catch (Exception ex)
                    {
                        Utils.ShowUnhandledException(ex, "MainForm->removeGroupButton_Click");
                    }
                }
                sounds.Remove(currGroupName);
                soundGroups.RemoveAt(deletingIndex);
                moveToBox.Items.RemoveAt(deletingIndex);
                soundGroupBox.Items.RemoveAt(deletingIndex);
                if (soundGroupBox.Items.Count != 0)
                {
                    if (deletingIndex != 0)
                    {
                        soundGroupBox.SelectedIndex = deletingIndex - 1;
                    }
                    else
                    {
                        soundGroupBox.SelectedIndex = 0;
                    }
                }
            }
        }
        private void addSoundButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog soundFileDialog = new OpenFileDialog())
            {
                soundFileDialog.Multiselect = true;
                soundFileDialog.Filter = "All Supported Formats (*.wav, *.mp3)|*.wav;*.mp3|MP3 Files (*.mp3)|*.mp3|WAV Files (*.wav)|*.wav";
                if (soundFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string currFileName;
                    string currFilePath;
                    bool converted = true;
                    foreach (var currSoundPath in soundFileDialog.FileNames)
                    {
                        currFileName = Path.GetFileNameWithoutExtension(currSoundPath);
                        currFilePath = "soundboard\\" + currGroupName + "\\" + currFileName + ".wav";
                        if (!File.Exists(currFilePath))
                        {
                            switch (Path.GetExtension(currSoundPath))
                            {
                                case ".wav":
                                    using (WaveFileReader waveFileReader = new WaveFileReader(currSoundPath))
                                    {
                                        var currSoundFormat = waveFileReader.WaveFormat;
                                        if (currSoundFormat.Channels != 1 || currSoundFormat.SampleRate != 16000 ||
                                            currSoundFormat.BitsPerSample != 16 || currSoundFormat.Encoding != WaveFormatEncoding.Pcm)
                                        {
                                            using (WaveStream waveStream = new WaveFormatConversionStream(new WaveFormat(16000, 16, 1), waveFileReader))
                                            {
                                                WaveFileWriter.CreateWaveFile(currFilePath, waveStream);
                                            }
                                        }
                                        else
                                        {
                                            File.Copy(currSoundPath, currFilePath);
                                            converted = false;
                                        }
                                    }
                                break;
                                case ".mp3":
                                    using (Mp3FileReader mp3FileReader = new Mp3FileReader(currSoundPath))
                                    {
                                        using (WaveStream waveStream = new WaveFormatConversionStream(new WaveFormat(16000, 16, 1), mp3FileReader))
                                        {
                                            WaveFileWriter.CreateWaveFile(currFilePath, waveStream);
                                        }
                                    }
                                break;
                            }
                            soundboardList.Items.Add(currFileName);
                            if (converted && MessageBox.Show("The sound file has been converted into a compatible format. Do you wish to delete the old file?",
                                                             "Delete old file?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes && File.Exists(currSoundPath))
                            {
                                try
                                {
                                    File.Delete(currSoundPath);
                                }
                                catch (Exception ex)
                                {
                                    Utils.ShowUnhandledException(ex, "MainForm->addSoundButton_Click");
                                }
                            }
                            sounds[currGroupName].Add(new SoundData(currFileName, currFilePath, Keys.None, 0));
                            soundboardList.SelectedIndex = soundboardList.Items.Count - 1;
                        }
                        else
                        {
                            MessageBox.Show("ERROR: A soundfile with the name \"" + currFileName + "\" already exists!", "ERROR: Soundfile name conflict.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void soundboardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (soundboardList.SelectedIndex != -1)
            {
                SoundData selectedSound = sounds[currGroupName][soundboardList.SelectedIndex];
                moveDownButton.Enabled = soundboardList.SelectedIndex != soundboardList.Items.Count - 1;
                soundTimesLabel.Text = selectedSound.TimesPlayed + " times";
                soundHotkeyButton.Text = selectedSound.Hotkey.ToString();
                moveUpButton.Enabled = soundboardList.SelectedIndex != 0;
                soundNameBox.Text = selectedSound.Name;
                soundHotkeyButton.Enabled = true;
                moveSoundButton.Enabled = true;
                saveNameButton.Enabled = true;
                soundNameBox.Enabled = true;
                moveToBox.Enabled = true;
            }
            else
            {
                DisableSoundControls();
            }
        }
        private void DisableSoundControls()
        {
            soundTimesLabel.Text = "N/A times";
            soundHotkeyButton.Enabled = false;
            soundNameBox.Text = String.Empty;
            moveSoundButton.Enabled = false;
            soundHotkeyButton.Text = "None";
            saveNameButton.Enabled = false;
            soundNameBox.Enabled = false;
            moveToBox.Enabled = false;
        }
        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveSelectedSound(soundboardList.SelectedIndex - 1);
        }
        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveSelectedSound(soundboardList.SelectedIndex + 1);
        }
        private void MoveSelectedSound(int newIndex)
        {
            SoundData movingSound = sounds[currGroupName][soundboardList.SelectedIndex];
            sounds[currGroupName][soundboardList.SelectedIndex] = sounds[currGroupName][newIndex];
            soundboardList.Items[soundboardList.SelectedIndex] = sounds[currGroupName][newIndex].Name;
            soundboardList.Items[newIndex] = movingSound.Name;
            sounds[currGroupName][newIndex] = movingSound;
            soundboardList.SelectedIndex = newIndex;
        }
        private void soundNameBox_TextChanged(object sender, EventArgs e)
        {
            if (soundNameBox.Text != String.Empty)
            {
                bool enabled = true;
                foreach (var currSoundData in sounds[currGroupName])
                {
                    if (soundNameBox.Text == currSoundData.Name)
                    {
                        enabled = false;
                        break;
                    }
                }
                soundNameBox.Enabled = enabled;
            }
            else
            {
                saveNameButton.Enabled = false;
            }
        }
        private void saveNameButton_Click(object sender, EventArgs e)
        {
            sounds[currGroupName][soundboardList.SelectedIndex].Name = soundNameBox.Text;
            soundboardList.Items[soundboardList.SelectedIndex] = soundNameBox.Text;
        }
        private void HotkeyButton_Click(object sender, EventArgs e)
        {
            ((Button)sender).Text = "...";
            updateHotkey = true;
        }
        private void soundHotkeyButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (updateHotkey)
            {
                sounds[currGroupName][soundboardList.SelectedIndex].Hotkey = e.KeyCode;
                ((Button)sender).Text = e.KeyCode.ToString();
                updateHotkey = false;
            }
        }
        private void moveSoundButton_Click(object sender, EventArgs e)
        {
            if ((moveToBox.SelectedIndex - 1) != soundGroupBox.SelectedIndex)
            {
                string filePath = sounds[currGroupName][soundboardList.SelectedIndex].Path;
                if (moveToBox.SelectedIndex != 0)
                {
                    if (File.Exists(filePath))
                    {
                        SoundData movingSound = sounds[currGroupName][soundboardList.SelectedIndex];
                        movingSound.Path = "soundboard\\" + currGroupName + "\\" + Path.GetFileName(movingSound.Path);
                        sounds[soundGroups[moveToBox.SelectedIndex - 1]].Add(sounds[currGroupName][soundboardList.SelectedIndex]);
                    }
                    else
                    {
                        MessageBox.Show("ERROR: Sound file missing!", "ERROR: Missing file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Utils.ShowUnhandledException(ex, "MainForm->moveSoundButton_Click");
                    }
                }
                sounds[currGroupName].RemoveAt(soundboardList.SelectedIndex);
                if (soundboardList.SelectedIndex != 0)
                {
                    soundboardList.SelectedIndex--;
                    soundboardList.Items.RemoveAt(soundboardList.SelectedIndex + 1);
                }
                else
                {
                    soundboardList.Items.RemoveAt(0);
                    if (soundboardList.Items.Count != 0)
                    {
                        soundboardList.SelectedIndex = 0;
                    }
                }
            }
        }
        private void GlobaHotkeyButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (updateHotkey)
            {
                Button hotkeyButton = (Button)sender;
                hotkeyButton.Text = e.KeyCode.ToString();
                globalHotkeys[(hotkeyButton.TabIndex - 1) / 2] = e.KeyCode;
                updateHotkey = false;
            }
        }
        private void windowsStartupBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!startup)
            {
                using (RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (windowsStartupBox.Checked)
                    {
                        startupKey.SetValue("HiTVoIPSoundboard", Application.StartupPath, RegistryValueKind.String);
                    }
                    else if (startupKey.GetValue("HiTVoIPSoundboard") != null)
                    {
                        startupKey.DeleteValue("HiTVoIPSoundboard");
                    }
                }
            }
        }
        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            if (openSettingsForm == null)
            {
                using (SettingsForm settingsForm = new SettingsForm(this))
                {
                    openSettingsForm = settingsForm;
                    try
                    {
                        settingsForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter logWriter = new StreamWriter("log.txt"))
                        {
                            logWriter.WriteLine(ex.Message);
                            logWriter.WriteLine("---------------");
                            logWriter.WriteLine(ex.StackTrace);
                        }
                        Process.Start("log.txt");
                    }
                    globalHotkeys[4] = settingsForm.NewKeys[0];
                    globalHotkeys[5] = settingsForm.NewKeys[1];
                    globalHotkeys[6] = settingsForm.NewKeys[2];
                    openSettingsForm = null;
                }
            }
            else
            {
                openSettingsForm.WindowState = FormWindowState.Normal;
                openSettingsForm.Show();
            }
        }
        private void sourceGameMenuItem_Click(object sender, EventArgs e)
        {
            if (acceptsVACRisk)
            {
                SwitchToSourceMode();
            }
            else
            {
                using (VACDisclaimerDialog vacDisclaimerDialog = new VACDisclaimerDialog())
                {
                    if (vacDisclaimerDialog.ShowDialog() == DialogResult.OK)
                    {
                        RegistryKey appRegistryKey = Registry.CurrentUser.OpenSubKey("Software\\HiT\\VoIPSoundboard", true);
                        if (appRegistryKey == null)
                        {
                            appRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\HiT\\VoIPSoundboard");
                        }
                        appRegistryKey.SetValue("AcceptsVACRisk", 1, RegistryValueKind.DWord);
                        appRegistryKey.Close();
                        appRegistryKey = null;
                        acceptsVACRisk = true;
                        SwitchToSourceMode();
                    }
                }
            }
        }
        private void SwitchToSourceMode()
        {
            SetModeAsActive(sourceGameMenuItem);
            activeSoundboard = sourceSoundboard;
            soundboardMode = SoundBoardMode.SourceGame;
        }
        private void discordMenuItem_Click(object sender, EventArgs e)
        {
            SetModeAsActive(discordMenuItem);
            activeSoundboard = discordSoundboard;
            soundboardMode = SoundBoardMode.Discord;
        }
        private void skypeMenuItem_Click(object sender, EventArgs e)
        {
            SetModeAsActive(skypeMenuItem);
            activeSoundboard = skypeSoundboard;
            soundboardMode = SoundBoardMode.Skype;
        }
        private void SetModeAsActive(ToolStripMenuItem toolstripItem)
        {
            foreach (ToolStripMenuItem currMenuItem in setModeMenuItem.DropDownItems)
            {
                currMenuItem.Font = setModeMenuItem.Font;
            }
            toolstripItem.Font = new Font(toolstripItem.Font, FontStyle.Bold);
        }
        public Keys GetGlobalKey(int index)
        {
            return globalHotkeys[index];
        }
        private void makeHotkeysTXTMenuItem_Click(object sender, EventArgs e)
        {
            using (StreamWriter hotkeysWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\hotkeys.txt", false))
            {
                foreach (var currSoundGroup in soundGroups)
                {
                    hotkeysWriter.WriteLine("===============");
                    hotkeysWriter.WriteLine(currSoundGroup);
                    hotkeysWriter.WriteLine("===============");
                    foreach (var currSound in sounds[currSoundGroup])
                    {
                        hotkeysWriter.WriteLine(currSound.Hotkey.ToString() + ": " + currSound.Name);
                    }
                    hotkeysWriter.WriteLine();
                }
            }
            MessageBox.Show("INFO: hotkeys.txt has been created successfully!", "INFO: hotkeys.txt created.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            SaveSoundDB();
            MessageBox.Show("INFO: Database has been saved successfully!", "INFO: Database saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void SaveSoundDB()
        {
            using (FileStream soundsDBFileStream = new FileStream("soundboard\\sounds.hdb", FileMode.Create))
            {
                using (BinaryWriter soundsDataWriter = new BinaryWriter(soundsDBFileStream))
                {
                    soundsDataWriter.Write(sounds.Count);
                    foreach (var currGroup in sounds)
                    {
                        soundsDataWriter.Write(currGroup.Key);
                        soundsDataWriter.Write(currGroup.Value.Count);
                        foreach (var currSound in currGroup.Value)
                        {
                            soundsDataWriter.Write(currSound.Name);
                            soundsDataWriter.Write(currSound.Path);
                            soundsDataWriter.Write((int)currSound.Hotkey);
                            soundsDataWriter.Write(currSound.TimesPlayed);
                        }
                    }
                }
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            #if !DEBUG
                if (!requestedExit)
                {
                    trayIcon.Visible = true;
                    e.Cancel = true;
                    this.Hide();
                }
            #endif
        }
        private void ShowFormAgain(object sender, EventArgs e)
        {
            this.Show();
            Program.SetForegroundWindow(this.Handle);
        }
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            requestedExit = true;
            this.Close();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RegistryKey appRegistryKey = Registry.CurrentUser.OpenSubKey("Software\\HiT\\VoIPSoundboard", true);
            if (appRegistryKey == null)
            {
                appRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\HiT\\VoIPSoundboard");
            }
            appRegistryKey.SetValue("CheckFullScreen", skypeSoundboard.CheckFullscreen ? 1 : 0, RegistryValueKind.DWord);
            appRegistryKey.SetValue("FullScreenStatus", (int)skypeSoundboard.FullscreenStatus, RegistryValueKind.DWord);
            appRegistryKey.SetValue("SoundboardMode", (int)soundboardMode, RegistryValueKind.DWord);
            appRegistryKey.SetValue("UsingVersion", Program.VERSION, RegistryValueKind.String);
            appRegistryKey.SetValue("EnableSBKey", globalHotkeys[0], RegistryValueKind.DWord);
            appRegistryKey.SetValue("PriorGroupKey", globalHotkeys[1], RegistryValueKind.DWord);
            appRegistryKey.SetValue("NextGroupKey", globalHotkeys[2], RegistryValueKind.DWord);
            appRegistryKey.SetValue("RecordKey", globalHotkeys[3], RegistryValueKind.DWord);
            appRegistryKey.SetValue("MuteMicrophoneKey", globalHotkeys[4], RegistryValueKind.DWord);
            appRegistryKey.SetValue("DeafenSkypeKey", globalHotkeys[5], RegistryValueKind.DWord);
            appRegistryKey.SetValue("EnableTTSKey", globalHotkeys[6], RegistryValueKind.DWord);
            appRegistryKey.SetValue("StopSoundKey", globalHotkeys[GLOBAL_HOTKEYS - 1], RegistryValueKind.DWord);
            appRegistryKey.SetValue("TTSVoiceName", sourceSoundboard.SelectedTTSVoice, RegistryValueKind.String);
            if (discordSoundboard.HasLogin)
            {
                discordSoundboard.SaveSettings(appRegistryKey);
            }
            if (discordSoundboard.IsReady())
            {
                discordSoundboard.Client.Disconnect();
            }
            sourceSoundboard.StopReadMessagesService();
            appRegistryKey.Close();
            appRegistryKey = null;
            SaveSoundDB();
        }
    }
}
