using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.DomainModel
{
    internal class Commodities: ICollection<Commodity>
    {
        private readonly CommodityCollection _commodities;

        public Commodities()
        {
            _commodities = new CommodityCollection();
        }

        protected class CommodityCollection : KeyedCollection<string, Commodity>
        {
            protected override string GetKeyForItem(Commodity item)
            {
                return item.Name.ToCleanTitleCase();
            }

            public bool TryGetValue(string commodityName, out Commodity commodity)
            {
                if (Dictionary != null && Dictionary.TryGetValue(commodityName, out commodity))
                {
                    return true;
                }
                else
                {
                    commodity = null;
                    return false;
                }
            }
        }

        public Commodity this[string commodityName]
        {
            get { return _commodities[commodityName.ToCleanTitleCase()]; }
        }

        public IEnumerator<Commodity> GetEnumerator()
        {
            return _commodities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Commodity item)
        {
            _commodities.Add(item);
        }

        public void Clear()
        {
            _commodities.Clear();
        }

        public bool Contains(Commodity item)
        {
            return _commodities.Contains(item);
        }

        public void CopyTo(Commodity[] array, int arrayIndex)
        {
            _commodities.CopyTo(array, arrayIndex);
        }

        public bool Remove(Commodity item)
        {
            return _commodities.Remove(item);
        }

        public int Count { get { return _commodities.Count;  } }

        public bool IsReadOnly { get { return false; } }

        public void Update(Commodity commodity)
        {
            throw new System.NotImplementedException();
        }
    }
}