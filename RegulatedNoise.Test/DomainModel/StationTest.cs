using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RegulatedNoise.Core.DomainModel;

namespace RegulatedNoise.Test.DomainModel
{
    [TestClass]
    public class StationTest
    {
        [TestMethod]
        public void i_can_isntantiate()
        {
            var station = new Station("myStation");
        }

        [TestMethod]
        public void i_can_serialize()
        {
            var expected = NewStation();
            string json = JsonConvert.SerializeObject(expected);
            var actual = JsonConvert.DeserializeObject<Station>(json);
            TestHelpers.AssertAllPropertiesEqual(expected, actual);
        }

        [TestMethod]
        public void i_can_update_unset_allegiance_with_older_data()
        {
            var initial = new Station("mystation") { UpdatedAt = 10 };
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(sourceStation.Allegiance, initial.Allegiance, "allegiance has not been updated");
        }

        [TestMethod]
        public void i_can_update_allegiance_with_new_value()
        {
            var initial = new Station("mystation") { Allegiance = "oldAllegiance" };
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(sourceStation.Allegiance, initial.Allegiance, "allegiance has not been updated");
        }

        [TestMethod]
        public void older_allegiance_value_does_not_update_previous_value()
        {
            const string oldallegiance = "oldAllegiance";
            var initial = new Station("mystation") { Allegiance = oldallegiance, UpdatedAt = 1000 };
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(oldallegiance, initial.Allegiance, "allegiance should not have been updated");
        }

        [TestMethod]
        public void unset_allegiance_value_does_not_update_previous_value()
        {
            const string oldallegiance = "oldAllegiance";
            var initial = new Station("mystation") { Allegiance = oldallegiance };
            Station sourceStation = NewStation();
            sourceStation.Allegiance = null;
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(oldallegiance, initial.Allegiance, "allegiance should not have been updated");
        }

        [TestMethod]
        public void on_copy_allegiance_is_overwritten()
        {
            var initial = new Station("mystation") { Allegiance = "oldAllegiance" };
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Copy);
            Assert.AreEqual(sourceStation.Allegiance, initial.Allegiance, "allegiance has not been updated");
        }

        [TestMethod]
        public void i_can_update_unset_hasRefuel_with_older_data()
        {
            var initial = new Station("mystation") { UpdatedAt = 10 };
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(sourceStation.HasRefuel, initial.HasRefuel, "hasRefuel has not been updated");
        }

        [TestMethod]
        public void i_can_update_hasRefuel_with_new_value()
        {
            Station sourceStation = NewStation();
            var initial = new Station("mystation") { HasRefuel = !sourceStation.HasRefuel };
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(sourceStation.HasRefuel, initial.HasRefuel, "hasRefuel has not been updated");
        }

        [TestMethod]
        public void older_hasRefuel_value_does_not_update_previous_value()
        {
            const bool oldhasRefuel = false;
            Station sourceStation = NewStation();
            sourceStation.HasRefuel = !oldhasRefuel;
            var initial = new Station("mystation") { HasRefuel = oldhasRefuel, UpdatedAt = 1000 };
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(oldhasRefuel, initial.HasRefuel, "HasRefuel should not have been updated");
        }

        [TestMethod]
        public void unset_hasRefuel_value_does_not_update_previous_value()
        {
            const bool oldhasRefuel = false;
            Station sourceStation = NewStation();
            sourceStation.HasRefuel = null;
            var initial = new Station("mystation") { HasRefuel = oldhasRefuel };
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(oldhasRefuel, initial.HasRefuel, "HasRefuel should not have been updated");
        }

        [TestMethod]
        public void on_copy_hasRefuel_is_overwritten()
        {
            const bool oldhasRefuel = false;
            Station sourceStation = NewStation();
            sourceStation.HasRefuel = !oldhasRefuel;
            var initial = new Station("mystation") { HasRefuel = oldhasRefuel };
            initial.UpdateFrom(sourceStation, UpdateMode.Copy);
            Assert.AreEqual(sourceStation.HasRefuel, initial.HasRefuel, "HasRefuel has not been updated");
        }

        [TestMethod]
        public void older_distanceToStar_value_does_not_update_previous_value()
        {
            Station sourceStation = NewStation();
            int distanceToStar = sourceStation.DistanceToStar.Value + 10;
            var initial = new Station("mystation") { UpdatedAt = 10, DistanceToStar = distanceToStar };
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(distanceToStar, initial.DistanceToStar, "distance to star has not been updated");
        }

        [TestMethod]
        public void i_can_update_DistanceToStar_with_new_value()
        {
            var initial = new Station("mystation") { DistanceToStar = 54321 };
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(sourceStation.DistanceToStar, initial.DistanceToStar, "DistanceToStar has not been updated");
        }

        [TestMethod]
        public void on_copy_DistanceToStar_is_overwritten()
        {
            var initial = new Station("mystation") { DistanceToStar = 54332 };
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Copy);
            Assert.AreEqual(sourceStation.DistanceToStar, initial.DistanceToStar, "DistanceToStar has not been updated");
        }

        [TestMethod]
        public void on_clone_name_is_overwritten()
        {
            var initial = new Station("myoldstation");
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Clone);
            Assert.AreEqual(sourceStation.Name, initial.Name, "name has not been overwritten");
        }

        [TestMethod]
        public void on_clone_source_is_overwritten()
        {
            var initial = new Station("mystation");
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Clone);
            Assert.AreEqual(sourceStation.Source, initial.Source, "name has not been overwritten");
        }

        [TestMethod]
        public void on_copy_source_is_overwritten()
        {
            var initial = new Station("mystation");
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Copy);
            Assert.AreEqual(sourceStation.Source, initial.Source, "unexpected source");
        }

        [TestMethod]
        public void on_update_new_source_is_added()
        {
            var initial = new Station("mystation");
            Station sourceStation = NewStation();
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(UpdatableEntity.UNKNOWN_SOURCE + "@" + sourceStation.Source, initial.Source, "unexpected source");
        }

        [TestMethod]
        public void on_update_new_source_is_not_duplicated()
        {
            Station sourceStation = NewStation();
            var initial = new Station("mystation") { Source = sourceStation.Source };
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            Assert.AreEqual(sourceStation.Source, initial.Source, "unexpected source");
        }

        [TestMethod]
        public void on_update_with_older_data_available_ships_are_merged()
        {
            Station sourceStation = NewStation();
            var previousShips = new []{ "oldship1", "oldship2", sourceStation.AvailableShips[0] };
            var initial = new Station("mystation") { Source = sourceStation.Source, AvailableShips = previousShips, UpdatedAt = 1000};
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            foreach (string ship in sourceStation.AvailableShips)
            {
                Assert.IsTrue(initial.AvailableShips.Contains(ship), ship + " is not merged");
            }
            foreach (string ship in previousShips)
            {
                Assert.IsTrue(initial.AvailableShips.Contains(ship), ship + " should not have been removed");
            }
            Assert.AreEqual(4, initial.AvailableShips.Length, "ship(s) have been duplicated or forgot");
        }

        [TestMethod]
        public void on_update_with_new_data_available_ships_are_overwritten()
        {
            Station sourceStation = NewStation();
            var previousShips = new[] { "oldship1", "oldship2", sourceStation.AvailableShips[0] };
            var initial = new Station("mystation") { Source = sourceStation.Source, AvailableShips = previousShips };
            initial.UpdateFrom(sourceStation, UpdateMode.Update);
            foreach (string ship in sourceStation.AvailableShips)
            {
                Assert.IsTrue(initial.AvailableShips.Contains(ship), ship + " is not merged");
            }
            Assert.AreEqual(sourceStation.AvailableShips.Length, initial.AvailableShips.Length, "ship(s) have been duplicated or forgot");
        }

        private static Station NewStation()
        {
            return new Station("myStation") { Allegiance = "myAllegiance"
                , AvailableShips = new[]{ "ship1", "ship2" }
                , DistanceToStar = 1234
                , Economies = new []{ "eco1", "eco2" }
                , ExportCommodities = new []{ "c1", "c2" }
                , Faction = "myFaction"
                , Government = "myGovernment"
                , HasBlackmarket = true
                , HasCommodities = null
                , HasOutfitting = false
                , HasRearm = true
                , HasRefuel = true
                , HasRepair = true
                , HasShipyard = true
                , ImportCommodities = new []{ "c3"}
                , MaxLandingPadSize = LandingPadSize.L
                , ProhibitedCommodities = new [] { "c5" }
                , Source = "mySource"
                , State = "myState"
                , System = "mySystem"
                , Type = "myType"
                , UpdatedAt = 2};
        }
    }
}