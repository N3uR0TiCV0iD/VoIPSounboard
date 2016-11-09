namespace HiT.VoIPSoundboard
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.deafenHKButton = new System.Windows.Forms.Button();
            this.deafeanHKLabel = new System.Windows.Forms.Label();
            this.muteMicHKButton = new System.Windows.Forms.Button();
            this.muteMicHKLabel = new System.Windows.Forms.Label();
            this.volumeDuckingBox = new System.Windows.Forms.CheckBox();
            this.skypeSettingsBox = new System.Windows.Forms.GroupBox();
            this.fullscreenStatusBox = new System.Windows.Forms.ComboBox();
            this.fullscreenCheckBox = new System.Windows.Forms.CheckBox();
            this.discordSettingsBox = new System.Windows.Forms.GroupBox();
            this.peakPercentageLabel = new System.Windows.Forms.Label();
            this.microphonesBox = new System.Windows.Forms.ComboBox();
            this.micSourceLabel = new System.Windows.Forms.Label();
            this.inviteBox = new System.Windows.Forms.TextBox();
            this.acceptInviteButton = new System.Windows.Forms.Button();
            this.inviteLabel = new System.Windows.Forms.Label();
            this.serversBox = new System.Windows.Forms.ListBox();
            this.leaveServerButton = new System.Windows.Forms.Button();
            this.followButton = new System.Windows.Forms.Button();
            this.fetchedUsersBox = new System.Windows.Forms.ListBox();
            this.fetchUsersButton = new System.Windows.Forms.Button();
            this.showPasswordBox = new System.Windows.Forms.CheckBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.emailBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.speakMutedNotifyBox = new System.Windows.Forms.CheckBox();
            this.peakNotificationTrackBar = new System.Windows.Forms.TrackBar();
            this.pictureBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copycatMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.driverSettingsBox = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.selectedServerLogoBox = new System.Windows.Forms.PictureBox();
            this.selectedUserAvatarBox = new System.Windows.Forms.PictureBox();
            this.microphonePeakBar = new HiT.VoIPSoundboard.FlatProgressBar();
            this.skypeSettingsBox.SuspendLayout();
            this.discordSettingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peakNotificationTrackBar)).BeginInit();
            this.pictureBoxContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedServerLogoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedUserAvatarBox)).BeginInit();
            this.SuspendLayout();
            // 
            // deafenHKButton
            // 
            this.deafenHKButton.Location = new System.Drawing.Point(101, 70);
            this.deafenHKButton.Name = "deafenHKButton";
            this.deafenHKButton.Size = new System.Drawing.Size(70, 20);
            this.deafenHKButton.TabIndex = 5;
            this.deafenHKButton.Text = "None";
            this.deafenHKButton.UseVisualStyleBackColor = true;
            this.deafenHKButton.Click += new System.EventHandler(this.HotkeyButton_Click);
            this.deafenHKButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobaHotkeyButton_KeyDown);
            // 
            // deafeanHKLabel
            // 
            this.deafeanHKLabel.AutoSize = true;
            this.deafeanHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deafeanHKLabel.Location = new System.Drawing.Point(7, 74);
            this.deafeanHKLabel.Name = "deafeanHKLabel";
            this.deafeanHKLabel.Size = new System.Drawing.Size(96, 13);
            this.deafeanHKLabel.TabIndex = 4;
            this.deafeanHKLabel.Text = "Deafen Hotkey:";
            // 
            // muteMicHKButton
            // 
            this.muteMicHKButton.Location = new System.Drawing.Point(115, 45);
            this.muteMicHKButton.Name = "muteMicHKButton";
            this.muteMicHKButton.Size = new System.Drawing.Size(70, 20);
            this.muteMicHKButton.TabIndex = 3;
            this.muteMicHKButton.Text = "None";
            this.muteMicHKButton.UseVisualStyleBackColor = true;
            this.muteMicHKButton.Click += new System.EventHandler(this.HotkeyButton_Click);
            this.muteMicHKButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobaHotkeyButton_KeyDown);
            // 
            // muteMicHKLabel
            // 
            this.muteMicHKLabel.AutoSize = true;
            this.muteMicHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.muteMicHKLabel.Location = new System.Drawing.Point(7, 49);
            this.muteMicHKLabel.Name = "muteMicHKLabel";
            this.muteMicHKLabel.Size = new System.Drawing.Size(107, 13);
            this.muteMicHKLabel.TabIndex = 2;
            this.muteMicHKLabel.Text = "Mute Mic Hotkey:";
            // 
            // volumeDuckingBox
            // 
            this.volumeDuckingBox.AutoSize = true;
            this.volumeDuckingBox.Location = new System.Drawing.Point(10, 96);
            this.volumeDuckingBox.Name = "volumeDuckingBox";
            this.volumeDuckingBox.Size = new System.Drawing.Size(129, 17);
            this.volumeDuckingBox.TabIndex = 6;
            this.volumeDuckingBox.Text = "Allow volume ducking";
            this.volumeDuckingBox.UseVisualStyleBackColor = true;
            this.volumeDuckingBox.CheckedChanged += new System.EventHandler(this.volumeDuckingBox_CheckedChanged);
            // 
            // skypeSettingsBox
            // 
            this.skypeSettingsBox.Controls.Add(this.fullscreenStatusBox);
            this.skypeSettingsBox.Controls.Add(this.muteMicHKLabel);
            this.skypeSettingsBox.Controls.Add(this.fullscreenCheckBox);
            this.skypeSettingsBox.Controls.Add(this.volumeDuckingBox);
            this.skypeSettingsBox.Controls.Add(this.muteMicHKButton);
            this.skypeSettingsBox.Controls.Add(this.deafenHKButton);
            this.skypeSettingsBox.Controls.Add(this.deafeanHKLabel);
            this.skypeSettingsBox.Location = new System.Drawing.Point(10, 10);
            this.skypeSettingsBox.Name = "skypeSettingsBox";
            this.skypeSettingsBox.Size = new System.Drawing.Size(215, 120);
            this.skypeSettingsBox.TabIndex = 0;
            this.skypeSettingsBox.TabStop = false;
            this.skypeSettingsBox.Text = "Skype Settings";
            // 
            // fullscreenStatusBox
            // 
            this.fullscreenStatusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fullscreenStatusBox.FormattingEnabled = true;
            this.fullscreenStatusBox.Items.AddRange(new object[] {
            "Online",
            "Away",
            "Busy",
            "Invisible"});
            this.fullscreenStatusBox.Location = new System.Drawing.Point(115, 18);
            this.fullscreenStatusBox.Name = "fullscreenStatusBox";
            this.fullscreenStatusBox.Size = new System.Drawing.Size(91, 21);
            this.fullscreenStatusBox.TabIndex = 1;
            this.fullscreenStatusBox.SelectedIndexChanged += new System.EventHandler(this.fullscreenStatusBox_SelectedIndexChanged);
            // 
            // fullscreenCheckBox
            // 
            this.fullscreenCheckBox.AutoSize = true;
            this.fullscreenCheckBox.Location = new System.Drawing.Point(10, 20);
            this.fullscreenCheckBox.Name = "fullscreenCheckBox";
            this.fullscreenCheckBox.Size = new System.Drawing.Size(108, 17);
            this.fullscreenCheckBox.TabIndex = 0;
            this.fullscreenCheckBox.Text = "Fullscreen status:";
            this.fullscreenCheckBox.UseVisualStyleBackColor = true;
            this.fullscreenCheckBox.CheckedChanged += new System.EventHandler(this.fullscreenCheckBox_CheckedChanged);
            // 
            // discordSettingsBox
            // 
            this.discordSettingsBox.Controls.Add(this.microphonePeakBar);
            this.discordSettingsBox.Controls.Add(this.peakPercentageLabel);
            this.discordSettingsBox.Controls.Add(this.microphonesBox);
            this.discordSettingsBox.Controls.Add(this.micSourceLabel);
            this.discordSettingsBox.Controls.Add(this.inviteBox);
            this.discordSettingsBox.Controls.Add(this.acceptInviteButton);
            this.discordSettingsBox.Controls.Add(this.inviteLabel);
            this.discordSettingsBox.Controls.Add(this.selectedServerLogoBox);
            this.discordSettingsBox.Controls.Add(this.serversBox);
            this.discordSettingsBox.Controls.Add(this.leaveServerButton);
            this.discordSettingsBox.Controls.Add(this.selectedUserAvatarBox);
            this.discordSettingsBox.Controls.Add(this.followButton);
            this.discordSettingsBox.Controls.Add(this.fetchedUsersBox);
            this.discordSettingsBox.Controls.Add(this.fetchUsersButton);
            this.discordSettingsBox.Controls.Add(this.showPasswordBox);
            this.discordSettingsBox.Controls.Add(this.passwordBox);
            this.discordSettingsBox.Controls.Add(this.emailBox);
            this.discordSettingsBox.Controls.Add(this.passwordLabel);
            this.discordSettingsBox.Controls.Add(this.emailLabel);
            this.discordSettingsBox.Controls.Add(this.speakMutedNotifyBox);
            this.discordSettingsBox.Controls.Add(this.peakNotificationTrackBar);
            this.discordSettingsBox.Location = new System.Drawing.Point(10, 135);
            this.discordSettingsBox.Name = "discordSettingsBox";
            this.discordSettingsBox.Size = new System.Drawing.Size(420, 250);
            this.discordSettingsBox.TabIndex = 2;
            this.discordSettingsBox.TabStop = false;
            this.discordSettingsBox.Text = "Discord Settings";
            // 
            // peakPercentageLabel
            // 
            this.peakPercentageLabel.AutoSize = true;
            this.peakPercentageLabel.Enabled = false;
            this.peakPercentageLabel.Location = new System.Drawing.Point(335, 75);
            this.peakPercentageLabel.Name = "peakPercentageLabel";
            this.peakPercentageLabel.Size = new System.Drawing.Size(21, 13);
            this.peakPercentageLabel.TabIndex = 13;
            this.peakPercentageLabel.Text = "0%";
            // 
            // microphonesBox
            // 
            this.microphonesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.microphonesBox.Enabled = false;
            this.microphonesBox.FormattingEnabled = true;
            this.microphonesBox.Location = new System.Drawing.Point(275, 20);
            this.microphonesBox.Name = "microphonesBox";
            this.microphonesBox.Size = new System.Drawing.Size(135, 21);
            this.microphonesBox.TabIndex = 9;
            this.microphonesBox.DropDown += new System.EventHandler(this.microphonesBox_DropDown);
            this.microphonesBox.SelectedIndexChanged += new System.EventHandler(this.microphonesBox_SelectedIndexChanged);
            // 
            // micSourceLabel
            // 
            this.micSourceLabel.AutoSize = true;
            this.micSourceLabel.Enabled = false;
            this.micSourceLabel.Location = new System.Drawing.Point(212, 23);
            this.micSourceLabel.Name = "micSourceLabel";
            this.micSourceLabel.Size = new System.Drawing.Size(62, 13);
            this.micSourceLabel.TabIndex = 8;
            this.micSourceLabel.Text = "Mic source:";
            // 
            // inviteBox
            // 
            this.inviteBox.Enabled = false;
            this.inviteBox.Location = new System.Drawing.Point(250, 95);
            this.inviteBox.Name = "inviteBox";
            this.inviteBox.Size = new System.Drawing.Size(125, 20);
            this.inviteBox.TabIndex = 15;
            this.inviteBox.TextChanged += new System.EventHandler(this.inviteBox_TextChanged);
            this.inviteBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inviteBox_KeyDown);
            // 
            // acceptInviteButton
            // 
            this.acceptInviteButton.Enabled = false;
            this.acceptInviteButton.Location = new System.Drawing.Point(380, 95);
            this.acceptInviteButton.Name = "acceptInviteButton";
            this.acceptInviteButton.Size = new System.Drawing.Size(30, 20);
            this.acceptInviteButton.TabIndex = 16;
            this.acceptInviteButton.Text = "OK";
            this.acceptInviteButton.UseVisualStyleBackColor = true;
            this.acceptInviteButton.Click += new System.EventHandler(this.acceptInviteButton_Click);
            // 
            // inviteLabel
            // 
            this.inviteLabel.AutoSize = true;
            this.inviteLabel.Location = new System.Drawing.Point(215, 98);
            this.inviteLabel.Name = "inviteLabel";
            this.inviteLabel.Size = new System.Drawing.Size(36, 13);
            this.inviteLabel.TabIndex = 14;
            this.inviteLabel.Text = "Invite:";
            // 
            // serversBox
            // 
            this.serversBox.FormattingEnabled = true;
            this.serversBox.Location = new System.Drawing.Point(215, 125);
            this.serversBox.Name = "serversBox";
            this.serversBox.Size = new System.Drawing.Size(129, 82);
            this.serversBox.TabIndex = 17;
            this.serversBox.SelectedIndexChanged += new System.EventHandler(this.serversBox_SelectedIndexChanged);
            // 
            // leaveServerButton
            // 
            this.leaveServerButton.Enabled = false;
            this.leaveServerButton.Location = new System.Drawing.Point(215, 215);
            this.leaveServerButton.Name = "leaveServerButton";
            this.leaveServerButton.Size = new System.Drawing.Size(196, 23);
            this.leaveServerButton.TabIndex = 19;
            this.leaveServerButton.Text = "Leave";
            this.leaveServerButton.UseVisualStyleBackColor = true;
            this.leaveServerButton.Click += new System.EventHandler(this.leaveServerButton_Click);
            // 
            // followButton
            // 
            this.followButton.Enabled = false;
            this.followButton.Location = new System.Drawing.Point(10, 215);
            this.followButton.Name = "followButton";
            this.followButton.Size = new System.Drawing.Size(196, 23);
            this.followButton.TabIndex = 7;
            this.followButton.Text = "Follow";
            this.followButton.UseVisualStyleBackColor = true;
            this.followButton.Click += new System.EventHandler(this.followButton_Click);
            // 
            // fetchedUsersBox
            // 
            this.fetchedUsersBox.FormattingEnabled = true;
            this.fetchedUsersBox.Location = new System.Drawing.Point(10, 125);
            this.fetchedUsersBox.Name = "fetchedUsersBox";
            this.fetchedUsersBox.Size = new System.Drawing.Size(129, 82);
            this.fetchedUsersBox.TabIndex = 6;
            this.fetchedUsersBox.SelectedIndexChanged += new System.EventHandler(this.fetchedUsersBox_SelectedIndexChanged);
            // 
            // fetchUsersButton
            // 
            this.fetchUsersButton.Location = new System.Drawing.Point(10, 93);
            this.fetchUsersButton.Name = "fetchUsersButton";
            this.fetchUsersButton.Size = new System.Drawing.Size(196, 23);
            this.fetchUsersButton.TabIndex = 5;
            this.fetchUsersButton.Text = "Fetch Users";
            this.fetchUsersButton.UseVisualStyleBackColor = true;
            this.fetchUsersButton.Click += new System.EventHandler(this.fetchUsersButton_Click);
            // 
            // showPasswordBox
            // 
            this.showPasswordBox.AutoSize = true;
            this.showPasswordBox.Location = new System.Drawing.Point(13, 70);
            this.showPasswordBox.Name = "showPasswordBox";
            this.showPasswordBox.Size = new System.Drawing.Size(101, 17);
            this.showPasswordBox.TabIndex = 4;
            this.showPasswordBox.Text = "Show password";
            this.showPasswordBox.UseVisualStyleBackColor = true;
            this.showPasswordBox.CheckedChanged += new System.EventHandler(this.showPasswordBox_CheckedChanged);
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(65, 45);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(141, 20);
            this.passwordBox.TabIndex = 3;
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // emailBox
            // 
            this.emailBox.Location = new System.Drawing.Point(45, 20);
            this.emailBox.Name = "emailBox";
            this.emailBox.Size = new System.Drawing.Size(161, 20);
            this.emailBox.TabIndex = 1;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(10, 48);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "Password:";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(10, 23);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(35, 13);
            this.emailLabel.TabIndex = 0;
            this.emailLabel.Text = "Email:";
            // 
            // speakMutedNotifyBox
            // 
            this.speakMutedNotifyBox.AutoSize = true;
            this.speakMutedNotifyBox.Location = new System.Drawing.Point(332, 45);
            this.speakMutedNotifyBox.Name = "speakMutedNotifyBox";
            this.speakMutedNotifyBox.Size = new System.Drawing.Size(85, 30);
            this.speakMutedNotifyBox.TabIndex = 11;
            this.speakMutedNotifyBox.Text = "Notify speak\r\nwhile muted";
            this.speakMutedNotifyBox.UseVisualStyleBackColor = true;
            this.speakMutedNotifyBox.CheckedChanged += new System.EventHandler(this.speakMutedNotifyBox_CheckedChanged);
            // 
            // peakNotificationTrackBar
            // 
            this.peakNotificationTrackBar.AutoSize = false;
            this.peakNotificationTrackBar.Enabled = false;
            this.peakNotificationTrackBar.Location = new System.Drawing.Point(202, 63);
            this.peakNotificationTrackBar.Maximum = 30;
            this.peakNotificationTrackBar.Name = "peakNotificationTrackBar";
            this.peakNotificationTrackBar.Size = new System.Drawing.Size(137, 28);
            this.peakNotificationTrackBar.TabIndex = 12;
            this.peakNotificationTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.peakNotificationTrackBar.ValueChanged += new System.EventHandler(this.peakNotificationTrackBar_ValueChanged);
            // 
            // pictureBoxContextMenu
            // 
            this.pictureBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveMenuItem,
            this.copycatMenuItem});
            this.pictureBoxContextMenu.Name = "userAvatarContextMenu";
            this.pictureBoxContextMenu.Size = new System.Drawing.Size(119, 48);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(118, 22);
            this.saveMenuItem.Text = "Save";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // copycatMenuItem
            // 
            this.copycatMenuItem.Name = "copycatMenuItem";
            this.copycatMenuItem.Size = new System.Drawing.Size(118, 22);
            this.copycatMenuItem.Text = "Copycat";
            this.copycatMenuItem.Click += new System.EventHandler(this.copycatMenuItem_Click);
            // 
            // driverSettingsBox
            // 
            this.driverSettingsBox.Location = new System.Drawing.Point(235, 10);
            this.driverSettingsBox.Name = "driverSettingsBox";
            this.driverSettingsBox.Size = new System.Drawing.Size(195, 120);
            this.driverSettingsBox.TabIndex = 1;
            this.driverSettingsBox.TabStop = false;
            this.driverSettingsBox.Text = "Driver Settings";
            // 
            // selectedServerLogoBox
            // 
            this.selectedServerLogoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectedServerLogoBox.Location = new System.Drawing.Point(347, 135);
            this.selectedServerLogoBox.Name = "selectedServerLogoBox";
            this.selectedServerLogoBox.Size = new System.Drawing.Size(64, 64);
            this.selectedServerLogoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.selectedServerLogoBox.TabIndex = 12;
            this.selectedServerLogoBox.TabStop = false;
            this.selectedServerLogoBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.selectedServerLogoBox_MouseClick);
            // 
            // selectedUserAvatarBox
            // 
            this.selectedUserAvatarBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectedUserAvatarBox.Location = new System.Drawing.Point(142, 134);
            this.selectedUserAvatarBox.Name = "selectedUserAvatarBox";
            this.selectedUserAvatarBox.Size = new System.Drawing.Size(64, 64);
            this.selectedUserAvatarBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.selectedUserAvatarBox.TabIndex = 9;
            this.selectedUserAvatarBox.TabStop = false;
            this.selectedUserAvatarBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.selectedUserAvatarBox_MouseClick);
            // 
            // microphonePeakBar
            // 
            this.microphonePeakBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.microphonePeakBar.Enabled = false;
            this.microphonePeakBar.Location = new System.Drawing.Point(215, 45);
            this.microphonePeakBar.Maximum = 100;
            this.microphonePeakBar.Minimum = 0;
            this.microphonePeakBar.Name = "microphonePeakBar";
            this.microphonePeakBar.Size = new System.Drawing.Size(112, 20);
            this.microphonePeakBar.TabIndex = 10;
            this.microphonePeakBar.Value = 0;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 392);
            this.Controls.Add(this.driverSettingsBox);
            this.Controls.Add(this.discordSettingsBox);
            this.Controls.Add(this.skypeSettingsBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VoIPSoundboard Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.skypeSettingsBox.ResumeLayout(false);
            this.skypeSettingsBox.PerformLayout();
            this.discordSettingsBox.ResumeLayout(false);
            this.discordSettingsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peakNotificationTrackBar)).EndInit();
            this.pictureBoxContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectedServerLogoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedUserAvatarBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button deafenHKButton;
        private System.Windows.Forms.Label deafeanHKLabel;
        private System.Windows.Forms.Button muteMicHKButton;
        private System.Windows.Forms.Label muteMicHKLabel;
        private System.Windows.Forms.CheckBox volumeDuckingBox;
        private System.Windows.Forms.GroupBox skypeSettingsBox;
        private System.Windows.Forms.GroupBox discordSettingsBox;
        private System.Windows.Forms.CheckBox showPasswordBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox emailBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Button fetchUsersButton;
        private System.Windows.Forms.ComboBox fullscreenStatusBox;
        private System.Windows.Forms.CheckBox fullscreenCheckBox;
        private System.Windows.Forms.ListBox fetchedUsersBox;
        private System.Windows.Forms.Button followButton;
        private System.Windows.Forms.PictureBox selectedUserAvatarBox;
        private System.Windows.Forms.ContextMenuStrip pictureBoxContextMenu;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.Button acceptInviteButton;
        private System.Windows.Forms.Label inviteLabel;
        private System.Windows.Forms.TextBox inviteBox;
        private System.Windows.Forms.PictureBox selectedServerLogoBox;
        private System.Windows.Forms.ListBox serversBox;
        private System.Windows.Forms.Button leaveServerButton;
        private System.Windows.Forms.ComboBox microphonesBox;
        private System.Windows.Forms.GroupBox driverSettingsBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label micSourceLabel;
        private System.Windows.Forms.TrackBar peakNotificationTrackBar;
        private System.Windows.Forms.ToolStripMenuItem copycatMenuItem;
        private System.Windows.Forms.Label peakPercentageLabel;
        private System.Windows.Forms.CheckBox speakMutedNotifyBox;
        private FlatProgressBar microphonePeakBar;
    }
}