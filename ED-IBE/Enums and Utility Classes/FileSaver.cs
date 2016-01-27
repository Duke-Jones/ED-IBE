using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    static class FileSaver
    {
        private static string extensionNew = "new";
        private static string extensionOld = "bak";

        public static void rotateSaveFiles(string Filename, bool BackupOldFile=true)
        {
            string newFile, backupFile;

            newFile     = String.Format("{0}_{2}{1}", Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename)), Path.GetExtension(Filename), extensionNew);
            backupFile  = String.Format("{0}_{2}{1}", Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename)), Path.GetExtension(Filename), extensionOld);

            // delete old backup
            if (File.Exists(backupFile))
                File.Delete(backupFile);

            // rename current file to old backup
            if (BackupOldFile && File.Exists(Filename))
                File.Move(Filename, backupFile);

            // rename new file to current file
            File.Move(newFile, Filename);
        }
    }
}
