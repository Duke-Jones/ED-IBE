//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace RegulatedNoise.Enums_and_Utility_Classes
//{
//    public partial class VirtualJustInTimeDemo : Form
//    {
//        public VirtualJustInTimeDemo()
//        {
//            InitializeComponent();
//        }
//    }
//}


#if Test 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RegulatedNoise.Enums_and_Utility_Classes2
{
    public partial class VirtualJustInTimeDemo : Form
    {
        public SQL.DBConnector m_DBCon { get; set; }

        private DataGridView dataGridView1 = new DataGridView();
        private DataGridViewCache memoryCache;

        // Specify a connection string. Replace the given value with a 
        // valid connection string for a Northwind SQL Server sample
        // database accessible to your system.
        private string connectionString =
            "Initial Catalog=Elite_DB;Data Source=localhost;" +
            "Integrated Security=SSPI;Persist Security Info=False";
        private string table = "tbLog";

        public VirtualJustInTimeDemo(SQL.DBConnector DBCon)
        {
            m_DBCon = DBCon;
        }

        protected override void OnLoad(EventArgs e)
        {
            // Initialize the form.
            this.AutoSize = true;
            this.Controls.Add(this.dataGridView1);
            this.Text = "DataGridView virtual-mode just-in-time demo";

            // Complete the initialization of the DataGridView.
            this.dataGridView1.Size = new Size(800, 250);
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.VirtualMode = true;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.CellValueNeeded += new DataGridViewCellValueEventHandler(dataGridView1_CellValueNeeded);

            // Create a DataRetriever and use it to create a DataGridViewCache object
            // and to initialize the DataGridView columns and rows.
            try
            {
                DataRetriever retriever = new DataRetriever(m_DBCon, table, "select * from tbLog");
                memoryCache             = new DataGridViewCache(retriever, 16);

                foreach (DataColumn column in retriever.Columns)
                {
                    dataGridView1.Columns.Add(column.ColumnName, column.ColumnName);
                }
                this.dataGridView1.RowCount = retriever.RowCount;
            }
            catch (MySqlException ex)
            {
                cErr.showError(ex, "Connection could not be established. " +
                    "Verify that the connection string is valid.");
                Application.Exit();
            }

            // Adjust the column widths based on the displayed values.
            this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            base.OnLoad(e);
        }

        private void dataGridView1_CellValueNeeded(object sender,
            DataGridViewCellValueEventArgs e)
        {
            e.Value =  memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

    }

    public interface IDataPageRetriever
    {
        DataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage);
    }

    public class DataRetriever : IDataPageRetriever
    {
        private string          tableName;
        private string          DataStatement;
        private MySqlCommand    command;

        public DataRetriever(SQL.DBConnector DBCon, string tableName, String DataStatement)
        {
            MySqlConnection connection  = (MySqlConnection)DBCon.Connection;
            command                     = connection.CreateCommand();
            this.tableName              = tableName;
            this.DataStatement          = DataStatement;
        }

        private int rowCountValue = -1;

        public int RowCount
        {
            get
            {
                // Return the existing value if it has already been determined.
                if (rowCountValue != -1)
                {
                    return rowCountValue;
                }

                Object result = -1;
                // Retrieve the row count from the database.
                command.CommandText = "SELECT COUNT(*) FROM " + tableName;
                result = command.ExecuteScalar();
                if (result != null)
                    rowCountValue = Convert.ToInt32(result);

                return rowCountValue;
            }
        }

        private DataColumnCollection columnsValue;

        public DataColumnCollection Columns
        {
            get
            {
                // Return the existing value if it has already been determined.
                if (columnsValue != null)
                {
                    return columnsValue;
                }

                // Retrieve the column information from the database.
                //command.CommandText = "SELECT * FROM " + tableName;
                command.CommandText = DataStatement;
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.FillSchema(table, SchemaType.Source);
                columnsValue = table.Columns;
                return columnsValue;
            }
        }

        private string commaSeparatedListOfColumnNamesValue = null;
        private String usedPrefix = null;

        private string CommaSeparatedListOfColumnNames(String Prefix = "")
        {
            // Return the existing value if it has already been determined.
            if ((commaSeparatedListOfColumnNamesValue != null) && (Prefix == usedPrefix))
            {
                return commaSeparatedListOfColumnNamesValue;
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

            commaSeparatedListOfColumnNamesValue    = commaSeparatedColumnNames.ToString();
            usedPrefix                              = Prefix;

            return commaSeparatedListOfColumnNamesValue;
        }

        // Declare variables to be reused by the SupplyPageOfData method.
        public enum SQLSortOrder
        {
            asc,
            desc
        }

        private string columnToSortBy;
        private SQLSortOrder ColumnSortOrder = SQLSortOrder.desc;

        private MySqlDataAdapter adapter = new MySqlDataAdapter();

        public DataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage)
        {
            String sqlString;

            // Store the name of the ID column. This column must contain unique 
            // values so the SQL below will work properly.
            if (columnToSortBy == null)
            {
                columnToSortBy = this.Columns[0].ColumnName;
            }

            if (!this.Columns[columnToSortBy].Unique)
            {
                throw new InvalidOperationException(String.Format(
                    "Column {0} must contain unique values.", columnToSortBy));
            }

            // Retrieve the specified number of rows from the database, starting
            // with the row specified by the lowerPageBoundary parameter.
            sqlString = String.Format("select {0}" + 
                                      " from {2} L1 left join (" +
                                      "                         select time from {2} L3 order by L3.{3} {5} limit {4}" +
                                      "                       ) L2 on L1.{3} = L2.{3}" +
                                      " where L2.{3} is null order by L1.{3} {5} limit {1}", 
                                      CommaSeparatedListOfColumnNames("L1."), 
                                      rowsPerPage, 
                                      tableName, 
                                      columnToSortBy, 
                                      lowerPageBoundary, 
                                      ColumnSortOrder);

            // Retrieve the specified number of rows from the database, starting
            // with the row specified by the lowerPageBoundary parameter.
            command.CommandText = sqlString;
            adapter.SelectCommand = command;

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            adapter.Fill(table);
            return table;
        }

    }

}

#endif