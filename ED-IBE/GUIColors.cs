using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Caching;

namespace IBE
{
    public class GUIColors
    {
        private bool _UseColors;
        public const String DB_GROUPNAME         = "GUIColors";

        private MemoryCache        m_DataCache                      = MemoryCache.Default;

        public enum ColorNames
        {
            Default_ForeColor,
            Default_BackColor,
            Marked_ForeColor,
            Marked_BackColor,
            Marked_ForeColor1,
            Marked_BackColor1
        }

        private Dictionary<ColorNames, Color> defaultColors = new Dictionary<ColorNames,Color>() { {ColorNames.Default_ForeColor  ,  Color.FromKnownColor(KnownColor.ControlText)}, 
                                                                                                   {ColorNames.Default_BackColor  ,  Color.FromKnownColor(KnownColor.Control)}, 
                                                                                                   {ColorNames.Marked_ForeColor   ,  Color.FromKnownColor(KnownColor.ControlText)}, 
                                                                                                   {ColorNames.Marked_BackColor   ,  Color.FromArgb(0xFF, 0x80, 0x00)},
                                                                                                   {ColorNames.Marked_ForeColor1  ,  Color.FromKnownColor(KnownColor.ControlText)}, 
                                                                                                   {ColorNames.Marked_BackColor1  ,  Color.FromArgb(0xFF, 0xD8, 0x00)}  };

        private Dictionary<ColorNames, Color> defaultColors_Pr1 = new Dictionary<ColorNames,Color>() { {ColorNames.Default_ForeColor  ,  Color.FromArgb(0xFF, 0x80, 0x00)}, 
                                                                                                       {ColorNames.Default_BackColor  ,  Color.Black}, 
                                                                                                       {ColorNames.Marked_ForeColor   ,  Color.Black}, 
                                                                                                       {ColorNames.Marked_BackColor   ,  Color.FromArgb(0xFF, 0x80, 0x00)},
                                                                                                       {ColorNames.Marked_ForeColor1  ,  Color.Black}, 
                                                                                                       {ColorNames.Marked_BackColor1  ,  Color.FromArgb(0xFF, 0xD8, 0x00)}  };

        private Dictionary<ColorNames, Color> usedPresets;

        public GUIColors()
        {
            try
            {
                if(UsePreset == 1)
                    usedPresets = defaultColors;
                else
                    usedPresets = defaultColors_Pr1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating object", ex);
            }

        }

        /// <summary>
        /// gets or sets if the colors should be used
        /// </summary>
        public bool UseColors
        {
            get
            {
                return Program.DBCon.getIniValue<Boolean>(DB_GROUPNAME, "ColorsActive", false.ToString(), false);
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, "ColorsActive", value.ToString());
            }
        }

        /// <summary>
        /// gets or sets the used preset dictionary
        /// </summary>
        public Int32 UsePreset
        {
            get
            {
                return Program.DBCon.getIniValue<Int32>(DB_GROUPNAME, "UsedPreset", "1", false);
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, "UsedPreset", value.ToString());

                if(value == 1)
                    usedPresets = defaultColors;
                else
                    usedPresets = defaultColors_Pr1;

            }
        }

        /// <summary>
        ///  resets all colors the the default values
        /// </summary>
        public void ResetColors()
        {
            try
            {
                foreach (ColorNames colorName in Enum.GetValues(typeof(ColorNames)))
	            {
		            Program.DBCon.setIniValue(DB_GROUPNAME, colorName.ToString(), ((Color)(usedPresets[colorName])).ToArgb().ToString());
                    m_DataCache.Remove(colorName.ToString());
                    m_DataCache.Add(colorName.ToString(), ((Color)(usedPresets[colorName])), DateTimeOffset.Now.AddMinutes(10));
	            }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while resetting the GUI-colors to defaults", ex);
            }
        }

        /// <summary>
        /// gets the color for a colorname
        /// </summary>
        /// <param name="currentColor"></param>
        /// <returns></returns>
        public Color GetColor(ColorNames currentColor)
	    {
            try
            {
                Color retValue;

                if(m_DataCache.Get(currentColor.ToString()) == null)
                { 
                    retValue = Color.FromArgb(Program.DBCon.getIniValue<int>(DB_GROUPNAME, currentColor.ToString(), ((Color)(usedPresets[currentColor])).ToArgb().ToString()));
                    m_DataCache.Add(currentColor.ToString(), retValue, DateTimeOffset.Now.AddMinutes(10));
                }
                else
                {
                    retValue = (Color)m_DataCache.Get(currentColor.ToString());
                }

                return retValue;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting a GUI-color", ex);
            }
	    }

        /// <summary>
        /// sets the color for a colorname
        /// </summary>
        /// <param name="currentColor"></param>
        /// <returns></returns>
        public void SetColor(ColorNames currentColor, Color colorValue)
	    {
            try
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, currentColor.ToString(), colorValue.ToArgb().ToString());
                
                m_DataCache.Remove(currentColor.ToString());
                m_DataCache.Add(currentColor.ToString(), colorValue, DateTimeOffset.Now.AddMinutes(10));
                           
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting a GUI-color", ex);
            }
	    }

        /// <summary>
        /// sets colors to a control and optional to it's subcontrols
        /// </summary>
        /// <param name="guiObject"></param>
        /// <param name="forecolorName"></param>
        /// <param name="backcolorName"></param>
        /// <param name="recursive"></param>
        internal void SetColorToObject(Control guiObject, ColorNames forecolorName, ColorNames backcolorName, bool recursive = true)
        {
            try
            {
                Color foreColor = GetColor(forecolorName);
                Color backColor = GetColor(backcolorName);

                Boolean guiReset = false;

                if ((!UseColors) && (forecolorName == ColorNames.Default_ForeColor) && (backcolorName == ColorNames.Default_BackColor))
                {
                    // reset to system colors
                    foreColor = Color.FromKnownColor(KnownColor.ControlText);
                    backColor = Color.FromKnownColor(KnownColor.Control);

                    guiReset = true;
                }

                SetColorToObjectExtracted(guiObject, foreColor, backColor, recursive, guiReset);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting colors of a gui object", ex);
            }
        }

        /// <summary>
        /// recursive worker for setting colors
        /// </summary>
        /// <param name="guiObject"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <param name="recursive"></param>
        /// <param name="guiReset"></param>
        private void SetColorToObjectExtracted(Control guiObject, Color foreColor, Color backColor, Boolean recursive, Boolean guiReset)
        {
            try
            {
                IBE.Enums_and_Utility_Classes.RNBaseForm.SetControlColors(guiObject, foreColor, backColor, guiReset);

                if(recursive)
                {
                    foreach (Control subControl in guiObject.Controls)
                    { 
                      SetColorToObjectExtracted(subControl, foreColor, backColor, recursive, guiReset);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting gui colors (recursive worker)", ex);
            }
        }
    }

}
