using System;
using System.Windows.Forms;
namespace HiT.VoIPSoundboard
{
    public partial class VACDisclaimerDialog : Form
    {
        public VACDisclaimerDialog()
        {
            InitializeComponent();
        }
        private void agreementBox_CheckedChanged(object sender, EventArgs e)
        {
            OK_Button.Enabled = agreementBox.Checked;
        }
        private void OK_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void CANCEL_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
