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
using System.Diagnostics;
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    public partial class EditOcrResults : RNBaseForm
    {
        public override string thisObjectName { get { return "EditOcrResults"; } }

        public string ReturnValue;
        private int lastRow;
        private int currentRow;

        public EditOcrResults(string dataToEdit)
        {
            InitializeComponent();

            foreach (DataGridViewColumn Column in dgvData.Columns)
            {
                Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dgvData.Rows.Clear();

            var rows = dataToEdit.Split(new string[] {"\r\n"}, StringSplitOptions.None);

            foreach (var row in rows)
            {
               bool implausible = Form1.InstanceObject.checkPricePlausibility(new string[] {row});

                string[] Splitted = row.Split(';');

                if (Splitted.GetUpperBound(0) == 11)
                    dgvData.Rows.Add(Splitted[0], Splitted[1], Splitted[2], Splitted[3], Splitted[4], 
                                     Splitted[5], Splitted[6], Splitted[7], Splitted[8], Splitted[9],
                                     Splitted[10], Splitted[11], implausible.ToString());

                setRowStyle(dgvData.Rows[dgvData.RowCount - 1], implausible);                
            }
        }

        private void setRowStyle(DataGridViewRow DGVRow, bool implausible)
        {
            try
            {
                Debug.Print("setrowstyle");
                if (implausible)
                {
                    DGVRow.DefaultCellStyle.BackColor = Color.LightCoral;
                    DGVRow.DefaultCellStyle.SelectionBackColor = Color.LightCoral;
                    DGVRow.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                    DGVRow.Visible = true;

                }
                else
                {
                    DGVRow.DefaultCellStyle.BackColor = SystemColors.Window;
                    DGVRow.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    DGVRow.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                    DGVRow.Visible = (!cbOnlyImplausible.Checked);
                }
            }
            catch (Exception ex)
            {
                Debug.Print("STOP : " + ex.Message)   ;
            }
        }

        private bool suspendTextChanged = false;

        private void dgvData_CurrentCellChanged(object sender, EventArgs e)
        {
            StringBuilder SBuilder = new StringBuilder();
            suspendTextChanged = true;

            lastRow     = currentRow;
            if (dgvData.CurrentRow != null)
                currentRow = dgvData.CurrentRow.Index;
            else
                currentRow = -1;

            if (lastRow >= 0)
            { 
                for (int i = 0; i < 12; i++)
                {
                    if (i > 0)
                        SBuilder.Append(";");
                    
                    SBuilder.Append(dgvData.Rows[lastRow].Cells[i].Value.ToString());
                }

                bool implausible = Form1.InstanceObject.checkPricePlausibility(new string[] {SBuilder.ToString()});
                dgvData.Rows[lastRow].Cells[12].Value = implausible.ToString();

                setRowStyle(dgvData.Rows[lastRow], implausible);
            }

            if (dgvData.CurrentRow != null)
            { 
                string rowId = dgvData.CurrentRow.Cells[11].Value.ToString();

                if (pbEditOcrResultsOriginalImage.Image != null)
                    pbEditOcrResultsOriginalImage.Image.Dispose();

                if (File.Exists(".//OCR Correction Images//" + rowId + ".png"))
                    pbEditOcrResultsOriginalImage.Image = Bitmap.FromFile(".//OCR Correction Images//" + rowId + ".png");

                tbEditOcrResultsCommodityName.Text = dgvData.CurrentRow.Cells[2].Value.ToString();
                tbEditOcrResultsSellPrice.Text = dgvData.CurrentRow.Cells[3].Value.ToString();
                tbEditOcrResultsBuyPrice.Text = dgvData.CurrentRow.Cells[4].Value.ToString();
                tbEditOcrResultsDemand.Text = dgvData.CurrentRow.Cells[5].Value.ToString();
                tbEditOcrResultsDemandLevel.Text = dgvData.CurrentRow.Cells[6].Value.ToString();
                tbEditOcrResultsSupply.Text = dgvData.CurrentRow.Cells[7].Value.ToString();
                tbEditOcrResultsSupplyLevel.Text = dgvData.CurrentRow.Cells[8].Value.ToString();
            }

            suspendTextChanged = false;

        }

        private void tbEditOcrResultTextChanged(object sender, EventArgs e)
        {
            if (suspendTextChanged) return;

            dgvData.CurrentCellChanged -= dgvData_CurrentCellChanged;

            dgvData.CurrentRow.Cells[2].Value =  tbEditOcrResultsCommodityName.Text;
            dgvData.CurrentRow.Cells[3].Value =  tbEditOcrResultsSellPrice.Text;   
            dgvData.CurrentRow.Cells[4].Value =  tbEditOcrResultsBuyPrice.Text;       
            dgvData.CurrentRow.Cells[5].Value =  tbEditOcrResultsDemand.Text;         
            dgvData.CurrentRow.Cells[6].Value =  tbEditOcrResultsDemandLevel.Text;    
            dgvData.CurrentRow.Cells[7].Value =  tbEditOcrResultsSupply.Text;         
            dgvData.CurrentRow.Cells[8].Value =  tbEditOcrResultsSupplyLevel.Text;    

            dgvData.CurrentCellChanged += dgvData_CurrentCellChanged;
        }

        private void bEditOcrResultsOK_Click(object sender, EventArgs e)
        {
            StringBuilder SBuilder = new StringBuilder();

            foreach (DataGridViewRow currentRow in dgvData.Rows)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i > 0)
                        SBuilder.Append(";");
                    SBuilder.Append(currentRow.Cells[i].Value.ToString());

                    
                }
                SBuilder.Append("\r\n");
            }

            ReturnValue = SBuilder.ToString();
            DialogResult= DialogResult.OK;
            Close();
        }


        public bool onlyImplausible { 
            get
            {
                return cbOnlyImplausible.Checked;

            }

            set
            {
                cbOnlyImplausible.Checked = value;
            }
        }

        private void cbOnlyImplausible_CheckedChanged(object sender, EventArgs e)
        {
            int FirstVisible = -1;

            dgvData.CurrentCellChanged -= dgvData_CurrentCellChanged;

            foreach (DataGridViewRow currentRow in dgvData.Rows)
            {
                bool implausible = (((string)(currentRow.Cells[12].Value)) == (string)(true.ToString()));

                if (cbOnlyImplausible.Checked)
                { 
                    currentRow.Visible = implausible;

                    if (FirstVisible < 0 && currentRow.Visible)
                        FirstVisible = currentRow.Index;
                }
                else
                    currentRow.Visible = true;

                setRowStyle(currentRow, implausible);
            }

            dgvData.CurrentCellChanged += dgvData_CurrentCellChanged;

            if (dgvData.CurrentRow == null && FirstVisible >= 0)
                dgvData.CurrentCell = dgvData[0,FirstVisible];

        }

        private void cmdWarnLevels_Click(object sender, EventArgs e)
        {
            string Commodity = String.Empty;

            if (dgvData.CurrentRow != null)
                Commodity = dgvData.CurrentRow.Cells[2].Value.ToString();

            EDCommodityView CView = new EDCommodityView(Commodity);

            CView.ShowDialog(this);

            if (CView.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                checkWarnLevels();
                cbOnlyImplausible_CheckedChanged(this, null);
                dgvData.Refresh();
            }

        }

        private void checkWarnLevels()
        {
            StringBuilder SBuilder = new StringBuilder();

            foreach (DataGridViewRow currentRow in dgvData.Rows)
            {
                SBuilder.Clear();

                for (int i = 0; i < 12; i++)
                { 
                    if (i > 0)
                        SBuilder.Append(";");
                    SBuilder.Append(currentRow.Cells[i].Value.ToString());
                }

                currentRow.Cells[12].Value = Form1.InstanceObject.checkPricePlausibility(new string[] {SBuilder.ToString()}).ToString();
            }

        }

    }
}
