using System.Collections.ObjectModel;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.DomainModel
{
    internal class StationCollection : KeyedCollection<string, Station>
    {
        private readonly object _updating = new object();

        protected override string GetKeyForItem(Station item)
        {
            return item.Name.ToCleanTitleCase();
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