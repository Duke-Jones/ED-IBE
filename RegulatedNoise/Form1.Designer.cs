namespace RegulatedNoise
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rbSortByDistance = new System.Windows.Forms.RadioButton();
            this.rbSortByStation = new System.Windows.Forms.RadioButton();
            this.rbSortBySystem = new System.Windows.Forms.RadioButton();
            this.label89 = new System.Windows.Forms.Label();
            this.nudPurgeOldDataDays = new System.Windows.Forms.NumericUpDown();
            this.cmdPurgeOldData = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copySystenmameToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label53 = new System.Windows.Forms.Label();
            this.txtEDTime = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.txtLocalTime = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.tbCurrentStationinfoFromLogs = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.tbCurrentSystemFromLogs = new System.Windows.Forms.TextBox();
            this.button19 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.tabCtrlMain = new System.Windows.Forms.TabControl();
            this.tabHelpAndChangeLog = new System.Windows.Forms.TabPage();
            this.label93 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.linkLabel11 = new System.Windows.Forms.LinkLabel();
            this.linkLabel10 = new System.Windows.Forms.LinkLabel();
            this.llVisitUpdate = new System.Windows.Forms.LinkLabel();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.lblUpdateInfo = new System.Windows.Forms.Label();
            this.lblUpdateDetail = new System.Windows.Forms.Label();
            this.cmdDonate = new System.Windows.Forms.Button();
            this.label42 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label44 = new System.Windows.Forms.Label();
            this.linkLabel9 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkLabel6 = new System.Windows.Forms.LinkLabel();
            this.linkLabel7 = new System.Windows.Forms.LinkLabel();
            this.BackgroundSet = new System.Windows.Forms.Label();
            this.ForegroundSet = new System.Windows.Forms.Label();
            this.button20 = new System.Windows.Forms.Button();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.pbBackgroundColour = new System.Windows.Forms.PictureBox();
            this.pbForegroundColour = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button23 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblRegulatedNoise = new System.Windows.Forms.Label();
            this.tabSystemData = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.cmbStationStations = new System.Windows.Forms.ComboBox_ro();
            this.paEconomies = new System.Windows.Forms.Panel();
            this.cbStationEcoNone = new System.Windows.Forms.CheckBox_ro();
            this.button8 = new System.Windows.Forms.Button();
            this.cbStationEcoTourism = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoTerraforming = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoService = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoRefinery = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoMilitary = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoIndustrial = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoHighTech = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoExtraction = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoAgriculture = new System.Windows.Forms.CheckBox_ro();
            this.cmdStationCancel = new System.Windows.Forms.Button();
            this.lblStationCountTotal = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.cmdStationEdit = new System.Windows.Forms.Button();
            this.lblStationCount = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.lbStationEconomies = new System.Windows.Forms.ListBox();
            this.label64 = new System.Windows.Forms.Label();
            this.txtStationUpdatedAt = new System.Windows.Forms.TextBox();
            this.label73 = new System.Windows.Forms.Label();
            this.cbStationHasOutfitting = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasShipyard = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasRepair = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasRearm = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasRefuel = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasCommodities = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasBlackmarket = new System.Windows.Forms.CheckBox_ro();
            this.label87 = new System.Windows.Forms.Label();
            this.cmdStationNew = new System.Windows.Forms.Button();
            this.cmdStationSave = new System.Windows.Forms.Button();
            this.cmbStationType = new System.Windows.Forms.ComboBox_ro();
            this.label75 = new System.Windows.Forms.Label();
            this.cmbStationState = new System.Windows.Forms.ComboBox_ro();
            this.label76 = new System.Windows.Forms.Label();
            this.cmbStationAllegiance = new System.Windows.Forms.ComboBox_ro();
            this.label77 = new System.Windows.Forms.Label();
            this.cmbStationGovernment = new System.Windows.Forms.ComboBox_ro();
            this.label78 = new System.Windows.Forms.Label();
            this.txtStationFaction = new System.Windows.Forms.TextBox();
            this.label79 = new System.Windows.Forms.Label();
            this.txtStationDistanceToStar = new System.Windows.Forms.TextBox();
            this.label80 = new System.Windows.Forms.Label();
            this.cmbStationMaxLandingPadSize = new System.Windows.Forms.ComboBox_ro();
            this.label81 = new System.Windows.Forms.Label();
            this.txtStationName = new System.Windows.Forms.TextBox();
            this.label85 = new System.Windows.Forms.Label();
            this.txtStationId = new System.Windows.Forms.TextBox();
            this.label86 = new System.Windows.Forms.Label();
            this.lblStationRenameHint = new System.Windows.Forms.Label();
            this.gbSystemSystemData = new System.Windows.Forms.GroupBox();
            this.cmdTest = new System.Windows.Forms.Button();
            this.cmdSystemCancel = new System.Windows.Forms.Button();
            this.lblSystemCountTotal = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.cmbSystemsAllSystems = new System.Windows.Forms.ComboBox_ro();
            this.label88 = new System.Windows.Forms.Label();
            this.cmdSystemEdit = new System.Windows.Forms.Button();
            this.cbSystemNeedsPermit = new System.Windows.Forms.CheckBox_ro();
            this.cmdSystemNew = new System.Windows.Forms.Button();
            this.cmdSystemSave = new System.Windows.Forms.Button();
            this.txtSystemUpdatedAt = new System.Windows.Forms.TextBox();
            this.label63 = new System.Windows.Forms.Label();
            this.cmbSystemPrimaryEconomy = new System.Windows.Forms.ComboBox_ro();
            this.label65 = new System.Windows.Forms.Label();
            this.cmbSystemSecurity = new System.Windows.Forms.ComboBox_ro();
            this.label66 = new System.Windows.Forms.Label();
            this.cmbSystemState = new System.Windows.Forms.ComboBox_ro();
            this.label67 = new System.Windows.Forms.Label();
            this.cmbSystemAllegiance = new System.Windows.Forms.ComboBox_ro();
            this.label68 = new System.Windows.Forms.Label();
            this.cmbSystemGovernment = new System.Windows.Forms.ComboBox_ro();
            this.label69 = new System.Windows.Forms.Label();
            this.txtSystemPopulation = new System.Windows.Forms.MaskedTextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.txtSystemFaction = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.txtSystemZ = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.txtSystemY = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.txtSystemX = new System.Windows.Forms.TextBox();
            this.label58 = new System.Windows.Forms.Label();
            this.txtSystemName = new System.Windows.Forms.TextBox();
            this.label57 = new System.Windows.Forms.Label();
            this.txtSystemId = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.lblSystemRenameHint = new System.Windows.Forms.Label();
            this.tabPriceAnalysis = new System.Windows.Forms.TabPage();
            this.label55 = new System.Windows.Forms.Label();
            this.cmbStationToStar = new System.Windows.Forms.ComboBox();
            this.gbSorting = new System.Windows.Forms.GroupBox();
            this.txtlastStationCount = new System.Windows.Forms.TextBox();
            this.cblastVisitedFirst = new System.Windows.Forms.CheckBox();
            this.cbStationToStar = new System.Windows.Forms.CheckBox();
            this.cbIncludeWithinRegionOfStation = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.cbLimitLightYears = new System.Windows.Forms.CheckBox();
            this.cmbLightYears = new System.Windows.Forms.ComboBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvAllComms = new System.Windows.Forms.ListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label8 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label9 = new System.Windows.Forms.Label();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bStationDeleteRow = new System.Windows.Forms.Button();
            this.bStationEditRow = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tbStationRename = new System.Windows.Forms.TextBox();
            this.tbSystemRename = new System.Windows.Forms.TextBox();
            this.cmdApplySystemRename = new System.Windows.Forms.Button();
            this.bShowStationAtStarchartDotInfo = new System.Windows.Forms.Button();
            this.lbPrices = new System.Windows.Forms.ListView();
            this.cmbStation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLightYearsFromCurrentSystem = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bCommodityDeleteRow = new System.Windows.Forms.Button();
            this.bEditCommodity = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblAvgSell = new System.Windows.Forms.Label();
            this.lblMaxSell = new System.Windows.Forms.Label();
            this.lblMinSell = new System.Windows.Forms.Label();
            this.lblAvg = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCommodity = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbCommodities = new System.Windows.Forms.ListView();
            this.tabStationToStation = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.lvStationToStation = new System.Windows.Forms.ListView();
            this.lvStationToStationReturn = new System.Windows.Forms.ListView();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label70 = new System.Windows.Forms.Label();
            this.cmbMaxRouteDistance = new System.Windows.Forms.ComboBox();
            this.cbMaxRouteDistance = new System.Windows.Forms.CheckBox();
            this.cbPerLightYearRoundTrip = new System.Windows.Forms.CheckBox();
            this.lbAllRoundTrips = new System.Windows.Forms.ListBox();
            this.btnBestRoundTrip = new System.Windows.Forms.Button();
            this.lblStationToStationLightYears = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.lblStationToStationMax = new System.Windows.Forms.Label();
            this.bSwapStationToStations = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.cmbStationToStationTo = new System.Windows.Forms.ComboBox();
            this.cmbStationToStationFrom = new System.Windows.Forms.ComboBox();
            this.bShowStationToStationRouteAtStarchartDotClub = new System.Windows.Forms.Button();
            this.bShowStationRestrictionAtStarchartDotClub = new System.Windows.Forms.Button();
            this.tabCommandersLog = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.lvCommandersLog = new System.Windows.Forms.ListView();
            this.cbLogSystemName = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.cbLogEventType = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btCreateAddEntry = new System.Windows.Forms.Button();
            this.cbLogStationName = new System.Windows.Forms.ComboBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.nbCurrentCredits = new System.Windows.Forms.NumericUpDown();
            this.cbLogCargoName = new System.Windows.Forms.ComboBox();
            this.nbTransactionAmount = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.cbPrepareNewEntry = new System.Windows.Forms.Button();
            this.cbLogQuantity = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.cbCargoModifier = new System.Windows.Forms.ComboBox();
            this.tbLogEventID = new System.Windows.Forms.TextBox();
            this.tbLogNotes = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.dtpLogEventDate = new System.Windows.Forms.DateTimePicker();
            this.tabOCRGroup = new System.Windows.Forms.TabPage();
            this.tabCtrlOCR = new System.Windows.Forms.TabControl();
            this.tabOCR = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bIgnoreTrash = new System.Windows.Forms.Button();
            this.bClearOcrOutput = new System.Windows.Forms.Button();
            this.bEditResults = new System.Windows.Forms.Button();
            this.cbAutoImport = new System.Windows.Forms.CheckBox();
            this.cbUseEddnTestSchema = new System.Windows.Forms.CheckBox();
            this.cbDeleteScreenshotOnImport = new System.Windows.Forms.CheckBox();
            this.cbStartOCROnLoad = new System.Windows.Forms.CheckBox();
            this.cbExtendedInfoInCSV = new System.Windows.Forms.CheckBox();
            this.cmdHint = new System.Windows.Forms.Button();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.cbPostOnImport = new System.Windows.Forms.CheckBox();
            this.tbOcrSystemName = new System.Windows.Forms.TextBox();
            this.tbFinalOcrOutput = new System.Windows.Forms.TextBox();
            this.bContinueOcr = new System.Windows.Forms.Button();
            this.tbConfidence = new System.Windows.Forms.TextBox();
            this.tbCommoditiesOcrOutput = new System.Windows.Forms.TextBox();
            this.pbOcrCurrent = new System.Windows.Forms.PictureBox();
            this.tbOcrStationName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblScreenshotsQueued = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.pbStation = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pbTrimmed = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbOriginalImage = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabWebserver = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtWebserverPort = new System.Windows.Forms.TextBox();
            this.cbInterfaces = new System.Windows.Forms.ComboBox();
            this.label71 = new System.Windows.Forms.Label();
            this.cbStartWebserverOnLoad = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbBackgroundColour = new System.Windows.Forms.TextBox();
            this.tbForegroundColour = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cbColourScheme = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.bStart = new System.Windows.Forms.Button();
            this.bStop = new System.Windows.Forms.Button();
            this.lblURL = new System.Windows.Forms.Label();
            this.tabEDDN = new System.Windows.Forms.TabPage();
            this.label83 = new System.Windows.Forms.Label();
            this.lbEddnImplausible = new System.Windows.Forms.ListBox();
            this.tbEddnStats = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbSpoolImplausibleToFile = new System.Windows.Forms.CheckBox();
            this.cbSpoolEddnToFile = new System.Windows.Forms.CheckBox();
            this.bPurgeAllEddnData = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.checkboxImportEDDN = new System.Windows.Forms.CheckBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cmdStopEDDNListening = new System.Windows.Forms.Button();
            this.tbEDDNOutput = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.cbLoadStationsJSON = new System.Windows.Forms.CheckBox();
            this.label74 = new System.Windows.Forms.Label();
            this.cbAutoActivateSystemTab = new System.Windows.Forms.CheckBox();
            this.cbIncludeUnknownDTS = new System.Windows.Forms.CheckBox();
            this.cbAutoActivateOCRTab = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.cbAutoAdd_ReplaceVisited = new System.Windows.Forms.CheckBox();
            this.cbAutoAdd_Marketdata = new System.Windows.Forms.CheckBox();
            this.cbAutoAdd_Visited = new System.Windows.Forms.CheckBox();
            this.label49 = new System.Windows.Forms.Label();
            this.cbAutoAdd_JumpedTo = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label48 = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cmdFilter = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.txtGUIColorCutoffLevel = new System.Windows.Forms.TextBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label52 = new System.Windows.Forms.Label();
            this.cbCheckAOne = new System.Windows.Forms.CheckBox();
            this.lblPixelAmount = new System.Windows.Forms.Label();
            this.txtPixelAmount = new System.Windows.Forms.TextBox();
            this.lblPixelThreshold = new System.Windows.Forms.Label();
            this.txtPixelThreshold = new System.Windows.Forms.TextBox();
            this.txtTraineddataFile = new System.Windows.Forms.TextBox();
            this.cmdSelectTraineddataFile = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.cmdLoadCurrentSystem = new System.Windows.Forms.Button();
            this.bOpen = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeEconomyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dsCommodities = new RegulatedNoise.Enums_and_Utility_Classes.dsCommodities();
            this.namesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudPurgeOldDataDays)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabCtrlMain.SuspendLayout();
            this.tabHelpAndChangeLog.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabSystemData.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.paEconomies.SuspendLayout();
            this.gbSystemSystemData.SuspendLayout();
            this.tabPriceAnalysis.SuspendLayout();
            this.gbSorting.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabStationToStation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabCommandersLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbCurrentCredits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTransactionAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbLogQuantity)).BeginInit();
            this.tabOCRGroup.SuspendLayout();
            this.tabCtrlOCR.SuspendLayout();
            this.tabOCR.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOcrCurrent)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrimmed)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).BeginInit();
            this.tabWebserver.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabEDDN.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsCommodities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.namesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "traineddata";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Tesseract-Files|*.traineddata|All Files|*.*";
            this.openFileDialog1.Title = "select Tesseract Traineddata-File";
            // 
            // rbSortByDistance
            // 
            this.rbSortByDistance.AutoSize = true;
            this.rbSortByDistance.Location = new System.Drawing.Point(115, 14);
            this.rbSortByDistance.Name = "rbSortByDistance";
            this.rbSortByDistance.Size = new System.Drawing.Size(65, 17);
            this.rbSortByDistance.TabIndex = 23;
            this.rbSortByDistance.Text = "distance";
            this.toolTip1.SetToolTip(this.rbSortByDistance, "sorting by distance");
            this.rbSortByDistance.UseVisualStyleBackColor = true;
            this.rbSortByDistance.CheckedChanged += new System.EventHandler(this.rbSortBy_CheckedChanged);
            // 
            // rbSortByStation
            // 
            this.rbSortByStation.AutoSize = true;
            this.rbSortByStation.Location = new System.Drawing.Point(5, 31);
            this.rbSortByStation.Name = "rbSortByStation";
            this.rbSortByStation.Size = new System.Drawing.Size(85, 17);
            this.rbSortByStation.TabIndex = 19;
            this.rbSortByStation.Text = "station name";
            this.toolTip1.SetToolTip(this.rbSortByStation, "sorting by station ");
            this.rbSortByStation.UseVisualStyleBackColor = true;
            this.rbSortByStation.CheckedChanged += new System.EventHandler(this.rbSortBy_CheckedChanged);
            // 
            // rbSortBySystem
            // 
            this.rbSortBySystem.AutoSize = true;
            this.rbSortBySystem.Checked = true;
            this.rbSortBySystem.Location = new System.Drawing.Point(5, 14);
            this.rbSortBySystem.Name = "rbSortBySystem";
            this.rbSortBySystem.Size = new System.Drawing.Size(86, 17);
            this.rbSortBySystem.TabIndex = 18;
            this.rbSortBySystem.TabStop = true;
            this.rbSortBySystem.Text = "system name";
            this.toolTip1.SetToolTip(this.rbSortBySystem, "sorting by system");
            this.rbSortBySystem.UseVisualStyleBackColor = true;
            this.rbSortBySystem.CheckedChanged += new System.EventHandler(this.rbSortBy_CheckedChanged);
            // 
            // label89
            // 
            this.label89.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(203, 338);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(29, 13);
            this.label89.TabIndex = 19;
            this.label89.Text = "days";
            this.toolTip1.SetToolTip(this.label89, "0 purges all but todays price data, -1 purges ALL price data");
            // 
            // nudPurgeOldDataDays
            // 
            this.nudPurgeOldDataDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudPurgeOldDataDays.Location = new System.Drawing.Point(154, 335);
            this.nudPurgeOldDataDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.nudPurgeOldDataDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudPurgeOldDataDays.Name = "nudPurgeOldDataDays";
            this.nudPurgeOldDataDays.Size = new System.Drawing.Size(44, 20);
            this.nudPurgeOldDataDays.TabIndex = 62;
            this.nudPurgeOldDataDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.nudPurgeOldDataDays, "0 purges all but todays data, -1 purges ALL data");
            this.nudPurgeOldDataDays.ValueChanged += new System.EventHandler(this.nudPurgeOldDataDays_ValueChanged);
            // 
            // cmdPurgeOldData
            // 
            this.cmdPurgeOldData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdPurgeOldData.Location = new System.Drawing.Point(32, 333);
            this.cmdPurgeOldData.Name = "cmdPurgeOldData";
            this.cmdPurgeOldData.Size = new System.Drawing.Size(116, 23);
            this.cmdPurgeOldData.TabIndex = 8;
            this.cmdPurgeOldData.Text = "purge data older than";
            this.toolTip1.SetToolTip(this.cmdPurgeOldData, "0 purges all but todays price data, -1 purges ALL price data");
            this.cmdPurgeOldData.UseVisualStyleBackColor = true;
            this.cmdPurgeOldData.Click += new System.EventHandler(this.cmdPurgeOldData_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySystenmameToClipboardToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(246, 26);
            // 
            // copySystenmameToClipboardToolStripMenuItem
            // 
            this.copySystenmameToClipboardToolStripMenuItem.Name = "copySystenmameToClipboardToolStripMenuItem";
            this.copySystenmameToClipboardToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.copySystenmameToClipboardToolStripMenuItem.Text = "Copy Systemname To Clipboard";
            this.copySystenmameToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySystenmameToClipboardToolStripMenuItem_Click);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(499, 41);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(49, 13);
            this.label53.TabIndex = 15;
            this.label53.Text = "Universe";
            // 
            // txtEDTime
            // 
            this.txtEDTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEDTime.Location = new System.Drawing.Point(554, 36);
            this.txtEDTime.Name = "txtEDTime";
            this.txtEDTime.ReadOnly = true;
            this.txtEDTime.Size = new System.Drawing.Size(69, 22);
            this.txtEDTime.TabIndex = 14;
            this.txtEDTime.Text = "88:88:88";
            this.txtEDTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(515, 17);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(33, 13);
            this.label54.TabIndex = 13;
            this.label54.Text = "Local";
            // 
            // txtLocalTime
            // 
            this.txtLocalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalTime.Location = new System.Drawing.Point(554, 12);
            this.txtLocalTime.Name = "txtLocalTime";
            this.txtLocalTime.ReadOnly = true;
            this.txtLocalTime.Size = new System.Drawing.Size(69, 22);
            this.txtLocalTime.TabIndex = 11;
            this.txtLocalTime.Text = "88:88:88";
            this.txtLocalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(667, 38);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(152, 13);
            this.label45.TabIndex = 10;
            this.label45.Text = "Current Location (from log files)";
            // 
            // tbCurrentStationinfoFromLogs
            // 
            this.tbCurrentStationinfoFromLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentStationinfoFromLogs.Location = new System.Drawing.Point(823, 35);
            this.tbCurrentStationinfoFromLogs.Name = "tbCurrentStationinfoFromLogs";
            this.tbCurrentStationinfoFromLogs.ReadOnly = true;
            this.tbCurrentStationinfoFromLogs.Size = new System.Drawing.Size(255, 20);
            this.tbCurrentStationinfoFromLogs.TabIndex = 9;
            this.tbCurrentStationinfoFromLogs.Text = "scanning...";
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(672, 17);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(145, 13);
            this.label37.TabIndex = 8;
            this.label37.Text = "Current System (from log files)";
            // 
            // tbCurrentSystemFromLogs
            // 
            this.tbCurrentSystemFromLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentSystemFromLogs.Location = new System.Drawing.Point(823, 14);
            this.tbCurrentSystemFromLogs.Name = "tbCurrentSystemFromLogs";
            this.tbCurrentSystemFromLogs.ReadOnly = true;
            this.tbCurrentSystemFromLogs.Size = new System.Drawing.Size(255, 20);
            this.tbCurrentSystemFromLogs.TabIndex = 7;
            this.tbCurrentSystemFromLogs.Text = "scanning...";
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(125, 12);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(160, 23);
            this.button19.TabIndex = 6;
            this.button19.Text = "Open (Sub-)Folders of CSVs";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(291, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(116, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "Save Unified CSV";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // tabCtrlMain
            // 
            this.tabCtrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlMain.Controls.Add(this.tabHelpAndChangeLog);
            this.tabCtrlMain.Controls.Add(this.tabSystemData);
            this.tabCtrlMain.Controls.Add(this.tabPriceAnalysis);
            this.tabCtrlMain.Controls.Add(this.tabCommandersLog);
            this.tabCtrlMain.Controls.Add(this.tabOCRGroup);
            this.tabCtrlMain.Controls.Add(this.tabWebserver);
            this.tabCtrlMain.Controls.Add(this.tabEDDN);
            this.tabCtrlMain.Controls.Add(this.tabSettings);
            this.tabCtrlMain.Location = new System.Drawing.Point(18, 64);
            this.tabCtrlMain.Name = "tabCtrlMain";
            this.tabCtrlMain.SelectedIndex = 0;
            this.tabCtrlMain.Size = new System.Drawing.Size(1059, 615);
            this.tabCtrlMain.TabIndex = 4;
            this.tabCtrlMain.SelectedIndexChanged += new System.EventHandler(this.tabCtrlMain_SelectedIndexChanged);
            this.tabCtrlMain.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabCtrlMain_Selecting);
            this.tabCtrlMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabHelpAndChangeLog
            // 
            this.tabHelpAndChangeLog.Controls.Add(this.label93);
            this.tabHelpAndChangeLog.Controls.Add(this.label92);
            this.tabHelpAndChangeLog.Controls.Add(this.label91);
            this.tabHelpAndChangeLog.Controls.Add(this.linkLabel11);
            this.tabHelpAndChangeLog.Controls.Add(this.linkLabel10);
            this.tabHelpAndChangeLog.Controls.Add(this.llVisitUpdate);
            this.tabHelpAndChangeLog.Controls.Add(this.cmdUpdate);
            this.tabHelpAndChangeLog.Controls.Add(this.lblUpdateInfo);
            this.tabHelpAndChangeLog.Controls.Add(this.lblUpdateDetail);
            this.tabHelpAndChangeLog.Controls.Add(this.cmdDonate);
            this.tabHelpAndChangeLog.Controls.Add(this.label42);
            this.tabHelpAndChangeLog.Controls.Add(this.panel2);
            this.tabHelpAndChangeLog.Controls.Add(this.BackgroundSet);
            this.tabHelpAndChangeLog.Controls.Add(this.ForegroundSet);
            this.tabHelpAndChangeLog.Controls.Add(this.button20);
            this.tabHelpAndChangeLog.Controls.Add(this.label47);
            this.tabHelpAndChangeLog.Controls.Add(this.label46);
            this.tabHelpAndChangeLog.Controls.Add(this.pbBackgroundColour);
            this.tabHelpAndChangeLog.Controls.Add(this.pbForegroundColour);
            this.tabHelpAndChangeLog.Controls.Add(this.pictureBox1);
            this.tabHelpAndChangeLog.Controls.Add(this.button23);
            this.tabHelpAndChangeLog.Controls.Add(this.button22);
            this.tabHelpAndChangeLog.Controls.Add(this.lblSubtitle);
            this.tabHelpAndChangeLog.Controls.Add(this.lblRegulatedNoise);
            this.tabHelpAndChangeLog.Location = new System.Drawing.Point(4, 22);
            this.tabHelpAndChangeLog.Name = "tabHelpAndChangeLog";
            this.tabHelpAndChangeLog.Size = new System.Drawing.Size(1051, 589);
            this.tabHelpAndChangeLog.TabIndex = 9;
            this.tabHelpAndChangeLog.Text = "Help and Changelog";
            this.tabHelpAndChangeLog.UseVisualStyleBackColor = true;
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(297, 221);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(121, 13);
            this.label93.TabIndex = 47;
            this.label93.Text = "RN im deutschen Forum";
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(297, 186);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(92, 13);
            this.label92.TabIndex = 46;
            this.label92.Text = "english RN thread";
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Location = new System.Drawing.Point(27, 186);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(63, 13);
            this.label91.TabIndex = 45;
            this.label91.Text = "Project-Link";
            // 
            // linkLabel11
            // 
            this.linkLabel11.AutoSize = true;
            this.linkLabel11.Location = new System.Drawing.Point(297, 234);
            this.linkLabel11.Name = "linkLabel11";
            this.linkLabel11.Size = new System.Drawing.Size(343, 13);
            this.linkLabel11.TabIndex = 44;
            this.linkLabel11.TabStop = true;
            this.linkLabel11.Text = "http://www.elitedangerous.de/forum/viewtopic.php?f=66&t=6404&start=0";
            this.linkLabel11.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llVisitUpdate_LinkClicked);
            // 
            // linkLabel10
            // 
            this.linkLabel10.AutoSize = true;
            this.linkLabel10.Location = new System.Drawing.Point(297, 200);
            this.linkLabel10.Name = "linkLabel10";
            this.linkLabel10.Size = new System.Drawing.Size(271, 13);
            this.linkLabel10.TabIndex = 43;
            this.linkLabel10.TabStop = true;
            this.linkLabel10.Text = "https://forums.frontier.co.uk/showthread.php?t=137732";
            this.linkLabel10.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llVisitUpdate_LinkClicked);
            // 
            // llVisitUpdate
            // 
            this.llVisitUpdate.AutoSize = true;
            this.llVisitUpdate.Location = new System.Drawing.Point(27, 200);
            this.llVisitUpdate.Name = "llVisitUpdate";
            this.llVisitUpdate.Size = new System.Drawing.Size(238, 13);
            this.llVisitUpdate.TabIndex = 42;
            this.llVisitUpdate.TabStop = true;
            this.llVisitUpdate.Text = "https://github.com/Duke-Jones/RegulatedNoise";
            this.llVisitUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llVisitUpdate_LinkClicked);
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Location = new System.Drawing.Point(541, 257);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(167, 32);
            this.cmdUpdate.TabIndex = 41;
            this.cmdUpdate.Text = "visit update page";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // lblUpdateInfo
            // 
            this.lblUpdateInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUpdateInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateInfo.Location = new System.Drawing.Point(30, 260);
            this.lblUpdateInfo.Name = "lblUpdateInfo";
            this.lblUpdateInfo.Size = new System.Drawing.Size(491, 25);
            this.lblUpdateInfo.TabIndex = 40;
            this.lblUpdateInfo.Text = "label92";
            // 
            // lblUpdateDetail
            // 
            this.lblUpdateDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdateDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateDetail.Location = new System.Drawing.Point(30, 295);
            this.lblUpdateDetail.Name = "lblUpdateDetail";
            this.lblUpdateDetail.Size = new System.Drawing.Size(664, 206);
            this.lblUpdateDetail.TabIndex = 39;
            this.lblUpdateDetail.Text = "label91";
            // 
            // cmdDonate
            // 
            this.cmdDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDonate.BackgroundImage = global::RegulatedNoise.Properties.Resources.PayPalDonate;
            this.cmdDonate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cmdDonate.FlatAppearance.BorderSize = 0;
            this.cmdDonate.Location = new System.Drawing.Point(870, 380);
            this.cmdDonate.Name = "cmdDonate";
            this.cmdDonate.Size = new System.Drawing.Size(154, 36);
            this.cmdDonate.TabIndex = 37;
            this.cmdDonate.UseVisualStyleBackColor = true;
            this.cmdDonate.Click += new System.EventHandler(this.cmdDonate_Click);
            // 
            // label42
            // 
            this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(711, 350);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(315, 52);
            this.label42.TabIndex = 38;
            this.label42.Text = "If you like this program and you want to support my development, \r\nI was very hap" +
    "py about a small donation. \r\n\r\nThank you, Duke Jones";
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel2.Controls.Add(this.label44);
            this.panel2.Controls.Add(this.linkLabel9);
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Controls.Add(this.linkLabel8);
            this.panel2.Controls.Add(this.linkLabel2);
            this.panel2.Controls.Add(this.linkLabel3);
            this.panel2.Controls.Add(this.linkLabel4);
            this.panel2.Controls.Add(this.linkLabel5);
            this.panel2.Controls.Add(this.linkLabel6);
            this.panel2.Controls.Add(this.linkLabel7);
            this.panel2.Location = new System.Drawing.Point(1, 514);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1051, 68);
            this.panel2.TabIndex = 33;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(247, 6);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(546, 13);
            this.label44.TabIndex = 5;
            this.label44.Text = "RegulatedNoise is as unofficial as it gets.   Elite: Dangerous is a registered tr" +
    "ademark of Frontier Developments plc.";
            // 
            // linkLabel9
            // 
            this.linkLabel9.AutoSize = true;
            this.linkLabel9.Location = new System.Drawing.Point(545, 24);
            this.linkLabel9.Name = "linkLabel9";
            this.linkLabel9.Size = new System.Drawing.Size(509, 13);
            this.linkLabel9.TabIndex = 32;
            this.linkLabel9.TabStop = true;
            this.linkLabel9.Text = "System Location information kindly provided by Maddavo\'s Market Share at http://w" +
    "ww.davek.com.au/td/";
            this.linkLabel9.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel9_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(208, 43);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(162, 13);
            this.linkLabel1.TabIndex = 14;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Tesseract OCR - Apache license";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel8
            // 
            this.linkLabel8.AutoSize = true;
            this.linkLabel8.Location = new System.Drawing.Point(3, 24);
            this.linkLabel8.Name = "linkLabel8";
            this.linkLabel8.Size = new System.Drawing.Size(94, 13);
            this.linkLabel8.TabIndex = 31;
            this.linkLabel8.TabStop = true;
            this.linkLabel8.Text = "Json.NET License";
            this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel8_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(3, 43);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(199, 13);
            this.linkLabel2.TabIndex = 15;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Originally inspired by seeebek\'s EliteOCR";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(376, 43);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(143, 13);
            this.linkLabel3.TabIndex = 16;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Webserver from CodeProject";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(525, 43);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(85, 13);
            this.linkLabel4.TabIndex = 17;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "LibZMQ License";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked_1);
            // 
            // linkLabel5
            // 
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Location = new System.Drawing.Point(620, 43);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(78, 13);
            this.linkLabel5.TabIndex = 18;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "Clrzmq License";
            this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel5_LinkClicked);
            // 
            // linkLabel6
            // 
            this.linkLabel6.AutoSize = true;
            this.linkLabel6.Location = new System.Drawing.Point(704, 43);
            this.linkLabel6.Name = "linkLabel6";
            this.linkLabel6.Size = new System.Drawing.Size(339, 13);
            this.linkLabel6.TabIndex = 20;
            this.linkLabel6.TabStop = true;
            this.linkLabel6.Text = "Numeric OCR handled by EliteBrainerous by zxctypo, used with thanks";
            this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel6_LinkClicked_1);
            // 
            // linkLabel7
            // 
            this.linkLabel7.AutoSize = true;
            this.linkLabel7.Location = new System.Drawing.Point(100, 24);
            this.linkLabel7.Name = "linkLabel7";
            this.linkLabel7.Size = new System.Drawing.Size(445, 13);
            this.linkLabel7.TabIndex = 23;
            this.linkLabel7.TabStop = true;
            this.linkLabel7.Text = "System Location information kindly provided by Biteketkergetek at http://starchar" +
    "t.club/map/";
            this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel7_LinkClicked);
            // 
            // BackgroundSet
            // 
            this.BackgroundSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundSet.AutoSize = true;
            this.BackgroundSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackgroundSet.Location = new System.Drawing.Point(1000, 192);
            this.BackgroundSet.Name = "BackgroundSet";
            this.BackgroundSet.Size = new System.Drawing.Size(18, 20);
            this.BackgroundSet.TabIndex = 30;
            this.BackgroundSet.Text = "?";
            this.BackgroundSet.Click += new System.EventHandler(this.BackgroundSet_Click);
            // 
            // ForegroundSet
            // 
            this.ForegroundSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ForegroundSet.AutoSize = true;
            this.ForegroundSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForegroundSet.Location = new System.Drawing.Point(1000, 154);
            this.ForegroundSet.Name = "ForegroundSet";
            this.ForegroundSet.Size = new System.Drawing.Size(18, 20);
            this.ForegroundSet.TabIndex = 29;
            this.ForegroundSet.Text = "?";
            this.ForegroundSet.Click += new System.EventHandler(this.ForegroundSet_Click);
            // 
            // button20
            // 
            this.button20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button20.Location = new System.Drawing.Point(877, 224);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(148, 44);
            this.button20.TabIndex = 28;
            this.button20.Text = "Reset to default colours - requires app restart";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // label47
            // 
            this.label47.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(893, 194);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(98, 13);
            this.label47.TabIndex = 27;
            this.label47.Text = "Background Colour";
            // 
            // label46
            // 
            this.label46.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(893, 155);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(94, 13);
            this.label46.TabIndex = 26;
            this.label46.Text = "Foreground Colour";
            // 
            // pbBackgroundColour
            // 
            this.pbBackgroundColour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbBackgroundColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBackgroundColour.Location = new System.Drawing.Point(993, 186);
            this.pbBackgroundColour.Name = "pbBackgroundColour";
            this.pbBackgroundColour.Size = new System.Drawing.Size(32, 32);
            this.pbBackgroundColour.TabIndex = 25;
            this.pbBackgroundColour.TabStop = false;
            this.pbBackgroundColour.Click += new System.EventHandler(this.pbBackgroundColour_Click);
            // 
            // pbForegroundColour
            // 
            this.pbForegroundColour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbForegroundColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbForegroundColour.Location = new System.Drawing.Point(993, 148);
            this.pbForegroundColour.Name = "pbForegroundColour";
            this.pbForegroundColour.Size = new System.Drawing.Size(32, 32);
            this.pbForegroundColour.TabIndex = 24;
            this.pbForegroundColour.TabStop = false;
            this.pbForegroundColour.Click += new System.EventHandler(this.pbForegroundColour_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(877, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(148, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(203, 150);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(167, 23);
            this.button23.TabIndex = 7;
            this.button23.Text = "How can I analyse price data?";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.ShowCommodityHelpClick);
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(30, 150);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(167, 23);
            this.button22.TabIndex = 4;
            this.button22.Text = "How does the OCR work?";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.ShowOcrHelpClick);
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.Location = new System.Drawing.Point(49, 111);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(551, 21);
            this.lblSubtitle.TabIndex = 3;
            this.lblSubtitle.Text = "-=- Built-in OCR -=- Price Analysis -=- Commander\'s Log -=- Web Control -=-";
            // 
            // lblRegulatedNoise
            // 
            this.lblRegulatedNoise.AutoSize = true;
            this.lblRegulatedNoise.Font = new System.Drawing.Font("Segoe UI", 60F, System.Drawing.FontStyle.Bold);
            this.lblRegulatedNoise.Location = new System.Drawing.Point(12, 0);
            this.lblRegulatedNoise.Name = "lblRegulatedNoise";
            this.lblRegulatedNoise.Size = new System.Drawing.Size(641, 106);
            this.lblRegulatedNoise.TabIndex = 2;
            this.lblRegulatedNoise.Text = "RegulatedNoise";
            // 
            // tabSystemData
            // 
            this.tabSystemData.Controls.Add(this.groupBox14);
            this.tabSystemData.Controls.Add(this.gbSystemSystemData);
            this.tabSystemData.Location = new System.Drawing.Point(4, 22);
            this.tabSystemData.Name = "tabSystemData";
            this.tabSystemData.Size = new System.Drawing.Size(1051, 589);
            this.tabSystemData.TabIndex = 13;
            this.tabSystemData.Text = "System Data";
            this.tabSystemData.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox14.Controls.Add(this.cmbStationStations);
            this.groupBox14.Controls.Add(this.paEconomies);
            this.groupBox14.Controls.Add(this.cmdStationCancel);
            this.groupBox14.Controls.Add(this.lblStationCountTotal);
            this.groupBox14.Controls.Add(this.label90);
            this.groupBox14.Controls.Add(this.cmdStationEdit);
            this.groupBox14.Controls.Add(this.lblStationCount);
            this.groupBox14.Controls.Add(this.label72);
            this.groupBox14.Controls.Add(this.lbStationEconomies);
            this.groupBox14.Controls.Add(this.label64);
            this.groupBox14.Controls.Add(this.txtStationUpdatedAt);
            this.groupBox14.Controls.Add(this.label73);
            this.groupBox14.Controls.Add(this.cbStationHasOutfitting);
            this.groupBox14.Controls.Add(this.cbStationHasShipyard);
            this.groupBox14.Controls.Add(this.cbStationHasRepair);
            this.groupBox14.Controls.Add(this.cbStationHasRearm);
            this.groupBox14.Controls.Add(this.cbStationHasRefuel);
            this.groupBox14.Controls.Add(this.cbStationHasCommodities);
            this.groupBox14.Controls.Add(this.cbStationHasBlackmarket);
            this.groupBox14.Controls.Add(this.label87);
            this.groupBox14.Controls.Add(this.cmdStationNew);
            this.groupBox14.Controls.Add(this.cmdStationSave);
            this.groupBox14.Controls.Add(this.cmbStationType);
            this.groupBox14.Controls.Add(this.label75);
            this.groupBox14.Controls.Add(this.cmbStationState);
            this.groupBox14.Controls.Add(this.label76);
            this.groupBox14.Controls.Add(this.cmbStationAllegiance);
            this.groupBox14.Controls.Add(this.label77);
            this.groupBox14.Controls.Add(this.cmbStationGovernment);
            this.groupBox14.Controls.Add(this.label78);
            this.groupBox14.Controls.Add(this.txtStationFaction);
            this.groupBox14.Controls.Add(this.label79);
            this.groupBox14.Controls.Add(this.txtStationDistanceToStar);
            this.groupBox14.Controls.Add(this.label80);
            this.groupBox14.Controls.Add(this.cmbStationMaxLandingPadSize);
            this.groupBox14.Controls.Add(this.label81);
            this.groupBox14.Controls.Add(this.txtStationName);
            this.groupBox14.Controls.Add(this.label85);
            this.groupBox14.Controls.Add(this.txtStationId);
            this.groupBox14.Controls.Add(this.label86);
            this.groupBox14.Controls.Add(this.lblStationRenameHint);
            this.groupBox14.Location = new System.Drawing.Point(506, 49);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(530, 524);
            this.groupBox14.TabIndex = 0;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Station Data";
            // 
            // cmbStationStations
            // 
            this.cmbStationStations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStationStations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStationStations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationStations.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationStations.Location = new System.Drawing.Point(114, 31);
            this.cmbStationStations.Name = "cmbStationStations";
            this.cmbStationStations.ReadOnly = false;
            this.cmbStationStations.Size = new System.Drawing.Size(236, 23);
            this.cmbStationStations.TabIndex = 130;
            this.cmbStationStations.Visible_ro = false;
            // 
            // paEconomies
            // 
            this.paEconomies.Controls.Add(this.cbStationEcoNone);
            this.paEconomies.Controls.Add(this.button8);
            this.paEconomies.Controls.Add(this.cbStationEcoTourism);
            this.paEconomies.Controls.Add(this.cbStationEcoTerraforming);
            this.paEconomies.Controls.Add(this.cbStationEcoService);
            this.paEconomies.Controls.Add(this.cbStationEcoRefinery);
            this.paEconomies.Controls.Add(this.cbStationEcoMilitary);
            this.paEconomies.Controls.Add(this.cbStationEcoIndustrial);
            this.paEconomies.Controls.Add(this.cbStationEcoHighTech);
            this.paEconomies.Controls.Add(this.cbStationEcoExtraction);
            this.paEconomies.Controls.Add(this.cbStationEcoAgriculture);
            this.paEconomies.Location = new System.Drawing.Point(7, 364);
            this.paEconomies.Name = "paEconomies";
            this.paEconomies.Size = new System.Drawing.Size(173, 113);
            this.paEconomies.TabIndex = 129;
            this.paEconomies.Visible = false;
            // 
            // cbStationEcoNone
            // 
            this.cbStationEcoNone.AutoSize = true;
            this.cbStationEcoNone.Location = new System.Drawing.Point(84, 75);
            this.cbStationEcoNone.Name = "cbStationEcoNone";
            this.cbStationEcoNone.ReadOnly = false;
            this.cbStationEcoNone.Size = new System.Drawing.Size(52, 17);
            this.cbStationEcoNone.TabIndex = 10;
            this.cbStationEcoNone.Text = "None";
            this.cbStationEcoNone.UseVisualStyleBackColor = false;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(107, 92);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(62, 17);
            this.button8.TabIndex = 9;
            this.button8.Text = "OK";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.cmdStationEco_OK_Click);
            // 
            // cbStationEcoTourism
            // 
            this.cbStationEcoTourism.AutoSize = true;
            this.cbStationEcoTourism.Location = new System.Drawing.Point(84, 57);
            this.cbStationEcoTourism.Name = "cbStationEcoTourism";
            this.cbStationEcoTourism.ReadOnly = false;
            this.cbStationEcoTourism.Size = new System.Drawing.Size(63, 17);
            this.cbStationEcoTourism.TabIndex = 8;
            this.cbStationEcoTourism.Text = "Tourism";
            this.cbStationEcoTourism.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoTerraforming
            // 
            this.cbStationEcoTerraforming.AutoSize = true;
            this.cbStationEcoTerraforming.Location = new System.Drawing.Point(84, 39);
            this.cbStationEcoTerraforming.Name = "cbStationEcoTerraforming";
            this.cbStationEcoTerraforming.ReadOnly = false;
            this.cbStationEcoTerraforming.Size = new System.Drawing.Size(85, 17);
            this.cbStationEcoTerraforming.TabIndex = 7;
            this.cbStationEcoTerraforming.Text = "Terraforming";
            this.cbStationEcoTerraforming.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoService
            // 
            this.cbStationEcoService.AutoSize = true;
            this.cbStationEcoService.Location = new System.Drawing.Point(84, 21);
            this.cbStationEcoService.Name = "cbStationEcoService";
            this.cbStationEcoService.ReadOnly = false;
            this.cbStationEcoService.Size = new System.Drawing.Size(62, 17);
            this.cbStationEcoService.TabIndex = 6;
            this.cbStationEcoService.Text = "Service";
            this.cbStationEcoService.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoRefinery
            // 
            this.cbStationEcoRefinery.AutoSize = true;
            this.cbStationEcoRefinery.Location = new System.Drawing.Point(84, 3);
            this.cbStationEcoRefinery.Name = "cbStationEcoRefinery";
            this.cbStationEcoRefinery.ReadOnly = false;
            this.cbStationEcoRefinery.Size = new System.Drawing.Size(65, 17);
            this.cbStationEcoRefinery.TabIndex = 5;
            this.cbStationEcoRefinery.Text = "Refinery";
            this.cbStationEcoRefinery.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoMilitary
            // 
            this.cbStationEcoMilitary.AutoSize = true;
            this.cbStationEcoMilitary.Location = new System.Drawing.Point(3, 75);
            this.cbStationEcoMilitary.Name = "cbStationEcoMilitary";
            this.cbStationEcoMilitary.ReadOnly = false;
            this.cbStationEcoMilitary.Size = new System.Drawing.Size(58, 17);
            this.cbStationEcoMilitary.TabIndex = 4;
            this.cbStationEcoMilitary.Text = "Military";
            this.cbStationEcoMilitary.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoIndustrial
            // 
            this.cbStationEcoIndustrial.AutoSize = true;
            this.cbStationEcoIndustrial.Location = new System.Drawing.Point(3, 57);
            this.cbStationEcoIndustrial.Name = "cbStationEcoIndustrial";
            this.cbStationEcoIndustrial.ReadOnly = false;
            this.cbStationEcoIndustrial.Size = new System.Drawing.Size(68, 17);
            this.cbStationEcoIndustrial.TabIndex = 3;
            this.cbStationEcoIndustrial.Text = "Industrial";
            this.cbStationEcoIndustrial.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoHighTech
            // 
            this.cbStationEcoHighTech.AutoSize = true;
            this.cbStationEcoHighTech.Location = new System.Drawing.Point(3, 39);
            this.cbStationEcoHighTech.Name = "cbStationEcoHighTech";
            this.cbStationEcoHighTech.ReadOnly = false;
            this.cbStationEcoHighTech.Size = new System.Drawing.Size(76, 17);
            this.cbStationEcoHighTech.TabIndex = 2;
            this.cbStationEcoHighTech.Text = "High Tech";
            this.cbStationEcoHighTech.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoExtraction
            // 
            this.cbStationEcoExtraction.AutoSize = true;
            this.cbStationEcoExtraction.Location = new System.Drawing.Point(3, 21);
            this.cbStationEcoExtraction.Name = "cbStationEcoExtraction";
            this.cbStationEcoExtraction.ReadOnly = false;
            this.cbStationEcoExtraction.Size = new System.Drawing.Size(73, 17);
            this.cbStationEcoExtraction.TabIndex = 1;
            this.cbStationEcoExtraction.Text = "Extraction";
            this.cbStationEcoExtraction.UseVisualStyleBackColor = false;
            // 
            // cbStationEcoAgriculture
            // 
            this.cbStationEcoAgriculture.AutoSize = true;
            this.cbStationEcoAgriculture.Location = new System.Drawing.Point(3, 3);
            this.cbStationEcoAgriculture.Name = "cbStationEcoAgriculture";
            this.cbStationEcoAgriculture.ReadOnly = false;
            this.cbStationEcoAgriculture.Size = new System.Drawing.Size(76, 17);
            this.cbStationEcoAgriculture.TabIndex = 0;
            this.cbStationEcoAgriculture.Text = "Agriculture";
            this.cbStationEcoAgriculture.UseVisualStyleBackColor = false;
            // 
            // cmdStationCancel
            // 
            this.cmdStationCancel.Location = new System.Drawing.Point(169, 473);
            this.cmdStationCancel.Name = "cmdStationCancel";
            this.cmdStationCancel.Size = new System.Drawing.Size(110, 23);
            this.cmdStationCancel.TabIndex = 21;
            this.cmdStationCancel.Text = "Cancel";
            this.cmdStationCancel.UseVisualStyleBackColor = true;
            this.cmdStationCancel.Click += new System.EventHandler(this.cmdStationCancel_Click);
            // 
            // lblStationCountTotal
            // 
            this.lblStationCountTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStationCountTotal.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStationCountTotal.Location = new System.Drawing.Point(419, 50);
            this.lblStationCountTotal.Name = "lblStationCountTotal";
            this.lblStationCountTotal.Size = new System.Drawing.Size(104, 25);
            this.lblStationCountTotal.TabIndex = 128;
            this.lblStationCountTotal.Text = "0";
            this.lblStationCountTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label90.Location = new System.Drawing.Point(421, 16);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(102, 26);
            this.label90.TabIndex = 127;
            this.label90.Text = "Number Of Stations \r\nIn Database : ";
            // 
            // cmdStationEdit
            // 
            this.cmdStationEdit.Location = new System.Drawing.Point(168, 435);
            this.cmdStationEdit.Name = "cmdStationEdit";
            this.cmdStationEdit.Size = new System.Drawing.Size(110, 23);
            this.cmdStationEdit.TabIndex = 19;
            this.cmdStationEdit.Text = "Edit Station";
            this.cmdStationEdit.UseVisualStyleBackColor = true;
            this.cmdStationEdit.Click += new System.EventHandler(this.cmdStationEdit_Click);
            // 
            // lblStationCount
            // 
            this.lblStationCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStationCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStationCount.Location = new System.Drawing.Point(419, 116);
            this.lblStationCount.Name = "lblStationCount";
            this.lblStationCount.Size = new System.Drawing.Size(104, 25);
            this.lblStationCount.TabIndex = 123;
            this.lblStationCount.Text = "0";
            this.lblStationCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label72.Location = new System.Drawing.Point(421, 82);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(102, 26);
            this.label72.TabIndex = 122;
            this.label72.Text = "Number Of Stations \r\nIn System : ";
            // 
            // lbStationEconomies
            // 
            this.lbStationEconomies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStationEconomies.FormattingEnabled = true;
            this.lbStationEconomies.ItemHeight = 15;
            this.lbStationEconomies.Location = new System.Drawing.Point(127, 334);
            this.lbStationEconomies.Name = "lbStationEconomies";
            this.lbStationEconomies.Size = new System.Drawing.Size(151, 79);
            this.lbStationEconomies.TabIndex = 10;
            this.lbStationEconomies.Tag = "ReadOnly";
            this.lbStationEconomies.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lbStationEconomies_MouseClick);
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label64.Location = new System.Drawing.Point(22, 335);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(57, 13);
            this.label64.TabIndex = 119;
            this.label64.Text = "Econonies";
            // 
            // txtStationUpdatedAt
            // 
            this.txtStationUpdatedAt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationUpdatedAt.Location = new System.Drawing.Point(328, 372);
            this.txtStationUpdatedAt.Name = "txtStationUpdatedAt";
            this.txtStationUpdatedAt.ReadOnly = true;
            this.txtStationUpdatedAt.Size = new System.Drawing.Size(151, 21);
            this.txtStationUpdatedAt.TabIndex = 18;
            this.txtStationUpdatedAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label73.Location = new System.Drawing.Point(325, 356);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(48, 13);
            this.label73.TabIndex = 116;
            this.label73.Text = "Updated";
            // 
            // cbStationHasOutfitting
            // 
            this.cbStationHasOutfitting.AutoSize = true;
            this.cbStationHasOutfitting.Location = new System.Drawing.Point(328, 197);
            this.cbStationHasOutfitting.Name = "cbStationHasOutfitting";
            this.cbStationHasOutfitting.ReadOnly = false;
            this.cbStationHasOutfitting.Size = new System.Drawing.Size(68, 17);
            this.cbStationHasOutfitting.TabIndex = 13;
            this.cbStationHasOutfitting.Text = "Outfitting";
            this.cbStationHasOutfitting.UseVisualStyleBackColor = true;
            // 
            // cbStationHasShipyard
            // 
            this.cbStationHasShipyard.AutoSize = true;
            this.cbStationHasShipyard.Location = new System.Drawing.Point(328, 220);
            this.cbStationHasShipyard.Name = "cbStationHasShipyard";
            this.cbStationHasShipyard.ReadOnly = false;
            this.cbStationHasShipyard.Size = new System.Drawing.Size(67, 17);
            this.cbStationHasShipyard.TabIndex = 14;
            this.cbStationHasShipyard.Text = "Shipyard";
            this.cbStationHasShipyard.UseVisualStyleBackColor = true;
            // 
            // cbStationHasRepair
            // 
            this.cbStationHasRepair.AutoSize = true;
            this.cbStationHasRepair.Location = new System.Drawing.Point(424, 197);
            this.cbStationHasRepair.Name = "cbStationHasRepair";
            this.cbStationHasRepair.ReadOnly = false;
            this.cbStationHasRepair.Size = new System.Drawing.Size(57, 17);
            this.cbStationHasRepair.TabIndex = 17;
            this.cbStationHasRepair.Text = "Repair";
            this.cbStationHasRepair.UseVisualStyleBackColor = true;
            // 
            // cbStationHasRearm
            // 
            this.cbStationHasRearm.AutoSize = true;
            this.cbStationHasRearm.Location = new System.Drawing.Point(424, 151);
            this.cbStationHasRearm.Name = "cbStationHasRearm";
            this.cbStationHasRearm.ReadOnly = false;
            this.cbStationHasRearm.Size = new System.Drawing.Size(61, 17);
            this.cbStationHasRearm.TabIndex = 15;
            this.cbStationHasRearm.Text = "Re-Arm";
            this.cbStationHasRearm.UseVisualStyleBackColor = true;
            // 
            // cbStationHasRefuel
            // 
            this.cbStationHasRefuel.AutoSize = true;
            this.cbStationHasRefuel.Location = new System.Drawing.Point(424, 174);
            this.cbStationHasRefuel.Name = "cbStationHasRefuel";
            this.cbStationHasRefuel.ReadOnly = false;
            this.cbStationHasRefuel.Size = new System.Drawing.Size(57, 17);
            this.cbStationHasRefuel.TabIndex = 16;
            this.cbStationHasRefuel.Text = "Refuel";
            this.cbStationHasRefuel.UseVisualStyleBackColor = true;
            // 
            // cbStationHasCommodities
            // 
            this.cbStationHasCommodities.AutoSize = true;
            this.cbStationHasCommodities.Location = new System.Drawing.Point(328, 151);
            this.cbStationHasCommodities.Name = "cbStationHasCommodities";
            this.cbStationHasCommodities.ReadOnly = false;
            this.cbStationHasCommodities.Size = new System.Drawing.Size(85, 17);
            this.cbStationHasCommodities.TabIndex = 11;
            this.cbStationHasCommodities.Text = "Commodities";
            this.cbStationHasCommodities.UseVisualStyleBackColor = false;
            // 
            // cbStationHasBlackmarket
            // 
            this.cbStationHasBlackmarket.AutoSize = true;
            this.cbStationHasBlackmarket.Location = new System.Drawing.Point(328, 174);
            this.cbStationHasBlackmarket.Name = "cbStationHasBlackmarket";
            this.cbStationHasBlackmarket.ReadOnly = false;
            this.cbStationHasBlackmarket.Size = new System.Drawing.Size(85, 17);
            this.cbStationHasBlackmarket.TabIndex = 12;
            this.cbStationHasBlackmarket.Text = "Blackmarket";
            this.cbStationHasBlackmarket.UseVisualStyleBackColor = true;
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label87.Location = new System.Drawing.Point(22, 36);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(40, 13);
            this.label87.TabIndex = 95;
            this.label87.Text = "Station";
            // 
            // cmdStationNew
            // 
            this.cmdStationNew.Location = new System.Drawing.Point(285, 435);
            this.cmdStationNew.Name = "cmdStationNew";
            this.cmdStationNew.Size = new System.Drawing.Size(110, 23);
            this.cmdStationNew.TabIndex = 20;
            this.cmdStationNew.Text = "Add Station";
            this.cmdStationNew.UseVisualStyleBackColor = true;
            this.cmdStationNew.Click += new System.EventHandler(this.cmdStationNew_Click);
            // 
            // cmdStationSave
            // 
            this.cmdStationSave.Location = new System.Drawing.Point(285, 473);
            this.cmdStationSave.Name = "cmdStationSave";
            this.cmdStationSave.Size = new System.Drawing.Size(110, 23);
            this.cmdStationSave.TabIndex = 22;
            this.cmdStationSave.Text = "Save Changes";
            this.cmdStationSave.UseVisualStyleBackColor = true;
            this.cmdStationSave.Click += new System.EventHandler(this.cmdStationSave_Click);
            // 
            // cmbStationType
            // 
            this.cmbStationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationType.Location = new System.Drawing.Point(127, 202);
            this.cmbStationType.Name = "cmbStationType";
            this.cmbStationType.ReadOnly = false;
            this.cmbStationType.Size = new System.Drawing.Size(151, 23);
            this.cmbStationType.TabIndex = 9;
            this.cmbStationType.Visible_ro = false;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label75.Location = new System.Drawing.Point(22, 205);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(31, 13);
            this.label75.TabIndex = 86;
            this.label75.Text = "Type";
            // 
            // cmbStationState
            // 
            this.cmbStationState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationState.Location = new System.Drawing.Point(127, 281);
            this.cmbStationState.Name = "cmbStationState";
            this.cmbStationState.ReadOnly = false;
            this.cmbStationState.Size = new System.Drawing.Size(151, 23);
            this.cmbStationState.TabIndex = 8;
            this.cmbStationState.Visible_ro = false;
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label76.Location = new System.Drawing.Point(22, 284);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(32, 13);
            this.label76.TabIndex = 84;
            this.label76.Text = "State";
            // 
            // cmbStationAllegiance
            // 
            this.cmbStationAllegiance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationAllegiance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationAllegiance.Location = new System.Drawing.Point(127, 255);
            this.cmbStationAllegiance.Name = "cmbStationAllegiance";
            this.cmbStationAllegiance.ReadOnly = false;
            this.cmbStationAllegiance.Size = new System.Drawing.Size(151, 23);
            this.cmbStationAllegiance.TabIndex = 7;
            this.cmbStationAllegiance.Visible_ro = false;
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label77.Location = new System.Drawing.Point(22, 258);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(56, 13);
            this.label77.TabIndex = 82;
            this.label77.Text = "Allegiance";
            // 
            // cmbStationGovernment
            // 
            this.cmbStationGovernment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationGovernment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationGovernment.Location = new System.Drawing.Point(127, 229);
            this.cmbStationGovernment.Name = "cmbStationGovernment";
            this.cmbStationGovernment.ReadOnly = false;
            this.cmbStationGovernment.Size = new System.Drawing.Size(151, 23);
            this.cmbStationGovernment.TabIndex = 6;
            this.cmbStationGovernment.Visible_ro = false;
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label78.Location = new System.Drawing.Point(22, 232);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(65, 13);
            this.label78.TabIndex = 80;
            this.label78.Text = "Government";
            // 
            // txtStationFaction
            // 
            this.txtStationFaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationFaction.Location = new System.Drawing.Point(127, 308);
            this.txtStationFaction.Name = "txtStationFaction";
            this.txtStationFaction.Size = new System.Drawing.Size(352, 21);
            this.txtStationFaction.TabIndex = 5;
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label79.Location = new System.Drawing.Point(22, 313);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(42, 13);
            this.label79.TabIndex = 78;
            this.label79.Text = "Faction";
            // 
            // txtStationDistanceToStar
            // 
            this.txtStationDistanceToStar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationDistanceToStar.Location = new System.Drawing.Point(127, 147);
            this.txtStationDistanceToStar.Name = "txtStationDistanceToStar";
            this.txtStationDistanceToStar.Size = new System.Drawing.Size(68, 21);
            this.txtStationDistanceToStar.TabIndex = 3;
            this.txtStationDistanceToStar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label80.Location = new System.Drawing.Point(22, 152);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(103, 13);
            this.label80.TabIndex = 76;
            this.label80.Text = "Distance To Star (ls)";
            // 
            // cmbStationMaxLandingPadSize
            // 
            this.cmbStationMaxLandingPadSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationMaxLandingPadSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationMaxLandingPadSize.Location = new System.Drawing.Point(127, 174);
            this.cmbStationMaxLandingPadSize.Name = "cmbStationMaxLandingPadSize";
            this.cmbStationMaxLandingPadSize.ReadOnly = false;
            this.cmbStationMaxLandingPadSize.Size = new System.Drawing.Size(68, 23);
            this.cmbStationMaxLandingPadSize.TabIndex = 4;
            this.cmbStationMaxLandingPadSize.Visible_ro = false;
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label81.Location = new System.Drawing.Point(22, 179);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(88, 13);
            this.label81.TabIndex = 74;
            this.label81.Text = "max. Landingpad";
            // 
            // txtStationName
            // 
            this.txtStationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationName.Location = new System.Drawing.Point(114, 89);
            this.txtStationName.Name = "txtStationName";
            this.txtStationName.ReadOnly = true;
            this.txtStationName.Size = new System.Drawing.Size(236, 21);
            this.txtStationName.TabIndex = 2;
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label85.Location = new System.Drawing.Point(22, 92);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(35, 13);
            this.label85.TabIndex = 66;
            this.label85.Text = "Name";
            // 
            // txtStationId
            // 
            this.txtStationId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationId.Location = new System.Drawing.Point(114, 63);
            this.txtStationId.Name = "txtStationId";
            this.txtStationId.ReadOnly = true;
            this.txtStationId.Size = new System.Drawing.Size(66, 20);
            this.txtStationId.TabIndex = 1;
            this.txtStationId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label86.Location = new System.Drawing.Point(22, 66);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(18, 13);
            this.label86.TabIndex = 64;
            this.label86.Text = "ID";
            // 
            // lblStationRenameHint
            // 
            this.lblStationRenameHint.AutoSize = true;
            this.lblStationRenameHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStationRenameHint.Location = new System.Drawing.Point(23, 111);
            this.lblStationRenameHint.Name = "lblStationRenameHint";
            this.lblStationRenameHint.Size = new System.Drawing.Size(280, 12);
            this.lblStationRenameHint.TabIndex = 2;
            this.lblStationRenameHint.Text = "Name of station not editable because it comes from the EDDB data.";
            this.lblStationRenameHint.Visible = false;
            // 
            // gbSystemSystemData
            // 
            this.gbSystemSystemData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSystemSystemData.Controls.Add(this.cmdTest);
            this.gbSystemSystemData.Controls.Add(this.cmdSystemCancel);
            this.gbSystemSystemData.Controls.Add(this.lblSystemCountTotal);
            this.gbSystemSystemData.Controls.Add(this.label82);
            this.gbSystemSystemData.Controls.Add(this.label84);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemsAllSystems);
            this.gbSystemSystemData.Controls.Add(this.label88);
            this.gbSystemSystemData.Controls.Add(this.cmdSystemEdit);
            this.gbSystemSystemData.Controls.Add(this.cbSystemNeedsPermit);
            this.gbSystemSystemData.Controls.Add(this.cmdSystemNew);
            this.gbSystemSystemData.Controls.Add(this.cmdSystemSave);
            this.gbSystemSystemData.Controls.Add(this.txtSystemUpdatedAt);
            this.gbSystemSystemData.Controls.Add(this.label63);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemPrimaryEconomy);
            this.gbSystemSystemData.Controls.Add(this.label65);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemSecurity);
            this.gbSystemSystemData.Controls.Add(this.label66);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemState);
            this.gbSystemSystemData.Controls.Add(this.label67);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemAllegiance);
            this.gbSystemSystemData.Controls.Add(this.label68);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemGovernment);
            this.gbSystemSystemData.Controls.Add(this.label69);
            this.gbSystemSystemData.Controls.Add(this.txtSystemPopulation);
            this.gbSystemSystemData.Controls.Add(this.label62);
            this.gbSystemSystemData.Controls.Add(this.txtSystemFaction);
            this.gbSystemSystemData.Controls.Add(this.label61);
            this.gbSystemSystemData.Controls.Add(this.txtSystemZ);
            this.gbSystemSystemData.Controls.Add(this.label60);
            this.gbSystemSystemData.Controls.Add(this.txtSystemY);
            this.gbSystemSystemData.Controls.Add(this.label59);
            this.gbSystemSystemData.Controls.Add(this.txtSystemX);
            this.gbSystemSystemData.Controls.Add(this.label58);
            this.gbSystemSystemData.Controls.Add(this.txtSystemName);
            this.gbSystemSystemData.Controls.Add(this.label57);
            this.gbSystemSystemData.Controls.Add(this.txtSystemId);
            this.gbSystemSystemData.Controls.Add(this.label56);
            this.gbSystemSystemData.Controls.Add(this.lblSystemRenameHint);
            this.gbSystemSystemData.Location = new System.Drawing.Point(14, 49);
            this.gbSystemSystemData.Name = "gbSystemSystemData";
            this.gbSystemSystemData.Size = new System.Drawing.Size(486, 524);
            this.gbSystemSystemData.TabIndex = 0;
            this.gbSystemSystemData.TabStop = false;
            this.gbSystemSystemData.Text = "System Data";
            // 
            // cmdTest
            // 
            this.cmdTest.Location = new System.Drawing.Point(416, 499);
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.Size = new System.Drawing.Size(75, 23);
            this.cmdTest.TabIndex = 39;
            this.cmdTest.Text = "Test";
            this.cmdTest.UseVisualStyleBackColor = true;
            this.cmdTest.Visible = false;
            this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
            // 
            // cmdSystemCancel
            // 
            this.cmdSystemCancel.Location = new System.Drawing.Point(156, 473);
            this.cmdSystemCancel.Name = "cmdSystemCancel";
            this.cmdSystemCancel.Size = new System.Drawing.Size(110, 23);
            this.cmdSystemCancel.TabIndex = 17;
            this.cmdSystemCancel.Text = "Cancel";
            this.cmdSystemCancel.UseVisualStyleBackColor = false;
            this.cmdSystemCancel.Click += new System.EventHandler(this.cmdSystemCancel_Click);
            // 
            // lblSystemCountTotal
            // 
            this.lblSystemCountTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemCountTotal.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblSystemCountTotal.Location = new System.Drawing.Point(374, 50);
            this.lblSystemCountTotal.Name = "lblSystemCountTotal";
            this.lblSystemCountTotal.Size = new System.Drawing.Size(104, 25);
            this.lblSystemCountTotal.TabIndex = 130;
            this.lblSystemCountTotal.Text = "0";
            this.lblSystemCountTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label82.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label82.Location = new System.Drawing.Point(519, 37);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(16, 16);
            this.label82.TabIndex = 129;
            this.label82.Text = "0";
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label84.Location = new System.Drawing.Point(377, 16);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(103, 26);
            this.label84.TabIndex = 128;
            this.label84.Text = "Number Of Systems \r\nIn Database : ";
            // 
            // cmbSystemsAllSystems
            // 
            this.cmbSystemsAllSystems.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSystemsAllSystems.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSystemsAllSystems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemsAllSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemsAllSystems.Location = new System.Drawing.Point(89, 33);
            this.cmbSystemsAllSystems.Name = "cmbSystemsAllSystems";
            this.cmbSystemsAllSystems.ReadOnly = false;
            this.cmbSystemsAllSystems.Size = new System.Drawing.Size(254, 23);
            this.cmbSystemsAllSystems.TabIndex = 0;
            this.cmbSystemsAllSystems.Visible_ro = false;
            this.cmbSystemsAllSystems.SelectedIndexChanged += new System.EventHandler(this.cmbAllStations_SelectedIndexChanged);
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label88.Location = new System.Drawing.Point(25, 36);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(41, 13);
            this.label88.TabIndex = 126;
            this.label88.Text = "System";
            // 
            // cmdSystemEdit
            // 
            this.cmdSystemEdit.Location = new System.Drawing.Point(156, 435);
            this.cmdSystemEdit.Name = "cmdSystemEdit";
            this.cmdSystemEdit.Size = new System.Drawing.Size(110, 23);
            this.cmdSystemEdit.TabIndex = 15;
            this.cmdSystemEdit.Text = "Edit System";
            this.cmdSystemEdit.UseVisualStyleBackColor = false;
            this.cmdSystemEdit.Click += new System.EventHandler(this.cmdSystemEdit_Click);
            // 
            // cbSystemNeedsPermit
            // 
            this.cbSystemNeedsPermit.AutoSize = true;
            this.cbSystemNeedsPermit.Location = new System.Drawing.Point(89, 259);
            this.cbSystemNeedsPermit.Name = "cbSystemNeedsPermit";
            this.cbSystemNeedsPermit.ReadOnly = false;
            this.cbSystemNeedsPermit.Size = new System.Drawing.Size(89, 17);
            this.cbSystemNeedsPermit.TabIndex = 6;
            this.cbSystemNeedsPermit.Text = "Needs Permit";
            this.cbSystemNeedsPermit.UseVisualStyleBackColor = true;
            // 
            // cmdSystemNew
            // 
            this.cmdSystemNew.Location = new System.Drawing.Point(272, 435);
            this.cmdSystemNew.Name = "cmdSystemNew";
            this.cmdSystemNew.Size = new System.Drawing.Size(110, 23);
            this.cmdSystemNew.TabIndex = 16;
            this.cmdSystemNew.Text = "Add System";
            this.cmdSystemNew.UseVisualStyleBackColor = true;
            this.cmdSystemNew.Click += new System.EventHandler(this.cmdSystemNew_Click);
            // 
            // cmdSystemSave
            // 
            this.cmdSystemSave.Location = new System.Drawing.Point(272, 473);
            this.cmdSystemSave.Name = "cmdSystemSave";
            this.cmdSystemSave.Size = new System.Drawing.Size(110, 23);
            this.cmdSystemSave.TabIndex = 18;
            this.cmdSystemSave.Text = "Save Changes";
            this.cmdSystemSave.UseVisualStyleBackColor = true;
            this.cmdSystemSave.Click += new System.EventHandler(this.cmdSystemSave_Click);
            // 
            // txtSystemUpdatedAt
            // 
            this.txtSystemUpdatedAt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemUpdatedAt.Location = new System.Drawing.Point(295, 372);
            this.txtSystemUpdatedAt.Name = "txtSystemUpdatedAt";
            this.txtSystemUpdatedAt.ReadOnly = true;
            this.txtSystemUpdatedAt.Size = new System.Drawing.Size(151, 21);
            this.txtSystemUpdatedAt.TabIndex = 14;
            this.txtSystemUpdatedAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label63.Location = new System.Drawing.Point(203, 375);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(48, 13);
            this.label63.TabIndex = 58;
            this.label63.Text = "Updated";
            // 
            // cmbSystemPrimaryEconomy
            // 
            this.cmbSystemPrimaryEconomy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemPrimaryEconomy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemPrimaryEconomy.Location = new System.Drawing.Point(295, 201);
            this.cmbSystemPrimaryEconomy.Name = "cmbSystemPrimaryEconomy";
            this.cmbSystemPrimaryEconomy.ReadOnly = false;
            this.cmbSystemPrimaryEconomy.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemPrimaryEconomy.TabIndex = 13;
            this.cmbSystemPrimaryEconomy.Visible_ro = false;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label65.Location = new System.Drawing.Point(203, 204);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(88, 13);
            this.label65.TabIndex = 54;
            this.label65.Text = "Primary Economy";
            // 
            // cmbSystemSecurity
            // 
            this.cmbSystemSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemSecurity.Location = new System.Drawing.Point(295, 174);
            this.cmbSystemSecurity.Name = "cmbSystemSecurity";
            this.cmbSystemSecurity.ReadOnly = false;
            this.cmbSystemSecurity.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemSecurity.TabIndex = 8;
            this.cmbSystemSecurity.Visible_ro = false;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label66.Location = new System.Drawing.Point(203, 179);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(45, 13);
            this.label66.TabIndex = 52;
            this.label66.Text = "Security";
            // 
            // cmbSystemState
            // 
            this.cmbSystemState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemState.Location = new System.Drawing.Point(295, 283);
            this.cmbSystemState.Name = "cmbSystemState";
            this.cmbSystemState.ReadOnly = false;
            this.cmbSystemState.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemState.TabIndex = 12;
            this.cmbSystemState.Visible_ro = false;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label67.Location = new System.Drawing.Point(203, 286);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(32, 13);
            this.label67.TabIndex = 50;
            this.label67.Text = "State";
            // 
            // cmbSystemAllegiance
            // 
            this.cmbSystemAllegiance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemAllegiance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemAllegiance.Location = new System.Drawing.Point(295, 256);
            this.cmbSystemAllegiance.Name = "cmbSystemAllegiance";
            this.cmbSystemAllegiance.ReadOnly = false;
            this.cmbSystemAllegiance.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemAllegiance.TabIndex = 11;
            this.cmbSystemAllegiance.Visible_ro = false;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label68.Location = new System.Drawing.Point(203, 260);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(56, 13);
            this.label68.TabIndex = 48;
            this.label68.Text = "Allegiance";
            // 
            // cmbSystemGovernment
            // 
            this.cmbSystemGovernment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemGovernment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemGovernment.Location = new System.Drawing.Point(295, 228);
            this.cmbSystemGovernment.Name = "cmbSystemGovernment";
            this.cmbSystemGovernment.ReadOnly = false;
            this.cmbSystemGovernment.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemGovernment.TabIndex = 10;
            this.cmbSystemGovernment.Visible_ro = false;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label69.Location = new System.Drawing.Point(203, 231);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(65, 13);
            this.label69.TabIndex = 46;
            this.label69.Text = "Government";
            // 
            // txtSystemPopulation
            // 
            this.txtSystemPopulation.AsciiOnly = true;
            this.txtSystemPopulation.Culture = new System.Globalization.CultureInfo("");
            this.txtSystemPopulation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemPopulation.HidePromptOnLeave = true;
            this.txtSystemPopulation.Location = new System.Drawing.Point(295, 147);
            this.txtSystemPopulation.Name = "txtSystemPopulation";
            this.txtSystemPopulation.Size = new System.Drawing.Size(151, 21);
            this.txtSystemPopulation.TabIndex = 7;
            this.txtSystemPopulation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSystemPopulation.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label62.Location = new System.Drawing.Point(203, 152);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(57, 13);
            this.label62.TabIndex = 44;
            this.label62.Text = "Population";
            // 
            // txtSystemFaction
            // 
            this.txtSystemFaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemFaction.Location = new System.Drawing.Point(89, 310);
            this.txtSystemFaction.Name = "txtSystemFaction";
            this.txtSystemFaction.Size = new System.Drawing.Size(357, 21);
            this.txtSystemFaction.TabIndex = 9;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label61.Location = new System.Drawing.Point(41, 313);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(42, 13);
            this.label61.TabIndex = 42;
            this.label61.Text = "Faction";
            // 
            // txtSystemZ
            // 
            this.txtSystemZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemZ.Location = new System.Drawing.Point(89, 203);
            this.txtSystemZ.Name = "txtSystemZ";
            this.txtSystemZ.Size = new System.Drawing.Size(92, 21);
            this.txtSystemZ.TabIndex = 5;
            this.txtSystemZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label60.Location = new System.Drawing.Point(71, 208);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(12, 13);
            this.label60.TabIndex = 40;
            this.label60.Text = "z";
            // 
            // txtSystemY
            // 
            this.txtSystemY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemY.Location = new System.Drawing.Point(89, 174);
            this.txtSystemY.Name = "txtSystemY";
            this.txtSystemY.Size = new System.Drawing.Size(92, 21);
            this.txtSystemY.TabIndex = 4;
            this.txtSystemY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label59.Location = new System.Drawing.Point(71, 179);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(12, 13);
            this.label59.TabIndex = 38;
            this.label59.Text = "y";
            // 
            // txtSystemX
            // 
            this.txtSystemX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemX.Location = new System.Drawing.Point(89, 147);
            this.txtSystemX.Name = "txtSystemX";
            this.txtSystemX.Size = new System.Drawing.Size(92, 21);
            this.txtSystemX.TabIndex = 3;
            this.txtSystemX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label58.Location = new System.Drawing.Point(71, 152);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(12, 13);
            this.label58.TabIndex = 36;
            this.label58.Text = "x";
            // 
            // txtSystemName
            // 
            this.txtSystemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemName.Location = new System.Drawing.Point(89, 89);
            this.txtSystemName.Name = "txtSystemName";
            this.txtSystemName.ReadOnly = true;
            this.txtSystemName.Size = new System.Drawing.Size(254, 21);
            this.txtSystemName.TabIndex = 2;
            this.txtSystemName.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label57.Location = new System.Drawing.Point(25, 94);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(35, 13);
            this.label57.TabIndex = 34;
            this.label57.Text = "Name";
            // 
            // txtSystemId
            // 
            this.txtSystemId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemId.Location = new System.Drawing.Point(89, 63);
            this.txtSystemId.Name = "txtSystemId";
            this.txtSystemId.ReadOnly = true;
            this.txtSystemId.Size = new System.Drawing.Size(66, 20);
            this.txtSystemId.TabIndex = 1;
            this.txtSystemId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label56.Location = new System.Drawing.Point(25, 66);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(18, 13);
            this.label56.TabIndex = 32;
            this.label56.Text = "ID";
            // 
            // lblSystemRenameHint
            // 
            this.lblSystemRenameHint.AutoSize = true;
            this.lblSystemRenameHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemRenameHint.Location = new System.Drawing.Point(26, 111);
            this.lblSystemRenameHint.Name = "lblSystemRenameHint";
            this.lblSystemRenameHint.Size = new System.Drawing.Size(283, 12);
            this.lblSystemRenameHint.TabIndex = 63;
            this.lblSystemRenameHint.Text = "Name of system not editable because it comes from the EDDB data.";
            this.lblSystemRenameHint.Visible = false;
            // 
            // tabPriceAnalysis
            // 
            this.tabPriceAnalysis.Controls.Add(this.label55);
            this.tabPriceAnalysis.Controls.Add(this.cmbStationToStar);
            this.tabPriceAnalysis.Controls.Add(this.gbSorting);
            this.tabPriceAnalysis.Controls.Add(this.cbStationToStar);
            this.tabPriceAnalysis.Controls.Add(this.cbIncludeWithinRegionOfStation);
            this.tabPriceAnalysis.Controls.Add(this.label43);
            this.tabPriceAnalysis.Controls.Add(this.cbLimitLightYears);
            this.tabPriceAnalysis.Controls.Add(this.cmbLightYears);
            this.tabPriceAnalysis.Controls.Add(this.tabControl2);
            this.tabPriceAnalysis.Controls.Add(this.bShowStationRestrictionAtStarchartDotClub);
            this.tabPriceAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabPriceAnalysis.Name = "tabPriceAnalysis";
            this.tabPriceAnalysis.Size = new System.Drawing.Size(1051, 589);
            this.tabPriceAnalysis.TabIndex = 10;
            this.tabPriceAnalysis.Text = "Price Analysis";
            this.tabPriceAnalysis.UseVisualStyleBackColor = true;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(222, 32);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(14, 13);
            this.label55.TabIndex = 18;
            this.label55.Text = "ls";
            // 
            // cmbStationToStar
            // 
            this.cmbStationToStar.FormattingEnabled = true;
            this.cmbStationToStar.Items.AddRange(new object[] {
            "50",
            "100",
            "500",
            "1000",
            "2000",
            "5000"});
            this.cmbStationToStar.Location = new System.Drawing.Point(168, 29);
            this.cmbStationToStar.Name = "cmbStationToStar";
            this.cmbStationToStar.Size = new System.Drawing.Size(48, 21);
            this.cmbStationToStar.TabIndex = 16;
            this.cmbStationToStar.SelectedIndexChanged += new System.EventHandler(this.cmbStationToStar_SelectedIndexChanged);
            this.cmbStationToStar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbStationToStarInput_KeyPress);
            // 
            // gbSorting
            // 
            this.gbSorting.Controls.Add(this.txtlastStationCount);
            this.gbSorting.Controls.Add(this.cblastVisitedFirst);
            this.gbSorting.Controls.Add(this.rbSortByDistance);
            this.gbSorting.Controls.Add(this.rbSortByStation);
            this.gbSorting.Controls.Add(this.rbSortBySystem);
            this.gbSorting.Location = new System.Drawing.Point(543, 3);
            this.gbSorting.Name = "gbSorting";
            this.gbSorting.Size = new System.Drawing.Size(253, 52);
            this.gbSorting.TabIndex = 15;
            this.gbSorting.TabStop = false;
            this.gbSorting.Text = "order of entries";
            // 
            // txtlastStationCount
            // 
            this.txtlastStationCount.Location = new System.Drawing.Point(157, 29);
            this.txtlastStationCount.MaxLength = 2;
            this.txtlastStationCount.Name = "txtlastStationCount";
            this.txtlastStationCount.Size = new System.Drawing.Size(19, 20);
            this.txtlastStationCount.TabIndex = 26;
            this.txtlastStationCount.Text = "4";
            this.txtlastStationCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtlastStationCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtlastStationCount_KeyPress);
            this.txtlastStationCount.LostFocus += new System.EventHandler(this.txtlastStationCount_LostFocus);
            // 
            // cblastVisitedFirst
            // 
            this.cblastVisitedFirst.AutoSize = true;
            this.cblastVisitedFirst.Location = new System.Drawing.Point(115, 32);
            this.cblastVisitedFirst.Name = "cblastVisitedFirst";
            this.cblastVisitedFirst.Size = new System.Drawing.Size(130, 17);
            this.cblastVisitedFirst.TabIndex = 25;
            this.cblastVisitedFirst.Text = "last           stations first";
            this.cblastVisitedFirst.UseVisualStyleBackColor = true;
            this.cblastVisitedFirst.CheckedChanged += new System.EventHandler(this.cblastVisitedFirst_CheckedChanged);
            // 
            // cbStationToStar
            // 
            this.cbStationToStar.AutoSize = true;
            this.cbStationToStar.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbStationToStar.Location = new System.Drawing.Point(1, 31);
            this.cbStationToStar.Name = "cbStationToStar";
            this.cbStationToStar.Size = new System.Drawing.Size(164, 17);
            this.cbStationToStar.TabIndex = 17;
            this.cbStationToStar.Text = "Max. Station-to-Star Distance\r\n";
            this.cbStationToStar.UseVisualStyleBackColor = true;
            this.cbStationToStar.CheckedChanged += new System.EventHandler(this.cbStationToStar_CheckedChanged);
            // 
            // cbIncludeWithinRegionOfStation
            // 
            this.cbIncludeWithinRegionOfStation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbIncludeWithinRegionOfStation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbIncludeWithinRegionOfStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIncludeWithinRegionOfStation.FormattingEnabled = true;
            this.cbIncludeWithinRegionOfStation.Location = new System.Drawing.Point(294, 6);
            this.cbIncludeWithinRegionOfStation.Name = "cbIncludeWithinRegionOfStation";
            this.cbIncludeWithinRegionOfStation.Size = new System.Drawing.Size(200, 21);
            this.cbIncludeWithinRegionOfStation.TabIndex = 14;
            this.cbIncludeWithinRegionOfStation.SelectionChangeCommitted += new System.EventHandler(this.cbIncludeWithinRegionOfStation_SelectionChangeCommitted);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(222, 9);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(66, 13);
            this.label43.TabIndex = 13;
            this.label43.Text = "light years of";
            // 
            // cbLimitLightYears
            // 
            this.cbLimitLightYears.AutoSize = true;
            this.cbLimitLightYears.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbLimitLightYears.Location = new System.Drawing.Point(9, 8);
            this.cbLimitLightYears.Name = "cbLimitLightYears";
            this.cbLimitLightYears.Size = new System.Drawing.Size(156, 17);
            this.cbLimitLightYears.TabIndex = 12;
            this.cbLimitLightYears.Text = "Only include stations within ";
            this.cbLimitLightYears.UseVisualStyleBackColor = true;
            this.cbLimitLightYears.CheckedChanged += new System.EventHandler(this.checkboxLightYears_CheckedChanged);
            // 
            // cmbLightYears
            // 
            this.cmbLightYears.FormattingEnabled = true;
            this.cmbLightYears.Items.AddRange(new object[] {
            "10",
            "25",
            "50",
            "100",
            "200",
            "1000"});
            this.cmbLightYears.Location = new System.Drawing.Point(168, 6);
            this.cmbLightYears.Name = "cmbLightYears";
            this.cmbLightYears.Size = new System.Drawing.Size(48, 21);
            this.cmbLightYears.TabIndex = 11;
            this.cmbLightYears.SelectedIndexChanged += new System.EventHandler(this.cmbLightYears_SelectedIndexChanged);
            this.cmbLightYears.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbLightYearsInput_KeyPress);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabStationToStation);
            this.tabControl2.Location = new System.Drawing.Point(0, 54);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1048, 532);
            this.tabControl2.TabIndex = 10;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1040, 506);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "All Commodities";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvAllComms);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(1040, 503);
            this.splitContainer2.SplitterDistance = 822;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 8;
            // 
            // lvAllComms
            // 
            this.lvAllComms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAllComms.FullRowSelect = true;
            this.lvAllComms.Location = new System.Drawing.Point(0, 0);
            this.lvAllComms.Name = "lvAllComms";
            this.lvAllComms.Size = new System.Drawing.Size(821, 505);
            this.lvAllComms.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvAllComms.TabIndex = 4;
            this.lvAllComms.UseCompatibleStateImageBehavior = false;
            this.lvAllComms.View = System.Windows.Forms.View.Details;
            this.lvAllComms.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvAllComms_ColumnClick);
            this.lvAllComms.SelectedIndexChanged += new System.EventHandler(this.lvAllComms_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.chart1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.chart2);
            this.splitContainer1.Size = new System.Drawing.Size(212, 503);
            this.splitContainer1.SplitterDistance = 226;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(83, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Buy Prices";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(210, 224);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.DarkGreen;
            this.label9.Location = new System.Drawing.Point(79, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Sell Prices";
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart2.Location = new System.Drawing.Point(0, 0);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(210, 269);
            this.chart2.TabIndex = 6;
            this.chart2.Text = "chart2";
            this.chart2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bStationDeleteRow);
            this.tabPage1.Controls.Add(this.bStationEditRow);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.bShowStationAtStarchartDotInfo);
            this.tabPage1.Controls.Add(this.lbPrices);
            this.tabPage1.Controls.Add(this.cmbStation);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lblLightYearsFromCurrentSystem);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1040, 506);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "By Station";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bStationDeleteRow
            // 
            this.bStationDeleteRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bStationDeleteRow.Enabled = false;
            this.bStationDeleteRow.Location = new System.Drawing.Point(830, 45);
            this.bStationDeleteRow.Name = "bStationDeleteRow";
            this.bStationDeleteRow.Size = new System.Drawing.Size(97, 23);
            this.bStationDeleteRow.TabIndex = 17;
            this.bStationDeleteRow.Text = "Delete Row(s)";
            this.bStationDeleteRow.UseVisualStyleBackColor = true;
            this.bStationDeleteRow.Click += new System.EventHandler(this.bStationDeleteRow_Click);
            // 
            // bStationEditRow
            // 
            this.bStationEditRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bStationEditRow.Enabled = false;
            this.bStationEditRow.Location = new System.Drawing.Point(933, 45);
            this.bStationEditRow.Name = "bStationEditRow";
            this.bStationEditRow.Size = new System.Drawing.Size(97, 23);
            this.bStationEditRow.TabIndex = 16;
            this.bStationEditRow.Text = "Edit Row";
            this.bStationEditRow.UseVisualStyleBackColor = true;
            this.bStationEditRow.Click += new System.EventHandler(this.bStationEditRow_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.tbStationRename);
            this.groupBox5.Controls.Add(this.tbSystemRename);
            this.groupBox5.Controls.Add(this.cmdApplySystemRename);
            this.groupBox5.Location = new System.Drawing.Point(562, 1);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(475, 41);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Rename Station /System";
            // 
            // tbStationRename
            // 
            this.tbStationRename.Location = new System.Drawing.Point(6, 14);
            this.tbStationRename.Name = "tbStationRename";
            this.tbStationRename.Size = new System.Drawing.Size(177, 20);
            this.tbStationRename.TabIndex = 6;
            // 
            // tbSystemRename
            // 
            this.tbSystemRename.Location = new System.Drawing.Point(189, 14);
            this.tbSystemRename.Name = "tbSystemRename";
            this.tbSystemRename.Size = new System.Drawing.Size(177, 20);
            this.tbSystemRename.TabIndex = 5;
            // 
            // cmdApplySystemRename
            // 
            this.cmdApplySystemRename.Location = new System.Drawing.Point(372, 12);
            this.cmdApplySystemRename.Name = "cmdApplySystemRename";
            this.cmdApplySystemRename.Size = new System.Drawing.Size(97, 23);
            this.cmdApplySystemRename.TabIndex = 4;
            this.cmdApplySystemRename.Text = "Apply Changes";
            this.cmdApplySystemRename.UseVisualStyleBackColor = true;
            this.cmdApplySystemRename.Click += new System.EventHandler(this.RenameStation);
            // 
            // bShowStationAtStarchartDotInfo
            // 
            this.bShowStationAtStarchartDotInfo.Image = ((System.Drawing.Image)(resources.GetObject("bShowStationAtStarchartDotInfo.Image")));
            this.bShowStationAtStarchartDotInfo.Location = new System.Drawing.Point(304, 12);
            this.bShowStationAtStarchartDotInfo.Name = "bShowStationAtStarchartDotInfo";
            this.bShowStationAtStarchartDotInfo.Size = new System.Drawing.Size(29, 27);
            this.bShowStationAtStarchartDotInfo.TabIndex = 14;
            this.bShowStationAtStarchartDotInfo.UseVisualStyleBackColor = true;
            this.bShowStationAtStarchartDotInfo.Click += new System.EventHandler(this.bShowStationAtStarchartDotInfo_Click);
            // 
            // lbPrices
            // 
            this.lbPrices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPrices.FullRowSelect = true;
            this.lbPrices.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lbPrices.Location = new System.Drawing.Point(0, 71);
            this.lbPrices.Name = "lbPrices";
            this.lbPrices.Size = new System.Drawing.Size(1040, 429);
            this.lbPrices.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lbPrices.TabIndex = 3;
            this.lbPrices.UseCompatibleStateImageBehavior = false;
            this.lbPrices.View = System.Windows.Forms.View.Details;
            this.lbPrices.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lbPrices_ColumnClick);
            this.lbPrices.SelectedIndexChanged += new System.EventHandler(this.lbPrices_SelectedIndexChanged);
            this.lbPrices.DoubleClick += new System.EventHandler(this.lbPrices_Click);
            // 
            // cmbStation
            // 
            this.cmbStation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbStation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStation.FormattingEnabled = true;
            this.cmbStation.Location = new System.Drawing.Point(52, 15);
            this.cmbStation.Name = "cmbStation";
            this.cmbStation.Size = new System.Drawing.Size(250, 21);
            this.cmbStation.TabIndex = 1;
            this.cmbStation.SelectedIndexChanged += new System.EventHandler(this.cbStation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Station";
            // 
            // lblLightYearsFromCurrentSystem
            // 
            this.lblLightYearsFromCurrentSystem.AutoSize = true;
            this.lblLightYearsFromCurrentSystem.Location = new System.Drawing.Point(339, 18);
            this.lblLightYearsFromCurrentSystem.Name = "lblLightYearsFromCurrentSystem";
            this.lblLightYearsFromCurrentSystem.Size = new System.Drawing.Size(63, 13);
            this.lblLightYearsFromCurrentSystem.TabIndex = 15;
            this.lblLightYearsFromCurrentSystem.Text = "( light years)";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bCommodityDeleteRow);
            this.tabPage2.Controls.Add(this.bEditCommodity);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.lblAvgSell);
            this.tabPage2.Controls.Add(this.lblMaxSell);
            this.tabPage2.Controls.Add(this.lblMinSell);
            this.tabPage2.Controls.Add(this.lblAvg);
            this.tabPage2.Controls.Add(this.lblMax);
            this.tabPage2.Controls.Add(this.lblMin);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.cbCommodity);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.lbCommodities);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1040, 506);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "By Commodity";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bCommodityDeleteRow
            // 
            this.bCommodityDeleteRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCommodityDeleteRow.Enabled = false;
            this.bCommodityDeleteRow.Location = new System.Drawing.Point(860, 16);
            this.bCommodityDeleteRow.Name = "bCommodityDeleteRow";
            this.bCommodityDeleteRow.Size = new System.Drawing.Size(93, 23);
            this.bCommodityDeleteRow.TabIndex = 25;
            this.bCommodityDeleteRow.Text = "Delete Row(s)";
            this.bCommodityDeleteRow.UseVisualStyleBackColor = true;
            this.bCommodityDeleteRow.Click += new System.EventHandler(this.bCommodityDeleteRow_Click);
            // 
            // bEditCommodity
            // 
            this.bEditCommodity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bEditCommodity.Enabled = false;
            this.bEditCommodity.Location = new System.Drawing.Point(959, 16);
            this.bEditCommodity.Name = "bEditCommodity";
            this.bEditCommodity.Size = new System.Drawing.Size(75, 23);
            this.bEditCommodity.TabIndex = 24;
            this.bEditCommodity.Text = "Edit Row";
            this.bEditCommodity.UseVisualStyleBackColor = true;
            this.bEditCommodity.Click += new System.EventHandler(this.bCommodityEditRow_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 3.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(556, 23);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(16, 16);
            this.button3.TabIndex = 23;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 3.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(556, 8);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(16, 16);
            this.button4.TabIndex = 22;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 3.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(478, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(16, 16);
            this.button2.TabIndex = 21;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 3.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(478, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(16, 16);
            this.button1.TabIndex = 5;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DarkGreen;
            this.label6.Location = new System.Drawing.Point(382, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Sell";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.DarkRed;
            this.label7.Location = new System.Drawing.Point(382, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Buy";
            // 
            // lblAvgSell
            // 
            this.lblAvgSell.AutoSize = true;
            this.lblAvgSell.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblAvgSell.Location = new System.Drawing.Point(611, 24);
            this.lblAvgSell.Name = "lblAvgSell";
            this.lblAvgSell.Size = new System.Drawing.Size(26, 13);
            this.lblAvgSell.TabIndex = 18;
            this.lblAvgSell.Text = "Avg";
            // 
            // lblMaxSell
            // 
            this.lblMaxSell.AutoSize = true;
            this.lblMaxSell.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblMaxSell.Location = new System.Drawing.Point(527, 24);
            this.lblMaxSell.Name = "lblMaxSell";
            this.lblMaxSell.Size = new System.Drawing.Size(27, 13);
            this.lblMaxSell.TabIndex = 17;
            this.lblMaxSell.Text = "Max";
            // 
            // lblMinSell
            // 
            this.lblMinSell.AutoSize = true;
            this.lblMinSell.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblMinSell.Location = new System.Drawing.Point(451, 24);
            this.lblMinSell.Name = "lblMinSell";
            this.lblMinSell.Size = new System.Drawing.Size(24, 13);
            this.lblMinSell.TabIndex = 16;
            this.lblMinSell.Text = "Min";
            // 
            // lblAvg
            // 
            this.lblAvg.AutoSize = true;
            this.lblAvg.ForeColor = System.Drawing.Color.DarkRed;
            this.lblAvg.Location = new System.Drawing.Point(611, 9);
            this.lblAvg.Name = "lblAvg";
            this.lblAvg.Size = new System.Drawing.Size(26, 13);
            this.lblAvg.TabIndex = 12;
            this.lblAvg.Text = "Avg";
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.ForeColor = System.Drawing.Color.DarkRed;
            this.lblMax.Location = new System.Drawing.Point(527, 9);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(27, 13);
            this.lblMax.TabIndex = 11;
            this.lblMax.Text = "Max";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.ForeColor = System.Drawing.Color.DarkRed;
            this.lblMin.Location = new System.Drawing.Point(451, 9);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(24, 13);
            this.lblMin.TabIndex = 10;
            this.lblMin.Text = "Min";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(581, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Avg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(497, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Max";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(421, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Min";
            // 
            // cbCommodity
            // 
            this.cbCommodity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbCommodity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCommodity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommodity.FormattingEnabled = true;
            this.cbCommodity.Location = new System.Drawing.Point(71, 14);
            this.cbCommodity.Name = "cbCommodity";
            this.cbCommodity.Size = new System.Drawing.Size(264, 21);
            this.cbCommodity.TabIndex = 5;
            this.cbCommodity.SelectedIndexChanged += new System.EventHandler(this.cbCommodity_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Commodity";
            // 
            // lbCommodities
            // 
            this.lbCommodities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCommodities.FullRowSelect = true;
            this.lbCommodities.Location = new System.Drawing.Point(0, 47);
            this.lbCommodities.Name = "lbCommodities";
            this.lbCommodities.Size = new System.Drawing.Size(1040, 453);
            this.lbCommodities.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lbCommodities.TabIndex = 4;
            this.lbCommodities.UseCompatibleStateImageBehavior = false;
            this.lbCommodities.View = System.Windows.Forms.View.Details;
            this.lbCommodities.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lbCommodities_ColumnClick);
            this.lbCommodities.SelectedIndexChanged += new System.EventHandler(this.lbCommodities_SelectedIndexChanged);
            // 
            // tabStationToStation
            // 
            this.tabStationToStation.Controls.Add(this.splitContainer4);
            this.tabStationToStation.Controls.Add(this.groupBox7);
            this.tabStationToStation.Controls.Add(this.lblStationToStationLightYears);
            this.tabStationToStation.Controls.Add(this.label26);
            this.tabStationToStation.Controls.Add(this.lblStationToStationMax);
            this.tabStationToStation.Controls.Add(this.bSwapStationToStations);
            this.tabStationToStation.Controls.Add(this.label30);
            this.tabStationToStation.Controls.Add(this.label29);
            this.tabStationToStation.Controls.Add(this.cmbStationToStationTo);
            this.tabStationToStation.Controls.Add(this.cmbStationToStationFrom);
            this.tabStationToStation.Controls.Add(this.bShowStationToStationRouteAtStarchartDotClub);
            this.tabStationToStation.Location = new System.Drawing.Point(4, 22);
            this.tabStationToStation.Name = "tabStationToStation";
            this.tabStationToStation.Size = new System.Drawing.Size(1040, 506);
            this.tabStationToStation.TabIndex = 8;
            this.tabStationToStation.Text = "Station-To-Station";
            this.tabStationToStation.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.splitContainer4.Location = new System.Drawing.Point(9, 36);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.lvStationToStation);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.lvStationToStationReturn);
            this.splitContainer4.Size = new System.Drawing.Size(609, 460);
            this.splitContainer4.SplitterDistance = 228;
            this.splitContainer4.TabIndex = 19;
            // 
            // lvStationToStation
            // 
            this.lvStationToStation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvStationToStation.FullRowSelect = true;
            this.lvStationToStation.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvStationToStation.Location = new System.Drawing.Point(0, 3);
            this.lvStationToStation.Name = "lvStationToStation";
            this.lvStationToStation.Size = new System.Drawing.Size(606, 222);
            this.lvStationToStation.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvStationToStation.TabIndex = 4;
            this.lvStationToStation.UseCompatibleStateImageBehavior = false;
            this.lvStationToStation.View = System.Windows.Forms.View.Details;
            this.lvStationToStation.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStationToStation_ColumnClick);
            // 
            // lvStationToStationReturn
            // 
            this.lvStationToStationReturn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvStationToStationReturn.FullRowSelect = true;
            this.lvStationToStationReturn.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvStationToStationReturn.Location = new System.Drawing.Point(0, 3);
            this.lvStationToStationReturn.Name = "lvStationToStationReturn";
            this.lvStationToStationReturn.Size = new System.Drawing.Size(606, 222);
            this.lvStationToStationReturn.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvStationToStationReturn.TabIndex = 18;
            this.lvStationToStationReturn.UseCompatibleStateImageBehavior = false;
            this.lvStationToStationReturn.View = System.Windows.Forms.View.Details;
            this.lvStationToStationReturn.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStationToStationReturn_ColumnClick);
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.label70);
            this.groupBox7.Controls.Add(this.cmbMaxRouteDistance);
            this.groupBox7.Controls.Add(this.cbMaxRouteDistance);
            this.groupBox7.Controls.Add(this.cbPerLightYearRoundTrip);
            this.groupBox7.Controls.Add(this.lbAllRoundTrips);
            this.groupBox7.Controls.Add(this.btnBestRoundTrip);
            this.groupBox7.Location = new System.Drawing.Point(624, 34);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(409, 460);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Find Round-Trips";
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(298, 30);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(14, 13);
            this.label70.TabIndex = 21;
            this.label70.Text = "ly";
            // 
            // cmbMaxRouteDistance
            // 
            this.cmbMaxRouteDistance.FormattingEnabled = true;
            this.cmbMaxRouteDistance.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "30",
            "50",
            "100"});
            this.cmbMaxRouteDistance.Location = new System.Drawing.Point(244, 27);
            this.cmbMaxRouteDistance.Name = "cmbMaxRouteDistance";
            this.cmbMaxRouteDistance.Size = new System.Drawing.Size(48, 21);
            this.cmbMaxRouteDistance.TabIndex = 19;
            this.cmbMaxRouteDistance.SelectedIndexChanged += new System.EventHandler(this.cmbMaxRouteDistance_SelectedIndexChanged);
            this.cmbMaxRouteDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbMaxRouteDistance_KeyPress);
            this.cmbMaxRouteDistance.LostFocus += new System.EventHandler(this.cmbMaxRouteDistance_LostFocus);
            // 
            // cbMaxRouteDistance
            // 
            this.cbMaxRouteDistance.AutoSize = true;
            this.cbMaxRouteDistance.Location = new System.Drawing.Point(129, 29);
            this.cbMaxRouteDistance.Name = "cbMaxRouteDistance";
            this.cbMaxRouteDistance.Size = new System.Drawing.Size(115, 17);
            this.cbMaxRouteDistance.TabIndex = 20;
            this.cbMaxRouteDistance.Text = "Max. Trip Distance";
            this.cbMaxRouteDistance.UseVisualStyleBackColor = true;
            this.cbMaxRouteDistance.CheckedChanged += new System.EventHandler(this.cbMaxRouteDistance_CheckedChanged);
            // 
            // cbPerLightYearRoundTrip
            // 
            this.cbPerLightYearRoundTrip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPerLightYearRoundTrip.AutoSize = true;
            this.cbPerLightYearRoundTrip.Checked = true;
            this.cbPerLightYearRoundTrip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPerLightYearRoundTrip.Location = new System.Drawing.Point(129, 13);
            this.cbPerLightYearRoundTrip.Name = "cbPerLightYearRoundTrip";
            this.cbPerLightYearRoundTrip.Size = new System.Drawing.Size(156, 17);
            this.cbPerLightYearRoundTrip.TabIndex = 14;
            this.cbPerLightYearRoundTrip.Text = "Take distance into account";
            this.cbPerLightYearRoundTrip.UseVisualStyleBackColor = true;
            this.cbPerLightYearRoundTrip.CheckedChanged += new System.EventHandler(this.cbPerLightYearRoundTrip_CheckedChanged);
            // 
            // lbAllRoundTrips
            // 
            this.lbAllRoundTrips.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAllRoundTrips.FormattingEnabled = true;
            this.lbAllRoundTrips.Location = new System.Drawing.Point(6, 52);
            this.lbAllRoundTrips.Name = "lbAllRoundTrips";
            this.lbAllRoundTrips.Size = new System.Drawing.Size(397, 394);
            this.lbAllRoundTrips.TabIndex = 13;
            this.lbAllRoundTrips.SelectedIndexChanged += new System.EventHandler(this.lbAllRoundTrips_SelectedIndexChanged);
            // 
            // btnBestRoundTrip
            // 
            this.btnBestRoundTrip.Location = new System.Drawing.Point(6, 19);
            this.btnBestRoundTrip.Name = "btnBestRoundTrip";
            this.btnBestRoundTrip.Size = new System.Drawing.Size(117, 23);
            this.btnBestRoundTrip.TabIndex = 12;
            this.btnBestRoundTrip.Text = "Best Round-Trip";
            this.btnBestRoundTrip.UseVisualStyleBackColor = true;
            this.btnBestRoundTrip.Click += new System.EventHandler(this.btnBestRoundTrip_Click);
            // 
            // lblStationToStationLightYears
            // 
            this.lblStationToStationLightYears.AutoSize = true;
            this.lblStationToStationLightYears.Location = new System.Drawing.Point(661, 18);
            this.lblStationToStationLightYears.Name = "lblStationToStationLightYears";
            this.lblStationToStationLightYears.Size = new System.Drawing.Size(112, 13);
            this.lblStationToStationLightYears.TabIndex = 16;
            this.lblStationToStationLightYears.Text = "( light years each way)";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(661, 5);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(101, 13);
            this.label26.TabIndex = 10;
            this.label26.Text = "Maximum round-trip:";
            // 
            // lblStationToStationMax
            // 
            this.lblStationToStationMax.AutoSize = true;
            this.lblStationToStationMax.Location = new System.Drawing.Point(762, 6);
            this.lblStationToStationMax.Name = "lblStationToStationMax";
            this.lblStationToStationMax.Size = new System.Drawing.Size(13, 13);
            this.lblStationToStationMax.TabIndex = 11;
            this.lblStationToStationMax.Text = "0";
            // 
            // bSwapStationToStations
            // 
            this.bSwapStationToStations.Location = new System.Drawing.Point(306, 6);
            this.bSwapStationToStations.Name = "bSwapStationToStations";
            this.bSwapStationToStations.Size = new System.Drawing.Size(31, 21);
            this.bSwapStationToStations.TabIndex = 9;
            this.bSwapStationToStations.Text = "<->";
            this.bSwapStationToStations.UseVisualStyleBackColor = true;
            this.bSwapStationToStations.Click += new System.EventHandler(this.bSwapStationToStations_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(346, 9);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(20, 13);
            this.label30.TabIndex = 8;
            this.label30.Text = "To";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 9);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(30, 13);
            this.label29.TabIndex = 7;
            this.label29.Text = "From";
            // 
            // cmbStationToStationTo
            // 
            this.cmbStationToStationTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbStationToStationTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStationToStationTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationToStationTo.FormattingEnabled = true;
            this.cmbStationToStationTo.Location = new System.Drawing.Point(366, 6);
            this.cmbStationToStationTo.Name = "cmbStationToStationTo";
            this.cmbStationToStationTo.Size = new System.Drawing.Size(250, 21);
            this.cmbStationToStationTo.TabIndex = 6;
            this.cmbStationToStationTo.SelectedIndexChanged += new System.EventHandler(this.cbStationToStationTo_SelectedIndexChanged);
            // 
            // cmbStationToStationFrom
            // 
            this.cmbStationToStationFrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbStationToStationFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStationToStationFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationToStationFrom.FormattingEnabled = true;
            this.cmbStationToStationFrom.Location = new System.Drawing.Point(37, 6);
            this.cmbStationToStationFrom.Name = "cmbStationToStationFrom";
            this.cmbStationToStationFrom.Size = new System.Drawing.Size(250, 21);
            this.cmbStationToStationFrom.TabIndex = 5;
            this.cmbStationToStationFrom.SelectedIndexChanged += new System.EventHandler(this.cbStationToStationFrom_SelectedIndexChanged);
            // 
            // bShowStationToStationRouteAtStarchartDotClub
            // 
            this.bShowStationToStationRouteAtStarchartDotClub.Image = ((System.Drawing.Image)(resources.GetObject("bShowStationToStationRouteAtStarchartDotClub.Image")));
            this.bShowStationToStationRouteAtStarchartDotClub.Location = new System.Drawing.Point(626, 3);
            this.bShowStationToStationRouteAtStarchartDotClub.Name = "bShowStationToStationRouteAtStarchartDotClub";
            this.bShowStationToStationRouteAtStarchartDotClub.Size = new System.Drawing.Size(29, 27);
            this.bShowStationToStationRouteAtStarchartDotClub.TabIndex = 13;
            this.bShowStationToStationRouteAtStarchartDotClub.UseVisualStyleBackColor = true;
            this.bShowStationToStationRouteAtStarchartDotClub.Click += new System.EventHandler(this.bShowStationToStationRouteAtStarchartDotClub_Click);
            // 
            // bShowStationRestrictionAtStarchartDotClub
            // 
            this.bShowStationRestrictionAtStarchartDotClub.Image = ((System.Drawing.Image)(resources.GetObject("bShowStationRestrictionAtStarchartDotClub.Image")));
            this.bShowStationRestrictionAtStarchartDotClub.Location = new System.Drawing.Point(500, 2);
            this.bShowStationRestrictionAtStarchartDotClub.Name = "bShowStationRestrictionAtStarchartDotClub";
            this.bShowStationRestrictionAtStarchartDotClub.Size = new System.Drawing.Size(29, 27);
            this.bShowStationRestrictionAtStarchartDotClub.TabIndex = 14;
            this.bShowStationRestrictionAtStarchartDotClub.UseVisualStyleBackColor = true;
            this.bShowStationRestrictionAtStarchartDotClub.Click += new System.EventHandler(this.bShowStationRestrictionAtStarchartDotClub_Click);
            // 
            // tabCommandersLog
            // 
            this.tabCommandersLog.Controls.Add(this.splitContainer3);
            this.tabCommandersLog.Location = new System.Drawing.Point(4, 22);
            this.tabCommandersLog.Name = "tabCommandersLog";
            this.tabCommandersLog.Size = new System.Drawing.Size(1051, 589);
            this.tabCommandersLog.TabIndex = 6;
            this.tabCommandersLog.Text = "Commander\'s Log";
            this.tabCommandersLog.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.lvCommandersLog);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.cbLogSystemName);
            this.splitContainer3.Panel2.Controls.Add(this.label41);
            this.splitContainer3.Panel2.Controls.Add(this.cbLogEventType);
            this.splitContainer3.Panel2.Controls.Add(this.label40);
            this.splitContainer3.Panel2.Controls.Add(this.label18);
            this.splitContainer3.Panel2.Controls.Add(this.btCreateAddEntry);
            this.splitContainer3.Panel2.Controls.Add(this.cbLogStationName);
            this.splitContainer3.Panel2.Controls.Add(this.label39);
            this.splitContainer3.Panel2.Controls.Add(this.label19);
            this.splitContainer3.Panel2.Controls.Add(this.label38);
            this.splitContainer3.Panel2.Controls.Add(this.label20);
            this.splitContainer3.Panel2.Controls.Add(this.nbCurrentCredits);
            this.splitContainer3.Panel2.Controls.Add(this.cbLogCargoName);
            this.splitContainer3.Panel2.Controls.Add(this.nbTransactionAmount);
            this.splitContainer3.Panel2.Controls.Add(this.label22);
            this.splitContainer3.Panel2.Controls.Add(this.cbPrepareNewEntry);
            this.splitContainer3.Panel2.Controls.Add(this.cbLogQuantity);
            this.splitContainer3.Panel2.Controls.Add(this.label23);
            this.splitContainer3.Panel2.Controls.Add(this.cbCargoModifier);
            this.splitContainer3.Panel2.Controls.Add(this.tbLogEventID);
            this.splitContainer3.Panel2.Controls.Add(this.tbLogNotes);
            this.splitContainer3.Panel2.Controls.Add(this.button10);
            this.splitContainer3.Panel2.Controls.Add(this.label21);
            this.splitContainer3.Panel2.Controls.Add(this.button9);
            this.splitContainer3.Panel2.Controls.Add(this.dtpLogEventDate);
            this.splitContainer3.Size = new System.Drawing.Size(1059, 583);
            this.splitContainer3.SplitterDistance = 535;
            this.splitContainer3.TabIndex = 38;
            // 
            // lvCommandersLog
            // 
            this.lvCommandersLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCommandersLog.FullRowSelect = true;
            this.lvCommandersLog.HideSelection = false;
            this.lvCommandersLog.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvCommandersLog.Location = new System.Drawing.Point(0, 0);
            this.lvCommandersLog.MultiSelect = false;
            this.lvCommandersLog.Name = "lvCommandersLog";
            this.lvCommandersLog.Size = new System.Drawing.Size(532, 586);
            this.lvCommandersLog.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvCommandersLog.TabIndex = 35;
            this.lvCommandersLog.UseCompatibleStateImageBehavior = false;
            this.lvCommandersLog.View = System.Windows.Forms.View.Details;
            this.lvCommandersLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvCommandersLog_ColumnClick);
            this.lvCommandersLog.SelectedIndexChanged += new System.EventHandler(this.lvCommandersLog_SelectedIndexChanged);
            this.lvCommandersLog.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvCommandersLog_MouseClick);
            // 
            // cbLogSystemName
            // 
            this.cbLogSystemName.FormattingEnabled = true;
            this.cbLogSystemName.Location = new System.Drawing.Point(111, 111);
            this.cbLogSystemName.Name = "cbLogSystemName";
            this.cbLogSystemName.Size = new System.Drawing.Size(334, 21);
            this.cbLogSystemName.TabIndex = 9;
            this.cbLogSystemName.DropDown += new System.EventHandler(this.cbLogSystemName_DropDown);
            this.cbLogSystemName.SelectedIndexChanged += new System.EventHandler(this.cbLogSystemName_SelectedIndexChanged);
            this.cbLogSystemName.TextChanged += new System.EventHandler(this.cbLogSystemName_TextChanged);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(256, 190);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(170, 13);
            this.label41.TabIndex = 37;
            this.label41.Text = "have to use the suggested values.";
            // 
            // cbLogEventType
            // 
            this.cbLogEventType.FormattingEnabled = true;
            this.cbLogEventType.Items.AddRange(new object[] {
            "Jumped To",
            "Visited",
            "Market Data Collected",
            "Cargo",
            "Fight",
            "Docked",
            "Took Off",
            "Saved Game",
            "Loaded Game",
            "Accepted Mission",
            "Completed Mission",
            "Other"});
            this.cbLogEventType.Location = new System.Drawing.Point(111, 84);
            this.cbLogEventType.Name = "cbLogEventType";
            this.cbLogEventType.Size = new System.Drawing.Size(121, 21);
            this.cbLogEventType.TabIndex = 6;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(256, 175);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(225, 13);
            this.label40.TabIndex = 36;
            this.label40.Text = "Note: all fields accept free text input, you don\'t";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(70, 87);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 7;
            this.label18.Text = "Event";
            // 
            // btCreateAddEntry
            // 
            this.btCreateAddEntry.Location = new System.Drawing.Point(114, 424);
            this.btCreateAddEntry.Name = "btCreateAddEntry";
            this.btCreateAddEntry.Size = new System.Drawing.Size(164, 23);
            this.btCreateAddEntry.TabIndex = 34;
            this.btCreateAddEntry.Text = "Save Changed Data";
            this.btCreateAddEntry.UseVisualStyleBackColor = true;
            this.btCreateAddEntry.Click += new System.EventHandler(this.saveLogEntry);
            // 
            // cbLogStationName
            // 
            this.cbLogStationName.FormattingEnabled = true;
            this.cbLogStationName.Location = new System.Drawing.Point(111, 138);
            this.cbLogStationName.Name = "cbLogStationName";
            this.cbLogStationName.Size = new System.Drawing.Size(334, 21);
            this.cbLogStationName.TabIndex = 8;
            this.cbLogStationName.DropDown += new System.EventHandler(this.cbLogStationName_DropDown);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(29, 192);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(76, 13);
            this.label39.TabIndex = 33;
            this.label39.Text = "Current Credits";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(65, 141);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(40, 13);
            this.label19.TabIndex = 10;
            this.label19.Text = "Station";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(7, 167);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(98, 13);
            this.label38.TabIndex = 32;
            this.label38.Text = "Transaction Credits";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(62, 114);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 13);
            this.label20.TabIndex = 11;
            this.label20.Text = "System";
            // 
            // nbCurrentCredits
            // 
            this.nbCurrentCredits.Location = new System.Drawing.Point(111, 190);
            this.nbCurrentCredits.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nbCurrentCredits.Name = "nbCurrentCredits";
            this.nbCurrentCredits.Size = new System.Drawing.Size(120, 20);
            this.nbCurrentCredits.TabIndex = 31;
            // 
            // cbLogCargoName
            // 
            this.cbLogCargoName.FormattingEnabled = true;
            this.cbLogCargoName.Location = new System.Drawing.Point(111, 216);
            this.cbLogCargoName.Name = "cbLogCargoName";
            this.cbLogCargoName.Size = new System.Drawing.Size(121, 21);
            this.cbLogCargoName.TabIndex = 12;
            this.cbLogCargoName.DropDown += new System.EventHandler(this.cbLogCargoName_DropDown);
            // 
            // nbTransactionAmount
            // 
            this.nbTransactionAmount.Location = new System.Drawing.Point(111, 165);
            this.nbTransactionAmount.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nbTransactionAmount.Name = "nbTransactionAmount";
            this.nbTransactionAmount.Size = new System.Drawing.Size(120, 20);
            this.nbTransactionAmount.TabIndex = 30;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(70, 219);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "Cargo";
            // 
            // cbPrepareNewEntry
            // 
            this.cbPrepareNewEntry.Location = new System.Drawing.Point(111, 55);
            this.cbPrepareNewEntry.Name = "cbPrepareNewEntry";
            this.cbPrepareNewEntry.Size = new System.Drawing.Size(164, 23);
            this.cbPrepareNewEntry.TabIndex = 26;
            this.cbPrepareNewEntry.Text = "prepare for new entry";
            this.cbPrepareNewEntry.UseVisualStyleBackColor = true;
            this.cbPrepareNewEntry.Click += new System.EventHandler(this.button11_Click);
            // 
            // cbLogQuantity
            // 
            this.cbLogQuantity.Location = new System.Drawing.Point(365, 216);
            this.cbLogQuantity.Name = "cbLogQuantity";
            this.cbLogQuantity.Size = new System.Drawing.Size(120, 20);
            this.cbLogQuantity.TabIndex = 17;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(56, 399);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(49, 13);
            this.label23.TabIndex = 25;
            this.label23.Text = "Event ID";
            // 
            // cbCargoModifier
            // 
            this.cbCargoModifier.FormattingEnabled = true;
            this.cbCargoModifier.Items.AddRange(new object[] {
            "Bought",
            "Sold",
            "Mined",
            "Stolen"});
            this.cbCargoModifier.Location = new System.Drawing.Point(238, 216);
            this.cbCargoModifier.Name = "cbCargoModifier";
            this.cbCargoModifier.Size = new System.Drawing.Size(121, 21);
            this.cbCargoModifier.TabIndex = 18;
            // 
            // tbLogEventID
            // 
            this.tbLogEventID.Location = new System.Drawing.Point(111, 399);
            this.tbLogEventID.Name = "tbLogEventID";
            this.tbLogEventID.ReadOnly = true;
            this.tbLogEventID.Size = new System.Drawing.Size(374, 20);
            this.tbLogEventID.TabIndex = 24;
            // 
            // tbLogNotes
            // 
            this.tbLogNotes.Location = new System.Drawing.Point(111, 243);
            this.tbLogNotes.Multiline = true;
            this.tbLogNotes.Name = "tbLogNotes";
            this.tbLogNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLogNotes.Size = new System.Drawing.Size(374, 150);
            this.tbLogNotes.TabIndex = 19;
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button10.Location = new System.Drawing.Point(400, 550);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(85, 23);
            this.button10.TabIndex = 23;
            this.button10.Text = "Load from File";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(70, 246);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(35, 13);
            this.label21.TabIndex = 20;
            this.label21.Text = "Notes";
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button9.Location = new System.Drawing.Point(311, 550);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(85, 23);
            this.button9.TabIndex = 22;
            this.button9.Text = "Save to File";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // dtpLogEventDate
            // 
            this.dtpLogEventDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpLogEventDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLogEventDate.Location = new System.Drawing.Point(238, 85);
            this.dtpLogEventDate.Name = "dtpLogEventDate";
            this.dtpLogEventDate.Size = new System.Drawing.Size(200, 20);
            this.dtpLogEventDate.TabIndex = 21;
            // 
            // tabOCRGroup
            // 
            this.tabOCRGroup.Controls.Add(this.tabCtrlOCR);
            this.tabOCRGroup.Location = new System.Drawing.Point(4, 22);
            this.tabOCRGroup.Name = "tabOCRGroup";
            this.tabOCRGroup.Size = new System.Drawing.Size(1051, 589);
            this.tabOCRGroup.TabIndex = 11;
            this.tabOCRGroup.Text = "Optical Character Recognition";
            this.tabOCRGroup.UseVisualStyleBackColor = true;
            // 
            // tabCtrlOCR
            // 
            this.tabCtrlOCR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlOCR.Controls.Add(this.tabOCR);
            this.tabCtrlOCR.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlOCR.Name = "tabCtrlOCR";
            this.tabCtrlOCR.SelectedIndex = 0;
            this.tabCtrlOCR.Size = new System.Drawing.Size(1048, 586);
            this.tabCtrlOCR.TabIndex = 0;
            // 
            // tabOCR
            // 
            this.tabOCR.Controls.Add(this.groupBox4);
            this.tabOCR.Controls.Add(this.groupBox3);
            this.tabOCR.Location = new System.Drawing.Point(4, 22);
            this.tabOCR.Name = "tabOCR";
            this.tabOCR.Size = new System.Drawing.Size(1040, 560);
            this.tabOCR.TabIndex = 4;
            this.tabOCR.Text = "Capture and Correct";
            this.tabOCR.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.bIgnoreTrash);
            this.groupBox4.Controls.Add(this.bClearOcrOutput);
            this.groupBox4.Controls.Add(this.bEditResults);
            this.groupBox4.Controls.Add(this.cbAutoImport);
            this.groupBox4.Controls.Add(this.cbUseEddnTestSchema);
            this.groupBox4.Controls.Add(this.cbDeleteScreenshotOnImport);
            this.groupBox4.Controls.Add(this.cbStartOCROnLoad);
            this.groupBox4.Controls.Add(this.cbExtendedInfoInCSV);
            this.groupBox4.Controls.Add(this.cmdHint);
            this.groupBox4.Controls.Add(this.label36);
            this.groupBox4.Controls.Add(this.label35);
            this.groupBox4.Controls.Add(this.label34);
            this.groupBox4.Controls.Add(this.label33);
            this.groupBox4.Controls.Add(this.label32);
            this.groupBox4.Controls.Add(this.tbUsername);
            this.groupBox4.Controls.Add(this.label31);
            this.groupBox4.Controls.Add(this.cbPostOnImport);
            this.groupBox4.Controls.Add(this.tbOcrSystemName);
            this.groupBox4.Controls.Add(this.tbFinalOcrOutput);
            this.groupBox4.Controls.Add(this.bContinueOcr);
            this.groupBox4.Controls.Add(this.tbConfidence);
            this.groupBox4.Controls.Add(this.tbCommoditiesOcrOutput);
            this.groupBox4.Controls.Add(this.pbOcrCurrent);
            this.groupBox4.Controls.Add(this.tbOcrStationName);
            this.groupBox4.Location = new System.Drawing.Point(441, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(587, 545);
            this.groupBox4.TabIndex = 22;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "OCR Corrections";
            // 
            // bIgnoreTrash
            // 
            this.bIgnoreTrash.Enabled = false;
            this.bIgnoreTrash.Location = new System.Drawing.Point(422, 152);
            this.bIgnoreTrash.Name = "bIgnoreTrash";
            this.bIgnoreTrash.Size = new System.Drawing.Size(154, 23);
            this.bIgnoreTrash.TabIndex = 34;
            this.bIgnoreTrash.Text = "&Ignore as Trash";
            this.bIgnoreTrash.UseVisualStyleBackColor = true;
            this.bIgnoreTrash.Click += new System.EventHandler(this.cmdIgnore_Click);
            // 
            // bClearOcrOutput
            // 
            this.bClearOcrOutput.Enabled = false;
            this.bClearOcrOutput.Location = new System.Drawing.Point(295, 402);
            this.bClearOcrOutput.Name = "bClearOcrOutput";
            this.bClearOcrOutput.Size = new System.Drawing.Size(282, 23);
            this.bClearOcrOutput.TabIndex = 33;
            this.bClearOcrOutput.Text = "Clear Results";
            this.bClearOcrOutput.UseVisualStyleBackColor = true;
            this.bClearOcrOutput.Click += new System.EventHandler(this.bClearOcrOutput_Click);
            // 
            // bEditResults
            // 
            this.bEditResults.Enabled = false;
            this.bEditResults.Location = new System.Drawing.Point(6, 402);
            this.bEditResults.Name = "bEditResults";
            this.bEditResults.Size = new System.Drawing.Size(282, 23);
            this.bEditResults.TabIndex = 32;
            this.bEditResults.Text = "Edit Results";
            this.bEditResults.UseVisualStyleBackColor = true;
            this.bEditResults.Click += new System.EventHandler(this.bEditResults_Click);
            // 
            // cbAutoImport
            // 
            this.cbAutoImport.AutoSize = true;
            this.cbAutoImport.Location = new System.Drawing.Point(261, 431);
            this.cbAutoImport.Name = "cbAutoImport";
            this.cbAutoImport.Size = new System.Drawing.Size(80, 17);
            this.cbAutoImport.TabIndex = 31;
            this.cbAutoImport.Text = "Auto Import";
            this.cbAutoImport.UseVisualStyleBackColor = true;
            this.cbAutoImport.CheckedChanged += new System.EventHandler(this.cbAutoImport_CheckedChanged);
            // 
            // cbUseEddnTestSchema
            // 
            this.cbUseEddnTestSchema.AutoSize = true;
            this.cbUseEddnTestSchema.Location = new System.Drawing.Point(169, 471);
            this.cbUseEddnTestSchema.Name = "cbUseEddnTestSchema";
            this.cbUseEddnTestSchema.Size = new System.Drawing.Size(89, 17);
            this.cbUseEddnTestSchema.TabIndex = 30;
            this.cbUseEddnTestSchema.Text = "Test Schema";
            this.cbUseEddnTestSchema.UseVisualStyleBackColor = true;
            this.cbUseEddnTestSchema.CheckedChanged += new System.EventHandler(this.cbUseEddnTestSchema_CheckedChanged);
            // 
            // cbDeleteScreenshotOnImport
            // 
            this.cbDeleteScreenshotOnImport.AutoSize = true;
            this.cbDeleteScreenshotOnImport.Location = new System.Drawing.Point(355, 431);
            this.cbDeleteScreenshotOnImport.Name = "cbDeleteScreenshotOnImport";
            this.cbDeleteScreenshotOnImport.Size = new System.Drawing.Size(222, 17);
            this.cbDeleteScreenshotOnImport.TabIndex = 29;
            this.cbDeleteScreenshotOnImport.Text = "Delete screenshot automatically on import";
            this.cbDeleteScreenshotOnImport.UseVisualStyleBackColor = true;
            this.cbDeleteScreenshotOnImport.CheckedChanged += new System.EventHandler(this.cbDeleteScreenshotOnImport_CheckedChanged);
            // 
            // cbStartOCROnLoad
            // 
            this.cbStartOCROnLoad.AutoSize = true;
            this.cbStartOCROnLoad.Location = new System.Drawing.Point(6, 431);
            this.cbStartOCROnLoad.Name = "cbStartOCROnLoad";
            this.cbStartOCROnLoad.Size = new System.Drawing.Size(252, 17);
            this.cbStartOCROnLoad.TabIndex = 28;
            this.cbStartOCROnLoad.Text = "Start OCR automatically when this app is started";
            this.cbStartOCROnLoad.UseVisualStyleBackColor = true;
            this.cbStartOCROnLoad.CheckedChanged += new System.EventHandler(this.cbStartOCROnLoad_CheckedChanged);
            // 
            // cbExtendedInfoInCSV
            // 
            this.cbExtendedInfoInCSV.AutoSize = true;
            this.cbExtendedInfoInCSV.Checked = true;
            this.cbExtendedInfoInCSV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExtendedInfoInCSV.Location = new System.Drawing.Point(6, 452);
            this.cbExtendedInfoInCSV.Name = "cbExtendedInfoInCSV";
            this.cbExtendedInfoInCSV.Size = new System.Drawing.Size(552, 17);
            this.cbExtendedInfoInCSV.TabIndex = 27;
            this.cbExtendedInfoInCSV.Text = "Include extended info in CSV (extra functionality in RegulatedNoise, but may brea" +
    "k compatibility with other apps)";
            this.cbExtendedInfoInCSV.UseVisualStyleBackColor = true;
            this.cbExtendedInfoInCSV.CheckedChanged += new System.EventHandler(this.cbExtendedInfoInCSV_CheckedChanged);
            // 
            // cmdHint
            // 
            this.cmdHint.ForeColor = System.Drawing.Color.Crimson;
            this.cmdHint.Location = new System.Drawing.Point(426, 49);
            this.cmdHint.Name = "cmdHint";
            this.cmdHint.Size = new System.Drawing.Size(150, 23);
            this.cmdHint.TabIndex = 26;
            this.cmdHint.Text = "Really Useful Tip";
            this.cmdHint.UseVisualStyleBackColor = true;
            this.cmdHint.Click += new System.EventHandler(this.cmdHint_Click);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(7, 162);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(112, 13);
            this.label36.TabIndex = 25;
            this.label36.Text = "CSV Output from OCR";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(11, 137);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(71, 13);
            this.label35.TabIndex = 24;
            this.label35.Text = "Correct Value";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(11, 94);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(85, 13);
            this.label34.TabIndex = 23;
            this.label34.Text = "Image to Correct";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(11, 52);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(72, 13);
            this.label33.TabIndex = 22;
            this.label33.Text = "System Name";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(11, 29);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(71, 13);
            this.label32.TabIndex = 21;
            this.label32.Text = "Station Name";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(347, 469);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(230, 20);
            this.tbUsername.TabIndex = 20;
            this.tbUsername.TextChanged += new System.EventHandler(this.tbUsername_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(286, 472);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(55, 13);
            this.label31.TabIndex = 19;
            this.label31.Text = "Username";
            // 
            // cbPostOnImport
            // 
            this.cbPostOnImport.AutoSize = true;
            this.cbPostOnImport.Location = new System.Drawing.Point(6, 471);
            this.cbPostOnImport.Name = "cbPostOnImport";
            this.cbPostOnImport.Size = new System.Drawing.Size(163, 17);
            this.cbPostOnImport.TabIndex = 18;
            this.cbPostOnImport.Text = "Post data to EDDN on import";
            this.cbPostOnImport.UseVisualStyleBackColor = true;
            this.cbPostOnImport.CheckedChanged += new System.EventHandler(this.cbPostOnImport_CheckedChanged);
            // 
            // tbOcrSystemName
            // 
            this.tbOcrSystemName.Location = new System.Drawing.Point(110, 53);
            this.tbOcrSystemName.Name = "tbOcrSystemName";
            this.tbOcrSystemName.Size = new System.Drawing.Size(231, 20);
            this.tbOcrSystemName.TabIndex = 16;
            this.tbOcrSystemName.TextChanged += new System.EventHandler(this.tbOcrSystemName_TextChanged);
            // 
            // tbFinalOcrOutput
            // 
            this.tbFinalOcrOutput.Location = new System.Drawing.Point(6, 180);
            this.tbFinalOcrOutput.Multiline = true;
            this.tbFinalOcrOutput.Name = "tbFinalOcrOutput";
            this.tbFinalOcrOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFinalOcrOutput.Size = new System.Drawing.Size(571, 216);
            this.tbFinalOcrOutput.TabIndex = 15;
            this.tbFinalOcrOutput.WordWrap = false;
            // 
            // bContinueOcr
            // 
            this.bContinueOcr.Enabled = false;
            this.bContinueOcr.Location = new System.Drawing.Point(422, 128);
            this.bContinueOcr.Name = "bContinueOcr";
            this.bContinueOcr.Size = new System.Drawing.Size(154, 23);
            this.bContinueOcr.TabIndex = 14;
            this.bContinueOcr.Text = "C&ontinue";
            this.bContinueOcr.UseVisualStyleBackColor = true;
            this.bContinueOcr.Click += new System.EventHandler(this.bContinueOcr_Click);
            // 
            // tbConfidence
            // 
            this.tbConfidence.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfidence.Location = new System.Drawing.Point(347, 128);
            this.tbConfidence.Name = "tbConfidence";
            this.tbConfidence.Size = new System.Drawing.Size(62, 29);
            this.tbConfidence.TabIndex = 13;
            // 
            // tbCommoditiesOcrOutput
            // 
            this.tbCommoditiesOcrOutput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbCommoditiesOcrOutput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCommoditiesOcrOutput.Location = new System.Drawing.Point(110, 128);
            this.tbCommoditiesOcrOutput.Name = "tbCommoditiesOcrOutput";
            this.tbCommoditiesOcrOutput.Size = new System.Drawing.Size(231, 29);
            this.tbCommoditiesOcrOutput.TabIndex = 12;
            this.tbCommoditiesOcrOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCommoditiesOcrOutput_Keypress);
            // 
            // pbOcrCurrent
            // 
            this.pbOcrCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbOcrCurrent.Location = new System.Drawing.Point(110, 79);
            this.pbOcrCurrent.Name = "pbOcrCurrent";
            this.pbOcrCurrent.Size = new System.Drawing.Size(231, 43);
            this.pbOcrCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOcrCurrent.TabIndex = 11;
            this.pbOcrCurrent.TabStop = false;
            // 
            // tbOcrStationName
            // 
            this.tbOcrStationName.Location = new System.Drawing.Point(110, 27);
            this.tbOcrStationName.Name = "tbOcrStationName";
            this.tbOcrStationName.Size = new System.Drawing.Size(231, 20);
            this.tbOcrStationName.TabIndex = 10;
            this.tbOcrStationName.TextChanged += new System.EventHandler(this.tbOcrStationName_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.lblScreenshotsQueued);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.pbStation);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.pbTrimmed);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(10, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(424, 545);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Capture Price Screenshots";
            // 
            // lblScreenshotsQueued
            // 
            this.lblScreenshotsQueued.AutoSize = true;
            this.lblScreenshotsQueued.Location = new System.Drawing.Point(43, 52);
            this.lblScreenshotsQueued.Name = "lblScreenshotsQueued";
            this.lblScreenshotsQueued.Size = new System.Drawing.Size(58, 13);
            this.lblScreenshotsQueued.TabIndex = 10;
            this.lblScreenshotsQueued.Text = "(0 queued)";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(8, 24);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(400, 23);
            this.button7.TabIndex = 0;
            this.button7.Text = "Monitor Directory for Commodity Screenshots";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // pbStation
            // 
            this.pbStation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbStation.Location = new System.Drawing.Point(8, 325);
            this.pbStation.Name = "pbStation";
            this.pbStation.Size = new System.Drawing.Size(205, 19);
            this.pbStation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStation.TabIndex = 9;
            this.pbStation.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 309);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "Trimmed";
            // 
            // pbTrimmed
            // 
            this.pbTrimmed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTrimmed.Location = new System.Drawing.Point(8, 355);
            this.pbTrimmed.Name = "pbTrimmed";
            this.pbTrimmed.Size = new System.Drawing.Size(241, 179);
            this.pbTrimmed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTrimmed.TabIndex = 7;
            this.pbTrimmed.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pbOriginalImage);
            this.panel1.Location = new System.Drawing.Point(5, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 233);
            this.panel1.TabIndex = 3;
            // 
            // pbOriginalImage
            // 
            this.pbOriginalImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbOriginalImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbOriginalImage.Location = new System.Drawing.Point(3, 3);
            this.pbOriginalImage.Name = "pbOriginalImage";
            this.pbOriginalImage.Size = new System.Drawing.Size(409, 227);
            this.pbOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbOriginalImage.TabIndex = 1;
            this.pbOriginalImage.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Original";
            // 
            // tabWebserver
            // 
            this.tabWebserver.Controls.Add(this.groupBox1);
            this.tabWebserver.Location = new System.Drawing.Point(4, 22);
            this.tabWebserver.Name = "tabWebserver";
            this.tabWebserver.Size = new System.Drawing.Size(1051, 589);
            this.tabWebserver.TabIndex = 3;
            this.tabWebserver.Text = "Webserver";
            this.tabWebserver.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtWebserverPort);
            this.groupBox1.Controls.Add(this.cbInterfaces);
            this.groupBox1.Controls.Add(this.label71);
            this.groupBox1.Controls.Add(this.cbStartWebserverOnLoad);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.tbBackgroundColour);
            this.groupBox1.Controls.Add(this.tbForegroundColour);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cbColourScheme);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.bStart);
            this.groupBox1.Controls.Add(this.bStop);
            this.groupBox1.Controls.Add(this.lblURL);
            this.groupBox1.Location = new System.Drawing.Point(10, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1027, 559);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Webserver";
            // 
            // txtWebserverPort
            // 
            this.txtWebserverPort.Location = new System.Drawing.Point(146, 18);
            this.txtWebserverPort.Name = "txtWebserverPort";
            this.txtWebserverPort.Size = new System.Drawing.Size(41, 20);
            this.txtWebserverPort.TabIndex = 15;
            this.txtWebserverPort.Text = "8080";
            this.txtWebserverPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbInterfaces
            // 
            this.cbInterfaces.FormattingEnabled = true;
            this.cbInterfaces.Location = new System.Drawing.Point(10, 18);
            this.cbInterfaces.Name = "cbInterfaces";
            this.cbInterfaces.Size = new System.Drawing.Size(130, 21);
            this.cbInterfaces.TabIndex = 3;
            this.cbInterfaces.SelectedIndexChanged += new System.EventHandler(this.cbInterfaces_SelectedIndexChanged);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.Location = new System.Drawing.Point(139, 21);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(10, 13);
            this.label71.TabIndex = 14;
            this.label71.Text = ":";
            // 
            // cbStartWebserverOnLoad
            // 
            this.cbStartWebserverOnLoad.AutoSize = true;
            this.cbStartWebserverOnLoad.Location = new System.Drawing.Point(11, 190);
            this.cbStartWebserverOnLoad.Name = "cbStartWebserverOnLoad";
            this.cbStartWebserverOnLoad.Size = new System.Drawing.Size(281, 17);
            this.cbStartWebserverOnLoad.TabIndex = 13;
            this.cbStartWebserverOnLoad.Text = "Start Webserver automatically when this app is started";
            this.cbStartWebserverOnLoad.UseVisualStyleBackColor = true;
            this.cbStartWebserverOnLoad.CheckedChanged += new System.EventHandler(this.cbStartWebserverOnLoad_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(226, 215);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 13);
            this.label17.TabIndex = 12;
            this.label17.Text = "Background";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(131, 216);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 13);
            this.label16.TabIndex = 11;
            this.label16.Text = "Foreground";
            // 
            // tbBackgroundColour
            // 
            this.tbBackgroundColour.Location = new System.Drawing.Point(229, 231);
            this.tbBackgroundColour.Name = "tbBackgroundColour";
            this.tbBackgroundColour.Size = new System.Drawing.Size(87, 20);
            this.tbBackgroundColour.TabIndex = 10;
            this.tbBackgroundColour.TextChanged += new System.EventHandler(this.tbBackgroundColour_TextChanged);
            // 
            // tbForegroundColour
            // 
            this.tbForegroundColour.Location = new System.Drawing.Point(136, 232);
            this.tbForegroundColour.Name = "tbForegroundColour";
            this.tbForegroundColour.Size = new System.Drawing.Size(87, 20);
            this.tbForegroundColour.TabIndex = 9;
            this.tbForegroundColour.TextChanged += new System.EventHandler(this.tbForegroundColour_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 215);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(79, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Colour Scheme";
            // 
            // cbColourScheme
            // 
            this.cbColourScheme.FormattingEnabled = true;
            this.cbColourScheme.Items.AddRange(new object[] {
            "Black on White",
            "White on Black",
            "Orange on Black"});
            this.cbColourScheme.Location = new System.Drawing.Point(9, 231);
            this.cbColourScheme.Name = "cbColourScheme";
            this.cbColourScheme.Size = new System.Drawing.Size(121, 21);
            this.cbColourScheme.TabIndex = 7;
            this.cbColourScheme.SelectedIndexChanged += new System.EventHandler(this.cbColourScheme_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 167);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "Options";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 63);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(535, 104);
            this.label10.TabIndex = 5;
            this.label10.Text = resources.GetString("label10.Text");
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(253, 18);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(111, 23);
            this.bStart.TabIndex = 1;
            this.bStart.Text = "Start Webserver";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(370, 18);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(111, 23);
            this.bStop.TabIndex = 2;
            this.bStop.Text = "Stop Webserver";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Font = new System.Drawing.Font("Segoe UI", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblURL.Location = new System.Drawing.Point(8, 44);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(42, 13);
            this.lblURL.TabIndex = 4;
            this.lblURL.Text = "http://";
            this.lblURL.Click += new System.EventHandler(this.lblURL_Click);
            // 
            // tabEDDN
            // 
            this.tabEDDN.Controls.Add(this.label83);
            this.tabEDDN.Controls.Add(this.lbEddnImplausible);
            this.tabEDDN.Controls.Add(this.tbEddnStats);
            this.tabEDDN.Controls.Add(this.groupBox2);
            this.tabEDDN.Location = new System.Drawing.Point(4, 22);
            this.tabEDDN.Name = "tabEDDN";
            this.tabEDDN.Size = new System.Drawing.Size(1051, 589);
            this.tabEDDN.TabIndex = 7;
            this.tabEDDN.Text = "EDDN";
            this.tabEDDN.UseVisualStyleBackColor = true;
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(6, 348);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(76, 13);
            this.label83.TabIndex = 6;
            this.label83.Text = "Rejected Data";
            // 
            // lbEddnImplausible
            // 
            this.lbEddnImplausible.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbEddnImplausible.FormattingEnabled = true;
            this.lbEddnImplausible.HorizontalScrollbar = true;
            this.lbEddnImplausible.Location = new System.Drawing.Point(3, 362);
            this.lbEddnImplausible.Name = "lbEddnImplausible";
            this.lbEddnImplausible.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEddnImplausible.Size = new System.Drawing.Size(1045, 212);
            this.lbEddnImplausible.TabIndex = 2;
            // 
            // tbEddnStats
            // 
            this.tbEddnStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEddnStats.Location = new System.Drawing.Point(459, 3);
            this.tbEddnStats.Multiline = true;
            this.tbEddnStats.Name = "tbEddnStats";
            this.tbEddnStats.Size = new System.Drawing.Size(589, 340);
            this.tbEddnStats.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbSpoolImplausibleToFile);
            this.groupBox2.Controls.Add(this.cbSpoolEddnToFile);
            this.groupBox2.Controls.Add(this.bPurgeAllEddnData);
            this.groupBox2.Controls.Add(this.button17);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.checkboxImportEDDN);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.cmdStopEDDNListening);
            this.groupBox2.Controls.Add(this.tbEDDNOutput);
            this.groupBox2.Controls.Add(this.button15);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 340);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Listen for EDDN Events";
            // 
            // cbSpoolImplausibleToFile
            // 
            this.cbSpoolImplausibleToFile.AutoSize = true;
            this.cbSpoolImplausibleToFile.Location = new System.Drawing.Point(10, 50);
            this.cbSpoolImplausibleToFile.Name = "cbSpoolImplausibleToFile";
            this.cbSpoolImplausibleToFile.Size = new System.Drawing.Size(210, 17);
            this.cbSpoolImplausibleToFile.TabIndex = 14;
            this.cbSpoolImplausibleToFile.Text = "Spool implausible to EddnImpOutput.txt";
            this.cbSpoolImplausibleToFile.UseVisualStyleBackColor = true;
            // 
            // cbSpoolEddnToFile
            // 
            this.cbSpoolEddnToFile.AutoSize = true;
            this.cbSpoolEddnToFile.Location = new System.Drawing.Point(10, 34);
            this.cbSpoolEddnToFile.Name = "cbSpoolEddnToFile";
            this.cbSpoolEddnToFile.Size = new System.Drawing.Size(139, 17);
            this.cbSpoolEddnToFile.TabIndex = 13;
            this.cbSpoolEddnToFile.Text = "Spool to EddnOutput.txt";
            this.cbSpoolEddnToFile.UseVisualStyleBackColor = true;
            // 
            // bPurgeAllEddnData
            // 
            this.bPurgeAllEddnData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bPurgeAllEddnData.Location = new System.Drawing.Point(315, 309);
            this.bPurgeAllEddnData.Name = "bPurgeAllEddnData";
            this.bPurgeAllEddnData.Size = new System.Drawing.Size(129, 23);
            this.bPurgeAllEddnData.TabIndex = 12;
            this.bPurgeAllEddnData.Text = "Purge all EDDN data";
            this.bPurgeAllEddnData.UseVisualStyleBackColor = true;
            this.bPurgeAllEddnData.Click += new System.EventHandler(this.cmdPurgeEDDNData);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(283, 127);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(161, 23);
            this.button17.TabIndex = 11;
            this.button17.Text = "Flush all EDDN data to UI";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(7, 112);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(280, 13);
            this.label28.TabIndex = 10;
            this.label28.Text = "No idea how much data this can collect before collapsing.";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(7, 94);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(241, 13);
            this.label27.TabIndex = 9;
            this.label27.Text = "Obsolete Information checking will be suspended;";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(7, 77);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(271, 13);
            this.label25.TabIndex = 7;
            this.label25.Text = "The UI will be updated once every ten seconds at most;";
            // 
            // checkboxImportEDDN
            // 
            this.checkboxImportEDDN.AutoSize = true;
            this.checkboxImportEDDN.Checked = true;
            this.checkboxImportEDDN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxImportEDDN.Location = new System.Drawing.Point(10, 18);
            this.checkboxImportEDDN.Name = "checkboxImportEDDN";
            this.checkboxImportEDDN.Size = new System.Drawing.Size(222, 17);
            this.checkboxImportEDDN.TabIndex = 6;
            this.checkboxImportEDDN.Text = "Import received data into RegulatedNoise";
            this.checkboxImportEDDN.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(7, 139);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(64, 13);
            this.label24.TabIndex = 5;
            this.label24.Text = "Raw Output";
            // 
            // cmdStopEDDNListening
            // 
            this.cmdStopEDDNListening.Location = new System.Drawing.Point(351, 93);
            this.cmdStopEDDNListening.Name = "cmdStopEDDNListening";
            this.cmdStopEDDNListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStopEDDNListening.TabIndex = 4;
            this.cmdStopEDDNListening.Text = "Stop Listening";
            this.cmdStopEDDNListening.UseVisualStyleBackColor = true;
            this.cmdStopEDDNListening.Click += new System.EventHandler(this.cmdStopEDDNListening_Click);
            // 
            // tbEDDNOutput
            // 
            this.tbEDDNOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEDDNOutput.Location = new System.Drawing.Point(6, 155);
            this.tbEDDNOutput.Multiline = true;
            this.tbEDDNOutput.Name = "tbEDDNOutput";
            this.tbEDDNOutput.Size = new System.Drawing.Size(438, 148);
            this.tbEDDNOutput.TabIndex = 3;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(351, 65);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(93, 23);
            this.button15.TabIndex = 2;
            this.button15.Text = "Start Listening";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.groupBox12);
            this.tabSettings.Controls.Add(this.groupBox10);
            this.tabSettings.Controls.Add(this.groupBox8);
            this.tabSettings.Controls.Add(this.groupBox6);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(1051, 589);
            this.tabSettings.TabIndex = 12;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Controls.Add(this.label89);
            this.groupBox12.Controls.Add(this.nudPurgeOldDataDays);
            this.groupBox12.Controls.Add(this.cmdPurgeOldData);
            this.groupBox12.Controls.Add(this.cbLoadStationsJSON);
            this.groupBox12.Controls.Add(this.label74);
            this.groupBox12.Controls.Add(this.cbAutoActivateSystemTab);
            this.groupBox12.Controls.Add(this.cbIncludeUnknownDTS);
            this.groupBox12.Controls.Add(this.cbAutoActivateOCRTab);
            this.groupBox12.Controls.Add(this.button6);
            this.groupBox12.Location = new System.Drawing.Point(653, 166);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(400, 421);
            this.groupBox12.TabIndex = 12;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Other";
            // 
            // cbLoadStationsJSON
            // 
            this.cbLoadStationsJSON.AutoSize = true;
            this.cbLoadStationsJSON.Location = new System.Drawing.Point(32, 131);
            this.cbLoadStationsJSON.Name = "cbLoadStationsJSON";
            this.cbLoadStationsJSON.Size = new System.Drawing.Size(308, 30);
            this.cbLoadStationsJSON.TabIndex = 6;
            this.cbLoadStationsJSON.Text = "Load <stations.json> instead of <stations_light.json> even if \r\nthe plausibility " +
    "warn levels already calculated.";
            this.cbLoadStationsJSON.UseVisualStyleBackColor = true;
            this.cbLoadStationsJSON.CheckedChanged += new System.EventHandler(this.cbLoadStationsJSON_CheckedChanged);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label74.Location = new System.Drawing.Point(49, 159);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(269, 36);
            this.label74.TabIndex = 7;
            this.label74.Text = "(not recommended/no benefit - loading the programm needs \r\nonly more time - have " +
    "in mind: if you want to update the database\r\nso update the correct file : normal" +
    "ly <stations_light.json>)";
            // 
            // cbAutoActivateSystemTab
            // 
            this.cbAutoActivateSystemTab.AutoSize = true;
            this.cbAutoActivateSystemTab.Location = new System.Drawing.Point(32, 50);
            this.cbAutoActivateSystemTab.Name = "cbAutoActivateSystemTab";
            this.cbAutoActivateSystemTab.Size = new System.Drawing.Size(312, 17);
            this.cbAutoActivateSystemTab.TabIndex = 5;
            this.cbAutoActivateSystemTab.Text = "automatically activate the \"System Data\" tab after hyperjump";
            this.cbAutoActivateSystemTab.UseVisualStyleBackColor = true;
            this.cbAutoActivateSystemTab.CheckedChanged += new System.EventHandler(this.cbAutoActivateSystem_CheckedChanged);
            // 
            // cbIncludeUnknownDTS
            // 
            this.cbIncludeUnknownDTS.AutoSize = true;
            this.cbIncludeUnknownDTS.Location = new System.Drawing.Point(32, 80);
            this.cbIncludeUnknownDTS.Name = "cbIncludeUnknownDTS";
            this.cbIncludeUnknownDTS.Size = new System.Drawing.Size(349, 43);
            this.cbIncludeUnknownDTS.TabIndex = 4;
            this.cbIncludeUnknownDTS.Text = "include station on search if a filter-by property  is unknown \r\n(otherwise it wil" +
    "l be excluded, properties can be <Distance To Star>, \r\n<Landingpadsize> and so o" +
    "n)";
            this.cbIncludeUnknownDTS.UseVisualStyleBackColor = true;
            this.cbIncludeUnknownDTS.CheckedChanged += new System.EventHandler(this.cbIncludeUnknownDTS_CheckedChanged);
            // 
            // cbAutoActivateOCRTab
            // 
            this.cbAutoActivateOCRTab.AutoSize = true;
            this.cbAutoActivateOCRTab.Location = new System.Drawing.Point(32, 27);
            this.cbAutoActivateOCRTab.Name = "cbAutoActivateOCRTab";
            this.cbAutoActivateOCRTab.Size = new System.Drawing.Size(325, 17);
            this.cbAutoActivateOCRTab.TabIndex = 3;
            this.cbAutoActivateOCRTab.Text = "Automatically activate the OCR-Tab when the recognition starts";
            this.cbAutoActivateOCRTab.UseVisualStyleBackColor = true;
            this.cbAutoActivateOCRTab.CheckedChanged += new System.EventHandler(this.cbActivateOCRTab_CheckedChanged);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Location = new System.Drawing.Point(32, 369);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(226, 23);
            this.button6.TabIndex = 2;
            this.button6.Text = "Edit Commodity Price Warn levels";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.cmdWarnLevels_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.Controls.Add(this.cbAutoAdd_ReplaceVisited);
            this.groupBox10.Controls.Add(this.cbAutoAdd_Marketdata);
            this.groupBox10.Controls.Add(this.cbAutoAdd_Visited);
            this.groupBox10.Controls.Add(this.label49);
            this.groupBox10.Controls.Add(this.cbAutoAdd_JumpedTo);
            this.groupBox10.Location = new System.Drawing.Point(653, 6);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(400, 151);
            this.groupBox10.TabIndex = 11;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Commander\'s Log";
            // 
            // cbAutoAdd_ReplaceVisited
            // 
            this.cbAutoAdd_ReplaceVisited.AutoSize = true;
            this.cbAutoAdd_ReplaceVisited.Location = new System.Drawing.Point(60, 111);
            this.cbAutoAdd_ReplaceVisited.Name = "cbAutoAdd_ReplaceVisited";
            this.cbAutoAdd_ReplaceVisited.Size = new System.Drawing.Size(292, 17);
            this.cbAutoAdd_ReplaceVisited.TabIndex = 4;
            this.cbAutoAdd_ReplaceVisited.Text = "replace previous \"Visited\" with  \"Market Data Collected\"";
            this.cbAutoAdd_ReplaceVisited.UseVisualStyleBackColor = true;
            this.cbAutoAdd_ReplaceVisited.CheckedChanged += new System.EventHandler(this.cbAutoAdd_ReplaceVisited_CheckedChanged);
            // 
            // cbAutoAdd_Marketdata
            // 
            this.cbAutoAdd_Marketdata.AutoSize = true;
            this.cbAutoAdd_Marketdata.Location = new System.Drawing.Point(32, 90);
            this.cbAutoAdd_Marketdata.Name = "cbAutoAdd_Marketdata";
            this.cbAutoAdd_Marketdata.Size = new System.Drawing.Size(173, 17);
            this.cbAutoAdd_Marketdata.TabIndex = 3;
            this.cbAutoAdd_Marketdata.Text = "\"Market Data Collected\"-Event";
            this.cbAutoAdd_Marketdata.UseVisualStyleBackColor = true;
            this.cbAutoAdd_Marketdata.CheckedChanged += new System.EventHandler(this.cbAutoAdd_Marketdata_CheckedChanged);
            // 
            // cbAutoAdd_Visited
            // 
            this.cbAutoAdd_Visited.AutoSize = true;
            this.cbAutoAdd_Visited.Location = new System.Drawing.Point(32, 67);
            this.cbAutoAdd_Visited.Name = "cbAutoAdd_Visited";
            this.cbAutoAdd_Visited.Size = new System.Drawing.Size(98, 17);
            this.cbAutoAdd_Visited.TabIndex = 2;
            this.cbAutoAdd_Visited.Text = "\"Visited\"-Event";
            this.cbAutoAdd_Visited.UseVisualStyleBackColor = true;
            this.cbAutoAdd_Visited.CheckedChanged += new System.EventHandler(this.cbAutoAdd_Visited_CheckedChanged);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(29, 22);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(195, 13);
            this.label49.TabIndex = 1;
            this.label49.Text = "generate following events automatically:";
            // 
            // cbAutoAdd_JumpedTo
            // 
            this.cbAutoAdd_JumpedTo.AutoSize = true;
            this.cbAutoAdd_JumpedTo.Location = new System.Drawing.Point(32, 44);
            this.cbAutoAdd_JumpedTo.Name = "cbAutoAdd_JumpedTo";
            this.cbAutoAdd_JumpedTo.Size = new System.Drawing.Size(116, 17);
            this.cbAutoAdd_JumpedTo.TabIndex = 0;
            this.cbAutoAdd_JumpedTo.Text = "\"Jumped to\"-Event";
            this.cbAutoAdd_JumpedTo.UseVisualStyleBackColor = true;
            this.cbAutoAdd_JumpedTo.CheckedChanged += new System.EventHandler(this.cbAutoAdd_JumpedTo_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox8.Controls.Add(this.label48);
            this.groupBox8.Controls.Add(this.cmbLanguage);
            this.groupBox8.Location = new System.Drawing.Point(6, 420);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(641, 167);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Language";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(229, 16);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(308, 26);
            this.label48.TabIndex = 1;
            this.label48.Text = "Select the language of your Elite Dangerous version.\r\n(EDDN is - at the moment - " +
    "only with english language available)";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(262, 48);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(275, 21);
            this.cmbLanguage.TabIndex = 0;
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox9);
            this.groupBox6.Controls.Add(this.groupBox11);
            this.groupBox6.Controls.Add(this.txtTraineddataFile);
            this.groupBox6.Controls.Add(this.cmdSelectTraineddataFile);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(641, 408);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "OCR-Settings";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.cmdFilter);
            this.groupBox9.Controls.Add(this.label50);
            this.groupBox9.Controls.Add(this.label51);
            this.groupBox9.Controls.Add(this.txtGUIColorCutoffLevel);
            this.groupBox9.Location = new System.Drawing.Point(11, 210);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(597, 150);
            this.groupBox9.TabIndex = 15;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "UI Color Cutoff Level";
            // 
            // cmdFilter
            // 
            this.cmdFilter.Location = new System.Drawing.Point(350, 107);
            this.cmdFilter.Name = "cmdFilter";
            this.cmdFilter.Size = new System.Drawing.Size(176, 23);
            this.cmdFilter.TabIndex = 18;
            this.cmdFilter.Text = "Filter-Test";
            this.cmdFilter.UseVisualStyleBackColor = true;
            this.cmdFilter.Click += new System.EventHandler(this.cmdFilter_Click);
            // 
            // label50
            // 
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(18, 19);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(526, 85);
            this.label50.TabIndex = 15;
            this.label50.Text = resources.GetString("label50.Text");
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(18, 110);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(215, 13);
            this.label51.TabIndex = 14;
            this.label51.Text = "UI Color Cutoff Level (0 to 255, default=150)";
            // 
            // txtGUIColorCutoffLevel
            // 
            this.txtGUIColorCutoffLevel.Location = new System.Drawing.Point(239, 107);
            this.txtGUIColorCutoffLevel.Name = "txtGUIColorCutoffLevel";
            this.txtGUIColorCutoffLevel.Size = new System.Drawing.Size(37, 20);
            this.txtGUIColorCutoffLevel.TabIndex = 13;
            this.txtGUIColorCutoffLevel.LostFocus += new System.EventHandler(this.txtGUIColorCutoffLevel_LostFocus);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label52);
            this.groupBox11.Controls.Add(this.cbCheckAOne);
            this.groupBox11.Controls.Add(this.lblPixelAmount);
            this.groupBox11.Controls.Add(this.txtPixelAmount);
            this.groupBox11.Controls.Add(this.lblPixelThreshold);
            this.groupBox11.Controls.Add(this.txtPixelThreshold);
            this.groupBox11.Location = new System.Drawing.Point(11, 63);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(597, 141);
            this.groupBox11.TabIndex = 14;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "OCR Performance Improvement";
            // 
            // label52
            // 
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label52.Location = new System.Drawing.Point(18, 17);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(554, 71);
            this.label52.TabIndex = 19;
            this.label52.Text = resources.GetString("label52.Text");
            // 
            // cbCheckAOne
            // 
            this.cbCheckAOne.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbCheckAOne.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.cbCheckAOne.Location = new System.Drawing.Point(350, 97);
            this.cbCheckAOne.Name = "cbCheckAOne";
            this.cbCheckAOne.Size = new System.Drawing.Size(176, 23);
            this.cbCheckAOne.TabIndex = 18;
            this.cbCheckAOne.Text = "check next screenshot for a \"1\"";
            this.cbCheckAOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbCheckAOne.UseVisualStyleBackColor = true;
            // 
            // lblPixelAmount
            // 
            this.lblPixelAmount.AutoSize = true;
            this.lblPixelAmount.Location = new System.Drawing.Point(18, 118);
            this.lblPixelAmount.Name = "lblPixelAmount";
            this.lblPixelAmount.Size = new System.Drawing.Size(179, 13);
            this.lblPixelAmount.TabIndex = 16;
            this.lblPixelAmount.Text = "dark pixel amount (default=22, 0=off)";
            // 
            // txtPixelAmount
            // 
            this.txtPixelAmount.Location = new System.Drawing.Point(212, 115);
            this.txtPixelAmount.Name = "txtPixelAmount";
            this.txtPixelAmount.Size = new System.Drawing.Size(37, 20);
            this.txtPixelAmount.TabIndex = 15;
            this.txtPixelAmount.LostFocus += new System.EventHandler(this.txtPixelAmount_LostFocus);
            // 
            // lblPixelThreshold
            // 
            this.lblPixelThreshold.AutoSize = true;
            this.lblPixelThreshold.Location = new System.Drawing.Point(18, 91);
            this.lblPixelThreshold.Name = "lblPixelThreshold";
            this.lblPixelThreshold.Size = new System.Drawing.Size(160, 13);
            this.lblPixelThreshold.TabIndex = 14;
            this.lblPixelThreshold.Text = "dark pixel threshold (default=0.6)";
            // 
            // txtPixelThreshold
            // 
            this.txtPixelThreshold.Location = new System.Drawing.Point(212, 88);
            this.txtPixelThreshold.Name = "txtPixelThreshold";
            this.txtPixelThreshold.Size = new System.Drawing.Size(37, 20);
            this.txtPixelThreshold.TabIndex = 13;
            this.txtPixelThreshold.LostFocus += new System.EventHandler(this.txtPixelThreshold_LostFocus);
            // 
            // txtTraineddataFile
            // 
            this.txtTraineddataFile.Location = new System.Drawing.Point(117, 36);
            this.txtTraineddataFile.Name = "txtTraineddataFile";
            this.txtTraineddataFile.ReadOnly = true;
            this.txtTraineddataFile.Size = new System.Drawing.Size(197, 20);
            this.txtTraineddataFile.TabIndex = 13;
            // 
            // cmdSelectTraineddataFile
            // 
            this.cmdSelectTraineddataFile.Location = new System.Drawing.Point(361, 25);
            this.cmdSelectTraineddataFile.Name = "cmdSelectTraineddataFile";
            this.cmdSelectTraineddataFile.Size = new System.Drawing.Size(176, 23);
            this.cmdSelectTraineddataFile.TabIndex = 8;
            this.cmdSelectTraineddataFile.Text = "Select";
            this.cmdSelectTraineddataFile.UseVisualStyleBackColor = true;
            this.cmdSelectTraineddataFile.Click += new System.EventHandler(this.cmdSelectTraineddataFile_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(137, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(177, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "\"traineddata\"-File for TesseractOCR";
            // 
            // cmdLoadCurrentSystem
            // 
            this.cmdLoadCurrentSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLoadCurrentSystem.Location = new System.Drawing.Point(929, 57);
            this.cmdLoadCurrentSystem.Name = "cmdLoadCurrentSystem";
            this.cmdLoadCurrentSystem.Size = new System.Drawing.Size(148, 23);
            this.cmdLoadCurrentSystem.TabIndex = 60;
            this.cmdLoadCurrentSystem.Text = "Show Current System Data";
            this.cmdLoadCurrentSystem.UseVisualStyleBackColor = true;
            this.cmdLoadCurrentSystem.Click += new System.EventHandler(this.cmdLoadCurrentSystem_Click);
            // 
            // bOpen
            // 
            this.bOpen.Location = new System.Drawing.Point(18, 12);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(101, 23);
            this.bOpen.TabIndex = 0;
            this.bOpen.Text = "Open CSV Files";
            this.bOpen.UseVisualStyleBackColor = false;
            this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.removeEconomyToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(168, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.toolStripMenuItem1.Text = "add Economy";
            // 
            // removeEconomyToolStripMenuItem
            // 
            this.removeEconomyToolStripMenuItem.Name = "removeEconomyToolStripMenuItem";
            this.removeEconomyToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.removeEconomyToolStripMenuItem.Text = "remove Economy";
            // 
            // dsCommodities
            // 
            this.dsCommodities.DataSetName = "dsCommodities";
            this.dsCommodities.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // namesBindingSource
            // 
            this.namesBindingSource.DataMember = "Names";
            this.namesBindingSource.DataSource = this.dsCommodities;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1089, 686);
            this.Controls.Add(this.cmdLoadCurrentSystem);
            this.Controls.Add(this.label53);
            this.Controls.Add(this.txtEDTime);
            this.Controls.Add(this.label54);
            this.Controls.Add(this.txtLocalTime);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.tbCurrentStationinfoFromLogs);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.tbCurrentSystemFromLogs);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.tabCtrlMain);
            this.Controls.Add(this.bOpen);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "RegulatedNoise v";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.Form_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudPurgeOldDataDays)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabCtrlMain.ResumeLayout(false);
            this.tabHelpAndChangeLog.ResumeLayout(false);
            this.tabHelpAndChangeLog.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabSystemData.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.paEconomies.ResumeLayout(false);
            this.paEconomies.PerformLayout();
            this.gbSystemSystemData.ResumeLayout(false);
            this.gbSystemSystemData.PerformLayout();
            this.tabPriceAnalysis.ResumeLayout(false);
            this.tabPriceAnalysis.PerformLayout();
            this.gbSorting.ResumeLayout(false);
            this.gbSorting.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabStationToStation.ResumeLayout(false);
            this.tabStationToStation.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabCommandersLog.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbCurrentCredits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTransactionAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbLogQuantity)).EndInit();
            this.tabOCRGroup.ResumeLayout(false);
            this.tabCtrlOCR.ResumeLayout(false);
            this.tabOCR.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOcrCurrent)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrimmed)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).EndInit();
            this.tabWebserver.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabEDDN.ResumeLayout(false);
            this.tabEDDN.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsCommodities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.namesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Button bOpen;
        private System.Windows.Forms.ComboBox cmbStation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lbPrices;
        private System.Windows.Forms.TabControl tabCtrlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView lbCommodities;
        private System.Windows.Forms.ComboBox cbCommodity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAvg;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAvgSell;
        private System.Windows.Forms.Label lblMaxSell;
        private System.Windows.Forms.Label lblMinSell;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView lvAllComms;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbSystemRename;
        private System.Windows.Forms.Button cmdApplySystemRename;
        private System.Windows.Forms.TabPage tabWebserver;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.ComboBox cbInterfaces;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbBackgroundColour;
        private System.Windows.Forms.TextBox tbForegroundColour;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbColourScheme;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tabCommandersLog;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label21;
        public System.Windows.Forms.ComboBox cbLogEventType;
        public System.Windows.Forms.ComboBox cbCargoModifier;
        public System.Windows.Forms.NumericUpDown cbLogQuantity;
        public System.Windows.Forms.ComboBox cbLogCargoName;
        public System.Windows.Forms.ComboBox cbLogSystemName;
        public System.Windows.Forms.ComboBox cbLogStationName;
        public System.Windows.Forms.TextBox tbLogNotes;
        public System.Windows.Forms.DateTimePicker dtpLogEventDate;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label23;
        public System.Windows.Forms.TextBox tbLogEventID;
        private System.Windows.Forms.Button cbPrepareNewEntry;
        private System.Windows.Forms.TabPage tabEDDN;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button15;
        public System.Windows.Forms.TextBox tbEDDNOutput;
        private System.Windows.Forms.Button cmdStopEDDNListening;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.CheckBox checkboxImportEDDN;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TabPage tabStationToStation;
        private System.Windows.Forms.ComboBox cmbStationToStationTo;
        private System.Windows.Forms.ComboBox cmbStationToStationFrom;
        private System.Windows.Forms.ListView lvStationToStation;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label38;
        public System.Windows.Forms.NumericUpDown nbCurrentCredits;
        public System.Windows.Forms.NumericUpDown nbTransactionAmount;
        public System.Windows.Forms.TextBox tbCurrentSystemFromLogs;
        public System.Windows.Forms.ListView lvCommandersLog;
        public System.Windows.Forms.Button btCreateAddEntry;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TabPage tabHelpAndChangeLog;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblRegulatedNoise;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPriceAnalysis;
        private System.Windows.Forms.TabPage tabOCRGroup;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox cbStartWebserverOnLoad;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tbStationRename;
        private System.Windows.Forms.Button bSwapStationToStations;
        private System.Windows.Forms.Button bEditCommodity;
        private System.Windows.Forms.Button bCommodityDeleteRow;
        private System.Windows.Forms.Label lblStationToStationMax;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnBestRoundTrip;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.CheckBox cbLimitLightYears;
        private System.Windows.Forms.ComboBox cmbLightYears;
        private System.Windows.Forms.PictureBox pbBackgroundColour;
        private System.Windows.Forms.PictureBox pbForegroundColour;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label BackgroundSet;
        private System.Windows.Forms.Label ForegroundSet;
        private System.Windows.Forms.Button bShowStationToStationRouteAtStarchartDotClub;
        private System.Windows.Forms.Button bShowStationAtStarchartDotInfo;
        private System.Windows.Forms.ComboBox cbIncludeWithinRegionOfStation;
        private System.Windows.Forms.Button bShowStationRestrictionAtStarchartDotClub;
        private System.Windows.Forms.Label lblLightYearsFromCurrentSystem;
        private System.Windows.Forms.Label lblStationToStationLightYears;
        private System.Windows.Forms.Button bPurgeAllEddnData;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ListBox lbAllRoundTrips;
        private System.Windows.Forms.CheckBox cbPerLightYearRoundTrip;
        private System.Windows.Forms.ListView lvStationToStationReturn;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TabControl tabCtrlOCR;
        private System.Windows.Forms.TabPage tabOCR;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button bEditResults;
        private System.Windows.Forms.CheckBox cbAutoImport;
        private System.Windows.Forms.CheckBox cbUseEddnTestSchema;
        private System.Windows.Forms.CheckBox cbDeleteScreenshotOnImport;
        private System.Windows.Forms.CheckBox cbStartOCROnLoad;
        private System.Windows.Forms.CheckBox cbExtendedInfoInCSV;
        private System.Windows.Forms.Button cmdHint;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        internal System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.CheckBox cbPostOnImport;
        private System.Windows.Forms.TextBox tbOcrSystemName;
        private System.Windows.Forms.TextBox tbFinalOcrOutput;
        private System.Windows.Forms.Button bContinueOcr;
        private System.Windows.Forms.TextBox tbConfidence;
        private System.Windows.Forms.TextBox tbCommoditiesOcrOutput;
        private System.Windows.Forms.PictureBox pbOcrCurrent;
        private System.Windows.Forms.TextBox tbOcrStationName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblScreenshotsQueued;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.PictureBox pbStation;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pbTrimmed;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.PictureBox pbOriginalImage;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbSpoolEddnToFile;
        private System.Windows.Forms.Button bClearOcrOutput;
        private System.Windows.Forms.TextBox tbEddnStats;
        private System.Windows.Forms.Button bIgnoreTrash;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button cmdSelectTraineddataFile;
        private System.Windows.Forms.Label label12;
        private RegulatedNoise.Enums_and_Utility_Classes.dsCommodities dsCommodities;
        private System.Windows.Forms.BindingSource namesBindingSource;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.CheckBox cbAutoAdd_JumpedTo;
        private System.Windows.Forms.TextBox txtTraineddataFile;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label lblPixelAmount;
        private System.Windows.Forms.TextBox txtPixelAmount;
        private System.Windows.Forms.Label lblPixelThreshold;
        private System.Windows.Forms.TextBox txtPixelThreshold;
        internal System.Windows.Forms.CheckBox cbCheckAOne;
        private System.Windows.Forms.GroupBox gbSorting;
        private System.Windows.Forms.RadioButton rbSortByStation;
        private System.Windows.Forms.RadioButton rbSortBySystem;
        private System.Windows.Forms.Button bStationDeleteRow;
        private System.Windows.Forms.Button bStationEditRow;
        private System.Windows.Forms.RadioButton rbSortByDistance;
        private System.Windows.Forms.TextBox txtlastStationCount;
        private System.Windows.Forms.CheckBox cblastVisitedFirst;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox txtGUIColorCutoffLevel;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Button cmdFilter;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.CheckBox cbAutoActivateOCRTab;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Button cmdDonate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.LinkLabel linkLabel9;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel8;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.LinkLabel linkLabel6;
        private System.Windows.Forms.LinkLabel linkLabel7;
        public System.Windows.Forms.TextBox tbCurrentStationinfoFromLogs;
        private System.Windows.Forms.Label label45;
        public System.Windows.Forms.TextBox txtLocalTime;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label53;
        public System.Windows.Forms.TextBox txtEDTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copySystenmameToClipboardToolStripMenuItem;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.CheckBox cbStationToStar;
        private System.Windows.Forms.ComboBox cmbStationToStar;
        private System.Windows.Forms.TabPage tabSystemData;
        private System.Windows.Forms.MaskedTextBox txtSystemPopulation;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.TextBox txtSystemFaction;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox txtSystemZ;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.TextBox txtSystemY;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.TextBox txtSystemX;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.TextBox txtSystemName;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TextBox txtSystemId;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox txtSystemUpdatedAt;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.ComboBox_ro cmbSystemPrimaryEconomy;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.ComboBox_ro cmbSystemSecurity;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ComboBox_ro cmbSystemState;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.ComboBox_ro cmbSystemAllegiance;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.ComboBox_ro cmbSystemGovernment;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Button cmdLoadCurrentSystem;
        private System.Windows.Forms.Button cmdSystemNew;
        private System.Windows.Forms.Button cmdSystemSave;
        private System.Windows.Forms.Label lblSystemRenameHint;
        private System.Windows.Forms.CheckBox cbIncludeUnknownDTS;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.ComboBox cmbMaxRouteDistance;
        private System.Windows.Forms.CheckBox cbMaxRouteDistance;
        private System.Windows.Forms.TextBox txtWebserverPort;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label lblStationRenameHint;
        private System.Windows.Forms.Button cmdStationNew;
        private System.Windows.Forms.Button cmdStationSave;
        private System.Windows.Forms.ComboBox_ro cmbStationType;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.ComboBox_ro cmbStationState;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.ComboBox_ro cmbStationAllegiance;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.ComboBox_ro cmbStationGovernment;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.TextBox txtStationFaction;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.TextBox txtStationDistanceToStar;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.ComboBox_ro cmbStationMaxLandingPadSize;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.TextBox txtStationName;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.TextBox txtStationId;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.CheckBox_ro cbStationHasOutfitting;
        private System.Windows.Forms.CheckBox_ro cbStationHasShipyard;
        private System.Windows.Forms.CheckBox_ro cbStationHasRepair;
        private System.Windows.Forms.CheckBox_ro cbStationHasRearm;
        private System.Windows.Forms.CheckBox_ro cbStationHasRefuel;
        private System.Windows.Forms.CheckBox_ro cbStationHasCommodities;
        private System.Windows.Forms.CheckBox_ro cbStationHasBlackmarket;
        private System.Windows.Forms.TextBox txtStationUpdatedAt;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.CheckBox_ro cbSystemNeedsPermit;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.ListBox lbStationEconomies;
        private System.Windows.Forms.CheckBox cbAutoActivateSystemTab;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeEconomyToolStripMenuItem;
        private System.Windows.Forms.Label lblStationCount;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.CheckBox cbLoadStationsJSON;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.ListBox lbEddnImplausible;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Button cmdTest;
        private System.Windows.Forms.Button cmdStationEdit;
        private System.Windows.Forms.Button cmdSystemEdit;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.GroupBox gbSystemSystemData;
        private System.Windows.Forms.Label lblStationCountTotal;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.ComboBox_ro cmbSystemsAllSystems;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label lblSystemCountTotal;
        private System.Windows.Forms.Button cmdSystemCancel;
        private System.Windows.Forms.Button cmdStationCancel;
        private System.Windows.Forms.Panel paEconomies;
        private System.Windows.Forms.CheckBox_ro cbStationEcoAgriculture;
        private System.Windows.Forms.CheckBox_ro cbStationEcoTourism;
        private System.Windows.Forms.CheckBox_ro cbStationEcoTerraforming;
        private System.Windows.Forms.CheckBox_ro cbStationEcoService;
        private System.Windows.Forms.CheckBox_ro cbStationEcoRefinery;
        private System.Windows.Forms.CheckBox_ro cbStationEcoMilitary;
        private System.Windows.Forms.CheckBox_ro cbStationEcoIndustrial;
        private System.Windows.Forms.CheckBox_ro cbStationEcoHighTech;
        private System.Windows.Forms.CheckBox_ro cbStationEcoExtraction;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ComboBox_ro cmbStationStations;
        private System.Windows.Forms.Button cmdPurgeOldData;
        private System.Windows.Forms.CheckBox_ro cbStationEcoNone;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.NumericUpDown nudPurgeOldDataDays;
        private System.Windows.Forms.CheckBox cbSpoolImplausibleToFile;
        private System.Windows.Forms.CheckBox cbAutoAdd_Marketdata;
        private System.Windows.Forms.CheckBox cbAutoAdd_Visited;
        private System.Windows.Forms.CheckBox cbAutoAdd_ReplaceVisited;
        internal System.Windows.Forms.Label lblUpdateInfo;
        internal System.Windows.Forms.Label lblUpdateDetail;
        private System.Windows.Forms.Button cmdUpdate;
        private System.Windows.Forms.LinkLabel llVisitUpdate;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.LinkLabel linkLabel11;
        private System.Windows.Forms.LinkLabel linkLabel10;

        
    }
}

