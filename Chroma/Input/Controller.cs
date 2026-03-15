namespace Chroma.Input;

using System.Collections.Generic;
using Chroma.Diagnostics.Logging;
using Chroma.Input.GameControllers;
using Chroma.Input.GameControllers.Drivers.Capabilities;
using Chroma.Natives.Bindings.SDL;

public static class Controller
{
    private static readonly Log _log = LogManager.GetForCurrentAssembly();

    public static int DeviceCount => ControllerRegistry.Instance.DeviceCount;

    public static ControllerDriver? Get(int playerIndex)
        => ControllerRegistry.Instance.GetControllerDriver(playerIndex);

    public static bool Is<T>(int playerIndex) where T : ControllerDriver
    {
        var driver = Get(playerIndex);
        return driver != null && driver.Is<T>();
    }

    public static T? As<T>(int playerIndex) where T : ControllerDriver
        => Get(playerIndex)?.As<T>();

    public static void SetDeadZone(int playerIndex, ControllerAxis axis, ushort value)
        => Get(playerIndex)?.SetDeadZone(axis, value);

    public static void SetDeadZoneAllAxes(int playerIndex, ushort value)
        => Get(playerIndex)?.SetDeadZoneAllAxes(value);

    public static bool CanIgnoreAxisMotion(int playerIndex, ControllerAxis axis, short axisValue)
        => Get(playerIndex)?.CanIgnoreAxisMotion(axis, axisValue) ?? false;

    public static int GetRawAxisValue(int playerIndex, ControllerAxis axis)
        => Get(playerIndex)?.GetRawAxisValue(axis) ?? 0;

    public static float GetRawAxisValueNormalized(int playerIndex, ControllerAxis axis)
        => Get(playerIndex)?.GetRawAxisValueNormalized(axis) ?? 0;

    public static int GetAxisValue(int playerIndex, ControllerAxis axis)
        => Get(playerIndex)?.GetAxisValue(axis) ?? 0;

    public static float GetAxisValueNormalized(int playerIndex, ControllerAxis axis)
        => Get(playerIndex)?.GetAxisValueNormalized(axis) ?? 0;

    public static bool IsButtonDown(int playerIndex, ControllerButton button)
        => Get(playerIndex)?.IsButtonDown(button) ?? false;

    public static bool IsButtonUp(int playerIndex, ControllerButton button)
        => !IsButtonDown(playerIndex, button);

    public static void Rumble(int playerIndex, ushort leftIntensity, ushort rightIntensity, uint duration)
        => Get(playerIndex)?.Rumble(leftIntensity, rightIntensity, duration);

    public static void AddMapping(string controllerMapping)
    {
        if (string.IsNullOrEmpty(controllerMapping))
        {
            _log.Warning("Tried to add a null or empty controller mapping.");
            return;
        }

        if (SDL2.SDL_GameControllerAddMapping(controllerMapping) < 0)
            _log.Error($"Failed to add a controller mapping: {SDL2.SDL_GetError()}.");
    }

    public static string RetrieveMapping(int playerIndex)
        => Get(playerIndex)?.RetrieveMapping() ?? string.Empty;

    public static IReadOnlySet<ControllerButton>? GetActiveButtons(int playerIndex)
        => Get(playerIndex)?.ActiveButtons;

    internal static void OnConnected(Game game, ControllerEventArgs e)
    {
        game.OnControllerConnected(e);
    }

    internal static void OnDisconnected(Game game, ControllerEventArgs e)
    {
        game.OnControllerDisconnected(e);
    }

    internal static void OnButtonReleased(Game game, ControllerButtonEventArgs e)
    {
        e.Controller.OnButtonReleased(e);
        game.OnControllerButtonReleased(e);
    }

    internal static void OnButtonPressed(Game game, ControllerButtonEventArgs e)
    {
        e.Controller.OnButtonPressed(e);
        game.OnControllerButtonPressed(e);
    }

    internal static void OnAxisMoved(Game game, ControllerAxisEventArgs e)
    {
        e.Controller.OnAxisMoved(e);
        game.OnControllerAxisMoved(e);
    }

    internal static void OnTouchpadMoved(Game game, ControllerTouchpadEventArgs e)
    {
        if (e.Controller is ITouchEnabled touchEnabledDriver)
        {
            touchEnabledDriver.OnTouchpadMoved(e.TouchpadIndex, e.FingerIndex, e.Position.X, e.Position.Y);
            game.OnControllerTouchpadMoved(e);
        }
    }

    internal static void OnTouchpadTouched(Game game, ControllerTouchpadEventArgs e)
    {
        if (e.Controller is ITouchEnabled touchEnabledDriver)
        {
            touchEnabledDriver.OnTouchpadTouched(e.TouchpadIndex, e.FingerIndex, e.Position.X, e.Position.Y);
            game.OnControllerTouchpadTouched(e);
        }
    }

    internal static void OnTouchpadReleased(Game game, ControllerTouchpadEventArgs e)
    {
        if (e.Controller is ITouchEnabled touchEnabledDriver)
        {
            touchEnabledDriver.OnTouchpadReleased(e.TouchpadIndex, e.FingerIndex, e.Position.X, e.Position.Y);
            game.OnControllerTouchpadReleased(e);
        }
    }

    internal static void OnGyroscopeStateChanged(Game game, ControllerSensorEventArgs e)
    {
        if (e.Controller is IGyroscopeEnabled gyroEnabledDriver)
        {
            gyroEnabledDriver.OnGyroscopeStateChanged(e.Values.X, e.Values.Y, e.Values.Z);
            game.OnControllerGyroscopeStateChanged(e);
        }
    }

    internal static void OnAccelerometerStateChanged(Game game, ControllerSensorEventArgs e)
    {
        if (e.Controller is IAccelerometerEnabled acceleroEnabledDriver)
        {
            acceleroEnabledDriver.OnAccelerometerStateChanged(e.Values.X, e.Values.Y, e.Values.Z);
            game.OnControllerAccelerometerStateChanged(e);
        }
    }
}