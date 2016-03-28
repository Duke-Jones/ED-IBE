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

        public bool WasRunning
        {
            get
            {
                return m_wasRunning;
            }
        }

        public Process SQLDB_Process { get { return m_Process; } }

        public class DBProcessParams
        {
            public UInt16           Port                = 0;
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
            m_Params = Parameter;

            m_wasRunning = IsListenerOnPort(m_Params.Port);
            
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

                if (!m_Process.HasExited)
                {
                    do
                    {

                        isRunning = IsListenerOnPort(m_Params.Port);

                        if(!isRunning)
                            System.Threading.Thread.Sleep(1000);
                    
                        Debug.Print("Waiting " + pc.currentMeasuring().ToString());

                    } while ((!isRunning) && ((pc.currentMeasuring() / 1000) < m_Params.DBStartTimeout));
                }
                else
                {
                    throw new Exception("can't start sql server !");
                }
            }
        }

        /// <summary>
        /// stops the sql server
        /// </summary>
        public void StopServer(String user, String pass)
        {
            ProcessStartInfo psi=null;
            String commandLine;
            String commandArgs;

            try
            {

                commandLine = m_Params.Commandline.Replace("mysqld.exe", "mysqladmin.exe");
                commandArgs = String.Format("-u {0} --password={1} --port={2} shutdown", user, pass, m_Params.Port);

                // start the process for stopping the server
                if(Debugger.IsAttached)
                { 
                    psi                         = new ProcessStartInfo(commandLine, commandArgs);
                    psi.WorkingDirectory        = m_Params.Workingdirectory;
                    psi.WindowStyle             = ProcessWindowStyle.Normal;
                    psi.CreateNoWindow          = false;
                    psi.RedirectStandardOutput  = false;
                    psi.UseShellExecute         = true;
                }
                else
                { 
                    psi                         = new ProcessStartInfo(commandLine, commandArgs);
                    psi.WorkingDirectory        = m_Params.Workingdirectory;
                    psi.WindowStyle             = ProcessWindowStyle.Hidden;
                    psi.CreateNoWindow          = true;
                    psi.RedirectStandardOutput  = false;
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

        /// <summary>
        /// returns true, if this port has a active tcp listener
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Boolean IsListenerOnPort(UInt16 port)
        {
            Boolean retValue = false;

            IPGlobalProperties      ipGlobalProperties;
            IPEndPoint[]            tcpConnInfoArray;

            ipGlobalProperties      = IPGlobalProperties.GetIPGlobalProperties();
            tcpConnInfoArray        = ipGlobalProperties.GetActiveTcpListeners();

            foreach (IPEndPoint tcpi in tcpConnInfoArray)
                if (tcpi.Port == port)
                {
                    retValue = true;
                    break;
                }

            return retValue;
        }

        /// <summary>
        /// returns true, if this port has a active tcp connection
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Boolean IsConnectionOnPort(UInt16 port)
        {
            Boolean retValue = false;

            IPGlobalProperties         ipGlobalProperties;
            TcpConnectionInformation[] tcpConnInfoArray;

            ipGlobalProperties      = IPGlobalProperties.GetIPGlobalProperties();
            tcpConnInfoArray        = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
                if (tcpi.LocalEndPoint.Port == port)
                {
                    retValue = true;
                    break;
                }

            return retValue;
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
