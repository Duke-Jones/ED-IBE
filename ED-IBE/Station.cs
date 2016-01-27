#if !EDDB_Data
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise
{
    public class Station
    {
        public string System { get; set; }
        public string Name { get; set; }
        public long LightSecondsFromStar { get; set; }
        public StationHasBlackMarket StationHasBlackMarket{ get; set; }
        public StationPadSize StationPadSize { get; set; }
    }
}
#endif
