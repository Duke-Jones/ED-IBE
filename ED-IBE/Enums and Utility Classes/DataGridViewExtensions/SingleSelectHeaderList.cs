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
    public partial class SingleSelectHeaderList : Form
    {
        public SingleSelectHeaderList()
        {
            InitializeComponent();
            if (!((Control)this).IsDesignMode())
                ((Control)this).Retheme();
        }

        /// <summary>
        /// Indicates that the FilterListBox will handle (or ignore) all 
        /// keystrokes that are not handled by the operating system. 
        /// </summary>
        /// <param name="keyData">A Keys value that represents the keyboard input.</param>
        /// <returns>true in all cases.</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }

        /// <summary>
        /// Processes a keyboard message directly, preventing it from being
        /// intercepted by the parent DataGridView control.
        /// </summary>
        /// <param name="m">A Message, passed by reference, that 
        /// represents the window message to process.</param>
        /// <returns>true if the message was processed by the control;
        /// otherwise, false.</returns>
        protected override bool ProcessKeyMessage(ref Message m)
        {
            return ProcessKeyEventArgs(ref m);
        }

        private void SingleSelectHeaderList_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    this.Close();
                    e.Handled = true;
                    break;
            }
        }
    }
}
