using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

// <summary>
// genaue Timerklasse, um Zeitmessungen durchzuführen (Auflösung ca. 10ms)
// </summary>
// <remarks></remarks>
public class PerformanceTimer{

    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceCounter(out Int64 lpPerformanceCount);

    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceFrequency(out Int64 lpFrequency);

    private Int64   m_Counter1;          // start counter
    private Int64   m_Counter2   =0;     // end counter
    private Int64   m_Frequency  =0;     // tick frequency
    private String  m_Name;              // name of timer
    private Boolean m_Started;           // flag : "started"

    public PerformanceTimer()
    {
        m_Started = false;
        QueryPerformanceFrequency(out m_Frequency);
    }

    // <summary>
    // Startet die Messung, registriert die aktuelle Zeit
    // </summary>
    // <remarks></remarks>
    public void startMeasuring()
    {
        m_Started   = true;
        m_Name      = String.Empty;
        QueryPerformanceCounter(out m_Counter1);
    }

    // <summary>
    // Startet die Messung, registriert die aktuelle Zeit
    // </summary>
    // <param name="Name"></param>
    // <remarks></remarks>
    public void startMeasuring(String Name)
    {
        m_Started   = true;
        m_Name      = Name;
        QueryPerformanceCounter(out m_Counter1);
    }

    // <summary>
    // Beendet die Messung und gibt die verstrichende Zeit in ms zurück
    // </summary>
    // <returns></returns>
    // <remarks></remarks>
    public Int64 stopMeasuring() 
    {
        if (m_Started)
        {
            m_Started = false;
            QueryPerformanceCounter(out m_Counter2);
            return Convert.ToInt64(Convert.ToInt64(((m_Counter2 - m_Counter1) * 1000)) / m_Frequency );
        }
        else
        {
            return 0;
        }
    }

    // <summary>
    // Gibt die verstrichende Zeit in ms zurück und lässt die Messung weiterlaufen
    // </summary>
    // <returns></returns>
    // <remarks></remarks>
    public Int64 currentMeasuring()
    {
        if (m_Started)
        {
            QueryPerformanceCounter(out m_Counter2);
            return Convert.ToInt64(Convert.ToInt64(((m_Counter2 - m_Counter1) * 1000)) / m_Frequency );
        }
        else
        {
            return 0; 
        }   
    }

    // <summary>
    // Setzt den Beginn der Zeitmessung auf den übergeben Wert
    // </summary>
    // <remarks></remarks>
    public void setcurrentMeasuring(Int64 newMilliseconds)
    {
        QueryPerformanceCounter(out m_Counter1);
        m_Counter1 = m_Counter1 - Convert.ToInt64(Math.Round((double)(newMilliseconds * m_Frequency) / 1000, 0));
    }
   
    // <summary>
    // Beendet die Messung und gibt die verstrichende Zeit als Debug-Information aus
    // </summary>
    // <returns></returns>
    // <remarks></remarks>
    public Int64 stopMeasuringAndPrint()
    {
        Int64 ms = stopMeasuring();
        m_Started = false;
        Debug.Print(string.Format("{0} ms runtime / {1}", ms.ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("sv-SE")), m_Name));
        return ms;
    }

    // <summary>
    // Ermittelt die optimale Wartezeit bei Zeitschleifen in Abhängigkeit zur zulässigen Gesamtwartezeit
    // </summary>
    // <param name="TotalWaitTime">zulässige Gesamtwartezeit</param>
    // <returns></returns>
    // <remarks></remarks>
    public static Int64 WaitTimeOptimum(Int64 TotalWaitTime)
    {
        if ((TotalWaitTime / 10) > 50)
            return 100;
        else if ((TotalWaitTime / 10) == 0)
            return 1;
        else
            return TotalWaitTime / 10;
    }

    // <summary>
    // legt den Thread optimal schlafen und gibt im Anschluß
    // zurück, ob die maximale Wartezeit abgelaufen ist
    // </summary>
    // <param name="TotalWaitTime"></param>
    // <returns>1=Zeit abgelaufen, 0=Zeit nicht abgelaufen</returns>
    // <remarks></remarks>
    public Int64 SleepOptimum(Int64 TotalWaitTime)
    {
        Int64 SleepTime=0;     // zu setzende SleepTime
        Int64 Rest=0;          // Restzeit bis Maximalzeit verstrichen
      
        try{
            // optimale Schlafzeit
            SleepTime = WaitTimeOptimum(TotalWaitTime);

            // Restzeit bis Ablauf der Maximalzeit
            Rest = TotalWaitTime - currentMeasuring();
            if (Rest < SleepTime)
            {
                // Rest ist kleiner, aktuelle Restlaufzeit neu berechnen und übernehmen
                SleepTime = TotalWaitTime - currentMeasuring();
            }

            if (Rest > 0)
            {
                System.Threading.Thread.Sleep((Int32)SleepTime);

                if (currentMeasuring() >= TotalWaitTime)
                    return 1;
                else
                    return 0;
            }    
            else
                return 1;
            
        }
        catch (Exception ex)
        {
            throw new Exception(String.Format("Fehler beim Schlafenlegen des Threads mit SleepOptimum, TotalWaitTime={0:d}, Rest={1:d}, SleepTime={2:d}", 
                                            TotalWaitTime, Rest, SleepTime), ex);   
        }
             

    }

    // <summary>
    // Gibt zurück, ob der Timer gestartet wurde
    // </summary>
    // <value></value>
    // <returns></returns>
    // <remarks></remarks>
    public bool isStarted
    {
       get
       {
           return m_Started; 
       }
    }

    public void PrintAndReset(string Info)
    {
        Debug.Print(string.Format("{0} {1} ms runtime", Info, currentMeasuring().ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("sv-SE"))));
        startMeasuring();
    }

}
