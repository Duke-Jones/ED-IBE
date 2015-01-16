using System.Collections.Generic;

namespace EdClasses.ClassDefinitions.Commodities
{
    public interface IEdCommodity
    {
        int Id { get; set; }
        string Name { get; set; }
        int Sell { get; set; }
        int Buy { get; set; }
        int Demand { get; set; }
        DemandSupplyRate DemandRate { get; set; }
        int Supply { get; set; }
        DemandSupplyRate SupplyRate { get; set; }

        CommodityType CommodityType { get; }
    }

    public class EdCommodity : IEdCommodity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sell { get; set; }
        public int Buy { get; set; }
        public int Demand { get; set; }
        public DemandSupplyRate DemandRate { get; set; }
        public int Supply { get; set; }
        public DemandSupplyRate SupplyRate { get; set; }

        private CommodityType commodityType { get; set; }

        public CommodityType CommodityType
        {
            get
            {
                if (commodityType != new CommodityType()) return commodityType;
                
                //search knowncommodities values after name and return key
                foreach (var knownCommodityType in _knownCommodities)
                {
                    if (knownCommodityType.Value.Contains(Name))
                    {
                        commodityType = knownCommodityType.Key;
                        return commodityType;
                    }

                }
                return CommodityType.Unknown;
            }
        }

        //This will be moved out to a database when the time comes, so its a bit dirty
        private readonly Dictionary<CommodityType, List<string>> _knownCommodities = new Dictionary<CommodityType, List<string>>
                {
                    {CommodityType.Chemicals, new List<string> {"Chemical Drugs", "Explosives", "Hydrogen Fuel"}},
                    {CommodityType.ConsumerItems, new List<string> {"Clothing", "Consumer Technology", "Domestic Appliances"}},
                    {CommodityType.Foods, new List<string> {"Algae", "Animal Meat", "Coffee", "Fish", "Food Cartridges", "Fruit and Vegetables", "Grain", "Synthetic Meat", "Tea"}},
                    {CommodityType.IndustrialMaterials, new List<string> {"Polymers", "Semiconductors", "Superconductors"}},
                    {CommodityType.LegalDrugs, new List<string> {"Beer", "Liquor", "Narcotics", "Tobacco", "Wine"}},
                    {CommodityType.Machinery, new List<string> {"Atmospheric Processors", "Crop Harvesters", "Marine Equiptment", "Microbial Furnaces", "Mineral Extractors", "Power Generators", "Water Purifiers"}},
                    {CommodityType.Medicines, new List<string> {"Agri-Medicines", "Basic Medicines", "Combat Stabilisers", "Progenitor Cells"}},
                    {CommodityType.Metals, new List<string> {"Aluminium", "Beryllium", "Cobalt", "Copper", "Gallium", "Gold", "Indium", "Lithium", "Palladium", "Platinum", "Silver", "Tantalum", "Titanium", "Uranium"}},
                    {CommodityType.Minerals, new List<string> {"Bauxite", "Bertrandite", "Coltan", "Gallite", "Indite", "Lepidolite", "Rutile", "Uraninite"}},
                    {CommodityType.Salvage, new List<string> {"Black Box", "Technical Blueprints", "Trade Data"}},
                    {CommodityType.Slavery, new List<string> {"Imperial Slaves", "Slaves"}},
                    {CommodityType.Technology, new List<string> {"Advanced Catalysers", "Animal Monitors", "Aquaponic Systems", "Artificial Habitat Modules", "Auto Fabricators", "Bioreducing Lichen", "Computer Components", "H.E. Suits", "Land Enrichment Systems", "Resonating Separators", "Robotics"}},
                    {CommodityType.Textiles, new List<string> {"Leather", "Natural Fabrics", "Synthetic Fabrics"}},
                    {CommodityType.Waste, new List<string> {"Biowaste", "Chemical Waste", "Scrap"}},
                    {CommodityType.Weapons, new List<string> {"Non Lethal Weapons", "Personal Weapons", "Reactive Armour", "Battle Weapons"}},
                };
    }


    public enum DemandSupplyRate
    {
        Low,
        Medium,
        High,
        None
    }

    public enum CommodityType
    {
        Unknown,
        Chemicals,
        ConsumerItems,
        Foods,
        IndustrialMaterials,
        LegalDrugs,
        Machinery,
        Medicines,
        Metals,
        Minerals,
        Salvage,
        Slavery,
        Technology,
        Textiles,
        Waste,
        Weapons
    }
}
