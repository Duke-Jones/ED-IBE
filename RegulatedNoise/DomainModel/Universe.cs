using System;
using System.Collections.Generic;
using RegulatedNoise.Annotations;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise.DomainModel
{
    internal class Universe : SystemCollection
    {
        private readonly object _updating = new object();

        public void UpdateRange([NotNull] IEnumerable<StarSystem> systems)
        {
            if (systems == null) throw new ArgumentNullException("systems");
            foreach (StarSystem system in systems)
            {
                Update(system);
            }
        }

        public void Update(Station station)
        {
            StarSystem existingSystem;
            lock (_updating)
            {
                if (Dictionary == null || !Dictionary.TryGetValue(station.System, out existingSystem))
                {
                    existingSystem = new StarSystem() { Name = station.System };
                }
                existingSystem.UpdateStations(station);
            }
        }

        public void Update(StarSystem system)
        {
            StarSystem existingSystem;
            lock (_updating)
            {
                if (Dictionary == null || !Dictionary.TryGetValue(system.Name, out existingSystem))
                {
                    Add(system);
                }
                else
                {
                    existingSystem.UpdateFrom(system, UpdateMode.Update);
                }
            }
        }
    }
}