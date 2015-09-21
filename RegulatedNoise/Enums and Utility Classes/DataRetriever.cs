using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using RegulatedNoise.SQL;
using System.Diagnostics;

namespace RegulatedNoise.Enums_and_Utility_Classes
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
        public enum SQLSortOrder
        {
            asc,
            desc
        }

        private string                m_BaseTableName;
        private string                m_DataStatement;
        private MySqlCommand          m_Command;
        private DataRetrieverCache    m_MemoryCache;
        private List<String>          m_PrimaryKey;
        private string                m_ColumnToSortBy;
        private SQLSortOrder          m_ColumnSortOrder;
        private MySqlDataAdapter      m_Adapter = new MySqlDataAdapter();
        private string                m_CommaSeparatedListOfColumnNamesValue = null;
        private String                m_UsedPrefix = null;
        private int                   m_RowCountValue = -1;
        private DataColumnCollection  m_ColumnsValue;
        private DataTable             m_TableType = null;

        /// <summary>
        /// constructor for the DataRetriever (used for loading and caching data in DGV VirtualMode)
        /// </summary>
        /// <param name="DBCon">used DBConnector</param>
        /// <param name="m_BaseTableName">name of the table with the base structure</param>
        /// <param name="m_DataStatement">sql-statement for loading the data</param>
        /// <param name="SortByColumn">column for sorting (must be existing in the base table (m_BaseTableName) and in the 'DataStement')</param>
        /// <param name="SortOrder">sort oder</param>
        /// <param name="SortOrder">optional blueprint for typed tables</param>
        public DataRetriever(SQL.DBConnector DBCon, string BaseTableName, String DataStatement, String SortByColumn, SQLSortOrder SortOrder, DataTable TypeTable = null)
        {
            m_Command                = ((MySqlConnection)DBCon.Connection).CreateCommand();
            m_BaseTableName          = BaseTableName;
            m_DataStatement          = DataStatement;
            m_ColumnToSortBy         = SortByColumn;
            m_ColumnSortOrder        = SortOrder;
            m_PrimaryKey             = DBCon.getPrimaryKey(this.m_BaseTableName);
            m_TableType              = TypeTable;

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
                // Return the existing value if it has already been determined.
                if (m_ColumnsValue != null)
                {
                    return m_ColumnsValue;
                }

                // Retrieve the column information from the database.
                m_Command.CommandText       = m_DataStatement;
                MySqlDataAdapter adapter    = new MySqlDataAdapter();
                adapter.SelectCommand       = m_Command;
                DataTable table             = GetDataTable();
                table.Locale                = System.Globalization.CultureInfo.InvariantCulture;

                adapter.FillSchema(table, SchemaType.Source);

                m_ColumnsValue                = table.Columns;

                return m_ColumnsValue;
            }
        }

        public DataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage)
        {
            Debug.Print("retrieve Page " + lowerPageBoundary + " (" + rowsPerPage + ")");

            // Retrieve the specified number of rows from the database, starting
            // with the row specified by the lowerPageBoundary parameter.
            m_Command.CommandText = String.Format("select * from ({0}) L1 left join (" +
                                                "                         select {6} from ({0}) L3 order by L3.{3} {5} limit {4}" +
                                                "                       ) L2 on L1.{6} = L2.{6}" +
                                                " where L2.{6} is null order by L1.{3} {5} limit {1}", 
                                                m_DataStatement, 
                                                rowsPerPage, 
                                                m_BaseTableName, 
                                                m_ColumnToSortBy, 
                                                lowerPageBoundary, 
                                                m_ColumnSortOrder, 
                                                m_PrimaryKey[0]);

            m_Adapter.SelectCommand = m_Command;
            DataTable table         = GetDataTable();
            m_Adapter.Fill(table);
            return table;
        }

        public int RowCount
        {
            get
            {
                // Return the existing value if it has already been determined.
                if (m_RowCountValue != -1)
                {
                    return m_RowCountValue;
                }

                Object result = -1;
                // Retrieve the row count from the database.
                m_Command.CommandText = "SELECT COUNT(*) FROM " + m_BaseTableName;
                result = m_Command.ExecuteScalar();
                if (result != null)
                    m_RowCountValue = Convert.ToInt32(result);

                return m_RowCountValue;
            }
        }

        private string CommaSeparatedListOfColumnNames(String Prefix = "")
        {
            // Return the existing value if it has already been determined.
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
                element = cachePages[0].table.Rows[rowIndex % RowsPerPage];
                return true;
            }
            else if (IsRowCachedInPage(1, rowIndex))
            {
                element = cachePages[1].table.Rows[rowIndex % RowsPerPage];
                return true;
            }

            return false;
        }

        // Sets the value of the element parameter if the value is in the cache.
        public void SetElementToPage(int rowIndex, int columnIndex, object element)
        {
            if (IsRowCachedInPage(0, rowIndex))
            {
                cachePages[0].table.Rows[rowIndex % RowsPerPage][columnIndex] = element;
            }
            else if (IsRowCachedInPage(1, rowIndex))
            {
                cachePages[1].table.Rows[rowIndex % RowsPerPage][columnIndex] = element;
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
    }
}
