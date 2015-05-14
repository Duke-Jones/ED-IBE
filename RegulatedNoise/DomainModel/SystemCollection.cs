using System.Collections.ObjectModel;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.DomainModel
{
    internal class SystemCollection : KeyedCollection<string, StarSystem>
    {
        protected override string GetKeyForItem(StarSystem item)
        {
            return item.Name.ToCleanTitleCase();
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