using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IBE.EDSM
{
    public partial class EDStarmapInterfaceView : IBE.Enums_and_Utility_Classes.RNBaseForm
    {
        private EDStarmapInterface          m_DataSource;                   // data object

        public EDStarmapInterfaceView(EDStarmapInterface dataSource)
        {
            InitializeComponent();

           DataSource = dataSource;
        }

        /// <summary>
        /// sets or gets the data object
        /// </summary>
        public EDStarmapInterface DataSource
        {
            get
            {
                return m_DataSource;
            }
            set
            {
                m_DataSource     = value;

                if((m_DataSource != null) && (m_DataSource.GUI != this))
                { 
                    m_DataSource.GUI = this;
                }
            }
        }

        private void EDStarmapInterfaceView_Load(object sender, EventArgs e)
        {
            try
            {
                m_DataSource.GUIInterface.loadAllSettings(this);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in EDStarmapInterfaceView_Load");
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataSource.GUIInterface.saveSetting(txtCmdrName);
                DataSource.GUIInterface.saveSetting(txtAPIKey);

                ShowStatus();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while saving new values");
            }            

        }

        private void ShowStatus()
        {
            Cursor lastCursor = Cursor;

            try
            {
                Cursor = Cursors.WaitCursor;


                txtServerstatus.Text  = "refreshing info...";
                txtServerstatus.Refresh();
                txtAccountstatus.Text = "refreshing info...";
                txtAccountstatus.Refresh();

                ServerStatus status = DataSource.ServerStatus();

                if(status != null)
                {
                    txtServerstatus.Text = String.Format("Connection to EDSM : '{0}',   E:D Serverstatus = '{1}'", "successful", status.Message);
                }
                else
                {
                    txtServerstatus.Text = String.Format("Connection to EDSM : '{0}',   E:D Serverstatus = '{1}'", "failed", "unknown");
                }
                
                txtServerstatus.Refresh();


                if((!String.IsNullOrWhiteSpace(txtCmdrName.Text)) && (!String.IsNullOrWhiteSpace(txtAPIKey.Text)))
                {
                    EDStarmapInterface.ErrorCodes errCode = DataSource.LoginTest();
                    txtAccountstatus.Text = String.Format("Loginstatus = '{1}' ({0})", (Int32)errCode, errCode.ToString());
                }
                else
                {
                    txtAccountstatus.Text = "login data missing";
                }
                txtAccountstatus.Refresh();

                Cursor = lastCursor;
            }
            catch (Exception ex)
            {
                Cursor = lastCursor;
                throw new Exception("Error while showing edsm connection status", ex);
            }            

        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdClose");
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtCmdrName.Text    = "";
                txtAPIKey.Text      = "";

                DataSource.GUIInterface.saveSetting(txtCmdrName);
                DataSource.GUIInterface.saveSetting(txtAPIKey);

                ShowStatus();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdClear");
            }

        }

        private void EDStarmapInterfaceView_Shown(object sender, EventArgs e)
        {
            try
            {
                this.Refresh();
                ShowStatus();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in EDStarmapInterfaceView_Shown");
            }
        }

        private void cbCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DataSource.GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cbCheckedChanged");
            }
        }
    }
}
