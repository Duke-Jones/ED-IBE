using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using RegulatedNoise.Annotations;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    internal class Commodities: ICollection<MarketDataRow>
    {
        public event EventHandler<MarketDataEventArgs> OnMarketDataUpdate
        {
            add { _allMarketDatas.OnMarketDataUpdate += value; }
            remove { _allMarketDatas.OnMarketDataUpdate -= value; }
        }

        protected class MarketDataCollection : Market
        {
            protected override string GetKeyForItem(MarketDataRow item)
            {
                return item.MarketDataId;
            }
        }

        protected abstract class MarketCollection<TMarket> : KeyedCollection<string, TMarket>
            where TMarket: ICollection<MarketDataRow>
        {
            public void Add([NotNull] MarketDataRow marketDataRow)
            {
                if (marketDataRow == null) throw new ArgumentNullException("marketDataRow");
                TMarket market;
                if (Dictionary == null || !Dictionary.TryGetValue(GetKey(marketDataRow), out market))
                {
                    market = NewMarket(marketDataRow);
                    Add(market);
                }
                market.Add(marketDataRow);
            }

            protected abstract string GetKey(MarketDataRow marketDataRow);

            public bool Remove([NotNull] MarketDataRow marketDataRow)
            {
                if (marketDataRow == null) throw new ArgumentNullException("marketDataRow");
                TMarket market;
                if (Dictionary == null || !Dictionary.TryGetValue(GetKey(marketDataRow), out market))
                {
                    return false;
                }
                else
                {
                    return market.Remove(marketDataRow);
                }
            }

            protected abstract TMarket NewMarket(MarketDataRow marketDataRow);

            public void AddRange([NotNull] IEnumerable<MarketDataRow> marketDataRows)
            {
                if (marketDataRows == null) throw new ArgumentNullException("marketDataRows");
                foreach (MarketDataRow marketDataRow in marketDataRows)
                {
                    Add(marketDataRow);
                }
            }

            public bool TryGetValue(string marketId, out TMarket market)
            {
                if (Dictionary == null)
                {
                    market = default(TMarket);
                    return false;
                }
                else
                {
                    return Dictionary.TryGetValue(marketId, out market);
                }
            }
        }

        protected class StationMarketCollection : MarketCollection<StationMarket>
        {
            protected override string GetKeyForItem(StationMarket item)
            {
                return item.StationID;
            }

            protected override string GetKey(MarketDataRow marketDataRow)
            {
                return marketDataRow.StationID;
            }

            protected override StationMarket NewMarket(MarketDataRow marketDataRow)
            {
                return new StationMarket(GetKey(marketDataRow));
            }
        }

        protected class CommodityMarketCollection : MarketCollection<CommodityMarket>
        {
            protected override string GetKeyForItem(CommodityMarket item)
            {
                return item.Commodity;
            }

            protected override string GetKey(MarketDataRow marketDataRow)
            {
                return marketDataRow.CommodityName;
            }

            protected override CommodityMarket NewMarket(MarketDataRow marketDataRow)
            {
                return new CommodityMarket(GetKey(marketDataRow));
            }
        }

        public int Count { get { return _allMarketDatas.Count; } }
        
        public bool IsReadOnly { get { return false; } }

        public MarketDataRow this[string marketDataId]
        {
            get { return _allMarketDatas[marketDataId]; }
        }

        private readonly StationMarketCollection _byStation;

        private readonly CommodityMarketCollection _byCommodity;

        private readonly MarketDataCollection _allMarketDatas;
        
        private readonly object _updating = new object();

        public Commodities()
        {
            _byStation = new StationMarketCollection();
            _byCommodity = new CommodityMarketCollection();
            _allMarketDatas = new MarketDataCollection();
        }

        public IEnumerable<MarketDataRow> StationMarket(string stationId)
        {
            return GetMarketDatas(stationId, _byStation);
        }

        public IEnumerable<MarketDataRow> CommodityMarket(string commodityName)
        {
            return GetMarketDatas(commodityName, _byCommodity);
        }

        protected IEnumerable<MarketDataRow> GetMarketDatas<TMarket>(string marketId, MarketCollection<TMarket> marketCollection)
            where TMarket : ICollection<MarketDataRow>
        {
            TMarket market;
            if (!marketCollection.TryGetValue(marketId, out market))
            {
                return new MarketDataRow[0];
            }
            else
            {
                return market;
            }
        }

        public void Add(MarketDataRow marketData)
        {
            _allMarketDatas.Add(marketData);
        }

        public bool Contains(MarketDataRow marketData)
        {
            return _allMarketDatas.Contains(marketData);
        }

        public void CopyTo(MarketDataRow[] array, int arrayIndex)
        {
            _allMarketDatas.CopyTo(array, arrayIndex);
        }

        public bool Remove([NotNull] MarketDataRow marketDataRow)
        {
            lock (_updating)
            {
                bool deleted = _allMarketDatas.Delete(marketDataRow);
                if (deleted)
                {
                    _byStation.Remove(marketDataRow);
                    _byCommodity.Remove(marketDataRow);
                }
                return deleted;
            }
        }

        public void Clear()
        {
            lock (_updating)
            {
                _allMarketDatas.Clear();
                _byStation.Clear();
                _byCommodity.Clear();
            }
        }

        public void Update([NotNull] MarketDataRow marketDataRow)
        {
            lock (_updating)
            {
                Market.UpdateState update = _allMarketDatas.Update(marketDataRow);
                switch (update)
                {
                    case Market.UpdateState.Added:
                        _byStation.Add(marketDataRow);
                        _byCommodity.Add(marketDataRow);
                        break;
                    case Market.UpdateState.Replace:
                        _byStation.Remove(marketDataRow);
                        _byCommodity.Remove(marketDataRow);
                        break;
                    case Market.UpdateState.Discarded:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(update + " unhandled update operation");
                }
            }
            
            //if (_allMarketDatas.Update(marketDataRow))
            //{
            //    _byStation[marketDataRow.StationID][marketDataRow.MarketDataId] = marketDataRow;
            //}
            //_byStation.Update(marketDataRow);
            //_byCommodity.Update(marketDataRow);
        }

        public void UpdateRange([NotNull] IEnumerable<MarketDataRow> marketDataRows)
        {
            if (marketDataRows == null) throw new ArgumentNullException("marketDataRows");
            foreach (MarketDataRow marketDataRow in marketDataRows)
            {
                Update(marketDataRow);
            }
        }

        public IEnumerator<MarketDataRow> GetEnumerator()
        {
            return _allMarketDatas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class MarketDataEventArgs: EventArgs
    {
        public readonly MarketDataRow Previous;

        public readonly MarketDataRow Actual;

        public bool IsAdded
        {
            get { return Previous == null; }
        }

        public bool IsRemoved
        {
            get { return Actual == null; }
        }

        public bool IsReplaced
        {
            get { return Previous != null && Actual != null; }
        }

        public MarketDataEventArgs(MarketDataRow previous = null, MarketDataRow actual = null)
        {
            Debug.Assert(previous != null || actual != null, "at least one marketdata should not be null");
            Previous = previous;
            Actual = actual;
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
