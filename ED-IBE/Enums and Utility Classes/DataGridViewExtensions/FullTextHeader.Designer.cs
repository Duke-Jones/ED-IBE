namespace DataGridViewAutoFilter
{
    partial class FullTextHeader
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdOk = new System.Windows.Forms.ButtonExt();
            this.cmdClear = new System.Windows.Forms.ButtonExt();
            this.cmbConstraint = new System.Windows.Forms.ComboBox();
            this.txtFilterText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdOk.Location = new System.Drawing.Point(99, 55);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(46, 22);
            this.cmdOk.TabIndex = 2;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            // 
            // cmdClear
            // 
            this.cmdClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdClear.Location = new System.Drawing.Point(47, 55);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(46, 22);
            this.cmdClear.TabIndex = 3;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // cmbConstraint
            // 
            this.cmbConstraint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConstraint.FormattingEnabled = true;
            this.cmbConstraint.Items.AddRange(new object[] {
            "filter off",
            "equals",
            "contains",
            "starts with",
            "ends with"});
            this.cmbConstraint.Location = new System.Drawing.Point(3, 3);
            this.cmbConstraint.Name = "cmbConstraint";
            this.cmbConstraint.Size = new System.Drawing.Size(144, 21);
            this.cmbConstraint.TabIndex = 0;
            // 
            // txtFilterText
            // 
            this.txtFilterText.Location = new System.Drawing.Point(3, 30);
            this.txtFilterText.Name = "txtFilterText";
            this.txtFilterText.Size = new System.Drawing.Size(144, 20);
            this.txtFilterText.TabIndex = 1;
            // 
            // FullTextHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(150, 82);
            this.Controls.Add(this.txtFilterText);
            this.Controls.Add(this.cmbConstraint);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.cmdOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FullTextHeader";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ButtonExt cmdOk;
        private System.Windows.Forms.ButtonExt cmdClear;
        internal System.Windows.Forms.ComboBox cmbConstraint;
        internal System.Windows.Forms.TextBox txtFilterText;
    }
}
