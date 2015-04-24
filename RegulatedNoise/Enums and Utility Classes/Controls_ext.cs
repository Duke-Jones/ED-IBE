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
                }
                else
                {
                    this.Visible = true;
                    this.textBox.Visible = false;
                }
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
