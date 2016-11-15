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
    public partial class DateTimeHeader : Form
    {
        private Boolean m_changingDate  = false;
        private Boolean m_changingTime  = false;
        private Boolean m_isShown       = false;

        public DateTimeHeader()
        {
            InitializeComponent();

            if(!((Control)this).IsDesignMode())
                ((Control)this).Retheme();
            
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            dtpAfter.Checked = false;
            dtpBefore.Checked = false;
            this.cmdOk.PerformClick();
        }

        private void dtpAfter_ValueChanged(object sender, EventArgs e)
        {
            if(m_isShown && dtpAfter.Focused)
            {
                dtpAfter.Value      = dtpAfter.Value.Date;
                dtpAfterTime.Value  = dtpAfter.Value;
            }
            
        }

        private void dtpBefore_ValueChanged(object sender, EventArgs e)
        {
            if(m_isShown && dtpBefore.Focused)
            {
                dtpBefore.Value     = dtpBefore.Value.Date + new TimeSpan(23,59,59);
                dtpBeforeTime.Value = dtpBefore.Value;
            }
        }

        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            if(m_isShown && ((dtpBeforeTime.Focused) || (dtpAfterTime.Focused)))
            {
                dtpAfter.Value  = dtpAfterTime.Value;
                dtpBefore.Value = dtpBeforeTime.Value;
            }
        }

        private void DateTimeHeader_Shown(object sender, EventArgs e)
        {
            
        }

        internal void Show(DataGridView parent, DateTimePicker m_dtpBefore, DateTimePicker m_dtpAfter)
        {
                this.Show(parent);

                this.dtpBefore.Value        = m_dtpBefore.Value;
                this.dtpBeforeTime.Value    = m_dtpBefore.Value;

                this.dtpAfter.Value         = m_dtpAfter.Value;
                this.dtpAfterTime.Value     = m_dtpAfter.Value;

                this.dtpAfter.Checked       = m_dtpAfter.Checked;
                this.dtpBefore.Checked      = m_dtpBefore.Checked;

                m_isShown = true;

        }
    }
}
