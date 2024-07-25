namespace Scissors;

using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

public class GameCore : Game
{
    private bool _useScissor = true;
    private Rectangle _scissor = new(64, 64, 128, 128);
    private Texture _grid;

    public GameCore() : base(new(false, false))
    {
    }

    protected override IContentProvider InitializeContentPipeline()
    {
        return new FileSystemContentProvider(
            Path.Combine(AppContext.BaseDirectory, "../../../../_common")
        );
    }

    protected override void Initialize(IContentProvider content)
    {
        _grid = content.Load<Texture>("Textures/grid.png");
    }

    protected override void Update(float delta)
    {
    }

    protected override void Draw(RenderContext context)
    {
        if (_useScissor)
            RenderSettings.Scissor = _scissor;

        context.DrawTexture(_grid, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);

        if (_useScissor)
        {
            RenderSettings.Scissor = Rectangle.Empty;

            RenderSettings.LineThickness = 1;
            context.Rectangle(
                ShapeMode.Stroke,
                _scissor,
                Color.Red
            );
        }

        context.DrawString(
            "Use <F1> to toggle scissor on/off.\n" +
            "Use <Left> <Right> <Up> <Down> to move the scissor around the screen.\nHold shift to modify the size instead.",
            new Vector2(8),
            Color.Red
        );
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        if (e.KeyCode == KeyCode.F1)
        {
            _useScissor = !_useScissor;
        }

        if (e.KeyCode == KeyCode.Up)
        {
            if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                _scissor.Height -= 10;
            else
                _scissor.Y -= 10;
        }
        else if (e.KeyCode == KeyCode.Down)
        {
            if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                _scissor.Height += 10;
            else
                _scissor.Y += 10;
        }

        if (e.KeyCode == KeyCode.Left)
        {
            if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                _scissor.Width -= 10;
            else
                _scissor.X -= 10;
        }
        else if (e.KeyCode == KeyCode.Right)
        {
            if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                _scissor.Width += 10;
            else
                _scissor.X += 10;
        }
    }
}