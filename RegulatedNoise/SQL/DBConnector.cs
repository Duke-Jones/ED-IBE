using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using RegulatedNoise.Enums_and_Utility_Classes;
using System.Data;
using System.Collections;

namespace RegulatedNoise.SQL
{
    public class DBConnector : IDisposable
    {
        public class ConnectionParams
        {
            public String                               Name;                  
            public String                               Server;                  
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
        public DBConnector(ConnectionParams Parameter) 
        {
            try 
            {
                m_Connection                    = new MySqlConnection();
                m_Command                       = new MySqlCommand();

                Init(Parameter);

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
        private bool MonitorTryEnter(object Target, Int32 msTimeOut) 
        {
            bool retValue;
            retValue = Monitor.TryEnter(Target, msTimeOut);
            return retValue;
        }

        /// <summary>
        /// removes the signal for the current thread from the object monitor
        /// </summary>
        /// <param name="Target"></param>
        private void MonitorExit(object Target) 
        {
            Monitor.Exit(Target);
        }
    
        /// <summary>
        /// signals a pulse to the monitor object
        /// </summary>
        /// <param name="Target"></param>
        private void MonitorPulse(object Target) 
        {
            Monitor.Pulse(Target);
        }

        /// <summary>
        /// executes a non-return query
        /// </summary>
        /// <param name="CommandText"></param>
        /// <returns></returns>
        public Int32 Execute(string CommandText) 
        {
            Int32 retValue = 0;

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut))
                throw new Exception("Timeout while waiting for Monitor-Lock");

            try {
                DbCommand Command       = new MySqlCommand();
                Command.CommandText     = CommandText;
                Command.Connection      = m_Connection;

                if (m_Transaction != null) 
                    Command.Transaction = m_Transaction;

                retValue = Command.ExecuteNonQuery();

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

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                throw new Exception("Timeout while waiting for Monitor-Lock");

            try {
                DbCommand Command               = new MySqlCommand();
                DbDataAdapter DataAdapter       = new MySqlDataAdapter();
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

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                throw new Exception("Timeout while waiting for Monitor-Lock");

            try {
                DbCommand Command = new MySqlCommand();
                DbDataAdapter DataAdapter = new MySqlDataAdapter();

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

        public Int32 TableRead(string CommandText, string Tablename, System.Data.DataSet Data) 
        {
            Int32 retValue = 0;

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Timeout while waiting for monitor-lock for TableRead()");
            }

            try {
                if (!m_UpdateObjects.ContainsKey(Tablename)) {
                    //  loading data first time
                    if (string.IsNullOrEmpty(CommandText)) 
                        throw new Exception("no sql command specified");

                    DbCommand Command                   = new MySqlCommand();
                    DbCommandBuilder CommandBuilder     = new MySqlCommandBuilder();
                    DbDataAdapter DataAdapter           = new MySqlDataAdapter();

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

        public Int32 TableUpdate(string Tablename, System.Data.DataSet Data, bool removeTableReadObject, DbDataAdapter DataAdapter){

            Int32         retValue = 0;
            DbDataAdapter lDataAdapter;

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) 
                throw new Exception("Timeout while waiting for monitor-lock for TableUpdate()");

            try{
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
	        try {
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
        public string getIniValue(string Group, string Key, string DefaultValue = "", bool AllowEmptyValue = true, bool WriteEmptyValue = false)
        {
	        string functionReturnValue = null;

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
                else if(WriteEmptyValue)
                {
		            // Defaultwert eintragen
		            sqlString = "insert into tbInitValue (InitGroup, InitKey, InitValue) values (" + SQLAString(Group) + "," + SQLAString(Key) + "," + SQLAString(DefaultValue) + ")";
		            Execute(sqlString);
                }

		        Result = DefaultValue;
	        } else if (string.IsNullOrEmpty(Result) & !AllowEmptyValue) {
		        // Wert ist leer, Leerwerte sind aber nicht erlaubt

		        if (string.IsNullOrEmpty(DefaultValue)) {
			        // Leerwert nicht erlaubt aber kein Wert vorhanden
			        throw new Exception("Leerwert nicht erlaubt, aber kein Wert vorhanden (2): <getIniValue(" + Group + ", " + Key + ", " + DefaultValue + ", " + AllowEmptyValue + ")");
		        }

		        sqlString = "update tbInitValue" + " set InitValue = " + SQLAString(DefaultValue);
		        Execute(sqlString);

		        Result = DefaultValue;
	        }

	        functionReturnValue = Result;
	        return functionReturnValue;

        }

        /// <summary>
        /// Schreibt einen Wert in die Init-Tabelle der verbundenen Datenbank
        /// </summary>
        /// <param name="Group">Gruppe des zu schreibenden Datums</param>
        /// <param name="Key">Key des zu schreibenden Datums</param>
        /// <param name="Value">zu setzender Wert</param>
        /// <remarks></remarks>
        public void setIniValue(string Group, string Key, string Value)
        {

	        DataTable Data = new DataTable();
	        string sqlString = null;

	        sqlString = "select InitValue from tbInitValue" + " where InitGroup = " + SQLAString(Group) + " and   InitKey   = " + SQLAString(Key);

	        Execute(sqlString, Data);

	        if ((Data.Rows.Count == 0)) {
		        // Wert gar nicht vorhanden

		        // Wert eintragen
		        sqlString = "insert into tbInitValue (InitGroup, InitKey, InitValue) values (" + SQLAString(Group) + "," + SQLAString(Key) + "," + SQLAString(Value) + ")";
		        Execute(sqlString);
	        } else {
		        // Wert bereits vorhanden
		        sqlString = "update tbInitValue" + " set InitValue   = " + SQLAString(Value) + " where InitGroup = " + SQLAString(Group) + " and   InitKey   = " + SQLAString(Key);
		        Execute(sqlString);
	        }

        }

        /// <summary>
        /// returns a fully escaped string for a database query
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SQLEscape(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
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
            return System.Text.RegularExpressions.Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%_]",
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
    }


#if false
    public class DBConnector {
    
    // '' <summary>
    // '' Stellt Verbindungen zur einer SQL-Datenbank her und Zugriffsfunktionen zur Verf�gung
    // '' </summary>
    // '' <remarks></remarks>
    public class DBConnector {
    

        private ConnectionParams                        m_ConfigData;
       
        const double                                    DEFAULT_MAX_TIME           = 10.0;

        private DbConnection                            m_Connection;
        private DbCommand                               m_Command;
        private DbTransaction                           m_Transaction;
    
        private string                                  m_SQLServer;
        private string                                  m_DataBase;
    
        private Int32                                   m_Transcount;
    
        private bool                                    m_RollBackPending;          // rollback is running
    
        private double                                  m_maxTransactionTime;       //  max. time for transactions
        private DateTime                                m_TransStartTime;
    
        //  Startzeitpunkt der Transaktion
        private System.Timers.Timer                     m_Watchdog_Trans;
        private System.Timers.Timer                     m_Watchdog_Alive;
    
        private string                                  m_TransActString;
    
        private Dictionary<string, DbDataAdapter>       m_UpdateObjects;
    
    
        // '' <summary>
        // '' Konstruktor
        // '' </summary>
        // '' <param name="DatabaseType"></param>
        // '' <param name="Objectname">Name(ID) des zugeh�rigen Objektes</param>
        // '' <param name="IniFile">Pfad zur zu nutzende Ini-Datei</param>
        // '' <remarks></remarks>
        public DBConnector() 
        {
        }
    
        // '' <summary>
        // '' F�hrt notwendige Initialisierungen durch
        // '' </summary>
        // '' <remarks></remarks>
        private void Init(ConnectionParams Parameter) 
        {
            try 
            {
                m_ConfigData                    = Parameter;

                m_Connection                    = new MySqlConnection();
                m_Command                       = new MySqlCommand();

                m_Watchdog_Trans                = new System.Timers.Timer(10000);
                m_Watchdog_Trans.AutoReset      = true;
                m_Watchdog_Trans.Elapsed       += new System.EventHandler(this.Watchdog_Trans_Elapsed);

                if (m_ConfigData.StayAlive) {
                    m_Watchdog_Alive = new System.Timers.Timer((30 * 1000));
                    m_Watchdog_Alive.AutoReset = true;
                    m_Watchdog_Alive.Elapsed += new System.EventHandler(this.Watchdog_Alive_Elapsed);
                    m_Watchdog_Alive.Start();
                }
                m_UpdateObjects = new Dictionary<string, DbDataAdapter>; 
            }
            catch (Exception ex) {
                throw new Exception("Error in Init function", ex);
            }
        }
    
        // '' <summary>
        // '' Gibt zur�ck, ob eine Transaktion derzeit aktiv ist
        // '' </summary>
        // '' <value></value>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public bool TransActive {
            get 
            {
                return (m_Transcount > 0);
            }
        }
    
    
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

                m_Connection.ConnectionString = tempConnString.ToString();
                m_Connection.Open();

                if ((m_Connection.State == System.Data.ConnectionState.Open)) {
                    retvalue = 1;
                }

                return retvalue;
            }
            catch (Exception ex) 
            {
                throw new Exception("Fehler beim Verbinden, ConnectionString = <" + (tempConnString.ToString() + (">" + (" ("+ (m_ConfigData.Name + ")")))), ex);
            }
            
        }
    
        // '' <summary>
        // '' Stellt die Verbindung zur Datenbank her
        // '' </summary>
        // '' <returns><c>1</c> wenn Datenbank verbunden werden konnte,
        // '' <c>0</c> anderenfalls.</returns>
        // '' <remarks></remarks>
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
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der betroffenen Datens�tze zur�ck
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <returns>Anzahl der betroffenen Zeilen</returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText) {
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 1, <" & CommandText & ">") 
            try {
                DbCommand Command = new MySqlCommand();
                Command.CommandText = CommandText;
                Command.Connection = m_Connection;
                if (m_Transaction != null) 
                {
                    Command.Transaction = m_Transaction;
                }
                return Command.ExecuteNonQuery;
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteNonQuery\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der betroffenen Datens�tze zur�ck
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="DataReader">(out) gibt einen DbDataReader zur�ck</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText, ref DbDataReader DataReader) 
        {
            Int32 retValue = 0;

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 2, <" & CommandText & ">") 
            try {
                DbCommand Command = new MySqlCommand();
                Command.CommandText = CommandText;
                Command.Connection = m_Connection;
                if (m_Transaction != null) {
                    Command.Transaction = m_Transaction;
                }
                DataReader = Command.ExecuteReader();
                retValue = DataReader.RecordsAffected;
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteReader\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der betroffenen Datens�tze zur�ck
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="DataAdapter">(out) gibt einen DbDataAdapter zur�ck</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText, ref DbDataAdapter DataAdapter) 
        {
            Int32 retValue = 0;

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 3, <" & CommandText & ">") 
            try {
                DbCommand Command = new MySqlCommand();
                Command.CommandText = CommandText;
                Command.Connection = m_Connection;
                if (m_Transaction != null) {

                    Command.Transaction = m_Transaction;
                }
                DataAdapter.SelectCommand = Command;
                retValue = 0;
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteReader\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der Tabellen zur�ck
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="Data">(out) gibt ein Dataset zur�ck</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText, ref System.Data.DataSet Data) {
            return Execute(CommandText, String.Empty, Data);
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der Tabellen
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="Data">(out) gibt ein Dataset zur�ck</param>
        // '' <param name="TableName">zu setzender Tabellenname</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText, string Tablename, ref System.Data.DataSet Data) 
        {
            Int32 retValue = 0;

            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 4, <" & CommandText & ">") 
            try {
                DbCommand Command = new MySqlCommand();
                DbDataAdapter DataAdapter = new MySqlDataAdapter();
                Command.CommandText = CommandText;
                Command.Connection = m_Connection;
                if (m_Transaction != null) {
                    Command.Transaction = m_Transaction;
                }
                DataAdapter.SelectCommand = Command;
                if (!string.IsNullOrEmpty(TableName)) {
                    DataAdapter.Fill(Data, TableName);
                }
                else {
                    DataAdapter.Fill(Data);
                }
                retValue = Data.Tables.Count;
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteReader\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);

            return retValue;
        }
    
        // '' <summary>
        // '' Liest eine (ggf. neu) Tabelle ein und legt Befehle zum automatsichen aktualisieren der Datens�tze an
        // '' falls noch nicht vorhanden
        // '' </summary>
        // '' <param name="TableName">zu setzender/nutzender Tabellenname</param>
        // '' <param name="Data">(out) das Dataset, das die Tabelle enth�lt</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableRead(string Tablename, ref System.Data.DataSet Data) {
            try {
                return TableRead(String.Empty, Tablename, Data);
            }
            catch (Exception ex) {
                throw new Exception("Fehler bei \'readTable\' zum neu Einlesen von Daten", ex);
            }
        }
    
        // '' <summary>
        // '' L�scht den Verweis auf den DataAdapter f�r die angegebene Tabelle
        // '' </summary>
        // '' <param name="Tablename"></param>
        // '' <remarks></remarks>
        public void TableReadRemove(string Tablename) {
            try {
                if (m_UpdateObjects.ContainsKey(Tablename)) {
                    m_UpdateObjects.Item[Tablename].Dispose;
                    m_UpdateObjects.Remove(Tablename);
                }
            }
            catch (Exception ex) {
                throw new Exception("Fehler bei \'TableReadRemove\'", ex);
            }
        }
    
        // '' <summary>
        // '' Liest eine (ggf. neu) Tabelle ein und legt Befehle zum automatsichen aktualisieren der Datens�tze an
        // '' falls noch nicht vorhanden, Das DataAdapter-Objekt wird intern gehalten und muss manuelle gel�scht werden.
        // '' Der Name eines DataAdapter-Objektes darf pro Datenbankverbindung nur 1x vorhanden sein
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="Data">(out) das Dataset, das die Tabelle enth�lt</param>
        // '' <param name="TableName">zu setzender/nutzender Tabellenname</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableRead(string CommandText, string Tablename, ref System.Data.DataSet Data) {
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r readTable()");
            }
            // LogFile.Write("Execute 4, <" & CommandText & ">") 
            try {
                if (!m_UpdateObjects.ContainsKey(Tablename)) {
                    //  Daten das erste mal laden
                    if (string.IsNullOrEmpty(CommandText)) {
                        throw new Exception("kein SQL-Kommando angegeben");
                    }
                    DBCommand Command = new MySqlCommand();
                    DBCommandBuilder CommandBuilder = new MySqlCommandBuilder();
                    DbDataAdapter DataAdapter = new MySqlDataAdapter();
                    //  Sql-Kommando vorbereiten
                    Command.CommandText = CommandText;
                    Command.Connection = m_Connection;
                    if (m_Transaction != null) {
                        Command.Transaction = m_Transaction;
                    }
                    //  Datenadapter und Stringbuilder (f�r �nderungen an den Tabellen) vorbereiten
                    CommandBuilder.DataAdapter = DataAdapter;
                    DataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    DataAdapter.SelectCommand = Command;
                    //  Daten lesen und Tabelle f�llen
                    DataAdapter.Fill(Data, TableName);
                    //  CommandBuilder-Objekt speichern
                    m_UpdateObjects.Add(Tablename, DataAdapter);
                    TableRead = Data.Tables.Count;
                }
                else {
                    //  Daten wurden schon geladen, jetzt nur aktualsieren
                    Data.Tables[Tablename].Rows.Clear();
                    m_UpdateObjects.Item[Tablename].Fill(Data.Tables[Tablename]);
                }
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'readTable\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);
        }
    
        // '' <summary>
        // '' Liest eine Tabelle neu �ber ein DataAdapter ein. Das DataAdapter-Objekt muss extern gehalten werden
        // '' </summary>
        // '' <param name="Data">(out) das Dataset, das die Tabelle enth�lt</param>
        // '' <param name="TableName">zu setzender/nutzender Tabellenname</param>
        // '' <param name="DataAdapter">zu nutzender Dataadapter</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableRead(string Tablename, ref System.Data.DataSet Data, ref DbDataAdapter DataAdapter) {
            string lCommandText = String.Empty;
            return TableRead(lCommandText, Tablename, Data, DataAdapter);
        }
    
        // '' <summary>
        // '' Liest eine (ggf. neu) Tabelle ein und legt Befehle zum automatsichen aktualisieren der Datens�tze an
        // '' falls noch nicht vorhanden. Das DataAdapter-Objekt muss extern gehalten werden
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="Data">(out) das Dataset, das die Tabelle enth�lt</param>
        // '' <param name="TableName">zu setzender/nutzender Tabellenname</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableRead(string CommandText, string Tablename, ref System.Data.DataSet Data, ref DbDataAdapter DataAdapter) {
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r readTable()");
            }
            try {
                if ((DataAdapter == null)) {
                    //  Daten das erste mal laden
                    if (string.IsNullOrEmpty(CommandText)) {
                        throw new Exception("kein SQL-Kommando angegeben");
                    }
                    DbCommand Command = new MySqlCommand();
                    DBCommandBuilder CommandBuilder = new MySqlCommandBuilder();
                    //  Sql-Kommando vorbereiten
                    Command.CommandText = CommandText;
                    Command.Connection = m_Connection;
                    if (m_Transaction != null) {
                        Command.Transaction = m_Transaction;
                    }
                    //  Datenadapter und Stringbuilder (f�r �nderungen an den Tabellen) vorbereiten
                    DataAdapter = new MySqlDataAdapter();
                    CommandBuilder.DataAdapter = DataAdapter;
                    DataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    DataAdapter.SelectCommand = Command;
                    //  Daten lesen und Tabelle f�llen
                    DataAdapter.Fill(Data, TableName);
                    TableRead = Data.Tables.Count;
                }
                else {
                    //  Daten wurden schon geladen, jetzt nur aktualsieren
                    //  ich weiss nicht, warum hier das l�schen drin war, denn dan iwrd immer die Tabelle neu geladen.
                    //  ich nehme es mal raus. Hoffentlich gibts es keine Nebeneffekte 24.05.2013
                    // Data.Tables(Tablename).Rows.Clear()
                    DataAdapter.Fill(Data.Tables[Tablename]);
                    //             epDBTools.showDataTable(Data.Tables(Tablename))
                }
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'readTable(2)\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);
        }
    
        // '' <summary>
        // '' Aktualsiert die Daten in der angebenen Datentabelle
        // '' </summary>
        // '' <param name="Tablename">Name der zu Tabelle mit den zu speichernden Daten</param>
        // '' <param name="Data">Dataset mit den Tabellen</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableUpdate(string Tablename, ref System.Data.DataSet Data) {
            return TableUpdate(Tablename, Data, DataViewRowState.None, false, null);
        }
    
        // '' <summary>
        // '' Aktualisiert die Daten in der angebenen Datentabelle
        // '' </summary>
        // '' <param name="Tablename">Name der zu Tabelle mit den zu speichernden Daten</param>
        // '' <param name="Data">Dataset mit den Tabellen</param>
        // '' <param name="removeTableReadObject">True: abschliessend wird das bereitgestellte Aktualisierungsobjekt f�r diese Tabelle gel�scht</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableUpdate(string Tablename, ref System.Data.DataSet Data, bool removeTableReadObject) {
            return TableUpdate(Tablename, Data, System.Data.DataViewRowState.None, removeTableReadObject, null);
        }
    
        // '' <summary>
        // '' Aktualisiert die Daten in der angebenen Datentabelle
        // '' </summary>
        // '' <param name="Tablename">Name der zu Tabelle mit den zu speichernden Daten</param>
        // '' <param name="Data">Dataset mit den Tabellen</param>
        // '' <param name="recordStates">Status der zu speichernden Datens�tze (or-Verkn�pfung erlaubt)</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableUpdate(string Tablename, ref System.Data.DataSet Data, System.Data.DataViewRowState recordStates) {
            return TableUpdate(Tablename, Data, recordStates, false, null);
        }
    
        // '' <summary>
        // '' Aktualisiert die Daten in der angebenen Datentabelle
        // '' </summary>
        // '' <param name="Tablename">Name der zu Tabelle mit den zu speichernden Daten</param>
        // '' <param name="Data">Dataset mit den Tabellen</param>
        // '' <param name="recordStates">Status der zu speichernden Datens�tze (or-Verkn�pfung erlaubt)</param>
        // '' <param name="removeTableReadObject">True: abschliessend wird das bereitgestellte Aktualisierungsobjekt f�r diese Tabelle gel�scht</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 TableUpdate(string Tablename, ref System.Data.DataSet Data, System.Data.DataViewRowState recordStates, bool removeTableReadObject) {
            return TableUpdate(Tablename, Data, recordStates, false, null);
        }
    
        // '' <summary>
        // '' Aktualisiert die Daten in der angebenen Datentabelle
        // '' </summary>
        // '' <param name="Tablename">Name der zu Tabelle mit den zu speichernden Daten</param>
        // '' <param name="Data">Dataset mit den Tabellen</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        // '' <param name="DataAdapter"></param>
        public Int32 TableUpdate(string Tablename, ref System.Data.DataSet Data, DbDataAdapter DataAdapter) {
            return TableUpdate(Tablename, Data, System.Data.DataViewRowState.None, false, DataAdapter);
        }
    
        // '' <summary>
        // '' Aktualisiert die Daten in der angebenen Datentabelle
        // '' </summary>
        // '' <param name="Tablename">Name der zu Tabelle mit den zu speichernden Daten</param>
        // '' <param name="Data">Dataset mit den Tabellen</param>
        // '' <param name="recordStates">Status der zu speichernden Datens�tze (or-Verkn�pfung erlaubt)</param>
        // '' <param name="removeTableReadObject">True: abschliessend wird das bereitgestellte Aktualisierungsobjekt f�r diese Tabelle gel�scht</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        // '' <param name="DataAdapter"></param>
        public Int32 TableUpdate(string Tablename, ref System.Data.DataSet Data, System.Data.DataViewRowState recordStates, bool removeTableReadObject, DbDataAdapter DataAdapter) {
            DbDataAdapter lDataAdapter;
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r TableUpdate()");
            }
            try {
                if (((DataAdapter == null) 
                            && !m_UpdateObjects.ContainsKey(Tablename))) {
                    //  die Tabelle ist nicht bekannt
                    throw new Exception(string.Format("Die Tabelle {0} ist nicht vorhanden", Tablename));
                }
                else {
                    //  DataAdapter w�hlen
                    if (DataAdapter) {
                        IsNot;
                        null;
                        lDataAdapter = DataAdapter;
                    }
                    else {
                        lDataAdapter = m_UpdateObjects.Item[Tablename];
                    }
                    //  Daten aktualisieren
                    if ((recordStates == DataViewRowState.None)) {
                        // alle Datens�tze aktualsieren
                        lDataAdapter.Update(Data, TableName);
                    }
                    else {
                        //  nur bestimmte Datens�tze aktualisieren
                        DataTable newTable = Data.Tables[Tablename].GetChanges(recordStates);
                        if (newTable) {
                            IsNot;
                            (null 
                                        && (newTable.Rows.Count > 0));
                            //  Tabellen-Kopie mit betreffenden Datens�tzen aktualisieren
                            lDataAdapter.Update(newTable);
                            //  �nderungen der betreffende Zeilen in der Ursprungstabelle akzeptieren
                            for (I = (Data.Tables[Tablename].Rows.Count - 1); (I <= 0); I = (I + -1)) {
                                if (((Data.Tables[Tablename].Rows[I].RowState && recordStates) 
                                            > 0)) {
                                    Data.Tables[Tablename].Rows[I].AcceptChanges();
                                }
                            }
                        }
                    }
                }
                if (((DataAdapter == null) 
                            && removeTableReadObject)) {
                    TableReadRemove(Tablename);
                }
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'TableUpdate\' f�r <" 
                                + (Tablename + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der betroffenen Datens�tze zur�ck
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="Data">(out) gibt ein Dataset zur�ck</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText, ref System.Data.DataTable Data) {
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 5, <" & CommandText & ">") 
            try {
                DbCommand Command = new MySqlCommand();
                DbDataAdapter DataAdapter = new MySqlDataAdapter();
                if ((Data == null)) {
                    Data = new System.Data.DataTable();
                }
                Command.CommandText = CommandText;
                Command.Connection = m_Connection;
                if (m_Transaction != null) {
                    Command.Transaction = m_Transaction;
                }
                DataAdapter.SelectCommand = Command;
                Data.Clear;
                DataAdapter.Fill(Data);
                Execute = Data.Rows.Count;
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteReader\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der betroffenen Datens�tze zur�ck
        // '' </summary>
        // '' <param name="Command">vorgeparstes SQL-Kommando</param>
        // '' <param name="Data">(out) gibt ein Dataset zur�ck</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(DBCommand Command, ref System.Data.DataTable Data) {
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 5, <" & CommandText & ">") 
            try {
                DbDataAdapter DataAdapter = new MySqlDataAdapter();
                if ((Data == null)) {
                    Data = new System.Data.DataTable();
                }
                DataAdapter.SelectCommand = Command;
                Data.Clear;
                DataAdapter.Fill(Data);
                Execute = Data.Rows.Count;
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteReader\', SQLString = <" 
                                + (Command.CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt die Anzahl der betroffenen Datens�tze zur�ck
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="Data">(out) gibt ein Dataset zur�ck</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText, ref System.Windows.Forms.BindingSource Data) {
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 6, <" & CommandText & ">") 
            try {
                DbCommand Command = new MySqlCommand();
                DbDataAdapter DataAdapter = new MySqlDataAdapter();
                System.Data.DataTable DataTable;
                DataTable = new System.Data.DataTable();
                Command.CommandText = CommandText;
                Command.Connection = m_Connection;
                if (m_Transaction != null) {
                    Command.Transaction = m_Transaction;
                }
                DataAdapter.SelectCommand = Command;
                DataTable.Clear;
                DataAdapter.Fill(DataTable);
                Data = new System.Windows.Forms.BindingSource(DataAdapter, null);
                Execute = DataTable.Rows.Count;
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteReader\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);
        }
    
        // '' <summary>
        // '' F�hrt einen SQL-Kommando aus und gibt einen Skalarwert zur�ck
        // '' </summary>
        // '' <param name="CommandText">auszuf�hrendes SQL-Kommando</param>
        // '' <param name="DataObject">(out) gibt ein Datum zur�ck</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public Int32 Execute(string CommandText, ref object DataObject) {
            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Execute()");
            }
            // LogFile.Write("Execute 7, <" & CommandText & ">") 
            try {
                DbCommand Command = new MySqlCommand();
                Command.CommandText = CommandText;
                Command.Connection = m_Connection;
                if (m_Transaction != null) 
                {
                    Command.Transaction = m_Transaction;
                }
                DataObject = Command.ExecuteScalar();
                Execute = ( (DataObject == null) ? 0 : 1 );
            }
            catch (Exception ex) {
                MonitorExit(this);
                throw new Exception(("Fehler bei \'ExecuteScalar\', SQLString = <" 
                                + (CommandText + (">" + ("(" 
                                + (m_ConfigData.Name + ")"))))), ex);
            }
            MonitorExit(this);
        }
    
        // '' <summary>
        // '' Erm�glicht Zugriff auf das Mysql-Verbindungsobjekt
        // '' </summary>
        // '' <value></value>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public DBConnector Connection {
            get {
                return m_Connection;
            }
        }
    
        public System.Data.ConnectionState State {
            get {
                return m_Connection.State;
            }
        }
    
        public static string SQLNow() {
            return SQLDateTime(DateTime.Now);
        }
    
        // '' <summary>
        // '' Gibt die aktuelle Systemzeit im MySQL-Format zur�ck
        // '' </summary>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public static string SQLToday() {
            return SQLDate(DateTime.Now);
        }
    
        // '' <summary>
        // '' Gibt den den �bergebenenen Datums-/Zeitwert im MySQL-Format zur�ck
        // '' </summary>
        // '' <param name="datDateTime">umzuwandelndes Datumsformat</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public static string SQLDateTimeOracle(DateTime datDateTime) {
            return string.Format("to_Date(\'{0:dd.MM.yyyy HH:mm:ss}\', \'DD.MM.YYYY HH24:MI:SS\')", datDateTime);
        }
    
        // '' <summary>
        // '' Gibt den den �bergebenenen Datums-/Zeitwert im MySQL-Format zur�ck
        // '' </summary>
        // '' <param name="datDateTime">umzuwandelndes Datumsformat</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public static string SQLDateTime(DateTime datDateTime) {
            return string.Format("{{ts \'{0:yyyy-MM-dd HH:mm:ss}\'}}", datDateTime);
        }
    
        // '' <summary>
        // '' Gibt den den �bergebenenen Datumswert im MySQL-Format zur�ck
        // '' </summary>
        // '' <param name="datDateTime">umzuwandelndes Datumsformat</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public static string SQLDate(DateTime datDateTime) {
            return string.Format("{{ts \'{0:yyyy-MM-dd}\'}}", datDateTime);
        }
    
        // '' <summary>
        // '' Gibt den den �bergebenenen Datumswert im MySQL-Format zur�ck
        // '' </summary>
        // '' <param name="datDateTime">umzuwandelndes Datumsformat</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public static string SQLDateOracle(DateTime datDateTime) {
            return string.Format("to_Date(\'{0:dd.MM.yyyy}\', \'DD.MM.YYYY\')", datDateTime);
        }
    
        // '' <summary>
        // '' Hilfsfunktion um Apostrophs f�r einen DBString anzuf�gen
        // '' </summary>
        // '' <param name="DBString">zu erweiterender DBString</param>
        // '' <returns>String mit angeh�ngten Apostrophs</returns>
        // '' <remarks></remarks>
        public static string SQLAString(string DBString) {
            // TODO: On Error Resume Next Warning!!!: The statement is not translatable 
            DBString = DBString.Replace("\'", "");
            return ("\'" 
                        + (DBString + "\'"));
        }
    
        // '' <summary>
        // '' Hilfsfunktion um ein Double in einen DB-konformen String zu wandeln
        // '' </summary>
        // '' <param name="DBDouble">zu wandelnder Double-Wert</param>
        // '' <returns>Double-String mit angeh�ngten Apostrophs</returns>
        // '' <remarks></remarks>
        public static string SQLDecimal(double DBDouble) {
            // TODO: On Error Resume Next Warning!!!: The statement is not translatable 
            return DBDouble.ToString("g", new Globalization.CultureInfo("en-US"));
        }
    
        // '' <summary>
        // '' Hilfsfunktion um ein Single in einen DB-konformen String zu wandeln
        // '' </summary>
        // '' <param name="DBFloat">zu wandelnder Single-Wert</param>
        // '' <returns>Single-String mit angeh�ngten Apostrophs</returns>
        // '' <remarks></remarks>
        public static string SQLDecimal(float DBFloat) {
            // TODO: On Error Resume Next Warning!!!: The statement is not translatable 
            return DBFloat.ToString("g", new Globalization.CultureInfo("en-US"));
        }
    
    
        // '' <summary>
        // '' Erzeugt aus einer bestimmten Spalte eine DataTable eine Liste vom Typ 'T'
        // '' </summary>
        // '' <typeparam name="T">Typ der Liste</typeparam>
        // '' <param name="Table">Datentabelle</param>
        // '' <param name="Field">Name des Feldes, dessen Daten in die Liste �berf�hrt werden sollen</param>
        // '' <returns>Liste vom Typ 'T' mit den Inhalten der benannten Spalte</returns>
        // '' <remarks></remarks>
        public static void makeListFromtable(void Of, void T) {
            ((DataTable)(Table));
            ((string)(Field));
            List(Of, T);
            List newList = new List(Of, T);
            try {
                foreach (Zeile in Table.AsEnumerable) {
                    newList.Add(Zeile.Item[Field]);
                }
                return newList;
            }
            catch (Exception ex) {
                throw new Exception("Fehler beim Erzeugen einer Liste aus einer Tabelle", ex);
            }
        }
    
        // '' <summary>
        // '' Erzeugt eine OR-Verkn�pfung mit den Inhalten der Liste
        // '' </summary>
        // '' <typeparam name="T">Typ der Spalte (Int32 oder String)</typeparam>
        // '' <param name="Field">Name der Spalte</param>
        // '' <param name="Condition">Vergleichsoperand</param>
        // '' <returns>"oder"-verkn�pfte Liste der Werte in Klammern</returns>
        // '' <remarks></remarks>
        public static void getOrString(void Of, void T) {
            ((string)(Field));
            ((string)(Condition));
            ((DataTable)(Table));
            ((string)(TableFieldName));
            String;
            List[] Parts;
            Of;
            T;
            try {
                Parts = makeListFromtable(Of, T)[Table, TableFieldName];
                return getOrString(Of, T)[Field, Condition, Parts];
            }
            catch (Exception ex) {
                throw new Exception("Fehler beim Erzeugen eines Or-String aus eine Spalte einer Tabelle", ex);
            }
        }
    
   
        // '' <summary>
        // '' Erzeugt eine OR-Verkn�pfung mit den Inhalten der Liste
        // '' </summary>
        // '' <typeparam name="T">Typ der Spalte (Int32 oder String)</typeparam>
        // '' <param name="Field">Name der Spalte</param>
        // '' <param name="Condition">Vergleichsoperand</param>
        // '' <param name="Parts">Liste mit Vergleichswerten</param>
        // '' <returns>"oder"-verkn�pfte Liste der Werte in Klammern</returns>
        // '' <remarks></remarks>
        public static void getOrString(void Of, void T) {
            ((string)(Field));
            ((string)(Condition));
            ((List[])(Parts));
            Of;
            T;
            String;
            System.Text.StringBuilder SB;
            try {
                SB = new System.Text.StringBuilder();
                foreach (T Entry in Parts) {
                    if ((SB.Length > 0)) {
                        SB.Append(" Or ");
                    }
                    switch (typeof(T).Name) {
                        case "Int32":
                            SB.AppendFormat("{0} {1} {2}", Field, Condition, Entry);
                            break;
                        default:
                            SB.AppendFormat("{0} {1} {2}", Field, Condition, epDBManager.SQLAString(Entry.ToString));
                            break;
                    }
                }
                if ((SB.Length > 0)) {
                    return (" (" 
                                + (SB.ToString + ") "));
                }
                else {
                    return String.Empty;
                }
            }
            catch (Exception ex) {
                throw new Exception("Fehler beim Erzeugen des Or-Strings", ex);
            }
        }
    
        // '' <summary>
        // '' Startet eine Transaktion, bzw. erh�ht den Transaktionz�hler
        // '' </summary>
        // '' <param name="maxTransactionTime">nach Ablauf der Zeit wird ein Rollback ausgef�hrt und eine Exception geworfen</param>
        // '' <returns></returns>
        // '' <remarks></remarks>
        internal Int32 TransBegin(double maxTransactionTime, string ProcedureName, void =, void ) {
            try {
                // LogFile.Write("Einsprung Transbegin aus <" & ProcedureName & ">, Transstring <" & m_TransActString & ">")         
                // Warning!!! Optional parameters not supported
                //  threadsicherheit durch Monitor erzeugen
                if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                    throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Transbegin(Double)");
                }
                if (m_RollBackPending) {
                    // LogFile.Write("TransBegin 1a")       
                    //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                    MonitorPulse(this);
                    MonitorExit(this);
                    throw new Exception("Aufruf von BeginTrans trotz laufendem Rollback");
                }
                else if ((m_Transcount == 0)) {
                    // LogFile.Write("TransBegin 2a")       
                    TransStringAdd(ProcedureName);
                    // LogFile.Write("Starte Transaktion ")
                    m_Transaction = m_Connection.BeginTransaction();
                    m_TransStartTime = DateTime.Now;
                    if ((m_maxTransactionTime < maxTransactionTime)) {
                        m_maxTransactionTime = DEFAULT_MAX_TIME;
                    }
                    m_Watchdog_Trans.Start();
                    m_Transcount = 1;
                }
                else {
                    // LogFile.Write("TransBegin 3a")       
                    TransStringAdd(ProcedureName);
                    m_Transcount++;
                }
                // LogFile.Write("Aussprung Transbegin aus <" & ProcedureName & ">, Transstring <" & m_TransActString & ">")         
            }
            catch (Exception ex) {
                // LogFile.Write("TransBegin 4a")       
                try {
                    //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                    MonitorPulse(this);
                    MonitorExit(this);
                }
                catch (System.Exception End) {
                    try {
                        throw new Exception("Fehler beim Transaktions-Start", ex);
                    }
                }
                // '' <summary>
                // '' Startet eine Transaktion, bzw. erh�ht den Transaktionz�hler
                // '' </summary>
                // '' <returns></returns>
                // '' <remarks></remarks>
                ((Int32)(TransBegin(Optional, ProcedureNameAsString=)));
                try {
                    // LogFile.Write("Einsprung Transbegin aus <" & ProcedureName & ">, Transstring <" & m_TransActString & ">")         
                    //  threadsicherheit durch Monitor erzeugen
                    if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                        throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Transbegin()");
                    }
                    if (m_RollBackPending) {
                        // LogFile.Write("TransBegin 1")       
                        //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                        MonitorPulse(this);
                        MonitorExit(this);
                        throw new Exception("Aufruf von BeginTrans trotz laufendem Rollback");
                    }
                    else if ((m_Transcount == 0)) {
                        // LogFile.Write("TransBegin 2")       
                        TransStringAdd(ProcedureName);
                        m_Transaction = m_Connection.BeginTransaction();
                        m_TransStartTime = DateTime.Now;
                        m_Watchdog_Trans.Start();
                        m_Transcount = 1;
                    }
                    else {
                        // LogFile.Write("TransBegin 3")       
                        TransStringAdd(ProcedureName);
                        m_Transcount++;
                    }
                    // LogFile.Write("Aussprung Transbegin aus <" & ProcedureName & ">, Transstring <" & m_TransActString & ">")         
                }
                catch (Exception ex) {
                    // LogFile.Write("TransBegin 4") 
                    try {
                        //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                        MonitorPulse(this);
                        MonitorExit(this);
                    }
                    catch (System.Exception End) {
                        try {
                            throw new Exception("Fehler beim Transaktions-Start", ex);
                        }
                    }
                    // '' <summary>
                    // '' Best�tigt eine Transaktion und setzt den Transaktionz�hler zur�ck
                    // '' </summary>
                    // '' <returns></returns>
                    // '' <remarks></remarks>
                    ((Int32)(TransCommit()));
                    try {
                        // LogFile.Write("Einsprung TransCommit, Transstring <" & m_TransActString & ">"       )
                        //  threadsicherheit durch Monitor erzeugen
                        if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                            throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Transbegin()");
                        }
                        if (m_RollBackPending) {
                            //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                            // LogFile.Write("TransCommit 1")       
                            MonitorPulse(this);
                            MonitorExit(this);
                            throw new Exception("Aufruf von CommitTrans trotz laufendem Rollback");
                        }
                        else if ((m_Transcount == 1)) {
                            // LogFile.Write("TransCommit 2")       
                            TransStringRemove();
                            m_Watchdog_Trans.Stop();
                            m_Transaction.Commit();
                            m_Transcount = 0;
                            m_Transaction = null;
                            m_maxTransactionTime = 0;
                            //  eine Ebene im Monitor wieder entfernen
                            MonitorExit(this);
                        }
                        else if ((m_Transcount > 1)) {
                            // LogFile.Write("TransCommit 3")       
                            //  eine Ebene im Monitor wieder entfernen
                            TransStringRemove();
                            m_Transcount--;
                            MonitorExit(this);
                        }
                        else {
                            //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                            // LogFile.Write("TransCommit 4")       
                            TransStringRemove();
                            MonitorPulse(this);
                            MonitorExit(this);
                            throw new Exception("Keine Transaktion aktiv");
                        }
                        //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                        MonitorPulse(this);
                        MonitorExit(this);
                        // LogFile.Write("Aussprung TransCommit, Transstring <" & m_TransActString & ">")         
                        return m_Transcount;
                    }
                    catch (Exception ex) {
                        // LogFile.Write("TransCommit 5")       
                        try {
                            //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                            MonitorPulse(this);
                            MonitorExit(this);
                        }
                        catch (System.Exception End) {
                            try {
                                throw new Exception("Fehler beim Transaktions-Commit", ex);
                            }
                        }
                        // '' <summary>
                        // '' Verwirft die �nderungen innerhalb der Transaktion
                        // '' </summary>
                        // '' <returns></returns>
                        // '' <remarks></remarks>
                        ((Int32)(TransRollback()));
                        try {
                            // LogFile.Write("Einsprung TransRollback, Transstring <" & m_TransActString & ">")         
                            //  threadsicherheit durch Monitor erzeugen
                            if (!MonitorTryEnter(this, m_ConfigData.TimeOut)) {
                                throw new Exception("Zeit�berschreitung beim warten auf Monitor-Sperre f�r Transbegin()");
                            }
                            if ((m_Transcount == 1)) {
                                // LogFile.Write("TransRollback 1")       
                                m_Watchdog_Trans.Stop();
                                if (!(m_Transaction == null)) {
                                    m_Transaction.Rollback();
                                }
                                m_Transcount = 0;
                                m_Transaction = null;
                                m_RollBackPending = false;
                                m_maxTransactionTime = 0;
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
                            //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                            MonitorPulse(this);
                            MonitorExit(this);
                            // LogFile.Write("Aussprung TransRollback, Transstring <" & m_TransActString & ">")         
                            return m_Transcount;
                        }
                        catch (Exception ex) {
                            // LogFile.Write("TransRollback 3")       
                            try {
                                //  diese Ebene im Monitor wieder entfernen und den anderen Bescheid sagen
                                MonitorPulse(this);
                                MonitorExit(this);
                            }
                            catch (System.Exception End) {
                                try {
                                    throw new Exception("Fehler beim Transaktions-Commit", ex);
                                }
                            }
    // '' <summary>
    // '' Timerroutine f�r Transaktions-Zeit�berschreitungs-�berwachung
    // '' </summary>
    // '' <param name="sender"></param>
    // '' <param name="e"></param>
    // '' <remarks></remarks>
    Watchdog_Trans_Elapsed(((object)(sender)), ((System.Timers.ElapsedEventArgs)(e)));
    double tempTime;
    ((epPerformanceTimer)(AliveTimer));
    try {
        m_Watchdog_Trans.Stop();
        if (m_ConfigData.getValue(Of, Boolean)["StayAlive) {
            if ((AliveTimer == null)) {
                AliveTimer = new epPerformanceTimer();
                AliveTimer.startMeasuring();
            }
            if ((AliveTimer.currentMeasuring >= (10 * (60 * 1000)))) {
                DataTable Data = new DataTable();
                this.Execute("show status like \'Uptime\'", Data);
                Data.Dispose();
                AliveTimer.startMeasuring();
            }
        }
        if (!(m_Transaction == null)) {
            if (((m_maxTransactionTime > 0) 
                        && (DateTime.Now.Subtract(m_TransStartTime).Seconds > m_maxTransactionTime))) {
                m_Transaction.Rollback();
                m_Transaction = null;
                m_RollBackPending = true;
                tempTime = m_maxTransactionTime;
                m_maxTransactionTime = 0;
                throw new Exception(("Transaktionsrollback wurde durch Zeit�berschreitung (t_max=" 
                                + (tempTime.ToString("0.0") + "s) initiiert.")));
            }
            else {
                m_Watchdog_Trans.Start();
            }
        }
    }
    catch (Exception ex) {
        throw new Exception("Fehler bei der Transaktions-Zeit�berwachung", ex);
    }
}
                        // '' <summary>
                        // '' Timerroutine f�r Alive-Signalsteuerung
                        // '' </summary>
                        // '' <param name="sender"></param>
                        // '' <param name="e"></param>
                        // '' <remarks></remarks>
                        Watchdog_Alive_Elapsed(((object)(sender)), ((System.Timers.ElapsedEventArgs)(e)));
                        try {
                            DataTable Data = new DataTable();
                            this.Execute("show status like \'Uptime\'", Data);
                            Data.Dispose();
                        }
                        catch (Exception ex) {
                            throw new Exception(string.Format("Fehler bei der Aktiverhaltung der Verbindung {0} <{1}>", "\r\n", this.Connection.ConnectionString), ex);
                        }
                    }
                    // '' <summary>
                    // '' F�gt einen neuen Teil zum Protokollierungsstring hinzu
                    // '' </summary>
                    // '' <param name="Name"></param>
                    // '' <remarks></remarks>
                    TransStringAdd(((string)(Name)));
                    if ((string.IsNullOrEmpty(m_TransActString) 
                                || (m_TransActString.Trim.Length > 0))) {
                        m_TransActString = string.Format("{0}({1})", Name, m_Transcount);
                    }
                    else {
                        m_TransActString = string.Format("{0},{1}({2})", m_TransActString, Name, m_Transcount);
                    }
                    // LogFile.Write("Transaktionsstart : " & m_TransActString)
                }
                // '' <summary>
                // '' Entfernt einen Teil vom Protokollierungsstring
                // '' </summary>
                // '' <remarks></remarks>
                TransStringRemove();
                // LogFile.Write("Transaktionsende : " & m_TransActString)
                if ((string.IsNullOrEmpty(m_TransActString) 
                            || (m_TransActString.Trim.Length > 0))) {
                    m_TransActString = String.Empty;
                }
                else if ((m_TransActString.LastIndexOf(",") > -1)) {
                    m_TransActString = m_TransActString.Substring(0, (m_TransActString.LastIndexOf(",") - 1));
                }
                else {
                    m_TransActString = String.Empty;
                }
            }
            // '' <summary>
            // '' �ffnet einen Mutex innerhalb einer Transaktion, um Datenbankzugriffen bei
            // '' anwendungs�bergreifenden Threads deadlock-sicher zu synchronisieren
            // '' Mutex bleibt dann bis zu einem "Commit" oder einem "Rollback" aktiv
            // '' </summary>
            // '' <param name="Name">Name des Mutex</param>
            // '' <remarks></remarks>
        }
    
        void openTransaktionsmutex(string Name) {
            string sqlString;
            string sqlInsert;
            DataTable Data = null;
            bool insertDone;
            try {
                if (!TransActive) {
                    throw new Exception("Keine laufende Transaktion vorhanden");
                }
                else {
                    insertDone = false;
                    for (
                    ; (Data.Rows.Count == 0); 
                    ) {
                        sqlString = ("select InitValue from tbInitValue" + (" where InitGroup = " 
                                    + (SQLAString("TransLock") + (" and   InitKey   = " 
                                    + (SQLAString(Name) + " for update")))));
                        if ((Execute(sqlString, Data) == 0)) {
                            if (!insertDone) {
                                sqlInsert = ("insert ignore into tbInitValue(InitGroup, InitKey, InitValue) Values (" 
                                            + (SQLAString("TransLock") + (", " 
                                            + (SQLAString(Name) + (", " + "0)")))));
                                Execute(sqlInsert);
                                insertDone = true;
                            }
                            else {
                                throw new Exception("Transaktionsmutex konnte erzeugt werden");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Fehler beim Starten des Transaktions-Locks", ex);
            }
        }
    
        private bool MonitorTryEnter(object Target, Int32 msTimeOut) 
        {
            bool retValue;
            // LogFileActivity.Write(Me.ObjectName & ".Monitor.TryEnter() .. aus Thread " & Thread.CurrentThread.ManagedThreadId & vbNewLine & Environment.StackTrace())      
            retValue = Monitor.TryEnter(Target, msTimeOut);
            if (retValue) {
                // LogFileActivity.Write(Me.ObjectName & ".Monitor.TryEnter() ok aus Thread " & Thread.CurrentThread.ManagedThreadId)
            }
            else {
                // LogFileActivity.Write(Me.ObjectName & ".Monitor.TryEnter() ERROR aus Thread " & Thread.CurrentThread.ManagedThreadId)
            }
            return retValue;
        }
    
        private void MonitorExit(object Target) 
        {
            // LogFileActivity.Write(Me.ObjectName & ".Monitor.Exit() .. aus Thread " & Thread.CurrentThread.ManagedThreadId)      
            Monitor.Exit(Target);
            //       LogFileActivity.Write(Me.ObjectName & ".Monitor.Exit() .. aus Thread " & Thread.CurrentThread.ManagedThreadId)      
        }
    
        private void MonitorPulse(object Target) 
        {
            // LogFileActivity.Write(Me.ObjectName & ".Monitor.MonitorPulse() .. aus Thread " & Thread.CurrentThread.ManagedThreadId)      
            Monitor.Pulse(Target);
            //       LogFileActivity.Write(Me.ObjectName & ".Monitor.MonitorPulse() ok aus Thread " & Thread.CurrentThread.ManagedThreadId)      
        }
    }

#endif

}