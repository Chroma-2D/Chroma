using System;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace KeyboardInput
{
    public class GameCore : Game
    {
        private KeyCode _lastPressedKeyCode;
        private ScanCode _lastPressedScanCode;

        private KeyCode _lastReleasedKeyCode;
        private ScanCode _lastReleasedScanCode;

        private Vector2 _pos = new(256);
        private Vector2 _spd = new(256);
        private Texture _burg;

        public GameCore()
            : base(new(false, false))
        {
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            _burg = Content.Load<Texture>("Textures/burg.png");
        }

        protected override void Update(float delta)
        {
            var vposd = _spd.Y * delta;
            var hposd = _spd.X * delta;

            if (Keyboard.IsKeyDown(KeyCode.Up))
                _pos.Y -= vposd;
            else if (Keyboard.IsKeyDown(KeyCode.Down))
                _pos.Y += vposd;

            if (Keyboard.IsKeyDown(KeyCode.Left))
                _pos.X -= hposd;
            else if (Keyboard.IsKeyDown(KeyCode.Right))
                _pos.X += hposd;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawTexture(
                _burg,
                _pos,
                new(0.5f)
            );

            context.DrawString(
                $"Last pressed key code: {(_lastPressedKeyCode == 0 ? "none" : _lastPressedKeyCode.ToString() + " (SC: " + Keyboard.KeyCodeToScanCode(_lastPressedKeyCode) + ")")}\n" +
                $"Last released key code: {(_lastReleasedKeyCode == 0 ? "none" : _lastReleasedKeyCode.ToString() + " (SC: " + Keyboard.KeyCodeToScanCode(_lastReleasedKeyCode) + ")")}\n\n" +
                $"Last pressed scan code: {(_lastPressedScanCode == 0 ? "none" : _lastPressedScanCode.ToString() + " (KC: " + Keyboard.ScanCodeToKeyCode(_lastPressedScanCode) + ")")}\n" +
                $"Last released scan code: {(_lastReleasedScanCode == 0 ? "none" : _lastReleasedScanCode.ToString() + " (KC: " + Keyboard.ScanCodeToKeyCode(_lastReleasedScanCode) + ")")}\n\n" +
                $"Currently active modifiers: {Keyboard.ActiveModifiers.ToString()}\n" +
                $"Currently pressed keys: {string.Join(", ", Keyboard.ActiveKeys)}" +
                "\n\nUse arrow keys to move the burger around.",
                16, 16, Color.White
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            _lastPressedKeyCode = e.KeyCode;
            _lastPressedScanCode = e.ScanCode;
        }

        protected override void KeyReleased(KeyEventArgs e)
        {
            _lastReleasedKeyCode = e.KeyCode;
            _lastReleasedScanCode = e.ScanCode;
        }
    }
}