using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;

namespace IBE.SQL
{
    public partial class DirectSQL : RNBaseForm
    {
        private DataTable       m_DataTable;
        private DataTable       m_DataTable2;
        private BindingSource   m_BindingSource;
        private DBConnector     m_DBCon;


        public DirectSQL(DBConnector useDBCon)
        {
            InitializeComponent();
            try
            {
                m_DataTable                 = new DataTable();
                m_DataTable2                = new DataTable();
                m_BindingSource             = new BindingSource();
                dgvResults.DataSource       = m_BindingSource;
                m_BindingSource.DataSource  = m_DataTable;

                m_DBCon = new DBConnector(useDBCon.ConfigData, true);
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while creating object");
            }
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            try
            {
                m_DataTable.Dispose();
            }
            catch (Exception)
            {
            }
            try
            {
                m_DataTable2.Dispose();
            }
            catch (Exception)
            {
            }
            try
            {
                m_BindingSource.Dispose();
            }
            catch (Exception)
            {
            }
            try
            {
                m_DBCon.Dispose();
            }
            catch (Exception)
            {
            }

            this.Close();
        }

        private void cmdExecute_Click(object sender, EventArgs e)
        {
            String sqlString;
            Int32 result = 0;
            Int32 result2 = 0;

            try
            {

                Cursor = Cursors.WaitCursor;
                cmdExecute.Enabled = false;

                if(!String.IsNullOrEmpty(txtCommand.SelectedText.Trim()))
                    sqlString = txtCommand.SelectedText.Trim();
                else
                    sqlString = txtCommand.Text.Trim();

                if(!String.IsNullOrEmpty(sqlString))
                {
                    m_DataTable.Columns.Clear();

                    sqlString.IndexOf("select", 0, StringComparison.CurrentCultureIgnoreCase);
                    result = m_DBCon.Execute(sqlString, m_DataTable);

                    m_DBCon.Execute("SELECT ROW_COUNT()", m_DataTable2); 

                    if(m_DataTable2.Rows.Count > 0)
                        result2 = Int32.Parse(m_DataTable2.Rows[0][0].ToString());

                    if(result2 >= 0)
                        txtAnswer.Text = String.Format("records affected: {0}", result2);                    
                    else
                        txtAnswer.Text = String.Format("records returned {0}", result);                    


                }

                Cursor = Cursors.Default;
                cmdExecute.Enabled = true;
            }
            catch (Exception ex)
            {
                cmdExecute.Enabled = true;
                Cursor = Cursors.Default;
                txtAnswer.Text = "Error while executing:\n" + ex.GetBaseException().Message;
            }
        }

    }
}
