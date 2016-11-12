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
    public partial class MultiSelectHeaderList : Form
    {
        public MultiSelectHeaderList()
        {
            InitializeComponent();
            if (!((Control)this).IsDesignMode())
                ((Control)this).Retheme();
        }

        private void cmdInvert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < FilterListBox.Items.Count; i++)
            {
                FilterListBox.SetItemChecked(i, !FilterListBox.GetItemChecked(i));
            }
        }

        public List<string> SelectedValues
        {
            get
            {
                List<String> retValue = new List<string>();

                for (int i = 0; i < FilterListBox.Items.Count; i++)
                {
                    if(FilterListBox.GetItemChecked(i))
                        retValue.Add(FilterListBox.Items[i].ToString());
                }

                return retValue;
            }
            set
            {
                for (int i = 0; i < FilterListBox.Items.Count; i++)
                {
                    FilterListBox.SetItemChecked(i, value.Contains(FilterListBox.Items[i].ToString()));
                }
            }
        }

        private void cmdAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < FilterListBox.Items.Count; i++)
            {
                FilterListBox.SetItemChecked(i, true);
            }
        }

    }
}
