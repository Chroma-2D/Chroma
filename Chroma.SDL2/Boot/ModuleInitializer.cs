using System;

namespace Chroma.SDL2.Boot
{
    internal static class ModuleInitializer
    {
        private static EmbeddedDllLoader DllLoader { get; } = new EmbeddedDllLoader();

        public static void Initialize()
        {
            Console.WriteLine("Chroma.SDL2 bindings initializing...");

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Console.WriteLine(" => Running on Windows. Will extract and load required natives...");
                DllLoader.InitializeNativeDlls();
            }
            else
            {
                Console.WriteLine(" => Non-windows platform. Make sure the following libraries are available:");
                Console.WriteLine("    SDL2, SDL2_image, SDL2_mixer, SDL2_ttf, SDL2_gpu.\n");
                Console.WriteLine("    If any of those are missing, the engine will fail right about... now.");
            }

            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            SDL_ttf.TTF_Init();
        }
    }
}
