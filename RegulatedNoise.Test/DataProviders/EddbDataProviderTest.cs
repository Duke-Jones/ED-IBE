using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.DomainModel;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise.Test.DataProviders
{
    [TestClass]
    public class EddbDataProviderTest
    {
        [TestMethod]
        public void i_can_import_data()
        {
            var eddb = new EddbDataProvider();
            var model = new DataModel();
            eddb.ImportData(model);
        }
    }
}