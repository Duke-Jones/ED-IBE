using System.ComponentModel;
using System.Windows.Forms;

namespace RegulatedNoise.EDDB_Data
{
    partial class EDCommodityView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtAveragePrice = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSupplyBuyHigh = new System.Windows.Forms.TextBox();
            this.txtSupplyBuyLow = new System.Windows.Forms.TextBox();
            this.txtSupplySellHigh = new System.Windows.Forms.TextBox();
            this.txtSupplySellLow = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDemandBuyHigh = new System.Windows.Forms.TextBox();
            this.txtDemandBuyLow = new System.Windows.Forms.TextBox();
            this.txtDemandSellHigh = new System.Windows.Forms.TextBox();
            this.txtDemandSellLow = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cmdCommodity = new System.Windows.Forms.ComboBox();
            this.cmdFullList = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(263, 274);
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
            this.cmdOk.Location = new System.Drawing.Point(182, 274);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 1;
            this.cmdOk.Text = "OK";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Category";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Average Price";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(104, 39);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(52, 20);
            this.txtId.TabIndex = 16;
            // 
            // txtAveragePrice
            // 
            this.txtAveragePrice.Location = new System.Drawing.Point(104, 90);
            this.txtAveragePrice.Name = "txtAveragePrice";
            this.txtAveragePrice.ReadOnly = true;
            this.txtAveragePrice.Size = new System.Drawing.Size(53, 20);
            this.txtAveragePrice.TabIndex = 19;
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(104, 64);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.ReadOnly = true;
            this.txtCategory.Size = new System.Drawing.Size(214, 20);
            this.txtCategory.TabIndex = 18;
            this.txtCategory.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSupplyBuyHigh);
            this.groupBox1.Controls.Add(this.txtSupplyBuyLow);
            this.groupBox1.Controls.Add(this.txtSupplySellHigh);
            this.groupBox1.Controls.Add(this.txtSupplySellLow);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.groupBox1.Location = new System.Drawing.Point(181, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 132);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Price Warnlevels \"Supply\"";
            // 
            // txtSupplyBuyHigh
            // 
            this.txtSupplyBuyHigh.Location = new System.Drawing.Point(76, 98);
            this.txtSupplyBuyHigh.Name = "txtSupplyBuyHigh";
            this.txtSupplyBuyHigh.Size = new System.Drawing.Size(53, 20);
            this.txtSupplyBuyHigh.TabIndex = 32;
            this.txtSupplyBuyHigh.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            this.txtSupplyBuyHigh.GotFocus += new System.EventHandler(this.txtField_GotFocus);
            // 
            // txtSupplyBuyLow
            // 
            this.txtSupplyBuyLow.Location = new System.Drawing.Point(76, 72);
            this.txtSupplyBuyLow.Name = "txtSupplyBuyLow";
            this.txtSupplyBuyLow.Size = new System.Drawing.Size(53, 20);
            this.txtSupplyBuyLow.TabIndex = 31;
            this.txtSupplyBuyLow.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            // 
            // txtSupplySellHigh
            // 
            this.txtSupplySellHigh.Location = new System.Drawing.Point(76, 46);
            this.txtSupplySellHigh.Name = "txtSupplySellHigh";
            this.txtSupplySellHigh.Size = new System.Drawing.Size(53, 20);
            this.txtSupplySellHigh.TabIndex = 30;
            this.txtSupplySellHigh.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            // 
            // txtSupplySellLow
            // 
            this.txtSupplySellLow.Location = new System.Drawing.Point(76, 20);
            this.txtSupplySellLow.Name = "txtSupplySellLow";
            this.txtSupplySellLow.Size = new System.Drawing.Size(53, 20);
            this.txtSupplySellLow.TabIndex = 29;
            this.txtSupplySellLow.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.DarkBlue;
            this.label9.Location = new System.Drawing.Point(16, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Buy High";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.DarkBlue;
            this.label7.Location = new System.Drawing.Point(16, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Buy Low";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DarkRed;
            this.label6.Location = new System.Drawing.Point(16, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Sell High";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkRed;
            this.label4.Location = new System.Drawing.Point(16, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Sell Low";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDemandBuyHigh);
            this.groupBox2.Controls.Add(this.txtDemandBuyLow);
            this.groupBox2.Controls.Add(this.txtDemandSellHigh);
            this.groupBox2.Controls.Add(this.txtDemandSellLow);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.ForeColor = System.Drawing.Color.DarkGreen;
            this.groupBox2.Location = new System.Drawing.Point(18, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 132);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Price Warnlevels \"Demand\"";
            // 
            // txtDemandBuyHigh
            // 
            this.txtDemandBuyHigh.Location = new System.Drawing.Point(76, 98);
            this.txtDemandBuyHigh.Name = "txtDemandBuyHigh";
            this.txtDemandBuyHigh.Size = new System.Drawing.Size(53, 20);
            this.txtDemandBuyHigh.TabIndex = 32;
            this.txtDemandBuyHigh.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            // 
            // txtDemandBuyLow
            // 
            this.txtDemandBuyLow.Location = new System.Drawing.Point(76, 72);
            this.txtDemandBuyLow.Name = "txtDemandBuyLow";
            this.txtDemandBuyLow.Size = new System.Drawing.Size(53, 20);
            this.txtDemandBuyLow.TabIndex = 31;
            this.txtDemandBuyLow.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            // 
            // txtDemandSellHigh
            // 
            this.txtDemandSellHigh.Location = new System.Drawing.Point(76, 46);
            this.txtDemandSellHigh.Name = "txtDemandSellHigh";
            this.txtDemandSellHigh.Size = new System.Drawing.Size(53, 20);
            this.txtDemandSellHigh.TabIndex = 30;
            this.txtDemandSellHigh.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            // 
            // txtDemandSellLow
            // 
            this.txtDemandSellLow.Location = new System.Drawing.Point(76, 20);
            this.txtDemandSellLow.Name = "txtDemandSellLow";
            this.txtDemandSellLow.Size = new System.Drawing.Size(53, 20);
            this.txtDemandSellLow.TabIndex = 29;
            this.txtDemandSellLow.TextChanged += new System.EventHandler(this.txtField_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(16, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Buy High";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.DarkBlue;
            this.label15.Location = new System.Drawing.Point(16, 75);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 13);
            this.label15.TabIndex = 27;
            this.label15.Text = "Buy Low";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.DarkRed;
            this.label16.Location = new System.Drawing.Point(16, 49);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 13);
            this.label16.TabIndex = 26;
            this.label16.Text = "Sell High";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.DarkRed;
            this.label17.Location = new System.Drawing.Point(16, 23);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 13);
            this.label17.TabIndex = 25;
            this.label17.Text = "Sell Low";
            // 
            // cmdCommodity
            // 
            this.cmdCommodity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdCommodity.FormattingEnabled = true;
            this.cmdCommodity.Location = new System.Drawing.Point(18, 12);
            this.cmdCommodity.Name = "cmdCommodity";
            this.cmdCommodity.Size = new System.Drawing.Size(299, 21);
            this.cmdCommodity.Sorted = true;
            this.cmdCommodity.TabIndex = 34;
            this.cmdCommodity.SelectedIndexChanged += new System.EventHandler(this.cmdCommodity_SelectedIndexChanged);
            // 
            // cmdFullList
            // 
            this.cmdFullList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFullList.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdFullList.Location = new System.Drawing.Point(18, 274);
            this.cmdFullList.Name = "cmdFullList";
            this.cmdFullList.Size = new System.Drawing.Size(75, 23);
            this.cmdFullList.TabIndex = 35;
            this.cmdFullList.Text = "Full List";
            this.cmdFullList.UseVisualStyleBackColor = true;
            this.cmdFullList.Click += new System.EventHandler(this.cmdFullList_Click);
            // 
            // EDCommodityView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 309);
            this.Controls.Add(this.cmdFullList);
            this.Controls.Add(this.cmdCommodity);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtAveragePrice);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cmdCancel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(366, 347);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(366, 347);
            this.Name = "EDCommodityView";
            this.Text = "Commodity Price Warn levels";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button cmdCancel;
        private Button cmdOk;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtId;
        private TextBox txtAveragePrice;
        private TextBox txtCategory;
        private GroupBox groupBox1;
        private TextBox txtSupplyBuyHigh;
        private TextBox txtSupplyBuyLow;
        private TextBox txtSupplySellHigh;
        private TextBox txtSupplySellLow;
        private Label label9;
        private Label label7;
        private Label label6;
        private Label label4;
        private GroupBox groupBox2;
        private TextBox txtDemandBuyHigh;
        private TextBox txtDemandBuyLow;
        private TextBox txtDemandSellHigh;
        private TextBox txtDemandSellLow;
        private Label label5;
        private Label label15;
        private Label label16;
        private Label label17;
        private ComboBox cmdCommodity;
        private Button cmdFullList;
    }
}