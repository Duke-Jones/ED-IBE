using Newtonsoft.Json;

namespace RegulatedNoise.DomainModel
{
    public class Commodity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category_id")]
        public string Category { get; set; }

        [JsonProperty("average_price")]
        public int? AveragePrice { get; set; }

        [JsonProperty("demand_price_levels")]
        public WarningLevels DemandWarningLevels { get; set; }

        [JsonProperty("supply_price_levels")]
        public WarningLevels SupplyWarningLevels { get; set; }
    }
}