using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.EDDB_Data
{
	public partial class EDCommodityView : RNBaseForm
	{
		public override string thisObjectName { get { return "EDCommodityView"; } }

		private readonly List<EDCommoditiesExt> m_commodities;
		private Boolean m_DataChanged = false;
		private string m_OldValue;
		private Boolean m_NoRefresh = false;

		public EDCommodityView(string presetCommodity = "")
		{
			InitializeComponent();
			cmdCommodity.Sorted = false;

			m_commodities = Form1.InstanceObject.myMilkyway.cloneCommodities().OrderBy(x => x.Name).ToList();

			cmdCommodity.DataSource = m_commodities;
			cmdCommodity.ValueMember = "ID";
			cmdCommodity.DisplayMember = "Name";

			if (!string.IsNullOrEmpty(presetCommodity))
			{
				string baseName = Form1.InstanceObject.getCommodityBasename(presetCommodity);

				if (string.IsNullOrEmpty(baseName))
					baseName = presetCommodity;

				cmdCommodity.SelectedIndex = m_commodities.FindIndex(x => x.Name.Equals(baseName, StringComparison.InvariantCultureIgnoreCase));
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

			EDCommoditiesExt currentCommodity = CurrentCommodity;
			if (currentCommodity == null)
				return;
			
			txtId.Text = currentCommodity.Id.ToString();
			txtCategory.Text = currentCommodity.Category.ToString();
			txtAveragePrice.Text = currentCommodity.AveragePrice.ToString();
			txtDemandSellLow.Text = currentCommodity.PriceWarningLevel_Demand_Sell_Low.ToString();
			txtDemandSellHigh.Text = currentCommodity.PriceWarningLevel_Demand_Sell_High.ToString();
			txtDemandBuyLow.Text = currentCommodity.PriceWarningLevel_Demand_Buy_Low.ToString();
			txtDemandBuyHigh.Text = currentCommodity.PriceWarningLevel_Demand_Buy_High.ToString();

			txtSupplySellLow.Text = currentCommodity.PriceWarningLevel_Supply_Sell_Low.ToString();
			txtSupplySellHigh.Text = currentCommodity.PriceWarningLevel_Supply_Sell_High.ToString();
			txtSupplyBuyLow.Text = currentCommodity.PriceWarningLevel_Supply_Buy_Low.ToString();
			txtSupplyBuyHigh.Text = currentCommodity.PriceWarningLevel_Supply_Buy_High.ToString();

			m_NoRefresh = false;
		}

		private EDCommoditiesExt CurrentCommodity
		{
			get
			{
				var edCommoditiesExt = ((EDCommoditiesExt)cmdCommodity.SelectedValue);
				if (edCommoditiesExt != null)
				{
					return m_commodities.Find(x => x.Id == edCommoditiesExt.Id);
				}
				else
				{
					Trace.TraceWarning("no commodity selected");
					return null;
				}
			}
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

					EDCommoditiesExt currentCommodity = CurrentCommodity;
					if (currentCommodity == null)
						return;

					currentCommodity.PriceWarningLevel_Demand_Sell_Low = int.Parse(txtDemandSellLow.Text);
					currentCommodity.PriceWarningLevel_Demand_Sell_High = int.Parse(txtDemandSellHigh.Text);
					currentCommodity.PriceWarningLevel_Demand_Buy_Low = int.Parse(txtDemandBuyLow.Text);
					currentCommodity.PriceWarningLevel_Demand_Buy_High = int.Parse(txtDemandBuyHigh.Text);

					currentCommodity.PriceWarningLevel_Supply_Sell_Low = int.Parse(txtSupplySellLow.Text);
					currentCommodity.PriceWarningLevel_Supply_Sell_High = int.Parse(txtSupplySellHigh.Text);
					currentCommodity.PriceWarningLevel_Supply_Buy_Low = int.Parse(txtSupplyBuyLow.Text);
					currentCommodity.PriceWarningLevel_Supply_Buy_High = int.Parse(txtSupplyBuyHigh.Text);
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
				if (MessageBox.Show("Save Changed Data ?", "Commodity Data Changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					// save and change
					Form1.InstanceObject.myMilkyway.setCommodities(m_commodities.OrderBy(x => x.Id).ToList());
					Form1.InstanceObject.myMilkyway.saveRNCommodityData(@"./Data/commodities_RN.json", true);
					this.Close();
				}
				else
				{
					this.DialogResult = DialogResult.None;
				}
			}
			else
				this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			if (m_DataChanged)
			{
				if (MessageBox.Show("Dismiss Changed Data ?", "Commodity Data Changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					this.Close();
				}
				else
				{
					this.DialogResult = DialogResult.None;
				}
			}
			else
				this.Close();
		}

		private void cmdFullList_Click(object sender, EventArgs e)
		{
			EDCommodityListView view = new EDCommodityListView(cmdCommodity.Text);
			Visible = false;
			view.ShowDialog(this);
			Close();
		}

	}
}
