using Chroma.Audio.Sources;

namespace Chroma.Audio.Spatial
{
    // todo decide if 3d processing is really necessary
    internal abstract class SpatialProperties<T> where T : AudioSource
    {
        protected T Owner { get; }

        public abstract bool EnableProcessing { get; set; }
        public abstract bool EnableDistanceDelay { get; set; }
        public abstract bool IsRelativeToListener { get; set; }
        
        public abstract float MinimumDistance { get; set; }
        public abstract float MaximumDistance { get; set; }

        public abstract uint AttenuationModel { get; set; }
        public abstract float AttenuationFactor { get; set; }

        public abstract float DopplerFactor { get; set; }
        
        //todo: investigate 3D colliders
        //todo: investigate 3D attenuators

        internal SpatialProperties(T owner)
            => Owner = owner;
    }
}