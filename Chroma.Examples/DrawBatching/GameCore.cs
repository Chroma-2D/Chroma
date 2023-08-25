using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.Batching;
using Chroma.Input;

namespace DrawBatching
{
    public class GameCore : Game
    {
        private Texture _texA;
        private Texture _texB;
        private Texture _texC;

        private DrawOrder _order;

        public GameCore() : base(new(false, false))
        {
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            _texA = Content.Load<Texture>("Textures/burg.png");
            
            _texB = Content.Load<Texture>("Textures/pentagram.png");
            _texB.VirtualResolution = new Size(256, 256);
            _texB.FilteringMode = TextureFilteringMode.NearestNeighbor;
            
            _texC = Content.Load<Texture>("Textures/walls.jpeg");
        }

        protected override void Draw(RenderContext context)
        {
            context.Batch((ctx) => ctx.DrawTexture(_texA, new Vector2(48, 48), Vector2.One, Vector2.Zero, 0f), 0);
            context.Batch((ctx) => ctx.DrawTexture(_texC, new Vector2(72, 72), Vector2.One, Vector2.Zero, 0f), 1);
            context.Batch((ctx) => ctx.DrawTexture(_texB, new Vector2(96, 96), Vector2.One, Vector2.Zero, 0f), 2);

            context.DrawBatch(_order);

            context.DrawString(
                "Use <F1> to change the order the batch in back-to-front fashion.\n" +
                "Use <F2> to change the order the batch in front-to-back fashion.",
                new Vector2(8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                _order = DrawOrder.BackToFront;
            }
            else if(e.KeyCode == KeyCode.F2)
            {
                _order = DrawOrder.FrontToBack;
            }
        }
    }
}