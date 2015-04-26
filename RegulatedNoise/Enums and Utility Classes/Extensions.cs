using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegulatedNoise;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    
    static class Extensions_CheckBox
    {
        public static int? toNInt(this CheckBox thisCheckBox)
        {
            int? retValue = null ;

            switch (thisCheckBox.CheckState)
            {
                case CheckState.Checked:
                    retValue = 1;
                    break;
                case CheckState.Indeterminate:
                    retValue = null;
                    break;
                case CheckState.Unchecked:
                    retValue = 0;
                    break;
                default:
                    break;
            }

            return retValue;
        }
    }

    static class Extensions_IntNullable
    {
        public static CheckState toCheckState(this int? thisInt)
        {
            CheckState retValue = CheckState.Indeterminate;

            switch (thisInt)
	        {
                case null:
                    retValue = CheckState.Indeterminate;
                    break;
                case 0:
                    retValue = CheckState.Unchecked;
                    break;
                default:
                    retValue = CheckState.Checked;
                    break;
	        }

            return retValue;
        }

        public static string ToNString(this int? thisInt)
        {
            string retValue = null;

            switch (thisInt)
	        {
                case null:
                    retValue = Program.NULLSTRING;
                    break;
                default:
                    retValue = thisInt.ToString();
                    break;
	        }

            return retValue;
        }
    }

    static class Extensions_LongNullable
    {

        public static string ToNString(this long? thisLong)
        {
            if(thisLong == null)
                return Program.NULLSTRING;
            else
                return thisLong.ToString();
        }

        public static string ToNString(this long? thisLong, string format, IFormatProvider provider)
        {
            if(thisLong == null)
                return Program.NULLSTRING;
            else
                return ((long)thisLong).ToString(format, provider);
        }

        public static string ToNString(this int? thisInt, string format, IFormatProvider provider)
        {
            if(thisInt == null)
                return Program.NULLSTRING;
            else
                return ((int)thisInt).ToString(format, provider);
        }
    }
    
    
    static class Extensions_StringArrayNullable
    {
        /// <summary>
        /// clones a string array and return null if the source array is null
        /// </summary>
        /// <param name="thisString">a string or null</param>
        /// <returns></returns>
        public static string[] CloneN(this string[] thisStringArray)
        {
            if(thisStringArray == null)
                return null;
            else
                return (string[])thisStringArray.Clone();
        }
    }

    static class Extensions_StringNullable
    {
        /// <summary>
        /// converts a string that can be null to a string that represents null as a string ("undefined")
        /// </summary>
        /// <param name="thisString">a string or null</param>
        /// <returns></returns>
        public static string NToString(this string thisString)
        {
            if(thisString == null)
                return Program.NULLSTRING;
            else
                return thisString;
        }

        /// <summary>
        /// converts a possible null-representing string ("undefined") to a string or null
        /// </summary>
        /// <param name="thisString">a string or null</param>
        /// <returns></returns>
        public static string ToNString(this string thisString)
        {

            if(String.IsNullOrEmpty(thisString) || thisString.Equals(Program.NULLSTRING))
                return null;
            else
                return thisString;
        }

        public static Double ToDouble(this string thisString, string defaultValue="")
        {
            Double Value = 0.0;

            if(Double.TryParse(thisString, out Value))
                return Value;
            else
                return Double.Parse(defaultValue);
        }

        public static long? ToNLong(this string thisString, string defaultValue="")
        {
            long Value = 0;

            if(String.IsNullOrEmpty(thisString) || thisString.Equals(Program.NULLSTRING))
                return null;
            else
                if(long.TryParse(thisString, out Value))
                    return (long?)Value;
                else
                    return defaultValue.ToNLong();
        }

        public static int? ToNInt(this string thisString, string defaultValue="")
        {
            long Value = 0;

            if(String.IsNullOrEmpty(thisString) || thisString.Equals(Program.NULLSTRING))
                return null;
            else
                if(long.TryParse(thisString, out Value))
                    return (int?)Value;
                else
                    return defaultValue.ToNInt();
        }


    }

    static class Extensions_Object
    {
        /// <summary>
        /// converts a string that can be null to a string that represents null as a string ("undefined")
        /// </summary>
        /// <param name="thisString">a string or null</param>
        /// <returns></returns>
        public static string NToString(this Object thisObject)
        {

            if(thisObject == null)
                return Program.NULLSTRING;
            else
                return thisObject.ToString();
        }

    }



}
