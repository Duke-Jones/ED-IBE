using System;
using System.Diagnostics;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegulatedNoise.Test
{
    [TestClass]
    public class EDMilyWayTest
    {
        [TestMethod]
        public void datafile_download_poc()
        {
            using (var client = new WebClient())
            {
                var timer = new Stopwatch();
                timer.Start();
                var url = "http://eddb.io/archive/v3/commodities.json";
                //var url = "http://eddb.io/archive/v3/systems.json";
                string content = client.DownloadString(new Uri(url));
                timer.Stop();
                Debug.WriteLine("commodities download time: " + timer.Elapsed);
                timer.Reset();
                timer.Start();
                content = client.DownloadStringTaskAsync(new Uri(url)).Result;
                timer.Stop();
                Debug.WriteLine("commodities download time: " + timer.Elapsed);
                timer.Reset();
                timer.Start();
                bool completed = EDDB_Data.EDMilkyway.DownloadDataFileAsync(new Uri(url), "commodities.json", "test download").Wait(TimeSpan.FromSeconds(5));
                Assert.IsTrue(completed);
                timer.Stop();
                Debug.WriteLine("commodities download time: " + timer.Elapsed);
            }
        }
    }
}