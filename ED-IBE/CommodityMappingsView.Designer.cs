namespace IBE
{
    partial class CommodityMappingsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommodityMappingsView));
            this.dgvMappings = new System.Windows.Forms.DataGridView();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.dsEliteDB = new IBE.SQL.Datasets.dsEliteDB();
            this.tbcommoditymappingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tbcommoditymappingTableAdapter = new IBE.SQL.Datasets.dsEliteDBTableAdapters.tbcommoditymappingTableAdapter();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mappedNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcommoditymappingBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMappings
            // 
            this.dgvMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMappings.AutoGenerateColumns = false;
            this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.mappedNameDataGridViewTextBoxColumn});
            this.dgvMappings.DataSource = this.tbcommoditymappingBindingSource;
            this.dgvMappings.Location = new System.Drawing.Point(12, 12);
            this.dgvMappings.Name = "dgvMappings";
            this.dgvMappings.Size = new System.Drawing.Size(607, 310);
            this.dgvMappings.TabIndex = 0;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(625, 51);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(129, 33);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(625, 12);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(129, 33);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // dsEliteDB
            // 
            this.dsEliteDB.DataSetName = "dsEliteDB";
            this.dsEliteDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tbcommoditymappingBindingSource
            // 
            this.tbcommoditymappingBindingSource.DataMember = "tbcommoditymapping";
            this.tbcommoditymappingBindingSource.DataSource = this.dsEliteDB;
            // 
            // tbcommoditymappingTableAdapter
            // 
            this.tbcommoditymappingTableAdapter.ClearBeforeFill = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name (known misspellings)";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // mappedNameDataGridViewTextBoxColumn
            // 
            this.mappedNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mappedNameDataGridViewTextBoxColumn.DataPropertyName = "MappedName";
            this.mappedNameDataGridViewTextBoxColumn.HeaderText = "Mapped Name (well-known commodity)";
            this.mappedNameDataGridViewTextBoxColumn.Name = "mappedNameDataGridViewTextBoxColumn";
            // 
            // CommodityMappingsView
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(766, 334);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.dgvMappings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommodityMappingsView";
            this.Text = "Commodity Mappings";
            this.Load += new System.EventHandler(this.CommodityMappingsView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcommoditymappingBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMappings;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private SQL.Datasets.dsEliteDB dsEliteDB;
        private System.Windows.Forms.BindingSource tbcommoditymappingBindingSource;
        private SQL.Datasets.dsEliteDBTableAdapters.tbcommoditymappingTableAdapter tbcommoditymappingTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mappedNameDataGridViewTextBoxColumn;
    }
}
