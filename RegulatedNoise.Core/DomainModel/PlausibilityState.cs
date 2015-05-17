namespace RegulatedNoise.Core.DomainModel
{
	public struct PlausibilityState
    {
        public readonly bool Plausible;

        public readonly string Comments;

        public PlausibilityState(bool plausible) 
            : this()
        {
            Plausible = plausible;
        }

        public PlausibilityState(bool plausible, string comments)
            :this(plausible)
        {
            Comments = comments;
        }
    }
}