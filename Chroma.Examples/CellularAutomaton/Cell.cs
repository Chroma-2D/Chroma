using Chroma.Graphics;

namespace CellularAutomaton
{
    public class Cell
    {
        protected Map Map { get; }
        
        public int X { get; }
        public int Y { get; }

        public Color Color { get; set; } = Color.CornflowerBlue;

        public Cell(Map map, int x, int y)
        {
            Map = map;
            
            X = x;
            Y = y;
        }

        public void Draw(RenderContext context)
        {
            // +2 because opengl rasterization.
            context.Pixel(X + 2, Y + 2, Color);
        }

        public virtual void Update(float delta)
        {
        }
    }
}