using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;

namespace IBE
{
    
    public static class CErr
    {
        static public void processError(Exception ex)
        {
            processError(ex, "", true);
        }

        static public void processError(Exception ex, string Infotext)
        {
            processError(ex, Infotext, true);
        }

        static public void processError(Exception ex, string Infotext, Boolean ignoreAllowed)
        {
            ErrorViewer errViewer = new ErrorViewer();
            errViewer.ShowDialog(ex, Infotext, ignoreAllowed);
        }

        internal static string getErrorString(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
