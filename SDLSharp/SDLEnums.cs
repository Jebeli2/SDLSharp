namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Flags]
    public enum BlendMode
    {
        None = 0x00000000,
        Blend = 0x00000001,
        Add = 0x00000002,
        Mod = 0x00000004,
        Mul = 0x00000008,
        Invalid = 0x7FFFFFFF
    }

    public enum TextureFilter
    {
        Linear,
        Nearest,
        Best
    }


    [Flags]
    public enum FontStyle
    {
        Normal = 0x00,
        Bold = 0x01,
        Italic = 0x02,
        Underline = 0x04,
        Strikethrough = 0x08
    }

    public enum FontHinting
    {
        Normal = 0,
        Light = 1,
        Mono = 2,
        None = 3,
        LightSubPixel = 4
    }

    public enum MusicFinishReason
    {
        Finished,
        Interrupted
    }
    public enum HorizontalAlignment
    {
        Left,
        Right,
        Center,
        Stretch
    }

    public enum VerticalAlignment
    {
        Top,
        Bottom,
        Center,
        Stretch
    }

    public enum DisplayMode
    {
        Windowed,
        Desktop,
        FullSize,
        MultiMonitor
    }

    public enum MouseButton
    {
        None = 0,
        Left = 1,
        Middle = 2,
        Right = 3,
        X1 = 4,
        X2 = 5
    }

    public enum KeyButtonState
    {
        Invalid = -1,
        Released = 0,
        Pressed = 1
    }

    public enum MouseWheelDirection
    {
        Normal = 0,
        Flipped = 1
    }

    public enum LogPriority
    {
        None = 0,
        Verbose = 1,
        Debug = 2,
        Info = 3,
        Warn = 4,
        Error = 5,
        Critical = 6,
        Max = 7
    }

    public enum LogCategory
    {
        APPLICATION,
        ERROR,
        ASSERT,
        SYSTEM,
        AUDIO,
        VIDEO,
        RENDER,
        INPUT,
        TEST,
        RESERVED1,
        RESERVED2,
        RESERVED3,
        RESERVED4,
        RESERVED5,
        RESERVED6,
        RESERVED7,
        RESERVED8,
        RESERVED9,
        RESERVED10,
        CUSTOM,
        MAX
    }


    public enum ScanCode
    {
        SCANCODE_UNKNOWN = 0,
        SCANCODE_A = 4,
        SCANCODE_B = 5,
        SCANCODE_C = 6,
        SCANCODE_D = 7,
        SCANCODE_E = 8,
        SCANCODE_F = 9,
        SCANCODE_G = 10,
        SCANCODE_H = 11,
        SCANCODE_I = 12,
        SCANCODE_J = 13,
        SCANCODE_K = 14,
        SCANCODE_L = 15,
        SCANCODE_M = 16,
        SCANCODE_N = 17,
        SCANCODE_O = 18,
        SCANCODE_P = 19,
        SCANCODE_Q = 20,
        SCANCODE_R = 21,
        SCANCODE_S = 22,
        SCANCODE_T = 23,
        SCANCODE_U = 24,
        SCANCODE_V = 25,
        SCANCODE_W = 26,
        SCANCODE_X = 27,
        SCANCODE_Y = 28,
        SCANCODE_Z = 29,
        SCANCODE_1 = 30,
        SCANCODE_2 = 31,
        SCANCODE_3 = 32,
        SCANCODE_4 = 33,
        SCANCODE_5 = 34,
        SCANCODE_6 = 35,
        SCANCODE_7 = 36,
        SCANCODE_8 = 37,
        SCANCODE_9 = 38,
        SCANCODE_0 = 39,
        SCANCODE_RETURN = 40,
        SCANCODE_ESCAPE = 41,
        SCANCODE_BACKSPACE = 42,
        SCANCODE_TAB = 43,
        SCANCODE_SPACE = 44,
        SCANCODE_MINUS = 45,
        SCANCODE_EQUALS = 46,
        SCANCODE_LEFTBRACKET = 47,
        SCANCODE_RIGHTBRACKET = 48,
        SCANCODE_BACKSLASH = 49,
        SCANCODE_NONUSHASH = 50,
        SCANCODE_SEMICOLON = 51,
        SCANCODE_APOSTROPHE = 52,
        SCANCODE_GRAVE = 53,
        SCANCODE_COMMA = 54,
        SCANCODE_PERIOD = 55,
        SCANCODE_SLASH = 56,
        SCANCODE_CAPSLOCK = 57,
        SCANCODE_F1 = 58,
        SCANCODE_F2 = 59,
        SCANCODE_F3 = 60,
        SCANCODE_F4 = 61,
        SCANCODE_F5 = 62,
        SCANCODE_F6 = 63,
        SCANCODE_F7 = 64,
        SCANCODE_F8 = 65,
        SCANCODE_F9 = 66,
        SCANCODE_F10 = 67,
        SCANCODE_F11 = 68,
        SCANCODE_F12 = 69,
        SCANCODE_PRINTSCREEN = 70,
        SCANCODE_SCROLLLOCK = 71,
        SCANCODE_PAUSE = 72,
        SCANCODE_INSERT = 73,
        SCANCODE_HOME = 74,
        SCANCODE_PAGEUP = 75,
        SCANCODE_DELETE = 76,
        SCANCODE_END = 77,
        SCANCODE_PAGEDOWN = 78,
        SCANCODE_RIGHT = 79,
        SCANCODE_LEFT = 80,
        SCANCODE_DOWN = 81,
        SCANCODE_UP = 82,
        SCANCODE_NUMLOCKCLEAR = 83,
        SCANCODE_KP_DIVIDE = 84,
        SCANCODE_KP_MULTIPLY = 85,
        SCANCODE_KP_MINUS = 86,
        SCANCODE_KP_PLUS = 87,
        SCANCODE_KP_ENTER = 88,
        SCANCODE_KP_1 = 89,
        SCANCODE_KP_2 = 90,
        SCANCODE_KP_3 = 91,
        SCANCODE_KP_4 = 92,
        SCANCODE_KP_5 = 93,
        SCANCODE_KP_6 = 94,
        SCANCODE_KP_7 = 95,
        SCANCODE_KP_8 = 96,
        SCANCODE_KP_9 = 97,
        SCANCODE_KP_0 = 98,
        SCANCODE_KP_PERIOD = 99,
        SCANCODE_NONUSBACKSLASH = 100,
        SCANCODE_APPLICATION = 101,
        SCANCODE_POWER = 102,
        SCANCODE_KP_EQUALS = 103,
        SCANCODE_F13 = 104,
        SCANCODE_F14 = 105,
        SCANCODE_F15 = 106,
        SCANCODE_F16 = 107,
        SCANCODE_F17 = 108,
        SCANCODE_F18 = 109,
        SCANCODE_F19 = 110,
        SCANCODE_F20 = 111,
        SCANCODE_F21 = 112,
        SCANCODE_F22 = 113,
        SCANCODE_F23 = 114,
        SCANCODE_F24 = 115,
        SCANCODE_EXECUTE = 116,
        SCANCODE_HELP = 117,
        SCANCODE_MENU = 118,
        SCANCODE_SELECT = 119,
        SCANCODE_STOP = 120,
        SCANCODE_AGAIN = 121,
        SCANCODE_UNDO = 122,
        SCANCODE_CUT = 123,
        SCANCODE_COPY = 124,
        SCANCODE_PASTE = 125,
        SCANCODE_FIND = 126,
        SCANCODE_MUTE = 127,
        SCANCODE_VOLUMEUP = 128,
        SCANCODE_VOLUMEDOWN = 129,
        SCANCODE_LOCKINGCAPSLOCK = 130,
        SCANCODE_LOCKINGNUMLOCK = 131,
        SCANCODE_LOCKINGSCROLLLOCK = 132,
        SCANCODE_KP_COMMA = 133,
        SCANCODE_KP_EQUALSAS400 = 134,
        SCANCODE_INTERNATIONAL1 = 135,
        SCANCODE_INTERNATIONAL2 = 136,
        SCANCODE_INTERNATIONAL3 = 137,
        SCANCODE_INTERNATIONAL4 = 138,
        SCANCODE_INTERNATIONAL5 = 139,
        SCANCODE_INTERNATIONAL6 = 140,
        SCANCODE_INTERNATIONAL7 = 141,
        SCANCODE_INTERNATIONAL8 = 142,
        SCANCODE_INTERNATIONAL9 = 143,
        SCANCODE_LANG1 = 144,
        SCANCODE_LANG2 = 145,
        SCANCODE_LANG3 = 146,
        SCANCODE_LANG4 = 147,
        SCANCODE_LANG5 = 148,
        SCANCODE_LANG6 = 149,
        SCANCODE_LANG7 = 150,
        SCANCODE_LANG8 = 151,
        SCANCODE_LANG9 = 152,
        SCANCODE_ALTERASE = 153,
        SCANCODE_SYSREQ = 154,
        SCANCODE_CANCEL = 155,
        SCANCODE_CLEAR = 156,
        SCANCODE_PRIOR = 157,
        SCANCODE_RETURN2 = 158,
        SCANCODE_SEPARATOR = 159,
        SCANCODE_OUT = 160,
        SCANCODE_OPER = 161,
        SCANCODE_CLEARAGAIN = 162,
        SCANCODE_CRSEL = 163,
        SCANCODE_EXSEL = 164,
        SCANCODE_KP_00 = 176,
        SCANCODE_KP_000 = 177,
        SCANCODE_THOUSANDSSEPARATOR = 178,
        SCANCODE_DECIMALSEPARATOR = 179,
        SCANCODE_CURRENCYUNIT = 180,
        SCANCODE_CURRENCYSUBUNIT = 181,
        SCANCODE_KP_LEFTPAREN = 182,
        SCANCODE_KP_RIGHTPAREN = 183,
        SCANCODE_KP_LEFTBRACE = 184,
        SCANCODE_KP_RIGHTBRACE = 185,
        SCANCODE_KP_TAB = 186,
        SCANCODE_KP_BACKSPACE = 187,
        SCANCODE_KP_A = 188,
        SCANCODE_KP_B = 189,
        SCANCODE_KP_C = 190,
        SCANCODE_KP_D = 191,
        SCANCODE_KP_E = 192,
        SCANCODE_KP_F = 193,
        SCANCODE_KP_XOR = 194,
        SCANCODE_KP_POWER = 195,
        SCANCODE_KP_PERCENT = 196,
        SCANCODE_KP_LESS = 197,
        SCANCODE_KP_GREATER = 198,
        SCANCODE_KP_AMPERSAND = 199,
        SCANCODE_KP_DBLAMPERSAND = 200,
        SCANCODE_KP_VERTICALBAR = 201,
        SCANCODE_KP_DBLVERTICALBAR = 202,
        SCANCODE_KP_COLON = 203,
        SCANCODE_KP_HASH = 204,
        SCANCODE_KP_SPACE = 205,
        SCANCODE_KP_AT = 206,
        SCANCODE_KP_EXCLAM = 207,
        SCANCODE_KP_MEMSTORE = 208,
        SCANCODE_KP_MEMRECALL = 209,
        SCANCODE_KP_MEMCLEAR = 210,
        SCANCODE_KP_MEMADD = 211,
        SCANCODE_KP_MEMSUBTRACT = 212,
        SCANCODE_KP_MEMMULTIPLY = 213,
        SCANCODE_KP_MEMDIVIDE = 214,
        SCANCODE_KP_PLUSMINUS = 215,
        SCANCODE_KP_CLEAR = 216,
        SCANCODE_KP_CLEARENTRY = 217,
        SCANCODE_KP_BINARY = 218,
        SCANCODE_KP_OCTAL = 219,
        SCANCODE_KP_DECIMAL = 220,
        SCANCODE_KP_HEXADECIMAL = 221,
        SCANCODE_LCTRL = 224,
        SCANCODE_LSHIFT = 225,
        SCANCODE_LALT = 226,
        SCANCODE_LGUI = 227,
        SCANCODE_RCTRL = 228,
        SCANCODE_RSHIFT = 229,
        SCANCODE_RALT = 230,
        SCANCODE_RGUI = 231,
        SCANCODE_MODE = 257,
        SCANCODE_AUDIONEXT = 258,
        SCANCODE_AUDIOPREV = 259,
        SCANCODE_AUDIOSTOP = 260,
        SCANCODE_AUDIOPLAY = 261,
        SCANCODE_AUDIOMUTE = 262,
        SCANCODE_MEDIASELECT = 263,
        SCANCODE_WWW = 264,
        SCANCODE_MAIL = 265,
        SCANCODE_CALCULATOR = 266,
        SCANCODE_COMPUTER = 267,
        SCANCODE_AC_SEARCH = 268,
        SCANCODE_AC_HOME = 269,
        SCANCODE_AC_BACK = 270,
        SCANCODE_AC_FORWARD = 271,
        SCANCODE_AC_STOP = 272,
        SCANCODE_AC_REFRESH = 273,
        SCANCODE_AC_BOOKMARKS = 274,
        SCANCODE_BRIGHTNESSDOWN = 275,
        SCANCODE_BRIGHTNESSUP = 276,
        SCANCODE_DISPLAYSWITCH = 277,
        SCANCODE_KBDILLUMTOGGLE = 278,
        SCANCODE_KBDILLUMDOWN = 279,
        SCANCODE_KBDILLUMUP = 280,
        SCANCODE_EJECT = 281,
        SCANCODE_SLEEP = 282,
        SCANCODE_APP1 = 283,
        SCANCODE_APP2 = 284,
        SCANCODE_AUDIOREWIND = 285,
        SCANCODE_AUDIOFASTFORWARD = 286,
        NUM_SCANCODES = 512
    }

    public enum KeyCode
    {
        SCANCODE_MASK = (1 << 30),
        UNKNOWN = 0,

        RETURN = '\r',
        ESCAPE = 27,
        BACKSPACE = '\b',
        TAB = '\t',
        SPACE = ' ',
        EXCLAIM = '!',
        QUOTEDBL = '"',
        HASH = '#',
        PERCENT = '%',
        DOLLAR = '$',
        AMPERSAND = '&',
        QUOTE = '\'',
        LEFTPAREN = '(',
        RIGHTPAREN = ')',
        ASTERISK = '*',
        PLUS = '+',
        COMMA = ',',
        MINUS = '-',
        PERIOD = '.',
        SLASH = '/',
        NUM0 = '0',
        NUM1 = '1',
        NUM2 = '2',
        NUM3 = '3',
        NUM4 = '4',
        NUM5 = '5',
        NUM6 = '6',
        NUM7 = '7',
        NUM8 = '8',
        NUM9 = '9',
        COLON = ':',
        SEMICOLON = ';',
        LESS = '<',
        EQUALS = '=',
        GREATER = '>',
        QUESTION = '?',
        AT = '@',
        LEFTBRACKET = '[',
        BACKSLASH = '\\',
        RIGHTBRACKET = ']',
        CARET = '^',
        UNDERSCORE = '_',
        BACKQUOTE = '`',
        a = 'a',
        b = 'b',
        c = 'c',
        d = 'd',
        e = 'e',
        f = 'f',
        g = 'g',
        h = 'h',
        i = 'i',
        j = 'j',
        k = 'k',
        l = 'l',
        m = 'm',
        n = 'n',
        o = 'o',
        p = 'p',
        q = 'q',
        r = 'r',
        s = 's',
        t = 't',
        u = 'u',
        v = 'v',
        w = 'w',
        x = 'x',
        y = 'y',
        z = 'z',

        CAPSLOCK = (int)ScanCode.SCANCODE_CAPSLOCK | SCANCODE_MASK,

        F1 = (int)ScanCode.SCANCODE_F1 | SCANCODE_MASK,
        F2 = (int)ScanCode.SCANCODE_F2 | SCANCODE_MASK,
        F3 = (int)ScanCode.SCANCODE_F3 | SCANCODE_MASK,
        F4 = (int)ScanCode.SCANCODE_F4 | SCANCODE_MASK,
        F5 = (int)ScanCode.SCANCODE_F5 | SCANCODE_MASK,
        F6 = (int)ScanCode.SCANCODE_F6 | SCANCODE_MASK,
        F7 = (int)ScanCode.SCANCODE_F7 | SCANCODE_MASK,
        F8 = (int)ScanCode.SCANCODE_F8 | SCANCODE_MASK,
        F9 = (int)ScanCode.SCANCODE_F9 | SCANCODE_MASK,
        F10 = (int)ScanCode.SCANCODE_F10 | SCANCODE_MASK,
        F11 = (int)ScanCode.SCANCODE_F11 | SCANCODE_MASK,
        F12 = (int)ScanCode.SCANCODE_F12 | SCANCODE_MASK,

        PRINTSCREEN = (int)ScanCode.SCANCODE_PRINTSCREEN | SCANCODE_MASK,
        SCROLLLOCK = (int)ScanCode.SCANCODE_SCROLLLOCK | SCANCODE_MASK,
        PAUSE = (int)ScanCode.SCANCODE_PAUSE | SCANCODE_MASK,
        INSERT = (int)ScanCode.SCANCODE_INSERT | SCANCODE_MASK,
        HOME = (int)ScanCode.SCANCODE_HOME | SCANCODE_MASK,
        PAGEUP = (int)ScanCode.SCANCODE_PAGEUP | SCANCODE_MASK,
        DELETE = 127,
        END = (int)ScanCode.SCANCODE_END | SCANCODE_MASK,
        PAGEDOWN = (int)ScanCode.SCANCODE_PAGEDOWN | SCANCODE_MASK,
        RIGHT = (int)ScanCode.SCANCODE_RIGHT | SCANCODE_MASK,
        LEFT = (int)ScanCode.SCANCODE_LEFT | SCANCODE_MASK,
        DOWN = (int)ScanCode.SCANCODE_DOWN | SCANCODE_MASK,
        UP = (int)ScanCode.SCANCODE_UP | SCANCODE_MASK,

        NUMLOCKCLEAR = (int)ScanCode.SCANCODE_NUMLOCKCLEAR | SCANCODE_MASK,
        KP_DIVIDE = (int)ScanCode.SCANCODE_KP_DIVIDE | SCANCODE_MASK,
        KP_MULTIPLY = (int)ScanCode.SCANCODE_KP_MULTIPLY | SCANCODE_MASK,
        KP_MINUS = (int)ScanCode.SCANCODE_KP_MINUS | SCANCODE_MASK,
        KP_PLUS = (int)ScanCode.SCANCODE_KP_PLUS | SCANCODE_MASK,
        KP_ENTER = (int)ScanCode.SCANCODE_KP_ENTER | SCANCODE_MASK,
        KP_1 = (int)ScanCode.SCANCODE_KP_1 | SCANCODE_MASK,
        KP_2 = (int)ScanCode.SCANCODE_KP_2 | SCANCODE_MASK,
        KP_3 = (int)ScanCode.SCANCODE_KP_3 | SCANCODE_MASK,
        KP_4 = (int)ScanCode.SCANCODE_KP_4 | SCANCODE_MASK,
        KP_5 = (int)ScanCode.SCANCODE_KP_5 | SCANCODE_MASK,
        KP_6 = (int)ScanCode.SCANCODE_KP_6 | SCANCODE_MASK,
        KP_7 = (int)ScanCode.SCANCODE_KP_7 | SCANCODE_MASK,
        KP_8 = (int)ScanCode.SCANCODE_KP_8 | SCANCODE_MASK,
        KP_9 = (int)ScanCode.SCANCODE_KP_9 | SCANCODE_MASK,
        KP_0 = (int)ScanCode.SCANCODE_KP_0 | SCANCODE_MASK,
        KP_PERIOD = (int)ScanCode.SCANCODE_KP_PERIOD | SCANCODE_MASK,

        APPLICATION = (int)ScanCode.SCANCODE_APPLICATION | SCANCODE_MASK,
        POWER = (int)ScanCode.SCANCODE_POWER | SCANCODE_MASK,
        KP_EQUALS = (int)ScanCode.SCANCODE_KP_EQUALS | SCANCODE_MASK,
        F13 = (int)ScanCode.SCANCODE_F13 | SCANCODE_MASK,
        F14 = (int)ScanCode.SCANCODE_F14 | SCANCODE_MASK,
        F15 = (int)ScanCode.SCANCODE_F15 | SCANCODE_MASK,
        F16 = (int)ScanCode.SCANCODE_F16 | SCANCODE_MASK,
        F17 = (int)ScanCode.SCANCODE_F17 | SCANCODE_MASK,
        F18 = (int)ScanCode.SCANCODE_F18 | SCANCODE_MASK,
        F19 = (int)ScanCode.SCANCODE_F19 | SCANCODE_MASK,
        F20 = (int)ScanCode.SCANCODE_F20 | SCANCODE_MASK,
        F21 = (int)ScanCode.SCANCODE_F21 | SCANCODE_MASK,
        F22 = (int)ScanCode.SCANCODE_F22 | SCANCODE_MASK,
        F23 = (int)ScanCode.SCANCODE_F23 | SCANCODE_MASK,
        F24 = (int)ScanCode.SCANCODE_F24 | SCANCODE_MASK,
        EXECUTE = (int)ScanCode.SCANCODE_EXECUTE | SCANCODE_MASK,
        HELP = (int)ScanCode.SCANCODE_HELP | SCANCODE_MASK,
        MENU = (int)ScanCode.SCANCODE_MENU | SCANCODE_MASK,
        SELECT = (int)ScanCode.SCANCODE_SELECT | SCANCODE_MASK,
        STOP = (int)ScanCode.SCANCODE_STOP | SCANCODE_MASK,
        AGAIN = (int)ScanCode.SCANCODE_AGAIN | SCANCODE_MASK,
        UNDO = (int)ScanCode.SCANCODE_UNDO | SCANCODE_MASK,
        CUT = (int)ScanCode.SCANCODE_CUT | SCANCODE_MASK,
        COPY = (int)ScanCode.SCANCODE_COPY | SCANCODE_MASK,
        PASTE = (int)ScanCode.SCANCODE_PASTE | SCANCODE_MASK,
        FIND = (int)ScanCode.SCANCODE_FIND | SCANCODE_MASK,
        MUTE = (int)ScanCode.SCANCODE_MUTE | SCANCODE_MASK,
        VOLUMEUP = (int)ScanCode.SCANCODE_VOLUMEUP | SCANCODE_MASK,
        VOLUMEDOWN = (int)ScanCode.SCANCODE_VOLUMEDOWN | SCANCODE_MASK,
        KP_COMMA = (int)ScanCode.SCANCODE_KP_COMMA | SCANCODE_MASK,
        KP_EQUALSAS400 = (int)ScanCode.SCANCODE_KP_EQUALSAS400 | SCANCODE_MASK,
        ALTERASE = (int)ScanCode.SCANCODE_ALTERASE | SCANCODE_MASK,
        SYSREQ = (int)ScanCode.SCANCODE_SYSREQ | SCANCODE_MASK,
        CANCEL = (int)ScanCode.SCANCODE_CANCEL | SCANCODE_MASK,
        CLEAR = (int)ScanCode.SCANCODE_CLEAR | SCANCODE_MASK,
        PRIOR = (int)ScanCode.SCANCODE_PRIOR | SCANCODE_MASK,
        RETURN2 = (int)ScanCode.SCANCODE_RETURN2 | SCANCODE_MASK,
        SEPARATOR = (int)ScanCode.SCANCODE_SEPARATOR | SCANCODE_MASK,
        OUT = (int)ScanCode.SCANCODE_OUT | SCANCODE_MASK,
        OPER = (int)ScanCode.SCANCODE_OPER | SCANCODE_MASK,
        CLEARAGAIN = (int)ScanCode.SCANCODE_CLEARAGAIN | SCANCODE_MASK,
        CRSEL = (int)ScanCode.SCANCODE_CRSEL | SCANCODE_MASK,
        EXSEL = (int)ScanCode.SCANCODE_EXSEL | SCANCODE_MASK,
        KP_00 = (int)ScanCode.SCANCODE_KP_00 | SCANCODE_MASK,
        KP_000 = (int)ScanCode.SCANCODE_KP_000 | SCANCODE_MASK,
        THOUSANDSSEPARATOR = (int)ScanCode.SCANCODE_THOUSANDSSEPARATOR | SCANCODE_MASK,
        DECIMALSEPARATOR = (int)ScanCode.SCANCODE_DECIMALSEPARATOR | SCANCODE_MASK,
        CURRENCYUNIT = (int)ScanCode.SCANCODE_CURRENCYUNIT | SCANCODE_MASK,
        CURRENCYSUBUNIT = (int)ScanCode.SCANCODE_CURRENCYSUBUNIT | SCANCODE_MASK,
        KP_LEFTPAREN = (int)ScanCode.SCANCODE_KP_LEFTPAREN | SCANCODE_MASK,
        KP_RIGHTPAREN = (int)ScanCode.SCANCODE_KP_RIGHTPAREN | SCANCODE_MASK,
        KP_LEFTBRACE = (int)ScanCode.SCANCODE_KP_LEFTBRACE | SCANCODE_MASK,
        KP_RIGHTBRACE = (int)ScanCode.SCANCODE_KP_RIGHTBRACE | SCANCODE_MASK,
        KP_TAB = (int)ScanCode.SCANCODE_KP_TAB | SCANCODE_MASK,
        KP_BACKSPACE = (int)ScanCode.SCANCODE_KP_BACKSPACE | SCANCODE_MASK,
        KP_A = (int)ScanCode.SCANCODE_KP_A | SCANCODE_MASK,
        KP_B = (int)ScanCode.SCANCODE_KP_B | SCANCODE_MASK,
        KP_C = (int)ScanCode.SCANCODE_KP_C | SCANCODE_MASK,
        KP_D = (int)ScanCode.SCANCODE_KP_D | SCANCODE_MASK,
        KP_E = (int)ScanCode.SCANCODE_KP_E | SCANCODE_MASK,
        KP_F = (int)ScanCode.SCANCODE_KP_F | SCANCODE_MASK,
        KP_XOR = (int)ScanCode.SCANCODE_KP_XOR | SCANCODE_MASK,
        KP_POWER = (int)ScanCode.SCANCODE_KP_POWER | SCANCODE_MASK,
        KP_PERCENT = (int)ScanCode.SCANCODE_KP_PERCENT | SCANCODE_MASK,
        KP_LESS = (int)ScanCode.SCANCODE_KP_LESS | SCANCODE_MASK,
        KP_GREATER = (int)ScanCode.SCANCODE_KP_GREATER | SCANCODE_MASK,
        KP_AMPERSAND = (int)ScanCode.SCANCODE_KP_AMPERSAND | SCANCODE_MASK,
        KP_DBLAMPERSAND = (int)ScanCode.SCANCODE_KP_DBLAMPERSAND | SCANCODE_MASK,
        KP_VERTICALBAR = (int)ScanCode.SCANCODE_KP_VERTICALBAR | SCANCODE_MASK,
        KP_DBLVERTICALBAR = (int)ScanCode.SCANCODE_KP_DBLVERTICALBAR | SCANCODE_MASK,
        KP_COLON = (int)ScanCode.SCANCODE_KP_COLON | SCANCODE_MASK,
        KP_HASH = (int)ScanCode.SCANCODE_KP_HASH | SCANCODE_MASK,
        KP_SPACE = (int)ScanCode.SCANCODE_KP_SPACE | SCANCODE_MASK,
        KP_AT = (int)ScanCode.SCANCODE_KP_AT | SCANCODE_MASK,
        KP_EXCLAM = (int)ScanCode.SCANCODE_KP_EXCLAM | SCANCODE_MASK,
        KP_MEMSTORE = (int)ScanCode.SCANCODE_KP_MEMSTORE | SCANCODE_MASK,
        KP_MEMRECALL = (int)ScanCode.SCANCODE_KP_MEMRECALL | SCANCODE_MASK,
        KP_MEMCLEAR = (int)ScanCode.SCANCODE_KP_MEMCLEAR | SCANCODE_MASK,
        KP_MEMADD = (int)ScanCode.SCANCODE_KP_MEMADD | SCANCODE_MASK,
        KP_MEMSUBTRACT = (int)ScanCode.SCANCODE_KP_MEMSUBTRACT | SCANCODE_MASK,
        KP_MEMMULTIPLY = (int)ScanCode.SCANCODE_KP_MEMMULTIPLY | SCANCODE_MASK,
        KP_MEMDIVIDE = (int)ScanCode.SCANCODE_KP_MEMDIVIDE | SCANCODE_MASK,
        KP_PLUSMINUS = (int)ScanCode.SCANCODE_KP_PLUSMINUS | SCANCODE_MASK,
        KP_CLEAR = (int)ScanCode.SCANCODE_KP_CLEAR | SCANCODE_MASK,
        KP_CLEARENTRY = (int)ScanCode.SCANCODE_KP_CLEARENTRY | SCANCODE_MASK,
        KP_BINARY = (int)ScanCode.SCANCODE_KP_BINARY | SCANCODE_MASK,
        KP_OCTAL = (int)ScanCode.SCANCODE_KP_OCTAL | SCANCODE_MASK,
        KP_DECIMAL = (int)ScanCode.SCANCODE_KP_DECIMAL | SCANCODE_MASK,
        KP_HEXADECIMAL = (int)ScanCode.SCANCODE_KP_HEXADECIMAL | SCANCODE_MASK,
        LCTRL = (int)ScanCode.SCANCODE_LCTRL | SCANCODE_MASK,
        LSHIFT = (int)ScanCode.SCANCODE_LSHIFT | SCANCODE_MASK,
        LALT = (int)ScanCode.SCANCODE_LALT | SCANCODE_MASK,
        LGUI = (int)ScanCode.SCANCODE_LGUI | SCANCODE_MASK,
        RCTRL = (int)ScanCode.SCANCODE_RCTRL | SCANCODE_MASK,
        RSHIFT = (int)ScanCode.SCANCODE_RSHIFT | SCANCODE_MASK,
        RALT = (int)ScanCode.SCANCODE_RALT | SCANCODE_MASK,
        RGUI = (int)ScanCode.SCANCODE_RGUI | SCANCODE_MASK,
        MODE = (int)ScanCode.SCANCODE_MODE | SCANCODE_MASK,
        AUDIONEXT = (int)ScanCode.SCANCODE_AUDIONEXT | SCANCODE_MASK,
        AUDIOPREV = (int)ScanCode.SCANCODE_AUDIOPREV | SCANCODE_MASK,
        AUDIOSTOP = (int)ScanCode.SCANCODE_AUDIOSTOP | SCANCODE_MASK,
        AUDIOPLAY = (int)ScanCode.SCANCODE_AUDIOPLAY | SCANCODE_MASK,
        AUDIOMUTE = (int)ScanCode.SCANCODE_AUDIOMUTE | SCANCODE_MASK,
        MEDIASELECT = (int)ScanCode.SCANCODE_MEDIASELECT | SCANCODE_MASK,
        WWW = (int)ScanCode.SCANCODE_WWW | SCANCODE_MASK,
        MAIL = (int)ScanCode.SCANCODE_MAIL | SCANCODE_MASK,
        CALCULATOR = (int)ScanCode.SCANCODE_CALCULATOR | SCANCODE_MASK,
        COMPUTER = (int)ScanCode.SCANCODE_COMPUTER | SCANCODE_MASK,
        AC_SEARCH = (int)ScanCode.SCANCODE_AC_SEARCH | SCANCODE_MASK,
        AC_HOME = (int)ScanCode.SCANCODE_AC_HOME | SCANCODE_MASK,
        AC_BACK = (int)ScanCode.SCANCODE_AC_BACK | SCANCODE_MASK,
        AC_FORWARD = (int)ScanCode.SCANCODE_AC_FORWARD | SCANCODE_MASK,
        AC_STOP = (int)ScanCode.SCANCODE_AC_STOP | SCANCODE_MASK,
        AC_REFRESH = (int)ScanCode.SCANCODE_AC_REFRESH | SCANCODE_MASK,
        AC_BOOKMARKS = (int)ScanCode.SCANCODE_AC_BOOKMARKS | SCANCODE_MASK,
        BRIGHTNESSDOWN = (int)ScanCode.SCANCODE_BRIGHTNESSDOWN | SCANCODE_MASK,
        BRIGHTNESSUP = (int)ScanCode.SCANCODE_BRIGHTNESSUP | SCANCODE_MASK,
        DISPLAYSWITCH = (int)ScanCode.SCANCODE_DISPLAYSWITCH | SCANCODE_MASK,
        KBDILLUMTOGGLE = (int)ScanCode.SCANCODE_KBDILLUMTOGGLE | SCANCODE_MASK,
        KBDILLUMDOWN = (int)ScanCode.SCANCODE_KBDILLUMDOWN | SCANCODE_MASK,
        KBDILLUMUP = (int)ScanCode.SCANCODE_KBDILLUMUP | SCANCODE_MASK,
        EJECT = (int)ScanCode.SCANCODE_EJECT | SCANCODE_MASK,
        SLEEP = (int)ScanCode.SCANCODE_SLEEP | SCANCODE_MASK,
        APP1 = (int)ScanCode.SCANCODE_APP1 | SCANCODE_MASK,
        APP2 = (int)ScanCode.SCANCODE_APP2 | SCANCODE_MASK,
        AUDIOREWIND = (int)ScanCode.SCANCODE_AUDIOREWIND | SCANCODE_MASK,
        AUDIOFASTFORWARD = (int)ScanCode.SCANCODE_AUDIOFASTFORWARD | SCANCODE_MASK,
    }

    [Flags]
    public enum KeyMod : ushort
    {
        NONE = 0x0000,
        LSHIFT = 0x0001,
        RSHIFT = 0x0002,
        LCTRL = 0x0040,
        RCTRL = 0x0080,
        LALT = 0x0100,
        RALT = 0x0200,
        LGUI = 0x0400,
        RGUI = 0x0800,
        NUM = 0x1000,
        CAPS = 0x2000,
        MODE = 0x4000,
        SCROLL = 0x8000,
        CTRL = (LCTRL | RCTRL),
        SHIFT = (LSHIFT | RSHIFT),
        ALT = (LALT | RALT),
        GUI = (LGUI | RGUI)
    }

    public enum Icons
    {
        NONE = 0x0,
        PX500 = 0x0000F100,
        PX500_WITH_CIRCLE = 0x0000F101,
        ADD_TO_LIST = 0x0000F102,
        ADD_USER = 0x0000F103,
        ADDRESS = 0x0000F104,
        ADJUST = 0x0000F105,
        AIR = 0x0000F106,
        AIRCRAFT = 0x0000F107,
        AIRCRAFT_LANDING = 0x0000F108,
        AIRCRAFT_TAKE_OFF = 0x0000F109,
        ALIGN_BOTTOM = 0x0000F10A,
        ALIGN_HORIZONTAL_MIDDLE = 0x0000F10B,
        ALIGN_LEFT = 0x0000F10C,
        ALIGN_RIGHT = 0x0000F10D,
        ALIGN_TOP = 0x0000F10E,
        ALIGN_VERTICAL_MIDDLE = 0x0000F10F,
        APP_STORE = 0x0000F110,
        ARCHIVE = 0x0000F111,
        AREA_GRAPH = 0x0000F112,
        ARROW_BOLD_DOWN = 0x0000F113,
        ARROW_BOLD_LEFT = 0x0000F114,
        ARROW_BOLD_RIGHT = 0x0000F115,
        ARROW_BOLD_UP = 0x0000F116,
        ARROW_DOWN = 0x0000F117,
        ARROW_LEFT = 0x0000F118,
        ARROW_LONG_DOWN = 0x0000F119,
        ARROW_LONG_LEFT = 0x0000F11A,
        ARROW_LONG_RIGHT = 0x0000F11B,
        ARROW_LONG_UP = 0x0000F11C,
        ARROW_RIGHT = 0x0000F11D,
        ARROW_UP = 0x0000F11E,
        ARROW_WITH_CIRCLE_DOWN = 0x0000F11F,
        ARROW_WITH_CIRCLE_LEFT = 0x0000F120,
        ARROW_WITH_CIRCLE_RIGHT = 0x0000F121,
        ARROW_WITH_CIRCLE_UP = 0x0000F122,
        ATTACHMENT = 0x0000F123,
        AWARENESS_RIBBON = 0x0000F124,
        BACK = 0x0000F125,
        BACK_IN_TIME = 0x0000F126,
        BAIDU = 0x0000F127,
        BAR_GRAPH = 0x0000F128
            , BASECAMP = 0x0000F129
            , BATTERY = 0x0000F12A
            , BEAMED_NOTE = 0x0000F12B
            , BEHANCE = 0x0000F12C
            , BELL = 0x0000F12D
            , BLACKBOARD = 0x0000F12E
            , BLOCK = 0x0000F12F
            , BOOK = 0x0000F130
            , BOOKMARK = 0x0000F131
            , BOOKMARKS = 0x0000F132
            , BOWL = 0x0000F133
            , BOX = 0x0000F134
            , BRIEFCASE = 0x0000F135
            , BROWSER = 0x0000F136
            , BRUSH = 0x0000F137
            , BUCKET = 0x0000F138
            , BUG = 0x0000F139
            , CAKE = 0x0000F13A
            , CALCULATOR = 0x0000F13B
            , CALENDAR = 0x0000F13C
            , CAMERA = 0x0000F13D
            , CCW = 0x0000F13E
            , CHAT = 0x0000F13F
            , CHECK = 0x0000F140
            , CHEVRON_DOWN = 0x0000F141
            , CHEVRON_LEFT = 0x0000F142
            , CHEVRON_RIGHT = 0x0000F143
            , CHEVRON_SMALL_DOWN = 0x0000F144
            , CHEVRON_SMALL_LEFT = 0x0000F145
            , CHEVRON_SMALL_RIGHT = 0x0000F146
            , CHEVRON_SMALL_UP = 0x0000F147
            , CHEVRON_THIN_DOWN = 0x0000F148
            , CHEVRON_THIN_LEFT = 0x0000F149
            , CHEVRON_THIN_RIGHT = 0x0000F14A
            , CHEVRON_THIN_UP = 0x0000F14B
            , CHEVRON_UP = 0x0000F14C
            , CHEVRON_WITH_CIRCLE_DOWN = 0x0000F14D
            , CHEVRON_WITH_CIRCLE_LEFT = 0x0000F14E
            , CHEVRON_WITH_CIRCLE_RIGHT = 0x0000F14F
            , CHEVRON_WITH_CIRCLE_UP = 0x0000F150
            , CIRCLE = 0x0000F151
            , CIRCLE_WITH_CROSS = 0x0000F152
            , CIRCLE_WITH_MINUS = 0x0000F153
            , CIRCLE_WITH_PLUS = 0x0000F154
            , CIRCULAR_GRAPH = 0x0000F155
            , CLAPPERBOARD = 0x0000F156
            , CLASSIC_COMPUTER = 0x0000F157
            , CLIPBOARD = 0x0000F158
            , CLOCK = 0x0000F159
            , CLOUD = 0x0000F15A
            , CODE = 0x0000F15B
            , COG = 0x0000F15C
            , COLOURS = 0x0000F15D
            , COMPASS = 0x0000F15E
            , CONTROLLER_FAST_BACKWARD = 0x0000F15F
            , CONTROLLER_FAST_FORWARD = 0x0000F160
            , CONTROLLER_JUMP_TO_START = 0x0000F161
            , CONTROLLER_NEXT = 0x0000F162
            , CONTROLLER_PAUS = 0x0000F163
            , CONTROLLER_PLAY = 0x0000F164
            , CONTROLLER_RECORD = 0x0000F165
            , CONTROLLER_STOP = 0x0000F166
            , CONTROLLER_VOLUME = 0x0000F167
            , COPY = 0x0000F168
            , CREATIVE_CLOUD = 0x0000F169
            , CREATIVE_COMMONS = 0x0000F16A
            , CREATIVE_COMMONS_ATTRIBUTION = 0x0000F16B
            , CREATIVE_COMMONS_NODERIVS = 0x0000F16C
            , CREATIVE_COMMONS_NONCOMMERCIAL_EU = 0x0000F16D
            , CREATIVE_COMMONS_NONCOMMERCIAL_US = 0x0000F16E
            , CREATIVE_COMMONS_PUBLIC_DOMAIN = 0x0000F16F
            , CREATIVE_COMMONS_REMIX = 0x0000F170
            , CREATIVE_COMMONS_SHARE = 0x0000F171
            , CREATIVE_COMMONS_SHAREALIKE = 0x0000F172
            , CREDIT = 0x0000F173
            , CREDIT_CARD = 0x0000F174
            , CROP = 0x0000F175
            , CROSS = 0x0000F176
            , CUP = 0x0000F177
            , CW = 0x0000F178
            , CYCLE = 0x0000F179
            , DATABASE = 0x0000F17A
            , DIAL_PAD = 0x0000F17B
            , DIRECTION = 0x0000F17C
            , DOCUMENT = 0x0000F17D
            , DOCUMENT_LANDSCAPE = 0x0000F17E
            , DOCUMENTS = 0x0000F17F
            , DOT_SINGLE = 0x0000F180
            , DOTS_THREE_HORIZONTAL = 0x0000F181
            , DOTS_THREE_VERTICAL = 0x0000F182
            , DOTS_TWO_HORIZONTAL = 0x0000F183
            , DOTS_TWO_VERTICAL = 0x0000F184
            , DOWNLOAD = 0x0000F185
            , DRIBBBLE = 0x0000F186
            , DRIBBBLE_WITH_CIRCLE = 0x0000F187
            , DRINK = 0x0000F188
            , DRIVE = 0x0000F189
            , DROP = 0x0000F18A
            , DROPBOX = 0x0000F18B
            , EDIT = 0x0000F18C
            , EMAIL = 0x0000F18D
            , EMOJI_FLIRT = 0x0000F18E
            , EMOJI_HAPPY = 0x0000F18F
            , EMOJI_NEUTRAL = 0x0000F190
            , EMOJI_SAD = 0x0000F191
            , ERASE = 0x0000F192
            , ERASER = 0x0000F193
            , EVERNOTE = 0x0000F194
            , EXPORT = 0x0000F195
            , EYE = 0x0000F196
            , EYE_WITH_LINE = 0x0000F197
            , FACEBOOK = 0x0000F198
            , FACEBOOK_WITH_CIRCLE = 0x0000F199
            , FEATHER = 0x0000F19A
            , FINGERPRINT = 0x0000F19B
            , FLAG = 0x0000F19C
            , FLASH = 0x0000F19D
            , FLASHLIGHT = 0x0000F19E
            , FLAT_BRUSH = 0x0000F19F
            , FLATTR = 0x0000F1A0
            , FLICKR = 0x0000F1A1
            , FLICKR_WITH_CIRCLE = 0x0000F1A2
            , FLOW_BRANCH = 0x0000F1A3
            , FLOW_CASCADE = 0x0000F1A4
            , FLOW_LINE = 0x0000F1A5
            , FLOW_PARALLEL = 0x0000F1A6
            , FLOW_TREE = 0x0000F1A7
            , FLOWER = 0x0000F1A8
            , FOLDER = 0x0000F1A9
            , FOLDER_IMAGES = 0x0000F1AA
            , FOLDER_MUSIC = 0x0000F1AB
            , FOLDER_VIDEO = 0x0000F1AC
            , FORWARD = 0x0000F1AD
            , FOURSQUARE = 0x0000F1AE
            , FUNNEL = 0x0000F1AF
            , GAME_CONTROLLER = 0x0000F1B0
            , GAUGE = 0x0000F1B1
            , GITHUB = 0x0000F1B2
            , GITHUB_WITH_CIRCLE = 0x0000F1B3
            , GLOBE = 0x0000F1B4
            , GOOGLE_DRIVE = 0x0000F1B5
            , GOOGLE_HANGOUTS = 0x0000F1B6
            , GOOGLE_PLAY = 0x0000F1B7
            , GOOGLE_PLUS = 0x0000F1B8
            , GOOGLE_PLUS_WITH_CIRCLE = 0x0000F1B9
            , GRADUATION_CAP = 0x0000F1BA
            , GRID = 0x0000F1BB
            , GROOVESHARK = 0x0000F1BC
            , HAIR_CROSS = 0x0000F1BD
            , HAND = 0x0000F1BE
            , HEART = 0x0000F1BF
            , HEART_OUTLINED = 0x0000F1C0
            , HELP = 0x0000F1C1
            , HELP_WITH_CIRCLE = 0x0000F1C2
            , HOME = 0x0000F1C3
            , HOUR_GLASS = 0x0000F1C4
            , HOUZZ = 0x0000F1C5
            , ICLOUD = 0x0000F1C6
            , IMAGE = 0x0000F1C7
            , IMAGE_INVERTED = 0x0000F1C8
            , IMAGES = 0x0000F1C9
            , INBOX = 0x0000F1CA
            , INFINITY = 0x0000F1CB
            , INFO = 0x0000F1CC
            , INFO_WITH_CIRCLE = 0x0000F1CD
            , INSTAGRAM = 0x0000F1CE
            , INSTAGRAM_WITH_CIRCLE = 0x0000F1CF
            , INSTALL = 0x0000F1D0
            , KEY = 0x0000F1D1
            , KEYBOARD = 0x0000F1D2
            , LAB_FLASK = 0x0000F1D3
            , LANDLINE = 0x0000F1D4
            , LANGUAGE = 0x0000F1D5
            , LAPTOP = 0x0000F1D6
            , LASTFM = 0x0000F1D7
            , LASTFM_WITH_CIRCLE = 0x0000F1D8
            , LAYERS = 0x0000F1D9
            , LEAF = 0x0000F1DA
            , LEVEL_DOWN = 0x0000F1DB
            , LEVEL_UP = 0x0000F1DC
            , LIFEBUOY = 0x0000F1DD
            , LIGHT_BULB = 0x0000F1DE
            , LIGHT_DOWN = 0x0000F1DF
            , LIGHT_UP = 0x0000F1E0
            , LINE_GRAPH = 0x0000F1E1
            , LINK = 0x0000F1E2
            , LINKEDIN = 0x0000F1E3
            , LINKEDIN_WITH_CIRCLE = 0x0000F1E4,
        LIST = 0x0000F1E5
            , LOCATION = 0x0000F1E6
            , LOCATION_PIN = 0x0000F1E7
            , LOCK = 0x0000F1E8
            , LOCK_OPEN = 0x0000F1E9
            , LOG_OUT = 0x0000F1EA
            , LOGIN = 0x0000F1EB
            , LOOP = 0x0000F1EC
            , MAGNET = 0x0000F1ED
            , MAGNIFYING_GLASS = 0x0000F1EE
            , MAIL = 0x0000F1EF
            , MAIL_WITH_CIRCLE = 0x0000F1F0
            , MAN = 0x0000F1F1
            , MAP = 0x0000F1F2
            , MASK = 0x0000F1F3
            , MEDAL = 0x0000F1F4
            , MEDIUM = 0x0000F1F5
            , MEDIUM_WITH_CIRCLE = 0x0000F1F6
            , MEGAPHONE = 0x0000F1F7
            , MENU = 0x0000F1F8
            , MERGE = 0x0000F1F9
            , MESSAGE = 0x0000F1FA
            , MIC = 0x0000F1FB
            , MINUS = 0x0000F1FC
            , MIXI = 0x0000F1FD
            , MOBILE = 0x0000F1FE
            , MODERN_MIC = 0x0000F1FF
            , MOON = 0x0000F200
            , MOUSE = 0x0000F201
            , MOUSE_POINTER = 0x0000F202
            , MUSIC = 0x0000F203
            , NETWORK = 0x0000F204
            , NEW = 0x0000F205
            , NEW_MESSAGE = 0x0000F206
            , NEWS = 0x0000F207
            , NEWSLETTER = 0x0000F208
            , NOTE = 0x0000F209
            , NOTIFICATION = 0x0000F20A
            , NOTIFICATIONS_OFF = 0x0000F20B
            , OLD_MOBILE = 0x0000F20C
            , OLD_PHONE = 0x0000F20D
            , ONEDRIVE = 0x0000F20E
            , OPEN_BOOK = 0x0000F20F
            , PALETTE = 0x0000F210
            , PAPER_PLANE = 0x0000F211
            , PAYPAL = 0x0000F212
            , PENCIL = 0x0000F213
            , PHONE = 0x0000F214
            , PICASA = 0x0000F215
            , PIE_CHART = 0x0000F216
            , PIN = 0x0000F217
            , PINTEREST = 0x0000F218
            , PINTEREST_WITH_CIRCLE = 0x0000F219
            , PLUS = 0x0000F21A
            , POPUP = 0x0000F21B
            , POWER_PLUG = 0x0000F21C
            , PRICE_RIBBON = 0x0000F21D
            , PRICE_TAG = 0x0000F21E
            , PRINT = 0x0000F21F
            , PROGRESS_EMPTY = 0x0000F220
            , PROGRESS_FULL = 0x0000F221
            , PROGRESS_ONE = 0x0000F222
            , PROGRESS_TWO = 0x0000F223
            , PUBLISH = 0x0000F224
            , QQ = 0x0000F225
            , QQ_WITH_CIRCLE = 0x0000F226
            , QUOTE = 0x0000F227
            , RADIO = 0x0000F228
            , RAFT = 0x0000F229
            , RAFT_WITH_CIRCLE = 0x0000F22A
            , RAINBOW = 0x0000F22B
            , RDIO = 0x0000F22C
            , RDIO_WITH_CIRCLE = 0x0000F22D
            , REMOVE_USER = 0x0000F22E
            , RENREN = 0x0000F22F
            , REPLY = 0x0000F230
            , REPLY_ALL = 0x0000F231
            , RESIZE_100_PERCENT = 0x0000F232
            , RESIZE_FULL_SCREEN = 0x0000F233,
        RETWEET = 0x0000F234
            , ROCKET = 0x0000F235
            , ROUND_BRUSH = 0x0000F236
            , RSS = 0x0000F237
            , RULER = 0x0000F238
            , SAVE = 0x0000F239
            , SCISSORS = 0x0000F23A
            , SCRIBD = 0x0000F23B
            , SELECT_ARROWS = 0x0000F23C
            , SHARE = 0x0000F23D
            , SHARE_ALTERNATIVE = 0x0000F23E
            , SHAREABLE = 0x0000F23F
            , SHIELD = 0x0000F240
            , SHOP = 0x0000F241
            , SHOPPING_BAG = 0x0000F242
            , SHOPPING_BASKET = 0x0000F243
            , SHOPPING_CART = 0x0000F244
            , SHUFFLE = 0x0000F245
            , SIGNAL = 0x0000F246
            , SINA_WEIBO = 0x0000F247
            , SKYPE = 0x0000F248
            , SKYPE_WITH_CIRCLE = 0x0000F249
            , SLIDESHARE = 0x0000F24A
            , SMASHING = 0x0000F24B
            , SOUND = 0x0000F24C
            , SOUND_MIX = 0x0000F24D
            , SOUND_MUTE = 0x0000F24E
            , SOUNDCLOUD = 0x0000F24F
            , SPORTS_CLUB = 0x0000F250
            , SPOTIFY = 0x0000F251
            , SPOTIFY_WITH_CIRCLE = 0x0000F252
            , SPREADSHEET = 0x0000F253
            , SQUARED_CROSS = 0x0000F254
            , SQUARED_MINUS = 0x0000F255
            , SQUARED_PLUS = 0x0000F256
            , STAR = 0x0000F257
            , STAR_OUTLINED = 0x0000F258
            , STOPWATCH = 0x0000F259
            , STUMBLEUPON = 0x0000F25A
            , STUMBLEUPON_WITH_CIRCLE = 0x0000F25B
            , SUITCASE = 0x0000F25C
            , SWAP = 0x0000F25D
            , SWARM = 0x0000F25E
            , SWEDEN = 0x0000F25F
            , SWITCH = 0x0000F260
            , TABLET = 0x0000F261
            , TABLET_MOBILE_COMBO = 0x0000F262
            , TAG = 0x0000F263
            , TEXT = 0x0000F264
            , TEXT_DOCUMENT = 0x0000F265
            , TEXT_DOCUMENT_INVERTED = 0x0000F266
            , THERMOMETER = 0x0000F267
            , THUMBS_DOWN = 0x0000F268
            , THUMBS_UP = 0x0000F269
            , THUNDER_CLOUD = 0x0000F26A
            , TICKET = 0x0000F26B
            , TIME_SLOT = 0x0000F26C
            , TOOLS = 0x0000F26D
            , TRAFFIC_CONE = 0x0000F26E
            , TRASH = 0x0000F26F
            , TREE = 0x0000F270
            , TRIANGLE_DOWN = 0x0000F271
            , TRIANGLE_LEFT = 0x0000F272
            , TRIANGLE_RIGHT = 0x0000F273
            , TRIANGLE_UP = 0x0000F274
            , TRIPADVISOR = 0x0000F275
            , TROPHY = 0x0000F276
            , TUMBLR = 0x0000F277
            , TUMBLR_WITH_CIRCLE = 0x0000F278
            , TV = 0x0000F279
            , TWITTER = 0x0000F27A
            , TWITTER_WITH_CIRCLE = 0x0000F27B
            , TYPING = 0x0000F27C
            , UNINSTALL = 0x0000F27D
            , UNREAD = 0x0000F27E
            , UNTAG = 0x0000F27F
            , UPLOAD = 0x0000F280
            , UPLOAD_TO_CLOUD = 0x0000F281
            , USER = 0x0000F282
            , USERS = 0x0000F283
            , V_CARD = 0x0000F284
            , VIDEO = 0x0000F285
            , VIDEO_CAMERA = 0x0000F286
            , VIMEO = 0x0000F287
            , VIMEO_WITH_CIRCLE = 0x0000F288
            , VINE = 0x0000F289
            , VINE_WITH_CIRCLE = 0x0000F28A
            , VINYL = 0x0000F28B
            , VK = 0x0000F28C
            , VK_ALTERNITIVE = 0x0000F28D
            , VK_WITH_CIRCLE = 0x0000F28E
            , VOICEMAIL = 0x0000F28F
            , WALLET = 0x0000F290,
        WARNING = 0x0000F291,
        WATER = 0x0000F292,
        WINDOWS_STORE = 0x0000F293,
        XING = 0x0000F294,
        XING_WITH_CIRCLE = 0x0000F295,
        YELP = 0x0000F296,
        YOUKO = 0x0000F297,
        YOUKO_WITH_CIRCLE = 0x0000F298,
        YOUTUBE = 0x0000F299,
        YOUTUBE_WITH_CIRCLE = 0x0000F29A

    }

}
