using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RegulatedNoise.Enums_and_Utility_Classes;
using System.IO;

namespace RegulatedNoise
{
    public partial class frmDataIO : RegulatedNoise.Enums_and_Utility_Classes.RNBaseForm
    {
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
        /// imports the whole data from the old RN version
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdImportOldData_Click(object sender, EventArgs e)
        {
            String FileName;
            String RNPath;

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
                                Program.Data.ImportCommodities(Path.Combine(RNPath, FileName));
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
                            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<int,int>();
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
                                Program.Data.ImportPricesFromAutoSave(Path.Combine(RNPath, FileName));
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
        /// <param name="RNPath"></param>
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
                if(e.Index == 0 && e.Total == 0)
                {
                    lbProgess.Items.Add("-------------------------------");
                    lbProgess.Items.Add(String.Format("{0}", e.Tablename));
                }
                else if(e.Index == 1 && e.Total == 1)
                {
                    lbProgess.Items.Add(String.Format("{0} : 100%", e.Tablename));
                }
                else
                { 
                    lbProgess.Items.Add(String.Format("{0} : {1}% ({2} of {3})", e.Tablename, 100 * e.Index/e.Total, e.Index, e.Total));
                }
                
                lbProgess.SelectedIndex = lbProgess.Items.Count-1;
                lbProgess.Refresh();
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

        private void cmdImportSystemsAndStations_Click(object sender, EventArgs e)
        {

        }

    }
}
