using System.Collections.Generic;
using EdClasses.ClassDefinitions.Commodities;

namespace EdClasses.ClassDefinitions
{
    public interface IEdStation
    {
        int Id { get; set; } //Should be predefined
        string Name { get; set; }
        List<IEdCommodity> Commodities { get; set; }
    }
    
    public class EdStation : IEdStation
    {
        public EdStation()
        {
            Commodities = new List<IEdCommodity>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IEdCommodity> Commodities { get; set; }
    }
}
