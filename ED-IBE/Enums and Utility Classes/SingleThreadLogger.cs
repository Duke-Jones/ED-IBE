using System;
using System.IO;

namespace IBE
{
    public class SingleThreadLogger
    {
        private readonly string _logPathName;

        public SingleThreadLogger(ThreadLoggerType threadLoggerType)
        {
            String destPath = Program.GetDataPath("Logs");
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            _logPathName = Path.Combine(destPath, string.Format("{0}_{1:yyyy-MM-dd HH-mm-ss}{2}.log", threadLoggerType, DateTime.Now, Guid.NewGuid()));
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
