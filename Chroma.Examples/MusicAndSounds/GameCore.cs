using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.Audio;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace MusicAndSounds
{
    public class GameCore : Game
    {
        private Sound _iSawedTheDemons;
        private Sound _hymnToAurora;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
            Window.GoWindowed(new Size(1024, 600));
        }

        protected override void LoadContent()
        {
            _iSawedTheDemons = Content.Load<Sound>("Sounds/i_sawed_the_demons.ogg");
            _hymnToAurora = Content.Load<Sound>("Sounds/aurora.mod");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to play/stop the OGG format music: ({_iSawedTheDemons.State}).\n" +
                $"Use <F2> to play/stop the tracker module: ({_hymnToAurora.State}).\n",
                new Vector2(8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    if (_iSawedTheDemons.State == AudioSourceState.Playing ||
                        _iSawedTheDemons.State == AudioSourceState.Paused)
                        _iSawedTheDemons.Stop();
                    else
                        _iSawedTheDemons.Play();
                    break;
                
                case KeyCode.F2:
                    if(_hymnToAurora.State == AudioSourceState.Playing ||
                       _hymnToAurora.State == AudioSourceState.Paused)
                        _hymnToAurora.Stop();
                    else
                        _hymnToAurora.Play();
                    break;
            }
        }
    }
}