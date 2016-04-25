namespace IBE
{
    partial class CommoditySelector
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommoditySelector));
            this.dgvCommodities = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commodityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loccommodityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.averagepriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwldemandbuylowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwldemandbuyhighDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwlsupplybuylowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwlsupplybuyhighDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwldemandselllowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwldemandsellhighDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwlsupplyselllowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pwlsupplysellhighDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.israreDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.is_Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tbcommodityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsEliteDB = new IBE.SQL.Datasets.dsEliteDB();
            this.tbcommodityTableAdapter = new IBE.SQL.Datasets.dsEliteDBTableAdapters.tbcommodityTableAdapter();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommodities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcommodityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCommodities
            // 
            this.dgvCommodities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCommodities.AutoGenerateColumns = false;
            this.dgvCommodities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCommodities.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.commodityDataGridViewTextBoxColumn,
            this.loccommodityDataGridViewTextBoxColumn,
            this.categoryidDataGridViewTextBoxColumn,
            this.averagepriceDataGridViewTextBoxColumn,
            this.pwldemandbuylowDataGridViewTextBoxColumn,
            this.pwldemandbuyhighDataGridViewTextBoxColumn,
            this.pwlsupplybuylowDataGridViewTextBoxColumn,
            this.pwlsupplybuyhighDataGridViewTextBoxColumn,
            this.pwldemandselllowDataGridViewTextBoxColumn,
            this.pwldemandsellhighDataGridViewTextBoxColumn,
            this.pwlsupplyselllowDataGridViewTextBoxColumn,
            this.pwlsupplysellhighDataGridViewTextBoxColumn,
            this.israreDataGridViewCheckBoxColumn,
            this.is_Selected});
            this.dgvCommodities.DataSource = this.tbcommodityBindingSource;
            this.dgvCommodities.DoubleBuffer = true;
            this.dgvCommodities.Location = new System.Drawing.Point(12, 12);
            this.dgvCommodities.MultiSelect = false;
            this.dgvCommodities.Name = "dgvCommodities";
            this.dgvCommodities.RowTemplate.ReadOnly = true;
            this.dgvCommodities.Size = new System.Drawing.Size(467, 416);
            this.dgvCommodities.TabIndex = 0;
            this.dgvCommodities.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCommodities_CellContentClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.idDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 40;
            // 
            // commodityDataGridViewTextBoxColumn
            // 
            this.commodityDataGridViewTextBoxColumn.DataPropertyName = "commodity";
            this.commodityDataGridViewTextBoxColumn.HeaderText = "commodity";
            this.commodityDataGridViewTextBoxColumn.Name = "commodityDataGridViewTextBoxColumn";
            this.commodityDataGridViewTextBoxColumn.ReadOnly = true;
            this.commodityDataGridViewTextBoxColumn.Visible = false;
            // 
            // loccommodityDataGridViewTextBoxColumn
            // 
            this.loccommodityDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.loccommodityDataGridViewTextBoxColumn.DataPropertyName = "loccommodity";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.loccommodityDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.loccommodityDataGridViewTextBoxColumn.HeaderText = "Name";
            this.loccommodityDataGridViewTextBoxColumn.Name = "loccommodityDataGridViewTextBoxColumn";
            this.loccommodityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // categoryidDataGridViewTextBoxColumn
            // 
            this.categoryidDataGridViewTextBoxColumn.DataPropertyName = "category_id";
            this.categoryidDataGridViewTextBoxColumn.HeaderText = "category_id";
            this.categoryidDataGridViewTextBoxColumn.Name = "categoryidDataGridViewTextBoxColumn";
            this.categoryidDataGridViewTextBoxColumn.ReadOnly = true;
            this.categoryidDataGridViewTextBoxColumn.Visible = false;
            // 
            // averagepriceDataGridViewTextBoxColumn
            // 
            this.averagepriceDataGridViewTextBoxColumn.DataPropertyName = "average_price";
            this.averagepriceDataGridViewTextBoxColumn.HeaderText = "average_price";
            this.averagepriceDataGridViewTextBoxColumn.Name = "averagepriceDataGridViewTextBoxColumn";
            this.averagepriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.averagepriceDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwldemandbuylowDataGridViewTextBoxColumn
            // 
            this.pwldemandbuylowDataGridViewTextBoxColumn.DataPropertyName = "pwl_demand_buy_low";
            this.pwldemandbuylowDataGridViewTextBoxColumn.HeaderText = "pwl_demand_buy_low";
            this.pwldemandbuylowDataGridViewTextBoxColumn.Name = "pwldemandbuylowDataGridViewTextBoxColumn";
            this.pwldemandbuylowDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwldemandbuylowDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwldemandbuyhighDataGridViewTextBoxColumn
            // 
            this.pwldemandbuyhighDataGridViewTextBoxColumn.DataPropertyName = "pwl_demand_buy_high";
            this.pwldemandbuyhighDataGridViewTextBoxColumn.HeaderText = "pwl_demand_buy_high";
            this.pwldemandbuyhighDataGridViewTextBoxColumn.Name = "pwldemandbuyhighDataGridViewTextBoxColumn";
            this.pwldemandbuyhighDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwldemandbuyhighDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwlsupplybuylowDataGridViewTextBoxColumn
            // 
            this.pwlsupplybuylowDataGridViewTextBoxColumn.DataPropertyName = "pwl_supply_buy_low";
            this.pwlsupplybuylowDataGridViewTextBoxColumn.HeaderText = "pwl_supply_buy_low";
            this.pwlsupplybuylowDataGridViewTextBoxColumn.Name = "pwlsupplybuylowDataGridViewTextBoxColumn";
            this.pwlsupplybuylowDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwlsupplybuylowDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwlsupplybuyhighDataGridViewTextBoxColumn
            // 
            this.pwlsupplybuyhighDataGridViewTextBoxColumn.DataPropertyName = "pwl_supply_buy_high";
            this.pwlsupplybuyhighDataGridViewTextBoxColumn.HeaderText = "pwl_supply_buy_high";
            this.pwlsupplybuyhighDataGridViewTextBoxColumn.Name = "pwlsupplybuyhighDataGridViewTextBoxColumn";
            this.pwlsupplybuyhighDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwlsupplybuyhighDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwldemandselllowDataGridViewTextBoxColumn
            // 
            this.pwldemandselllowDataGridViewTextBoxColumn.DataPropertyName = "pwl_demand_sell_low";
            this.pwldemandselllowDataGridViewTextBoxColumn.HeaderText = "pwl_demand_sell_low";
            this.pwldemandselllowDataGridViewTextBoxColumn.Name = "pwldemandselllowDataGridViewTextBoxColumn";
            this.pwldemandselllowDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwldemandselllowDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwldemandsellhighDataGridViewTextBoxColumn
            // 
            this.pwldemandsellhighDataGridViewTextBoxColumn.DataPropertyName = "pwl_demand_sell_high";
            this.pwldemandsellhighDataGridViewTextBoxColumn.HeaderText = "pwl_demand_sell_high";
            this.pwldemandsellhighDataGridViewTextBoxColumn.Name = "pwldemandsellhighDataGridViewTextBoxColumn";
            this.pwldemandsellhighDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwldemandsellhighDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwlsupplyselllowDataGridViewTextBoxColumn
            // 
            this.pwlsupplyselllowDataGridViewTextBoxColumn.DataPropertyName = "pwl_supply_sell_low";
            this.pwlsupplyselllowDataGridViewTextBoxColumn.HeaderText = "pwl_supply_sell_low";
            this.pwlsupplyselllowDataGridViewTextBoxColumn.Name = "pwlsupplyselllowDataGridViewTextBoxColumn";
            this.pwlsupplyselllowDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwlsupplyselllowDataGridViewTextBoxColumn.Visible = false;
            // 
            // pwlsupplysellhighDataGridViewTextBoxColumn
            // 
            this.pwlsupplysellhighDataGridViewTextBoxColumn.DataPropertyName = "pwl_supply_sell_high";
            this.pwlsupplysellhighDataGridViewTextBoxColumn.HeaderText = "pwl_supply_sell_high";
            this.pwlsupplysellhighDataGridViewTextBoxColumn.Name = "pwlsupplysellhighDataGridViewTextBoxColumn";
            this.pwlsupplysellhighDataGridViewTextBoxColumn.ReadOnly = true;
            this.pwlsupplysellhighDataGridViewTextBoxColumn.Visible = false;
            // 
            // israreDataGridViewCheckBoxColumn
            // 
            this.israreDataGridViewCheckBoxColumn.DataPropertyName = "is_rare";
            this.israreDataGridViewCheckBoxColumn.HeaderText = "is_rare";
            this.israreDataGridViewCheckBoxColumn.Name = "israreDataGridViewCheckBoxColumn";
            this.israreDataGridViewCheckBoxColumn.ReadOnly = true;
            this.israreDataGridViewCheckBoxColumn.Visible = false;
            // 
            // is_Selected
            // 
            this.is_Selected.FalseValue = "0";
            this.is_Selected.HeaderText = "Select";
            this.is_Selected.Name = "is_Selected";
            this.is_Selected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_Selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_Selected.TrueValue = "1";
            this.is_Selected.Width = 50;
            // 
            // tbcommodityBindingSource
            // 
            this.tbcommodityBindingSource.DataMember = "tbcommodity";
            this.tbcommodityBindingSource.DataSource = this.dsEliteDB;
            // 
            // dsEliteDB
            // 
            this.dsEliteDB.DataSetName = "dsEliteDB";
            this.dsEliteDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tbcommodityTableAdapter
            // 
            this.tbcommodityTableAdapter.ClearBeforeFill = true;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(495, 12);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(129, 33);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(495, 51);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(129, 33);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // CommoditySelector
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(636, 444);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.dgvCommodities);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommoditySelector";
            this.Text = "Commodities";
            this.Load += new System.EventHandler(this.CommoditySelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommodities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcommodityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private IBE.Enums_and_Utility_Classes.DataGridViewExt dgvCommodities;
        private SQL.Datasets.dsEliteDB dsEliteDB;
        private System.Windows.Forms.BindingSource tbcommodityBindingSource;
        private SQL.Datasets.dsEliteDBTableAdapters.tbcommodityTableAdapter tbcommodityTableAdapter;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commodityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loccommodityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn averagepriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwldemandbuylowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwldemandbuyhighDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwlsupplybuylowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwlsupplybuyhighDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwldemandselllowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwldemandsellhighDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwlsupplyselllowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pwlsupplysellhighDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn israreDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_Selected;
    }
}