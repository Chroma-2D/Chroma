using Chroma.SDL2;

namespace Chroma.Diagnostics
{
    internal class FpsCounter
    {
        private readonly uint[] _frameTimes;

        private uint _lastTickValue;
        private uint _totalFrames;

        public float FPS { get; private set; }
        public uint Precision { get; set; } = 60;

        internal FpsCounter()
        {
            _frameTimes = new uint[Precision];
            _lastTickValue = SDL.SDL_GetTicks();
        }

        internal void Update()
        {
            var frameTimeIndex = _totalFrames % Precision;
            var ticks = SDL.SDL_GetTicks();

            _frameTimes[frameTimeIndex] = ticks - _lastTickValue;
            _lastTickValue = ticks;
            _totalFrames++;

            var count = _totalFrames < Precision ? _totalFrames : Precision;

            for (var i = 0; i < count; i++)
                FPS += _frameTimes[i];

            FPS /= count;
            FPS = 1000f / FPS;
        }
    }
}
