using System;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
namespace HiT.VoIPSoundboard
{
    public partial class ToastInfoForm : Form
    {
        const int WAIT_TIME = 2500;
        Thread idleThread;
        Stopwatch idleTimer;
        Rectangle workingArea;
        public ToastInfoForm(string message)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.idleThread = new Thread(() => WaitIdleStatus());
            this.workingArea = Screen.PrimaryScreen.WorkingArea;
            this.idleTimer = new Stopwatch();
            InitializeComponent();
            this.Message = message;
        }
        private void ToastInfoForm_Load(object sender, EventArgs e)
        {
            this.showUpdater.Enabled = true;
        }
        public string Message
        {
            set
            {
                if (messageLabel.Text != String.Empty)
                {
                    idleTimer.Reset();
                    idleTimer.Start();
                }
                messageLabel.Text = value;
                this.Width = 55 + TextRenderer.MeasureText(value, messageLabel.Font).Width + 5;
                this.Location = new Point(workingArea.Width - this.Width, this.Location.Y);
            }
        }
        private void showUpdater_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.25;
            this.Height = (int)Math.Floor(50 * this.Opacity);
            this.Location = new Point(this.Location.X, workingArea.Height - this.Height);
            if (this.Opacity == 1)
            {
                showUpdater.Enabled = false;
                idleThread.Start();
                idleTimer.Start();
            }
        }
        private void WaitIdleStatus()
        {
            while (idleTimer.ElapsedMilliseconds <= WAIT_TIME)
            {
                Thread.Sleep(250);
            }
            this.Invoke((MethodInvoker)delegate
            {
                hideUpdater.Enabled = true;
            });
        }
        private void hideUpdater_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.25;
            this.Height = (int)Math.Floor(50 * this.Opacity);
            this.Location = new Point(this.Location.X, workingArea.Height - this.Height);
            if (this.Opacity == 0)
            {
                this.Close();
            }
        }
    }
}
