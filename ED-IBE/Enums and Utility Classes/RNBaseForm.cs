using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace IBE.Enums_and_Utility_Classes
{
    public partial class RNBaseForm : Form
    {
        public virtual string thisObjectName { get { return ""; } }

        private bool               m_LoadingDone       = false;
        private WindowData         m_Buffer            = new WindowData();

        public bool                DoPositioning { get; set; }
        public Control             ParentControl   { get; set; }

        public RNBaseForm()
        {
            DoPositioning = true;
            ParentControl = null;

            // Make the GUI ignore the DPI setting
            //Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);

            InitializeComponent();
        }

        protected void loadWindowPosition()
        {
            if (Program.DBCon == null)
                return;

            string Classname            = this.GetType().Name;

            if(DoPositioning  && (Program.DBCon.getIniValue(Classname, "Location") != ""))
            {
                m_Buffer.LocationString = Program.DBCon.getIniValue(Classname, "Location",    m_Buffer.LocationString, false);
                m_Buffer.StateString    = Program.DBCon.getIniValue(Classname, "WindowState", m_Buffer.StateString,    false);

                m_Buffer.SetValuesToForm(this);
            }
            else if(ParentControl != null)
            {
                Point center = ParentControl.PointToScreen(new Point(ParentControl.Width / 2, ParentControl.Height / 2));
                this.Location = new Point(center.X - (this.Width / 2), (Int32)Math.Round(center.Y * 0.75 - (this.Height / 2), 0));
            }
            
            m_LoadingDone = true;
        }

        public WindowData GetWindowData()
        {
            WindowData retValue     = new WindowData();
            String     Classname    = this.GetType().Name;

            if(Program.DBCon.getIniValue(Classname, "Location") != "")
            {
                retValue.LocationString = Program.DBCon.getIniValue(Classname, "Location",    m_Buffer.LocationString, false);
                retValue.StateString    = Program.DBCon.getIniValue(Classname, "WindowState", m_Buffer.StateString,    false);
            }

            return retValue;
        }

        protected void saveWindowPosition()
        {
            string Classname        = this.GetType().Name;
    
            if (this.WindowState != FormWindowState.Minimized)
                if (m_Buffer.State != this.WindowState)
                {
                    m_Buffer.State = this.WindowState;
                    Program.DBCon.setIniValue(Classname, "WindowState", m_Buffer.StateString);
                }

            if (this.WindowState == FormWindowState.Normal)
            {
                if ((m_Buffer.Position.Y        != this.Top) ||
                    (m_Buffer.Position.X        != this.Left) ||
                    (m_Buffer.Position.Height   != this.Height) ||
                    (m_Buffer.Position.Width    != this.Width))
                {
                    m_Buffer.GetValuesFromForm(this);
                    Program.DBCon.setIniValue(Classname, "Location", m_Buffer.LocationString);
                }
            }
        }

        protected void Form_Resize(object sender, System.EventArgs e)
        {
            if (m_LoadingDone)
                saveWindowPosition();
        }

        protected void Form_ResizeEnd(object sender, System.EventArgs e)
        {
            if (m_LoadingDone)
                saveWindowPosition();
        }

        private void Form_Shown(object sender, System.EventArgs e)
        {
            if (!this.IsDesignMode())
            { 
                loadWindowPosition();
            }

            this.Icon = Properties.Resources.IBE;
        }

        /// <summary>
        /// shows the form, also if it's minimized or in the background
        /// </summary>
        public void ShowEx()
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            this.Show();
            this.BringToFront();
        }

        private void RNBaseForm_Load(object sender, EventArgs e)
        {
            if (!this.IsDesignMode())
            { 
                this.Retheme();
            }

        }
    }
}
