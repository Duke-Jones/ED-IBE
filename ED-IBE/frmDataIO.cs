using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;
using System.IO;
using IBE.SQL;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace IBE
{
    public partial class frmDataIO : RNBaseForm
    {
        public const String        DB_GROUPNAME                    = "DataIO";

        public delegate void DelTextParam(String text);
        private readonly SynchronizationContext localSynchronizationContext;
        private SynchronizationContext InfoTargetSynchronizationContext;
        private SynchronizationContext usedSynchronizationContext;

        SplashScreenForm m_InfoTarget;

        public SplashScreenForm InfoTarget
        {
            get
            {
                return m_InfoTarget;
            }
            set
            {
                m_InfoTarget = value;
            }
        }                 // allows to redirect the progress info to another listbox


        private Boolean m_DataImportHappened = false; 

        private Boolean m_GotNewEDCDFiles;
        
        private DBGuiInterface      m_GUIInterface;
        private Boolean             m_DownloadFinished;
        private String              m_DataPath          = Program.GetDataPath("data");


        private List<String>        m_DL_Files          = new List<string> {"systems.json", 
                                                                            "stations.json", 
                                                                            "commodities.json"};

        private List<String>        m_DL_Files_Pop      = new List<string> {"systems_populated.json", 
                                                                            "stations.json", 
                                                                            "commodities.json"};

        private List<String>        m_DL_FilesPrice     = new List<string> {"listings.csv"};

        private List<String>        m_DL_Files_EDCD     = new List<string> {"commodity.csv",
                                                                            "outfitting.csv",
                                                                            "shipyard.csv"};

        private Boolean m_CancelAction = false;
        private System.Collections.Queue m_DownloadQueue = new System.Collections.Queue();
        private WebClient m_DownLoader;
        private DownloadData m_CurrentDownloadInfo; 
        private PerformanceTimer m_PerfTimer;


        [Flags] public enum enImportTypes
        {
            Undefiend                       = 0x00000000,
            EDDB_Commodities                = 0x00000001,           /* default is "commodities.json" */  
            RN_Localizations_Commodities    = 0x00000002,           /* default is "newCommodityClassification.xml" */
            RN_Localizations_EcoLevels      = 0x00000004,           /* default is "newCommodityClassification.xml" */
            RN_SelfAddedLocalizations       = 0x00000008,           /* default is "Commodities_Own.xml" */
            RN_Pricewarnlevels              = 0x00000010,           /* default is "Commodities_RN.json" */
            EDDB_Systems                    = 0x00000020,           /* default is "systems.json" */  
            EDDB_Stations                   = 0x00000040,           /* default is "stations.json" */  
            RN_Systems                      = 0x00000080,           /* default is "systems_own.json" */  
            RN_Stations                     = 0x00000100,           /* default is "stations_own.json" */  
            RN_CommandersLog                = 0x00000200,           /* default is "CommandersLogAutoSave.xml" */  
            RN_StationHistory               = 0x00000400,           /* default is "StationHistory.json" */  
            RN_MarketData                   = 0x00000800,           /* default is "AutoSave.csv" */  
            IBE_Localizations_Commodities   = 0x00001000,           /* default is "commodities.csv"  */ 
            EDDB_MarketData                 = 0x00002000,           /* default is "listings.csv"  */
            EDCD_Commodity                  = 0x00004000,           /* default is "EDCD_commodity.csv"  */
            EDCD_Outfitting                 = 0x00008000,           /* default is "EDCD_outfitting.csv"  */
            EDCD_Shipyard                   = 0x00010000,           /* default is "EDCD_shipyard.csv"  */
            EDDB_Systems_Populated          = 0x00020000,           /* default is "systems_populated.json" */  
        }

        public frmDataIO()
        {
            InitializeComponent();

            localSynchronizationContext = SynchronizationContext.Current;

            this.Load             += frmDataIO_Load;
            m_InfoTarget        = null;

            // https://api.github.com/repos/EDCD/FDevIDs/commits?commodity.csv
            m_DownLoader = new WebClient();
            m_DownLoader.DownloadFileCompleted   += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            m_DownLoader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            m_PerfTimer = new PerformanceTimer();
        }

        private void frmDataIO_Load(object sender, EventArgs e)
        {
            try
            {
                cmdImportOldData.Enabled    = !Program.Data.OldDataImportDone;
                m_GUIInterface              = new DBGuiInterface(DB_GROUPNAME, Program.DBCon);

                m_GUIInterface.loadAllSettings(this);

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in Load-Event");
            }
        }

        private void SetButtons(Boolean setEnabled)
        {
            try
            {

                localSynchronizationContext.Send(new SendOrPostCallback(o =>
                {
                    cmdImportOldData.Enabled                        = (!Program.Data.OldDataImportDone) && (Boolean)o;
                    cmdImportCommandersLog.Enabled                  = (Boolean)o;
                    cmdDownloadSystemsAndStations.Enabled           = (Boolean)o;
                    cmdImportSystemsAndStationsFromDownload.Enabled = (Boolean)o;
                    cmdImportSystemsAndStations.Enabled             = (Boolean)o;
                    cmdExportCSV.Enabled                            = (Boolean)o;
                    rbDefaultLanguage.Enabled                       = (Boolean)o;
                    rbUserLanguage.Enabled                          = (Boolean)o;
                    cmdImportFromCSV.Enabled                        = (Boolean)o;
                    rbImportNewer.Enabled                           = (Boolean)o;
                    rbImportSame.Enabled                            = (Boolean)o;
                    cmdTest.Enabled                                 = (Boolean)o;
                    cmdPurgeOldData.Enabled                         = (Boolean)o;
                    rbFormatExtended.Enabled                        = (Boolean)o;
                    rbFormatSimple.Enabled                          = (Boolean)o;
                    cmdExit.Enabled                                 = (Boolean)o; 
                    cmdEDCDDownloadID.Enabled                       = (Boolean)o;
                    cmdEDCDImportID.Enabled                         = (Boolean)o;

                    rbImportPrices_No.Enabled                       = (Boolean)o;
                    rbImportPrices_Bubble.Enabled                   = (Boolean)o;
                    rbImportPrices_All.Enabled                      = (Boolean)o;

                    cmdPurgeNotMoreExistingDataDays.Enabled         = (Boolean)o;
                    cbCheckObsoleteOnRecieve.Enabled                = (Boolean)o;
                    cbAutoImportEDCD.Enabled                        = (Boolean)o;

                    nudPurgeOldDataDays.Enabled                     = (Boolean)o;
                    nudPurgeNotMoreExistingDataDays.Enabled         = (Boolean)o;
                    cmdDeleteUnusedSystemData.Enabled               = (Boolean)o;


                }), setEnabled);

                if (!setEnabled)
                    m_CancelAction = false;

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while dis/enabling buttons");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async void StartEDCDCheck()
        {
            try
            { 
                String baseUrl         = "https://raw.githubusercontent.com/EDCD/FDevIDs/master/";
                String gitCheckUrl     = "https://api.github.com/repos/EDCD/FDevIDs/";
                String savePrefix      = "EDCD_";
                String infoString      = "connecting to github.com...";
                List<String> filesList = new List<string>(m_DL_Files_EDCD);
                String specialDestiationFolder = "";

                if(Debugger.IsAttached && ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                    specialDestiationFolder = Program.GetDataPath(@"..\..\..\Data\");

                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info ="checking for new EDCD data..."});

                DownloadFiles(baseUrl, savePrefix, infoString, filesList, gitCheckUrl, specialDestiationFolder, false);

                if(m_GotNewEDCDFiles)
                {
                    PriceImportParameters importParams = null;

                    enImportTypes importFlags = enImportTypes.EDCD_Outfitting | enImportTypes.EDCD_Commodity | enImportTypes.EDCD_Shipyard;

                    Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Info="importing new ids from EDCD...", NewLine=true});

                    ImportDataAsync(null, importFlags, null, m_DataPath, false, null);

                    Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Info="importing new ids from EDCD...<OK>", ForceRefresh = true});
                }
                else
                {
                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info ="checking for new EDCD data...<OK>"});
                }


            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while downloading files with EDCD-IDs");
            }
        }

        /// <summary>
        /// start the master import (master data for systems/station/commoditynames ...)
        /// </summary>
        public async Task StartMasterImport(String path)
        {
            this.Visible = false;

            try
            {
                enImportTypes importFlags = enImportTypes.EDDB_Commodities | 
                                            enImportTypes.EDDB_Systems_Populated | 
                                            enImportTypes.EDDB_Stations |
                                            enImportTypes.IBE_Localizations_Commodities |
                                            enImportTypes.RN_Localizations_EcoLevels | 
                                            enImportTypes.EDCD_Outfitting | 
                                            enImportTypes.EDCD_Commodity | 
                                            enImportTypes.EDCD_Shipyard;

                var t = new Task(() => ImportDataAsync(null, importFlags, null, path));
                t.Start();
                await t;

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while starting master import");
            }
        }

        /// <summary>
        /// start the master update (data for systems/stations/commoditynames ...)
        /// </summary>
        public async Task StartMasterUpdate(String path)
        {
            this.Visible = false;

            try
            {
                enImportTypes importFlags = enImportTypes.EDDB_Commodities | 
                                            enImportTypes.EDDB_Systems_Populated | 
                                            enImportTypes.EDDB_Stations | 
                                            enImportTypes.EDCD_Outfitting | 
                                            enImportTypes.EDCD_Commodity | 
                                            enImportTypes.EDCD_Shipyard;

                var t = new Task(() => ImportDataAsync(null, importFlags, null, path));
                t.Start();
                await t;

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while starting master update");
            }
        }

        /// <summary>
        /// start the master update (data for systems/stations/commoditynames ...)
        /// </summary>
        public async Task StartFDevIDImport(String path)
        {
            this.Visible = false;

            try
            {
                enImportTypes importFlags = enImportTypes.EDCD_Outfitting | 
                                            enImportTypes.EDCD_Commodity | 
                                            enImportTypes.EDCD_Shipyard;

                var t = new Task(() => ImportDataAsync(null, importFlags, null, path));
                t.Start();
                await t;

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while starting FDevID import");
            }
        }

        /// <summary>
        /// checks if a file ist existing, if not it shows a info-message
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private bool FileExistsOrMessage(string RNPath, string FileName)
        {
            Boolean retValue;
            try
            {
                if(File.Exists(Path.Combine(RNPath, FileName)))
                    retValue = true;
                else 
                {
                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="Skipping <" + FileName + "> - file not found.", CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                    retValue = false;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking for file", ex);
            }
        }

        /// <summary>
        /// shows the data progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Data_Progress(object sender, EliteDBIO.ProgressEventArgs e)
        {
            try
            {
                ListBox destination;

                if(InfoTarget != null)
                    usedSynchronizationContext = InfoTarget.ThreadSynchronizationContext;
                else
                     usedSynchronizationContext = localSynchronizationContext;

                usedSynchronizationContext.Send(new SendOrPostCallback(o =>
                {
                    if (m_InfoTarget != null)
                    {
                        destination     = m_InfoTarget.InfoTarget;
                        e.AddSeparator  = false;
                    }
                    else
                    {
                        destination = lbProgess;
                    }

                
                    if(((EliteDBIO.ProgressEventArgs)o).Clear)
                        destination.Items.Clear();

                    if (((EliteDBIO.ProgressEventArgs)o).AddSeparator)
                    {
                        destination.Items.Add("-------------------------------");
                        destination.Items.Add("");
                    }
                
                    if(((EliteDBIO.ProgressEventArgs)o).CurrentValue > 0)
                    {
                        if (((EliteDBIO.ProgressEventArgs)o).NewLine || (destination.Items.Count <= 0))
                        {
                            if(((EliteDBIO.ProgressEventArgs)o).TotalValue >= 0)
                                destination.Items.Add(String.Format("{0} : {1}% ({2} of {3}{4})", ((EliteDBIO.ProgressEventArgs)o).Info, 100 * ((EliteDBIO.ProgressEventArgs)o).CurrentValue / ((EliteDBIO.ProgressEventArgs)o).TotalValue, ((EliteDBIO.ProgressEventArgs)o).CurrentValue, ((EliteDBIO.ProgressEventArgs)o).TotalValue, ((EliteDBIO.ProgressEventArgs)o).Unit));
                            else
                                destination.Items.Add(String.Format("{0} : {2}{4}", ((EliteDBIO.ProgressEventArgs)o).Info, 0, ((EliteDBIO.ProgressEventArgs)o).CurrentValue, 0, ((EliteDBIO.ProgressEventArgs)o).Unit));
                    
                        }
                        else
                        {
                            if(((EliteDBIO.ProgressEventArgs)o).TotalValue > 0)
                                destination.Items[destination.Items.Count - 1] = String.Format("{0} : {1}% ({2} of {3}{4})", ((EliteDBIO.ProgressEventArgs)o).Info, 100 * ((EliteDBIO.ProgressEventArgs)o).CurrentValue / ((EliteDBIO.ProgressEventArgs)o).TotalValue, ((EliteDBIO.ProgressEventArgs)o).CurrentValue, ((EliteDBIO.ProgressEventArgs)o).TotalValue, ((EliteDBIO.ProgressEventArgs)o).Unit);
                            else
                                destination.Items[destination.Items.Count - 1] = String.Format("{0} : {2}{4}", ((EliteDBIO.ProgressEventArgs)o).Info, 0, ((EliteDBIO.ProgressEventArgs)o).CurrentValue, 0, ((EliteDBIO.ProgressEventArgs)o).Unit);
                        }

                        if(destination.TopIndex != (destination.Items.Count - 1))
                            destination.TopIndex = destination.Items.Count - 1;

                        destination.Refresh();
                    }
                    else if(((EliteDBIO.ProgressEventArgs)o).Info != null)
                    {
                        if (((EliteDBIO.ProgressEventArgs)o).NewLine || (destination.Items.Count <= 0))
                        {
                            destination.Items.Add(((EliteDBIO.ProgressEventArgs)o).Info);
                        }
                        else
                        {
                            destination.Items[destination.Items.Count - 1] = ((EliteDBIO.ProgressEventArgs)o).Info;
                        }

                        if (destination.TopIndex != (destination.Items.Count - 1))
                            destination.TopIndex = destination.Items.Count - 1;

                        destination.Refresh();
                    }

                }), e);

                if(m_CancelAction)
                    e.Cancelled = true;

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while reporting progress (new)");
            }
        }

        /// <summary>
        /// main working routine for importing data
        /// </summary>
        /// <param name="importInfo">if this is set a FolderDialog will ask with this text for a        /// If this is not set, the path from "General"->"Path_Import" or from "optionalPath" will be taken</param>
        /// <param name="importFlags">flags what to import</param>
        /// <param name="optionalFilter">filefilter (only for importing Commander's Log multiple times)</param>
        /// <param name="optionalPath">preset for the import path. Also taken by the FolderDialog if importInfo is set</param>
        /// <param name="RNData">causes to look for some files in the "/data/" subdirectory (for importing old RN data) </param>
        /// <param name="RNData">for control type of import EDDB price data (only for enImportTypes.EDDB_MarketData) </param>
        /// <returns></returns>
        private async Task<Boolean> ImportDataAsync(string importInfo, enImportTypes importFlags, String optionalFilter = "", String optionalPath = "", Boolean RNData = false, PriceImportParameters importParams = null)
        {
            String FileName;
            String sourcePath;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<int,int>();
            Boolean retValue = false;
            Boolean restartEDDN = false;
            Boolean stationOrCommodityImport = false;
            FolderBrowserDialog fbFolderDialog = new FolderBrowserDialog();
            OpenFileDialog foDialog = new OpenFileDialog();
            DialogResult dlgResult= DialogResult.None;

            try
            {
                if (optionalPath == "")
                    sourcePath = Program.DBCon.getIniValue("General", "Path_Import", Program.GetDataPath("data"), false);
                else
                    sourcePath = optionalPath;

                if (!String.IsNullOrEmpty(importInfo))
                {
                    fbFolderDialog.RootFolder   = Environment.SpecialFolder.MyComputer;
                    fbFolderDialog.SelectedPath = sourcePath;
                    fbFolderDialog.Description  = importInfo;
                    

                    dlgResult = ShowInvokedDialog((CommonDialog)fbFolderDialog);

                }

                if ((importInfo == null) || (dlgResult == System.Windows.Forms.DialogResult.OK))
                {
                    if (importInfo != null)
                        sourcePath = fbFolderDialog.SelectedPath.Trim();
                    

                    if (!String.IsNullOrEmpty(sourcePath))
                    {
                        if(Program.EDDNComm.ListenersRunning > 0)
                        { 
                            Program.EDDNComm.StopEDDNListening();
                            restartEDDN = true;
                        }

                        if (importInfo != null)
                            Program.DBCon.setIniValue("General", "Path_Import", sourcePath);

                        Program.Data.Progress += Data_Progress;

                        Application.DoEvents();


                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDDB_Commodities))
                        {
                            // import the commodities from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commodities...", AddSeparator=true});
                            FileName = "commodities.json";
                            
                            if(RNData)
                                FileName = @"Data\" + FileName;


                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                String existingHash = Program.DBCon.getIniValue<String>("ImportFileMD5", FileName, "");
                                String currentHash  = BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(File.ReadAllBytes(Path.Combine(sourcePath, FileName)))).Replace("-", "");

                                if(String.IsNullOrWhiteSpace(existingHash) || (existingHash != currentHash))
                                {
                                    Program.Data.ImportCommoditiesFromFile(Path.Combine(sourcePath, FileName));
                                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcategory.TableName);
                                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commodities...<OK>", NewLine = true});
                                    stationOrCommodityImport = true;
                                    Program.DBCon.setIniValue("ImportFileMD5", FileName, currentHash);
                                }
                                else
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File skipped (already imported) : " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });

                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_Localizations_Commodities))
                        {
                            // import the localizations (commodities) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commodity localizations...", AddSeparator=true});
                            FileName = "Commodities.xml";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportCommodityLocalizations(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commodity localizations...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.IBE_Localizations_Commodities))
                        {
                            // import the new localizations (commodities) from csv-fileName
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commodity localizations...", AddSeparator=true});
                            FileName = "commodities.csv";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportLocalizationDataFromCSV(Path.Combine(sourcePath, FileName), SQL.EliteDBIO.enLocalizationType.Commodity, SQL.EliteDBIO.enLocalisationImportType.overwriteNonBase);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commodity localizations...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_Localizations_EcoLevels))
                        {
                            // import the localizations (economy levels) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import economy level localizations...", AddSeparator=true});
                            FileName = "Commodities.xml";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportEconomyLevelLocalizations(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tblevellocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbeconomylevel.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import economy level localizations...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }


                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_SelfAddedLocalizations))
                        {
                            // import the self added localizations from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import self-added commodity localizations...", AddSeparator=true});
                            FileName = "Commodities_Own.xml";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportCommodityLocalizations(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import self-added commodity localizations...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_Pricewarnlevels))
                        {
                            // import the pricewarnlevels from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import pricewarnlevels...", AddSeparator=true});
                            FileName = "Commodities_RN.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportCommodityPriceWarnLevels(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import pricewarnlevels...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDDB_Systems))
                        {
                            // import the systems and stations from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import systems...", AddSeparator=true});
                            FileName = "systems.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportSystems(Path.Combine(sourcePath, FileName));
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import systems...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDDB_Systems_Populated))
                        {
                            // import the systems and stations from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import populated systems...", AddSeparator=true});
                            FileName = "systems_populated.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                String existingHash = Program.DBCon.getIniValue<String>("ImportFileMD5", FileName, "");
                                String currentHash  = BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(File.ReadAllBytes(Path.Combine(sourcePath, FileName)))).Replace("-", "");

                                if(String.IsNullOrWhiteSpace(existingHash) || (existingHash != currentHash))
                                {
                                    Program.Data.ImportSystems(Path.Combine(sourcePath, FileName));
                                    //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                    //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems_org.TableName);
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import populated systems...<OK>", NewLine = true});
                                    stationOrCommodityImport = true;
                                    Program.DBCon.setIniValue("ImportFileMD5", FileName, currentHash);
                                }
                                else
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File skipped (already imported) : " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });

                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDDB_Stations))
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import stations...", AddSeparator=true});
                            FileName = "stations.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                String existingHash = Program.DBCon.getIniValue<String>("ImportFileMD5", FileName, "");
                                String currentHash  = BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(File.ReadAllBytes(Path.Combine(sourcePath, FileName)))).Replace("-", "");

                                if(String.IsNullOrWhiteSpace(existingHash) || (existingHash != currentHash))
                                {
                                    Program.Data.ImportStations(Path.Combine(sourcePath, FileName), false);
                                    //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                    //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations_org.TableName);
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import stations...<OK>", NewLine = true});
                                    stationOrCommodityImport = true;
                                    Program.DBCon.setIniValue("ImportFileMD5", FileName, currentHash);
                                }
                                else
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File skipped (already imported) : " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_Systems))
                        {
                            // import the self-changed or added systems and stations
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import self-added systems...", AddSeparator=true});
                            FileName = "systems_own.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                changedSystemIDs = Program.Data.ImportSystems_Own(Path.Combine(sourcePath, FileName));
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import self-added systems...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_Stations))
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import self-added stations...", AddSeparator=true});
                            FileName = "stations_own.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportStations_Own(Path.Combine(sourcePath, FileName), changedSystemIDs);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import self-added stations...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_CommandersLog))
                        {
                            // import the Commander's Log from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commander's log...", AddSeparator=true});
                            string[] files;
                            string Filter;

                            if (!String.IsNullOrEmpty(optionalFilter))
                                Filter = optionalFilter;
                            else
                                Filter = "CommandersLogAutoSave.xml";

                            files = Directory.GetFiles(sourcePath, Filter);

                            if (files.GetUpperBound(0) >= 0)
                            {
                                foreach (String importFile in files)
                                {
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = "importing " + importFile + "...", AddSeparator=true });

                                    Int32 added = Program.Data.ImportCommandersLog(Path.Combine(sourcePath, importFile));
                                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                    
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = "importing " + importFile + "... (" + added + " new entries)", CurrentValue = 1, TotalValue = 1, ForceRefresh = true });
                                }
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import commander's log...<OK>", NewLine = true});

                                Program.Data.RecalcJumpDistancesInLog(new DateTime(1970, 01, 01));

                                m_DataImportHappened = true;              
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + Filter, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_StationHistory))
                        {
                            //import the history of visited stations
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import visited stations...", AddSeparator=true});
                            FileName = "StationHistory.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportVisitedStations(Path.Combine(sourcePath, FileName));
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import visited stations...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.RN_MarketData))
                        {
                            //import the self collected price data
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import collected price data...", AddSeparator=true});
                            FileName = "AutoSave.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportPricesFromCSVFile(Path.Combine(sourcePath, FileName), EliteDBIO.enImportBehaviour.OnlyNewer, EliteDBIO.enDataSource.fromRN);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import collected price data...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDDB_MarketData))
                        {
                            //import the self collected price data
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import EDDN price data...", AddSeparator=true});
                            //import the price data from EDDB
                            FileName = "listings.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.visystemsandstations.TableName);
                                Program.Data.ImportPricesFromCSVFile(Path.Combine(sourcePath, FileName), EliteDBIO.enImportBehaviour.OnlyNewer, EliteDBIO.enDataSource.fromFILE, importParams);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="import EDDN price data...<OK>", NewLine = true});
                                stationOrCommodityImport = true;
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDCD_Outfitting))
                        {
                            //import the outfitting data from EDCD
                            FileName = "EDCD_outfitting.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportEDCDData(enImportTypes.EDCD_Outfitting, Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables("tboutfittingbase");

                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDCD_Shipyard))
                        {
                            //import the shipyard data from EDCD
                            FileName = "EDCD_shipyard.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportEDCDData(enImportTypes.EDCD_Shipyard, Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables("tbshipyardbase");

                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if ((!m_CancelAction) && importFlags.HasFlag(enImportTypes.EDCD_Commodity))
                        {
                            //import the commodity data from EDCD
                            FileName = "EDCD_commodity.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportEDCDData(enImportTypes.EDCD_Commodity, Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables("tbcommoditybase");
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info="File not found: " + FileName, CurrentValue= 1, TotalValue=1, ForceRefresh=true });
                            }
                        }

                        if (stationOrCommodityImport)
                        {
                            // update the visited information
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="updating visited systems and stations...", AddSeparator=true});
                            Program.Data.updateVisitedBaseFromLog(SQL.EliteDBIO.enVisitType.Systems | SQL.EliteDBIO.enVisitType.Stations);
                            Program.Data.updateVisitedFlagsFromBase(true, true);
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="updating visited systems and stations...<OK>", NewLine = true});

                            // insert missing localization entries
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="insert missing translation of commodities", AddSeparator=true});
                            Program.Data.AddMissingLocalizationEntries();
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="insert missing translation of commodities...<OK>", NewLine = true});

                            // update localization of all commodities
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="updating active localization of commodities", AddSeparator=true});
                            Program.Data.updateTranslation();
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info="updating active localization of commodities...<OK>", NewLine = true});
                        }

                        Program.Data.Progress -= Data_Progress;

                        retValue = true;

                        if(restartEDDN)
                            Program.EDDNComm.StartEDDNListening();
                    }
                }

                return retValue;
            }
            catch (Exception ex)
            {
                if(restartEDDN)
                    Program.EDDNComm.StartEDDNListening();

                throw new Exception("Error while importing data to database", ex);
            }
        }

        private void cmdDownloadSystemsAndStations_Click(object sender, EventArgs e)
        {
            Debug.Print("start function base call");
            SetButtons(false);
            DownloadSystemsAndStations();
            Debug.Print("end function base call");
        }

        /// <summary>
        /// shows a dialog (auto-invoked)
        /// </summary>
        /// <param name="dialogObject"></param>
        /// <returns></returns>
        private DialogResult ShowInvokedDialog(CommonDialog dialogObject)
        {
            if(this.InvokeRequired)
            {
                return (DialogResult)this.Invoke(new Func<DialogResult>(() => ShowInvokedDialog(dialogObject)));
            }
            else
            {
                return dialogObject.ShowDialog(this);
            }
        }


        private void DownloadSystemsAndStations()
        {
            try
            {
                String baseUrl = Program.DBCon.getIniValue<String>(DB_GROUPNAME, "EDDBDumpLocation", @"https://eddb.io/archive/v5/", false);
                                                                                                       
                String savePrefix = "";
                String infoString = "connecting to eddb.io...";
                List<String> filesList;

                if (false)
                {
                    filesList = new List<string>(m_DL_Files);
                }
                else
                {
                    filesList = new List<string>(m_DL_Files_Pop);
                }

                String specialDestiationFolder = "";

                filesList.AddRange(m_DL_FilesPrice);

                if (Debugger.IsAttached && ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                    specialDestiationFolder = Program.GetDataPath(@"..\..\..\Data\");

                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Clear = true });

                DownloadFiles(baseUrl, savePrefix, infoString, filesList, "", specialDestiationFolder);

            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while downloading system/station data from EDDB");
            }
        }

        /// <summary>
        /// checks if the downloadfiles existing
        /// </summary>
        /// <returns></returns>
        private Boolean EDDBDownloadComplete()
        {

            String currentDestinationFile;
            Boolean retValue = true;
            List<String>        filesList = new List<string>(m_DL_Files);
                                              
            try
            {
                if(!rbImportPrices_No.Checked)
                    filesList.AddRange(m_DL_FilesPrice);

                for (int i = 0; i < filesList.Count; i++)
                {
                    currentDestinationFile = Path.Combine(m_DataPath, filesList[i]);

                    if(!File.Exists(currentDestinationFile))
                    { 
                        retValue = false;
                        break;
                    }
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking EDDB files", ex);
            }
        }

        private async void cmdImportSystemsAndStationsFromDownload_Click(object sender, EventArgs e)
        {
            PriceImportParameters importParams = null;
            Boolean cantImport = false;
            enImportTypes importFlags;
                                  
            try
            {
                SetButtons(false);
                if (false)
                {
                    importFlags = enImportTypes.EDDB_Commodities |
                                  enImportTypes.EDDB_Systems |
                                  enImportTypes.EDDB_Stations;
                }
                else
                {
                    importFlags = enImportTypes.EDDB_Commodities | 
                                  enImportTypes.EDDB_Systems_Populated | 
                                  enImportTypes.EDDB_Stations;

                }

                if(!rbImportPrices_No.Checked)
                {
                    importFlags |= enImportTypes.EDDB_MarketData;

                    if(rbImportPrices_Bubble.Checked)
                    { 
                        if(String.IsNullOrEmpty(Program.actualCondition.System))
                        { 
                            MessageBox.Show(this, "Import is not possible because your current system is unknown !\r\n\r\n"+
                                                  "Be sure 'VerboseLogging' is enabled in your ED 'AppConfig.xml'\r\n" +
                                                  "and restart the game\r\n\r\n" +
                                                  "Retry import if your system is shown in the 'Current System' field.", "Import not possible !",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            cantImport = true;
                        }
                        else
                            importParams = new PriceImportParameters() { Radius = txtBubbleSize.Int32Value, SystemID = Program.actualCondition.System_ID.Value};

                    }
                }

                if(!cantImport)
                { 
                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() {Info = "importing EDDN data...", Clear = true });

                    var t = new Task(() => ImportDataAsync(null, importFlags, null, m_DataPath, false, importParams));
                    t.Start();
                    await t;

                    //synchronizationContext.Send(new SendOrPostCallback(o =>
                    //{
                        if(m_CancelAction)
                            MessageBox.Show(this, "Import was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                            MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}), null);

                    m_DataImportHappened = true;
                }

                SetButtons(true);
                
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while importing downloaded system/station data from EDDB");
            }
        }

        private async void cmdImportSystemsAndStations_Click(object sender, EventArgs e)
        {
            PriceImportParameters importParams = null;

            try
            {
                SetButtons(false);
                enImportTypes importFlags = enImportTypes.EDDB_Commodities | 
                                            enImportTypes.EDDB_Systems_Populated | 
                                            enImportTypes.EDDB_Stations;

                if(!rbImportPrices_No.Checked)
                {
                    importFlags |= enImportTypes.EDDB_MarketData;

                    if(rbImportPrices_Bubble.Checked)
                        importParams = new PriceImportParameters() { Radius = 20, SystemID = Program.actualCondition.System_ID.Value};
                }

                Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Info="importing EDDN data...", Clear=true});


                var t = new Task(() => ImportDataAsync("Select folder with system/station datafiles (systems.json/stations.json/commodities.json)", importFlags, "", "", false, importParams));
                t.Start();
                await t;

                if(m_CancelAction)
                    MessageBox.Show(this, "Import was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetButtons(true);

                m_DataImportHappened = true;
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while importing system/station data from EDDB");
            }
        }


        private async void cmdImportCommandersLog_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtons(false);

                Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Clear=true });

                enImportTypes importFlags = enImportTypes.RN_CommandersLog;
                var t = new Task(() => ImportDataAsync("Select folder with your old Commander's Log data (accepted file pattern : CommandersLog*.xml)", importFlags, "CommandersLog*.xml"));
                t.Start();
                await t;

                SetButtons(true);

                m_DataImportHappened = true;

            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while importing system/station data from EDDN");
            }
        }

        /// <summary>
        /// imports the collected data from the old RN directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void cmdImportOldData_Click(object sender, EventArgs e)
        {
            String RNPath;
            FolderBrowserDialog fbFolderDialog = new FolderBrowserDialog();

            try
            {
                SetButtons(false);

                
                enImportTypes importFlags = enImportTypes.RN_SelfAddedLocalizations |  
                                            enImportTypes.RN_Pricewarnlevels | 
                                            enImportTypes.RN_Systems | 
                                            enImportTypes.RN_Stations | 
                                            enImportTypes.RN_CommandersLog | 
                                            enImportTypes.RN_StationHistory | 
                                            enImportTypes.RN_MarketData;

                fbFolderDialog.RootFolder       = Environment.SpecialFolder.MyComputer;
                fbFolderDialog.Description      = "Select your RN-Folder with the old data files ....";
                fbFolderDialog.SelectedPath     = System.IO.Directory.GetCurrentDirectory();

                if (fbFolderDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    RNPath = fbFolderDialog.SelectedPath.Trim();

                    if (!String.IsNullOrEmpty(RNPath))
                    {
                        if (File.Exists(Path.Combine(RNPath, "RegulatedNoise.exe")))
                        {
                            Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Clear=true });

                            Program.Data.Progress += Data_Progress;
                            Cursor = Cursors.WaitCursor;

                            Application.DoEvents();

                            var t = new Task(() => ImportDataAsync(null, importFlags, null, RNPath, true));
                            t.Start();
                            await t;

                            Program.Data.OldDataImportDone = true;
                            cmdImportOldData.Enabled = false;

                            MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Program.Data.Progress -= Data_Progress;
                            Cursor = Cursors.Default;
                        }
                        else
                        {
                            MessageBox.Show(this, "<RegulatedNoise.exe> not found. Wrong directory ?", "Data import",  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }

                SetButtons(true);
            }
            catch (Exception ex)
            {
                SetButtons(true);
                Cursor = Cursors.Default;
                CErr.processError(ex, "Error while importing the existing RN-data");
            }
        }

        /// <summary>
        /// exports the market data to a csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void cmdExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtons(false);

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter              = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.DefaultExt          = "csv";
                saveFileDialog1.Title               = "Export market data";
                saveFileDialog1.OverwritePrompt     = true;
                saveFileDialog1.InitialDirectory    = Program.DBCon.getIniValue("General", "Path_CSVExport", Program.GetDataPath("data"), false);

	            DialogResult result = saveFileDialog1.ShowDialog();

		        if (result == DialogResult.OK)
                {
                    Program.DBCon.setIniValue("General", "Path_CSVExport", Path.GetDirectoryName(saveFileDialog1.FileName));

                    Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Info="exporting data to csv...", Clear=true});

                    Program.Data.Progress += Data_Progress;

                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = "export prices to csv...", CurrentValue = 0, TotalValue = 0, AddSeparator=true });

                    var t = new Task(() => Program.Data.ExportMarketDataToCSV(saveFileDialog1.FileName, rbUserLanguage.Checked, rbFormatExtended.Checked));
                    t.Start();
                    await t;

                    Program.Data.Progress -= Data_Progress;

                    if(m_CancelAction)
                        MessageBox.Show(this, "Export was cancelled and is unfinished!", "Data export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Export has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SetButtons(true);
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while exporting to csv");
            }
        }

        /// <summary>
        /// imports the market data from a csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void cmdImportFromCSV_Click(object sender, EventArgs e)
        {
            String sourcePath;
            OpenFileDialog fbFileDialog;
            try
            {
                SetButtons(false);

                sourcePath = Program.DBCon.getIniValue("General", "Path_CSVImport", Program.GetDataPath("data"), false);

                fbFileDialog = new OpenFileDialog();
                fbFileDialog.InitialDirectory = sourcePath;
                fbFileDialog.Title = "Select the csv-file with market data to import...";
                fbFileDialog.CheckFileExists    = true;
                fbFileDialog.Filter             = "CVS files (*.csv)|*.csv|Text documents (*.txt)|*.txt|All files (*.*)|*.*";
                fbFileDialog.FilterIndex = 1;


                fbFileDialog.ShowDialog(this);

                if ((!String.IsNullOrEmpty(fbFileDialog.FileName)) && System.IO.File.Exists(fbFileDialog.FileName))
                {
                    
                    Program.DBCon.setIniValue("General", "Path_CSVImport", Path.GetDirectoryName(fbFileDialog.FileName));

                    Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Info="importing data from csv...", Clear=true});

                    Program.Data.Progress += Data_Progress;

                    SQL.EliteDBIO.enImportBehaviour importBehaviour = SQL.EliteDBIO.enImportBehaviour.OnlyNewer;
                    if(rbImportSame.Checked)
                        importBehaviour = SQL.EliteDBIO.enImportBehaviour.NewerOrEqual;

                    var t = new Task(() => Program.Data.ImportPricesFromCSVFile(fbFileDialog.FileName, importBehaviour, EliteDBIO.enDataSource.fromFILE));
                    t.Start();
                    await t;

                    Program.Data.Progress -= Data_Progress;

                    if(m_CancelAction)
                        MessageBox.Show(this, "Import was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SetButtons(true);

                m_DataImportHappened = true;
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while importing from csv");
            }       
        }


        private void cmdTest_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtons(false);

                Program.Data.AddMissingLocalizationEntries();

                SetButtons(true);
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex);
            }
        }

        private void frmDataIO_FormClosed(object sender, FormClosedEventArgs e)
        {
            try 
	        {	
                if (m_DataImportHappened)
                { 
                    Program.PriceAnalysis.GUI.setFilterHasChanged(true);
                    Program.CommandersLog.GUI.RefreshData();
                }
	        }
	        catch (Exception ex)
	        {
		        CErr.processError(ex, "Error in frmDataIO_FormClosed");
	        }
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdExit_Click");   
            }
        }

        private async void cmdPurgeOldData_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show(String.Format("Delete all data older than {0} days", nudPurgeOldDataDays.Value), "Delete old price data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.OK)
                {
                    Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Clear=true });

                    SetButtons(false);

                    DateTime deadline = DateTime.UtcNow.AddDays(-1*(Int32)(nudPurgeOldDataDays.Value)).Date;

                    Program.Data.Progress += Data_Progress;

                    var t = new Task(() => Program.Data.DeleteMarketData((Int32)nudPurgeOldDataDays.Value));
                    t.Start();
                    await t;

                    Program.Data.Progress -= Data_Progress;

                    if(m_CancelAction)
                        MessageBox.Show(this, "Deleting was cancelled !", "Deleting old data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Deleting of old data finished !", "Deleting old data", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SetButtons(true);
                }
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error in cmdPurgeOldData_Click");
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
                CErr.processError(ex, "Error in nud*****_KeyDown");
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
                CErr.processError(ex, "Error in nud*****_Leave");
            }
        }

        private void frmDataIO_Shown(object sender, EventArgs e)
        {
            try
            {
                SetButtons(true);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in Shown event");
            }
        }

        private void frmDataIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!cmdExit.Enabled)
                e.Cancel = true;
        }

        private void Radiobutton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in Radiobutton_CheckedChanged");
            }
        }

        /// <summary>
        /// starts download of the files
        /// </summary>
        /// <param name="baseUrl">download url of the files</param>
        /// <param name="savePrefix">name prefix for download files, takes original name if empty</param>
        /// <param name="infoString">infostring for messages</param>
        /// <param name="filesList">list of files to download to</param>
        /// <param name="gitCheckPath">if not empty assuming download grom github with a special age-check </param>
        private void DownloadFiles(String baseUrl, String savePrefix, String infoString, List<String> filesList, String gitCheckPath = "", String specialDestiationFolder = "", Boolean asyncDownload = true)
        {
            List<DateTime> filesTimes = new List<DateTime>();

            Boolean download;
            String currentDestinationFile;
            String lDataPath = m_DataPath;

            try
            {
                m_GotNewEDCDFiles = false;

                if(!String.IsNullOrEmpty(specialDestiationFolder))
                    lDataPath = specialDestiationFolder;                    

                // get the timestamps from the webfiles
                for (int i = 0; i < filesList.Count; i++)
                {
                    if (String.IsNullOrEmpty(gitCheckPath))
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + filesList[i]);
                        request.Method = "HEAD";
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            filesTimes.Add(response.LastModified);
                    }
                    else
                    {
                        String releaseData;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(gitCheckPath + "commits?" + filesList[i]);
                        request.Method = "GET";
                        request.UserAgent = "ED-IBE";
                        request.ServicePoint.Expect100Continue = false;
                        using (StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream()))
                        {
                            releaseData      = responseReader.ReadToEnd();
                            JObject data     = (JObject)(Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(releaseData).First);
                            JToken dateValue = data.SelectToken("commit.committer.date");

                            if(dateValue != null)
                            {
                                DateTime parsed = DateTime.UtcNow;

                                if(DateTime.TryParse(dateValue.ToString(), out parsed))
                                    filesTimes.Add(parsed);
                                else
                                    filesTimes.Add(DateTime.UtcNow);
                            }
                            else
                            {
                                filesTimes.Add(DateTime.UtcNow);
                            }
                        }
                    }
                }

                m_CancelAction                      = false;

                // download file if newer
                for (int i = 0; i < filesList.Count; i++)
                {
                    download = false;

                    currentDestinationFile = Path.Combine(lDataPath, savePrefix + filesList[i]);

                    if (File.Exists(currentDestinationFile))
                    {
                        var localFileTime = File.GetCreationTime(currentDestinationFile);

                        if (filesTimes[i] != localFileTime)
                        {
                            download = true;
                            File.Delete(currentDestinationFile);
                        }
                    }
                    else
                        download = true;

                    if(asyncDownload)
                    {
                        if (download)
                        {
                            var downloadInfo = String.Format("downloading file {0} of {1}: {2}...", i + 1, filesList.Count, filesList[i]);

                            m_DownloadQueue.Enqueue(new DownloadData() { DownloadUri = new Uri(baseUrl + filesList[i]),
                                                                         LocalFileName = currentDestinationFile,
                                                                         BelongingFileTime = filesTimes[i],
                                                                         InfoString = downloadInfo,
                                                                         TotalDownloadBytes = 0,
                                                                         UsedUnit = "" });
                            m_GotNewEDCDFiles = true;
                        }
                        else
                        {
                            var downloadInfo = String.Format("skipping download {0} of {1}: {2} - newest version already existing...", i + 1, filesList.Count, savePrefix + filesList[i]);
                            m_DownloadQueue.Enqueue(new DownloadData() { InfoString = downloadInfo});
                        }
                        tmrDownload.Start();
                    }
                    else
                    {
                        if (download)
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info=String.Format("downloading file {0} of {1}: {2}...", i + 1, filesList.Count, filesList[i]), NewLine=true});

                            WebClient webClient = new WebClient();
                            webClient.DownloadFile(new Uri(baseUrl + filesList[i]), currentDestinationFile);
                            File.SetCreationTime(currentDestinationFile, filesTimes[i]);

                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info=String.Format("downloading file {0} of {1}: {2}...<OK>", i + 1, filesList.Count, filesList[i]), ForceRefresh=true});

                            m_GotNewEDCDFiles = true;
                        }
                    }

                    if (m_CancelAction)
                        break;
                }

            }
            catch (Exception ex)
            {
                if(!ex.GetBaseException().Message.Contains("'api.github.com'"))
                {
                    throw new Exception("Error while initiating the download of files", ex);
                }
                
            }
        }

        private void tmrDownload_Tick(object sender, EventArgs e)
        {
            try
            {
                Debug.Print("check for downloads...");
                tmrDownload.Stop();

                if (m_CancelAction)
                {
                    tmrDownload.Stop();
                    m_DownloadQueue.Clear();
                    m_CurrentDownloadInfo = null;
                    SetButtons(true);
                }
                else
                {
                    if(m_DownloadQueue.Count > 0)
                    {
                        tmrDownload.Stop();
                        if(m_CurrentDownloadInfo != null)
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { AddSeparator = true });

                        m_CurrentDownloadInfo = (DownloadData)m_DownloadQueue.Dequeue();

                        if(m_CurrentDownloadInfo.DownloadUri != null)
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = "downloading " + System.IO.Path.GetFileName(m_CurrentDownloadInfo.LocalFileName) + "..." });
                            Debug.Print("Download Found: {0} to {1}", m_CurrentDownloadInfo.DownloadUri, m_CurrentDownloadInfo.LocalFileName);
                            m_PerfTimer.startMeasuring();
                            m_DownLoader.DownloadFileAsync(m_CurrentDownloadInfo.DownloadUri, m_CurrentDownloadInfo.LocalFileName);
                        }
                        else
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = m_CurrentDownloadInfo.InfoString });

                            if(m_DownloadQueue.Count > 0)
                            {
                                tmrDownload.Start();
                            }
                            else
                            {
                                m_CurrentDownloadInfo = null;
                                SetButtons(true);
                            }
                        }
                    }
                    else
                        SetButtons(true);
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while initiating the asynchronous download of files");
            }
        }

       /// <summary>
       /// returns the best unit and it's factor for the view of bytes to download
       /// </summary>
       /// <param name="bytesTotal"></param>
       /// <param name="usedPrefix"></param>
       /// <returns></returns>
        private Int32 GetPrefix(Int64 bytesTotal, ref String usedPrefix)
        {
            Int32 factor;
            if (bytesTotal > (10 * 1024))
            {
                if (bytesTotal > (10 * 1024 * 1024))
                {
                    factor = 1024 * 1024;
                    usedPrefix = " Mbyte";

                }
                else
                {
                    factor = 1024;
                    usedPrefix = " kbyte";
                }
            }
            else
            {
                factor = 1;
                usedPrefix = " byte";
            }

            return factor;
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            

            try
            {
                m_CurrentDownloadInfo.TotalDownloadBytes = e.TotalBytesToReceive;

                if(m_CancelAction)
                {
                    ((WebClient)sender).CancelAsync();
                }
                else
                {
                    if(!m_DownloadFinished)
                    { 
                        if(m_PerfTimer.currentMeasuring() > 50)
                        {
                    
                            var uUnit = m_CurrentDownloadInfo.UsedUnit;
                            var factor = GetPrefix(e.TotalBytesToReceive, ref uUnit);
                            m_CurrentDownloadInfo.UsedUnit = uUnit;
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = m_CurrentDownloadInfo.InfoString,
                                                                                  CurrentValue = (Int32)(e.BytesReceived / (factor)),
                                                                                  TotalValue = (Int32)(e.TotalBytesToReceive / (factor)),
                                                                                  Unit = m_CurrentDownloadInfo.UsedUnit });
                            m_PerfTimer.startMeasuring();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in webClient_DownloadProgressChanged");
            }
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (!m_CancelAction)
                {
                    File.SetCreationTime(m_CurrentDownloadInfo.LocalFileName, m_CurrentDownloadInfo.BelongingFileTime);
                    var uUnit = m_CurrentDownloadInfo.UsedUnit;
                    Int32 factor = GetPrefix(m_CurrentDownloadInfo.TotalDownloadBytes, ref uUnit);
                    m_CurrentDownloadInfo.UsedUnit = uUnit;
                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = m_CurrentDownloadInfo.InfoString,
                                                                          CurrentValue = (Int32)(m_CurrentDownloadInfo.TotalDownloadBytes / (factor)),
                                                                          TotalValue = (Int32)(m_CurrentDownloadInfo.TotalDownloadBytes / (factor)),
                                                                          Unit = m_CurrentDownloadInfo.UsedUnit });

                }
                else
                {
                    if (File.Exists(m_CurrentDownloadInfo.LocalFileName))
                        File.Delete(m_CurrentDownloadInfo.LocalFileName);
                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Info = "download cancelled !!! ", NewLine = true,  AddSeparator = true });
                }

                if(m_DownloadQueue.Count > 0)
                {
                    tmrDownload.Start();
                }
                else
                {
                    m_CurrentDownloadInfo = null;
                    SetButtons(true);
                }
        
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in webClient_DownloadFileCompleted");
            }
        }


        private async void cmdEDCDDownloadID_Click(object sender, EventArgs e)
        {
            try
            { 
                String baseUrl         = "https://raw.githubusercontent.com/EDCD/FDevIDs/master/";
                String gitCheckUrl     = "https://api.github.com/repos/EDCD/FDevIDs/";
                String savePrefix      = "EDCD_";
                String infoString      = "connecting to github.com...";
                List<String> filesList = new List<string>(m_DL_Files_EDCD);
                String specialDestiationFolder = "";

                SetButtons(false);

                if(Debugger.IsAttached && ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                    specialDestiationFolder = Program.GetDataPath(@"..\..\..\Data\");

                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Clear = true });

                DownloadFiles(baseUrl, savePrefix, infoString, filesList, gitCheckUrl, specialDestiationFolder);
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error while downloading files with EDCD-IDs");
            }
        }

        private async void cmdEDCDImportID_Click(object sender, EventArgs e)
        {
            PriceImportParameters importParams = null;

            try
            {
                SetButtons(false);
                enImportTypes importFlags = enImportTypes.EDCD_Outfitting | enImportTypes.EDCD_Commodity | enImportTypes.EDCD_Shipyard;

                Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Info="importing ids of known things from EDCD...", Clear=true });

                var t = new Task(() => ImportDataAsync(null, importFlags, null, m_DataPath, false, null));
                t.Start();
                await t;

                if (m_CancelAction)
                    MessageBox.Show(this, "Import was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetButtons(true);

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while downloading files with EDCD-IDs");
            }
        }

        private void lbProgess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                CopySelectedValuesToClipboard();
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new System.Text.StringBuilder();

            if (lbProgess.SelectedItems.Count > 0)
            {
                foreach (String item in lbProgess.SelectedItems)
                    builder.AppendLine(item);
            }
            else
            {
                builder.Append(lbProgess.Text);
            }
                
            Debug.Print("CtC");

            if(builder.Length > 0)
                Clipboard.SetText(builder.ToString());
            else
                Clipboard.Clear();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            m_CancelAction = true;
            Debug.Print("cancelled !");
        }

        private async void cmdPurgeNotMoreExistingDataDays_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show(String.Format("Delete all data older than {0} days", nudPurgeOldDataDays.Value), "Delete old price data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.OK)
                {
                    Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Clear=true });

                    SetButtons(false);

                    Program.Data.Progress += Data_Progress;

                    var t = new Task(() => Program.Data.DeleteNoLongerExistingMarketData((Int32)nudPurgeNotMoreExistingDataDays.Value));
                    t.Start();
                    await t;

                    Program.Data.Progress -= Data_Progress;

                    if(m_CancelAction)
                        MessageBox.Show(this, "Deleting was cancelled !", "Delete no longer existing data from stations", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Deleting of old data finished !", "Delete no longer existing data from station", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SetButtons(true);
                }
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error in cmdPurgeOldData_Click");
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in CheckBox_CheckedChanged");
            }
        }

        private async void cmdDeleteUnusedSystemData_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show(String.Format("Start cleanup ?"), "Delete unused system data...", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.OK)
                {
                    Data_Progress(this, new EliteDBIO.ProgressEventArgs() { Clear=true });

                    SetButtons(false);

                    Program.Data.Progress += Data_Progress;

                    var t = new Task(() => Program.Data.DeleteUnusedSystemData());
                    t.Start();
                    await t;

                    Program.Data.Progress -= Data_Progress;

                    if(m_CancelAction)
                        MessageBox.Show(this, "Deleting cancelled !", "Delete unused system data...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "All unused systems deleted !", "Delete unused system data...", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SetButtons(true);
                }
            }
            catch (Exception ex)
            {
                SetButtons(true);
                CErr.processError(ex, "Error in cmdDeleteUnusedSystemData_Click");
            }
        }
    }

    internal class DownloadData
    {
        public Uri DownloadUri { get; set; } 
        public String LocalFileName { get; set; } 
        public DateTime BelongingFileTime { get; set; } 
        public String InfoString { get; set; } 
        public Int64 TotalDownloadBytes { get; set; } 
        public String UsedUnit { get; set; } 


    }
}
