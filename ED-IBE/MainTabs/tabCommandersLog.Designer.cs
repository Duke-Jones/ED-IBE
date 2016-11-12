namespace IBE.MTCommandersLog
{
    partial class tabCommandersLog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tabCommandersLog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbCL_LogEdit = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLogDistance = new System.Windows.Forms.TextBoxDoubleB();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdCL_DeleteEntry = new System.Windows.Forms.Button();
            this.cmdCL_Cancel = new System.Windows.Forms.Button();
            this.cmdCL_EditEntry = new System.Windows.Forms.Button();
            this.label94 = new System.Windows.Forms.Label();
            this.label95 = new System.Windows.Forms.Label();
            this.cmdCL_PrepareNew = new System.Windows.Forms.Button();
            this.cbLogSystemName = new System.Windows.Forms.ComboBox_ro();
            this.dtpLogEventDate = new System.Windows.Forms.DateTimePicker_ro();
            this.label21 = new System.Windows.Forms.Label();
            this.cbLogEventType = new System.Windows.Forms.ComboBox_ro();
            this.tbLogNotes = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cbLogCargoAction = new System.Windows.Forms.ComboBox_ro();
            this.cmdCL_Save = new System.Windows.Forms.Button();
            this.cbLogStationName = new System.Windows.Forms.ComboBox_ro();
            this.nbLogQuantity = new System.Windows.Forms.NumericUpDown_ro();
            this.label39 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.nbTransactionAmount = new System.Windows.Forms.NumericUpDown_ro();
            this.label38 = new System.Windows.Forms.Label();
            this.cbLogCargoName = new System.Windows.Forms.ComboBox_ro();
            this.label20 = new System.Windows.Forms.Label();
            this.nbCurrentCredits = new System.Windows.Forms.NumericUpDown_ro();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_ShowEditField = new System.Windows.Forms.CheckBox();
            this.cmsLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copySystemnameToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyStationnameToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRecalcJumpDistance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSendToEDSM = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.filterStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.showAllLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.bindNavCmdrsLog = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dgvCommandersLog = new IBE.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.systemname = new DataGridViewAutoFilter.DataGridViewAutoFilterFullTextBoxColumn();
            this.stationname = new DataGridViewAutoFilter.DataGridViewAutoFilterFullTextBoxColumn();
            this.eventtype = new DataGridViewAutoFilter.DataGridViewAutoFilterMultiTextBoxColumn();
            this.action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loccommodity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cargovolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credits_transaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credits_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notes = new DataGridViewAutoFilter.DataGridViewAutoFilterFullTextBoxColumn();
            this.gbCL_LogEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbLogQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTransactionAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbCurrentCredits)).BeginInit();
            this.cmsLog.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindNavCmdrsLog)).BeginInit();
            this.bindNavCmdrsLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandersLog)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCL_LogEdit
            // 
            this.gbCL_LogEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCL_LogEdit.Controls.Add(this.button1);
            this.gbCL_LogEdit.Controls.Add(this.label3);
            this.gbCL_LogEdit.Controls.Add(this.txtLogDistance);
            this.gbCL_LogEdit.Controls.Add(this.label2);
            this.gbCL_LogEdit.Controls.Add(this.cmdCL_DeleteEntry);
            this.gbCL_LogEdit.Controls.Add(this.cmdCL_Cancel);
            this.gbCL_LogEdit.Controls.Add(this.cmdCL_EditEntry);
            this.gbCL_LogEdit.Controls.Add(this.label94);
            this.gbCL_LogEdit.Controls.Add(this.label95);
            this.gbCL_LogEdit.Controls.Add(this.cmdCL_PrepareNew);
            this.gbCL_LogEdit.Controls.Add(this.cbLogSystemName);
            this.gbCL_LogEdit.Controls.Add(this.dtpLogEventDate);
            this.gbCL_LogEdit.Controls.Add(this.label21);
            this.gbCL_LogEdit.Controls.Add(this.cbLogEventType);
            this.gbCL_LogEdit.Controls.Add(this.tbLogNotes);
            this.gbCL_LogEdit.Controls.Add(this.label18);
            this.gbCL_LogEdit.Controls.Add(this.cbLogCargoAction);
            this.gbCL_LogEdit.Controls.Add(this.cmdCL_Save);
            this.gbCL_LogEdit.Controls.Add(this.cbLogStationName);
            this.gbCL_LogEdit.Controls.Add(this.nbLogQuantity);
            this.gbCL_LogEdit.Controls.Add(this.label39);
            this.gbCL_LogEdit.Controls.Add(this.label22);
            this.gbCL_LogEdit.Controls.Add(this.label19);
            this.gbCL_LogEdit.Controls.Add(this.nbTransactionAmount);
            this.gbCL_LogEdit.Controls.Add(this.label38);
            this.gbCL_LogEdit.Controls.Add(this.cbLogCargoName);
            this.gbCL_LogEdit.Controls.Add(this.label20);
            this.gbCL_LogEdit.Controls.Add(this.nbCurrentCredits);
            this.gbCL_LogEdit.Controls.Add(this.label1);
            this.gbCL_LogEdit.Location = new System.Drawing.Point(4, 31);
            this.gbCL_LogEdit.Name = "gbCL_LogEdit";
            this.gbCL_LogEdit.Size = new System.Drawing.Size(1201, 146);
            this.gbCL_LogEdit.TabIndex = 49;
            this.gbCL_LogEdit.TabStop = false;
            this.gbCL_LogEdit.Text = "Current Data";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(268, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 51;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "ly";
            // 
            // txtLogDistance
            // 
            this.txtLogDistance.DefaultValue = "";
            this.txtLogDistance.Digits = null;
            this.txtLogDistance.Format = "#,##0.0";
            this.txtLogDistance.Location = new System.Drawing.Point(69, 121);
            this.txtLogDistance.Name = "txtLogDistance";
            this.txtLogDistance.Size = new System.Drawing.Size(105, 20);
            this.txtLogDistance.TabIndex = 46;
            this.txtLogDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLogDistance.Value = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Distance";
            // 
            // cmdCL_DeleteEntry
            // 
            this.cmdCL_DeleteEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCL_DeleteEntry.Enabled = false;
            this.cmdCL_DeleteEntry.Location = new System.Drawing.Point(1061, 114);
            this.cmdCL_DeleteEntry.Name = "cmdCL_DeleteEntry";
            this.cmdCL_DeleteEntry.Size = new System.Drawing.Size(134, 23);
            this.cmdCL_DeleteEntry.TabIndex = 45;
            this.cmdCL_DeleteEntry.Text = "Delete";
            this.cmdCL_DeleteEntry.UseVisualStyleBackColor = false;
            this.cmdCL_DeleteEntry.Click += new System.EventHandler(this.cmdCL_DeleteEntry_Click);
            // 
            // cmdCL_Cancel
            // 
            this.cmdCL_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCL_Cancel.Location = new System.Drawing.Point(1061, 62);
            this.cmdCL_Cancel.Name = "cmdCL_Cancel";
            this.cmdCL_Cancel.Size = new System.Drawing.Size(134, 23);
            this.cmdCL_Cancel.TabIndex = 43;
            this.cmdCL_Cancel.Text = "Cancel";
            this.cmdCL_Cancel.UseVisualStyleBackColor = false;
            this.cmdCL_Cancel.Click += new System.EventHandler(this.cmdCL_Cancel_Click);
            // 
            // cmdCL_EditEntry
            // 
            this.cmdCL_EditEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCL_EditEntry.Location = new System.Drawing.Point(1061, 36);
            this.cmdCL_EditEntry.Name = "cmdCL_EditEntry";
            this.cmdCL_EditEntry.Size = new System.Drawing.Size(134, 23);
            this.cmdCL_EditEntry.TabIndex = 41;
            this.cmdCL_EditEntry.Text = "Edit";
            this.cmdCL_EditEntry.UseVisualStyleBackColor = false;
            this.cmdCL_EditEntry.Click += new System.EventHandler(this.cmdCL_EditEntry_Click);
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Location = new System.Drawing.Point(193, 98);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(19, 13);
            this.label94.TabIndex = 40;
            this.label94.Text = "cr.";
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.Location = new System.Drawing.Point(632, 98);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(19, 13);
            this.label95.TabIndex = 39;
            this.label95.Text = "cr.";
            // 
            // cmdCL_PrepareNew
            // 
            this.cmdCL_PrepareNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCL_PrepareNew.Location = new System.Drawing.Point(1061, 10);
            this.cmdCL_PrepareNew.Name = "cmdCL_PrepareNew";
            this.cmdCL_PrepareNew.Size = new System.Drawing.Size(134, 23);
            this.cmdCL_PrepareNew.TabIndex = 38;
            this.cmdCL_PrepareNew.Text = "Prepare New";
            this.cmdCL_PrepareNew.UseVisualStyleBackColor = true;
            this.cmdCL_PrepareNew.Click += new System.EventHandler(this.cmdCL_PrepareNew_Click);
            // 
            // cbLogSystemName
            // 
            this.cbLogSystemName.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cbLogSystemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cbLogSystemName.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cbLogSystemName.FormattingEnabled = true;
            this.cbLogSystemName.Location = new System.Drawing.Point(69, 41);
            this.cbLogSystemName.Name = "cbLogSystemName";
            this.cbLogSystemName.ReadOnly = false;
            this.cbLogSystemName.Size = new System.Drawing.Size(367, 21);
            this.cbLogSystemName.TabIndex = 9;
            this.cbLogSystemName.TextUpdate += new System.EventHandler(this.cbLogSystemName_TextUpdate);
            // 
            // dtpLogEventDate
            // 
            this.dtpLogEventDate.BackColor_ro = System.Drawing.SystemColors.Control;
            this.dtpLogEventDate.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.dtpLogEventDate.Location = new System.Drawing.Point(229, 15);
            this.dtpLogEventDate.Name = "dtpLogEventDate";
            this.dtpLogEventDate.ReadOnly = false;
            this.dtpLogEventDate.Size = new System.Drawing.Size(207, 20);
            this.dtpLogEventDate.TabIndex = 21;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(708, 17);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(35, 13);
            this.label21.TabIndex = 20;
            this.label21.Text = "Notes";
            // 
            // cbLogEventType
            // 
            this.cbLogEventType.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cbLogEventType.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cbLogEventType.FormattingEnabled = true;
            this.cbLogEventType.Location = new System.Drawing.Point(69, 14);
            this.cbLogEventType.Name = "cbLogEventType";
            this.cbLogEventType.ReadOnly = false;
            this.cbLogEventType.Size = new System.Drawing.Size(154, 21);
            this.cbLogEventType.TabIndex = 6;
            // 
            // tbLogNotes
            // 
            this.tbLogNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLogNotes.Location = new System.Drawing.Point(749, 17);
            this.tbLogNotes.Multiline = true;
            this.tbLogNotes.Name = "tbLogNotes";
            this.tbLogNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLogNotes.Size = new System.Drawing.Size(300, 120);
            this.tbLogNotes.TabIndex = 19;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(24, 17);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 7;
            this.label18.Text = "Event";
            // 
            // cbLogCargoAction
            // 
            this.cbLogCargoAction.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cbLogCargoAction.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cbLogCargoAction.FormattingEnabled = true;
            this.cbLogCargoAction.Items.AddRange(new object[] {
            "Bought",
            "Sold",
            "Mined",
            "Stolen"});
            this.cbLogCargoAction.Location = new System.Drawing.Point(511, 41);
            this.cbLogCargoAction.Name = "cbLogCargoAction";
            this.cbLogCargoAction.ReadOnly = false;
            this.cbLogCargoAction.Size = new System.Drawing.Size(191, 21);
            this.cbLogCargoAction.TabIndex = 18;
            // 
            // cmdCL_Save
            // 
            this.cmdCL_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCL_Save.Location = new System.Drawing.Point(1061, 88);
            this.cmdCL_Save.Name = "cmdCL_Save";
            this.cmdCL_Save.Size = new System.Drawing.Size(134, 23);
            this.cmdCL_Save.TabIndex = 34;
            this.cmdCL_Save.Text = "Save Changes";
            this.cmdCL_Save.UseVisualStyleBackColor = true;
            this.cmdCL_Save.Click += new System.EventHandler(this.cmdCL_Save_Click);
            // 
            // cbLogStationName
            // 
            this.cbLogStationName.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cbLogStationName.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cbLogStationName.FormattingEnabled = true;
            this.cbLogStationName.Location = new System.Drawing.Point(69, 68);
            this.cbLogStationName.Name = "cbLogStationName";
            this.cbLogStationName.ReadOnly = false;
            this.cbLogStationName.Size = new System.Drawing.Size(367, 21);
            this.cbLogStationName.TabIndex = 8;
            // 
            // nbLogQuantity
            // 
            this.nbLogQuantity.BackColor_ro = System.Drawing.SystemColors.Control;
            this.nbLogQuantity.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.nbLogQuantity.Location = new System.Drawing.Point(511, 69);
            this.nbLogQuantity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nbLogQuantity.Name = "nbLogQuantity";
            this.nbLogQuantity.Size = new System.Drawing.Size(120, 20);
            this.nbLogQuantity.TabIndex = 17;
            this.nbLogQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(20, 98);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(39, 13);
            this.label39.TabIndex = 33;
            this.label39.Text = "Capital";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(470, 17);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "Cargo";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(19, 71);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(40, 13);
            this.label19.TabIndex = 10;
            this.label19.Text = "Station";
            // 
            // nbTransactionAmount
            // 
            this.nbTransactionAmount.BackColor_ro = System.Drawing.SystemColors.Control;
            this.nbTransactionAmount.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.nbTransactionAmount.Location = new System.Drawing.Point(511, 95);
            this.nbTransactionAmount.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nbTransactionAmount.Name = "nbTransactionAmount";
            this.nbTransactionAmount.Size = new System.Drawing.Size(120, 20);
            this.nbTransactionAmount.TabIndex = 30;
            this.nbTransactionAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(442, 98);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(63, 13);
            this.label38.TabIndex = 32;
            this.label38.Text = "Transaction";
            // 
            // cbLogCargoName
            // 
            this.cbLogCargoName.BackColor_ro = System.Drawing.SystemColors.Control;
            this.cbLogCargoName.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.cbLogCargoName.FormattingEnabled = true;
            this.cbLogCargoName.Location = new System.Drawing.Point(511, 14);
            this.cbLogCargoName.Name = "cbLogCargoName";
            this.cbLogCargoName.ReadOnly = false;
            this.cbLogCargoName.Size = new System.Drawing.Size(191, 21);
            this.cbLogCargoName.TabIndex = 12;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(18, 44);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 13);
            this.label20.TabIndex = 11;
            this.label20.Text = "System";
            // 
            // nbCurrentCredits
            // 
            this.nbCurrentCredits.BackColor_ro = System.Drawing.SystemColors.Control;
            this.nbCurrentCredits.ForeColor_ro = System.Drawing.SystemColors.WindowText;
            this.nbCurrentCredits.Location = new System.Drawing.Point(69, 95);
            this.nbCurrentCredits.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nbCurrentCredits.Name = "nbCurrentCredits";
            this.nbCurrentCredits.Size = new System.Drawing.Size(120, 20);
            this.nbCurrentCredits.TabIndex = 31;
            this.nbCurrentCredits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(433, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "(utc)";
            // 
            // cb_ShowEditField
            // 
            this.cb_ShowEditField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_ShowEditField.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_ShowEditField.Location = new System.Drawing.Point(1065, 6);
            this.cb_ShowEditField.Name = "cb_ShowEditField";
            this.cb_ShowEditField.Size = new System.Drawing.Size(134, 23);
            this.cb_ShowEditField.TabIndex = 46;
            this.cb_ShowEditField.Text = "Show/Hide Edit-Panel";
            this.cb_ShowEditField.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_ShowEditField.UseVisualStyleBackColor = true;
            this.cb_ShowEditField.CheckedChanged += new System.EventHandler(this.cmdCL_ShowHide_CheckedChanged);
            // 
            // cmsLog
            // 
            this.cmsLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySystemnameToClipboardToolStripMenuItem,
            this.copyStationnameToClipboardToolStripMenuItem,
            this.toolStripMenuItem1,
            this.tsmiRecalcJumpDistance,
            this.toolStripMenuItem2,
            this.tsmiSendToEDSM});
            this.cmsLog.Name = "cmsLog";
            this.cmsLog.Size = new System.Drawing.Size(208, 104);
            // 
            // copySystemnameToClipboardToolStripMenuItem
            // 
            this.copySystemnameToClipboardToolStripMenuItem.Name = "copySystemnameToClipboardToolStripMenuItem";
            this.copySystemnameToClipboardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.copySystemnameToClipboardToolStripMenuItem.Text = "Copy systemname to clipboard";
            this.copySystemnameToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySystemnameToClipboardToolStripMenuItem_Click);
            // 
            // copyStationnameToClipboardToolStripMenuItem
            // 
            this.copyStationnameToClipboardToolStripMenuItem.Name = "copyStationnameToClipboardToolStripMenuItem";
            this.copyStationnameToClipboardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.copyStationnameToClipboardToolStripMenuItem.Text = "Copy stationname to clipboard";
            this.copyStationnameToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyStationnameToClipboardToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(204, 6);
            // 
            // tsmiRecalcJumpDistance
            // 
            this.tsmiRecalcJumpDistance.Name = "tsmiRecalcJumpDistance";
            this.tsmiRecalcJumpDistance.Size = new System.Drawing.Size(207, 22);
            this.tsmiRecalcJumpDistance.Text = "recalculate jump distance";
            this.tsmiRecalcJumpDistance.Click += new System.EventHandler(this.tsmiRecalcJumpDistance_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(204, 6);
            // 
            // tsmiSendToEDSM
            // 
            this.tsmiSendToEDSM.Name = "tsmiSendToEDSM";
            this.tsmiSendToEDSM.Size = new System.Drawing.Size(207, 22);
            this.tsmiSendToEDSM.Text = "send log entry(s) to EDSM";
            this.tsmiSendToEDSM.Click += new System.EventHandler(this.tsmiSendToEDSM_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterStatusLabel,
            this.showAllLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 597);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1208, 22);
            this.statusStrip1.TabIndex = 50;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // filterStatusLabel
            // 
            this.filterStatusLabel.Name = "filterStatusLabel";
            this.filterStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.filterStatusLabel.Visible = false;
            // 
            // showAllLabel
            // 
            this.showAllLabel.IsLink = true;
            this.showAllLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.showAllLabel.Name = "showAllLabel";
            this.showAllLabel.Size = new System.Drawing.Size(42, 17);
            this.showAllLabel.Text = "Show &All";
            this.showAllLabel.Visible = false;
            // 
            // bindNavCmdrsLog
            // 
            this.bindNavCmdrsLog.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindNavCmdrsLog.CountItem = this.bindingNavigatorCountItem;
            this.bindNavCmdrsLog.CountItemFormat = "of {0}";
            this.bindNavCmdrsLog.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindNavCmdrsLog.Dock = System.Windows.Forms.DockStyle.None;
            this.bindNavCmdrsLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindNavCmdrsLog.Location = new System.Drawing.Point(4, 180);
            this.bindNavCmdrsLog.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindNavCmdrsLog.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindNavCmdrsLog.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindNavCmdrsLog.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindNavCmdrsLog.Name = "bindNavCmdrsLog";
            this.bindNavCmdrsLog.PositionItem = this.bindingNavigatorPositionItem;
            this.bindNavCmdrsLog.Size = new System.Drawing.Size(279, 25);
            this.bindNavCmdrsLog.TabIndex = 51;
            this.bindNavCmdrsLog.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Neu hinzufügen";
            this.bindingNavigatorAddNewItem.Visible = false;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(28, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Elements in Commander\'s Log";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Löschen";
            this.bindingNavigatorDeleteItem.Visible = false;
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Jump to first entry";
            this.bindingNavigatorMoveFirstItem.Click += new System.EventHandler(this.bindingNavigatorMoveFirstItem_Click);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Next entry";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 21);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bindingNavigatorPositionItem.ToolTipText = "Current log item";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Previous entry";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Jump to last entry";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // dgvCommandersLog
            // 
            this.dgvCommandersLog.AllowUserToAddRows = false;
            this.dgvCommandersLog.AllowUserToDeleteRows = false;
            this.dgvCommandersLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCommandersLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCommandersLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCommandersLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.systemname,
            this.stationname,
            this.eventtype,
            this.action,
            this.loccommodity,
            this.cargovolume,
            this.credits_transaction,
            this.credits_total,
            this.distance,
            this.notes});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCommandersLog.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvCommandersLog.DoubleBuffer = true;
            this.dgvCommandersLog.Location = new System.Drawing.Point(3, 207);
            this.dgvCommandersLog.Name = "dgvCommandersLog";
            this.dgvCommandersLog.ReadOnly = true;
            this.dgvCommandersLog.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCommandersLog.RowTemplate.Height = 33;
            this.dgvCommandersLog.Size = new System.Drawing.Size(1202, 409);
            this.dgvCommandersLog.TabIndex = 44;
            this.dgvCommandersLog.Tag = "CommandersLog;1";
            this.dgvCommandersLog.ColumnSorted += new System.EventHandler<IBE.Enums_and_Utility_Classes.DataGridViewExt.SortedEventArgs>(this.dgvCommandersLog_ColumnSorted);
            this.dgvCommandersLog.SelectionChanged += new System.EventHandler(this.dgvCommandersLog_SelectionChanged);
            this.dgvCommandersLog.Click += new System.EventHandler(this.dgvCommandersLog_Click);
            this.dgvCommandersLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCommandersLog_KeyDown);
            // 
            // time
            // 
            this.time.DataPropertyName = "time";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.time.DefaultCellStyle = dataGridViewCellStyle1;
            this.time.HeaderText = "Time";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            this.time.Width = 70;
            // 
            // systemname
            // 
            this.systemname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.systemname.DataPropertyName = "systemname";
            this.systemname.HeaderText = "System";
            this.systemname.Name = "systemname";
            this.systemname.ReadOnly = true;
            this.systemname.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // stationname
            // 
            this.stationname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.stationname.DataPropertyName = "stationname";
            this.stationname.HeaderText = "Station";
            this.stationname.Name = "stationname";
            this.stationname.ReadOnly = true;
            this.stationname.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // eventtype
            // 
            this.eventtype.DataPropertyName = "eventtype";
            this.eventtype.HeaderText = "Event";
            this.eventtype.Name = "eventtype";
            this.eventtype.ReadOnly = true;
            this.eventtype.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // action
            // 
            this.action.DataPropertyName = "cargoaction";
            this.action.HeaderText = "Action";
            this.action.Name = "action";
            this.action.ReadOnly = true;
            // 
            // loccommodity
            // 
            this.loccommodity.DataPropertyName = "loccommodity";
            this.loccommodity.HeaderText = "Commodity";
            this.loccommodity.Name = "loccommodity";
            this.loccommodity.ReadOnly = true;
            // 
            // cargovolume
            // 
            this.cargovolume.DataPropertyName = "cargovolume";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.cargovolume.DefaultCellStyle = dataGridViewCellStyle2;
            this.cargovolume.HeaderText = "Volume";
            this.cargovolume.Name = "cargovolume";
            this.cargovolume.ReadOnly = true;
            this.cargovolume.Width = 70;
            // 
            // credits_transaction
            // 
            this.credits_transaction.DataPropertyName = "credits_transaction";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.credits_transaction.DefaultCellStyle = dataGridViewCellStyle3;
            this.credits_transaction.HeaderText = "Transaction";
            this.credits_transaction.Name = "credits_transaction";
            this.credits_transaction.ReadOnly = true;
            this.credits_transaction.Width = 70;
            // 
            // credits_total
            // 
            this.credits_total.DataPropertyName = "credits_total";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.credits_total.DefaultCellStyle = dataGridViewCellStyle4;
            this.credits_total.HeaderText = "Cr. total";
            this.credits_total.Name = "credits_total";
            this.credits_total.ReadOnly = true;
            // 
            // distance
            // 
            this.distance.DataPropertyName = "distance";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle5.Format = "N1";
            dataGridViewCellStyle5.NullValue = null;
            this.distance.DefaultCellStyle = dataGridViewCellStyle5;
            this.distance.HeaderText = "Distance";
            this.distance.Name = "distance";
            this.distance.ReadOnly = true;
            // 
            // notes
            // 
            this.notes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.notes.DataPropertyName = "notes";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.notes.DefaultCellStyle = dataGridViewCellStyle6;
            this.notes.HeaderText = "notes";
            this.notes.Name = "notes";
            this.notes.ReadOnly = true;
            this.notes.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // tabCommandersLog
            // 
            this.Controls.Add(this.bindNavCmdrsLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gbCL_LogEdit);
            this.Controls.Add(this.dgvCommandersLog);
            this.Controls.Add(this.cb_ShowEditField);
            this.Name = "tabCommandersLog";
            this.Size = new System.Drawing.Size(1208, 619);
            this.gbCL_LogEdit.ResumeLayout(false);
            this.gbCL_LogEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbLogQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTransactionAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbCurrentCredits)).EndInit();
            this.cmsLog.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindNavCmdrsLog)).EndInit();
            this.bindNavCmdrsLog.ResumeLayout(false);
            this.bindNavCmdrsLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandersLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCL_LogEdit;
        private System.Windows.Forms.Button cmdCL_Cancel;
        private System.Windows.Forms.Button cmdCL_EditEntry;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.Label label95;
        public System.Windows.Forms.Button cmdCL_PrepareNew;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tbLogNotes;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button cmdCL_Save;
        private System.Windows.Forms.NumericUpDown_ro nbLogQuantity;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown_ro nbTransactionAmount;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown_ro nbCurrentCredits;
        private System.Windows.Forms.CheckBox cb_ShowEditField;
        internal System.Windows.Forms.DateTimePicker_ro dtpLogEventDate;
        internal System.Windows.Forms.ComboBox_ro cbLogEventType;
        internal System.Windows.Forms.ComboBox_ro cbLogCargoAction;
        internal System.Windows.Forms.ComboBox_ro cbLogStationName;
        internal System.Windows.Forms.ComboBox_ro cbLogCargoName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdCL_DeleteEntry;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBoxDoubleB txtLogDistance;
        private System.Windows.Forms.Label label2;
        internal Enums_and_Utility_Classes.DataGridViewExt dgvCommandersLog;
        internal System.Windows.Forms.ComboBox_ro cbLogSystemName;
        private System.Windows.Forms.ContextMenuStrip cmsLog;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecalcJumpDistance;
        private System.Windows.Forms.ToolStripMenuItem tsmiSendToEDSM;
        private System.Windows.Forms.ToolStripMenuItem copySystemnameToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyStationnameToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel filterStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel showAllLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private DataGridViewAutoFilter.DataGridViewAutoFilterFullTextBoxColumn systemname;
        private DataGridViewAutoFilter.DataGridViewAutoFilterFullTextBoxColumn stationname;
        private DataGridViewAutoFilter.DataGridViewAutoFilterMultiTextBoxColumn eventtype;
        private System.Windows.Forms.DataGridViewTextBoxColumn action;
        private System.Windows.Forms.DataGridViewTextBoxColumn loccommodity;
        private System.Windows.Forms.DataGridViewTextBoxColumn cargovolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn credits_transaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn credits_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn distance;
        private DataGridViewAutoFilter.DataGridViewAutoFilterFullTextBoxColumn notes;
        internal System.Windows.Forms.BindingNavigator bindNavCmdrsLog;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
    }
}
