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
using DataGridViewAutoFilter;

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
        private Int32               m_OffsetBindingNavigator;
        private Int32               m_InitialTopOfEditGroupBox;

        private Boolean             m_CellValueNeededIsRegistered   = false;        // true if the event is already registred
        private Boolean             m_FirstRowShown                 = false;        // true after first time shown
        private DBGuiInterface      m_GUIInterface;
        private DataGridView        m_ClickedDGV;
        private MouseEventArgs      m_ClickedDGVArgs;

        /// <summary>
        /// Constructor
        /// </summary>
        public tabCommandersLog()
        {
            InitializeComponent();
            Dock        = DockStyle.Fill;
            this.Name   = "tabCommandersLog";

            if (!((Control)this).IsDesignMode())
                ((Control)this).Retheme();
            
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
                m_OffsetBindingNavigator                = dgvCommandersLog.Top - bindNavCmdrsLog.Top;
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

                ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["eventtype"].HeaderCell).RetrieverSQLSelect = "select distinct E.eventtype";
                ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["eventtype"].HeaderCell).FilterChanged += FilterChanged_Event;

                ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["systemname"].HeaderCell).FilterChanged += FilterChanged_Event;

                ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["stationname"].HeaderCell).FilterChanged += FilterChanged_Event;

                ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["notes"].HeaderCell).FilterChanged += FilterChanged_Event;

                ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["time"].HeaderCell).FilterChanged += FilterChanged_Event;
                ((DataGridViewAutoFilterDateTimeColumnHeaderCell)dgvCommandersLog.Columns["time"].HeaderCell).DtpAfter.Value = Program.DBCon.Execute<DateTime>("select min(time) from tbLog").Date;

                dgvCommandersLog.RowEnter                += dgvCommandersLog_RowEnter;
                dgvCommandersLog.RowPrePaint             += dgvCommandersLog_RowPrePaint;
                dgvCommandersLog.Paint                   += dgvCommandersLog_Paint;

                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, new DBConnector(Program.DBCon.ConfigData, true));
                m_GUIInterface.loadAllSettings(this);

                bindNavCmdrsLog.CountItem.Enabled           = true;
                bindNavCmdrsLog.PositionItem.Enabled        = true;

                Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                Cursor = oldCursor;
                throw new Exception("Error during initialization the commanders log tab", ex);
            }

        }

        private void FilterChanged_Event(object sender, DataGridViewAutoFilterHeaderCell.DataChangedEventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender, e))
                {
                    sortDataGridView((DataGridView)sender);
                }

                RefreshData();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in FilterChanged_Event");
            }
        }

        /// <summary>
        /// initiate filling of editable fields on first activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvCommandersLog_Paint(object sender, PaintEventArgs e)
        {
            var oldCursor = Program.MainForm.Cursor;

            try
            {
                if(!m_FirstRowShown)
                { 
                    
                    
                    Program.MainForm.Cursor = Cursors.WaitCursor;

                    dgvCommandersLog.RowCount = m_DataSource.InitRetriever();

                    ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["eventtype"].HeaderCell).Retriever = m_DataSource.Retriever;
                    ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["systemname"].HeaderCell).Retriever = m_DataSource.Retriever;
                    ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["stationname"].HeaderCell).Retriever = m_DataSource.Retriever;
                    ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["notes"].HeaderCell).Retriever = m_DataSource.Retriever;
                    ((DataGridViewAutoFilterHeaderCell)dgvCommandersLog.Columns["time"].HeaderCell).Retriever = m_DataSource.Retriever;

                    dgvCommandersLog.RowCount = m_DataSource.InitRetriever(true);

                    showRowInFields(new DataGridViewCellEventArgs(0,0));

                    SetNavigatorButtons(0);

                    m_FirstRowShown = true;

                    RefreshData();
                    Program.MainForm.Cursor = oldCursor;
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in dgvCommandersLog_Paint");
                Program.MainForm.Cursor = oldCursor;
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
                CErr.processError(ex, "Error in dgvCommandersLog_RowPrePaint");
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
                CErr.processError(ex, "Error in dgvCommandersLog_CellValuePushed");
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
                CErr.processError(ex, "Error in dgvCommandersLog_CellValueNeeded");
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
                CErr.processError(ex, "Error in cbLogSystemName_SelectedIndexChanged");
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
                CErr.processError(ex, "Error in m_DataSource_DataChanged");
            }
        }

        private delegate void delInt32(int? currentRow);

        /// <summary>
        /// forces refreshing this tab
        /// </summary>
        private void RefreshTab(int? currentRow)
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
                    if(m_FirstRowShown)
                    {
                        m_DataSource.Retriever.MemoryCache.Clear();
                        dgvCommandersLog.RowCount  = 0;
                        dgvCommandersLog.RowCount  = m_DataSource.Retriever.RowCount(true);
                        dgvCommandersLog.Invalidate();
                    

                        // jump to the new row
                        if ((currentRow != null) && (dgvCommandersLog.RowCount > currentRow))
                        try
                        {
                            dgvCommandersLog.CurrentCell = dgvCommandersLog[1, currentRow.Value];
                        }
                        catch{}

                        SetNavigatorButtons(currentRow);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing the tab (RefreshTab), RowCount = " + m_DataSource.Retriever.RowCount() + ", currentRow = " + currentRow, ex);
            }
        }

        /// <summary>
        /// things todo, if another tab has become the active tab
        /// </summary>
        public void Unselected()
        {
            try
            {
                if (m_CL_State == enCLAction.Edit) 
                    cmdCL_Cancel_Click(cmdCL_Cancel, new EventArgs());
            }
            catch (Exception ex)
            {
                throw new Exception("Error while doing cleanups because tab is unselected", ex);
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
                CErr.processError(ex, "Error while start editing entry");
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
                cbLogStationName.Text           = Program.actualCondition.Station;
                nbTransactionAmount.Text        = "0";
                nbCurrentCredits.Text           = Program.CompanionIO.SGetCreditsTotal().ToString();
                cbLogCargoName.Text             = "";
                cbLogCargoAction.Text            = "";
                nbLogQuantity.Text              = "0";
                tbLogNotes.Text                 = "";
                txtLogDistance.Text             = "";
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while preparing new entry");
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
                CErr.processError(ex, "Error while saving log entry");
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
                currentRow.Cells["eventtype"].Value           = cbLogEventType.Text;
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
                CErr.processError(ex, "Error while saving entry");
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
                CErr.processError(ex, "Error while cancel editing entry");
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
                CErr.processError(ex, "Error while dgvCommandersLog_RowEnter-event");
            }
        }

        private void showRowInFields(DataGridViewCellEventArgs e)
        {
            try
            {
                if((e.RowIndex >= 0) && (dgvCommandersLog.Rows.Count > 0) && (dgvCommandersLog.Rows[e.RowIndex].Cells["time"].Value != null))
                {
                    var currentRow = dgvCommandersLog.Rows[e.RowIndex];

                    cbLogEventType.Text             = (String)currentRow.Cells["eventtype"].Value.ToString();
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
                    
                    bindNavCmdrsLog.PositionItem.Text = (e.RowIndex + 1).ToString();  

                    SetNavigatorButtons(e.RowIndex);

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

                    bindNavCmdrsLog.PositionItem.Text = "-";  
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
                CErr.processError(ex, "Error in setCLFieldsEditable()");
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
               CErr.processError(ex, "Error in cmdCL_ShowHide_Click");    
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
                    bindNavCmdrsLog.Top      = m_InitialTopOfGrid - m_OffsetBindingNavigator;
                }
                else
                {
                    dgvCommandersLog.Top     = gbCL_LogEdit.Top + m_OffsetBindingNavigator;
                    bindNavCmdrsLog.Top      = gbCL_LogEdit.Top;
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
                int? rowIdx = null;

                if (dgvCommandersLog.CurrentCell != null)
                    rowIdx = dgvCommandersLog.CurrentCell.RowIndex;

                RefreshTab(rowIdx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing data", ex);
            }
        }

        private void dgvCommandersLog_Click(object sender, EventArgs e)
        {
            DataGridView.HitTestInfo hit;

            try
            {
                m_ClickedDGVArgs = (MouseEventArgs)e;

                if(m_ClickedDGVArgs.Button == System.Windows.Forms.MouseButtons.Right)
                { 
                    m_ClickedDGV    = (DataGridView)sender;
                    hit             = m_ClickedDGV.HitTest(m_ClickedDGVArgs.X, m_ClickedDGVArgs.Y);

                    if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
                    {
                        DataGridViewSettings Tool = new DataGridViewSettings();

                        if(Tool.setVisibility(m_ClickedDGV) == DialogResult.OK)
                        {
                            m_GUIInterface.saveSetting(m_ClickedDGV);
                        }
                    }
                    else if (hit.Type == DataGridViewHitTestType.Cell)
                    {
                        cmsLog.Show(m_ClickedDGV, m_ClickedDGVArgs.Location);
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
                CErr.processError(ex, "Error while changing the sort order (all CmdrsLog)");
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
                CErr.processError(ex, "Error in dgvCommandersLog_SelectionChanged");
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
                CErr.processError(ex, "Error while deleting rows from the commanders log");
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
                CErr.processError(ex, "Error in cbLogSystemName_TextChanged");
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

        private void tsmiSendToEDSM_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rowColl;
            List<DateTime> timeStamps = new List<DateTime>();
            DataGridView.HitTestInfo hit;
            String resultString = "";

            try
            {
                if(dgvCommandersLog.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow dgvRow in dgvCommandersLog.SelectedRows)
                    {
                        dgvCommandersLog.CurrentCell = dgvRow.Cells[dgvCommandersLog.Columns["time"].Index];
                        timeStamps.Add((DateTime)dgvRow.Cells[dgvCommandersLog.Columns["time"].Index].Value);
                    }
                }
                else
                {
                    hit = m_ClickedDGV.HitTest(m_ClickedDGVArgs.X, m_ClickedDGVArgs.Y);
                    timeStamps.Add((DateTime)m_ClickedDGV.Rows[hit.RowIndex].Cells[dgvCommandersLog.Columns["time"].Index].Value);
                }

                Program.Data.SendLogToEDSM(timeStamps);

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while sending selected log rows to EDSM");
            }
        }

        private void tsmiRecalcJumpDistance_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rowColl;
            List<DateTime> timeStamps = new List<DateTime>();
            DataGridView.HitTestInfo hit;
            String resultString = "";

            try
            {
                if(dgvCommandersLog.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow dgvRow in dgvCommandersLog.SelectedRows)
                    {
                        dgvCommandersLog.CurrentCell = dgvRow.Cells[dgvCommandersLog.Columns["time"].Index];
                        timeStamps.Add((DateTime)dgvRow.Cells[dgvCommandersLog.Columns["time"].Index].Value);
                    }
                }
                else
                {
                    hit = m_ClickedDGV.HitTest(m_ClickedDGVArgs.X, m_ClickedDGVArgs.Y);
                    timeStamps.Add((DateTime)m_ClickedDGV.Rows[hit.RowIndex].Cells[dgvCommandersLog.Columns["time"].Index].Value);
                }

                Program.Data.RecalcJumpDistancesInLog(timeStamps.Min(), timeStamps.Max());
                RefreshData();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while sending selected log rows to EDSM");
            }
        }

        private void copySystemnameToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView.HitTestInfo hit;
            String resultString = "";
            
            try
            {
                hit = m_ClickedDGV.HitTest(m_ClickedDGVArgs.X, m_ClickedDGVArgs.Y);

                if(m_ClickedDGV.Equals(dgvCommandersLog))
                {
                    var timeStamp = (DateTime)m_ClickedDGV.Rows[hit.RowIndex].Cells[dgvCommandersLog.Columns["time"].Index].Value;

                    dsEliteDB.vilogRow data = Program.Data.GetLogByTimestamp(timeStamp);

                    if(data != null)
                        resultString = data.systemname;
                }

                Clipboard.SetText(resultString);
                Debug.Print(resultString);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in copySystemnameToClipboardToolStripMenuItem_Click");
            }
        }

        private void copyStationnameToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView.HitTestInfo hit;
            String resultString = "";
            
            try
            {
                hit = m_ClickedDGV.HitTest(m_ClickedDGVArgs.X, m_ClickedDGVArgs.Y);

                if(m_ClickedDGV.Equals(dgvCommandersLog))
                {
                    var timeStamp = (DateTime)m_ClickedDGV.Rows[hit.RowIndex].Cells[dgvCommandersLog.Columns["time"].Index].Value;

                    dsEliteDB.vilogRow data = Program.Data.GetLogByTimestamp(timeStamp);

                    if(data != null)
                        resultString = data.stationname;
                }

                Clipboard.SetText(resultString);
                Debug.Print(resultString);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in copyStationnameToClipboardToolStripMenuItem_Click");
            }
        }

        // Displays the drop-down list when the user presses 
        // ALT+DOWN ARROW or ALT+UP ARROW.
        private void dgvCommandersLog_KeyDown(object sender, KeyEventArgs e)
        {
            {
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
                if(dgvCommandersLog.CurrentCell.OwningColumn.HeaderCell.GetType().BaseType.Equals(typeof(DataGridViewAutoFilterHeaderCell)))
                {
                    DataGridViewAutoFilterHeaderCell filterCell = (DataGridViewAutoFilterHeaderCell)dgvCommandersLog.CurrentCell.OwningColumn.HeaderCell;
                    if (filterCell != null)
                    {
                        filterCell.ShowColumnFilter();
                        e.Handled = true;
                    }
                }
            }
        }

        private void multiSelectHeaderList1_Load(object sender, EventArgs e)
        {

        }

        MultiSelectHeaderList ml;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String inputData = "";
                String rawEventName = "";
                Newtonsoft.Json.Linq.JToken journalEntry = null;
                FileScanner.EDJournalScanner.JournalEvent identifiedEventType = FileScanner.EDJournalScanner.JournalEvent.Undefined;

                if((InputBox.Show("Input eventdata", "", ref inputData) == DialogResult.OK) && (!String.IsNullOrWhiteSpace(inputData)))
                {
                    try
                    {
                        journalEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JToken>(inputData);

                        // identify the event
                        rawEventName = journalEntry.Value<String>("event");

                        if(rawEventName != null)
                        {
                            if(!Enum.TryParse<FileScanner.EDJournalScanner.JournalEvent>(rawEventName, out identifiedEventType))
                            {
                                identifiedEventType = FileScanner.EDJournalScanner.JournalEvent.Not_Supported;

                            }

                            journalEntry["timestamp"] = DateTime.UtcNow;

                            FileScanner.EDJournalScanner.JournalEventArgs newJournalArgItem = new FileScanner.EDJournalScanner.JournalEventArgs() { EventType = identifiedEventType, Data = journalEntry, History = null };
                            Program.JournalScanner.InjectJournalEvent(newJournalArgItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        CErr.processError(ex, "Error while parsing a injected journal event");
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while creating test event");
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((dgvCommandersLog.CurrentCell != null) && (dgvCommandersLog.CurrentCell.RowIndex > 0))
                    dgvCommandersLog.CurrentCell = dgvCommandersLog[1, dgvCommandersLog.CurrentCell.RowIndex - 1];

                SetNavigatorButtons(dgvCommandersLog.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in bindingNavigatorMovePreviousItem_Click");
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((dgvCommandersLog.CurrentCell != null) && (dgvCommandersLog.CurrentCell.RowIndex < (dgvCommandersLog.RowCount-1)))
                    dgvCommandersLog.CurrentCell = dgvCommandersLog[1, dgvCommandersLog.CurrentCell.RowIndex + 1];

                SetNavigatorButtons(dgvCommandersLog.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in bindingNavigatorMoveNextItem_Click");
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCommandersLog.RowCount > 0)
                    dgvCommandersLog.CurrentCell = dgvCommandersLog[1, 0];

                SetNavigatorButtons(dgvCommandersLog.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in bindingNavigatorMoveFirstItem_Click");
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCommandersLog.RowCount > 0)
                    dgvCommandersLog.CurrentCell = dgvCommandersLog[1, dgvCommandersLog.RowCount-1];

                SetNavigatorButtons(dgvCommandersLog.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in bindingNavigatorMoveLastItem_Click");
            }
        }
        private void SetNavigatorButtons(int? rowIndex)
        {
            try
            {
                if(rowIndex != null)
                {
                    bindNavCmdrsLog.MovePreviousItem.Enabled    = (rowIndex > 0);
                    bindNavCmdrsLog.MoveFirstItem.Enabled       = (rowIndex > 0);
                    bindNavCmdrsLog.MoveLastItem.Enabled        = (rowIndex < (dgvCommandersLog.RowCount-1));
                    bindNavCmdrsLog.MoveNextItem.Enabled        = (rowIndex < (dgvCommandersLog.RowCount-1));
                }
                else
                {
                    bindNavCmdrsLog.MovePreviousItem.Enabled    = false;
                    bindNavCmdrsLog.MoveFirstItem.Enabled       = false;
                    bindNavCmdrsLog.MoveLastItem.Enabled        = false;
                    bindNavCmdrsLog.MoveNextItem.Enabled        = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting navigator buttons", ex);
            }
        }

    }
}
