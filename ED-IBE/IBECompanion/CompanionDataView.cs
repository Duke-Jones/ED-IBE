using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBE.SQL;
using System.Net.Mail;

namespace IBE.IBECompanion
{
    public partial class CompanioDataView : IBE.Enums_and_Utility_Classes.RNBaseForm
    {
        public const String                             DB_GROUPNAME                    = "CompanionAPI";
        private DBGuiInterface                          m_GUIInterface;

        public CompanioDataView()
        {
            InitializeComponent();
        }

        private void CompanioDataView_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                m_GUIInterface = new DBGuiInterface(DB_GROUPNAME, Program.DBCon);
                m_GUIInterface.loadAllSettings(this);

                GetState();

                Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error in CompanioDataView_Load");
            }
        }

        private void GetState()
        {
            try
            {
                MailAddress mailAddress = null;

                try
                {
                   mailAddress = new MailAddress(txtEmail.Text);
                }
                catch(Exception ex)
                {
                   mailAddress = null;
                }

                if(mailAddress != null)
                {
                    var profileExists = Program.CompanionIO.LoadProfile(mailAddress.Address);
                    if (profileExists)
                    {
                        Program.DBCon.setIniValue(DB_GROUPNAME, "EmailAddress", txtEmail.Text);

                        var response = Program.CompanionIO.GetProfileData(false);
                        var json = "";

                        switch (response.LoginStatus)
	                    {
		                    case EDCompanionAPI.Models.LoginStatus.Ok:
                                var name = (String )Program.CompanionIO.GetData().SelectToken("commander.name", false);

                                if(String.IsNullOrEmpty(name))
                                    txtStatus.Text =  "Connected and at call, Commander !";
                                else
                                    txtStatus.Text =  "Connected and at call, Cmdr. " + name + "!";

                                txtStatus.ForeColor = Color.Black;
                                txtStatus.BackColor = Color.SpringGreen;

                                cmdConnect.Enabled  = false;
                                cmdVerify.Enabled   = false;
                                break;
                            case EDCompanionAPI.Models.LoginStatus.PendingVerification:
                                txtStatus.Text =  "Pending, waiting for verification code !";
                                txtStatus.ForeColor = Color.Black;
                                txtStatus.BackColor = Color.FromArgb(0xFF, 0xFF, 0xCC);

                                cmdConnect.Enabled  = true;
                                cmdVerify.Enabled   = true;
                                break;
                            case EDCompanionAPI.Models.LoginStatus.IncorrectCredentials:
                                txtStatus.Text =  "Incorrect credentials !";
                                txtStatus.ForeColor = Color.Black;
                                txtStatus.BackColor = Color.Red;

                                cmdConnect.Enabled  = true;
                                cmdVerify.Enabled   = false;
                                break;
                            case EDCompanionAPI.Models.LoginStatus.UnknownError:
                                cmdConnect.Enabled  = true;
                                txtStatus.ForeColor = Color.Black;
                                txtStatus.BackColor = Color.Red;

                                cmdVerify.Enabled   = false;
                                txtStatus.Text =  "Unknown Error !";
                                break;
                            case EDCompanionAPI.Models.LoginStatus.NotAccessible:
                                txtStatus.Text =  "No data recieved, servers may in maintenance mode !";
                                txtStatus.ForeColor = Color.Black;
                                txtStatus.BackColor = Color.FromArgb(0xFF, 0xFF, 0xCC);
                                cmdConnect.Enabled  = false;
                                cmdVerify.Enabled   = false;

                                break;
                            default:
                                cmdConnect.Enabled  = true;
                                cmdVerify.Enabled   = false;

                                throw new Exception("Unexpected state : " + response.LoginStatus.ToString());
                             break;
	                    }
                    }
                    else
                    {
                        txtStatus.Text =  "Not initialized !";
                        txtStatus.ForeColor = Color.Black;
                        txtStatus.BackColor = Color.FromArgb(0xFF, 0xFF, 0xCC);

                        cmdConnect.Enabled  = true;
                        cmdVerify.Enabled   = false;
                    }

                }
                else
                {
                    txtStatus.Text      =  "Missing or invalid account data !";
                    cmdConnect.Enabled  = true;
                    cmdVerify.Enabled   = false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting current state", ex);
            }
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                MailAddress mailAddress = null;

                try
                {
                   mailAddress = new MailAddress(txtEmail.Text);
                }
                catch(Exception ex)
                {
                   mailAddress = null;
                }

                if(mailAddress != null)
                {
                    Program.CompanionIO.CreateProfile(mailAddress.Address, txtPassword.Text);
                }

                GetState();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error in cmdConnect_Click");
            }
        }

        private void cmdVerify_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var verificationResponse = Program.CompanionIO.SubmitVerification(txtVerficationCode.Text);

                GetState();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error in cmdVerify_Click");
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if(MessageBox.Show(this, "Delete the account data ?", "Companion IO", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                { 
                    Program.CompanionIO.DeleteProfile(txtEmail.Text);    
                    txtPassword.Clear();
                    txtVerficationCode.Clear();
                }

                GetState();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "cmdClear_Click");
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdToClipBoard_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(Program.CompanionIO.GetRawData());
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "cmdToClipBoard_Click");
            }
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = cmdConnect;
            this.UpdateDefaultButton();
        }

        private void txtVerficationCode_TextChanged(object sender, EventArgs e)
        {
            if(cmdVerify.Enabled)
            { 
                this.AcceptButton = cmdVerify;
                this.UpdateDefaultButton();
            }

        }


    }
}
