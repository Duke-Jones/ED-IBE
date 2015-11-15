using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegulatedNoise.SQL;
using System.Diagnostics;
using RegulatedNoise.SQL.Datasets;
using System.Globalization;
using CodeProject.Dialog;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise.MTSettings
{
    public partial class tabSettings : UserControl
    {

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
                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME);
                m_GUIInterface.loadAllSettings(this);

                Cursor = oldCursor;
            }
            catch (Exception ex)
            {
                Cursor = oldCursor;
                throw new Exception("Error during initialization the commanders log tab", ex);
            }
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
                cErr.showError(ex, "Error in m_DataSource_DataChanged");
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
            OCRFile.FileName = Program.Settings_old.TraineddataFile;
            OCRFile.InitialDirectory = System.IO.Path.GetFullPath("./tessdata");  
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
                    Program.Settings_old.EBPixelAmount = newValue;
                else
                    txtOCRPixelAmount.Text = Program.Settings_old.EBPixelAmount.ToString();
            else
                txtOCRPixelAmount.Text = Program.Settings_old.EBPixelAmount.ToString();
        }

        private void txtGUIColorCutoffLevel_LostFocus(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(txtGUIColorCutoffLevel.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out newValue))
                if (newValue >= 0 && newValue <= 255)
                    Program.Settings_old.GUIColorCutoffLevel = newValue;
                else
                    txtGUIColorCutoffLevel.Text = Program.Settings_old.GUIColorCutoffLevel.ToString();
            else
                txtGUIColorCutoffLevel.Text = Program.Settings_old.GUIColorCutoffLevel.ToString();
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

            FTest.CutoffLevel = Program.Settings_old.GUIColorCutoffLevel;
            FTest.TestBitmap = _refbmp;

            FTest.ShowDialog(this);

            if (FTest.DialogResult == System.Windows.Forms.DialogResult.OK)
            { 
                txtGUIColorCutoffLevel.Text = FTest.CutoffLevel.ToString();
                Program.Settings_old.GUIColorCutoffLevel = FTest.CutoffLevel;
            }
        }

        private void cmdWarnLevels_Click(object sender, EventArgs e)
        {
            string Commodity = String.Empty;

            EDCommodityListView CView = new EDCommodityListView();

            CView.ShowDialog(this);

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
                cErr.showError(ex, "Error in ComboBox_CheckedChanged");
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
                cErr.showError(ex, "Error in txtOCRPixelThreshold_KeyDown");
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
                cErr.showError(ex, "Error in txtOCRPixelThreshold_Leave");
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
                cErr.showError(ex, "Error in txtOCRPixelAmount_KeyDown");
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
                cErr.showError(ex, "Error in txtOCRPixelAmount_Leave");
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
                cErr.showError(ex, "Error in txtGUIColorCutoffLevel_KeyDown");
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
                cErr.showError(ex, "Error in txtGUIColorCutoffLevel_Leave");
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
                cErr.showError(ex, "Error in nudPurgeOldDataDays_KeyDown");
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
                cErr.showError(ex, "Error in nudPurgeOldDataDays_Leave");
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
                cErr.showError(ex, "Error in cmbLanguage_SelectedIndexChanged");
            }
        }
    }
}
