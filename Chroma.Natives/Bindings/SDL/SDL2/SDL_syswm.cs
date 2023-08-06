using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
        public enum SDL_SYSWM_TYPE
        {
            SDL_SYSWM_UNKNOWN,
            SDL_SYSWM_WINDOWS,
            SDL_SYSWM_X11,
            SDL_SYSWM_DIRECTFB,
            SDL_SYSWM_COCOA,
            SDL_SYSWM_UIKIT,
            SDL_SYSWM_WAYLAND,
            SDL_SYSWM_MIR,  
            SDL_SYSWM_WINRT,
            SDL_SYSWM_ANDROID,
            SDL_SYSWM_VIVANTE,
            SDL_SYSWM_OS2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_win
        {
            public IntPtr window;    /* HWND */ 
            public IntPtr hdc;       /* HDC */
            public IntPtr hinstance; /* HINSTANCE */ 
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_winrt
        {
            public IntPtr window; /* IInspectable* */
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_x11
        {
            public IntPtr display;
            public IntPtr window;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_dfb
        {
            public IntPtr dfb;
            public IntPtr window;
            public IntPtr surface;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_cocoa
        {
            public IntPtr window;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_uikit
        {
            public IntPtr window;
            public uint framebuffer;
            public uint colorbuffer;
            public uint resolveFramebuffer;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_wayland
        {
            public IntPtr display;
            public IntPtr surface;
            public IntPtr shell_surface;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_mir
        {
            public IntPtr connection;
            public IntPtr surface;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_android
        {
            public IntPtr window;
            public IntPtr surface;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo_vivante
        {
            public IntPtr display;
            public IntPtr window;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_SysWMinfo
        {
            [FieldOffset(0)]
            public SDL_version version;

            [FieldOffset(4)]
            public SDL_SYSWM_TYPE subsystem;

            [FieldOffset(8)]
            public SDL_SysWMinfo_win win;
            
            [FieldOffset(8)]
            public SDL_SysWMinfo_winrt winrt;
            
            [FieldOffset(8)]
            public SDL_SysWMinfo_x11 x11;

            [FieldOffset(8)]
            public SDL_SysWMinfo_dfb dfb;
            
            [FieldOffset(8)]
            public SDL_SysWMinfo_cocoa cocoa;
            
            [FieldOffset(8)]
            public SDL_SysWMinfo_uikit uikit;
            
            [FieldOffset(8)]
            public SDL_SysWMinfo_wayland wl;
                   
            [FieldOffset(8)]
            public SDL_SysWMinfo_mir mir;
            
            [FieldOffset(8)]
            public SDL_SysWMinfo_android android;

            [FieldOffset(8)]
            public SDL_SysWMinfo_vivante vivante;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SDL_GetWindowWMInfo(IntPtr window, [In, Out] ref SDL_SysWMinfo info);
    }
}