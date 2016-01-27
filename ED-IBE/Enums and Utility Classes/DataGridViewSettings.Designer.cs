namespace RegulatedNoise.Enums_and_Utility_Classes
{
    partial class DataGridViewSettings
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.dgvColumns = new RegulatedNoise.Enums_and_Utility_Classes.DataGridViewExt(this.components);
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisplayIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colAutoSizeMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFillWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMinimumWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Location = new System.Drawing.Point(536, 238);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 1;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(617, 238);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // dgvColumns
            // 
            this.dgvColumns.AllowUserToAddRows = false;
            this.dgvColumns.AllowUserToDeleteRows = false;
            this.dgvColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colCaption,
            this.colDisplayIndex,
            this.colVisible,
            this.colAutoSizeMode,
            this.colWidth,
            this.colFillWeight,
            this.colMinimumWidth});
            this.dgvColumns.DoubleBuffer = true;
            this.dgvColumns.Location = new System.Drawing.Point(12, 12);
            this.dgvColumns.Name = "dgvColumns";
            this.dgvColumns.Size = new System.Drawing.Size(680, 220);
            this.dgvColumns.TabIndex = 0;
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Visible = false;
            // 
            // colCaption
            // 
            this.colCaption.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCaption.HeaderText = "Caption";
            this.colCaption.Name = "colCaption";
            this.colCaption.ReadOnly = true;
            // 
            // colDisplayIndex
            // 
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.colDisplayIndex.DefaultCellStyle = dataGridViewCellStyle1;
            this.colDisplayIndex.HeaderText = "Displayindex";
            this.colDisplayIndex.Name = "colDisplayIndex";
            this.colDisplayIndex.Width = 80;
            // 
            // colVisible
            // 
            this.colVisible.HeaderText = "Visible";
            this.colVisible.Name = "colVisible";
            this.colVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colVisible.Width = 60;
            // 
            // colAutoSizeMode
            // 
            this.colAutoSizeMode.HeaderText = "AutoSizeMode";
            this.colAutoSizeMode.Items.AddRange(new object[] {
            "AllCells",
            "AllCellsExceptHeader",
            "ColumnHeader",
            "DisplayedCells",
            "DisplayedCellsExceptHeader",
            "Fill",
            "None",
            "NotSet"});
            this.colAutoSizeMode.Name = "colAutoSizeMode";
            this.colAutoSizeMode.Width = 120;
            // 
            // colWidth
            // 
            dataGridViewCellStyle2.Format = "N0";
            this.colWidth.DefaultCellStyle = dataGridViewCellStyle2;
            this.colWidth.HeaderText = "Width";
            this.colWidth.Name = "colWidth";
            this.colWidth.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWidth.Width = 60;
            // 
            // colFillWeight
            // 
            dataGridViewCellStyle3.Format = "N1";
            dataGridViewCellStyle3.NullValue = null;
            this.colFillWeight.DefaultCellStyle = dataGridViewCellStyle3;
            this.colFillWeight.HeaderText = "FillWeight";
            this.colFillWeight.Name = "colFillWeight";
            this.colFillWeight.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFillWeight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFillWeight.Width = 60;
            // 
            // colMinimumWidth
            // 
            dataGridViewCellStyle4.Format = "N0";
            this.colMinimumWidth.DefaultCellStyle = dataGridViewCellStyle4;
            this.colMinimumWidth.HeaderText = "MinimumWidth";
            this.colMinimumWidth.Name = "colMinimumWidth";
            // 
            // DataGridViewSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 273);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.dgvColumns);
            this.Name = "DataGridViewSettings";
            this.Text = "Select columns";
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewExt dgvColumns;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisplayIndex;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAutoSizeMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFillWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMinimumWidth;
    }
}