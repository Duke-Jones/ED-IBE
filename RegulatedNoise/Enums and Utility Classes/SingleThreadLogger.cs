using System;
using System.IO;

namespace RegulatedNoise
{
    class SingleThreadLogger
    {
        private readonly string _logPathName;

        public SingleThreadLogger(ThreadLoggerType threadLoggerType)
        {
            if (!Directory.Exists(".//Logs"))
                Directory.CreateDirectory(".//Logs");

            _logPathName = ".//Logs//" + threadLoggerType + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")+Guid.NewGuid()+".log";
        }

        public void Log(string logMessage, bool error = false)
        {
            File.AppendAllText(_logPathName,DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + (error ? ": ERROR: " : ": ") + logMessage + Environment.NewLine);
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
