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
   public class Class1
    {

        public EdSystem Test()
        {
            var System = new EdSystem();
            System.Name = "LS 3482";
            System.Id = 1;

            var stationA = new EdStation();
            stationA.Id = 1;
            stationA.Name = "Eudoxus Dock";

            var CBeer = new EdCommodity();
            CBeer.Id = 1;
            CBeer.Name = "Beer";
            CBeer.Sell = 176;
            CBeer.Buy = 0;
            CBeer.Demand = 9755;
            CBeer.DemandRate = DemandSupplyRate.High;
            CBeer.Supply = 0;
            CBeer.SupplyRate = DemandSupplyRate.None;

            var CCrop = new EdCommodity();
            CCrop.Id = 2;
            CCrop.Name = "Crop Harvesters";
            CCrop.Sell = 1997;
            CCrop.Buy = 2023;
            CCrop.Demand = 0;
            CCrop.DemandRate = DemandSupplyRate.None;
            CCrop.Supply = 10655;
            CCrop.SupplyRate = DemandSupplyRate.Medium;

            stationA.Commodities.Add(CBeer);
            stationA.Commodities.Add(CCrop);

            System.Stations.Add(stationA);

            return System;

        }
    }
}
