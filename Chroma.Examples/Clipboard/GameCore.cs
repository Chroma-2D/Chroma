namespace Clipboard;

using System;
using System.Globalization;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;

public class GameCore : Game
{
    public GameCore() : base(new(false, false))
    {
    }
        
    protected override void Draw(RenderContext context)
    {
        context.DrawString(
            "Use <F1> to set clipboard text to the current date and time.\n\n" + 
            $"Current clipboard text: {Clipboard.Text}\n" +
            $"Clipboard has text? {Clipboard.HasText}",
            new Vector2(8)
        );
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        if (e.KeyCode == KeyCode.F1)
        {
            Clipboard.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }
    }
}