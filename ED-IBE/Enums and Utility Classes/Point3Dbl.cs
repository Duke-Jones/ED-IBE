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
    }
}
