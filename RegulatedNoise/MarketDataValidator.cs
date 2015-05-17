#region file header
// ////////////////////////////////////////////////////////////////////
// ///
// ///  
// /// 16.05.2015
// ///
// ///
// ////////////////////////////////////////////////////////////////////
#endregion

using RegulatedNoise.Core.DataProviders;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise
{
	public class MarketDataValidator: IValidator<MarketDataRow>
	{
		public PlausibilityState Validate(MarketDataRow marketDataRow)
		{
			return ApplicationContext.Milkyway.IsImplausible(marketDataRow, marketDataRow.Source == Eddn.SOURCENAME || marketDataRow.Source == EddbDataProvider.SOURCENAME || marketDataRow.Source == TradeDangerousDataProvider.SOURCENAME);
		}
	}
}