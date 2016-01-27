using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IBE
{
    public class GUIColors
    {
        public Color Default_BackColor  { get; set; }
        public Color Default_ForeColor  { get; set; }

        public Color Marked_ForeColor   { get; set; }
        public Color Marked_BackColor   { get; set; }


        public GUIColors()
        {
            try
            {
                Default_ForeColor = SystemColors.ControlText;
                Default_BackColor = SystemColors.Control;

                Marked_ForeColor  = SystemColors.ControlText;
                Marked_BackColor  = Color.Coral;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating object", ex);
            }

        }

    }
}
