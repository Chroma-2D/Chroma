using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL
{
    internal static partial class SDL2
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Point
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Rect
        {
            public int x;
            public int y;
            public int w;
            public int h;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_FPoint
        {
            public float x;
            public float y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_FRect
        {
            public float x;
            public float y;
            public float w;
            public float h;
        }
    }
}