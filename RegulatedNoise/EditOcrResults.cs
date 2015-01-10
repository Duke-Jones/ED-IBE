using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;

namespace RegulatedNoise
{
    public partial class EditOcrResults : Form
    {
        private string _startOfRow, _endOfRow;
        public string ReturnValue;

        public EditOcrResults(string dataToEdit)
        {
            InitializeComponent();
            lbEditOcrResults.Items.Clear();

            var rows = dataToEdit.Split(new string[] {"\r\n"}, StringSplitOptions.None);

            foreach (var row in rows)
            {
                if(row.Contains(';'))
                    lbEditOcrResults.Items.Add(row);
            }
        }

        private bool suspendTextChanged = false;

        private void lbEditOcrResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            suspendTextChanged = true;
            var row = lbEditOcrResults.Text;
            var rowId = row.Substring(row.LastIndexOf(";", System.StringComparison.Ordinal)+1);

            if (pbEditOcrResultsOriginalImage.Image != null)
                pbEditOcrResultsOriginalImage.Image.Dispose();

            if (File.Exists(".//OCR Correction Images//" + rowId + ".png"))
                pbEditOcrResultsOriginalImage.Image = Bitmap.FromFile(".//OCR Correction Images//" + rowId + ".png");

            var results = row.Split(';');
            if (results.GetLength(0) > 7)
            {
                tbEditOcrResultsCommodityName.Text = results[2];
                tbEditOcrResultsSellPrice.Text = results[3];
                tbEditOcrResultsBuyPrice.Text = results[4];
                tbEditOcrResultsDemand.Text = results[5];
                tbEditOcrResultsDemandLevel.Text = results[6];
                tbEditOcrResultsSupply.Text = results[7];
                tbEditOcrResultsSupplyLevel.Text = results[8];
                _startOfRow = results[0] + ";" + results[1] + ";";
                _endOfRow = results[9]+";"+results[10]+";"+results[11];
            }
            suspendTextChanged = false;

        }

        private void tbEditOcrResultTextChanged(object sender, EventArgs e)
        {
            if (suspendTextChanged) return;

            lbEditOcrResults.SelectedIndexChanged -= lbEditOcrResults_SelectedIndexChanged;
            lbEditOcrResults.Items[lbEditOcrResults.SelectedIndex] = _startOfRow +
                tbEditOcrResultsCommodityName.Text + ";" +
                tbEditOcrResultsSellPrice.Text + ";" +
                tbEditOcrResultsBuyPrice.Text + ";" +
                tbEditOcrResultsDemand.Text + ";" +
                tbEditOcrResultsDemandLevel.Text + ";" +
                tbEditOcrResultsSupply.Text + ";" +
                tbEditOcrResultsSupplyLevel.Text + ";" +
                _endOfRow
                ;

            lbEditOcrResults.SelectedIndexChanged += lbEditOcrResults_SelectedIndexChanged;
        }

        private void bEditOcrResultsOK_Click(object sender, EventArgs e)
        {
            foreach (var x in lbEditOcrResults.Items)
            {
                ReturnValue += x + "\r\n";
            }
            DialogResult= DialogResult.OK;
            Close();
        }
    }
}
