namespace CellularAutomaton;

using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;

public class GameCore : Game
{
    private bool _go;
    private Map _map;

    public GameCore()
        : base(new(false, false))
    {
        Window.Mode.SetWindowed(1280, 800, true);
    }

    protected override void Initialize(IContentProvider content)
    {
        _map = new Map(384, 384);
        _map.RenderSize = new(768, 768);

        _map.Position = Window.Center - new Vector2(
            _map.RenderSize.Value.Width / 2,
            _map.RenderSize.Value.Height / 2
        );

        _map.SpawnCell<CorruptorCell>(_map.Width / 2, _map.Height / 2);
    }

    protected override void Draw(RenderContext context)
    {
        if (!_go) return;
        _map.Draw(context);
    }

    protected override void Update(float delta)
    {
        Window.Title = $"{PerformanceCounter.FPS} FPS";

        if (!_go) return;
        _map.Update(delta);
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        _go = true;
    }
}