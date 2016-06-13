using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.Enums_and_Utility_Classes
{
    public partial class DataGridViewSettings : RNBaseForm
    {
        public DataGridView HandledDGV { get; set; }

        public DataGridViewSettings()
        {
            InitializeComponent();


        }

        public DialogResult setVisibility(DataGridView editedDataGridView)
        {
            try
            {

                HandledDGV = editedDataGridView;

                foreach (DataGridViewColumn CurrentColumn in editedDataGridView.Columns)
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


                this.ShowDialog(editedDataGridView);

                if(this.DialogResult == System.Windows.Forms.DialogResult.OK)
                { 
                    Int32 ColumnIndex=0;

                    foreach (DataGridViewColumn CurrentColumn in editedDataGridView.Columns)
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

        static public void CloneSettings(ref DataGridView EditedDataGridView1, ref DataGridView EditedDataGridView2)
        {
            try
            {
                foreach (DataGridViewColumn CurrentColumn in EditedDataGridView1.Columns)
                { 
                    EditedDataGridView2.Columns[CurrentColumn.Index].DisplayIndex    = CurrentColumn.DisplayIndex;
                    EditedDataGridView2.Columns[CurrentColumn.Index].Visible         = CurrentColumn.Visible;
                    EditedDataGridView2.Columns[CurrentColumn.Index].AutoSizeMode    = CurrentColumn.AutoSizeMode;
                    EditedDataGridView2.Columns[CurrentColumn.Index].Width           = CurrentColumn.Width;
                    EditedDataGridView2.Columns[CurrentColumn.Index].FillWeight      = CurrentColumn.FillWeight;
                    EditedDataGridView2.Columns[CurrentColumn.Index].MinimumWidth    = CurrentColumn.MinimumWidth;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while cloning settings", ex);
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

        private void DataGridViewSettings_Load(object sender, EventArgs e)
        {
            try
            {   
                // set the position relative to the edited grid
                this.Location = HandledDGV.PointToScreen(new Point(60,120));
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in DataGridViewSettings_Load");
            }

        }
    }
}
