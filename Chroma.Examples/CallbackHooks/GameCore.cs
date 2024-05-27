using System.Numerics;
using Chroma;
using Chroma.Extensibility;
using Chroma.Graphics;
using Chroma.Input;

namespace CallbackHooks
{
    public class GameCore : Game
    {
        private int _interceptionCount;
        private int _nonInterceptedKeypressCount;
        private int _f4pressCount;

        public GameCore()
            : base(new(false, false))
        {
            HookRegistry.AttachAll();
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"F1 interception(s): {_interceptionCount}\n" +
                $"Non-intercepted key events: {_nonInterceptedKeypressCount}\n" +
                $"F4 (or is it? ^^) press count: {_f4pressCount}\n", 
                new Vector2(8, 8)
            );
        }

        protected override void Update(float delta)
        {
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            _nonInterceptedKeypressCount++;

            if (e.KeyCode == KeyCode.F2)
            {
                HookRegistry.Detach(HookPoint.KeyPressed, HookAttachment.Prefix, BeforeKeyPressed);
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                HookRegistry.Attach(HookPoint.KeyPressed, HookAttachment.Prefix, BeforeKeyPressed);
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                _f4pressCount++;
            }
        }

        // Signature of a hook must match (Game game, T e) where T is the hooked callback's argument type.
        // Optionally some parameters may be mutated by providing `ref` parameter modifier.
        [Hook(HookPoint.KeyPressed, HookAttachment.Prefix)]
        private static bool BeforeKeyPressed(Game game, ref KeyEventArgs e)
        {
            var gameInstance = (game as GameCore)!;

            if (e.KeyCode == KeyCode.F1)
            {
                e = e.WithScanCode(ScanCode.F4)
                     .WithKeyCode(KeyCode.F4);
                
                gameInstance._interceptionCount++;
                return true;
            }

            return true;
        }
    }
}