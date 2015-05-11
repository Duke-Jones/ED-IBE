using System;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise
{
    public class EddnMessageEventArgs : EventArgs
    {
        public readonly EddnMessage Message;

        public EddnMessageEventArgs(EddnMessage message)
        {
            Message = message;
        }
    }
}