using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBE;
using System.Reflection;

namespace IBE.Enums_and_Utility_Classes
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

        public static string ToNString(this int? thisInt, String NullString)
        {
            string retValue = null;

            switch (thisInt)
	        {
                case null:
                    retValue = NullString;
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
    
    static class Extensions_Int32ArrayNullable
    {
        /// <summary>
        /// clones a string array and return null if the source array is null
        /// </summary>
        /// <param name="thisString">a string or null</param>
        /// <returns></returns>
        public static Int32[] CloneN(this Int32[] thisInt32Array)
        {
            if(thisInt32Array == null)
                return null;
            else
                return (Int32[])thisInt32Array.Clone();
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
        public static Boolean EqualsNullOrEmpty(this string thisString, String otherString)
        {
            if (string.IsNullOrEmpty(thisString))
            {
                return string.IsNullOrEmpty(otherString);
            }
            else
            {
                return string.Equals(thisString, otherString);
            }
        }

        /// <summary>
        /// converts a string that can be null to a string that represents null as a string ("undefined")
        /// </summary>
        /// <param name="thisString">a string or null</param>
        /// <returns></returns>
        public static string NToString(this string thisString, String NullString)
        {
            if(thisString == null)
                return NullString;
            else
                return thisString;
        }

        /// <summary>
        /// converts a string that can be null to a string that represents null as a string ("undefined")
        /// </summary>
        /// <param name="thisString">a string or null</param>
        /// <returns></returns>
        public static string NToString(this string thisString)
        {
            return NToString(thisString, Program.NULLSTRING);
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

    static class Extensions_Control
    {
        /// <summary>
        /// Checks whether a control or its parent is in design mode.
        /// </summary>
        /// <param name="c">The control to check.</param>
        /// <returns>Returns TRUE if in design mode, false otherwise.</returns>
        public static bool IsDesignMode(this System.Windows.Forms.Control c)
        {
            if ( c == null )
            {
                return false;
            }
            else
            {
                while ( c != null )
                {
                if ( c.Site != null && c.Site.DesignMode )
                {
                    return true;
                }
                else
                {
                    c = c.Parent;
                }
                }

                return false;
            }
        }

        public static bool SetControlColors(this Control controlObject, System.Drawing.Color newForeColor, System.Drawing.Color newBackColor, Boolean reset = false)
        {
            bool noBackColor;
            var props = controlObject.GetType().GetProperties().Select(y => y.Name);

            noBackColor = false;

            controlObject.BackColor = newBackColor;
            controlObject.ForeColor = newForeColor;

            if (props.Contains("FlatStyle"))
            {
                var prop = controlObject.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

                if(reset)
                    prop.SetValue(controlObject, FlatStyle.Standard);
                else
                    prop.SetValue(controlObject, FlatStyle.Flat);
            }

            if (props.Contains("FlatStyle"))
            {
                var prop = controlObject.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

                prop.SetValue(controlObject, FlatStyle.Flat);
            }
            if (props.Contains("BorderStyle") && controlObject.GetType() != typeof(Label))
            {
                var prop = controlObject.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

                prop.SetValue(controlObject, BorderStyle.FixedSingle);
            }
            if (props.Contains("LinkColor"))
            {
                var prop = controlObject.GetType().GetProperty("LinkColor", BindingFlags.Public | BindingFlags.Instance);

                prop.SetValue(controlObject, newForeColor);
            }
            if (props.Contains("BackColor_ro"))
            {
                var prop = controlObject.GetType().GetProperty("BackColor_ro", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newBackColor);
            }
            if (props.Contains("ForeColor_ro"))
            {
                var prop = controlObject.GetType().GetProperty("ForeColor_ro", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newForeColor);
            }
            if (props.Contains("BackgroundColor"))
            {
                var prop = controlObject.GetType().GetProperty("BackgroundColor", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newBackColor);
            }
            if (props.Contains("GridColor"))
            {
                var prop = controlObject.GetType().GetProperty("GridColor", BindingFlags.Public | BindingFlags.Instance);
                prop.SetValue(controlObject, newForeColor);
            }
            if (props.Contains("DefaultCellStyle"))
            {
                // DataGridView
                var prop = controlObject.GetType().GetProperty("DefaultCellStyle", BindingFlags.Public | BindingFlags.Instance);

                var propsCellStyle = prop.GetType().GetProperties().Select(y => y.Name);

                if (propsCellStyle.Contains("BackColor"))
                {
                    var prop2 = propsCellStyle.GetType().GetProperty("BackColor", BindingFlags.Public | BindingFlags.Instance);
                    prop2.SetValue(controlObject, newBackColor);
                }
                if (propsCellStyle.Contains("ForeColor"))
                {
                    var prop2 = propsCellStyle.GetType().GetProperty("ForeColor", BindingFlags.Public | BindingFlags.Instance);
                    prop2.SetValue(controlObject, newForeColor);
                }
            }
            if (props.Contains("Columns") && controlObject.GetType() == typeof(DataGridViewExt))
            {

                DataGridViewExt dgv = (DataGridViewExt)controlObject;

                dgv.EnableHeadersVisualStyles = false;

                dgv.RowHeadersDefaultCellStyle.BackColor = newForeColor;
                dgv.RowHeadersDefaultCellStyle.ForeColor = newBackColor;

                dgv.ColumnHeadersDefaultCellStyle.BackColor = newForeColor;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = newBackColor;


                // DataGridView
                var prop = controlObject.GetType().GetProperty("Columns", BindingFlags.Public | BindingFlags.Instance);

                var propValues = (DataGridViewColumnCollection)prop.GetValue(controlObject, null);

                foreach (DataGridViewColumn propValue in propValues)
                {
                    propValue.DefaultCellStyle.ForeColor = newForeColor;
                    propValue.DefaultCellStyle.BackColor = newBackColor;
                    propValue.HeaderCell.Style.BackColor = newForeColor;
                    propValue.HeaderCell.Style.ForeColor = newBackColor;
                }
            }
            return noBackColor;
        }

        // Recurse controls on form
        public static IEnumerable<Control> GetAllControls(this Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAllControls(ctrl))
                                      .Concat(controls);
        }

        public static void Retheme(this Control controlObject)
        {
            bool noBackColor = false;

            Dictionary<String, Boolean> ignoreList = new Dictionary<string, bool>() {{"lblDonate", false},
                                                                                     {"lbEDDNInfo", false}};

            try
            {
                if (!Program.Colors.UseColors) return;

                var x = controlObject.GetAllControls();

                var f = Program.Colors.GetColor(GUIColors.ColorNames.Default_ForeColor);
                var b = Program.Colors.GetColor(GUIColors.ColorNames.Default_BackColor);

                foreach (Control c in x)
                {
                    if (!ignoreList.ContainsKey(c.Name))
                    {
                        noBackColor = c.SetControlColors(f, b);
                    }
                }

                //if(!noBackColor)
                controlObject.BackColor = b;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retheming-function", ex);
            }
        }
    }

    static class Extensions_Event
    {

        /// <summary>
        /// adds a null-checking "Raise" mechanism to events
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Raise <T> (this EventHandler <T> eventHandler, Object sender, T e) where T : EventArgs
        {
           if (eventHandler != null) {
              eventHandler (sender, e);
           }
        }
    }

    static class Extensions_DateTime
    {
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }

    static class Extensions_IEnumerable
    {
        public static bool ContentEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {

            var cnt = new Dictionary<T, int>();

            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]++;
                else
                    cnt.Add(s, 1);
            }

            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                    cnt[s]--;
                else
                    return false;
            }

            return cnt.Values.All(c => c == 0);
        }
    }

}
