using System.IO;
using System.Numerics;
using Chroma;
using Chroma.Audio;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;

namespace MusicAndSounds
{
    public class GameCore : Game
    {
        private Sound _trucksSound;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _trucksSound = Content.Load<Sound>("Sounds/i_sawed_the_demons.ogg");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to SAW THE FUCKING DEMONS ({_trucksSound.State}).\n",
                new Vector2(8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    if (_trucksSound.State == AudioSourceState.Playing || _trucksSound.State == AudioSourceState.Paused)
                        _trucksSound.Stop();
                    else
                        _trucksSound.Play();
                    break;
            }
        }
    }
}