namespace Chroma.Input.GameControllers;

using System;
using System.Collections.Generic;

internal sealed class ControllerRegistry
{
    internal const int MaxSupportedPlayers = 24;

    private static readonly Lazy<ControllerRegistry> _lazyInitializer = new(
        () => new ControllerRegistry()
    );

    private readonly Dictionary<IntPtr, ControllerDriver> _controllers;
    private readonly Dictionary<int, IntPtr> _playerMappings;

    internal int DeviceCount => _controllers.Count;

    public static ControllerRegistry Instance => _lazyInitializer.Value;

    private ControllerRegistry()
    {
        _controllers = new Dictionary<IntPtr, ControllerDriver>();
        _playerMappings = new Dictionary<int, IntPtr>(MaxSupportedPlayers);

        for (var i = 0; i < MaxSupportedPlayers; i++)
            _playerMappings.Add(i, IntPtr.Zero);
    }

    public void Register(IntPtr instance, ControllerDriver controller)
    {
        if (_controllers.ContainsKey(instance))
            throw new InvalidOperationException("Duplicate controller instance pointer.");

        if (_playerMappings[controller.Info.PlayerIndex] != IntPtr.Zero)
            throw new InvalidOperationException("Duplicate controller player index.");

        _playerMappings[controller.Info.PlayerIndex] = instance;
        _controllers.Add(instance, controller);
    }

    public void Unregister(IntPtr instance)
    {
        if (!_controllers.ContainsKey(instance))
            throw new InvalidOperationException($"Controller with instance ID {instance} does not exist.");

        var playerIndex = _controllers[instance].Info.PlayerIndex;
        _playerMappings[playerIndex] = IntPtr.Zero;

        _controllers.Remove(instance);
    }

    public int FindFirstFreePlayerSlot()
    {
        for (var i = 0; i < _playerMappings.Count; i++)
            if (_playerMappings[i] == IntPtr.Zero)
                return i;

        return -1;
    }

    public ControllerDriver GetControllerDriver(int playerIndex)
    {
        if (!_playerMappings.ContainsKey(playerIndex) || _playerMappings[playerIndex] == IntPtr.Zero)
            return null;

        var instancePointer = _playerMappings[playerIndex];
        return _controllers[instancePointer];
    }

    internal ControllerDriver GetControllerDriverByPointer(IntPtr instancePointer)
    {
        if (!_controllers.ContainsKey(instancePointer))
            return null;

        return _controllers[instancePointer];
    }
}