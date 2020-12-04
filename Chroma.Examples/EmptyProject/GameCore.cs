using Chroma;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;

namespace EmptyProject
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        internal GameCore()
        {
            Log.Info("Hello, world!");

            GraphicsManager.LimitFramerate = false;
            Graphics.VerticalSyncMode = VerticalSyncMode.None;
            TimeStepTarget = 150;
        }

        protected override void Update(float delta)
        {
            Window.Title = $"{PerformanceCounter.FPS:F2} | {PerformanceCounter.Delta:F6}";
            base.Update(delta);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                UseFixedTimeStep = !UseFixedTimeStep;
            }
        }
    }
}