using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using RegulatedNoise.Annotations;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    internal abstract class Market : KeyedCollection<string, MarketDataRow>
    {
        public enum UpdateState
        {
            Added,
            Replace,
            Discarded
        }

        private readonly object _updating = new object();
        public virtual event EventHandler<MarketDataEventArgs> OnMarketDataUpdate;

        public UpdateState Update([NotNull] MarketDataRow marketData)
        {
            if (marketData == null) throw new ArgumentNullException("marketData");
            MarketDataRow existing = null;
            UpdateState updateState;
            lock (_updating)
            {
                if (Dictionary == null)
                {
                    Add(marketData);
                    updateState = UpdateState.Added;
                }
                else
                {
                    if (Dictionary.TryGetValue(marketData.MarketDataId, out existing))
                    {
                        if (marketData.SampleDate > existing.SampleDate)
                        {
                            Remove(existing);
                            Add(marketData);
                            updateState = UpdateState.Replace;
                        }
                        else
                        {
                            //existing marketdata is newer
                            marketData = null;
                            updateState = UpdateState.Discarded;
                        }
                    }
                    else
                    {
                        Add(marketData);
                        updateState = UpdateState.Added;
                    }
                }
            }
            if (updateState != UpdateState.Discarded)
            {
                RaiseMarketDataUpdate(new MarketDataEventArgs(previous:existing, actual:marketData));
            }
            return updateState;
        }

        public bool Delete([NotNull] MarketDataRow marketDataRow)
        {
            if (marketDataRow == null) throw new ArgumentNullException("marketDataRow");
            bool removed;
            lock (_updating)
            {
                removed = Remove(marketDataRow);
            }
            if (removed)
            {
                RaiseMarketDataUpdate(new MarketDataEventArgs(previous:marketDataRow));                
            }
            return removed;
        }

        protected void RaiseMarketDataReplace(MarketDataRow existing, MarketDataRow update)
        {
            RaiseMarketDataUpdate(new MarketDataEventArgs(previous: existing, actual: update));
        }

        protected void RaiseNewMarketData(MarketDataRow newlyAdded)
        {
            RaiseMarketDataUpdate(new MarketDataEventArgs(actual:newlyAdded));
        }

        protected virtual void RaiseMarketDataUpdate(MarketDataEventArgs e)
        {
            var handler = OnMarketDataUpdate;
            if (handler != null)
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning("marketdata update notification failure " + ex);
                }
        }
    }
}