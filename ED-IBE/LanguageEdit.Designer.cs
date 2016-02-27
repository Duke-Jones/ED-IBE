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
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdConfirm = new System.Windows.Forms.Button();
            this.groupbox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbOnlyUserlanguage = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdExportCSV = new System.Windows.Forms.Button();
            this.cmdImportFromCSV = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataOwn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbType.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupbox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowDrop = true;
            this.dgvData.AllowUserToAddRows = false;
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
            this.dgvData.Size = new System.Drawing.Size(619, 476);
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
            this.dgvDataOwn.Size = new System.Drawing.Size(616, 476);
            this.dgvDataOwn.TabIndex = 1;
            this.dgvDataOwn.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvData_CellFormatting);
            this.dgvDataOwn.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
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
            this.label1.Size = new System.Drawing.Size(173, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Known items from EDDB";
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
            this.splitContainer1.Location = new System.Drawing.Point(12, 134);
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
            this.splitContainer1.Size = new System.Drawing.Size(1252, 497);
            this.splitContainer1.SplitterDistance = 625;
            this.splitContainer1.TabIndex = 4;
            // 
            // rbCommodities
            // 
            this.rbCommodities.AutoSize = true;
            this.rbCommodities.Location = new System.Drawing.Point(16, 19);
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
            this.gbType.Size = new System.Drawing.Size(132, 109);
            this.gbType.TabIndex = 6;
            this.gbType.TabStop = false;
            this.gbType.Tag = "Type;Commodity";
            this.gbType.Text = "Type";
            // 
            // rbEconomyLevels
            // 
            this.rbEconomyLevels.AutoSize = true;
            this.rbEconomyLevels.Location = new System.Drawing.Point(16, 65);
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
            this.rbCategories.Location = new System.Drawing.Point(16, 42);
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
            this.clbLanguageFilter.Size = new System.Drawing.Size(147, 93);
            this.clbLanguageFilter.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbLanguageFilter);
            this.groupBox1.Location = new System.Drawing.Point(150, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 112);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Language-Filter";
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Location = new System.Drawing.Point(1151, 12);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(113, 23);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.Text = "&Save Changes";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExit.Location = new System.Drawing.Point(1151, 41);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(113, 23);
            this.cmdExit.TabIndex = 10;
            this.cmdExit.Text = "&Close";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdConfirm
            // 
            this.cmdConfirm.Location = new System.Drawing.Point(219, 19);
            this.cmdConfirm.Name = "cmdConfirm";
            this.cmdConfirm.Size = new System.Drawing.Size(183, 51);
            this.cmdConfirm.TabIndex = 11;
            this.cmdConfirm.Text = "Confirm Translation";
            this.toolTip1.SetToolTip(this.cmdConfirm, "If ED-IBE has collected unknown or not yet translated items");
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
            this.groupbox3.Location = new System.Drawing.Point(309, 15);
            this.groupbox3.Name = "groupbox3";
            this.groupbox3.Size = new System.Drawing.Size(408, 106);
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
            this.toolTip1.SetToolTip(this.label3, "If ED-IBE has collected unknown or not yet translated items");
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(15, 53);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(183, 17);
            this.radioButton1.TabIndex = 13;
            this.radioButton1.Text = "for all but base language (english)";
            this.toolTip1.SetToolTip(this.radioButton1, "If ED-IBE has collected unknown or not yet translated items");
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
            this.toolTip1.SetToolTip(this.rbOnlyUserlanguage, "If ED-IBE has collected unknown or not yet translated items");
            this.rbOnlyUserlanguage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdExportCSV);
            this.groupBox2.Controls.Add(this.cmdImportFromCSV);
            this.groupBox2.Location = new System.Drawing.Point(723, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 106);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import/Export (CSV)";
            // 
            // cmdExportCSV
            // 
            this.cmdExportCSV.Location = new System.Drawing.Point(10, 19);
            this.cmdExportCSV.Name = "cmdExportCSV";
            this.cmdExportCSV.Size = new System.Drawing.Size(156, 34);
            this.cmdExportCSV.TabIndex = 0;
            this.cmdExportCSV.Text = "Export data to CSV";
            this.cmdExportCSV.UseVisualStyleBackColor = true;
            this.cmdExportCSV.Click += new System.EventHandler(this.cmdExportCSV_Click);
            // 
            // cmdImportFromCSV
            // 
            this.cmdImportFromCSV.Location = new System.Drawing.Point(10, 59);
            this.cmdImportFromCSV.Name = "cmdImportFromCSV";
            this.cmdImportFromCSV.Size = new System.Drawing.Size(156, 34);
            this.cmdImportFromCSV.TabIndex = 3;
            this.cmdImportFromCSV.Text = "Import data from CSV";
            this.cmdImportFromCSV.UseVisualStyleBackColor = true;
            this.cmdImportFromCSV.Click += new System.EventHandler(this.cmdImportFromCSV_Click);
            // 
            // LanguageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1276, 643);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupbox3);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbType);
            this.Controls.Add(this.splitContainer1);
            this.Name = "LanguageEdit";
            this.Text = "Language-Edit";
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
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.Button cmdConfirm;
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
        private System.Windows.Forms.Button cmdExportCSV;
        private System.Windows.Forms.Button cmdImportFromCSV;
    }
}
