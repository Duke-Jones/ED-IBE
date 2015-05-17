using System;

namespace RegulatedNoise.Core.DomainModel
{
    public enum ProposalLevel
    {
        Low,
        Med,
        High
    }

    public static class ProposalLevelExtensions
    {
        public static string Display(this ProposalLevel? proposalLevel)
        {
            if (proposalLevel.HasValue)
            {
                return proposalLevel.ToString().ToUpper();
            }
            else
            {
                return String.Empty;
            }
        }

        public static ProposalLevel? ToProposalLevel(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            else
            {
                ProposalLevel level;
                if (Enum.TryParse(text, true, out level))
                {
                    return level;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}