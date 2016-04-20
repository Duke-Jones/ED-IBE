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
            this.label83 = new System.Windows.Forms.Label();
            this.lbEddnImplausible = new System.Windows.Forms.ListBox();
            this.tbEddnStatsSW = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdRemoveTrusted = new System.Windows.Forms.Button();
            this.cmdAddTrusted = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvTrustedSenders = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.colSenderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblListenerStatus = new System.Windows.Forms.Label();
            this.pbListenerStatus = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEDDNAutoListen = new System.Windows.Forms.CheckBox();
            this.cbSpoolImplausibleToFile = new System.Windows.Forms.CheckBox();
            this.cbSpoolEddnToFile = new System.Windows.Forms.CheckBox();
            this.cbImportEDDN = new System.Windows.Forms.CheckBox();
            this.cmdStopListening = new System.Windows.Forms.Button();
            this.cmdStartListening = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.tbEDDNOutput = new System.Windows.Forms.TextBox();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSend = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbSchemaTest = new System.Windows.Forms.RadioButton();
            this.rbSchemaReal = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblSenderStatus = new System.Windows.Forms.Label();
            this.pbSenderStatus = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdStopSender = new System.Windows.Forms.Button();
            this.cmdStartSender = new System.Windows.Forms.Button();
            this.cbEDDNAutoSend = new System.Windows.Forms.CheckBox();
            this.cbPostOCRData = new System.Windows.Forms.CheckBox();
            this.cbPostCompanionData = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbCmdrsName = new System.Windows.Forms.RadioButton();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.rbUserID = new System.Windows.Forms.RadioButton();
            this.txtCmdrsName = new System.Windows.Forms.TextBox();
            this.tabRecieve = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tbcStatistics = new System.Windows.Forms.TabControl();
            this.tabBySoftware = new System.Windows.Forms.TabPage();
            this.tabByRelay = new System.Windows.Forms.TabPage();
            this.tbEddnStatsRL = new System.Windows.Forms.TextBox();
            this.tabByCommander = new System.Windows.Forms.TabPage();
            this.tbEddnStatsCM = new System.Windows.Forms.TextBox();
            this.tabByMessageType = new System.Windows.Forms.TabPage();
            this.tbEddnStatsMT = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lbEDDNInfo = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrustedSenders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbListenerStatus)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabSend.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSenderStatus)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabRecieve.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tbcStatistics.SuspendLayout();
            this.tabBySoftware.SuspendLayout();
            this.tabByRelay.SuspendLayout();
            this.tabByCommander.SuspendLayout();
            this.tabByMessageType.SuspendLayout();
            this.SuspendLayout();
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
            // groupBox2
            // 
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
            // cmdRemoveTrusted
            // 
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
            // lblListenerStatus
            // 
            this.lblListenerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListenerStatus.Location = new System.Drawing.Point(346, 162);
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
            this.pbListenerStatus.Location = new System.Drawing.Point(382, 119);
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
            this.label1.Location = new System.Drawing.Point(350, 103);
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
            this.cmdStopListening.Location = new System.Drawing.Point(358, 58);
            this.cmdStopListening.Name = "cmdStopListening";
            this.cmdStopListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStopListening.TabIndex = 4;
            this.cmdStopListening.Text = "Stop Listening";
            this.cmdStopListening.UseVisualStyleBackColor = true;
            this.cmdStopListening.Click += new System.EventHandler(this.cmdStopListening_Click);
            // 
            // cmdStartListening
            // 
            this.cmdStartListening.Location = new System.Drawing.Point(358, 27);
            this.cmdStartListening.Name = "cmdStartListening";
            this.cmdStartListening.Size = new System.Drawing.Size(93, 23);
            this.cmdStartListening.TabIndex = 2;
            this.cmdStartListening.Text = "Start Listening";
            this.cmdStartListening.UseVisualStyleBackColor = true;
            this.cmdStartListening.Click += new System.EventHandler(this.cmdStartListening_Click);
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
            // tmrRefresh
            // 
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSend);
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
            // tabSend
            // 
            this.tabSend.Controls.Add(this.lbEDDNInfo);
            this.tabSend.Controls.Add(this.groupBox4);
            this.tabSend.Controls.Add(this.groupBox3);
            this.tabSend.Controls.Add(this.groupBox1);
            this.tabSend.Location = new System.Drawing.Point(4, 22);
            this.tabSend.Name = "tabSend";
            this.tabSend.Padding = new System.Windows.Forms.Padding(3);
            this.tabSend.Size = new System.Drawing.Size(1055, 661);
            this.tabSend.TabIndex = 1;
            this.tabSend.Text = "Transmit Data";
            this.tabSend.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbSchemaTest);
            this.groupBox4.Controls.Add(this.rbSchemaReal);
            this.groupBox4.Location = new System.Drawing.Point(471, 92);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(374, 121);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            this.groupBox4.Tag = "Schema;Real";
            this.groupBox4.Text = "Schema";
            this.toolTip1.SetToolTip(this.groupBox4, "Choose, if you only want to check/test something\r\nor if you really want to send t" +
        "o EDDN.\r\nThis setting concerns SENDING AND RECEIVING.");
            // 
            // rbSchemaTest
            // 
            this.rbSchemaTest.AutoSize = true;
            this.rbSchemaTest.Location = new System.Drawing.Point(28, 44);
            this.rbSchemaTest.Name = "rbSchemaTest";
            this.rbSchemaTest.Size = new System.Drawing.Size(129, 17);
            this.rbSchemaTest.TabIndex = 1;
            this.rbSchemaTest.TabStop = true;
            this.rbSchemaTest.Tag = "Test";
            this.rbSchemaTest.Text = "Post To Test-Network";
            this.toolTip1.SetToolTip(this.rbSchemaTest, "Choose, if you only want to check/test something");
            this.rbSchemaTest.UseVisualStyleBackColor = true;
            this.rbSchemaTest.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // rbSchemaReal
            // 
            this.rbSchemaReal.AutoSize = true;
            this.rbSchemaReal.Location = new System.Drawing.Point(28, 21);
            this.rbSchemaReal.Name = "rbSchemaReal";
            this.rbSchemaReal.Size = new System.Drawing.Size(130, 17);
            this.rbSchemaReal.TabIndex = 0;
            this.rbSchemaReal.TabStop = true;
            this.rbSchemaReal.Tag = "Real";
            this.rbSchemaReal.Text = "Post To Real-Network";
            this.toolTip1.SetToolTip(this.rbSchemaReal, "Choose, if you only want to check/test something");
            this.rbSchemaReal.UseVisualStyleBackColor = true;
            this.rbSchemaReal.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblSenderStatus);
            this.groupBox3.Controls.Add(this.pbSenderStatus);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.cmdStopSender);
            this.groupBox3.Controls.Add(this.cmdStartSender);
            this.groupBox3.Controls.Add(this.cbEDDNAutoSend);
            this.groupBox3.Controls.Add(this.cbPostOCRData);
            this.groupBox3.Controls.Add(this.cbPostCompanionData);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(457, 207);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Post Data To EDDN";
            // 
            // lblSenderStatus
            // 
            this.lblSenderStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenderStatus.Location = new System.Drawing.Point(29, 165);
            this.lblSenderStatus.Name = "lblSenderStatus";
            this.lblSenderStatus.Size = new System.Drawing.Size(111, 13);
            this.lblSenderStatus.TabIndex = 52;
            this.lblSenderStatus.Text = "Disabled";
            this.lblSenderStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbSenderStatus
            // 
            this.pbSenderStatus.Image = global::IBE.Properties.Resources.green_led_off_md;
            this.pbSenderStatus.InitialImage = global::IBE.Properties.Resources.green_led_off_md;
            this.pbSenderStatus.Location = new System.Drawing.Point(65, 122);
            this.pbSenderStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pbSenderStatus.Name = "pbSenderStatus";
            this.pbSenderStatus.Size = new System.Drawing.Size(40, 40);
            this.pbSenderStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSenderStatus.TabIndex = 51;
            this.pbSenderStatus.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 50;
            this.label3.Text = "EDDN-Sender Status";
            // 
            // cmdStopSender
            // 
            this.cmdStopSender.Location = new System.Drawing.Point(36, 57);
            this.cmdStopSender.Name = "cmdStopSender";
            this.cmdStopSender.Size = new System.Drawing.Size(93, 23);
            this.cmdStopSender.TabIndex = 49;
            this.cmdStopSender.Text = "Disable";
            this.cmdStopSender.UseVisualStyleBackColor = true;
            this.cmdStopSender.Click += new System.EventHandler(this.cmdStopSender_Click);
            // 
            // cmdStartSender
            // 
            this.cmdStartSender.Location = new System.Drawing.Point(36, 26);
            this.cmdStartSender.Name = "cmdStartSender";
            this.cmdStartSender.Size = new System.Drawing.Size(93, 23);
            this.cmdStartSender.TabIndex = 48;
            this.cmdStartSender.Text = "Enable";
            this.cmdStartSender.UseVisualStyleBackColor = true;
            this.cmdStartSender.Click += new System.EventHandler(this.cmdStartSender_Click);
            // 
            // cbEDDNAutoSend
            // 
            this.cbEDDNAutoSend.AutoSize = true;
            this.cbEDDNAutoSend.Location = new System.Drawing.Point(201, 91);
            this.cbEDDNAutoSend.Name = "cbEDDNAutoSend";
            this.cbEDDNAutoSend.Size = new System.Drawing.Size(207, 17);
            this.cbEDDNAutoSend.TabIndex = 47;
            this.cbEDDNAutoSend.Tag = "AutoSend;false";
            this.cbEDDNAutoSend.Text = "Autoactivate Sender On Program Start";
            this.cbEDDNAutoSend.UseVisualStyleBackColor = true;
            this.cbEDDNAutoSend.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // cbPostOCRData
            // 
            this.cbPostOCRData.AutoSize = true;
            this.cbPostOCRData.Location = new System.Drawing.Point(201, 26);
            this.cbPostOCRData.Name = "cbPostOCRData";
            this.cbPostOCRData.Size = new System.Drawing.Size(137, 17);
            this.cbPostOCRData.TabIndex = 40;
            this.cbPostOCRData.Tag = "EDDNPostOCRData;True";
            this.cbPostOCRData.Text = "Market Data From OCR";
            this.cbPostOCRData.UseVisualStyleBackColor = true;
            this.cbPostOCRData.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // cbPostCompanionData
            // 
            this.cbPostCompanionData.AutoSize = true;
            this.cbPostCompanionData.Location = new System.Drawing.Point(201, 49);
            this.cbPostCompanionData.Name = "cbPostCompanionData";
            this.cbPostCompanionData.Size = new System.Drawing.Size(190, 17);
            this.cbPostCompanionData.TabIndex = 46;
            this.cbPostCompanionData.Tag = "EDDNPostCompanionData;True";
            this.cbPostCompanionData.Text = "Market Data From Companion API ";
            this.cbPostCompanionData.UseVisualStyleBackColor = true;
            this.cbPostCompanionData.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbCmdrsName);
            this.groupBox1.Controls.Add(this.tbUsername);
            this.groupBox1.Controls.Add(this.rbUserID);
            this.groupBox1.Controls.Add(this.txtCmdrsName);
            this.groupBox1.Location = new System.Drawing.Point(471, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(374, 80);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "Identification;useUserName";
            this.groupBox1.Text = "Identification";
            this.toolTip1.SetToolTip(this.groupBox1, "Choose the anonymous ID or your name for sending to EDDN.");
            // 
            // rbCmdrsName
            // 
            this.rbCmdrsName.AutoSize = true;
            this.rbCmdrsName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbCmdrsName.Location = new System.Drawing.Point(20, 45);
            this.rbCmdrsName.Name = "rbCmdrsName";
            this.rbCmdrsName.Size = new System.Drawing.Size(85, 17);
            this.rbCmdrsName.TabIndex = 45;
            this.rbCmdrsName.TabStop = true;
            this.rbCmdrsName.Tag = "useUserName";
            this.rbCmdrsName.Text = "Cmdrs Name";
            this.toolTip1.SetToolTip(this.rbCmdrsName, "Choose the anonymous ID or your name for sending to EDDN.");
            this.rbCmdrsName.UseVisualStyleBackColor = true;
            this.rbCmdrsName.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(114, 23);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(230, 20);
            this.tbUsername.TabIndex = 41;
            this.tbUsername.Tag = "UserID;EMPTY";
            this.toolTip1.SetToolTip(this.tbUsername, "Choose the anonymous ID or your name for sending to EDDN.");
            this.tbUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            this.tbUsername.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // rbUserID
            // 
            this.rbUserID.AutoSize = true;
            this.rbUserID.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbUserID.Location = new System.Drawing.Point(44, 24);
            this.rbUserID.Name = "rbUserID";
            this.rbUserID.Size = new System.Drawing.Size(61, 17);
            this.rbUserID.TabIndex = 44;
            this.rbUserID.TabStop = true;
            this.rbUserID.Tag = "useUserID";
            this.rbUserID.Text = "User ID";
            this.toolTip1.SetToolTip(this.rbUserID, "Choose the anonymous ID or your name for sending to EDDN.");
            this.rbUserID.UseVisualStyleBackColor = true;
            this.rbUserID.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // txtCmdrsName
            // 
            this.txtCmdrsName.Location = new System.Drawing.Point(114, 45);
            this.txtCmdrsName.Name = "txtCmdrsName";
            this.txtCmdrsName.Size = new System.Drawing.Size(230, 20);
            this.txtCmdrsName.TabIndex = 43;
            this.txtCmdrsName.Tag = "UserName;EMPTY";
            this.toolTip1.SetToolTip(this.txtCmdrsName, "Choose the anonymous ID or your name for sending to EDDN.");
            this.txtCmdrsName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            this.txtCmdrsName.Leave += new System.EventHandler(this.TextBox_Leave);
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
            this.tabRecieve.Text = "Recieve Data";
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
            // lbEDDNInfo
            // 
            this.lbEDDNInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbEDDNInfo.AutoSize = true;
            this.lbEDDNInfo.Font = new System.Drawing.Font("Cooper Black", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEDDNInfo.ForeColor = System.Drawing.Color.Crimson;
            this.lbEDDNInfo.Location = new System.Drawing.Point(191, 298);
            this.lbEDDNInfo.Name = "lbEDDNInfo";
            this.lbEDDNInfo.Size = new System.Drawing.Size(709, 180);
            this.lbEDDNInfo.TabIndex = 49;
            this.lbEDDNInfo.Text = "Have in mind:\r\n\r\nThe EDDN/EDDB lives from the data. \r\nIf you want to receive data" +
    " permanently, \r\nit would be fair in return also to send data.";
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrustedSenders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbListenerStatus)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabSend.ResumeLayout(false);
            this.tabSend.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSenderStatus)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Button cmdStopListening;
        public System.Windows.Forms.TextBox tbEDDNOutput;
        private System.Windows.Forms.Button cmdStartListening;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Label lblListenerStatus;
        private System.Windows.Forms.PictureBox pbListenerStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSend;
        private System.Windows.Forms.TabPage tabRecieve;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbSchemaTest;
        private System.Windows.Forms.RadioButton rbSchemaReal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbPostOCRData;
        private System.Windows.Forms.CheckBox cbPostCompanionData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbCmdrsName;
        internal System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.RadioButton rbUserID;
        internal System.Windows.Forms.TextBox txtCmdrsName;
        private System.Windows.Forms.CheckBox cbEDDNAutoSend;
        private System.Windows.Forms.Label lblSenderStatus;
        private System.Windows.Forms.PictureBox pbSenderStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdStopSender;
        private System.Windows.Forms.Button cmdStartSender;
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
        private System.Windows.Forms.Button cmdRemoveTrusted;
        private System.Windows.Forms.Button cmdAddTrusted;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSenderName;
        private System.Windows.Forms.TabPage tabByMessageType;
        private System.Windows.Forms.TextBox tbEddnStatsMT;
        private System.Windows.Forms.Label lbEDDNInfo;
    }
}