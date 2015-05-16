using System;
using System.Collections.Generic;
using System.Linq;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.Trading
{
    public class TradeEngine
    {
        public static int TradeRouteIncome(MarketDataRow fromRow, MarketDataRow toRow)
        {
            if (fromRow.CommodityName == toRow.CommodityName
                && fromRow.BuyPrice > 0 && fromRow.Stock > 0
                && toRow.Demand > 0)
            {
                return toRow.SellPrice - fromRow.BuyPrice;
            }
            else
            {
                return 0;
            }
        }

        public static TradeRoute CreateTradeRoute(MarketDataRow fromRow, MarketDataRow toRow)
        {
            return new TradeRoute(fromRow, toRow, ApplicationContext.Milkyway.DistanceInLightYears(fromRow.SystemName, toRow.SystemName));
        }

        public static Tuple<IEnumerable<TradeRoute>, IEnumerable<TradeRoute>> GetBestRoundTripBetweenTwoStations(string stationFrom, string stationTo, out int bestRoundTrip)
        {
            if (stationFrom == null || stationTo == null) { bestRoundTrip = 0; return null; }
            var resultsOutbound = new List<TradeRoute>();
            var resultsReturn = new List<TradeRoute>();
            int outwardIncome = 0;
            int returnIncome = 0;
            IEnumerable<MarketDataRow> toStationMarket = ApplicationContext.GalacticMarket.StationMarket(stationTo);

            foreach (var fromRow in ApplicationContext.GalacticMarket.StationMarket(stationFrom))
            {
                MarketDataRow toRow = toStationMarket.FirstOrDefault(x => x.CommodityName == fromRow.CommodityName);

                if (fromRow == null || toRow == null) continue;

                int tradeRouteIncome = TradeRouteIncome(fromRow, toRow);
                if (tradeRouteIncome > 0)
                {
                    resultsOutbound.Add(CreateTradeRoute(fromRow, toRow));
                    outwardIncome = Math.Max(outwardIncome, tradeRouteIncome);
                }

                tradeRouteIncome = TradeRouteIncome(toRow, fromRow);
                if (tradeRouteIncome > 0)
                {
                    resultsReturn.Add(CreateTradeRoute(toRow, fromRow));
                    returnIncome = Math.Max(returnIncome, tradeRouteIncome);
                }
            }
            bestRoundTrip = outwardIncome + returnIncome;
            return new Tuple<IEnumerable<TradeRoute>, IEnumerable<TradeRoute>>(resultsOutbound, resultsReturn);
        }
    }
}