using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    public partial class DataGridViewSettings : Form
    {
        public DataGridViewSettings()
        {
            InitializeComponent();
        }

        public DialogResult setVisibility(DataGridView EditedDataGridView)
        {
            try
            {

                

                System.Drawing.Rectangle CellRectangle1 = EditedDataGridView.GetCellDisplayRectangle(2, 2, true);


                this.Location = EditedDataGridView.PointToClient(new Point(EditedDataGridView.Left +  2 * EditedDataGridView.RowHeadersWidth, EditedDataGridView.Top + 2 * EditedDataGridView.ColumnHeadersHeight));

                foreach (DataGridViewColumn CurrentColumn in EditedDataGridView.Columns)
                { 
                    dgvColumns.Rows.Add(CurrentColumn.Name, 
                                        CurrentColumn.HeaderText, 
                                        CurrentColumn.DisplayIndex.ToString(),
                                        CurrentColumn.Visible,
                                        CurrentColumn.AutoSizeMode.ToString(),
                                        CurrentColumn.Width.ToString(),
                                        CurrentColumn.FillWeight.ToString(), 
                                        CurrentColumn.MinimumWidth.ToString());
                }


                this.ShowDialog(EditedDataGridView);

                if(this.DialogResult == System.Windows.Forms.DialogResult.OK)
                { 
                    Int32 ColumnIndex=0;

                    foreach (DataGridViewColumn CurrentColumn in EditedDataGridView.Columns)
                    {
                        CurrentColumn.DisplayIndex   = Int32.Parse(dgvColumns.Rows[ColumnIndex].Cells["colDisplayIndex"].Value.ToString());
                        CurrentColumn.Visible        = (Boolean)dgvColumns.Rows[ColumnIndex].Cells["colVisible"].Value;
                        CurrentColumn.AutoSizeMode   = (DataGridViewAutoSizeColumnMode)Enum.Parse(typeof(DataGridViewAutoSizeColumnMode), (String)dgvColumns.Rows[ColumnIndex].Cells["colAutoSizeMode"].Value);

                        if(CurrentColumn.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill)
                            CurrentColumn.FillWeight     = Single.Parse(dgvColumns.Rows[ColumnIndex].Cells["colFillWeight"].Value.ToString().Replace(",", "."),System.Globalization.NumberStyles.AllowDecimalPoint,System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        else
                            CurrentColumn.Width          = Int32.Parse(dgvColumns.Rows[ColumnIndex].Cells["colWidth"].Value.ToString());

                        CurrentColumn.MinimumWidth   = Int32.Parse(dgvColumns.Rows[ColumnIndex].Cells["colMinimumWidth"].Value.ToString());

                        ColumnIndex++;
                        
                    }
                }

                return DialogResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while editing column visibility", ex);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in cmdCancel_Click", ex);
            }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in cmdOk_Click", ex);
            }
        }
    }
}
