using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RegulatedNoise.Annotations;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    internal class Commodities
    {
        private class StationMarketCollection : KeyedCollection<string, StationMarket>
        {
            protected override string GetKeyForItem(StationMarket item)
            {
                return item.StationID;
            }

            public void Add([NotNull] MarketDataRow marketDataRow)
            {
                if (marketDataRow == null) throw new ArgumentNullException("marketDataRow");
                StationMarket market;
                if (Dictionary == null || !Dictionary.TryGetValue(marketDataRow.StationID, out market))
                {
                    market = new StationMarket(marketDataRow.StationID);
                    Add(market);
                }
                market.Add(marketDataRow);
            }

            public void AddRange([NotNull] IEnumerable<MarketDataRow> marketDataRows)
            {
                if (marketDataRows == null) throw new ArgumentNullException("marketDataRows");
                foreach (MarketDataRow marketDataRow in marketDataRows)
                {
                    Add(marketDataRow);
                }
            }
        }

        private class CommodityMarketCollection : KeyedCollection<string, CommodityMarket>
        {
            protected override string GetKeyForItem(CommodityMarket item)
            {
                return item.Commodity;
            }

            public void Add([NotNull] MarketDataRow marketDataRow)
            {
                if (marketDataRow == null) throw new ArgumentNullException("marketDataRow");
                CommodityMarket market;
                if (Dictionary == null || !Dictionary.TryGetValue(marketDataRow.StationID, out market))
                {
                    market = new CommodityMarket(marketDataRow.CommodityName);
                    Add(market);
                }
                market.Add(marketDataRow);
            }

            public void AddRange([NotNull] IEnumerable<MarketDataRow> marketDataRows)
            {
                if (marketDataRows == null) throw new ArgumentNullException("marketDataRows");
                foreach (MarketDataRow marketDataRow in marketDataRows)
                {
                    Add(marketDataRow);
                }
            }
        }

        private StationMarketCollection _byStation;

        private CommodityMarketCollection _byCommodity;

        public Commodities()
        {
            _byStation = new StationMarketCollection();
            _byCommodity = new CommodityMarketCollection();
        }

        public IEnumerable<StationMarket> StationMarket
        {
            get { return _byStation; }
        }

        public IEnumerable<CommodityMarket> CommodityMarket
        {
            get { return _byCommodity; }
        }

        public void Add([NotNull] MarketDataRow marketDataRow)
        {
            _byStation.Add(marketDataRow);
            _byCommodity.Add(marketDataRow);
        }

        public void AddRange([NotNull] IEnumerable<MarketDataRow> marketDataRows)
        {
            if (marketDataRows == null) throw new ArgumentNullException("marketDataRows");
            foreach (MarketDataRow marketDataRow in marketDataRows)
            {
                Add(marketDataRow);
            }
        }
    }

    internal class CommodityMarket : Market
    {
        public CommodityMarket(string commodity)
        {
            Commodity = commodity;
        }

        public string Commodity { get; private set; }

        protected override string GetKeyForItem(MarketDataRow item)
        {
            return item.StationID;
        }
    }

    internal class StationMarket : Market
    {
        public StationMarket(string stationId)
        {
            StationID = stationId;
        }

        public string StationID { get; private set; }

        protected override string GetKeyForItem(MarketDataRow item)
        {
            return item.CommodityName;
        }
    }
}
