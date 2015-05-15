using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RegulatedNoise.DomainModel;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.EDDB_Data
{
    internal class EddbDataProvider
    {
        private const string EDDB_COMMODITIES_DATAFILE = @"./Data/commodities.json";
        private const string EDDB_STATIONS_LITE_DATAFILE = @"./Data/stations_lite.json";
        private const string EDDB_STATIONS_FULL_DATAFILE = @"./Data/stations.json";
        private const string EDDB_SYSTEMS_DATAFILE = @"./Data/systems.json";

        private const string EDDB_COMMODITIES_URL = @"http://eddb.io/archive/v3/commodities.json";
        private const string EDDB_SYSTEMS_URL = @"http://eddb.io/archive/v3/systems.json";
        private const string EDDB_STATIONS_LITE_URL = @"http://eddb.io/archive/v3/stations_lite.json";
        //private const string EDDB_STATIONS_FULL_URL = @"http://eddb.io/archive/v3/stations.json";

        public void ImportData(DataModel model)
        {
            DownloadDataFiles();
            ImportSystems(model.Universe);
            ImportCommodities(model.Commodities);
            ImportStations(model.Universe);
        }

        private void ImportCommodities(Commodities commodities)
        {
            List<Commodity> eddbSystems = ReadFile<List<Commodity>>(EDDB_COMMODITIES_DATAFILE);
            foreach (Commodity commodity in (IEnumerable<Commodity>)eddbSystems)
            {
                commodities.Update(commodity);
            }
        }

        private void ImportStations(Universe universe)
        {
            if (File.Exists(EDDB_STATIONS_FULL_DATAFILE))
            {
                List<EDStation> eddbStations = ReadFile<List<EDStation>>(EDDB_STATIONS_FULL_DATAFILE);
                foreach (EDStation eddbStation in eddbStations)
                {
                    universe.Update(ToStation(eddbStation));
                }
            }
            else if (File.Exists(EDDB_STATIONS_LITE_DATAFILE))
            {
                //imports only stations
            }
        }

        private Station ToStation(EDStation eddbStation)
        {
            Station station = new Station(eddbStation.Name.ToCleanTitleCase())
            {
                Allegiance = eddbStation.Allegiance
                ,DistanceToStar = eddbStation.DistanceToStar
                ,Economies = eddbStation.Economies
                ,ExportCommodities = eddbStation.ExportCommodities
                ,Faction = eddbStation.Faction
                , Government = eddbStation.Government
                , HasBlackmarket = eddbStation.HasBlackmarket
                , HasCommodities = eddbStation.HasCommodities
                , HasOutfitting = eddbStation.HasOutfitting
                , HasRearm = eddbStation.HasRearm
                , HasRepair = eddbStation.HasRepair
                , HasRefuel = eddbStation.HasRefuel
                , HasShipyard = eddbStation.HasShipyard
                , ImportCommodities = eddbStation.ImportCommodities
                , MaxLandingPadSize = eddbStation.MaxLandingPadSize
                , ProhibitedCommodities = eddbStation.ProhibitedCommodities
                , Source = "EDDB"
                , State = eddbStation.State
                , System
                , Type = eddbStation.Type
                , UpdatedAt = eddbStation.UpdatedAt
            }

        }

        private void ImportSystems(Universe universe)
        {
            List<EDSystem> eddbSystems = ReadFile<List<EDSystem>>(EDDB_SYSTEMS_DATAFILE);
            foreach (EDSystem system in (IEnumerable<EDSystem>) eddbSystems)
            {
                universe.Update(ToStarSystem(system));
            }
        }

        private TEntity ReadFile<TEntity>(string filepath)
        {
            if (File.Exists(filepath))
            {
                using (var reader = new StreamReader(filepath))
                using (var jreader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<TEntity>(jreader);
                }
            }
            else
            {
                throw new FileNotFoundException(filepath + ": does not exist");
            }
        }

        private StarSystem ToStarSystem(EDSystem eddbSystem)
        {
            var starSystem = new StarSystem(eddbSystem.Name)
            {
                Allegiance = eddbSystem.Allegiance
                ,Faction = eddbSystem.Faction
                ,Government = eddbSystem.Government
                ,NeedsPermit = ToNBool(eddbSystem.NeedsPermit)
                ,Population = eddbSystem.Population
                ,PrimaryEconomy = eddbSystem.PrimaryEconomy
                , Security = eddbSystem.Security
                , Source = "EDDB"
                , State = eddbSystem.State
                , UpdatedAt = eddbSystem.UpdatedAt
                , X = eddbSystem.X
                , Y = eddbSystem.Y
                , Z = eddbSystem.Z
            };
            return starSystem;
        }

        private static bool? ToNBool(int? needsPermit)
        {
            if (needsPermit.HasValue)
            {
                if (needsPermit == 0)
                {
                    return false;
                }
                else if (needsPermit == 1)
                {
                    return true;
                }
                else
                {
                    throw new NotSupportedException(needsPermit + ": unable to convert from int to bool");
                }
            }
            else
            {
                return null;
            }
        }

        private static void DownloadDataFiles()
        {
            var tasks = new List<Task>();
            if (!File.Exists(EDDB_COMMODITIES_DATAFILE))
            {
                tasks.Add(Task.Run(() => DownloadDataFile(new Uri(EDDB_COMMODITIES_URL), EDDB_COMMODITIES_DATAFILE,
                    "eddb commodities data")));
            }
            if (!File.Exists(EDDB_SYSTEMS_DATAFILE))
            {
                tasks.Add(Task.Run(() => DownloadDataFile(new Uri(EDDB_SYSTEMS_URL), EDDB_SYSTEMS_DATAFILE,
                    "eddb stations lite data")));
            }
            if (!File.Exists(EDDB_STATIONS_FULL_DATAFILE) && !File.Exists(EDDB_STATIONS_LITE_DATAFILE))
            {
                tasks.Add(Task.Run(() => DownloadDataFile(new Uri(EDDB_STATIONS_LITE_URL), EDDB_STATIONS_LITE_DATAFILE,
                    "eddb stations lite data")));
            }
            if (tasks.Any())
            {
                while (!Task.WaitAll(tasks.ToArray(), TimeSpan.FromMinutes(5)) && EventBus.Request("eddb server not responding, still waiting?"))
                {
                }
            }
        }

        private static void DownloadDataFile(Uri address, string filepath, string contentDescription)
        {
            EventBus.InitializationProgress("trying to download " + contentDescription + "...");
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(address, filepath);
            }
            EventBus.InitializationProgress("..." + contentDescription + " download completed");
        }
    }
}