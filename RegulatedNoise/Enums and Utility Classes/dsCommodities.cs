using System;
using System.Collections.Generic;
using System.Linq;

namespace RegulatedNoise.Enums_and_Utility_Classes {
    
    
    public partial class dsCommodities {

        /// <summary>
        /// prepares the commodities in the correct language
        /// </summary>
        public string GetCommodityBasename(string commodityName)
        {
            enLanguage language = ApplicationContext.RegulatedNoiseSettings.Language;
            return GetCommodityBasename(language, commodityName);
        }

        /// <summary>
        /// prepares the commodities in the correct language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="commodityName"></param>
        public string GetCommodityBasename(enLanguage language, string commodityName)
        {
            string baseName = String.Empty;
            dsCommodities.NamesRow[] currentCommodity = null;

            switch (language)
            {
                case enLanguage.eng:
                    currentCommodity = (dsCommodities.NamesRow[])(Names.Select("eng='" + commodityName + "'"));
                    break;
                case enLanguage.ger:
                    currentCommodity = (dsCommodities.NamesRow[])(Names.Select("ger='" + commodityName + "'"));
                    break;
                case enLanguage.fra:
                    currentCommodity = (dsCommodities.NamesRow[])(Names.Select("fra='" + commodityName + "'"));
                    break;
            }

            if (currentCommodity != null && currentCommodity.Any())
                baseName = currentCommodity[0].eng;

            return baseName;

        }

        /// <summary>
        /// prepares the commodities in the correct language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="commodityName"></param>
        public string GetLocalizedCommodity(string commodityName)
        {
            enLanguage language = ApplicationContext.RegulatedNoiseSettings.Language;
            string baseName = String.Empty;

            List<dsCommodities.NamesRow> currentCommodity = Names.Where(x => ((x.eng.Equals(commodityName, StringComparison.InvariantCultureIgnoreCase)) ||
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
