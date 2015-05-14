using Newtonsoft.Json;

namespace RegulatedNoise.DomainModel
{
    public class Commodity
    {
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
    }
}