using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IBE
{
    public partial class CommodityMappingsView : IBE.Enums_and_Utility_Classes.RNBaseForm
    {

        BindingSource m_BindingSource        = new BindingSource();
        SQL.Datasets.dsEliteDB.tbcommoditymappingDataTable m_Table;

        public CommodityMappingsView()
        {
            InitializeComponent();
        }

        private void CommodityMappingsView_Load(object sender, EventArgs e)
        {
            try
            {
                
                dgvMappings.AutoGenerateColumns = false;
                m_Table = (SQL.Datasets.dsEliteDB.tbcommoditymappingDataTable)Program.Data.BaseData.tbcommoditymapping.Copy();

                m_BindingSource.DataSource = m_Table;
                dgvMappings.DataSource = m_BindingSource;

                m_BindingSource.Sort = "Name asc";

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in CommodityMappingsView_Load");
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_Table.GetChanges() != null)
                {
                    var mbResult = MessageBox.Show(this, "Save changes ?", "Changed mappings...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    if (mbResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        Program.Data.BaseData.tbcommoditymapping.Merge(m_Table);
                        Program.Data.PrepareBaseTables(m_Table.TableName, true);
                    }
                    else if (mbResult == System.Windows.Forms.DialogResult.Cancel)
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdOK_Click");
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_Table.GetChanges() != null)
                {
                    var mbResult = MessageBox.Show(this, "Reject changes ?", "Changed mappings...", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    if (mbResult == System.Windows.Forms.DialogResult.Cancel)
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                    
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdCancel_Click");
            }
        }
    }
}
