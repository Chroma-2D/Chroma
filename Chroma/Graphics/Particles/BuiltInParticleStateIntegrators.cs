using System;

namespace Chroma.Graphics.Particles
{
    public static class BuiltInParticleStateIntegrators
    {
        public static void FadeOut(Particle part, float delta)
            => part.Color.A = (byte)(255 * (float)part.TTL / part.InitialTTL);

        public static void SineCurvePosition(Particle part, float delta)
            => part.Position += part.Velocity * delta * MathF.Sin((float)part.TTL / part.InitialTTL);

        public static void ScaleDown(Particle part, float delta)
            => part.Scale = part.InitialScale * ((float)part.TTL / part.InitialTTL);

        public static void LinearPosition(Particle part, float delta)
            => part.Position += part.Velocity * delta;
        
        public static void VelocityBasedRotation(Particle part, float delta)
            => part.Rotation += part.Velocity.Length() * delta;

        public static void SlowDownX(Particle part, float delta)
            => part.Velocity.X = part.InitialVelocity.X * ((float)part.TTL / part.InitialTTL);

        public static void SlowDownY(Particle part, float delta)
            => part.Velocity.Y = part.InitialVelocity.Y * ((float)part.TTL / part.InitialTTL);
    }
}