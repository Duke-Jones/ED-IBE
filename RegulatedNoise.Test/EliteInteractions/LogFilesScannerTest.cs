using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.EliteInteractions;

namespace RegulatedNoise.Test.EliteInteractions
{
    [TestClass]
    public class LogFilesScannerTest
    {
        [TestMethod]
        public void i_can_instantiate()
        {
            using (var scanner = NewScanner()) { }
        }

        [TestMethod]
        public void event_is_raised_on_location_found()
        {
            var events = new List<LocationUpdateEventArgs>();
            AutoResetEvent waiter = new AutoResetEvent(false);
            using (var scanner = NewScanner())
            {
                scanner.OnCurrentLocationUpdate += (sender, args) =>
                {
                    events.Add(args);
                    waiter.Set();
                };
                scanner.PollingPeriod = 0;
                scanner.UpdateSystemNameFromLogFile();
                if (!waiter.WaitOne(2000)) {  Assert.Inconclusive("no event raised within 2s");}
                Assert.IsTrue(events.Any(), "no expected event raised");
                foreach (LocationUpdateEventArgs @event in events)
                {
                    Debug.WriteLine(@event);
                }
            }
        }

        private LogFilesScanner NewScanner()
        {
            return new LogFilesScanner(new RegulatedNoiseSettings() { ProductsPath = "playground/Products" ,PilotsName = "Bobby" });
        }
    }
}