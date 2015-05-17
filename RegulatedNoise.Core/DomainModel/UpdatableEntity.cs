using System;

namespace RegulatedNoise.Core.DomainModel
{
    public abstract class UpdatableEntity
    {
        public const string UNKNOWN_SOURCE = "Unknown";
        
        public string Source { get; set; }

        protected UpdatableEntity()
        {
            Source = UNKNOWN_SOURCE;
        }

        /// <summary>
        /// copy the values from another system exept for the ID
        /// </summary>
        /// <param name="source">The source system.</param>
        /// <param name="updateMode">The update mode.</param>
        protected void UpdateFrom(UpdatableEntity source, UpdateMode updateMode)
        {
            bool doCopy = updateMode == UpdateMode.Clone || updateMode == UpdateMode.Copy;
            if (doCopy || String.IsNullOrEmpty(Source))
            {
                Source = source.Source;
            }
            else if (!Source.Contains(source.Source))
            {
                Source = Source + "@" + source.Source;
            }
        }
    }
}