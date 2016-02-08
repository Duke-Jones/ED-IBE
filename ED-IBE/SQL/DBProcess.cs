using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.IO;

namespace IBE.SQL
{
    class DBProcess : IDisposable
    {
        private bool                m_wasRunning        = false;
        private DBProcessParams     m_Params            = null;
        private Process             m_Process           = null;
        private bool                disposed            = false;

        public Process SQLDB_Process { get { return m_Process;} }

        public class DBProcessParams
        {
            public Int32            Port                = 0;
            public String           Commandline         = "";
            public String           Commandargs         = "";
            public String           Workingdirectory    = "";
            public Int32            DBStartTimeout      = 0;
        }

        /// <summary>
        /// starts the database process if it's not running
        /// </summary>
        /// <param name="Parameter"></param>
        public DBProcess(DBProcessParams Parameter)
        {
            IPGlobalProperties      ipGlobalProperties;
            IPEndPoint[]            tcpConnInfoArray;

            m_Params = Parameter;

            ipGlobalProperties      = IPGlobalProperties.GetIPGlobalProperties();
            tcpConnInfoArray        = ipGlobalProperties.GetActiveTcpListeners();


            // check if the port is open - if open we assume the db is running
            foreach (IPEndPoint tcpi in tcpConnInfoArray)
                if (tcpi.Port == m_Params.Port)
                {
                    m_wasRunning = true;
                    break;
                }
            
            if(!m_wasRunning)
            {
                ProcessStartInfo psi;

                // start the DB server process
                if(Debugger.IsAttached)
                { 
                    psi                         = new ProcessStartInfo(m_Params.Commandline,m_Params.Commandargs);
                    psi.WorkingDirectory        = m_Params.Workingdirectory;
                    psi.WindowStyle             = ProcessWindowStyle.Normal;
                    psi.CreateNoWindow          = false;
                    psi.RedirectStandardOutput  = false;
                    psi.UseShellExecute         = true;
                }
                else
                { 
                    psi                         = new ProcessStartInfo(m_Params.Commandline,m_Params.Commandargs);
                    psi.WorkingDirectory        = m_Params.Workingdirectory;
                    psi.WindowStyle             = ProcessWindowStyle.Hidden;
                    psi.CreateNoWindow          = true;
                    psi.RedirectStandardOutput  = false;
                    psi.UseShellExecute         = true;
                     
                }

                m_Process = System.Diagnostics.Process.Start(psi);

                // wait for db ready state
                Boolean isRunning = false;
                PerformanceTimer pc = new PerformanceTimer();

                pc.startMeasuring();
                System.Threading.Thread.Sleep(1000);
                do
                {
                    ipGlobalProperties      = IPGlobalProperties.GetIPGlobalProperties();
                    tcpConnInfoArray        = ipGlobalProperties.GetActiveTcpListeners();

                    foreach (IPEndPoint tcpi in tcpConnInfoArray)
                        if (tcpi.Port == m_Params.Port)
                        {
                            isRunning = true;
                            break;
                        }

                    if(!isRunning)
                        System.Threading.Thread.Sleep(1000);
                    
                    Debug.Print("Waiting " + pc.currentMeasuring().ToString());

                } while ((!isRunning) && ((pc.currentMeasuring() / 1000) < m_Params.DBStartTimeout));


            }
        }

        /// <summary>
        /// stops the sql server
        /// </summary>
        public void StopServer(String user, String pass)
        {
            ProcessStartInfo psi=null;

            try
            {

                String CommandArgs = String.Format("-u {0} --password={1} shutdown", user, pass);
                String fullPath = Path.GetDirectoryName(Path.GetFullPath(m_Params.Workingdirectory));


                psi                         = new ProcessStartInfo("mysqladmin.exe", CommandArgs);
                psi.WorkingDirectory        = m_Params.Workingdirectory;
                psi.RedirectStandardOutput  = false;

                // start the process for stopping the server
                if(true || Debugger.IsAttached)
                { 
                    psi.WindowStyle             = ProcessWindowStyle.Normal;
                    psi.CreateNoWindow          = false;
                    psi.UseShellExecute         = true;
                }
                else
                { 
                    psi.WindowStyle             = ProcessWindowStyle.Hidden;
                    psi.CreateNoWindow          = true;
                    psi.UseShellExecute         = true;
                }

                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error while shutting down the server: {0}\n{1}\n{2}", psi.FileName, psi.Arguments, psi.WorkingDirectory), ex);
            }
        }

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if((!m_wasRunning) && (m_Process != null))
                    {
                        m_Process.CloseMainWindow();
                        m_Process.WaitForExit();
                        m_Process.Dispose();
                        m_Process = null;
                    }
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~DBProcess()
        {
            // Simply call Dispose(false).
            Dispose (false);
        } 


    }
}
