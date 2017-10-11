using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IBE.SQL;
using IBE.Enums_and_Utility_Classes;

namespace IBE.EDDN
{
    public partial class EDDNView : RNBaseForm
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

                LoadTrustedSenders();
                LoadEDDNRelays();

                m_Communicator.DataChangedEvent += m_Communicator_DataChangedEvent;
                tmrRefresh.Start();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in Load event");
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

                if ((m_Communicator.ListenersRunning > 0) != (lblListenerStatus.Text == "Listening"))
                {
                    if(m_Communicator.ListenersRunning > 0)
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

                    if((lChanged & EDDNCommunicator.enDataTypes.RecieveData) != 0)
                    {
                        if(m_Communicator.RawData.Count() > 0)
                            tbEDDNOutput.Text = m_Communicator.RawData[m_Communicator.RawData.Count()-1];
                        else
                            tbEDDNOutput.Text = "";
                    }

                    if((lChanged & EDDNCommunicator.enDataTypes.ImplausibleData) != 0)
                    {
                        if(m_Communicator.RejectedData.Count() > 0)
                        {
                            do
                            {
                                lbEddnImplausible.Items.Add(m_Communicator.RejectedData[0]);    
                                m_Communicator.RejectedData.RemoveAt(0);
                            } while (m_Communicator.RejectedData.Count() > 0);
                        }

                    }

                    if((lChanged & EDDNCommunicator.enDataTypes.Statistics) != 0)
                    {
                        tbEddnStatsSW.Text = "";

                        if(m_Communicator.StatisticDataSW.Count() > 0)
                        {
                            System.Text.StringBuilder output = new System.Text.StringBuilder();
                            foreach (var appVersion in m_Communicator.StatisticDataSW.OrderByDescending(x => x.Value.MessagesReceived))
                            {
                                output.AppendFormat("{0} : {1} messages ({2} datasets)\r\n", appVersion.Key, appVersion.Value.MessagesReceived, appVersion.Value.DatasetsReceived);
                            }

                            tbEddnStatsSW.Text = output.ToString();
                        }

                        if(m_Communicator.StatisticDataRL.Count() > 0)
                        {
                            System.Text.StringBuilder output = new System.Text.StringBuilder();
                            foreach (var appVersion in m_Communicator.StatisticDataRL.OrderByDescending(x => x.Value.MessagesReceived))
                            {
                                output.AppendFormat("{0} : {1} messages ({2} datasets)\r\n", appVersion.Key, appVersion.Value.MessagesReceived, appVersion.Value.DatasetsReceived);
                            }

                            tbEddnStatsRL.Text = output.ToString();
                        }

                        if(m_Communicator.StatisticDataCM.Count() > 0)
                        {
                            System.Text.StringBuilder output = new System.Text.StringBuilder();
                            foreach (var appVersion in m_Communicator.StatisticDataCM.OrderByDescending(x => x.Value.MessagesReceived))
                            {
                                output.AppendFormat("{0} : {1} messages ({2} datasets)\r\n", appVersion.Key, appVersion.Value.MessagesReceived, appVersion.Value.DatasetsReceived);
                            }

                            tbEddnStatsCM.Text = output.ToString();
                        }

                        if(m_Communicator.StatisticDataMT.Count() > 0)
                        {
                            System.Text.StringBuilder output = new System.Text.StringBuilder();
                            foreach (var mType in m_Communicator.StatisticDataMT.OrderByDescending(x => x.Value.MessagesReceived))
                            {
                                output.AppendFormat("{0} : {1} messages\r\n", mType.Key, mType.Value.MessagesReceived);
                            }

                            tbEddnStatsMT.Text = output.ToString();
                        }
                    }
                }

                tmrRefresh.Start();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while refreshing the display, refreshing disabled.");
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
                CErr.processError(ex, "Error in ComboBox_CheckedChanged");
            }
        }

        private void cmdStartListening_Click(object sender, EventArgs e)
        {
            try
            {
                m_Communicator.StartEDDNListening();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdStartListening_Click");
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
                CErr.processError(ex, "Error in cmdStopListening_Click");
            }
        }

        private void cmdStartSender_Click(object sender, EventArgs e)
        {
            try
            {
                //if(!m_Communicator.SenderIsActivated)
                //{
                //    m_Communicator.ActivateSender();
                //}
                //else
                //{
                //    MessageBox.Show(this, "EDDNComminicator is already listing", "EDDN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdStartSender_Click");
            }
        }

        private void cmdStopSender_Click(object sender, EventArgs e)
        {
            try
            {
                m_Communicator.DeactivateSender();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdStopSender_Click");
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                    m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in TextBox_KeyDown", ex);
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                m_GUIInterface.saveSetting(sender);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in TextBox_Leave", ex);
            }
        }

        private void LoadTrustedSenders()
        {
            try
            {
                dgvTrustedSenders.DataSource = Program.Data.BaseData.tbtrustedsenders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading trusted senders", ex);
            }
        }

        private void cmdAddTrusted_Click(object sender, EventArgs e)
        {
            try
            {
                String newName = "";

                if ((InputBox.Show("Trusted Senders", "Name of the sender ?", ref newName) == System.Windows.Forms.DialogResult.OK) && (!String.IsNullOrEmpty(newName.Trim())))
                {
                    Program.Data.BaseData.tbtrustedsenders.Rows.Add(newName.Trim());

                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbtrustedsenders.TableName, true);
                }

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while adding a trusted sender");
            }
        }

        private void cmdRemoveTrusted_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvTrustedSenders.SelectedRows.Count > 0)
                {
                    if(MessageBox.Show("Remove selected senders from list ?", "Trusted Senders", MessageBoxButtons.OKCancel,  MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (DataGridViewRow selRow in dgvTrustedSenders.SelectedRows)
                        {
                            ((SQL.Datasets.dsEliteDB.tbtrustedsendersRow)((System.Data.DataRowView)(selRow.DataBoundItem)).Row).Delete();
                        }

                        Program.Data.PrepareBaseTables(Program.Data.BaseData.tbtrustedsenders.TableName, true);
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while removing a trusted sender");
            }
        }

        private void LoadEDDNRelays()
        {
            try
            {
                dgvEDDNRelays.DataSource = Program.Data.BaseData.tbeddnrelays;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading EDDN relays", ex);
            }
        }

        private void cmdAddEDDNRelay_Click(object sender, EventArgs e)
        {
            try
            {
                String newName = "";

                if ((InputBox.Show("EDDN Relays", "Adress of relay ?", ref newName) == System.Windows.Forms.DialogResult.OK) && (!String.IsNullOrEmpty(newName.Trim())))
                {
                    Program.Data.BaseData.tbeddnrelays.Rows.Add(newName.Trim());

                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbeddnrelays.TableName, true);
                }

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while adding a EDDN relay");
            }
        }

        private void cmdRemoveEDDNRelay_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvEDDNRelays.SelectedRows.Count > 0)
                {
                    if(MessageBox.Show("Remove relay from list ?", "EDDN Relays", MessageBoxButtons.OKCancel,  MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (DataGridViewRow selRow in dgvEDDNRelays.SelectedRows)
                        {
                            ((SQL.Datasets.dsEliteDB.tbeddnrelaysRow)((System.Data.DataRowView)(selRow.DataBoundItem)).Row).Delete();
                        }

                        Program.Data.PrepareBaseTables(Program.Data.BaseData.tbeddnrelays.TableName, true);
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while removing a EDDN relay");
            }
        }

    }
}
