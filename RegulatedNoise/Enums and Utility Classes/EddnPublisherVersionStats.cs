namespace RegulatedNoise.Enums_and_Utility_Classes
{
    internal class EddnPublisherVersionStats
    {
        public int MessagesReceived { get; set; }
        public string Publisher { get; private set; }

        public EddnPublisherVersionStats(string publisher)
        {
            Publisher = publisher;
        }

        public override string ToString()
        {
            return Publisher + " : " + MessagesReceived + " messages";
        }
    }
}
