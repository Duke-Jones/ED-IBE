using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;

namespace IBE
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
            String FileName = String.Format("ed-ibe-dump-v{0}.dmp", VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3).Replace(".", "_"));

            string Info;
            Boolean oldValue = true;

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
                                                                                                                                    

            Info += string.Format("{0}{0}(dumpfile " + FileName + " created, see detailed info in logfile \"{1}\")", Environment.NewLine, _logger.logPathName);

            Info += string.Format("{0}{0}Suppress exception ? (App can be unstable!)", Environment.NewLine);


            Program.CreateMiniDump(FileName);

            if(!Program.SplashScreen.IsDisposed)
            {
                oldValue = Program.SplashScreen.TopMost;
                Program.SplashScreen.TopMost = false;
            }
                
            // ask user what to do
            if (noAsking || (MessageBox.Show(Info, "Exception occured",MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification) == DialogResult.No))
            {
                MessageBox.Show("Fatal error.\r\n\r\nA dump file (\"" + FileName + "\" has been created in your data directory.  \r\n\r\nPlease place this in a file-sharing service such as SendSpace, Google Drive or Dropbox, then link to the file in the Frontier forums or on the GitHub archive or send e mail to Duke.Jones@gmx.de.  This will allow the developers to fix this problem.  \r\n\r\nThanks, and sorry about the crash...");
                Environment.Exit(-1);
            }

            if(!Program.SplashScreen.IsDisposed)
            {
                Program.SplashScreen.TopMost = oldValue;
            }
}

        //static public void processError(Exception ex, string Infotext, Boolean ForceEnd = false)
        //{
        //    string Info;

        //    // first log the complete exception 
        //    _logger.Log(Infotext, true);
        //    _logger.Log(ex.ToString(), true);
        //    _logger.Log(ex.Message, true);
        //    _logger.Log(ex.StackTrace, true);
        //    if (ex.InnerException != null)
        //        _logger.Log(ex.InnerException.ToString(), true);

        //    if (Infotext.Trim().Length > 0) 
        //        Info = Infotext + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine;
        //    else
        //        Info = ex.Message + Environment.NewLine;
            
        //    if (ex.InnerException != null)
        //        Info += Environment.NewLine + ex.GetBaseException().Message;

        //    Info += string.Format("{0}{0}(see detailed info in logfile \"{1}\")", Environment.NewLine, _logger.logPathName);
        //    Info += "Create a dump file ?";

        //    if (MessageBox.Show(Info, "Exception occured",MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        //       Program.CreateMiniDump("RegulatedNoiseDump.dmp");

        //}
    }
}
