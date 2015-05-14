using System.Collections.ObjectModel;

namespace RegulatedNoise.DomainModel
{
    internal class SystemCollection : KeyedCollection<string, StarSystem>
    {
        protected override string GetKeyForItem(StarSystem item)
        {
            return item.Name.ToLower();
        }
    }
}