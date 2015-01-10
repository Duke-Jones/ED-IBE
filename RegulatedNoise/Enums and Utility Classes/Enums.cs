using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise
{
    public enum AppDelegateType
    {
        AddEventToLog,
        UpdateSystemNameLiveFromLog,
        ChangeGridSort,
        MaximizeWindow
    }

    public enum ThreadLoggerType
    {
        Form,
        Webserver,
        Ocr,
        EddnSubscriber,
        App
    }

    public enum StationHasBlackMarket
    {
        Unknown,
        No,
        Yes
    }

    public enum StationPadSize
    {
        Unknown,
        Medium,
        Large
    }
}
