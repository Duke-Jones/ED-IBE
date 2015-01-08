using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise
{
    public partial class EditPriceData : Form
    {
        public Form1.CsvRow RowToEdit;

        public EditPriceData(Form1.CsvRow csvRow, List<string> commodities)
        {
            InitializeComponent();

            RowToEdit = csvRow;

            tbEditSystem.Text        = RowToEdit.SystemName;
            tbEditStation.Text       = RowToEdit.StationName.Substring(0,RowToEdit.StationName.IndexOf("[")-1);
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

            var returnValue = new Form1.CsvRow
            {
                SystemName = tbEditSystem.Text,
                StationName = tbEditStation.Text + " ["+tbEditSystem.Text+"]",
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
