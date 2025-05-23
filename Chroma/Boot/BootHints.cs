namespace Chroma.Boot;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class BootHints
{
    private const string BootHintsFileName = "boot.hints.json";
    
    private static Dictionary<string, string> _userBootHints = new();

    public static bool Writeable { get; private set; } = true;

    static BootHints()
    {
    }

    public static string Get(string name)
    {
        if (_userBootHints.TryGetValue(name, out var value))
            return value;

        return string.Empty;
    }

    public static void Set(string name, string value)
    {
        EnsureWriteable();

        _userBootHints[name] = value;
        Save();
    }

    public static bool IsSet(string name)
        => _userBootHints.ContainsKey(name);

    public static bool Unset(string name)
    {
        EnsureWriteable();

        if (_userBootHints.Remove(name))
        {
            Save();
            return true;
        }

        return false;
    }

    internal static void Lock()
        => Writeable = false;

    private static void Load()
    {
        var hintsPath = Path.Combine(
            AppContext.BaseDirectory,
            BootHintsFileName
        );

        try
        {
            _userBootHints = JsonSerializer.Deserialize<Dictionary<string, string>>(
                File.ReadAllText(hintsPath)
            ) ?? new Dictionary<string, string>();
        }
        catch
        {
            /* Ignore. */
        }
    }
    
    private static void Save()
    {
        var hintsPath = Path.Combine(
            AppContext.BaseDirectory,
            BootHintsFileName
        );

        try
        {
            File.WriteAllText(
                hintsPath,
                JsonSerializer.Serialize(
                    _userBootHints, 
                    new JsonSerializerOptions { WriteIndented = true }
                )
            );
        }
        catch
        {
            /* Ignore. */
        }
    }

    private static void EnsureWriteable()
    {
        if (!Writeable)
        {
            throw new InvalidOperationException(
                "Boot hints can only be modified before constructing the Game class."
            );
        }
    }
}