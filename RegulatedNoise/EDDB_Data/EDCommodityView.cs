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
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.EDDB_Data
{
    public partial class EDCommodityView : RNBaseForm
    {
        public override string thisObjectName { get { return "EDCommodityView"; } }

        private List<EDCommoditiesExt> Commodities;
        private Boolean m_DataChanged = false;
        private string m_OldValue;
        private Boolean m_NoRefresh = false;

        public EDCommodityView(string presetCommodity = "")
        {
            InitializeComponent();
            cmdCommodity.Sorted = false;

            Commodities = Form1.InstanceObject.myMilkyway.cloneCommodities().OrderBy(x => x.Name).ToList();

            cmdCommodity.DataSource     = Commodities;
            cmdCommodity.ValueMember    = "ID";
            cmdCommodity.DisplayMember  = "Name";

            if (!string.IsNullOrEmpty(presetCommodity))
            {
                string BaseName = Form1.InstanceObject.getCommodityBasename(presetCommodity);

                if(string.IsNullOrEmpty(BaseName))
                    BaseName = presetCommodity;

                cmdCommodity.SelectedIndex = Commodities.FindIndex(x => x.Name.Equals(BaseName, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            { 
                cmdCommodity.SelectedIndex = 0;
                cmdCommodity_SelectedIndexChanged(this, new EventArgs());
            }

        }

        private void cmdCommodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            m_NoRefresh = true;

            EDCommoditiesExt currentCommodity = Commodities.Find(x => x.Id == (int)(cmdCommodity.SelectedValue));

            txtId.Text                      = currentCommodity.Id.ToString();
            txtCategory.Text                = currentCommodity.Category.ToString();
            txtAveragePrice.Text            = currentCommodity.AveragePrice.ToString();
            txtDemandSellLow.Text           = currentCommodity.PriceWarningLevel_Demand_Sell_Low.ToString();
            txtDemandSellHigh.Text          = currentCommodity.PriceWarningLevel_Demand_Sell_High.ToString();
            txtDemandBuyLow.Text            = currentCommodity.PriceWarningLevel_Demand_Buy_Low.ToString();
            txtDemandBuyHigh.Text           = currentCommodity.PriceWarningLevel_Demand_Buy_High.ToString();

            txtSupplySellLow.Text           = currentCommodity.PriceWarningLevel_Supply_Sell_Low.ToString();
            txtSupplySellHigh.Text          = currentCommodity.PriceWarningLevel_Supply_Sell_High.ToString();
            txtSupplyBuyLow.Text            = currentCommodity.PriceWarningLevel_Supply_Buy_Low.ToString();
            txtSupplyBuyHigh.Text           = currentCommodity.PriceWarningLevel_Supply_Buy_High.ToString();

            m_NoRefresh = false;
        }

        private void txtField_TextChanged(object sender, EventArgs e)
        {
            if (!m_NoRefresh)
            { 
                int IntValue;
                TextBox currentTextBox = ((TextBox)sender);
            
                if (int.TryParse(currentTextBox.Text, out IntValue))
                { 
                    m_DataChanged = true;

                    EDCommoditiesExt currentCommodity = Commodities.Find(x => x.Id == (int)(cmdCommodity.SelectedValue));

                    currentCommodity.PriceWarningLevel_Demand_Sell_Low   = int.Parse(txtDemandSellLow.Text);
                    currentCommodity.PriceWarningLevel_Demand_Sell_High  = int.Parse(txtDemandSellHigh.Text);
                    currentCommodity.PriceWarningLevel_Demand_Buy_Low    = int.Parse(txtDemandBuyLow.Text);
                    currentCommodity.PriceWarningLevel_Demand_Buy_High   = int.Parse(txtDemandBuyHigh.Text);

                    currentCommodity.PriceWarningLevel_Supply_Sell_Low   = int.Parse(txtSupplySellLow.Text);
                    currentCommodity.PriceWarningLevel_Supply_Sell_High  = int.Parse(txtSupplySellHigh.Text);
                    currentCommodity.PriceWarningLevel_Supply_Buy_Low    = int.Parse(txtSupplyBuyLow.Text);
                    currentCommodity.PriceWarningLevel_Supply_Buy_High   = int.Parse(txtSupplyBuyHigh.Text);
                }
                else
                {
                    currentTextBox.Text = m_OldValue;
                }
            }
        }

        private void txtField_GotFocus(object sender, EventArgs e)
        {
            m_OldValue = ((TextBox)sender).Text;
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (m_DataChanged)
            {
                if (MessageBox.Show("Save Changed Data ?", "Commodity Data Changed",  MessageBoxButtons.OKCancel,  MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                { 
                    // save and change
                    Form1.InstanceObject.myMilkyway.setCommodities(Commodities.OrderBy(x => x.Id).ToList());
                    Form1.InstanceObject.myMilkyway.saveRNCommodityData(@"./Data/commodities_RN.json", true);
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

        private void cmdFullList_Click(object sender, EventArgs e)
        {
            string Commodity = String.Empty;

            EDCommodityListView CView = new EDCommodityListView(cmdCommodity.Text);

            this.Visible = false;

            CView.ShowDialog(this);

            this.Close();

        }

    }
}
