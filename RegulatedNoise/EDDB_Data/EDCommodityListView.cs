using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.EDDB_Data
{
    public partial class EDCommodityListView : RNBaseForm
    {
        public override string thisObjectName { get { return "EDCommodityListView"; } }

        private List<EDCommoditiesExt> Commodities;
        private Boolean m_DataChanged = false;
        private string m_OldValue;

        public EDCommodityListView(string presetCommodity = "")
        {
            InitializeComponent();
            int selectedRow;

            Commodities = Form1.InstanceObject.myMilkyway.cloneCommodities().OrderBy(x => x.Name).ToList();

            foreach (EDCommoditiesExt Commodity in Commodities)
            {
                dgvWarnlevels.Rows.Add(Commodity.Id, Commodity.Name, Commodity.Category, Commodity.AveragePrice, 
                                       Commodity.PriceWarningLevel_Demand_Sell_Low, Commodity.PriceWarningLevel_Demand_Sell_High, 
                                       Commodity.PriceWarningLevel_Demand_Buy_Low, Commodity.PriceWarningLevel_Demand_Buy_High,
                                       Commodity.PriceWarningLevel_Supply_Sell_Low, Commodity.PriceWarningLevel_Supply_Sell_High, 
                                       Commodity.PriceWarningLevel_Supply_Buy_Low, Commodity.PriceWarningLevel_Supply_Buy_High);
            } 

            dgvWarnlevels.Columns[4].HeaderCell.Style.ForeColor = Color.DarkGreen;
            dgvWarnlevels.Columns[5].HeaderCell.Style.ForeColor = Color.DarkGreen;
            dgvWarnlevels.Columns[6].HeaderCell.Style.ForeColor = Color.DarkGreen;
            dgvWarnlevels.Columns[7].HeaderCell.Style.ForeColor = Color.DarkGreen;

            dgvWarnlevels.Columns[8].HeaderCell.Style.ForeColor = Color.DarkGoldenrod;
            dgvWarnlevels.Columns[9].HeaderCell.Style.ForeColor = Color.DarkGoldenrod;
            dgvWarnlevels.Columns[10].HeaderCell.Style.ForeColor = Color.DarkGoldenrod;
            dgvWarnlevels.Columns[11].HeaderCell.Style.ForeColor = Color.DarkGoldenrod;

            if (!string.IsNullOrEmpty(presetCommodity))
            {
                string BaseName = Form1.InstanceObject.getCommodityBasename(presetCommodity);
                if(string.IsNullOrEmpty(BaseName))
                    BaseName = presetCommodity;

                selectedRow   = Commodities.FindIndex(x => x.Name.Equals(BaseName, StringComparison.InvariantCultureIgnoreCase));

                dgvWarnlevels.CurrentCell = dgvWarnlevels.Rows[selectedRow].Cells[4]; 
            }
            else
            { 
                dgvWarnlevels.CurrentCell = dgvWarnlevels.Rows[0].Cells[4]; 
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            m_OldValue = dgvWarnlevels[e.ColumnIndex, e.RowIndex].Value.ToString();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if ((m_OldValue != null) && (!m_OldValue.Equals(e.FormattedValue))) 
                m_DataChanged = true;

            m_OldValue = null;
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 4 && e.RowIndex >= 0)
                if (int.Parse(e.FormattedValue.ToString()) <= 0)
                    e.CellStyle.BackColor = Color.LightCoral;
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (m_DataChanged)
            {
                if (MessageBox.Show("Save Changed Data ?", "Commodity Data Changed",  MessageBoxButtons.OKCancel,  MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                { 
                    // save and change
                    updateCommodityList();
                    Form1.InstanceObject.myMilkyway.setCommodities(Commodities.OrderBy(x => x.Id).ToList());
                    this.Close();
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
            }
            else
                this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            if (m_DataChanged)
            {
                if (MessageBox.Show("Dismiss Changed Data ?", "Commodity Data Changed",  MessageBoxButtons.OKCancel,  MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                { 
                    this.Close();
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
            }
            else
                this.Close();
        }

        private void updateCommodityList()
        {
            int Counter=0;
            try
            {
                List<EDCommoditiesExt> retValue = new List<EDCommoditiesExt>();
                EDCommoditiesExt currentCommodity;

                foreach (DataGridViewRow Commodity in dgvWarnlevels.Rows)
                {
                    if (Counter == 83) 
                        Debug.Print(Counter.ToString());   

                    Debug.Print(Counter.ToString());
                    currentCommodity = Commodities.Find(x => x.Id == int.Parse(Commodity.Cells["Id"].Value.ToString()));

                    currentCommodity.PriceWarningLevel_Demand_Sell_Low = int.Parse(Commodity.Cells[4].Value.ToString());
                    currentCommodity.PriceWarningLevel_Demand_Sell_High = int.Parse(Commodity.Cells[5].Value.ToString());
                    currentCommodity.PriceWarningLevel_Demand_Buy_Low = int.Parse(Commodity.Cells[6].Value.ToString());
                    currentCommodity.PriceWarningLevel_Demand_Buy_High = int.Parse(Commodity.Cells[7].Value.ToString());
                    currentCommodity.PriceWarningLevel_Supply_Sell_Low = int.Parse(Commodity.Cells[8].Value.ToString());
                    currentCommodity.PriceWarningLevel_Supply_Sell_High = int.Parse(Commodity.Cells[9].Value.ToString());
                    currentCommodity.PriceWarningLevel_Supply_Buy_Low = int.Parse(Commodity.Cells[10].Value.ToString());
                    currentCommodity.PriceWarningLevel_Supply_Buy_High = int.Parse(Commodity.Cells[11].Value.ToString());

                    Counter++;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("STOP : " + ex.Message)   ;
            }
        }
    }
}
