using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegulatedNoise.SQL;
using System.Diagnostics;
using RegulatedNoise.SQL.Datasets;
namespace RegulatedNoise.Commander_s_Log
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

        /// <summary>
        /// Constructor
        /// </summary>
        public tabCommandersLog()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
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
                dgvCommandersLog.SelectionMode            = DataGridViewSelectionMode.FullRowSelect;

                dgvCommandersLog.RowCount                 = m_DataSource.InitRetriever();

                dgvCommandersLog.RowEnter                += dgvCommandersLog_RowEnter;
                dgvCommandersLog.RowPrePaint             += dgvCommandersLog_RowPrePaint;
                dgvCommandersLog.Paint                   += dgvCommandersLog_Paint;

                setEditfieldBoxVisible(Program.DBCon.getIniValue<Boolean>(DB_GROUPNAME, "showEditFields", "true", false));

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
                cErr.showError(ex, "Error in dgvCommandersLog_Paint");
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
                cErr.showError(ex, "Error in dgvCommandersLog_RowPrePaint");
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
                //Debug.Print(String.Format("Edited : Row {0}, Column {1}, Value: {2}", e.RowIndex, e.ColumnIndex, e.Value.ToString()));
                m_DataSource.Retriever.MemoryCache.SetElementToPage(e.RowIndex, e.ColumnIndex, e.Value);
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in dgvCommandersLog_CellValuePushed");
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
                e.Value = m_DataSource.Retriever.MemoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in dgvCommandersLog_CellValueNeeded");
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
                m_DataSource.prepareCmb_EventTypes(ref cbLogStationName, cbLogSystemName);   
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in cbLogSystemName_SelectedIndexChanged");
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
                // force refresh
                m_DataSource.Retriever.MemoryCache.Clear();
                dgvCommandersLog.Invalidate();

                // jump to the new row
                dgvCommandersLog.CurrentCell = dgvCommandersLog[1, e.DataRow];

            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in m_DataSource_DataChanged");
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
                m_CL_State = enCLAction.Edit;
                setCLFieldsEditable(true);
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while start editing entry");
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
                dtpLogEventDate.Value           = (DateTime)DateTime.Now;
                cbLogSystemName.Text            = Program.actualCondition.System      != Condition.STR_Scanning ? Program.actualCondition.System  : "";
                cbLogStationName.Text           = Program.actualCondition.Station     != Condition.STR_Scanning ? Program.actualCondition.Station : "";
                nbTransactionAmount.Text        = "0";
                nbCurrentCredits.Text           = "0";
                cbLogCargoName.Text             = "";
                cbLogCargoAction.Text            = "";
                nbLogQuantity.Text              = "0";
                tbLogNotes.Text                 = "";
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while preparing new entry");
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
                cErr.showError(ex, "Error while saving log entry");
            }
        }

        /// <summary>
        /// initiates the save procedure
        /// (copying data to the datacache (trough DataGridView) and writing changes to DB)
        /// </summary>
        private void saveLogEntry()
        {
            try
            {
                setCLFieldsEditable(false);

                dgvCommandersLog.ReadOnly = false;

                // put the changed data into the DataGridView (this will fire the "CellValuePushed"-event)
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["eevent"].Value              = cbLogEventType.Text;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["time"].Value                = dtpLogEventDate.Value;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["systemname"].Value          = cbLogSystemName.Text;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["stationname"].Value         = cbLogStationName.Text;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["credits_transaction"].Value = nbTransactionAmount.Value;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["credits_total"].Value       = nbCurrentCredits.Value;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["loccommodity"].Value        = cbLogCargoName.Text;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["action"].Value              = cbLogCargoAction.Text;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["cargovolume"].Value         = nbLogQuantity.Value;
                dgvCommandersLog.Rows[dgvCommandersLog.CurrentRow.Index].Cells["notes"].Value               = tbLogNotes.Text;

                dgvCommandersLog.ReadOnly = true;

                // save changed data (from data cache through "CellValuePushed"-event) to database
                m_DataSource.SaveEvent((dsCommandersLog.vilogRow)m_DataSource.Retriever.MemoryCache.RetrieveDataColumn(dgvCommandersLog.CurrentRow.Index));

                m_CL_State = enCLAction.None;

            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while saving entry");
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
                cErr.showError(ex, "Error while cancel editing entry");
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
                cErr.showError(ex, "Error while dgvCommandersLog_RowEnter-event");
            }
        }

        private void showRowInFields(DataGridViewCellEventArgs e)
        {
            try
            {
                if((e.RowIndex >= 0) && (dgvCommandersLog.Rows[e.RowIndex].Cells["time"].Value != null))
                {
                    cbLogEventType.Text         = (String)dgvCommandersLog.Rows[e.RowIndex].Cells["eevent"].Value.ToString();
                    dtpLogEventDate.Value       = (DateTime)dgvCommandersLog.Rows[e.RowIndex].Cells["time"].Value;
                    cbLogSystemName.Text        = (String)dgvCommandersLog.Rows[e.RowIndex].Cells["systemname"].Value.ToString();
                    cbLogStationName.Text       = (String)dgvCommandersLog.Rows[e.RowIndex].Cells["stationname"].Value.ToString();
                    nbTransactionAmount.Text    = (String)dgvCommandersLog.Rows[e.RowIndex].Cells["credits_transaction"].Value.ToString();
                    nbCurrentCredits.Value      = (Int32)dgvCommandersLog.Rows[e.RowIndex].Cells["credits_total"].Value;
                    cbLogCargoName.Text         = (String)dgvCommandersLog.Rows[e.RowIndex].Cells["loccommodity"].Value.ToString();
                    cbLogCargoAction.Text       = (String)dgvCommandersLog.Rows[e.RowIndex].Cells["action"].Value.ToString();
                    nbLogQuantity.Value         = (Int32)dgvCommandersLog.Rows[e.RowIndex].Cells["cargovolume"].Value;
                    tbLogNotes.Text             = (String)dgvCommandersLog.Rows[e.RowIndex].Cells["notes"].Value.ToString().Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                }
                else
                {
                    cbLogEventType.Text         = (String)"";
                    dtpLogEventDate.Value       = (DateTime)DateTime.Now;
                    cbLogSystemName.Text        = (String)"";
                    cbLogStationName.Text       = (String)"";
                    nbTransactionAmount.Text    = (String)"";
                    nbCurrentCredits.Value      = (Int32)0;
                    cbLogCargoName.Text         = (String)"";
                    cbLogCargoAction.Text       = (String)"";
                    nbLogQuantity.Value         = (Int32)0;
                    tbLogNotes.Text             = (String)"";
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

                dgvCommandersLog.Enabled          = !Enabled;
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in setCLFieldsEditable()");
            }
        }

        private void setColumns(ListView currentListView)
        {
            List<ColumnData> currentData = Program.Settings.ListViewColumnData[currentListView.Name];

            switch (currentListView.Name)
            {
                case "lvCommandersLog":
                    currentListView.Columns[0].Width 	=  113;
                    currentListView.Columns[1].Width 	=  119;
                    currentListView.Columns[2].Width 	=  122;
                    currentListView.Columns[3].Width 	=  141;
                    currentListView.Columns[4].Width 	=   96;
                    currentListView.Columns[5].Width 	=   72;
                    currentListView.Columns[6].Width 	=   77;
                    currentListView.Columns[7].Width 	=  127;
                    currentListView.Columns[8].Width 	=   60;
                    currentListView.Columns[9].Width 	=   63;
                    currentListView.Columns[10].Width 	=   60;
                    break;
            }

            foreach (ColumnHeader currentHeader in currentListView.Columns)
            {
                ColumnData Data = currentData.Find(x => x.ColumnName.Equals(currentHeader.Name, StringComparison.InvariantCultureIgnoreCase));
                if (Data.Width > -1)
                    currentHeader.Width = Data.Width;
            }

            currentListView.ColumnWidthChanged += lvCommandersLog_ColumnWidthChanged;
        }

        void lvCommandersLog_ColumnWidthChanged(object sender, System.Windows.Forms.ColumnWidthChangedEventArgs e)
        {
            saveColumns((ListView)sender);            
        }

        private void saveColumns(ListView currentListView)
        {
            List<ColumnData> currentData = Program.Settings.ListViewColumnData[currentListView.Name];

            foreach (ColumnHeader currentHeader in currentListView.Columns)
            {
                ColumnData Data = currentData.Find(x => x.ColumnName.Equals(currentHeader.Name, StringComparison.InvariantCultureIgnoreCase));
                Data.Width = currentHeader.Width;
            }

            //SaveSettings();
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
            }
            catch (Exception ex)
            {
               cErr.showError(ex, "Error in cmdCL_ShowHide_Click");    
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
    }
}
