using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using RegulatedNoise.Enums_and_Utility_Classes;


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

                if (readOnly && !((Control)this).IsDesignMode())
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

        private bool Visible_ro
        {
            get { return this.Visible; }
            set
            {
                if (value || ((Control)this).IsDesignMode())
                {
                    ReadOnly = ReadOnly;
                }
                else
                {
                    this.Visible = value;
                    this.TextBox_ro.Visible = !value;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

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

    class DateTimePicker_ro : DateTimePicker
    {
        public DateTimePicker_ro()
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

                if (readOnly && !((Control)this).IsDesignMode())
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

        private bool Visible_ro
        {
            get { return this.Visible; }
            set
            {
                if (value || ((Control)this).IsDesignMode())
                {
                    ReadOnly = ReadOnly;
                }
                else
                {
                    this.Visible = value;
                    this.TextBox_ro.Visible = !value;
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

    class NumericUpDown_ro : NumericUpDown
    {
        public TextBox TextBox_ro;

        private bool readOnly = false;

        public NumericUpDown_ro()
        {
            TextBox_ro              = new TextBox();
            TextBox_ro.ReadOnly     = true;
            TextBox_ro.Visible      = false;
        }

        public new bool ReadOnly
        {
            get { return readOnly; }
            set
            {
                readOnly = value;

                if (readOnly && !((Control)this).IsDesignMode())
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

        private bool Visible_ro
        {
            get { return this.Visible; }
            set
            {
                if (value || ((Control)this).IsDesignMode())
                {
                    ReadOnly = ReadOnly;
                }
                else
                {
                    this.Visible = value;
                    this.TextBox_ro.Visible = !value;
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

        public new HorizontalAlignment TextAlign { 
            get{
                return ((NumericUpDown)this).TextAlign;
            } 
            set
            {
                 ((NumericUpDown)this).TextAlign    = value;
                this.TextBox_ro.TextAlign           = value;
            }
        }
    }

    public class TextBoxInt32 : TextBox
    {
        [System.ComponentModel.Browsable(true)]
        public int? MinValue { get; set; }

        [System.ComponentModel.Browsable(true)]
        public int? MaxValue { get; set; }

        /// <summary>
        ///  checks if the value is within its borders
        /// </summary>
        /// <returns></returns>
        public Boolean checkValue()
        {
            Boolean isOk = true;
            Int32 IntValue = 0;

            if(Int32.TryParse(this.Text, out IntValue))
            {
                if(MinValue.HasValue && (IntValue < MinValue))
                    isOk = false;

                if(MaxValue.HasValue && (IntValue > MaxValue))
                    isOk = false;
            }
            else
                isOk = false;
            
            return isOk;
        }
    }

    public class TextBoxDouble : TextBox
    {
        [System.ComponentModel.Browsable(true)]
        public Double? MinValue { get; set; }

        [System.ComponentModel.Browsable(true)]
        public Double? MaxValue { get; set; }

        [System.ComponentModel.Browsable(true)]
        public int? Digits { get; set; }

        /// <summary>
        ///  checks if the value is within its borders
        /// </summary>
        /// <returns></returns>
        public Boolean checkValue()
        {
            Boolean isOk = true;
            Double DoubleValue = 0;

            if(Double.TryParse(this.Text, out DoubleValue))
            {
                if(Digits != null)
                { 
                    DoubleValue = Math.Round(DoubleValue, (Int32)Digits);
                    this.Text = DoubleValue.ToString(String.Format("F{0}", Digits));
                }

                if(MinValue.HasValue && (DoubleValue < MinValue))
                    isOk = false;

                if(MaxValue.HasValue && (DoubleValue > MaxValue))
                    isOk = false;
            }
            else
                isOk = false;
            
            return isOk;
        }


    }

    public class ComboBoxInt32 : ComboBox
    {
        [System.ComponentModel.Browsable(true)]
        public int? MinValue { get; set; }

        [System.ComponentModel.Browsable(true)]
        public int? MaxValue { get; set; }

        /// <summary>
        ///  checks if the value is within its borders
        /// </summary>
        /// <returns></returns>
        public Boolean checkValue()
        {
            Boolean isOk = true;
            Int32 IntValue = 0;

            if(Int32.TryParse(this.Text, out IntValue))
            {
                if(MinValue.HasValue && (IntValue < MinValue))
                    isOk = false;

                if(MaxValue.HasValue && (IntValue > MaxValue))
                    isOk = false;
            }
            else
                isOk = false;
            
            return isOk;
        }
    }
}
