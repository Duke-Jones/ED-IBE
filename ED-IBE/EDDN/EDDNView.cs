using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IBE.SQL;

namespace IBE.EDDN
{
    public partial class EDDNView : Form
    {

        public const String        DB_GROUPNAME                    = "EDDN";

        private DBGuiInterface                  m_GUIInterface;
        private EDDNCommunicator                m_Communicator;
        private EDDNCommunicator.enDataTypes    m_ChangedData = EDDNCommunicator.enDataTypes.ImplausibleData | 
                                                                EDDNCommunicator.enDataTypes.RecieveData | 
                                                                EDDNCommunicator.enDataTypes.Statistics;

        public EDDNView(EDDNCommunicator communicator)
        {
            InitializeComponent();
            m_Communicator  = communicator;
        }

        private void EDDNView_Load(object sender, EventArgs e)
        {
            try
            {
                // loading all settings
                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, new DBConnector(Program.DBCon.ConfigData, true));
                m_GUIInterface.loadAllSettings(this);

                m_Communicator.DataChangedEvent += m_Communicator_DataChangedEvent;
                tmrRefresh.Start();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in Load event");
            }
        }

        /// <summary>
        /// fired, if any data of the communicator has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Communicator_DataChangedEvent(object sender, EDDNCommunicator.DataChangedEventArgs e)
        {
            m_ChangedData |= e.DataType;
        }

        /// <summary>
        /// refreshing the display, if necessary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrRefresh.Stop();

                if (m_Communicator.ListenerIsRunning != (lblListenerStatus.Text == "Listening"))
                {
                    if(m_Communicator.ListenerIsRunning)
                    { 
                        pbListenerStatus.Image = Properties.Resources.green_led_on_md;
                        lblListenerStatus.Text = "Listening";
                    }
                    else
                    { 
                        pbListenerStatus.Image = Properties.Resources.green_led_off_md;
                        lblListenerStatus.Text = "Off";
                    }
                    
                }

                if(m_ChangedData != EDDNCommunicator.enDataTypes.NoChanges)
                {
                    EDDNCommunicator.enDataTypes lChanged = m_ChangedData;
                    m_ChangedData = EDDNCommunicator.enDataTypes.NoChanges;

                    if(lChanged == EDDNCommunicator.enDataTypes.RecieveData)
                    {
                        if(m_Communicator.RawData.Count() > 0)
                            tbEDDNOutput.Text = m_Communicator.RawData[m_Communicator.RawData.Count()-1];
                        else
                            tbEDDNOutput.Text = "";
                    }

                    if(lChanged == EDDNCommunicator.enDataTypes.ImplausibleData)
                    {
                        if(m_Communicator.RawData.Count() > 0)
                            lbEddnImplausible.Text = m_Communicator.RejectedData[m_Communicator.RejectedData.Count()-1];
                        else
                            lbEddnImplausible.Text = "";

                    }

                    if(lChanged == EDDNCommunicator.enDataTypes.Statistics)
                    {
                        if(m_Communicator.StatisticData.Count() > 0)
                        {
                            System.Text.StringBuilder output = new System.Text.StringBuilder();
                            foreach (var appVersion in m_Communicator.StatisticData.OrderByDescending(x => x.Value.MessagesReceived))
                            {
                                output.AppendFormat("{0} : {1} messages ({2} datasets)\r\n", appVersion.Key, appVersion.Value.MessagesReceived, appVersion.Value.DatasetsReceived);
                            }

                            tbEddnStats.Text = output.ToString();
                        }
                        else
                            tbEddnStats.Text = "waiting for data...";

                    }
                }

                tmrRefresh.Start();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while refreshing the display, refreshing disabled.");
            }            
        }

        /// <summary>
        /// saves the changed settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_GUIInterface.saveSetting(sender))
                {

                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in ComboBox_CheckedChanged");
            }
        }

        private void cmdStartListening_Click(object sender, EventArgs e)
        {
            try
            {
                if(!m_Communicator.ListenerIsRunning)
                {
                    m_Communicator.StartEDDNListening();
                }
                else
                {
                    MessageBox.Show(this, "EDDNComminicator is already listing", "EDDN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdStartListening_Click");
            }
        }

        private void cmdStopListening_Click(object sender, EventArgs e)
        {
            try
            {
                m_Communicator.StopEDDNListening();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in cmdStopListening_Click");
            }
        }

    }
}
