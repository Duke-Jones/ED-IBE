namespace RegulatedNoise
{
    partial class OcrCalibratorTab
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
            this.btn_calibrate = new System.Windows.Forms.Button();
            this.pb_calibratorBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_resolution = new System.Windows.Forms.TextBox();
            this.tb_fov = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pl_calibrationWindow = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.tb_description = new System.Windows.Forms.TextBox();
            this.pb_example = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_rawdata = new System.Windows.Forms.TextBox();
            this.pb_uicolor = new System.Windows.Forms.PictureBox();
            this.tb_uicolor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pb_calibratorMagnifier = new System.Windows.Forms.PictureBox();
            this.btn_calibration_reset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_calibratorBox)).BeginInit();
            this.pl_calibrationWindow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_example)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_uicolor)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_calibratorMagnifier)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_calibrate
            // 
            this.btn_calibrate.Location = new System.Drawing.Point(4, 4);
            this.btn_calibrate.Name = "btn_calibrate";
            this.btn_calibrate.Size = new System.Drawing.Size(187, 23);
            this.btn_calibrate.TabIndex = 0;
            this.btn_calibrate.Text = "Calibrate!";
            this.btn_calibrate.UseVisualStyleBackColor = true;
            this.btn_calibrate.Click += new System.EventHandler(this.btn_calibrate_Click);
            // 
            // pb_calibratorBox
            // 
            this.pb_calibratorBox.Location = new System.Drawing.Point(0, 0);
            this.pb_calibratorBox.Name = "pb_calibratorBox";
            this.pb_calibratorBox.Size = new System.Drawing.Size(763, 468);
            this.pb_calibratorBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb_calibratorBox.TabIndex = 1;
            this.pb_calibratorBox.TabStop = false;
            this.pb_calibratorBox.Paint += new System.Windows.Forms.PaintEventHandler(this.Pb_calibratorBox_Paint);
            this.pb_calibratorBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Pb_calibratorBox_MouseDown);
            this.pb_calibratorBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Pb_calibratorBox_MouseMove);
            this.pb_calibratorBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Pb_calibratorBox_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(434, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Resolution:";
            // 
            // tb_resolution
            // 
            this.tb_resolution.Location = new System.Drawing.Point(500, 6);
            this.tb_resolution.Name = "tb_resolution";
            this.tb_resolution.ReadOnly = true;
            this.tb_resolution.Size = new System.Drawing.Size(75, 20);
            this.tb_resolution.TabIndex = 3;
            this.tb_resolution.Text = "...";
            this.tb_resolution.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_fov
            // 
            this.tb_fov.Enabled = false;
            this.tb_fov.Location = new System.Drawing.Point(618, 6);
            this.tb_fov.Name = "tb_fov";
            this.tb_fov.ReadOnly = true;
            this.tb_fov.Size = new System.Drawing.Size(75, 20);
            this.tb_fov.TabIndex = 5;
            this.tb_fov.Text = "---";
            this.tb_fov.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(581, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "FOV:";
            // 
            // pl_calibrationWindow
            // 
            this.pl_calibrationWindow.AutoScroll = true;
            this.pl_calibrationWindow.Controls.Add(this.lblWarning);
            this.pl_calibrationWindow.Controls.Add(this.pb_calibratorBox);
            this.pl_calibrationWindow.Location = new System.Drawing.Point(4, 36);
            this.pl_calibrationWindow.Name = "pl_calibrationWindow";
            this.pl_calibrationWindow.Size = new System.Drawing.Size(763, 468);
            this.pl_calibrationWindow.TabIndex = 6;
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.Location = new System.Drawing.Point(58, 175);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(654, 50);
            this.lblWarning.TabIndex = 2;
            this.lblWarning.Text = "No calibration possible because no display data has loaded. \r\nPlease check the ac" +
    "cess to the ED display data.";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_description
            // 
            this.tb_description.Location = new System.Drawing.Point(916, 36);
            this.tb_description.Multiline = true;
            this.tb_description.Name = "tb_description";
            this.tb_description.ReadOnly = true;
            this.tb_description.Size = new System.Drawing.Size(124, 107);
            this.tb_description.TabIndex = 7;
            // 
            // pb_example
            // 
            this.pb_example.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_example.Location = new System.Drawing.Point(773, 36);
            this.pb_example.Name = "pb_example";
            this.pb_example.Size = new System.Drawing.Size(137, 107);
            this.pb_example.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_example.TabIndex = 8;
            this.pb_example.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(964, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rawdata:";
            // 
            // tb_rawdata
            // 
            this.tb_rawdata.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.tb_rawdata.Location = new System.Drawing.Point(967, 351);
            this.tb_rawdata.Multiline = true;
            this.tb_rawdata.Name = "tb_rawdata";
            this.tb_rawdata.ReadOnly = true;
            this.tb_rawdata.Size = new System.Drawing.Size(73, 150);
            this.tb_rawdata.TabIndex = 10;
            // 
            // pb_uicolor
            // 
            this.pb_uicolor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_uicolor.Location = new System.Drawing.Point(773, 351);
            this.pb_uicolor.Name = "pb_uicolor";
            this.pb_uicolor.Size = new System.Drawing.Size(188, 123);
            this.pb_uicolor.TabIndex = 11;
            this.pb_uicolor.TabStop = false;
            // 
            // tb_uicolor
            // 
            this.tb_uicolor.Location = new System.Drawing.Point(773, 480);
            this.tb_uicolor.Name = "tb_uicolor";
            this.tb_uicolor.Size = new System.Drawing.Size(188, 20);
            this.tb_uicolor.TabIndex = 12;
            this.tb_uicolor.TextChanged += new System.EventHandler(this.Tb_uicolor_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(770, 335);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "UI Color:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(716, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(193, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Note: calibration is saved automatically.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pb_calibratorMagnifier);
            this.groupBox1.Location = new System.Drawing.Point(773, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 187);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Magnifier";
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
            this.pb_calibratorMagnifier.Paint += new System.Windows.Forms.PaintEventHandler(this.Pb_calibratorMagnifier_Paint);
            // 
            // btn_calibration_reset
            // 
            this.btn_calibration_reset.Enabled = false;
            this.btn_calibration_reset.Location = new System.Drawing.Point(197, 4);
            this.btn_calibration_reset.Name = "btn_calibration_reset";
            this.btn_calibration_reset.Size = new System.Drawing.Size(98, 23);
            this.btn_calibration_reset.TabIndex = 16;
            this.btn_calibration_reset.Text = "Reset to Auto";
            this.btn_calibration_reset.UseVisualStyleBackColor = true;
            this.btn_calibration_reset.Click += new System.EventHandler(this.Btn_calibration_reset_Click);
            // 
            // OcrCalibratorTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.btn_calibration_reset);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_uicolor);
            this.Controls.Add(this.pb_uicolor);
            this.Controls.Add(this.tb_rawdata);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pb_example);
            this.Controls.Add(this.tb_description);
            this.Controls.Add(this.pl_calibrationWindow);
            this.Controls.Add(this.tb_fov);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_resolution);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_calibrate);
            this.Name = "OcrCalibratorTab";
            this.Size = new System.Drawing.Size(1047, 504);
            this.Load += new System.EventHandler(this.OcrCalibratorTab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_calibratorBox)).EndInit();
            this.pl_calibrationWindow.ResumeLayout(false);
            this.pl_calibrationWindow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_example)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_uicolor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_calibratorMagnifier)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_calibrate;
        private System.Windows.Forms.PictureBox pb_calibratorBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_resolution;
        private System.Windows.Forms.TextBox tb_fov;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pl_calibrationWindow;
        private System.Windows.Forms.TextBox tb_description;
        private System.Windows.Forms.PictureBox pb_example;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_rawdata;
        private System.Windows.Forms.PictureBox pb_uicolor;
        private System.Windows.Forms.TextBox tb_uicolor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pb_calibratorMagnifier;
        private System.Windows.Forms.Button btn_calibration_reset;
        public System.Windows.Forms.Label lblWarning;


    }
}
