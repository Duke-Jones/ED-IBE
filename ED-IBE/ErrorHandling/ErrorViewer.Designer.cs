namespace IBE
{
    partial class ErrorViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorViewer));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblErrorInfo = new System.Windows.Forms.Label();
            this.txtErrorDetail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdShutdown = new System.Windows.Forms.Button();
            this.cmdDumpfile = new System.Windows.Forms.Button();
            this.cmdIgnore = new System.Windows.Forms.Button();
            this.lblLogDestination = new System.Windows.Forms.Label();
            this.cmdOpenLocation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(68, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Exception occured:\r\n\r\n";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblErrorInfo
            // 
            this.lblErrorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrorInfo.AutoEllipsis = true;
            this.lblErrorInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorInfo.Location = new System.Drawing.Point(68, 30);
            this.lblErrorInfo.Name = "lblErrorInfo";
            this.lblErrorInfo.Size = new System.Drawing.Size(555, 47);
            this.lblErrorInfo.TabIndex = 2;
            this.lblErrorInfo.Text = "ExceptionInfo";
            // 
            // txtErrorDetail
            // 
            this.txtErrorDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErrorDetail.Location = new System.Drawing.Point(12, 99);
            this.txtErrorDetail.Multiline = true;
            this.txtErrorDetail.Name = "txtErrorDetail";
            this.txtErrorDetail.Size = new System.Drawing.Size(611, 197);
            this.txtErrorDetail.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Details:";
            // 
            // cmdShutdown
            // 
            this.cmdShutdown.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cmdShutdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdShutdown.Location = new System.Drawing.Point(527, 312);
            this.cmdShutdown.Name = "cmdShutdown";
            this.cmdShutdown.Size = new System.Drawing.Size(96, 40);
            this.cmdShutdown.TabIndex = 1;
            this.cmdShutdown.Text = "Shutdown IBE";
            this.cmdShutdown.UseVisualStyleBackColor = true;
            this.cmdShutdown.Click += new System.EventHandler(this.cmdShutdown_Click);
            // 
            // cmdDumpfile
            // 
            this.cmdDumpfile.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cmdDumpfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdDumpfile.Location = new System.Drawing.Point(12, 312);
            this.cmdDumpfile.Name = "cmdDumpfile";
            this.cmdDumpfile.Size = new System.Drawing.Size(96, 40);
            this.cmdDumpfile.TabIndex = 2;
            this.cmdDumpfile.Text = "Create Dumpfile";
            this.cmdDumpfile.UseVisualStyleBackColor = true;
            this.cmdDumpfile.Click += new System.EventHandler(this.cmdDumpfile_Click);
            // 
            // cmdIgnore
            // 
            this.cmdIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdIgnore.Location = new System.Drawing.Point(425, 312);
            this.cmdIgnore.Name = "cmdIgnore";
            this.cmdIgnore.Size = new System.Drawing.Size(96, 40);
            this.cmdIgnore.TabIndex = 0;
            this.cmdIgnore.Text = "Ignore Exception";
            this.cmdIgnore.UseVisualStyleBackColor = true;
            this.cmdIgnore.Click += new System.EventHandler(this.cmdIgnore_Click);
            // 
            // lblLogDestination
            // 
            this.lblLogDestination.AutoSize = true;
            this.lblLogDestination.Location = new System.Drawing.Point(68, 83);
            this.lblLogDestination.Name = "lblLogDestination";
            this.lblLogDestination.Size = new System.Drawing.Size(0, 13);
            this.lblLogDestination.TabIndex = 8;
            // 
            // cmdOpenLocation
            // 
            this.cmdOpenLocation.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cmdOpenLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOpenLocation.Location = new System.Drawing.Point(502, 71);
            this.cmdOpenLocation.Name = "cmdOpenLocation";
            this.cmdOpenLocation.Size = new System.Drawing.Size(121, 22);
            this.cmdOpenLocation.TabIndex = 9;
            this.cmdOpenLocation.Text = "Open Location";
            this.cmdOpenLocation.UseVisualStyleBackColor = true;
            this.cmdOpenLocation.Click += new System.EventHandler(this.cmdOpenLocation_Click);
            // 
            // ErrorViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 364);
            this.Controls.Add(this.cmdOpenLocation);
            this.Controls.Add(this.lblLogDestination);
            this.Controls.Add(this.cmdIgnore);
            this.Controls.Add(this.cmdDumpfile);
            this.Controls.Add(this.cmdShutdown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtErrorDetail);
            this.Controls.Add(this.lblErrorInfo);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ErrorViewer";
            this.Text = "ErrorViewer";
            this.Load += new System.EventHandler(this.ErrorViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblErrorInfo;
        private System.Windows.Forms.TextBox txtErrorDetail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdShutdown;
        private System.Windows.Forms.Button cmdDumpfile;
        private System.Windows.Forms.Button cmdIgnore;
        private System.Windows.Forms.Label lblLogDestination;
        private System.Windows.Forms.Button cmdOpenLocation;
    }
}