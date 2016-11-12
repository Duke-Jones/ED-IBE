using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using IBE.SQL;
using System.Diagnostics;

namespace IBE.Enums_and_Utility_Classes
{
    public interface IDataPageRetriever
    {
        DataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage);
    }

    /// <summary>
    /// retriever class
    /// </summary>
    public class DataRetriever : IDataPageRetriever
    {
        private string                      m_BaseTableName;
        private string                      m_ColumnStatement;
        private string                      m_BaseStatement;
        private MySqlCommand                m_Command;
        private DataRetrieverCache          m_MemoryCache;
        private List<String>                m_PrimaryKey;
        private string                      m_ColumnToSortBy;
        private DBConnector.SQLSortOrder    m_ColumnSortOrder;
        private MySqlDataAdapter            m_Adapter = new MySqlDataAdapter();
        private string                      m_CommaSeparatedListOfColumnNamesValue = null;
        private String                      m_UsedPrefix = null;
        private int                         m_RowCountValue = -1;
        private DataColumnCollection        m_ColumnsValue;
        private DataTable                   m_TableType = null;
        private SQL.DBConnector             m_DBCon;
        private PerformanceTimer            m_Pt = new PerformanceTimer();
        private Int32                       m_rowCountCache = 0;
        internal readonly Dictionary<String, String> Filter = new Dictionary<string, string>();
        private System.Windows.Forms.BindingNavigator m_BindingNavigator = null;


        /// <summary>
        /// constructor for the DataRetriever (used for loading and caching data in DGV VirtualMode)
        /// </summary>
        /// <param name="DBCon">used DBConnector</param>
        /// <param name="m_BaseTableName">name of the table with the base structure</param>
        /// <param name="m_DataStatement">sql-statement for loading the data</param>
        /// <param name="SortByColumn">column for sorting (must be existingClassification in the base table (m_BaseTableName) and in the 'DataStement')</param>
        /// <param name="SortOrder">sort oder</param>
        /// <param name="SortOrder">optional blueprint for typed tables</param>
        public DataRetriever(SQL.DBConnector DBCon, string BaseTableName, string columnStatement, String baseStatement, String SortByColumn, DBConnector.SQLSortOrder SortOrder, System.Windows.Forms.BindingNavigator bindingNavigator, DataTable TypeTable = null)
        {
            m_Command                = ((MySqlConnection)DBCon.Connection).CreateCommand();
            m_BaseTableName          = BaseTableName;
            m_ColumnStatement        = columnStatement;
            m_BaseStatement          = baseStatement;
            m_ColumnToSortBy         = SortByColumn;
            m_ColumnSortOrder        = SortOrder;
            m_PrimaryKey             = DBCon.getPrimaryKey(this.m_BaseTableName);
            m_TableType              = TypeTable;
            m_DBCon                  = DBCon;
            m_BindingNavigator       = bindingNavigator;

            if(this.m_PrimaryKey.Count != 1)
                throw new Exception("Length of primary key is not '1' (table '" + BaseTableName + "')");

            m_MemoryCache                 = new DataRetrieverCache(this, 50);
        }

        public DataRetrieverCache MemoryCache
        {
            get
            {
                return m_MemoryCache;
            }
        }

        private DataTable GetDataTable()
        {
            DataTable table;

            if (m_TableType == null)
                table = new DataTable();
            else
                table = m_TableType.Clone();

            return table;
        }

        public DataColumnCollection Columns
        {
            get
            {
                // Return the existingClassification value if it has already been determined.
                if (m_ColumnsValue != null)
                {
                    return m_ColumnsValue;
                }

                // Retrieve the column information from the database.

                m_Command.CommandText       = m_ColumnStatement + m_BaseStatement + GetWhereStatement();
                MySqlDataAdapter adapter    = new MySqlDataAdapter();
                adapter.SelectCommand       = m_Command;
                DataTable table             = GetDataTable();
                table.Locale                = System.Globalization.CultureInfo.InvariantCulture;

                adapter.FillSchema(table, SchemaType.Source);

                m_ColumnsValue                = table.Columns;

                return m_ColumnsValue;
            }
        }

        public string BaseStatement
        {
            get
            {
                return m_BaseStatement;
            }

            set
            {
                m_BaseStatement = value;
            }
        }


        public String GetWhereStatement(string ignoreFilter = "")
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<String, String> definedFilter in Filter)
            {
                if(definedFilter.Key != ignoreFilter)
                {
                    if(sb.Length > 0)
                        sb.Append(" and ");
                    sb.Append("(" + definedFilter.Value + ")");
                }
            }

            if(sb.Length > 0)
                return " where (" + sb.ToString() + ")";
            else
                return "";
        }

        public string ColumnStatement
        {
            get
            {
                return m_ColumnStatement;
            }

            set
            {
                m_ColumnStatement = value;
            }
        }

        public DataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage)
        {
            Debug.Print("retrieve Page " + lowerPageBoundary + " (" + rowsPerPage + ")");

            // Retrieve the specified number of rows from the database, starting
            // with the row specified by the lowerPageBoundary parameter.
            String sqlString    = String.Format("select * from ({0}) L1 left join (" +
                                                "                         select {6} from ({0}) L3 order by L3.{3} {5} limit {4}" +
                                                "                       ) L2 on L1.{6} = L2.{6}" +
                                                " where L2.{6} is null order by L1.{3} {5} limit {1}", 
                                                m_ColumnStatement + m_BaseStatement + GetWhereStatement(), 
                                                rowsPerPage, 
                                                m_BaseTableName, 
                                                m_ColumnToSortBy, 
                                                lowerPageBoundary, 
                                                m_ColumnSortOrder, 
                                                m_PrimaryKey[0]);



            DataTable table         = GetDataTable();

            m_DBCon.Execute(sqlString, table);

            return table;
        }

        public int RowCount(Boolean refresh = false)
        {
            Object result = -1;
            if ((m_Pt.currentMeasuring() >= 60000) || (!m_Pt.isStarted) || refresh)
            {
                // Retrieve the row count from the database.

                m_Command.CommandText = "select count(*) " + m_BaseStatement + GetWhereStatement(); 

                try
                {
                    result = m_Command.ExecuteScalar();

                    if (result != null)
                    {
                        m_rowCountCache = Convert.ToInt32(result);
                        m_BindingNavigator.CountItem.Text = String.Format(m_BindingNavigator.CountItemFormat, m_rowCountCache);
                        m_Pt.startMeasuring();
                    }
                }
                catch (Exception ex)
                {
                    // ignore this, sometimes this happens
                    Debug.Print("doh");
                }
            }

            return m_rowCountCache;
        }

        private string CommaSeparatedListOfColumnNames(String Prefix = "")
        {
            // Return the existingClassification value if it has already been determined.
            if ((m_CommaSeparatedListOfColumnNamesValue != null) && (Prefix == m_UsedPrefix))
            {
                return m_CommaSeparatedListOfColumnNamesValue;
            }

            // Store a list of column names for use in the
            // SupplyPageOfData method.
            System.Text.StringBuilder commaSeparatedColumnNames =
                new System.Text.StringBuilder();
            bool firstColumn = true;
            foreach (DataColumn column in Columns)
            {
                if (!firstColumn)
                {
                    commaSeparatedColumnNames.Append(", ");
                }
                if (!String.IsNullOrEmpty(Prefix))
                    commaSeparatedColumnNames.Append(Prefix);

                commaSeparatedColumnNames.Append(column.ColumnName);
                firstColumn = false;
            }

            m_CommaSeparatedListOfColumnNamesValue    = commaSeparatedColumnNames.ToString();
            m_UsedPrefix                              = Prefix;

            return m_CommaSeparatedListOfColumnNamesValue;
        }

        internal void SetFilter(string name, string filterString)
        {
            if(String.IsNullOrWhiteSpace(filterString))
            {
                if(Filter.ContainsKey(name))
                    Filter.Remove(name);
            }
            else
            {
                if(Filter.ContainsKey(name))
                    Filter[name] = filterString;
                else
                    Filter.Add(name, filterString);
            }
        }
    }

    /// <summary>
    /// caching class
    /// </summary>
    public class DataRetrieverCache
    {
        private static int RowsPerPage;

        // Represents one page of data.  
        public struct DataPage
        {
            public DataTable table;
            private int lowestIndexValue;
            private int highestIndexValue;

            public DataPage(DataTable table, int rowIndex)
            {
                this.table = table;
                lowestIndexValue = MapToLowerBoundary(rowIndex);
                highestIndexValue = MapToUpperBoundary(rowIndex);
                System.Diagnostics.Debug.Assert(lowestIndexValue >= 0);
                System.Diagnostics.Debug.Assert(highestIndexValue >= 0);
            }

            public void ClearPage()
            {
                lowestIndexValue  = -1;
                highestIndexValue = -1;
            }

            public int LowestIndex
            {
                get
                {
                    return lowestIndexValue;
                }
            }

            public int HighestIndex
            {
                get
                {
                    return highestIndexValue;
                }
            }

            public static int MapToLowerBoundary(int rowIndex)
            {
                // Return the lowest index of a page containing the given index.
                return (rowIndex / RowsPerPage) * RowsPerPage;
            }

            private static int MapToUpperBoundary(int rowIndex)
            {
                // Return the highest index of a page containing the given index.
                return MapToLowerBoundary(rowIndex) + RowsPerPage - 1;
            }
        }

        private DataPage[]          cachePages;
        private IDataPageRetriever  dataSupply;

        public DataRetrieverCache(IDataPageRetriever dataSupplier, int rowsPerPage)
        {
            dataSupply = dataSupplier;
            DataRetrieverCache.RowsPerPage = rowsPerPage;
            LoadFirstTwoPages();
        }

        // Sets the value of the element parameter if the value is in the cache.
        private bool IfPageCached_ThenSetElement(int rowIndex, int columnIndex, ref object element)
        {
            if (IsRowCachedInPage(0, rowIndex))
            {
                element = cachePages[0].table.Rows[rowIndex % RowsPerPage][columnIndex];
                return true;
            }
            else if (IsRowCachedInPage(1, rowIndex))
            {
                element = cachePages[1].table.Rows[rowIndex % RowsPerPage][columnIndex];
                return true;
            }

            return false;
        }

        // Sets the value of the element parameter if the value is in the cache.
        private bool IfPageCached_ThenSetElement(int rowIndex, ref DataRow element)
        {
            if (IsRowCachedInPage(0, rowIndex))
            {
                if (cachePages[0].table.Rows.Count >= (rowIndex % RowsPerPage + 1))
                {
                    element = cachePages[0].table.Rows[rowIndex % RowsPerPage];
                    return true;
                }
            }
            else if (IsRowCachedInPage(1, rowIndex))
            {
                if (cachePages[1].table.Rows.Count >= (rowIndex % RowsPerPage + 1))
                {
                    element = cachePages[1].table.Rows[rowIndex % RowsPerPage];
                    return true;
                }
            }

            return false;
        }

        // Sets the value of the element parameter if the value is in the cache.
        public void SetElementToPage(int rowIndex, int columnIndex, object element)
        {
            if (IsRowCachedInPage(0, rowIndex))
            {
                cachePages[0].table.Rows[rowIndex % RowsPerPage][columnIndex] = element == null ? DBNull.Value : element;
            }
            else if (IsRowCachedInPage(1, rowIndex))
            {
                cachePages[1].table.Rows[rowIndex % RowsPerPage][columnIndex] = element == null ? DBNull.Value : element;;
            }
        }

        public object RetrieveElement(int rowIndex, int columnIndex)
        {
            object element = null;

            if (IfPageCached_ThenSetElement(rowIndex, columnIndex, ref element))
            {
                return element;
            }
            else
            {
                return RetrieveData_CacheIt_ThenReturnElement(rowIndex, columnIndex);
            }
        }

        public void PushElement(int rowIndex, int columnIndex, object element)
        {

            SetElementToPage(rowIndex, columnIndex, element);

        }

        private void LoadFirstTwoPages()
        {
            cachePages = new DataPage[]{
                new DataPage(dataSupply.SupplyPageOfData(DataPage.MapToLowerBoundary(          0), RowsPerPage),           0), 
                new DataPage(dataSupply.SupplyPageOfData(DataPage.MapToLowerBoundary(RowsPerPage), RowsPerPage), RowsPerPage)};
        }

        private object RetrieveData_CacheIt_ThenReturnElement(int rowIndex, int columnIndex)
        {
            // Retrieve a page worth of data containing the requested value.
            DataTable table = dataSupply.SupplyPageOfData(DataPage.MapToLowerBoundary(rowIndex), RowsPerPage);

            // Replace the cached page furthest from the requested cell
            // with a new page containing the newly retrieved data.
            cachePages[GetIndexToUnusedPage(rowIndex)] = new DataPage(table, rowIndex);

            return RetrieveElement(rowIndex, columnIndex);
        }

        // Returns the index of the cached page most distant from the given index
        // and therefore least likely to be reused.
        private int GetIndexToUnusedPage(int rowIndex)
        {
            if (rowIndex > cachePages[0].HighestIndex &&
                rowIndex > cachePages[1].HighestIndex)
            {
                int offsetFromPage0 = rowIndex - cachePages[0].HighestIndex;
                int offsetFromPage1 = rowIndex - cachePages[1].HighestIndex;
                if (offsetFromPage0 < offsetFromPage1)
                {
                    return 1;
                }
                return 0;
            }
            else
            {
                int offsetFromPage0 = cachePages[0].LowestIndex - rowIndex;
                int offsetFromPage1 = cachePages[1].LowestIndex - rowIndex;
                if (offsetFromPage0 < offsetFromPage1)
                {
                    return 1;
                }
                return 0;
            }

        }

        // Returns a value indicating whether the given row index is contained
        // in the given DataPage. 
        private bool IsRowCachedInPage(int pageNumber, int rowIndex)
        {
            return rowIndex <= cachePages[pageNumber].HighestIndex &&
                   rowIndex >= cachePages[pageNumber].LowestIndex;
        }


        public DataRow RetrieveDataColumn(int rowIndex)
        {
            DataRow element = null;

            IfPageCached_ThenSetElement(rowIndex, ref element);

            return element;
        }

        internal void Clear()
        {
            cachePages[0].ClearPage();
            cachePages[1].ClearPage();
        }
    }
}
