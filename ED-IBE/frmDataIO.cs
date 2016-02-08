using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;
using System.IO;

namespace IBE
{
    public partial class frmDataIO : IBE.Enums_and_Utility_Classes.RNBaseForm
    {
        public delegate void DelTextParam(String text);

        public ListBox InfoTarget { get; set; }                 // allows to redirect the progress info to another listbox

        [Flags] enum enImportTypes
        {
            Undefiend                       = 0x0000,
            EDDB_Commodities                = 0x0001,     /* default is "commodities.json" */  
            RN_Localizations_Commodities    = 0x0002,     /* default is "newCommodityClassification.xml" */
            RN_Localizations_EcoLevels      = 0x0004,     /* default is "newCommodityClassification.xml" */
            RN_SelfAddedLocalizations       = 0x0008,     /* default is "Commodities_Own.xml" */
            RN_Pricewarnlevels              = 0x0010,     /* default is "Commodities_RN.json" */
            EDDB_Systems                    = 0x0020,     /* default is "systems.json" */  
            EDDB_Stations                   = 0x0040,     /* default is "stations.json" */  
            RN_Systems                      = 0x0080,     /* default is "systems_own.json" */  
            RN_Stations                     = 0x0100,     /* default is "stations_own.json" */  
            RN_CommandersLog                = 0x0200,     /* default is "CommandersLogAutoSave.xml" */  
            RN_StationHistory               = 0x0400,     /* default is "StationHistory.json" */  
            RN_MarketData                   = 0x0800,     /* default is "AutoSave.csv" */  
        }

        public frmDataIO()
        {
            InitializeComponent();
            this.Load += frmDataIO_Load;
        }

        void frmDataIO_Load(object sender, EventArgs e)
        {
            try
            {
                cmdImportOldData.Enabled    = !Program.Data.OldDataImportDone;
                cbImportPriceData.Enabled   = !Program.Data.OldDataImportDone;
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in Load-Event");
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
                    enImportTypes importFlags = enImportTypes.EDDB_Commodities | enImportTypes.EDDB_Systems | enImportTypes.EDDB_Stations;
                    ImportData(null, importFlags, "", path);
                }
                catch (Exception ex)
                {
                    cErr.processError(ex, "Error while starting master import");
                }
            }
        }

        /// <summary>
        /// imports the whole data from the old RN directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdImportOldData_Click(object sender, EventArgs e)
        {
            String FileName;
            String RNPath;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<int,int>();

            try
            {
                fbFolderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                fbFolderDialog.Description = "Select your RN-Folder with the old data files ....";
                fbFolderDialog.SelectedPath = System.IO.Directory.GetCurrentDirectory();

                if(System.Diagnostics.Debugger.IsAttached)
        #if ep_Debug
                    fbFolderDialog.SelectedPath = @"I:\RN\RegulatedNoise_MySQL\RegulatedNoise\bin\Debug DJ ep";
        #else
                    fbFolderDialog.SelectedPath = @"F:\Games\ED\sonstiges\RegulatedNoise.v1.81";                    
        #endif 
                if(fbFolderDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    RNPath = fbFolderDialog.SelectedPath.Trim();

                    if (!String.IsNullOrEmpty(RNPath))
                    {

                        if(File.Exists(Path.Combine(RNPath, "RegulatedNoise.exe")))
                        { 
                            Program.Data.Progress += Data_Progress;
                            Cursor = Cursors.WaitCursor;

                            lbProgess.Items.Clear();

                            Application.DoEvents();

                            // import the commodities from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodities...", Index = 0, Total = 0});
                            FileName = @"Data\commodities.json";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportCommoditiesFromFile(Path.Combine(RNPath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcategory.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodities...", Index = 1, Total = 1});
                            }

                            // import the localizations (commodities) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodity localizations...", Index = 0, Total = 0});
                            FileName = @"Data\Commodities.xml";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportCommodityLocalizations(Path.Combine(RNPath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodity localizations...", Index = 1, Total = 1});
                            }

                            // import the localizations (economy levels) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import economy level localizations...", Index = 0, Total = 0});
                            FileName = @"Data\Commodities.xml";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportEconomyLevelLocalizations(Path.Combine(RNPath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tblevellocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbeconomylevel.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import economy level localizations...", Index = 1, Total = 1});
                            }
                            

                            // import the self added localizations from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added commodity localizations...", Index = 0, Total = 0});
                            FileName = @"Data\Commodities_Own.xml";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportCommodityLocalizations(Path.Combine(RNPath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added commodity localizations...", Index = 1, Total = 1});
                            }

                            // import the pricewarnlevels from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import pricewarnlevels...", Index = 0, Total = 0});
                            FileName = @"Data\Commodities_RN.json";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportCommodityPriceWarnLevels(Path.Combine(RNPath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import pricewarnlevels...", Index = 1, Total = 1});
                            }

                            // import the systems and stations from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import systems...", Index = 0, Total = 0});
                            FileName = @"Data\systems.json";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportSystems(Path.Combine(RNPath, FileName));
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import systems...", Index = 1, Total = 1});
                            }

                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import stations...", Index = 0, Total = 0});
                            FileName = @"Data\stations.json";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportStations(Path.Combine(RNPath, FileName), cbImportPriceData.Checked);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import stations...", Index = 1, Total = 1});
                            }

                            // import the self-changed or added systems and stations 
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added systems...", Index = 0, Total = 0});
                            FileName = @"Data\systems_own.json";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                changedSystemIDs = Program.Data.ImportSystems_Own(Path.Combine(RNPath, FileName));
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added systems...", Index = 1, Total = 1});
                            }

                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added stations...", Index = 0, Total = 0});
                            FileName = @"Data\stations_own.json";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportStations_Own(Path.Combine(RNPath, FileName), changedSystemIDs);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added stations...", Index = 1, Total = 1});
                            }

                            // import the Commander's Log from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commander's log...", Index = 0, Total = 0});
                            FileName = @"CommandersLogAutoSave.xml";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportCommandersLog(Path.Combine(RNPath, FileName));
                                Program.Data.addMissingDistancesInLog(new DateTime(1970, 01, 01));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commander's log...", Index = 1, Total = 1});
                            }

                            //import the history of visited stations
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import visited stations...", Index = 0, Total = 0});
                            FileName = @"Data\StationHistory.json";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportVisitedStations(Path.Combine(RNPath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedsystems.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedstations.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import visited stations...", Index = 1, Total = 1});
                            }

                            //import the self collected price data
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import collected price data...", Index = 0, Total = 0});
                            FileName = @"AutoSave.csv";
                            if(FileExistsOrMessage(RNPath, FileName))
                            { 
                                Program.Data.ImportPricesFromCSVFile(Path.Combine(RNPath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedsystems.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedstations.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import collected price data...", Index = 1, Total = 1});
                            }

                            // update the visited information
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating visited systems and stations...", Index = 0, Total = 0});
                            Program.Data.updateVisitedBaseFromLog(SQL.EliteDBIO.enVisitType.Systems | SQL.EliteDBIO.enVisitType.Stations);
                            Program.Data.updateVisitedFlagsFromBase();
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating visited systems and stations...", Index = 1, Total = 1});

                            // update localization of all commodities
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating translation of commodities", Index = 0, Total = 0});
                            Program.Data.updateTranslation();
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating translation of commodities...", Index = 1, Total = 1});


                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "finished", Index = 1, Total = 1});

                            Cursor = Cursors.Default;

                            Program.Data.Progress -= Data_Progress;

                            // set a flag : full import of old data is done
                            Program.Data.OldDataImportDone  = true;
                            cmdImportOldData.Enabled        = false;
                            cbImportPriceData.Enabled       = false;
                        }
                        else
                        {
                            MessageBox.Show("<RegulatedNoise.exe> not found. Wrong directory ?", "Data import",  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.showError(ex,"Error while importing the whole old data to database");
            }
        }

        /// <summary>
        /// checks if a file ist existingClassification, If not it shows a info-messsage
        /// </summary
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



        void Data_Progress(object sender, SQL.EliteDBIO.ProgressEventArgs e)
        {
            try
            {
                ListBox destination;

                if (InfoTarget != null)
                    destination = InfoTarget;
                else
                    destination = lbProgess;

                if(e.Index == 0 && e.Total == 0)
                {
                    destination.Items.Add("-------------------------------");
                    destination.Items.Add(String.Format("{0}", e.Tablename));
                }
                else if(e.Index == 1 && e.Total == 1)
                {
                    destination.Items.Add(String.Format("{0} : 100%", e.Tablename));
                }
                else
                { 
                    destination.Items.Add(String.Format("{0} : {1}% ({2} of {3})", e.Tablename, 100 * e.Index/e.Total, e.Index, e.Total));
                }
                
                destination.SelectedIndex = destination.Items.Count-1;
                destination.Refresh();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error while reporting progress");
            }
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Error in Test", ex);
            }
        }

        private void cmdClearAll_Click(object sender, EventArgs e)
        {
            String retString = "";

            try
            {

                if(InputBox.Show("Clear database", "This will delete all your data. Are you sure ? Type 'yes' to proceed.",ref retString) == System.Windows.Forms.DialogResult.OK)
                    if(retString == "yes")
                    {
                        Program.Data.Progress += Data_Progress;
                        Cursor = Cursors.WaitCursor;
                        lbProgess.Items.Clear();
                        Application.DoEvents();

                        Program.Data.ClearAll();

                        Program.DBCon.setIniValue("Database", "Version", new Version(0,0,0,0).ToString());
                        Program.Data.OldDataImportDone  = false;

                        cmdImportOldData.Enabled        = true;
                        cbImportPriceData.Enabled       = true;

                        System.Threading.Thread.Sleep(50);
                        Cursor = Cursors.Default;

                        Program.Data.Progress -= Data_Progress;
                        
                    }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;

                cErr.showError(ex, "Error in cmdClearAll_Click");
            }
        }

        private void ImportData(string importInfo, enImportTypes importFlags, String optionalFilter = "", String optionalPath = "")
        {
            String FileName;
            String sourcePath;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<int,int>();

            try
            {
                if (optionalPath == "")
                    sourcePath = Program.DBCon.getIniValue("General", "Path_Import", Program.GetDataPath("data"), false);
                else
                    sourcePath = optionalPath;

                if (importInfo != null)
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

                        if (importInfo != null)
                            Program.DBCon.setIniValue("General", "Path_Import", sourcePath);

                        Program.Data.Progress += Data_Progress;
                        Cursor = Cursors.WaitCursor;

                        lbProgess.Items.Clear();

                        Application.DoEvents();


                        if (importFlags.HasFlag(enImportTypes.EDDB_Commodities))
                        {
                            // import the commodities from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodities...", Index = 0, Total = 0 });
                            FileName = "commodities.json";
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

                        if (importFlags.HasFlag(enImportTypes.RN_Localizations_Commodities))
                        {
                            // import the localizations (commodities) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commodity localizations...", Index = 0, Total = 0 });
                            FileName = "Commodities.xml";
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

                        if (importFlags.HasFlag(enImportTypes.RN_Localizations_EcoLevels))
                        {
                            // import the localizations (economy levels) from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import economy level localizations...", Index = 0, Total = 0 });
                            FileName = "Commodities.xml";
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


                        if (importFlags.HasFlag(enImportTypes.RN_SelfAddedLocalizations))
                        {
                            // import the self added localizations from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added commodity localizations...", Index = 0, Total = 0 });
                            FileName = "Commodities_Own.xml";
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

                        if (importFlags.HasFlag(enImportTypes.RN_Pricewarnlevels))
                        {
                            // import the pricewarnlevels from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import pricewarnlevels...", Index = 0, Total = 0 });
                            FileName = "Commodities_RN.json";
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

                        if (importFlags.HasFlag(enImportTypes.EDDB_Systems))
                        {
                            // import the systems and stations from EDDB
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import systems...", Index = 0, Total = 0 });
                            FileName = "systems.json";
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

                        if (importFlags.HasFlag(enImportTypes.EDDB_Stations))
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import stations...", Index = 0, Total = 0 });
                            FileName = "stations.json";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportStations(Path.Combine(sourcePath, FileName), cbImportPriceData.Checked);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                                //Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations_org.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import stations...", Index = 1, Total = 1 });
                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + FileName, Index = 1, Total = 1 });
                            }
                        }

                        if (importFlags.HasFlag(enImportTypes.RN_Systems))
                        {
                            // import the self-changed or added systems and stations
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added systems...", Index = 0, Total = 0 });
                            FileName = "systems_own.json";
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

                        if (importFlags.HasFlag(enImportTypes.RN_Stations))
                        {
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import self-added stations...", Index = 0, Total = 0 });
                            FileName = "stations_own.json";
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

                        if (importFlags.HasFlag(enImportTypes.RN_CommandersLog))
                        {
                            // import the Commander's Log from the old RN files
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import commander's log...", Index = 0, Total = 0 });
                            string[] files;
                            string Filter;

                            if (optionalFilter != "")
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

                            }
                            else
                            {
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "File not found: " + Filter, Index = 1, Total = 1 });
                            }
                        }

                        if (importFlags.HasFlag(enImportTypes.RN_StationHistory))
                        {
                            //import the history of visited stations
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import visited stations...", Index = 0, Total = 0 });
                            FileName = "StationHistory.json";
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

                        if (importFlags.HasFlag(enImportTypes.RN_MarketData))
                        {
                            //import the self collected price data
                            Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import collected price data...", Index = 0, Total = 0 });
                            FileName = "AutoSave.csv";
                            if (FileExistsOrMessage(sourcePath, FileName))
                            {
                                Program.Data.ImportPricesFromCSVFile(Path.Combine(sourcePath, FileName));
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedsystems.TableName);
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbvisitedstations.TableName);
                                Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "import collected price data...", Index = 1, Total = 1 });
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

                        // update localization of all commodities
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating translation of commodities", Index = 0, Total = 0 });
                        Program.Data.updateTranslation();
                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "updating translation of commodities...", Index = 1, Total = 1 });

                        Data_Progress(this, new SQL.EliteDBIO.ProgressEventArgs() { Tablename = "finished", Index = 1, Total = 1 });

                        Cursor = Cursors.Default;

                        Program.Data.Progress -= Data_Progress;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                throw new Exception("Error while importing data to database", ex);
            }
        }

        private void cmdImportSystemsAndStations_Click(object sender, EventArgs e)
        {
            try
            {

                enImportTypes importFlags = enImportTypes.EDDB_Commodities | enImportTypes.EDDB_Systems | enImportTypes.EDDB_Stations;
                ImportData("Select folder with system/station datafiles (systems.json/stations.json/commodities.json)", importFlags);

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while importing system/station data from EDDN");
            }
        }

        private void cmdImportCommandersLog_Click(object sender, EventArgs e)
        {
            try
            {
                enImportTypes importFlags = enImportTypes.RN_CommandersLog;
                ImportData("Select folder with system/station datafiles (systems.json/stations.json/commodities.json)", importFlags, "CommandersLog*.xml");
            }            catch (Exception ex)
            {
                cErr.processError(ex, "Error while importing system/station data from EDDN");
            }
        }
    }
}
