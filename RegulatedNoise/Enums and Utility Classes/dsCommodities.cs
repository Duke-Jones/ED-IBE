using System;
using System.Collections.Generic;
using System.Linq;
using RegulatedNoise.Core.DomainModel;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
	public partial class dsCommodities: ILocalizer
	{
		/// <summary>
		/// translate <param name="commodityName"></param> from current application language
		/// <see cref="RegulatedNoiseSettings.Language"/> in english
		/// </summary>
		public string GetCommodityBasename(string commodityName)
		{
			enLanguage language = ApplicationContext.RegulatedNoiseSettings.Language;
			return GetCommodityBasename(language, commodityName);
		}

		/// <summary>
		/// translate <param name="commodityName"></param> in english
		/// </summary>
		/// <param name="language"><param name="commodityName"> language</param></param>
		/// <param name="commodityName"></param>
		public string GetCommodityBasename(enLanguage language, string commodityName)
		{
			string baseName = String.Empty;
			NamesRow[] currentCommodity = null;

			switch (language)
			{
				case enLanguage.eng:
					currentCommodity = (NamesRow[])(Names.Select("eng='" + commodityName + "'"));
					break;
				case enLanguage.ger:
					currentCommodity = (NamesRow[])(Names.Select("ger='" + commodityName + "'"));
					break;
				case enLanguage.fra:
					currentCommodity = (NamesRow[])(Names.Select("fra='" + commodityName + "'"));
					break;
			}

			if (currentCommodity != null && currentCommodity.Any())
				baseName = currentCommodity[0].eng;

			return baseName;

		}

		/// <summary>
		/// translate <param name="commodityName"></param> in current application language
		/// <see cref="RegulatedNoiseSettings.Language"/>
		/// </summary>
		/// <param name="commodityName">Name of the commodity.</param>
		/// <returns></returns>
		public string GetLocalizedCommodity(string commodityName)
		{
			return TranslateToCurrent(commodityName);
		}

		public string TranslateToCurrent(string toLocalize)
		{
			return TranslateIn(ApplicationContext.RegulatedNoiseSettings.Language, toLocalize);
		}

		/// <summary>
		/// Translates <param name="commodityName"></param> in english.
		/// </summary>
		/// <param name="commodityName">Name of the commodity.</param>
		/// <returns></returns>
		public string TranslateInEnglish(string commodityName)
		{
			return TranslateIn(enLanguage.eng, commodityName);
		}

		/// <summary>
		/// Translates <param name="commodityName"></param> in given language <param name="language"></param>.
		/// </summary>
		/// <param name="language">The target language.</param>
		/// <param name="commodityName">Name of the commodity.</param>
		/// <returns></returns>
		public string TranslateIn(enLanguage language, string commodityName)
		{
			string baseName = String.Empty;
			List<NamesRow> currentCommodity =
				 Names.Where(x => ((x.eng.Equals(commodityName, StringComparison.InvariantCultureIgnoreCase)) ||
										 (x.ger.Equals(commodityName, StringComparison.InvariantCultureIgnoreCase)) ||
										 (x.fra.Equals(commodityName, StringComparison.InvariantCultureIgnoreCase)))).ToList();

			if (currentCommodity.Any())
			{
				switch (language)
				{
					case enLanguage.eng:
						baseName = currentCommodity[0].eng;
						break;
					case enLanguage.ger:
						baseName = currentCommodity[0].ger;
						break;
					case enLanguage.fra:
						baseName = currentCommodity[0].fra;
						break;
				}
			}
			return baseName;
		}
	}
}
