namespace IBE.MTSettings
{
    partial class tabSettings
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tabSettings));
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.gbTesseract = new System.Windows.Forms.GroupBox();
            this.txtOCRTraineddataFile = new System.Windows.Forms.TextBox();
            this.cmdSelectTraineddataFile = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cmdFilter = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.txtGUIColorCutoffLevel = new System.Windows.Forms.TextBoxInt32();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label52 = new System.Windows.Forms.Label();
            this.cbCheckNextScreenshotForOne = new System.Windows.Forms.CheckBox();
            this.lblPixelAmount = new System.Windows.Forms.Label();
            this.txtOCRPixelAmount = new System.Windows.Forms.TextBoxInt32();
            this.lblPixelThreshold = new System.Windows.Forms.Label();
            this.txtOCRPixelThreshold = new System.Windows.Forms.TextBoxDouble();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.cbAutoAdd_ReplaceVisited = new System.Windows.Forms.CheckBox();
            this.cbAutoAdd_Marketdata = new System.Windows.Forms.CheckBox();
            this.cbAutoAdd_Visited = new System.Windows.Forms.CheckBox();
            this.label49 = new System.Windows.Forms.Label();
            this.cbAutoAdd_JumpedTo = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label48 = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label89 = new System.Windows.Forms.Label();
            this.nudPurgeOldDataDays = new System.Windows.Forms.NumericUpDown();
            this.cmdPurgeOldData = new System.Windows.Forms.Button();
            this.cbAutoActivateSystemTab = new System.Windows.Forms.CheckBox();
            this.cbAutoActivateOCRTab = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label25 = new System.Windows.Forms.Label();
            this.txtExtTool_ParamMarket = new System.Windows.Forms.TextBox();
            this.txtExtTool_ParamLocation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExtTool_Path = new System.Windows.Forms.TextBox();
            this.cmdSelectExternalToolPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbVisitedFilter = new System.Windows.Forms.ComboBoxInt32();
            this.txtSQLConnectionPort = new System.Windows.Forms.TextBoxInt32();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rbNoDistanceToStarConsider = new System.Windows.Forms.RadioButton();
            this.rbNoDistanceToStarIgnore = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbNoLandingPadSizeConsider = new System.Windows.Forms.RadioButton();
            this.rbNoLandingPadSizeIgnore = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.gbExternalDataInterface = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tcDataInterface = new System.Windows.Forms.TabControl();
            this.tabOCRSettings = new System.Windows.Forms.TabPage();
            this.tabExternalToolSettings = new System.Windows.Forms.TabPage();
            this.gbDataInterface = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbUseExternalTool = new System.Windows.Forms.RadioButton();
            this.rbUseOCR = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmdChangeSQLPort = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtGamePath = new System.Windows.Forms.TextBox();
            this.cmdGamePath = new System.Windows.Forms.Button();
            this.groupBox6.SuspendLayout();
            this.gbTesseract.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPurgeOldDataDays)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.gbExternalDataInterface.SuspendLayout();
            this.tcDataInterface.SuspendLayout();
            this.tabOCRSettings.SuspendLayout();
            this.tabExternalToolSettings.SuspendLayout();
            this.gbDataInterface.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.gbTesseract);
            this.groupBox6.Controls.Add(this.groupBox9);
            this.groupBox6.Controls.Add(this.groupBox11);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(619, 428);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "OCR-Settings";
            // 
            // gbTesseract
            // 
            this.gbTesseract.Controls.Add(this.txtOCRTraineddataFile);
            this.gbTesseract.Controls.Add(this.cmdSelectTraineddataFile);
            this.gbTesseract.Controls.Add(this.label12);
            this.gbTesseract.Location = new System.Drawing.Point(11, 16);
            this.gbTesseract.Name = "gbTesseract";
            this.gbTesseract.Size = new System.Drawing.Size(597, 68);
            this.gbTesseract.TabIndex = 16;
            this.gbTesseract.TabStop = false;
            this.gbTesseract.Text = "Tesseract Setting";
            // 
            // txtOCRTraineddataFile
            // 
            this.txtOCRTraineddataFile.Location = new System.Drawing.Point(102, 32);
            this.txtOCRTraineddataFile.Name = "txtOCRTraineddataFile";
            this.txtOCRTraineddataFile.ReadOnly = true;
            this.txtOCRTraineddataFile.Size = new System.Drawing.Size(197, 20);
            this.txtOCRTraineddataFile.TabIndex = 13;
            this.txtOCRTraineddataFile.Tag = "OCRTraineddataFile;big";
            // 
            // cmdSelectTraineddataFile
            // 
            this.cmdSelectTraineddataFile.Location = new System.Drawing.Point(346, 30);
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
            this.label12.Location = new System.Drawing.Point(122, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(177, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "\"traineddata\"-File for TesseractOCR";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.cmdFilter);
            this.groupBox9.Controls.Add(this.label50);
            this.groupBox9.Controls.Add(this.label51);
            this.groupBox9.Controls.Add(this.txtGUIColorCutoffLevel);
            this.groupBox9.Location = new System.Drawing.Point(11, 237);
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
            this.txtGUIColorCutoffLevel.MaxValue = 255;
            this.txtGUIColorCutoffLevel.MinValue = 0;
            this.txtGUIColorCutoffLevel.Name = "txtGUIColorCutoffLevel";
            this.txtGUIColorCutoffLevel.Size = new System.Drawing.Size(37, 20);
            this.txtGUIColorCutoffLevel.TabIndex = 13;
            this.txtGUIColorCutoffLevel.Tag = "GUIColorCutoffLevelValue;150";
            this.txtGUIColorCutoffLevel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGUIColorCutoffLevel_KeyDown);
            this.txtGUIColorCutoffLevel.Leave += new System.EventHandler(this.txtGUIColorCutoffLevel_Leave);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label52);
            this.groupBox11.Controls.Add(this.cbCheckNextScreenshotForOne);
            this.groupBox11.Controls.Add(this.lblPixelAmount);
            this.groupBox11.Controls.Add(this.txtOCRPixelAmount);
            this.groupBox11.Controls.Add(this.lblPixelThreshold);
            this.groupBox11.Controls.Add(this.txtOCRPixelThreshold);
            this.groupBox11.Location = new System.Drawing.Point(11, 90);
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
            // cbCheckNextScreenshotForOne
            // 
            this.cbCheckNextScreenshotForOne.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbCheckNextScreenshotForOne.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.cbCheckNextScreenshotForOne.Location = new System.Drawing.Point(350, 97);
            this.cbCheckNextScreenshotForOne.Name = "cbCheckNextScreenshotForOne";
            this.cbCheckNextScreenshotForOne.Size = new System.Drawing.Size(176, 23);
            this.cbCheckNextScreenshotForOne.TabIndex = 18;
            this.cbCheckNextScreenshotForOne.Tag = "CheckNextScreenshotForOne;false";
            this.cbCheckNextScreenshotForOne.Text = "check next screenshot for a \"1\"";
            this.cbCheckNextScreenshotForOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbCheckNextScreenshotForOne.UseVisualStyleBackColor = true;
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
            // txtOCRPixelAmount
            // 
            this.txtOCRPixelAmount.Location = new System.Drawing.Point(212, 115);
            this.txtOCRPixelAmount.MaxValue = null;
            this.txtOCRPixelAmount.MinValue = null;
            this.txtOCRPixelAmount.Name = "txtOCRPixelAmount";
            this.txtOCRPixelAmount.Size = new System.Drawing.Size(37, 20);
            this.txtOCRPixelAmount.TabIndex = 15;
            this.txtOCRPixelAmount.Tag = "OCRPixelAmountValue;22";
            this.txtOCRPixelAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOCRPixelAmount_KeyDown);
            this.txtOCRPixelAmount.Leave += new System.EventHandler(this.txtOCRPixelAmount_Leave);
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
            // txtOCRPixelThreshold
            // 
            this.txtOCRPixelThreshold.Digits = 2;
            this.txtOCRPixelThreshold.Location = new System.Drawing.Point(212, 88);
            this.txtOCRPixelThreshold.MaxValue = 1D;
            this.txtOCRPixelThreshold.MinValue = 0.01D;
            this.txtOCRPixelThreshold.Name = "txtOCRPixelThreshold";
            this.txtOCRPixelThreshold.Size = new System.Drawing.Size(37, 20);
            this.txtOCRPixelThreshold.TabIndex = 13;
            this.txtOCRPixelThreshold.Tag = "OCRPixelThresholdValue;0.6";
            this.txtOCRPixelThreshold.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOCRPixelThreshold_KeyDown);
            this.txtOCRPixelThreshold.Leave += new System.EventHandler(this.txtOCRPixelThreshold_Leave);
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
            this.groupBox10.Location = new System.Drawing.Point(650, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(527, 151);
            this.groupBox10.TabIndex = 12;
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
            this.cbAutoAdd_ReplaceVisited.Tag = "AutoAdd_ReplaceVisited;true";
            this.cbAutoAdd_ReplaceVisited.Text = "replace previous \"Visited\" with  \"Market Data Collected\"";
            this.cbAutoAdd_ReplaceVisited.UseVisualStyleBackColor = true;
            this.cbAutoAdd_ReplaceVisited.CheckedChanged += new System.EventHandler(this.ComboBox_CheckedChanged);
            // 
            // cbAutoAdd_Marketdata
            // 
            this.cbAutoAdd_Marketdata.AutoSize = true;
            this.cbAutoAdd_Marketdata.Location = new System.Drawing.Point(32, 90);
            this.cbAutoAdd_Marketdata.Name = "cbAutoAdd_Marketdata";
            this.cbAutoAdd_Marketdata.Size = new System.Drawing.Size(173, 17);
            this.cbAutoAdd_Marketdata.TabIndex = 3;
            this.cbAutoAdd_Marketdata.Tag = "AutoAdd_Marketdata;true";
            this.cbAutoAdd_Marketdata.Text = "\"Market Data Collected\"-Event";
            this.cbAutoAdd_Marketdata.UseVisualStyleBackColor = true;
            this.cbAutoAdd_Marketdata.CheckedChanged += new System.EventHandler(this.ComboBox_CheckedChanged);
            // 
            // cbAutoAdd_Visited
            // 
            this.cbAutoAdd_Visited.AutoSize = true;
            this.cbAutoAdd_Visited.Location = new System.Drawing.Point(32, 67);
            this.cbAutoAdd_Visited.Name = "cbAutoAdd_Visited";
            this.cbAutoAdd_Visited.Size = new System.Drawing.Size(98, 17);
            this.cbAutoAdd_Visited.TabIndex = 2;
            this.cbAutoAdd_Visited.Tag = "AutoAdd_Visited;true";
            this.cbAutoAdd_Visited.Text = "\"Visited\"-Event";
            this.cbAutoAdd_Visited.UseVisualStyleBackColor = true;
            this.cbAutoAdd_Visited.CheckedChanged += new System.EventHandler(this.ComboBox_CheckedChanged);
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
            this.cbAutoAdd_JumpedTo.Tag = "AutoAdd_JumpedTo;true";
            this.cbAutoAdd_JumpedTo.Text = "\"Jumped to\"-Event";
            this.cbAutoAdd_JumpedTo.UseVisualStyleBackColor = true;
            this.cbAutoAdd_JumpedTo.CheckedChanged += new System.EventHandler(this.ComboBox_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.label48);
            this.groupBox8.Controls.Add(this.cmbLanguage);
            this.groupBox8.Location = new System.Drawing.Point(650, 160);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(525, 45);
            this.groupBox8.TabIndex = 13;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Language";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(8, 16);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(255, 13);
            this.label48.TabIndex = 1;
            this.label48.Text = "Select the language of your Elite Dangerous version.";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(269, 13);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(75, 21);
            this.cmbLanguage.TabIndex = 0;
            this.cmbLanguage.Tag = "Language;eng";
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.Combobox_SelectedIndexChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Controls.Add(this.label89);
            this.groupBox12.Controls.Add(this.nudPurgeOldDataDays);
            this.groupBox12.Controls.Add(this.cmdPurgeOldData);
            this.groupBox12.Controls.Add(this.cbAutoActivateSystemTab);
            this.groupBox12.Controls.Add(this.cbAutoActivateOCRTab);
            this.groupBox12.Controls.Add(this.button6);
            this.groupBox12.Location = new System.Drawing.Point(650, 418);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(527, 179);
            this.groupBox12.TabIndex = 14;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Other";
            // 
            // label89
            // 
            this.label89.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(203, 96);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(29, 13);
            this.label89.TabIndex = 19;
            this.label89.Text = "days";
            // 
            // nudPurgeOldDataDays
            // 
            this.nudPurgeOldDataDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudPurgeOldDataDays.Location = new System.Drawing.Point(154, 93);
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
            this.nudPurgeOldDataDays.Tag = "PurgeOldDataDays;30";
            this.nudPurgeOldDataDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPurgeOldDataDays.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudPurgeOldDataDays_KeyDown);
            this.nudPurgeOldDataDays.Leave += new System.EventHandler(this.nudPurgeOldDataDays_Leave);
            // 
            // cmdPurgeOldData
            // 
            this.cmdPurgeOldData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdPurgeOldData.Location = new System.Drawing.Point(32, 91);
            this.cmdPurgeOldData.Name = "cmdPurgeOldData";
            this.cmdPurgeOldData.Size = new System.Drawing.Size(116, 23);
            this.cmdPurgeOldData.TabIndex = 8;
            this.cmdPurgeOldData.Text = "purge data older than";
            this.cmdPurgeOldData.UseVisualStyleBackColor = true;
            this.cmdPurgeOldData.Click += new System.EventHandler(this.cmdPurgeOldData_Click);
            // 
            // cbAutoActivateSystemTab
            // 
            this.cbAutoActivateSystemTab.AutoSize = true;
            this.cbAutoActivateSystemTab.Location = new System.Drawing.Point(32, 50);
            this.cbAutoActivateSystemTab.Name = "cbAutoActivateSystemTab";
            this.cbAutoActivateSystemTab.Size = new System.Drawing.Size(312, 17);
            this.cbAutoActivateSystemTab.TabIndex = 5;
            this.cbAutoActivateSystemTab.Tag = "AutoActivateSystemTab;true";
            this.cbAutoActivateSystemTab.Text = "automatically activate the \"System Data\" tab after hyperjump";
            this.cbAutoActivateSystemTab.UseVisualStyleBackColor = true;
            this.cbAutoActivateSystemTab.CheckedChanged += new System.EventHandler(this.ComboBox_CheckedChanged);
            // 
            // cbAutoActivateOCRTab
            // 
            this.cbAutoActivateOCRTab.AutoSize = true;
            this.cbAutoActivateOCRTab.Location = new System.Drawing.Point(32, 27);
            this.cbAutoActivateOCRTab.Name = "cbAutoActivateOCRTab";
            this.cbAutoActivateOCRTab.Size = new System.Drawing.Size(325, 17);
            this.cbAutoActivateOCRTab.TabIndex = 3;
            this.cbAutoActivateOCRTab.Tag = "AutoActivateOCRTab;true";
            this.cbAutoActivateOCRTab.Text = "Automatically activate the OCR-Tab when the recognition starts";
            this.cbAutoActivateOCRTab.UseVisualStyleBackColor = true;
            this.cbAutoActivateOCRTab.CheckedChanged += new System.EventHandler(this.ComboBox_CheckedChanged);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Location = new System.Drawing.Point(32, 127);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(226, 23);
            this.button6.TabIndex = 2;
            this.button6.Text = "Edit Commodity Price Warn levels";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.cmdWarnLevels_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(12, 19);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(65, 13);
            this.label25.TabIndex = 64;
            this.label25.Text = "Filter Setting";
            this.toolTip1.SetToolTip(this.label25, "Here you can select, if you want to see in all analysis");
            // 
            // txtExtTool_ParamMarket
            // 
            this.txtExtTool_ParamMarket.Location = new System.Drawing.Point(11, 211);
            this.txtExtTool_ParamMarket.Multiline = true;
            this.txtExtTool_ParamMarket.Name = "txtExtTool_ParamMarket";
            this.txtExtTool_ParamMarket.Size = new System.Drawing.Size(416, 20);
            this.txtExtTool_ParamMarket.TabIndex = 72;
            this.txtExtTool_ParamMarket.Tag = "ExtTool_ParamMarket;-m !OUTPUTFILE!";
            this.toolTip1.SetToolTip(this.txtExtTool_ParamMarket, "Example (for EDMC): \"-m !OUTPUTFILE!\"");
            this.txtExtTool_ParamMarket.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            this.txtExtTool_ParamMarket.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // txtExtTool_ParamLocation
            // 
            this.txtExtTool_ParamLocation.Location = new System.Drawing.Point(11, 157);
            this.txtExtTool_ParamLocation.Name = "txtExtTool_ParamLocation";
            this.txtExtTool_ParamLocation.Size = new System.Drawing.Size(416, 20);
            this.txtExtTool_ParamLocation.TabIndex = 70;
            this.txtExtTool_ParamLocation.Tag = "ExtTool_ParamLocation;-m !OUTPUTFILE!";
            this.toolTip1.SetToolTip(this.txtExtTool_ParamLocation, "Example (for EDMC): \"-m !OUTPUTFILE!\"");
            this.txtExtTool_ParamLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            this.txtExtTool_ParamLocation.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(571, 29);
            this.label5.TabIndex = 73;
            this.label5.Text = "c) Parameters for getting market data. Use placeholder \"!OUTPUTFILE!\" (without qu" +
    "otes) to specify the location of parameter for the destination file *).";
            this.toolTip1.SetToolTip(this.label5, "Example (for EDMC): \"-m !OUTPUTFILE!\"");
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(562, 52);
            this.label4.TabIndex = 71;
            this.label4.Text = resources.GetString("label4.Text");
            this.toolTip1.SetToolTip(this.label4, "Example (for EDMC): \"-m !OUTPUTFILE!\"");
            // 
            // txtExtTool_Path
            // 
            this.txtExtTool_Path.Location = new System.Drawing.Point(11, 83);
            this.txtExtTool_Path.Name = "txtExtTool_Path";
            this.txtExtTool_Path.Size = new System.Drawing.Size(420, 20);
            this.txtExtTool_Path.TabIndex = 69;
            this.txtExtTool_Path.Tag = "ExtTool_Path;EMPTY";
            this.toolTip1.SetToolTip(this.txtExtTool_Path, "Example (for EDMC): \"C:\\Program Files (x86)\\EDMarketConnector\\EDMC.exe\"");
            this.txtExtTool_Path.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            this.txtExtTool_Path.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // cmdSelectExternalToolPath
            // 
            this.cmdSelectExternalToolPath.Location = new System.Drawing.Point(437, 81);
            this.cmdSelectExternalToolPath.Name = "cmdSelectExternalToolPath";
            this.cmdSelectExternalToolPath.Size = new System.Drawing.Size(171, 23);
            this.cmdSelectExternalToolPath.TabIndex = 68;
            this.cmdSelectExternalToolPath.Text = "Select";
            this.toolTip1.SetToolTip(this.cmdSelectExternalToolPath, "Example (for EDMC): \"C:\\Program Files (x86)\\EDMarketConnector\\EDMC.exe\"");
            this.cmdSelectExternalToolPath.UseVisualStyleBackColor = true;
            this.cmdSelectExternalToolPath.Click += new System.EventHandler(this.cmdSelectExternalToolPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(352, 13);
            this.label3.TabIndex = 67;
            this.label3.Text = "a) Path to the ED Market Connector (command line version \"EDMC.exe\")";
            this.toolTip1.SetToolTip(this.label3, "Example (for EDMC): \"C:\\Program Files (x86)\\EDMarketConnector\\EDMC.exe\"");
            // 
            // cmbVisitedFilter
            // 
            this.cmbVisitedFilter.Cursor = System.Windows.Forms.Cursors.UpArrow;
            this.cmbVisitedFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVisitedFilter.FormattingEnabled = true;
            this.cmbVisitedFilter.Location = new System.Drawing.Point(83, 14);
            this.cmbVisitedFilter.MaxValue = null;
            this.cmbVisitedFilter.MinValue = null;
            this.cmbVisitedFilter.Name = "cmbVisitedFilter";
            this.cmbVisitedFilter.Size = new System.Drawing.Size(192, 21);
            this.cmbVisitedFilter.TabIndex = 63;
            this.cmbVisitedFilter.Tag = "VisitedFilter;1";
            this.toolTip1.SetToolTip(this.cmbVisitedFilter, "Here you can select, if you want to see in all analysis\r\n- all stations (independ" +
        "ent if you\'ve visted them or not)\r\n- only stations in systems you\'ve visited \r\n-" +
        " only stations you\'ve directly visted");
            this.cmbVisitedFilter.SelectedIndexChanged += new System.EventHandler(this.Combobox_SelectedIndexChanged);
            // 
            // txtSQLConnectionPort
            // 
            this.txtSQLConnectionPort.Location = new System.Drawing.Point(70, 24);
            this.txtSQLConnectionPort.MaxValue = null;
            this.txtSQLConnectionPort.MinValue = null;
            this.txtSQLConnectionPort.Name = "txtSQLConnectionPort";
            this.txtSQLConnectionPort.Size = new System.Drawing.Size(63, 20);
            this.txtSQLConnectionPort.TabIndex = 17;
            this.txtSQLConnectionPort.Tag = "OCRPixelAmountValue;22";
            this.toolTip1.SetToolTip(this.txtSQLConnectionPort, "connection port for the mysql-server (restart required)");
            this.txtSQLConnectionPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSQLConnectionPort_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbVisitedFilter);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Location = new System.Drawing.Point(650, 211);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(525, 133);
            this.groupBox1.TabIndex = 65;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datafilter";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rbNoDistanceToStarConsider);
            this.groupBox7.Controls.Add(this.rbNoDistanceToStarIgnore);
            this.groupBox7.Location = new System.Drawing.Point(312, 65);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(203, 45);
            this.groupBox7.TabIndex = 67;
            this.groupBox7.TabStop = false;
            this.groupBox7.Tag = "NoDistanceToStar;consider";
            this.groupBox7.Text = "Stations Without Distance-To-Star";
            // 
            // rbNoDistanceToStarConsider
            // 
            this.rbNoDistanceToStarConsider.AutoSize = true;
            this.rbNoDistanceToStarConsider.Checked = true;
            this.rbNoDistanceToStarConsider.Location = new System.Drawing.Point(23, 19);
            this.rbNoDistanceToStarConsider.Name = "rbNoDistanceToStarConsider";
            this.rbNoDistanceToStarConsider.Size = new System.Drawing.Size(66, 17);
            this.rbNoDistanceToStarConsider.TabIndex = 3;
            this.rbNoDistanceToStarConsider.TabStop = true;
            this.rbNoDistanceToStarConsider.Tag = "consider";
            this.rbNoDistanceToStarConsider.Text = "Consider";
            this.rbNoDistanceToStarConsider.UseVisualStyleBackColor = true;
            this.rbNoDistanceToStarConsider.CheckedChanged += new System.EventHandler(this.rbInterface_CheckedChanged);
            // 
            // rbNoDistanceToStarIgnore
            // 
            this.rbNoDistanceToStarIgnore.AutoSize = true;
            this.rbNoDistanceToStarIgnore.Location = new System.Drawing.Point(117, 19);
            this.rbNoDistanceToStarIgnore.Name = "rbNoDistanceToStarIgnore";
            this.rbNoDistanceToStarIgnore.Size = new System.Drawing.Size(55, 17);
            this.rbNoDistanceToStarIgnore.TabIndex = 2;
            this.rbNoDistanceToStarIgnore.Tag = "ignore";
            this.rbNoDistanceToStarIgnore.Text = "Ignore";
            this.rbNoDistanceToStarIgnore.UseVisualStyleBackColor = true;
            this.rbNoDistanceToStarIgnore.CheckedChanged += new System.EventHandler(this.rbInterface_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbNoLandingPadSizeConsider);
            this.groupBox5.Controls.Add(this.rbNoLandingPadSizeIgnore);
            this.groupBox5.Location = new System.Drawing.Point(312, 14);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(203, 45);
            this.groupBox5.TabIndex = 66;
            this.groupBox5.TabStop = false;
            this.groupBox5.Tag = "NoLandingPadSize;consider";
            this.groupBox5.Text = "Stations Without Landingpad - Sizes";
            // 
            // rbNoLandingPadSizeConsider
            // 
            this.rbNoLandingPadSizeConsider.AutoSize = true;
            this.rbNoLandingPadSizeConsider.Checked = true;
            this.rbNoLandingPadSizeConsider.Location = new System.Drawing.Point(23, 19);
            this.rbNoLandingPadSizeConsider.Name = "rbNoLandingPadSizeConsider";
            this.rbNoLandingPadSizeConsider.Size = new System.Drawing.Size(66, 17);
            this.rbNoLandingPadSizeConsider.TabIndex = 1;
            this.rbNoLandingPadSizeConsider.TabStop = true;
            this.rbNoLandingPadSizeConsider.Tag = "consider";
            this.rbNoLandingPadSizeConsider.Text = "Consider";
            this.rbNoLandingPadSizeConsider.UseVisualStyleBackColor = true;
            this.rbNoLandingPadSizeConsider.CheckedChanged += new System.EventHandler(this.rbInterface_CheckedChanged);
            // 
            // rbNoLandingPadSizeIgnore
            // 
            this.rbNoLandingPadSizeIgnore.AutoSize = true;
            this.rbNoLandingPadSizeIgnore.Location = new System.Drawing.Point(117, 19);
            this.rbNoLandingPadSizeIgnore.Name = "rbNoLandingPadSizeIgnore";
            this.rbNoLandingPadSizeIgnore.Size = new System.Drawing.Size(55, 17);
            this.rbNoLandingPadSizeIgnore.TabIndex = 0;
            this.rbNoLandingPadSizeIgnore.Tag = "ignore";
            this.rbNoLandingPadSizeIgnore.Text = "Ignore";
            this.rbNoLandingPadSizeIgnore.UseVisualStyleBackColor = true;
            this.rbNoLandingPadSizeIgnore.CheckedChanged += new System.EventHandler(this.rbInterface_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 92);
            this.label1.TabIndex = 65;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // gbExternalDataInterface
            // 
            this.gbExternalDataInterface.Controls.Add(this.txtExtTool_ParamMarket);
            this.gbExternalDataInterface.Controls.Add(this.label6);
            this.gbExternalDataInterface.Controls.Add(this.txtExtTool_ParamLocation);
            this.gbExternalDataInterface.Controls.Add(this.label5);
            this.gbExternalDataInterface.Controls.Add(this.label4);
            this.gbExternalDataInterface.Controls.Add(this.txtExtTool_Path);
            this.gbExternalDataInterface.Controls.Add(this.cmdSelectExternalToolPath);
            this.gbExternalDataInterface.Controls.Add(this.label3);
            this.gbExternalDataInterface.Controls.Add(this.label2);
            this.gbExternalDataInterface.Location = new System.Drawing.Point(6, 6);
            this.gbExternalDataInterface.Name = "gbExternalDataInterface";
            this.gbExternalDataInterface.Size = new System.Drawing.Size(619, 393);
            this.gbExternalDataInterface.TabIndex = 66;
            this.gbExternalDataInterface.TabStop = false;
            this.gbExternalDataInterface.Text = "ED Market Connector Interface";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(571, 29);
            this.label6.TabIndex = 74;
            this.label6.Text = "*) the path and the filename will be automatically selected. You can find it in y" +
    "our \"LocalApplicationDataPath\".";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(605, 40);
            this.label2.TabIndex = 66;
            this.label2.Text = "If you don\'t wan\'t to use the internal OCR-interface you also can use the \"ED Mar" +
    "ket Connector\" \r\nfor getting the current station and market data.";
            // 
            // tcDataInterface
            // 
            this.tcDataInterface.Controls.Add(this.tabOCRSettings);
            this.tcDataInterface.Controls.Add(this.tabExternalToolSettings);
            this.tcDataInterface.Location = new System.Drawing.Point(6, 90);
            this.tcDataInterface.Name = "tcDataInterface";
            this.tcDataInterface.SelectedIndex = 0;
            this.tcDataInterface.Size = new System.Drawing.Size(629, 426);
            this.tcDataInterface.TabIndex = 67;
            // 
            // tabOCRSettings
            // 
            this.tabOCRSettings.Controls.Add(this.groupBox6);
            this.tabOCRSettings.Location = new System.Drawing.Point(4, 22);
            this.tabOCRSettings.Name = "tabOCRSettings";
            this.tabOCRSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabOCRSettings.Size = new System.Drawing.Size(621, 400);
            this.tabOCRSettings.TabIndex = 0;
            this.tabOCRSettings.Text = "OCR";
            this.tabOCRSettings.UseVisualStyleBackColor = true;
            // 
            // tabExternalToolSettings
            // 
            this.tabExternalToolSettings.Controls.Add(this.gbExternalDataInterface);
            this.tabExternalToolSettings.Location = new System.Drawing.Point(4, 22);
            this.tabExternalToolSettings.Name = "tabExternalToolSettings";
            this.tabExternalToolSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabExternalToolSettings.Size = new System.Drawing.Size(621, 400);
            this.tabExternalToolSettings.TabIndex = 1;
            this.tabExternalToolSettings.Text = "ED Market Connector";
            this.tabExternalToolSettings.UseVisualStyleBackColor = true;
            // 
            // gbDataInterface
            // 
            this.gbDataInterface.Controls.Add(this.groupBox2);
            this.gbDataInterface.Controls.Add(this.tcDataInterface);
            this.gbDataInterface.Location = new System.Drawing.Point(3, 3);
            this.gbDataInterface.Name = "gbDataInterface";
            this.gbDataInterface.Size = new System.Drawing.Size(641, 525);
            this.gbDataInterface.TabIndex = 68;
            this.gbDataInterface.TabStop = false;
            this.gbDataInterface.Text = "Data Interface";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbUseExternalTool);
            this.groupBox2.Controls.Add(this.rbUseOCR);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(629, 65);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "SelectedInterface;OCR";
            this.groupBox2.Text = "Select Interface";
            // 
            // rbUseExternalTool
            // 
            this.rbUseExternalTool.AutoSize = true;
            this.rbUseExternalTool.Location = new System.Drawing.Point(24, 38);
            this.rbUseExternalTool.Name = "rbUseExternalTool";
            this.rbUseExternalTool.Size = new System.Drawing.Size(107, 17);
            this.rbUseExternalTool.TabIndex = 69;
            this.rbUseExternalTool.TabStop = true;
            this.rbUseExternalTool.Tag = "External";
            this.rbUseExternalTool.Text = "use External Tool";
            this.rbUseExternalTool.UseVisualStyleBackColor = true;
            this.rbUseExternalTool.CheckedChanged += new System.EventHandler(this.rbInterface_CheckedChanged);
            // 
            // rbUseOCR
            // 
            this.rbUseOCR.AutoSize = true;
            this.rbUseOCR.Location = new System.Drawing.Point(24, 15);
            this.rbUseOCR.Name = "rbUseOCR";
            this.rbUseOCR.Size = new System.Drawing.Size(103, 17);
            this.rbUseOCR.TabIndex = 68;
            this.rbUseOCR.TabStop = true;
            this.rbUseOCR.Tag = "OCR";
            this.rbUseOCR.Text = "use Built-In OCR";
            this.rbUseOCR.UseVisualStyleBackColor = true;
            this.rbUseOCR.CheckedChanged += new System.EventHandler(this.rbInterface_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.cmdChangeSQLPort);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.txtSQLConnectionPort);
            this.groupBox4.Location = new System.Drawing.Point(3, 534);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(641, 63);
            this.groupBox4.TabIndex = 71;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SQL-Server";
            // 
            // cmdChangeSQLPort
            // 
            this.cmdChangeSQLPort.Location = new System.Drawing.Point(139, 22);
            this.cmdChangeSQLPort.Name = "cmdChangeSQLPort";
            this.cmdChangeSQLPort.Size = new System.Drawing.Size(55, 23);
            this.cmdChangeSQLPort.TabIndex = 19;
            this.cmdChangeSQLPort.Text = "Set";
            this.cmdChangeSQLPort.UseVisualStyleBackColor = true;
            this.cmdChangeSQLPort.Click += new System.EventHandler(this.cmdChangeSQLPort_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Port :";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtGamePath);
            this.groupBox3.Controls.Add(this.cmdGamePath);
            this.groupBox3.Location = new System.Drawing.Point(652, 350);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(523, 62);
            this.groupBox3.TabIndex = 69;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Active Game Path (for analysing ED-logfiles)";
            // 
            // txtGamePath
            // 
            this.txtGamePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGamePath.Location = new System.Drawing.Point(11, 14);
            this.txtGamePath.Name = "txtGamePath";
            this.txtGamePath.ReadOnly = true;
            this.txtGamePath.Size = new System.Drawing.Size(498, 20);
            this.txtGamePath.TabIndex = 68;
            this.txtGamePath.Tag = "GamePath;";
            // 
            // cmdGamePath
            // 
            this.cmdGamePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdGamePath.Location = new System.Drawing.Point(407, 36);
            this.cmdGamePath.Name = "cmdGamePath";
            this.cmdGamePath.Size = new System.Drawing.Size(102, 23);
            this.cmdGamePath.TabIndex = 67;
            this.cmdGamePath.Text = "Select";
            this.cmdGamePath.UseVisualStyleBackColor = true;
            this.cmdGamePath.Click += new System.EventHandler(this.cmdGamePath_Click);
            // 
            // tabSettings
            // 
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbDataInterface);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox10);
            this.Name = "tabSettings";
            this.Size = new System.Drawing.Size(1180, 611);
            this.groupBox6.ResumeLayout(false);
            this.gbTesseract.ResumeLayout(false);
            this.gbTesseract.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPurgeOldDataDays)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.gbExternalDataInterface.ResumeLayout(false);
            this.gbExternalDataInterface.PerformLayout();
            this.tcDataInterface.ResumeLayout(false);
            this.tabOCRSettings.ResumeLayout(false);
            this.tabExternalToolSettings.ResumeLayout(false);
            this.gbDataInterface.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button cmdFilter;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBoxInt32 txtGUIColorCutoffLevel;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label52;
        internal System.Windows.Forms.CheckBox cbCheckNextScreenshotForOne;
        private System.Windows.Forms.Label lblPixelAmount;
        private System.Windows.Forms.TextBoxInt32 txtOCRPixelAmount;
        private System.Windows.Forms.Label lblPixelThreshold;
        private System.Windows.Forms.TextBoxDouble txtOCRPixelThreshold;
        private System.Windows.Forms.TextBox txtOCRTraineddataFile;
        private System.Windows.Forms.Button cmdSelectTraineddataFile;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.CheckBox cbAutoAdd_ReplaceVisited;
        private System.Windows.Forms.CheckBox cbAutoAdd_Marketdata;
        private System.Windows.Forms.CheckBox cbAutoAdd_Visited;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.CheckBox cbAutoAdd_JumpedTo;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.NumericUpDown nudPurgeOldDataDays;
        private System.Windows.Forms.Button cmdPurgeOldData;
        private System.Windows.Forms.CheckBox cbAutoActivateSystemTab;
        private System.Windows.Forms.CheckBox cbAutoActivateOCRTab;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBoxInt32 cmbVisitedFilter;
        private System.Windows.Forms.GroupBox gbTesseract;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbExternalDataInterface;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExtTool_ParamMarket;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtExtTool_ParamLocation;
        private System.Windows.Forms.TextBox txtExtTool_Path;
        private System.Windows.Forms.Button cmdSelectExternalToolPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tcDataInterface;
        private System.Windows.Forms.TabPage tabOCRSettings;
        private System.Windows.Forms.TabPage tabExternalToolSettings;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox gbDataInterface;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbUseExternalTool;
        private System.Windows.Forms.RadioButton rbUseOCR;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtGamePath;
        private System.Windows.Forms.Button cmdGamePath;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbNoDistanceToStarConsider;
        private System.Windows.Forms.RadioButton rbNoDistanceToStarIgnore;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rbNoLandingPadSizeConsider;
        private System.Windows.Forms.RadioButton rbNoLandingPadSizeIgnore;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button cmdChangeSQLPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBoxInt32 txtSQLConnectionPort;


    }
}
