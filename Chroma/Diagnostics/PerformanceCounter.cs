using Chroma.Natives.Bindings.SDL;

namespace Chroma.Diagnostics
{
    public sealed class PerformanceCounter
    {
        private ulong _lastFpsUpdateTime = SDL2.SDL_GetTicks64();
        private ulong _lastLagMeasureTime = SDL2.SDL_GetTicks64();

        private ulong _framesThisCycle;

        private ulong _lastFrameTime;
        private ulong _nowFrameTime = SDL2.SDL_GetPerformanceCounter();

        internal static float Lag { get; set; }
        internal static double SumOfDeltaTimes { get; set; }

        public static float Delta { get; private set; }
        public static float FixedDelta { get; internal set; }
        public static float FPS { get; private set; }
        public static ulong LifetimeFrames { get; private set; }

        internal void Update()
        {
            var currentTime = SDL2.SDL_GetTicks64();

            _lastFrameTime = _nowFrameTime;
            _nowFrameTime = SDL2.SDL_GetPerformanceCounter();

            Delta = (_nowFrameTime - _lastFrameTime) / (float)SDL2.SDL_GetPerformanceFrequency();

            SumOfDeltaTimes += Delta;

            if (currentTime - _lastFpsUpdateTime >= 1000)
            {
                FPS = _framesThisCycle;

                _lastFpsUpdateTime = currentTime;
                _framesThisCycle = 0;
            }

            _framesThisCycle++;
            LifetimeFrames++;
        }

        internal void FixedUpdate()
        {
            var currentTime = SDL2.SDL_GetTicks64();

            Lag += (currentTime - _lastLagMeasureTime) / 1000f;
            _lastLagMeasureTime = currentTime;
        }
    }
}