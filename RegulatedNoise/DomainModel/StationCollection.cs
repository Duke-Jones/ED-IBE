using System.Collections.ObjectModel;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise.DomainModel
{
    internal class StationCollection : KeyedCollection<string, Station>
    {
        private readonly object _updating = new object();

        protected override string GetKeyForItem(Station item)
        {
            return item.Name.ToLower();
        }

        public void UpdateFrom(Station station)
        {
            Station existingStation;
            lock (_updating)
            {
                if (Dictionary == null || !Dictionary.TryGetValue(GetKeyForItem(station), out existingStation))
                {
                    Add(station);
                }
                else
                {
                    existingStation.UpdateFrom(station, UpdateMode.Update);
                }
            }
        }
    }
}