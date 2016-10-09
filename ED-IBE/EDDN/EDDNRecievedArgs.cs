using System;
using System.Collections.Generic;
using System.Linq;

namespace IBE.EDDN
{
    public class EDDNRecievedArgs : EventArgs
    {
        public enum enMessageInfo
        {
            ParseError,
            UnknownData,
            Commodity_v1_Recieved,
            Commodity_v2_Recieved,
            Outfitting_v1_Recieved,
            Shipyard_v1_Recieved,
            Commodity_v3_Recieved,
            Outfitting_v2_Recieved,
            Shipyard_v2_Recieved
        }

        public string Message;
        public string RawData;
        public enMessageInfo InfoType;
        public object Data;
        public String Adress;
    }
}
