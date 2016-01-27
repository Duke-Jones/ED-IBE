using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IBE.Enums_and_Utility_Classes
{
    public static class VersionHelper
    {
        static public String Parts(Version version, Int32 count)
        {
            String parts = "";

            for (int i = 1; i <= count; i++)
            {
                switch (i)
                {
                    case 1:
                        parts += version.Major;
                        break;

                    case 2:
                        parts += "." + version.Minor;
                        break;

                    case 3:
                        parts += "." + version.Build;
                        break;

                    case 4:
                        parts += "." + version.Revision;
                        break;

                    default:
                        break;
                }
            }

            return parts;
        }
    }
}
