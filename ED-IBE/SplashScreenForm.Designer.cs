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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreenForm));
            this.InfoTarget = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cmdMinimize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SplashInfo
            // 
            this.InfoTarget.BackColor = System.Drawing.SystemColors.WindowText;
            this.InfoTarget.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InfoTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(152)))), ((int)(((byte)(77)))));
            this.InfoTarget.FormattingEnabled = true;
            this.InfoTarget.Items.AddRange(new object[] {
            "starting..."});
            this.InfoTarget.Location = new System.Drawing.Point(68, 172);
            this.InfoTarget.Name = "SplashInfo";
            this.InfoTarget.ScrollAlwaysVisible = true;
            this.InfoTarget.Size = new System.Drawing.Size(569, 143);
            this.InfoTarget.TabIndex = 6;
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
            // cmdMinimize
            // 
            this.cmdMinimize.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.cmdMinimize.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cmdMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.cmdMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.cmdMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdMinimize.Font = new System.Drawing.Font("Wingdings 3", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cmdMinimize.Location = new System.Drawing.Point(660, 1);
            this.cmdMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.cmdMinimize.Name = "cmdMinimize";
            this.cmdMinimize.Size = new System.Drawing.Size(17, 19);
            this.cmdMinimize.TabIndex = 9;
            this.cmdMinimize.Text = "€\r\n";
            this.cmdMinimize.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cmdMinimize.UseVisualStyleBackColor = false;
            this.cmdMinimize.Click += new System.EventHandler(this.cmdMinimize_Click);
            // 
            // SplashScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::IBE.Properties.Resources.RNSplash;
            this.ClientSize = new System.Drawing.Size(700, 340);
            this.Controls.Add(this.cmdMinimize);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InfoTarget);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SplashScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ED-IBE Splashscreen";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.SplashScreenForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ListBox InfoTarget;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button cmdMinimize;
    }
}