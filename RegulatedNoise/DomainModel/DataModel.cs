using System;
using System.Diagnostics;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.DomainModel
{
    internal class DataModel
    {
        public event EventHandler<ValidationEventArgs> OnValidationEvent;

        public event EventHandler<MarketDataEventArgs> OnMarketDataUpdate
        {
            add { _galacticMarket.OnMarketDataUpdate += value; }
            remove { _galacticMarket.OnMarketDataUpdate -= value; }
        }

        private Commodities _commodities;
        public Commodities Commodities
        {
            get
            {
                if (_commodities == null)
                    _commodities = new Commodities();
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

        public PlausibilityState Validate(MarketDataRow marketData, bool simpleEDDNCheck)
        {
            string baseName = Commodities.GetBasename(marketData.CommodityName);
            if (baseName == null)
            {
                return new PlausibilityState(false, "unknown commodity");
            }
            Commodity commodityData = Commodities.TryGet(baseName);

            if (commodityData == null)
            {
                return new PlausibilityState(false, "unregistered commodity");
            }

            PlausibilityState plausibility = new PlausibilityState(true);
            if (marketData.SupplyLevel.HasValue && marketData.DemandLevel.HasValue)
            {
                // demand AND supply !?
                plausibility = new PlausibilityState(false, "both demand and supply");
            }
            else if ((marketData.SellPrice <= 0) && (marketData.BuyPrice <= 0))
            {
                // both on 0 is not plausible
                plausibility = new PlausibilityState(false, "nor sell, nor buy price");
            }
            else if (marketData.SupplyLevel.HasValue || (simpleEDDNCheck && (marketData.Stock > 0)))
            {
                if (marketData.BuyPrice <= 0)
                {
                    plausibility = new PlausibilityState(false, "buy price not provided when demand available");
                }
                // check supply data             
                else if (commodityData.SupplyWarningLevels.Sell.IsInRange(marketData.SellPrice))
                {
                    // sell price is out of range
                    plausibility = new PlausibilityState(false, "sell price out of supply prices warn level "
                                                                + marketData.SellPrice
                                                                + " [" + commodityData.SupplyWarningLevels.Sell.Low +
                                                                "," + commodityData.SupplyWarningLevels.Sell.High +
                                                                "]");
                }
                else if (commodityData.SupplyWarningLevels.Buy.IsInRange(marketData.BuyPrice))
                {
                    // buy price is out of range
                    plausibility = new PlausibilityState(false, "buy price out of supply prices warn level "
                                                                + marketData.SellPrice
                                                                + " [" +
                                                                commodityData.SupplyWarningLevels.Buy.Low +
                                                                "," +
                                                                commodityData.SupplyWarningLevels.Buy.High +
                                                                "]");
                }
                if (marketData.Stock <= 0)
                {
                    // no supply quantity
                    plausibility = new PlausibilityState(false, "supply not provided");
                }
            }
            else if (marketData.DemandLevel.HasValue || (simpleEDDNCheck && (marketData.Demand > 0)))
            {
                // check demand data
                if (marketData.SellPrice <= 0)
                {
                    // at least the sell price must be present
                    plausibility = new PlausibilityState(false, "sell price not provided when supply available");
                }
                else if (commodityData.DemandWarningLevels.Sell.IsInRange(marketData.SellPrice))
                {
                    // buy price is out of range
                    plausibility = new PlausibilityState(false, "sell price out of demand prices warn level "
                                                                + marketData.SellPrice
                                                                + " [" +
                                                                commodityData.DemandWarningLevels.Sell.Low +
                                                                "," +
                                                                commodityData.DemandWarningLevels.Sell.High +
                                                                "]");
                }
                else if (marketData.BuyPrice > 0 && (commodityData.DemandWarningLevels.Buy.IsInRange(marketData.BuyPrice)))
                {
                    // buy price is out of range
                    plausibility = new PlausibilityState(false, "buy price out of supply prices warn level "
                                                                + marketData.BuyPrice
                                                                + " [" +
                                                                commodityData.DemandWarningLevels.Buy.Low +
                                                                "," +
                                                                commodityData.DemandWarningLevels.Buy.High +
                                                                "]");
                }

                if (marketData.Demand <= 0)
                {
                    // no demand quantity
                    plausibility = new PlausibilityState(false, "demand not provided");
                }
            }
            else
            {
                // nothing ?!
                plausibility = new PlausibilityState(false, "nor demand,nor supply provided");
            }
            return plausibility;
        }

        public void UpdateMarket(MarketDataRow marketdata)
        {
            var plausibility = Validate(marketdata, marketdata.Source == EDDN.SOURCENAME);
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

    internal class ValidationEventArgs : EventArgs
    {
        public readonly PlausibilityState PlausibilityState;

        public ValidationEventArgs(PlausibilityState plausibilityState)
        {
            PlausibilityState = plausibilityState;
        }
    }
}