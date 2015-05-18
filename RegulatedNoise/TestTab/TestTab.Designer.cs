namespace RegulatedNoise.TestTab
{
    partial class TestTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.tbCustomEddnMessage = new System.Windows.Forms.TextBox();
			this.btSendCustomEddnMessage = new System.Windows.Forms.Button();
			this.tbFakeOCRResult = new System.Windows.Forms.TextBox();
			this.btSendFakeOCRResult = new System.Windows.Forms.Button();
			this.lbCommoditiesLog = new System.Windows.Forms.ListBox();
			this.btImportfromTd = new System.Windows.Forms.Button();
			this.tbFinderRequest = new System.Windows.Forms.TextBox();
			this.tbFinderResult = new System.Windows.Forms.TextBox();
			this.btFindSystem = new System.Windows.Forms.Button();
			this.btFindStation = new System.Windows.Forms.Button();
			this.btFindMarketData = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbCustomEddnMessage
			// 
			this.tbCustomEddnMessage.Location = new System.Drawing.Point(15, 15);
			this.tbCustomEddnMessage.Margin = new System.Windows.Forms.Padding(2);
			this.tbCustomEddnMessage.Multiline = true;
			this.tbCustomEddnMessage.Name = "tbCustomEddnMessage";
			this.tbCustomEddnMessage.Size = new System.Drawing.Size(493, 112);
			this.tbCustomEddnMessage.TabIndex = 0;
			// 
			// btSendCustomEddnMessage
			// 
			this.btSendCustomEddnMessage.Location = new System.Drawing.Point(511, 15);
			this.btSendCustomEddnMessage.Margin = new System.Windows.Forms.Padding(2);
			this.btSendCustomEddnMessage.Name = "btSendCustomEddnMessage";
			this.btSendCustomEddnMessage.Size = new System.Drawing.Size(133, 29);
			this.btSendCustomEddnMessage.TabIndex = 1;
			this.btSendCustomEddnMessage.Text = "Send Message to UI";
			this.btSendCustomEddnMessage.UseVisualStyleBackColor = true;
			this.btSendCustomEddnMessage.Click += new System.EventHandler(this.btSendCustomEddnMessage_Click);
			// 
			// tbFakeOCRResult
			// 
			this.tbFakeOCRResult.Location = new System.Drawing.Point(15, 141);
			this.tbFakeOCRResult.Margin = new System.Windows.Forms.Padding(2);
			this.tbFakeOCRResult.Multiline = true;
			this.tbFakeOCRResult.Name = "tbFakeOCRResult";
			this.tbFakeOCRResult.Size = new System.Drawing.Size(493, 75);
			this.tbFakeOCRResult.TabIndex = 0;
			this.tbFakeOCRResult.Text = "System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;";
			// 
			// btSendFakeOCRResult
			// 
			this.btSendFakeOCRResult.Location = new System.Drawing.Point(511, 141);
			this.btSendFakeOCRResult.Margin = new System.Windows.Forms.Padding(2);
			this.btSendFakeOCRResult.Name = "btSendFakeOCRResult";
			this.btSendFakeOCRResult.Size = new System.Drawing.Size(133, 26);
			this.btSendFakeOCRResult.TabIndex = 1;
			this.btSendFakeOCRResult.Text = "Send OCR to UI";
			this.btSendFakeOCRResult.UseVisualStyleBackColor = true;
			this.btSendFakeOCRResult.Click += new System.EventHandler(this.btSendFakeOCRResult_Click);
			// 
			// lbCommoditiesLog
			// 
			this.lbCommoditiesLog.FormattingEnabled = true;
			this.lbCommoditiesLog.Location = new System.Drawing.Point(15, 227);
			this.lbCommoditiesLog.Margin = new System.Windows.Forms.Padding(2);
			this.lbCommoditiesLog.Name = "lbCommoditiesLog";
			this.lbCommoditiesLog.Size = new System.Drawing.Size(493, 121);
			this.lbCommoditiesLog.TabIndex = 2;
			// 
			// btImportfromTd
			// 
			this.btImportfromTd.Location = new System.Drawing.Point(514, 227);
			this.btImportfromTd.Name = "btImportfromTd";
			this.btImportfromTd.Size = new System.Drawing.Size(130, 23);
			this.btImportfromTd.TabIndex = 3;
			this.btImportfromTd.Text = "Import Prices from TD";
			this.btImportfromTd.UseVisualStyleBackColor = true;
			this.btImportfromTd.Click += new System.EventHandler(this.btImportfromTd_Click);
			// 
			// tbFinderRequest
			// 
			this.tbFinderRequest.Location = new System.Drawing.Point(15, 365);
			this.tbFinderRequest.Name = "tbFinderRequest";
			this.tbFinderRequest.Size = new System.Drawing.Size(219, 20);
			this.tbFinderRequest.TabIndex = 4;
			// 
			// tbFinderResult
			// 
			this.tbFinderResult.Location = new System.Drawing.Point(15, 392);
			this.tbFinderResult.Multiline = true;
			this.tbFinderResult.Name = "tbFinderResult";
			this.tbFinderResult.Size = new System.Drawing.Size(493, 77);
			this.tbFinderResult.TabIndex = 5;
			// 
			// btFindSystem
			// 
			this.btFindSystem.Location = new System.Drawing.Point(241, 363);
			this.btFindSystem.Name = "btFindSystem";
			this.btFindSystem.Size = new System.Drawing.Size(36, 23);
			this.btFindSystem.TabIndex = 6;
			this.btFindSystem.Text = "Sys.";
			this.btFindSystem.UseVisualStyleBackColor = true;
			this.btFindSystem.Click += new System.EventHandler(this.btFindSystem_Click);
			// 
			// btFindStation
			// 
			this.btFindStation.Location = new System.Drawing.Point(283, 363);
			this.btFindStation.Name = "btFindStation";
			this.btFindStation.Size = new System.Drawing.Size(34, 23);
			this.btFindStation.TabIndex = 7;
			this.btFindStation.Text = "Sta";
			this.btFindStation.UseVisualStyleBackColor = true;
			this.btFindStation.Click += new System.EventHandler(this.btFindStation_Click);
			// 
			// btFindMarketData
			// 
			this.btFindMarketData.Location = new System.Drawing.Point(324, 363);
			this.btFindMarketData.Name = "btFindMarketData";
			this.btFindMarketData.Size = new System.Drawing.Size(36, 23);
			this.btFindMarketData.TabIndex = 8;
			this.btFindMarketData.Text = "MD";
			this.btFindMarketData.UseVisualStyleBackColor = true;
			this.btFindMarketData.Click += new System.EventHandler(this.btFindMarketData_Click);
			// 
			// TestTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btFindMarketData);
			this.Controls.Add(this.btFindStation);
			this.Controls.Add(this.btFindSystem);
			this.Controls.Add(this.tbFinderResult);
			this.Controls.Add(this.tbFinderRequest);
			this.Controls.Add(this.btImportfromTd);
			this.Controls.Add(this.lbCommoditiesLog);
			this.Controls.Add(this.btSendFakeOCRResult);
			this.Controls.Add(this.tbFakeOCRResult);
			this.Controls.Add(this.btSendCustomEddnMessage);
			this.Controls.Add(this.tbCustomEddnMessage);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "TestTab";
			this.Size = new System.Drawing.Size(749, 491);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCustomEddnMessage;
        private System.Windows.Forms.Button btSendCustomEddnMessage;
        private System.Windows.Forms.TextBox tbFakeOCRResult;
        private System.Windows.Forms.Button btSendFakeOCRResult;
        private System.Windows.Forms.ListBox lbCommoditiesLog;
		  private System.Windows.Forms.Button btImportfromTd;
		  private System.Windows.Forms.TextBox tbFinderRequest;
		  private System.Windows.Forms.TextBox tbFinderResult;
		  private System.Windows.Forms.Button btFindSystem;
		  private System.Windows.Forms.Button btFindStation;
		  private System.Windows.Forms.Button btFindMarketData;
    }
}
