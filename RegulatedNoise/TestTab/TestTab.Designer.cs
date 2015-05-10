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
            this.SuspendLayout();
            // 
            // tbCustomEddnMessage
            // 
            this.tbCustomEddnMessage.Location = new System.Drawing.Point(23, 23);
            this.tbCustomEddnMessage.Multiline = true;
            this.tbCustomEddnMessage.Name = "tbCustomEddnMessage";
            this.tbCustomEddnMessage.Size = new System.Drawing.Size(737, 286);
            this.tbCustomEddnMessage.TabIndex = 0;
            // 
            // btSendCustomEddnMessage
            // 
            this.btSendCustomEddnMessage.Location = new System.Drawing.Point(766, 23);
            this.btSendCustomEddnMessage.Name = "btSendCustomEddnMessage";
            this.btSendCustomEddnMessage.Size = new System.Drawing.Size(199, 44);
            this.btSendCustomEddnMessage.TabIndex = 1;
            this.btSendCustomEddnMessage.Text = "Send Message to UI";
            this.btSendCustomEddnMessage.UseVisualStyleBackColor = true;
            this.btSendCustomEddnMessage.Click += new System.EventHandler(this.btSendCustomEddnMessage_Click);
            // 
            // tbFakeOCRResult
            // 
            this.tbFakeOCRResult.Location = new System.Drawing.Point(23, 330);
            this.tbFakeOCRResult.Multiline = true;
            this.tbFakeOCRResult.Name = "tbFakeOCRResult";
            this.tbFakeOCRResult.Size = new System.Drawing.Size(737, 113);
            this.tbFakeOCRResult.TabIndex = 0;
            this.tbFakeOCRResult.Text = "System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;";
            // 
            // btSendFakeOCRResult
            // 
            this.btSendFakeOCRResult.Location = new System.Drawing.Point(766, 330);
            this.btSendFakeOCRResult.Name = "btSendFakeOCRResult";
            this.btSendFakeOCRResult.Size = new System.Drawing.Size(199, 40);
            this.btSendFakeOCRResult.TabIndex = 1;
            this.btSendFakeOCRResult.Text = "Send OCR to UI";
            this.btSendFakeOCRResult.UseVisualStyleBackColor = true;
            this.btSendFakeOCRResult.Click += new System.EventHandler(this.btSendFakeOCRResult_Click);
            // 
            // TestTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btSendFakeOCRResult);
            this.Controls.Add(this.tbFakeOCRResult);
            this.Controls.Add(this.btSendCustomEddnMessage);
            this.Controls.Add(this.tbCustomEddnMessage);
            this.Name = "TestTab";
            this.Size = new System.Drawing.Size(1124, 755);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCustomEddnMessage;
        private System.Windows.Forms.Button btSendCustomEddnMessage;
        private System.Windows.Forms.TextBox tbFakeOCRResult;
        private System.Windows.Forms.Button btSendFakeOCRResult;
    }
}
