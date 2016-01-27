using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    class StructureHelper
    {
        public static string CombinedNameToStationName(string combinedName)
        {
            String ret = String.Empty;

            if(combinedName.Trim().Length > 0)
            { 
                Int32 Index = combinedName.IndexOf("[");
                if(Index >= 0)
                    ret = combinedName.Substring(0, combinedName.IndexOf("[") - 1);
                else
                    ret = combinedName;
            }

            return ret;
        }

        public static string CombinedNameToSystemName(string combinedName)
        {
            try
            {
                var ret = combinedName.Substring(combinedName.IndexOf("[") + 1);
                ret = ret.TrimEnd(']');
                return ret;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
