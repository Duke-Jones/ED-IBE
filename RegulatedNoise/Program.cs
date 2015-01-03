using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise
{
    static class Program
    {
        private static SingleThreadLogger _logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _logger = new SingleThreadLogger(ThreadLoggerType.App);
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                _logger.Log("Error in program.cs:", true);
                _logger.Log(ex.ToString(), true);
                _logger.Log(ex.Message, true);
                _logger.Log(ex.StackTrace, true);
                if(ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString(), true);

                MessageBox.Show(
                    "Application has encountered an error and will close.\r\n\r\nException details:\r\n\r\n" + ex.Message +
                    "\r\n\r\nInner exception:\r\n" + ex.InnerException + "\r\nStack:\r\n" + ex.StackTrace);

                Application.Exit();
            }
        }
    }
}
