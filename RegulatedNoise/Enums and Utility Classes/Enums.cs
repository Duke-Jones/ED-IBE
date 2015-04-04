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
        ChangeGridSort,
        MaximizeWindow
    }

    public enum ThreadLoggerType
    {
        Form,
        Webserver,
        Ocr,
        EddnSubscriber,
        App, 
        Exception,
        FileScanner
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

    public enum enLanguage
    {
        eng,
        ger,
        fra
    }

    public enum enCommodityLevel
    {
        LOW  = 0,
        MED  = 1,
        HIGH = 2
    }

}
