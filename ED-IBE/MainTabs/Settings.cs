using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using IBE.Enums_and_Utility_Classes;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using IBE.SQL;
using IBE.SQL.Datasets;
using System.Collections.Generic;
using CodeProject.Dialog;

namespace IBE.MTSettings
{
    public class Settings
    {
        private const string tbn_BestMarketPrices = "tbBestMarketPrices";

        public enum enGUIEditElements
        {
            cbLogEventType,
            cbLogSystemName,
            cbLogStationName,
            cbLogCargoName,
            cbCargoAction
        }

#region event handler

        public event EventHandler<DataChangedEventArgs> DataChanged;

        protected virtual void OnDataChanged(DataChangedEventArgs e)
        {
            EventHandler<DataChangedEventArgs> myEvent = DataChanged;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class DataChangedEventArgs : EventArgs
        {
            public Int32    DataRow { get; set; }
            public DateTime DataKey { get; set; }
        }

#endregion

        private dsEliteDB           m_BaseData;
        public tabSettings          m_GUI;
        private BindingSource       m_BindingSource;
        private DataTable           m_Datatable;
        private DataRetriever       retriever;
        private Boolean             m_NoGuiNotifyAfterSave;

        /// <summary>
        /// constructor
        /// </summary>
        public Settings()
        {
            try
            {
                m_BindingSource             = new BindingSource();
                m_Datatable                 = new DataTable();

                m_BindingSource.DataSource  = m_Datatable;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating the object", ex);
            }
        }

        /// <summary>
        /// gets or sets the belonging base dataset
        /// </summary>
        public dsEliteDB BaseData
        {
            get
            {
                return m_BaseData;
            }
            set
            {
                m_BaseData = value;
            }
        }

        /// <summary>
        /// access to the belonging gui object
        /// </summary>
        public tabSettings GUI
        {
            get
            {
                return m_GUI;
            }
            set
            {
                m_GUI = value;
                if((m_GUI != null) && (m_GUI.DataSource != this))
                    m_GUI.DataSource = this;
            }
        }

        private static ObjectDirectory PurgeOldDataFromDirectory(ObjectDirectory directory, DateTime deadline)
        {
            ObjectDirectory newDirectory;
            
            if(directory.GetType() == typeof(StationDirectory))
                newDirectory = new StationDirectory();
            else
                newDirectory = new CommodityDirectory();

            foreach (var x in directory)
            {
                var newList = new List<CsvRow>();
                foreach (var y in x.Value)
                    if (y.SampleDate >= deadline)
                        newList.Add(y);

                if(newList.Count > 0)
                    newDirectory.Add(x.Key, newList);
            }
            return newDirectory;
        }
    }
}

