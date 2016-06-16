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
            if (!RNBaseForm.IsDesignMode(this))
            { 
                loadWindowPosition();
                Retheme();
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

        // Recurse controls on form
        public IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl))
                                      .Concat(controls);
        }

        /// <summary>
        /// Checks whether a control or its parent is in design mode.
        /// </summary>
        /// <param name="c">The control to check.</param>
        /// <returns>Returns TRUE if in design mode, false otherwise.</returns>
        public static bool IsDesignMode(Control c )
        {
          if ( c == null )
          {
            return false;
          }
          else
          {
            while ( c != null )
            {
              if ( c.Site != null && c.Site.DesignMode )
              {
                return true;
              }
              else
              {
                c = c.Parent;
              }
            }
 
            return false;
          }
        }

        protected void Retheme()
        {
            bool noBackColor = false;

            Dictionary<String, Boolean> ignoreList = new Dictionary<string, bool>() {{"lblDonate", false},
                                                                                   {"lbEDDNInfo", false}};

            try
            {
                if (!Program.Colors.UseColors) return;

                var x = GetAll(this);

                var f = Program.Colors.GetColor(GUIColors.ColorNames.Default_ForeColor);
                var b = Program.Colors.GetColor(GUIColors.ColorNames.Default_BackColor);

                foreach (Control c in x)
                {
                    if (!ignoreList.ContainsKey(c.Name))
                    {
                        noBackColor = SetControlColors(c, f, b);
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

        public static bool SetControlColors(Control controlObject, Color newForeColor, Color newBackColor, Boolean reset = false)
        {
            bool noBackColor;
            var props = controlObject.GetType().GetProperties().Select(y => y.Name);

            noBackColor = false;

            controlObject.BackColor = newBackColor;
            controlObject.ForeColor = newForeColor;

            if (props.Contains("FlatStyle"))
            {
                var prop = controlObject.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

                if(reset)
                    prop.SetValue(controlObject, FlatStyle.Standard);
                else
                    prop.SetValue(controlObject, FlatStyle.Flat);
            }

            if (props.Contains("FlatStyle"))
            {
                var prop = controlObject.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

                prop.SetValue(controlObject, FlatStyle.Flat);
            }
            if (props.Contains("BorderStyle") && controlObject.GetType() != typeof(Label))
            {
                var prop = controlObject.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

                prop.SetValue(controlObject, BorderStyle.FixedSingle);
            }
            if (props.Contains("LinkColor"))
            {
                var prop = controlObject.GetType().GetProperty("LinkColor", BindingFlags.Public | BindingFlags.Instance);

                prop.SetValue(controlObject, newForeColor);
            }
            if (props.Contains("BackColor_ro"))
            {
                var prop = controlObject.GetType().GetProperty("BackColor_ro", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newBackColor);
            }
            if (props.Contains("ForeColor_ro"))
            {
                var prop = controlObject.GetType().GetProperty("ForeColor_ro", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newForeColor);
            }
            if (props.Contains("BackgroundColor"))
            {
                var prop = controlObject.GetType().GetProperty("BackgroundColor", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newBackColor);
            }
            if (props.Contains("GridColor"))
            {
                var prop = controlObject.GetType().GetProperty("GridColor", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newForeColor);
            }
            if (props.Contains("DefaultCellStyle"))
            {
                // DataGridView
                var prop = controlObject.GetType().GetProperty("DefaultCellStyle", BindingFlags.Public | BindingFlags.Instance);

                var propsCellStyle = prop.GetType().GetProperties().Select(y => y.Name);

                if (propsCellStyle.Contains("BackColor"))
                {
                    var prop2 = propsCellStyle.GetType().GetProperty("BackColor", BindingFlags.Public | BindingFlags.Instance);
                    prop2.SetValue(controlObject, newBackColor);
                }
                if (propsCellStyle.Contains("ForeColor"))
                {
                    var prop2 = propsCellStyle.GetType().GetProperty("ForeColor", BindingFlags.Public | BindingFlags.Instance);
                    prop2.SetValue(controlObject, newForeColor);
                }
            }
            if (props.Contains("Columns") && controlObject.GetType() == typeof(DataGridViewExt))
            {

                DataGridViewExt dgv = (DataGridViewExt)controlObject;

                dgv.EnableHeadersVisualStyles = false;

                dgv.RowHeadersDefaultCellStyle.BackColor = newForeColor;
                dgv.RowHeadersDefaultCellStyle.ForeColor = newBackColor;

                dgv.ColumnHeadersDefaultCellStyle.BackColor = newForeColor;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = newBackColor;


                // DataGridView
                var prop = controlObject.GetType().GetProperty("Columns", BindingFlags.Public | BindingFlags.Instance);

                var propValues = (DataGridViewColumnCollection)prop.GetValue(controlObject, null);

                foreach (DataGridViewColumn propValue in propValues)
                {
                    propValue.DefaultCellStyle.ForeColor = newForeColor;
                    propValue.DefaultCellStyle.BackColor = newBackColor;
                    propValue.HeaderCell.Style.BackColor = newForeColor;
                    propValue.HeaderCell.Style.ForeColor = newBackColor;
                }
            }
            return noBackColor;
        }
    }
}
