using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.EDDN
{
    public class OutfittingObject
    {
        public const String Cat_Hardpoint     = "hardpoint";
        public const String Cat_Utility       = "utility";
        public const String Cat_Standard      = "standard";
        public const String Cat_Internal      = "internal";

        public String Category { get; set; }
        public String Name { get; set; }
        public String Class { get; set; }
        public String Ship { get; set; }
        public String Rating { get; set; }
        public String Mount { get; set; }
        public String Guidance { get; set; }
        public String Entitlement { get; set; }
        public String Enabled { get; set; }
        public String Priority { get; set; }
        public Int32 Id { get; set; }

    }
}
