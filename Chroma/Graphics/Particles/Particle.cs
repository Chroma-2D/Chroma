using System.Numerics;

namespace Chroma.Graphics.Particles
{
    public class Particle
    {
        public float Opacity { get; set; }
        public float Rotation { get; set; }

        public int TTL { get; set; }
        
        public Vector2 Scale { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Direction { get; set; }
    }
}