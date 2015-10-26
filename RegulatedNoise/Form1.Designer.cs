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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copySystenmameToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeEconomyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dsCommodities = new RegulatedNoise.Enums_and_Utility_Classes.dsCommodities();
            this.namesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmdLoadCurrentSystem = new System.Windows.Forms.Button();
            this.label53 = new System.Windows.Forms.Label();
            this.txtEDTime = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.txtLocalTime = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.tbCurrentStationinfoFromLogs = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.tbCurrentSystemFromLogs = new System.Windows.Forms.TextBox();
            this.msMainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabEDDN = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button15 = new System.Windows.Forms.Button();
            this.tbEDDNOutput = new System.Windows.Forms.TextBox();
            this.cmdStopEDDNListening = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.checkboxImportEDDN = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.button17 = new System.Windows.Forms.Button();
            this.bPurgeAllEddnData = new System.Windows.Forms.Button();
            this.cbSpoolEddnToFile = new System.Windows.Forms.CheckBox();
            this.cbSpoolImplausibleToFile = new System.Windows.Forms.CheckBox();
            this.cbEDDNAutoListen = new System.Windows.Forms.CheckBox();
            this.tbEddnStats = new System.Windows.Forms.TextBox();
            this.lbEddnImplausible = new System.Windows.Forms.ListBox();
            this.label83 = new System.Windows.Forms.Label();
            this.tabWebserver = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.bStop = new System.Windows.Forms.Button();
            this.bStart = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbColourScheme = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbForegroundColour = new System.Windows.Forms.TextBox();
            this.tbBackgroundColour = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cbStartWebserverOnLoad = new System.Windows.Forms.CheckBox();
            this.label71 = new System.Windows.Forms.Label();
            this.cbInterfaces = new System.Windows.Forms.ComboBox();
            this.txtWebserverPort = new System.Windows.Forms.TextBox();
            this.tabOCRGroup = new System.Windows.Forms.TabPage();
            this.tabCtrlOCR = new System.Windows.Forms.TabControl();
            this.tabOCR = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbOriginalImage = new System.Windows.Forms.PictureBox();
            this.pbTrimmed = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pbStation = new System.Windows.Forms.PictureBox();
            this.button7 = new System.Windows.Forms.Button();
            this.lblScreenshotsQueued = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbOcrStationName = new System.Windows.Forms.TextBox();
            this.pbOcrCurrent = new System.Windows.Forms.PictureBox();
            this.tbCommoditiesOcrOutput = new System.Windows.Forms.TextBox();
            this.tbConfidence = new System.Windows.Forms.TextBox();
            this.bContinueOcr = new System.Windows.Forms.Button();
            this.tbFinalOcrOutput = new System.Windows.Forms.TextBox();
            this.tbOcrSystemName = new System.Windows.Forms.TextBox();
            this.cbPostOnImport = new System.Windows.Forms.CheckBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.cmdHint = new System.Windows.Forms.Button();
            this.cbExtendedInfoInCSV = new System.Windows.Forms.CheckBox();
            this.cbStartOCROnLoad = new System.Windows.Forms.CheckBox();
            this.cbDeleteScreenshotOnImport = new System.Windows.Forms.CheckBox();
            this.cbUseEddnTestSchema = new System.Windows.Forms.CheckBox();
            this.cbAutoImport = new System.Windows.Forms.CheckBox();
            this.bEditResults = new System.Windows.Forms.Button();
            this.bClearOcrOutput = new System.Windows.Forms.Button();
            this.bIgnoreTrash = new System.Windows.Forms.Button();
            this.txtCmdrsName = new System.Windows.Forms.TextBox();
            this.rbUserID = new System.Windows.Forms.RadioButton();
            this.rbCmdrsName = new System.Windows.Forms.RadioButton();
            this.tabSystemData = new System.Windows.Forms.TabPage();
            this.gbSystemSystemData = new System.Windows.Forms.GroupBox();
            this.cmbSystemGovernment = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemAllegiance = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemState = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemSecurity = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemPrimaryEconomy = new System.Windows.Forms.ComboBox_ro();
            this.lblSystemRenameHint = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.txtSystemId = new System.Windows.Forms.TextBox();
            this.label57 = new System.Windows.Forms.Label();
            this.txtSystemName = new System.Windows.Forms.TextBox();
            this.label58 = new System.Windows.Forms.Label();
            this.txtSystemX = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.txtSystemY = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.txtSystemZ = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.txtSystemFaction = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.txtSystemPopulation = new System.Windows.Forms.MaskedTextBox();
            this.label69 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.txtSystemUpdatedAt = new System.Windows.Forms.TextBox();
            this.cmdSystemSave = new System.Windows.Forms.Button();
            this.cmdSystemNew = new System.Windows.Forms.Button();
            this.cbSystemNeedsPermit = new System.Windows.Forms.CheckBox_ro();
            this.cmbSystemsAllSystems = new System.Windows.Forms.ComboBox_ro();
            this.cmdSystemEdit = new System.Windows.Forms.Button();
            this.label88 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.lblSystemCountTotal = new System.Windows.Forms.Label();
            this.cmdSystemCancel = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.cmbStationMaxLandingPadSize = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationGovernment = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationAllegiance = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationState = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationType = new System.Windows.Forms.ComboBox_ro();
            this.lblStationRenameHint = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.txtStationId = new System.Windows.Forms.TextBox();
            this.label85 = new System.Windows.Forms.Label();
            this.txtStationName = new System.Windows.Forms.TextBox();
            this.label81 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.txtStationDistanceToStar = new System.Windows.Forms.TextBox();
            this.label79 = new System.Windows.Forms.Label();
            this.txtStationFaction = new System.Windows.Forms.TextBox();
            this.label78 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.cmdStationSave = new System.Windows.Forms.Button();
            this.cmdStationNew = new System.Windows.Forms.Button();
            this.label87 = new System.Windows.Forms.Label();
            this.cbStationHasBlackmarket = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasCommodities = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasRefuel = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasRearm = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasRepair = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasShipyard = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasOutfitting = new System.Windows.Forms.CheckBox_ro();
            this.label73 = new System.Windows.Forms.Label();
            this.txtStationUpdatedAt = new System.Windows.Forms.TextBox();
            this.label64 = new System.Windows.Forms.Label();
            this.lbStationEconomies = new System.Windows.Forms.ListBox();
            this.label72 = new System.Windows.Forms.Label();
            this.lblStationCount = new System.Windows.Forms.Label();
            this.cmdStationEdit = new System.Windows.Forms.Button();
            this.label90 = new System.Windows.Forms.Label();
            this.lblStationCountTotal = new System.Windows.Forms.Label();
            this.cmdStationCancel = new System.Windows.Forms.Button();
            this.paEconomies = new System.Windows.Forms.Panel();
            this.cbStationEcoAgriculture = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoExtraction = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoHighTech = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoIndustrial = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoMilitary = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoRefinery = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoService = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoTerraforming = new System.Windows.Forms.CheckBox_ro();
            this.cbStationEcoTourism = new System.Windows.Forms.CheckBox_ro();
            this.button8 = new System.Windows.Forms.Button();
            this.cbStationEcoNone = new System.Windows.Forms.CheckBox_ro();
            this.cmbStationStations = new System.Windows.Forms.ComboBox_ro();
            this.tabHelpAndChangeLog = new System.Windows.Forms.TabPage();
            this.lblRegulatedNoise = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbForegroundColour = new System.Windows.Forms.PictureBox();
            this.pbBackgroundColour = new System.Windows.Forms.PictureBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.button20 = new System.Windows.Forms.Button();
            this.ForegroundSet = new System.Windows.Forms.Label();
            this.BackgroundSet = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel7 = new System.Windows.Forms.LinkLabel();
            this.linkLabel6 = new System.Windows.Forms.LinkLabel();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel9 = new System.Windows.Forms.LinkLabel();
            this.label44 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.cmdDonate = new System.Windows.Forms.Button();
            this.lblUpdateDetail = new System.Windows.Forms.TextBox();
            this.lblUpdateInfo = new System.Windows.Forms.Label();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.llVisitUpdate = new System.Windows.Forms.LinkLabel();
            this.linkLabel10 = new System.Windows.Forms.LinkLabel();
            this.linkLabel11 = new System.Windows.Forms.LinkLabel();
            this.label91 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.tabCtrlMain = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsCommodities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.namesBindingSource)).BeginInit();
            this.msMainMenu.SuspendLayout();
            this.tabEDDN.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabWebserver.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabOCRGroup.SuspendLayout();
            this.tabCtrlOCR.SuspendLayout();
            this.tabOCR.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrimmed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStation)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOcrCurrent)).BeginInit();
            this.tabSystemData.SuspendLayout();
            this.gbSystemSystemData.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.paEconomies.SuspendLayout();
            this.tabHelpAndChangeLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColour)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabCtrlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "traineddata";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Tesseract-Files|*.traineddata|All Files|*.*";
            this.openFileDialog1.Title = "select Tesseract Traineddata-File";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySystenmameToClipboardToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(68, 26);
            // 
            // copySystenmameToClipboardToolStripMenuItem
            // 
            this.copySystenmameToClipboardToolStripMenuItem.Name = "copySystenmameToClipboardToolStripMenuItem";
            this.copySystenmameToClipboardToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
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
            // cmdLoadCurrentSystem
            // 
            this.cmdLoadCurrentSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLoadCurrentSystem.Location = new System.Drawing.Point(930, 45);
            this.cmdLoadCurrentSystem.Name = "cmdLoadCurrentSystem";
            this.cmdLoadCurrentSystem.Size = new System.Drawing.Size(148, 21);
            this.cmdLoadCurrentSystem.TabIndex = 60;
            this.cmdLoadCurrentSystem.Text = "Show Current System Data";
            this.cmdLoadCurrentSystem.UseVisualStyleBackColor = true;
            this.cmdLoadCurrentSystem.Click += new System.EventHandler(this.cmdLoadCurrentSystem_Click);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(499, 31);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(49, 13);
            this.label53.TabIndex = 15;
            this.label53.Text = "Universe";
            // 
            // txtEDTime
            // 
            this.txtEDTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEDTime.Location = new System.Drawing.Point(554, 26);
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
            this.label54.Location = new System.Drawing.Point(515, 7);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(33, 13);
            this.label54.TabIndex = 13;
            this.label54.Text = "Local";
            // 
            // txtLocalTime
            // 
            this.txtLocalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalTime.Location = new System.Drawing.Point(554, 2);
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
            this.label45.Location = new System.Drawing.Point(667, 28);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(152, 13);
            this.label45.TabIndex = 10;
            this.label45.Text = "Current Location (from log files)";
            // 
            // tbCurrentStationinfoFromLogs
            // 
            this.tbCurrentStationinfoFromLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentStationinfoFromLogs.Location = new System.Drawing.Point(823, 25);
            this.tbCurrentStationinfoFromLogs.Name = "tbCurrentStationinfoFromLogs";
            this.tbCurrentStationinfoFromLogs.ReadOnly = true;
            this.tbCurrentStationinfoFromLogs.Size = new System.Drawing.Size(255, 20);
            this.tbCurrentStationinfoFromLogs.TabIndex = 9;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(672, 7);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(145, 13);
            this.label37.TabIndex = 8;
            this.label37.Text = "Current System (from log files)";
            // 
            // tbCurrentSystemFromLogs
            // 
            this.tbCurrentSystemFromLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentSystemFromLogs.Location = new System.Drawing.Point(823, 4);
            this.tbCurrentSystemFromLogs.Name = "tbCurrentSystemFromLogs";
            this.tbCurrentSystemFromLogs.ReadOnly = true;
            this.tbCurrentSystemFromLogs.Size = new System.Drawing.Size(255, 20);
            this.tbCurrentSystemFromLogs.TabIndex = 7;
            this.tbCurrentSystemFromLogs.Text = "scanning...";
            // 
            // msMainMenu
            // 
            this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem});
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new System.Drawing.Size(1089, 24);
            this.msMainMenu.TabIndex = 61;
            this.msMainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // tabEDDN
            // 
            this.tabEDDN.Controls.Add(this.label83);
            this.tabEDDN.Controls.Add(this.lbEddnImplausible);
            this.tabEDDN.Controls.Add(this.tbEddnStats);
            this.tabEDDN.Controls.Add(this.groupBox2);
            this.tabEDDN.Location = new System.Drawing.Point(4, 22);
            this.tabEDDN.Name = "tabEDDN";
            this.tabEDDN.Size = new System.Drawing.Size(1076, 611);
            this.tabEDDN.TabIndex = 7;
            this.tabEDDN.Text = "EDDN";
            this.tabEDDN.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbEDDNAutoListen);
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
            this.groupBox2.Size = new System.Drawing.Size(450, 379);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Listen for EDDN Events";
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
            // tbEDDNOutput
            // 
            this.tbEDDNOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEDDNOutput.Location = new System.Drawing.Point(6, 190);
            this.tbEDDNOutput.Multiline = true;
            this.tbEDDNOutput.Name = "tbEDDNOutput";
            this.tbEDDNOutput.Size = new System.Drawing.Size(438, 152);
            this.tbEDDNOutput.TabIndex = 3;
            // 
            // cmdStopEDDNListening
            // 
            this.cmdStopEDDNListening.Location = new System.Drawing.Point(351, 127);
            this.cmdStopEDDNListening.Name = "cmdStopEDDNListening";
            this.cmdStopEDDNListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStopEDDNListening.TabIndex = 4;
            this.cmdStopEDDNListening.Text = "Stop Listening";
            this.cmdStopEDDNListening.UseVisualStyleBackColor = true;
            this.cmdStopEDDNListening.Click += new System.EventHandler(this.cmdStopEDDNListening_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(7, 174);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(64, 13);
            this.label24.TabIndex = 5;
            this.label24.Text = "Raw Output";
            // 
            // checkboxImportEDDN
            // 
            this.checkboxImportEDDN.AutoSize = true;
            this.checkboxImportEDDN.Checked = true;
            this.checkboxImportEDDN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxImportEDDN.Location = new System.Drawing.Point(10, 18);
            this.checkboxImportEDDN.Name = "checkboxImportEDDN";
            this.checkboxImportEDDN.Size = new System.Drawing.Size(221, 17);
            this.checkboxImportEDDN.TabIndex = 6;
            this.checkboxImportEDDN.Text = "import received data into RegulatedNoise";
            this.checkboxImportEDDN.UseVisualStyleBackColor = true;
            this.checkboxImportEDDN.CheckedChanged += new System.EventHandler(this.checkboxImportEDDN_CheckedChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(7, 107);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(271, 13);
            this.label25.TabIndex = 7;
            this.label25.Text = "The UI will be updated once every ten seconds at most;";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(7, 124);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(241, 13);
            this.label27.TabIndex = 9;
            this.label27.Text = "Obsolete Information checking will be suspended;";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(7, 142);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(280, 13);
            this.label28.TabIndex = 10;
            this.label28.Text = "No idea how much data this can collect before collapsing.";
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(283, 161);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(161, 23);
            this.button17.TabIndex = 11;
            this.button17.Text = "Flush all EDDN data to UI";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // bPurgeAllEddnData
            // 
            this.bPurgeAllEddnData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bPurgeAllEddnData.Location = new System.Drawing.Point(315, 348);
            this.bPurgeAllEddnData.Name = "bPurgeAllEddnData";
            this.bPurgeAllEddnData.Size = new System.Drawing.Size(129, 23);
            this.bPurgeAllEddnData.TabIndex = 12;
            this.bPurgeAllEddnData.Text = "Purge all EDDN data";
            this.bPurgeAllEddnData.UseVisualStyleBackColor = true;
            this.bPurgeAllEddnData.Click += new System.EventHandler(this.cmdPurgeEDDNData);
            // 
            // cbSpoolEddnToFile
            // 
            this.cbSpoolEddnToFile.AutoSize = true;
            this.cbSpoolEddnToFile.Location = new System.Drawing.Point(10, 34);
            this.cbSpoolEddnToFile.Name = "cbSpoolEddnToFile";
            this.cbSpoolEddnToFile.Size = new System.Drawing.Size(137, 17);
            this.cbSpoolEddnToFile.TabIndex = 13;
            this.cbSpoolEddnToFile.Text = "spool to EddnOutput.txt";
            this.cbSpoolEddnToFile.UseVisualStyleBackColor = true;
            this.cbSpoolEddnToFile.CheckedChanged += new System.EventHandler(this.cbSpoolEddnToFile_CheckedChanged);
            // 
            // cbSpoolImplausibleToFile
            // 
            this.cbSpoolImplausibleToFile.AutoSize = true;
            this.cbSpoolImplausibleToFile.Location = new System.Drawing.Point(10, 50);
            this.cbSpoolImplausibleToFile.Name = "cbSpoolImplausibleToFile";
            this.cbSpoolImplausibleToFile.Size = new System.Drawing.Size(208, 17);
            this.cbSpoolImplausibleToFile.TabIndex = 14;
            this.cbSpoolImplausibleToFile.Text = "spool implausible to EddnImpOutput.txt";
            this.cbSpoolImplausibleToFile.UseVisualStyleBackColor = true;
            this.cbSpoolImplausibleToFile.CheckedChanged += new System.EventHandler(this.cbSpoolImplausibleToFile_CheckedChanged);
            // 
            // cbEDDNAutoListen
            // 
            this.cbEDDNAutoListen.AutoSize = true;
            this.cbEDDNAutoListen.Location = new System.Drawing.Point(10, 67);
            this.cbEDDNAutoListen.Name = "cbEDDNAutoListen";
            this.cbEDDNAutoListen.Size = new System.Drawing.Size(187, 17);
            this.cbEDDNAutoListen.TabIndex = 15;
            this.cbEDDNAutoListen.Text = "autostart listening on program start";
            this.cbEDDNAutoListen.UseVisualStyleBackColor = true;
            this.cbEDDNAutoListen.CheckedChanged += new System.EventHandler(this.cbEDDNAutoListen_CheckedChanged);
            // 
            // tbEddnStats
            // 
            this.tbEddnStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEddnStats.Location = new System.Drawing.Point(459, 3);
            this.tbEddnStats.Multiline = true;
            this.tbEddnStats.Name = "tbEddnStats";
            this.tbEddnStats.Size = new System.Drawing.Size(589, 379);
            this.tbEddnStats.TabIndex = 1;
            // 
            // lbEddnImplausible
            // 
            this.lbEddnImplausible.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbEddnImplausible.FormattingEnabled = true;
            this.lbEddnImplausible.HorizontalScrollbar = true;
            this.lbEddnImplausible.Location = new System.Drawing.Point(3, 401);
            this.lbEddnImplausible.Name = "lbEddnImplausible";
            this.lbEddnImplausible.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEddnImplausible.Size = new System.Drawing.Size(1045, 173);
            this.lbEddnImplausible.TabIndex = 2;
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(6, 385);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(76, 13);
            this.label83.TabIndex = 6;
            this.label83.Text = "Rejected Data";
            // 
            // tabWebserver
            // 
            this.tabWebserver.Controls.Add(this.groupBox1);
            this.tabWebserver.Location = new System.Drawing.Point(4, 22);
            this.tabWebserver.Name = "tabWebserver";
            this.tabWebserver.Size = new System.Drawing.Size(1076, 611);
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 63);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(535, 104);
            this.label10.TabIndex = 5;
            this.label10.Text = resources.GetString("label10.Text");
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
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 215);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(79, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Colour Scheme";
            // 
            // tbForegroundColour
            // 
            this.tbForegroundColour.Location = new System.Drawing.Point(136, 232);
            this.tbForegroundColour.Name = "tbForegroundColour";
            this.tbForegroundColour.Size = new System.Drawing.Size(87, 20);
            this.tbForegroundColour.TabIndex = 9;
            this.tbForegroundColour.TextChanged += new System.EventHandler(this.tbForegroundColour_TextChanged);
            // 
            // tbBackgroundColour
            // 
            this.tbBackgroundColour.Location = new System.Drawing.Point(229, 231);
            this.tbBackgroundColour.Name = "tbBackgroundColour";
            this.tbBackgroundColour.Size = new System.Drawing.Size(87, 20);
            this.tbBackgroundColour.TabIndex = 10;
            this.tbBackgroundColour.TextChanged += new System.EventHandler(this.tbBackgroundColour_TextChanged);
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
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(226, 215);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 13);
            this.label17.TabIndex = 12;
            this.label17.Text = "Background";
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
            // cbInterfaces
            // 
            this.cbInterfaces.FormattingEnabled = true;
            this.cbInterfaces.Location = new System.Drawing.Point(10, 18);
            this.cbInterfaces.Name = "cbInterfaces";
            this.cbInterfaces.Size = new System.Drawing.Size(130, 21);
            this.cbInterfaces.TabIndex = 3;
            this.cbInterfaces.SelectedIndexChanged += new System.EventHandler(this.cbInterfaces_SelectedIndexChanged);
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
            // tabOCRGroup
            // 
            this.tabOCRGroup.Controls.Add(this.tabCtrlOCR);
            this.tabOCRGroup.Location = new System.Drawing.Point(4, 22);
            this.tabOCRGroup.Name = "tabOCRGroup";
            this.tabOCRGroup.Size = new System.Drawing.Size(1076, 611);
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
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Original";
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
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 309);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "Trimmed";
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
            // lblScreenshotsQueued
            // 
            this.lblScreenshotsQueued.AutoSize = true;
            this.lblScreenshotsQueued.Location = new System.Drawing.Point(43, 52);
            this.lblScreenshotsQueued.Name = "lblScreenshotsQueued";
            this.lblScreenshotsQueued.Size = new System.Drawing.Size(58, 13);
            this.lblScreenshotsQueued.TabIndex = 10;
            this.lblScreenshotsQueued.Text = "(0 queued)";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.rbCmdrsName);
            this.groupBox4.Controls.Add(this.rbUserID);
            this.groupBox4.Controls.Add(this.txtCmdrsName);
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
            // tbOcrStationName
            // 
            this.tbOcrStationName.Location = new System.Drawing.Point(110, 27);
            this.tbOcrStationName.Name = "tbOcrStationName";
            this.tbOcrStationName.Size = new System.Drawing.Size(231, 20);
            this.tbOcrStationName.TabIndex = 10;
            this.tbOcrStationName.TextChanged += new System.EventHandler(this.tbOcrStationName_TextChanged);
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
            // tbConfidence
            // 
            this.tbConfidence.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfidence.Location = new System.Drawing.Point(347, 128);
            this.tbConfidence.Name = "tbConfidence";
            this.tbConfidence.Size = new System.Drawing.Size(62, 29);
            this.tbConfidence.TabIndex = 13;
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
            // tbOcrSystemName
            // 
            this.tbOcrSystemName.Location = new System.Drawing.Point(110, 53);
            this.tbOcrSystemName.Name = "tbOcrSystemName";
            this.tbOcrSystemName.Size = new System.Drawing.Size(231, 20);
            this.tbOcrSystemName.TabIndex = 16;
            this.tbOcrSystemName.TextChanged += new System.EventHandler(this.tbOcrSystemName_TextChanged);
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
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(347, 469);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(230, 20);
            this.tbUsername.TabIndex = 20;
            this.tbUsername.TextChanged += new System.EventHandler(this.tbUsername_TextChanged);
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
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(11, 52);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(72, 13);
            this.label33.TabIndex = 22;
            this.label33.Text = "System Name";
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
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(11, 137);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(71, 13);
            this.label35.TabIndex = 24;
            this.label35.Text = "Correct Value";
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
            // cbUseEddnTestSchema
            // 
            this.cbUseEddnTestSchema.AutoSize = true;
            this.cbUseEddnTestSchema.Location = new System.Drawing.Point(6, 492);
            this.cbUseEddnTestSchema.Name = "cbUseEddnTestSchema";
            this.cbUseEddnTestSchema.Size = new System.Drawing.Size(89, 17);
            this.cbUseEddnTestSchema.TabIndex = 30;
            this.cbUseEddnTestSchema.Text = "Test Schema";
            this.cbUseEddnTestSchema.UseVisualStyleBackColor = true;
            this.cbUseEddnTestSchema.CheckedChanged += new System.EventHandler(this.cbUseEddnTestSchema_CheckedChanged);
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
            // txtCmdrsName
            // 
            this.txtCmdrsName.Location = new System.Drawing.Point(347, 493);
            this.txtCmdrsName.Name = "txtCmdrsName";
            this.txtCmdrsName.ReadOnly = true;
            this.txtCmdrsName.Size = new System.Drawing.Size(230, 20);
            this.txtCmdrsName.TabIndex = 36;
            // 
            // rbUserID
            // 
            this.rbUserID.AutoSize = true;
            this.rbUserID.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbUserID.Location = new System.Drawing.Point(277, 472);
            this.rbUserID.Name = "rbUserID";
            this.rbUserID.Size = new System.Drawing.Size(61, 17);
            this.rbUserID.TabIndex = 38;
            this.rbUserID.TabStop = true;
            this.rbUserID.Text = "User ID";
            this.rbUserID.UseVisualStyleBackColor = true;
            this.rbUserID.CheckedChanged += new System.EventHandler(this.rbUserID_CheckedChanged);
            // 
            // rbCmdrsName
            // 
            this.rbCmdrsName.AutoSize = true;
            this.rbCmdrsName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbCmdrsName.Location = new System.Drawing.Point(253, 493);
            this.rbCmdrsName.Name = "rbCmdrsName";
            this.rbCmdrsName.Size = new System.Drawing.Size(85, 17);
            this.rbCmdrsName.TabIndex = 39;
            this.rbCmdrsName.TabStop = true;
            this.rbCmdrsName.Text = "Cmdrs Name";
            this.rbCmdrsName.UseVisualStyleBackColor = true;
            this.rbCmdrsName.CheckedChanged += new System.EventHandler(this.rbCmdrsName_CheckedChanged);
            // 
            // tabSystemData
            // 
            this.tabSystemData.Controls.Add(this.groupBox14);
            this.tabSystemData.Controls.Add(this.gbSystemSystemData);
            this.tabSystemData.Location = new System.Drawing.Point(4, 22);
            this.tabSystemData.Name = "tabSystemData";
            this.tabSystemData.Size = new System.Drawing.Size(1076, 611);
            this.tabSystemData.TabIndex = 13;
            this.tabSystemData.Text = "System Data";
            this.tabSystemData.UseVisualStyleBackColor = true;
            // 
            // gbSystemSystemData
            // 
            this.gbSystemSystemData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSystemSystemData.Controls.Add(this.cmdSystemCancel);
            this.gbSystemSystemData.Controls.Add(this.lblSystemCountTotal);
            this.gbSystemSystemData.Controls.Add(this.label82);
            this.gbSystemSystemData.Controls.Add(this.label84);
            this.gbSystemSystemData.Controls.Add(this.label88);
            this.gbSystemSystemData.Controls.Add(this.cmdSystemEdit);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemsAllSystems);
            this.gbSystemSystemData.Controls.Add(this.cbSystemNeedsPermit);
            this.gbSystemSystemData.Controls.Add(this.cmdSystemNew);
            this.gbSystemSystemData.Controls.Add(this.cmdSystemSave);
            this.gbSystemSystemData.Controls.Add(this.txtSystemUpdatedAt);
            this.gbSystemSystemData.Controls.Add(this.label63);
            this.gbSystemSystemData.Controls.Add(this.label65);
            this.gbSystemSystemData.Controls.Add(this.label66);
            this.gbSystemSystemData.Controls.Add(this.label67);
            this.gbSystemSystemData.Controls.Add(this.label68);
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
            this.gbSystemSystemData.Controls.Add(this.cmbSystemPrimaryEconomy);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemSecurity);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemState);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemAllegiance);
            this.gbSystemSystemData.Controls.Add(this.cmbSystemGovernment);
            this.gbSystemSystemData.Location = new System.Drawing.Point(14, 49);
            this.gbSystemSystemData.Name = "gbSystemSystemData";
            this.gbSystemSystemData.Size = new System.Drawing.Size(486, 524);
            this.gbSystemSystemData.TabIndex = 0;
            this.gbSystemSystemData.TabStop = false;
            this.gbSystemSystemData.Text = "System Data";
            // 
            // cmbSystemGovernment
            // 
            this.cmbSystemGovernment.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbSystemGovernment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemGovernment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemGovernment.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbSystemGovernment.Location = new System.Drawing.Point(295, 228);
            this.cmbSystemGovernment.Name = "cmbSystemGovernment";
            this.cmbSystemGovernment.ReadOnly = false;
            this.cmbSystemGovernment.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemGovernment.TabIndex = 10;
            // 
            // cmbSystemAllegiance
            // 
            this.cmbSystemAllegiance.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbSystemAllegiance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemAllegiance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemAllegiance.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbSystemAllegiance.Location = new System.Drawing.Point(295, 256);
            this.cmbSystemAllegiance.Name = "cmbSystemAllegiance";
            this.cmbSystemAllegiance.ReadOnly = false;
            this.cmbSystemAllegiance.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemAllegiance.TabIndex = 11;
            // 
            // cmbSystemState
            // 
            this.cmbSystemState.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbSystemState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemState.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbSystemState.Location = new System.Drawing.Point(295, 283);
            this.cmbSystemState.Name = "cmbSystemState";
            this.cmbSystemState.ReadOnly = false;
            this.cmbSystemState.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemState.TabIndex = 12;
            // 
            // cmbSystemSecurity
            // 
            this.cmbSystemSecurity.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbSystemSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemSecurity.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbSystemSecurity.Location = new System.Drawing.Point(295, 174);
            this.cmbSystemSecurity.Name = "cmbSystemSecurity";
            this.cmbSystemSecurity.ReadOnly = false;
            this.cmbSystemSecurity.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemSecurity.TabIndex = 8;
            // 
            // cmbSystemPrimaryEconomy
            // 
            this.cmbSystemPrimaryEconomy.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbSystemPrimaryEconomy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemPrimaryEconomy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemPrimaryEconomy.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbSystemPrimaryEconomy.Location = new System.Drawing.Point(295, 201);
            this.cmbSystemPrimaryEconomy.Name = "cmbSystemPrimaryEconomy";
            this.cmbSystemPrimaryEconomy.ReadOnly = false;
            this.cmbSystemPrimaryEconomy.Size = new System.Drawing.Size(151, 23);
            this.cmbSystemPrimaryEconomy.TabIndex = 13;
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
            // txtSystemX
            // 
            this.txtSystemX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemX.Location = new System.Drawing.Point(89, 147);
            this.txtSystemX.Name = "txtSystemX";
            this.txtSystemX.Size = new System.Drawing.Size(92, 21);
            this.txtSystemX.TabIndex = 3;
            this.txtSystemX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            // txtSystemY
            // 
            this.txtSystemY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemY.Location = new System.Drawing.Point(89, 174);
            this.txtSystemY.Name = "txtSystemY";
            this.txtSystemY.Size = new System.Drawing.Size(92, 21);
            this.txtSystemY.TabIndex = 4;
            this.txtSystemY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            // txtSystemZ
            // 
            this.txtSystemZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemZ.Location = new System.Drawing.Point(89, 203);
            this.txtSystemZ.Name = "txtSystemZ";
            this.txtSystemZ.Size = new System.Drawing.Size(92, 21);
            this.txtSystemZ.TabIndex = 5;
            this.txtSystemZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            // txtSystemFaction
            // 
            this.txtSystemFaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemFaction.Location = new System.Drawing.Point(89, 310);
            this.txtSystemFaction.Name = "txtSystemFaction";
            this.txtSystemFaction.Size = new System.Drawing.Size(357, 21);
            this.txtSystemFaction.TabIndex = 9;
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
            // txtSystemUpdatedAt
            // 
            this.txtSystemUpdatedAt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemUpdatedAt.Location = new System.Drawing.Point(295, 372);
            this.txtSystemUpdatedAt.Name = "txtSystemUpdatedAt";
            this.txtSystemUpdatedAt.ReadOnly = true;
            this.txtSystemUpdatedAt.Size = new System.Drawing.Size(151, 21);
            this.txtSystemUpdatedAt.TabIndex = 14;
            this.txtSystemUpdatedAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // cmbSystemsAllSystems
            // 
            this.cmbSystemsAllSystems.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSystemsAllSystems.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSystemsAllSystems.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbSystemsAllSystems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemsAllSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSystemsAllSystems.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbSystemsAllSystems.Location = new System.Drawing.Point(89, 33);
            this.cmbSystemsAllSystems.Name = "cmbSystemsAllSystems";
            this.cmbSystemsAllSystems.ReadOnly = false;
            this.cmbSystemsAllSystems.Size = new System.Drawing.Size(254, 23);
            this.cmbSystemsAllSystems.TabIndex = 0;
            this.cmbSystemsAllSystems.SelectedIndexChanged += new System.EventHandler(this.cmbAllStations_SelectedIndexChanged);
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
            this.groupBox14.Controls.Add(this.label75);
            this.groupBox14.Controls.Add(this.label76);
            this.groupBox14.Controls.Add(this.label77);
            this.groupBox14.Controls.Add(this.label78);
            this.groupBox14.Controls.Add(this.txtStationFaction);
            this.groupBox14.Controls.Add(this.label79);
            this.groupBox14.Controls.Add(this.txtStationDistanceToStar);
            this.groupBox14.Controls.Add(this.label80);
            this.groupBox14.Controls.Add(this.label81);
            this.groupBox14.Controls.Add(this.txtStationName);
            this.groupBox14.Controls.Add(this.label85);
            this.groupBox14.Controls.Add(this.txtStationId);
            this.groupBox14.Controls.Add(this.label86);
            this.groupBox14.Controls.Add(this.lblStationRenameHint);
            this.groupBox14.Controls.Add(this.cmbStationType);
            this.groupBox14.Controls.Add(this.cmbStationState);
            this.groupBox14.Controls.Add(this.cmbStationAllegiance);
            this.groupBox14.Controls.Add(this.cmbStationGovernment);
            this.groupBox14.Controls.Add(this.cmbStationMaxLandingPadSize);
            this.groupBox14.Location = new System.Drawing.Point(506, 49);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(530, 524);
            this.groupBox14.TabIndex = 0;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Station Data";
            // 
            // cmbStationMaxLandingPadSize
            // 
            this.cmbStationMaxLandingPadSize.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbStationMaxLandingPadSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationMaxLandingPadSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationMaxLandingPadSize.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbStationMaxLandingPadSize.Location = new System.Drawing.Point(127, 174);
            this.cmbStationMaxLandingPadSize.Name = "cmbStationMaxLandingPadSize";
            this.cmbStationMaxLandingPadSize.ReadOnly = false;
            this.cmbStationMaxLandingPadSize.Size = new System.Drawing.Size(68, 23);
            this.cmbStationMaxLandingPadSize.TabIndex = 4;
            // 
            // cmbStationGovernment
            // 
            this.cmbStationGovernment.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbStationGovernment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationGovernment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationGovernment.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbStationGovernment.Location = new System.Drawing.Point(127, 229);
            this.cmbStationGovernment.Name = "cmbStationGovernment";
            this.cmbStationGovernment.ReadOnly = false;
            this.cmbStationGovernment.Size = new System.Drawing.Size(151, 23);
            this.cmbStationGovernment.TabIndex = 6;
            // 
            // cmbStationAllegiance
            // 
            this.cmbStationAllegiance.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbStationAllegiance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationAllegiance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationAllegiance.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbStationAllegiance.Location = new System.Drawing.Point(127, 255);
            this.cmbStationAllegiance.Name = "cmbStationAllegiance";
            this.cmbStationAllegiance.ReadOnly = false;
            this.cmbStationAllegiance.Size = new System.Drawing.Size(151, 23);
            this.cmbStationAllegiance.TabIndex = 7;
            // 
            // cmbStationState
            // 
            this.cmbStationState.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbStationState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationState.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbStationState.Location = new System.Drawing.Point(127, 281);
            this.cmbStationState.Name = "cmbStationState";
            this.cmbStationState.ReadOnly = false;
            this.cmbStationState.Size = new System.Drawing.Size(151, 23);
            this.cmbStationState.TabIndex = 8;
            // 
            // cmbStationType
            // 
            this.cmbStationType.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbStationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationType.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbStationType.Location = new System.Drawing.Point(127, 202);
            this.cmbStationType.Name = "cmbStationType";
            this.cmbStationType.ReadOnly = false;
            this.cmbStationType.Size = new System.Drawing.Size(151, 23);
            this.cmbStationType.TabIndex = 9;
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
            // txtStationName
            // 
            this.txtStationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationName.Location = new System.Drawing.Point(114, 89);
            this.txtStationName.Name = "txtStationName";
            this.txtStationName.ReadOnly = true;
            this.txtStationName.Size = new System.Drawing.Size(236, 21);
            this.txtStationName.TabIndex = 2;
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
            // txtStationDistanceToStar
            // 
            this.txtStationDistanceToStar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationDistanceToStar.Location = new System.Drawing.Point(127, 147);
            this.txtStationDistanceToStar.Name = "txtStationDistanceToStar";
            this.txtStationDistanceToStar.Size = new System.Drawing.Size(68, 21);
            this.txtStationDistanceToStar.TabIndex = 3;
            this.txtStationDistanceToStar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            // txtStationFaction
            // 
            this.txtStationFaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationFaction.Location = new System.Drawing.Point(127, 308);
            this.txtStationFaction.Name = "txtStationFaction";
            this.txtStationFaction.Size = new System.Drawing.Size(352, 21);
            this.txtStationFaction.TabIndex = 5;
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
            // txtStationUpdatedAt
            // 
            this.txtStationUpdatedAt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStationUpdatedAt.Location = new System.Drawing.Point(328, 372);
            this.txtStationUpdatedAt.Name = "txtStationUpdatedAt";
            this.txtStationUpdatedAt.ReadOnly = true;
            this.txtStationUpdatedAt.Size = new System.Drawing.Size(151, 21);
            this.txtStationUpdatedAt.TabIndex = 18;
            this.txtStationUpdatedAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label64.Location = new System.Drawing.Point(22, 335);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(59, 13);
            this.label64.TabIndex = 119;
            this.label64.Text = "Economies";
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
            // cmbStationStations
            // 
            this.cmbStationStations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStationStations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStationStations.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cmbStationStations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationStations.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStationStations.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cmbStationStations.Location = new System.Drawing.Point(114, 31);
            this.cmbStationStations.Name = "cmbStationStations";
            this.cmbStationStations.ReadOnly = false;
            this.cmbStationStations.Size = new System.Drawing.Size(236, 23);
            this.cmbStationStations.TabIndex = 130;
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
            this.tabHelpAndChangeLog.Size = new System.Drawing.Size(1076, 611);
            this.tabHelpAndChangeLog.TabIndex = 9;
            this.tabHelpAndChangeLog.Text = "Help and Changelog";
            this.tabHelpAndChangeLog.UseVisualStyleBackColor = true;
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
            // lblUpdateDetail
            // 
            this.lblUpdateDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateDetail.Location = new System.Drawing.Point(30, 295);
            this.lblUpdateDetail.Multiline = true;
            this.lblUpdateDetail.Name = "lblUpdateDetail";
            this.lblUpdateDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lblUpdateDetail.Size = new System.Drawing.Size(664, 214);
            this.lblUpdateDetail.TabIndex = 39;
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
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Location = new System.Drawing.Point(27, 186);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(63, 13);
            this.label91.TabIndex = 45;
            this.label91.Text = "Project-Link";
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
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(297, 221);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(121, 13);
            this.label93.TabIndex = 47;
            this.label93.Text = "RN im deutschen Forum";
            // 
            // tabCtrlMain
            // 
            this.tabCtrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlMain.Controls.Add(this.tabHelpAndChangeLog);
            this.tabCtrlMain.Controls.Add(this.tabSystemData);
            this.tabCtrlMain.Controls.Add(this.tabOCRGroup);
            this.tabCtrlMain.Controls.Add(this.tabWebserver);
            this.tabCtrlMain.Controls.Add(this.tabEDDN);
            this.tabCtrlMain.Location = new System.Drawing.Point(3, 48);
            this.tabCtrlMain.Name = "tabCtrlMain";
            this.tabCtrlMain.SelectedIndex = 0;
            this.tabCtrlMain.Size = new System.Drawing.Size(1084, 637);
            this.tabCtrlMain.TabIndex = 4;
            this.tabCtrlMain.SelectedIndexChanged += new System.EventHandler(this.tabCtrlMain_SelectedIndexChanged);
            this.tabCtrlMain.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabCtrlMain_Selecting);
            this.tabCtrlMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1089, 686);
            this.Controls.Add(this.msMainMenu);
            this.Controls.Add(this.cmdLoadCurrentSystem);
            this.Controls.Add(this.label53);
            this.Controls.Add(this.txtEDTime);
            this.Controls.Add(this.label54);
            this.Controls.Add(this.txtLocalTime);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.tbCurrentStationinfoFromLogs);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.tbCurrentSystemFromLogs);
            this.Controls.Add(this.tabCtrlMain);
            this.KeyPreview = true;
            this.MainMenuStrip = this.msMainMenu;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "RegulatedNoise v";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsCommodities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.namesBindingSource)).EndInit();
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            this.tabEDDN.ResumeLayout(false);
            this.tabEDDN.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabWebserver.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabOCRGroup.ResumeLayout(false);
            this.tabCtrlOCR.ResumeLayout(false);
            this.tabOCR.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrimmed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStation)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOcrCurrent)).EndInit();
            this.tabSystemData.ResumeLayout(false);
            this.gbSystemSystemData.ResumeLayout(false);
            this.gbSystemSystemData.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.paEconomies.ResumeLayout(false);
            this.paEconomies.PerformLayout();
            this.tabHelpAndChangeLog.ResumeLayout(false);
            this.tabHelpAndChangeLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColour)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabCtrlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox tbCurrentSystemFromLogs;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private RegulatedNoise.Enums_and_Utility_Classes.dsCommodities dsCommodities;
        private System.Windows.Forms.BindingSource namesBindingSource;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.TextBox tbCurrentStationinfoFromLogs;
        private System.Windows.Forms.Label label45;
        public System.Windows.Forms.TextBox txtLocalTime;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label53;
        public System.Windows.Forms.TextBox txtEDTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copySystenmameToClipboardToolStripMenuItem;
        private System.Windows.Forms.Button cmdLoadCurrentSystem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeEconomyToolStripMenuItem;
        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.TabPage tabEDDN;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.ListBox lbEddnImplausible;
        private System.Windows.Forms.TextBox tbEddnStats;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbEDDNAutoListen;
        private System.Windows.Forms.CheckBox cbSpoolImplausibleToFile;
        private System.Windows.Forms.CheckBox cbSpoolEddnToFile;
        private System.Windows.Forms.Button bPurgeAllEddnData;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox checkboxImportEDDN;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button cmdStopEDDNListening;
        public System.Windows.Forms.TextBox tbEDDNOutput;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.TabPage tabWebserver;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtWebserverPort;
        private System.Windows.Forms.ComboBox cbInterfaces;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.CheckBox cbStartWebserverOnLoad;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbBackgroundColour;
        private System.Windows.Forms.TextBox tbForegroundColour;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbColourScheme;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TabPage tabOCRGroup;
        private System.Windows.Forms.TabControl tabCtrlOCR;
        private System.Windows.Forms.TabPage tabOCR;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbCmdrsName;
        private System.Windows.Forms.RadioButton rbUserID;
        internal System.Windows.Forms.TextBox txtCmdrsName;
        private System.Windows.Forms.Button bIgnoreTrash;
        private System.Windows.Forms.Button bClearOcrOutput;
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
        private System.Windows.Forms.TabPage tabSystemData;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.ComboBox_ro cmbStationStations;
        private System.Windows.Forms.Panel paEconomies;
        private System.Windows.Forms.CheckBox_ro cbStationEcoNone;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.CheckBox_ro cbStationEcoTourism;
        private System.Windows.Forms.CheckBox_ro cbStationEcoTerraforming;
        private System.Windows.Forms.CheckBox_ro cbStationEcoService;
        private System.Windows.Forms.CheckBox_ro cbStationEcoRefinery;
        private System.Windows.Forms.CheckBox_ro cbStationEcoMilitary;
        private System.Windows.Forms.CheckBox_ro cbStationEcoIndustrial;
        private System.Windows.Forms.CheckBox_ro cbStationEcoHighTech;
        private System.Windows.Forms.CheckBox_ro cbStationEcoExtraction;
        private System.Windows.Forms.CheckBox_ro cbStationEcoAgriculture;
        private System.Windows.Forms.Button cmdStationCancel;
        private System.Windows.Forms.Label lblStationCountTotal;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.Button cmdStationEdit;
        private System.Windows.Forms.Label lblStationCount;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.ListBox lbStationEconomies;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.TextBox txtStationUpdatedAt;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.CheckBox_ro cbStationHasOutfitting;
        private System.Windows.Forms.CheckBox_ro cbStationHasShipyard;
        private System.Windows.Forms.CheckBox_ro cbStationHasRepair;
        private System.Windows.Forms.CheckBox_ro cbStationHasRearm;
        private System.Windows.Forms.CheckBox_ro cbStationHasRefuel;
        private System.Windows.Forms.CheckBox_ro cbStationHasCommodities;
        private System.Windows.Forms.CheckBox_ro cbStationHasBlackmarket;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Button cmdStationNew;
        private System.Windows.Forms.Button cmdStationSave;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.TextBox txtStationFaction;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.TextBox txtStationDistanceToStar;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.TextBox txtStationName;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.TextBox txtStationId;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label lblStationRenameHint;
        private System.Windows.Forms.ComboBox_ro cmbStationType;
        private System.Windows.Forms.ComboBox_ro cmbStationState;
        private System.Windows.Forms.ComboBox_ro cmbStationAllegiance;
        private System.Windows.Forms.ComboBox_ro cmbStationGovernment;
        private System.Windows.Forms.ComboBox_ro cmbStationMaxLandingPadSize;
        private System.Windows.Forms.GroupBox gbSystemSystemData;
        private System.Windows.Forms.Button cmdSystemCancel;
        private System.Windows.Forms.Label lblSystemCountTotal;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Button cmdSystemEdit;
        private System.Windows.Forms.ComboBox_ro cmbSystemsAllSystems;
        private System.Windows.Forms.CheckBox_ro cbSystemNeedsPermit;
        private System.Windows.Forms.Button cmdSystemNew;
        private System.Windows.Forms.Button cmdSystemSave;
        private System.Windows.Forms.TextBox txtSystemUpdatedAt;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Label label69;
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
        private System.Windows.Forms.Label lblSystemRenameHint;
        private System.Windows.Forms.ComboBox_ro cmbSystemPrimaryEconomy;
        private System.Windows.Forms.ComboBox_ro cmbSystemSecurity;
        private System.Windows.Forms.ComboBox_ro cmbSystemState;
        private System.Windows.Forms.ComboBox_ro cmbSystemAllegiance;
        private System.Windows.Forms.ComboBox_ro cmbSystemGovernment;
        private System.Windows.Forms.TabPage tabHelpAndChangeLog;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.LinkLabel linkLabel11;
        private System.Windows.Forms.LinkLabel linkLabel10;
        private System.Windows.Forms.LinkLabel llVisitUpdate;
        private System.Windows.Forms.Button cmdUpdate;
        internal System.Windows.Forms.Label lblUpdateInfo;
        internal System.Windows.Forms.TextBox lblUpdateDetail;
        private System.Windows.Forms.Button cmdDonate;
        private System.Windows.Forms.Label label42;
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
        private System.Windows.Forms.Label BackgroundSet;
        private System.Windows.Forms.Label ForegroundSet;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.PictureBox pbBackgroundColour;
        private System.Windows.Forms.PictureBox pbForegroundColour;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblRegulatedNoise;
        private System.Windows.Forms.TabControl tabCtrlMain;

        
    }
}

