using System;
using System.IO;
using System.Windows.Forms;
namespace HiT.VoIPSoundboard
{
    public partial class NewGroupDialog : Form
    {
        MainForm mainForm;
        public NewGroupDialog(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }
        public string GroupName
        {
            get
            {
                return newGroupBox.Text;
            }
        }
        private void newGroupBox_TextChanged(object sender, EventArgs e)
        {
            OK_Button.Enabled = newGroupBox.Text != String.Empty && !mainForm.ContainsGroup(newGroupBox.Text);
        }
        private void OK_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
