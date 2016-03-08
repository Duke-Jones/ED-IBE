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
            this.cmdClearAll = new System.Windows.Forms.Button();
            this.cmdImportCommandersLog = new System.Windows.Forms.Button();
            this.cmdExportCSV = new System.Windows.Forms.Button();
            this.cmdImportFromCSV = new System.Windows.Forms.Button();
            this.fbFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lbProgess = new System.Windows.Forms.ListBox();
            this.gbFirstTime = new System.Windows.Forms.GroupBox();
            this.gbRepeat = new System.Windows.Forms.GroupBox();
            this.rbImportSame = new System.Windows.Forms.RadioButton();
            this.rbImportNewer = new System.Windows.Forms.RadioButton();
            this.cbClear = new System.Windows.Forms.GroupBox();
            this.ofdFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbUserLanguage = new System.Windows.Forms.RadioButton();
            this.rbDefaultLanguage = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdTest = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.gbFirstTime.SuspendLayout();
            this.gbRepeat.SuspendLayout();
            this.cbClear.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
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
            // cmdClearAll
            // 
            this.cmdClearAll.BackColor = System.Drawing.Color.Salmon;
            this.cmdClearAll.Location = new System.Drawing.Point(33, 19);
            this.cmdClearAll.Name = "cmdClearAll";
            this.cmdClearAll.Size = new System.Drawing.Size(276, 34);
            this.cmdClearAll.TabIndex = 0;
            this.cmdClearAll.Text = "Clear all !";
            this.ttToolTip.SetToolTip(this.cmdClearAll, "Clears the whole database to allow the \"First time import\" again.\r\nBe carful - al" +
        "l data from the database will be lost.\r\n");
            this.cmdClearAll.UseVisualStyleBackColor = false;
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
            // lbProgess
            // 
            this.lbProgess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgess.FormattingEnabled = true;
            this.lbProgess.Location = new System.Drawing.Point(12, 260);
            this.lbProgess.Name = "lbProgess";
            this.lbProgess.Size = new System.Drawing.Size(687, 186);
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
            // cbClear
            // 
            this.cbClear.Controls.Add(this.cmdClearAll);
            this.cbClear.Location = new System.Drawing.Point(12, 182);
            this.cbClear.Name = "cbClear";
            this.cbClear.Size = new System.Drawing.Size(345, 72);
            this.cbClear.TabIndex = 5;
            this.cbClear.TabStop = false;
            this.cbClear.Text = "Reset Database";
            // 
            // ofdFileDialog
            // 
            this.ofdFileDialog.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(714, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 238);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Market Data (CSV)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbUserLanguage);
            this.panel2.Controls.Add(this.cmdExportCSV);
            this.panel2.Controls.Add(this.rbDefaultLanguage);
            this.panel2.Location = new System.Drawing.Point(10, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(313, 102);
            this.panel2.TabIndex = 8;
            // 
            // rbUserLanguage
            // 
            this.rbUserLanguage.AutoSize = true;
            this.rbUserLanguage.Location = new System.Drawing.Point(20, 69);
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
            this.rbDefaultLanguage.Location = new System.Drawing.Point(20, 52);
            this.rbDefaultLanguage.Name = "rbDefaultLanguage";
            this.rbDefaultLanguage.Size = new System.Drawing.Size(214, 17);
            this.rbDefaultLanguage.TabIndex = 1;
            this.rbDefaultLanguage.TabStop = true;
            this.rbDefaultLanguage.Text = "export names in base language (english)";
            this.rbDefaultLanguage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbImportSame);
            this.panel1.Controls.Add(this.cmdImportFromCSV);
            this.panel1.Controls.Add(this.rbImportNewer);
            this.panel1.Location = new System.Drawing.Point(10, 127);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(313, 96);
            this.panel1.TabIndex = 7;
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
            this.groupBox5.Controls.Add(this.cmdSave);
            this.groupBox5.Location = new System.Drawing.Point(1053, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(173, 238);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(11, 59);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(156, 34);
            this.cmdExit.TabIndex = 10;
            this.cmdExit.Text = "&Close";
            this.cmdExit.UseVisualStyleBackColor = true;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(11, 19);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(156, 34);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.Text = "&Save Changes";
            this.cmdSave.UseVisualStyleBackColor = true;
            // 
            // frmDataIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1238, 468);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbClear);
            this.Controls.Add(this.gbRepeat);
            this.Controls.Add(this.gbFirstTime);
            this.Controls.Add(this.lbProgess);
            this.Name = "frmDataIO";
            this.Text = "Data Interface";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDataIO_FormClosed);
            this.gbFirstTime.ResumeLayout(false);
            this.gbFirstTime.PerformLayout();
            this.gbRepeat.ResumeLayout(false);
            this.gbRepeat.PerformLayout();
            this.cbClear.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.GroupBox cbClear;
        private System.Windows.Forms.Button cmdClearAll;
        private System.Windows.Forms.OpenFileDialog ofdFileDialog;
        private System.Windows.Forms.Button cmdImportCommandersLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdExportCSV;
        private System.Windows.Forms.RadioButton rbUserLanguage;
        private System.Windows.Forms.RadioButton rbDefaultLanguage;
        private System.Windows.Forms.Button cmdImportFromCSV;
        private System.Windows.Forms.RadioButton rbImportSame;
        private System.Windows.Forms.RadioButton rbImportNewer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdTest;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.Button cmdSave;
    }
}
