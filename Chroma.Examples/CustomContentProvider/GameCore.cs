using System;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.Graphics;

namespace CustomContentProvider
{
    public class GameCore : Game
    {
        private Texture _texture;
        private float _rotation;

        public GameCore() : base(false) // suppress default scene construction
        {
            Content.Dispose();
            Content = new ZipContentProvider(Path.Combine(LocationOnDisk, "../../../../_common/assets.zip"));
        }

        protected override void LoadContent()
        {
            _texture = Content.Load<Texture>("Textures/pentagram.png");
            _texture.FilteringMode = TextureFilteringMode.NearestNeighbor;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawTexture(_texture, (Window.Center - _texture.Center), new Vector2(8), _texture.Center, _rotation);
        }

        protected override void Update(float delta)
        {
            _rotation += 15 * delta;
        }
    }
}