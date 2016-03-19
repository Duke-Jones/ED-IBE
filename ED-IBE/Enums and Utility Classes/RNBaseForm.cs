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
            loadWindowPosition();
            if (Program.DBCon != null)
                this.Icon = Properties.Resources.RegulatedNoise;
        }

        /// <summary>
        /// shows the form, also if it'currentPriceData minimized or in the background
        /// </summary>
        public void ShowEx()
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            this.Show();
            this.BringToFront();
        }

        // Recurse controls on form
        public IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl))
                                      .Concat(controls);
        }

        protected void Retheme()
        {
            bool noBackColor = false;

            try
            {
                if (Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "ForegroundColour", "") == "" || 
                    Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "BackgroundColour", "") == "") return;

                var x = GetAll(this);

                int redF = int.Parse(Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "ForegroundColour").Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                int greenF = int.Parse(Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "ForegroundColour").Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                int blueF = int.Parse(Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "ForegroundColour").Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                var f = Color.FromArgb(redF, greenF, blueF);
                int redB = int.Parse(Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "BackgroundColour").Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                int greenB = int.Parse(Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "BackgroundColour").Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                int blueB = int.Parse(Program.DBCon.getIniValue<String>(IBE.IBESettings.DB_GROUPNAME, "BackgroundColour").Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                var b = Color.FromArgb(redB, greenB, blueB);

                foreach (Control c in x)
                {
                    var props = c.GetType().GetProperties().Select(y => y.Name);

                    noBackColor = false;

                    c.BackColor = b;
                    c.ForeColor = f;
                    if (props.Contains("FlatStyle"))
                    {
                        var prop = c.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

                        prop.SetValue(c, FlatStyle.Flat);
                    }
                    if (props.Contains("BorderStyle") && c.GetType() != typeof(Label))
                    {
                        var prop = c.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

                        prop.SetValue(c, BorderStyle.FixedSingle);
                    }
                    if (props.Contains("LinkColor"))
                    {
                        var prop = c.GetType().GetProperty("LinkColor", BindingFlags.Public | BindingFlags.Instance);

                        prop.SetValue(c, f);
                    }
                    if (props.Contains("BackColor_ro"))
                    {
                        var prop = c.GetType().GetProperty("BackColor_ro", BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(c, b);
                    }
                    if (props.Contains("ForeColor_ro"))
                    {
                        var prop = c.GetType().GetProperty("ForeColor_ro", BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(c, f);
                    }
                    if (props.Contains("BackgroundColor"))
                    {
                        var prop = c.GetType().GetProperty("BackgroundColor", BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(c, b);
                    }
                    if (props.Contains("GridColor"))
                    {
                        var prop = c.GetType().GetProperty("GridColor", BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(c, f);
                    }
                    if (props.Contains("DefaultCellStyle"))
                    {
                        // DataGridView
                        var prop = c.GetType().GetProperty("DefaultCellStyle", BindingFlags.Public | BindingFlags.Instance);

                        var propsCellStyle = prop.GetType().GetProperties().Select(y => y.Name);

                        if (propsCellStyle.Contains("BackColor"))
                        {
                            var prop2 = propsCellStyle.GetType().GetProperty("BackColor", BindingFlags.Public | BindingFlags.Instance);
                            prop2.SetValue(c, b);
                        }
                        if (propsCellStyle.Contains("ForeColor"))
                        {
                            var prop2 = propsCellStyle.GetType().GetProperty("ForeColor", BindingFlags.Public | BindingFlags.Instance);
                            prop2.SetValue(c, f);
                        }
                    }
                    if (props.Contains("Columns") && c.GetType() == typeof(DataGridViewExt))
                    {

                        DataGridViewExt dgv = (DataGridViewExt)c;

                        dgv.EnableHeadersVisualStyles = false;

                        dgv.RowHeadersDefaultCellStyle.BackColor = f;
                        dgv.RowHeadersDefaultCellStyle.ForeColor = b;

                        dgv.ColumnHeadersDefaultCellStyle.BackColor = f;
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = b;


                        // DataGridView
                        var prop = c.GetType().GetProperty("Columns", BindingFlags.Public | BindingFlags.Instance);

                        var propValues = (DataGridViewColumnCollection)prop.GetValue(c, null);

                        foreach (DataGridViewColumn propValue in propValues)
                        {
                            propValue.DefaultCellStyle.ForeColor = f;
                            propValue.DefaultCellStyle.BackColor = b;
                            propValue.HeaderCell.Style.BackColor = f;
                            propValue.HeaderCell.Style.ForeColor = b;
                        }
                    }
                }

                //if(!noBackColor)
                    BackColor = b;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retheming-function", ex);
            }
        }
    }
}
