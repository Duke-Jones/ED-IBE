using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RegulatedNoise.Core.DomainModel;

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

	    public static async Task<IEnumerable<Tuple<string, double>>> GetBestRoundTripsAsync(ICollection<string> stationPerimeter
		    , double? maxDistance
		    , bool showIncomeByLightYear
		    , IProgress<Tuple<string, int, int>> onProgress
		    , CancellationToken cancellationToken)
	    {
		    var allRoundTrips = new List<Tuple<string, double>>();
		    int total = (stationPerimeter.Count / 2 * (stationPerimeter.Count / 2 + 1)) / 2;
		    int current = 0;
		    onProgress.Report(new Tuple<string, int, int>(String.Format("calculating best routes: from {0} stations",
			    stationPerimeter.Count), current, total));
		    int bestRoundTrip = 0;

		    foreach(string stationFrom in stationPerimeter)
		    {
			    foreach (string stationTo in stationPerimeter.Reverse())
			    {
				    cancellationToken.ThrowIfCancellationRequested();
				    int lexicalOrder = String.Compare(stationFrom, stationTo, StringComparison.InvariantCultureIgnoreCase);
				    if (lexicalOrder == 0) // same stations
				    {
					    break;
				    }
				    current += 1;
				    double distance = ApplicationContext.Milkyway.DistanceInLightYears(MarketDataRow.StationIdToSystemName(stationFrom),
					    MarketDataRow.StationIdToSystemName(stationTo));
				    if ((maxDistance.HasValue) && (distance > maxDistance))
				    {
					    continue; 
				    }
				    onProgress.Report(new Tuple<string, int, int>(null, current, total));
				    Debug.Print(current + "/" + total);
				    int currentTripIncome = await Task.Run(() => {
																						 int tripIncome;
																						 GetBestRoundTripBetweenTwoStations(stationFrom, stationTo, out tripIncome);
																						 return tripIncome;
																					}, cancellationToken).ConfigureAwait(false);
				    if (currentTripIncome > 0)
				    {
					    string key1, key2;
					    if (lexicalOrder < 0)
					    {
						    key1 = stationFrom;
						    key2 = stationTo;
					    }
					    else
					    {
						    key1 = stationTo;
						    key2 = stationFrom;
					    }
					    string credits;
					    double creditsDouble;

					    if (showIncomeByLightYear && distance < Double.MaxValue)
					    {
						    creditsDouble = currentTripIncome/(2.0*distance);
						    credits = String.Format("{0:0.000}", creditsDouble/(2.0*distance)) + " Cr/Ly";
					    }
					    else
					    {
						    creditsDouble = currentTripIncome;
						    credits = (currentTripIncome + " Cr");
					    }
						
					    allRoundTrips.Add(
						    new Tuple<string, double>(
							    credits.PadRight(13) + " : " + key1 + " <-> " + key2
							    , creditsDouble));

					    if (currentTripIncome > bestRoundTrip)
					    {
						    bestRoundTrip = currentTripIncome;
					    }
				    }
			    }
		    }
		    return allRoundTrips;
	    }
    }
}