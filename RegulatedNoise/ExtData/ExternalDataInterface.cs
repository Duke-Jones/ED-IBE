using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.ExtData
{
    public class ExternalDataInterface
    {
        
        #region LogEvents

        [Flags] public enum enExternalDataEvents
        {
            None         = 0,
            System          = 1,
            Location        = 2,
            Jump            = 4,
            Landed          = 8,
            DataCollected   = 16
        }

        public enum enExternalDataFunction
        {
            getLocation     = 0,
            getMarketdata   = 1,
        }

        public class ExternalDataEventData
        {
            public enExternalDataEvents EventType    { get; set; }
            public String               Value        { get; set; }
            public DateTime             Time         { get; set; }
        }



        #endregion

        #region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<LocationChangedEventArgs> ExternalDataEvent;

        protected virtual void OnLocationChanged(LocationChangedEventArgs e)
        {
            EventHandler<LocationChangedEventArgs> myEvent = ExternalDataEvent;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class LocationChangedEventArgs : EventArgs
        {
            public LocationChangedEventArgs()
            {
                Changed     = enExternalDataEvents.None;
                System      = "";
                Location    = "";
                
            }

            public String System                    { get; set; }
            public String Location                  { get; set; }
            public String OldSystem                 { get; set; }
            public String OldLocation               { get; set; }
            public enExternalDataEvents Changed     { get; set; }
            public Int32 Amount                     { get; set; }
        }

        #endregion

        String m_OutputDestination  = @"C:\temp\location.txt";
        List<String> m_OutPut       = new List<String>();
        String m_RetrievedSystem    = "";
        String m_RetrievedStation   = "";
        
        /// <summary>
        /// gets the current location and saves market data to a file
        /// </summary>
        /// <param name="System"></param>
        /// <param name="Station"></param>
        /// <param name="ErrorInfo"></param>
        /// <returns></returns>
        public Boolean getLocation(out String System, out String Station, out String Info, out String ErrorInfo)
        {
            try
            {
                return getData(enExternalDataFunction.getLocation, out System, out Station, out Info, out ErrorInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting location trough external data tool ", ex);
            }
        }

        /// <summary>
        /// gets the current location and saves market data to a file
        /// </summary>
        /// <param name="DataFunction"></param>
        /// <param name="System"></param>
        /// <param name="Station"></param>
        /// <param name="ErrorInfo"></param>
        /// <returns></returns>
        private Boolean getData(enExternalDataFunction DataFunction, out String System, out String Station, out String Info, out String ErrorInfo)
        {
            Process ProcessObject;
            Boolean retValue = false;

            try
            {
                m_OutPut.Clear();
                DeleteOldMarketdata();

                System      = "";
                Station     = "";
                Info        = "";
                ErrorInfo   = "";

                ProcessObject = new Process();

                ProcessObject.StartInfo.FileName                = Program.DBCon.getIniValue(MTSettings.tabSettings.DB_GROUPNAME, "ExtTool_Path").Trim();

                if(!String.IsNullOrEmpty(ProcessObject.StartInfo.FileName))
                { 
                    ProcessObject.StartInfo.WorkingDirectory        = Path.GetDirectoryName(ProcessObject.StartInfo.FileName);
                    ProcessObject.StartInfo.UseShellExecute         = false;
        	        ProcessObject.StartInfo.RedirectStandardOutput  = true;
        	        ProcessObject.StartInfo.RedirectStandardInput   = true;
                    ProcessObject.StartInfo.RedirectStandardError   = true;
                    ProcessObject.StartInfo.CreateNoWindow          = true;

                    if(DataFunction == enExternalDataFunction.getLocation)
                        ProcessObject.StartInfo.Arguments           = Program.DBCon.getIniValue(MTSettings.tabSettings.DB_GROUPNAME, 
                                                                                                "ExtTool_ParamLocation").Trim();
                    else
                        ProcessObject.StartInfo.Arguments           = Program.DBCon.getIniValue(MTSettings.tabSettings.DB_GROUPNAME, 
                                                                                                "txtExtTool_ParamMarket").Trim();

                    ProcessObject.StartInfo.Arguments               = ProcessObject.StartInfo.Arguments.Replace(
                                                                            "\\%OUTPUTFILE\\%", 
                                                                            Path.Combine(
                                                                                    Program.Paths.DataPath_temp, 
                                                                                    String.Format("marketdata_utc{0:yyyyMMdd_HHmmss}.csv", DateTime.UtcNow)));
                    
                    
                    //// Set our event handler to asynchronously read the output.
                    ProcessObject.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

                    ProcessObject.Start();

                    ProcessObject.BeginOutputReadLine(); 

                    ErrorInfo = ProcessObject.StandardError.ReadToEnd();

                    ProcessObject.WaitForExit();

                    ProcessObject.OutputDataReceived -= new DataReceivedEventHandler(SortOutputHandler);

                    ProcessObject.Close();
                    ProcessObject.Dispose();

                    if(m_OutPut.Count() > 0)
                    { 
                        // no error and we've got something
                        String [] Parts = m_OutPut[0].Split(new char[] {','});

                        if(Parts.Count() >= 2)
                        { 
                            System              = Parts[0].Trim();
                            Station             = Parts[1].Trim();

                            m_RetrievedSystem   = Parts[0].Trim();
                            m_RetrievedStation  = Parts[1].Trim();

                            Info = m_OutPut[0];

                            retValue = true;
                        }
                        else
                        {
                            Info = "No station information.";
                        }
                    }
                }

                return retValue;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving the current location", ex);
            }        
        }

        private void SortOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                m_OutPut.Add(outLine.Data);
            }
        }

        /// <summary>
        /// confirms the last retrieved location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Confirm()
        {
            DialogResult MBResult           = System.Windows.Forms.DialogResult.OK;
            String OldSystemString          = "";
            String OldLocationString        = "";
            enExternalDataEvents ChangedIs  = enExternalDataEvents.None;

            try
            {
                if(!Program.actualCondition.System.Equals(m_RetrievedSystem))
                {
                    MBResult = MessageBox.Show("The retrieved system do not correspond to the system from the logfile!\n" +
                                               "Confirm even so ?", "Unexpected system", 
                                               MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                }

                if(MBResult == System.Windows.Forms.DialogResult.OK)
                {
                    if(!Program.actualCondition.System.Equals(m_RetrievedSystem) ||
                       !Program.actualCondition.Location.Equals(m_RetrievedStation))
                    {
                        OldSystemString     = Program.actualCondition.System;
                        OldLocationString   = Program.actualCondition.Location;

                        ChangedIs = enExternalDataEvents.Landed;

                        if(!Program.actualCondition.System.Equals(m_RetrievedSystem))
                            ChangedIs |= enExternalDataEvents.System;

                        if(!Program.actualCondition.Location.Equals(m_RetrievedStation))
                            ChangedIs |= enExternalDataEvents.Location;
                    }

                    Program.actualCondition.System   = m_RetrievedSystem;
                    Program.actualCondition.Location = m_RetrievedStation;
                    
                    if(ChangedIs != enExternalDataEvents.None)
                    { 
                        // something has changed -> fire event
                        var EA = new LocationChangedEventArgs() { System        = Program.actualCondition.System,  
                                                                  Location      = Program.actualCondition.Location,
                                                                  OldSystem     = OldSystemString,  
                                                                  OldLocation   = OldLocationString,
                                                                  Changed       = ChangedIs};
                        ExternalDataEvent.Raise(this, EA);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while confirming retrieved location", ex);
            }
        }

        /// <summary>
        /// collects the marketdata from a external tool
        /// </summary>
        public Boolean getMarketData(out String System, out String Station, out String Info, out String ErrorInfo, out Int32 DataCount)
        {
            Boolean DataOk = false;
            String Datafile;
            Boolean retValue = false;

            try
            {
                System      = "";
                Station     = "";
                Info        = "";
                ErrorInfo   = "";
                DataCount   = 0;
                DataOk      = CheckDataFile(out Datafile);

                if(!DataOk)
                    if(getData(enExternalDataFunction.getMarketdata, out System, out Station, out Info, out ErrorInfo))
                        DataOk = CheckDataFile(out Datafile);

                if(DataOk)
                {
                    DataCount = Program.Data.ImportPricesFromCSVFile(Datafile);

                    // something has changed -> fire event
                    var EA = new LocationChangedEventArgs() { System        = Program.actualCondition.System,  
                                                              Location      = Program.actualCondition.Location,
                                                              OldSystem     = Program.actualCondition.System,  
                                                              OldLocation   = Program.actualCondition.Location,
                                                              Changed       = enExternalDataEvents.DataCollected,
                                                              Amount        = DataCount};
                    ExternalDataEvent.Raise(this, EA);

                    retValue = true;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while collecting market data from external tool", ex);
            }
        }

        private Boolean CheckDataFile(out String DataFile)
        {
            List<String> Files;
            DateTime TimeStamp;
            TimeSpan Age;
            Boolean DataOk = false;
            String Filename;

            DataFile = "";

            Files = System.IO.Directory.GetFiles(
                                Program.Paths.DataPath_temp, "marketdata_utc*.csv",
                                SearchOption.TopDirectoryOnly)
                                    .OrderByDescending(x => x).ToList();

            if (Files.Count >= 1)
            {
                TimeStamp = DateTime.MinValue;
                Filename = Path.GetFileName(Files[0]);

                if (DateTime.TryParseExact(Filename.Substring(14, 15), "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out TimeStamp))
                {
                    Age = DateTime.UtcNow.Subtract(TimeStamp);

                    if (Age.TotalSeconds <= 10)
                    { 
                        DataOk = true;
                        DataFile = Files[0];
                    }
                }
            }

            return DataOk;
        }

        private void DeleteOldMarketdata()
        {

            String[] Files;

            Files = System.IO.Directory.GetFiles(
                    Program.Paths.DataPath_temp, "marketdata*.csv",
                    SearchOption.TopDirectoryOnly);

            foreach (String FileInDir in Files)
                File.Delete(FileInDir);

        }
    }
}
