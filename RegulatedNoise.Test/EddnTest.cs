using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.Test
{
    [TestClass]
    public class EddnTest
    {
        [TestMethod]
        [Ignore]
        // activate only when build with NO_PATH_INIT set
        public void i_can_instantiate()
        {
			  using (var eddn = NewEddn()) { }
        }

        [TestMethod]
        [Ignore]
        // activate only when build with NO_PATH_INIT set
        // have to delay dispose to wait 10s eddn message queue polling
        // test will be upgraded when dispatcher queue will be used
        public void i_can_send_a_marketdata()
        {
            using (var eddn = NewEddn())
            {
                eddn.SendToEddn(new MarketDataRow() { CommodityName = "Tea", BuyPrice = 1234, SampleDate = DateTime.Now, StationName = "myStation", SystemName = "mySystem"});            
            }
        }

        private static Eddn NewEddn()
        {
            return new Eddn(ApplicationContext.CommoditiesLocalisation, ApplicationContext.RegulatedNoiseSettings) { TestMode = true };
        }
    }
}