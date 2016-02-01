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
            this.fbFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lbProgess = new System.Windows.Forms.ListBox();
            this.gbFirstTime = new System.Windows.Forms.GroupBox();
            this.gbRepeat = new System.Windows.Forms.GroupBox();
            this.cbClear = new System.Windows.Forms.GroupBox();
            this.ofdFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gbFirstTime.SuspendLayout();
            this.gbRepeat.SuspendLayout();
            this.cbClear.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdImportOldData
            // 
            this.cmdImportOldData.Location = new System.Drawing.Point(95, 15);
            this.cmdImportOldData.Name = "cmdImportOldData";
            this.cmdImportOldData.Size = new System.Drawing.Size(154, 34);
            this.cmdImportOldData.TabIndex = 0;
            this.cmdImportOldData.Text = "Import Old Datafiles";
            this.ttToolTip.SetToolTip(this.cmdImportOldData, resources.GetString("cmdImportOldData.ToolTip"));
            this.cmdImportOldData.UseVisualStyleBackColor = true;
            this.cmdImportOldData.Click += new System.EventHandler(this.cmdImportOldData_Click);
            // 
            // cbImportPriceData
            // 
            this.cbImportPriceData.AutoSize = true;
            this.cbImportPriceData.Location = new System.Drawing.Point(13, 55);
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
            this.cmdImportSystemsAndStations.Location = new System.Drawing.Point(12, 19);
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
            this.cmdClearAll.Location = new System.Drawing.Point(95, 19);
            this.cmdClearAll.Name = "cmdClearAll";
            this.cmdClearAll.Size = new System.Drawing.Size(154, 34);
            this.cmdClearAll.TabIndex = 0;
            this.cmdClearAll.Text = "Clear all !";
            this.ttToolTip.SetToolTip(this.cmdClearAll, "Clears the whole database to allow the \"First time import\" again.\r\nBe carful - al" +
        "l data from the database will be lost.");
            this.cmdClearAll.UseVisualStyleBackColor = true;
            this.cmdClearAll.Click += new System.EventHandler(this.cmdClearAll_Click);
            // 
            // cmdImportCommandersLog
            // 
            this.cmdImportCommandersLog.Location = new System.Drawing.Point(13, 92);
            this.cmdImportCommandersLog.Name = "cmdImportCommandersLog";
            this.cmdImportCommandersLog.Size = new System.Drawing.Size(276, 34);
            this.cmdImportCommandersLog.TabIndex = 3;
            this.cmdImportCommandersLog.Text = "Import RN-CommandersLog-files";
            this.ttToolTip.SetToolTip(this.cmdImportCommandersLog, resources.GetString("cmdImportCommandersLog.ToolTip"));
            this.cmdImportCommandersLog.UseVisualStyleBackColor = true;
            this.cmdImportCommandersLog.Click += new System.EventHandler(this.cmdImportCommandersLog_Click);
            // 
            // lbProgess
            // 
            this.lbProgess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgess.FormattingEnabled = true;
            this.lbProgess.Location = new System.Drawing.Point(12, 332);
            this.lbProgess.Name = "lbProgess";
            this.lbProgess.Size = new System.Drawing.Size(1083, 303);
            this.lbProgess.TabIndex = 1;
            // 
            // gbFirstTime
            // 
            this.gbFirstTime.Controls.Add(this.cmdImportOldData);
            this.gbFirstTime.Controls.Add(this.cbImportPriceData);
            this.gbFirstTime.Location = new System.Drawing.Point(12, 12);
            this.gbFirstTime.Name = "gbFirstTime";
            this.gbFirstTime.Size = new System.Drawing.Size(345, 80);
            this.gbFirstTime.TabIndex = 3;
            this.gbFirstTime.TabStop = false;
            this.gbFirstTime.Text = "First time import";
            // 
            // gbRepeat
            // 
            this.gbRepeat.Controls.Add(this.cmdImportCommandersLog);
            this.gbRepeat.Controls.Add(this.cmdImportSystemsAndStations);
            this.gbRepeat.Controls.Add(this.checkBox1);
            this.gbRepeat.Location = new System.Drawing.Point(363, 12);
            this.gbRepeat.Name = "gbRepeat";
            this.gbRepeat.Size = new System.Drawing.Size(345, 283);
            this.gbRepeat.TabIndex = 4;
            this.gbRepeat.TabStop = false;
            this.gbRepeat.Text = "Repeatable imports";
            // 
            // cbClear
            // 
            this.cbClear.Controls.Add(this.cmdClearAll);
            this.cbClear.Location = new System.Drawing.Point(12, 98);
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
            // frmDataIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1107, 664);
            this.Controls.Add(this.cbClear);
            this.Controls.Add(this.gbRepeat);
            this.Controls.Add(this.gbFirstTime);
            this.Controls.Add(this.lbProgess);
            this.Name = "frmDataIO";
            this.Text = "Data Interface";
            this.gbFirstTime.ResumeLayout(false);
            this.gbFirstTime.PerformLayout();
            this.gbRepeat.ResumeLayout(false);
            this.gbRepeat.PerformLayout();
            this.cbClear.ResumeLayout(false);
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
    }
}
