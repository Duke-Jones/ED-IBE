namespace IBE
{
    partial class SplashScreenForm
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
            this.SplashInfo = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SplashInfo
            // 
            this.SplashInfo.BackColor = System.Drawing.SystemColors.WindowText;
            this.SplashInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SplashInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(152)))), ((int)(((byte)(77)))));
            this.SplashInfo.FormattingEnabled = true;
            this.SplashInfo.Items.AddRange(new object[] {
            "starting..."});
            this.SplashInfo.Location = new System.Drawing.Point(68, 172);
            this.SplashInfo.Name = "SplashInfo";
            this.SplashInfo.ScrollAlwaysVisible = true;
            this.SplashInfo.Size = new System.Drawing.Size(569, 143);
            this.SplashInfo.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(616, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 146);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(152)))), ((int)(((byte)(77)))));
            this.lblVersion.Location = new System.Drawing.Point(607, 27);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(67, 13);
            this.lblVersion.TabIndex = 8;
            this.lblVersion.Text = "v0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SplashScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::IBE.Properties.Resources.RNSplash;
            this.ClientSize = new System.Drawing.Size(700, 340);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SplashInfo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "SplashScreenForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ListBox SplashInfo;
        private System.Windows.Forms.Label lblVersion;
    }
}