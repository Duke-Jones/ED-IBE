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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDNView));
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdRemoveTrusted = new System.Windows.Forms.ButtonExt();
            this.cmdAddTrusted = new System.Windows.Forms.ButtonExt();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvTrustedSenders = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.colSenderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmdRemoveRelay = new System.Windows.Forms.ButtonExt();
            this.cmdAddRelay = new System.Windows.Forms.ButtonExt();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvEDDNRelays = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRecieve = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tbcStatistics = new System.Windows.Forms.TabControl();
            this.tabBySoftware = new System.Windows.Forms.TabPage();
            this.tbEddnStatsSW = new System.Windows.Forms.TextBox();
            this.tabByRelay = new System.Windows.Forms.TabPage();
            this.tbEddnStatsRL = new System.Windows.Forms.TextBox();
            this.tabByCommander = new System.Windows.Forms.TabPage();
            this.tbEddnStatsCM = new System.Windows.Forms.TextBox();
            this.tabByMessageType = new System.Windows.Forms.TabPage();
            this.tbEddnStatsMT = new System.Windows.Forms.TextBox();
            this.label83 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbSpoolOnlyOwn = new System.Windows.Forms.CheckBox();
            this.lblListenerStatus = new System.Windows.Forms.Label();
            this.pbListenerStatus = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEDDNAutoListen = new System.Windows.Forms.CheckBox();
            this.cbSpoolImplausibleToFile = new System.Windows.Forms.CheckBox();
            this.cbSpoolEddnToFile = new System.Windows.Forms.CheckBox();
            this.cbImportEDDN = new System.Windows.Forms.CheckBox();
            this.cmdStopListening = new System.Windows.Forms.ButtonExt();
            this.cmdStartListening = new System.Windows.Forms.ButtonExt();
            this.lbEddnImplausible = new System.Windows.Forms.ListBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tbEDDNOutput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrustedSenders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEDDNRelays)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabRecieve.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tbcStatistics.SuspendLayout();
            this.tabBySoftware.SuspendLayout();
            this.tabByRelay.SuspendLayout();
            this.tabByCommander.SuspendLayout();
            this.tabByMessageType.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbListenerStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // cmdRemoveTrusted
            // 
            this.cmdRemoveTrusted.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdRemoveTrusted.Location = new System.Drawing.Point(380, 242);
            this.cmdRemoveTrusted.Name = "cmdRemoveTrusted";
            this.cmdRemoveTrusted.Size = new System.Drawing.Size(93, 23);
            this.cmdRemoveTrusted.TabIndex = 21;
            this.cmdRemoveTrusted.Text = "Remove";
            this.toolTip1.SetToolTip(this.cmdRemoveTrusted, "Removes selected senders from list (select row header)");
            this.cmdRemoveTrusted.UseVisualStyleBackColor = true;
            this.cmdRemoveTrusted.Click += new System.EventHandler(this.cmdRemoveTrusted_Click);
            // 
            // cmdAddTrusted
            // 
            this.cmdAddTrusted.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdAddTrusted.Location = new System.Drawing.Point(281, 242);
            this.cmdAddTrusted.Name = "cmdAddTrusted";
            this.cmdAddTrusted.Size = new System.Drawing.Size(93, 23);
            this.cmdAddTrusted.TabIndex = 20;
            this.cmdAddTrusted.Text = "Add";
            this.toolTip1.SetToolTip(this.cmdAddTrusted, "Adds a new sender to the list.");
            this.cmdAddTrusted.UseVisualStyleBackColor = true;
            this.cmdAddTrusted.Click += new System.EventHandler(this.cmdAddTrusted_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Trusted Senders";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // dgvTrustedSenders
            // 
            this.dgvTrustedSenders.AllowUserToAddRows = false;
            this.dgvTrustedSenders.AllowUserToDeleteRows = false;
            this.dgvTrustedSenders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrustedSenders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSenderName});
            this.dgvTrustedSenders.DoubleBuffer = true;
            this.dgvTrustedSenders.Location = new System.Drawing.Point(10, 273);
            this.dgvTrustedSenders.Name = "dgvTrustedSenders";
            this.dgvTrustedSenders.RowHeadersWidth = 20;
            this.dgvTrustedSenders.Size = new System.Drawing.Size(463, 109);
            this.dgvTrustedSenders.TabIndex = 19;
            this.toolTip1.SetToolTip(this.dgvTrustedSenders, resources.GetString("dgvTrustedSenders.ToolTip"));
            // 
            // colSenderName
            // 
            this.colSenderName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSenderName.DataPropertyName = "Name";
            this.colSenderName.HeaderText = "Name Of Sender";
            this.colSenderName.Name = "colSenderName";
            this.colSenderName.ReadOnly = true;
            // 
            // cmdRemoveRelay
            // 
            this.cmdRemoveRelay.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdRemoveRelay.Location = new System.Drawing.Point(379, 129);
            this.cmdRemoveRelay.Name = "cmdRemoveRelay";
            this.cmdRemoveRelay.Size = new System.Drawing.Size(93, 23);
            this.cmdRemoveRelay.TabIndex = 25;
            this.cmdRemoveRelay.Text = "Remove";
            this.toolTip1.SetToolTip(this.cmdRemoveRelay, "Removes selected relay from list (select row header)");
            this.cmdRemoveRelay.UseVisualStyleBackColor = true;
            this.cmdRemoveRelay.Click += new System.EventHandler(this.cmdRemoveEDDNRelay_Click);
            // 
            // cmdAddRelay
            // 
            this.cmdAddRelay.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdAddRelay.Location = new System.Drawing.Point(280, 129);
            this.cmdAddRelay.Name = "cmdAddRelay";
            this.cmdAddRelay.Size = new System.Drawing.Size(93, 23);
            this.cmdAddRelay.TabIndex = 24;
            this.cmdAddRelay.Text = "Add";
            this.toolTip1.SetToolTip(this.cmdAddRelay, "Adds a new relay to the list (format like \'tcp://eddn-relay.elite-markets.net:950" +
        "0\')");
            this.cmdAddRelay.UseVisualStyleBackColor = true;
            this.cmdAddRelay.Click += new System.EventHandler(this.cmdAddEDDNRelay_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "EDDN Relays";
            this.toolTip1.SetToolTip(this.label4, "EDDN relays to connect to");
            // 
            // dgvEDDNRelays
            // 
            this.dgvEDDNRelays.AllowUserToAddRows = false;
            this.dgvEDDNRelays.AllowUserToDeleteRows = false;
            this.dgvEDDNRelays.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEDDNRelays.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dgvEDDNRelays.DoubleBuffer = true;
            this.dgvEDDNRelays.Location = new System.Drawing.Point(9, 160);
            this.dgvEDDNRelays.Name = "dgvEDDNRelays";
            this.dgvEDDNRelays.RowHeadersWidth = 20;
            this.dgvEDDNRelays.Size = new System.Drawing.Size(463, 72);
            this.dgvEDDNRelays.TabIndex = 23;
            this.toolTip1.SetToolTip(this.dgvEDDNRelays, "Defines the EDDN relays where ED-IBE should listen to");
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Address";
            this.dataGridViewTextBoxColumn1.HeaderText = "Address";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabRecieve);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.MaximumSize = new System.Drawing.Size(1063, 687);
            this.tabControl1.MinimumSize = new System.Drawing.Size(1063, 687);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1063, 687);
            this.tabControl1.TabIndex = 11;
            // 
            // tabRecieve
            // 
            this.tabRecieve.Controls.Add(this.groupBox5);
            this.tabRecieve.Controls.Add(this.label83);
            this.tabRecieve.Controls.Add(this.groupBox2);
            this.tabRecieve.Controls.Add(this.lbEddnImplausible);
            this.tabRecieve.Controls.Add(this.label24);
            this.tabRecieve.Controls.Add(this.tbEDDNOutput);
            this.tabRecieve.Location = new System.Drawing.Point(4, 22);
            this.tabRecieve.Name = "tabRecieve";
            this.tabRecieve.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecieve.Size = new System.Drawing.Size(1055, 661);
            this.tabRecieve.TabIndex = 0;
            this.tabRecieve.Text = "Receive Data";
            this.tabRecieve.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.tbcStatistics);
            this.groupBox5.Location = new System.Drawing.Point(493, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(556, 392);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Statistics";
            // 
            // tbcStatistics
            // 
            this.tbcStatistics.Controls.Add(this.tabBySoftware);
            this.tbcStatistics.Controls.Add(this.tabByRelay);
            this.tbcStatistics.Controls.Add(this.tabByCommander);
            this.tbcStatistics.Controls.Add(this.tabByMessageType);
            this.tbcStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcStatistics.Location = new System.Drawing.Point(3, 16);
            this.tbcStatistics.Name = "tbcStatistics";
            this.tbcStatistics.SelectedIndex = 0;
            this.tbcStatistics.Size = new System.Drawing.Size(550, 373);
            this.tbcStatistics.TabIndex = 11;
            // 
            // tabBySoftware
            // 
            this.tabBySoftware.Controls.Add(this.tbEddnStatsSW);
            this.tabBySoftware.Location = new System.Drawing.Point(4, 22);
            this.tabBySoftware.Name = "tabBySoftware";
            this.tabBySoftware.Padding = new System.Windows.Forms.Padding(3);
            this.tabBySoftware.Size = new System.Drawing.Size(542, 347);
            this.tabBySoftware.TabIndex = 0;
            this.tabBySoftware.Text = "By Software";
            this.tabBySoftware.UseVisualStyleBackColor = true;
            // 
            // tbEddnStatsSW
            // 
            this.tbEddnStatsSW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEddnStatsSW.Location = new System.Drawing.Point(3, 3);
            this.tbEddnStatsSW.Multiline = true;
            this.tbEddnStatsSW.Name = "tbEddnStatsSW";
            this.tbEddnStatsSW.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEddnStatsSW.Size = new System.Drawing.Size(536, 341);
            this.tbEddnStatsSW.TabIndex = 8;
            // 
            // tabByRelay
            // 
            this.tabByRelay.Controls.Add(this.tbEddnStatsRL);
            this.tabByRelay.Location = new System.Drawing.Point(4, 22);
            this.tabByRelay.Name = "tabByRelay";
            this.tabByRelay.Padding = new System.Windows.Forms.Padding(3);
            this.tabByRelay.Size = new System.Drawing.Size(542, 347);
            this.tabByRelay.TabIndex = 1;
            this.tabByRelay.Text = "By Relay";
            this.tabByRelay.UseVisualStyleBackColor = true;
            // 
            // tbEddnStatsRL
            // 
            this.tbEddnStatsRL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEddnStatsRL.Location = new System.Drawing.Point(3, 3);
            this.tbEddnStatsRL.Multiline = true;
            this.tbEddnStatsRL.Name = "tbEddnStatsRL";
            this.tbEddnStatsRL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEddnStatsRL.Size = new System.Drawing.Size(536, 341);
            this.tbEddnStatsRL.TabIndex = 9;
            // 
            // tabByCommander
            // 
            this.tabByCommander.Controls.Add(this.tbEddnStatsCM);
            this.tabByCommander.Location = new System.Drawing.Point(4, 22);
            this.tabByCommander.Name = "tabByCommander";
            this.tabByCommander.Padding = new System.Windows.Forms.Padding(3);
            this.tabByCommander.Size = new System.Drawing.Size(542, 347);
            this.tabByCommander.TabIndex = 2;
            this.tabByCommander.Text = "By Commander";
            this.tabByCommander.UseVisualStyleBackColor = true;
            // 
            // tbEddnStatsCM
            // 
            this.tbEddnStatsCM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEddnStatsCM.Location = new System.Drawing.Point(3, 3);
            this.tbEddnStatsCM.Multiline = true;
            this.tbEddnStatsCM.Name = "tbEddnStatsCM";
            this.tbEddnStatsCM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEddnStatsCM.Size = new System.Drawing.Size(536, 341);
            this.tbEddnStatsCM.TabIndex = 10;
            // 
            // tabByMessageType
            // 
            this.tabByMessageType.Controls.Add(this.tbEddnStatsMT);
            this.tabByMessageType.Location = new System.Drawing.Point(4, 22);
            this.tabByMessageType.Name = "tabByMessageType";
            this.tabByMessageType.Size = new System.Drawing.Size(542, 347);
            this.tabByMessageType.TabIndex = 3;
            this.tabByMessageType.Text = "By Message";
            this.tabByMessageType.UseVisualStyleBackColor = true;
            // 
            // tbEddnStatsMT
            // 
            this.tbEddnStatsMT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEddnStatsMT.Location = new System.Drawing.Point(0, 0);
            this.tbEddnStatsMT.Multiline = true;
            this.tbEddnStatsMT.Name = "tbEddnStatsMT";
            this.tbEddnStatsMT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEddnStatsMT.Size = new System.Drawing.Size(542, 347);
            this.tbEddnStatsMT.TabIndex = 11;
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(490, 414);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(76, 13);
            this.label83.TabIndex = 10;
            this.label83.Text = "Rejected Data";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbSpoolOnlyOwn);
            this.groupBox2.Controls.Add(this.cmdRemoveRelay);
            this.groupBox2.Controls.Add(this.cmdAddRelay);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dgvEDDNRelays);
            this.groupBox2.Controls.Add(this.cmdRemoveTrusted);
            this.groupBox2.Controls.Add(this.cmdAddTrusted);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dgvTrustedSenders);
            this.groupBox2.Controls.Add(this.lblListenerStatus);
            this.groupBox2.Controls.Add(this.pbListenerStatus);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbEDDNAutoListen);
            this.groupBox2.Controls.Add(this.cbSpoolImplausibleToFile);
            this.groupBox2.Controls.Add(this.cbSpoolEddnToFile);
            this.groupBox2.Controls.Add(this.cbImportEDDN);
            this.groupBox2.Controls.Add(this.cmdStopListening);
            this.groupBox2.Controls.Add(this.cmdStartListening);
            this.groupBox2.Location = new System.Drawing.Point(8, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(479, 392);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Listen for EDDN Events";
            // 
            // cbSpoolOnlyOwn
            // 
            this.cbSpoolOnlyOwn.AutoSize = true;
            this.cbSpoolOnlyOwn.Location = new System.Drawing.Point(21, 52);
            this.cbSpoolOnlyOwn.Name = "cbSpoolOnlyOwn";
            this.cbSpoolOnlyOwn.Size = new System.Drawing.Size(96, 17);
            this.cbSpoolOnlyOwn.TabIndex = 26;
            this.cbSpoolOnlyOwn.Tag = "SpoolOnlyOwn;false";
            this.cbSpoolOnlyOwn.Text = "spool only own";
            this.cbSpoolOnlyOwn.UseVisualStyleBackColor = true;
            this.cbSpoolOnlyOwn.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // lblListenerStatus
            // 
            this.lblListenerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListenerStatus.Location = new System.Drawing.Point(344, 81);
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
            this.pbListenerStatus.Location = new System.Drawing.Point(380, 38);
            this.pbListenerStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pbListenerStatus.Name = "pbListenerStatus";
            this.pbListenerStatus.Size = new System.Drawing.Size(40, 40);
            this.pbListenerStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbListenerStatus.TabIndex = 17;
            this.pbListenerStatus.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(348, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "EDDN-Listener Status";
            // 
            // cbEDDNAutoListen
            // 
            this.cbEDDNAutoListen.AutoSize = true;
            this.cbEDDNAutoListen.Location = new System.Drawing.Point(10, 86);
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
            this.cbSpoolImplausibleToFile.Location = new System.Drawing.Point(10, 69);
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
            // cbImportEDDN
            // 
            this.cbImportEDDN.AutoSize = true;
            this.cbImportEDDN.Checked = true;
            this.cbImportEDDN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbImportEDDN.Location = new System.Drawing.Point(10, 18);
            this.cbImportEDDN.Name = "cbImportEDDN";
            this.cbImportEDDN.Size = new System.Drawing.Size(189, 17);
            this.cbImportEDDN.TabIndex = 6;
            this.cbImportEDDN.Tag = "ImportEDDN;true";
            this.cbImportEDDN.Text = "import received data into database";
            this.cbImportEDDN.UseVisualStyleBackColor = true;
            this.cbImportEDDN.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // cmdStopListening
            // 
            this.cmdStopListening.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdStopListening.Location = new System.Drawing.Point(242, 61);
            this.cmdStopListening.Name = "cmdStopListening";
            this.cmdStopListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStopListening.TabIndex = 4;
            this.cmdStopListening.Text = "Stop Listening";
            this.cmdStopListening.UseVisualStyleBackColor = true;
            this.cmdStopListening.Click += new System.EventHandler(this.cmdStopListening_Click);
            // 
            // cmdStartListening
            // 
            this.cmdStartListening.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdStartListening.Location = new System.Drawing.Point(242, 30);
            this.cmdStartListening.Name = "cmdStartListening";
            this.cmdStartListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStartListening.TabIndex = 2;
            this.cmdStartListening.Text = "Start Listening";
            this.cmdStartListening.UseVisualStyleBackColor = true;
            this.cmdStartListening.Click += new System.EventHandler(this.cmdStartListening_Click);
            // 
            // lbEddnImplausible
            // 
            this.lbEddnImplausible.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbEddnImplausible.FormattingEnabled = true;
            this.lbEddnImplausible.HorizontalScrollbar = true;
            this.lbEddnImplausible.Location = new System.Drawing.Point(493, 430);
            this.lbEddnImplausible.Name = "lbEddnImplausible";
            this.lbEddnImplausible.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEddnImplausible.Size = new System.Drawing.Size(554, 225);
            this.lbEddnImplausible.TabIndex = 9;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(8, 414);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(64, 13);
            this.label24.TabIndex = 5;
            this.label24.Text = "Raw Output";
            // 
            // tbEDDNOutput
            // 
            this.tbEDDNOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEDDNOutput.Location = new System.Drawing.Point(8, 430);
            this.tbEDDNOutput.Multiline = true;
            this.tbEDDNOutput.Name = "tbEDDNOutput";
            this.tbEDDNOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEDDNOutput.Size = new System.Drawing.Size(479, 224);
            this.tbEDDNOutput.TabIndex = 3;
            // 
            // EDDNView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 687);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EDDNView";
            this.Text = "EDDNView";
            this.Load += new System.EventHandler(this.EDDNView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrustedSenders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEDDNRelays)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabRecieve.ResumeLayout(false);
            this.tabRecieve.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.tbcStatistics.ResumeLayout(false);
            this.tabBySoftware.ResumeLayout(false);
            this.tabBySoftware.PerformLayout();
            this.tabByRelay.ResumeLayout(false);
            this.tabByRelay.PerformLayout();
            this.tabByCommander.ResumeLayout(false);
            this.tabByCommander.PerformLayout();
            this.tabByMessageType.ResumeLayout(false);
            this.tabByMessageType.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbListenerStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.ListBox lbEddnImplausible;
        private System.Windows.Forms.TextBox tbEddnStatsSW;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbEDDNAutoListen;
        private System.Windows.Forms.CheckBox cbSpoolImplausibleToFile;
        private System.Windows.Forms.CheckBox cbSpoolEddnToFile;
        private System.Windows.Forms.CheckBox cbImportEDDN;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ButtonExt cmdStopListening;
        public System.Windows.Forms.TextBox tbEDDNOutput;
        private System.Windows.Forms.ButtonExt cmdStartListening;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Label lblListenerStatus;
        private System.Windows.Forms.PictureBox pbListenerStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabRecieve;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TabControl tbcStatistics;
        private System.Windows.Forms.TabPage tabBySoftware;
        private System.Windows.Forms.TabPage tabByRelay;
        private System.Windows.Forms.TabPage tabByCommander;
        private System.Windows.Forms.TextBox tbEddnStatsRL;
        private System.Windows.Forms.TextBox tbEddnStatsCM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private IBE.Enums_and_Utility_Classes.DataGridViewExt dgvTrustedSenders;
        private System.Windows.Forms.ButtonExt cmdRemoveTrusted;
        private System.Windows.Forms.ButtonExt cmdAddTrusted;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSenderName;
        private System.Windows.Forms.TabPage tabByMessageType;
        private System.Windows.Forms.TextBox tbEddnStatsMT;
        private System.Windows.Forms.ButtonExt cmdRemoveRelay;
        private System.Windows.Forms.ButtonExt cmdAddRelay;
        private System.Windows.Forms.Label label4;
        private Enums_and_Utility_Classes.DataGridViewExt dgvEDDNRelays;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.CheckBox cbSpoolOnlyOwn;
    }
}