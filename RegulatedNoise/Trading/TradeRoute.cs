using System;
using RegulatedNoise.Annotations;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.Trading
{
    public struct TradeRoute
    {
        public readonly string CommodityName;

        public readonly string OriginStationId;

        public readonly int BuyPrice;

        public readonly int Stock;

        public readonly int Supply;

        public readonly ProposalLevel? SupplyLevel;

        public readonly string TargetStationId;

        public readonly int SellPrice;

        public readonly int Demand;

        public readonly ProposalLevel? DemandLevel;

        public readonly DateTime Age;

        public readonly int Profit;

        public readonly double Distance;

        public TradeRoute ([NotNull] MarketDataRow origin, [NotNull] MarketDataRow destination, double distance = -1)
        {
            if (origin == null) throw new ArgumentNullException("origin");
            if (destination == null) throw new ArgumentNullException("destination");
            if (String.Compare(origin.CommodityName, destination.CommodityName, StringComparison.InvariantCultureIgnoreCase) != 0)
                throw new ArgumentException("marketdata commodities must match");
            CommodityName = origin.CommodityName;
            OriginStationId = origin.StationID;
            TargetStationId = destination.StationID;
            Age = origin.SampleDate < destination.SampleDate ? origin.SampleDate : destination.SampleDate;
            Profit = destination.SellPrice - origin.BuyPrice;
            Distance = distance;
            Stock = origin.Stock;
            BuyPrice = origin.BuyPrice;
            Supply = origin.Stock;
            SupplyLevel = origin.SupplyLevel;
            Demand = destination.Demand;
            DemandLevel = destination.DemandLevel;
            SellPrice = destination.SellPrice;
        } 
    }
}