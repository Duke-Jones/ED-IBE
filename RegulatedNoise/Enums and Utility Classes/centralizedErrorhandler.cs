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

        static public void processError(Exception ex, string Infotext)
        {
            processError(ex, Infotext, false);
        }

        static public void processError(Exception ex, string Infotext, bool noAsking)
        {
            string Info;

            // first log the complete exception 
            _logger.Log(Infotext, true);
            _logger.Log(ex.ToString(), true);
            _logger.Log(ex.Message, true);
            _logger.Log(ex.StackTrace, true);
            if (ex.InnerException != null)
                _logger.Log(ex.InnerException.ToString(), true);

            if (Infotext.Trim().Length > 0) 
                Info = Infotext + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine;
            else
                Info = ex.Message + Environment.NewLine;
            
            if (ex.InnerException != null)
                Info += Environment.NewLine + ex.GetBaseException().Message;

            Info += string.Format("{0}{0}(see detailed info in logfile \"{1}\")", Environment.NewLine, _logger.logPathName);

            Info += string.Format("{0}{0}Suppress exception ? (App can be unstable!)", Environment.NewLine);

            Program.CreateMiniDump("RegulatedNoiseDump_handled.dmp");

            // ask user what to do
            if (noAsking || (MessageBox.Show(Info, "Exception occured",MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No))
            {
                MessageBox.Show("Fatal error.\r\n\r\nA dump file (\"RegulatedNoiseDump_handled.dmp\" has been created in your RegulatedNoise directory.  \r\n\r\nPlease place this in a file-sharing service such as Google Drive or Dropbox, then link to the file in the Frontier forums or on the GitHub archive.  This will allow the developers to fix this problem.  \r\n\r\nThanks, and sorry about the crash...");
                Environment.Exit(-1);
            }
        }

        static public void showError(Exception ex, string Infotext)
        {
            string Info;

            // first log the complete exception 
            _logger.Log(Infotext, true);
            _logger.Log(ex.ToString(), true);
            _logger.Log(ex.Message, true);
            _logger.Log(ex.StackTrace, true);
            if (ex.InnerException != null)
                _logger.Log(ex.InnerException.ToString(), true);

            if (Infotext.Trim().Length > 0) 
                Info = Infotext + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine;
            else
                Info = ex.Message + Environment.NewLine;
            
            if (ex.InnerException != null)
                Info += Environment.NewLine + ex.GetBaseException().Message;

            Info += string.Format("{0}{0}(see detailed info in logfile \"{1}\")", Environment.NewLine, _logger.logPathName);
            Info += string.Format("(dumpfile \"RegulatedNoiseDump.dmp\" created)", Environment.NewLine, _logger.logPathName);

            Program.CreateMiniDump("RegulatedNoiseDump.dmp");

            MessageBox.Show(Info, "Exception occured",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
