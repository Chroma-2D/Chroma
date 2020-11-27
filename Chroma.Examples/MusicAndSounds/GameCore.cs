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
        private TrackerModule _module;
        private SidTune _lastNinja;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _doomShotgun = Content.Load<Sound>("Sounds/doomsg.wav");
            _groovyMusic = Content.Load<Music>("Music/groovy.mp3");
            _module = Content.Load<TrackerModule>("Music/beyond.mo3");
            _lastNinja = Content.Load<SidTune>("Music/Last_Ninja_2.sid");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to start/stop the groovy music ({_groovyMusic.Status}) [{_groovyMusic.PositionSeconds}/{_groovyMusic.Length}].\n" +
                "Use <F2> to pause/resome the groovy music.\n" +
                $"Use <F3> to start/stop the Last Ninja 2 SID tune ({_lastNinja.Status}).\n" +
                "Use <F4> to pause/resume the Last Ninja 2 SID tune.\n" +
                $"Use <space> to play the shotgun sound. ({_doomShotgun.Status})\n" +
                $"Use <F4>/<F5> to tweak the shotgun sound volume -/+ ({_doomShotgun.Volume}).\n" +
                $"Use <F6>/<F7> to tweak the master volume -/+ ({Audio.MasterVolume}).",
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
                
                case KeyCode.F3:
                    if (_lastNinja.Status == PlaybackStatus.Playing || _lastNinja.Status == PlaybackStatus.Paused)
                        _lastNinja.Stop();
                    else
                        _lastNinja.Play();
                    break;
                
                case KeyCode.F4:
                    if(_lastNinja.Status == PlaybackStatus.Playing)
                        _lastNinja.Pause();
                    else if(_lastNinja.Status == PlaybackStatus.Paused)
                        _lastNinja.Play();
                    break;
                
                case KeyCode.Space:
                    _doomShotgun.ForcePlay();
                    break;
                
                case KeyCode.F5:
                    _doomShotgun.Volume--;
                    break;
                
                case KeyCode.F6:
                    _doomShotgun.Volume++;
                    break;
                
                case KeyCode.F7:
                    Audio.MasterVolume--;
                    break;
                
                case KeyCode.F8:
                    Audio.MasterVolume++;
                    break;
            }
        }
    }
}