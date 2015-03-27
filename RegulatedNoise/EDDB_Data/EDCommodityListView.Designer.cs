using System.Windows.Forms;
namespace RegulatedNoise.EDDB_Data
{
    partial class EDCommodityListView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDCommodityListView));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.dgvWarnlevels = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AveragePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DemandSellLow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DemandSellHigh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DemandBuyLow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DemandBuyHigh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplySellLow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplySellHigh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplyBuyLow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplyBuyHigh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarnlevels)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(1123, 513);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Location = new System.Drawing.Point(1042, 513);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 1;
            this.cmdOk.Text = "OK";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // dgvWarnlevels
            // 
            this.dgvWarnlevels.AllowUserToAddRows = false;
            this.dgvWarnlevels.AllowUserToDeleteRows = false;
            this.dgvWarnlevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWarnlevels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvWarnlevels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWarnlevels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.CName,
            this.Category,
            this.AveragePrice,
            this.DemandSellLow,
            this.DemandSellHigh,
            this.DemandBuyLow,
            this.DemandBuyHigh,
            this.SupplySellLow,
            this.SupplySellHigh,
            this.SupplyBuyLow,
            this.SupplyBuyHigh});
            this.dgvWarnlevels.Location = new System.Drawing.Point(12, 12);
            this.dgvWarnlevels.Name = "dgvWarnlevels";
            this.dgvWarnlevels.Size = new System.Drawing.Size(1186, 369);
            this.dgvWarnlevels.TabIndex = 36;
            this.dgvWarnlevels.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dgvWarnlevels.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.dgvWarnlevels.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 41;
            // 
            // CName
            // 
            this.CName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CName.HeaderText = "Name";
            this.CName.Name = "CName";
            this.CName.ReadOnly = true;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            this.Category.Visible = false;
            this.Category.Width = 74;
            // 
            // AveragePrice
            // 
            this.AveragePrice.HeaderText = "Average Price";
            this.AveragePrice.Name = "AveragePrice";
            this.AveragePrice.ReadOnly = true;
            this.AveragePrice.Width = 91;
            // 
            // DemandSellLow
            // 
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Maroon;
            this.DemandSellLow.DefaultCellStyle = dataGridViewCellStyle1;
            this.DemandSellLow.HeaderText = "Demand Sell Low";
            this.DemandSellLow.Name = "DemandSellLow";
            this.DemandSellLow.Width = 87;
            // 
            // DemandSellHigh
            // 
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Maroon;
            this.DemandSellHigh.DefaultCellStyle = dataGridViewCellStyle2;
            this.DemandSellHigh.HeaderText = "Demand Sell High";
            this.DemandSellHigh.Name = "DemandSellHigh";
            this.DemandSellHigh.Width = 87;
            // 
            // DemandBuyLow
            // 
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.DemandBuyLow.DefaultCellStyle = dataGridViewCellStyle3;
            this.DemandBuyLow.HeaderText = "Demand Buy Low";
            this.DemandBuyLow.Name = "DemandBuyLow";
            this.DemandBuyLow.Width = 88;
            // 
            // DemandBuyHigh
            // 
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.DemandBuyHigh.DefaultCellStyle = dataGridViewCellStyle4;
            this.DemandBuyHigh.HeaderText = "Demand Buy High";
            this.DemandBuyHigh.Name = "DemandBuyHigh";
            this.DemandBuyHigh.Width = 88;
            // 
            // SupplySellLow
            // 
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Maroon;
            this.SupplySellLow.DefaultCellStyle = dataGridViewCellStyle5;
            this.SupplySellLow.HeaderText = "Supply Sell Low";
            this.SupplySellLow.Name = "SupplySellLow";
            this.SupplySellLow.Width = 80;
            // 
            // SupplySellHigh
            // 
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Maroon;
            this.SupplySellHigh.DefaultCellStyle = dataGridViewCellStyle6;
            this.SupplySellHigh.HeaderText = "Supply Sell High";
            this.SupplySellHigh.Name = "SupplySellHigh";
            this.SupplySellHigh.Width = 80;
            // 
            // SupplyBuyLow
            // 
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SupplyBuyLow.DefaultCellStyle = dataGridViewCellStyle7;
            this.SupplyBuyLow.HeaderText = "Supply Buy Low";
            this.SupplyBuyLow.Name = "SupplyBuyLow";
            this.SupplyBuyLow.Width = 81;
            // 
            // SupplyBuyHigh
            // 
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SupplyBuyHigh.DefaultCellStyle = dataGridViewCellStyle8;
            this.SupplyBuyHigh.HeaderText = "Supply Buy High";
            this.SupplyBuyHigh.Name = "SupplyBuyHigh";
            this.SupplyBuyHigh.Width = 81;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 391);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(635, 150);
            this.label1.TabIndex = 37;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // EDCommodityListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 548);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvWarnlevels);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cmdCancel);
            this.Name = "EDCommodityListView";
            this.Text = "Commodity Price Warn levels";
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarnlevels)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.DataGridView dgvWarnlevels;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn CName;
        private DataGridViewTextBoxColumn Category;
        private DataGridViewTextBoxColumn AveragePrice;
        private DataGridViewTextBoxColumn DemandSellLow;
        private DataGridViewTextBoxColumn DemandSellHigh;
        private DataGridViewTextBoxColumn DemandBuyLow;
        private DataGridViewTextBoxColumn DemandBuyHigh;
        private DataGridViewTextBoxColumn SupplySellLow;
        private DataGridViewTextBoxColumn SupplySellHigh;
        private DataGridViewTextBoxColumn SupplyBuyLow;
        private DataGridViewTextBoxColumn SupplyBuyHigh;
        private Label label1;
    }
}