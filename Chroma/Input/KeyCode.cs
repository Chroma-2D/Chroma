﻿namespace Chroma.Input;

using Chroma.Natives.Bindings.SDL;

public enum KeyCode
{
    Return = SDL2.SDL_Keycode.SDLK_RETURN,
    ReturnAlt = SDL2.SDL_Keycode.SDLK_RETURN2,
    Escape = SDL2.SDL_Keycode.SDLK_ESCAPE,
    Backspace = SDL2.SDL_Keycode.SDLK_BACKSPACE,
    Tab = SDL2.SDL_Keycode.SDLK_TAB,
    Space = SDL2.SDL_Keycode.SDLK_SPACE,
    Exclamation = SDL2.SDL_Keycode.SDLK_EXCLAIM,
    DoubleQuote = SDL2.SDL_Keycode.SDLK_QUOTEDBL,
    Hash = SDL2.SDL_Keycode.SDLK_HASH,
    Percent = SDL2.SDL_Keycode.SDLK_PERCENT,
    Dollar = SDL2.SDL_Keycode.SDLK_DOLLAR,
    Ampersand = SDL2.SDL_Keycode.SDLK_AMPERSAND,
    Quote = SDL2.SDL_Keycode.SDLK_QUOTE,
    LeftParenthesis = SDL2.SDL_Keycode.SDLK_LEFTPAREN,
    RightParenthesis = SDL2.SDL_Keycode.SDLK_RIGHTPAREN,
    Asterisk = SDL2.SDL_Keycode.SDLK_ASTERISK,
    Plus = SDL2.SDL_Keycode.SDLK_PLUS,
    Comma = SDL2.SDL_Keycode.SDLK_COMMA,
    Minus = SDL2.SDL_Keycode.SDLK_MINUS,
    Period = SDL2.SDL_Keycode.SDLK_PERIOD,
    Slash = SDL2.SDL_Keycode.SDLK_SLASH,
    Alpha0 = SDL2.SDL_Keycode.SDLK_0,
    Alpha1 = SDL2.SDL_Keycode.SDLK_1,
    Alpha2 = SDL2.SDL_Keycode.SDLK_2,
    Alpha3 = SDL2.SDL_Keycode.SDLK_3,
    Alpha4 = SDL2.SDL_Keycode.SDLK_4,
    Alpha5 = SDL2.SDL_Keycode.SDLK_5,
    Alpha6 = SDL2.SDL_Keycode.SDLK_6,
    Alpha7 = SDL2.SDL_Keycode.SDLK_7,
    Alpha8 = SDL2.SDL_Keycode.SDLK_8,
    Alpha9 = SDL2.SDL_Keycode.SDLK_9,
    Colon = SDL2.SDL_Keycode.SDLK_COLON,
    Semicolon = SDL2.SDL_Keycode.SDLK_SEMICOLON,
    Less = SDL2.SDL_Keycode.SDLK_LESS,
    Equals = SDL2.SDL_Keycode.SDLK_EQUALS,
    Greater = SDL2.SDL_Keycode.SDLK_GREATER,
    Question = SDL2.SDL_Keycode.SDLK_QUESTION,
    At = SDL2.SDL_Keycode.SDLK_AT,
    LeftBracket = SDL2.SDL_Keycode.SDLK_LEFTBRACKET,
    Backslash = SDL2.SDL_Keycode.SDLK_BACKSLASH,
    RightBracket = SDL2.SDL_Keycode.SDLK_RIGHTBRACKET,
    Caret = SDL2.SDL_Keycode.SDLK_CARET,
    Underscore = SDL2.SDL_Keycode.SDLK_UNDERSCORE,
    Grave = SDL2.SDL_Keycode.SDLK_BACKQUOTE,
    Tilde = SDL2.SDL_Keycode.SDLK_BACKQUOTE,
    A = SDL2.SDL_Keycode.SDLK_a,
    B = SDL2.SDL_Keycode.SDLK_b,
    C = SDL2.SDL_Keycode.SDLK_c,
    D = SDL2.SDL_Keycode.SDLK_d,
    E = SDL2.SDL_Keycode.SDLK_e,
    F = SDL2.SDL_Keycode.SDLK_f,
    G = SDL2.SDL_Keycode.SDLK_g,
    H = SDL2.SDL_Keycode.SDLK_h,
    I = SDL2.SDL_Keycode.SDLK_i,
    J = SDL2.SDL_Keycode.SDLK_j,
    K = SDL2.SDL_Keycode.SDLK_k,
    L = SDL2.SDL_Keycode.SDLK_l,
    M = SDL2.SDL_Keycode.SDLK_m,
    N = SDL2.SDL_Keycode.SDLK_n,
    O = SDL2.SDL_Keycode.SDLK_o,
    P = SDL2.SDL_Keycode.SDLK_p,
    Q = SDL2.SDL_Keycode.SDLK_q,
    R = SDL2.SDL_Keycode.SDLK_r,
    S = SDL2.SDL_Keycode.SDLK_s,
    T = SDL2.SDL_Keycode.SDLK_t,
    U = SDL2.SDL_Keycode.SDLK_u,
    V = SDL2.SDL_Keycode.SDLK_v,
    W = SDL2.SDL_Keycode.SDLK_w,
    X = SDL2.SDL_Keycode.SDLK_x,
    Y = SDL2.SDL_Keycode.SDLK_y,
    Z = SDL2.SDL_Keycode.SDLK_z,
    CapsLock = SDL2.SDL_Keycode.SDLK_CAPSLOCK,
    F1 = SDL2.SDL_Keycode.SDLK_F1,
    F2 = SDL2.SDL_Keycode.SDLK_F2,
    F3 = SDL2.SDL_Keycode.SDLK_F3,
    F4 = SDL2.SDL_Keycode.SDLK_F4,
    F5 = SDL2.SDL_Keycode.SDLK_F5,
    F6 = SDL2.SDL_Keycode.SDLK_F6,
    F7 = SDL2.SDL_Keycode.SDLK_F7,
    F8 = SDL2.SDL_Keycode.SDLK_F8,
    F9 = SDL2.SDL_Keycode.SDLK_F9,
    F10 = SDL2.SDL_Keycode.SDLK_F10,
    F11 = SDL2.SDL_Keycode.SDLK_F11,
    F12 = SDL2.SDL_Keycode.SDLK_F12,
    F13 = SDL2.SDL_Keycode.SDLK_F13,
    F14 = SDL2.SDL_Keycode.SDLK_F14,
    F15 = SDL2.SDL_Keycode.SDLK_F15,
    F16 = SDL2.SDL_Keycode.SDLK_F16,
    F17 = SDL2.SDL_Keycode.SDLK_F17,
    F18 = SDL2.SDL_Keycode.SDLK_F18,
    F19 = SDL2.SDL_Keycode.SDLK_F19,
    F20 = SDL2.SDL_Keycode.SDLK_F20,
    F21 = SDL2.SDL_Keycode.SDLK_F21,
    F22 = SDL2.SDL_Keycode.SDLK_F22,
    F23 = SDL2.SDL_Keycode.SDLK_F23,
    F24 = SDL2.SDL_Keycode.SDLK_F24,
    PrintScreen = SDL2.SDL_Keycode.SDLK_PRINTSCREEN,
    ScrollLock = SDL2.SDL_Keycode.SDLK_SCROLLLOCK,
    Pause = SDL2.SDL_Keycode.SDLK_PAUSE,
    Insert = SDL2.SDL_Keycode.SDLK_INSERT,
    Home = SDL2.SDL_Keycode.SDLK_HOME,
    PageUp = SDL2.SDL_Keycode.SDLK_PAGEUP,
    Delete = SDL2.SDL_Keycode.SDLK_DELETE,
    End = SDL2.SDL_Keycode.SDLK_END,
    PageDown = SDL2.SDL_Keycode.SDLK_PAGEDOWN,
    Right = SDL2.SDL_Keycode.SDLK_RIGHT,
    Left = SDL2.SDL_Keycode.SDLK_LEFT,
    Up = SDL2.SDL_Keycode.SDLK_UP,
    Down = SDL2.SDL_Keycode.SDLK_DOWN,
    NumLock = SDL2.SDL_Keycode.SDLK_NUMLOCKCLEAR,
    NumSlash = SDL2.SDL_Keycode.SDLK_KP_DIVIDE,
    NumAsterisk = SDL2.SDL_Keycode.SDLK_KP_MULTIPLY,
    NumMinus = SDL2.SDL_Keycode.SDLK_KP_MINUS,
    NumPlus = SDL2.SDL_Keycode.SDLK_KP_PLUS,
    NumEnter = SDL2.SDL_Keycode.SDLK_KP_ENTER,
    NumEquals = SDL2.SDL_Keycode.SDLK_KP_EQUALS,
    NumEqualsAlt = SDL2.SDL_Keycode.SDLK_KP_EQUALSAS400,
    Numpad0 = SDL2.SDL_Keycode.SDLK_KP_0,
    Numpad1 = SDL2.SDL_Keycode.SDLK_KP_1,
    Numpad2 = SDL2.SDL_Keycode.SDLK_KP_2,
    Numpad3 = SDL2.SDL_Keycode.SDLK_KP_3,
    Numpad4 = SDL2.SDL_Keycode.SDLK_KP_4,
    Numpad5 = SDL2.SDL_Keycode.SDLK_KP_5,
    Numpad6 = SDL2.SDL_Keycode.SDLK_KP_6,
    Numpad7 = SDL2.SDL_Keycode.SDLK_KP_7,
    Numpad8 = SDL2.SDL_Keycode.SDLK_KP_8,
    Numpad9 = SDL2.SDL_Keycode.SDLK_KP_9,
    NumpadPeriod = SDL2.SDL_Keycode.SDLK_KP_PERIOD,
    NumpadComma = SDL2.SDL_Keycode.SDLK_KP_COMMA,
    Application = SDL2.SDL_Keycode.SDLK_APPLICATION,
    Power = SDL2.SDL_Keycode.SDLK_POWER,
    Execute = SDL2.SDL_Keycode.SDLK_EXECUTE,
    Help = SDL2.SDL_Keycode.SDLK_HELP,
    Menu = SDL2.SDL_Keycode.SDLK_MENU,
    Select = SDL2.SDL_Keycode.SDLK_SELECT,
    Stop = SDL2.SDL_Keycode.SDLK_STOP,
    Redo = SDL2.SDL_Keycode.SDLK_AGAIN,
    Undo = SDL2.SDL_Keycode.SDLK_UNDO,
    Cut = SDL2.SDL_Keycode.SDLK_CUT,
    Copy = SDL2.SDL_Keycode.SDLK_COPY,
    Paste = SDL2.SDL_Keycode.SDLK_PASTE,
    Find = SDL2.SDL_Keycode.SDLK_FIND,
    Mute = SDL2.SDL_Keycode.SDLK_MUTE,
    VolumeUp = SDL2.SDL_Keycode.SDLK_VOLUMEUP,
    VolumeDown = SDL2.SDL_Keycode.SDLK_VOLUMEDOWN,
    AltErase = SDL2.SDL_Keycode.SDLK_ALTERASE,
    SysRq = SDL2.SDL_Keycode.SDLK_SYSREQ,
    Cancel = SDL2.SDL_Keycode.SDLK_CANCEL,
    Clear = SDL2.SDL_Keycode.SDLK_CLEAR,
    Prior = SDL2.SDL_Keycode.SDLK_PRIOR,
    Separator = SDL2.SDL_Keycode.SDLK_SEPARATOR,
    Out = SDL2.SDL_Keycode.SDLK_OUT,
    Operator = SDL2.SDL_Keycode.SDLK_OPER,
    ClearAgain = SDL2.SDL_Keycode.SDLK_CLEARAGAIN,
    CrSel = SDL2.SDL_Keycode.SDLK_CRSEL,
    ExSel = SDL2.SDL_Keycode.SDLK_EXSEL,
    LeftControl = SDL2.SDL_Keycode.SDLK_LCTRL,
    LeftShift = SDL2.SDL_Keycode.SDLK_LSHIFT,
    LeftAlt = SDL2.SDL_Keycode.SDLK_LALT,
    LeftSuper = SDL2.SDL_Keycode.SDLK_LGUI,
    RightControl = SDL2.SDL_Keycode.SDLK_RCTRL,
    RightShift = SDL2.SDL_Keycode.SDLK_RSHIFT,
    RightAlt = SDL2.SDL_Keycode.SDLK_RALT,
    RightSuper = SDL2.SDL_Keycode.SDLK_RGUI,
    Mode = SDL2.SDL_Keycode.SDLK_MODE,
    MediaNextTrack = SDL2.SDL_Keycode.SDLK_AUDIONEXT,
    MediaPrevTrack = SDL2.SDL_Keycode.SDLK_AUDIOPREV,
    MediaRewind = SDL2.SDL_Keycode.SDLK_AUDIOREWIND,
    MediaFastForward = SDL2.SDL_Keycode.SDLK_AUDIOFASTFORWARD,
    MediaStop = SDL2.SDL_Keycode.SDLK_AUDIOSTOP,
    MediaPlayPause = SDL2.SDL_Keycode.SDLK_AUDIOPLAY,
    MediaMute = SDL2.SDL_Keycode.SDLK_AUDIOMUTE,
    MediaSelect = SDL2.SDL_Keycode.SDLK_MEDIASELECT,
    ApplicationBrowser = SDL2.SDL_Keycode.SDLK_WWW,
    ApplicationEmail = SDL2.SDL_Keycode.SDLK_MAIL,
    ApplicationCalculator = SDL2.SDL_Keycode.SDLK_CALCULATOR,
    ApplicationFileBrowser = SDL2.SDL_Keycode.SDLK_COMPUTER,
    BrowserSearch = SDL2.SDL_Keycode.SDLK_AC_SEARCH,
    BrowserHome = SDL2.SDL_Keycode.SDLK_AC_HOME,
    BrowserBack = SDL2.SDL_Keycode.SDLK_AC_BACK,
    BrowserForward = SDL2.SDL_Keycode.SDLK_AC_FORWARD,
    BrowserStop = SDL2.SDL_Keycode.SDLK_AC_STOP,
    BrowserRefresh = SDL2.SDL_Keycode.SDLK_AC_REFRESH,
    BrowserBookmarks = SDL2.SDL_Keycode.SDLK_AC_BOOKMARKS,
    BrightnessDown = SDL2.SDL_Keycode.SDLK_BRIGHTNESSDOWN,
    BrightnessUp = SDL2.SDL_Keycode.SDLK_BRIGHTNESSUP,
    DisplaySwitch = SDL2.SDL_Keycode.SDLK_DISPLAYSWITCH,
    Eject = SDL2.SDL_Keycode.SDLK_EJECT,
    Sleep = SDL2.SDL_Keycode.SDLK_SLEEP,
    Application1 = SDL2.SDL_Keycode.SDLK_APP1,
    Application2 = SDL2.SDL_Keycode.SDLK_APP2
}