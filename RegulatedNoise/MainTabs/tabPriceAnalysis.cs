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


        public const String                DB_GROUPNAME                    = "PriceAnalysis";
        public const String                CURRENT_SYSTEM                  = "<current system>";

        private PriceAnalysis               m_DataSource;                   // data object
        private enCLAction                  m_PA_State;                     // current gui state

        private Int32                       m_InitialTopOfGrid;
        private Int32                       m_InitialTopOfEditGroupBox;

        private Boolean                     m_CellValueNeededIsRegistered   = false;        // true if the event is already registred
        private Boolean                     m_FirstRowShown                 = false;        // true after first time shown
        private DBGuiInterface              m_GUIInterface;
        private Boolean                     m_RefreshStarted                = true;         // true, if the user started a new filtering

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

            try
            {

                Cursor = Cursors.WaitCursor;

                m_PA_State                              = enCLAction.None;

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
                
                //dgvCommandersLog.Invalidate();

                // jump to the new row
                //dgvCommandersLog.CurrentCell = dgvCommandersLog[1, e.DataRow];

            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in m_DataSource_DataChanged");
            }
        }

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

                this.Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                this.Cursor = oldCursor;
                throw new Exception("Error while starting to create a new baseview", ex);
            }
        }

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
                    cmdFilter.Enabled               = true;
                    cmdRoundTripCaclulation.Enabled = true;
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
                    cmdFilter.Enabled               = true;
                    cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                            cmdFilter.Enabled               = true;
                            cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                    cmdFilter.Enabled               = true;
                    cmdRoundTripCaclulation.Enabled = true;
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
                    cmdFilter.Enabled               = true;
                    cmdRoundTripCaclulation.Enabled = true;
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
                            cmdFilter.Enabled               = true;
                            cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                        cmdFilter.Enabled               = true;
                        cmdRoundTripCaclulation.Enabled = true;
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
                splitContainer2.Panel2Collapsed = !cbShowDiagramAllCommodities.Checked;
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
                    cmdFilter.Enabled               = true;
                    cmdRoundTripCaclulation.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in cbMinLandingPadSize_CheckedChanged");
            }
        }

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

        private void cmdFilter_Click(object sender, EventArgs e)
        {
            try
            {
                m_RefreshState["BaseData"]              = false;
                m_RefreshState["tpAllCommodities"]      = false;
                m_RefreshState["tpByStation"]           = false;
                m_RefreshState["tpByCommodity"]         = false;
                m_RefreshState["tpStationToStation"]    = false;

                refreshPriceView();

                cmdFilter.Enabled               = false;
                cmdRoundTripCaclulation.Enabled = false;
                m_RefreshStarted                = true;
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while filtering stations");
            }
        }

        private void refreshPriceView(Boolean TabWasChanged = false)
        {
            BindingSource bs;

            try
            {

                if (!m_RefreshState["BaseData"])
                { 
                    createNewBaseView();
                    m_RefreshState["BaseData"] = true;
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
                        if (!m_RefreshState["tpStationToStation"])
                        { 
                            Boolean startRecalculation = true;

                            if(TabWasChanged)
                                startRecalculation = (MessageBox.Show("Start recalculation of best profit route ?", 
                                                                      "Base data is changed", 
                                                                      MessageBoxButtons.YesNo, 
                                                                      MessageBoxIcon.Question, 
                                                                      MessageBoxDefaultButton.Button1) == DialogResult.Yes);

                            if(startRecalculation)
                            { 
                                
                                bs              = new BindingSource(); 
                                bs.DataSource   = m_DataSource.calculateTradingRoutes();        

                                dgvStationToStationRoutes.AutoGenerateColumns = false;
                                dgvStationToStationRoutes.DataSource          = bs;

                            }

                            m_RefreshState["tpStationToStation"] = true;
                        }

                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing current data view", ex);
            }
        }

        private void tabPriceSubTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                //refreshPriceView(true);
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error after changing active tabindex");
            }
        }

        private void cmdRoundTripCaclulation_Click(object sender, EventArgs e)
        {
            try
            {
                refreshPriceView();
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while starting recalculation of the best profit route");
            }
        }

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

    }
}
