using System;
using Chroma.Graphics;

namespace CellularAutomaton
{
    public class CorruptorCell : Cell
    {
        private static Random _rnd = new();
        private static int _totalCorruption = 1;
        private static byte _effectiveness = 9;

        public CorruptorCell(Map map, int x, int y)
            : base(map, x, y)
        {
            Color = new Color(
                _totalCorruption + (byte)(255 * Math.Sinh(Map.Width / 2 - y)), 
                0, 
                _totalCorruption + (byte)(255 * Math.Tanh(Map.Width / 2 - y)),
                (byte)(_rnd.NextSingle() * 255)
            );
            

        }

        public override void Update(float delta)
        {
            var (n, w, s, e) = (
                Map.GetNeighbor(this, Map.Direction.North),
                Map.GetNeighbor(this, Map.Direction.West),
                Map.GetNeighbor(this, Map.Direction.South),
                Map.GetNeighbor(this, Map.Direction.East)
            );

            if (n != null && n is FaderCell && n.Color.A > 255 - _effectiveness)
                Grow(n.X, n.Y);

            if (w != null && w is FaderCell && w.Color.A > 255 - _effectiveness)
                Grow(w.X, w.Y);

            if (s != null && s is FaderCell && s.Color.A > 255 - _effectiveness)
                Grow(s.X, s.Y);

            if (e != null && e is FaderCell && e.Color.A > 255 - _effectiveness)
                Grow(e.X, e.Y);
        }

        private void Grow(int x, int y)
        {
            Map.SpawnCell<CorruptorCell>(x, y);
        }
    }
}