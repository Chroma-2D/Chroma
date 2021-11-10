using System.Collections.Generic;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;
using Chroma.Windowing;

namespace Chroma.Input
{
    public static class Mouse
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        private static readonly HashSet<MouseButton> _buttonStates = new();

        public static IReadOnlySet<MouseButton> ActiveButtons => _buttonStates;

        public static bool IsRelativeModeEnabled
        {
            get => SDL2.SDL_GetRelativeMouseMode();
            set
            {
                if (SDL2.SDL_SetRelativeMouseMode(value) < 0)
                {
                    _log.Error(
                        $"Failed to {(value ? "enable" : "disable")} relative mouse mode: {SDL2.SDL_GetError()}"
                    );
                }
                
            }
        }

        public static bool IsCaptured
        {
            get => SDL2.SDL_GetWindowMouseGrab(Window.Instance.Handle);

            set
            {
                if (value) Capture();
                else Release();
            }
        }

        public static Vector2 WindowSpacePosition
        {
            get
            {
                _ = SDL2.SDL_GetMouseState(out var x, out var y);
                return new Vector2(x, y);
            }

            set => SetPosition((int)value.X, (int)value.Y);
        }

        public static Vector2 ScreenSpacePosition
        {
            get
            {
                _ = SDL2.SDL_GetGlobalMouseState(out var x, out var y);
                return new Vector2(x, y);
            }

            set => SetPosition((int)value.X, (int)value.Y, true);
        }

        public static Vector2 GetPosition(bool screenSpace = false)
        {
            return screenSpace
                ? ScreenSpacePosition
                : WindowSpacePosition;
        }

        public static void SetPosition(int x, int y, bool screenSpace = false)
        {
            if (screenSpace)
            {
                if (SDL2.SDL_WarpMouseGlobal(x, y) < 0)
                {
                    _log.Error($"Failed to warp mouse to screen-space coordinates ({x}, {y}): {SDL2.SDL_GetError()}");
                }
            }
            else
            {
                SDL2.SDL_WarpMouseInWindow(Window.Instance.Handle, x, y);
            }
        }

        public static bool IsButtonDown(MouseButton button)
        {
            var state = SDL2.SDL_GetMouseState(out _, out _);
            var mask = SDL2.SDL_BUTTON((uint)button);

            return (state & mask) != 0;
        }

        public static bool IsButtonUp(MouseButton button)
            => !IsButtonDown(button);

        public static void Capture()
            => SDL2.SDL_SetWindowMouseGrab(Window.Instance.Handle, true);
        
        public static void Release()
            => SDL2.SDL_SetWindowMouseGrab(Window.Instance.Handle, false);

        internal static void OnButtonPressed(Game game, MouseButtonEventArgs e)
        {
            _buttonStates.Add(e.Button);
            
            game.OnMousePressed(e);
        }

        internal static void OnButtonReleased(Game game, MouseButtonEventArgs e)
        {
            _buttonStates.Remove(e.Button);
            
            game.OnMouseReleased(e);
        }
    }
}