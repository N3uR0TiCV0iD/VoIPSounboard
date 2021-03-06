﻿using System;
using Discord;
using System.IO;
using System.Net;
using Discord.Net;
using NAudio.Wave;
using System.Media;
using SKYPE4COMLib;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using HiT.VoIPSoundboard.Soundboards;
using DiscordServer = Discord.Server;
using DiscordUser = Discord.User;
namespace HiT.VoIPSoundboard
{
    public partial class SettingsForm : Form
    {
        static readonly string TTSTMP_PATH = Path.GetTempPath() + "tts_tmp.wav";
        bool closed;
        bool startup;
        Keys[] newKeys;
        bool updateHotkey;
        bool savingAvatar;
        string iconPrefixName;
        Thread microphonePeakThread;
        List<int> selectedVoicesBanks;
        List<DiscordUser> fetchedUsers;
        SkypeSoundboard skypeSoundboard;
        SourceSoundboard sourceSoundboard;
        DiscordSoundboard discordSoundboard;
        List<DiscordServer> subscribedServers;
        public SettingsForm(MainForm mainForm)
        {
            this.startup = true;
            InitializeComponent();
            this.newKeys = new Keys[3];
            this.selectedVoicesBanks = new List<int>();
            this.fetchedUsers = new List<DiscordUser>();
            this.newKeys[0] = mainForm.GetGlobalKey(4);
            this.newKeys[1] = mainForm.GetGlobalKey(5);
            this.newKeys[2] = mainForm.GetGlobalKey(6);
            this.skypeSoundboard = mainForm.SkypeSoundboard;
            this.sourceSoundboard = mainForm.SourceSoundboard;
            this.subscribedServers = new List<DiscordServer>();
            this.discordSoundboard = mainForm.DiscordSoundboard;
            this.fullscreenCheckBox.Checked = skypeSoundboard.CheckFullscreen;
            this.microphonePeakThread = new Thread(() => UpdateMicrophonePeakService());
            this.speakMutedNotifyBox.Checked = this.discordSoundboard.NotifyMutedTalking;
            this.peakNotificationTrackBar.Value = (int)Math.Round(this.discordSoundboard.MutedPeakLevel * this.peakNotificationTrackBar.Maximum);
            this.toolTip.SetToolTip(volumeDuckingBox, "If checked, it will allow Skype to reduce the volume of background applications.");
            this.toolTip.SetToolTip(fullscreenCheckBox, "If checked, it will set your Skype status to the selected one while you have a fullscreen application running.");
            this.toolTip.SetToolTip(speakMutedNotifyBox, "If checked, it will play a sound to tell you that you are trying to talk while muted.\nThe volume limit at which the sound will play can be set on the left.");
            switch (skypeSoundboard.FullscreenStatus)
            {
                case TUserStatus.cusOnline:
                    this.fullscreenStatusBox.SelectedIndex = 0;
                break;
                case TUserStatus.cusAway:
                    this.fullscreenStatusBox.SelectedIndex = 1;
                break;
                case TUserStatus.cusDoNotDisturb:
                    this.fullscreenStatusBox.SelectedIndex = 2;
                break;
                case TUserStatus.cusInvisible:
                    this.fullscreenStatusBox.SelectedIndex = 3;
                break;
            }
            switch (discordSoundboard.Client.State)
            {
                case ConnectionState.Connecting:
                    this.fetchUsersButton.Enabled = false;
                break;
                case ConnectionState.Connected:
                    RefreshSubscribedServers();
                    inviteBox.Enabled = true;
                break;
            }
            this.enableTTSHKButton.Text = this.newKeys[2].ToString();
            this.muteMicHKButton.Text = this.newKeys[0].ToString();
            this.deafenHKButton.Text = this.newKeys[1].ToString();
            this.passwordBox.Text = discordSoundboard.Password;
            this.emailBox.Text = discordSoundboard.Email;
            using (RegistryKey audioPrefsKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Multimedia\\Audio"))
            {
                if (audioPrefsKey != null)
                {
                    object value = audioPrefsKey.GetValue("UserDuckingPreference");
                    volumeDuckingBox.Checked = value == null || (int)value != 3;
                }
            }
            for (int currBankIndex = 0; currBankIndex < sourceSoundboard.TTSBanksCount; currBankIndex++)
            {
                this.ttsBanksBox.Items.Add(sourceSoundboard.GetTTSBank(currBankIndex).BankName);
                this.selectedVoicesBanks.Add(0);
            }
            this.selectedVoicesBanks[sourceSoundboard.SelectedBankIndex] = sourceSoundboard.SelectedVoiceIndex;
            this.ttsBanksBox.SelectedIndex = sourceSoundboard.SelectedBankIndex;
            RefreshMicrophoneDevices();
            this.startup = true;
        }
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.microphonePeakThread.Start();
        }
        public Keys[] NewKeys
        {
            get
            {
                return newKeys;
            }
        }
        private void RefreshSubscribedServers()
        {
            serversBox.Items.Clear();
            subscribedServers.Clear();
            foreach (var currServer in discordSoundboard.Client.Servers)
            {
                serversBox.Items.Add(currServer.Name);
                subscribedServers.Add(currServer);
            }
        }
        private void fullscreenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            skypeSoundboard.CheckFullscreen = fullscreenCheckBox.Checked;
        }
        private void fullscreenStatusBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (fullscreenStatusBox.SelectedIndex)
            {
                case 0:
                    skypeSoundboard.FullscreenStatus = TUserStatus.cusOnline;
                break;
                case 1:
                    skypeSoundboard.FullscreenStatus = TUserStatus.cusAway;
                break;
                case 2:
                    skypeSoundboard.FullscreenStatus = TUserStatus.cusDoNotDisturb;
                break;
                case 3:
                    skypeSoundboard.FullscreenStatus = TUserStatus.cusInvisible;
                break;
            }
        }
        private void HotkeyButton_Click(object sender, EventArgs e)
        {
            ((Button)sender).Text = "...";
            updateHotkey = true;
        }
        private void GlobaHotkeyButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (updateHotkey)
            {
                Button hotkeyButton = (Button)sender;
                hotkeyButton.Text = e.KeyCode.ToString();
                newKeys[(hotkeyButton.TabIndex - 3) / 2] = e.KeyCode;
                updateHotkey = false;
            }
        }
        private void volumeDuckingBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!startup)
            {
                using (RegistryKey audioPrefsKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Multimedia\\Audio", true))
                {
                    if (audioPrefsKey != null)
                    {
                        audioPrefsKey.SetValue("UserDuckingPreference", volumeDuckingBox.Checked ? 1 : 3, RegistryValueKind.DWord);
                    }
                }
            }
        }
        private void ttsBanksBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void ttsBanksBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TTSBankData selectedTTSBank = sourceSoundboard.GetTTSBank(ttsBanksBox.SelectedIndex);
            ttsVoicesBox.Items.Clear();
            for (int currVoiceIndex = 0; currVoiceIndex < selectedTTSBank.VoicesCount; currVoiceIndex++)
            {
                ttsVoicesBox.Items.Add(selectedTTSBank.GetVoice(currVoiceIndex).VoiceName);
            }
            ttsVoicesBox.SelectedIndex = selectedVoicesBanks[ttsBanksBox.SelectedIndex];
            sourceSoundboard.SelectedBankIndex = ttsBanksBox.SelectedIndex;
        }
        private void ttsVoicesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedVoicesBanks[ttsBanksBox.SelectedIndex] = ttsVoicesBox.SelectedIndex;
            sourceSoundboard.SelectedVoiceIndex = ttsVoicesBox.SelectedIndex;
        }
        private void demoTTSButton_Click(object sender, EventArgs e)
        {
            MemoryStream ttsMP3Stream = sourceSoundboard.RequestTTSFile("Hello, this is a " + ttsVoicesBox.SelectedItem.ToString() + " voice. And this is how it sounds");
            if (ttsMP3Stream != null)
            {
                SoundPlayer soundPlayer = new SoundPlayer(TTSTMP_PATH);
                using (Mp3FileReader mp3FileReader = new Mp3FileReader(ttsMP3Stream))
                {
                    using (WaveStream waveStream = new WaveFormatConversionStream(new WaveFormat(22050, 16, 1), mp3FileReader))
                    {
                        WaveFileWriter.CreateWaveFile(TTSTMP_PATH, waveStream);
                    }
                }
                soundPlayer.Play();
            }
        }
        private void voiceRSSLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.voicerss.org");
        }
        private void showPasswordBox_CheckedChanged(object sender, EventArgs e)
        {
            passwordBox.UseSystemPasswordChar = !showPasswordBox.Checked;
        }
        private async void fetchUsersButton_Click(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                fetchedUsersBox.Items.Clear();
            });
            fetchedUsers.Clear();
            try
            {
                if (discordSoundboard.Client.State != ConnectionState.Connected)
                {
                    await discordSoundboard.Client.Connect(emailBox.Text, passwordBox.Text);
                    discordSoundboard.SetLogin(emailBox.Text, passwordBox.Text);
                    RefreshSubscribedServers();
                    inviteBox.Enabled = true;
                }
                this.Invoke((MethodInvoker)delegate
                {
                    foreach (var currServer in discordSoundboard.Client.Servers)
                    {
                        foreach (var currUser in currServer.Users)
                        {
                            if (currUser.Status != UserStatus.Offline && currUser.Id != discordSoundboard.Client.CurrentUser.Id && !IsAlreadyFetched(currUser.Id))
                            {
                                fetchedUsers.Add(currUser);
                                fetchedUsersBox.Items.Add(currUser.Name + " [" + currUser.Id + "]");
                            }
                        }
                    }
                });
            }
            catch (HttpException)
            {
                MessageBox.Show("ERROR: Wrong login credentials!", "ERROR: Invalid login.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                passwordBox.Text = String.Empty;
            }
            catch (Exception ex)
            {
                Utils.ShowUnhandledException(ex, "SettingsForm->fetchUsersButton_Click");
            }
        }
        private bool IsAlreadyFetched(ulong userID)
        {
            foreach (var currFetchedUser in fetchedUsers)
            {
                if (currFetchedUser.Id == userID)
                {
                    return true;
                }
            }
            return false;
        }
        private void fetchedUsersBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fetchedUsersBox.SelectedIndex != -1)
            {
                selectedUserAvatarBox.ImageLocation = fetchedUsers[fetchedUsersBox.SelectedIndex].AvatarUrl;
                followButton.Enabled = true;
            }
            else
            {
                followButton.Enabled = false;
            }
        }
        private void followButton_Click(object sender, EventArgs e)
        {
            DiscordUser selectedUser = fetchedUsers[fetchedUsersBox.SelectedIndex];
            selectedUserAvatarBox.ImageLocation = selectedUser.AvatarUrl;
            if (selectedUser.VoiceChannel != null)
            {
                discordSoundboard.JoinVoiceChannel(selectedUser.VoiceChannel);
            }
            discordSoundboard.ActiveFollowingUser = selectedUser;
            MessageBox.Show("Now following " + selectedUser.Name + " [ID: " + selectedUser.Id + "]",
                            "INFO: New following user.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void microphonesBox_DropDown(object sender, EventArgs e)
        {
            RefreshMicrophoneDevices();
        }
        private void RefreshMicrophoneDevices()
        {
            microphonesBox.Items.Clear();
            foreach (var currDeviceName in MicrophoneHelper.GetDeviceNames())
            {
                microphonesBox.Items.Add(currDeviceName);
            }
            if (microphonesBox.Items.Count != 0)
            {
                if (discordSoundboard.Microphone != null)
                {
                    int index;
                    MicrophoneHelper.GetMicrophoneFromID(discordSoundboard.MicrophoneID, out index);
                    microphonesBox.SelectedIndex = index;
                }
                else
                {
                    microphonesBox.SelectedIndex = 0;
                }
            }
        }
        private void microphonesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (microphonesBox.SelectedIndex != -1)
            {
                discordSoundboard.SetMicrophoneFromIndex(microphonesBox.SelectedIndex);
            }
        }
        private void UpdateMicrophonePeakService()
        {
            while (!closed && !Environment.HasShutdownStarted)
            {
                if (discordSoundboard.Microphone != null)
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            microphonePeakBar.Value = (int)(discordSoundboard.Microphone.AudioMeterInformation.MasterPeakValue * 100);
                        });
                    }
                    catch { }
                }
                Thread.Sleep(64);
            }
        }
        private void speakMutedNotifyBox_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = speakMutedNotifyBox.Checked;
            discordSoundboard.NotifyMutedTalking = isChecked;
            peakNotificationTrackBar.Enabled = isChecked;
            microphonePeakBar.Enabled = isChecked;
            microphonesBox.Enabled = isChecked;
            micSourceLabel.Enabled = isChecked;
            peakPercentageLabel.Enabled = isChecked;
        }
        private void peakNotificationTrackBar_ValueChanged(object sender, EventArgs e)
        {
            float peakValue = peakNotificationTrackBar.Value / (float)peakNotificationTrackBar.Maximum;
            peakPercentageLabel.Text = (int)(peakValue * 100) + "%";
            discordSoundboard.MutedPeakLevel = peakValue;
        }
        private void inviteBox_TextChanged(object sender, EventArgs e)
        {
            acceptInviteButton.Enabled = inviteBox.Text != String.Empty && inviteBox.Text.Length == 24 && inviteBox.Text.StartsWith("https://discord.gg/");
        }
        private void inviteBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                acceptInviteButton.PerformClick();
            }
        }
        private async void acceptInviteButton_Click(object sender, EventArgs e)
        {
            DiscordServer joinedServer = null;
            Invite invite = await discordSoundboard.Client.GetInvite(inviteBox.Text);
            acceptInviteButton.Enabled = false;
            inviteBox.Enabled = false;
            await invite.Accept();
            while (joinedServer == null)
            {
                Thread.Sleep(1500);
                joinedServer = discordSoundboard.Client.GetServer(invite.Server.Id);
            }
            serversBox.Items.Add(joinedServer.Name);
            subscribedServers.Add(joinedServer);
            acceptInviteButton.Enabled = true;
            inviteBox.Text = String.Empty;
            inviteBox.Enabled = true;
        }
        private void serversBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serversBox.SelectedIndex != -1)
            {
                selectedServerLogoBox.ImageLocation = subscribedServers[serversBox.SelectedIndex].IconUrl;
                leaveServerButton.Enabled = true;
            }
            else
            {
                leaveServerButton.Enabled = false;
            }
        }
        private void leaveServerButton_Click(object sender, EventArgs e)
        {
            subscribedServers[serversBox.SelectedIndex].Leave();
            serversBox.Items.RemoveAt(serversBox.SelectedIndex);
            selectedServerLogoBox.Image = null;
        }
        private void selectedUserAvatarBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !String.IsNullOrEmpty(selectedUserAvatarBox.ImageLocation))
            {
                iconPrefixName = fetchedUsersBox.SelectedItem.ToString().Replace(" ", "_");
                pictureBoxContextMenu.Show(MousePosition);
                savingAvatar = true;
            }
        }
        private void selectedServerLogoBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !String.IsNullOrEmpty(selectedServerLogoBox.ImageLocation))
            {
                iconPrefixName = serversBox.SelectedItem.ToString().Replace(" ", "_");
                pictureBoxContextMenu.Show(MousePosition);
                savingAvatar = false;
            }
        }
        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog iconSaveDialog = new SaveFileDialog())
            {
                iconSaveDialog.Filter = "JPG Files (*.jpg)|*.jpg";
                iconSaveDialog.FileName = iconPrefixName + (savingAvatar ? "-Avatar" : "-Logo");
                if (iconSaveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (savingAvatar)
                    {
                        selectedUserAvatarBox.Image.Save(iconSaveDialog.FileName, ImageFormat.Jpeg);
                    }
                    else
                    {
                        selectedServerLogoBox.Image.Save(iconSaveDialog.FileName, ImageFormat.Jpeg);
                    }
                }
            }
        }
        private async void copycatMenuItem_Click(object sender, EventArgs e)
        {
            string newUsername;
            using (MemoryStream avatarImageStream = new MemoryStream())
            {
                if (savingAvatar)
                {
                    DiscordUser meClone;
                    bool canChangeNickname;
                    DiscordUser copyingUserClone;
                    ulong copyingUserID = fetchedUsers[fetchedUsersBox.SelectedIndex].Id;
                    this.Invoke((MethodInvoker)delegate
                    {
                        selectedUserAvatarBox.Image.Save(avatarImageStream, ImageFormat.Jpeg);
                    });
                    newUsername = fetchedUsers[fetchedUsersBox.SelectedIndex].Name;
                    foreach (var currServer in discordSoundboard.Client.Servers)
                    {
                        copyingUserClone = currServer.GetUser(copyingUserID);
                        if (copyingUserClone != null) //Is the user in the current server?
                        {
                            //Yes he is! Let's see if we can change our nickname on this server...
                            canChangeNickname = false;
                            meClone = currServer.GetUser(discordSoundboard.Client.CurrentUser.Id);
                            foreach (var currMeCloneRole in meClone.Roles)
                            {
                                if (currMeCloneRole.Permissions.ChangeNickname)
                                {
                                    canChangeNickname = true;
                                    break;
                                }
                            }
                            if (canChangeNickname)
                            {
                                await meClone.Edit(nickname: copyingUserClone.Nickname);
                            }
                        }
                    }
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        selectedServerLogoBox.Image.Save(avatarImageStream, ImageFormat.Jpeg);
                    });
                    newUsername = subscribedServers[serversBox.SelectedIndex].Name;
                }
                avatarImageStream.Position = 0;
                try
                {
                    await discordSoundboard.Client.CurrentUser.Edit(discordSoundboard.Password, newUsername, avatar: avatarImageStream, avatarType: ImageType.Jpeg);
                    MessageBox.Show("INFO: Copycat was successfull! >:D", "INFO: Copycat success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (HttpException ex)
                {
                    switch (ex.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            MessageBox.Show("ERROR: Too many requests, try again later", "ERROR: Too many requests.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                        default:
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Utils.ShowUnhandledException(ex, "SettingsForm->copycatMenuItem_Click");
                }
            }
        }
        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            closed = true;
        }
    }
}
