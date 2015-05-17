using System.Collections.ObjectModel;
using RegulatedNoise.Core.Helpers;

namespace RegulatedNoise.Core.DomainModel
{
	internal class SystemCollection : KeyedCollection<string, StarSystem>
	{
		protected override string GetKeyForItem(StarSystem item)
		{
			return item.Name.ToCleanUpperCase();
		}

		public bool TryGetValue(string systemName, out StarSystem system)
		{
			if (Dictionary != null && Dictionary.TryGetValue(systemName, out system))
			{
				return true;
			}
			else
			{
				system = null;
				return false;
			}
			;
		}
	}
}