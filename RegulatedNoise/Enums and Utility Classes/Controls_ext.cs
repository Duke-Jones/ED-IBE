using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    class ComboBox_ro : ComboBox
    {
        public ComboBox_ro()
        {
            TextBox_ro = new TextBox();
            TextBox_ro.ReadOnly = true;
            TextBox_ro.Visible = false;
        }

        public TextBox TextBox_ro;

        private bool readOnly = false;

        public bool ReadOnly
        {
            get { return readOnly; }
            set
            {
                readOnly = value;

                if (readOnly)
                {
                    this.Visible = false;
                    TextBox_ro.Text = this.Text;
                    TextBox_ro.Location = this.Location;
                    TextBox_ro.Size = this.Size;
                    TextBox_ro.Visible = true;

                    if (TextBox_ro.Parent == null)
                        this.Parent.Controls.Add(TextBox_ro);

                    TextBox_ro.Font = this.Font;
                }
                else
                {
                    this.Visible = true;
                    this.TextBox_ro.Visible = false;
                }
            }
        }

        public bool Visible_ro
        {
            get { return this.Visible; }
            set
            {
                if (value)
                {
                    ReadOnly = ReadOnly;
                }
                else
                {
                    this.Visible = value;
                    this.TextBox_ro.Visible = value;
                }

            }
        }

        public Color BackColor_ro { 
            get { return TextBox_ro.BackColor;}
            set { TextBox_ro.BackColor = value; }
        }

        public Color ForeColor_ro { 
            get { return TextBox_ro.ForeColor;}
            set { TextBox_ro.ForeColor = value; }
        }
    }

    public class CheckBox_ro : CheckBox
    {
            private bool readOnly;

            protected override void OnClick(EventArgs e)
            {
                    // pass the event up only if its not readlonly
                    if (!ReadOnly) base.OnClick(e);
            }

            public bool ReadOnly
            {
                    get { return readOnly; }
                    set { readOnly = value; }
            }
    }
}
