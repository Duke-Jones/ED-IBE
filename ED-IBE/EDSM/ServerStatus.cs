using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.EDSM
{
    public enum ServerStates
    {
        Success     = 2,
        Warning     = 1,
        Danger      = 0
    }


    public class ServerStatus
    {
        public DateTime         LastUpdate;
        public String           Type;
        public String           Message;
        public ServerStates     Status;
    }
}
