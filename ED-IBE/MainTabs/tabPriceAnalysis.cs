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
        private Boolean                                 m_InitDone                      = false;
        private DataGridView                            m_ClickedDGV;
        private MouseEventArgs                          m_ClickedDGVArgs;

        /// <summary>
        /// Constructor
        /// </summary>
        public tabPriceAnalysis()
        {
            InitializeComponent();
            Dock            = DockStyle.Fill;
            this.Name       = "tabPriceAnalysis";
            m_ActiveCounter = 0;

            if (!((Control)this).IsDesignMode())
                ((Control)this).Retheme();
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
                    else if((DGV_Object.GetType().Equals(typeof(ComboBox))) || (DGV_Object.GetType().BaseType.Equals(typeof(ComboBox))))
                        ((ComboBox)DGV_Object).DataSource           = currentKVP.Value;
                    else
                        Debug.Print("unknown");
                }
                cmbStation1.Separator = "-";
                cmbStation1.DropDownWidth = 375;
                cmbStation1.ColumnWidths.AddRange(new List<Int32>() {40,3,40,17});
                cmbStation1.DrawMode  = DrawMode.OwnerDrawFixed;

                cmbStation2.Separator = "-";
                cmbStation2.DropDownWidth = 375;
                cmbStation2.ColumnWidths.AddRange(new List<Int32>() {40,3,40,17});
                cmbStation2.DrawMode  = DrawMode.OwnerDrawFixed;

                cmbByStation.Separator = "-";
                cmbByStation.DropDownWidth = 375;
                cmbByStation.ColumnWidths.AddRange(new List<Int32>() {40,3,40,17});
                cmbByStation.DrawMode  = DrawMode.OwnerDrawFixed;

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

                cmbRoutingType.Items.Clear();
                cmbRoutingType.Items.Add("one way");
                cmbRoutingType.Items.Add("round trip");

                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, new DBConnector(Program.DBCon.ConfigData, true));
                m_GUIInterface.loadAllSettings(this);

                loadCommoditiesForByCommodity();
                //loadSystemsForBaseSystem();

                createNewBaseView();

                SetComboBoxEventsActive(true);

                cmbSystemBase.Text = CURRENT_SYSTEM;

                SetFilterButtonText(cmdCommodityFilter1, m_DataSource.CommoditiesSend);
                SetFilterButtonText(cmdCommodityFilter2, m_DataSource.CommoditiesReturn);

                cmdCommodityFilter2.Enabled = Program.DBCon.getIniValue(tabPriceAnalysis.DB_GROUPNAME, "RoutingType", "round trip", false).Equals("round trip", StringComparison.InvariantCultureIgnoreCase);

                m_InitDone = true;                              

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
                CErr.processError(ex, "Error in m_DataSource_DataChanged");
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
                    Program.Colors.SetColorToObject(cmdFilter, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);

                    Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);

                    m_IsRefreshed[BASE_DATA]                = false;
                    m_IsRefreshed["tpAllCommodities"]       = false;
                    m_IsRefreshed["tpByStation"]            = false;
                    m_IsRefreshed["tpByCommodity"]          = false;
                    m_IsRefreshed["tpStationToStation"]     = false;

                }                                                                        
                else                                                                     
                {                                                           
                    Program.Colors.SetColorToObject(cmdFilter, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor);
                    // commented out: this is doing the button itself 
                    // cmdRoundTripCaclulation.ForeColor   = Program.Colors.GetColor(GUIColors.ColorNames.Default_ForeColor);
                    // cmdRoundTripCaclulation.BackColor   = Program.Colors.GetColor(GUIColors.ColorNames.Default_BackColor);
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
                CErr.processError(ex, "Error while starting the filter");
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
                CErr.processError(ex, "Error while filtering stations");
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
                            loadCommoditiesForByCommodity();
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
                CErr.processError(ex, "Error after changing active tabindex");
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
		                CErr.processError(ex, "Error while refresing data (inline)");
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
		                CErr.processError(ex, "Error while signalizing changed data (inline)");
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
                        cmbStation1.DisplayMembers.Clear();
                        cmbStation1.DisplayMembers.Add("SystemName");
                        cmbStation1.DisplayMembers.Add("<SEP>;-");
                        cmbStation1.DisplayMembers.Add("StationName");
                        cmbStation1.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbStation1.DisplayMember   = "SystemStation";

                        cmbStation2.DisplayMembers.Clear();
                        cmbStation2.DisplayMembers.Add("SystemName");
                        cmbStation2.DisplayMembers.Add("<SEP>;-");
                        cmbStation2.DisplayMembers.Add("StationName");
                        cmbStation2.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbStation2.DisplayMember   = "SystemStation";

                        cmbByStation.DisplayMembers.Clear();
                        cmbByStation.DisplayMembers.Add("SystemName");
                        cmbByStation.DisplayMembers.Add("<SEP>;-");
                        cmbByStation.DisplayMembers.Add("StationName");
                        cmbByStation.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbByStation.DisplayMember   = "SystemStation";

                        ((BindingSource)(cmbStation1.DataSource)).Sort = "time desc, SystemName";
                        break;
                    case "stationname":
                        cmbStation1.DisplayMembers.Clear();
                        cmbStation1.DisplayMembers.Add("StationName");
                        cmbStation1.DisplayMembers.Add("<SEP>;-");
                        cmbStation1.DisplayMembers.Add("SystemName");
                        cmbStation1.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbStation1.DisplayMember   = "StationSystem";

                        cmbStation2.DisplayMembers.Clear();
                        cmbStation2.DisplayMembers.Add("StationName");
                        cmbStation2.DisplayMembers.Add("<SEP>;-");
                        cmbStation2.DisplayMembers.Add("SystemName");
                        cmbStation2.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbStation2.DisplayMember   = "StationSystem";

                        cmbByStation.DisplayMembers.Clear();
                        cmbByStation.DisplayMembers.Add("StationName");
                        cmbByStation.DisplayMembers.Add("<SEP>;-");
                        cmbByStation.DisplayMembers.Add("SystemName");
                        cmbByStation.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbByStation.DisplayMember   = "StationSystem";

                        ((BindingSource)(cmbStation1.DataSource)).Sort = "time desc, StationName";
                        break;
                    case "distance":
                        cmbStation1.DisplayMembers.Clear();
                        cmbStation1.DisplayMembers.Add("SystemName");
                        cmbStation1.DisplayMembers.Add("<SEP>;-");
                        cmbStation1.DisplayMembers.Add("StationName");
                        cmbStation1.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbStation1.DisplayMember   = "SystemStation";

                        cmbStation2.DisplayMembers.Clear();
                        cmbStation2.DisplayMembers.Add("SystemName");
                        cmbStation2.DisplayMembers.Add("<SEP>;-");
                        cmbStation2.DisplayMembers.Add("StationName");
                        cmbStation2.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbStation2.DisplayMember   = "SystemStation";

                        cmbByStation.DisplayMembers.Clear();
                        cmbByStation.DisplayMembers.Add("SystemName");
                        cmbByStation.DisplayMembers.Add("<SEP>;-");
                        cmbByStation.DisplayMembers.Add("StationName");
                        cmbByStation.DisplayMembers.Add("Distance; ({0:f1} ly)");
                        cmbByStation.DisplayMember   = "SystemStation";

                        ((BindingSource)(cmbStation1.DataSource)).Sort = "time desc, Distance";
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
                if((cmbSystemBase.Text == "") || (cmbSystemBase.Text.Equals(CURRENT_SYSTEM, StringComparison.InvariantCultureIgnoreCase)))
                    sqlString = "select ID, x, y, z from tbSystems where Systemname = " + DBConnector.SQLAEscape(Program.actualCondition.System);
                else
                    sqlString = "select ID, x, y, z from tbSystems where Systemname = " + DBConnector.SQLAEscape(cmbSystemBase.Text);

                Data = new DataTable();
                Program.DBCon.Execute(sqlString, Data);

                if ((Data.Rows.Count > 0) && (cmbSystemBase.Text.Equals(CURRENT_SYSTEM, StringComparison.InvariantCultureIgnoreCase)))
                {
                    this.cmbSystemBase.TextUpdate -=  cmbSystemBase_TextUpdate;
                    cmbSystemBase.Text             = CURRENT_SYSTEM;
                    this.cmbSystemBase.TextUpdate +=  cmbSystemBase_TextUpdate;

                    sqlString = "select ID, x, y, z from tbSystems where Systemname = " + DBConnector.SQLAEscape(Program.actualCondition.System);
                    Program.DBCon.Execute(sqlString, Data);
                }

                if (Data.Rows.Count > 0)
                {
                    Int32     SystemID          = (Int32)Data.Rows[0]["ID"];
                    Point3Dbl baseCoodinates    = new Point3Dbl((Double)Data.Rows[0]["x"], (Double)Data.Rows[0]["y"], (Double)Data.Rows[0]["z"]);

                    Program.enVisitedFilter VFilter;

                    VFilter = (Program.enVisitedFilter)Program.DBCon.getIniValue<Int32>(IBE.IBESettingsView.DB_GROUPNAME,
                                                                                        "VisitedFilter",
                                                                                        ((Int32)Program.enVisitedFilter.showAll).ToString(),
                                                                                        false);

                    m_DataSource.createFilteredTable(baseCoodinates, Distance, DistanceToStar, minLandingPadSize, VFilter, locationType);

                    Int32 StationCount;
                    Int32 SystemCount;
                    m_DataSource.getFilteredSystemAndStationCount(out StationCount, out SystemCount);

                    lblSystemsFound.Text = SystemCount.ToString();
                    lblStationsFound.Text = StationCount.ToString();

                    // select the filtered stations
                    sqlString = "select Sy.ID As SystemID, Sy.SystemName, St.ID As StationID, St.StationName," +
                                "       concat(St.StationName, ' - ', Sy.SystemName,  ' (', Round(Distance,1), ' ly)') As StationSystem," +
                                "       concat(Sy.SystemName,  ' - ', St.StationName, ' (', Round(Distance,1), ' ly)') As SystemStation," +
                                "       concat(Sy.SystemName,  ' - ', St.StationName, ' (', Round(Distance,1), ' ly)') As SystemDistance," +
                                "       Fs.Distance, {ts'2000-01-01 00:00:00'} as time" +
                                " from tmFilteredStations Fs, tbSystems Sy, tbStations St" +
                                " where FS.Station_ID = St.ID" +
                                " and   St.System_ID  = Sy.ID;";


                    Program.DBCon.Execute(sqlString, m_DGVTables[cmbByStation.Name]);

                    if(cbLastVisitedStations.Checked && Int32.Parse(txtLastVisitedStations.Text) > 0)
                    {
                        DataTable currentSystem = new DataTable();
                        Int32 amountLastStations = Int32.Parse(txtLastVisitedStations.Text);

                        // select the 'n' previously visited stations (unattached from the filter settings)
                        sqlString = String.Format("select Sy.ID As SystemID, Sy.SystemName, St.ID As StationID, St.StationName," +
                                                  " concat(St.StationName, ' - ', Sy.SystemName,  ' (', Round(SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)),1), ' ly)') As StationSystem," +
                                                  " concat(Sy.SystemName,  ' - ', St.StationName, ' (', Round(SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)),1), ' ly)') As SystemStation," +
                                                  " concat(Sy.SystemName,  ' - ', St.StationName, ' (', Round(SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)),1), ' ly)') As SystemDistance," +
                                                  " Round(SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)),1) As Distance, V.time" +
                                                  " from tbSystems Sy, tbStations St, tbVisitedStations V" +
                                                  " where St.System_ID  = Sy.ID" +
                                                  " and   St.id         = V.Station_Id" +
                                                  " and   V.VisitType   = {4}" +
	                                              " order by V.time desc" +
 	                                              " limit {3}", 
                                                  DBConnector.SQLDecimal(baseCoodinates.X.Value), 
                                                  DBConnector.SQLDecimal(baseCoodinates.Y.Value), 
                                                  DBConnector.SQLDecimal(baseCoodinates.Z.Value),
                                                  amountLastStations, 
                                                  (sbyte)EliteDBIO.VisitedType.RealVisit);

                        DataTable tmpTable = new DataTable();
                        Program.DBCon.Execute(sqlString, tmpTable);

                        m_DGVTables[cmbByStation.Name].Merge(tmpTable, true, MissingSchemaAction.Ignore);

                        DataRow separatorRow = m_DGVTables[cmbByStation.Name].NewRow();
                        separatorRow["SystemID"]        = "0";
                        separatorRow["SystemName"]      = "----------";
                        separatorRow["StationID"]       = "0";
                        separatorRow["StationName"]     = "----------";
                        separatorRow["StationSystem"]   = "----------";
                        separatorRow["SystemStation"]   = "----------";
                        separatorRow["SystemDistance"]  = "----------";
                        separatorRow["Distance"]        = "0";
                        separatorRow["time"]            = new DateTime(2002, 01, 01);

                        m_DGVTables[cmbByStation.Name].Rows.Add(separatorRow);
                    }

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
                    {
                        if((m_DataSource.FixedStation == 0) || ((m_DGVTables[cmbStation1.Name].Select("StationID = " +  m_DataSource.FixedStation)).GetUpperBound(0) < 0))
                        { 
                            cbFixedStation.Checked = false;

                            if(m_DataSource.FixedStation != 0)
                                m_DataSource.FixedStation = 0;
                        }
                        else
                        { 
                            cmbStation1.SelectedValue = m_DataSource.FixedStation;
                        }
                    }
                        
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

                    //SetResultpanelPosition();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while saving new splitter position");
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
                                                                                    ((Int32)Program.enVisitedFilter.showAll).ToString(),
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
                CErr.processError(ex, "Error in cmbSystemBase_TextChanged");
            }
        }

        /// <summary>
        /// CheckBox_CheckedChanged
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
                        Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);
                    }
                    else if (((CheckBox)sender).Equals(cbFixedStation) && cbFixedStation.Checked && m_InitDone)
                    {             
                        cbFixedCurrentStation.Checked = false;

                        UpdateFixedStation();

                        Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);
                    }
                    else if (((CheckBox)sender).Equals(cbFixedCurrentStation) && cbFixedCurrentStation.Checked && m_InitDone)
                    {                                       
                        cbFixedStation.Checked = false;

                        UpdateFixedStation();

                        Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);
                    }
                    else
                    {
                        setFilterHasChanged(true);
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in CheckBox_CheckedChanged");
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
                CErr.processError(ex, "Error in rbOrderByStation_CheckedChanged");
            }
        }

        private void txtLastVisitedStations_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((TextBoxInt32)sender).checkValue())
                {
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        setFilterHasChanged(true);                
                    }
                }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cbOrderByAmount_CheckedChanged");
            }
        }

        private void txtLastVisitedStations_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(((TextBoxInt32)sender).checkValue())
                    {
                        if(m_GUIInterface.saveSetting(sender))
                        {
                            setFilterHasChanged(true);                
                        }
                    }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in txtOrderByAmount_KeyDown");
            }
        }

        private void cmbSystemLightYears_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                    if(((ComboBoxInt32)sender).checkValue())
                    {
                        if(m_GUIInterface.saveSetting(sender) && cbOnlyStationsWithin.Checked)
                        {
                            setFilterHasChanged(true);                
                        }
                    }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbSystemLightYears_KeyDown");
            }
        }

        private void cmbSystemLightYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                {
                    if(m_GUIInterface.saveSetting(sender) && cbOnlyStationsWithin.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbSystemLightYears_SelectedIndexChanged");
            }
        }

        private void cmbSystemLightYears_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                {
                    if(m_GUIInterface.saveSetting(sender) && cbOnlyStationsWithin.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbSystemLightYears_Leave");
            }
        }

        private void cmbKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                if(m_GUIInterface.saveSetting(sender))
                {
                    cmdCommodityFilter2.Enabled = Program.DBCon.getIniValue(tabPriceAnalysis.DB_GROUPNAME, "RoutingType", "round trip", false).Equals("round trip", StringComparison.InvariantCultureIgnoreCase);
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbKeyDown");
            }
        }

        private void cmbSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    cmdCommodityFilter2.Enabled = Program.DBCon.getIniValue(tabPriceAnalysis.DB_GROUPNAME, "RoutingType", "round trip", false).Equals("round trip", StringComparison.InvariantCultureIgnoreCase);
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbSelectedIndexChanged");
            }
        }

        private void cmbLeave(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    cmdCommodityFilter2.Enabled = Program.DBCon.getIniValue(tabPriceAnalysis.DB_GROUPNAME, "RoutingType", "round trip", false).Equals("round trip", StringComparison.InvariantCultureIgnoreCase);
                    setFilterHasChanged(true);                
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbLeave");
            }
        }

        private void cmdStationLightSeconds_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                {
                    if(m_GUIInterface.saveSetting(sender) && cbMaxDistanceToStar.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdStationLightSeconds_SelectedIndexChanged");
            }
        }

        private void cmdStationLightSeconds_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                {
                    if(m_GUIInterface.saveSetting(sender) && cbMaxDistanceToStar.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdStationLightSeconds_Leave");
            }
        }

        private void cmdStationLightSeconds_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(((ComboBoxInt32)sender).checkValue())
                    {
                        if(m_GUIInterface.saveSetting(sender) && cbMaxDistanceToStar.Checked)
                        {
                            setFilterHasChanged(true);                
                        }
                    }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdStationLightSeconds_KeyDown");
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
                CErr.processError(ex, "Error in cmbMinLandingPadSize_KeyDown");
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
                CErr.processError(ex, "Error in cmbMinLandingPadSize_Leave");
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
                CErr.processError(ex, "Error in cmbMinLandingPadSize_SelectedIndexChanged");
            }
        }

        private void cmbMaxTripDistance_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                    if(((ComboBoxInt32)sender).checkValue())
                    {
                        if(m_GUIInterface.saveSetting(sender) && cbMaxTripDistance.Checked)
                        {
                            setFilterHasChanged(true);                
                        }
                    }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbMaxTripDistance_KeyDown");
            }
        }

        private void cmbMaxTripDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(((ComboBoxInt32)sender).checkValue())
                {
                    if(m_GUIInterface.saveSetting(sender) && cbMaxTripDistance.Checked)
                    {
                        Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);
                    }
                }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbMaxTripDistance_SelectedIndexChanged");
            }
        }

        private void cmbMaxTripDistance_Leave(object sender, EventArgs e)
        {
            try
            {       
                if(((ComboBoxInt32)sender).checkValue())
                {
                    if(m_GUIInterface.saveSetting(sender) && cbMaxTripDistance.Checked)
                    {
                        setFilterHasChanged(true);                
                    }
                }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbMaxTripDistance_Leave");
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
                CErr.processError(ex, "Error in cmbLocation_KeyDown");
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
                CErr.processError(ex, "Error in cmbLocation_Leave");
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
                CErr.processError(ex, "Error in cmbLocation_SelectedIndexChanged");
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
                CErr.processError(ex, "Error while switching diagram view (all commodities)");
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
                CErr.processError(ex, "Error in cbMinLandingPadSize_CheckedChanged");
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
                CErr.processError(ex, "Error while changing the sort order (all commodities)");
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

                    Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor);

                    SetComboBoxEventsActive(true);

                    sortDataGridView(dgvStationToStationRoutes);

                    scStationToStation_2.Panel2Collapsed = (!Program.DBCon.getIniValue(tabPriceAnalysis.DB_GROUPNAME, "RoutingType", "round trip", false).Equals("round trip", StringComparison.InvariantCultureIgnoreCase));
                }

                SetButtons(true);

            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while starting recalculation of the best profit route");
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
                CErr.processError(ex, "Error in dgvStationToStationRoutes_RowEnter");
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
                CErr.processError(ex, "Error while selecting a new combobox value");
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
                    Debug.Print("Activate +" + m_ActiveCounter);
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
                    Debug.Print("Activate -" + m_ActiveCounter);
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
                CErr.processError(ex, "Error while switching both combobox values");
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
                CErr.processError(ex, "Error in cmbByStation_SelectedValueChanged");
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
                CErr.processError(ex, "Error while changing selected index");
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
            DataGridView dgv2 = null;
            DataGridView.HitTestInfo hit;

            try
            {
                m_ClickedDGVArgs   = (MouseEventArgs)e;

                if(m_ClickedDGVArgs.Button == System.Windows.Forms.MouseButtons.Right)
                { 
                    m_ClickedDGV   = (DataGridView)sender;
                    hit   = m_ClickedDGV.HitTest(m_ClickedDGVArgs.X, m_ClickedDGVArgs.Y);
                    
                    if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
                    {
                        DataGridViewSettings Tool = new DataGridViewSettings();

                        if(m_ClickedDGV.Equals(dgvStation1))
                            dgv2 = dgvStation2;
                        else if(m_ClickedDGV.Equals(dgvStation2))
                            dgv2 = dgvStation1;

                        if(Tool.setVisibility(m_ClickedDGV) == DialogResult.OK)
                        {
                            m_GUIInterface.saveSetting(m_ClickedDGV);

                            if(dgv2 != null)
                            { 
                                DataGridViewSettings.CloneSettings(ref m_ClickedDGV, ref dgv2);
                                m_GUIInterface.saveSetting(dgv2);
                            }
                        }
                    }
                    else if (hit.Type == DataGridViewHitTestType.Cell)
                    {
                        if(m_ClickedDGV.Equals(dgvStationToStationRoutes))
                        {
                            contextMenuStrip2.Show(m_ClickedDGV, m_ClickedDGVArgs.Location);
                        }
                        else if(m_ClickedDGV.Equals(dgvAllCommodities))
                        {
                            contextMenuStrip3.Show(m_ClickedDGV, m_ClickedDGVArgs.Location);
                        }
                        else
                        {
                            contextMenuStrip1.Show(m_ClickedDGV, m_ClickedDGVArgs.Location);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while changing DataGridView settings");
            }
        }

        private void SplitContainer_Resize(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface != null)
                    m_GUIInterface.loadSetting(sender);

                //SetResultpanelPosition();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in SplitContainer_Resize");
            }
        }

        ///// <summary>
        ///// set the Position and size of the panel to overlay the splitter 
        ///// </summary>
        //private void SetResultpanelPosition()
        //{
        //    try
        //    {
        //        var spLocation = scStationToStation_2.SplitterRectangle.Location;
        //        spLocation.Offset(scStationToStation_1.Location);
        //        spLocation.Offset(scStationToStation_2.Location);
        //        spLocation.Offset(1, 2);
        //        paResultDetail.Location = spLocation;

        //        var spRect = scStationToStation_2.SplitterRectangle.Size;
        //        spRect.Width -= 10;
        //        spRect.Height -= 2;
        //        paResultDetail.Size = spRect;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while repositioning the result panel", ex);
        //    }
        //}

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
                CErr.processError(ex, "Error in dgvStation_CellFormatting");
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
                CErr.processError(ex, "Error in nudTimeFilterDays_KeyDown");
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
                CErr.processError(ex, "Error in nudTimeFilterDays_Leave");
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
                CErr.processError(ex, "Error in nudTimeFilterDays_ValueChanged");
            }
        }

        public delegate void Int32Delegate(Int32 value);

        public void UpdateFixedStation()
        {
            try
            {
                int? Station1;

                if(cbFixedStation.Checked)
                {
                    try
                    {
                        Station1 = (int?)cmbStation1.SelectedValue;
                        m_DataSource.FixedStation = Station1.Value;
                    }
                    catch (Exception)
                    {
                        m_DataSource.FixedStation = 0;
                        cbFixedStation.Checked = false;
                    }
                }
                else if(cbFixedCurrentStation.Checked)
                {
                    try
                    {
                        if(Program.actualCondition.Station_ID != null)
                        {
                            Station1 = (int?)Program.actualCondition.Station_ID;
                            cmbStation1.Invoke((MethodInvoker)delegate(){cmbStation1.SelectedValue =  Station1;});
                            m_DataSource.FixedStation = Station1.Value;
                        }
                        else
                        {
                            Station1 = (int?)cmbStation1.SelectedValue;
                            m_DataSource.FixedStation = Station1.Value;
                        }
                    }
                    catch (Exception)
                    {
                        m_DataSource.FixedStation = 0;
                        cbFixedStation.Checked = false;
                    }
                }
                else
                    m_DataSource.FixedStation = 0;

                Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);
            
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
                        m_DataSource.CommoditiesSend = cList;
                    else
                        m_DataSource.CommoditiesReturn = cList;

                    SetFilterButtonText((Button)sender, cList);

                    Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdCommodityFilter_Click");
            }
        }

        private void cmdClearCommodityFilters_Click(object sender, EventArgs e)
        {
            try
            {
                m_DataSource.CommoditiesSend   = new List<Int32>();
                m_DataSource.CommoditiesReturn = new List<Int32>();

                cbFixedStation.Checked = false;

                SetFilterButtonText(cmdCommodityFilter1, m_DataSource.CommoditiesSend);
                SetFilterButtonText(cmdCommodityFilter2, m_DataSource.CommoditiesReturn);

                Program.Colors.SetColorToObject(cmdRoundTripCaclulation, GUIColors.ColorNames.Marked_ForeColor, GUIColors.ColorNames.Marked_BackColor);
           }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdClearCommodityFilters_Click");
            }
        }

        private void SetFilterButtonText(Button filterButton, List<Int32> cList)
        {
            if (cList.Count() == 0)
                filterButton.Text = "Buy : No Filter";
            else
                filterButton.Text = string.Format("Buy-Filter : {0} Commodities", cList.Count());
        }

        private void dgvStation_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            SQL.Datasets.dsEliteDB.visystemsandstationsRow[] stationInfo;
            SQL.Datasets.dsEliteDB.tmpa_s2s_stationdataRow commodityInfo;
            Int32 totalProfit = 0;
            DataGridViewExt currentDGV = (DataGridViewExt)sender;
            Label detailStation;
            Label detailStationExt = null;
            Label detailCommodity;
            Label detailProfit;
            Panel detailPanel;
            Int32 intValue = 0;

            try
            {
                if(currentDGV == dgvStation1)
                {
                    detailStation     = lbDetailStation1;
                    detailCommodity   = lbDetailCommodity1;
                    detailProfit      = lbDetailProfit1;     
                    detailPanel       = paStationDetail1;

                    if(!cmbRoutingType.Text.Equals("round trip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // if we don't calculate round trips we must consider 
                        // a manual update of the second station details
                        detailStationExt = lbDetailStation2;
                    }
                }
                else
                {
                    detailStation     = lbDetailStation2;
                    
                    if(cmbRoutingType.Text.Equals("round trip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        detailCommodity   = lbDetailCommodity2;
                        detailProfit      = lbDetailProfit2;
                    }
                    else
                    {
                        detailCommodity           = null;
                        detailProfit              = null;
                        lbDetailCommodity2. Text  = "none";
                        lbDetailProfit2.Text      = "none";
                        detailProfit = null;
                    }
                    detailPanel       = paStationDetail2;
                }


                if((currentDGV.RowCount > 0) && (e.RowIndex >= 0))
                {
                    commodityInfo = (SQL.Datasets.dsEliteDB.tmpa_s2s_stationdataRow)((DataRowView)currentDGV.Rows[e.RowIndex].DataBoundItem).Row;
                    stationInfo   = (SQL.Datasets.dsEliteDB.visystemsandstationsRow[])Program.Data.BaseData.visystemsandstations.Select("StationID=" + commodityInfo.Station_ID);

                    detailStation.Text    = String.Format("{0} / {1}", stationInfo[0].SystemName, stationInfo[0].StationName);

                    if(detailCommodity != null)
                        detailCommodity.Text  = commodityInfo.Commodity;

                    if(detailProfit != null)
                        detailProfit.Text     = commodityInfo.Profit.ToString();

                    if(stationInfo[0].StationID == (Program.actualCondition.Station_ID ?? 0))
                    {
                        Program.Colors.SetColorToObject(detailPanel, GUIColors.ColorNames.Marked_ForeColor1, GUIColors.ColorNames.Marked_BackColor1, true);
                    }
                    else
                    {
                        Program.Colors.SetColorToObject(detailPanel, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor, true);
                    }

                    if((detailStationExt != null) && (detailStationExt.Tag.GetType() == typeof(Int32)))
                    {
                        lbDetailCommodity2. Text  = "none";
                        lbDetailProfit2.Text      = "none";

                        if((Int32)detailStationExt.Tag == (Program.actualCondition.Station_ID ?? 0))
                        {
                            Program.Colors.SetColorToObject(paStationDetail2, GUIColors.ColorNames.Marked_ForeColor1, GUIColors.ColorNames.Marked_BackColor1, true);
                        }
                        else
                        {
                            Program.Colors.SetColorToObject(paStationDetail2, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor, true);
                        }
                    }
               }
                else
                {
                    detailStation.Text    =  "-";
                    detailCommodity.Text  =  "-";

                    if(detailProfit != null)
                        detailProfit.Text     =  "0";

                    Program.Colors.SetColorToObject(detailPanel, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor, true);

                    if((detailStationExt != null))
                        Program.Colors.SetColorToObject(paStationDetail2, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor, true);
                }

                if(Int32.TryParse(lbDetailProfit1.Text, out intValue))
                    totalProfit += intValue;

                if(Int32.TryParse(lbDetailProfit2.Text, out intValue))
                    totalProfit += intValue;

                lbDetailProfitTotal.Text = totalProfit.ToString();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in dgvStation_RowEnter");
            }
        }

        private void RefreshDetailColors()
        {
            SQL.Datasets.dsEliteDB.visystemsandstationsRow[] stationInfo;
            SQL.Datasets.dsEliteDB.tmpa_s2s_stationdataRow commodityInfo;
            DataGridViewExt currentDGV;
            Label detailStation;
            Label detailCommodity;
            Label detailProfit;
            Panel detailPanel;

            try
            {
                for (int i = 0; i < 2; i++)
			    {
                    if(i == 0)
                    {
                        currentDGV        = dgvStation1;
                        detailStation     = lbDetailStation1;
                        detailCommodity   = lbDetailCommodity1;
                        detailProfit      = lbDetailProfit1;     
                        detailPanel       = paStationDetail1;
                    }
                    else
                    {
                        currentDGV        = dgvStation2;
                        detailStation     = lbDetailStation2;
                        detailCommodity   = lbDetailCommodity2;
                        detailProfit      = lbDetailProfit2;
                        detailPanel       = paStationDetail2;
                    }

                    if((currentDGV.CurrentRow != null) && (currentDGV.RowCount > 0))
                    {
                        commodityInfo = (SQL.Datasets.dsEliteDB.tmpa_s2s_stationdataRow)((DataRowView)currentDGV.CurrentRow.DataBoundItem).Row;
                        stationInfo   = (SQL.Datasets.dsEliteDB.visystemsandstationsRow[])Program.Data.BaseData.visystemsandstations.Select("StationID=" + commodityInfo.Station_ID);

                        if(stationInfo[0].StationID == (Program.actualCondition.Station_ID ?? 0))
                        {
                            Program.Colors.SetColorToObject(detailPanel, GUIColors.ColorNames.Marked_ForeColor1, GUIColors.ColorNames.Marked_BackColor1, true);
                        }
                        else
                        {
                            Program.Colors.SetColorToObject(detailPanel, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor, true);
                        }
                   }
                    else
                    {
                        Program.Colors.SetColorToObject(detailPanel, GUIColors.ColorNames.Default_ForeColor, GUIColors.ColorNames.Default_BackColor, true);
                    }
			    }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in dgvStation_RowEnter", ex);
            }
        }

        private void dgvStationToStationRoutes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                { 
                    if(e.ColumnIndex == dgvStationToStationRoutes.Columns["systemName1DataGridViewTextBoxColumn"].Index)
                    {
                        if(Program.actualCondition.System_ID == (int?)dgvStationToStationRoutes.Rows[e.RowIndex].Cells["systemID1DataGridViewTextBoxColumn"].Value)
                        {
                            e.CellStyle.ForeColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_BackColor1);
                            e.CellStyle.BackColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_ForeColor);
                        }
                    }
                    else if(e.ColumnIndex == dgvStationToStationRoutes.Columns["stationName1DataGridViewTextBoxColumn"].Index)
                    {
                        if(Program.actualCondition.Station_ID == (int?)dgvStationToStationRoutes.Rows[e.RowIndex].Cells["stationID1DataGridViewTextBoxColumn"].Value)
                        {
                            e.CellStyle.ForeColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_BackColor1);
                            e.CellStyle.BackColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_ForeColor);
                        }
                    }
                    else if(e.ColumnIndex == dgvStationToStationRoutes.Columns["systemName2DataGridViewTextBoxColumn"].Index)
                    {
                        if(Program.actualCondition.System_ID == (int?)dgvStationToStationRoutes.Rows[e.RowIndex].Cells["systemID2DataGridViewTextBoxColumn"].Value)
                        {
                            e.CellStyle.ForeColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_BackColor1);
                            e.CellStyle.BackColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_ForeColor);
                        }
                    }
                    else if(e.ColumnIndex == dgvStationToStationRoutes.Columns["stationName2DataGridViewTextBoxColumn"].Index)
                    {
                        if(Program.actualCondition.Station_ID == (int?)dgvStationToStationRoutes.Rows[e.RowIndex].Cells["stationID2DataGridViewTextBoxColumn"].Value)
                        {
                            e.CellStyle.ForeColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_BackColor1);
                            e.CellStyle.BackColor = Program.Colors.GetColor(GUIColors.ColorNames.Marked_ForeColor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in dgvStationToStationRoutes_CellPainting");
            }
        }

        internal void RefreshColors()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(RefreshColors));
            }
            else
            {
                dgvStationToStationRoutes.Refresh();
                RefreshDetailColors();
            }
        }

        private void copySystemnameToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView.HitTestInfo hit;
            String resultString = "";
            
            try
            {
                hit = m_ClickedDGV.HitTest(m_ClickedDGVArgs.X, m_ClickedDGVArgs.Y);

                if(m_ClickedDGV.Equals(dgvStation1) || m_ClickedDGV.Equals(dgvStation2))
                {
                    Int32 stationID = ((dsEliteDB.tmpa_s2s_stationdataRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Station_ID;
                    resultString    = Program.Data.GetSystemnameFromStation(stationID);

                }
                else if(m_ClickedDGV.Equals(dgvStationToStationRoutes))
                {
                    if (((((ToolStripMenuItem)sender).Tag).ToString()) == "1")
                        resultString = ((dsEliteDB.tmpa_s2s_besttripsRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).SystemName_1;
                    else
                        resultString = ((dsEliteDB.tmpa_s2s_besttripsRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).SystemName_2;

                }
                else if(m_ClickedDGV.Equals(dgvByStation))
                {
                    Int32 stationID = ((dsEliteDB.tmpa_bystationRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Station_ID;
                    resultString    = Program.Data.GetSystemnameFromStation(stationID);

                }
                else if(m_ClickedDGV.Equals(dgvByCommodity))
                {
                    Int32 stationID = ((dsEliteDB.tmpa_bycommodityRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Station_ID;
                    resultString    = Program.Data.GetSystemnameFromStation(stationID);

                }
                else if(m_ClickedDGV.Equals(dgvAllCommodities))
                {
                    Int32 stationID;

                    if (((((ToolStripMenuItem)sender).Tag).ToString()) == "1")
                        resultString = ((dsEliteDB.tmpa_allcommoditiesRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Buy_System;
                    else
                        resultString = ((dsEliteDB.tmpa_allcommoditiesRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Sell_System;

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

                if(m_ClickedDGV.Equals(dgvStation1) || m_ClickedDGV.Equals(dgvStation2))
                {
                    Int32 stationID = ((dsEliteDB.tmpa_s2s_stationdataRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Station_ID;
                    resultString    = Program.Data.GetStationnameFromStationID(stationID);

                }
                else if(m_ClickedDGV.Equals(dgvStationToStationRoutes))
                {
                    if (((((ToolStripMenuItem)sender).Tag).ToString()) == "1")
                        resultString = ((dsEliteDB.tmpa_s2s_besttripsRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).StationName_1;
                    else
                        resultString = ((dsEliteDB.tmpa_s2s_besttripsRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).StationName_2;

                }
                else if(m_ClickedDGV.Equals(dgvByStation))
                {
                    Int32 stationID = ((dsEliteDB.tmpa_bystationRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Station_ID;
                    resultString    = Program.Data.GetStationnameFromStationID(stationID);

                }
                else if(m_ClickedDGV.Equals(dgvByCommodity))
                {
                    Int32 stationID = ((dsEliteDB.tmpa_bycommodityRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Station_ID;
                    resultString    = Program.Data.GetStationnameFromStationID(stationID);

                }
                else if(m_ClickedDGV.Equals(dgvAllCommodities))
                {
                    Int32 stationID;

                    if (((((ToolStripMenuItem)sender).Tag).ToString()) == "1")
                        resultString = ((dsEliteDB.tmpa_allcommoditiesRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Buy_Station;
                    else
                        resultString = ((dsEliteDB.tmpa_allcommoditiesRow)((DataRowView)m_ClickedDGV.Rows[hit.RowIndex].DataBoundItem).Row).Sell_Station;

                }

                Clipboard.SetText(resultString);
                Debug.Print(resultString);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in copyStationnameToClipboardToolStripMenuItem_Click");
            }
        }

        private void cmbMinSupply_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if(e.KeyChar == (char)13)
                    if(m_GUIInterface.saveSetting(sender) && cbMinSupply.Checked)
                        setFilterHasChanged(true);                
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbMinSupply_KeyPress");
            }
        }

        private void cmbMinSupply_Leave(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender) && cbMinSupply.Checked)
                    setFilterHasChanged(true);                
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmbMinSupply_Leave");
            }
        }

        private void cmbStation2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!cmbRoutingType.Text.Equals("round trip", StringComparison.InvariantCultureIgnoreCase))
            {
                if(cmbStation2.Text == "")
                    lbDetailStation2.Text    = "none";
                else
                {
                    lbDetailStation2.Text   = String.Format("{0} / {1}", ((System.Data.DataRowView)cmbStation2.SelectedItem).Row[1], ((System.Data.DataRowView)cmbStation2.SelectedItem).Row[3]);
                    lbDetailStation2.Tag    = (Int32)((System.Data.DataRowView)cmbStation2.SelectedItem).Row[2];
                }
            }
        }
    }
}
