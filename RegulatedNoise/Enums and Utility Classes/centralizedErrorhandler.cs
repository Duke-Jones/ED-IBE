using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise
{
    
    public static class cErr
    {
        static private SingleThreadLogger _logger = new SingleThreadLogger(ThreadLoggerType.Exception);

        static public void processError(Exception ex)
        {
            processError(ex, "", false);
        }

        static public void processError(Exception ex, string infotext)
        {
            processError(ex, infotext, false);
        }

        static public void processError(Exception ex, string infotext, bool noAsking)
        {
            string info;

            // first log the complete exception 
            _logger.Log(infotext + ": " + ex, true);

            if (infotext.Trim().Length > 0) 
                info = infotext + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine;
            else
                info = ex.Message + Environment.NewLine;
            
            if (ex.InnerException != null)
                info += Environment.NewLine + ex.GetBaseException().Message;

            info += string.Format("{0}{0}(see detailed info in logfile \"{1}\")", Environment.NewLine, _logger.logPathName);

            info += string.Format("{0}{0}Suppress exception ? (App can be unstable!)", Environment.NewLine);

            Program.CreateMiniDump("RegulatedNoiseDump_handled.dmp");

            // ask user what to do
            if (noAsking || (MessageBox.Show(info, "Exception occured",MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No))
            {
                MessageBox.Show("Fatal error.\r\n\r\nA dump file (\"RegulatedNoiseDump_handled.dmp\" has been created in your RegulatedNoise directory.  \r\n\r\nPlease place this in a file-sharing service such as Google Drive or Dropbox, then link to the file in the Frontier forums or on the GitHub archive.  This will allow the developers to fix this problem.  \r\n\r\nThanks, and sorry about the crash...");
                Environment.Exit(-1);
            }
        }

        static public void ShowError(Exception ex, string infotext)
        {
            string info;

            // first log the complete exception 
				_logger.Log(infotext + ": " + ex, true);

            if (infotext.Trim().Length > 0) 
                info = infotext + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine;
            else
                info = ex.Message + Environment.NewLine;
            
            if (ex.InnerException != null)
                info += Environment.NewLine + ex.GetBaseException().Message;

            info += string.Format("{0}{0}(see detailed info in logfile \"{1}\")", Environment.NewLine, _logger.logPathName);
            info += string.Format("(dumpfile \"RegulatedNoiseDump.dmp\" created)", Environment.NewLine, _logger.logPathName);

            Program.CreateMiniDump("RegulatedNoiseDump.dmp");

            MessageBox.Show(info, "Exception occured",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
