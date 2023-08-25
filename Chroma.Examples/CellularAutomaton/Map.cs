using System;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Color = Chroma.Graphics.Color;

namespace CellularAutomaton
{
    public class Map
    {
        public enum Direction
        {
            West,
            North,
            East,
            South
        }
        
        private Cell[] _cells;
        private RenderTarget _target;

        public Cell this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width) return null;
                if (y < 0 || y >= Height) return null;

                return _cells[y * Width + x];
            }

            set
            {
                if (x < 0 || x >= Width) return;
                if (y < 0 || y >= Height) return;

                _cells[y * Width + x] = value;
            }
        }
        
        public Vector2 Position { get; set; }

        public Size? RenderSize
        {
            get => _target.VirtualResolution;
            set => _target.VirtualResolution = value;
        }

        public int Width { get; }
        public int Height { get; }

        public Map(int width, int height)
        {
            _cells = new Cell[width * height];

            for (var i = 0; i < _cells.Length; i++)
            {
                _cells[i] = new FaderCell(
                    this,
                    i % width, i / width
                );
            }

            Width = width;
            Height = height;

            _target = new RenderTarget(width + 2, height + 2) { FilteringMode = TextureFilteringMode.NearestNeighbor };
        }

        public T SpawnCell<T>(int x, int y) where T : Cell
        {
            var cell = (T)Activator.CreateInstance(typeof(T), new object[] { this, x, y });
            _cells[y * Width + x] = cell;
            
            return cell;
        }
        
        public Cell GetNeighbor(Cell cell, Direction direction)
        {
            var (hoff, voff) = direction switch
            {
                Direction.West => (-1, 0),
                Direction.North => (0, -1),
                Direction.East => (1, 0),
                Direction.South => (0, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };

            return this[cell.X + hoff, cell.Y + voff];
        }

        public void Draw(RenderContext context)
        {
            context.RenderTo(_target, (ctx, tgt) =>
            {
                ctx.Clear(Color.White);
                ctx.Rectangle(ShapeMode.Fill, 1, 1, Height - 2, Width - 2, Color.Black);

                for (var i = 0; i < _cells.Length; i++)
                {
                    if (_cells[i] != null)
                    {
                        _cells[i]?.Draw(ctx);
                    }
                }
            });

            context.DrawTexture(_target, Position);
        }

        public void Update(float delta)
        {
            for (var i = 0; i < _cells.Length; i++)
                _cells[i]?.Update(delta);
        }
    }
}