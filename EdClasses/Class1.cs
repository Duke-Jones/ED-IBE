using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EdClasses.ClassDefinitions;
using EdClasses.ClassDefinitions.Commodities;

namespace EdClasses
{
    //Test Data
   public class Class1
    {

        public EdSystem Test()
        {
            var system = new EdSystem {Name = "LS 3482", Id = 1};

            var stationA = new EdStation {Id = 1, Name = "Eudoxus Dock"};

            var cBeer = new EdCommodity
            {
                Id = 1,
                Name = "Beer",
                Sell = 176,
                Buy = 0,
                Demand = 9755,
                DemandRate = DemandSupplyRate.High,
                Supply = 0,
                SupplyRate = DemandSupplyRate.None
            };

            var cCrop = new EdCommodity
            {
                Id = 2,
                Name = "Crop Harvesters",
                Sell = 1997,
                Buy = 2023,
                Demand = 0,
                DemandRate = DemandSupplyRate.None,
                Supply = 10655,
                SupplyRate = DemandSupplyRate.Medium
            };

            stationA.Commodities.Add(cBeer);
            stationA.Commodities.Add(cCrop);

            system.Stations.Add(stationA);

            return system;

        }
    }
}
