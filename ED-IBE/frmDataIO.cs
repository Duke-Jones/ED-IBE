using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;
using System.IO;
using IBE.SQL;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;

namespace IBE
{
    public partial class frmDataIO : RNBaseForm
    {
        public const String        DB_GROUPNAME                    = "DataIO";

        public delegate void DelTextParam(String text);

        public ListBox InfoTarget { get; set; }                 // allows to redirect the progress info to another listbox
        public Boolean ReUseLine { get; set; }                  // allows to redirect the progress info to another listbox
        private Boolean m_DataImportHappened = false; 
        
        private DBGuiInterface      m_GUIInterface;
        private ProgressView        m_ProgressView;
        private String              m_DownloadInfo;
        private Int32               m_lastFileSize;
        private String              m_lastFileSizeUnit;
        private Boolean             m_DownloadFinished;
        private String              m_DataPath          = Program.GetDataPath("data");

        private List<String>        m_DL_Files          = new List<string> {"systems.json", 
                                                                            "stations.json", 
                                                                            "commodities.json"};

        private List<String>        m_DL_FilesPrice     = new List<string> {"listings.csv"};

        private List<String>        m_DL_Files_EDCD     = new List<string> {"commodity.csv",
                                                                            "outfitting.csv",
                                                                            "shipyard.csv"};

        [Flags] enum enImportTypes
        {
            Undefiend                       = 0x0000,
            EDDB_Commodities                = 0x0001,           /* default is "commodities.json" */  
            RN_Localizations_Commodities    = 0x0002,           /* default is "newCommodityClassification.xml" */
            RN_Localizations_EcoLevels      = 0x0004,           /* default is "newCommodityClassification.xml" */
            RN_SelfAddedLocalizations       = 0x0008,           /* default is "Commodities_Own.xml" */
            RN_Pricewarnlevels              = 0x0010,           /* default is "Commodities_RN.json" */
            EDDB_Systems                    = 0x0020,           /* default is "systems.json" */  
            EDDB_Stations                   = 0x0040,           /* default is "stations.json" */  
            RN_Systems                      = 0x0080,           /* default is "systems_own.json" */  
            RN_Stations                     = 0x0100,           /* default is "stations_own.json" */  
            RN_CommandersLog                = 0x0200,           /* default is "CommandersLogAutoSave.xml" */  
            RN_StationHistory               = 0x0400,           /* default is "StationHistory.json" */  
            RN_MarketData                   = 0x0800,           /* default is "AutoSave.csv" */  
            IBE_Localizations_Commodities   = 0x1000,           /* default is "commodities.csv"  */ 
            EDDB_MarketData                 = 0x2000            /* default is "listings.csv"  */
        }

        public frmDataIO()
        {
            InitializeComponent();
            this.Load          += frmDataIO_Load;
            this.InfoTarget     = null;
            this.ReUseLine      = true;
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
                cErr.processError(ex, "Error in Load-Event");
            }
        }

        private void SetButtons(Boolean setEnabled)
        {
            try
            {
                cmdImportOldData.Enabled                        = (!Program.Data.OldDataImportDone) && setEnabled;
                cmdImportCommandersLog.Enabled                  = setEnabled;
                cmdDownloadSystemsAndStations.Enabled           = setEnabled;
                cmdImportSystemsAndStationsFromDownload.Enabled = setEnabled && EDDBDownloadComplete();
                cmdImportSystemsAndStations.Enabled             = setEnabled;
                cmdExportCSV.Enabled                            = setEnabled;
                rbDefaultLanguage.Enabled                       = setEnabled;
                rbUserLanguage.Enabled                          = setEnabled;
                cmdImportFromCSV.Enabled                        = setEnabled;
                rbImportNewer.Enabled                           = setEnabled;
                rbImportSame.Enabled                            = setEnabled;
                cmdTest.Enabled                                 = setEnabled;
                cmdPurgeOldData.Enabled                         = setEnabled;
                rbFormatExtended.Enabled                        = setEnabled;
                rbFormatSimple.Enabled                          = setEnabled;
                cmdExit.Enabled                                 = setEnabled;    
            }
            catch (Exception ex)
            {
                throw new Exception("Error while dis/enabling buttons", ex);
            }
        }

        /// <summary>
        /// start the master import (master data for systems/station/commoditynames ...)
        /// </summary>
        public void StartMasterImport(String path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelTextParam(StartMasterImport), new Object[] {path});
            }
            else
            {
                this.Visible = false;

                try
                {
                    enImportTypes importFlags = enImportTypes.EDDB_Commodities | 
                                                enImportTypes.EDDB_Systems | 
                                                enImportTypes.EDDB_Stations |
                                                enImportTypes.IBE_Localizations_Commodities |
                                                enImportTypes.RN_Localizations_EcoLevels;

                    ImportData(null, importFlags, null, path);
                }
                catch (Exception ex)
                {
                    cErr.processError(ex, "Error while starting master import");
                }
            }
        }

        /// <summary>
        /// start the master update (data for systems/stations/commoditynames ...)
        /// </summary>
        public void StartMasterUpdate(String path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelTextParam(StartMasterUpdate), new Object[] {path});
            }
            else
            {
                this.Visible = false;

                try
                {
                    enImportTypes importFlags = enImportTypes.EDDB_Commodities | 
                                                enImportTypes.EDDB_Systems | 
                                                enImportTypes.EDDB_Stations;

                    ImportData(null, importFlags, null, path);
                }
                catch (Exception ex)
                {
                    cErr.processError(ex, "Error while starting master import");
                }
            }
        }

        public void GetStartersKit(String path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelTextParam(StartMasterImport), new Object[] {path});
            }
            else
            {
                this.Visible = false;

                PriceImportParameters importParams = null;
                enImportTypes importFlags;                                  

                try
                {
                    SetButtons(false);
                    lbProgess.Items.Clear();

                    importFlags = enImportTypes.EDDB_MarketData;

                    importParams = new PriceImportParameters() { Radius = 20, SystemID = Program.actualCondition.System_ID.Value};
                
                    m_ProgressView = new ProgressView(this, false);
                    m_ProgressView.progressStart("importing starters kit...");

                    ImportData(null, importFlags, null, path, false, importParams);

                    SetButtons(true);

                    m_DataImportHappened = true;
                
                }
                catch (Exception ex)
                {
                    StopProgress();
                    SetButtons(true);
                    cErr.processError(ex, "Error while getting the starters kit");
                }
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
                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "Skipping <" + FileName + "> - file not found.", Index = 1, Total = 1});
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
        private void Data_Progress(object sender, SQL.EliteDBIO.ProgressEventArgs e)
        {
            try
            {
                e.progressObject = m_ProgressView;

                ListBox destination;

                if (InfoTarget != null)
                {
                    destination = InfoTarget;
                }
                else
                {
                    destination = lbProgess;
                }
                if (e.Index == 0 && e.Total == 0)
                {
                    destination.Items.Add("-------------------------------");
                    destination.Items.Add(String.Format("{0}", e.Tablename));

                    if(m_ProgressView != null)
                        m_ProgressView.progressUpdate(0,1, e.Tablename);

                }
                else if (e.Index == -1 && e.Total == -1)
                {
                    destination.Items.Add(String.Format("{0}", e.Tablename));

                    if(m_ProgressView != null)
                        m_ProgressView.progressUpdate(0,1, e.Tablename);

                }
                else
                {
                    if (e.Index == 1 && e.Total == 1)
                    {
                        destination.Items.Add(String.Format("{0} : 100%", e.Tablename));

                        if(m_ProgressView != null)
                            m_ProgressView.progressUpdate(1,1, e.Tablename);
                    }
                    else
                    {
                        if (ReUseLine && (destination.Items.Count > 0))
                        {
                            if(e.Total != 0)
                            {
                                destination.Items[destination.Items.Count - 1] = String.Format("{0} : {1}% ({2} of {3}{4})", e.Tablename, 100 * e.Index / e.Total, e.Index, e.Total, e.Unit);

                                if(m_ProgressView != null)
                                    m_ProgressView.progressUpdate(e.Index, e.Total, e.Tablename);
                            }
                            else
                                destination.Items[destination.Items.Count - 1] = String.Format("{0} : {2}{3}", e.Tablename, 0, e.Index, 0, e.Unit);
                        }
                        else
                        {
                            destination.Items.Add(String.Format("{0} : {1}% ({2} of {3}{4})", e.Tablename, 100 * e.Index / e.Total, e.Index, e.Total, e.Unit));

                            if(m_ProgressView != null)
                                m_ProgressView.progressUpdate(e.Index, e.Total, e.Tablename);

                        }
                    }
                }


                destination.TopIndex = destination.Items.Count - 1;

                destination.Refresh();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while reporting progress");
            }
        }

        /// <summary>
        /// main working routine for importing data
        /// </summary>
        /// <param name="importInfo">if this is set a FolderDialog will ask with this text for a import directory.
        /// If this is not set, the path from "General"->"Path_Import" or from "optionalPath" will be taken</param>
        /// <param name="importFlags">flags what to import</param>
        /// <param name="optionalFilter">filefilter (only for importing Commander's Log multiple times)</param>
        /// <param name="optionalPath">preset for the import path. Also taken by the FolderDialog if importInfo is set</param>
        /// <param name="RNData">causes to look for some files in the "/data/" subdirectory (for importing old RN data) </param>
        /// <param name="RNData">for control type of import EDDB price data (only for enImportTypes.EDDB_MarketData) </param>
        /// <returns></returns>
        private Boolean ImportData(string importInfo, enImportTypes importFlags, String optionalFilter = "", String optionalPath = "", Boolean RNData = false, PriceImportParameters importParams = null)
        {
            String FileName;
            String sourcePath;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<int,int>();
            Boolean retValue = false;
            Boolean restartEDDN = false;

            try
            {
                if (optionalPath == "")
                    sourcePath = Program.DBCon.getIniValue("General", "Path_Import", Program.GetDataPath("data"), false);
                else
                    sourcePath = optionalPath;

                if (!String.IsNullOrEmpty(importInfo))
                {
                    fbFolderDialog.RootFolder   = Environment.SpecialFolder.MyComputer;
                    fbFolderDialog.Description  = importInfo;
                    fbFolderDialog.SelectedPath = sourcePath;
                }

                if ((importInfo == null) || (fbFolderDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK))
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
                        Cursor = Cursors.WaitCursor;

                        lbProgess.Items.Clear();

                        Application.DoEvents();


                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.EDDB_Commodities))
                        {
                            // import the commodities from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodities...", Index = 0, Total = 0 });
                            FileName = "commodities.json";
                            
                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportCommoditiesFromFile(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcategory.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodities...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_Localizations_Commodities))
                        {
                            // import the localizations (commodities) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodity localizations...", Index = 0, Total = 0 });
                            FileName = "Commodities.xml";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportCommodityLocalizations(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodity localizations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.IBE_Localizations_Commodities))
                        {
                            // import the new localizations (commodities) from csv-fileName
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodity localizations...", Index = 0, Total = 0 });
                            FileName = "commodities.csv";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportLocalizationDataFromCSV(Path.Combine(sourcePath, FileName), SQL.EliteDBIO.enLocalizationType.Commodity, SQL.EliteDBIO.enLocalisationImportType.overwriteNonBase);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodity localizations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_Localizations_EcoLevels))
                        {
                            // import the localizations (economy levels) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import economy level localizations...", Index = 0, Total = 0 });
                            FileName = "Commodities.xml";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportEconomyLevelLocalizations(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tblevellocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbeconomylevel.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import economy level localizations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }


                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_SelfAddedLocalizations))
                        {
                            // import the self added localizations from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added commodity localizations...", Index = 0, Total = 0 });
                            FileName = "Commodities_Own.xml";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportCommodityLocalizations(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added commodity localizations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_Pricewarnlevels))
                        {
                            // import the pricewarnlevels from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import pricewarnlevels...", Index = 0, Total = 0 });
                            FileName = "Commodities_RN.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportCommodityPriceWarnLevels(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import pricewarnlevels...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.EDDB_Systems))
                        {
                            // import the systems and stations from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import systems...", Index = 0, Total = 0 });
                            FileName = "systems.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportSystems(Path.Combine(sourcePath, FileName));
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import systems...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.EDDB_Stations))
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import stations...", Index = 0, Total = 0 });
                            FileName = "stations.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportStations(Path.Combine(sourcePath, FileName), false);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import stations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_Systems))
                        {
                            // import the self-changed or added systems and stations
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added systems...", Index = 0, Total = 0 });
                            FileName = "systems_own.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                changedSystemIDs = Program.Data.ImportSystems_Own(Path.Combine(sourcePath, FileName));
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added systems...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_Stations))
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added stations...", Index = 0, Total = 0 });
                            FileName = "stations_own.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportStations_Own(Path.Combine(sourcePath, FileName), changedSystemIDs);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added stations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_CommandersLog))
                        {
                            // import the Commander's Log from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commander's log...", Index = 0, Total = 0 });
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
                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "importing " + importFile + "...", Index = 0, Total = 0 });

                                    Int32 added = Program.Data.ImportCommandersLog(Path.Combine(sourcePath, importFile));
                                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);

                                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "importing " + importFile + "... (" + added + " new entries)", Index = 1, Total = 1 });
                                }
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commander's log...", Index = 1, Total = 1 });

                                Program.Data.addMissingDistancesInLog(new DateTime(1970, 01, 01));

                                m_DataImportHappened = true;              
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + Filter, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_StationHistory))
                        {
                            //import the history of visited stations
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import visited stations...", Index = 0, Total = 0 });
                            FileName = "StationHistory.json";

                            if(RNData)
                                FileName = @"Data\" + FileName;

                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportVisitedStations(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedsystems.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedstations.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import visited stations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.RN_MarketData))
                        {
                            //import the self collected price data
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import collected price data...", Index = 0, Total = 0 });
                            FileName = "AutoSave.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportPricesFromCSVFile(Path.Combine(sourcePath, FileName), EliteDBIO.enImportBehaviour.OnlyNewer, EliteDBIO.enDataSource.fromRN);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedsystems.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedstations.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import collected price data...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if ((!ProgressCancelled()) && importFlags.HasFlag(enImportTypes.EDDB_MarketData))
                        {
                            //import the self collected price data
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import EDDN price data...", Index = 0, Total = 0 });
                            //import the price data from EDDB
                            FileName = "listings.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.visystemsandstations.TableName);
                                Program.Data.ImportPricesFromCSVFile(Path.Combine(sourcePath, FileName), EliteDBIO.enImportBehaviour.OnlyNewer, EliteDBIO.enDataSource.fromFILE, importParams);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedsystems.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedstations.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import EDDN price data...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        // update the visited information
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating visited systems and stations...", Index = 0, Total = 0 });
                        Program.Data.updateVisitedBaseFromLog(SQL.EliteDBIO.enVisitType.Systems | SQL.EliteDBIO.enVisitType.Stations);
                        Program.Data.updateVisitedFlagsFromBase();
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating visited systems and stations...", Index = 1, Total = 1 });

                        // insert missing localization entries
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "insert missing translation of commodities", Index = 0, Total = 0 });
                        Program.Data.AddMissingLocalizationEntries();
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "insert missing translation of commodities...", Index = 1, Total = 1 });

                        // update localization of all commodities
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating active localization of commodities", Index = 0, Total = 0 });
                        Program.Data.updateTranslation();
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating active localization of commodities...", Index = 1, Total = 1 });

                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "finished", Index = 1, Total = 1 });

                        Cursor = Cursors.Default;

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

                Cursor = Cursors.Default;
                throw new Exception("Error while importing data to database", ex);
            }
        }

        private void cmdDownloadSystemsAndStations_Click(object sender, EventArgs e)
        {
            try
            {
                String baseUrl         = "https://eddb.io/archive/v4/";
                String savePrefix      = "";
                String infoString      = "connecting to eddb.io...";
                List<String> filesList = new List<string>(m_DL_Files);
                filesList.AddRange(m_DL_FilesPrice);

                DownloadFiles(baseUrl, savePrefix, infoString, filesList);

            }
            catch (Exception ex)
            {
                StopProgress();
                SetButtons(true);
                cErr.processError(ex, "Error while downloading system/station data from EDDB");
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

        private void cmdImportSystemsAndStationsFromDownload_Click(object sender, EventArgs e)
        {
            PriceImportParameters importParams = null;
            Boolean cantImport = false;
                                  
            try
            {
                SetButtons(false);
                lbProgess.Items.Clear();

                enImportTypes importFlags = enImportTypes.EDDB_Commodities | 
                                            enImportTypes.EDDB_Systems | 
                                            enImportTypes.EDDB_Stations;

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
                            importParams = new PriceImportParameters() { Radius = 20, SystemID = Program.actualCondition.System_ID.Value};

                    }
                }

                if(!cantImport)
                { 
                    m_ProgressView = new ProgressView(this, false);
                    m_ProgressView.progressStart("importing EDDN data...");

                    ImportData(null, importFlags, null, m_DataPath, false, importParams);

                    if(StopProgress())
                        MessageBox.Show(this, "Import was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    m_DataImportHappened = true;
                }

                SetButtons(true);
                
            }
            catch (Exception ex)
            {
                StopProgress();
                SetButtons(true);
                cErr.processError(ex, "Error while importing downloaded system/station data from EDDB");
            }
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Int32 factor = 1024;
            m_lastFileSizeUnit = " kbyte";

            if(!m_DownloadFinished)
            { 
                if(m_ProgressView != null)
                {
                    if (e.TotalBytesToReceive < (10 * 1024))
                    {
                        factor = 1;
                        m_lastFileSizeUnit = " byte";
                    }

                    m_lastFileSize = (Int32)(e.TotalBytesToReceive / (factor));
                    m_ProgressView.progressUpdate((Int32)(e.BytesReceived / (factor)), m_lastFileSize);

                    if (m_ProgressView.Cancelled)
                        ((WebClient)sender).CancelAsync();
                }

                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = m_DownloadInfo, Index = (Int32)(e.BytesReceived / (factor)), Total = m_lastFileSize, Unit = m_lastFileSizeUnit });
                Debug.Print("1");
            }
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            m_DownloadFinished = true;
            
            if(!e.Cancelled)
            { 
                if(m_ProgressView != null)
                {
                    m_ProgressView.progressUpdate(1,1);
                }

                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = m_DownloadInfo, Index = m_lastFileSize, Total = m_lastFileSize, Unit = m_lastFileSizeUnit });
            }
            Debug.Print("2");
        }

        private void cmdImportSystemsAndStations_Click(object sender, EventArgs e)
        {
            PriceImportParameters importParams = null;

            try
            {
                SetButtons(false);
                lbProgess.Items.Clear();

                enImportTypes importFlags = enImportTypes.EDDB_Commodities | 
                                            enImportTypes.EDDB_Systems | 
                                            enImportTypes.EDDB_Stations;

                if(!rbImportPrices_No.Checked)
                {
                    importFlags |= enImportTypes.EDDB_MarketData;

                    if(rbImportPrices_Bubble.Checked)
                        importParams = new PriceImportParameters() { Radius = 20, SystemID = Program.actualCondition.System_ID.Value};
                }

                m_ProgressView = new ProgressView(this, false);
                m_ProgressView.progressStart("importing EDDN data...");

                ImportData("Select folder with system/station datafiles (systems.json/stations.json/commodities.json)", importFlags, "", "", false, importParams);

                if(StopProgress())
                    MessageBox.Show(this, "Import was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetButtons(true);

                m_DataImportHappened = true;
            }
            catch (Exception ex)
            {
                StopProgress();
                SetButtons(true);
                cErr.processError(ex, "Error while importing system/station data from EDDB");
            }
        }

        private void cmdImportCommandersLog_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtons(false);

                lbProgess.Items.Clear();

                enImportTypes importFlags = enImportTypes.RN_CommandersLog;
                ImportData("Select folder with your old Commander's Log data (accepted file pattern : CommandersLog*.xml)", importFlags, "CommandersLog*.xml");

                SetButtons(true);

                m_DataImportHappened = true;

            }
            catch (Exception ex)
            {
                SetButtons(true);
                cErr.processError(ex, "Error while importing system/station data from EDDN");
            }
        }

        /// <summary>
        /// imports the collected data from the old RN directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdImportOldData_Click(object sender, EventArgs e)
        {
            String RNPath;
            
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
                            lbProgess.Items.Clear();

                            Program.Data.Progress += Data_Progress;
                            Cursor = Cursors.WaitCursor;

                            Application.DoEvents();

                            ImportData(null, importFlags, null, RNPath, true);

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
                cErr.processError(ex, "Error while importing the existing RN-data");
            }
        }

        /// <summary>
        /// exports the market data to a csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtons(false);
                Cursor = Cursors.WaitCursor;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter              = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.DefaultExt          = "csv";
                saveFileDialog1.Title               = "Export market data";
                saveFileDialog1.OverwritePrompt     = true;
                saveFileDialog1.InitialDirectory    = Program.DBCon.getIniValue("General", "Path_Import", Program.GetDataPath("data"), false);

	            DialogResult result = saveFileDialog1.ShowDialog();

		        if (result == DialogResult.OK)
                {
                    lbProgess.Items.Clear();

                    m_ProgressView = new ProgressView(this, false);
                    m_ProgressView.progressStart("exporting data to csv...");

                    Program.Data.Progress += Data_Progress;

                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "export prices to csv...", Index = 0, Total = 0 });

                    Application.DoEvents();

                    Program.Data.ExportMarketDataToCSV(saveFileDialog1.FileName, rbUserLanguage.Checked, rbFormatExtended.Checked);

                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "export prices to csv...", Index = 1, Total = 1 });
                    Program.Data.Progress -= Data_Progress;

                    if(StopProgress())
                        MessageBox.Show(this, "Export was cancelled and is unfinished!", "Data export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Export has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SetButtons(true);
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                StopProgress();
                SetButtons(true);
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error while exporting to csv");
            }
        }

        /// <summary>
        /// imports the market data from a csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdImportFromCSV_Click(object sender, EventArgs e)
        {
            String sourcePath;
            OpenFileDialog fbFileDialog;
            try
            {
                Cursor = Cursors.WaitCursor;
                SetButtons(false);

                sourcePath = Program.DBCon.getIniValue("General", "Path_Import", Program.GetDataPath("data"), false);

                fbFileDialog = new OpenFileDialog();
                fbFileDialog.InitialDirectory = sourcePath;
                fbFileDialog.Title = "Select the csv-file with market data to import...";
                fbFileDialog.CheckFileExists    = true;
                fbFileDialog.Filter             = "CVS files (*.csv)|*.csv|Text documents (*.txt)|*.txt|All files (*.*)|*.*";
                fbFileDialog.FilterIndex = 3;


                fbFileDialog.ShowDialog(this);

                if ((!String.IsNullOrEmpty(fbFileDialog.FileName)) && System.IO.File.Exists(fbFileDialog.FileName))
                {
                    lbProgess.Items.Clear();

                    m_ProgressView = new ProgressView(this, false);
                    m_ProgressView.progressStart("importing data from csv...");

                    Program.Data.Progress += Data_Progress;

                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import price data from csv...", Index = 0, Total = 0 });

                    SQL.EliteDBIO.enImportBehaviour importBehaviour = SQL.EliteDBIO.enImportBehaviour.OnlyNewer;
                    if(rbImportSame.Checked)
                        importBehaviour = SQL.EliteDBIO.enImportBehaviour.NewerOrEqual;

                    Program.Data.ImportPricesFromCSVFile(Path.Combine(sourcePath, fbFileDialog.FileName), importBehaviour, EliteDBIO.enDataSource.fromFILE);
                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedsystems.TableName);
                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedstations.TableName);
                    Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import price data from csv...", Index = 1, Total = 1 });

                    Program.Data.Progress -= Data_Progress;

                    if(StopProgress())
                        MessageBox.Show(this, "Import was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show(this, "Import has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SetButtons(true);

                Cursor = Cursors.Default;

                m_DataImportHappened = true;
            }
            catch (Exception ex)
            {
                StopProgress();
                SetButtons(true);
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error while importing from csv");
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
                cErr.processError(ex);
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
		        cErr.processError(ex, "Error in frmDataIO_FormClosed");
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
                cErr.processError(ex, "Error in cmdExit_Click");   
            }
        }

        private void cmdPurgeOldData_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show(String.Format("Delete all data older than {0} days", nudPurgeOldDataDays.Value), "Delete old price data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.OK)
                {
                    lbProgess.Items.Clear();

                    SetButtons(false);

                    DateTime deadline = DateTime.Now.AddDays(-1*(Int32)(nudPurgeOldDataDays.Value)).Date;

                    Program.Data.Progress += Data_Progress;

                    Program.Data.DeleteMarketData((Int32)nudPurgeOldDataDays.Value);

                    Program.Data.Progress -= Data_Progress;

                    SetButtons(true);
                }
            }
            catch (Exception ex)
            {
                SetButtons(true);
                cErr.processError(ex, "Error in cmdPurgeOldData_Click");
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

        private void frmDataIO_Shown(object sender, EventArgs e)
        {
            try
            {
                SetButtons(true);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in Shown event");
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
                cErr.processError(ex, "Error in Radiobutton_CheckedChanged");
            }
        }

        /// <summary>
        /// quits the progressview an returns true if the progress was cancelled
        /// </summary>
        /// <returns></returns>
        private Boolean StopProgress()
        {
            Boolean retValue = ProgressCancelled();
                                         
            if (m_ProgressView != null)
            { 
                try
                {
                    m_ProgressView.progressStop();
                }
                catch (Exception)
                {
                }
            }
            m_ProgressView = null;

            return retValue;
        }

        private Boolean ProgressCancelled()
        {
            Boolean retValue = false;

            if ((m_ProgressView != null) && m_ProgressView.Cancelled)
                retValue = true;

            return retValue;
        }

        /// <summary>
        /// starts download of the files
        /// </summary>
        /// <param name="baseUrl">download url of the files</param>
        /// <param name="savePrefix">name prefix for download files, takes original name if empty</param>
        /// <param name="infoString">infostring for messages</param>
        /// <param name="filesList">list of files to download to</param>
        private void DownloadFiles(String baseUrl, String savePrefix, String infoString, List<String> filesList)
        {
            List<DateTime> filesTimes = new List<DateTime>();

            Boolean download;
            String currentDestinationFile;

            try
            {
                SetButtons(false);
                lbProgess.Items.Clear();

                m_ProgressView = new ProgressView(this, false);
                m_ProgressView.progressStart(infoString);

                // get the timestamps from the webfiles
                for (int i = 0; i < filesList.Count; i++)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + filesList[i]);
                    request.Method = "HEAD";
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        filesTimes.Add(response.LastModified);
                }

                WebClient downLoader = new WebClient();
                downLoader.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
                downLoader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);

                // download file if newer
                for (int i = 0; i < filesList.Count; i++)
                {
                    download = false;

                    currentDestinationFile = Path.Combine(m_DataPath, savePrefix + filesList[i]);

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

                    if (download)
                    {
                        m_ProgressView.progressUpdate(0, 100);
                        m_DownloadInfo = String.Format("downloading file {0} of {1}: {2}...", i + 1, filesList.Count, filesList[i]);
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = m_DownloadInfo, Index = 0, Total = 0 });

                        m_ProgressView.progressInfo(m_DownloadInfo);

                        m_DownloadFinished = false;
                        Debug.Print("in");
                        downLoader.DownloadFileAsync(new Uri(baseUrl + filesList[i]), currentDestinationFile);

                        do
                        {
                            System.Threading.Thread.Sleep(100);
                            Application.DoEvents();
                        } while (!m_DownloadFinished);

                        Debug.Print("out");
                        if (!m_ProgressView.Cancelled)
                        {
                            File.SetCreationTime(currentDestinationFile, filesTimes[i]);
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = m_DownloadInfo, Index = 1, Total = 1 });
                        }
                        else
                        {
                            if (File.Exists(currentDestinationFile))
                                File.Delete(currentDestinationFile);
                            lbProgess.Items.Add("");
                            lbProgess.Items.Add("download cancelled !!! ");
                        }
                    }
                    else
                    {
                        m_DownloadInfo = String.Format("skipping download, newest version already existing: {0} of {1}: {2}...", i + 1, filesList.Count, filesList[i]);
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = m_DownloadInfo, Index = 0, Total = 0 });

                        m_ProgressView.progressInfo(m_DownloadInfo);
                        m_ProgressView.progressUpdate(1, 1);

                        System.Threading.Thread.Sleep(500);
                    }

                    if (m_ProgressView.Cancelled)
                        break;
                }

                if (StopProgress())
                    MessageBox.Show(this, "Download was cancelled and is unfinished!", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(this, "Download has finished", "Data import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetButtons(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmdEDCDDownloadID_Click(object sender, EventArgs e)
        {
            try
            { 
                String baseUrl         = "https://raw.githubusercontent.com/EDCD/FDevIDs/master/";
                String savePrefix      = "EDCD_";
                String infoString      = "connecting to github.com...";
                List<String> filesList = new List<string>(m_DL_Files_EDCD);

                DownloadFiles(baseUrl, savePrefix, infoString, filesList);

            }
            catch (Exception ex)
            {
                StopProgress();
                SetButtons(true);
                cErr.processError(ex, "Error while downloading files with EDCD-IDs");
            }
        }
    }
}
