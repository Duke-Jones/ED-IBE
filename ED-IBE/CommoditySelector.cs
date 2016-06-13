using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;

namespace IBE
{
    public partial class CommoditySelector : RNBaseForm
    {
        List<Int32> m_selectedCommodities;

        BindingSource m_BindingSource        = new BindingSource();
        SQL.Datasets.dsEliteDB.tbcommodityDataTable m_Table;



        public CommoditySelector()
        {
            InitializeComponent();
        }

        public DialogResult Start(Form parent, ref List<Int32> selectedCommodities)
        { 
            try
            {
                m_selectedCommodities = selectedCommodities;

                this.ShowDialog(parent);

                if(DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    m_selectedCommodities.Clear();

                    foreach (SQL.Datasets.dsEliteDB.tbcommodityRow dRow in m_Table.Select("is_selected = true"))
		                m_selectedCommodities.Add(dRow.id);
                }
                else if(DialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    m_selectedCommodities.Clear();
                }
                
                return this.DialogResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while starting form", ex);
            }
        }

        private void CommoditySelector_Load(object sender, EventArgs e)
        {
            DataColumn col; 

            try
            {
                dgvCommodities.AutoGenerateColumns = false;
                m_Table = (SQL.Datasets.dsEliteDB.tbcommodityDataTable)Program.Data.BaseData.tbcommodity.Copy();

                col = new DataColumn("is_Selected", typeof(Boolean));
                col.DefaultValue = false;
                m_Table.Columns.Add(col);

                foreach (Int32 commodityID in m_selectedCommodities)
                    m_Table.FindByid(commodityID)["is_Selected"] = true;

                m_BindingSource.DataSource = m_Table;
                dgvCommodities.DataSource = m_BindingSource;

                m_BindingSource.Sort = "loccommodity asc";

                if(true || m_selectedCommodities.Count > 0)
                { 
                    cbOnlySelected.Checked = true;
                }

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in CommoditySelector_Load");
            }
        }

        private void cbOnlySelected_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(cbOnlySelected.Checked)
                    m_BindingSource.Filter = "is_selected = true";
                else
                    m_BindingSource.Filter = "";
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cbOnlySelected_CheckedChanged");
            }
        }

        private void txtSearchString_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(!String.IsNullOrEmpty(txtSearchString.Text.Trim()))
                    m_BindingSource.Filter = string.Format("loccommodity like '{0}*'", txtSearchString.Text.Trim());
                else
                    if(cbOnlySelected.Checked)
                        m_BindingSource.Filter = "is_selected = true";
                    else
                        m_BindingSource.Filter = "";


            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in txtSearchString_TextChanged");
            }
        }

        private void dgvCommodities_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                //if((e.RowIndex >= 0) && (e.ColumnIndex == dgvCommodities.Columns["is_Selected"].Index) && (!String.IsNullOrEmpty(txtSearchString.Text.Trim())) && (dgvCommodities.RowCount == 1))
                //{
                //    if((Boolean)(dgvCommodities[e.ColumnIndex, e.RowIndex].Value) == true)
                //    {               
                //        tmrAutoClear.Start();
                //    }
                //}
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in txtSearchString_TextChanged");
            }
        }

        private void dgvCommodities_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvCommodities.IsCurrentCellDirty)
                {
                    dgvCommodities.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in dgvCommodities_CurrentCellDirtyStateChanged");
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchString.Text = "";            
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in dgvCommodities_CurrentCellDirtyStateChanged");
            }
        }

        private void tmrAutoClear_Tick(object sender, EventArgs e)
        {
            try
            {
                //tmrAutoClear.Stop();
                //txtSearchString.Text = "";            
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in tmrAutoClear_Tick");
            }
        }
    }
}
