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
using System.Reflection;
using IBE.Enums_and_Utility_Classes;

namespace IBE.MTPriceAnalysis
{
    public partial class tabPriceAnalysis : UserControl
    {


        // current state of Commanders Log
        private enum enCLAction
	    {
            None,
            Edit,
            Add
	    }


        public const String                             DB_GROUPNAME                    = "PriceAnalysis";
        public const String                             CURRENT_SYSTEM                  = "<current system>";
        private const string                            BASE_DATA                       = "BaseData";
        
        private PriceAnalysis                           m_DataSource;                   // data object
        private Dictionary<String, DataTable>           m_DGVTables;                 
        private Dictionary<String, BindingSource>       m_BindingSources;

        private DBGuiInterface                          m_GUIInterface;

                                                                                            
        private Dictionary<String, Boolean>             m_IsRefreshed;                  // shows, which tabs already refreshed after a new filtering
        private Int32                                   m_ActiveCounter;                  

        /// <summary>
        /// Constructor
        /// </summary>
        public tabPriceAnalysis()
        {
            InitializeComponent();
            Dock            = DockStyle.Fill;
            this.Name       = "tabPriceAnalysis";
            m_ActiveCounter = 0;
        }

        /// <summary>
        /// sets or gets the data object
        /// </summary>
        public PriceAnalysis DataSource
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
            String ComboboxValues;
            DataTable Data;
            try
            {

                Cursor = Cursors.WaitCursor;

                // fill dictionary with "RefreshDone"-flags
                m_IsRefreshed      = new Dictionary<string,bool>();
                m_IsRefreshed.Add(BASE_DATA, false);
                foreach (TabPage SubTabPage in tabPriceSubTabs.TabPages)
                    m_IsRefreshed.Add(SubTabPage.Name, false);

                // preparing datatables 
                m_DGVTables     = new Dictionary<string,DataTable>();
                m_DGVTables.Add(dgvStation1.Name,               new dsEliteDB.tmpa_s2s_stationdataDataTable());
                m_DGVTables.Add(dgvStation2.Name,               new dsEliteDB.tmpa_s2s_stationdataDataTable());
                m_DGVTables.Add(dgvStationToStationRoutes.Name, new dsEliteDB.tmpa_s2s_besttripsDataTable());
                m_DGVTables.Add(dgvByStation.Name,              new dsEliteDB.tmpa_bystationDataTable());
                m_DGVTables.Add(dgvByCommodity.Name,            new dsEliteDB.tmpa_bycommodityDataTable());
                m_DGVTables.Add(dgvAllCommodities.Name,         new dsEliteDB.tmpa_allcommoditiesDataTable());
                

                Data = new DataTable();
                m_DGVTables.Add(cmbStation1.Name,               Data);
                m_DGVTables.Add(cmbStation2.Name,               Data);
                m_DGVTables.Add(cmbByStation.Name,              Data);
                m_DGVTables.Add(cmbByCommodity.Name,            Program.Data.BaseData.tbcommodity);
                m_DGVTables.Add(cmbSystemBase.Name,             new DataTable());


                // preparing bindingsources
                m_BindingSources    = new Dictionary<String, BindingSource>();
                m_BindingSources.Add(dgvStation1.Name,                  new BindingSource());
                m_BindingSources.Add(dgvStation2.Name,                  new BindingSource());
                m_BindingSources.Add(dgvStationToStationRoutes.Name,    new BindingSource());
                m_BindingSources.Add(dgvByStation.Name,                 new BindingSource());
                m_BindingSources.Add(dgvByCommodity.Name,               new BindingSource());
                m_BindingSources.Add(dgvAllCommodities.Name,            new BindingSource());

                m_BindingSources.Add(cmbStation1.Name,                  new BindingSource());
                m_BindingSources.Add(cmbStation2.Name,                  new BindingSource());
                m_BindingSources.Add(cmbByStation.Name,                 new BindingSource());
                m_BindingSources.Add(cmbByCommodity.Name,               new BindingSource());
                m_BindingSources.Add(cmbSystemBase.Name,                new BindingSource());

                // connect datatables to bindingsources and bindsources to datagrids
                foreach(KeyValuePair<String, BindingSource> currentKVP in m_BindingSources)
                { 
                    // set the DataTable as datasource of the BindingSource
		            currentKVP.Value.DataSource = m_DGVTables[currentKVP.Key];  

                    // set the BindingSource as datasource of the gui object
                    FieldInfo       DGGV_FieldInfo  = this.GetType().GetField(currentKVP.Key, BindingFlags.NonPublic | BindingFlags.Instance);
                    var DGV_Object                  =DGGV_FieldInfo.GetValue(this);
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

                cmbStation1.SelectedIndex       = -1;
                cmbStation2.SelectedIndex       = -1;
                cmbByStation.SelectedIndex      = -1;
                cmbByCommodity.SelectedIndex    = -1;
                cmbSystemBase.SelectedIndex     = -1;

                ComboboxValues = Program.DBCon.getIniValue(DB_GROUPNAME, "SystemLightYearCmbValues", "10;25;50;100;200;1000", false);
                cmbSystemLightYears.Items.Clear();
                foreach (String Value in ComboboxValues.Split(';'))
                cmbSystemLightYears.Items.Add(Int32.Parse(Value));

                ComboboxValues = Program.DBCon.getIniValue(DB_GROUPNAME, "StationToStarCmbValues", "50;100;500;1000;2000;5000", false);
                cmbStationLightSeconds.Items.Clear();
                foreach (String Value in ComboboxValues.Split(';'))
                cmbStationLightSeconds.Items.Add(Int32.Parse(Value));

                ComboboxValues = Program.DBCon.getIniValue(DB_GROUPNAME, "MaxTripDistances", "1;5;10;12;15;17;20;25;30;40;50;100;150", false);
                cmbMaxTripDistance.Items.Clear();
                foreach (String Value in ComboboxValues.Split(';'))
                cmbMaxTripDistance.Items.Add(Int32.Parse(Value));

                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, new DBConnector(Program.DBCon.ConfigData, true));
                m_GUIInterface.loadAllSettings(this);

                loadCommoditiesForByCommodity();
                //loadSystemsForBaseSystem();

                createNewBaseView();

                SetComboBoxEventsActive(true);

                cmbSystemBase.Text = CURRENT_SYSTEM;
                                              

                Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                Cursor = oldCursor;
                throw new Exception("Error during initialization the commanders log tab", ex);
            }
        }

        /// <summary>
        /// the data object informs the gui about changed data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_DataSource_DataChanged(object sender, PriceAnalysis.DataChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in m_DataSource_DataChanged");
            }
        }

#region global handling of the price tab

        private void SetButtons(Boolean setEnabled)
        {
            try
            {
                cmdRoundTripCaclulation.Enabled         = setEnabled;
                cmdFilter.Enabled                       = setEnabled;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while dis/enabling buttons", ex);
            }
        }

        /// <summary>
        /// sets the buttons as marked or not
        /// </summary>
        /// <param name="setMarked"></param>
        public void setFilterHasChanged(Boolean setMarked)
        {
            try
            {
                if(setMarked)
                {
                    cmdFilter.ForeColor                     = Program.Colors.Marked_ForeColor;
                    cmdFilter.BackColor                     = Program.Colors.Marked_BackColor;
                                                                                             
                    cmdRoundTripCaclulation.ForeColor       = Program.Colors.Marked_ForeColor;
                    cmdRoundTripCaclulation.BackColor       = Program.Colors.Marked_BackColor;

                    m_IsRefreshed[BASE_DATA]                = false;
                    m_IsRefreshed["tpAllCommodities"]       = false;
                    m_IsRefreshed["tpByStation"]            = false;
                    m_IsRefreshed["tpByCommodity"]          = false;
                    m_IsRefreshed["tpStationToStation"]     = false;

                }                                                                        
                else                                                                     
                {                                                                        
                    cmdFilter.ForeColor                 = Program.Colors.Default_ForeColor;
                    cmdFilter.BackColor                 = Program.Colors.Default_BackColor;
                      
                    // commented out: this is doing the button itself 
                    // cmdRoundTripCaclulation.ForeColor   = Program.Colors.Default_ForeColor;
                    // cmdRoundTripCaclulation.BackColor   = Program.Colors.Default_BackColor;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting buttons", ex);
            }
            
        }

        /// <summary>
        /// filter-button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdFilter_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtons(false);

                if (Program.actualCondition.System == "")
                    MessageBox.Show("Current system is unknown.", "Can't calculate...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    setFilterHasChanged(true);
                    ActivateFilterSettings();
                }

                SetButtons(true);
            }
            catch (Exception ex)
            {
                SetButtons(true);
                cErr.processError(ex, "Error while starting the filter");
            }
        }

        /// <summary>
        /// activates the current filter settings
        /// </summary>
        private void ActivateFilterSettings()
        {
            try
            {
                refreshPriceView();

                setFilterHasChanged(false);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while filtering stations");
            }
        }

        /// <summary>
        /// check if there some data to be refreshed
        /// </summary>
        /// <param name="TabWasChanged"></param>
        private void refreshPriceView(Boolean TabWasChanged = false)
        {
            try
            {

                if (!m_IsRefreshed[BASE_DATA])
                { 
                    SetComboBoxEventsActive(false);
                    createNewBaseView();
                    m_IsRefreshed[BASE_DATA] = true;
                    SetComboBoxEventsActive(true);
                }

                switch (tabPriceSubTabs.SelectedTab.Name)
                {
                    case "tpAllCommodities":

                        if (!m_IsRefreshed["tpAllCommodities"])
                        {
                            Refresh_AllCommodities();

                            m_GUIInterface.loadSetting(scAllCommodities_1);
                            m_GUIInterface.loadSetting(scAllCommodities_2);

                            m_IsRefreshed["tpAllCommodities"] = true;
                        }
                        
                        break;

                    case "tpByStation":
                        
                        if (!m_IsRefreshed["tpByStation"])
                        { 
                            Refresh_ByStation();

                            m_IsRefreshed["tpByStation"] = true;
                        }
                        break;

                    case "tpByCommodity":

                        if (!m_IsRefreshed["tpByCommodity"])
                        { 
                            Refresh_ByCommodity();                        

                            m_IsRefreshed["tpByCommodity"] = true;
                        }
                        break;

                    case "tpStationToStation":
                        if (!m_IsRefreshed["tpStationToStation"])
                        { 
                            loadStationCommoditiesFromComboBoxes();

                            m_GUIInterface.loadSetting(scStationToStation_1);
                            m_GUIInterface.loadSetting(scStationToStation_2);

                            m_IsRefreshed["tpStationToStation"] = true;
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing current data view", ex);
            }
        }

        /// <summary>
        /// if the active tab ist switched we have to check if on the new tab
        /// are still old data to be refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPriceSubTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                refreshPriceView(true);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error after changing active tabindex");
            }
        }

        /// <summary>
        /// external call to force refresh all data
        /// </summary>
        public void RefreshData()
        {
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(RefreshData));
                else
                {
                    try 
	                {	        
                        setFilterHasChanged(true);                
                        ActivateFilterSettings();
	                }
	                catch (Exception ex)
	                {
		                cErr.processError(ex, "Error while refresing data (inline)");
	                }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing data (outline)", ex);
            }
        }

        /// <summary>
        /// external call to signal this tab "data has changed"
        /// </summary>
        public void SignalizeChangedData()
        {
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(SignalizeChangedData));
                else
                {
                    try 
	                {	        
                        setFilterHasChanged(true);                
	                }
	                catch (Exception ex)
	                {
		                cErr.processError(ex, "Error while signalizing changed data (inline)");
	                }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while signalizing changed data (outline)", ex);
            }
        }

        /// <summary>
        /// reorder the entrys of the comboboxes
        /// </summary>
        private void orderComboBoxes()
        {
            String Sorting;
            try
            {
                Sorting = Program.DBCon.getIniValue(DB_GROUPNAME, "OrderOfEntrys", "systemname", false);

                switch (Sorting)
                {
                    case "systemname":
                        cmbStation1.DisplayMember   = "StationSystem";
                        cmbStation2.DisplayMember   = "StationSystem";
                        cmbByStation.DisplayMember  = "StationSystem";

                        ((BindingSource)(cmbStation1.DataSource)).Sort = "SystemName";
                        break;
                    case "stationname":
                        cmbStation1.DisplayMember   = "SystemStation";
                        cmbStation2.DisplayMember   = "SystemStation";
                        cmbByStation.DisplayMember  = "SystemStation";

                        ((BindingSource)(cmbStation1.DataSource)).Sort = "StationName";
                        break;
                    case "distance":
                        cmbStation1.DisplayMember   = "SystemDistance";
                        cmbStation2.DisplayMember   = "SystemDistance";
                        cmbByStation.DisplayMember  = "SystemDistance";

                        ((BindingSource)(cmbStation1.DataSource)).Sort = "Distance";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sorting comboboxes", ex);
            }
        }

        /// <summary>
        /// starts a recalculation of the base data
        /// (filtering out the station which defined by the filter settings)
        /// </summary>
        private void createNewBaseView()
        {
            Object Distance = null;
            Object DistanceToStar = null;
            Object minLandingPadSize = null;
            Object locationType = null;
            Cursor oldCursor =  this.Cursor;
            String sqlString;
            DataTable Data;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if(cbOnlyStationsWithin.Checked)                 
                    Distance = Int32.Parse(cmbSystemLightYears.Text);

                if(cbMaxDistanceToStar.Checked)                 
                    DistanceToStar = Int32.Parse(cmbStationLightSeconds.Text);

                if(cbMinLandingPadSize.Checked)                 
                    minLandingPadSize = cmbMinLandingPadSize.Text;
                
                if(cbLocation.Checked)                 
                    locationType = cmbLocation.Text;

                // get the id of the selected "base system"
                if(cmbSystemBase.Text.Equals(CURRENT_SYSTEM, StringComparison.InvariantCultureIgnoreCase))
                    sqlString = "select ID from tbSystems where Systemname = " + DBConnector.SQLAEscape(Program.actualCondition.System);
                else
                    sqlString = "select ID from tbSystems where Systemname = " + DBConnector.SQLAEscape(cmbSystemBase.Text);

                Data = new DataTable();
                Program.DBCon.Execute(sqlString, Data);

                if ((Data.Rows.Count > 0) && (cmbSystemBase.Text.Equals(CURRENT_SYSTEM, StringComparison.InvariantCultureIgnoreCase)))
                {
                    this.cmbSystemBase.TextUpdate -=  cmbSystemBase_TextUpdate;
                    cmbSystemBase.Text             = CURRENT_SYSTEM;
                    this.cmbSystemBase.TextUpdate +=  cmbSystemBase_TextUpdate;

                    sqlString = "select ID from tbSystems where Systemname = " + DBConnector.SQLAEscape(Program.actualCondition.System);
                    Program.DBCon.Execute(sqlString, Data);
                }

                if (Data.Rows.Count > 0)
                {
                    Int32 SystemID;
                    Program.enVisitedFilter VFilter;
                    SystemID = (Int32)Data.Rows[0]["ID"];

                    VFilter = (Program.enVisitedFilter)Program.DBCon.getIniValue<Int32>(IBE.IBESettingsView.DB_GROUPNAME,
                                                                                        "VisitedFilter",
                                                                                        ((Int32)Program.enVisitedFilter.showOnlyVistedSystems).ToString(),
                                                                                        false);

                    m_DataSource.createFilteredTable(SystemID, Distance, DistanceToStar, minLandingPadSize, VFilter, locationType);

                    Int32 StationCount;
                    Int32 SystemCount;
                    m_DataSource.getFilteredSystemAndStationCount(out StationCount, out SystemCount);

                    lblSystemsFound.Text = SystemCount.ToString();
                    lblStationsFound.Text = StationCount.ToString();

                    sqlString = "select Sy.ID As SystemID, Sy.SystemName, St.ID As StationID, St.StationName," +
                                "       concat(St.StationName, '    -   ', Sy.SystemName,  '     (', Round(Distance,1), ' ly)') As StationSystem," +
                                "       concat(Sy.SystemName,  '    -   ', St.StationName, '     (', Round(Distance,1), ' ly)') As SystemStation," +
                                "       concat(Sy.SystemName,  '    -   ', St.StationName, '     (', Round(Distance,1), ' ly)') As SystemDistance," +
                                "       Fs.Distance" +
                                " from tmFilteredStations Fs, tbSystems Sy, tbStations St" +
                                " where FS.Station_ID = St.ID" +
                                " and   St.System_ID  = Sy.ID;";


                    Program.DBCon.Execute(sqlString, m_DGVTables[cmbByStation.Name]);
                
                    if(cmbStation1.ValueMember == "")
                    { 
                        // prepare functional settings of the comboboxes
                        // (earlyier not possible because the columns are not existing at the beginning)
                        cmbStation1.DisplayMember       = "StationSystem";
                        cmbStation1.ValueMember         = "StationID";
                        cmbStation2.DisplayMember       = "StationSystem";
                        cmbStation2.ValueMember         = "StationID";
                        cmbByStation.DisplayMember      = "StationSystem";
                        cmbByStation.ValueMember        = "StationID";
                    }

                    orderComboBoxes();

                    if(cbFixedStation.Checked)
                        cmbStation1.SelectedValue = m_DataSource.FixedStation;
                }
                
                this.Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                this.Cursor = oldCursor;
                throw new Exception("Error while starting to create a new baseview", ex);
            }
        }

        /// <summary>
        /// saves new splitter position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Splittercontainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            try
            {
                if(m_GUIInterface != null)
                    m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while saving new splitter position");
            }
        }

        /// <summary>
        /// loads the systems in the BaseSystem combobox
        /// </summary>
        private void loadSystemsForBaseSystem()
        {
            Program.enVisitedFilter VFilter;
            String cText;
            try
            {
                cText = cmbSystemBase.Text;
                VFilter = (Program.enVisitedFilter)Program.DBCon.getIniValue<Int32>(IBE.IBESettingsView.DB_GROUPNAME, 
                                                                                    "VisitedFilter", 
                                                                                    ((Int32)Program.enVisitedFilter.showOnlyVistedSystems).ToString(),
                                                                                    false);

                //m_DataSource.loadSystems(m_DGVTables[cmbSystemBase.Name], VFilter);/

                cmbSystemBase.SuspendLayout();
                cmbSystemBase.BeginUpdate();

                m_DataSource.LoadSystemsForBaseComboBox(cText, m_DGVTables[cmbSystemBase.Name], VFilter);

                if(cmbSystemBase.ValueMember == "")
                {
                    cmbSystemBase.DisplayMember     = "SystemName";
                    cmbSystemBase.ValueMember       = "SystemID";
                }

                cmbSystemBase.DroppedDown = true;

                cmbSystemBase.Text = cText;
                cmbSystemBase.SelectionStart = cText.Length;

                cmbSystemBase.ResumeLayout();
                cmbSystemBase.EndUpdate();



                //m_BindingSources[cmbSystemBase.Name].Sort = "SystemName";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the list of all commodities for the combobox", ex);
            }
        }

        /// <summary>
        /// when editing we try to load the matching systems into the Combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSystemBase_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                loadSystemsForBaseSystem();
                SignalizeChangedData();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbSystemBase_TextChanged");
            }
        }

        /// <summary>
        /// "Location to Star Distance" enabled/disabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    if (((CheckBox)sender).Equals(cbMaxTripDistance))
                    {
                        cmdRoundTripCaclulation.ForeColor = Program.Colors.Marked_ForeColor;
                        cmdRoundTripCaclulation.BackColor = Program.Colors.Marked_BackColor;
                    }
                    else
                    {
                        setFilterHasChanged(true);
                    }
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbMaxDistanceToStar_CheckedChanged");
            }
        }

        private void rbOrderBy_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    setFilterHasChanged(true);    
                    orderComboBoxes();
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in rbOrderByStation_CheckedChanged");
            }
        }

        private void txtOrderByAmount_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((TextBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        setFilterHasChanged(true);                
                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbOrderByAmount_CheckedChanged");
            }
        }

        private void txtOrderByAmount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(((TextBoxInt32)sender).checkValue())
                        if(m_GUIInterface.saveSetting(sender))
                        {
                            setFilterHasChanged(true);                
                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtOrderByAmount_KeyDown");
            }
        }

        private void cmbSystemLightYears_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                    if(((ComboBoxInt32)sender).checkValue())
                        if(m_GUIInterface.saveSetting(sender) && cbOnlyStationsWithin.Checked)
                        {
                            setFilterHasChanged(true);                
                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbSystemLightYears_KeyDown");
            }
        }

        private void cmbSystemLightYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender) && cbOnlyStationsWithin.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbSystemLightYears_SelectedIndexChanged");
            }
        }

        private void cmbSystemLightYears_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender) && cbOnlyStationsWithin.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbSystemLightYears_Leave");
            }
        }

        private void cmdStationLightSeconds_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender) && cbMaxDistanceToStar.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdStationLightSeconds_SelectedIndexChanged");
            }
        }

        private void cmdStationLightSeconds_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender) && cbMaxDistanceToStar.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdStationLightSeconds_Leave");
            }
        }

        private void cmdStationLightSeconds_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(((ComboBoxInt32)sender).checkValue())
                        if(m_GUIInterface.saveSetting(sender) && cbMaxDistanceToStar.Checked)
                        {
                            setFilterHasChanged(true);                
                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdStationLightSeconds_KeyDown");
            }
        }

        private void cmbMinLandingPadSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(m_GUIInterface.saveSetting(sender) && cbMinLandingPadSize.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbMinLandingPadSize_KeyDown");
            }
        }

        private void cmbMinLandingPadSize_Leave(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender) && cbMinLandingPadSize.Checked)
                {
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbMinLandingPadSize_Leave");
            }
        }

        private void cmbMinLandingPadSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender) && cbMinLandingPadSize.Checked)
                {
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbMinLandingPadSize_SelectedIndexChanged");
            }
        }

        private void cmbMaxTripDistance_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                    if(((ComboBoxInt32)sender).checkValue())
                        if(m_GUIInterface.saveSetting(sender) && cbMaxTripDistance.Checked)
                        {
                            setFilterHasChanged(true);                
                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbMaxTripDistance_KeyDown");
            }
        }

        private void cmbMaxTripDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender) && cbMaxTripDistance.Checked)
                    {
                        cmdRoundTripCaclulation.ForeColor   = Program.Colors.Marked_ForeColor;
                        cmdRoundTripCaclulation.BackColor   = Program.Colors.Marked_BackColor;
                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbMaxTripDistance_SelectedIndexChanged");
            }
        }

        private void cmbMaxTripDistance_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender) && cbMaxTripDistance.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbMaxTripDistance_Leave");
            }
        }

        private void cmbLocation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(m_GUIInterface.saveSetting(sender) && cbLocation.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbLocation_KeyDown");
            }
        }

        private void cmbLocation_Leave(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender) && cbLocation.Checked)
                {
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbLocation_Leave");
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender) && cbLocation.Checked)
                {
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbLocation_SelectedIndexChanged");
            }
        }

        private void cbShowDiagramAllCommodities_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                scAllCommodities_1.Panel2Collapsed = !cbShowDiagramAllCommodities.Checked;
                m_GUIInterface.saveSetting(sender);

                if(!scAllCommodities_1.Panel2Collapsed)
                { 
                    m_GUIInterface.loadSetting(scAllCommodities_1);
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while switching diagram view (all commodities)");
            }
        }

        private void cbOnlyTradedCommodities_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    setFilterHasChanged(true);
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbMinLandingPadSize_CheckedChanged");
            }
        }

#endregion

#region special : all commodities tab

        /// <summary>
        /// change of sorting of allcommodities-tab is requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_ColumnSorted(object sender, Enums_and_Utility_Classes.DataGridViewExt.SortedEventArgs e)
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
                cErr.processError(ex, "Error while changing the sort order (all commodities)");
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
        /// refreshes the AllCommodities grid
        /// </summary>
        private void Refresh_AllCommodities()
        {
            try
            {
                if (cmbByStation.SelectedValue != null)
                    m_DataSource.getPriceExtremum(m_DGVTables[dgvAllCommodities.Name], cbOnlyTradedCommodities.Checked);
                else
                    m_DGVTables[dgvAllCommodities.Name].Clear();

                sortDataGridView(dgvAllCommodities);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing 'AllCommodities'", ex);
            }
        }


#endregion

#region special : Location to Location tab

        /// <summary>
        /// calculation of the roundtrip is wanted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRoundTripCaclulation_Click(object sender, EventArgs e)
        {
            BindingSource bs;

            try
            {
                //m_DataSource.CommoditiesReturn.Clear();
                //m_DataSource.CommoditiesSend.Clear();


                //m_DataSource.CommoditiesReturn.Add(41);
                //m_DataSource.CommoditiesReturn.Add(43);
                //m_DataSource.CommoditiesReturn.Add(48);
                ////m_DataSource.CommoditiesSend.Add(61);

                SetButtons(false);

                if (Program.actualCondition.System == "")
                    MessageBox.Show("Current system is unknown.", "Can't calculate...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    SetComboBoxEventsActive(false);

                    ActivateFilterSettings();

                    bs              = new BindingSource(); 
                    bs.DataSource   = m_DataSource.calculateTradingRoutes();        

                    dgvStationToStationRoutes.AutoGenerateColumns = false;
                    dgvStationToStationRoutes.DataSource          = bs;

                    if(dgvStationToStationRoutes.RowCount > 0)
                    {
                        //dgvStationToStationRoutes.Rows[0].Selected  = true;
                        if(dgvStationToStationRoutes.CurrentCell.Equals(dgvStationToStationRoutes.Rows[0].Cells[1]))
                            setFromToComboBoxesFromRoutingRow(0);
                        else
                            dgvStationToStationRoutes.CurrentCell       = dgvStationToStationRoutes.Rows[0].Cells[1]; 
                    }
                
                    loadStationCommoditiesFromComboBoxes();

                    m_IsRefreshed["tpStationToStation"] = true;

                    cmdRoundTripCaclulation.ForeColor   = Program.Colors.Default_ForeColor;
                    cmdRoundTripCaclulation.BackColor   = Program.Colors.Default_BackColor;

                    SetComboBoxEventsActive(true);

                    sortDataGridView(dgvStationToStationRoutes);
                }

                SetButtons(true);

            }
            catch (Exception ex)
            {
                SetButtons(true);
                cErr.processError(ex, "Error while starting recalculation of the best profit route");
            }
        }

        /// <summary>
        /// load the stations from the new row into the comboboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStationToStationRoutes_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                setFromToComboBoxesFromRoutingRow(e.RowIndex);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvStationToStationRoutes_RowEnter");
            }
        }

        /// <summary>
        /// presets the From- and To- Combobox with the stations from the given
        /// rownumber of the Routes-datagrid
        /// </summary>
        /// <param name="CurrentRow"></param>
        private void setFromToComboBoxesFromRoutingRow(Int32 CurrentRow)
        {
            int? Station1 = null;
            int? Station2 = null;

            try
            {
                    Station1 = (int?)dgvStationToStationRoutes.Rows[CurrentRow].Cells["stationID1DataGridViewTextBoxColumn"].Value;
                    Station2 = (int?)dgvStationToStationRoutes.Rows[CurrentRow].Cells["stationID2DataGridViewTextBoxColumn"].Value;

                    cmbStation1.SelectedValue = Station1;
                    cmbStation2.SelectedValue = Station2;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setiing fromto comboboxes", ex);
            }
        }

        /// <summary>
        /// loading the detail data of the 2 selected stations in the current 
        /// "best routes" datagrid
        /// </summary>
        private void loadStationCommoditiesFromComboBoxes()
        {
            dsEliteDB.tmpa_s2s_stationdataDataTable Data = null;
            int? Station1 = null;
            int? Station2 = null;
            List<Int32> commodityFilter = null;

            try
            {
                UpdateFixedStation();

                for (int i = 0; i < 2; i++)
                {
                    switch (i)
                    {
                        case 0:
                            try
                            {
                                Station1 = (int?)cmbStation1.SelectedValue;
                            }
                            catch (Exception){
                                Station1 = null;
                            }
                            
                            try
                            {
                                Station2 = (int?)cmbStation2.SelectedValue;
                            }
                            catch (Exception){
                                Station2 = null;
                            }

                            Data            = (dsEliteDB.tmpa_s2s_stationdataDataTable)m_DGVTables[dgvStation1.Name];
                            commodityFilter = m_DataSource.CommoditiesSend;                            

                            break;
                        case 1:
                            try
                            {
                                Station2 = (int?)cmbStation1.SelectedValue;
                            }
                            catch (Exception){
                                Station2 = null;
                            }
                            
                            try
                            {
                                Station1 = (int?)cmbStation2.SelectedValue;
                            }
                            catch (Exception){
                                Station1 = null;
                            }

                            Data            = (dsEliteDB.tmpa_s2s_stationdataDataTable)m_DGVTables[dgvStation2.Name];
                            commodityFilter  = m_DataSource.CommoditiesReturn;
                            break;
                    }             
                    
                    m_DataSource.loadBestProfitStationCommodities(Data, Station1, Station2, commodityFilter);

                }

                sortDataGridView(dgvStation1);
                sortDataGridView(dgvStation2);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while viewing the station details for best-profit route", ex);
            }
        }

        /// <summary>
        /// SelectedValueChanged for StationFrom and -To
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStation_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                loadStationCommoditiesFromComboBoxes();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while selecting a new combobox value");
            }
        }

        /// <summary>
        /// (de-)activate the ValueChanged event handler for the comboboxes
        /// </summary>
        /// <param name="Activate"></param>
        private void SetComboBoxEventsActive(Boolean Activate)
        {
            try
            {
                if(Activate)
                {
                    if (m_ActiveCounter == 0)
                    {
                        cmbStation1.SelectedValueChanged    += cmbStation_SelectedValueChanged;
                        cmbStation2.SelectedValueChanged    += cmbStation_SelectedValueChanged;
                        cmbByStation.SelectedValueChanged   += cmbByStation_SelectedValueChanged;
                        cmbByCommodity.SelectedValueChanged += cmbByCommodity_SelectedValueChanged;
                    }
                    m_ActiveCounter++;
                }
                else
                {
                    if(m_ActiveCounter == 1)
                    {
                        cmbStation1.SelectedValueChanged    -= cmbStation_SelectedValueChanged;
                        cmbStation2.SelectedValueChanged    -= cmbStation_SelectedValueChanged;
                        cmbByStation.SelectedValueChanged   -= cmbByStation_SelectedValueChanged;
                        cmbByCommodity.SelectedValueChanged -= cmbByCommodity_SelectedValueChanged;
                    }
                    m_ActiveCounter--;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while switching Combobox-Eventhandler", ex);
            }
        }

        /// <summary>
        /// switching the stations in the comboboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSwitchStations_Click(object sender, EventArgs e)
        {
            int? tempValue;

            try
            {
                SetComboBoxEventsActive(false);

                tempValue                   = (int?)cmbStation1.SelectedValue;
                cmbStation1.SelectedValue   = cmbStation2.SelectedValue;
                cmbStation2.SelectedValue   = tempValue;
        
                loadStationCommoditiesFromComboBoxes();

                SetComboBoxEventsActive(true);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while switching both combobox values");
            }
        }

#endregion

#region special: ByStation tab

        /// <summary>
        /// cmbByStation_SelectedValueChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbByStation_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                Refresh_ByStation();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbByStation_SelectedValueChanged");
            }                
        }

        /// <summary>
        /// refreshes the ByStation grid
        /// </summary>
        private void Refresh_ByStation()
        {
            try
            {
                if(cmbByStation.SelectedValue != null)
                    m_DataSource.loadCommoditiesByStation(m_DGVTables[dgvByStation.Name], (int?)cmbByStation.SelectedValue);
                else
                    m_DGVTables[cmbByStation.Name].Clear();

                sortDataGridView(dgvByStation);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing 'ByStation'", ex);
            }
        }


#endregion

#region special: ByCommodity tab

        /// <summary>
        /// cmbByCommodity_SelectedValueChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbByCommodity_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                Refresh_ByCommodity();            
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while changing selected index");
            }
        }

        /// <summary>
        /// refreshes the ByCommodity grid
        /// </summary>
        private void Refresh_ByCommodity()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(Refresh_ByCommodity));
            else
            {
                try
                {
                    if (cmbByCommodity.SelectedValue != null)
                        m_DataSource.loadStationsByCommodity(m_DGVTables[dgvByCommodity.Name], (int?)cmbByCommodity.SelectedValue);
                    else
                        m_DGVTables[dgvByCommodity.Name].Clear();

                    sortDataGridView(dgvByCommodity);

                }
                catch (Exception ex)
                {
                    throw new Exception("Error while refreshing 'ByCommodity'", ex);
                }
            }
        }

        /// <summary>
        /// refreshes the comboxbox with the current list of all known commodities
        /// </summary>
        private void loadCommoditiesForByCommodity()
        {
            try
            {
                //m_DataSource.loadCommodities(m_DGVTables[cmbByCommodity.Name]);

                if(cmbByCommodity.ValueMember == "")
                {
                    cmbByCommodity.ValueMember      = "id";
                    cmbByCommodity.DisplayMember    = "loccommodity";
                }

                m_BindingSources[cmbByCommodity.Name].Sort = "loccommodity";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the list of all commodities for the combobox", ex);
            }
        }

#endregion

        private void DataGridView_Click(object sender, EventArgs e)
        {
            MouseEventArgs args;
            DataGridView dgv1;
            DataGridView dgv2 = null;
            DataGridView.HitTestInfo hit;

            try
            {
                args   = (MouseEventArgs)e;

                if(args.Button == System.Windows.Forms.MouseButtons.Right)
                { 
                    dgv1   = (DataGridView)sender;
                    hit   = dgv1.HitTest(args.X, args.Y);

                    if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
                    {
                        DataGridViewSettings Tool = new DataGridViewSettings();

                        if(dgv1.Equals(dgvStation1))
                            dgv2 = dgvStation2;
                        else if(dgv1.Equals(dgvStation2))
                            dgv2 = dgvStation1;

                        if(Tool.setVisibility(dgv1) == DialogResult.OK)
                        {
                            m_GUIInterface.saveSetting(dgv1);

                            if(dgv2 != null)
                            { 
                                DataGridViewSettings.CloneSettings(ref dgv1, ref dgv2);
                                m_GUIInterface.saveSetting(dgv2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while changing DataGridView settings");
            }
        }

        private void SplitContainer_Resize(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface != null)
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in SplitContainer_Resize");
            }
        }

        private void dgvStation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow dRow;
            Int32 intValue;

            try
            {
                DataGridViewExt dGrid       = (DataGridViewExt)sender;
                DataGridViewColumn dColumn  =  dGrid.Columns[e.ColumnIndex];

                if (dColumn.DataPropertyName.Equals("Demandlevel", StringComparison.InvariantCultureIgnoreCase) || 
                    dColumn.DataPropertyName.Equals("Supplylevel", StringComparison.InvariantCultureIgnoreCase))
                {
                    dRow    =  dGrid.Rows[e.RowIndex];
                    if (Int32.TryParse(dRow.Cells[dColumn.Name].Value.ToString(), out intValue))
                        e.Value = (String)Program.Data.BaseTableIDToName("economylevel", intValue, "loclevel");
                }
                else if (dColumn.DataPropertyName.StartsWith("Sources_id", StringComparison.InvariantCultureIgnoreCase)  || 
                         dColumn.DataPropertyName.StartsWith("Buy_Sources_id", StringComparison.InvariantCultureIgnoreCase) || 
                         dColumn.DataPropertyName.StartsWith("Sell_Sources_id", StringComparison.InvariantCultureIgnoreCase))
                {
                    dRow    =  dGrid.Rows[e.RowIndex];
                    
                    if (Int32.TryParse(dRow.Cells[dColumn.Name].Value.ToString(), out intValue))
                        e.Value = (String)Program.Data.BaseTableIDToName("source", intValue);
                }

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in dgvStation_CellFormatting");
            }
        }

        private void nudTimeFilterDays_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(m_GUIInterface.saveSetting(sender) && cbTimeFilter.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in nudTimeFilterDays_KeyDown");
            }
        }

        private void nudTimeFilterDays_Leave(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender) && cbTimeFilter.Checked)
                {
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in nudTimeFilterDays_Leave");
            }
        }

        private void nudTimeFilterDays_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender) && cbTimeFilter.Checked)
                {
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in nudTimeFilterDays_ValueChanged");
            }
        }

        private void cbFixedStation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateFixedStation();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cbFixedStation_CheckedChanged");
            }
        }

        private void UpdateFixedStation()
        {
            try
            {
                int? Station1;

                if(cbFixedStation.Checked)
                {
                    try
                    {
                        Station1 = (int?)cmbStation1.SelectedValue;
                    }
                    catch (Exception){
                        Station1 = null;
                    }

                    m_DataSource.FixedStation = Station1;
                }
                else
                    m_DataSource.FixedStation = null;

                cmdRoundTripCaclulation.ForeColor = Program.Colors.Marked_ForeColor;
                cmdRoundTripCaclulation.BackColor = Program.Colors.Marked_BackColor;
            
            }
            catch (Exception ex)
            {
                throw new Exception("Error in cbFixedStation_CheckedChanged", ex);
            }
        }

        private void cmdCommodityFilter_Click(object sender, EventArgs e)
        {
            CommoditySelector cSelector;
            DialogResult dResult = DialogResult.None;
            List<Int32> cList;

            try
            {
                cSelector = new CommoditySelector();

                if(sender.Equals(cmdCommodityFilter1))
                    cList = new List<Int32>(m_DataSource.CommoditiesSend);
                else
                    cList = new List<Int32>(m_DataSource.CommoditiesReturn);

                dResult = cSelector.Start(this.ParentForm, ref cList);

                if(dResult == DialogResult.OK)
                {
                    if(sender.Equals(cmdCommodityFilter1))
                    {
                        m_DataSource.CommoditiesSend.Clear();
                        m_DataSource.CommoditiesSend.AddRange(cList);

                        SetFilterButtonText(cmdCommodityFilter1, cList);
                    }
                    else
                    {
                        m_DataSource.CommoditiesReturn.Clear();
                        m_DataSource.CommoditiesReturn.AddRange(cList);

                        SetFilterButtonText(cmdCommodityFilter2, cList);
                    }

                    cmdRoundTripCaclulation.ForeColor = Program.Colors.Marked_ForeColor;
                    cmdRoundTripCaclulation.BackColor = Program.Colors.Marked_BackColor;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdCommodityFilter_Click");
            }
        }

        private void cmdClearCommodityFilters_Click(object sender, EventArgs e)
        {
            try
            {
                m_DataSource.CommoditiesSend.Clear();
                m_DataSource.CommoditiesReturn.Clear();

                cbFixedStation.Checked = false;

                SetFilterButtonText(cmdCommodityFilter1, m_DataSource.CommoditiesSend);
                SetFilterButtonText(cmdCommodityFilter2, m_DataSource.CommoditiesReturn);

                cmdRoundTripCaclulation.ForeColor = Program.Colors.Marked_ForeColor;
                cmdRoundTripCaclulation.BackColor = Program.Colors.Marked_BackColor;

           }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdClearCommodityFilters_Click");
            }
        }

        private void SetFilterButtonText(Button filterButton, List<Int32> cList)
        {
            if (cList.Count() == 0)
                filterButton.Text = "Buy-Filter : Off";
            else
                filterButton.Text = string.Format("Buy-Filter : {0} Commodities", cList.Count());
        }
    }
}
