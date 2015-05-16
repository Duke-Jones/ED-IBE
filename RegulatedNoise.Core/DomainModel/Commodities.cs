using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RegulatedNoise.Core.Helpers;

namespace RegulatedNoise.Core.DomainModel
{
	public class Commodities: ICollection<Commodity>
    {
        private readonly CommodityCollection _commodities;

        private readonly ILocalizer _localization;

        public Commodities(ILocalizer localizer)
        {
	        if (localizer == null)
	        {
		        throw new ArgumentNullException("localizer");
	        }
	        _commodities = new CommodityCollection();
	        _localization = localizer;
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

        public Commodity TryGet(string commodityName)
        {
            Commodity commodity;
            _commodities.TryGetValue(commodityName.ToCleanTitleCase(), out commodity);
            return commodity;
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
            commodity.Name = GetBasename(commodity.Name);
            commodity.LocalizedName = _localization.TranslateToCurrent(commodity.Name);
            Commodity existingCommodity;
            if (!_commodities.TryGetValue(commodity.Name, out existingCommodity))
            {
                Add(commodity);
            }
            else
            {
                existingCommodity.UpdateFrom(commodity, UpdateMode.Update);
            }
        }

        public string GetBasename(string commodityName)
        {
            return _localization.TranslateInEnglish(commodityName);
        }
    }

	public interface ILocalizer
	{
		string TranslateToCurrent(string toLocalize);
		string TranslateInEnglish(string commodityName);
	}
}