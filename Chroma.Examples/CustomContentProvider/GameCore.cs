using System.IO;
using System.Numerics;
using Chroma;
using Chroma.Audio;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace CustomContentProvider
{
    public class GameCore : Game
    {
        private Sound _shotgun;
        private Texture _texture;
        private float _rotation;

        public GameCore() : base(false) // suppress default scene construction
        {
            Content.Dispose();
            Content = new ZipContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common/assets.zip"));
        }

        protected override void LoadContent()
        {
            _texture = Content.Load<Texture>("Textures/pentagram.png");
            _texture.FilteringMode = TextureFilteringMode.NearestNeighbor;
            
            _shotgun = Content.Load<Sound>("doomsg.wav");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawTexture(
                _texture, 
                (Window.Center - _texture.Center), 
                new Vector2(8), 
                _texture.Center,
                _rotation
            );
        }

        protected override void Update(float delta)
        {
            _rotation += 15 * delta;
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _shotgun.PlayOneShot();
            }
        }
    }
}