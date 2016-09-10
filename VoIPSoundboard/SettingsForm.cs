using System;
using Discord;
using Discord.Net;
using SKYPE4COMLib;
using Microsoft.Win32;
using System.Threading;
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
        bool startup;
        Keys[] skypeKeys;
        bool updateHotkey;
        bool savingAvatar;
        string iconPrefixName;
        List<DiscordUser> fetchedUsers;
        SkypeSoundboard skypeSoundboard;
        DiscordSoundboard discordSoundboard;
        List<DiscordServer> subscribedServers;
        public SettingsForm(MainForm mainForm)
        {
            this.startup = true;
            InitializeComponent();
            this.skypeKeys = new Keys[2];
            this.subscribedServers = new List<Server>();
            this.fetchedUsers = new List<DiscordUser>();
            this.skypeKeys[0] = mainForm.GetGlobalKey(4);
            this.skypeKeys[1] = mainForm.GetGlobalKey(5);
            this.skypeSoundboard = mainForm.SkypeSoundboard;
            this.discordSoundboard = mainForm.DiscordSoundboard;
            this.fullscreenCheckBox.Checked = skypeSoundboard.CheckFullscreen;
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
            this.muteMicHKButton.Text = this.skypeKeys[0].ToString();
            this.deafenHKButton.Text = this.skypeKeys[1].ToString();
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
            this.startup = true;
        }
        public Keys[] SkypeKeys
        {
            get
            {
                return skypeKeys;
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
                skypeKeys[(hotkeyButton.TabIndex - 3) / 2] = e.KeyCode;
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
                MessageBox.Show("ERROR: Wrong login credentials", "ERROR: Invalid login.", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
