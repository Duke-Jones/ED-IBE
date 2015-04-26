using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace System.Windows.Forms
{
    class ComboBox_ro : ComboBox
    {
        public ComboBox_ro()
        {
            textBox = new TextBox();
            textBox.ReadOnly = true;
            textBox.Visible = false;
        }

        private TextBox textBox;

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
                    textBox.Text = this.Text;
                    textBox.Location = this.Location;
                    textBox.Size = this.Size;
                    textBox.Visible = true;

                    if (textBox.Parent == null)
                        this.Parent.Controls.Add(textBox);

                    textBox.Font = this.Font;
                }
                else
                {
                    this.Visible = true;
                    this.textBox.Visible = false;
                }
            }
        }

        public bool Visible_ro
        {
            get { return this.Visible; }
            set
            {
                this.Visible           = value;
                this.textBox.Visible   = value;
            }
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
