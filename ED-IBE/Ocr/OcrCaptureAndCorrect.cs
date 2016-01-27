using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CodeProject.Dialog;
using IBE.MTCommandersLog;

namespace IBE.Ocr
{
    public partial class OcrCaptureAndCorrect : UserControl
    {
        private FileSystemWatcher _fileSystemWatcher;
        public Form1 _parent;
        private Ocr ocr;
        public OcrCaptureAndCorrect()
        {
            InitializeComponent();
        }

        private void OcrCaptureAndCorrect_Load(object sender, EventArgs e)
        {
            cbAutoImport.Checked = Program.Settings_old.AutoImport;
            cbExtendedInfoInCSV.Checked = Program.Settings_old.IncludeExtendedCSVInfo;
            cbDeleteScreenshotOnImport.Checked = Program.Settings_old.DeleteScreenshotOnImport;
            cbUseEddnTestSchema.Checked = Program.Settings_old.UseEddnTestSchema;
            cbPostOnImport.Checked = Program.Settings_old.PostToEddnOnImport;



            getEDDNUserid();
            selectEDDN_ID();


            // Debug spesific functionality
#if DEBUG
            bManualLoadImage.Visible = true;
#endif
        }

        private void getEDDNUserid()
        {
            if (Program.Settings_old.UserName != "")
                tbUsername.Text = Program.Settings_old.UserName;
            else
                tbUsername.Text = Guid.NewGuid().ToString();

            txtCmdrsName.Text = Program.Settings_old.PilotsName;
        }
        private void selectEDDN_ID()
        {
            if (Program.Settings_old.usePilotsName)
            {
                if (!String.IsNullOrEmpty(Program.Settings_old.PilotsName))
                {
                    rbCmdrsName.Checked = true;
                }
                else
                {
                    rbUserID.Checked = true;
                    Program.Settings_old.usePilotsName = false;
                    rbCmdrsName.Enabled = false;
                }
            }
            else
                rbUserID.Checked = true;

            rbCmdrsName.Enabled = !String.IsNullOrEmpty(Program.Settings_old.PilotsName);
        }

        public void startOcrOnload(bool doStart)
        {

            ocr = new Ocr(_parent);
            cbStartOCROnLoad.Checked = true;
            //ocr.StartMonitoring(Program.Settings_old.MostRecentOCRFolder);

            if (_fileSystemWatcher == null)
                _fileSystemWatcher = new FileSystemWatcher();

            _fileSystemWatcher.Path = Program.Settings_old.MostRecentOCRFolder;

            _fileSystemWatcher.Filter = "*.bmp";

            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess |
                         NotifyFilters.LastWrite |
                         NotifyFilters.FileName |
                         NotifyFilters.DirectoryName;

            _fileSystemWatcher.IncludeSubdirectories = false;

            _fileSystemWatcher.Created += ScreenshotCreated;

            _fileSystemWatcher.EnableRaisingEvents = true;

            ocr.IsMonitoring = true;
        }

        private Thread _ocrThread;
        private List<string> _preOcrBuffer = new List<string>();
        private System.Threading.Timer _preOcrBufferTimer;
        private void ScreenshotCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            Thread.Sleep(1000);

            while (!File.Exists(fileSystemEventArgs.FullPath))
            {
                //MsgBox.Show("File created... but it doesn't exist?!  Hit OK and I'll retry...");
                Thread.Sleep(100);
            }

            //MsgBox.Show("Good news! " + fileSystemEventArgs.FullPath +
            //                " exists!  Let's pause for a moment before opening it...");

            ScreenshotsQueued("(" + (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) + " queued)");
            // if the textfield support auto-uppercase we must consider
            string s = CommoditiesText("").ToString().ToUpper();

            if (s == "Imported!".ToUpper() || s == "Finished!".ToUpper() || s == "" || s == "No rows found...".ToUpper())
                CommoditiesText("Working...");



            if (_ocrThread == null || !_ocrThread.IsAlive)
            {
                Form1.InstanceObject.ActivateOCRTab();

                // some stateful enabling for the buttons

                _parent.setButton(bClearOcrOutput, false);
                _parent.setButton(bEditResults, false);

                _ocrThread = new Thread(() => ocr.ScreenshotCreated(fileSystemEventArgs.FullPath, Program.actualCondition.System));
                _ocrThread.IsBackground = false;
                _ocrThread.Start();
            }
            else
            {
                _preOcrBuffer.Add(fileSystemEventArgs.FullPath);

                if (_preOcrBufferTimer == null)
                {
                    var autoEvent = new AutoResetEvent(false);
                    _preOcrBufferTimer = new System.Threading.Timer(CheckOcrBuffer, autoEvent, 1000, 1000);
                }
            }
        }

        private void CheckOcrBuffer(object sender)
        {
            if (!_ocrThread.IsAlive)
            {
                if (_preOcrBuffer.Count > 0)
                {
                    // some stateful enabling for the buttons
                    _parent.setButton(bClearOcrOutput, false);
                    _parent.setButton(bEditResults, false);

                    var s = _preOcrBuffer[0];
                    _preOcrBuffer.RemoveAt(0);
                    _ocrThread = new Thread(() => ocr.ScreenshotCreated(s, Program.actualCondition.System));
                    _ocrThread.IsBackground = false;
                    _ocrThread.Start();
                    ScreenshotsQueued("(" +
                                      (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) +
                                      " queued)");
                }
            }
        }
        public delegate string CommoditiesTextDelegate(string s);

        public string CommoditiesText(string s)
        {
            if (InvokeRequired)
            {
                return (string)(Invoke(new CommoditiesTextDelegate(CommoditiesText), s));
            }

            if (s != "")
                tbCommoditiesOcrOutput.Text = s;

            return tbCommoditiesOcrOutput.Text;
        }




        public delegate void ScreenshotsQueuedDelegate(string s);
        public delegate void del_setControlText(Control CtrlObject, string newText);
        public delegate void del_setLocationInfo(string System, string Location, Boolean ForceChangedLocation);
        public delegate void del_EventLocationInfo(string System, string Location);

        public void ScreenshotsQueued(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new ScreenshotsQueuedDelegate(ScreenshotsQueued), s);
                return;
            }

            lblScreenshotsQueued.Text = s;
        }

        #region UpdateOriginalImage
        public delegate Point[] UpdateOriginalImageDelegate(Bitmap b);

        public Point[] UpdateOriginalImage(Bitmap b)
        {
            try
            {
                if (InvokeRequired)
                {
                    return (Point[])(Invoke(new UpdateOriginalImageDelegate(UpdateOriginalImage), b));
                }

                if (pbOriginalImage.Image != null) pbOriginalImage.Image.Dispose();
                pbOriginalImage.Image = b;

                Point[] returnValue = new Point[12];

                int ctr = 0;
                foreach (CalibrationPoint o in OcrCalibrator.CalibrationBoxes)
                {
                    returnValue[ctr] = ((Point)o.Position);
                    ctr++;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Doh!");
                _parent._logger.Log("Exception in UpdateOriginalImage", true);
                _parent._logger.Log(ex.ToString(), true);
                _parent._logger.Log(ex.Message, true);
                _parent._logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    _parent._logger.Log(ex.InnerException.ToString(), true);
                return new Point[12];
            }
        }
        #endregion

        #region UpdateTrimmedImage
        public delegate void UpdateTrimmedImageDelegate(Bitmap b, Bitmap bHeader);

        public void UpdateTrimmedImage(Bitmap b, Bitmap bHeader)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateTrimmedImageDelegate(UpdateTrimmedImage), b, bHeader);
                return;
            }

            try
            {
                if (pbTrimmed.Image != null) pbTrimmed.Image.Dispose();
                if (pbStation.Image != null) pbStation.Image.Dispose();

                if (b != null)
                {
                    pbTrimmed.Image = (Bitmap)(b.Clone());
                    pbStation.Image = (Bitmap)(bHeader.Clone());
                }
                else
                {
                    pbTrimmed.Image = null;
                    pbStation.Image = null;
                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
            }
        }

        public delegate Bitmap ReturnTrimmedImageDelegate();

        public Bitmap ReturnTrimmedImage()
        {
            if (InvokeRequired)
            {
                return (Bitmap)(Invoke(new ReturnTrimmedImageDelegate(ReturnTrimmedImage)));
            }

            try
            {
                if (pbOcrCurrent.Image == null) return null;

                return (Bitmap)(pbOcrCurrent.Image.Clone());
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
                return null;
            }
        }

        public delegate bool ReturnOcrMonitoringStatusDelegate();

        public bool ReturnOcrMonitoringStatus()
        {
            if (InvokeRequired)
            {
                return (bool)(Invoke(new ReturnOcrMonitoringStatusDelegate(ReturnOcrMonitoringStatus)));
            }

            return ocr.IsMonitoring;
        }

        public delegate void ImportCurrentOcrDataDelegate();

        public void ImportCurrentOcrData()
        {
            if (InvokeRequired)
            {
                Invoke(new ImportCurrentOcrDataDelegate(ImportCurrentOcrData));
                return;
            }
            ImportFinalOcrOutput();
        }

        public delegate void SetOcrValueFromWebDelegate(string s);

        public void SetOcrValueFromWeb(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new SetOcrValueFromWebDelegate(SetOcrValueFromWeb), s);
                return;
            }

            tbCommoditiesOcrOutput.Text = s;
            bContinueOcr_Click(null, null);
        }
        private void bContinueOcr_Click(object sender, EventArgs e)
        {
            Boolean isOK = false;
            Boolean finished = false;
            DialogResult Answer;
            string commodity;
            List<string> KnownCommodityNames;

            commodity = _parent._textInfo.ToTitleCase(tbCommoditiesOcrOutput.Text.ToLower().Trim());

            KnownCommodityNames = Program.Data.getCommodityNames();

            if (commodity.ToUpper() == "Implausible Results!".ToUpper())
            {
                // check results
                var f = new EditOcrResults(tbFinalOcrOutput.Text);
                f.onlyImplausible = true;
                var q = f.ShowDialog();

                if (q == DialogResult.OK)
                {
                    tbFinalOcrOutput.Text = f.ReturnValue;
                }

                Acquisition(true);

                isOK = false;
            }
            else if (commodity.ToUpper() == "Imported!".ToUpper() || commodity.ToUpper() == "Finished!".ToUpper() || commodity.ToUpper() == "No rows found...".ToUpper())
            {
                // its the end
                isOK = true;
                finished = true;
            }
            else if (commodity.Length == 0 || KnownCommodityNames.Contains(commodity))
            {
                // ok, no typing error
                isOK = true;
            }
            else
            {
                // unknown commodity, is it a new one or a typing error ?
                Answer = MsgBox.Show(String.Format("Do you want to add '{0}' to the known commodities ?", commodity), "Unknown commodity !",
                                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (Answer == System.Windows.Forms.DialogResult.OK)
                {
                    // yes, it's really new
                    //Program.Data.ImportCommodity(commodity);

                    throw new NotImplementedException();
                    //_Milkyway.addLocalized2RN(_commodities.Names);
                    isOK = true;
                }
            }

            if (isOK)
            {
                if (_commodityTexts == null || _correctionColumn >= _commodityTexts.GetLength(1) || finished)
                {
                    if (MsgBox.Show("Import this?", "Import?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ImportFinalOcrOutput();
                        tbFinalOcrOutput.Text = "";
                        bContinueOcr.Enabled = false;
                        bIgnoreTrash.Enabled = false;
                        _commoditiesSoFar = new List<string>();
                        bClearOcrOutput.Enabled = false;
                        bEditResults.Enabled = false;
                    }
                }
                else
                {
                    _commodityTexts[_correctionRow, _correctionColumn] = commodity.ToUpper();
                    _commoditiesSoFar.Add(_commodityTexts[_correctionRow, _correctionColumn].ToUpper());

                    ContinueDisplayingResults();
                }
            }

        }
        public delegate void SetStationAndSystemFromWebDelegate(string s, string t);

        public void SetStationAndSystem(string s, string t)
        {
            if (InvokeRequired)
            {
                Invoke(new SetStationAndSystemFromWebDelegate(SetStationAndSystem), s, t);
                return;
            }

            tbOcrStationName.Text = s;
            tbOcrSystemName.Text = t;
        }

        public delegate string[] GetOcrValueForWebDelegate();

        public string[] GetOcrValueForWeb()
        {
            if (InvokeRequired)
            {
                return (string[])(Invoke(new GetOcrValueForWebDelegate(GetOcrValueForWeb)));
            }

            if (pbTrimmed.Image == null)
                return new[] { "<FINISHED>", "<FINISHED>", "<FINISHED>", "" };

            if (_commodityTexts != null && _correctionColumn >= _commodityTexts.GetLength(1))
            {
                var buffered = _screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count;
                if (buffered > 0)
                    return new[] { "Cached" + buffered, "Cached" + buffered, "Cached" + buffered, "" };

                return new[] { "<FINISHED>", "<FINISHED>", "<FINISHED>", "" };
            }

            return new[] { tbCommoditiesOcrOutput.Text, tbOcrStationName.Text, tbOcrSystemName.Text, _correctionColumn.ToString(CultureInfo.InvariantCulture) };
        }
        #endregion

        #region DisplayResults
        public delegate void DisplayResultsDelegate(string s);

        public void DisplayResults(string s)
        {
            if (InvokeRequired)
            {
                Int32 currentTry = 0;
                bool Retry = false;

                do
                {
                    Retry = false;
                    try
                    {
                        Invoke(new DisplayResultsDelegate(DisplayResults), s);
                        return;
                    }
                    catch (Exception ex)
                    {
                        if (currentTry >= 3)
                            throw ex;
                        else
                        {
                            Thread.Sleep(333);
                            currentTry++;
                            Retry = true;
                        }
                    }
                } while (Retry);
            }

            tbOcrStationName.Text = s; // CLARK HUB

        }

        public delegate void DisplayCommodityResultsDelegate(string[,] s, Bitmap[,] originalBitmaps, float[,] originalBitmapConfidences, string[] rowIds, string screenshotName);

        private int _correctionRow, _correctionColumn;

        private string[,] _commodityTexts;
        private Bitmap[,] _originalBitmaps;
        private float[,] _originalBitmapConfidences;
        private string[] _rowIds;
        private string _screenshotName;

        private List<ScreeenshotResults> _screenshotResultsBuffer = new List<ScreeenshotResults>();
        private string _csvOutputSoFar;
        private List<string> _commoditiesSoFar = new List<string>();

        public void DisplayCommodityResults(string[,] s, Bitmap[,] originalBitmaps, float[,] originalBitmapConfidences, string[] rowIds, string screenshotName)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayCommodityResultsDelegate(DisplayCommodityResults), s, originalBitmaps, originalBitmapConfidences, rowIds, screenshotName);
                return;
            }

            if (_commodityTexts != null && _correctionColumn < _commodityTexts.GetLength(1)) // there is an existingClassification screenshot being processed...
            {
                _screenshotResultsBuffer.Add(new ScreeenshotResults { originalBitmapConfidences = originalBitmapConfidences, originalBitmaps = originalBitmaps, s = s, rowIds = rowIds, screenshotName = screenshotName });
                ScreenshotsQueued("(" + (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) + " queued)");
                return;
            }

            if (originalBitmaps.GetLength(0) != 0)
                BeginCorrectingScreenshot(s, originalBitmaps, originalBitmapConfidences, rowIds, screenshotName);
            else
                tbCommoditiesOcrOutput.Text = "No rows found...";
        }

        private void BeginCorrectingScreenshot(string[,] s, Bitmap[,] originalBitmaps, float[,] originalBitmapConfidences, string[] rowIds, string screenshotName)
        {
            _commodityTexts = s;
            _originalBitmaps = originalBitmaps;
            _originalBitmapConfidences = originalBitmapConfidences;
            _screenshotName = screenshotName;
            _rowIds = rowIds;
            _correctionColumn = 0;
            _correctionRow = -1;
            bContinueOcr.Text = "C&ontinue";
            bClearOcrOutput.Enabled = false;
            bEditResults.Enabled = false;
            tbFinalOcrOutput.Enabled = false;
            ContinueDisplayingResults();
        }

        private void ContinueDisplayingResults()
        {
            List<string> KnownCommodityNames;
            Dictionary<String, String> EconomyLevels;

            try
            {
                KnownCommodityNames = Program.Data.getCommodityNames();
                EconomyLevels = Program.Data.getEconomyLevels();

                do
                {


                    _correctionRow++;
                    if (_correctionRow > _commodityTexts.GetLength(0) - 1) { _correctionRow = 0; _correctionColumn++; }

                    if (_commodityTexts.GetLength(0) == 0) return;

                    if (_correctionColumn < _commodityTexts.GetLength(1))
                    {
                        //Debug.WriteLine(correctionRow + " - " + correctionColumn);
                        pbOcrCurrent.Image = _originalBitmaps[_correctionRow, _correctionColumn];
                    }

                    if (_correctionColumn == 0) // hacks for commodity name
                    {
                        var currentTextCamelCase =
                            _parent._textInfo.ToTitleCase(_commodityTexts[_correctionRow, _correctionColumn].ToLower()); // There *was* a reason why I did this...

                        // if the ocr have found no char so we dont need to ask Mr. Levenshtein 
                        if (currentTextCamelCase.Trim().Length > 0)
                        {
                            if (KnownCommodityNames.Contains(
                                currentTextCamelCase))
                                _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                            else
                            {
                                var replacedCamelCase = StripPunctuationFromScannedText(currentTextCamelCase); // ignore spaces when using levenshtein to find commodity names
                                var lowestLevenshteinNumber = 10000;
                                var nextLowestLevenshteinNumber = 10000;
                                var lowestMatchingCommodity = "";
                                var lowestMatchingCommodityRef = "";
                                double LevenshteinLimit = 0;



                                foreach (var reference in KnownCommodityNames)
                                {
                                    var upperRef = StripPunctuationFromScannedText(reference);
                                    var levenshteinNumber = _parent._levenshtein.LD2(upperRef, replacedCamelCase);
                                    //if(levenshteinNumber != _levenshtein.LD(upperRef, replacedCamelCase))
                                    //    Debug.WriteLine("Doh!");

                                    if (upperRef != lowestMatchingCommodityRef)
                                    {
                                        if (levenshteinNumber < lowestLevenshteinNumber)
                                        {
                                            nextLowestLevenshteinNumber = lowestLevenshteinNumber;
                                            lowestLevenshteinNumber = levenshteinNumber;
                                            lowestMatchingCommodityRef = upperRef;
                                            lowestMatchingCommodity = reference.ToUpper();
                                        }
                                        else if (levenshteinNumber < nextLowestLevenshteinNumber)
                                        {
                                            nextLowestLevenshteinNumber = levenshteinNumber;
                                        }
                                    }
                                }

                                // it's better if this depends on the length of the word - this factor works pretty good
                                LevenshteinLimit = Math.Round((currentTextCamelCase.Length * 0.7), 0);

                                if (lowestLevenshteinNumber <= LevenshteinLimit)
                                {
                                    _originalBitmapConfidences[_correctionRow, _correctionColumn] = .9f;
                                    _commodityTexts[_correctionRow, _correctionColumn] = lowestMatchingCommodity;
                                }

                                if (lowestLevenshteinNumber <= LevenshteinLimit && lowestLevenshteinNumber + 3 < nextLowestLevenshteinNumber) // INDIUM versus INDITE... could factor length in here
                                    _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;

                            }

                            if (_commodityTexts[_correctionRow, _correctionColumn].Equals("Getreide", StringComparison.InvariantCultureIgnoreCase))
                                Debug.Print("STOP");

                            if (_commoditiesSoFar.Contains(_commodityTexts[_correctionRow, _correctionColumn].ToUpper()))
                            {
                                _commodityTexts[_correctionRow, _correctionColumn] = "";
                                _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                            }

                            // If we're doing a batch of screenshots, don't keep doing the same commodity when we keep finding it
                            // but only if it's sure - otherwise it will be registered later
                            if (_originalBitmapConfidences[_correctionRow, _correctionColumn] == 1)
                            {
                                _commoditiesSoFar.Add(_commodityTexts[_correctionRow, _correctionColumn].ToUpper());
                            }
                        }
                        else
                        {
                            // that was nothing 
                            _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                            _commodityTexts[_correctionRow, _correctionColumn] = "";
                        }
                    }
                    else if (_correctionColumn == 5 || _correctionColumn == 7) // hacks for LOW/MED/HIGH
                    {
                        var commodityLevelUpperCase = StripPunctuationFromScannedText(_commodityTexts[_correctionRow, _correctionColumn]);

                        var levenshteinLow = _parent._levenshtein.LD2(EconomyLevels["LOW"].ToUpper(), commodityLevelUpperCase);
                        var levenshteinMed = _parent._levenshtein.LD2(EconomyLevels["MED"].ToUpper(), commodityLevelUpperCase);
                        var levenshteinHigh = _parent._levenshtein.LD2(EconomyLevels["HIGH"].ToUpper(), commodityLevelUpperCase);
                        var levenshteinBlank = _parent._levenshtein.LD2("", commodityLevelUpperCase);

                        //Pick the lowest levenshtein number
                        var lowestLevenshtein = Math.Min(Math.Min(levenshteinLow, levenshteinMed), Math.Min(levenshteinHigh, levenshteinBlank));

                        if (lowestLevenshtein == levenshteinLow)
                        {
                            _commodityTexts[_correctionRow, _correctionColumn] = EconomyLevels["LOW"];
                        }
                        else if (lowestLevenshtein == levenshteinMed)
                        {
                            _commodityTexts[_correctionRow, _correctionColumn] = EconomyLevels["MED"];
                        }
                        else if (lowestLevenshtein == levenshteinHigh)
                        {
                            _commodityTexts[_correctionRow, _correctionColumn] = EconomyLevels["HIGH"];
                        }
                        else // lowestLevenshtein == levenshteinBlank
                        {
                            _commodityTexts[_correctionRow, _correctionColumn] = "";
                        }

                        // we will never be challenged on low/med/high again.  this doesn't get internationalized on foreign-language installs... does it? :)
                        _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                    }
                }
                // Don't pause for cells which have a high confidence, or have no commodity name
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                while (_correctionColumn < _commodityTexts.GetLength(1) && (_originalBitmapConfidences[_correctionRow, _correctionColumn] > .9f || _originalBitmapConfidences[_correctionRow, _correctionColumn] == 0 || _commodityTexts[_correctionRow, 0] == ""));

                if (_correctionColumn < _commodityTexts.GetLength(1))
                {
                    // doing again some stateful enabling
                    tbCommoditiesOcrOutput.Text = _commodityTexts[_correctionRow, _correctionColumn];
                    tbConfidence.Text = _originalBitmapConfidences[_correctionRow, _correctionColumn].ToString(CultureInfo.InvariantCulture);
                    bContinueOcr.Enabled = true;
                    bIgnoreTrash.Enabled = true;
                }
                else
                {
                    bContinueOcr.Enabled = false;
                    bIgnoreTrash.Enabled = false;

                    string finalOutput = _csvOutputSoFar;

                    for (int row = 0; row < _commodityTexts.GetLength(0); row++)
                    {
                        if (_commodityTexts[row, 0] != "") // don't create CSV if there's no commodity name
                        {
                            finalOutput += tbOcrSystemName.Text + ";" + tbOcrStationName.Text + ";";

                            for (int col = 0; col < _commodityTexts.GetLength(1); col++)
                            {
                                _commodityTexts[row, col] = _commodityTexts[row, col].Replace("\r", "").Replace("\n", "");

                                if (col == 3) continue; // don't export cargo levels
                                finalOutput += _commodityTexts[row, col] + ";";
                            }
                            finalOutput += ocr.CurrentScreenshotDateTime.ToString("s").Substring(0, 16) + ";";

                            if (cbExtendedInfoInCSV.Checked)
                                finalOutput += Path.GetFileName(_screenshotName) + ";";

                            finalOutput += _rowIds[row] + "\r\n";
                        }
                    }

                    _csvOutputSoFar += finalOutput;

                    if (pbOriginalImage.Image != null)
                        pbOriginalImage.Image.Dispose();

                    UpdateOriginalImage(null);
                    UpdateTrimmedImage(null, null);

                    if (Program.Settings_old.DeleteScreenshotOnImport)
                        File.Delete(_screenshotName);

                    Acquisition();

                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
            }
        }

        private void Acquisition(bool noAutoImport = false)
        {

            if (_screenshotResultsBuffer.Count == 0)
            {
                tbFinalOcrOutput.Text += _csvOutputSoFar;
                tbFinalOcrOutput.Text = removeClones(tbFinalOcrOutput.Text);
                _csvOutputSoFar = null;


                pbOcrCurrent.Image = null;
                if (_preOcrBuffer.Count == 0 && ocr.ScreenshotBuffer.Count == 0)
                {

                    if (_parent.checkPricePlausibility(tbFinalOcrOutput.Text.Replace("\r", "").Split('\n')))
                    {
                        tbFinalOcrOutput.Enabled = true;
                        tbCommoditiesOcrOutput.Text = "Implausible Results!";
                        bContinueOcr.Text = "Check Implausible";
                        bContinueOcr.Enabled = true;
                        bIgnoreTrash.Enabled = false;
                        bClearOcrOutput.Enabled = true;
                        bEditResults.Enabled = true;
                    }
                    else
                    {
                        tbFinalOcrOutput.Enabled = true;

                        if ((!noAutoImport) && Program.Settings_old.AutoImport)
                        {
                            tbCommoditiesOcrOutput.Text = "Imported!";
                            ImportFinalOcrOutput();
                            tbFinalOcrOutput.Text = "";
                            _csvOutputSoFar = "";
                            _commoditiesSoFar = new List<string>();
                            bClearOcrOutput.Enabled = false;
                            bEditResults.Enabled = false;
                        }
                        else
                        {

                            tbCommoditiesOcrOutput.Text = "Finished!";
                            bContinueOcr.Text = "Imp&ort";
                            bContinueOcr.Enabled = true;
                            bIgnoreTrash.Enabled = false;
                            bClearOcrOutput.Enabled = true;
                            bEditResults.Enabled = true;
                        }
                    }
                }
                else
                {
                    tbCommoditiesOcrOutput.Text = "Working...!";
                    bClearOcrOutput.Enabled = false;
                    bEditResults.Enabled = false;
                }

            }
            else
            {
                var nextScreenshot = _screenshotResultsBuffer[0];
                _screenshotResultsBuffer.Remove(nextScreenshot);
                ScreenshotsQueued("(" + (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) + " queued)");
                BeginCorrectingScreenshot(nextScreenshot.s, nextScreenshot.originalBitmaps, nextScreenshot.originalBitmapConfidences, nextScreenshot.rowIds, nextScreenshot.screenshotName);
            }

        }

        private string removeClones(string p)
        {
            StringBuilder cleanedEntries = new StringBuilder();
            HashSet<string> existing = new HashSet<string>();

            var rows = tbFinalOcrOutput.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in rows)
            {
                string Commodity = row.Split(new string[] { ";" }, StringSplitOptions.None)[2].ToUpper();

                if (!existing.Contains(Commodity))
                {
                    cleanedEntries.Append(row + "\r\n");
                    existing.Add(Commodity);
                }

            }

            return cleanedEntries.ToString();
        }

        private string StripPunctuationFromScannedText(string input)
        {
            return _parent._textInfo.ToUpper(input.Replace(" ", "").Replace("-", "").Replace(".", "").Replace(",", ""));
        }

        #endregion

        #region Generic Delegates

        public delegate void GenericSingleParameterMessageDelegate(Object o, AppDelegateType a);

        public void GenericSingleParameterMessage(Object o, AppDelegateType a)
        {
            if (InvokeRequired)
            {
                Invoke(new GenericSingleParameterMessageDelegate(GenericSingleParameterMessage), o, a);
                return;
            }

            switch (a)
            {
                case AppDelegateType.AddEventToLog:
                    Program.CommandersLog.SaveEvent((CommandersLogEvent)o);
                    break;
                case AppDelegateType.MaximizeWindow:
                    _parent.WindowState = FormWindowState.Minimized;
                    Show();
                    _parent.WindowState = FormWindowState.Normal;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        #endregion

        #region UpdateCurrentImage
        public delegate void UpdateCurrentImageDelegate(Bitmap b);

        public void UpdateCurrentImage(Bitmap b)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateCurrentImageDelegate(UpdateCurrentImage), b);
                return;
            }

            try
            {
                pbOcrCurrent.Image = (Bitmap)(b.Clone());
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
            }

        }
        #endregion

        private void ImportFinalOcrOutput()
        {
            String[] CSVStrings;
            String currentSystem;

            try
            {
                currentSystem = Program.actualCondition.System;
                CSVStrings = tbFinalOcrOutput.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < CSVStrings.ToList().Count(); i++) //TODO: can we replace .tolist.count with length?
                {
                    if (CSVStrings[i].StartsWith(";"))
                        CSVStrings[i] = currentSystem + CSVStrings[i];
                }

                Program.Data.ImportPricesFromCSVStrings(CSVStrings);

                if (Program.actualCondition.Location.Equals("", StringComparison.InvariantCultureIgnoreCase))
                    Program.actualCondition.Location = tbOcrStationName.Text;

                Program.CommandersLog.createMarketdataCollectedEvent();

                _parent.SetupGui();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing the OCR results", ex);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (OcrCalibrator.CalibrationBoxes == null || OcrCalibrator.CalibrationBoxes.Count < 10)
            {
                MsgBox.Show("You need to calibrate first.  Go to the OCR Calibration tab to do so...");
                return;
            }

            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Program.Settings_old.MostRecentOCRFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Program.Settings_old.MostRecentOCRFolder = dialog.SelectedPath;

                if (_fileSystemWatcher == null)
                    _fileSystemWatcher = new FileSystemWatcher();

                _fileSystemWatcher.Path = dialog.SelectedPath;

                _fileSystemWatcher.Filter = "*.bmp";

                _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess |
                             NotifyFilters.LastWrite |
                             NotifyFilters.FileName |
                             NotifyFilters.DirectoryName;

                _fileSystemWatcher.IncludeSubdirectories = false;

                _fileSystemWatcher.Created += ScreenshotCreated;

                _fileSystemWatcher.EnableRaisingEvents = true;

                ocr.IsMonitoring = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OcrCalibrator.CalibrationBoxes == null || OcrCalibrator.CalibrationBoxes.Count < 10)
            {
                MsgBox.Show("You need to calibrate first.  Go to the OCR Calibration tab to do so...");
                return;
            }
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Program.Settings_old.MostRecentOCRFolder;
            dialog.Filter = "BMP Files|*.bmp";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fsa = new FileSystemEventArgs(WatcherChangeTypes.All, Path.GetDirectoryName(dialog.FileName), dialog.SafeFileName);
                ScreenshotCreated(this, fsa);

            }
        }

        private string _oldOcrName;

        private void tbOcrStationName_TextChanged(object sender, EventArgs e)
        {
            if (tbOcrStationName.Text != _oldOcrName && _oldOcrName != null)
            {
                var rows = tbFinalOcrOutput.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string newRows = "";

                foreach (var row in rows)
                {
                    var newRow1 = row.Substring(0, row.IndexOf(";"));
                    var newRow2 = tbOcrStationName.Text;
                    var newRow3 = row.Substring(row.IndexOf(";", 1));
                    newRow3 = newRow3.Substring(newRow3.IndexOf(";", 1));
                    newRows = newRows + newRow1 + ";" + newRow2 + newRow3 + "\r\n";

                }
                tbFinalOcrOutput.Text = newRows;
            }

            _oldOcrName = tbOcrStationName.Text;
        }

        private string _oldOcrSystemName;

        private void tbOcrSystemName_TextChanged(object sender, EventArgs e)
        {
            if (tbOcrSystemName.Text != _oldOcrSystemName && _oldOcrSystemName != null)
            {
                var rows = tbFinalOcrOutput.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string newRows = "";

                foreach (var row in rows)
                {
                    var newRow1 = row.Substring(row.IndexOf(";"));
                    newRows += tbOcrSystemName.Text + newRow1 + "\r\n";

                }
                tbFinalOcrOutput.Text = newRows;
            }

            _oldOcrSystemName = tbOcrSystemName.Text;
        }

        private void bEditResults_Click(object sender, EventArgs e)
        {
            var f = new EditOcrResults(tbFinalOcrOutput.Text);
            var q = f.ShowDialog();

            if (q == DialogResult.OK)
            {
                tbFinalOcrOutput.Text = f.ReturnValue;
            }
        }

        private void bClearOcrOutput_Click(object sender, EventArgs e)
        {
            clearOcrOutput();
        }

        public void clearOcrOutput()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(clearOcrOutput));
                return;
            }

            // when we do clear so we must consider all dependences (!->_commoditiesSoFar)
            // doing some stateful enabling of button again
            tbFinalOcrOutput.Text = "";
            bContinueOcr.Enabled = false;
            bIgnoreTrash.Enabled = false;
            _commoditiesSoFar = new List<string>();
            bClearOcrOutput.Enabled = false;
            bEditResults.Enabled = false;
            tbCommoditiesOcrOutput.Text = "Finished!";

            tbOcrStationName.Text = "";
            tbOcrSystemName.Text = "";
            pbOcrCurrent.Image = null;
            UpdateOriginalImage(null);
            UpdateTrimmedImage(null, null);


        }

        /// <summary>
        /// Perform an "ignoring" of the current value (it's cosier)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdIgnore_Click(object sender, EventArgs e)
        {

            tbCommoditiesOcrOutput.Text = "";

            bContinueOcr_Click(sender, e);

        }


        /// <summary>
        /// direct submitting of the commodities with "Enter" if changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCommoditiesOcrOutput_Keypress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (bContinueOcr.Enabled)
                {
                    bContinueOcr_Click(sender, new EventArgs());
                }
            }
        }

        public void setControlText(Control CtrlObject, string newText)
        {
            if (CtrlObject.InvokeRequired)
                CtrlObject.Invoke(new del_setControlText(setControlText), CtrlObject, newText);
            else
            {
                CtrlObject.Text = newText;
            }
        } /// <summary>
          /// selects, which ID to use for sending to EDDNCommunicator
          /// </summary>
 

        private void rbUserID_CheckedChanged(object sender, EventArgs e)
        {
            tbUsername.Enabled = rbUserID.Checked;
            txtCmdrsName.Enabled = rbCmdrsName.Checked;
        }

        private void cbStartOCROnLoad_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStartOCROnLoad.Checked && Program.Settings_old.MostRecentOCRFolder == "")
            {
                MessageBox.Show("You need to pick a directory first, using the Monitor Directory button.  Once you've done that, you can enable Start OCR On Load.");
                Program.Settings_old.StartOCROnLoad = false;
                cbStartOCROnLoad.Checked = false;
            }
            else
            {
                Program.Settings_old.StartOCROnLoad = cbStartOCROnLoad.Checked;
            }
        }

        private void cbAutoImport_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings_old.AutoImport = cbAutoImport.Checked;
        }

        private void rbCmdrsName_CheckedChanged(object sender, EventArgs e)
        {
            tbUsername.Enabled = rbUserID.Checked;
            txtCmdrsName.Enabled = rbCmdrsName.Checked;

            Program.Settings_old.usePilotsName = rbCmdrsName.Checked;
        }
        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            Program.Settings_old.UserName = tbUsername.Text;
        }

        private void cbExtendedInfoInCSV_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings_old.IncludeExtendedCSVInfo = cbExtendedInfoInCSV.Checked;
        }

        private void cbPostOnImport_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings_old.PostToEddnOnImport = cbPostOnImport.Checked;
        }


        private void cbDeleteScreenshotOnImport_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings_old.DeleteScreenshotOnImport = cbDeleteScreenshotOnImport.Checked;
        }

        private void cmdHint_Click(object sender, EventArgs e)
        {
            MsgBox.Show(
                "If you leave the Commodity Name blank in the UI or webpage, that entire row will be ignored on import (though it will still appear in the CSV). This is really useful when half a row has been OCR'ed and it's all gone horribly wrong :)",
                "Really Useful Tip...");
        }

        private void cbUseEddnTestSchema_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings_old.UseEddnTestSchema = cbUseEddnTestSchema.Checked;
        }
    }


}
