using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Chroma.NALO;
using Chroma.Natives.Bindings.SDL;
using Chroma.Natives.Boot.Config;
using Chroma.Natives.Syscalls;

namespace Chroma.Natives.Boot
{
    internal static class ModuleInitializer
    {
        internal static BootConfig BootConfig { get; private set; }

        [ModuleInitializer]
        internal static void Initialize()
        {
            if (!Environment.Is64BitOperatingSystem)
                throw new PlatformNotSupportedException("Chroma supports 64-bit systems only.");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                SetupConsoleMode();

            {
                EarlyLog.Info($"It is {DateTime.Now.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture)}.");
                EarlyLog.Info("Please wait. I'm trying to boot...");
                
                ReadBootConfig();

                try
                {
                    if (BootConfig.SkipChecksumVerification)
                        EarlyLog.Warning("Checksum verification disabled. Living on the edge, huh?");

                    NativeLoader.LoadNatives(BootConfig.SkipChecksumVerification);
                }
                catch (NativeLoaderException nle)
                {
                    EarlyLog.Error($"{nle.Message}. Inner exception: {nle.InnerException}");
                    Console.WriteLine("Press any key to terminate...");
                    Console.ReadKey();

                    Environment.Exit(1);
                }

                if (BootConfig.HookSdlLog)
                {
                    SdlBootTimeHook.Hook();
                }

                SetSdlHints();
                InitializeSdlSystems();
            }

            SdlBootTimeHook.UnHook();
        }

        private static void SetupConsoleMode()
        {
            var stdHandle = Windows.GetStdHandle(Windows.STD_OUTPUT_HANDLE);

            Windows.GetConsoleMode(stdHandle, out var consoleMode);
            consoleMode |= Windows.ENABLE_PROCESSED_OUTPUT;
            consoleMode |= Windows.ENABLE_VIRTUAL_TERMINAL_PROCESSING;

            Windows.SetConsoleMode(stdHandle, consoleMode);
        }

        private static void ReadBootConfig()
        {
            var bootConfigPath = Path.Combine(
                AppContext.BaseDirectory,
                "boot.json"
            );

            try
            {
                using var sr = new StreamReader(bootConfigPath);
                BootConfig = JsonSerializer.Deserialize<BootConfig>(sr.ReadToEnd());
            }
            catch (Exception e)
            {
                EarlyLog.Warning($"No boot.json or it was invalid ({e.Message}) defaults created.");

                BootConfig = new BootConfig();

                using var sw = new StreamWriter(bootConfigPath);
                sw.WriteLine(
                    JsonSerializer.Serialize(BootConfig, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        IgnoreReadOnlyProperties = false
                    })
                );
            }
        }

        private static void SetSdlHints()
        {
            if (BootConfig.SdlInitializationHints != null)
            {
                foreach (var kvp in BootConfig.SdlInitializationHints)
                {
                    if (!SDL2.SDL_SetHint(kvp.Key, kvp.Value))
                    {
                        EarlyLog.Error($"Failed to set '{kvp.Key}' to '{kvp.Value}': {SDL2.SDL_GetError()}");
                    }
                }
            }
        }

        private static void InitializeSdlSystems()
        {
            SDL2.SDL_GetVersion(out var sdlVersion);
            EarlyLog.Info($"Initializing SDL {sdlVersion.major}.{sdlVersion.minor}.{sdlVersion.patch}");

            SDL2.SDL_Init(BootConfig.SdlModules.SdlInitFlags);
            if (BootConfig.EnableSdlGpuDebugging)
            {
                EarlyLog.Info("Enabling SDL_gpu debugging...");
                SDL_gpu.GPU_SetDebugLevel(SDL_gpu.GPU_DebugLevelEnum.GPU_DEBUG_LEVEL_MAX);
            }
            
            EarlyLog.Info("Handing over to the core. " +
                          $"Its log will be located somewhere in {AppContext.BaseDirectory}Logs...");
            Console.WriteLine("---");
        }
    }
}