using System;
using System.Diagnostics;

namespace RegulatedNoise.Core.DomainModel
{
	public class DataModel
	{
		public event EventHandler<ValidationEventArgs> OnValidationEvent;

		public event EventHandler<MarketDataEventArgs> OnMarketDataUpdate
		{
			add { _galacticMarket.OnMarketDataUpdate += value; }
			remove { _galacticMarket.OnMarketDataUpdate -= value; }
		}

		private readonly ILocalizer _localizer;
		private readonly IValidator<MarketDataRow> _marketDataValidator;

		private Commodities _commodities;
		public Commodities Commodities
		{
			get
			{
				if (_commodities == null)
					_commodities = new Commodities(_localizer);
				return _commodities;
			}
		}

		private GalacticMarket _galacticMarket;
		public GalacticMarket GalacticMarket
		{
			get
			{
				if (_galacticMarket == null)
					_galacticMarket = new GalacticMarket();
				return _galacticMarket;
			}
		}

		private Universe _universe;
		public Universe Universe
		{
			get
			{
				if (_universe == null)
					_universe = new Universe();
				return _universe;
			}
		}

		public DataModel(ILocalizer localizer, IValidator<MarketDataRow> marketDataValidator)
		{
			if (marketDataValidator == null)
			{
				throw new ArgumentNullException("marketDataValidator");
			}
			_localizer = localizer;
			_marketDataValidator = marketDataValidator;
		}

		public void UpdateMarket(MarketDataRow marketdata)
		{
			PlausibilityState plausibility = _marketDataValidator.Validate(marketdata);
			if (plausibility.Plausible)
			{
				GalacticMarket.Update(marketdata);
			}
			else
			{
				RaiseValidationEvent(new ValidationEventArgs(plausibility));
			}
		}

		protected virtual void RaiseValidationEvent(ValidationEventArgs e)
		{
			var handler = OnValidationEvent;
			if (handler != null)
				try
				{
					handler(this, e);
				}
				catch (Exception ex)
				{
					Trace.TraceError("validation notification failure: " + ex);
				}
		}
	}

	public interface IValidator<in TEntity>
	{
		PlausibilityState Validate(TEntity entity);
	}

	public class ValidationEventArgs : EventArgs
	{
		public readonly PlausibilityState PlausibilityState;

		public ValidationEventArgs(PlausibilityState plausibilityState)
		{
			PlausibilityState = plausibilityState;
		}
	}
}