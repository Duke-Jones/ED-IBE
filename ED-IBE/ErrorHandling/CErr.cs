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
            processError(ex, "");
        }

        static public void processError(Exception ex, string Infotext)
        {
            ErrorViewer errViewer = new ErrorViewer();
            errViewer.ShowDialog(ex, Infotext);
        }
    }
}
