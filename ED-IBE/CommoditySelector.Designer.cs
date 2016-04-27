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
            this.is_Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
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
            this.tbcommodityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsEliteDB = new IBE.SQL.Datasets.dsEliteDB();
            this.tbcommodityTableAdapter = new IBE.SQL.Datasets.dsEliteDBTableAdapters.tbcommodityTableAdapter();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cbOnlySelected = new System.Windows.Forms.CheckBox();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdClear = new System.Windows.Forms.Button();
            this.cmdReset = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommodities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcommodityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCommodities
            // 
            this.dgvCommodities.AllowUserToAddRows = false;
            this.dgvCommodities.AllowUserToDeleteRows = false;
            this.dgvCommodities.AutoGenerateColumns = false;
            this.dgvCommodities.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvCommodities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCommodities.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.is_Selected,
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
            this.israreDataGridViewCheckBoxColumn});
            this.dgvCommodities.DataSource = this.tbcommodityBindingSource;
            this.dgvCommodities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCommodities.DoubleBuffer = true;
            this.dgvCommodities.Location = new System.Drawing.Point(0, 0);
            this.dgvCommodities.MultiSelect = false;
            this.dgvCommodities.Name = "dgvCommodities";
            this.dgvCommodities.Size = new System.Drawing.Size(467, 230);
            this.dgvCommodities.TabIndex = 0;
            this.dgvCommodities.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCommodities_CellValueChanged);
            this.dgvCommodities.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvCommodities_CurrentCellDirtyStateChanged);
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
            // is_Selected
            // 
            this.is_Selected.DataPropertyName = "is_selected";
            this.is_Selected.FalseValue = "False";
            this.is_Selected.HeaderText = "Select";
            this.is_Selected.Name = "is_Selected";
            this.is_Selected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.is_Selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.is_Selected.TrueValue = "True";
            this.is_Selected.Width = 60;
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
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(495, 90);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(129, 33);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cbOnlySelected
            // 
            this.cbOnlySelected.AutoSize = true;
            this.cbOnlySelected.Location = new System.Drawing.Point(500, 140);
            this.cbOnlySelected.Name = "cbOnlySelected";
            this.cbOnlySelected.Size = new System.Drawing.Size(118, 17);
            this.cbOnlySelected.TabIndex = 3;
            this.cbOnlySelected.Text = "&Show only selected";
            this.cbOnlySelected.UseVisualStyleBackColor = true;
            this.cbOnlySelected.CheckedChanged += new System.EventHandler(this.cbOnlySelected_CheckedChanged);
            // 
            // txtSearchString
            // 
            this.txtSearchString.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchString.Location = new System.Drawing.Point(73, 17);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new System.Drawing.Size(303, 22);
            this.txtSearchString.TabIndex = 0;
            this.txtSearchString.TextChanged += new System.EventHandler(this.txtSearchString_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search :";
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(382, 12);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(97, 33);
            this.cmdClear.TabIndex = 2;
            this.cmdClear.Text = "&Clear Search";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // cmdReset
            // 
            this.cmdReset.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.cmdReset.Location = new System.Drawing.Point(495, 51);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(129, 33);
            this.cmdReset.TabIndex = 5;
            this.cmdReset.Text = "C&lear + OK";
            this.cmdReset.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dgvCommodities);
            this.panel1.Location = new System.Drawing.Point(12, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 230);
            this.panel1.TabIndex = 8;
            // 
            // CommoditySelector
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(636, 293);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmdReset);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSearchString);
            this.Controls.Add(this.cbOnlySelected);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommoditySelector";
            this.Text = "Commodities";
            this.Load += new System.EventHandler(this.CommoditySelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommodities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcommodityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEliteDB)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private IBE.Enums_and_Utility_Classes.DataGridViewExt dgvCommodities;
        private SQL.Datasets.dsEliteDB dsEliteDB;
        private System.Windows.Forms.BindingSource tbcommodityBindingSource;
        private SQL.Datasets.dsEliteDBTableAdapters.tbcommodityTableAdapter tbcommodityTableAdapter;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox cbOnlySelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_Selected;
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
        private System.Windows.Forms.TextBox txtSearchString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.Panel panel1;
    }
}