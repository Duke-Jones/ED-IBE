namespace RegulatedNoise
{
    partial class frmDataIO
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
            this.components = new System.ComponentModel.Container();
            this.ttToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmdImportOldData = new System.Windows.Forms.Button();
            this.fbFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lbProgess = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // cmdImportOldData
            // 
            this.cmdImportOldData.Location = new System.Drawing.Point(12, 12);
            this.cmdImportOldData.Name = "cmdImportOldData";
            this.cmdImportOldData.Size = new System.Drawing.Size(154, 34);
            this.cmdImportOldData.TabIndex = 0;
            this.cmdImportOldData.Text = "Import Old Datafiles";
            this.ttToolTip.SetToolTip(this.cmdImportOldData, "Imports the whole data structure from the existing old RegulatedNoise version. ");
            this.cmdImportOldData.UseVisualStyleBackColor = true;
            this.cmdImportOldData.Click += new System.EventHandler(this.cmdImportOldData_Click);
            // 
            // lbProgess
            // 
            this.lbProgess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgess.FormattingEnabled = true;
            this.lbProgess.Location = new System.Drawing.Point(12, 293);
            this.lbProgess.Name = "lbProgess";
            this.lbProgess.Size = new System.Drawing.Size(868, 134);
            this.lbProgess.TabIndex = 1;
            // 
            // frmDataIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 439);
            this.Controls.Add(this.lbProgess);
            this.Controls.Add(this.cmdImportOldData);
            this.Name = "frmDataIO";
            this.Text = "Data Interface";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip ttToolTip;
        private System.Windows.Forms.Button cmdImportOldData;
        private System.Windows.Forms.FolderBrowserDialog fbFolderDialog;
        private System.Windows.Forms.ListBox lbProgess;
    }
}
