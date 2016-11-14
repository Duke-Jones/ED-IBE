namespace DataGridViewAutoFilter
{
    partial class MultiSelectHeaderList
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
            this.FilterListBox = new System.Windows.Forms.CheckedListBox();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdInvert = new System.Windows.Forms.Button();
            this.cmdAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FilterListBox
            // 
            this.FilterListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterListBox.CheckOnClick = true;
            this.FilterListBox.FormattingEnabled = true;
            this.FilterListBox.Location = new System.Drawing.Point(3, 3);
            this.FilterListBox.Name = "FilterListBox";
            this.FilterListBox.Size = new System.Drawing.Size(142, 109);
            this.FilterListBox.TabIndex = 0;
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdOk.Location = new System.Drawing.Point(102, 112);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(42, 22);
            this.cmdOk.TabIndex = 1;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            // 
            // cmdInvert
            // 
            this.cmdInvert.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdInvert.Location = new System.Drawing.Point(4, 112);
            this.cmdInvert.Name = "cmdInvert";
            this.cmdInvert.Size = new System.Drawing.Size(42, 22);
            this.cmdInvert.TabIndex = 3;
            this.cmdInvert.Text = "Invert";
            this.cmdInvert.UseVisualStyleBackColor = true;
            this.cmdInvert.Click += new System.EventHandler(this.cmdInvert_Click);
            // 
            // cmdAll
            // 
            this.cmdAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdAll.Location = new System.Drawing.Point(53, 112);
            this.cmdAll.Name = "cmdAll";
            this.cmdAll.Size = new System.Drawing.Size(42, 22);
            this.cmdAll.TabIndex = 2;
            this.cmdAll.Text = "All";
            this.cmdAll.UseVisualStyleBackColor = true;
            this.cmdAll.Click += new System.EventHandler(this.cmdAll_Click);
            // 
            // MultiSelectHeaderList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(148, 139);
            this.Controls.Add(this.cmdAll);
            this.Controls.Add(this.cmdInvert);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.FilterListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MultiSelectHeaderList";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cmdInvert;
        internal System.Windows.Forms.CheckedListBox FilterListBox;
        internal System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdAll;
    }
}
