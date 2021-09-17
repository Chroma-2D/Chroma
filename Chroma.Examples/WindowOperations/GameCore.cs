using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using Chroma;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Windowing;
using Chroma.Windowing.DragDrop;
using Color = Chroma.Graphics.Color;

namespace WindowOperations
{
    public class GameCore : Game
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        private bool _drawCenterVector;
        private int _lastResult;

        private string _lastDroppedText;
        private string _lastDroppedFileList;

        public GameCore() : base(new(false, false))
        {
            Window.FilesDropped += WindowOnFilesDropped;
            Window.TextDropped += WindowOnTextDropped;
            
            var sb = new StringBuilder();

            var exts = Graphics.GlExtensions;
            for (var i = 0; i < exts.Count; i++)
            {
                _log.Info(exts[i]);
            }

            var displays = Graphics.GetDisplayList().ToArray();

            for (var i = 0; i < displays.Length; i++)
            {
                sb.AppendLine($"Display {i} DPI: {displays[i].DPI}");
                sb.AppendLine($"Display {i} Bounds: {displays[i].Bounds}");
                sb.AppendLine($"Display {i} Desktop Bounds: {displays[i].DesktopBounds}");

                sb.AppendLine($"Display {i} supports:");
                var modes = displays[i].QuerySupportedDisplayModes();

                foreach (var m in modes)
                    sb.AppendLine($"  {m.Width}x{m.Height}@{m.RefreshRate}");
            }

            _log.Info(sb.ToString());
        }

        private void WindowOnTextDropped(object sender, TextDragDropEventArgs e)
        {
            _lastDroppedText = e.Text;
            _log.Info($"Text has been dropped onto the game window:\n{e.Text}.");
        }

        private void WindowOnFilesDropped(object sender, FileDragDropEventArgs e)
        {
            _lastDroppedFileList = string.Join('\n', e.Files);
            _log.Info($"Files have been dropped onto the game window:\n{_lastDroppedFileList}.");
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
                "Use <F4> to switch into 1024x600 windowed mode - hold any <Shift> to center the window afterwards.\n" +
                "Use <F5> toggle always-on-top status of the window.\n" +
                $"Use <F6> to toggle window border ({Window.EnableBorder}).\n" +
                "Use <F7> to set maximum window size to 800x600.\n" +
                "Use <F8> to set minimum window size to 320x240.\n" +
                "Use <F9> to reset minimum and maximum window sizes.\n" +
                $"Use <F10> to cycle between display synchronization modes ({Graphics.VerticalSyncMode}).\n" +
                $"Use <F11> to show a cross-platform message box (last result: {_lastResult}).\n" +
                "Use <space> to toggle the center vector on/off.\n" +
                $"Use <~> to toggle window hit testing ({Window.IsHitTestEnabled}).\n\n" +
                $"Current viewport resolution: {Window.Size.Width}x{Window.Size.Height}\n" +
                $"Maximum screen dimensions: {Window.MaximumSize.Width}x{Window.MaximumSize.Height}\n" +
                $"Minimum screen dimensions: {Window.MinimumSize.Width}x{Window.MinimumSize.Height}\n" +
                $"Has keyboard focus: {Window.HasKeyboardFocus}\n" +
                $"Is mouse over: {Window.IsMouseOver}\n" +
                $"Last dropped file: {_lastDroppedFileList}\n" +
                $"Last dropped text: {_lastDroppedText}\n",
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
                Window.GoWindowed(
                    new Size(1024, 600),
                    e.IsAnyShiftPressed
                );
            }
            else if (e.KeyCode == KeyCode.F5)
            {
                Window.TopMost = !Window.TopMost;
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
                    .WithButton("Okay...", _ => _drawCenterVector = true)
                    .WithButton("/shrug", _ => _drawCenterVector = false)
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
            else if (e.KeyCode == KeyCode.Tilde)
            {
                if (Window.IsHitTestEnabled)
                    Window.HitTest = null;
                else
                    Window.HitTest = HitTest;
            }
        }

        private WindowHitTestResult HitTest(Window window, Point point)
        {
            return WindowHitTestResult.Draggable;
        }
    }
}