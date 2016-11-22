using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiColumnComboBox
{
    class ComboBoxMC : ComboBox
    {
        public ComboBoxMC()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DrawItem += ComboBoxMC_DrawItem;
            this.DropDown += ComboBoxMC_DropDown;
        }

        private void ComboBoxMC_DropDown(object sender, EventArgs e)
        {
            //int newWidth;
            //foreach (string s in ((ComboBox)sender).Items)
            //{
            //    newWidth = (int) g.MeasureString(s, font).Width 
            //        + vertScrollBarWidth;
            //    if (width < newWidth )
            //    {
            //        width = newWidth;
            //    }

            //}
        }

        public enum ColumnWidthTypes
        {
            Absolute,
            Relative
        }


        ColumnWidthTypes columnWidthType = ColumnWidthTypes.Relative;

        public ColumnWidthTypes ColumnWidthType
        {
            get
            {
                return columnWidthType;
            }
            set
            {
                columnWidthType = value;
            }
        }

        public readonly     List<String> DisplayMembers     = new List<string>();
        public readonly     List<Int32>  ColumnWidths       = new List<Int32>();
        public readonly     List<Int32>  ColumnWidthMax     = new List<Int32>();

        public String Separator { get; set; }

        private void ComboBoxMC_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle destinationRect;
            Int32 newRectStartOffset  = 0;
            Int32 currentWidth = 0;

            // draw the default background
            e.DrawBackground();

            if(e.Index >= 0)
            {

                DataRowView drv = (DataRowView)this.Items[e.Index];

                destinationRect       = e.Bounds;

                for (int i = 0; i < DisplayMembers.Count; i++)
                {
                    if(columnWidthType == ColumnWidthTypes.Absolute)
                        currentWidth = ColumnWidths[i];
                    else
                        currentWidth = (e.Bounds.Width * ColumnWidths[i]) / 100;

                    destinationRect.Offset(new Point(newRectStartOffset, 0));
                    destinationRect.Width = currentWidth;

                    // Draw the text on the first column
                    using (SolidBrush sb = new SolidBrush(e.ForeColor))
                        e.Graphics.DrawString(GetDisplayItem(drv, i), e.Font, sb, destinationRect);

                    newRectStartOffset = destinationRect.Width;
                }

                
            }

        }

        /// <summary>
        /// returns the i-th part of the display members
        /// </summary>
        /// <param name="drv">datarow with string data</param>
        /// <param name="i">index of the needed part</param>
        /// <returns></returns>
        private string GetDisplayItem(DataRowView drv, int i)
        {
            if(DisplayMembers[i].StartsWith("<SEP>"))
            {
                if(DisplayMembers[i].Contains(";"))
                {
                    List<String> parts = DisplayMembers[i].Split(new char[] {';'}).ToList();
                    return parts[1];
                }
                else
                    return Separator;                

                
            }
            else
            {
                if(DisplayMembers[i].Contains(";"))
                {
                    List<String> parts = DisplayMembers[i].Split(new char[] {';'}).ToList();
                    return String.Format(parts[1], drv[parts[0]]);
                }
                else
                    return drv[DisplayMembers[i]].ToString();
            }
        }
    }
}
