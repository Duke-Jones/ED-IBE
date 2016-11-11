using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using IBE.Enums_and_Utility_Classes;

namespace DataGridViewAutoFilter
{
    public partial class FullTextHeader : UserControl
    {
        public FullTextHeader()
        {
            InitializeComponent();

            if(!((Control)this).IsDesignMode())
                ((Control)this).Retheme();

            cmbConstraint.SelectedIndex = 0;
            txtFilterText.Text          = "";
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            cmbConstraint.SelectedIndex = 0;
            txtFilterText.Text = "";
            this.cmdOk.PerformClick();
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.cmdOk.PerformClick();
            }
            
        }
    }
}
