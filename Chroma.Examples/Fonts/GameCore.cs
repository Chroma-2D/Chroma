using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Fonts
{
    public class GameCore : Game
    {
        private TrueTypeFont _republika;
        private TrueTypeFont _alienlines;
        private TrueTypeFont _renegade;

        private BitmapFont _plasticBag;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }
        
        protected override void LoadContent()
        {
            _republika = Content.Load<TrueTypeFont>("Fonts/republika.ttf", 32);
            _alienlines = Content.Load<TrueTypeFont>("Fonts/alienlines.ttf", 24);
            _renegade = Content.Load<TrueTypeFont>("Fonts/renegade.otf", 48);

            _plasticBag = Content.Load<BitmapFont>("BitmapFonts/plasticbag.fnt");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                _republika, 
                "This is a test of 32px high Republika TTF.\n" +
                "And here's its line break.",
                new Vector2(8), 
                Color.Red
            );
            
            context.DrawString(
                _alienlines,
                "This is a test of 24px high Alien Lines TTF.\n" +
                "And here's its line break.",
                new Vector2(8, 96),
                Color.Aqua
            );
            
            context.DrawString(
                _plasticBag,
                "This is a test of 16px high Plastic Bag bitmap font.\n" +
                "And here's its line break.",
                new Vector2(8, 164),
                Color.Lime
            );
            
            context.DrawString(
                _renegade,
                "This is a test of 48px high\n" +
                "Renegade OTF.\n" +
                "And here's its line break.",
                new Vector2(8, 196),
                Color.HotPink
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _plasticBag.UseKerning = !_plasticBag.UseKerning;
            }
        }
    }
}