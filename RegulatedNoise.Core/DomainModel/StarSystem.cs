using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RegulatedNoise.Annotations;
using RegulatedNoise.Core.Helpers;

namespace RegulatedNoise.Core.DomainModel
{
    /// <summary>
    /// 
    /// </summary>
    public class StarSystem : UpdatableEntity
    {
        private readonly StationCollection _stations;
        private string _name;

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            private set { _name = value.ToCleanUpperCase(); }
        }

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }

        [JsonProperty("population")]
        public long? Population { get; set; }

        [JsonProperty("government")]
        public string Government { get; set; }

        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("security")]
        public string Security { get; set; }

        [JsonProperty("primary_economy")]
        public string PrimaryEconomy { get; set; }

        [JsonProperty("needs_permit")]
        public bool? NeedsPermit { get; set; }

        [JsonProperty("updated_at")]
        public int UpdatedAt { get; set; }

        public IEnumerable<Station> Stations { get {  return _stations; } }

        /// <summary>
        /// creates a new system
        /// </summary>
        protected StarSystem()
        {
            _stations = new StationCollection();
            Name = String.Empty;
        }

        public StarSystem(string name)
            :this()
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("invalid system name", "name");
            Name = name;
        }

        /// <summary>
        /// creates a new system as a copy of another system
        /// only the id must declared extra
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sourceSystem">The source system.</param>
        public StarSystem(string name, StarSystem sourceSystem)
            :this(name)
        {
            UpdateFrom(sourceSystem, UpdateMode.Copy);   
        }

        /// <summary>
        /// copy the values from another system exept for the ID
        /// </summary>
        /// <param name="sourceSystem">The source system.</param>
        /// <param name="updateMode">The update mode.</param>
        public void UpdateFrom(StarSystem sourceSystem, UpdateMode updateMode)
        {
            bool doCopy = updateMode == UpdateMode.Clone || updateMode == UpdateMode.Copy;
            bool isNewer = UpdatedAt < sourceSystem.UpdatedAt;
            if (updateMode == UpdateMode.Clone)
            {
                Name = sourceSystem.Name;
            }
            if (doCopy || isNewer)
            {
                X = sourceSystem.X;
                Y = sourceSystem.Y;
                Z = sourceSystem.Z;
            }
            if (doCopy || String.IsNullOrEmpty(Faction) || (isNewer && !String.IsNullOrEmpty(sourceSystem.Faction))) 
                Faction = sourceSystem.Faction;
            if (doCopy || !Population.HasValue || (isNewer && sourceSystem.Population.HasValue)) 
                Population = sourceSystem.Population;
            if (doCopy || String.IsNullOrEmpty(Government) || (isNewer && !String.IsNullOrEmpty(sourceSystem.Government))) 
                Government = sourceSystem.Government;
            if (doCopy || String.IsNullOrEmpty(Allegiance) || (isNewer && !String.IsNullOrEmpty(sourceSystem.Allegiance))) 
                Allegiance = sourceSystem.Allegiance;
            if (doCopy || String.IsNullOrEmpty(State) || (isNewer && !String.IsNullOrEmpty(sourceSystem.State))) 
                State = sourceSystem.State;
            if (doCopy || String.IsNullOrEmpty(Security) || (isNewer && !String.IsNullOrEmpty(sourceSystem.Security))) 
                Security = sourceSystem.Security;
            if (doCopy || String.IsNullOrEmpty(PrimaryEconomy) || (isNewer && !String.IsNullOrEmpty(sourceSystem.PrimaryEconomy))) 
                PrimaryEconomy = sourceSystem.PrimaryEconomy;
            if (doCopy || !NeedsPermit.HasValue || (isNewer && sourceSystem.NeedsPermit.HasValue)) 
                NeedsPermit = sourceSystem.NeedsPermit;
            if (doCopy || UpdatedAt == 0 || isNewer) 
                UpdatedAt = sourceSystem.UpdatedAt;
            base.UpdateFrom(sourceSystem, updateMode);
        }

        public void UpdateStations([NotNull] Station station)
        {
            if (station == null) throw new ArgumentNullException("station");
            if (station.System != Name) throw new ArgumentException("station system " + station.System + " does not match system " + Name, "station");
            _stations.UpdateFrom(station);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}