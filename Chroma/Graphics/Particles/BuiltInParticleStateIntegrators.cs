namespace Chroma.Graphics.Particles
{
    public static class BuiltInParticleStateIntegrators
    {
        public static void LinearStateIntegrator(Particle part, float delta)
        {
            part.Color.A = (byte)(255 * (float)part.TTL / part.InitialTTL);
            part.Position += part.Velocity * delta;
            part.Scale = part.InitialScale * ((float)part.TTL / part.InitialTTL);
            part.Rotation += part.Velocity.Length() * delta;
        }
    }
}