using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using IBE.Enums_and_Utility_Classes;
using System.Data;
using System.Collections;
using System.Diagnostics;

namespace IBE.SQL
{
    public class DBConnector : IDisposable
    {
        public class ConnectionParams
        {
            public String                               Name;                  
            public String                               Server;                  
            public Int32                                Port;                  
            public String                               Database;
            public String                               User;
            public String                               Pass;
            public Int32                                TimeOut;
            public Boolean                              StayAlive;
            public Int32                                ConnectTimeout;
        }

        public enum SQLSortOrder
        {
            asc,
            desc
        }

        private Dictionary<Int32, Int32>                m_lockCounters = new Dictionary<int,int>();  
        private ConnectionParams                        m_ConfigData;
        private DbConnection                            m_Connection;
        private DbCommand                               m_Command;
        private DbTransaction                           m_Transaction;
        private bool                                    disposed                = false;

        //private string                                  m_SQLServer;
        //private string                                  m_DataBase;
    
        private Int32                                   m_Transcount;
    
        private bool                                    m_RollBackPending;          // rollback is running
    
        private string                                  m_TransActString;
    
        private Dictionary<string, DbDataAdapter>       m_UpdateObjects;

        ///// <summary>
        ///// simple constructor
        ///// </summary>
        //public DBConnector() 
        //{
        //}
    
        /// <summary>
        /// constructor for predefined settings
        /// </summary>
        /// <param name="Parameter"></param>
        public DBConnector(ConnectionParams Parameter, Boolean AutoConnect = false) 
        {
            try 
            {
                m_Connection                    = new MySqlConnection();
                m_Command                       = new MySqlCommand();

                Init(Parameter);

                if(AutoConnect)
                    Connect();

            }
            catch (Exception ex) {
                throw new Exception("Error in Init function", ex);
            }
        }


        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    foreach (KeyValuePair<string, DbDataAdapter> item in m_UpdateObjects)
                        item.Value.Dispose();

                    m_UpdateObjects.Clear();

                    m_Command.Dispose();
                    m_Connection.Dispose();
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~DBConnector()
        {
            // Simply call Dispose(false).
            Dispose (false);
        } 

        /// <summary>
        /// access to the Connection object
        /// </summary>
        public DbConnection Connection
        {
            get { return m_Connection; }
        }

        /// <summary>
        /// initializes the connection
        /// </summary>
        /// <param name="Parameter"></param>
        private void Init(ConnectionParams Parameter) 
        {
            try 
            {
                m_Transcount                    = 0;
                m_Transaction                   = null;

                m_ConfigData                    = Parameter;

                m_Connection                    = new MySqlConnection();
                m_Command                       = new MySqlCommand();

                m_UpdateObjects = new Dictionary<string, DbDataAdapter>(); 
            }
            catch (Exception ex) {
                throw new Exception("Error in Init function", ex);
            }
        }
    
        /// <summary>
        /// returns the depth of current transaction
        /// </summary>
        /// <returns></returns>
        public bool TransActive() 
        {
            return (m_Transcount > 0);
        }
    
        /// <summary>
        /// connects to the database
        /// </summary>
        /// <returns></returns>
        public Int32 Connect() 
        {
            Int32 retvalue;
            System.Text.StringBuilder tempConnString;

            retvalue = 0;

            tempConnString = new System.Text.StringBuilder();
            try {
                if (!string.IsNullOrEmpty(m_ConfigData.Database)) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("database=");
                    tempConnString.Append(m_ConfigData.Database);
                }
                if (!string.IsNullOrEmpty(m_ConfigData.Server)) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("server=");
                    tempConnString.Append(m_ConfigData.Server);
                }
                if (m_ConfigData.Port > 0) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("Port=");
                    tempConnString.Append(m_ConfigData.Port);
                }
                if (!string.IsNullOrEmpty(m_ConfigData.User)) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("user id=");
                    tempConnString.Append(m_ConfigData.User);
                }
                if (!string.IsNullOrEmpty(m_ConfigData.Pass)) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("Password=");
                    tempConnString.Append(m_ConfigData.Pass);
                }
                if (m_ConfigData.ConnectTimeout >=0) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("ConnectionTimeout=");
                    tempConnString.Append(m_ConfigData.ConnectTimeout);
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("DefaultCommandTimeout=");
                    tempConnString.Append(m_ConfigData.ConnectTimeout);
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("Keepalive=600");
                }

                if (true) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("Allow User Variables=");
                    tempConnString.Append("True");
                }

                if (true) 
                {
                    if (tempConnString.Length > 0)
                        tempConnString.Append(";");

                    tempConnString.Append("UseAffectedRows=");
                    tempConnString.Append("True");
                }

                m_Connection.ConnectionString = tempConnString.ToString();
                m_Connection.Open();

                if (m_Connection.State == System.Data.ConnectionState.Open) {
                    retvalue = 1;
                }

                return retvalue;
            }
            catch (Exception ex) 
            {
                throw new Exception("Fehler beim Verbinden, ConnectionString = <" + (tempConnString.ToString() + (">" + (" ("+ (m_ConfigData.Name + ")")))), ex);
            }
            
        }

        /// <summary>
        /// closes the connection to the database
        /// </summary>
        /// <returns></returns>
        public Int32 Close() 
        {
            try 
            {
                m_Connection.Close();
                return 1;
            }
            catch (Exception ex) 
            {
                throw new Exception("Fehler beim Schliessen der Verbindung", ex);
            }
        }

        /// <summary>
        /// sets the signal for the current thread to the object monitor
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="msTimeOut"></param>
        /// <returns></returns>
        private bool MonitorTryEnter(object Target, Int32 msTimeOut, String debugInfo = "n/a") 
        {
            bool retValue;
            retValue = Monitor.TryEnter(Target, msTimeOut);

            if(retValue)
            {
                if(m_lockCounters.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                    m_lockCounters[Thread.CurrentThread.ManagedThreadId] += 1;
                else
                    m_lockCounters.Add(Thread.CurrentThread.ManagedThreadId, 1);

                //Debug.Print(string.Format("locked by thread {0} (layer {2}) : {1}", Thread.CurrentThread.ManagedThreadId, debugInfo, m_lockCounters[Thread.CurrentThread.ManagedThreadId]));
            }
            return retValue;
        }

        /// <summary>
        /// removes the signal for the current thread from the object monitor
        /// </summary>
        /// <param name="Target"></param>
        private void MonitorExit(object Target, String debugInfo = "n/a") 
        {
            Monitor.Exit(Target);
            m_lockCounters[Thread.CurrentThread.ManagedThreadId] -= 1;
            //Debug.Print(string.Format("------ by thread {0} (layer {2} : {1}", Thread.CurrentThread.ManagedThreadId, debugInfo, m_lockCounters[Thread.CurrentThread.ManagedThreadId]));
        }
    
        /// <summary>
        /// signals a pulse to the monitor object
        /// </summary>
        /// <param name="Target"></param>
        private void MonitorPulse(object Target, String debugInfo = "n/a") 
        {
            Monitor.Pulse(Target);
            //Debug.Print("p----- by thread " + Thread.CurrentThread.ManagedThreadId.ToString() + " : " + debugInfo);
        }

        /// <summary>
        /// executes a non-return query
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        public Int32 Execute(string CommandText) 
        {
            Int32 retValue = 0;

            try {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut))
                    throw new Exception("Timeout while waiting for Monitor-Lock");

                using (DbCommand Command = new MySqlCommand())
                {
                    Command.CommandText     = CommandText;
                    Command.Connection      = m_Connection;

                    if (m_Transaction != null) 
                        Command.Transaction = m_Transaction;

                    retValue = Command.ExecuteNonQuery();

                    System.Diagnostics.Debug.Print("SQL:<" + CommandText + ">");
                }
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Error on \'ExecuteNonQuery\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }

        /// <summary>
        /// executes a query and puts the result in a dataset
        /// </summary>
        /// <param name="CommandText"></param>
        /// <param name="Tablename"></param>
        /// <param name="m_BaseData"></param>
        /// <returns></returns>
        public Int32 Execute(string CommandText, string Tablename, System.Data.DataSet Data) 
        {
            Int32 retValue = 0;

            try {

                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Timeout while waiting for Monitor-Lock");

                using (DbCommand Command = new MySqlCommand())
                {
                    using (DbDataAdapter DataAdapter = new MySqlDataAdapter())
                    {
                        Command.CommandText             = CommandText;
                        Command.Connection              = m_Connection;

                        if (m_Transaction != null) 
                            Command.Transaction = m_Transaction;

                        DataAdapter.SelectCommand = Command;

                        if (!string.IsNullOrEmpty(Tablename)) 
                            DataAdapter.Fill(Data, Tablename);
                        else
                            DataAdapter.Fill(Data);

                        retValue = Data.Tables.Count;
                    }
                }

            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Error on \'ExecuteReader\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }

        /// <summary>
        /// executes a query and puts the result in a datatable
        /// </summary>
        /// <param name="CommandText"></param>
        /// <param name="m_BaseData"></param>
        /// <returns></returns>
        public Int32 Execute(string CommandText, System.Data.DataTable Data) {
            Int32 retValue;

            try {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Timeout while waiting for Monitor-Lock");

                using (DbCommand Command = new MySqlCommand())
                {
                    using (DbDataAdapter DataAdapter = new MySqlDataAdapter())
                    {
                        Command.CommandText = CommandText;
                        Command.Connection = m_Connection;
                        if (m_Transaction != null) {
                            Command.Transaction = m_Transaction;
                        }
                        DataAdapter.SelectCommand = Command;
                        Data.Clear();
                        DataAdapter.Fill(Data);
                        retValue = Data.Rows.Count;
                    }
                }
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Error on \'ExecuteReader\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }

        public T Execute <T>(string CommandText) 
        {
            T retValue;

            if (typeof(T) == typeof(String))
                retValue = (T)Convert.ChangeType("", typeof(T));
            else if (typeof(T) == typeof(DateTime))
            {
                retValue = (T)Convert.ChangeType(new DateTime(2016, 11, 01), typeof(T));
            }
            else
                retValue = Activator.CreateInstance<T>();
            


            Object DataObject = null;

            try {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Timeout while waiting for Monitor-Lock");

                using (DbCommand Command = new MySqlCommand())
                {
                    Command.CommandText = CommandText;
                    Command.Connection = m_Connection;
                    if (m_Transaction != null) 
                    {
                        Command.Transaction = m_Transaction;
                    }

                    DataObject = Command.ExecuteScalar();

                    if((DataObject != null) && (DataObject != System.DBNull.Value))
                    {
                        if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            Type answer = Nullable.GetUnderlyingType(typeof(T));
                            retValue = (T)Convert.ChangeType(DataObject, answer);
                        }
                        else
                            retValue = (T)Convert.ChangeType(DataObject, typeof(T));
                    }
                }
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Error in \'ExecuteScalar\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }

        /// <summary>
        /// refreshes a with "TableRead" already loaded table out of the database
        /// </summary>
        /// <param name="Tablename">name of the table to refresh</param>
        /// <param name="m_BaseData">used dataset which is holding the table</param>
        /// <returns></returns>
        public Int32 TableRefresh(string Tablename, System.Data.DataSet Data)
        {
            string commandText = String.Empty;
            return TableRead(commandText, Tablename, Data);
        }

        /// <summary>
        /// refreshes a with "TableRead" already loaded table out of the database
        /// </summary>
        /// <param name="Tablename">name of the table to refresh</param>
        /// <param name="m_BaseData">used dataset which is holding the table</param>
        /// <returns></returns>
        public Int32 TableRefresh(DataTable TypifiedTable)
        {
            return TableRefresh(TypifiedTable.TableName, TypifiedTable.DataSet);
        }


        //ok
        public Int32 TableRead(string CommandText, System.Data.DataTable Data, ref MySqlDataAdapter dataAdapter) 
        {
            return TableRead(CommandText, Data.TableName, Data.DataSet, ref dataAdapter);
        }

        // ok
        public Int32 TableRead(string CommandText, System.Data.DataTable Data) 
        {
            return TableRead(CommandText, Data.TableName, Data.DataSet);
        }

        // ok
        public Int32 TableRead(string CommandText, string Tablename, System.Data.DataSet Data) 
        {
            Int32 retValue = 0;

            try {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Timeout while waiting for monitor-lock for TableRead()");

                if (!m_UpdateObjects.ContainsKey(Tablename)) {
                    //  loading data first time
                    if (string.IsNullOrEmpty(CommandText)) 
                        throw new Exception("no sql command specified");

                    DbCommand Command = new MySqlCommand();
                    DbCommandBuilder CommandBuilder = new MySqlCommandBuilder();
                    DbDataAdapter DataAdapter = new MySqlDataAdapter();
                            
                    //  prepare m_Command
                    Command.CommandText     = CommandText;
                    Command.Connection      = m_Connection;

                    if (m_Transaction != null) 
                        Command.Transaction = m_Transaction;

                    //  preparing dataadapter and stringbuilder (for changes on the tables) 
                    CommandBuilder.DataAdapter          = DataAdapter;
                    DataAdapter.MissingSchemaAction     = System.Data.MissingSchemaAction.AddWithKey;
                    DataAdapter.SelectCommand           = Command;

                    //  read data and fill table
                    DataAdapter.Fill(Data, Tablename);

                    //  save commandbuilder object 
                    m_UpdateObjects.Add(Tablename, DataAdapter);
                    
                    retValue  = Data.Tables.Count;

                }
                else 
                {
                    //  data already loaded - now only refreshing
                    Data.Tables[Tablename].Rows.Clear();
                    m_UpdateObjects[Tablename].Fill(Data.Tables[Tablename]);
                }
            }
            catch (Exception ex) 
            {
                MonitorExit(this);
                throw new Exception(("Error on \'TableRead\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }

        //ok
        public Int32 TableRead(string Tablename, System.Data.DataSet Data, ref MySqlDataAdapter dataAdapter) 
        {
            try 
            {
                return TableRead("", Tablename, Data, ref dataAdapter);
            }
            catch (Exception ex) 
            {
                throw new Exception("Error while reloading data through DataAdapter", ex);
            }
        }

        //ok
        public Int32 TableRead(string CommandText, string Tablename, System.Data.DataSet Data, ref MySqlDataAdapter dataAdapter) 
        {
            Int32 retValue = 0;

            try {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Timeout while waiting for monitor-lock for TableRead()");

                if (dataAdapter == null) {
                    //  loading data first time
                    if (string.IsNullOrEmpty(CommandText)) 
                        throw new Exception("no sql command specified");

                    DbCommand Command = new MySqlCommand();
                    DbCommandBuilder CommandBuilder = new MySqlCommandBuilder();
                    dataAdapter = new MySqlDataAdapter();

                    //  prepare m_Command
                    Command.CommandText     = CommandText;
                    Command.Connection      = m_Connection;

                    if (m_Transaction != null) 
                        Command.Transaction = m_Transaction;

                    //  preparing dataadapter and stringbuilder (for changes on the tables) 
                    CommandBuilder.DataAdapter          = dataAdapter;
                    dataAdapter.MissingSchemaAction     = System.Data.MissingSchemaAction.AddWithKey;
                    dataAdapter.SelectCommand           = (MySqlCommand)Command;

                    //  read data and fill table
                    dataAdapter.Fill(Data, Tablename);

                    retValue  = Data.Tables.Count;
                }
                else 
                {
                    //  data already loaded - now only refreshing
                    Data.Tables[Tablename].Rows.Clear();
                    dataAdapter.Fill(Data.Tables[Tablename]);
                }
            }
            catch (Exception ex) 
            {
                MonitorExit(this);
                throw new Exception(("Error on \'TableRead\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }

        public Int32 TableUpdate(string Tablename, System.Data.DataSet Data) {
            return TableUpdate(Tablename, Data, false, null);
        }
    
        public Int32 TableUpdate(string Tablename, System.Data.DataSet Data, bool removeTableReadObject) {
            return TableUpdate(Tablename, Data, removeTableReadObject, null);
        }
    
        public Int32 TableUpdate(string Tablename, System.Data.DataSet Data, System.Data.DataViewRowState recordStates) {
            return TableUpdate(Tablename, Data, false, null);
        }
    
        public Int32 TableUpdate(string Tablename, System.Data.DataSet Data, System.Data.DataViewRowState recordStates, bool removeTableReadObject) {
            return TableUpdate(Tablename, Data, false, null);
        }
    
        public Int32 TableUpdate(string Tablename, System.Data.DataSet Data, DbDataAdapter DataAdapter) {
            return TableUpdate(Tablename, Data, false, DataAdapter);
        }

        public Int32 TableUpdate(System.Data.DataTable TypifiedTable)
        {
            return TableUpdate(TypifiedTable.TableName, TypifiedTable.DataSet, false, null);
        }
        public Int32 TableUpdate(System.Data.DataTable TypifiedTable, DbDataAdapter DataAdapter)
        {
            return TableUpdate(TypifiedTable.TableName, TypifiedTable.DataSet, false, DataAdapter);
        }

        public Int32 TableUpdate(System.Data.DataTable TypifiedTable, bool removeTableReadObject)
        {
            return TableUpdate(TypifiedTable.TableName, TypifiedTable.DataSet, removeTableReadObject, null);
        }

        //public Int32 TableUpdate(System.Data.DataSet Data, DbDataAdapter DataAdapter)
        //{
        //    string tablename = String.Empty;
        //    bool removeTableReadObject = false;
        //    return TableUpdate(tablename, Data, removeTableReadObject, DataAdapter);
        //}

        public Int32 TableUpdate(string Tablename, System.Data.DataSet Data, bool removeTableReadObject, DbDataAdapter DataAdapter){

            Int32         retValue = 0;
            DbDataAdapter lDataAdapter;

            try{

                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Timeout while waiting for monitor-lock for TableUpdate()");

                if ((DataAdapter == null) && !m_UpdateObjects.ContainsKey(Tablename)){
                    //  table is unknown
                    throw new Exception(string.Format("Table {0} is not existing", Tablename));

                }else{
                    //  select dataadapter
                    if (DataAdapter != null) 
                        lDataAdapter = DataAdapter;
                    else 
                        lDataAdapter = m_UpdateObjects[Tablename];

                    // refresh data
                    lDataAdapter.Update(Data, Tablename);
                }

                if ((DataAdapter == null) && removeTableReadObject) 
                    TableReadRemove(Tablename);

            }catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Error on \'TableUpdate\', SQLString <" 
                                + (Tablename + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }

            MonitorExit(this);

            return retValue;
        }

        public void TableReadRemove(DataTable TypifiedTable)
        {
            TableReadRemove(TypifiedTable.TableName);
        }

        public void TableReadRemove(string Tablename) 
        {
            try{
                if (m_UpdateObjects.ContainsKey(Tablename)){

                    m_UpdateObjects[Tablename].Dispose();
                    m_UpdateObjects.Remove(Tablename);

                }
            }catch (Exception ex) {
                throw new Exception("Error on \'TableReadRemove\'", ex);
            }
        }

        public static String SQLNow(){
            return SQLDateTime(DateTime.Now);
        }
    
        public static String SQLToday() {
            return SQLDate(DateTime.Now);
        }
    
        public static String SQLDateTime(DateTime datDateTime) {
            return string.Format("{{ts \'{0:yyyy-MM-dd HH:mm:ss}\'}}", datDateTime);
        }
    
        public static String SQLDate(DateTime datDateTime) {
            return string.Format("{{ts \'{0:yyyy-MM-dd}\'}}", datDateTime);
        }
    
        public static String SQLDateOracle(DateTime datDateTime) {
            return string.Format("to_Date(\'{0:dd.MM.yyyy}\', \'DD.MM.YYYY\')", datDateTime);
        }
    
        public static String SQLAString(string DBString) {
            // DBString = DBString.Replace("\'", "");
            return ("\'" + (DBString + "\'"));
        }
    
        public static string SQLDecimal(double DBDouble) {
            return DBDouble.ToString("g", new System.Globalization.CultureInfo("en-US"));
        }
    
        public static string SQLDecimal(float DBFloat) {
            return DBFloat.ToString("g", new System.Globalization.CultureInfo("en-US"));
        }

        public Int32 TransBegin()
        {
            String procedureName = String.Empty;
            return TransBegin(procedureName);
        }

        public Int32 TransBegin(String ProcedureName){
            try {

                //make it thread-save with a monitor
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Transbegin()");

                if (m_RollBackPending) {
                    //  remove this layer in the monitor and tell all other
                    MonitorPulse(this);
                    MonitorExit(this);
                    throw new Exception("Aufruf von BeginTrans trotz laufendem Rollback");
                }
                else if ((m_Transcount == 0)) {
                    // this is the first level - initiate the transaction
                    TransStringAdd(ProcedureName);
                    m_Transaction = m_Connection.BeginTransaction();
                    m_Transcount = 1;
                }
                else {
                    // this is a higher level - only increase transaction level counter
                    TransStringAdd(ProcedureName);
                    m_Transcount++;
                }

                return m_Transcount;

            }catch (Exception ex) {
                try {
                    //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                    MonitorPulse(this);
                    MonitorExit(this);
                }
                catch (Exception) {}

                throw new Exception("Fehler beim Transaktions-Start", ex);
            }
        }

        // '' <summary>
        // '' Best�tigt eine Transaktion und setzt den Transaktionz�hler zur�ck
        // '' </summary>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TransCommit(){
            try {
                //make it thread-save with a monitor
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                    throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Transbegin()");

                if (m_RollBackPending) {
                    //  remove this layer in the monitor and tell all other
                    MonitorPulse(this);
                    MonitorExit(this);
                    throw new Exception("Aufruf von CommitTrans trotz laufendem Rollback");
                }
                else if ((m_Transcount == 1)) {
                    TransStringRemove();
                    m_Transaction.Commit();
                    m_Transcount = 0;
                    m_Transaction = null;
                    //  eine Ebene im Monitor wieder entfernen
                    MonitorExit(this);
                }
                else if ((m_Transcount > 1)) {
                    //  eine Ebene im Monitor wieder entfernen
                    TransStringRemove();
                    m_Transcount--;
                    MonitorExit(this);
                }
                else {
                    //  remove this layer in the monitor and tell all other
                    TransStringRemove();
                    MonitorPulse(this);
                    MonitorExit(this);
                    throw new Exception("Keine Transaktion aktiv");
                }
                
                MonitorPulse(this);
                MonitorExit(this);
                // LogFile.Write("Aussprung TransCommit, Transstring <" & m_TransActString & ">")         

                return m_Transcount;
            }
            catch (Exception ex) {

                try {
                    //  remove this layer in the monitor and tell all other
                    MonitorPulse(this);
                    MonitorExit(this);
                }
                catch (System.Exception) {}

                throw new Exception("Fehler beim Transaktions-Commit", ex);
            }
        }


        public Int32 TransRollback(){
            try {
                //make it thread-save with a monitor
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut))
                    throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Transbegin()");

                if (m_Transcount == 1) {
                    if (!(m_Transaction == null)) 
                        m_Transaction.Rollback();

                    m_Transcount = 0;
                    m_Transaction = null;
                    m_RollBackPending = false;
                    //  eine Ebene im Monitor wieder
                    MonitorExit(this);
                }
                else if ((m_Transcount > 1)) {
                    // LogFile.Write("TransRollback 2")       
                    m_RollBackPending = true;
                    m_Transcount--;
                    //  eine Ebene im Monitor wieder
                    MonitorExit(this);
                }

                //  remove this layer in the monitor and tell all other
                MonitorPulse(this);
                MonitorExit(this);

                return m_Transcount;
            }
            catch (Exception ex) {
                try {
                    //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                    MonitorPulse(this);
                    MonitorExit(this);
                }
                catch (Exception) {}

                throw new Exception("Fehler beim Transaktions-Commit", ex);
            }   
        }

        public void TransStringAdd(String Name){

            if (String.IsNullOrEmpty(m_TransActString) || (m_TransActString.Trim().Length > 0))
                m_TransActString = string.Format("{0}({1})", Name, m_Transcount);
            else 
                m_TransActString = string.Format("{0},{1}({2})", m_TransActString, Name, m_Transcount);

        }

        public void TransStringRemove(){

            if (string.IsNullOrEmpty(m_TransActString) || (m_TransActString.Trim().Length > 0))
                m_TransActString = String.Empty;

            else if (m_TransActString.LastIndexOf(",") > -1)
                m_TransActString = m_TransActString.Substring(0, (m_TransActString.LastIndexOf(",") - 1));

            else
                m_TransActString = String.Empty;
        }


        /// <summary>
        /// Liest einen Wert aus der Init-Tabelle der verbundenen Datenbank und gibt ihn als Typ (Of T) zurück
        /// </summary>
        /// <param name="Group">Gruppe des zu lesenden Datums</param>
        /// <param name="Key">Key  des zu lesenden Datums</param>
        /// <param name="DefaultValue">optionaler Defaultwert. Wird gesetzt, wenn Wert leer oder nicht vorhanden ist</param>
        /// <param name="AllowEmptyValue">True: Leerstring ist erlaubt, False: Leerstring wird durch DefaultValue ersetzt</param>
        /// <param name="RewriteOnBadCast">True: ist eine Typkonvertierung in (Of T) nicht möglich wird der Defaultwert eingesetzt und auch in die Datenbank geschrieben</param>
        /// <returns></returns>
        public T getIniValue<T>(string Group, string Key, string DefaultValue = "", bool AllowEmptyValue = true, bool RewriteOnBadCast = true)
        {
            T functionReturnValue = default(T);

            try
            {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
	                throw new Exception("Timeout while waiting for Monitor-Lock");

	            try {

                    if(typeof(T).BaseType.Name.Equals("Enum"))
                    {
                        String value    = getIniValue(Group, Key, DefaultValue, AllowEmptyValue);
                        functionReturnValue = (T)Enum.Parse(typeof(T), value, true);
                    }
                    else if (!typeof(T).IsValueType)
                    {
                        String value    = getIniValue(Group, Key, DefaultValue, AllowEmptyValue);
                        var parse       = typeof(T).GetMethod("Parse", new[] { typeof(string) });

                        if (parse != null) 
                            functionReturnValue = (T)parse.Invoke(null, new object[] { value });
                        else
                            functionReturnValue = (T)Convert.ChangeType(getIniValue(Group, Key, DefaultValue, AllowEmptyValue), typeof(T));
                    }
                    else
                        functionReturnValue = (T)Convert.ChangeType(getIniValue(Group, Key, DefaultValue, AllowEmptyValue), typeof(T));

	            } catch (ArgumentNullException ex) {
		            throw new Exception("conversionType ist Nothing", ex);
	            } catch (Exception ex) {
		            if (RewriteOnBadCast) {
			            // Versuchen, den Defaultwert einzutragen
			            try {
				            setIniValue(Group, Key, DefaultValue);
				            functionReturnValue = getIniValue<T>(Group, Key, DefaultValue, AllowEmptyValue, false);
			            } catch {
				            // Defaultwert ist ebenfalls ungültig -> Abbruch
				            throw new Exception("Diese Konvertierung wird nicht unterstützt oder der Wert ist Nothing und conversionType ist ein Werttyp (Value-Rewrite done), R", ex);
			            }
		            } else {
			            // direkter Abbruch
			            throw new Exception("Diese Konvertierung wird nicht unterstützt oder der Wert ist Nothing und conversionType ist ein Werttyp", ex);
		            }
	            }
            }
            catch (Exception ex) {
	            MonitorExit(this);
	            throw new Exception("Error in GetIniValue", ex);
            }
            MonitorExit(this);

            return functionReturnValue;

        }

        /// <summary>
        /// Liest einen Wert aus der Init-Tabelle der verbundenen Datenbank
        /// </summary>
        /// <param name="Group">Gruppe des zu lesenden Datums</param>
        /// <param name="Key">Key  des zu lesenden Datums</param>
        /// <param name="DefaultValue">optionaler Defaultwert. Wird gesetzt, wenn Wert leer oder nicht vorhanden ist</param>
        /// <param name="AllowEmptyValue">True: Leerstring ist erlaubt, False: Leerstring wird durch DefaultValue ersetzt</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string getIniValue(string Group, string Key, string DefaultValue = "", bool AllowEmptyValue = true)
        {
            string functionReturnValue = null;

            try
            {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
	                throw new Exception("Timeout while waiting for Monitor-Lock");


	            DataTable Data = new DataTable();
	            string sqlString = null;
	            string Result = null;

	            functionReturnValue = string.Empty;
	            Result = string.Empty;

	            sqlString = "select InitValue from tbInitValue" + " where InitGroup = " + SQLAString(Group) + " and   InitKey   = " + SQLAString(Key);

	            Execute(sqlString, Data);

	            if (Data.Rows.Count > 0) {
		            // Datum gefunden
		            Result = Data.Rows[0]["InitValue"].ToString();
	            }

	            if ((Data.Rows.Count == 0)) {
		            // Wert gar nicht vorhanden

		            if (!AllowEmptyValue & string.IsNullOrEmpty(DefaultValue)) {
			            // Leerwert nicht erlaubt aber kein Wert vorhanden
			            throw new Exception("Leerwert nicht erlaubt, aber kein Wert vorhanden (1): <getIniValue(" + Group + ", " + Key + ", " + DefaultValue + ", " + AllowEmptyValue + ")>");
		            }
                    else if(!AllowEmptyValue)
                    {
		                // Defaultwert eintragen
		                sqlString = "insert into tbInitValue (InitGroup, InitKey, InitValue) values (" + SQLAString(Group) + "," + SQLAString(Key) + "," + SQLAEscape(DefaultValue) + ")";
		                Execute(sqlString);
                    }

		            Result = DefaultValue;
	            } else if (string.IsNullOrEmpty(Result) & !AllowEmptyValue) {
		            // Wert ist leer, Leerwerte sind aber nicht erlaubt

		            if (string.IsNullOrEmpty(DefaultValue)) {
			            // Leerwert nicht erlaubt aber kein Wert vorhanden
			            throw new Exception("Leerwert nicht erlaubt, aber kein Wert vorhanden (2): <getIniValue(" + Group + ", " + Key + ", " + DefaultValue + ", " + AllowEmptyValue + ")");
		            }

		            sqlString = "update tbInitValue" + " set InitValue = " + SQLAEscape(DefaultValue);
		            Execute(sqlString);

		            Result = DefaultValue;
	            }

	            functionReturnValue = Result;

            }
            catch (Exception ex) {
	            MonitorExit(this);
	            throw new Exception("Error in GetIniValue", ex);
            }
            MonitorExit(this);

	        return functionReturnValue;

        }

        /// <summary>
        /// Schreibt einen Wert in die Init-Tabelle der verbundenen Datenbank
        /// </summary>
        /// <param name="Group">Gruppe des zu schreibenden Datums</param>
        /// <param name="Key">Key des zu schreibenden Datums</param>
        /// <param name="Value">zu setzender Wert</param>
        /// <returns>"false" if value was not changed (same value as before); true if the value was changed</returns>
        public Boolean setIniValue(string Group, string Key, string Value)
        {
            Boolean retValue  = false;

            try
            {
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
	                throw new Exception("Timeout while waiting for Monitor-Lock");

                DataTable Data = new DataTable();
	            string sqlString = null;

	            sqlString = "select InitValue from tbInitValue" + " where InitGroup = " + SQLAString(Group) + " and   InitKey   = " + SQLAString(Key);

	            Execute(sqlString, Data);

	            if ((Data.Rows.Count == 0)) {
		            // Wert gar nicht vorhanden

		            // Wert eintragen
		            sqlString = "insert into tbInitValue (InitGroup, InitKey, InitValue) values (" + SQLAString(Group) + "," + SQLAString(Key) + "," + SQLAEscape(Value) + ")";
		            retValue = (Execute(sqlString) != 0);

	            } else {
		            // Wert bereits vorhanden
		            sqlString = "update tbInitValue" + " set InitValue   = " + SQLAEscape(Value) + " where InitGroup = " + SQLAString(Group) + " and   InitKey   = " + SQLAString(Key);
		            retValue = (Execute(sqlString) != 0);
	            }

            }
            catch (Exception ex) {
	            MonitorExit(this);
	            throw new Exception("Error in GetIniValue", ex);
            }
            MonitorExit(this);

            return retValue;
        }

        /// <summary>
        /// returns a fully escaped string for a database query
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SQLAEscape(string str)
        {
            return "'" + SQLEscape(str) + "'";
        }

        /// <summary>
        /// returns a fully escaped string for a database query
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SQLEscape(string str, bool allowPercent = false)
        {
            String compare = allowPercent ? @"[\x00'""\b\n\r\t\cZ\\]" : @"[\x00'""\b\n\r\t\cZ\\%]";

            return System.Text.RegularExpressions.Regex.Replace(str, compare,
                delegate(System.Text.RegularExpressions.Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                            return "\\0";   
                        case "\b":              // BACKSPACE character
                            return "\\b";
                        case "\n":              // NEWLINE (linefeed) character
                            return "\\n";
                        case "\r":              // CARRIAGE RETURN character
                            return "\\r";
                        case "\t":              // TAB
                            return "\\t";
                        case "\u001A":          // Ctrl-Z
                            return "\\Z";
                        case "'":          
                            return "''";
                        default:
                            return "\\" + v;
                    }
                });
        } 

        /// <summary>
        /// return a fully escaped string for a datatable
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DTEscape(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%]",
                delegate(System.Text.RegularExpressions.Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                            return "\\0";   
                        case "\b":              // BACKSPACE character
                            return "\\b";
                        case "\n":              // NEWLINE (linefeed) character
                            return "\\n";
                        case "\r":              // CARRIAGE RETURN character
                            return "\\r";
                        case "\t":              // TAB
                            return "\\t";
                        case "\u001A":          // Ctrl-Z
                            return "\\Z";
                        case "'":       
                            return "''";
                        default:
                            return "\\" + v;
                    }
                });
        } 
    
        /// <summary>
        /// retrieving the primary key of a table in the current connection
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public List<String> getPrimaryKey(String Table)
        {
            String       sqlString;
            List<String> retValue;
            DataTable    Data;

            try
            {
                Data        = new DataTable();
                retValue    = new List<String>();

                sqlString = "SELECT kcu.table_name, kcu.column_name" +
                            " FROM information_schema.key_column_usage AS kcu, information_schema.table_constraints AS tc" +
                            " WHERE tc.table_name       = kcu.table_name" +
                            "   AND tc.constraint_type  = 'PRIMARY KEY'" +
                            "   AND kcu.constraint_name = 'PRIMARY'" +
                            "   AND kcu.table_name      = " + DBConnector.SQLAString(Table);                   

                this.Execute(sqlString, Data);

                foreach (DataRow Row in Data.Rows)
                    retValue.Add(Row["column_name"].ToString());

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retriving the primary key for table <" + Table + ">", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"<table</param>
        /// <param name="OrderBy">name of the column to order by</param>
        /// <param name="OrderDirection">order direction</param>
        /// <param name="KeyColumn">name of the column with the searched value</param>
        /// <param name="KeyValue">searched value (for strings etc. add single quotes)</param>
        /// <returns>zero-based index of the column, -1 if not found</returns>
        public Int32 getRowIndex(String TableName, String OrderBy, SQLSortOrder OrderDirection, String KeyColumn, String KeyValue)
        {
            String       sqlString;
            DataTable    Data;
            Int32        retValue;

            try
            {
                retValue = -1;

                sqlString = String.Format("select ROWNUM from ( " +
                                          "   SELECT @rownum:= @rownum+1 ROWNUM, t.*" +
                                          "       FROM (SELECT @rownum:=0) r, (SELECT * FROM {0} ORDER BY {1} {2}) t ) t2" +
                                          " where {3} = {4} limit 1", TableName, OrderBy, OrderDirection.ToString(), KeyColumn, KeyValue );

                Data = new DataTable();

                if (this.Execute(sqlString, Data) == 1)
                { 
                    retValue = (Int32)((Double)Data.Rows[0]["ROWNUM"])-1 ;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving a rowindex", ex);
            }
        }

        public ConnectionParams ConfigData
        {
            get
            {
                return m_ConfigData;
            }
        }

        internal static string GetString_Or<T>(string p, List<T> CommoditiesSend)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                foreach (T item in CommoditiesSend)
                {
                    if(sb.Length > 0)
                        sb.Append(" or ");

                    if(Type.GetTypeCode(typeof(T)) == TypeCode.Int32)
                        sb.Append(String.Format("({0} = {1})", p, item.ToString()));                    
                    else if(Type.GetTypeCode(typeof(T)) == TypeCode.String)
                        sb.Append(String.Format("({0} = '{1})'", p, item.ToString()));                    

                }
                    
                return " (" + sb.ToString() + ")"; 

            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating or-string", ex);
            }
        }
    }

}