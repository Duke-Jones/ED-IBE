using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    public partial class EditPriceData : RNBaseForm
    {
        public override string thisObjectName { get { return "EditPriceData"; } }

        public CsvRow RowToEdit;

        public EditPriceData(CsvRow csvRow, List<string> commodities)
        {
            InitializeComponent();

            RowToEdit = csvRow;

            tbEditSystem.Text        = RowToEdit.SystemName;
            tbEditStation.Text       = RowToEdit.StationID;
            cbEditCommodityName.Text = RowToEdit.CommodityName;
            nEditSell.Value          = RowToEdit.SellPrice;
            nEditBuy.Value           = RowToEdit.BuyPrice;
            nEditDemand.Value        = RowToEdit.Demand;
            nEditSupply.Value        = RowToEdit.Supply;
            tbEditDemandLevel.Text   = RowToEdit.DemandLevel;
            tbEditSupplyLevel.Text   = RowToEdit.SupplyLevel;
            dtpEditSampleDate.Value  = RowToEdit.SampleDate;
            tbEditFilename.Text      = RowToEdit.SourceFileName;

            foreach (var x in commodities.OrderBy(y => y))
                cbEditCommodityName.Items.Add(x);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            var returnValue = new CsvRow
            {
                SystemName = tbEditSystem.Text,
                StationID = tbEditStation.Text + " ["+tbEditSystem.Text+"]",
                CommodityName = cbEditCommodityName.Text,
                SellPrice = nEditSell.Value,
                BuyPrice = nEditBuy.Value,
                Demand = nEditDemand.Value,
                Supply = nEditSupply.Value,
                DemandLevel = tbEditDemandLevel.Text,
                SupplyLevel = tbEditSupplyLevel.Text,
                SampleDate = dtpEditSampleDate.Value,
                SourceFileName = tbEditFilename.Text
            };

            RowToEdit = returnValue;

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
