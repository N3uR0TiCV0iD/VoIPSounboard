using System;
using System.Drawing;
using System.Windows.Forms;
namespace HiT.VoIPSoundboard
{
    public class FlatProgressBar : Control
    {
        int min;
        int max;
        int value;
        public FlatProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.max = 100;
        }
        public int Minimum
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
                this.Refresh();
            }
        }
        public int Maximum
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
                this.Refresh();
            }
        }
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value < this.min)
                {
                    //Throw exception
                }
                else if (value > this.max)
                {
                    //Throw exception
                }
                else
                {
                    this.value = value;
                    this.Refresh();
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Control, 0, 0, this.Width - 1, this.Height - 1);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            e.Graphics.FillRectangle(this.Enabled ? new SolidBrush(this.BackColor) : SystemBrushes.ControlDark, 1, 1, ((this.Width - 2) / (float)(max - min)) * (value - min), this.Height - 2);
        }
    }
}
