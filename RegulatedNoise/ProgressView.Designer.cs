namespace RegulatedNoise
{
    partial class ProgressView
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblInfotext = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblProgress
            // 
            this.lblProgress.AutoEllipsis = true;
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(189, 84);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(34, 13);
            this.lblProgress.TabIndex = 10;
            this.lblProgress.Text = "33 %";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInfotext
            // 
            this.lblInfotext.AutoEllipsis = true;
            this.lblInfotext.BackColor = System.Drawing.Color.Transparent;
            this.lblInfotext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblInfotext.Location = new System.Drawing.Point(12, 9);
            this.lblInfotext.Name = "lblInfotext";
            this.lblInfotext.Size = new System.Drawing.Size(389, 46);
            this.lblInfotext.TabIndex = 9;
            this.lblInfotext.Text = "Info";
            this.lblInfotext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(147, 100);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(118, 31);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(12, 59);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(389, 22);
            this.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressBar1.TabIndex = 7;
            this.ProgressBar1.Value = 55;
            // 
            // ProgressView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(414, 141);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblInfotext);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.ProgressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(430, 179);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(430, 179);
            this.Name = "ProgressView";
            this.Text = "Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblProgress;
        internal System.Windows.Forms.Label lblInfotext;
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.ProgressBar ProgressBar1;
    }
}
