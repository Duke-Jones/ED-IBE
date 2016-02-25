using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;  
using System.Windows.Forms;
using IBE.SQL;
using System.Diagnostics;
using System.Reflection;
using IBE.Enums_and_Utility_Classes;
using MySql.Data.MySqlClient;

namespace IBE
{
    public partial class LanguageEdit : IBE.Enums_and_Utility_Classes.RNBaseForm
    {
        public const String                             DB_GROUPNAME                    = "LanguageEdit";
        
        private DataSet                                 m_MainDataset;
        private Dictionary<Object, DataTable>           m_DGVTables;                 
        private Dictionary<Object, BindingSource>       m_BindingSources;
        private Dictionary<Object, MySqlDataAdapter>    m_DataAdapter;
        private DBGuiInterface                          m_GUIInterface;
        private Boolean                                 m_DataChanged = false;        
        private Dictionary<Int32, Int32>                m_ChangedIDs;

        public LanguageEdit()
        {
            InitializeComponent();
        }

        private void LanguageEdit_Load(object sender, EventArgs e)
        {
            try
            {
                Program.Data.AddMissingLocalizationEntries();
                Init();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while loading form");
            }
        }

        /// <summary>
        /// initialization of the whole log
        /// </summary>
        public void Init()
        {
            Cursor oldCursor = Cursor;

            try
            {
                m_ChangedIDs = new Dictionary<Int32, Int32>();

                clbLanguageFilter.Items.Clear();
                foreach (SQL.Datasets.dsEliteDB.tblanguageRow langRow in Program.Data.BaseData.tblanguage.Rows)
                {
                    clbLanguageFilter.Items.Add(langRow.language, Program.DBCon.getIniValue<Boolean>(DB_GROUPNAME, langRow.language, true.ToString(), false));
                } 
                clbLanguageFilter.ItemCheck += clbLanguageFilter_ItemCheck;
    

                Cursor = Cursors.WaitCursor;

                // preparing datatables 
                m_DGVTables     = new Dictionary<object,DataTable>();
                m_DGVTables.Add(dgvData,                 new DataTable(dgvData.Name)) ;
                m_DGVTables.Add(dgvDataOwn,              new DataTable(dgvDataOwn.Name));

                m_MainDataset = new DataSet();
                m_DataAdapter = new Dictionary<Object, MySqlDataAdapter>();

                foreach (KeyValuePair<Object, DataTable> dtKVP in m_DGVTables)
                {
                    m_MainDataset.Tables.Add(dtKVP.Value);                    
                    m_DataAdapter.Add(dtKVP.Key, null);
                }

                // preparing bindingsources
                m_BindingSources    = new Dictionary<object, BindingSource>();
                m_BindingSources.Add(dgvData,            new BindingSource());
                m_BindingSources.Add(dgvDataOwn,         new BindingSource());


                // connect datatables to bindingsources and bindsources to datagrids
                foreach(KeyValuePair<object, BindingSource> currentKVP in m_BindingSources)
                { 
                    // set the DataTable as datasource of the BindingSource
		            currentKVP.Value.DataSource = m_DGVTables[currentKVP.Key];  

                    // set the BindingSource as datasource of the gui object
                    var DGV_Object                  = currentKVP.Key;
                    if(DGV_Object.GetType().Equals(typeof(DataGridViewExt)))
                    {
                        ((DataGridViewExt)DGV_Object).AutoGenerateColumns   = false;
                        ((DataGridViewExt)DGV_Object).DataSource            = currentKVP.Value;
                    }
                    else if(DGV_Object.GetType().Equals(typeof(ComboBox)))
                        ((ComboBox)DGV_Object).DataSource           = currentKVP.Value;
                    else
                        Debug.Print("unknown");
                }

                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, new DBConnector(Program.DBCon.ConfigData, true));
                m_GUIInterface.loadAllSettings(this);

                rbCommodities.CheckedChanged   += rbType_CheckedChanged;
                rbCategories.CheckedChanged    += rbType_CheckedChanged;
                rbEconomyLevels.CheckedChanged += rbType_CheckedChanged;

                LoadData();
                FilterData();

                Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                Cursor = oldCursor;
                throw new Exception("Error during initialization the commanders log tab", ex);
            }
        }

        /// <summary>
        /// load the selected data into DataGridViews
        /// </summary>
        private void LoadData()
        {
            String sqlString1;
            String sqlString2;
            String parameterName;
            String activeSetting;
            MySqlDataAdapter DataAdapter;

            try
            {
                Cursor = Cursors.WaitCursor;

                m_ChangedIDs.Clear();
                cmdSave.Enabled             = false;
                m_DataAdapter[dgvData]      = null;
                m_DataAdapter[dgvDataOwn]   = null;

                m_MainDataset.Clear();

                parameterName = gbType.Tag.ToString().Split(new char[] {';'})[0];
                activeSetting = Program.DBCon.getIniValue(DB_GROUPNAME, parameterName);

                if (clbLanguageFilter.CheckedItems.Count > 0)
                {
                    switch (activeSetting)
                    {
                        case "Commodity":
                            sqlString1 = "select commodity_id As id1, language_id As id2, locname as name from tbCommodityLocalization where commodity_id >= 0 order by id1";
                            sqlString2 = "select commodity_id As id1, language_id As id2, locname as name from tbCommodityLocalization where commodity_id < 0  order by id1 desc";
                            break;

                        case "Category":
                            sqlString1 = "select category_id As id1, language_id As id2, locname as name from tbCategoryLocalization where category_id >= 0  order by id1";
                            sqlString2 = "select category_id As id1, language_id As id2, locname as name from tbCategoryLocalization where category_id < 0  order by id1 desc";
                            break;

                        case "Economylevel":
                            sqlString1 = "select economylevel_id As id1, language_id As id2, locname as name from tbLevelLocalization where economylevel_id >= 0  order by id1";
                            sqlString2 = "select economylevel_id As id1, language_id As id2, locname as name from tbLevelLocalization where economylevel_id < 0  order by id1 desc";
                            break;

                        default:
                            throw new Exception("unknown setting :  " + activeSetting);
                    }

                    DataAdapter = m_DataAdapter[dgvData];
                    Program.DBCon.TableRead(sqlString1, dgvData.Name, m_MainDataset, ref DataAdapter);
                    m_DataAdapter[dgvData] = DataAdapter;

                    DataAdapter = m_DataAdapter[dgvDataOwn];
                    Program.DBCon.TableRead(sqlString2, dgvDataOwn.Name, m_MainDataset, ref DataAdapter);
                    m_DataAdapter[dgvDataOwn] = DataAdapter;
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                throw new Exception("Error while loading data", ex);
            }
        }

        /// <summary>
        /// filters the data to show only wanted languages
        /// </summary>
        private void FilterData()
        {
            String languageSelection = "";

            try
            {
                foreach (var item in clbLanguageFilter.CheckedItems)
                {
                    if (languageSelection.Length > 0)
                        languageSelection += " or ";

                    languageSelection += "id2 = " + Program.Data.BaseTableNameToID("language", item.ToString());
                }

                m_BindingSources[dgvData].Filter    = languageSelection;
                m_BindingSources[dgvDataOwn].Filter = languageSelection;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while filtering the data", ex);
            }
        }

        /// <summary>
        /// saves changes to the database
        /// </summary>
        private void SaveData()
        {
            String sqlString;

            try
            {
                Program.DBCon.TransBegin();

                Program.DBCon.TableUpdate(dgvData.Name, m_MainDataset, m_DataAdapter[dgvData]);
                Program.DBCon.TableUpdate(dgvDataOwn.Name, m_MainDataset, m_DataAdapter[dgvDataOwn]);

                foreach (var changedValuePair in m_ChangedIDs)
                {
                    // change the collected data to the new id
                    sqlString = String.Format("update tbCommodityData" +
                                              " set   commodity_id = {1}" +
                                              " where commodity_id = {0}", 
                                              changedValuePair.Key, 
                                              changedValuePair.Value);
                    Program.DBCon.Execute(sqlString);

                    // delete entry from tbCommodity, the ForeigenKeys will delete the 
                    // entries from the other affected tables
                    sqlString = String.Format("delete from tbCommodity" +
                                              " where id = {0}", 
                                              changedValuePair.Key);
                    Program.DBCon.Execute(sqlString);
                }

                Program.DBCon.TransCommit();

                cmdSave.Enabled  = m_MainDataset.HasChanges();

                Program.Data.DeleteMultiplePrices();
                Program.Data.AddMissingLocalizationEntries();
                Program.Data.updateTranslation();
            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                throw new Exception("Error while saving data", ex);
            }
        }

        /// <summary>
        /// changes the data to selected "type"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_GUIInterface.saveSetting(sender)) 
                {
                    LoadData();           
                    FilterData();
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in rbType_CheckedChanged");
            }
        }   

        private void dgvData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow currentDataGridRowView;
            DataRowView currentDataRowView;        
            DataRow currentDataRow;
                
            try
            {
                
                if (e.ColumnIndex == 1)
                {
                    int? value = (int?)((DataGridViewExt)sender).Rows[e.RowIndex].Cells[1].Value;

                    if (value != null)
                        e.Value = Program.Data.BaseTableIDToName("language", value);
                }

                currentDataGridRowView  = (DataGridViewRow)((DataGridViewExt)sender).Rows[e.RowIndex];
                currentDataRowView      = (DataRowView)currentDataGridRowView.DataBoundItem;
                currentDataRow          = (DataRow)currentDataRowView.Row;

                //Debug.Print(m_DGVTables[(DataGridViewExt)sender].Rows[e.RowIndex][0].ToString());
                //if(m_DGVTables[(DataGridViewExt)sender].Rows[e.RowIndex][0] == "118")
                //    Debug.Print("s");

                if(currentDataRow.RowState != DataRowState.Unchanged)
                {
                    e.CellStyle.BackColor = Program.Colors.Marked_BackColor;
                    e.CellStyle.ForeColor = Program.Colors.Marked_ForeColor;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvData_CellFormatting");
            }
        }

        private void clbLanguageFilter_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, clbLanguageFilter.Items[e.Index].ToString(),e.NewValue == CheckState.Checked ? true.ToString() : false.ToString());
                this.BeginInvoke((MethodInvoker) (() => FilterData()));
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in clbLanguageFilter_ItemCheck");
            }
        }

        private void dgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (m_BindingSources != null)
                    m_BindingSources[((DataGridViewExt)sender)].EndEdit();

                cmdSave.Enabled = (m_MainDataset != null) && (m_MainDataset.HasChanges());                   
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvData_CellValueChanged");   
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                SaveData();
                m_DataChanged = true;

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error in cmdSave_Click");   
            }
        }

        private void LanguageEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (m_DataChanged)
                {
                    Program.Data.updateTranslation();

                    Program.Data.PrepareBaseTables("tbcommoditylocalization");
                    Program.Data.PrepareBaseTables("tblevellocalization");
                    Program.Data.PrepareBaseTables("tbcategorylocalization");

                    Program.Data.PrepareBaseTables("tbcommodity");
                    Program.Data.PrepareBaseTables("tbcategory");
                    Program.Data.PrepareBaseTables("tbeconomylevel");
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error in LanguageEdit_FormClosing");   
            }
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdExit_Click");   
            }
        }

        private void dgvDataOwn_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    dgvDataOwn.DoDragDrop(this.dgvDataOwn.CurrentRow, DragDropEffects.Link);
                }
            }            
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvDataOwn_MouseMove");   
            }
        }

        private void dgvData_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
                {
                    e.Effect = DragDropEffects.Link;
                }            
            }            
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvData_DragEnter");   
            }
        }

        private void dgvData_DragDrop(object sender, DragEventArgs e)
        {

            try
            {
                DataGridViewRow row = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                Point clientPoint = dgvData.PointToClient(new Point(e.X, e.Y));

                var rowIndexOfItemUnderMouseToDrop = dgvData.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if ((row != null) || (rowIndexOfItemUnderMouseToDrop != -1))
                {

                    //DataGridViewRow newrow = row.Clone() as DataGridViewRow;

                    //for (int i = 0; i < newrow.Cells.Count; i++)
                    //{
                    //    newrow.Cells .Value = row.Cells.Value;
                    //}
                    
                    //dgvData.Rows.Add(newrow);
                }

                
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvData_DragDrop");   
            }
        }

        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if ((dgvData.SelectedRows.Count == 1) && (dgvDataOwn.SelectedRows.Count == 1))
                    cmdConfirm.Enabled = true;
                else
                    cmdConfirm.Enabled = false;
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvData_SelectionChanged");        
            }
        }

        private void cmdConfirm_Click(object sender, EventArgs e)
        {
            String InfoString;
            Int32 languageID;
            DataRow[] currentBaseItem;
            DataRow[] currentItems;
            DataRow[] ownBaseItem;
            DataRow[] ownItems;
            String currentLanguage;

            try
            {
                currentLanguage = Program.DBCon.getIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "Language");

                currentBaseItem = m_DGVTables[dgvData   ].Select(string.Format("id1 = {0} and id2 = 0", dgvData.SelectedRows[0].Cells[0].Value));
                ownBaseItem     = m_DGVTables[dgvDataOwn].Select(string.Format("id1 = {0} and id2 = 0", dgvDataOwn.SelectedRows[0].Cells[0].Value));


                if (rbOnlyUserlanguage.Checked)
                {
                    languageID      = (Int32)Program.Data.BaseTableNameToID("language", currentLanguage);

                    currentItems    = m_DGVTables[dgvData   ].Select(string.Format("id1 = {0} and id2 = {1}", dgvData.SelectedRows[0].Cells[0].Value, languageID));
                    ownItems        = m_DGVTables[dgvDataOwn].Select(string.Format("id1 = {0} and id2 = {1}", dgvDataOwn.SelectedRows[0].Cells[0].Value, languageID));
                    
                }
                else
                {
                    currentItems    = m_DGVTables[dgvData   ].Select(string.Format("id1 = {0} and id2 <> 0", dgvData.SelectedRows[0].Cells[0].Value));
                    ownItems        = m_DGVTables[dgvDataOwn].Select(string.Format("id1 = {0} and id2 <> 0", dgvDataOwn.SelectedRows[0].Cells[0].Value));
                }

                InfoString  = "Do you want to change the translation for \n\n";

                InfoString += String.Format("(eng) : {1} (id {0})\n\n", dgvData.SelectedRows[0].Cells[0].Value, dgvData.SelectedRows[0].Cells[2].Value);

                for (int i = 0; i <= currentItems.GetUpperBound(0); i++)
                {
                    InfoString += String.Format("({0}) : from <{1}> to <{2}>\n", Program.Data.BaseTableIDToName("language", (int?)currentItems[i][1]), currentItems[i][2].ToString(), ownItems[i][2].ToString());    
                }

                if (MessageBox.Show(InfoString, "Update translation...",  MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                {
                    for (int i = 0; i <= currentItems.GetUpperBound(0); i++)
                    {
                        currentItems[i][2] = ownItems[i][2];
                        
                    }

                    m_ChangedIDs.Add((Int32)ownBaseItem[0][0], (Int32)currentBaseItem[0][0]);
                    var deleteOwn = m_DGVTables[dgvDataOwn].Select(string.Format("id1 = {0}", ownBaseItem[0][0]));

                    foreach (var item in deleteOwn)
                    {
                        m_DGVTables[dgvDataOwn].Rows.Remove(item);
                    }
                }

                cmdSave.Enabled = (m_MainDataset != null) && (m_MainDataset.HasChanges());                   

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdConfirm_Click");        
            }
        }


    }
}
