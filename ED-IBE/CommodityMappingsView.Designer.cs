using IBE.Enums_and_Utility_Classes;

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
            this.dsEliteDB = new IBE.SQL.Datasets.dsEliteDB();
            this.tbdnmapcommodityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tbdnmap_commodityTableAdapter = new IBE.SQL.Datasets.dsEliteDBTableAdapters.tbdnmap_commodityTableAdapter();
            this.cmdOK = new System.Windows.Forms.ButtonExt();
            this.cmdCancel = new System.Windows.Forms.ButtonExt();
            this.dgvMappings = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.companionNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companionAdditionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameAdditionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbdnmapcommodityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
            this.SuspendLayout();
            // 
            // dsEliteDB
            // 
            this.dsEliteDB.DataSetName = "dsEliteDB";
            this.dsEliteDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tbdnmapcommodityBindingSource
            // 
            this.tbdnmapcommodityBindingSource.DataMember = "tbdnmap_commodity";
            this.tbdnmapcommodityBindingSource.DataSource = this.dsEliteDB;
            // 
            // tbdnmap_commodityTableAdapter
            // 
            this.tbdnmap_commodityTableAdapter.ClearBeforeFill = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdOK.Location = new System.Drawing.Point(625, 12);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(129, 33);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.DisabledTextColor = System.Drawing.Color.DimGray;
            this.cmdCancel.Location = new System.Drawing.Point(625, 51);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(129, 33);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // dgvMappings
            // 
            this.dgvMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMappings.AutoGenerateColumns = false;
            this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.companionNameDataGridViewTextBoxColumn,
            this.companionAdditionDataGridViewTextBoxColumn,
            this.gameNameDataGridViewTextBoxColumn,
            this.gameAdditionDataGridViewTextBoxColumn});
            this.dgvMappings.DataSource = this.tbdnmapcommodityBindingSource;
            this.dgvMappings.DoubleBuffer = true;
            this.dgvMappings.Location = new System.Drawing.Point(12, 12);
            this.dgvMappings.Name = "dgvMappings";
            this.dgvMappings.Size = new System.Drawing.Size(607, 310);
            this.dgvMappings.TabIndex = 0;
            this.dgvMappings.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvMappings_DefaultValuesNeeded);
            // 
            // companionNameDataGridViewTextBoxColumn
            // 
            this.companionNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.companionNameDataGridViewTextBoxColumn.DataPropertyName = "CompanionName";
            this.companionNameDataGridViewTextBoxColumn.HeaderText = "Companion-Name (or misspelling)";
            this.companionNameDataGridViewTextBoxColumn.Name = "companionNameDataGridViewTextBoxColumn";
            // 
            // companionAdditionDataGridViewTextBoxColumn
            // 
            this.companionAdditionDataGridViewTextBoxColumn.DataPropertyName = "CompanionAddition";
            this.companionAdditionDataGridViewTextBoxColumn.HeaderText = "CompanionAddition";
            this.companionAdditionDataGridViewTextBoxColumn.Name = "companionAdditionDataGridViewTextBoxColumn";
            this.companionAdditionDataGridViewTextBoxColumn.Visible = false;
            // 
            // gameNameDataGridViewTextBoxColumn
            // 
            this.gameNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.gameNameDataGridViewTextBoxColumn.DataPropertyName = "GameName";
            this.gameNameDataGridViewTextBoxColumn.HeaderText = "Game-Name";
            this.gameNameDataGridViewTextBoxColumn.Name = "gameNameDataGridViewTextBoxColumn";
            // 
            // gameAdditionDataGridViewTextBoxColumn
            // 
            this.gameAdditionDataGridViewTextBoxColumn.DataPropertyName = "GameAddition";
            this.gameAdditionDataGridViewTextBoxColumn.HeaderText = "GameAddition";
            this.gameAdditionDataGridViewTextBoxColumn.Name = "gameAdditionDataGridViewTextBoxColumn";
            this.gameAdditionDataGridViewTextBoxColumn.Visible = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbdnmapcommodityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewExt dgvMappings;
        private System.Windows.Forms.ButtonExt cmdCancel;
        private System.Windows.Forms.ButtonExt cmdOK;
        private SQL.Datasets.dsEliteDB dsEliteDB;
        private SQL.Datasets.dsEliteDBTableAdapters.tbdnmap_commodityTableAdapter tbdnmap_commodityTableAdapter;
        private System.Windows.Forms.BindingSource tbdnmapcommodityBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn companionNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companionAdditionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gameNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gameAdditionDataGridViewTextBoxColumn;
    }
}
