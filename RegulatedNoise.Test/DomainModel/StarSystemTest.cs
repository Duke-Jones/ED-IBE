using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RegulatedNoise.Core.DomainModel;

namespace RegulatedNoise.Test.DomainModel
{
    [TestClass]
    public class StarSystemTest
    {
        [TestMethod]
        public void i_can_instantiate()
        {
            var system = new StarSystem("mysystem");
        }

        [TestMethod]
        public void i_can_serialize()
        {
            var expected = NewSystem();
            string json = JsonConvert.SerializeObject(expected);
            var actual = JsonConvert.DeserializeObject<StarSystem>(json);
            TestHelpers.AssertAllPropertiesEqual(expected, actual);
        }

        [TestMethod]
        public void i_can_update_unset_allegiance_with_older_data()
        {
            var initial = new StarSystem("mysystem") { UpdatedAt = 10 };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(sourceSystem.Allegiance, initial.Allegiance, "allegiance has not been updated");
        }

        [TestMethod]
        public void i_can_update_allegiance_with_new_value()
        {
            var initial = new StarSystem("mysystem") { Allegiance = "oldAllegiance"};
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(sourceSystem.Allegiance, initial.Allegiance, "allegiance has not been updated");
        }

        [TestMethod]
        public void older_allegiance_value_does_not_update_previous_value()
        {
            const string oldallegiance = "oldAllegiance";
            var initial = new StarSystem("mysystem") { Allegiance = oldallegiance, UpdatedAt = 1000 };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(oldallegiance, initial.Allegiance, "allegiance should not have been updated");
        }

        [TestMethod]
        public void unset_allegiance_value_does_not_update_previous_value()
        {
            const string oldallegiance = "oldAllegiance";
            var initial = new StarSystem("mysystem") { Allegiance = oldallegiance };
            StarSystem sourceSystem = NewSystem();
            sourceSystem.Allegiance = null;
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(oldallegiance, initial.Allegiance, "allegiance should not have been updated");
        }

        [TestMethod]
        public void on_copy_allegiance_is_overwritten()
        {
            var initial = new StarSystem("mysystem") { Allegiance = "oldAllegiance" };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Copy);
            Assert.AreEqual(sourceSystem.Allegiance, initial.Allegiance, "allegiance has not been updated");
        }

        [TestMethod]
        public void i_can_update_unset_needPermit_with_older_data()
        {
            var initial = new StarSystem("mysystem") { UpdatedAt = 10 };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(sourceSystem.NeedsPermit, initial.NeedsPermit, "NeedsPermit has not been updated");
        }

        [TestMethod]
        public void i_can_update_needPermit_with_new_value()
        {
            StarSystem sourceSystem = NewSystem();
            var initial = new StarSystem("mysystem") { NeedsPermit = !sourceSystem.NeedsPermit };
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(sourceSystem.NeedsPermit, initial.NeedsPermit, "NeedsPermit has not been updated");
        }

        [TestMethod]
        public void older_needPermit_value_does_not_update_previous_value()
        {
            const bool oldneedPermit = false;
            StarSystem sourceSystem = NewSystem();
            sourceSystem.NeedsPermit = !oldneedPermit;
            var initial = new StarSystem("mysystem") { NeedsPermit = oldneedPermit, UpdatedAt = 1000 };
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(oldneedPermit, initial.NeedsPermit, "NeedsPermit should not have been updated");
        }

        [TestMethod]
        public void unset_needPermit_value_does_not_update_previous_value()
        {
            const bool oldneedPermit = false;
            StarSystem sourceSystem = NewSystem();
            sourceSystem.NeedsPermit = null;
            var initial = new StarSystem("mysystem") { NeedsPermit = oldneedPermit };
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(oldneedPermit, initial.NeedsPermit, "NeedsPermit should not have been updated");
        }

        [TestMethod]
        public void on_copy_needPermit_is_overwritten()
        {
            const bool oldneedPermit = false;
            StarSystem sourceSystem = NewSystem();
            sourceSystem.NeedsPermit = !oldneedPermit;
            var initial = new StarSystem("mysystem") { NeedsPermit = oldneedPermit };
            initial.UpdateFrom(sourceSystem, UpdateMode.Copy);
            Assert.AreEqual(sourceSystem.NeedsPermit, initial.NeedsPermit, "NeedsPermit has not been updated");
        }

        [TestMethod]
        public void older_X_value_does_not_update_previous_value()
        {
            const double x = -1.2;
            var initial = new StarSystem("mysystem") { UpdatedAt = 10, X = x };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(x, initial.X, "X has not been updated");
        }

        [TestMethod]
        public void i_can_update_X_with_new_value()
        {
            var initial = new StarSystem("mysystem") { X = -1.2 };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(sourceSystem.X, initial.X, "X has not been updated");
        }

        [TestMethod]
        public void on_copy_X_is_overwritten()
        {
            var initial = new StarSystem("mysystem") { X = -1.2 };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Copy);
            Assert.AreEqual(sourceSystem.X, initial.X, "X has not been updated");
        }

        [TestMethod]
        public void on_clone_name_is_overwritten()
        {
            var initial = new StarSystem("myoldsystem") { Allegiance = "oldAllegiance" };
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Clone);
            Assert.AreEqual(sourceSystem.Name, initial.Name, "name has not been overwritten");
        }

        [TestMethod]
        public void on_clone_source_is_overwritten()
        {
            var initial = new StarSystem("mysystem");
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Clone);
            Assert.AreEqual(sourceSystem.Source, initial.Source, "name has not been overwritten");
        }

        [TestMethod]
        public void on_copy_source_is_overwritten()
        {
            var initial = new StarSystem("mysystem");
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Copy);
            Assert.AreEqual(sourceSystem.Source, initial.Source, "unexpected source");
        }

        [TestMethod]
        public void on_update_new_source_is_added()
        {
            var initial = new StarSystem("mysystem");
            StarSystem sourceSystem = NewSystem();
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(UpdatableEntity.UNKNOWN_SOURCE +"@" + sourceSystem.Source, initial.Source, "unexpected source");
        }

        [TestMethod]
        public void on_update_new_source_is_not_duplicated()
        {
            StarSystem sourceSystem = NewSystem();
            var initial = new StarSystem("mysystem") { Source = sourceSystem.Source};
            initial.UpdateFrom(sourceSystem, UpdateMode.Update);
            Assert.AreEqual(sourceSystem.Source, initial.Source, "unexpected source");
        }

        private static StarSystem NewSystem(string systemName = "mysystem")
        {
            return new StarSystem(systemName)
            {
                Allegiance = "myAllegiance", Faction = "myFaction", Government = "myGovernment"
                , NeedsPermit = false, Population = 100000000, PrimaryEconomy = "myPrimaryEconomy"
                , Security = "mySecurity", Source = "Test", State = "myState", UpdatedAt = 1, X = 1.1, Y = 1.2, Z = 1.3
            };
        }
    }
}