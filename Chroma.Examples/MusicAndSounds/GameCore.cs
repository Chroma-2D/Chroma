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
        private Sound _doomSoundtrack;
        private Vector2 _mousePos;
        
        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
            Audio.DistanceModel = DistanceModel.LinearDistance;
            
            Window.GoWindowed(new Size(1024, 600));
        }

        protected override void LoadContent()
        {
            _doomSoundtrack = Content.Load<Sound>("Sounds/i_sawed_the_demons.ogg");
            _doomSoundtrack.RelativeToListener = true;
            _doomSoundtrack.Gain = 100; 
            
            Audio.ListenerPosition = new Vector2(Window.Center.X, Window.Center.Y);
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to play/stop the music: ({_doomSoundtrack.State}).\n" +
                $"Use <Num+>/<Num-> to control volume ({_doomSoundtrack.Gain}) of the music track.\n" +
                $" -> Hold <Ctrl> to control its minimum volume ({_doomSoundtrack.MaximumGain})\n" +
                $"     and <Shift> to control its maximum volume ({_doomSoundtrack.MinimumGain}).\n" +
                $"Listener position: {Audio.ListenerPosition}\n" +
                $"Sound position: {_doomSoundtrack.Position}",
                new Vector2(8)
            );
            
            context.Rectangle(
                ShapeMode.Stroke,
                new Vector2(Audio.ListenerPosition.X, Audio.ListenerPosition.Y) - new Vector2(20),
                40, 40, Color.HotPink
            );
            
            context.DrawString(
                "you",
                Window.Center - new Vector2(14, 13),
                Color.HotPink
            );
            
            context.Circle(
                ShapeMode.Fill,
                new Vector2(_doomSoundtrack.Position.X, _doomSoundtrack.Position.Y) - new Vector2(4),
                16,
                Color.Yellow
            );

            DrawOrientationForward(context);
            
            _doomSoundtrack.Position = new Vector2(_mousePos.X, _mousePos.Y);
        }

        private void DrawOrientationForward(RenderContext context)
        {
            context.Line(
                Window.Center,
                new Vector2(100 * Audio.Orientation[0].X, Window.Center.Y), 
                Color.Red
           );
            
            context.Line(
                Window.Center,
                new Vector2(Window.Center.X, 100 * Audio.Orientation[0].Y), 
                Color.CornflowerBlue
            );
            
            context.Line(
                Window.Center,
                new Vector2(Window.Center.X, -100 * Audio.Orientation[0].Z), 
                Color.Green
            );
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _mousePos = e.Position;
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    if (_doomSoundtrack.State == AudioSourceState.Playing ||
                        _doomSoundtrack.State == AudioSourceState.Paused)
                        _doomSoundtrack.Stop();
                    else
                        _doomSoundtrack.Play();
                    break;
                
                case KeyCode.F2:
                    Audio.DistanceModel = DistanceModel.ExponentDistance;
                    break;
                
                case KeyCode.F3:
                    Audio.DistanceModel = DistanceModel.InverseDistance;
                    break;
                
                case KeyCode.F4:
                    Audio.DistanceModel = DistanceModel.LinearDistance;
                    break;
                
                case KeyCode.F5:
                    Audio.DistanceModel = DistanceModel.ExponentDistanceClamped;
                    break;
                
                case KeyCode.F6:
                    Audio.DistanceModel = DistanceModel.InverseDistanceClamped;
                    break;
                
                case KeyCode.F7:
                    Audio.DistanceModel = DistanceModel.LinearDistanceClamped;
                    break;
                
                case KeyCode.F8:
                    Audio.DistanceModel = DistanceModel.None;
                    break;
                
                case KeyCode.NumPlus:
                    if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                        _doomSoundtrack.MinimumGain += 0.1f;
                    else if (e.Modifiers.HasFlag(KeyModifiers.LeftControl))
                        _doomSoundtrack.MaximumGain += 0.1f;
                    else
                        _doomSoundtrack.Gain += 0.1f;
                    break;
                
                case KeyCode.NumMinus:
                    if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                        _doomSoundtrack.MinimumGain -= 0.1f;
                    else if (e.Modifiers.HasFlag(KeyModifiers.LeftControl))
                        _doomSoundtrack.MaximumGain -= 0.1f;
                    else
                        _doomSoundtrack.Gain -= 0.1f;
                    break;
            }
        }
    }
}