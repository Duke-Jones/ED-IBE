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

        private const String        DB_GROUPNAME                    = "PriceAnalysis";

        private PriceAnalysis       m_DataSource;                   // data object
        private enCLAction          m_PA_State;                     // current gui state

        private Int32               m_InitialTopOfGrid;
        private Int32               m_InitialTopOfEditGroupBox;

        private Boolean             m_CellValueNeededIsRegistered   = false;        // true if the event is already registred
        private Boolean             m_FirstRowShown                 = false;        // true after first time shown
        private DBGuiInterface      m_GUIInterface;

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
                cmdStationLightSeconds.Items.Clear();
                foreach (String Value in ComboboxValues.Split(';'))
                    cmdStationLightSeconds.Items.Add(Int32.Parse(Value));

                ////preparing the combo boxes
                //m_DataSource.prepareCmb_EventTypes(ref cbLogEventType);
                //m_DataSource.prepareCmb_EventTypes(ref cbLogSystemName);
                //m_DataSource.prepareCmb_EventTypes(ref cbLogStationName, cbLogSystemName);
                //m_DataSource.prepareCmb_EventTypes(ref cbLogCargoName);
                //m_DataSource.prepareCmb_EventTypes(ref cbLogCargoAction);


                //dtpLogEventDate.CustomFormat = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern + " " + 
                //                               System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern;

                //dtpLogEventDate.Format       = System.Windows.Forms.DateTimePickerFormat.Custom;
    
                //setCLFieldsEditable(false);

                //// preparing the datagridview                
                //dgvAllCommodities.VirtualMode              = true;
                //dgvAllCommodities.ReadOnly                 = true;
                //dgvAllCommodities.AllowUserToAddRows       = false;
                //dgvAllCommodities.AllowUserToOrderColumns  = false;
                //dgvAllCommodities.SelectionMode            = DataGridViewSelectionMode.FullRowSelect;

                //dgvAllCommodities.RowCount                 = m_DataSource.InitRetriever();

                //dgvAllCommodities.RowEnter                += dgvCommandersLog_RowEnter;
                //dgvAllCommodities.RowPrePaint             += dgvCommandersLog_RowPrePaint;
                //dgvAllCommodities.Paint                   += dgvCommandersLog_Paint;

                //setEditfieldBoxVisible(Program.DBCon.getIniValue<Boolean>(DB_GROUPNAME, "showEditFields", "true", false));

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

        /// <summary>
        /// "Stations Within" enabled/disabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbOnlyStationsWithin_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in CheckBox_CheckedChanged");
            }
        }

        private void createNewBaseView()
        {
            Object Distance = null;
            Object DistanceToStar = null;
            Object minLandingPadSize = null;
            Cursor oldCursor =  this.Cursor;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if(cbOnlyStationsWithin.Checked)                 
                    Distance = Int32.Parse(cmbSystemLightYears.Text);

                if(cbMaxDistanceToStar.Checked)                 
                    DistanceToStar = Int32.Parse(cmdStationLightSeconds.Text);

                if(cbMinLandingPadSize.Checked)                 
                    minLandingPadSize = cmbMinLandingPadSize.Text;
                
                //m_DataSource.createFilteredTable((Int32)cmbSystemBase.SelectedValue, Distance, DistanceToStar, minLandingPadSize);

                PriceAnalysis.enVisitedFilter VFilter = (PriceAnalysis.enVisitedFilter)Program.DBCon.getIniValue<Int32>("Global", "VisitedFilter", ((Int32)PriceAnalysis.enVisitedFilter.showOnlyVistedSystems).ToString(), false);

                m_DataSource.createFilteredTable(17072, Distance, DistanceToStar, minLandingPadSize, VFilter);

                Int32 StationCount;
                Int32 SystemCount;
                m_DataSource.getFilteredSystemAndStationCount(out StationCount, out SystemCount);

                lblSystemsFound.Text  = SystemCount.ToString();
                lblStationsFound.Text = StationCount.ToString();

                BindingSource bs = new BindingSource(); 

                bs.DataSource = m_DataSource.getMinMax(cbOnlyTradedCommodities.Checked);


                dgvAllCommodities.AutoGenerateColumns = false;
                dgvAllCommodities.DataSource = bs;
                sortAllCommodities();


                m_DataSource.calculateTradingRoutes(20);





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
        private void cbMaxDistanceToStar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in cbMaxDistanceToStar_CheckedChanged");
            }
        }

        private void rbOrderBySystem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in rbOrderBySystem_CheckedChanged");
            }
        }

        private void rbOrderByStation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in rbOrderByStation_CheckedChanged");
            }
        }

        private void rbOrderByDistance_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in rbOrderByDistance_CheckedChanged");
            }
        }

        private void cbOrderByAmount_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in cbOrderByAmount_CheckedChanged");
            }
        }

        private void txtOrderByAmount_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((TextBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        createNewBaseView();
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
                            createNewBaseView();
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
                        if(m_GUIInterface.saveSetting(sender))
                        {
                            createNewBaseView();
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
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        createNewBaseView();
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
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        createNewBaseView();
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
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        createNewBaseView();
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
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        createNewBaseView();
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
                        if(m_GUIInterface.saveSetting(sender))
                        {
                            createNewBaseView();
                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in cmdStationLightSeconds_KeyDown");
            }
        }

        private void cbMinLandingPadSize_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in cbMinLandingPadSize_CheckedChanged");
            }
        }

        private void cmbMinLandingPadSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(m_GUIInterface.saveSetting(sender))
                    {
                        createNewBaseView();
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
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
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
                if(m_GUIInterface.saveSetting(sender))
                {
                    createNewBaseView();
                }
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in cmbMinLandingPadSize_SelectedIndexChanged");
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
                    createNewBaseView();
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

        private void dgvAllCommodities_Sorted(object sender, EventArgs e)
        {

        }

        private void comboBoxInt321_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.Print(comboBoxInt321.SelectedValue.ToString());
        }

    }


}
