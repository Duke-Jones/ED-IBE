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
using System.Reflection;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.MTPriceAnalysis
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

        private PriceAnalysis                           m_DataSource;                   // data object
        private enCLAction                              m_PA_State;                     // current gui state
        private Dictionary<String, DataTable>           m_DGVTables;                 
        private Dictionary<String, BindingSource>       m_BindingSources;

        private Int32                                   m_InitialTopOfGrid;
        private Int32                                   m_InitialTopOfEditGroupBox;

        private Boolean                                 m_CellValueNeededIsRegistered   = false;        // true if the event is already registred
        private Boolean                                 m_FirstRowShown                 = false;        // true after first time shown
        private DBGuiInterface                          m_GUIInterface;
        private Boolean                                 m_RefreshStarted                = true;         // true, if the user started a new filtering

                                                                                            // shows, which tabs already refreshed after a new filtering
        private Dictionary<String, Boolean> m_RefreshState                  = new Dictionary<string,bool>() { {"BaseData",           false},
                                                                                                              {"tpAllCommodities",   false},
                                                                                                              {"tpByStation",        false},
                                                                                                              {"tpByCommodity",      false},
                                                                                                              {"tpStationToStation", false}};

        /// <summary>
        /// Constructor
        /// </summary>
        public tabPriceAnalysis()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
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

                m_PA_State                              = enCLAction.None;

                // preparing datatables 
                m_DGVTables     = new Dictionary<string,DataTable>();
                m_DGVTables.Add(dgvStation1.Name,               new dsEliteDB.tmpa_s2s_stationdataDataTable());
                m_DGVTables.Add(dgvStation2.Name,               new dsEliteDB.tmpa_s2s_stationdataDataTable());
                m_DGVTables.Add(dgvStationToStationRoutes.Name, new dsEliteDB.tmpa_s2s_besttripsDataTable());

                Data = new DataTable();
                m_DGVTables.Add(cmbStation1.Name,   Data);
                m_DGVTables.Add(cmbStation2.Name,   Data);
                m_DGVTables.Add(cmbByStation.Name,  Data);

                // preparing bindingsources
                m_BindingSources    = new Dictionary<String, BindingSource>();
                m_BindingSources.Add(dgvStation1.Name,                  new BindingSource());
                m_BindingSources.Add(dgvStation2.Name,                  new BindingSource());
                m_BindingSources.Add(dgvStationToStationRoutes.Name,    new BindingSource());
                m_BindingSources.Add(cmbStation1.Name,                  new BindingSource());
                m_BindingSources.Add(cmbStation2.Name,                  new BindingSource());
                m_BindingSources.Add(cmbByStation.Name,                 new BindingSource());

                // connect datatables to bindingsources and bindsources to datagrids
                foreach(KeyValuePair<String, BindingSource> currentKVP in m_BindingSources)
                { 
                    // set the DataTable as datasource of the BindingSource
		            currentKVP.Value.DataSource = m_DGVTables[currentKVP.Key];  

                    // set the BindingSource as datasource of the gui object
                    FieldInfo       DGGV_FieldInfo  = this.GetType().GetField(currentKVP.Key, BindingFlags.NonPublic | BindingFlags.Instance);
                    var DGV_Object                  =DGGV_FieldInfo.GetValue(this);
                    if(DGV_Object.GetType().Equals(typeof(DataGridViewExt)))
                        ((DataGridViewExt)DGV_Object).DataSource    = currentKVP.Value;
                    else if(DGV_Object.GetType().Equals(typeof(ComboBox)))
                        ((ComboBox)DGV_Object).DataSource           = currentKVP.Value;
                    else
                        Debug.Print("unknown");
                }

                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME);
                m_GUIInterface.loadAllSettings(this);
                
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

                cmbSystemBase.Items.Add(CURRENT_SYSTEM);
                cmbSystemBase.SelectedIndex = 0;

                createNewBaseView();

                SetComboBoxEventsActive(true);

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
                throw new NotImplementedException();

                // force refresh
                m_DataSource.Retriever.MemoryCache.Clear();
                
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in m_DataSource_DataChanged");
            }
        }

#region global handling of the price tab

        /// <summary>
        /// "Station to Star Distance" enabled/disabled
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
                cErr.showError(ex, "Error in cbMaxDistanceToStar_CheckedChanged");
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
                cErr.showError(ex, "Error in rbOrderByStation_CheckedChanged");
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
                cErr.showError(ex, "Error in cbOrderByAmount_CheckedChanged");
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
                cErr.showError(ex, "Error in txtOrderByAmount_KeyDown");
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
                cErr.showError(ex, "Error in cmbSystemLightYears_KeyDown");
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
                cErr.showError(ex, "Error in cmbSystemLightYears_SelectedIndexChanged");
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
                cErr.showError(ex, "Error in cmbSystemLightYears_Leave");
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
                cErr.showError(ex, "Error in cmdStationLightSeconds_SelectedIndexChanged");
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
                cErr.showError(ex, "Error in cmdStationLightSeconds_Leave");
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
                cErr.showError(ex, "Error in cmdStationLightSeconds_KeyDown");
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
                cErr.showError(ex, "Error in cmbMinLandingPadSize_KeyDown");
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
                cErr.showError(ex, "Error in cmbMinLandingPadSize_Leave");
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
                cErr.showError(ex, "Error in cmbMinLandingPadSize_SelectedIndexChanged");
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
                cErr.showError(ex, "Error in cmbMaxTripDistance_KeyDown");
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
                cErr.showError(ex, "Error in cmbMaxTripDistance_SelectedIndexChanged");
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
                cErr.showError(ex, "Error in cmbMaxTripDistance_Leave");
            }
        }

        private void cbShowDiagramAllCommodities_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                scAllCommodities_1.Panel2Collapsed = !cbShowDiagramAllCommodities.Checked;
                m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while switching diagram view (all commodities)");
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
                cErr.showError(ex, "Error in cbMinLandingPadSize_CheckedChanged");
            }
        }

        /// <summary>
        /// sets the buttons as marked or not
        /// </summary>
        /// <param name="setMarked"></param>
        private void setFilterHasChanged(Boolean setMarked)
        {
            try
            {
                if(setMarked)
                {
                    cmdFilter.ForeColor                     = Program.Colors.Marked_ForeColor;
                    cmdFilter.BackColor                     = Program.Colors.Marked_BackColor;
                                                                                             
                    cmdRoundTripCaclulation.ForeColor       = Program.Colors.Marked_ForeColor;
                    cmdRoundTripCaclulation.BackColor       = Program.Colors.Marked_BackColor;

                    m_RefreshState["BaseData"]              = false;
                    m_RefreshState["tpAllCommodities"]      = false;
                    m_RefreshState["tpByStation"]           = false;
                    m_RefreshState["tpByCommodity"]         = false;
                    m_RefreshState["tpStationToStation"]    = false;

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
                ActivateFilterSettings();
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while starting the filter");
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

                m_RefreshStarted                = true;
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while filtering stations");
            }
        }

        /// <summary>
        /// check if there some data to be refreshed
        /// </summary>
        /// <param name="TabWasChanged"></param>
        private void refreshPriceView(Boolean TabWasChanged = false)
        {
            BindingSource bs;

            try
            {

                if (!m_RefreshState["BaseData"])
                { 
                    SetComboBoxEventsActive(false);
                    createNewBaseView();
                    m_RefreshState["BaseData"] = true;
                    SetComboBoxEventsActive(true);
                }

                switch (tabPriceSubTabs.SelectedTab.Name)
                {
                    case "tpAllCommodities":

                        if (!m_RefreshState["tpAllCommodities"])
                        { 
                            bs              = new BindingSource(); 
                            bs.DataSource   = m_DataSource.getPriceExtremum(cbOnlyTradedCommodities.Checked);

                            dgvAllCommodities.AutoGenerateColumns = false;
                            dgvAllCommodities.DataSource          = bs;
                            sortAllCommodities();

                            m_RefreshState["tpAllCommodities"] = true;
                        }
                        
                        break;

                    case "tpByStation":
                        
                        if (!m_RefreshState["tpByStation"])
                        { 
                        

                            m_RefreshState["tpByStation"] = true;
                        }
                        break;

                    case "tpByCommodity":

                        if (!m_RefreshState["tpByCommodity"])
                        { 
                        

                            m_RefreshState["tpByCommodity"] = true;
                        }
                        break;

                    case "tpStationToStation":
                        // here we do nothing because
                        // recalculation is only by explicit request

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
                refreshPriceView(true);
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error after changing active tabindex");
            }
        }

        // reorder the entrys of the comboboxes
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

                        ((BindingSource)(cmbStation1.DataSource)).Sort = "StationName";
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
            Cursor oldCursor =  this.Cursor;
            String sqlString;
            DataTable Data;
            Int32 SystemID;
            Program.enVisitedFilter VFilter;
            BindingSource bs;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if(cbOnlyStationsWithin.Checked)                 
                    Distance = Int32.Parse(cmbSystemLightYears.Text);

                if(cbMaxDistanceToStar.Checked)                 
                    DistanceToStar = Int32.Parse(cmbStationLightSeconds.Text);

                if(cbMinLandingPadSize.Checked)                 
                    minLandingPadSize = cmbMinLandingPadSize.Text;
                
                // get the id of the selected "base system"
                if(cmbSystemBase.SelectedIndex == 0)
                    sqlString = "select ID from tbSystems where Systemname = " + DBConnector.SQLAString(Program.actualCondition.System);
                else
                    sqlString = "select ID from tbSystems where Systemname = " + DBConnector.SQLAString(cmbSystemBase.Text);
                Data = new DataTable();
                Program.DBCon.Execute(sqlString, Data);
                SystemID = (Int32)Data.Rows[0]["ID"];

                VFilter = (Program.enVisitedFilter)Program.DBCon.getIniValue<Int32>(RegulatedNoise.MTSettings.tabSettings.DB_GROUPNAME, 
                                                                                    "VisitedFilter", 
                                                                                    ((Int32)Program.enVisitedFilter.showOnlyVistedSystems).ToString(),
                                                                                    false);

                m_DataSource.createFilteredTable(SystemID, Distance, DistanceToStar, minLandingPadSize, VFilter);

                Int32 StationCount;
                Int32 SystemCount;
                m_DataSource.getFilteredSystemAndStationCount(out StationCount, out SystemCount);

                lblSystemsFound.Text  = SystemCount.ToString();
                lblStationsFound.Text = StationCount.ToString();

                sqlString = "select Sy.ID As SystemID, Sy.SystemName, St.ID As StationID, St.StationName," + 
                            "       concat(St.StationName, '    -   ', Sy.SystemName,  '     (', Round(Distance,1), ' ly)') As StationSystem," +
                            "       concat(Sy.SystemName,  '    -   ', St.StationName, '     (', Round(Distance,1), ' ly)') As SystemStation," +
                            "       concat(Sy.SystemName,  '    -   ', St.StationName, '     (', Round(Distance,1), ' ly)') As SystemDistance," +
                            "       Fs.Distance" +
                            " from tmFilteredStations Fs, tbSystems Sy, tbStations St" +
                            " where FS.Station_ID = St.ID" +
                            " and   St.System_ID  = Sy.ID;" ;
                

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
                
                this.Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                this.Cursor = oldCursor;
                throw new Exception("Error while starting to create a new baseview", ex);
            }
        }

#endregion

#region special : all commodities tab

        /// <summary>
        /// change of sorting of allcommodities-tab is requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAllCommodities_ColumnSorted(object sender, Enums_and_Utility_Classes.DataGridViewExt.SortedEventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender, e))
                {
                    sortAllCommodities();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while changing the sort order (all commodities)");
            }
        }

        /// <summary>
        /// starts sorting of the all commodities-tab by the setting from the database
        /// </summary>
        private void sortAllCommodities()
        {
            Cursor oldCursor =  this.Cursor;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                m_GUIInterface.loadSetting(dgvAllCommodities);  
                this.Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                this.Cursor = oldCursor;
                throw new Exception("Error while sorting grid (all commodities)", ex);
            }
        }

#endregion

#region special : Station to Station tab

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
                SetComboBoxEventsActive(false);

                ActivateFilterSettings();

                bs              = new BindingSource(); 
                bs.DataSource   = m_DataSource.calculateTradingRoutes();        

                dgvStationToStationRoutes.AutoGenerateColumns = false;
                dgvStationToStationRoutes.DataSource          = bs;

                m_RefreshState["tpStationToStation"] = true;

                cmdRoundTripCaclulation.ForeColor   = Program.Colors.Default_ForeColor;
                cmdRoundTripCaclulation.BackColor   = Program.Colors.Default_BackColor;

                if(dgvStationToStationRoutes.RowCount > 0)
                {
                    //dgvStationToStationRoutes.Rows[0].Selected  = true;
                    if(dgvStationToStationRoutes.CurrentCell.Equals(dgvStationToStationRoutes.Rows[0].Cells[1]))
                        setFromToComboBoxesFromRoutingRow(0);
                    else
                        dgvStationToStationRoutes.CurrentCell       = dgvStationToStationRoutes.Rows[0].Cells[1]; 
                }
                
                loadStationCommoditiesFromComboBoxes();

                SetComboBoxEventsActive(true);

            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while starting recalculation of the best profit route");
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
                cErr.showError(ex, "Error in dgvStationToStationRoutes_RowEnter");
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
            DataTable Data = null;
            int? Station1 = null;
            int? Station2 = null;

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    switch (i)
                    {
                        case 0:
                            Station1 = (int?)cmbStation1.SelectedValue;
                            Station2 = (int?)cmbStation2.SelectedValue;

                            Data     = m_DGVTables[dgvStation1.Name];
                            break;
                        case 1:
                            Station2 = (int?)cmbStation1.SelectedValue;
                            Station1 = (int?)cmbStation2.SelectedValue;

                            Data     = m_DGVTables[dgvStation2.Name];
                            break;
                    }             
                    
                    m_DataSource.loadStationCommodities(Data, Station1, Station2);

                }
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
                cErr.showError(ex, "Error while selecting a new combobox value");
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
                    cmbStation1.SelectedValueChanged += cmbStation_SelectedValueChanged;
                    cmbStation2.SelectedValueChanged += cmbStation_SelectedValueChanged;
                }
                else
                {
                    cmbStation1.SelectedValueChanged -= cmbStation_SelectedValueChanged;
                    cmbStation2.SelectedValueChanged -= cmbStation_SelectedValueChanged;
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
                cErr.showError(ex, "Error while switching both combobox values");
            }
        }

#endregion

    }
}
