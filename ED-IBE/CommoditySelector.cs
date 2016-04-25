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

        DataTable data              = new DataTable();
        BindingSource bs            = new BindingSource();

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

                return this.DialogResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while starting form", ex);
            }
        }

        private void CommoditySelector_Load(object sender, EventArgs e)
        {
            try
            {
                
                dgvCommodities.AutoGenerateColumns = false;
                bs.DataSource = Program.Data.BaseData.tbcommodity;
                dgvCommodities.DataSource = bs;

                dgvCommodities.CellFormatting   += dgvCommodities_CellFormatting;
                dgvCommodities.CellValueChanged += dgvCommodities_CellValueChanged;

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in CommoditySelector_Load");
            }
        }

        private void dgvCommodities_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.ColumnIndex == dgvCommodities.Columns["is_Selected"].Index)
                {
                    Int32 commodityID = (Int32)dgvCommodities.Rows[e.RowIndex].Cells[dgvCommodities.Columns["idDataGridViewTextBoxColumn"].Index].Value;

                    if((Boolean)dgvCommodities.Rows[e.RowIndex].Cells[dgvCommodities.Columns["is_Selected"].Index].Value)
                    {
                        if(!m_selectedCommodities.Contains(commodityID))
                            m_selectedCommodities.Add(commodityID);
                    }
                    else
                    { 
                        if(m_selectedCommodities.Contains(commodityID))
                            m_selectedCommodities.Remove(commodityID);
                    }
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommodities_CellValueChanged");
            }
        }

        private void dgvCommodities_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if(e.ColumnIndex == dgvCommodities.Columns["is_Selected"].Index)
                {
                    Int32 commodityID = (Int32)dgvCommodities.Rows[e.RowIndex].Cells[dgvCommodities.Columns["idDataGridViewTextBoxColumn"].Index].Value;

                    dgvCommodities.Rows[e.RowIndex].Cells[dgvCommodities.Columns["is_Selected"].Index].Value = m_selectedCommodities.Contains(commodityID);
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommodities_CellFormatting");
            }
        }

        private void dgvCommodities_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvCommodities.Columns["is_Selected"].Index)
                {
                    dgvCommodities.Rows[e.RowIndex].Cells["is_Selected"].Value = (!(Boolean)dgvCommodities.Rows[e.RowIndex].Cells["is_Selected"].Value);
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommodities_CellContentClick");
            }
        }
    }
}
