using System;
using System.Drawing;
using System.Windows.Forms;

namespace RegulatedNoise
{
    public partial class SplashScreenForm : Form
    {
        Timer _closeTimer;
        public SplashScreenForm()
        {
            InitializeComponent();
            listBox1.Items.Add("");
        }

        public void InfoAdd(string info)
        { 
            listBox1.Items.Insert(listBox1.Items.Count-1, info);
            listBox1.SelectedIndex = listBox1.Items.Count-1;
            listBox1.SelectedIndex = -1;
            this.Refresh();
        }

        public void InfoChange(string info)
        { 
            listBox1.Items[listBox1.Items.Count-2] = info;
            listBox1.SelectedIndex = listBox1.Items.Count-1;
            listBox1.SelectedIndex = -1;
            this.Refresh();
        }

        public void CloseDelayed()
        {
            _closeTimer = new Timer();
            _closeTimer.Tick += _CloseTimer_Tick;
            _closeTimer.Interval = 1000;
            _closeTimer.Start();
            this.Refresh();
        }

        void _CloseTimer_Tick(object sender, EventArgs e)
        {
            _closeTimer.Dispose();
            Close();
        }

        internal void SetPosition(WindowData windowData)
        {
            if((windowData != null) && (windowData.Position.Top >= 0))
            {
                Rectangle rec_WA = Screen.FromRectangle(windowData.Position).WorkingArea;
                Location = new Point((Int32)(rec_WA.X + ((rec_WA.Width - this.Width) / 2)), (Int32)(rec_WA.Y + ((rec_WA.Height - this.Height) / 2)));
            }
        }
    }
}
