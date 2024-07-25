namespace CellularAutomaton;

using System;
using Chroma.Graphics;

public class FaderCell : Cell
{
    private static Random _rnd = new();

    private int _fadeDirection = -1;
    private float _alpha;

    public float FadeSpeed { get; private set; }
        
    public FaderCell(Map map, int x, int y) 
        : base(map, x, y)
    {
        _alpha = _rnd.NextSingle();
        FadeSpeed = _rnd.NextSingle() + 1.0f;

        var rgb = new byte[3];
        _rnd.NextBytes(rgb);
            
        Color = new Color(0, rgb[1], 0, (byte)(255 * _alpha));
    }

    public override void Update(float delta)
    {

        if (_alpha <= 0)
            _fadeDirection = 1;
        else if (_alpha >= 1.0f)
            _fadeDirection = -1;

        _alpha += _fadeDirection * (FadeSpeed * delta);

        if (_alpha > 1.0f)
            _alpha = 1.0f;
        else if (_alpha <= 0.0f)
            _alpha = 0.0f;
            
        Color = new Color(Color.R, Color.G, Color.B, (byte)(255 * _alpha));
    }
}