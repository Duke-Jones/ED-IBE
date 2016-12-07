namespace DataGridViewAutoFilter
{
    partial class DateTimeHeader
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
            this.dtpAfter = new System.Windows.Forms.DateTimePicker();
            this.dtpBefore = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpAfterTime = new System.Windows.Forms.DateTimePicker();
            this.dtpBeforeTime = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdOk.Location = new System.Drawing.Point(63, 138);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(46, 22);
            this.cmdOk.TabIndex = 2;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            // 
            // cmdClear
            // 
            this.cmdClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdClear.Location = new System.Drawing.Point(11, 138);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(46, 22);
            this.cmdClear.TabIndex = 3;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // dtpAfter
            // 
            this.dtpAfter.Checked = false;
            this.dtpAfter.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAfter.Location = new System.Drawing.Point(6, 23);
            this.dtpAfter.Name = "dtpAfter";
            this.dtpAfter.ShowCheckBox = true;
            this.dtpAfter.Size = new System.Drawing.Size(103, 20);
            this.dtpAfter.TabIndex = 4;
            this.dtpAfter.ValueChanged += new System.EventHandler(this.dtpAfter_ValueChanged);
            // 
            // dtpBefore
            // 
            this.dtpBefore.Checked = false;
            this.dtpBefore.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBefore.Location = new System.Drawing.Point(6, 83);
            this.dtpBefore.Name = "dtpBefore";
            this.dtpBefore.ShowCheckBox = true;
            this.dtpBefore.Size = new System.Drawing.Size(103, 20);
            this.dtpBefore.TabIndex = 5;
            this.dtpBefore.ValueChanged += new System.EventHandler(this.dtpBefore_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "from:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "to:";
            // 
            // dtpAfterTime
            // 
            this.dtpAfterTime.Checked = false;
            this.dtpAfterTime.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpAfterTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpAfterTime.Location = new System.Drawing.Point(25, 49);
            this.dtpAfterTime.Name = "dtpAfterTime";
            this.dtpAfterTime.ShowUpDown = true;
            this.dtpAfterTime.Size = new System.Drawing.Size(84, 20);
            this.dtpAfterTime.TabIndex = 8;
            this.dtpAfterTime.ValueChanged += new System.EventHandler(this.dtpTime_ValueChanged);
            // 
            // dtpBeforeTime
            // 
            this.dtpBeforeTime.Checked = false;
            this.dtpBeforeTime.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpBeforeTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpBeforeTime.Location = new System.Drawing.Point(25, 109);
            this.dtpBeforeTime.Name = "dtpBeforeTime";
            this.dtpBeforeTime.ShowUpDown = true;
            this.dtpBeforeTime.Size = new System.Drawing.Size(84, 20);
            this.dtpBeforeTime.TabIndex = 9;
            this.dtpBeforeTime.ValueChanged += new System.EventHandler(this.dtpTime_ValueChanged);
            // 
            // DateTimeHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(119, 167);
            this.Controls.Add(this.dtpBeforeTime);
            this.Controls.Add(this.dtpAfterTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpBefore);
            this.Controls.Add(this.dtpAfter);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.cmdOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "DateTimeHeader";
            this.ShowInTaskbar = false;
            this.Shown += new System.EventHandler(this.DateTimeHeader_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ButtonExt cmdOk;
        private System.Windows.Forms.ButtonExt cmdClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.DateTimePicker dtpAfter;
        internal System.Windows.Forms.DateTimePicker dtpBefore;
        internal System.Windows.Forms.DateTimePicker dtpAfterTime;
        internal System.Windows.Forms.DateTimePicker dtpBeforeTime;
    }
}
