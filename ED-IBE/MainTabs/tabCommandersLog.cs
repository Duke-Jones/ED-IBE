using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBE.SQL;
using System.Diagnostics;
using IBE.SQL.Datasets;
using IBE.Enums_and_Utility_Classes;

namespace IBE.MTCommandersLog
{
    public partial class tabCommandersLog : UserControl
    {
        // current state of Commanders Log
        private enum enCLAction
	    {
            None,
            Edit,
            Add
	    }

        private const String        DB_GROUPNAME                    = "CmdrsLog";

        private CommandersLog       m_DataSource;                   // data object
        private enCLAction          m_CL_State;                     // current gui state

        private Int32               m_InitialTopOfGrid;
        private Int32               m_InitialTopOfEditGroupBox;

        private Boolean             m_CellValueNeededIsRegistered   = false;        // true if the event is already registred
        private Boolean             m_FirstRowShown                 = false;        // true after first time shown
        private DBGuiInterface      m_GUIInterface;

        /// <summary>
        /// Constructor
        /// </summary>
        public tabCommandersLog()
        {
            InitializeComponent();
            Dock        = DockStyle.Fill;
            this.Name   = "tabCommandersLog";
        }

        /// <summary>
        /// sets or gets the data object
        /// </summary>
        public CommandersLog DataSource
        {
            get
            {
                return m_DataSource;
            }
            set
            {
                m_DataSource     = value;

                if((m_DataSource != null) && (m_DataSource.GUI != this))
                { 
                    if(m_DataSource.GUI != null)
                        m_DataSource.DataChanged -= m_DataSource_DataChanged;

                    m_DataSource.GUI = this;

                    m_DataSource.DataChanged += m_DataSource_DataChanged;
                }
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
                Cursor = Cursors.WaitCursor;

                m_InitialTopOfGrid                      = dgvCommandersLog.Top;
                m_InitialTopOfEditGroupBox              = gbCL_LogEdit.Top;
                m_CL_State                              = enCLAction.None;

                cbLogSystemName.SelectedIndexChanged += cbLogSystemName_SelectedIndexChanged;

                //preparing the combo boxes
                m_DataSource.prepareCmb_EventTypes(ref cbLogEventType);

                m_DataSource.prepareCmb_EventTypes(ref cbLogSystemName);
                m_DataSource.prepareCmb_EventTypes(ref cbLogStationName, cbLogSystemName);
                m_DataSource.prepareCmb_EventTypes(ref cbLogCargoName);
                m_DataSource.prepareCmb_EventTypes(ref cbLogCargoAction);

                dtpLogEventDate.CustomFormat = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern + " " + 
                                               System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern;

                dtpLogEventDate.Format       = System.Windows.Forms.DateTimePickerFormat.Custom;
    
                setCLFieldsEditable(false);

                // preparing the datagridview                
                dgvCommandersLog.VirtualMode              = true;
                dgvCommandersLog.ReadOnly                 = true;
                dgvCommandersLog.AllowUserToAddRows       = false;
                dgvCommandersLog.AllowUserToOrderColumns  = false;

                dgvCommandersLog.RowCount                 = m_DataSource.InitRetriever();

                dgvCommandersLog.RowEnter                += dgvCommandersLog_RowEnter;
                dgvCommandersLog.RowPrePaint             += dgvCommandersLog_RowPrePaint;
                dgvCommandersLog.Paint                   += dgvCommandersLog_Paint;

                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, new DBConnector(Program.DBCon.ConfigData, true));
                m_GUIInterface.loadAllSettings(this);

                Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                Cursor = oldCursor;
                throw new Exception("Error during initialization the commanders log tab", ex);
            }

        }

        /// <summary>
        /// initiate filling of editable fields on first activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvCommandersLog_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if(!m_FirstRowShown)
                { 
                    showRowInFields(new DataGridViewCellEventArgs(0,0));
                    m_FirstRowShown = true;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommandersLog_Paint");
            }
        }

        /// <summary>
        /// before the first row is painted we register the CellValueNeeded-handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvCommandersLog_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                if(!m_CellValueNeededIsRegistered)
                { 
                    dgvCommandersLog.CellValueNeeded          += new DataGridViewCellValueEventHandler(dgvCommandersLog_CellValueNeeded);
                    dgvCommandersLog.CellValuePushed          += new DataGridViewCellValueEventHandler(dgvCommandersLog_CellValuePushed);
                    m_CellValueNeededIsRegistered = true;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommandersLog_RowPrePaint");
            }
        }

        /// <summary>
        /// puts changed data to the datacache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvCommandersLog_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                //Debug.Print(String.Format("Edited : CommodityRow {0}, Column {1}, Value: {2}", e.RowIndex, e.ColumnIndex, e.Value.ToString()));
                m_DataSource.Retriever.MemoryCache.SetElementToPage(e.RowIndex, e.ColumnIndex, e.Value);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommandersLog_CellValuePushed");
            }
        }

        /// <summary>
        /// gets the cell data from the datacache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandersLog_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                Int32 firstRow = dgvCommandersLog.FirstDisplayedScrollingRowIndex - 50;
                if (firstRow < 0)
                    firstRow = 0;
                Int32 minPageStart = (firstRow / 50) * 50;

                Int32 lastRow = dgvCommandersLog.FirstDisplayedScrollingRowIndex + 50;
                Int32 maxPageEnd   = ((lastRow / 50) + 1) * 50 - 1;
                if (maxPageEnd >= m_DataSource.Retriever.RowCount())
                    maxPageEnd = m_DataSource.Retriever.RowCount() -1;
                
                //if(e.RowIndex == 650)
                //    Debug.Print("Stop");

                if ((e.RowIndex >= minPageStart) && (e.RowIndex <= maxPageEnd))
                {
                    //Debug.Print("Erste Zeile : " + dgvCommandersLog.FirstDisplayedScrollingRowIndex + ", Zeilendaten angefragt: " + e.RowIndex + ", erste sinnvolle Zeile : " + minPageStart + ", letzte sinnvolle Zeile : " + maxPageEnd);
                    e.Value = m_DataSource.Retriever.MemoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
                }
                //else
                //    Debug.Print("Erste Zeile : " + dgvCommandersLog.FirstDisplayedScrollingRowIndex + ", Zeilendaten angefragt: " + e.RowIndex + ", erste sinnvolle Zeile : " + minPageStart + ", letzte sinnvolle Zeile : " + maxPageEnd + " verweigert");

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommandersLog_CellValueNeeded");
            }
        }

        /// <summary>
        /// change the available stations if system is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLogSystemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshStationComboBoxData();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbLogSystemName_SelectedIndexChanged");
            }
        }

        /// <summary>
        /// the data object informs the gui about changed data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_DataSource_DataChanged(object sender, CommandersLog.DataChangedEventArgs e)
        {
            try
            {
                RefreshTab(e.DataRow);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in m_DataSource_DataChanged");
            }
        }

        private delegate void delInt32(Int32 currentRow);

        /// <summary>
        /// forces refreshing this tab
        /// </summary>
        private void RefreshTab(Int32 currentRow)
        {
            try
            {
                if(this.InvokeRequired)
                {
                    this.Invoke(new delInt32(RefreshTab), currentRow);
                }
                else
                {
                    // force refresh
                    m_DataSource.Retriever.MemoryCache.Clear();
                    dgvCommandersLog.RowCount  = m_DataSource.Retriever.RowCount(true);
                    dgvCommandersLog.Invalidate();
                    

                    // jump to the new row
                    if (dgvCommandersLog.RowCount > currentRow)
                        dgvCommandersLog.CurrentCell = dgvCommandersLog[1, currentRow];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing the tab (RefreshTab)", ex);
            }
        }


        /// <summary>
        /// initates editing of the editable fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCL_EditEntry_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshSystemComboBoxData();
                RefreshStationComboBoxData();

                m_CL_State = enCLAction.Edit;
                setCLFieldsEditable(true);

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while start editing entry");
            }
        }

        /// <summary>
        /// prepares a new lof event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCL_PrepareNew_Click(object sender, EventArgs e)
        {
            try
            {
                m_CL_State = enCLAction.Add;

                setCLFieldsEditable(true, true);

                cbLogEventType.SelectedValue    = (Int32)12;
                dtpLogEventDate.Value           = (DateTime)DateTime.UtcNow;
                cbLogSystemName.Text            = Program.actualCondition.System;
                cbLogStationName.Text           = Program.actualCondition.Location;
                nbTransactionAmount.Text        = "0";
                nbCurrentCredits.Text           = "0";
                cbLogCargoName.Text             = "";
                cbLogCargoAction.Text            = "";
                nbLogQuantity.Text              = "0";
                tbLogNotes.Text                 = "";
                txtLogDistance.Text             = "";
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while preparing new entry");
            }
        }

        /// <summary>
        /// saving a new or edited event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCL_Save_Click(object sender, EventArgs e)
        {
            try
            {
                saveLogEntry();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while saving log entry");
            }
        }

        /// <summary>
        /// initiates the save procedure
        /// (copying data to the datacache (trough DataGridView) and writing changes to DB)
        /// </summary>
        private void saveLogEntry()
        {
            Double Distance;
            DataGridViewRow currentRow;
            try
            {
                setCLFieldsEditable(false);

                dgvCommandersLog.ReadOnly = false;

                

                if (dgvCommandersLog.CurrentRow == null)
                {
                    dgvCommandersLog.Rows.Add();
                    currentRow = dgvCommandersLog.Rows[dgvCommandersLog.RowCount-1];
                }
                else
                    currentRow = dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index];

                //dgvCommandersLog.Rows.Add()
                // put the changed data into the DataGridView (this will fire the "CellValuePushed"-event)
                currentRow.Cells["eevent"].Value              = cbLogEventType.Text;
                currentRow.Cells["time"].Value                = dtpLogEventDate.Value;
                currentRow.Cells["systemname"].Value          = cbLogSystemName.Text;
                currentRow.Cells["stationname"].Value         = cbLogStationName.Text;
                currentRow.Cells["credits_transaction"].Value = nbTransactionAmount.Value;
                currentRow.Cells["credits_total"].Value       = nbCurrentCredits.Value;
                currentRow.Cells["loccommodity"].Value        = cbLogCargoName.Text;
                currentRow.Cells["action"].Value              = cbLogCargoAction.Text;
                currentRow.Cells["cargovolume"].Value         = nbLogQuantity.Value;
                currentRow.Cells["notes"].Value               = tbLogNotes.Text;
                currentRow.Cells["distance"].Value            =  txtLogDistance.Value.ToString().ToNString();

                dgvCommandersLog.ReadOnly = true;

                // save changed data (from data cache through "CellValuePushed"-event) to database
                m_DataSource.SaveEvent((dsEliteDB.vilogRow)m_DataSource.Retriever.MemoryCache.RetrieveDataColumn(dgvCommandersLog.CurrentRow.Index));

                m_CL_State = enCLAction.None;

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while saving entry");
            }

        }

        private void cmdCL_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                showRowInFields(new DataGridViewCellEventArgs(dgvCommandersLog.CurrentCellAddress.X, dgvCommandersLog.CurrentCellAddress.Y));

                setCLFieldsEditable(false);

                m_CL_State = enCLAction.None;
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while cancel editing entry");
            }
        }

        private void dgvCommandersLog_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                showRowInFields(e);

                setCLFieldsEditable(false);

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while dgvCommandersLog_RowEnter-event");
            }
        }

        private void showRowInFields(DataGridViewCellEventArgs e)
        {
            try
            {
                if((e.RowIndex >= 0) && (dgvCommandersLog.Rows.Count > 0) && (dgvCommandersLog.Rows[e.RowIndex].Cells["time"].Value != null))
                {
                    var currentRow = dgvCommandersLog.Rows[e.RowIndex];

                    cbLogEventType.Text             = (String)currentRow.Cells["eevent"].Value.ToString();
                    dtpLogEventDate.Value           = (DateTime)currentRow.Cells["time"].Value;
                    cbLogSystemName.Text            = (String)currentRow.Cells["systemname"].Value.ToString();
                    cbLogSystemName.TextBox_ro.Text = (String)currentRow.Cells["systemname"].Value.ToString();
                    
                    // force reloading of the stations-ComboBox-data to avoid internal exceptions
                    //m_DataSource.prepareCmb_EventTypes(ref cbLogStationName, cbLogSystemName);   

                    //Debug.Print(currentRow.Cells["stationname"].Value.ToString());
                    cbLogStationName.Text       = (String)currentRow.Cells["stationname"].Value.ToString();
                    nbTransactionAmount.Text    = (String)currentRow.Cells["credits_transaction"].Value.ToString();
                    nbCurrentCredits.Value      = (Int32)currentRow.Cells["credits_total"].Value;
                    cbLogCargoName.Text         = (String)currentRow.Cells["loccommodity"].Value.ToString();
                    cbLogCargoAction.Text       = (String)currentRow.Cells["action"].Value.ToString();
                    nbLogQuantity.Value         = (Int32)currentRow.Cells["cargovolume"].Value;
                    tbLogNotes.Text             = (String)currentRow.Cells["notes"].Value.ToString().Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                    txtLogDistance.Value        =  currentRow.Cells["distance"].Value;
                    
                }
                else
                {
                    cbLogEventType.Text         = (String)"";
                    dtpLogEventDate.Value       = (DateTime)DateTime.UtcNow;
                    cbLogSystemName.Text        = (String)"";
                    cbLogStationName.Text       = (String)"";
                    nbTransactionAmount.Text    = (String)"";
                    nbCurrentCredits.Value      = (Int32)0;
                    cbLogCargoName.Text         = (String)"";
                    cbLogCargoAction.Text       = (String)"";
                    nbLogQuantity.Value         = (Int32)0;
                    tbLogNotes.Text             = (String)"";
                    txtLogDistance.Text         = (String)"";
                }

                setCLFieldsEditable(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in showRowInFields", ex);
            }
        }

        private void setCLFieldsEditable(Boolean Enabled, Boolean TimeEditable = false)
        {
            try
            {
                if(Enabled)
                {
                    cbLogSystemName.DropDownStyle  = ComboBoxStyle.DropDown;
                    cbLogStationName.DropDownStyle = ComboBoxStyle.DropDown;
                }
                else
                {
                    cbLogSystemName.DropDownStyle  = ComboBoxStyle.Simple;
                    cbLogStationName.DropDownStyle = ComboBoxStyle.Simple;
                }

                cbLogEventType.ReadOnly           = !Enabled;
                dtpLogEventDate.ReadOnly          = !TimeEditable;
                cbLogSystemName.ReadOnly          = !Enabled;
                cbLogStationName.ReadOnly         = !Enabled;
                nbTransactionAmount.ReadOnly      = !Enabled;
                nbCurrentCredits.ReadOnly         = !Enabled;
                cbLogCargoName.ReadOnly           = !Enabled;
                cbLogCargoAction.ReadOnly         = !Enabled;
                nbLogQuantity.ReadOnly            = !Enabled;
                tbLogNotes.ReadOnly               = !Enabled;
                txtLogDistance.ReadOnly           = !Enabled;

                dgvCommandersLog.Enabled          = !Enabled;
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in setCLFieldsEditable()");
            }
        }

        /// <summary>
        /// show/hides the editable fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCL_ShowHide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                setEditfieldBoxVisible(cb_ShowEditField.Checked);
                m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
               cErr.processError(ex, "Error in cmdCL_ShowHide_Click");    
            }
        }

        /// <summary>
        /// switches the visibilty of the editbutton-panel and saves the setting to db
        /// </summary>
        /// <param name="setVisible"></param>
        private void setEditfieldBoxVisible(Boolean setVisible)
        {
            try 
	        {	        
                if(setVisible)
                {
                    gbCL_LogEdit.Visible     = true;
                    dgvCommandersLog.Top     = m_InitialTopOfGrid;
                    dgvCommandersLog.Height  = this.Height - dgvCommandersLog.Top;
                }
                else
                {
                    dgvCommandersLog.Top     = gbCL_LogEdit.Top;
                    dgvCommandersLog.Height  = this.Height - dgvCommandersLog.Top;
                    gbCL_LogEdit.Visible     = false;
                }

                cb_ShowEditField.Checked = setVisible;
                Program.DBCon.setIniValue(DB_GROUPNAME, "showEditFields", setVisible.ToString());
	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while changing visibility of editfield-groupbox", ex);
	        }

        }

        /// <summary>
        /// external call for refreshing this tab
        /// </summary>
        public void RefreshData()
        {
            try
            {
                RefreshTab(dgvCommandersLog.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing data", ex);
            }
        }

        private void dgvCommandersLog_Click(object sender, EventArgs e)
        {
            MouseEventArgs args;
            DataGridView dgv;
            DataGridView.HitTestInfo hit;

            try
            {
                args   = (MouseEventArgs)e;

                if(args.Button == System.Windows.Forms.MouseButtons.Right)
                { 
                    dgv   = (DataGridView)sender;
                    hit   = dgv.HitTest(args.X, args.Y);

                    if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
                    {
                        DataGridViewSettings Tool = new DataGridViewSettings();

                        if(Tool.setVisibility(dgv) == DialogResult.OK)
                        {
                            m_GUIInterface.saveSetting(dgv);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DataGridView_Click", ex);
            }
        }

        private void dgvCommandersLog_ColumnSorted(object sender, DataGridViewExt.SortedEventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender, e))
                {
                    sortDataGridView((DataGridView)sender);
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while changing the sort order (all CmdrsLog)");
            }
        }

        /// <summary>
        /// starts sorting of the all commodities-tab by the setting from the database
        /// </summary>
        private void sortDataGridView(DataGridView SortedDataGridView)
        {
            Cursor oldCursor =  this.Cursor;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                m_GUIInterface.loadSetting(SortedDataGridView);  
                this.Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                this.Cursor = oldCursor;
                throw new Exception("Error while sorting grid", ex);
            }
        }

        /// <summary>
        /// switching the state of the delete button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandersLog_SelectionChanged(object sender, EventArgs e)
        {

            try
            {
                cmdCL_DeleteEntry.Enabled = (dgvCommandersLog.SelectedRows.Count > 0);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvCommandersLog_SelectionChanged");
            }
        }

        private void cmdCL_DeleteEntry_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 RowCount =  dgvCommandersLog.SelectedRows.Count;
                DialogResult DlgResult = DialogResult.Cancel;

                if(RowCount == 1)
                   DlgResult = MessageBox.Show("Delete one row. Are you sure ?", "Delete rows from log ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                else if(RowCount > 1)
                   DlgResult = MessageBox.Show("Delete " + RowCount + "  rows. Are you sure ?", "Delete rows from log ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if(DlgResult == DialogResult.OK)
                { 
                    Int32 RowIndex = dgvCommandersLog.SelectedRows[0].Index;

                    m_DataSource.DeleteRows(dgvCommandersLog.SelectedRows);

                    RefreshTab(RowIndex);
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while deleting rows from the commanders log");
            }
        }
        
        /// <summary>
        /// when editing we try to load the matching systems into the Combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLogSystemName_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                RefreshSystemComboBoxData();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbLogSystemName_TextChanged");
            }
        }

        private void RefreshSystemComboBoxData()
        {
            try
            {
                String cText = cbLogSystemName.Text;

                cbLogSystemName.SuspendLayout();
                cbLogSystemName.BeginUpdate();

                m_DataSource.LoadSystemComboBoxData(cText, cbLogSystemName);
                cbLogSystemName.DroppedDown = true;

                cbLogSystemName.Text = cText;
                cbLogSystemName.SelectionStart = cText.Length;

                cbLogSystemName.ResumeLayout();
                cbLogSystemName.EndUpdate();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing combobox-data for systems-combobox", ex);
            }
        }

        private void RefreshStationComboBoxData()
        {
            try
            {
                m_DataSource.prepareCmb_EventTypes(ref cbLogStationName, cbLogSystemName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing combobox-data for stations-combobox", ex);
            }
        }

        private void dgvCommandersLog_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dgvCommandersLog.CellValueNeeded          += new DataGridViewCellValueEventHandler(dgvCommandersLog_CellValueNeeded);
        }
    }
}
