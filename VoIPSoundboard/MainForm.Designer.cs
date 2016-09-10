namespace HiT.VoIPSoundboard
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.windowsStartupBox = new System.Windows.Forms.CheckBox();
            this.soundboardBox = new System.Windows.Forms.GroupBox();
            this.soundTimesLabel = new System.Windows.Forms.Label();
            this.moveSoundButton = new System.Windows.Forms.Button();
            this.moveToBox = new System.Windows.Forms.ComboBox();
            this.moveToLabel = new System.Windows.Forms.Label();
            this.addSoundButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.removeGroupButton = new System.Windows.Forms.Button();
            this.addGroupButton = new System.Windows.Forms.Button();
            this.saveNameButton = new System.Windows.Forms.Button();
            this.soundGroupBox = new System.Windows.Forms.ComboBox();
            this.groupLabel = new System.Windows.Forms.Label();
            this.soundNameBox = new System.Windows.Forms.TextBox();
            this.soundboardList = new System.Windows.Forms.ListBox();
            this.soundNameLabel = new System.Windows.Forms.Label();
            this.soundHotkeyButton = new System.Windows.Forms.Button();
            this.soundHotkeyLabel = new System.Windows.Forms.Label();
            this.soundUsedLabel = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeHotkeysTXTMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.setModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skypeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.recordHKButton = new System.Windows.Forms.Button();
            this.recordHKLabel = new System.Windows.Forms.Label();
            this.stopHKButton = new System.Windows.Forms.Button();
            this.nextGroupHKButton = new System.Windows.Forms.Button();
            this.priorGroupHKButton = new System.Windows.Forms.Button();
            this.enableSBHKButton = new System.Windows.Forms.Button();
            this.nextGroupHKLabel = new System.Windows.Forms.Label();
            this.priorGroupHKLabel = new System.Windows.Forms.Label();
            this.enableSBHKLabel = new System.Windows.Forms.Label();
            this.stopHKLabel = new System.Windows.Forms.Label();
            this.soundboardBox.SuspendLayout();
            this.trayIconMenu.SuspendLayout();
            this.settingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // windowsStartupBox
            // 
            this.windowsStartupBox.AutoSize = true;
            this.windowsStartupBox.Location = new System.Drawing.Point(10, 140);
            this.windowsStartupBox.Name = "windowsStartupBox";
            this.windowsStartupBox.Size = new System.Drawing.Size(117, 17);
            this.windowsStartupBox.TabIndex = 10;
            this.windowsStartupBox.Text = "Start with Windows";
            this.windowsStartupBox.UseVisualStyleBackColor = true;
            this.windowsStartupBox.CheckedChanged += new System.EventHandler(this.windowsStartupBox_CheckedChanged);
            // 
            // soundboardBox
            // 
            this.soundboardBox.Controls.Add(this.soundTimesLabel);
            this.soundboardBox.Controls.Add(this.moveSoundButton);
            this.soundboardBox.Controls.Add(this.moveToBox);
            this.soundboardBox.Controls.Add(this.moveToLabel);
            this.soundboardBox.Controls.Add(this.addSoundButton);
            this.soundboardBox.Controls.Add(this.moveDownButton);
            this.soundboardBox.Controls.Add(this.moveUpButton);
            this.soundboardBox.Controls.Add(this.removeGroupButton);
            this.soundboardBox.Controls.Add(this.addGroupButton);
            this.soundboardBox.Controls.Add(this.saveNameButton);
            this.soundboardBox.Controls.Add(this.soundGroupBox);
            this.soundboardBox.Controls.Add(this.groupLabel);
            this.soundboardBox.Controls.Add(this.soundNameBox);
            this.soundboardBox.Controls.Add(this.soundboardList);
            this.soundboardBox.Controls.Add(this.soundNameLabel);
            this.soundboardBox.Controls.Add(this.soundHotkeyButton);
            this.soundboardBox.Controls.Add(this.soundHotkeyLabel);
            this.soundboardBox.Controls.Add(this.soundUsedLabel);
            this.soundboardBox.Location = new System.Drawing.Point(10, 10);
            this.soundboardBox.Name = "soundboardBox";
            this.soundboardBox.Size = new System.Drawing.Size(215, 215);
            this.soundboardBox.TabIndex = 0;
            this.soundboardBox.TabStop = false;
            this.soundboardBox.Text = "Soundboard";
            // 
            // soundTimesLabel
            // 
            this.soundTimesLabel.AutoSize = true;
            this.soundTimesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soundTimesLabel.Location = new System.Drawing.Point(160, 165);
            this.soundTimesLabel.Name = "soundTimesLabel";
            this.soundTimesLabel.Size = new System.Drawing.Size(47, 12);
            this.soundTimesLabel.TabIndex = 14;
            this.soundTimesLabel.Text = "N/A times";
            // 
            // moveSoundButton
            // 
            this.moveSoundButton.Enabled = false;
            this.moveSoundButton.Location = new System.Drawing.Point(161, 185);
            this.moveSoundButton.Name = "moveSoundButton";
            this.moveSoundButton.Size = new System.Drawing.Size(45, 21);
            this.moveSoundButton.TabIndex = 17;
            this.moveSoundButton.Text = "Move";
            this.moveSoundButton.UseVisualStyleBackColor = true;
            this.moveSoundButton.Click += new System.EventHandler(this.moveSoundButton_Click);
            // 
            // moveToBox
            // 
            this.moveToBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.moveToBox.Enabled = false;
            this.moveToBox.FormattingEnabled = true;
            this.moveToBox.Items.AddRange(new object[] {
            "Trash"});
            this.moveToBox.Location = new System.Drawing.Point(55, 185);
            this.moveToBox.Name = "moveToBox";
            this.moveToBox.Size = new System.Drawing.Size(100, 21);
            this.moveToBox.TabIndex = 16;
            // 
            // moveToLabel
            // 
            this.moveToLabel.AutoSize = true;
            this.moveToLabel.Location = new System.Drawing.Point(7, 188);
            this.moveToLabel.Name = "moveToLabel";
            this.moveToLabel.Size = new System.Drawing.Size(50, 13);
            this.moveToLabel.TabIndex = 15;
            this.moveToLabel.Text = "MoveTo:";
            // 
            // addSoundButton
            // 
            this.addSoundButton.Enabled = false;
            this.addSoundButton.Location = new System.Drawing.Point(160, 103);
            this.addSoundButton.Name = "addSoundButton";
            this.addSoundButton.Size = new System.Drawing.Size(46, 23);
            this.addSoundButton.TabIndex = 7;
            this.addSoundButton.Text = "+";
            this.addSoundButton.UseVisualStyleBackColor = true;
            this.addSoundButton.Click += new System.EventHandler(this.addSoundButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Enabled = false;
            this.moveDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moveDownButton.Location = new System.Drawing.Point(160, 75);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(46, 23);
            this.moveDownButton.TabIndex = 6;
            this.moveDownButton.Text = "↓";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Enabled = false;
            this.moveUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moveUpButton.Location = new System.Drawing.Point(160, 47);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(46, 23);
            this.moveUpButton.TabIndex = 5;
            this.moveUpButton.Text = "↑";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // removeGroupButton
            // 
            this.removeGroupButton.Enabled = false;
            this.removeGroupButton.Location = new System.Drawing.Point(185, 20);
            this.removeGroupButton.Name = "removeGroupButton";
            this.removeGroupButton.Size = new System.Drawing.Size(21, 21);
            this.removeGroupButton.TabIndex = 3;
            this.removeGroupButton.Text = "-";
            this.removeGroupButton.UseVisualStyleBackColor = true;
            this.removeGroupButton.Click += new System.EventHandler(this.removeGroupButton_Click);
            // 
            // addGroupButton
            // 
            this.addGroupButton.Location = new System.Drawing.Point(160, 20);
            this.addGroupButton.Name = "addGroupButton";
            this.addGroupButton.Size = new System.Drawing.Size(21, 21);
            this.addGroupButton.TabIndex = 2;
            this.addGroupButton.Text = "+";
            this.addGroupButton.UseVisualStyleBackColor = true;
            this.addGroupButton.Click += new System.EventHandler(this.addGroupButton_Click);
            // 
            // saveNameButton
            // 
            this.saveNameButton.Enabled = false;
            this.saveNameButton.Location = new System.Drawing.Point(160, 135);
            this.saveNameButton.Name = "saveNameButton";
            this.saveNameButton.Size = new System.Drawing.Size(46, 20);
            this.saveNameButton.TabIndex = 10;
            this.saveNameButton.Text = "Save";
            this.saveNameButton.UseVisualStyleBackColor = true;
            this.saveNameButton.Click += new System.EventHandler(this.saveNameButton_Click);
            // 
            // soundGroupBox
            // 
            this.soundGroupBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.soundGroupBox.Enabled = false;
            this.soundGroupBox.FormattingEnabled = true;
            this.soundGroupBox.Location = new System.Drawing.Point(45, 20);
            this.soundGroupBox.Name = "soundGroupBox";
            this.soundGroupBox.Size = new System.Drawing.Size(110, 21);
            this.soundGroupBox.TabIndex = 1;
            this.soundGroupBox.SelectedIndexChanged += new System.EventHandler(this.soundGroupBox_SelectedIndexChanged);
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.Location = new System.Drawing.Point(7, 23);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(39, 13);
            this.groupLabel.TabIndex = 0;
            this.groupLabel.Text = "Group:";
            // 
            // soundNameBox
            // 
            this.soundNameBox.Enabled = false;
            this.soundNameBox.Location = new System.Drawing.Point(50, 135);
            this.soundNameBox.Name = "soundNameBox";
            this.soundNameBox.Size = new System.Drawing.Size(105, 20);
            this.soundNameBox.TabIndex = 9;
            this.soundNameBox.TextChanged += new System.EventHandler(this.soundNameBox_TextChanged);
            // 
            // soundboardList
            // 
            this.soundboardList.FormattingEnabled = true;
            this.soundboardList.Location = new System.Drawing.Point(10, 47);
            this.soundboardList.Name = "soundboardList";
            this.soundboardList.Size = new System.Drawing.Size(145, 82);
            this.soundboardList.TabIndex = 4;
            this.soundboardList.SelectedIndexChanged += new System.EventHandler(this.soundboardList_SelectedIndexChanged);
            // 
            // soundNameLabel
            // 
            this.soundNameLabel.AutoSize = true;
            this.soundNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soundNameLabel.Location = new System.Drawing.Point(7, 138);
            this.soundNameLabel.Name = "soundNameLabel";
            this.soundNameLabel.Size = new System.Drawing.Size(43, 13);
            this.soundNameLabel.TabIndex = 8;
            this.soundNameLabel.Text = "Name:";
            // 
            // soundHotkeyButton
            // 
            this.soundHotkeyButton.Enabled = false;
            this.soundHotkeyButton.Location = new System.Drawing.Point(57, 160);
            this.soundHotkeyButton.Name = "soundHotkeyButton";
            this.soundHotkeyButton.Size = new System.Drawing.Size(70, 20);
            this.soundHotkeyButton.TabIndex = 12;
            this.soundHotkeyButton.Text = "None";
            this.soundHotkeyButton.UseVisualStyleBackColor = true;
            this.soundHotkeyButton.Click += new System.EventHandler(this.HotkeyButton_Click);
            this.soundHotkeyButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.soundHotkeyButton_KeyDown);
            // 
            // soundHotkeyLabel
            // 
            this.soundHotkeyLabel.AutoSize = true;
            this.soundHotkeyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soundHotkeyLabel.Location = new System.Drawing.Point(7, 164);
            this.soundHotkeyLabel.Name = "soundHotkeyLabel";
            this.soundHotkeyLabel.Size = new System.Drawing.Size(51, 13);
            this.soundHotkeyLabel.TabIndex = 11;
            this.soundHotkeyLabel.Text = "Hotkey:";
            // 
            // soundUsedLabel
            // 
            this.soundUsedLabel.AutoSize = true;
            this.soundUsedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soundUsedLabel.Location = new System.Drawing.Point(125, 164);
            this.soundUsedLabel.Name = "soundUsedLabel";
            this.soundUsedLabel.Size = new System.Drawing.Size(40, 13);
            this.soundUsedLabel.TabIndex = 13;
            this.soundUsedLabel.Text = "Used:";
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayIconMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "HiT - VoIPSoundboard";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.ShowFormAgain);
            // 
            // trayIconMenu
            // 
            this.trayIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.makeHotkeysTXTMenuItem,
            this.separator1,
            this.setModeMenuItem,
            this.settingsMenuItem,
            this.separator2,
            this.saveMenuItem,
            this.exitMenuItem});
            this.trayIconMenu.Name = "trayIconMenu";
            this.trayIconMenu.Size = new System.Drawing.Size(164, 148);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.ShowFormAgain);
            // 
            // makeHotkeysTXTMenuItem
            // 
            this.makeHotkeysTXTMenuItem.Name = "makeHotkeysTXTMenuItem";
            this.makeHotkeysTXTMenuItem.Size = new System.Drawing.Size(163, 22);
            this.makeHotkeysTXTMenuItem.Text = "Make hotkeys.txt";
            this.makeHotkeysTXTMenuItem.Click += new System.EventHandler(this.makeHotkeysTXTMenuItem_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(160, 6);
            // 
            // setModeMenuItem
            // 
            this.setModeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceGameMenuItem,
            this.discordMenuItem,
            this.skypeMenuItem});
            this.setModeMenuItem.Name = "setModeMenuItem";
            this.setModeMenuItem.Size = new System.Drawing.Size(163, 22);
            this.setModeMenuItem.Text = "Set Mode";
            // 
            // sourceGameMenuItem
            // 
            this.sourceGameMenuItem.Name = "sourceGameMenuItem";
            this.sourceGameMenuItem.Size = new System.Drawing.Size(144, 22);
            this.sourceGameMenuItem.Text = "Source Game";
            this.sourceGameMenuItem.Click += new System.EventHandler(this.sourceGameMenuItem_Click);
            // 
            // discordMenuItem
            // 
            this.discordMenuItem.Name = "discordMenuItem";
            this.discordMenuItem.Size = new System.Drawing.Size(144, 22);
            this.discordMenuItem.Text = "Discord";
            this.discordMenuItem.Click += new System.EventHandler(this.discordMenuItem_Click);
            // 
            // skypeMenuItem
            // 
            this.skypeMenuItem.Name = "skypeMenuItem";
            this.skypeMenuItem.Size = new System.Drawing.Size(144, 22);
            this.skypeMenuItem.Text = "Skype";
            this.skypeMenuItem.Click += new System.EventHandler(this.skypeMenuItem_Click);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(163, 22);
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(160, 6);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveMenuItem.Text = "Save";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // settingsBox
            // 
            this.settingsBox.Controls.Add(this.recordHKButton);
            this.settingsBox.Controls.Add(this.recordHKLabel);
            this.settingsBox.Controls.Add(this.stopHKButton);
            this.settingsBox.Controls.Add(this.nextGroupHKButton);
            this.settingsBox.Controls.Add(this.priorGroupHKButton);
            this.settingsBox.Controls.Add(this.enableSBHKButton);
            this.settingsBox.Controls.Add(this.nextGroupHKLabel);
            this.settingsBox.Controls.Add(this.priorGroupHKLabel);
            this.settingsBox.Controls.Add(this.enableSBHKLabel);
            this.settingsBox.Controls.Add(this.stopHKLabel);
            this.settingsBox.Controls.Add(this.windowsStartupBox);
            this.settingsBox.Location = new System.Drawing.Point(10, 230);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(215, 165);
            this.settingsBox.TabIndex = 1;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // recordHKButton
            // 
            this.recordHKButton.Location = new System.Drawing.Point(100, 90);
            this.recordHKButton.Name = "recordHKButton";
            this.recordHKButton.Size = new System.Drawing.Size(70, 20);
            this.recordHKButton.TabIndex = 7;
            this.recordHKButton.Text = "None";
            this.recordHKButton.UseVisualStyleBackColor = true;
            // 
            // recordHKLabel
            // 
            this.recordHKLabel.AutoSize = true;
            this.recordHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordHKLabel.Location = new System.Drawing.Point(6, 94);
            this.recordHKLabel.Name = "recordHKLabel";
            this.recordHKLabel.Size = new System.Drawing.Size(96, 13);
            this.recordHKLabel.TabIndex = 6;
            this.recordHKLabel.Text = "Record Hotkey:";
            // 
            // stopHKButton
            // 
            this.stopHKButton.Location = new System.Drawing.Point(85, 115);
            this.stopHKButton.Name = "stopHKButton";
            this.stopHKButton.Size = new System.Drawing.Size(70, 20);
            this.stopHKButton.TabIndex = 13;
            this.stopHKButton.Text = "None";
            this.stopHKButton.UseVisualStyleBackColor = true;
            this.stopHKButton.Click += new System.EventHandler(this.HotkeyButton_Click);
            this.stopHKButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobaHotkeyButton_KeyDown);
            // 
            // nextGroupHKButton
            // 
            this.nextGroupHKButton.Location = new System.Drawing.Point(125, 65);
            this.nextGroupHKButton.Name = "nextGroupHKButton";
            this.nextGroupHKButton.Size = new System.Drawing.Size(70, 20);
            this.nextGroupHKButton.TabIndex = 5;
            this.nextGroupHKButton.Text = "None";
            this.nextGroupHKButton.UseVisualStyleBackColor = true;
            this.nextGroupHKButton.Click += new System.EventHandler(this.HotkeyButton_Click);
            this.nextGroupHKButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobaHotkeyButton_KeyDown);
            // 
            // priorGroupHKButton
            // 
            this.priorGroupHKButton.Location = new System.Drawing.Point(125, 40);
            this.priorGroupHKButton.Name = "priorGroupHKButton";
            this.priorGroupHKButton.Size = new System.Drawing.Size(70, 20);
            this.priorGroupHKButton.TabIndex = 3;
            this.priorGroupHKButton.Text = "None";
            this.priorGroupHKButton.UseVisualStyleBackColor = true;
            this.priorGroupHKButton.Click += new System.EventHandler(this.HotkeyButton_Click);
            this.priorGroupHKButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobaHotkeyButton_KeyDown);
            // 
            // enableSBHKButton
            // 
            this.enableSBHKButton.Location = new System.Drawing.Point(115, 15);
            this.enableSBHKButton.Name = "enableSBHKButton";
            this.enableSBHKButton.Size = new System.Drawing.Size(70, 20);
            this.enableSBHKButton.TabIndex = 1;
            this.enableSBHKButton.Text = "None";
            this.enableSBHKButton.UseVisualStyleBackColor = true;
            this.enableSBHKButton.Click += new System.EventHandler(this.HotkeyButton_Click);
            this.enableSBHKButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlobaHotkeyButton_KeyDown);
            // 
            // nextGroupHKLabel
            // 
            this.nextGroupHKLabel.AutoSize = true;
            this.nextGroupHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextGroupHKLabel.Location = new System.Drawing.Point(7, 69);
            this.nextGroupHKLabel.Name = "nextGroupHKLabel";
            this.nextGroupHKLabel.Size = new System.Drawing.Size(119, 13);
            this.nextGroupHKLabel.TabIndex = 4;
            this.nextGroupHKLabel.Text = "Next Group Hotkey:";
            // 
            // priorGroupHKLabel
            // 
            this.priorGroupHKLabel.AutoSize = true;
            this.priorGroupHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.priorGroupHKLabel.Location = new System.Drawing.Point(7, 44);
            this.priorGroupHKLabel.Name = "priorGroupHKLabel";
            this.priorGroupHKLabel.Size = new System.Drawing.Size(119, 13);
            this.priorGroupHKLabel.TabIndex = 2;
            this.priorGroupHKLabel.Text = "Prior Group Hotkey:";
            // 
            // enableSBHKLabel
            // 
            this.enableSBHKLabel.AutoSize = true;
            this.enableSBHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableSBHKLabel.Location = new System.Drawing.Point(7, 19);
            this.enableSBHKLabel.Name = "enableSBHKLabel";
            this.enableSBHKLabel.Size = new System.Drawing.Size(110, 13);
            this.enableSBHKLabel.TabIndex = 0;
            this.enableSBHKLabel.Text = "EnableSB Hotkey:";
            // 
            // stopHKLabel
            // 
            this.stopHKLabel.AutoSize = true;
            this.stopHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopHKLabel.Location = new System.Drawing.Point(7, 119);
            this.stopHKLabel.Name = "stopHKLabel";
            this.stopHKLabel.Size = new System.Drawing.Size(81, 13);
            this.stopHKLabel.TabIndex = 8;
            this.stopHKLabel.Text = "Stop Hotkey:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 406);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.soundboardBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HiT - VoIPSoundboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.soundboardBox.ResumeLayout(false);
            this.soundboardBox.PerformLayout();
            this.trayIconMenu.ResumeLayout(false);
            this.settingsBox.ResumeLayout(false);
            this.settingsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox windowsStartupBox;
        private System.Windows.Forms.GroupBox soundboardBox;
        private System.Windows.Forms.Button saveNameButton;
        private System.Windows.Forms.TextBox soundNameBox;
        private System.Windows.Forms.Button soundHotkeyButton;
        private System.Windows.Forms.Label soundUsedLabel;
        private System.Windows.Forms.Label soundHotkeyLabel;
        private System.Windows.Forms.Label soundNameLabel;
        private System.Windows.Forms.ListBox soundboardList;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Label soundTimesLabel;
        private System.Windows.Forms.ContextMenuStrip trayIconMenu;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.Button addSoundButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button removeGroupButton;
        private System.Windows.Forms.Button addGroupButton;
        private System.Windows.Forms.ComboBox soundGroupBox;
        private System.Windows.Forms.Label groupLabel;
        private System.Windows.Forms.ComboBox moveToBox;
        private System.Windows.Forms.Label moveToLabel;
        private System.Windows.Forms.Button moveSoundButton;
        private System.Windows.Forms.GroupBox settingsBox;
        private System.Windows.Forms.Label enableSBHKLabel;
        private System.Windows.Forms.Label stopHKLabel;
        private System.Windows.Forms.Button stopHKButton;
        private System.Windows.Forms.Button nextGroupHKButton;
        private System.Windows.Forms.Button priorGroupHKButton;
        private System.Windows.Forms.Button enableSBHKButton;
        private System.Windows.Forms.Label nextGroupHKLabel;
        private System.Windows.Forms.Label priorGroupHKLabel;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeHotkeysTXTMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripMenuItem setModeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discordMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skypeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.Button recordHKButton;
        private System.Windows.Forms.Label recordHKLabel;
        private System.Windows.Forms.ToolStripMenuItem sourceGameMenuItem;
    }
}

