using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise
{
    /// <summary>
    /// class for holding the current state/location of the commander/ship
    /// </summary>
    public class Condition
    {
        public const string STR_Scanning = "scanning...";

        public String   System  { get; set; }
        public String   Station { get; set; }

        public Condition()
        {
            System  = STR_Scanning;
            Station = STR_Scanning;
        }
    }
}
