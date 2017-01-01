using IBE.Enums_and_Utility_Classes;
namespace IBE
{
    partial class LanguageEdit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageEdit));
            this.dgvData = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.column_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_language_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDataOwn = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.column_id2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_language_id2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_name2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rbCommodities = new System.Windows.Forms.RadioButton();
            this.gbType = new System.Windows.Forms.GroupBox();
            this.rbEconomyLevels = new System.Windows.Forms.RadioButton();
            this.rbCategories = new System.Windows.Forms.RadioButton();
            this.clbLanguageFilter = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdSave = new System.Windows.Forms.ButtonExt();
            this.cmdExit = new System.Windows.Forms.ButtonExt();
            this.cmdConfirm = new System.Windows.Forms.ButtonExt();
            this.groupbox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbOnlyUserlanguage = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdExportCSV = new System.Windows.Forms.ButtonExt();
            this.cmdImportFromCSV = new System.Windows.Forms.ButtonExt();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbImportIntelligent = new System.Windows.Forms.RadioButton();
            this.rbImportOverwriteButBase = new System.Windows.Forms.RadioButton();
            this.rbImportOnlyNew = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cmdCorrectSpelling = new System.Windows.Forms.ButtonExt();
            this.cmdMappings = new System.Windows.Forms.ButtonExt();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataOwn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbType.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupbox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowDrop = true;
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToOrderColumns = true;
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_id,
            this.column_language_id,
            this.column_name});
            this.dgvData.DoubleBuffer = true;
            this.dgvData.Location = new System.Drawing.Point(3, 17);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(476, 429);
            this.dgvData.TabIndex = 0;
            this.dgvData.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvData_CellFormatting);
            this.dgvData.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellValueChanged);
            this.dgvData.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
            this.dgvData.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvData_DragDrop);
            this.dgvData.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvData_DragEnter);
            // 
            // column_id
            // 
            this.column_id.DataPropertyName = "id1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.NullValue = null;
            this.column_id.DefaultCellStyle = dataGridViewCellStyle1;
            this.column_id.HeaderText = "ID";
            this.column_id.Name = "column_id";
            this.column_id.ReadOnly = true;
            this.column_id.Width = 50;
            // 
            // column_language_id
            // 
            this.column_language_id.DataPropertyName = "id2";
            this.column_language_id.HeaderText = "Language";
            this.column_language_id.Name = "column_language_id";
            this.column_language_id.ReadOnly = true;
            // 
            // column_name
            // 
            this.column_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_name.DataPropertyName = "name";
            this.column_name.HeaderText = "Name";
            this.column_name.Name = "column_name";
            // 
            // dgvDataOwn
            // 
            this.dgvDataOwn.AllowUserToAddRows = false;
            this.dgvDataOwn.AllowUserToOrderColumns = true;
            this.dgvDataOwn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataOwn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataOwn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_id2,
            this.column_language_id2,
            this.column_name2});
            this.dgvDataOwn.DoubleBuffer = true;
            this.dgvDataOwn.Location = new System.Drawing.Point(3, 17);
            this.dgvDataOwn.Name = "dgvDataOwn";
            this.dgvDataOwn.Size = new System.Drawing.Size(475, 429);
            this.dgvDataOwn.TabIndex = 1;
            this.dgvDataOwn.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvData_CellFormatting);
            this.dgvDataOwn.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellValueChanged);
            this.dgvDataOwn.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvDataOwn_RowsRemoved);
            this.dgvDataOwn.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
            this.dgvDataOwn.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvDataOwn_UserDeletingRow);
            this.dgvDataOwn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvDataOwn_MouseMove);
            // 
            // column_id2
            // 
            this.column_id2.DataPropertyName = "id1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.NullValue = null;
            this.column_id2.DefaultCellStyle = dataGridViewCellStyle2;
            this.column_id2.HeaderText = "ID";
            this.column_id2.Name = "column_id2";
            this.column_id2.ReadOnly = true;
            this.column_id2.Width = 50;
            // 
            // column_language_id2
            // 
            this.column_language_id2.DataPropertyName = "id2";
            this.column_language_id2.HeaderText = "Language ID";
            this.column_language_id2.Name = "column_language_id2";
            this.column_language_id2.ReadOnly = true;
            // 
            // column_name2
            // 
            this.column_name2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_name2.DataPropertyName = "name";
            this.column_name2.HeaderText = "Name";
            this.column_name2.Name = "column_name2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Well-known items from EDDB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Self-added items";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 276);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvData);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.dgvDataOwn);
            this.splitContainer1.Size = new System.Drawing.Size(968, 450);
            this.splitContainer1.SplitterDistance = 482;
            this.splitContainer1.TabIndex = 4;
            // 
            // rbCommodities
            // 
            this.rbCommodities.AutoSize = true;
            this.rbCommodities.Location = new System.Drawing.Point(33, 24);
            this.rbCommodities.Name = "rbCommodities";
            this.rbCommodities.Size = new System.Drawing.Size(76, 17);
            this.rbCommodities.TabIndex = 5;
            this.rbCommodities.TabStop = true;
            this.rbCommodities.Tag = "Commodity";
            this.rbCommodities.Text = "Commodity";
            this.rbCommodities.UseVisualStyleBackColor = true;
            // 
            // gbType
            // 
            this.gbType.Controls.Add(this.rbEconomyLevels);
            this.gbType.Controls.Add(this.rbCategories);
            this.gbType.Controls.Add(this.rbCommodities);
            this.gbType.Location = new System.Drawing.Point(12, 12);
            this.gbType.Name = "gbType";
            this.gbType.Size = new System.Drawing.Size(150, 108);
            this.gbType.TabIndex = 6;
            this.gbType.TabStop = false;
            this.gbType.Tag = "Type;Commodity";
            this.gbType.Text = "Type";
            // 
            // rbEconomyLevels
            // 
            this.rbEconomyLevels.AutoSize = true;
            this.rbEconomyLevels.Location = new System.Drawing.Point(33, 70);
            this.rbEconomyLevels.Name = "rbEconomyLevels";
            this.rbEconomyLevels.Size = new System.Drawing.Size(91, 17);
            this.rbEconomyLevels.TabIndex = 7;
            this.rbEconomyLevels.TabStop = true;
            this.rbEconomyLevels.Tag = "Economylevel";
            this.rbEconomyLevels.Text = "Economylevel";
            this.rbEconomyLevels.UseVisualStyleBackColor = true;
            // 
            // rbCategories
            // 
            this.rbCategories.AutoSize = true;
            this.rbCategories.Location = new System.Drawing.Point(33, 47);
            this.rbCategories.Name = "rbCategories";
            this.rbCategories.Size = new System.Drawing.Size(67, 17);
            this.rbCategories.TabIndex = 6;
            this.rbCategories.TabStop = true;
            this.rbCategories.Tag = "Category";
            this.rbCategories.Text = "Category";
            this.rbCategories.UseVisualStyleBackColor = true;
            // 
            // clbLanguageFilter
            // 
            this.clbLanguageFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbLanguageFilter.FormattingEnabled = true;
            this.clbLanguageFilter.Location = new System.Drawing.Point(3, 16);
            this.clbLanguageFilter.Name = "clbLanguageFilter";
            this.clbLanguageFilter.Size = new System.Drawing.Size(147, 89);
            this.clbLanguageFilter.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbLanguageFilter);
            this.groupBox1.Location = new System.Drawing.Point(165, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 108);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Language-Filter";
            // 
            // cmdSave
            // 
            this.cmdSave.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdSave.Location = new System.Drawing.Point(11, 19);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(156, 34);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.Text = "&Save Changes";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdExit.Location = new System.Drawing.Point(11, 59);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(156, 34);
            this.cmdExit.TabIndex = 10;
            this.cmdExit.Text = "&Close";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdConfirm
            // 
            this.cmdConfirm.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdConfirm.Location = new System.Drawing.Point(216, 28);
            this.cmdConfirm.Name = "cmdConfirm";
            this.cmdConfirm.Size = new System.Drawing.Size(156, 34);
            this.cmdConfirm.TabIndex = 11;
            this.cmdConfirm.Text = "Confirm Translation";
            this.toolTip1.SetToolTip(this.cmdConfirm, resources.GetString("cmdConfirm.ToolTip"));
            this.cmdConfirm.UseVisualStyleBackColor = true;
            this.cmdConfirm.Click += new System.EventHandler(this.cmdConfirm_Click);
            // 
            // groupbox3
            // 
            this.groupbox3.Controls.Add(this.label4);
            this.groupbox3.Controls.Add(this.label3);
            this.groupbox3.Controls.Add(this.radioButton1);
            this.groupbox3.Controls.Add(this.rbOnlyUserlanguage);
            this.groupbox3.Controls.Add(this.cmdConfirm);
            this.groupbox3.Location = new System.Drawing.Point(12, 126);
            this.groupbox3.Name = "groupbox3";
            this.groupbox3.Size = new System.Drawing.Size(390, 79);
            this.groupbox3.TabIndex = 12;
            this.groupbox3.TabStop = false;
            this.groupbox3.Text = "Assign self-added items to EDDB items";
            this.toolTip1.SetToolTip(this.groupbox3, resources.GetString("groupbox3.ToolTip"));
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Confirmation of translation applies to";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(15, 53);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(183, 17);
            this.radioButton1.TabIndex = 13;
            this.radioButton1.Text = "for all but base language (english)";
            this.toolTip1.SetToolTip(this.radioButton1, "Concatenate all but the base name (=english name)\r\nto the selected \"known name\" o" +
        "n the left side.\r\n");
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // rbOnlyUserlanguage
            // 
            this.rbOnlyUserlanguage.AutoSize = true;
            this.rbOnlyUserlanguage.Checked = true;
            this.rbOnlyUserlanguage.Location = new System.Drawing.Point(15, 33);
            this.rbOnlyUserlanguage.Name = "rbOnlyUserlanguage";
            this.rbOnlyUserlanguage.Size = new System.Drawing.Size(129, 17);
            this.rbOnlyUserlanguage.TabIndex = 12;
            this.rbOnlyUserlanguage.TabStop = true;
            this.rbOnlyUserlanguage.Text = "only for user language";
            this.toolTip1.SetToolTip(this.rbOnlyUserlanguage, "Concatenate only the userdefined string *in your selected language*\r\nto the selec" +
        "ted \"known name\" on the left side.\r\nThe strings in other languages will be left " +
        "unchanged.");
            this.rbOnlyUserlanguage.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // cmdExportCSV
            // 
            this.cmdExportCSV.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdExportCSV.Location = new System.Drawing.Point(10, 19);
            this.cmdExportCSV.Name = "cmdExportCSV";
            this.cmdExportCSV.Size = new System.Drawing.Size(156, 34);
            this.cmdExportCSV.TabIndex = 0;
            this.cmdExportCSV.Text = "Export data to CSV";
            this.toolTip1.SetToolTip(this.cmdExportCSV, "Exports the localization strings into a csv-file.");
            this.cmdExportCSV.UseVisualStyleBackColor = true;
            this.cmdExportCSV.Click += new System.EventHandler(this.cmdExportCSV_Click);
            // 
            // cmdImportFromCSV
            // 
            this.cmdImportFromCSV.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdImportFromCSV.Location = new System.Drawing.Point(10, 59);
            this.cmdImportFromCSV.Name = "cmdImportFromCSV";
            this.cmdImportFromCSV.Size = new System.Drawing.Size(156, 34);
            this.cmdImportFromCSV.TabIndex = 3;
            this.cmdImportFromCSV.Text = "Import data from CSV";
            this.toolTip1.SetToolTip(this.cmdImportFromCSV, resources.GetString("cmdImportFromCSV.ToolTip"));
            this.cmdImportFromCSV.UseVisualStyleBackColor = true;
            this.cmdImportFromCSV.Click += new System.EventHandler(this.cmdImportFromCSV_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbImportIntelligent);
            this.groupBox4.Controls.Add(this.rbImportOverwriteButBase);
            this.groupBox4.Controls.Add(this.rbImportOnlyNew);
            this.groupBox4.Location = new System.Drawing.Point(10, 99);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(156, 83);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Tag = "";
            this.groupBox4.Text = "Import Type";
            this.toolTip1.SetToolTip(this.groupBox4, "Handling of already existing names.");
            this.groupBox4.Visible = false;
            // 
            // rbImportIntelligent
            // 
            this.rbImportIntelligent.AutoSize = true;
            this.rbImportIntelligent.Checked = true;
            this.rbImportIntelligent.Location = new System.Drawing.Point(9, 61);
            this.rbImportIntelligent.Name = "rbImportIntelligent";
            this.rbImportIntelligent.Size = new System.Drawing.Size(70, 17);
            this.rbImportIntelligent.TabIndex = 7;
            this.rbImportIntelligent.TabStop = true;
            this.rbImportIntelligent.Tag = "";
            this.rbImportIntelligent.Text = "Intelligent";
            this.toolTip1.SetToolTip(this.rbImportIntelligent, resources.GetString("rbImportIntelligent.ToolTip"));
            this.rbImportIntelligent.UseVisualStyleBackColor = true;
            // 
            // rbImportOverwriteButBase
            // 
            this.rbImportOverwriteButBase.AutoSize = true;
            this.rbImportOverwriteButBase.Location = new System.Drawing.Point(9, 44);
            this.rbImportOverwriteButBase.Name = "rbImportOverwriteButBase";
            this.rbImportOverwriteButBase.Size = new System.Drawing.Size(140, 17);
            this.rbImportOverwriteButBase.TabIndex = 6;
            this.rbImportOverwriteButBase.Tag = "";
            this.rbImportOverwriteButBase.Text = "Overwrite All But English";
            this.toolTip1.SetToolTip(this.rbImportOverwriteButBase, "Handling of already existing names: \r\nImport will only happen, if not existing ye" +
        "t or the string is from a non-base language.\r\n(\"base language\" is english as glo" +
        "bal identifier)\r\n");
            this.rbImportOverwriteButBase.UseVisualStyleBackColor = true;
            // 
            // rbImportOnlyNew
            // 
            this.rbImportOnlyNew.AutoSize = true;
            this.rbImportOnlyNew.Location = new System.Drawing.Point(9, 27);
            this.rbImportOnlyNew.Name = "rbImportOnlyNew";
            this.rbImportOnlyNew.Size = new System.Drawing.Size(103, 17);
            this.rbImportOnlyNew.TabIndex = 5;
            this.rbImportOnlyNew.Tag = "";
            this.rbImportOnlyNew.Text = "Import Only New";
            this.toolTip1.SetToolTip(this.rbImportOnlyNew, "Handling of already existing names: \r\nImport will only happen, if not existing ye" +
        "t.");
            this.rbImportOnlyNew.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cmdCorrectSpelling);
            this.groupBox6.Location = new System.Drawing.Point(408, 126);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(198, 79);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Correct misspelled commodities";
            this.toolTip1.SetToolTip(this.groupBox6, resources.GetString("groupBox6.ToolTip"));
            // 
            // cmdCorrectSpelling
            // 
            this.cmdCorrectSpelling.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdCorrectSpelling.Location = new System.Drawing.Point(15, 28);
            this.cmdCorrectSpelling.Name = "cmdCorrectSpelling";
            this.cmdCorrectSpelling.Size = new System.Drawing.Size(156, 34);
            this.cmdCorrectSpelling.TabIndex = 11;
            this.cmdCorrectSpelling.Text = "Assign to existing";
            this.toolTip1.SetToolTip(this.cmdCorrectSpelling, resources.GetString("cmdCorrectSpelling.ToolTip"));
            this.cmdCorrectSpelling.UseVisualStyleBackColor = true;
            this.cmdCorrectSpelling.Click += new System.EventHandler(this.cmdCorrectSpelling_Click);
            // 
            // cmdMappings
            // 
            this.cmdMappings.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdMappings.Location = new System.Drawing.Point(11, 135);
            this.cmdMappings.Name = "cmdMappings";
            this.cmdMappings.Size = new System.Drawing.Size(156, 34);
            this.cmdMappings.TabIndex = 11;
            this.cmdMappings.Text = "Show Mappings";
            this.toolTip1.SetToolTip(this.cmdMappings, "Shows the current mappings for misspelled commodity names (for auto-correction on" +
        " import of market data)");
            this.cmdMappings.UseVisualStyleBackColor = true;
            this.cmdMappings.Click += new System.EventHandler(this.cmdMappings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.cmdExportCSV);
            this.groupBox2.Controls.Add(this.cmdImportFromCSV);
            this.groupBox2.Location = new System.Drawing.Point(614, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 190);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import/Export (CSV)";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.cmdMappings);
            this.groupBox5.Controls.Add(this.cmdExit);
            this.groupBox5.Controls.Add(this.cmdSave);
            this.groupBox5.Location = new System.Drawing.Point(803, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(173, 190);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(244, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(504, 65);
            this.label5.TabIndex = 15;
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // LanguageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(992, 738);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupbox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbType);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1008, 776);
            this.Name = "LanguageEdit";
            this.Text = "Localization-Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LanguageEdit_FormClosing);
            this.Load += new System.EventHandler(this.LanguageEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataOwn)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbType.ResumeLayout(false);
            this.gbType.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupbox3.ResumeLayout(false);
            this.groupbox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewExt dgvData;
        private DataGridViewExt dgvDataOwn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton rbCommodities;
        private System.Windows.Forms.GroupBox gbType;
        private System.Windows.Forms.RadioButton rbEconomyLevels;
        private System.Windows.Forms.RadioButton rbCategories;
        private System.Windows.Forms.CheckedListBox clbLanguageFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ButtonExt cmdSave;
        private System.Windows.Forms.ButtonExt cmdExit;
        private System.Windows.Forms.ButtonExt cmdConfirm;
        private System.Windows.Forms.GroupBox groupbox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton rbOnlyUserlanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_language_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_id2;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_language_id2;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_name2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ButtonExt cmdExportCSV;
        private System.Windows.Forms.ButtonExt cmdImportFromCSV;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbImportOverwriteButBase;
        private System.Windows.Forms.RadioButton rbImportOnlyNew;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ButtonExt cmdCorrectSpelling;
        private System.Windows.Forms.ButtonExt cmdMappings;
        private System.Windows.Forms.RadioButton rbImportIntelligent;
    }
}
