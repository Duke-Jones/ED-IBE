namespace IBE.EDDN
{
    partial class EDDNView
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
            this.label83 = new System.Windows.Forms.Label();
            this.lbEddnImplausible = new System.Windows.Forms.ListBox();
            this.tbEddnStats = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblListenerStatus = new System.Windows.Forms.Label();
            this.pbListenerStatus = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEDDNAutoListen = new System.Windows.Forms.CheckBox();
            this.cbSpoolImplausibleToFile = new System.Windows.Forms.CheckBox();
            this.cbSpoolEddnToFile = new System.Windows.Forms.CheckBox();
            this.bPurgeAllEddnData = new System.Windows.Forms.Button();
            this.cbImportEDDN = new System.Windows.Forms.CheckBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cmdStopListening = new System.Windows.Forms.Button();
            this.tbEDDNOutput = new System.Windows.Forms.TextBox();
            this.cmdStartListening = new System.Windows.Forms.Button();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbListenerStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(15, 394);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(76, 13);
            this.label83.TabIndex = 10;
            this.label83.Text = "Rejected Data";
            // 
            // lbEddnImplausible
            // 
            this.lbEddnImplausible.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbEddnImplausible.FormattingEnabled = true;
            this.lbEddnImplausible.HorizontalScrollbar = true;
            this.lbEddnImplausible.Location = new System.Drawing.Point(12, 410);
            this.lbEddnImplausible.Name = "lbEddnImplausible";
            this.lbEddnImplausible.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEddnImplausible.Size = new System.Drawing.Size(946, 212);
            this.lbEddnImplausible.TabIndex = 9;
            // 
            // tbEddnStats
            // 
            this.tbEddnStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEddnStats.Location = new System.Drawing.Point(468, 12);
            this.tbEddnStats.Multiline = true;
            this.tbEddnStats.Name = "tbEddnStats";
            this.tbEddnStats.Size = new System.Drawing.Size(490, 379);
            this.tbEddnStats.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblListenerStatus);
            this.groupBox2.Controls.Add(this.pbListenerStatus);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbEDDNAutoListen);
            this.groupBox2.Controls.Add(this.cbSpoolImplausibleToFile);
            this.groupBox2.Controls.Add(this.cbSpoolEddnToFile);
            this.groupBox2.Controls.Add(this.bPurgeAllEddnData);
            this.groupBox2.Controls.Add(this.cbImportEDDN);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.cmdStopListening);
            this.groupBox2.Controls.Add(this.tbEDDNOutput);
            this.groupBox2.Controls.Add(this.cmdStartListening);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 379);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Listen for EDDN Events";
            // 
            // lblListenerStatus
            // 
            this.lblListenerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListenerStatus.Location = new System.Drawing.Point(315, 162);
            this.lblListenerStatus.Name = "lblListenerStatus";
            this.lblListenerStatus.Size = new System.Drawing.Size(111, 13);
            this.lblListenerStatus.TabIndex = 18;
            this.lblListenerStatus.Text = "Disabled";
            this.lblListenerStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbListenerStatus
            // 
            this.pbListenerStatus.Image = global::IBE.Properties.Resources.green_led_off_md;
            this.pbListenerStatus.InitialImage = global::IBE.Properties.Resources.green_led_off_md;
            this.pbListenerStatus.Location = new System.Drawing.Point(351, 119);
            this.pbListenerStatus.Name = "pbListenerStatus";
            this.pbListenerStatus.Size = new System.Drawing.Size(40, 40);
            this.pbListenerStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbListenerStatus.TabIndex = 17;
            this.pbListenerStatus.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(319, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "EDDN-Listener Status";
            // 
            // cbEDDNAutoListen
            // 
            this.cbEDDNAutoListen.AutoSize = true;
            this.cbEDDNAutoListen.Location = new System.Drawing.Point(10, 67);
            this.cbEDDNAutoListen.Name = "cbEDDNAutoListen";
            this.cbEDDNAutoListen.Size = new System.Drawing.Size(187, 17);
            this.cbEDDNAutoListen.TabIndex = 15;
            this.cbEDDNAutoListen.Tag = "AutoListen;false";
            this.cbEDDNAutoListen.Text = "autostart listening on program start";
            this.cbEDDNAutoListen.UseVisualStyleBackColor = true;
            this.cbEDDNAutoListen.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // cbSpoolImplausibleToFile
            // 
            this.cbSpoolImplausibleToFile.AutoSize = true;
            this.cbSpoolImplausibleToFile.Location = new System.Drawing.Point(10, 50);
            this.cbSpoolImplausibleToFile.Name = "cbSpoolImplausibleToFile";
            this.cbSpoolImplausibleToFile.Size = new System.Drawing.Size(208, 17);
            this.cbSpoolImplausibleToFile.TabIndex = 14;
            this.cbSpoolImplausibleToFile.Tag = "SpoolImplausibleToFile;false";
            this.cbSpoolImplausibleToFile.Text = "spool implausible to EddnImpOutput.txt";
            this.cbSpoolImplausibleToFile.UseVisualStyleBackColor = true;
            this.cbSpoolImplausibleToFile.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // cbSpoolEddnToFile
            // 
            this.cbSpoolEddnToFile.AutoSize = true;
            this.cbSpoolEddnToFile.Location = new System.Drawing.Point(10, 34);
            this.cbSpoolEddnToFile.Name = "cbSpoolEddnToFile";
            this.cbSpoolEddnToFile.Size = new System.Drawing.Size(137, 17);
            this.cbSpoolEddnToFile.TabIndex = 13;
            this.cbSpoolEddnToFile.Tag = "SpoolEddnToFile;false";
            this.cbSpoolEddnToFile.Text = "spool to EddnOutput.txt";
            this.cbSpoolEddnToFile.UseVisualStyleBackColor = true;
            this.cbSpoolEddnToFile.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
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
            // 
            // cbImportEDDN
            // 
            this.cbImportEDDN.AutoSize = true;
            this.cbImportEDDN.Checked = true;
            this.cbImportEDDN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbImportEDDN.Location = new System.Drawing.Point(10, 18);
            this.cbImportEDDN.Name = "cbImportEDDN";
            this.cbImportEDDN.Size = new System.Drawing.Size(221, 17);
            this.cbImportEDDN.TabIndex = 6;
            this.cbImportEDDN.Tag = "ImportEDDN;true";
            this.cbImportEDDN.Text = "import received data into RegulatedNoise";
            this.cbImportEDDN.UseVisualStyleBackColor = true;
            this.cbImportEDDN.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
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
            // cmdStopListening
            // 
            this.cmdStopListening.Location = new System.Drawing.Point(351, 50);
            this.cmdStopListening.Name = "cmdStopListening";
            this.cmdStopListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStopListening.TabIndex = 4;
            this.cmdStopListening.Text = "Stop Listening";
            this.cmdStopListening.UseVisualStyleBackColor = true;
            this.cmdStopListening.Click += new System.EventHandler(this.cmdStopListening_Click);
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
            // cmdStartListening
            // 
            this.cmdStartListening.Location = new System.Drawing.Point(351, 19);
            this.cmdStartListening.Name = "cmdStartListening";
            this.cmdStartListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStartListening.TabIndex = 2;
            this.cmdStartListening.Text = "Start Listening";
            this.cmdStartListening.UseVisualStyleBackColor = true;
            this.cmdStartListening.Click += new System.EventHandler(this.cmdStartListening_Click);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // EDDNView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 634);
            this.Controls.Add(this.label83);
            this.Controls.Add(this.lbEddnImplausible);
            this.Controls.Add(this.tbEddnStats);
            this.Controls.Add(this.groupBox2);
            this.Name = "EDDNView";
            this.Text = "EDDNView";
            this.Load += new System.EventHandler(this.EDDNView_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbListenerStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.ListBox lbEddnImplausible;
        private System.Windows.Forms.TextBox tbEddnStats;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbEDDNAutoListen;
        private System.Windows.Forms.CheckBox cbSpoolImplausibleToFile;
        private System.Windows.Forms.CheckBox cbSpoolEddnToFile;
        private System.Windows.Forms.Button bPurgeAllEddnData;
        private System.Windows.Forms.CheckBox cbImportEDDN;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button cmdStopListening;
        public System.Windows.Forms.TextBox tbEDDNOutput;
        private System.Windows.Forms.Button cmdStartListening;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Label lblListenerStatus;
        private System.Windows.Forms.PictureBox pbListenerStatus;
        private System.Windows.Forms.Label label1;
    }
}