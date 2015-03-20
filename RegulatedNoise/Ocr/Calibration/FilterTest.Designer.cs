    partial class FilterTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterTest));
            this.nudCutoffValue = new System.Windows.Forms.NumericUpDown();
            this.cmdSaveClose = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.paPicturePanel = new System.Windows.Forms.Panel();
            this.pbPicture = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pb_calibratorMagnifier = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pbSampleTooHigh = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pbSampleTooLow = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffValue)).BeginInit();
            this.paPicturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_calibratorMagnifier)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSampleTooHigh)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSampleTooLow)).BeginInit();
            this.SuspendLayout();
            // 
            // nudCutoffValue
            // 
            this.nudCutoffValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudCutoffValue.Location = new System.Drawing.Point(936, 587);
            this.nudCutoffValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudCutoffValue.Name = "nudCutoffValue";
            this.nudCutoffValue.Size = new System.Drawing.Size(57, 20);
            this.nudCutoffValue.TabIndex = 1;
            this.nudCutoffValue.ValueChanged += new System.EventHandler(this.nudCutoffValue_ValueChanged);
            // 
            // cmdSaveClose
            // 
            this.cmdSaveClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSaveClose.Location = new System.Drawing.Point(733, 642);
            this.cmdSaveClose.Name = "cmdSaveClose";
            this.cmdSaveClose.Size = new System.Drawing.Size(151, 23);
            this.cmdSaveClose.TabIndex = 2;
            this.cmdSaveClose.Text = "Save New Value and Close ";
            this.cmdSaveClose.UseVisualStyleBackColor = true;
            this.cmdSaveClose.Click += new System.EventHandler(this.cmdSaveClose_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(890, 642);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(103, 23);
            this.cmdClose.TabIndex = 3;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdCloseOnly_Click);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel.TabIndex = 4;
            // 
            // paPicturePanel
            // 
            this.paPicturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paPicturePanel.AutoScroll = true;
            this.paPicturePanel.Controls.Add(this.pbPicture);
            this.paPicturePanel.Location = new System.Drawing.Point(12, 18);
            this.paPicturePanel.Name = "paPicturePanel";
            this.paPicturePanel.Size = new System.Drawing.Size(700, 558);
            this.paPicturePanel.TabIndex = 5;
            // 
            // pbPicture
            // 
            this.pbPicture.Location = new System.Drawing.Point(3, 3);
            this.pbPicture.Name = "pbPicture";
            this.pbPicture.Size = new System.Drawing.Size(421, 335);
            this.pbPicture.TabIndex = 0;
            this.pbPicture.TabStop = false;
            this.pbPicture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbPicture_Click);
            this.pbPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbPicture_MouseDown);
            this.pbPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbPicture_MouseMove);
            this.pbPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbPicture_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pb_calibratorMagnifier);
            this.groupBox1.Location = new System.Drawing.Point(726, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 187);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Magnifier (Click On Main Picture)";
            // 
            // pb_calibratorMagnifier
            // 
            this.pb_calibratorMagnifier.BackColor = System.Drawing.Color.DarkGray;
            this.pb_calibratorMagnifier.Location = new System.Drawing.Point(6, 19);
            this.pb_calibratorMagnifier.Name = "pb_calibratorMagnifier";
            this.pb_calibratorMagnifier.Size = new System.Drawing.Size(255, 162);
            this.pb_calibratorMagnifier.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_calibratorMagnifier.TabIndex = 0;
            this.pb_calibratorMagnifier.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.pbSampleTooHigh);
            this.groupBox2.Location = new System.Drawing.Point(726, 205);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 187);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sample: CutOff-Value Too High";
            // 
            // pbSampleTooHigh
            // 
            this.pbSampleTooHigh.BackColor = System.Drawing.Color.DarkGray;
            this.pbSampleTooHigh.Image = global::RegulatedNoise.Properties.Resources.SampleTooHigh;
            this.pbSampleTooHigh.Location = new System.Drawing.Point(6, 19);
            this.pbSampleTooHigh.Name = "pbSampleTooHigh";
            this.pbSampleTooHigh.Size = new System.Drawing.Size(255, 162);
            this.pbSampleTooHigh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSampleTooHigh.TabIndex = 0;
            this.pbSampleTooHigh.TabStop = false;
            this.pbSampleTooHigh.Click += new System.EventHandler(this.pbSampleTooHigh_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.pbSampleTooLow);
            this.groupBox3.Location = new System.Drawing.Point(726, 398);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 187);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sample: CutOff-Value Too Low";
            // 
            // pbSampleTooLow
            // 
            this.pbSampleTooLow.BackColor = System.Drawing.Color.DarkGray;
            this.pbSampleTooLow.Image = global::RegulatedNoise.Properties.Resources.SampleTooLow;
            this.pbSampleTooLow.Location = new System.Drawing.Point(6, 19);
            this.pbSampleTooLow.Name = "pbSampleTooLow";
            this.pbSampleTooLow.Size = new System.Drawing.Size(255, 162);
            this.pbSampleTooLow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSampleTooLow.TabIndex = 0;
            this.pbSampleTooLow.TabStop = false;
            this.pbSampleTooLow.Click += new System.EventHandler(this.pbSampleTooLow_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(91, 579);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(621, 92);
            this.label1.TabIndex = 18;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(766, 589);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "UI Color Cutoff Level (0 to 255)";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(754, 605);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "(Activate A New Value With [Enter])";
            // 
            // FilterTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1005, 677);
            this.Controls.Add(this.cmdSaveClose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.paPicturePanel);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.nudCutoffValue);
            this.Name = "FilterTest";
            this.Text = "FilterTest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FilterTest_FormClosing);
            this.Load += new System.EventHandler(this.FilterTest_Load);
            this.Shown += new System.EventHandler(this.FilterTest_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffValue)).EndInit();
            this.paPicturePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_calibratorMagnifier)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSampleTooHigh)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSampleTooLow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudCutoffValue;
        private System.Windows.Forms.Button cmdSaveClose;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Panel paPicturePanel;
        private System.Windows.Forms.PictureBox pbPicture;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pb_calibratorMagnifier;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pbSampleTooHigh;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pbSampleTooLow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
