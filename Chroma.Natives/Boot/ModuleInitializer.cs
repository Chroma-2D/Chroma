namespace Chroma.Natives.Boot;

using System;
using System.Collections.Generic;
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

internal static class ModuleInitializer
{
    private static Dictionary<string, string> UserBootHints { get; set; }
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

        var bootHintsPath = Path.Combine(
            AppContext.BaseDirectory,
            "boot.hints.json"
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

        if (File.Exists(bootHintsPath))
        {
            UserBootHints = JsonSerializer.Deserialize<Dictionary<string, string>>(
                File.ReadAllText(bootHintsPath)
            );
        }
    }

    private static void SetSdlHints()
    {
        if (BootConfig.SdlInitializationHints != null)
        {
            foreach (var (name, value) in BootConfig.SdlInitializationHints) 
                TrySetHint(name, value);
        }

        if (UserBootHints != null)
        {
            foreach (var (name, value) in UserBootHints)
                TrySetHint(name, value);
        }
    }

    private static void TrySetHint(string name, string value)
    {
        if (!SDL2.SDL_SetHint(name, value))
        {
            EarlyLog.Error($"Failed to set hint '{name}' to '{value}': {SDL2.SDL_GetError()}");
        }
        else
        {
            EarlyLog.Debug($"SDL hint '{name}': {value}");
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