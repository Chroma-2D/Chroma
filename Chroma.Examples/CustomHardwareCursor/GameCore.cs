namespace CustomHardwareCursor;

using System;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;

public class GameCore : Game
{
    private Cursor _cursor;

    public GameCore() : base(new(false, false))
    {
    }

    protected override IContentProvider InitializeContentPipeline()
    {
        // in normal use cases this is completely unnecessary
        // but there is more than one example project
        // so sharing between them is necessary to keep the
        // source tree clean
        return new FileSystemContentProvider(
            Path.Combine(AppContext.BaseDirectory, "../../../../_common")
        );
    }

    protected override void Initialize(IContentProvider content)
    {
        _cursor = content.Load<Cursor>("Cursors/cursor.png");
    }

    protected override void Draw(RenderContext context)
    {
        context.DrawString(
            "Press <F1> to toggle between default and custom cursor.\n" +
            "Press <F2> to toggle the cursor's visibility.\n" +
            $"Press <F3> to confine the cursor to the current window (captured? {Mouse.IsCaptured}).\n" +
            $"Mouse screen-space position: {Mouse.ScreenSpacePosition}\n" +
            $"Mouse window-space position: {Mouse.WindowSpacePosition}\n",
            new Vector2(8, 8)
        );
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        if (e.KeyCode == KeyCode.F1)
        {
            if (!_cursor.IsCurrent) _cursor.SetCurrent();
            else Cursor.Reset();
        }
        else if (e.KeyCode == KeyCode.F2)
        {
            Cursor.IsVisible = !Cursor.IsVisible;
        }
        else if (e.KeyCode == KeyCode.F3)
        {
            Mouse.IsCaptured = !Mouse.IsCaptured;
        }
    }
}