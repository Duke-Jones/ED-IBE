using System;
using System.Collections.Generic;
using RegulatedNoise.Annotations;
using RegulatedNoise.Core.Helpers;

namespace RegulatedNoise.Core.DomainModel
{
	public class Universe
	{
		private readonly object _updating = new object();

		private readonly SystemCollection _systems;

		public Universe()
		{
			_systems = new SystemCollection();
		}

		public StarSystem this[string systemName]
		{
			get { return _systems[systemName.ToCleanUpperCase()]; }
		}

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
				if (!_systems.TryGetValue(station.System, out existingSystem))
				{
					existingSystem = new StarSystem(station.System);
					_systems.Add(existingSystem);
				}
				existingSystem.UpdateStations(station);
			}
		}

		public void Update(StarSystem system)
		{
			StarSystem existingSystem;
			lock (_updating)
			{
				if (!_systems.TryGetValue(system.Name, out existingSystem))
				{
					_systems.Add(system);
				}
				else
				{
					existingSystem.UpdateFrom(system, UpdateMode.Update);
				}
			}
		}
	}
}