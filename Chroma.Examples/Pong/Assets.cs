using Chroma.ContentManagement;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Pong
{
    public static class Assets
    {
        public static Texture Stretchy { get; private set; }
        public static TrueTypeFont ScoreFont { get; private set; }
        
        public static void Load(IContentProvider content)
        {
            Stretchy = content.Load<Texture>("Sprites/stretchy.png");
            ScoreFont = content.Load<TrueTypeFont>("Fonts/visitor2.ttf", 120);
            ScoreFont.HintingMode = HintingMode.Monochrome;
        }
    }
}