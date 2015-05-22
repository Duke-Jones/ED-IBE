using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    public partial class RNBaseForm : Form
    {
        public virtual string thisObjectName { get { return ""; } }

        private bool m_LoadingDone = false;

        public RNBaseForm()
        {
            InitializeComponent();
        }

        protected void loadWindowPosition()
        {
            if (Form1.RegulatedNoiseSettings == null)
                return;

            string Classname        = this.GetType().Name;
            WindowData FormPosition;

            if (Form1.RegulatedNoiseSettings.WindowBaseData.TryGetValue(Classname, out FormPosition))
            {

                if (FormPosition.Position.Height > -1)
                {
                    this.Top = FormPosition.Position.Top;
                    this.Left = FormPosition.Position.Left;
                    this.Height = FormPosition.Position.Height;
                    this.Width = FormPosition.Position.Width;

                    this.WindowState = FormPosition.State;
                }
                else
                {
                    FormPosition.Position.Y = this.Top;
                    FormPosition.Position.X = this.Left;
                    FormPosition.Position.Height = this.Height;
                    FormPosition.Position.Width = this.Width;

                    FormPosition.State = this.WindowState;
                }

            }
            else
            {
                Form1.RegulatedNoiseSettings.WindowBaseData.Add(Classname, new WindowData());
                loadWindowPosition();
                //MessageBox.Show("Not positioninfo for <" + Classname + "> found !");
            }

            m_LoadingDone = true;
        }

        protected void saveWindowPosition()
        {
            bool changed = false;

            string Classname        = this.GetType().Name;
            WindowData FormPosition;

            if (Form1.RegulatedNoiseSettings.WindowBaseData.TryGetValue(Classname, out FormPosition))
            {
                if (this.WindowState != FormWindowState.Minimized)
                    if (FormPosition.State != this.WindowState)
                    {
                        FormPosition.State = this.WindowState;
                        changed = true;
                    }

                if (this.WindowState == FormWindowState.Normal)
                {
                    if ((FormPosition.Position.Y != this.Top) ||
                        (FormPosition.Position.X != this.Left) ||
                        (FormPosition.Position.Height != this.Height) ||
                        (FormPosition.Position.Width != this.Width))
                    {
                        FormPosition.Position.Y = this.Top;
                        FormPosition.Position.X = this.Left;
                        FormPosition.Position.Height = this.Height;
                        FormPosition.Position.Width = this.Width;

                        changed = true;
                    }
                }

                if (changed)
                {
                    //SaveSettings();
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
        }

    }
}
