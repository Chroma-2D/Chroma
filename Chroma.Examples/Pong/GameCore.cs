using Chroma;
using Chroma.Audio.Sfxr;
using Chroma.ContentManagement;
using Chroma.Graphics;

namespace Pong
{
    public class GameCore : Game
    {
        private Board _board;

        public GameCore() : base(new(false, false))
        {
            Window.Title = "Chroma Framework - Pong Example";
        }

        protected override void Initialize(IContentProvider content)
        {
            Sfxr.Initialize(this);
            Assets.Load(content);
            
            _board = new Board(Window.Size);
        }

        protected override void Update(float delta)
        {
            _board.Update(delta);
        }

        protected override void Draw(RenderContext context)
        {
            _board.Draw(context);
        }
    }
}