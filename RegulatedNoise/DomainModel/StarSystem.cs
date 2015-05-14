using System;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.DomainModel
{
    /// <summary>
    /// 
    /// </summary>
    public class StarSystem
    {
        private static double EPSILON = 1e-6;
        private readonly StationCollection _stations;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

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
        public int? NeedsPermit { get; set; }

        [JsonProperty("updated_at")]
        public int UpdatedAt { get; set; }

        public string Source { get; set; }

        /// <summary>
        /// creates a new system
        /// </summary>
        public StarSystem()
        {
            _stations = new StationCollection();
            Reset();
        }

        /// <summary>
        /// creates a new system as a copy of another system
        /// only the id must declared extra
        /// </summary>
        /// <param name="newId">The new identifier.</param>
        /// <param name="sourceSystem">The source system.</param>
        public StarSystem(int newId, StarSystem sourceSystem)
            :this()
        {
            Id              = newId;
            UpdateFrom(sourceSystem, UpdateMode.Copy);   
        }

        /// <summary>
        /// true, if all data *except the ID* is equal (case insensitive)
        /// </summary>
        /// <param name="system">The eq system.</param>
        /// <returns></returns>
        public bool EquivalentTo(StarSystem system)
        {
            if (ReferenceEquals(this, system)) return true;
            if (system == null) return false;
            return (ObjectCompare.EqualsNullable(this.Name, system.Name) &&
                    ObjectCompare.EqualsNullable(this.X, system.X) &&
                    ObjectCompare.EqualsNullable(this.Y, system.Y) &&
                    ObjectCompare.EqualsNullable(this.Z, system.Z) &&
                    ObjectCompare.EqualsNullable(this.Faction, system.Faction) &&
                    ObjectCompare.EqualsNullable(this.Population, system.Population) &&
                    ObjectCompare.EqualsNullable(this.Government, system.Government) &&
                    ObjectCompare.EqualsNullable(this.Allegiance, system.Allegiance) &&
                    ObjectCompare.EqualsNullable(this.State, system.State) &&
                    ObjectCompare.EqualsNullable(this.Security, system.Security) &&
                    ObjectCompare.EqualsNullable(this.PrimaryEconomy, system.PrimaryEconomy) &&
                    ObjectCompare.EqualsNullable(this.NeedsPermit, system.NeedsPermit) &&
                    ObjectCompare.EqualsNullable(this.UpdatedAt, system.UpdatedAt));
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
                Id = sourceSystem.Id;
            }
            X = sourceSystem.X;
            Y = sourceSystem.Y;
            Z = sourceSystem.Z;
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
            if (doCopy || String.IsNullOrEmpty(Source))
            {
                Source = sourceSystem.Source;
            }
            else
            {
                Source = Source + "@" + sourceSystem.Source;
            }
        }

        /// <summary>
        /// reset all data 
        /// </summary>
        public void Reset()
        { 
            Id              = 0;
            Name            = String.Empty;
            X               = 0.0;
            Y               = 0.0;
            Z               = 0.0;
            Faction         = null;
            Population      = null;
            Government      = null;
            Allegiance      = null;
            State           = null;
            Security        = null;
            PrimaryEconomy  = null;
            NeedsPermit     = null;
            UpdatedAt       = 0;

        }

        /// <summary>
        /// return the coordinates of the system
        /// </summary>
        /// <returns></returns>
        internal Point3D SystemCoordinates()
        {
            return new Point3D((float)X, (float)Y, (float)Z);
        }

        public void UpdateStations(Station station)
        {
            _stations.UpdateFrom(station);
        }
    }
}