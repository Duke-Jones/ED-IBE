namespace RegulatedNoise.MTSettings
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
            this.txtOCRTraineddataFile = new System.Windows.Forms.TextBox();
            this.cmdSelectTraineddataFile = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
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
            this.groupBox6.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPurgeOldDataDays)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox9);
            this.groupBox6.Controls.Add(this.groupBox11);
            this.groupBox6.Controls.Add(this.txtOCRTraineddataFile);
            this.groupBox6.Controls.Add(this.cmdSelectTraineddataFile);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(641, 408);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "OCR-Settings";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.cmdFilter);
            this.groupBox9.Controls.Add(this.label50);
            this.groupBox9.Controls.Add(this.label51);
            this.groupBox9.Controls.Add(this.txtGUIColorCutoffLevel);
            this.groupBox9.Location = new System.Drawing.Point(11, 210);
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
            this.groupBox11.Location = new System.Drawing.Point(11, 63);
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
            // txtOCRTraineddataFile
            // 
            this.txtOCRTraineddataFile.Location = new System.Drawing.Point(117, 36);
            this.txtOCRTraineddataFile.Name = "txtOCRTraineddataFile";
            this.txtOCRTraineddataFile.ReadOnly = true;
            this.txtOCRTraineddataFile.Size = new System.Drawing.Size(197, 20);
            this.txtOCRTraineddataFile.TabIndex = 13;
            this.txtOCRTraineddataFile.Tag = "OCRTraineddataFile;big";
            // 
            // cmdSelectTraineddataFile
            // 
            this.cmdSelectTraineddataFile.Location = new System.Drawing.Point(361, 25);
            this.cmdSelectTraineddataFile.Name = "cmdSelectTraineddataFile";
            this.cmdSelectTraineddataFile.Size = new System.Drawing.Size(176, 23);
            this.cmdSelectTraineddataFile.TabIndex = 8;
            this.cmdSelectTraineddataFile.Text = "Select";
            this.cmdSelectTraineddataFile.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(137, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(177, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "\"traineddata\"-File for TesseractOCR";
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
            this.groupBox10.Size = new System.Drawing.Size(396, 151);
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
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox8.Controls.Add(this.label48);
            this.groupBox8.Controls.Add(this.cmbLanguage);
            this.groupBox8.Location = new System.Drawing.Point(3, 417);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(363, 45);
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
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
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
            this.groupBox12.Location = new System.Drawing.Point(650, 160);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(396, 429);
            this.groupBox12.TabIndex = 14;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Other";
            // 
            // label89
            // 
            this.label89.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(203, 346);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(29, 13);
            this.label89.TabIndex = 19;
            this.label89.Text = "days";
            // 
            // nudPurgeOldDataDays
            // 
            this.nudPurgeOldDataDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudPurgeOldDataDays.Location = new System.Drawing.Point(154, 343);
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
            this.cmdPurgeOldData.Location = new System.Drawing.Point(32, 341);
            this.cmdPurgeOldData.Name = "cmdPurgeOldData";
            this.cmdPurgeOldData.Size = new System.Drawing.Size(116, 23);
            this.cmdPurgeOldData.TabIndex = 8;
            this.cmdPurgeOldData.Text = "purge data older than";
            this.cmdPurgeOldData.UseVisualStyleBackColor = true;
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
            this.button6.Location = new System.Drawing.Point(32, 377);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(226, 23);
            this.button6.TabIndex = 2;
            this.button6.Text = "Edit Commodity Price Warn levels";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox6);
            this.Name = "tabSettings";
            this.Size = new System.Drawing.Size(1049, 592);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
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


    }
}
