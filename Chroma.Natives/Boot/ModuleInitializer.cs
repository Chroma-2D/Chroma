using System;
using Chroma.Natives.SDL;

namespace Chroma.Natives.Boot
{
    internal static class ModuleInitializer
    {
        private static EmbeddedDllLoader DllLoader { get; } = new EmbeddedDllLoader();

        public static void Initialize()
        {
            Console.WriteLine("Chroma.Natives initializing...");

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Console.WriteLine(" => Running on Windows. Will extract and load required natives...");
                DllLoader.InitializeNativeDlls();
            }
            else
            {
                Console.WriteLine(" => Non-windows platform. Make sure the following packages are available:");
                Console.WriteLine("    SDL2, SDL2_image, SDL2_mixer, freetype, SDL2_gpu.\n");
                Console.WriteLine("    If any of those are missing, the engine will fail right about... now.");
            }

            Console.WriteLine("---");
            Console.WriteLine("Initializing SDL2 core...");
            SDL2.SDL_Init(SDL2.SDL_INIT_EVERYTHING);
        }
    }
}
