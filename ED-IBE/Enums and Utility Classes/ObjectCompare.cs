using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    static public class ObjectCompare
    {
        static public bool EqualsNullable(object Object1, object Object2)
        {
            bool retValue = false;

            if ((Object1 == null) && (Object2 == null))
            {
                retValue = true;
            }
            else if ((Object1 != null) && (Object2 != null))
            {
                if(Object1.GetType() == typeof(String[]))
                {
                    return StringArrayCompare.EqualsStrArray((String[])Object1, (String[])Object2);
                }
                else
                {
                    if (Object1.Equals(Object2))
                        retValue = true;
                    else
                        retValue = false;
                }
            }
            else
                retValue = false;

            return retValue;
        }
    }

    static public class StringArrayCompare
    {
        static public bool EqualsStrArray(string[] strArray1, string[] strArray2)
        {
            bool retValue = false;
                
            if(strArray1.Length == strArray2.Length)
            {
                retValue = true;

                foreach (string item in strArray1)
                {
                    if(strArray2.First(x => x.Equals(item, StringComparison.InvariantCultureIgnoreCase)) == null)
                    { 
                        retValue = false;
                        break;
                    }
                }
            }

            return retValue;
        }
    }
}
