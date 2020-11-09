using Chroma.Natives.SDL;

namespace Chroma.Diagnostics
{
    internal class FpsCounter
    {
        private uint _lastTime;
        private ulong _totalFrames;

        internal static float TotalShaderTime;
        
        public float FPS { get; private set; }
        public ulong LifetimeFrames { get; private set; }

        internal FpsCounter()
        {
            _lastTime = SDL2.SDL_GetTicks();
        }

        internal void Update()
        {
            var currentTime = SDL2.SDL_GetTicks();

            if (currentTime - _lastTime > 1000)
            {
                FPS = _totalFrames;

                _lastTime = currentTime;
                _totalFrames = 0;
            }

            _totalFrames++;
            LifetimeFrames++;
        }
    }
}