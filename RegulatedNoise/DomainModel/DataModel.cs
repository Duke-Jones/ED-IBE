namespace RegulatedNoise.DomainModel
{
    internal class DataModel
    {
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

        private GlobalMarket _globalMarket;
        public GlobalMarket GlobalMarket
        {
            get
            {
                if (_globalMarket == null)
                    _globalMarket = new GlobalMarket();
                return _globalMarket;
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
    }
}