namespace IBE.EDSM
{
    partial class EDStarmapInterfaceView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDStarmapInterfaceView));
            this.label5 = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.ButtonExt();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAccountstatus = new System.Windows.Forms.TextBox();
            this.cmdClear = new System.Windows.Forms.ButtonExt();
            this.cmdSave = new System.Windows.Forms.ButtonExt();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAPIKey = new System.Windows.Forms.TextBox();
            this.txtServerstatus = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCmdrName = new System.Windows.Forms.TextBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.cbSaveToFile = new System.Windows.Forms.CheckBox();
            this.cbSendToEDSM = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Serverstatus";
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(420, 12);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 0;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtAccountstatus);
            this.groupBox1.Controls.Add(this.cmdClear);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmdSave);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtAPIKey);
            this.groupBox1.Controls.Add(this.txtServerstatus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCmdrName);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 206);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EDSM Account";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Accountstatus";
            // 
            // txtAccountstatus
            // 
            this.txtAccountstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountstatus.Location = new System.Drawing.Point(23, 167);
            this.txtAccountstatus.Name = "txtAccountstatus";
            this.txtAccountstatus.Size = new System.Drawing.Size(354, 22);
            this.txtAccountstatus.TabIndex = 18;
            this.txtAccountstatus.TabStop = false;
            this.txtAccountstatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(286, 78);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(99, 23);
            this.cmdClear.TabIndex = 3;
            this.cmdClear.Text = "Clear Profile";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(286, 35);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(99, 23);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "API Key";
            // 
            // txtAPIKey
            // 
            this.txtAPIKey.Location = new System.Drawing.Point(27, 80);
            this.txtAPIKey.Name = "txtAPIKey";
            this.txtAPIKey.Size = new System.Drawing.Size(238, 20);
            this.txtAPIKey.TabIndex = 1;
            this.txtAPIKey.Tag = "API_Key;EMPTY";
            this.txtAPIKey.UseSystemPasswordChar = true;
            // 
            // txtServerstatus
            // 
            this.txtServerstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerstatus.Location = new System.Drawing.Point(24, 129);
            this.txtServerstatus.Name = "txtServerstatus";
            this.txtServerstatus.Size = new System.Drawing.Size(354, 22);
            this.txtServerstatus.TabIndex = 14;
            this.txtServerstatus.TabStop = false;
            this.txtServerstatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Commander Name";
            // 
            // txtCmdrName
            // 
            this.txtCmdrName.Location = new System.Drawing.Point(27, 37);
            this.txtCmdrName.Name = "txtCmdrName";
            this.txtCmdrName.Size = new System.Drawing.Size(238, 20);
            this.txtCmdrName.TabIndex = 0;
            this.txtCmdrName.Tag = "CommandersName;EMPTY";
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.cbSaveToFile);
            this.gbSettings.Controls.Add(this.cbSendToEDSM);
            this.gbSettings.Location = new System.Drawing.Point(12, 218);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(401, 93);
            this.gbSettings.TabIndex = 18;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // cbSaveToFile
            // 
            this.cbSaveToFile.AutoSize = true;
            this.cbSaveToFile.Location = new System.Drawing.Point(27, 58);
            this.cbSaveToFile.Name = "cbSaveToFile";
            this.cbSaveToFile.Size = new System.Drawing.Size(134, 17);
            this.cbSaveToFile.TabIndex = 1;
            this.cbSaveToFile.Tag = "SaveToFile;False";
            this.cbSaveToFile.Text = "log route to file (debug)";
            this.cbSaveToFile.UseVisualStyleBackColor = true;
            this.cbSaveToFile.CheckedChanged += new System.EventHandler(this.cbCheckedChanged);
            // 
            // cbSendToEDSM
            // 
            this.cbSendToEDSM.AutoSize = true;
            this.cbSendToEDSM.Location = new System.Drawing.Point(27, 35);
            this.cbSendToEDSM.Name = "cbSendToEDSM";
            this.cbSendToEDSM.Size = new System.Drawing.Size(122, 17);
            this.cbSendToEDSM.TabIndex = 0;
            this.cbSendToEDSM.Tag = "SendToEDSM;True";
            this.cbSendToEDSM.Text = "send route to EDSM";
            this.cbSendToEDSM.UseVisualStyleBackColor = true;
            this.cbSendToEDSM.CheckedChanged += new System.EventHandler(this.cbCheckedChanged);
            // 
            // EDStarmapInterfaceView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(505, 323);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(521, 361);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(521, 361);
            this.Name = "EDStarmapInterfaceView";
            this.Text = "ED Star Map - Interface";
            this.Load += new System.EventHandler(this.EDStarmapInterfaceView_Load);
            this.Shown += new System.EventHandler(this.EDStarmapInterfaceView_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ButtonExt cmdClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ButtonExt cmdClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAPIKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCmdrName;
        private System.Windows.Forms.TextBox txtServerstatus;
        private System.Windows.Forms.ButtonExt cmdSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAccountstatus;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.CheckBox cbSaveToFile;
        private System.Windows.Forms.CheckBox cbSendToEDSM;
    }
}
