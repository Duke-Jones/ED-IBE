using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RegulatedNoise
{
    public class ProgramPaths
    {
        private String _ProgramPath;

        public String ProgramPath
        {
            get
            {
                String WantedPath;

                WantedPath = Program.DBCon.getIniValue("General", "PathProgram");

                if(!Directory.Exists(WantedPath))
                    Directory.CreateDirectory(WantedPath);

                return WantedPath;
            }
        }

        public String DataPath
        {
            get
            {
                String WantedPath;

                WantedPath = Program.DBCon.getIniValue("General", "PathData");

                if(!Directory.Exists(WantedPath))
                    Directory.CreateDirectory(WantedPath);

                return WantedPath;
            }
        }

        public String DataPath_temp
        {
            get
            {
                String WantedPath;
                WantedPath = Path.Combine(DataPath, "temp");

                if(!Directory.Exists(WantedPath))
                    Directory.CreateDirectory(WantedPath);

                return WantedPath;
            }
        }

        public String DataPath_Database
        {
            get
            {
                String WantedPath;
                WantedPath = Path.Combine(DataPath, "Database");

                if(!Directory.Exists(WantedPath))
                    Directory.CreateDirectory(WantedPath);

                return WantedPath;
            }
        }
    }
}
