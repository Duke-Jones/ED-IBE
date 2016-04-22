namespace IBE
{
    partial class frmDataIO
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataIO));
            this.ttToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.rbFormatSimple = new System.Windows.Forms.RadioButton();
            this.rbFormatExtended = new System.Windows.Forms.RadioButton();
            this.cmdExportCSV = new System.Windows.Forms.Button();
            this.cmdImportFromCSV = new System.Windows.Forms.Button();
            this.gbRepeat = new System.Windows.Forms.GroupBox();
            this.gbPriceImport = new System.Windows.Forms.GroupBox();
            this.rbImportPrices_All = new System.Windows.Forms.RadioButton();
            this.rbImportPrices_Bubble = new System.Windows.Forms.RadioButton();
            this.rbImportPrices_No = new System.Windows.Forms.RadioButton();
            this.cmdImportSystemsAndStationsFromDownload = new System.Windows.Forms.Button();
            this.cmdDownloadSystemsAndStations = new System.Windows.Forms.Button();
            this.cmdImportSystemsAndStations = new System.Windows.Forms.Button();
            this.cmdImportCommandersLog = new System.Windows.Forms.Button();
            this.cmdImportOldData = new System.Windows.Forms.Button();
            this.fbFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cmdPurgeOldData = new System.Windows.Forms.Button();
            this.label89 = new System.Windows.Forms.Label();
            this.nudPurgeOldDataDays = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmdExit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupboxExport = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbUserLanguage = new System.Windows.Forms.RadioButton();
            this.rbDefaultLanguage = new System.Windows.Forms.RadioButton();
            this.groupboxImport = new System.Windows.Forms.GroupBox();
            this.rbImportSame = new System.Windows.Forms.RadioButton();
            this.rbImportNewer = new System.Windows.Forms.RadioButton();
            this.gbFirstTime = new System.Windows.Forms.GroupBox();
            this.lbProgess = new System.Windows.Forms.ListBox();
            this.gbRepeat.SuspendLayout();
            this.gbPriceImport.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPurgeOldDataDays)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupboxExport.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupboxImport.SuspendLayout();
            this.gbFirstTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // ttToolTip
            // 
            this.ttToolTip.AutoPopDelay = 20000;
            this.ttToolTip.InitialDelay = 500;
            this.ttToolTip.ReshowDelay = 100;
            // 
            // rbFormatSimple
            // 
            this.rbFormatSimple.AutoSize = true;
            this.rbFormatSimple.Location = new System.Drawing.Point(155, 17);
            this.rbFormatSimple.Name = "rbFormatSimple";
            this.rbFormatSimple.Size = new System.Drawing.Size(124, 17);
            this.rbFormatSimple.TabIndex = 1;
            this.rbFormatSimple.Text = "Simple Export Format";
            this.ttToolTip.SetToolTip(this.rbFormatSimple, "Simple export format. Useful if you want to import the data to other tools\r\n(they" +
        " don\'t know the extended data from ED-IBE)\r\n\r\n");
            this.rbFormatSimple.UseVisualStyleBackColor = true;
            // 
            // rbFormatExtended
            // 
            this.rbFormatExtended.AutoSize = true;
            this.rbFormatExtended.Checked = true;
            this.rbFormatExtended.Location = new System.Drawing.Point(9, 17);
            this.rbFormatExtended.Name = "rbFormatExtended";
            this.rbFormatExtended.Size = new System.Drawing.Size(109, 17);
            this.rbFormatExtended.TabIndex = 0;
            this.rbFormatExtended.TabStop = true;
            this.rbFormatExtended.Text = "Full Export Format";
            this.ttToolTip.SetToolTip(this.rbFormatExtended, "Export with all informations from ED-IBE, e.g. with datasource.\r\nUseful if you wa" +
        "nt to re-import the data into ED-IBE. Otherwise \r\nyou will lose information like" +
        " e.g. datasource.");
            this.rbFormatExtended.UseVisualStyleBackColor = true;
            // 
            // cmdExportCSV
            // 
            this.cmdExportCSV.Location = new System.Drawing.Point(15, 16);
            this.cmdExportCSV.Name = "cmdExportCSV";
            this.cmdExportCSV.Size = new System.Drawing.Size(276, 34);
            this.cmdExportCSV.TabIndex = 0;
            this.cmdExportCSV.Text = "Export Marketdata to CSV";
            this.ttToolTip.SetToolTip(this.cmdExportCSV, "Exports all marketdata into a csv-file");
            this.cmdExportCSV.UseVisualStyleBackColor = true;
            this.cmdExportCSV.Click += new System.EventHandler(this.cmdExportCSV_Click);
            // 
            // cmdImportFromCSV
            // 
            this.cmdImportFromCSV.Location = new System.Drawing.Point(15, 12);
            this.cmdImportFromCSV.Name = "cmdImportFromCSV";
            this.cmdImportFromCSV.Size = new System.Drawing.Size(276, 34);
            this.cmdImportFromCSV.TabIndex = 3;
            this.cmdImportFromCSV.Text = "Import Marketdata from CSV";
            this.ttToolTip.SetToolTip(this.cmdImportFromCSV, "Exports all marketdata into a csv-file");
            this.cmdImportFromCSV.UseVisualStyleBackColor = true;
            this.cmdImportFromCSV.Click += new System.EventHandler(this.cmdImportFromCSV_Click);
            // 
            // gbRepeat
            // 
            this.gbRepeat.Controls.Add(this.gbPriceImport);
            this.gbRepeat.Controls.Add(this.cmdImportSystemsAndStationsFromDownload);
            this.gbRepeat.Controls.Add(this.cmdDownloadSystemsAndStations);
            this.gbRepeat.Controls.Add(this.cmdImportSystemsAndStations);
            this.gbRepeat.Location = new System.Drawing.Point(363, 12);
            this.gbRepeat.Name = "gbRepeat";
            this.gbRepeat.Size = new System.Drawing.Size(345, 242);
            this.gbRepeat.TabIndex = 4;
            this.gbRepeat.TabStop = false;
            this.gbRepeat.Text = "EDDB imports";
            this.ttToolTip.SetToolTip(this.gbRepeat, "Upating the master data.\r\nGet dumpfiles from EDDB (https://eddb.io/api)\r\nand upda" +
        "te master data for systems, stations and other.");
            // 
            // gbPriceImport
            // 
            this.gbPriceImport.Controls.Add(this.rbImportPrices_All);
            this.gbPriceImport.Controls.Add(this.rbImportPrices_Bubble);
            this.gbPriceImport.Controls.Add(this.rbImportPrices_No);
            this.gbPriceImport.Location = new System.Drawing.Point(45, 120);
            this.gbPriceImport.Name = "gbPriceImport";
            this.gbPriceImport.Size = new System.Drawing.Size(253, 112);
            this.gbPriceImport.TabIndex = 5;
            this.gbPriceImport.TabStop = false;
            this.gbPriceImport.Tag = "PriceImport;ImportPricesNo";
            this.gbPriceImport.Text = "Import prices from EDDB ";
            this.ttToolTip.SetToolTip(this.gbPriceImport, "Do you want to import prices from EDDB");
            // 
            // rbImportPrices_All
            // 
            this.rbImportPrices_All.AutoSize = true;
            this.rbImportPrices_All.Location = new System.Drawing.Point(25, 76);
            this.rbImportPrices_All.Name = "rbImportPrices_All";
            this.rbImportPrices_All.Size = new System.Drawing.Size(190, 30);
            this.rbImportPrices_All.TabIndex = 2;
            this.rbImportPrices_All.Tag = "ImportPricesAll";
            this.rbImportPrices_All.Text = "Yeah, gimme all prices I can get \r\n(takes long time, up to a few hours)";
            this.rbImportPrices_All.UseVisualStyleBackColor = true;
            // 
            // rbImportPrices_Bubble
            // 
            this.rbImportPrices_Bubble.AutoSize = true;
            this.rbImportPrices_Bubble.Location = new System.Drawing.Point(25, 40);
            this.rbImportPrices_Bubble.Name = "rbImportPrices_Bubble";
            this.rbImportPrices_Bubble.Size = new System.Drawing.Size(214, 30);
            this.rbImportPrices_Bubble.TabIndex = 1;
            this.rbImportPrices_Bubble.Tag = "ImportPricesBubble";
            this.rbImportPrices_Bubble.Text = "Ok, give me a starter kit out of a bubble \r\naround of 20 ly of my current positio" +
    "n";
            this.rbImportPrices_Bubble.UseVisualStyleBackColor = true;
            // 
            // rbImportPrices_No
            // 
            this.rbImportPrices_No.AutoSize = true;
            this.rbImportPrices_No.Checked = true;
            this.rbImportPrices_No.Location = new System.Drawing.Point(25, 19);
            this.rbImportPrices_No.Name = "rbImportPrices_No";
            this.rbImportPrices_No.Size = new System.Drawing.Size(176, 17);
            this.rbImportPrices_No.TabIndex = 0;
            this.rbImportPrices_No.TabStop = true;
            this.rbImportPrices_No.Tag = "ImportPricesNo";
            this.rbImportPrices_No.Text = "No, I want to collect data myself";
            this.rbImportPrices_No.UseVisualStyleBackColor = true;
            this.rbImportPrices_No.CheckedChanged += new System.EventHandler(this.Radiobutton_CheckedChanged);
            // 
            // cmdImportSystemsAndStationsFromDownload
            // 
            this.cmdImportSystemsAndStationsFromDownload.Location = new System.Drawing.Point(14, 72);
            this.cmdImportSystemsAndStationsFromDownload.Name = "cmdImportSystemsAndStationsFromDownload";
            this.cmdImportSystemsAndStationsFromDownload.Size = new System.Drawing.Size(153, 41);
            this.cmdImportSystemsAndStationsFromDownload.TabIndex = 4;
            this.cmdImportSystemsAndStationsFromDownload.Text = "Import from downloaded EDDB dumpfiles";
            this.ttToolTip.SetToolTip(this.cmdImportSystemsAndStationsFromDownload, "Imports the dumpfiles downloaded from https://eddb.io/api \r\n(\"system.json\", \"stat" +
        "ions.json\", \"commodities.json\").\r\n\r\nNo update of price data !\r\n");
            this.cmdImportSystemsAndStationsFromDownload.UseVisualStyleBackColor = true;
            this.cmdImportSystemsAndStationsFromDownload.Click += new System.EventHandler(this.cmdImportSystemsAndStationsFromDownload_Click);
            // 
            // cmdDownloadSystemsAndStations
            // 
            this.cmdDownloadSystemsAndStations.Location = new System.Drawing.Point(14, 23);
            this.cmdDownloadSystemsAndStations.Name = "cmdDownloadSystemsAndStations";
            this.cmdDownloadSystemsAndStations.Size = new System.Drawing.Size(153, 41);
            this.cmdDownloadSystemsAndStations.TabIndex = 3;
            this.cmdDownloadSystemsAndStations.Text = "Download latest EDDB dumpfiles";
            this.ttToolTip.SetToolTip(this.cmdDownloadSystemsAndStations, "Downloads the latest dumpfiles of systems, stations and commodities from \r\nhttps:" +
        "//eddb.io/api (\"system.json\", \"stations.json\", \"commodities.json\").\r\n\r\n");
            this.cmdDownloadSystemsAndStations.UseVisualStyleBackColor = true;
            this.cmdDownloadSystemsAndStations.Click += new System.EventHandler(this.cmdDownloadSystemsAndStations_Click);
            // 
            // cmdImportSystemsAndStations
            // 
            this.cmdImportSystemsAndStations.Location = new System.Drawing.Point(179, 73);
            this.cmdImportSystemsAndStations.Name = "cmdImportSystemsAndStations";
            this.cmdImportSystemsAndStations.Size = new System.Drawing.Size(153, 41);
            this.cmdImportSystemsAndStations.TabIndex = 0;
            this.cmdImportSystemsAndStations.Text = "Import from manual selected EDDB dumpfiles";
            this.ttToolTip.SetToolTip(this.cmdImportSystemsAndStations, resources.GetString("cmdImportSystemsAndStations.ToolTip"));
            this.cmdImportSystemsAndStations.UseVisualStyleBackColor = true;
            this.cmdImportSystemsAndStations.Click += new System.EventHandler(this.cmdImportSystemsAndStations_Click);
            // 
            // cmdImportCommandersLog
            // 
            this.cmdImportCommandersLog.Location = new System.Drawing.Point(33, 72);
            this.cmdImportCommandersLog.Name = "cmdImportCommandersLog";
            this.cmdImportCommandersLog.Size = new System.Drawing.Size(276, 41);
            this.cmdImportCommandersLog.TabIndex = 3;
            this.cmdImportCommandersLog.Text = "Import RN-CommandersLog-files";
            this.ttToolTip.SetToolTip(this.cmdImportCommandersLog, "The old RN had perfomance problems with too big Commander\'s Logs\r\nIf you have spl" +
        "itted the old log in multiple files you can import the with this function.\r\n");
            this.cmdImportCommandersLog.UseVisualStyleBackColor = true;
            this.cmdImportCommandersLog.Click += new System.EventHandler(this.cmdImportCommandersLog_Click);
            // 
            // cmdImportOldData
            // 
            this.cmdImportOldData.Location = new System.Drawing.Point(33, 23);
            this.cmdImportOldData.Name = "cmdImportOldData";
            this.cmdImportOldData.Size = new System.Drawing.Size(276, 41);
            this.cmdImportOldData.TabIndex = 0;
            this.cmdImportOldData.Text = "Import Old Datafiles";
            this.ttToolTip.SetToolTip(this.cmdImportOldData, "Imports the whole data structure from the existing old RegulatedNoise version.\r\nY" +
        "ou can start this function only one time. ");
            this.cmdImportOldData.UseVisualStyleBackColor = true;
            this.cmdImportOldData.Click += new System.EventHandler(this.cmdImportOldData_Click);
            // 
            // ofdFileDialog
            // 
            this.ofdFileDialog.FileName = "openFileDialog1";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cmdPurgeOldData);
            this.groupBox6.Controls.Add(this.label89);
            this.groupBox6.Controls.Add(this.nudPurgeOldDataDays);
            this.groupBox6.Location = new System.Drawing.Point(714, 387);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(323, 59);
            this.groupBox6.TabIndex = 66;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Delete marketdata to reduce database";
            // 
            // cmdPurgeOldData
            // 
            this.cmdPurgeOldData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdPurgeOldData.Location = new System.Drawing.Point(32, 21);
            this.cmdPurgeOldData.Name = "cmdPurgeOldData";
            this.cmdPurgeOldData.Size = new System.Drawing.Size(167, 23);
            this.cmdPurgeOldData.TabIndex = 63;
            this.cmdPurgeOldData.Text = "Delete Marketdata Older Than";
            this.cmdPurgeOldData.UseVisualStyleBackColor = true;
            this.cmdPurgeOldData.Click += new System.EventHandler(this.cmdPurgeOldData_Click);
            // 
            // label89
            // 
            this.label89.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(254, 26);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(31, 13);
            this.label89.TabIndex = 64;
            this.label89.Text = "Days";
            // 
            // nudPurgeOldDataDays
            // 
            this.nudPurgeOldDataDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudPurgeOldDataDays.Location = new System.Drawing.Point(205, 23);
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
            this.nudPurgeOldDataDays.TabIndex = 65;
            this.nudPurgeOldDataDays.Tag = "PurgeOldDataDays;30";
            this.nudPurgeOldDataDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPurgeOldDataDays.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudPurgeOldDataDays.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudPurgeOldDataDays_KeyDown);
            this.nudPurgeOldDataDays.Leave += new System.EventHandler(this.nudPurgeOldDataDays_Leave);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.cmdExit);
            this.groupBox5.Location = new System.Drawing.Point(1053, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(173, 81);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(11, 35);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(156, 34);
            this.cmdExit.TabIndex = 10;
            this.cmdExit.Text = "&Close";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cmdTest);
            this.groupBox2.Location = new System.Drawing.Point(12, 181);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 73);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Other";
            this.groupBox2.Visible = false;
            // 
            // cmdTest
            // 
            this.cmdTest.Location = new System.Drawing.Point(33, 19);
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.Size = new System.Drawing.Size(82, 34);
            this.cmdTest.TabIndex = 0;
            this.cmdTest.Text = "Test";
            this.cmdTest.UseVisualStyleBackColor = true;
            this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupboxExport);
            this.groupBox1.Controls.Add(this.groupboxImport);
            this.groupBox1.Location = new System.Drawing.Point(714, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 332);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Market Data (CSV)";
            // 
            // groupboxExport
            // 
            this.groupboxExport.Controls.Add(this.groupBox4);
            this.groupboxExport.Controls.Add(this.groupBox3);
            this.groupboxExport.Controls.Add(this.cmdExportCSV);
            this.groupboxExport.Location = new System.Drawing.Point(10, 19);
            this.groupboxExport.Name = "groupboxExport";
            this.groupboxExport.Size = new System.Drawing.Size(313, 176);
            this.groupboxExport.TabIndex = 8;
            this.groupboxExport.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbFormatSimple);
            this.groupBox4.Controls.Add(this.rbFormatExtended);
            this.groupBox4.Location = new System.Drawing.Point(7, 122);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(300, 42);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Format";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbUserLanguage);
            this.groupBox3.Controls.Add(this.rbDefaultLanguage);
            this.groupBox3.Location = new System.Drawing.Point(7, 56);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(300, 60);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Language";
            // 
            // rbUserLanguage
            // 
            this.rbUserLanguage.AutoSize = true;
            this.rbUserLanguage.Location = new System.Drawing.Point(9, 35);
            this.rbUserLanguage.Name = "rbUserLanguage";
            this.rbUserLanguage.Size = new System.Drawing.Size(282, 17);
            this.rbUserLanguage.TabIndex = 2;
            this.rbUserLanguage.Text = "export names in user preferred language (from settings)";
            this.rbUserLanguage.UseVisualStyleBackColor = true;
            // 
            // rbDefaultLanguage
            // 
            this.rbDefaultLanguage.AutoSize = true;
            this.rbDefaultLanguage.Checked = true;
            this.rbDefaultLanguage.Location = new System.Drawing.Point(9, 18);
            this.rbDefaultLanguage.Name = "rbDefaultLanguage";
            this.rbDefaultLanguage.Size = new System.Drawing.Size(214, 17);
            this.rbDefaultLanguage.TabIndex = 1;
            this.rbDefaultLanguage.TabStop = true;
            this.rbDefaultLanguage.Text = "export names in base language (english)";
            this.rbDefaultLanguage.UseVisualStyleBackColor = true;
            // 
            // groupboxImport
            // 
            this.groupboxImport.Controls.Add(this.rbImportSame);
            this.groupboxImport.Controls.Add(this.cmdImportFromCSV);
            this.groupboxImport.Controls.Add(this.rbImportNewer);
            this.groupboxImport.Location = new System.Drawing.Point(10, 223);
            this.groupboxImport.Name = "groupboxImport";
            this.groupboxImport.Size = new System.Drawing.Size(313, 96);
            this.groupboxImport.TabIndex = 7;
            this.groupboxImport.TabStop = false;
            // 
            // rbImportSame
            // 
            this.rbImportSame.AutoSize = true;
            this.rbImportSame.Location = new System.Drawing.Point(15, 65);
            this.rbImportSame.Name = "rbImportSame";
            this.rbImportSame.Size = new System.Drawing.Size(240, 17);
            this.rbImportSame.TabIndex = 5;
            this.rbImportSame.Text = "only import data if timestamp is equal or newer";
            this.rbImportSame.UseVisualStyleBackColor = true;
            // 
            // rbImportNewer
            // 
            this.rbImportNewer.AutoSize = true;
            this.rbImportNewer.Checked = true;
            this.rbImportNewer.Location = new System.Drawing.Point(15, 48);
            this.rbImportNewer.Name = "rbImportNewer";
            this.rbImportNewer.Size = new System.Drawing.Size(199, 17);
            this.rbImportNewer.TabIndex = 4;
            this.rbImportNewer.TabStop = true;
            this.rbImportNewer.Text = "only import data if timestamp is newer";
            this.rbImportNewer.UseVisualStyleBackColor = true;
            // 
            // gbFirstTime
            // 
            this.gbFirstTime.Controls.Add(this.cmdImportCommandersLog);
            this.gbFirstTime.Controls.Add(this.cmdImportOldData);
            this.gbFirstTime.Location = new System.Drawing.Point(12, 12);
            this.gbFirstTime.Name = "gbFirstTime";
            this.gbFirstTime.Size = new System.Drawing.Size(345, 127);
            this.gbFirstTime.TabIndex = 3;
            this.gbFirstTime.TabStop = false;
            this.gbFirstTime.Text = "Import data from \"RegulatedNoise\"";
            // 
            // lbProgess
            // 
            this.lbProgess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgess.FormattingEnabled = true;
            this.lbProgess.Location = new System.Drawing.Point(12, 260);
            this.lbProgess.Name = "lbProgess";
            this.lbProgess.Size = new System.Drawing.Size(696, 186);
            this.lbProgess.TabIndex = 1;
            // 
            // frmDataIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1238, 468);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbRepeat);
            this.Controls.Add(this.gbFirstTime);
            this.Controls.Add(this.lbProgess);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1254, 506);
            this.MinimumSize = new System.Drawing.Size(1254, 506);
            this.Name = "frmDataIO";
            this.Text = "Data Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDataIO_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDataIO_FormClosed);
            this.Shown += new System.EventHandler(this.frmDataIO_Shown);
            this.gbRepeat.ResumeLayout(false);
            this.gbPriceImport.ResumeLayout(false);
            this.gbPriceImport.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPurgeOldDataDays)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupboxExport.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupboxImport.ResumeLayout(false);
            this.groupboxImport.PerformLayout();
            this.gbFirstTime.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip ttToolTip;
        private System.Windows.Forms.Button cmdImportOldData;
        private System.Windows.Forms.FolderBrowserDialog fbFolderDialog;
        private System.Windows.Forms.ListBox lbProgess;
        private System.Windows.Forms.GroupBox gbFirstTime;
        private System.Windows.Forms.GroupBox gbRepeat;
        private System.Windows.Forms.Button cmdImportSystemsAndStations;
        private System.Windows.Forms.OpenFileDialog ofdFileDialog;
        private System.Windows.Forms.Button cmdImportCommandersLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdExportCSV;
        private System.Windows.Forms.RadioButton rbUserLanguage;
        private System.Windows.Forms.RadioButton rbDefaultLanguage;
        private System.Windows.Forms.Button cmdImportFromCSV;
        private System.Windows.Forms.RadioButton rbImportSame;
        private System.Windows.Forms.RadioButton rbImportNewer;
        private System.Windows.Forms.GroupBox groupboxExport;
        private System.Windows.Forms.GroupBox groupboxImport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdTest;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbFormatSimple;
        private System.Windows.Forms.RadioButton rbFormatExtended;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.NumericUpDown nudPurgeOldDataDays;
        private System.Windows.Forms.Button cmdPurgeOldData;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button cmdDownloadSystemsAndStations;
        private System.Windows.Forms.Button cmdImportSystemsAndStationsFromDownload;
        private System.Windows.Forms.GroupBox gbPriceImport;
        private System.Windows.Forms.RadioButton rbImportPrices_All;
        private System.Windows.Forms.RadioButton rbImportPrices_Bubble;
        private System.Windows.Forms.RadioButton rbImportPrices_No;
    }
}
