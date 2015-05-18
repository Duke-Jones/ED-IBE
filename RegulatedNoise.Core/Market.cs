using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RegulatedNoise.Annotations;
using RegulatedNoise.Core.DomainModel;

namespace RegulatedNoise.Core
{
	public abstract class Market : IReadOnlyCollection<MarketDataRow>
	{
		protected readonly SortedDictionary<string, MarketDataRow> Dictionary;

		public int Count { get { return Dictionary.Count; } }

		public MarketDataRow this[string marketdataId]
		{
			get { return Dictionary[marketdataId]; }
			internal set { Dictionary[marketdataId] = value; }
		}

		protected Market()
		{
			Dictionary = new SortedDictionary<string, MarketDataRow>();
		}

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
					string key = GetKeyForItem(marketData);
					if (Dictionary.TryGetValue(key, out existing))
					{
						if (marketData.SampleDate > existing.SampleDate)
						{
							Dictionary[key] = marketData;
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
				RaiseMarketDataUpdate(new MarketDataEventArgs(previous: existing, actual: marketData));
			}
			return updateState;
		}
		
		public void Add(MarketDataRow marketDataRow)
		{
			Dictionary.Add(GetKeyForItem(marketDataRow), marketDataRow);
		}

		public void Set(MarketDataRow marketDataRow)
		{
			Dictionary[GetKeyForItem(marketDataRow)] = marketDataRow;
		}

		public bool Remove(MarketDataRow marketDataRow)
		{
			return Dictionary.Remove(GetKeyForItem(marketDataRow));
		}

		public void Clear()
		{
			Dictionary.Clear();
		}

		public bool Contains(string marketDataId)
		{
			return Dictionary.ContainsKey(marketDataId);
		}

		public bool Contains(MarketDataRow marketData)
		{
			return Dictionary.ContainsKey(GetKeyForItem(marketData));
		}

		public void CopyTo(MarketDataRow[] array, int arrayIndex)
		{
			Dictionary.Values.CopyTo(array, arrayIndex);
		}

		protected abstract string GetKeyForItem(MarketDataRow marketDataRow);

		public bool NotifiedRemove([NotNull] MarketDataRow marketDataRow)
		{
			if (marketDataRow == null) throw new ArgumentNullException("marketDataRow");
			bool removed;
			lock (_updating)
			{
				removed = Remove(marketDataRow);
			}
			if (removed)
			{
				RaiseMarketDataUpdate(new MarketDataEventArgs(previous: marketDataRow));
			}
			return removed;
		}

		public void RemoveAll(Predicate<MarketDataRow> filter)
		{
			lock (_updating)
			{
				var toRemove = this.Where(md => filter(md)).ToList();
				foreach (var marketDataRow in toRemove)
				{
					NotifiedRemove(marketDataRow);
				}
			}
		}

		protected void RaiseMarketDataReplace(MarketDataRow existing, MarketDataRow update)
		{
			RaiseMarketDataUpdate(new MarketDataEventArgs(previous: existing, actual: update));
		}

		protected void RaiseNewMarketData(MarketDataRow newlyAdded)
		{
			RaiseMarketDataUpdate(new MarketDataEventArgs(actual: newlyAdded));
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

		public IEnumerator<MarketDataRow> GetEnumerator()
		{
			return Dictionary.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}