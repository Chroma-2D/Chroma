using System;
using System.IO;
using System.Numerics;
using Chroma.MemoryManagement;

namespace Chroma.Graphics
{
    public class Sprite : DisposableResource
    {
        protected Texture Texture { get; }

        public Vector2 Position;
        public Vector2 Scale = Vector2.One;
        public Vector2 Origin;
        public Vector2 Shearing;

        public float Rotation;

        public Sprite(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The file path provided does not exist.");

            Texture = new Texture(filePath);
        }

        public Sprite(Texture texture)
        {
            if (texture.Disposed)
                throw new ArgumentException("Texture you're trying to use was already disposed.", nameof(texture));

            Texture = texture;
        }

        public virtual void Draw(RenderContext context)
        {
            if (Shearing != Vector2.Zero)
            {
                context.Transform.Push();
                context.Transform.Shear(Shearing);
            }

            context.DrawTexture(
                Texture, 
                Position, 
                Scale,
                Origin,
                Rotation
             );

            if (Shearing != Vector2.Zero)
            {
                context.Transform.Pop();
            }
        }

        protected override void FreeManagedResources()
        {
            Texture.Dispose();
        }
    }
}