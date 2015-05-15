using System;
using Newtonsoft.Json;

namespace RegulatedNoise.DomainModel
{
    public class Commodity: UpdatableEntity
    {
        private string _localizedName;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("average_price")]
        public int? AveragePrice { get; set; }

        [JsonProperty("demand_price_levels")]
        public WarningLevels DemandWarningLevels { get; set; }

        [JsonProperty("supply_price_levels")]
        public WarningLevels SupplyWarningLevels { get; set; }

        [JsonIgnore]
        public string LocalizedName
        {
            get
            {
                if(_localizedName == null)
                {
                    return Name;
                }
                else
                {
                    return _localizedName;
                }
            }
            set
            {
                _localizedName = value;
            }
        }

        public void UpdateFrom(Commodity sourceCommodity, UpdateMode updateMode)
        {
            bool doCopy = updateMode == UpdateMode.Clone || updateMode == UpdateMode.Copy;
            if (updateMode == UpdateMode.Clone)
            {
                Name = sourceCommodity.Name;
            }
            if (doCopy || String.IsNullOrEmpty(Category))
                Category = sourceCommodity.Category;
            if (doCopy || !AveragePrice.HasValue)
                AveragePrice = sourceCommodity.AveragePrice;
            if (doCopy || !DemandWarningLevels.Buy.Low.HasValue)
                DemandWarningLevels.Buy.Low = sourceCommodity.DemandWarningLevels.Buy.Low;
            if (doCopy || !DemandWarningLevels.Buy.High.HasValue)
                DemandWarningLevels.Buy.High = sourceCommodity.DemandWarningLevels.Buy.High;
            if (doCopy || !SupplyWarningLevels.Buy.Low.HasValue)
                SupplyWarningLevels.Buy.Low = sourceCommodity.SupplyWarningLevels.Buy.Low;
            if (doCopy || !SupplyWarningLevels.Buy.High.HasValue)
                SupplyWarningLevels.Buy.High = sourceCommodity.SupplyWarningLevels.Buy.High;
            base.UpdateFrom(sourceCommodity, updateMode);
        }
    }

    public class WarningLevels
    {
        [JsonProperty("sell")]
        public PriceBounds Sell { get; private set; }

        [JsonProperty("buy")]
        public PriceBounds Buy { get; private set; }

        public WarningLevels()
        {
            Sell = new PriceBounds();
            Buy = new PriceBounds();
        }
    }

    public class PriceBounds
    {
        [JsonProperty("low")]
        public int? Low { get; set; }

        [JsonProperty("high")]
        public int? High { get; set; }

        public bool IsInRange(int price)
        {
            return ((Low.HasValue) && (price < Low)) 
                     || ((High.HasValue) && (price > High));
        }
    }
}