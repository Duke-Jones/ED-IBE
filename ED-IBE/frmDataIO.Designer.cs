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
            this.cmdImportOldData = new System.Windows.Forms.Button();
            this.cbImportPriceData = new System.Windows.Forms.CheckBox();
            this.cmdImportSystemsAndStations = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cmdImportCommandersLog = new System.Windows.Forms.Button();
            this.cmdExportCSV = new System.Windows.Forms.Button();
            this.cmdImportFromCSV = new System.Windows.Forms.Button();
            this.rbFormatExtended = new System.Windows.Forms.RadioButton();
            this.rbFormatSimple = new System.Windows.Forms.RadioButton();
            this.fbFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lbProgess = new System.Windows.Forms.ListBox();
            this.gbFirstTime = new System.Windows.Forms.GroupBox();
            this.gbRepeat = new System.Windows.Forms.GroupBox();
            this.rbImportSame = new System.Windows.Forms.RadioButton();
            this.rbImportNewer = new System.Windows.Forms.RadioButton();
            this.ofdFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupboxExport = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbUserLanguage = new System.Windows.Forms.RadioButton();
            this.rbDefaultLanguage = new System.Windows.Forms.RadioButton();
            this.groupboxImport = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdTest = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmdExit = new System.Windows.Forms.Button();
            this.gbFirstTime.SuspendLayout();
            this.gbRepeat.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupboxExport.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupboxImport.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // ttToolTip
            // 
            this.ttToolTip.AutoPopDelay = 20000;
            this.ttToolTip.InitialDelay = 500;
            this.ttToolTip.ReshowDelay = 100;
            // 
            // cmdImportOldData
            // 
            this.cmdImportOldData.Location = new System.Drawing.Point(33, 23);
            this.cmdImportOldData.Name = "cmdImportOldData";
            this.cmdImportOldData.Size = new System.Drawing.Size(276, 34);
            this.cmdImportOldData.TabIndex = 0;
            this.cmdImportOldData.Text = "Import Old Datafiles";
            this.ttToolTip.SetToolTip(this.cmdImportOldData, "Imports the whole data structure from the existing old RegulatedNoise version.\r\nY" +
        "ou can start this function only one time. ");
            this.cmdImportOldData.UseVisualStyleBackColor = true;
            this.cmdImportOldData.Click += new System.EventHandler(this.cmdImportOldData_Click);
            // 
            // cbImportPriceData
            // 
            this.cbImportPriceData.AutoSize = true;
            this.cbImportPriceData.Location = new System.Drawing.Point(13, 63);
            this.cbImportPriceData.Name = "cbImportPriceData";
            this.cbImportPriceData.Size = new System.Drawing.Size(318, 17);
            this.cbImportPriceData.TabIndex = 2;
            this.cbImportPriceData.Text = "also import price-data from EDDB-files (slow, maybe > 10 mins)";
            this.ttToolTip.SetToolTip(this.cbImportPriceData, resources.GetString("cbImportPriceData.ToolTip"));
            this.cbImportPriceData.UseVisualStyleBackColor = true;
            this.cbImportPriceData.Visible = false;
            // 
            // cmdImportSystemsAndStations
            // 
            this.cmdImportSystemsAndStations.Location = new System.Drawing.Point(36, 19);
            this.cmdImportSystemsAndStations.Name = "cmdImportSystemsAndStations";
            this.cmdImportSystemsAndStations.Size = new System.Drawing.Size(276, 34);
            this.cmdImportSystemsAndStations.TabIndex = 0;
            this.cmdImportSystemsAndStations.Text = "Import data of systems/stations/commodities from EDDB-files";
            this.ttToolTip.SetToolTip(this.cmdImportSystemsAndStations, "Imports the data of systems, stations and commodities from \r\nEDDN files (\"system." +
        "json\", \"stations.json\", \"commodities.json\").\r\nSuggestion is to import all three " +
        "files to avoid missing dependences.\r\n");
            this.cmdImportSystemsAndStations.UseVisualStyleBackColor = true;
            this.cmdImportSystemsAndStations.Click += new System.EventHandler(this.cmdImportSystemsAndStations_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 55);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(318, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "also import price-data from EDDB-files (slow, maybe > 10 mins)";
            this.ttToolTip.SetToolTip(this.checkBox1, resources.GetString("checkBox1.ToolTip"));
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // cmdImportCommandersLog
            // 
            this.cmdImportCommandersLog.Location = new System.Drawing.Point(33, 106);
            this.cmdImportCommandersLog.Name = "cmdImportCommandersLog";
            this.cmdImportCommandersLog.Size = new System.Drawing.Size(276, 34);
            this.cmdImportCommandersLog.TabIndex = 3;
            this.cmdImportCommandersLog.Text = "Import RN-CommandersLog-files";
            this.ttToolTip.SetToolTip(this.cmdImportCommandersLog, "The old RN had perfomance problems with too big Commander\'s Logs\r\nIf you have spl" +
        "itted the old log in multiple files you can import the with this function.\r\n");
            this.cmdImportCommandersLog.UseVisualStyleBackColor = true;
            this.cmdImportCommandersLog.Click += new System.EventHandler(this.cmdImportCommandersLog_Click);
            // 
            // cmdExportCSV
            // 
            this.cmdExportCSV.Location = new System.Drawing.Point(20, 17);
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
            // gbFirstTime
            // 
            this.gbFirstTime.Controls.Add(this.cmdImportCommandersLog);
            this.gbFirstTime.Controls.Add(this.cmdImportOldData);
            this.gbFirstTime.Controls.Add(this.cbImportPriceData);
            this.gbFirstTime.Location = new System.Drawing.Point(12, 12);
            this.gbFirstTime.Name = "gbFirstTime";
            this.gbFirstTime.Size = new System.Drawing.Size(345, 164);
            this.gbFirstTime.TabIndex = 3;
            this.gbFirstTime.TabStop = false;
            this.gbFirstTime.Text = "Import data from \"RegulatedNoise\"";
            // 
            // gbRepeat
            // 
            this.gbRepeat.Controls.Add(this.cmdImportSystemsAndStations);
            this.gbRepeat.Controls.Add(this.checkBox1);
            this.gbRepeat.Location = new System.Drawing.Point(363, 12);
            this.gbRepeat.Name = "gbRepeat";
            this.gbRepeat.Size = new System.Drawing.Size(345, 96);
            this.gbRepeat.TabIndex = 4;
            this.gbRepeat.TabStop = false;
            this.gbRepeat.Text = "EDDN imports";
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
            // ofdFileDialog
            // 
            this.ofdFileDialog.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupboxExport);
            this.groupBox1.Controls.Add(this.groupboxImport);
            this.groupBox1.Location = new System.Drawing.Point(714, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 308);
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
            this.groupboxImport.Location = new System.Drawing.Point(10, 201);
            this.groupboxImport.Name = "groupboxImport";
            this.groupboxImport.Size = new System.Drawing.Size(313, 96);
            this.groupboxImport.TabIndex = 7;
            this.groupboxImport.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cmdTest);
            this.groupBox2.Location = new System.Drawing.Point(894, 384);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 72);
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
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.cmdExit);
            this.groupBox5.Location = new System.Drawing.Point(1053, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(173, 238);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(11, 36);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(156, 34);
            this.cmdExit.TabIndex = 10;
            this.cmdExit.Text = "&Close";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // frmDataIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1238, 468);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDataIO_FormClosed);
            this.gbFirstTime.ResumeLayout(false);
            this.gbFirstTime.PerformLayout();
            this.gbRepeat.ResumeLayout(false);
            this.gbRepeat.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupboxExport.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupboxImport.ResumeLayout(false);
            this.groupboxImport.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip ttToolTip;
        private System.Windows.Forms.Button cmdImportOldData;
        private System.Windows.Forms.FolderBrowserDialog fbFolderDialog;
        private System.Windows.Forms.ListBox lbProgess;
        private System.Windows.Forms.CheckBox cbImportPriceData;
        private System.Windows.Forms.GroupBox gbFirstTime;
        private System.Windows.Forms.GroupBox gbRepeat;
        private System.Windows.Forms.Button cmdImportSystemsAndStations;
        private System.Windows.Forms.CheckBox checkBox1;
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
    }
}
