using System;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.MemoryManagement;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Input
{
    public class Cursor : DisposableResource
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        
        private static bool _isVisible = true;
        private IntPtr _cursorHandle;
        private IntPtr _sdlTextureHandle;

        public static bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (SDL2.SDL_ShowCursor(value ? 1 : 0) < 0)
                {
                    _log.Error($"Failed to {(value ? "show" : "hide")} the cursor: {SDL2.SDL_GetError()}");
                    return;
                }
                
                _isVisible = value;
            }
        }

        public Texture Texture { get; }
        public Vector2 HotSpot { get; }

        public bool IsCurrent => SDL2.SDL_GetCursor() == _cursorHandle;

        public Cursor(string imageFilePath, Vector2 hotSpot)
        {
            Texture = new Texture(imageFilePath);
            HotSpot = hotSpot;

            Initialize();
        }

        public Cursor(Texture texture, Vector2 hotSpot)
        {
            if (texture.Disposed)
                throw new ArgumentException("The texture provided was previously disposed.");

            Texture = new Texture(texture);
            HotSpot = hotSpot;

            Initialize();
        }

        public void SetCurrent()
        {
            EnsureNotDisposed();
            SDL2.SDL_SetCursor(_cursorHandle);
        }

        public static void Reset()
            => SDL2.SDL_SetCursor(SDL2.SDL_GetDefaultCursor());

        protected override void FreeNativeResources()
        {
            if (_cursorHandle != IntPtr.Zero)
                SDL2.SDL_FreeCursor(_cursorHandle);

            if (_sdlTextureHandle != IntPtr.Zero)
                SDL2.SDL_FreeSurface(_sdlTextureHandle);
        }

        protected override void FreeManagedResources()
            => Texture.Dispose();

        private void Initialize()
        {
            _sdlTextureHandle = Texture.AsSdlSurface();
            
            _cursorHandle = SDL2.SDL_CreateColorCursor(
                _sdlTextureHandle,
                (int)HotSpot.X,
                (int)HotSpot.Y
            );

            if (_cursorHandle == IntPtr.Zero)
                _log.Error($"Failed to load the cursor: {SDL2.SDL_GetError()}");
        }
    }
}