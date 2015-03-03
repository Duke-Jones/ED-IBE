using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegulatedNoise
{
    public partial class Form1
    {
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://forums.frontier.co.uk/showthread.php?t=68771");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.apache.org/licenses/LICENSE-2.0");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.codeproject.com/Articles/452052/Build-Your-Own-Web-Server");
        }

        private void linkLabel4_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/zeromq/libzmq/blob/master/COPYING.LESSER");
        }

        private void linkLabel6_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://forums.frontier.co.uk/showthread.php?t=76190");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/zeromq/clrzmq/blob/master/LICENSE");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://starchart.club/map/");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://raw.githubusercontent.com/JamesNK/Newtonsoft.Json/master/LICENSE.md");
        }

        private void bShowStationAtStarchartDotInfo_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://starchart.club/map/system/" + CombinedNameToSystemName(cmbStation.Text));
        }

        private void bShowStationToStationRouteAtStarchartDotClub_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://starchart.club/map/route/" + CombinedNameToSystemName(cmbStationToStationFrom.Text) + @"/" + CombinedNameToSystemName(cmbStationToStationTo.Text) + @"/@" + CombinedNameToSystemName(cmbStationToStationFrom.Text));
        }

        private void bShowStationRestrictionAtStarchartDotClub_Click(object sender, EventArgs e)
        {
            var systemName = cbIncludeWithinRegionOfStation.Text;
            if (systemName == "<Current System>")
            {
                systemName = tbCurrentSystemFromLogs.Text;
            }
            Process.Start(@"http://starchart.club/map/system/" + systemName);
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.davek.com.au/td/");
        }

    }
}
