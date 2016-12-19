using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;
using System.Threading.Tasks;

namespace IBE
{
    public partial class SplashScreenForm : Form
    {
        public readonly System.Threading.SynchronizationContext ThreadSynchronizationContext;

        public delegate void TransportStringDelegate(string text);
        public delegate void TransportWindowDataDelegate(WindowData windowData);
        public delegate void TransportBooleanDelegate(Boolean boolValue);
        public delegate void EventDelegate(Object sender, EventArgs e);

        private SingleThreadLogger _Logger = null;
        private Timer _CloseTimer;
        //private System.Windows.Forms.Timer _MinimizeTimer;
        private PerformanceTimer m_StartTimer;

        static SplashScreenForm SplashObject    = null;
        static Boolean oldTopmost               = true;

        public static Form GetPrimaryGUI(Form mainForm)
        {
            if((SplashObject == null) || (Program.SplashScreen.IsDisposed))
                return mainForm;
            else
                return SplashObject;
        }

        public static void SetTopmost(bool value)
        {
            if((SplashObject == null) || (Program.SplashScreen.IsDisposed))
                return;

            if(SplashObject.InvokeRequired)
            {
                SplashObject.Invoke(new TransportBooleanDelegate(SetTopmost), value);
                return;
            }

            if (SplashObject.WindowState == FormWindowState.Minimized)
                SplashObject.WindowState = FormWindowState.Normal;

            if (value)
            {
                Program.SplashScreen.TopMost = oldTopmost;

                if(!oldTopmost)
                    Program.SplashScreen.SendToBack();
            }
            else
            {
                oldTopmost = Program.SplashScreen.TopMost;
                Program.SplashScreen.TopMost = false;

                Program.SplashScreen.SendToBack();
            }
        }
        
        public SplashScreenForm()
        {
            InitializeComponent();

            ThreadSynchronizationContext = System.Threading.SynchronizationContext.Current;

            //_MinimizeTimer = new System.Windows.Forms.Timer(this.components);
            InfoTarget.Items.Clear();
            SplashObject = this;
            lblVersion.Text = "v" + VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3);
        }

        public SingleThreadLogger Logger
        {
            get
            {
                return _Logger;
            }
            set
            {
                _Logger = value;
            }
        }

        public void InfoAdd(string Info)
        { 
            if(this.InvokeRequired)
            {
                this.Invoke(new TransportStringDelegate(InfoAdd), Info);
                return;
            }

            InfoTarget.Items.Insert(InfoTarget.Items.Count, Info);
            InfoTarget.SelectedIndex = InfoTarget.Items.Count-1;
            InfoTarget.SelectedIndex = -1;
            this.Refresh();

            if (_Logger != null)
                _Logger.Log(Info);
        }

        public void InfoChange(string Info)
        { 
            if(this.InvokeRequired)
            {
                this.Invoke(new TransportStringDelegate(InfoChange), Info);
                return;
            }

            InfoTarget.Items[InfoTarget.Items.Count-1] = Info;
            InfoTarget.SelectedIndex = InfoTarget.Items.Count-1;
            InfoTarget.SelectedIndex = -1;
            this.Refresh();

            if (_Logger != null)
                _Logger.Log(Info);
        }

        public void InfoAppendLast(string Info)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new TransportStringDelegate(InfoAppendLast), Info);
                return;
            }

            InfoTarget.Items[InfoTarget.Items.Count - 1] += Info;
            InfoTarget.SelectedIndex = InfoTarget.Items.Count - 1;
            InfoTarget.SelectedIndex = -1;
            Refresh();

            if (_Logger != null)
                _Logger.Log(Info);
        }

        public void CloseDelayed()
        {
            _CloseTimer = new System.Windows.Forms.Timer();
            _CloseTimer.Tick += _CloseTimer_Tick;
            _CloseTimer.Interval = 1000;
            _CloseTimer.Start();
        }

        public void CloseImmediately()
        {
            _CloseTimer = new System.Windows.Forms.Timer();
            _CloseTimer.Tick += _CloseTimer_Tick;
            _CloseTimer.Interval = 1;
            _CloseTimer.Start();
        }

        void _CloseTimer_Tick(object sender, EventArgs e)
        {
            _CloseTimer.Stop();
            this.Invoke(new MethodInvoker(Close_internal));
        }

        private void Close_internal()
        {
            _CloseTimer.Dispose();
            _Logger = null;
            SplashObject = null;
            this.Close();
        }

        internal void setPosition(WindowData windowData)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new TransportWindowDataDelegate(setPosition), windowData);
                return;
            }

            if((windowData != null) && (windowData.Position.Top >= 0))
            {
                System.Drawing.Rectangle rec_WA = Screen.FromRectangle(windowData.Position).WorkingArea;

                this.Location = new Point((Int32)(rec_WA.X + ((rec_WA.Width - this.Width) / 2)), (Int32)(rec_WA.Y + ((rec_WA.Height - this.Height) / 2)));
            }
        }

        private void cmdMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void SplashScreenForm_Shown(object sender, EventArgs e)
        {
            m_StartTimer = new PerformanceTimer();
            m_StartTimer.startMeasuring();
        }

        /// <summary>
        /// minimizes the form at latest n seconds after first shown
        /// </summary>
        /// <param name="minimizeTime"></param>
        public async void AutoMinimizeAsync(Int32 minimizeTime=2000)
        {
            Int32 restTime = (Int32)(minimizeTime - m_StartTimer.currentMeasuring());
            
            if(restTime < 1)
                restTime = 1;

            await Task.Delay(restTime);

            Minimize();
        }

        private void Minimize()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(Minimize));
                return;
            }

            this.WindowState = FormWindowState.Minimized;
        }
    }
}
