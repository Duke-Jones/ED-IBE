using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegulatedNoise
{
    public partial class ProgressView : RegulatedNoise.Enums_and_Utility_Classes.RNBaseForm
    {
        PerformanceTimer m_PTimer = new PerformanceTimer();
        //string THISOBJECTNAME = "FormProgressView";
        public delegate void del_Sub_PInt32(Int32 Value);
        public delegate void del_Sub_PString(string Text);
        bool m_Cancelled;           //  True: Abbruch wurde angefordert

        public ProgressView()
        {
            InitializeComponent();

            m_Cancelled = false;
            m_PTimer.startMeasuring();
        }


    
    // <summary>
    // shows the progress view
    // </summary>
    // <param name="Info"></param>
    // <remarks></remarks>
    public void progressStart(string Info="") {
        try {
            this.Show();
            progressUpdate(0);
            progressInfo(Info);
            Application.DoEvents();
            this.TopMost = true;
        }
        catch (Exception ex) {
            throw new Exception("error while starting the progress view", ex);
        }
    }
    
    // <summary>
    // set the progress to a new value
    // </summary>
    // <param name="Value">progress current value</param>
    // <param name="Total">total value</param>
    // <remarks></remarks>
    public void progressUpdate(Int32 Value, Int32 Total) {
        Int32 ProzValue;
        try {
            if ((Total > 0)) {
                ProzValue = (int)Math.Round((double)(Value) / (double)(Total) * 100.0, 0);

                if ((ProzValue < 0)) {
                    ProzValue = 0;
                }
                else if ((ProzValue > 100)) {
                    ProzValue = 100;
                }
            }
            else {
                ProzValue = 0;
            }
            if (m_PTimer.currentMeasuring() >= 50)
            { 
                m_PTimer.startMeasuring();
                progressUpdate(ProzValue);
            }
            
        }
        catch (Exception ex) {
            throw new Exception("error while setting a new value", ex);
        }
    }
    
    // <summary>
    // set the progress to a new value
    // </summary>
    // <param name="Percent">progress in percent</param>
    // <remarks></remarks>
    private void progressUpdate(Int32 Percent) {
        try {
            if (ProgressBar1.InvokeRequired) {
                ProgressBar1.Invoke(new del_Sub_PInt32(this.progressUpdate), Percent);
            }
            else {
                ProgressBar1.Value = Percent;
                if (Percent>0 && Percent<100)
                    ProgressBar1.Value = Percent-1;

                lblProgress.Text = string.Format("{0}%", Percent);
                ProgressBar1.Refresh();
                Application.DoEvents();
            }
        }
        catch (Exception ex) {
            throw new Exception("error while setting a new value", ex);
        }
    }
    
    // <summary>
    // sets a new info string
    // </summary>
    // <param name="Info">new information</param>
    // <remarks></remarks>
    public void progressInfo(string Info) {
        try {
            if (lblInfotext.InvokeRequired) {
                lblInfotext.Invoke(new del_Sub_PString(this.progressInfo), Info);
            }
            else {
                if (string.IsNullOrEmpty(Info)) {
                    this.Height = 125;
                }
                else {
                    this.Height = 161;
                }
                lblInfotext.Text = Info;
                Application.DoEvents();
            }
        }
        catch (Exception ex) {
            throw new Exception("error while setting a newinfo string", ex);
        }
    }
    
    // <summary>
    // closes the progress view
    // </summary>
    // <remarks></remarks>
    public void progressStop() {
        try {
            this.Close();
        }
        catch (Exception ex) {
            throw new Exception("error while closing the progress view", ex);
        }
    }
    
    // <summary>
    // show if the user has "cancel" clicked
    // </summary>
    // <value></value>
    // <returns></returns>
    // <remarks></remarks>
    public bool Cancelled {
        get {
            return m_Cancelled;
        }
    }
    
    /// <summary>
    /// event function of cancel button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmdCancel_Click(object sender, System.EventArgs e) {
        m_Cancelled = true;
    }

}
}
