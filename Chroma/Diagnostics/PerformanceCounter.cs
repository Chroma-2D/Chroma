using Chroma.Natives.SDL;

namespace Chroma.Diagnostics
{
    public class PerformanceCounter
    {
        private uint _lastTime;
        private ulong _framesThisCycle;
        
        private ulong _lastFrameTime;
        private ulong _nowFrameTime = SDL2.SDL_GetPerformanceCounter();

        internal float Delta { get; set; }
        
        internal static float SumOfDeltaTimes { get; set; }

        public static float FPS { get; private set; }
        public static ulong LifetimeFrames { get; private set; }

        internal PerformanceCounter()
        {
            _lastTime = SDL2.SDL_GetTicks();
        }

        internal void Update()
        {
            _lastFrameTime = _nowFrameTime;
            _nowFrameTime = SDL2.SDL_GetPerformanceCounter();
            Delta = (_nowFrameTime - _lastFrameTime) / (float)SDL2.SDL_GetPerformanceFrequency();
            SumOfDeltaTimes += Delta;
            
            var currentTime = SDL2.SDL_GetTicks();

            if (currentTime - _lastTime > 1000)
            {
                FPS = _framesThisCycle;

                _lastTime = currentTime;
                _framesThisCycle = 0;
            }

            _framesThisCycle++;
            LifetimeFrames++;
        }
    }
}