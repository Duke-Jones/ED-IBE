using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBE.SQL;
using System.Diagnostics;
using IBE.SQL.Datasets;
using System.Globalization;
using CodeProject.Dialog;
using IBE.EDDB_Data;
using System.IO;
using IBE.Enums_and_Utility_Classes;

namespace IBE.MTSettings
{
    public partial class tabSettings : UserControl
    {

        #region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<EventArgs> SettingChangedEvent;

        protected virtual void OnSettingChanged(EventArgs e)
        {
            EventHandler<EventArgs> myEvent = SettingChangedEvent;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        #endregion

        public const String        DB_GROUPNAME                    = "Settings";

        private Settings            m_DataSource;                   // data object

        private Int32               m_InitialTopOfGrid;
        private Int32               m_InitialTopOfEditGroupBox;

        private Boolean             m_CellValueNeededIsRegistered   = false;        // true if the event is already registred
        private Boolean             m_FirstRowShown                 = false;        // true after first time shown
        private DBGuiInterface      m_GUIInterface;

        /// <summary>
        /// Constructor
        /// </summary>
        public tabSettings()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            this.Name = "tabSettings";
        }

        /// <summary>
        /// sets or gets the data object
        /// </summary>
        public Settings DataSource
        {
            get
            {
                return m_DataSource;
            }
            set
            {
                m_DataSource     = value;

                if((m_DataSource != null) && (m_DataSource.GUI != this))
                { 
                    if(m_DataSource.GUI != null)
                        m_DataSource.DataChanged -= m_DataSource_DataChanged;

                    m_DataSource.GUI = this;

                    m_DataSource.DataChanged += m_DataSource_DataChanged;
                }
            }
        }

        /// <summary>
        /// initialization of the whole log
        /// </summary>
        public void Init()
        {
            Cursor oldCursor = Cursor;
            DataTable Data;

            try
            {
                Cursor = Cursors.WaitCursor;

                // loading languages to combobox
                Data = new DataTable();
                Program.DBCon.Execute("select * from tbLanguage", Data);
                cmbLanguage.DataSource      = Data;
                cmbLanguage.DisplayMember   = "language";
                cmbLanguage.ValueMember     = "language";

                //prepare visited filter
                Tuple<Int32, String> newEntry;
                newEntry = new Tuple<Int32, String>(0,"show all");
                cmbVisitedFilter.Items.Add(newEntry);
                newEntry = new Tuple<Int32, String>(1,"only visited systems");
                cmbVisitedFilter.Items.Add(newEntry);
                newEntry = new Tuple<Int32, String>(2,"only visited stations");
                cmbVisitedFilter.Items.Add(newEntry);
                cmbVisitedFilter.DisplayMember   = "Item2";
                cmbVisitedFilter.ValueMember     = "Item1";

                // loading all settings
                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, new DBConnector(Program.DBCon.ConfigData, true));
                m_GUIInterface.loadAllSettings(this);


                m_GUIInterface.DataSavedEvent += m_GUIInterface_DataSavedEvent;

                Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                Cursor = oldCursor;
                throw new Exception("Error during initialization the commanders log tab", ex);
            }
        }

        void m_GUIInterface_DataSavedEvent(object sender, EventArgs e)
        {
            SettingChangedEvent.Raise(this, e);
        }

        /// <summary>
        /// the data object informs the gui about changed data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_DataSource_DataChanged(object sender, Settings.DataChangedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in m_DataSource_DataChanged");
            }
        }

        /// <summary>
        /// prepares the "Language" combobox
        /// </summary>
        private void setLanguageCombobox()
        {
            List<enumBindTo> lstEnum = new List<enumBindTo>();
            Array Names;

            // Speicherstruktur
            lstEnum.Clear();
            Names = Enum.GetValues(Type.GetType("RegulatedNoise.enLanguage", true));

            for (int i = 0; i <= Names.GetUpperBound(0); i++)
            {
                enumBindTo cls = new enumBindTo();

                cls.EnumValue = (Int32)Names.GetValue(i);
                cls.EnumString = Names.GetValue(i).ToString();

                lstEnum.Add(cls);
            }

            cmbLanguage.ValueMember = "EnumValue";
            cmbLanguage.DisplayMember = "EnumString";
            cmbLanguage.DataSource = lstEnum;

        }

        /// <summary>
        /// selects another "traineddata" file for TesseractOCR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSelectTraineddataFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OCRFile = new OpenFileDialog();

            OCRFile.Filter = "Tesseract-Files|*.traineddata|All Files|*.*";
            OCRFile.FileName = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "TraineddataFile");
            OCRFile.InitialDirectory = Program.GetDataPath("tessdata");  
            OCRFile.Title = "select Tesseract Traineddata-File...";

            if (OCRFile.ShowDialog(this) == DialogResult.OK)
            {
                txtOCRTraineddataFile.Text = System.IO.Path.GetFileNameWithoutExtension(OCRFile.FileName);
                m_GUIInterface.saveSetting(txtOCRTraineddataFile);
            }

        }

        private void txtPixelAmount_LostFocus(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(txtOCRPixelAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out newValue))
                if (newValue >= 0 && newValue <= 99)
                    Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "EBPixelAmount", newValue.ToString());
                else
                    txtOCRPixelAmount.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "EBPixelAmount");
            else
                txtOCRPixelAmount.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "EBPixelAmount");
        }

        private void txtGUIColorCutoffLevel_LostFocus(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(txtGUIColorCutoffLevel.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out newValue))
                if (newValue >= 0 && newValue <= 255)
                    Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GUIColorCutoffLevel", newValue.ToString());
                else
                    txtGUIColorCutoffLevel.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GUIColorCutoffLevel");
            else
                txtGUIColorCutoffLevel.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GUIColorCutoffLevel");
        }     
    
        private void loadToolTips()
        {
            toolTip1.SetToolTip(txtOCRPixelAmount, "if the bitmap has less dark pixels it will not processed by EliteBrainerous, is set to 0 all bitmaps will be processed");
            toolTip1.SetToolTip(lblPixelAmount, "if the bitmap has less dark pixels it will not processed by EliteBrainerous, is set to 0 all bitmaps will be processed");

            toolTip1.SetToolTip(txtOCRPixelThreshold, "defines what a dark pixel is 0.0 is black, 1.0 is white");
            toolTip1.SetToolTip(lblPixelThreshold, "defines what a dark pixel is 0.0 is black, 1.0 is white");

            toolTip1.SetToolTip(cbCheckNextScreenshotForOne, "Activate the pixel check with a click on this button. Then buy -one- ton of a commodity and take a screenshot of the market with the \"1\" on it.\nSee how much dark pixels the 1 has and take approximately the half of this value as \"dark pixel amount\"");
            
        }

        /// <summary>
        /// starts the filter test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdFilter_Click(object sender, EventArgs e)
        {

            Bitmap _refbmp = getReferenceScreenshot();

            if (_refbmp == null)
            {
                return;
            }

            FilterTest FTest = new FilterTest();

            FTest.CutoffLevel = Program.DBCon.getIniValue<Int32>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GUIColorCutoffLevel");
            FTest.TestBitmap = _refbmp;

            FTest.ShowDialog(this);

            if (FTest.DialogResult == System.Windows.Forms.DialogResult.OK)
            { 
                txtGUIColorCutoffLevel.Text = FTest.CutoffLevel.ToString();
                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GUIColorCutoffLevel", FTest.CutoffLevel.ToString());
            }
        }

        private void cmdWarnLevels_Click(object sender, EventArgs e)
        {
            try
            {
                string Commodity = String.Empty;

                EDCommodityListView CView = new EDCommodityListView();

                CView.ShowDialog(this);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while showing warnlevels from settingstab");
            }
        }

        private Bitmap getReferenceScreenshot()
        {
            var openFile = new OpenFileDialog
            {
                DefaultExt = "bmp",
                Multiselect = true,
                Filter = "BMP (*.bmp)|*.bmp",
                InitialDirectory =
                    Environment.GetFolderPath((Environment.SpecialFolder.MyPictures)) +
                    @"\Frontier Developments\Elite Dangerous",
                Title = "Open a screenshot for calibration"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var bmp = new Bitmap(openFile.FileName);

                if (bmp.Height == Form1.GameSettings.Display.Resolution.Y &&
                    bmp.Width == Form1.GameSettings.Display.Resolution.X) return bmp;
                var wrongres = MsgBox.Show("The selected image has a different resolution from your current game settings. Do you want to pick another image?", "Ooops...", MessageBoxButtons.YesNo);
                if (wrongres == DialogResult.Yes)
                {
                    return getReferenceScreenshot();
                }
                
                return bmp;
            }
            return null;
        }

        private void cmdPurgeOldData_Click(object sender, EventArgs e)
        {

            if(MsgBox.Show(String.Format("Delete all data older than {0} days", nudPurgeOldDataDays.Value), "Delete old price data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.OK)
            {
                throw new NotImplementedException();

                DateTime deadline = DateTime.Now.AddDays(-1*(Int32)(nudPurgeOldDataDays.Value)).Date;
            }

        }
        
        /// <summary>
        /// Combobox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {

                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in ComboBox_CheckedChanged");
            }
        }

        private void txtOCRPixelThreshold_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                    if(((TextBoxDouble)sender).checkValue())
                        if(m_GUIInterface.saveSetting(sender))
                        {

                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtOCRPixelThreshold_KeyDown");
            }
        }

        private void txtOCRPixelThreshold_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((TextBoxDouble)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender))
                    {

                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtOCRPixelThreshold_Leave");
            }
        }

        private void txtOCRPixelAmount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                    if(((TextBoxInt32)sender).checkValue())
                        if(m_GUIInterface.saveSetting(sender))
                        {

                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtOCRPixelAmount_KeyDown");
            }
        }

        private void txtOCRPixelAmount_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((TextBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender))
                    {

                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtOCRPixelAmount_Leave");
            }
        }

        private void txtGUIColorCutoffLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if(e.KeyCode == Keys.Enter)
                    if(((TextBoxInt32)sender).checkValue())
                        if(m_GUIInterface.saveSetting(sender))
                        {

                        }
                    else
                        m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtGUIColorCutoffLevel_KeyDown");
            }
        }

        private void txtGUIColorCutoffLevel_Leave(object sender, EventArgs e)
        {
            try
            {
                if(((TextBoxInt32)sender).checkValue())
                    if(m_GUIInterface.saveSetting(sender))
                    {

                    }
                else
                    m_GUIInterface.loadSetting(sender);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtGUIColorCutoffLevel_Leave");
            }
        }

        private void nudPurgeOldDataDays_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    if(m_GUIInterface.saveSetting(sender))
                    {

                    }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in nudPurgeOldDataDays_KeyDown");
            }
        }

        private void nudPurgeOldDataDays_Leave(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {

                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in nudPurgeOldDataDays_Leave");
            }
        }

        private void Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if((m_GUIInterface != null) && m_GUIInterface.saveSetting(sender))
                {
                    if(sender == cmbLanguage)
                        Program.Data.switchLanguage((String)cmbLanguage.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmbLanguage_SelectedIndexChanged");
            }
        }

        private void cmdSelectExternalToolPath_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ToolPath = new OpenFileDialog();

                ToolPath.Filter = "Program-Files|*.exe|All Files|*.*";
                ToolPath.FileName = System.IO.Path.GetFileName(txtExtTool_Path.Text);
                try
                {
                    ToolPath.InitialDirectory = System.IO.Path.GetFullPath(txtExtTool_Path.Text);
                }
                catch (Exception)
                {
                    ToolPath.InitialDirectory = "C:";
                }
                
                ToolPath.Title = "select external tool...";

                if (ToolPath.ShowDialog(this) == DialogResult.OK)
                {
                    txtExtTool_Path.Text = ToolPath.FileName;
                    m_GUIInterface.saveSetting(txtExtTool_Path);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the path to the external data tool", ex);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in TextBox_KeyDown", ex);
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in TextBox_Leave", ex);
            }
        }

        private void rbInterface_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                m_GUIInterface.saveSetting(sender);

                if(rbUseOCR.Checked)
                    tcDataInterface.SelectedTab = tcDataInterface.TabPages["tabOCRSettings"];
                else
                    tcDataInterface.SelectedTab = tcDataInterface.TabPages["tabExternalToolSettings"];

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in rbOrderByStation_CheckedChanged");
            }
        }

        /// <summary>
        /// selects another game path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdGamePath_Click(object sender, EventArgs e)
        {
            try
            {
                SelectGamePath();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdGamePath_Click");
            }
        }

        public DialogResult SelectGamePath()
        {
            FolderBrowserDialog BrwsDlg = new FolderBrowserDialog();
            DialogResult result;

            BrwsDlg.Description  = "Please select manually your active game path. (it's one of the subdirs under the ED-'products'-dir)";
            BrwsDlg.SelectedPath = Program.DBCon.getIniValue(DB_GROUPNAME, "GamePath");

            result = BrwsDlg.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                String newPath = BrwsDlg.SelectedPath;
                String newProductPath = Directory.GetParent(newPath).FullName;
                String newGamePath = newPath;

                if (newProductPath.Substring(Directory.GetParent(newProductPath).FullName.Length).Replace("\\","").Equals("Products", StringComparison.InvariantCultureIgnoreCase))
                {

                    txtGamePath.Text = newGamePath;
                    m_GUIInterface.saveSetting(txtGamePath);

                    Program.DBCon.setIniValue(DB_GROUPNAME, "ProductsPath", newProductPath);

                    MessageBox.Show("Path changed. Please restart ED-IBE", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                else
                {
                    result = MessageBox.Show("Sorry, this seems not to be the correct dir (no 'Products' in the path).", "Wrong path", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                }
            }

            return result;
        }

    }
}
