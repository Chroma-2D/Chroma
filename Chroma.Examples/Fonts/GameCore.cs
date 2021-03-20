using System;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;

namespace Fonts
{
    public class GameCore : Game
    {
        private TrueTypeFont _republika;
        private TrueTypeFont _alienlines;
        private TrueTypeFont _renegade;
        private TrueTypeFont _plasticBag;

        private BitmapFont _plasticBagBmf;

        public GameCore() : base(new(false, false))
        {
            Content = new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }
        
        protected override void LoadContent()
        {
            _republika = Content.Load<TrueTypeFont>("Fonts/republika.ttf", 32);
            _alienlines = Content.Load<TrueTypeFont>("Fonts/alienlines.ttf", 24);
            _renegade = Content.Load<TrueTypeFont>("Fonts/renegade.otf", 48);
            _plasticBag = Content.Load<TrueTypeFont>("Fonts/plasticbag.ttf", 16);
            _plasticBagBmf = Content.Load<BitmapFont>("BitmapFonts/plasticbag.fnt");
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

            var alienlinesStr = "This is a test of 24px high Alien Lines TTF.\n" +
                                "And here's its line break.1234567890!@#$%^&*()_+_-=[];'\\";
            context.DrawString(
                _alienlines,
                alienlinesStr,
                new Vector2(8, 96),
                Color.Aqua
            );

            var measure1 = _alienlines.Measure(alienlinesStr);
            context.Rectangle(
                ShapeMode.Stroke,
                new Vector2(8, 96),
                measure1.Width,
                measure1.Height,
                Color.Red);

            var plasticBagBmfStr = "This is a test of 16px high Plastic Bag Bitmap Font.\n" +
                      "And here's its line break.\n1234567890-=!@#$aA";
            context.DrawString(
                _plasticBagBmf,
                plasticBagBmfStr,
                new Vector2(8, 164),
                Color.Lime
            );

            var measure2 = _plasticBagBmf.Measure(plasticBagBmfStr);
            context.Rectangle(
                ShapeMode.Stroke,
                new Vector2(8, 164),
                measure2.Width,
                measure2.Height,
                Color.Red
            );

            var plasticBagTtfStr = "This is a test of 16px high Plastic Bag TTF.\n" +
                                   "And here's its line break. 1234567890-=!@#$aA";
            var measure3 = _plasticBag.Measure(plasticBagTtfStr);
            context.DrawString(
                _plasticBag,
                plasticBagTtfStr,
                new Vector2(16 + measure2.Width, 164),
                Color.Violet
            );
            
            context.Rectangle(
                ShapeMode.Stroke,
                new Vector2(16 + measure2.Width, 164),
                measure3.Width,
                measure3.Height,
                Color.Red
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
                _republika.IsKerningEnabled = !_republika.IsKerningEnabled;
                _alienlines.IsKerningEnabled = !_alienlines.IsKerningEnabled;
                _plasticBag.IsKerningEnabled = !_plasticBag.IsKerningEnabled;
                _plasticBagBmf.IsKerningEnabled = !_plasticBagBmf.IsKerningEnabled;
                _renegade.IsKerningEnabled = !_renegade.IsKerningEnabled;
            }
        }
    }
}