using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE
{
    public partial class ErrorViewer : IBE.Enums_and_Utility_Classes.RNBaseForm
    {


        private String _LogPath;

        public ErrorViewer()
        {
            InitializeComponent();
            pictureBox1.Image = SystemIcons.Error.ToBitmap();
        }

        public void ShowDialog(Exception ex, string infotext)
        {
            string Info;
            Boolean oldValue = true;
            StringBuilder errorMessage = new StringBuilder();
            _LogPath = Program.GetDataPath("Logs");
            SingleThreadLogger _logger = new SingleThreadLogger(ThreadLoggerType.Exception, _LogPath, true);
            Exception currentException = ex;

            errorMessage.AppendLine(String.Format("{0:dd.MM.yyyy HH:mm:ss} : {1}", DateTime.UtcNow, infotext));

            if(String.IsNullOrEmpty(infotext))
                infotext = currentException.Message;

            do
            {
                errorMessage.AppendLine("--------------------------------------------------------------------------------");
                errorMessage.AppendLine(String.Format("{0:dd.MM.yyyy HH:mm:ss} : {1}", DateTime.UtcNow, currentException.Message));
                errorMessage.AppendLine(String.Format("{0:dd.MM.yyyy HH:mm:ss} : {1}", DateTime.UtcNow, currentException.StackTrace));

                if((currentException.InnerException == null) && (infotext != currentException.Message))
                    infotext += "\r\n -> " + currentException.Message;

                currentException = currentException.InnerException;

            } while (currentException != null);

            errorMessage.AppendLine("");
            errorMessage.AppendLine("********************************************************************************");
            errorMessage.AppendLine("");

            _logger.Log(errorMessage.ToString());

            txtErrorDetail.Text = errorMessage.ToString();
            lblErrorInfo.Text = infotext;
            lblLogDestination.Text = string.Format("(Logfile : {0})", _logger.logPathName);
            txtErrorDetail.SelectionStart = 0;
            txtErrorDetail.SelectionLength = 0;

            if(!Program.SplashScreen.IsDisposed)
            {
                oldValue = Program.SplashScreen.TopMost;
                Program.SplashScreen.TopMost = false;
            }
            
            this.ShowDialog();    


            if(!Program.SplashScreen.IsDisposed)
            {
                Program.SplashScreen.TopMost = oldValue;
            }
        }

        private void cmdDumpfile_Click(object sender, EventArgs e)
        {
            String filename = String.Format("ed-ibe-dump-v{0}.dmp", IBE.Enums_and_Utility_Classes.VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3).Replace(".", "_"));;
            String subPath = @"Logs\" + filename;

            Program.CreateMiniDump(subPath);
            MessageBox.Show("A dump file (\"" + filename + "\") has been created in " + Program.GetDataPath("Logs") + "\r\n\r\n" +
                            "You may place this in a file-sharing service such as SendSpace, Google Drive or Dropbox," + 
                            "then link to the file in the Frontier forums or on the GitHub archive or send e mail to Duke.Jones@gmx.de . " +
                            "This will allow the developer to fix this problem.  \r\n\r\nThanks, and sorry about the crash...");
        }

        private void cmdIgnore_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdShutdown_Click(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }

        private void cmdOpenLocation_Click(object sender, EventArgs e)
        {
		    System.Diagnostics.Process.Start("explorer.exe", _LogPath);
        }

        private void ErrorViewer_Load(object sender, EventArgs e)
        {

        }
    }
}
