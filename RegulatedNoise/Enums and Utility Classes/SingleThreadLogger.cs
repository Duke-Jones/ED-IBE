using System;
using System.IO;

namespace RegulatedNoise
{
    class SingleThreadLogger
    {
	    private readonly string _logPathName;

        public SingleThreadLogger(ThreadLoggerType threadLoggerType)
        {
			  if (!Directory.Exists(ApplicationContext.LOGS_PATH))
				  Directory.CreateDirectory(ApplicationContext.LOGS_PATH);

            _logPathName = Path.Combine(ApplicationContext.LOGS_PATH, threadLoggerType + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")+Guid.NewGuid()+".log");
        }

        public void Log(string logMessage, bool error = false)
        {
            File.AppendAllText(_logPathName, DateTime.Now.ToString("HH:mm:ss") + (error ? ": ERROR: " : ": ") + logMessage + Environment.NewLine);
        }

        public string LogPathName
        {
            get
            {
                return _logPathName;
            }
        }

    }
}
