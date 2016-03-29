namespace IBE.Ocr
{
    partial class tabOCR
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bManualLoadImage = new System.Windows.Forms.Button();
            this.lblScreenshotsQueued = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pbStation = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pbTrimmed = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbOriginalImage = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bIgnoreTrash = new System.Windows.Forms.Button();
            this.bClearOcrOutput = new System.Windows.Forms.Button();
            this.bEditResults = new System.Windows.Forms.Button();
            this.cbAutoImport = new System.Windows.Forms.CheckBox();
            this.cbDeleteScreenshotOnImport = new System.Windows.Forms.CheckBox();
            this.cbStartOCROnLoad = new System.Windows.Forms.CheckBox();
            this.cmdHint = new System.Windows.Forms.Button();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.tbOcrSystemName = new System.Windows.Forms.TextBox();
            this.tbFinalOcrOutput = new System.Windows.Forms.TextBox();
            this.bContinueOcr = new System.Windows.Forms.Button();
            this.tbConfidence = new System.Windows.Forms.TextBox();
            this.tbCommoditiesOcrOutput = new System.Windows.Forms.TextBox();
            this.pbOcrCurrent = new System.Windows.Forms.PictureBox();
            this.tbOcrStationName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrimmed)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOcrCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.bManualLoadImage);
            this.groupBox1.Controls.Add(this.lblScreenshotsQueued);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.pbStation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pbTrimmed);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 601);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Capture Price Screenshots";
            // 
            // bManualLoadImage
            // 
            this.bManualLoadImage.Location = new System.Drawing.Point(263, 47);
            this.bManualLoadImage.Name = "bManualLoadImage";
            this.bManualLoadImage.Size = new System.Drawing.Size(145, 23);
            this.bManualLoadImage.TabIndex = 11;
            this.bManualLoadImage.Text = "(debug) LoadPicture";
            this.bManualLoadImage.UseVisualStyleBackColor = true;
            this.bManualLoadImage.Visible = false;
            this.bManualLoadImage.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblScreenshotsQueued
            // 
            this.lblScreenshotsQueued.AutoSize = true;
            this.lblScreenshotsQueued.Location = new System.Drawing.Point(43, 52);
            this.lblScreenshotsQueued.Name = "lblScreenshotsQueued";
            this.lblScreenshotsQueued.Size = new System.Drawing.Size(58, 13);
            this.lblScreenshotsQueued.TabIndex = 10;
            this.lblScreenshotsQueued.Text = "(0 queued)";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(400, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Monitor Directory for Commodity Screenshots";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button7_Click);
            // 
            // pbStation
            // 
            this.pbStation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbStation.Location = new System.Drawing.Point(8, 325);
            this.pbStation.Name = "pbStation";
            this.pbStation.Size = new System.Drawing.Size(205, 19);
            this.pbStation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStation.TabIndex = 9;
            this.pbStation.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 309);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Trimmed";
            // 
            // pbTrimmed
            // 
            this.pbTrimmed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTrimmed.Location = new System.Drawing.Point(8, 355);
            this.pbTrimmed.Name = "pbTrimmed";
            this.pbTrimmed.Size = new System.Drawing.Size(241, 179);
            this.pbTrimmed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTrimmed.TabIndex = 7;
            this.pbTrimmed.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.pbOriginalImage);
            this.panel2.Location = new System.Drawing.Point(5, 68);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(415, 233);
            this.panel2.TabIndex = 3;
            // 
            // pbOriginalImage
            // 
            this.pbOriginalImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbOriginalImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbOriginalImage.Location = new System.Drawing.Point(3, 3);
            this.pbOriginalImage.Name = "pbOriginalImage";
            this.pbOriginalImage.Size = new System.Drawing.Size(409, 227);
            this.pbOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbOriginalImage.TabIndex = 1;
            this.pbOriginalImage.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Original";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.bIgnoreTrash);
            this.groupBox4.Controls.Add(this.bClearOcrOutput);
            this.groupBox4.Controls.Add(this.bEditResults);
            this.groupBox4.Controls.Add(this.cbAutoImport);
            this.groupBox4.Controls.Add(this.cbDeleteScreenshotOnImport);
            this.groupBox4.Controls.Add(this.cbStartOCROnLoad);
            this.groupBox4.Controls.Add(this.cmdHint);
            this.groupBox4.Controls.Add(this.label36);
            this.groupBox4.Controls.Add(this.label35);
            this.groupBox4.Controls.Add(this.label34);
            this.groupBox4.Controls.Add(this.label33);
            this.groupBox4.Controls.Add(this.label32);
            this.groupBox4.Controls.Add(this.tbOcrSystemName);
            this.groupBox4.Controls.Add(this.tbFinalOcrOutput);
            this.groupBox4.Controls.Add(this.bContinueOcr);
            this.groupBox4.Controls.Add(this.tbConfidence);
            this.groupBox4.Controls.Add(this.tbCommoditiesOcrOutput);
            this.groupBox4.Controls.Add(this.pbOcrCurrent);
            this.groupBox4.Controls.Add(this.tbOcrStationName);
            this.groupBox4.Location = new System.Drawing.Point(433, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(587, 545);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "OCR Corrections";
            // 
            // bIgnoreTrash
            // 
            this.bIgnoreTrash.Enabled = false;
            this.bIgnoreTrash.Location = new System.Drawing.Point(422, 152);
            this.bIgnoreTrash.Name = "bIgnoreTrash";
            this.bIgnoreTrash.Size = new System.Drawing.Size(154, 23);
            this.bIgnoreTrash.TabIndex = 34;
            this.bIgnoreTrash.Text = "&Ignore as Trash";
            this.bIgnoreTrash.UseVisualStyleBackColor = true;
            this.bIgnoreTrash.TextChanged += new System.EventHandler(this.cmdIgnore_Click);
            this.bIgnoreTrash.Click += new System.EventHandler(this.cmdIgnore_Click);
            // 
            // bClearOcrOutput
            // 
            this.bClearOcrOutput.Enabled = false;
            this.bClearOcrOutput.Location = new System.Drawing.Point(295, 402);
            this.bClearOcrOutput.Name = "bClearOcrOutput";
            this.bClearOcrOutput.Size = new System.Drawing.Size(282, 23);
            this.bClearOcrOutput.TabIndex = 33;
            this.bClearOcrOutput.Text = "Clear Results";
            this.bClearOcrOutput.UseVisualStyleBackColor = true;
            this.bClearOcrOutput.Click += new System.EventHandler(this.bClearOcrOutput_Click);
            // 
            // bEditResults
            // 
            this.bEditResults.Enabled = false;
            this.bEditResults.Location = new System.Drawing.Point(6, 402);
            this.bEditResults.Name = "bEditResults";
            this.bEditResults.Size = new System.Drawing.Size(282, 23);
            this.bEditResults.TabIndex = 32;
            this.bEditResults.Text = "Edit Results";
            this.bEditResults.UseVisualStyleBackColor = true;
            this.bEditResults.TextChanged += new System.EventHandler(this.bEditResults_Click);
            // 
            // cbAutoImport
            // 
            this.cbAutoImport.AutoSize = true;
            this.cbAutoImport.Location = new System.Drawing.Point(6, 477);
            this.cbAutoImport.Name = "cbAutoImport";
            this.cbAutoImport.Size = new System.Drawing.Size(80, 17);
            this.cbAutoImport.TabIndex = 31;
            this.cbAutoImport.Text = "Auto Import";
            this.cbAutoImport.UseVisualStyleBackColor = true;
            this.cbAutoImport.CheckedChanged += new System.EventHandler(this.cbAutoImport_CheckedChanged);
            // 
            // cbDeleteScreenshotOnImport
            // 
            this.cbDeleteScreenshotOnImport.AutoSize = true;
            this.cbDeleteScreenshotOnImport.Location = new System.Drawing.Point(6, 454);
            this.cbDeleteScreenshotOnImport.Name = "cbDeleteScreenshotOnImport";
            this.cbDeleteScreenshotOnImport.Size = new System.Drawing.Size(222, 17);
            this.cbDeleteScreenshotOnImport.TabIndex = 29;
            this.cbDeleteScreenshotOnImport.Text = "Delete screenshot automatically on import";
            this.cbDeleteScreenshotOnImport.UseVisualStyleBackColor = true;
            this.cbDeleteScreenshotOnImport.CheckedChanged += new System.EventHandler(this.cbDeleteScreenshotOnImport_CheckedChanged);
            // 
            // cbStartOCROnLoad
            // 
            this.cbStartOCROnLoad.AutoSize = true;
            this.cbStartOCROnLoad.Location = new System.Drawing.Point(6, 431);
            this.cbStartOCROnLoad.Name = "cbStartOCROnLoad";
            this.cbStartOCROnLoad.Size = new System.Drawing.Size(252, 17);
            this.cbStartOCROnLoad.TabIndex = 28;
            this.cbStartOCROnLoad.Text = "Start OCR automatically when this app is started";
            this.cbStartOCROnLoad.UseVisualStyleBackColor = true;
            this.cbStartOCROnLoad.CheckStateChanged += new System.EventHandler(this.cbStartOCROnLoad_CheckedChanged);
            // 
            // cmdHint
            // 
            this.cmdHint.ForeColor = System.Drawing.Color.Crimson;
            this.cmdHint.Location = new System.Drawing.Point(426, 49);
            this.cmdHint.Name = "cmdHint";
            this.cmdHint.Size = new System.Drawing.Size(150, 23);
            this.cmdHint.TabIndex = 26;
            this.cmdHint.Text = "Really Useful Tip";
            this.cmdHint.UseVisualStyleBackColor = true;
            this.cmdHint.Click += new System.EventHandler(this.cmdHint_Click);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(7, 162);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(112, 13);
            this.label36.TabIndex = 25;
            this.label36.Text = "CSV Output from OCR";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(11, 137);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(71, 13);
            this.label35.TabIndex = 24;
            this.label35.Text = "Correct Value";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(11, 94);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(85, 13);
            this.label34.TabIndex = 23;
            this.label34.Text = "Image to Correct";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(11, 52);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(72, 13);
            this.label33.TabIndex = 22;
            this.label33.Text = "System Name";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(11, 29);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(71, 13);
            this.label32.TabIndex = 21;
            this.label32.Text = "Station Name";
            // 
            // tbOcrSystemName
            // 
            this.tbOcrSystemName.Location = new System.Drawing.Point(110, 53);
            this.tbOcrSystemName.Name = "tbOcrSystemName";
            this.tbOcrSystemName.Size = new System.Drawing.Size(231, 20);
            this.tbOcrSystemName.TabIndex = 16;
            this.tbOcrSystemName.TextChanged += new System.EventHandler(this.tbOcrSystemName_TextChanged);
            // 
            // tbFinalOcrOutput
            // 
            this.tbFinalOcrOutput.Location = new System.Drawing.Point(6, 180);
            this.tbFinalOcrOutput.Multiline = true;
            this.tbFinalOcrOutput.Name = "tbFinalOcrOutput";
            this.tbFinalOcrOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFinalOcrOutput.Size = new System.Drawing.Size(571, 216);
            this.tbFinalOcrOutput.TabIndex = 15;
            this.tbFinalOcrOutput.WordWrap = false;
            // 
            // bContinueOcr
            // 
            this.bContinueOcr.Enabled = false;
            this.bContinueOcr.Location = new System.Drawing.Point(422, 128);
            this.bContinueOcr.Name = "bContinueOcr";
            this.bContinueOcr.Size = new System.Drawing.Size(154, 23);
            this.bContinueOcr.TabIndex = 14;
            this.bContinueOcr.Text = "C&ontinue";
            this.bContinueOcr.UseVisualStyleBackColor = true;
            this.bContinueOcr.TextChanged += new System.EventHandler(this.bContinueOcr_Click);
            this.bContinueOcr.Click += new System.EventHandler(this.bContinueOcr_Click);
            // 
            // tbConfidence
            // 
            this.tbConfidence.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfidence.Location = new System.Drawing.Point(347, 128);
            this.tbConfidence.Name = "tbConfidence";
            this.tbConfidence.Size = new System.Drawing.Size(62, 29);
            this.tbConfidence.TabIndex = 13;
            // 
            // tbCommoditiesOcrOutput
            // 
            this.tbCommoditiesOcrOutput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbCommoditiesOcrOutput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCommoditiesOcrOutput.Location = new System.Drawing.Point(110, 128);
            this.tbCommoditiesOcrOutput.Name = "tbCommoditiesOcrOutput";
            this.tbCommoditiesOcrOutput.Size = new System.Drawing.Size(231, 29);
            this.tbCommoditiesOcrOutput.TabIndex = 12;
            this.tbCommoditiesOcrOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCommoditiesOcrOutput_Keypress);
            // 
            // pbOcrCurrent
            // 
            this.pbOcrCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbOcrCurrent.Location = new System.Drawing.Point(110, 79);
            this.pbOcrCurrent.Name = "pbOcrCurrent";
            this.pbOcrCurrent.Size = new System.Drawing.Size(231, 43);
            this.pbOcrCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOcrCurrent.TabIndex = 11;
            this.pbOcrCurrent.TabStop = false;
            // 
            // tbOcrStationName
            // 
            this.tbOcrStationName.Location = new System.Drawing.Point(110, 27);
            this.tbOcrStationName.Name = "tbOcrStationName";
            this.tbOcrStationName.Size = new System.Drawing.Size(231, 20);
            this.tbOcrStationName.TabIndex = 10;
            this.tbOcrStationName.TextChanged += new System.EventHandler(this.tbOcrStationName_TextChanged);
            // 
            // tabOCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "tabOCR";
            this.Size = new System.Drawing.Size(1047, 560);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrimmed)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOcrCurrent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bManualLoadImage;
        private System.Windows.Forms.Label lblScreenshotsQueued;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pbStation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbTrimmed;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.PictureBox pbOriginalImage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button bIgnoreTrash;
        private System.Windows.Forms.Button bClearOcrOutput;
        private System.Windows.Forms.Button bEditResults;
        private System.Windows.Forms.CheckBox cbAutoImport;
        private System.Windows.Forms.CheckBox cbDeleteScreenshotOnImport;
        private System.Windows.Forms.CheckBox cbStartOCROnLoad;
        private System.Windows.Forms.Button cmdHint;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox tbOcrSystemName;
        private System.Windows.Forms.TextBox tbFinalOcrOutput;
        private System.Windows.Forms.Button bContinueOcr;
        private System.Windows.Forms.TextBox tbConfidence;
        private System.Windows.Forms.TextBox tbCommoditiesOcrOutput;
        private System.Windows.Forms.PictureBox pbOcrCurrent;
        private System.Windows.Forms.TextBox tbOcrStationName;
    }
}
