using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
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
        private TrueTypeFont _copam8x8;

        private BitmapFont _plasticBagBmf;

        public GameCore() : base(new(false, false))
        {
            Content = new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );

            Graphics.VerticalSyncMode = VerticalSyncMode.None;
            Cursor.IsVisible = false;
        }
        
        protected override void LoadContent()
        {
            _republika = Content.Load<TrueTypeFont>("Fonts/republika.ttf", 32);
            _republika.PreferAutoHinter = false;
            
            _alienlines = Content.Load<TrueTypeFont>("Fonts/alienlines.ttf", 24);
            _renegade = Content.Load<TrueTypeFont>("Fonts/renegade.otf", 48);
            _plasticBag = Content.Load<TrueTypeFont>("Fonts/plasticbag.ttf", 16);
            _copam8x8 = Content.Load<TrueTypeFont>("Fonts/Copam_8x8.ttf", 16);
            _plasticBagBmf = Content.Load<BitmapFont>("BitmapFonts/plasticbag.fnt");
        }

        protected override void Update(float delta)
        {
            Window.Title = $"Mem [MGD: {(GC.GetTotalMemory(true) / 1024f / 1024f).ToString("F3")}MB]" +
                           $" | [ALL: {(Process.GetCurrentProcess().PrivateMemorySize64 / 1024f / 1024f).ToString("F3")}MB]";
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
                new Vector2(8, 152),
                Color.Lime
            );
            
            var measure2 = _plasticBagBmf.Measure(plasticBagBmfStr);
            context.Rectangle(
                ShapeMode.Stroke,
                new Vector2(8, 152),
                measure2.Width,
                measure2.Height,
                Color.Red
            );
            
            var plasticBagTtfStr = "This is a test of 16px high Plastic Bag TTF.\n" +
                                   "And here's its line break. 1234567890-=!@#$aA";
            context.DrawString(
                _plasticBag,
                plasticBagTtfStr,
                new Vector2(16 + measure2.Width, 152),
                Color.Violet
            );
            
            var measure3 = _plasticBag.Measure(plasticBagTtfStr);
            context.Rectangle(
                ShapeMode.Stroke,
                new Vector2(16 + measure2.Width, 152),
                measure3.Width,
                measure3.Height,
                Color.Red
            );
            
            context.DrawString(
                _renegade,
                "This is a test of 48px high\n" +
                "Renegade OTF.",
                new Vector2(8, 204),
                Color.HotPink
            );

            context.DrawString(
                _copam8x8,
                _renegade.HintingMode.ToString() + $"\nHinting: {_renegade.HintingEnabled}\nAutoHint: {_renegade.PreferAutoHinter}",
                Mouse.GetPosition()
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _renegade.HintingMode = (HintingMode)((((int)_renegade.HintingMode) + 1) % 3);
            }

            if (e.KeyCode == KeyCode.F1)
            {
                _renegade.PreferAutoHinter = !_renegade.PreferAutoHinter;
            }

            if (e.KeyCode == KeyCode.F2)
            {
                _renegade.HintingEnabled = !_renegade.HintingEnabled;
            }
        }
    }
}