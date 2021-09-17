﻿using static Chroma.Natives.SDL.SDL2;

namespace Chroma.Input
{
    public enum KeyCode
    {
        Return = SDL_Keycode.SDLK_RETURN,
        ReturnAlt = SDL_Keycode.SDLK_RETURN2,
        Escape = SDL_Keycode.SDLK_ESCAPE,
        Backspace = SDL_Keycode.SDLK_BACKSPACE,
        Tab = SDL_Keycode.SDLK_TAB,
        Space = SDL_Keycode.SDLK_SPACE,
        Exclamation = SDL_Keycode.SDLK_EXCLAIM,
        DoubleQuote = SDL_Keycode.SDLK_QUOTEDBL,
        Hash = SDL_Keycode.SDLK_HASH,
        Percent = SDL_Keycode.SDLK_PERCENT,
        Dollar = SDL_Keycode.SDLK_DOLLAR,
        Ampersand = SDL_Keycode.SDLK_AMPERSAND,
        Quote = SDL_Keycode.SDLK_QUOTE,
        LeftParenthesis = SDL_Keycode.SDLK_LEFTPAREN,
        RightParenthesis = SDL_Keycode.SDLK_RIGHTPAREN,
        Asterisk = SDL_Keycode.SDLK_ASTERISK,
        Plus = SDL_Keycode.SDLK_PLUS,
        Comma = SDL_Keycode.SDLK_COMMA,
        Minus = SDL_Keycode.SDLK_MINUS,
        Period = SDL_Keycode.SDLK_PERIOD,
        Slash = SDL_Keycode.SDLK_SLASH,
        Alpha0 = SDL_Keycode.SDLK_0,
        Alpha1 = SDL_Keycode.SDLK_1,
        Alpha2 = SDL_Keycode.SDLK_2,
        Alpha3 = SDL_Keycode.SDLK_3,
        Alpha4 = SDL_Keycode.SDLK_4,
        Alpha5 = SDL_Keycode.SDLK_5,
        Alpha6 = SDL_Keycode.SDLK_6,
        Alpha7 = SDL_Keycode.SDLK_7,
        Alpha8 = SDL_Keycode.SDLK_8,
        Alpha9 = SDL_Keycode.SDLK_9,
        Colon = SDL_Keycode.SDLK_COLON,
        Semicolon = SDL_Keycode.SDLK_SEMICOLON,
        Less = SDL_Keycode.SDLK_LESS,
        Equals = SDL_Keycode.SDLK_EQUALS,
        Greater = SDL_Keycode.SDLK_GREATER,
        Question = SDL_Keycode.SDLK_QUESTION,
        At = SDL_Keycode.SDLK_AT,
        LeftBracket = SDL_Keycode.SDLK_LEFTBRACKET,
        Backslash = SDL_Keycode.SDLK_BACKSLASH,
        RightBracket = SDL_Keycode.SDLK_RIGHTBRACKET,
        Caret = SDL_Keycode.SDLK_CARET,
        Underscore = SDL_Keycode.SDLK_UNDERSCORE,
        Grave = SDL_Keycode.SDLK_BACKQUOTE,
        Tilde = SDL_Keycode.SDLK_BACKQUOTE,
        A = SDL_Keycode.SDLK_a,
        B = SDL_Keycode.SDLK_b,
        C = SDL_Keycode.SDLK_c,
        D = SDL_Keycode.SDLK_d,
        E = SDL_Keycode.SDLK_e,
        F = SDL_Keycode.SDLK_f,
        G = SDL_Keycode.SDLK_g,
        H = SDL_Keycode.SDLK_h,
        I = SDL_Keycode.SDLK_i,
        J = SDL_Keycode.SDLK_j,
        K = SDL_Keycode.SDLK_k,
        L = SDL_Keycode.SDLK_l,
        M = SDL_Keycode.SDLK_m,
        N = SDL_Keycode.SDLK_n,
        O = SDL_Keycode.SDLK_o,
        P = SDL_Keycode.SDLK_p,
        Q = SDL_Keycode.SDLK_q,
        R = SDL_Keycode.SDLK_r,
        S = SDL_Keycode.SDLK_s,
        T = SDL_Keycode.SDLK_t,
        U = SDL_Keycode.SDLK_u,
        V = SDL_Keycode.SDLK_v,
        W = SDL_Keycode.SDLK_w,
        X = SDL_Keycode.SDLK_x,
        Y = SDL_Keycode.SDLK_y,
        Z = SDL_Keycode.SDLK_z,
        CapsLock = SDL_Keycode.SDLK_CAPSLOCK,
        F1 = SDL_Keycode.SDLK_F1,
        F2 = SDL_Keycode.SDLK_F2,
        F3 = SDL_Keycode.SDLK_F3,
        F4 = SDL_Keycode.SDLK_F4,
        F5 = SDL_Keycode.SDLK_F5,
        F6 = SDL_Keycode.SDLK_F6,
        F7 = SDL_Keycode.SDLK_F7,
        F8 = SDL_Keycode.SDLK_F8,
        F9 = SDL_Keycode.SDLK_F9,
        F10 = SDL_Keycode.SDLK_F10,
        F11 = SDL_Keycode.SDLK_F11,
        F12 = SDL_Keycode.SDLK_F12,
        F13 = SDL_Keycode.SDLK_F13,
        F14 = SDL_Keycode.SDLK_F14,
        F15 = SDL_Keycode.SDLK_F15,
        F16 = SDL_Keycode.SDLK_F16,
        F17 = SDL_Keycode.SDLK_F17,
        F18 = SDL_Keycode.SDLK_F18,
        F19 = SDL_Keycode.SDLK_F19,
        F20 = SDL_Keycode.SDLK_F20,
        F21 = SDL_Keycode.SDLK_F21,
        F22 = SDL_Keycode.SDLK_F22,
        F23 = SDL_Keycode.SDLK_F23,
        F24 = SDL_Keycode.SDLK_F24,
        PrintScreen = SDL_Keycode.SDLK_PRINTSCREEN,
        ScrollLock = SDL_Keycode.SDLK_SCROLLLOCK,
        Pause = SDL_Keycode.SDLK_PAUSE,
        Insert = SDL_Keycode.SDLK_INSERT,
        Home = SDL_Keycode.SDLK_HOME,
        PageUp = SDL_Keycode.SDLK_PAGEUP,
        Delete = SDL_Keycode.SDLK_DELETE,
        End = SDL_Keycode.SDLK_END,
        PageDown = SDL_Keycode.SDLK_PAGEDOWN,
        Right = SDL_Keycode.SDLK_RIGHT,
        Left = SDL_Keycode.SDLK_LEFT,
        Up = SDL_Keycode.SDLK_UP,
        Down = SDL_Keycode.SDLK_DOWN,
        NumLock = SDL_Keycode.SDLK_NUMLOCKCLEAR,
        NumSlash = SDL_Keycode.SDLK_KP_DIVIDE,
        NumAsterisk = SDL_Keycode.SDLK_KP_MULTIPLY,
        NumMinus = SDL_Keycode.SDLK_KP_MINUS,
        NumPlus = SDL_Keycode.SDLK_KP_PLUS,
        NumEnter = SDL_Keycode.SDLK_KP_ENTER,
        NumEquals = SDL_Keycode.SDLK_KP_EQUALS,
        NumEqualsAlt = SDL_Keycode.SDLK_KP_EQUALSAS400,
        Numpad0 = SDL_Keycode.SDLK_KP_0,
        Numpad1 = SDL_Keycode.SDLK_KP_1,
        Numpad2 = SDL_Keycode.SDLK_KP_2,
        Numpad3 = SDL_Keycode.SDLK_KP_3,
        Numpad4 = SDL_Keycode.SDLK_KP_4,
        Numpad5 = SDL_Keycode.SDLK_KP_5,
        Numpad6 = SDL_Keycode.SDLK_KP_6,
        Numpad7 = SDL_Keycode.SDLK_KP_7,
        Numpad8 = SDL_Keycode.SDLK_KP_8,
        Numpad9 = SDL_Keycode.SDLK_KP_9,
        NumpadPeriod = SDL_Keycode.SDLK_KP_PERIOD,
        NumpadComma = SDL_Keycode.SDLK_KP_COMMA,
        Application = SDL_Keycode.SDLK_APPLICATION,
        Power = SDL_Keycode.SDLK_POWER,
        Execute = SDL_Keycode.SDLK_EXECUTE,
        Help = SDL_Keycode.SDLK_HELP,
        Menu = SDL_Keycode.SDLK_MENU,
        Select = SDL_Keycode.SDLK_SELECT,
        Stop = SDL_Keycode.SDLK_STOP,
        Redo = SDL_Keycode.SDLK_AGAIN,
        Undo = SDL_Keycode.SDLK_UNDO,
        Cut = SDL_Keycode.SDLK_CUT,
        Copy = SDL_Keycode.SDLK_COPY,
        Paste = SDL_Keycode.SDLK_PASTE,
        Find = SDL_Keycode.SDLK_FIND,
        Mute = SDL_Keycode.SDLK_MUTE,
        VolumeUp = SDL_Keycode.SDLK_VOLUMEUP,
        VolumeDown = SDL_Keycode.SDLK_VOLUMEDOWN,
        AltErase = SDL_Keycode.SDLK_ALTERASE,
        SysRq = SDL_Keycode.SDLK_SYSREQ,
        Cancel = SDL_Keycode.SDLK_CANCEL,
        Clear = SDL_Keycode.SDLK_CLEAR,
        Prior = SDL_Keycode.SDLK_PRIOR,
        Separator = SDL_Keycode.SDLK_SEPARATOR,
        Out = SDL_Keycode.SDLK_OUT,
        Operator = SDL_Keycode.SDLK_OPER,
        ClearAgain = SDL_Keycode.SDLK_CLEARAGAIN,
        CrSel = SDL_Keycode.SDLK_CRSEL,
        ExSel = SDL_Keycode.SDLK_EXSEL,
        LeftControl = SDL_Keycode.SDLK_LCTRL,
        LeftShift = SDL_Keycode.SDLK_LSHIFT,
        LeftAlt = SDL_Keycode.SDLK_LALT,
        LeftSuper = SDL_Keycode.SDLK_LGUI,
        RightControl = SDL_Keycode.SDLK_RCTRL,
        RightShift = SDL_Keycode.SDLK_RSHIFT,
        RightAlt = SDL_Keycode.SDLK_RALT,
        RightSuper = SDL_Keycode.SDLK_RGUI,
        Mode = SDL_Keycode.SDLK_MODE,
        MediaNextTrack = SDL_Keycode.SDLK_AUDIONEXT,
        MediaPrevTrack = SDL_Keycode.SDLK_AUDIOPREV,
        MediaRewind = SDL_Keycode.SDLK_AUDIOREWIND,
        MediaFastForward = SDL_Keycode.SDLK_AUDIOFASTFORWARD,
        MediaStop = SDL_Keycode.SDLK_AUDIOSTOP,
        MediaPlayPause = SDL_Keycode.SDLK_AUDIOPLAY,
        MediaMute = SDL_Keycode.SDLK_AUDIOMUTE,
        MediaSelect = SDL_Keycode.SDLK_MEDIASELECT,
        ApplicationBrowser = SDL_Keycode.SDLK_WWW,
        ApplicationEmail = SDL_Keycode.SDLK_MAIL,
        ApplicationCalculator = SDL_Keycode.SDLK_CALCULATOR,
        ApplicationFileBrowser = SDL_Keycode.SDLK_COMPUTER,
        BrowserSearch = SDL_Keycode.SDLK_AC_SEARCH,
        BrowserHome = SDL_Keycode.SDLK_AC_HOME,
        BrowserBack = SDL_Keycode.SDLK_AC_BACK,
        BrowserForward = SDL_Keycode.SDLK_AC_FORWARD,
        BrowserStop = SDL_Keycode.SDLK_AC_STOP,
        BrowserRefresh = SDL_Keycode.SDLK_AC_REFRESH,
        BrowserBookmarks = SDL_Keycode.SDLK_AC_BOOKMARKS,
        BrightnessDown = SDL_Keycode.SDLK_BRIGHTNESSDOWN,
        BrightnessUp = SDL_Keycode.SDLK_BRIGHTNESSUP,
        DisplaySwitch = SDL_Keycode.SDLK_DISPLAYSWITCH,
        Eject = SDL_Keycode.SDLK_EJECT,
        Sleep = SDL_Keycode.SDLK_SLEEP,
        Application1 = SDL_Keycode.SDLK_APP1,
        Application2 = SDL_Keycode.SDLK_APP2
    }
}