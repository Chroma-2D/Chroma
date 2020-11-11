using System;
using System.Drawing;
using System.Numerics;
using System.Text;
using Chroma;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.Windowing;
using Color = Chroma.Graphics.Color;

namespace WindowOperations
{
    public class GameCore : Game
    {
        private bool _drawCenterVector;
        private int _lastResult;

        private Log _log = LogManager.GetForCurrentAssembly();

        public GameCore()
        {
            var sb = new StringBuilder();

            var displays = Graphics.GetDisplayList();

            for (var i = 0; i < displays.Count; i++)
            {
                sb.AppendLine($"Display {i} DPI: {displays[i].DPI}");
                sb.AppendLine($"Display {i} Bounds: {displays[i].Bounds}");
                sb.AppendLine($"Display {i} Desktop Bounds: {displays[i].DesktopBounds}");

                sb.AppendLine($"Display {i} supports:");
                var modes = displays[i].QuerySupportedDisplayModes();

                for (var j = 0; j < modes.Count; j++)
                {
                    var m = modes[j];
                    sb.AppendLine($"  {m.Width}x{m.Height}@{m.RefreshRate}");
                }
            }

            _log.Info(sb.ToString());
        }

        protected override void Draw(RenderContext context)
        {
            if (_drawCenterVector)
            {
                context.Line(
                    Mouse.GetPosition(),
                    Window.Center,
                    Color.Lime
                );
            }

            context.DrawString(
                $"Use <F1> to toggle window resizable status ({Window.CanResize}).\n" +
                "Use <F2> to switch into exclusive fullscreen mode with native resolution\n" +
                "Use <F3> to switch into borderless fullscreen mode with native resolution\n" +
                "Use <F4> to switch into 1024x600 windowed mode - hold <Lshift> to center the window afterwards.\n" +
                "Use <F5> to change the title - hold <Lshift> to reset it.\n" +
                $"Use <F6> to toggle window border ({Window.EnableBorder}).\n" +
                $"Use <F7> to set maximum window size to 800x600.\n" +
                $"Use <F8> to set minimum window size to 320x240.\n" +
                $"Use <F9> to reset minimum and maximum window sizes to 0x0.\n" +
                $"Use <F10> to cycle between display synchronization modes ({Graphics.VerticalSyncMode}).\n" +
                $"Use <F11> to show a cross-platform message box (last result: {_lastResult}).\n" +
                $"Use <space> to toggle the center vector on/off.\n\n" +
                $"Current viewport resolution: {Window.Size.Width}x{Window.Size.Height}\n" +
                $"Maximum screen dimensions: {Window.MaximumSize.Width}x{Window.MaximumSize.Height}\n" +
                $"Minimum screen dimensions: {Window.MinimumSize.Width}x{Window.MinimumSize.Height}",
                new Vector2(8)
            );
        }

        protected override void Update(float delta)
        {
            Window.Title =
                $"FPS: {PerformanceCounter.FPS}, frame {PerformanceCounter.LifetimeFrames}. On display: {Window.CurrentDisplay.Index}";
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                Window.CanResize = !Window.CanResize;
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                Window.GoFullscreen(true);
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                Window.GoFullscreen(false); // alternatively don't pass any parameters
                // because they're optional for borderless native fullscreen
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                var center = false;

                if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                    center = true;

                Window.GoWindowed(new Size(1024, 600), center);
            }
            else if (e.KeyCode == KeyCode.F5)
            {
                var msg = "F5 key was pressed!";

                if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                    msg = "Chroma Framework";

                Window.Title = msg;
            }
            else if (e.KeyCode == KeyCode.F6)
            {
                Window.EnableBorder = !Window.EnableBorder;
            }
            else if (e.KeyCode == KeyCode.F7)
            {
                Window.MaximumSize = new Size(800, 600);
            }
            else if (e.KeyCode == KeyCode.F8)
            {
                Window.MinimumSize = new Size(320, 240);
            }
            else if (e.KeyCode == KeyCode.F9)
            {
                Window.MaximumSize = Window.MinimumSize = Size.Empty;
            }
            else if (e.KeyCode == KeyCode.F10)
            {
                // <YandereDevMode IgnoreSassyComments="true"
                //                 IfElseGoodGoodCodeBad="yesplease">
                if (Graphics.VerticalSyncMode == VerticalSyncMode.Adaptive)
                {
                    Graphics.VerticalSyncMode = VerticalSyncMode.None;
                }
                else if (Graphics.VerticalSyncMode == VerticalSyncMode.None)
                {
                    Graphics.VerticalSyncMode = VerticalSyncMode.Retrace;
                }
                else if (Graphics.VerticalSyncMode == VerticalSyncMode.Retrace)
                {
                    Graphics.VerticalSyncMode = VerticalSyncMode.Adaptive;
                }
                // </YandereDevMode>
            }
            else if (e.KeyCode == KeyCode.Space)
            {
                _drawCenterVector = !_drawCenterVector;
            }
            else if (e.KeyCode == KeyCode.F11)
            {
                _lastResult = new MessageBox(MessageBoxSeverity.Information)
                    .Titled("Test message box")
                    .WithMessage("This is a test message. For testing!")
                    .WithButton("Alright?")
                    .WithButton("Okay...", i => _drawCenterVector = true)
                    .WithButton("/shrug", i => _drawCenterVector = false)
                    .HandleAbnormalClosureWith(() => Console.WriteLine("Fukc. MessageBox closed abnormally."))
                    .Show(Window);
            }
            else if (e.KeyCode == KeyCode.F12)
            {
                MessageBox.Show(
                    MessageBoxSeverity.Error,
                    "A test error message box.",
                    "This is a test message to let you know about the error.",
                    Window
                );
            }
        }
    }
}