namespace RegulatedNoise
{
    partial class EditPriceData
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
            this.button1 = new System.Windows.Forms.Button();
            this.tbEditSystem = new System.Windows.Forms.TextBox();
            this.tbEditStation = new System.Windows.Forms.TextBox();
            this.nEditSell = new System.Windows.Forms.NumericUpDown();
            this.nEditBuy = new System.Windows.Forms.NumericUpDown();
            this.nEditDemand = new System.Windows.Forms.NumericUpDown();
            this.tbEditDemandLevel = new System.Windows.Forms.TextBox();
            this.nEditSupply = new System.Windows.Forms.NumericUpDown();
            this.tbEditSupplyLevel = new System.Windows.Forms.TextBox();
            this.dtpEditSampleDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tbEditFilename = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbEditCommodityName = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nEditSell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEditBuy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEditDemand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEditSupply)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(190, 299);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbEditSystem
            // 
            this.tbEditSystem.Location = new System.Drawing.Point(109, 12);
            this.tbEditSystem.Name = "tbEditSystem";
            this.tbEditSystem.Size = new System.Drawing.Size(156, 20);
            this.tbEditSystem.TabIndex = 1;
            // 
            // tbEditStation
            // 
            this.tbEditStation.Location = new System.Drawing.Point(109, 38);
            this.tbEditStation.Name = "tbEditStation";
            this.tbEditStation.Size = new System.Drawing.Size(156, 20);
            this.tbEditStation.TabIndex = 2;
            // 
            // nEditSell
            // 
            this.nEditSell.Location = new System.Drawing.Point(109, 90);
            this.nEditSell.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nEditSell.Name = "nEditSell";
            this.nEditSell.Size = new System.Drawing.Size(156, 20);
            this.nEditSell.TabIndex = 4;
            // 
            // nEditBuy
            // 
            this.nEditBuy.Location = new System.Drawing.Point(109, 116);
            this.nEditBuy.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nEditBuy.Name = "nEditBuy";
            this.nEditBuy.Size = new System.Drawing.Size(156, 20);
            this.nEditBuy.TabIndex = 5;
            // 
            // nEditDemand
            // 
            this.nEditDemand.Location = new System.Drawing.Point(109, 142);
            this.nEditDemand.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nEditDemand.Name = "nEditDemand";
            this.nEditDemand.Size = new System.Drawing.Size(156, 20);
            this.nEditDemand.TabIndex = 6;
            // 
            // tbEditDemandLevel
            // 
            this.tbEditDemandLevel.Location = new System.Drawing.Point(109, 168);
            this.tbEditDemandLevel.Name = "tbEditDemandLevel";
            this.tbEditDemandLevel.Size = new System.Drawing.Size(156, 20);
            this.tbEditDemandLevel.TabIndex = 7;
            // 
            // nEditSupply
            // 
            this.nEditSupply.Location = new System.Drawing.Point(109, 194);
            this.nEditSupply.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nEditSupply.Name = "nEditSupply";
            this.nEditSupply.Size = new System.Drawing.Size(156, 20);
            this.nEditSupply.TabIndex = 8;
            // 
            // tbEditSupplyLevel
            // 
            this.tbEditSupplyLevel.Location = new System.Drawing.Point(109, 220);
            this.tbEditSupplyLevel.Name = "tbEditSupplyLevel";
            this.tbEditSupplyLevel.Size = new System.Drawing.Size(156, 20);
            this.tbEditSupplyLevel.TabIndex = 9;
            // 
            // dtpEditSampleDate
            // 
            this.dtpEditSampleDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEditSampleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEditSampleDate.Location = new System.Drawing.Point(109, 272);
            this.dtpEditSampleDate.Name = "dtpEditSampleDate";
            this.dtpEditSampleDate.Size = new System.Drawing.Size(156, 20);
            this.dtpEditSampleDate.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "System";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Station";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Commodity Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Sell Price";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Buy Price";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Demand";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Demand Level";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 196);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Supply";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 223);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Supply Level";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 275);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Sample Date";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(109, 299);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 21;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbEditFilename
            // 
            this.tbEditFilename.Location = new System.Drawing.Point(109, 246);
            this.tbEditFilename.Name = "tbEditFilename";
            this.tbEditFilename.Size = new System.Drawing.Size(156, 20);
            this.tbEditFilename.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 249);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Screenshot";
            // 
            // cbEditCommodityName
            // 
            this.cbEditCommodityName.FormattingEnabled = true;
            this.cbEditCommodityName.Location = new System.Drawing.Point(109, 64);
            this.cbEditCommodityName.Name = "cbEditCommodityName";
            this.cbEditCommodityName.Size = new System.Drawing.Size(156, 21);
            this.cbEditCommodityName.TabIndex = 24;
            // 
            // EditPriceData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 334);
            this.Controls.Add(this.cbEditCommodityName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbEditFilename);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEditSampleDate);
            this.Controls.Add(this.tbEditSupplyLevel);
            this.Controls.Add(this.nEditSupply);
            this.Controls.Add(this.tbEditDemandLevel);
            this.Controls.Add(this.nEditDemand);
            this.Controls.Add(this.nEditBuy);
            this.Controls.Add(this.nEditSell);
            this.Controls.Add(this.tbEditStation);
            this.Controls.Add(this.tbEditSystem);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(293, 373);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(293, 373);
            this.Name = "EditPriceData";
            this.Text = "Edit Commodity Data";
            ((System.ComponentModel.ISupportInitialize)(this.nEditSell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEditBuy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEditDemand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nEditSupply)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbEditSystem;
        private System.Windows.Forms.TextBox tbEditStation;
        private System.Windows.Forms.NumericUpDown nEditSell;
        private System.Windows.Forms.NumericUpDown nEditBuy;
        private System.Windows.Forms.NumericUpDown nEditDemand;
        private System.Windows.Forms.TextBox tbEditDemandLevel;
        private System.Windows.Forms.NumericUpDown nEditSupply;
        private System.Windows.Forms.TextBox tbEditSupplyLevel;
        private System.Windows.Forms.DateTimePicker dtpEditSampleDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbEditFilename;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbEditCommodityName;
    }
}