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

        public GameCore()
            : base(new(false, false))
        {
            HookRegistry.AttachAll();
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"F1 interception(s): {_interceptionCount}\n" +
                $"Non-intercepted key events: {_nonInterceptedKeypressCount}", 
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
        }

        // Signature of a hook must match (Game game, T e) where T is the hooked callback's argument type.
        [Hook(HookPoint.KeyPressed, HookAttachment.Prefix)]
        private static bool BeforeKeyPressed(Game game, KeyEventArgs e)
        {
            var gameInstance = game as GameCore;

            if (e.KeyCode == KeyCode.F1)
            {
                gameInstance._interceptionCount++;
                return false;
            }

            return true;
        }
    }
}