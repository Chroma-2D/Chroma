using System;
using System.Globalization;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;

namespace Clipboard
{
    public class GameCore : Game
    {
        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "Use <F1> to set clipboard text to the current date and time.\n\n" + 
                $"Current clipboard text: {Chroma.Clipboard.Text}\n" +
                $"Clipboard has text? {Chroma.Clipboard.HasText}",
                new Vector2(8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                Chroma.Clipboard.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}