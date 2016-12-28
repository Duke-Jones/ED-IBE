using System;
using System.IO;

namespace IBE
{
    public class SingleThreadLogger
    {
        private readonly string _logPathName;
        private Int32 maxFileSize = 10 * 1024 * 1024; // max. file size in MB

        public SingleThreadLogger(ThreadLoggerType threadLoggerType, String destPath = "", Boolean reuseLog = false)
        {
            if(String.IsNullOrEmpty(destPath))
                destPath = Program.GetDataPath("Logs");

            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            if(reuseLog)
                _logPathName = Path.Combine(destPath, string.Format("{0}.log", threadLoggerType));
            else
                _logPathName = Path.Combine(destPath, string.Format("{0}_{1:yyyy-MM-dd HH-mm-ss}.log", threadLoggerType, DateTime.UtcNow));
                
        }

        public void Log(string logMessage)
        {
            File.AppendAllText(_logPathName, string.Format("{0:dd.MM.yyyy HH:mm:ss} : {1}{2}", DateTime.UtcNow, logMessage, Environment.NewLine));

            if(new FileInfo(_logPathName).Length > maxFileSize)
            {
                try
                {
                    File.Copy(_logPathName, _logPathName+".old", true);
                }
                catch (Exception)
                {
                }
            }

        }

        public string logPathName
        {
            get
            {
                return _logPathName;
            }
        }
    }
}
