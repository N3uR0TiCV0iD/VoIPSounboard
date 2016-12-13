namespace HiT.VoIPSoundboard
{
    partial class WhatsNewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhatsNewForm));
            this.whatsNewBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // whatsNewBox
            // 
            this.whatsNewBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whatsNewBox.Location = new System.Drawing.Point(0, 0);
            this.whatsNewBox.Multiline = true;
            this.whatsNewBox.Name = "whatsNewBox";
            this.whatsNewBox.ReadOnly = true;
            this.whatsNewBox.Size = new System.Drawing.Size(434, 162);
            this.whatsNewBox.TabIndex = 0;
            this.whatsNewBox.TabStop = false;
            // 
            // WhatsNewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 162);
            this.Controls.Add(this.whatsNewBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WhatsNewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VoIPSoundboard - What\'s new?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox whatsNewBox;
    }
}