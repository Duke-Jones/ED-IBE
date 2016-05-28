using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.Enums_and_Utility_Classes
{
    public class Point3Dbl
    {
        public Point3Dbl()
        {
            X = null;   
            Y = null;   
            Z = null;   
        }

        public Point3Dbl(double? xValue, double? yValue, double? zValue)
        {
            X = xValue;   
            Y = xValue;   
            Z = xValue;   
        }

        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }

        public bool Valid 
        { 
            get
            {
                return X.HasValue && Y.HasValue && Z.HasValue;
            }
        }

        /// <summary>
        /// parses 
        /// </summary>
        /// <param name="coordinateString"></param>
        internal static Boolean TryParse(string coordinateString, out Point3Dbl coordinate)
        {
            System.Globalization.CultureInfo customCulture  = System.Globalization.CultureInfo.InvariantCulture;
            var style = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowDecimalPoint;
            Point3Dbl parsedCoordinate = new Point3Dbl();

            Boolean retValue = true;

            String[] parts = coordinateString.Split(new char[] {','});

            if((parts.GetUpperBound(0) == 2))
            {
                for (int i = 0; i <= parts.GetUpperBound(0); i++)
                {
                    Double dblValue = 0.0;

                    if(Double.TryParse(parts[i], style, customCulture, out dblValue))
                    { 
                        switch (i)
                        {
                            case 0:
                                parsedCoordinate.X = dblValue;
                                break;
                            case 1:
                                parsedCoordinate.Y = dblValue;
                                break;
                            case 2:
                                parsedCoordinate.Z = dblValue;
                                break;
                        }
                    }
                    else
                        retValue = false;
                }
            }
            else
                retValue = false;

            if(retValue)
            {
                coordinate = parsedCoordinate;
            }
            else
            {
                coordinate = new Point3Dbl();
            }

            return retValue;
        }

        public override String ToString()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
             
            return String.Format("{0},{1},{2}", 
                                 X.ToString().Replace(customCulture.NumberFormat.NumberDecimalSeparator, "."), 
                                 Y.ToString().Replace(customCulture.NumberFormat.NumberDecimalSeparator, "."), 
                                 Z.ToString().Replace(customCulture.NumberFormat.NumberDecimalSeparator, "."));
        }
    }
}
