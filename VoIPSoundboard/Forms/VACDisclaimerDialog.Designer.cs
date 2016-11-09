namespace HiT.VoIPSoundboard
{
    partial class VACDisclaimerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VACDisclaimerDialog));
            this.agreementBox = new System.Windows.Forms.CheckBox();
            this.disclaimerLabel = new System.Windows.Forms.Label();
            this.OK_Button = new System.Windows.Forms.Button();
            this.CANCEL_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // agreementBox
            // 
            this.agreementBox.AutoSize = true;
            this.agreementBox.Location = new System.Drawing.Point(15, 165);
            this.agreementBox.Name = "agreementBox";
            this.agreementBox.Size = new System.Drawing.Size(249, 17);
            this.agreementBox.TabIndex = 1;
            this.agreementBox.Text = "I understand and agree with the warning above";
            this.agreementBox.UseVisualStyleBackColor = true;
            this.agreementBox.CheckedChanged += new System.EventHandler(this.agreementBox_CheckedChanged);
            // 
            // disclaimerLabel
            // 
            this.disclaimerLabel.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disclaimerLabel.Location = new System.Drawing.Point(10, 15);
            this.disclaimerLabel.Name = "disclaimerLabel";
            this.disclaimerLabel.Size = new System.Drawing.Size(265, 135);
            this.disclaimerLabel.TabIndex = 0;
            this.disclaimerLabel.Text = "WARNING: This can and might get you VAC banned, use it at your own risk.\r\n\r\nI AM " +
    "NOT held responsible if you use it and get banned.";
            // 
            // OK_Button
            // 
            this.OK_Button.Enabled = false;
            this.OK_Button.Location = new System.Drawing.Point(170, 200);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(100, 23);
            this.OK_Button.TabIndex = 2;
            this.OK_Button.Text = "Continue";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // CANCEL_Button
            // 
            this.CANCEL_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CANCEL_Button.Location = new System.Drawing.Point(15, 200);
            this.CANCEL_Button.Name = "CANCEL_Button";
            this.CANCEL_Button.Size = new System.Drawing.Size(100, 23);
            this.CANCEL_Button.TabIndex = 3;
            this.CANCEL_Button.Text = "Cancel";
            this.CANCEL_Button.UseVisualStyleBackColor = true;
            this.CANCEL_Button.Click += new System.EventHandler(this.CANCEL_Button_Click);
            // 
            // VACDisclaimerDialog
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CANCEL_Button;
            this.ClientSize = new System.Drawing.Size(284, 237);
            this.Controls.Add(this.CANCEL_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.disclaimerLabel);
            this.Controls.Add(this.agreementBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VACDisclaimerDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VoIPSoundboard - VAC Disclaimer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox agreementBox;
        private System.Windows.Forms.Label disclaimerLabel;
        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Button CANCEL_Button;
    }
}