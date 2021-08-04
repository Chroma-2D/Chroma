using System;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;

namespace FixedTimeStep
{
    public class GameCore : Game
    {
        private Action _currentLagInducer;
        
        public GameCore() : base(new(false, false))
        {
        }
        
        protected override void Update(float delta)
        {
            _currentLagInducer?.Invoke();
            Console.WriteLine($"Update: {delta}");
        }

        protected override void FixedUpdate(float delta)
        {
            Console.WriteLine($"FixedUpdate: {delta}");
        }

        protected override void Draw(RenderContext context)
        {
            Console.WriteLine("Draw");
            
            context.DrawString(
                "Press <F1> to switch to mega lag inducer.\n"+
                "Press <F2> to switch to small lag inducer.\n"+
                "Press <F3> to disable lag inducing.\n" +
                "Press <F4> to change fixed time step target to 30 FPS.\n" +
                "Press <F5> to reset fixed time step back to 75 FPS.\n\n"+
                $"Current fixed time step target: {FixedTimeStepTarget} FPS.\n" +
                "Check console for change in behavior.",
                new Vector2(16)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                _currentLagInducer = MegaLagInducer;
            }
            else if(e.KeyCode == KeyCode.F2)
            {
                _currentLagInducer = SmallLagInducer;
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                _currentLagInducer = null;
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                FixedTimeStepTarget = 30;
            }
            else if (e.KeyCode == KeyCode.F5)
            {
                FixedTimeStepTarget = 75;
            }
        }

        private void MegaLagInducer()
        {
            int x = 0;
            for (var i = 0; i < 1000000000; i++)
            {
                x++;
            }
        }

        private void SmallLagInducer()
        {
            int x = 0;
            for (var i = 0; i < 1000000; i++)
            {
                x++;
            }
        }
    }
}