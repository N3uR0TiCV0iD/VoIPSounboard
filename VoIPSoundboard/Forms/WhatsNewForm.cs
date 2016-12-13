using System;
using System.Windows.Forms;
using HiT.VoIPSoundboard.Properties;
namespace HiT.VoIPSoundboard
{
    public partial class WhatsNewForm : Form
    {
        public const bool DEBUG_SHOWFORM = true;
        public WhatsNewForm()
        {
            InitializeComponent();
            whatsNewBox.Text = Resources.WhatsNew;
        }
    }
}
