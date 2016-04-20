namespace IBE
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
            this.dsCommodities = new IBE.Enums_and_Utility_Classes.dsCommodities();
            this.namesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.msMainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editLocalizationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.directDBAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createJumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.eDDNInterfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdLoadCurrentSystem = new System.Windows.Forms.Button();
            this.label53 = new System.Windows.Forms.Label();
            this.txtEDTime = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.txtLocalTime = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.tbCurrentStationinfoFromLogs = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.tbCurrentSystemFromLogs = new System.Windows.Forms.TextBox();
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
            this.lblUpdateDetail = new System.Windows.Forms.TextBox();
            this.cmdDonate = new System.Windows.Forms.Button();
            this.lblDonate = new System.Windows.Forms.Label();
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
            this.cbStationHasMarket = new System.Windows.Forms.CheckBox_ro();
            this.cbStationHasBlackmarket = new System.Windows.Forms.CheckBox_ro();
            this.label87 = new System.Windows.Forms.Label();
            this.cmdStationNew = new System.Windows.Forms.Button();
            this.cmdStationSave = new System.Windows.Forms.Button();
            this.label75 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.txtStationFaction = new System.Windows.Forms.TextBox();
            this.label79 = new System.Windows.Forms.Label();
            this.txtStationDistanceToStar = new System.Windows.Forms.TextBox();
            this.label80 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.txtStationName = new System.Windows.Forms.TextBox();
            this.label85 = new System.Windows.Forms.Label();
            this.txtStationId = new System.Windows.Forms.TextBox();
            this.label86 = new System.Windows.Forms.Label();
            this.lblStationRenameHint = new System.Windows.Forms.Label();
            this.cmbStationType = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationState = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationAllegiance = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationGovernment = new System.Windows.Forms.ComboBox_ro();
            this.cmbStationMaxLandingPadSize = new System.Windows.Forms.ComboBox_ro();
            this.gbSystemSystemData = new System.Windows.Forms.GroupBox();
            this.cmdSystemCancel = new System.Windows.Forms.Button();
            this.lblSystemCountTotal = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.cmdSystemEdit = new System.Windows.Forms.Button();
            this.cmbSystemsAllSystems = new System.Windows.Forms.ComboBox_ro();
            this.cbSystemNeedsPermit = new System.Windows.Forms.CheckBox_ro();
            this.cmdSystemNew = new System.Windows.Forms.Button();
            this.cmdSystemSave = new System.Windows.Forms.Button();
            this.txtSystemUpdatedAt = new System.Windows.Forms.TextBox();
            this.label63 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
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
            this.cmbSystemPrimaryEconomy = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemSecurity = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemState = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemAllegiance = new System.Windows.Forms.ComboBox_ro();
            this.cmbSystemGovernment = new System.Windows.Forms.ComboBox_ro();
            this.tabOCRGroup = new System.Windows.Forms.TabPage();
            this.tabCtrlOCR = new System.Windows.Forms.TabControl();
            this.tabExternal = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.txtExtInfo2 = new System.Windows.Forms.TextBox();
            this.txtLocalDataCollected = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdConfirm = new System.Windows.Forms.Button();
            this.cmdGetMarketData = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRecievedStation = new System.Windows.Forms.TextBox();
            this.txtRecievedSystem = new System.Windows.Forms.TextBox();
            this.txtExtInfo = new System.Windows.Forms.TextBox();
            this.cmdLanded = new System.Windows.Forms.Button();
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
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsCommodities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.namesBindingSource)).BeginInit();
            this.msMainMenu.SuspendLayout();
            this.tabCtrlMain.SuspendLayout();
            this.tabHelpAndChangeLog.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColour)).BeginInit();
            this.tabSystemData.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.paEconomies.SuspendLayout();
            this.gbSystemSystemData.SuspendLayout();
            this.tabOCRGroup.SuspendLayout();
            this.tabCtrlOCR.SuspendLayout();
            this.tabExternal.SuspendLayout();
            this.tabWebserver.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "traineddata";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Tesseract-Files|*.traineddata|All Files|*.*";
            this.openFileDialog1.Title = "select Tesseract Traineddata-File";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
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
            // msMainMenu
            // 
            this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new System.Drawing.Size(1154, 24);
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
            this.importToolStripMenuItem,
            this.editLocalizationsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.directDBAccessToolStripMenuItem,
            this.testToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "&Data";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.importToolStripMenuItem.Text = "&Import && Export";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // editLocalizationsToolStripMenuItem
            // 
            this.editLocalizationsToolStripMenuItem.Name = "editLocalizationsToolStripMenuItem";
            this.editLocalizationsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.editLocalizationsToolStripMenuItem.Text = "Edit Localizations";
            this.editLocalizationsToolStripMenuItem.Click += new System.EventHandler(this.editLocalizationsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(162, 6);
            // 
            // directDBAccessToolStripMenuItem
            // 
            this.directDBAccessToolStripMenuItem.Name = "directDBAccessToolStripMenuItem";
            this.directDBAccessToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.directDBAccessToolStripMenuItem.Text = "Direct DB Access";
            this.directDBAccessToolStripMenuItem.Click += new System.EventHandler(this.directDBAccessToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createJumpToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // createJumpToolStripMenuItem
            // 
            this.createJumpToolStripMenuItem.Name = "createJumpToolStripMenuItem";
            this.createJumpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.createJumpToolStripMenuItem.Text = "create Jump";
            this.createJumpToolStripMenuItem.Click += new System.EventHandler(this.createJumpToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem1,
            this.eDDNInterfaceToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.settingsToolStripMenuItem1.Text = "Settings";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.settingsToolStripMenuItem1_Click);
            // 
            // eDDNInterfaceToolStripMenuItem
            // 
            this.eDDNInterfaceToolStripMenuItem.Name = "eDDNInterfaceToolStripMenuItem";
            this.eDDNInterfaceToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.eDDNInterfaceToolStripMenuItem.Text = "EDDN-Interface";
            this.eDDNInterfaceToolStripMenuItem.Click += new System.EventHandler(this.eDDNInterfaceToolStripMenuItem_Click);
            // 
            // cmdLoadCurrentSystem
            // 
            this.cmdLoadCurrentSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLoadCurrentSystem.Location = new System.Drawing.Point(1000, 69);
            this.cmdLoadCurrentSystem.Name = "cmdLoadCurrentSystem";
            this.cmdLoadCurrentSystem.Size = new System.Drawing.Size(148, 21);
            this.cmdLoadCurrentSystem.TabIndex = 60;
            this.cmdLoadCurrentSystem.Text = "Show Current System Data";
            this.cmdLoadCurrentSystem.UseVisualStyleBackColor = true;
            this.cmdLoadCurrentSystem.Visible = false;
            this.cmdLoadCurrentSystem.Click += new System.EventHandler(this.cmdLoadCurrentSystem_Click);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(499, 57);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(49, 13);
            this.label53.TabIndex = 15;
            this.label53.Text = "Universe";
            // 
            // txtEDTime
            // 
            this.txtEDTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEDTime.Location = new System.Drawing.Point(554, 52);
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
            this.label54.Location = new System.Drawing.Point(515, 33);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(33, 13);
            this.label54.TabIndex = 13;
            this.label54.Text = "Local";
            // 
            // txtLocalTime
            // 
            this.txtLocalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalTime.Location = new System.Drawing.Point(554, 28);
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
            this.label45.Location = new System.Drawing.Point(796, 52);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(91, 13);
            this.label45.TabIndex = 10;
            this.label45.Text = "Current Location :";
            // 
            // tbCurrentStationinfoFromLogs
            // 
            this.tbCurrentStationinfoFromLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentStationinfoFromLogs.Location = new System.Drawing.Point(893, 49);
            this.tbCurrentStationinfoFromLogs.Name = "tbCurrentStationinfoFromLogs";
            this.tbCurrentStationinfoFromLogs.ReadOnly = true;
            this.tbCurrentStationinfoFromLogs.Size = new System.Drawing.Size(255, 20);
            this.tbCurrentStationinfoFromLogs.TabIndex = 9;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(806, 31);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(81, 13);
            this.label37.TabIndex = 8;
            this.label37.Text = "Current System:";
            // 
            // tbCurrentSystemFromLogs
            // 
            this.tbCurrentSystemFromLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentSystemFromLogs.Location = new System.Drawing.Point(893, 28);
            this.tbCurrentSystemFromLogs.Name = "tbCurrentSystemFromLogs";
            this.tbCurrentSystemFromLogs.ReadOnly = true;
            this.tbCurrentSystemFromLogs.Size = new System.Drawing.Size(255, 20);
            this.tbCurrentSystemFromLogs.TabIndex = 7;
            this.tbCurrentSystemFromLogs.Text = "scanning...";
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
            this.tabCtrlMain.Location = new System.Drawing.Point(3, 72);
            this.tabCtrlMain.Name = "tabCtrlMain";
            this.tabCtrlMain.SelectedIndex = 0;
            this.tabCtrlMain.Size = new System.Drawing.Size(1149, 664);
            this.tabCtrlMain.TabIndex = 4;
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
            this.tabHelpAndChangeLog.Controls.Add(this.lblDonate);
            this.tabHelpAndChangeLog.Controls.Add(this.panel2);
            this.tabHelpAndChangeLog.Controls.Add(this.BackgroundSet);
            this.tabHelpAndChangeLog.Controls.Add(this.ForegroundSet);
            this.tabHelpAndChangeLog.Controls.Add(this.button20);
            this.tabHelpAndChangeLog.Controls.Add(this.label47);
            this.tabHelpAndChangeLog.Controls.Add(this.label46);
            this.tabHelpAndChangeLog.Controls.Add(this.pbBackgroundColour);
            this.tabHelpAndChangeLog.Controls.Add(this.pbForegroundColour);
            this.tabHelpAndChangeLog.Controls.Add(this.button23);
            this.tabHelpAndChangeLog.Controls.Add(this.button22);
            this.tabHelpAndChangeLog.Controls.Add(this.lblSubtitle);
            this.tabHelpAndChangeLog.Controls.Add(this.lblRegulatedNoise);
            this.tabHelpAndChangeLog.Location = new System.Drawing.Point(4, 22);
            this.tabHelpAndChangeLog.Name = "tabHelpAndChangeLog";
            this.tabHelpAndChangeLog.Size = new System.Drawing.Size(1141, 638);
            this.tabHelpAndChangeLog.TabIndex = 9;
            this.tabHelpAndChangeLog.Text = "Help and Changelog";
            this.tabHelpAndChangeLog.UseVisualStyleBackColor = true;
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(297, 221);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(122, 13);
            this.label93.TabIndex = 47;
            this.label93.Text = "IBE im deutschen Forum";
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(297, 186);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(93, 13);
            this.label92.TabIndex = 46;
            this.label92.Text = "english IBE thread";
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
            this.llVisitUpdate.Size = new System.Drawing.Size(197, 13);
            this.llVisitUpdate.TabIndex = 42;
            this.llVisitUpdate.TabStop = true;
            this.llVisitUpdate.Text = "https://github.com/Duke-Jones/ED-IBE";
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
            this.lblUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateDetail.Location = new System.Drawing.Point(30, 295);
            this.lblUpdateDetail.Multiline = true;
            this.lblUpdateDetail.Name = "lblUpdateDetail";
            this.lblUpdateDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.lblUpdateDetail.Size = new System.Drawing.Size(729, 241);
            this.lblUpdateDetail.TabIndex = 39;
            // 
            // cmdDonate
            // 
            this.cmdDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDonate.BackColor = System.Drawing.Color.PaleGreen;
            this.cmdDonate.BackgroundImage = global::IBE.Properties.Resources.PayPalDonate;
            this.cmdDonate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cmdDonate.FlatAppearance.BorderSize = 0;
            this.cmdDonate.Location = new System.Drawing.Point(866, 330);
            this.cmdDonate.Name = "cmdDonate";
            this.cmdDonate.Size = new System.Drawing.Size(161, 43);
            this.cmdDonate.TabIndex = 37;
            this.cmdDonate.UseVisualStyleBackColor = false;
            this.cmdDonate.Click += new System.EventHandler(this.cmdDonate_Click);
            // 
            // lblDonate
            // 
            this.lblDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDonate.BackColor = System.Drawing.Color.Transparent;
            this.lblDonate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDonate.ForeColor = System.Drawing.Color.Green;
            this.lblDonate.Location = new System.Drawing.Point(767, 235);
            this.lblDonate.Name = "lblDonate";
            this.lblDonate.Size = new System.Drawing.Size(352, 139);
            this.lblDonate.TabIndex = 38;
            this.lblDonate.Text = "If you like this program and you want to support \r\nmy development, I was very hap" +
    "py \r\nabout a small donation. \r\n\r\nThank you, Duke Jones\r\n";
            this.lblDonate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.panel2.Location = new System.Drawing.Point(28, 555);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1079, 77);
            this.panel2.TabIndex = 33;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(235, 6);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(663, 13);
            this.label44.TabIndex = 5;
            this.label44.Text = "ED - Intelligent Boardcomputer Extension is as unofficial as it gets.   Elite: Da" +
    "ngerous is a registered trademark of Frontier Developments plc.";
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
            this.BackgroundSet.Location = new System.Drawing.Point(1101, 69);
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
            this.ForegroundSet.Location = new System.Drawing.Point(1101, 31);
            this.ForegroundSet.Name = "ForegroundSet";
            this.ForegroundSet.Size = new System.Drawing.Size(18, 20);
            this.ForegroundSet.TabIndex = 29;
            this.ForegroundSet.Text = "?";
            this.ForegroundSet.Click += new System.EventHandler(this.ForegroundSet_Click);
            // 
            // button20
            // 
            this.button20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button20.Location = new System.Drawing.Point(978, 101);
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
            this.label47.Location = new System.Drawing.Point(994, 71);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(98, 13);
            this.label47.TabIndex = 27;
            this.label47.Text = "Background Colour";
            // 
            // label46
            // 
            this.label46.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(994, 32);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(94, 13);
            this.label46.TabIndex = 26;
            this.label46.Text = "Foreground Colour";
            // 
            // pbBackgroundColour
            // 
            this.pbBackgroundColour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbBackgroundColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBackgroundColour.Location = new System.Drawing.Point(1094, 63);
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
            this.pbForegroundColour.Location = new System.Drawing.Point(1094, 25);
            this.pbForegroundColour.Name = "pbForegroundColour";
            this.pbForegroundColour.Size = new System.Drawing.Size(32, 32);
            this.pbForegroundColour.TabIndex = 24;
            this.pbForegroundColour.TabStop = false;
            this.pbForegroundColour.Click += new System.EventHandler(this.pbForegroundColour_Click);
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(203, 150);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(167, 23);
            this.button23.TabIndex = 7;
            this.button23.Text = "How can I analyse price data?";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Visible = false;
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
            this.button22.Visible = false;
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
            this.lblRegulatedNoise.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegulatedNoise.Location = new System.Drawing.Point(12, 49);
            this.lblRegulatedNoise.Name = "lblRegulatedNoise";
            this.lblRegulatedNoise.Size = new System.Drawing.Size(746, 50);
            this.lblRegulatedNoise.TabIndex = 2;
            this.lblRegulatedNoise.Text = "ED - Intelligent Boardcomputer Extension";
            // 
            // tabSystemData
            // 
            this.tabSystemData.Controls.Add(this.groupBox14);
            this.tabSystemData.Controls.Add(this.gbSystemSystemData);
            this.tabSystemData.Location = new System.Drawing.Point(4, 22);
            this.tabSystemData.Name = "tabSystemData";
            this.tabSystemData.Size = new System.Drawing.Size(1141, 638);
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
            this.groupBox14.Controls.Add(this.cbStationHasMarket);
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
            this.groupBox14.Size = new System.Drawing.Size(558, 543);
            this.groupBox14.TabIndex = 0;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Station Data";
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
            this.label64.Size = new System.Drawing.Size(59, 13);
            this.label64.TabIndex = 119;
            this.label64.Text = "Economies";
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
            // cbStationHasMarket
            // 
            this.cbStationHasMarket.AutoSize = true;
            this.cbStationHasMarket.Location = new System.Drawing.Point(328, 151);
            this.cbStationHasMarket.Name = "cbStationHasMarket";
            this.cbStationHasMarket.ReadOnly = false;
            this.cbStationHasMarket.Size = new System.Drawing.Size(85, 17);
            this.cbStationHasMarket.TabIndex = 11;
            this.cbStationHasMarket.Text = "Commodities";
            this.cbStationHasMarket.UseVisualStyleBackColor = false;
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
            this.gbSystemSystemData.Size = new System.Drawing.Size(486, 543);
            this.gbSystemSystemData.TabIndex = 0;
            this.gbSystemSystemData.TabStop = false;
            this.gbSystemSystemData.Text = "System Data";
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
            this.txtSystemUpdatedAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // tabOCRGroup
            // 
            this.tabOCRGroup.Controls.Add(this.tabCtrlOCR);
            this.tabOCRGroup.Location = new System.Drawing.Point(4, 22);
            this.tabOCRGroup.Name = "tabOCRGroup";
            this.tabOCRGroup.Size = new System.Drawing.Size(1141, 638);
            this.tabOCRGroup.TabIndex = 11;
            this.tabOCRGroup.Text = "Market Data Interface";
            this.tabOCRGroup.UseVisualStyleBackColor = true;
            // 
            // tabCtrlOCR
            // 
            this.tabCtrlOCR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlOCR.Controls.Add(this.tabExternal);
            this.tabCtrlOCR.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlOCR.Name = "tabCtrlOCR";
            this.tabCtrlOCR.SelectedIndex = 0;
            this.tabCtrlOCR.Size = new System.Drawing.Size(1076, 605);
            this.tabCtrlOCR.TabIndex = 0;
            // 
            // tabExternal
            // 
            this.tabExternal.Controls.Add(this.label5);
            this.tabExternal.Controls.Add(this.txtExtInfo2);
            this.tabExternal.Controls.Add(this.txtLocalDataCollected);
            this.tabExternal.Controls.Add(this.label4);
            this.tabExternal.Controls.Add(this.cmdConfirm);
            this.tabExternal.Controls.Add(this.cmdGetMarketData);
            this.tabExternal.Controls.Add(this.label3);
            this.tabExternal.Controls.Add(this.label2);
            this.tabExternal.Controls.Add(this.label1);
            this.tabExternal.Controls.Add(this.txtRecievedStation);
            this.tabExternal.Controls.Add(this.txtRecievedSystem);
            this.tabExternal.Controls.Add(this.txtExtInfo);
            this.tabExternal.Controls.Add(this.cmdLanded);
            this.tabExternal.Location = new System.Drawing.Point(4, 22);
            this.tabExternal.Name = "tabExternal";
            this.tabExternal.Padding = new System.Windows.Forms.Padding(3);
            this.tabExternal.Size = new System.Drawing.Size(1068, 579);
            this.tabExternal.TabIndex = 5;
            this.tabExternal.Text = "External IO";
            this.tabExternal.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Info 2";
            // 
            // txtExtInfo2
            // 
            this.txtExtInfo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExtInfo2.Location = new System.Drawing.Point(34, 253);
            this.txtExtInfo2.Multiline = true;
            this.txtExtInfo2.Name = "txtExtInfo2";
            this.txtExtInfo2.ReadOnly = true;
            this.txtExtInfo2.Size = new System.Drawing.Size(400, 46);
            this.txtExtInfo2.TabIndex = 11;
            // 
            // txtLocalDataCollected
            // 
            this.txtLocalDataCollected.Location = new System.Drawing.Point(199, 132);
            this.txtLocalDataCollected.Name = "txtLocalDataCollected";
            this.txtLocalDataCollected.ReadOnly = true;
            this.txtLocalDataCollected.Size = new System.Drawing.Size(235, 20);
            this.txtLocalDataCollected.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "local data collected:";
            // 
            // cmdConfirm
            // 
            this.cmdConfirm.Enabled = false;
            this.cmdConfirm.Location = new System.Drawing.Point(34, 74);
            this.cmdConfirm.Name = "cmdConfirm";
            this.cmdConfirm.Size = new System.Drawing.Size(139, 36);
            this.cmdConfirm.TabIndex = 8;
            this.cmdConfirm.Text = "Confirm Location";
            this.cmdConfirm.UseVisualStyleBackColor = true;
            this.cmdConfirm.Click += new System.EventHandler(this.cmdConfirm_Click);
            // 
            // cmdGetMarketData
            // 
            this.cmdGetMarketData.Enabled = false;
            this.cmdGetMarketData.Location = new System.Drawing.Point(34, 116);
            this.cmdGetMarketData.Name = "cmdGetMarketData";
            this.cmdGetMarketData.Size = new System.Drawing.Size(139, 36);
            this.cmdGetMarketData.TabIndex = 7;
            this.cmdGetMarketData.Text = "Get Market Data";
            this.cmdGetMarketData.UseVisualStyleBackColor = true;
            this.cmdGetMarketData.Click += new System.EventHandler(this.cmdGetMarketData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Recieved station";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Recieved system";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Info";
            // 
            // txtRecievedStation
            // 
            this.txtRecievedStation.Location = new System.Drawing.Point(199, 89);
            this.txtRecievedStation.Name = "txtRecievedStation";
            this.txtRecievedStation.ReadOnly = true;
            this.txtRecievedStation.Size = new System.Drawing.Size(235, 20);
            this.txtRecievedStation.TabIndex = 3;
            // 
            // txtRecievedSystem
            // 
            this.txtRecievedSystem.Location = new System.Drawing.Point(199, 48);
            this.txtRecievedSystem.Name = "txtRecievedSystem";
            this.txtRecievedSystem.ReadOnly = true;
            this.txtRecievedSystem.Size = new System.Drawing.Size(235, 20);
            this.txtRecievedSystem.TabIndex = 2;
            // 
            // txtExtInfo
            // 
            this.txtExtInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExtInfo.Location = new System.Drawing.Point(34, 182);
            this.txtExtInfo.Multiline = true;
            this.txtExtInfo.Name = "txtExtInfo";
            this.txtExtInfo.ReadOnly = true;
            this.txtExtInfo.Size = new System.Drawing.Size(400, 46);
            this.txtExtInfo.TabIndex = 1;
            // 
            // cmdLanded
            // 
            this.cmdLanded.Location = new System.Drawing.Point(34, 32);
            this.cmdLanded.Name = "cmdLanded";
            this.cmdLanded.Size = new System.Drawing.Size(139, 36);
            this.cmdLanded.TabIndex = 0;
            this.cmdLanded.Text = "Landed";
            this.cmdLanded.UseVisualStyleBackColor = true;
            this.cmdLanded.Click += new System.EventHandler(this.cmdLanded_Click);
            // 
            // tabWebserver
            // 
            this.tabWebserver.Controls.Add(this.groupBox1);
            this.tabWebserver.Location = new System.Drawing.Point(4, 22);
            this.tabWebserver.Name = "tabWebserver";
            this.tabWebserver.Size = new System.Drawing.Size(1141, 638);
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
            this.groupBox1.Size = new System.Drawing.Size(1055, 578);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1154, 737);
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
            this.Enabled = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.msMainMenu;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "ED - Intelligent Boardcomputer Extension v";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsCommodities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.namesBindingSource)).EndInit();
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            this.tabCtrlMain.ResumeLayout(false);
            this.tabHelpAndChangeLog.ResumeLayout(false);
            this.tabHelpAndChangeLog.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundColour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForegroundColour)).EndInit();
            this.tabSystemData.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.paEconomies.ResumeLayout(false);
            this.paEconomies.PerformLayout();
            this.gbSystemSystemData.ResumeLayout(false);
            this.gbSystemSystemData.PerformLayout();
            this.tabOCRGroup.ResumeLayout(false);
            this.tabCtrlOCR.ResumeLayout(false);
            this.tabExternal.ResumeLayout(false);
            this.tabExternal.PerformLayout();
            this.tabWebserver.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox tbCurrentSystemFromLogs;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private IBE.Enums_and_Utility_Classes.dsCommodities dsCommodities;
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
        private System.Windows.Forms.CheckBox_ro cbStationHasMarket;
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
        private System.Windows.Forms.Label lblDonate;
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
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblRegulatedNoise;
        private System.Windows.Forms.TabControl tabCtrlMain;
        private System.Windows.Forms.TabPage tabExternal;
        private System.Windows.Forms.Button cmdLanded;
        private System.Windows.Forms.TextBox txtExtInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRecievedStation;
        private System.Windows.Forms.TextBox txtRecievedSystem;
        private System.Windows.Forms.Button cmdConfirm;
        private System.Windows.Forms.Button cmdGetMarketData;
        private System.Windows.Forms.TextBox txtLocalDataCollected;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExtInfo2;
        private System.Windows.Forms.ToolStripMenuItem editLocalizationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createJumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem directDBAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem eDDNInterfaceToolStripMenuItem;
    }
}

