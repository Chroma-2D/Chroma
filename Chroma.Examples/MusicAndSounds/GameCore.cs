using System.IO;
using System.Numerics;
using Chroma;
using Chroma.Audio;
using Chroma.Audio.Sources;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;

namespace MusicAndSounds
{
    public class GameCore : Game
    {
        private Sound _doomShotgun;
        private Music _groovyMusic;

        public GameCore()
        {
            Content = new FileSystemContentProvider(Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _doomShotgun = Content.Load<Sound>("Sounds/doomsg.wav");
            _groovyMusic = Content.Load<Music>("Music/groovy.mp3");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to start/stop the groovy music ({_groovyMusic.Status}).\n" +
                "Use <F2> to pause/unpause the groovy music.\n" +
                $"Use <space> to play the shotgun sound. ({_doomShotgun.Status})\n" +
                $"Use <F3>/<F4> to tweak the shotgun sound volume -/+ ({_doomShotgun.Volume}).\n" +
                $"Use <F5>/<F6> to tweak the groovy music volume -/+ ({Audio.MusicVolume}).",
                new Vector2(8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    if (_groovyMusic.Status == PlaybackStatus.Playing || _groovyMusic.Status == PlaybackStatus.Paused)
                        _groovyMusic.Stop();
                    else
                        _groovyMusic.Play();
                    break;
                
                case KeyCode.F2:
                    if(_groovyMusic.Status == PlaybackStatus.Playing)
                        _groovyMusic.Pause();
                    else if(_groovyMusic.Status == PlaybackStatus.Paused)
                        _groovyMusic.Play();
                    break;
                
                case KeyCode.Space:
                    _doomShotgun.PlayOneShot();
                    break;
                
                case KeyCode.F3:
                    _doomShotgun.Volume--;
                    break;
                
                case KeyCode.F4:
                    _doomShotgun.Volume++;
                    break;
                
                case KeyCode.F5:
                    Audio.MusicVolume--;
                    break;
                
                case KeyCode.F6:
                    Audio.MusicVolume++;
                    break;
            }
        }
    }
}