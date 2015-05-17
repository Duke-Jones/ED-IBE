using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.Test.DataProviders
{
    [TestClass]
    public class EddbDataProviderTest
    {
        [TestMethod]
        public void i_can_import_data()
        {
            var eddb = new EddbDataProvider();
            var model = new DataModel(new dsCommodities(), new MarketDataValidator());
            eddb.ImportData(model);
        }
    }
}