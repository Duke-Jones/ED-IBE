namespace IBE
{
    partial class GUIColorsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUIColorsView));
            this.cmdClose = new System.Windows.Forms.ButtonExt();
            this.label1 = new System.Windows.Forms.Label();
            this.cbActivated = new System.Windows.Forms.CheckBox();
            this.cmdResetColors = new System.Windows.Forms.ButtonExt();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.paColor_0 = new System.Windows.Forms.Panel();
            this.lblColorName_0 = new System.Windows.Forms.Label();
            this.pbColor_0 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbPresetElite = new System.Windows.Forms.RadioButton();
            this.rbPresetDefault = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.paColor_0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor_0)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(306, 28);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(159, 44);
            this.cmdClose.TabIndex = 1;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(256, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 247);
            this.label1.TabIndex = 51;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // cbActivated
            // 
            this.cbActivated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbActivated.AutoSize = true;
            this.cbActivated.Location = new System.Drawing.Point(259, 97);
            this.cbActivated.Name = "cbActivated";
            this.cbActivated.Size = new System.Drawing.Size(111, 17);
            this.cbActivated.TabIndex = 50;
            this.cbActivated.Text = "Activate Retheme";
            this.cbActivated.UseVisualStyleBackColor = true;
            // 
            // cmdResetColors
            // 
            this.cmdResetColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdResetColors.Location = new System.Drawing.Point(31, 74);
            this.cmdResetColors.Name = "cmdResetColors";
            this.cmdResetColors.Size = new System.Drawing.Size(159, 44);
            this.cmdResetColors.TabIndex = 49;
            this.cmdResetColors.Text = "Get Preset Colors";
            this.cmdResetColors.UseVisualStyleBackColor = true;
            this.cmdResetColors.Click += new System.EventHandler(this.cmdResetColors_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 574);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Colors";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.paColor_0);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(229, 555);
            this.flowLayoutPanel1.TabIndex = 39;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // paColor_0
            // 
            this.paColor_0.Controls.Add(this.lblColorName_0);
            this.paColor_0.Controls.Add(this.pbColor_0);
            this.paColor_0.Location = new System.Drawing.Point(3, 3);
            this.paColor_0.Name = "paColor_0";
            this.paColor_0.Size = new System.Drawing.Size(210, 30);
            this.paColor_0.TabIndex = 38;
            // 
            // lblColorName_0
            // 
            this.lblColorName_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorName_0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorName_0.Location = new System.Drawing.Point(24, 5);
            this.lblColorName_0.Name = "lblColorName_0";
            this.lblColorName_0.Size = new System.Drawing.Size(124, 19);
            this.lblColorName_0.TabIndex = 33;
            this.lblColorName_0.Text = "Foreground";
            this.lblColorName_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbColor_0
            // 
            this.pbColor_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbColor_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbColor_0.Location = new System.Drawing.Point(166, 3);
            this.pbColor_0.Name = "pbColor_0";
            this.pbColor_0.Size = new System.Drawing.Size(41, 24);
            this.pbColor_0.TabIndex = 31;
            this.pbColor_0.TabStop = false;
            this.pbColor_0.Click += new System.EventHandler(this.pbColor_0_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdResetColors);
            this.groupBox2.Controls.Add(this.rbPresetElite);
            this.groupBox2.Controls.Add(this.rbPresetDefault);
            this.groupBox2.Location = new System.Drawing.Point(271, 396);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 147);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Active \"Default Colors\" Preset";
            // 
            // rbPresetElite
            // 
            this.rbPresetElite.AutoSize = true;
            this.rbPresetElite.Location = new System.Drawing.Point(57, 51);
            this.rbPresetElite.Name = "rbPresetElite";
            this.rbPresetElite.Size = new System.Drawing.Size(100, 17);
            this.rbPresetElite.TabIndex = 1;
            this.rbPresetElite.TabStop = true;
            this.rbPresetElite.Tag = "2";
            this.rbPresetElite.Text = "Elite Dangerous";
            this.rbPresetElite.UseVisualStyleBackColor = true;
            // 
            // rbPresetDefault
            // 
            this.rbPresetDefault.AutoSize = true;
            this.rbPresetDefault.Checked = true;
            this.rbPresetDefault.Location = new System.Drawing.Point(57, 28);
            this.rbPresetDefault.Name = "rbPresetDefault";
            this.rbPresetDefault.Size = new System.Drawing.Size(59, 17);
            this.rbPresetDefault.TabIndex = 0;
            this.rbPresetDefault.TabStop = true;
            this.rbPresetDefault.Tag = "1";
            this.rbPresetDefault.Text = "Default";
            this.rbPresetDefault.UseVisualStyleBackColor = true;
            this.rbPresetDefault.CheckedChanged += new System.EventHandler(this.rbPreset_CheckedChanged);
            // 
            // GUIColorsView
            // 
            this.AcceptButton = this.cmdClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(511, 598);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbActivated);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(527, 636);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(527, 636);
            this.Name = "GUIColorsView";
            this.Text = "GUI Colors";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUIColorsView_FormClosing);
            this.Load += new System.EventHandler(this.GUIColorsView_Load);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.paColor_0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbColor_0)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel paColor_0;
        private System.Windows.Forms.PictureBox pbColor_0;
        private System.Windows.Forms.ButtonExt cmdClose;
        private System.Windows.Forms.Label lblColorName_0;
        private System.Windows.Forms.ButtonExt cmdResetColors;
        private System.Windows.Forms.CheckBox cbActivated;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbPresetElite;
        private System.Windows.Forms.RadioButton rbPresetDefault;
    }
}
