using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Config
{
    public enum VirtualKeyboardKey
    {
        NONE = 0,
        //
        // Zusammenfassung:
        //     Left mouse button
        LBUTTON = 1,
        //
        // Zusammenfassung:
        //     Right mouse button
        RBUTTON = 2,
        //
        // Zusammenfassung:
        //     Control-break processing
        CANCEL = 3,
        //
        // Zusammenfassung:
        //     Middle mouse button (three-button mouse) - NOT contiguous with LBUTTON and RBUTTON
        MBUTTON = 4,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: X1 mouse button - NOT contiguous with LBUTTON and RBUTTON
        XBUTTON1 = 5,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: X2 mouse button - NOT contiguous with LBUTTON and RBUTTON
        XBUTTON2 = 6,
        //
        // Zusammenfassung:
        //     BACKSPACE key
        BACK = 8,
        //
        // Zusammenfassung:
        //     TAB key
        TAB = 9,
        //
        // Zusammenfassung:
        //     CLEAR key
        CLEAR = 12,
        //
        // Zusammenfassung:
        //     ENTER key
        RETURN = 13,
        //
        // Zusammenfassung:
        //     SHIFT key
        SHIFT = 0x10,
        //
        // Zusammenfassung:
        //     CTRL key
        CONTROL = 17,
        //
        // Zusammenfassung:
        //     ALT key
        MENU = 18,
        //
        // Zusammenfassung:
        //     PAUSE key
        PAUSE = 19,
        //
        // Zusammenfassung:
        //     CAPS LOCK key
        CAPITAL = 20,
        //
        // Zusammenfassung:
        //     Input Method Editor (IME) Kana mode
        KANA = 21,
        //
        // Zusammenfassung:
        //     IME Hanguel mode (maintained for compatibility; use HANGUL)
        HANGEUL = 21,
        //
        // Zusammenfassung:
        //     IME Hangul mode
        HANGUL = 21,
        //
        // Zusammenfassung:
        //     IME Junja mode
        JUNJA = 23,
        //
        // Zusammenfassung:
        //     IME final mode
        FINAL = 24,
        //
        // Zusammenfassung:
        //     IME Hanja mode
        HANJA = 25,
        //
        // Zusammenfassung:
        //     IME Kanji mode
        KANJI = 25,
        //
        // Zusammenfassung:
        //     ESC key
        ESCAPE = 27,
        //
        // Zusammenfassung:
        //     IME convert
        CONVERT = 28,
        //
        // Zusammenfassung:
        //     IME nonconvert
        NONCONVERT = 29,
        //
        // Zusammenfassung:
        //     IME accept
        ACCEPT = 30,
        //
        // Zusammenfassung:
        //     IME mode change request
        MODECHANGE = 0x1F,
        //
        // Zusammenfassung:
        //     SPACEBAR
        SPACE = 0x20,
        //
        // Zusammenfassung:
        //     PAGE UP key
        PRIOR = 33,
        //
        // Zusammenfassung:
        //     PAGE DOWN key
        NEXT = 34,
        //
        // Zusammenfassung:
        //     END key
        END = 35,
        //
        // Zusammenfassung:
        //     HOME key
        HOME = 36,
        //
        // Zusammenfassung:
        //     LEFT ARROW key
        LEFT = 37,
        //
        // Zusammenfassung:
        //     UP ARROW key
        UP = 38,
        //
        // Zusammenfassung:
        //     RIGHT ARROW key
        RIGHT = 39,
        //
        // Zusammenfassung:
        //     DOWN ARROW key
        DOWN = 40,
        //
        // Zusammenfassung:
        //     SELECT key
        SELECT = 41,
        //
        // Zusammenfassung:
        //     PRINT key
        PRINT = 42,
        //
        // Zusammenfassung:
        //     EXECUTE key
        EXECUTE = 43,
        //
        // Zusammenfassung:
        //     PRINT SCREEN key
        SNAPSHOT = 44,
        //
        // Zusammenfassung:
        //     INS key
        INSERT = 45,
        //
        // Zusammenfassung:
        //     DEL key
        DELETE = 46,
        //
        // Zusammenfassung:
        //     HELP key
        HELP = 47,
        //
        // Zusammenfassung:
        //     0 key
        VK_0 = 48,
        //
        // Zusammenfassung:
        //     1 key
        VK_1 = 49,
        //
        // Zusammenfassung:
        //     2 key
        VK_2 = 50,
        //
        // Zusammenfassung:
        //     3 key
        VK_3 = 51,
        //
        // Zusammenfassung:
        //     4 key
        VK_4 = 52,
        //
        // Zusammenfassung:
        //     5 key
        VK_5 = 53,
        //
        // Zusammenfassung:
        //     6 key
        VK_6 = 54,
        //
        // Zusammenfassung:
        //     7 key
        VK_7 = 55,
        //
        // Zusammenfassung:
        //     8 key
        VK_8 = 56,
        //
        // Zusammenfassung:
        //     9 key
        VK_9 = 57,
        //
        // Zusammenfassung:
        //     A key
        VK_A = 65,
        //
        // Zusammenfassung:
        //     B key
        VK_B = 66,
        //
        // Zusammenfassung:
        //     C key
        VK_C = 67,
        //
        // Zusammenfassung:
        //     D key
        VK_D = 68,
        //
        // Zusammenfassung:
        //     E key
        VK_E = 69,
        //
        // Zusammenfassung:
        //     F key
        VK_F = 70,
        //
        // Zusammenfassung:
        //     G key
        VK_G = 71,
        //
        // Zusammenfassung:
        //     H key
        VK_H = 72,
        //
        // Zusammenfassung:
        //     I key
        VK_I = 73,
        //
        // Zusammenfassung:
        //     J key
        VK_J = 74,
        //
        // Zusammenfassung:
        //     K key
        VK_K = 75,
        //
        // Zusammenfassung:
        //     L key
        VK_L = 76,
        //
        // Zusammenfassung:
        //     M key
        VK_M = 77,
        //
        // Zusammenfassung:
        //     N key
        VK_N = 78,
        //
        // Zusammenfassung:
        //     O key
        VK_O = 79,
        //
        // Zusammenfassung:
        //     P key
        VK_P = 80,
        //
        // Zusammenfassung:
        //     Q key
        VK_Q = 81,
        //
        // Zusammenfassung:
        //     R key
        VK_R = 82,
        //
        // Zusammenfassung:
        //     S key
        VK_S = 83,
        //
        // Zusammenfassung:
        //     T key
        VK_T = 84,
        //
        // Zusammenfassung:
        //     U key
        VK_U = 85,
        //
        // Zusammenfassung:
        //     V key
        VK_V = 86,
        //
        // Zusammenfassung:
        //     W key
        VK_W = 87,
        //
        // Zusammenfassung:
        //     X key
        VK_X = 88,
        //
        // Zusammenfassung:
        //     Y key
        VK_Y = 89,
        //
        // Zusammenfassung:
        //     Z key
        VK_Z = 90,
        //
        // Zusammenfassung:
        //     Left Windows key (Microsoft Natural keyboard)
        LWIN = 91,
        //
        // Zusammenfassung:
        //     Right Windows key (Natural keyboard)
        RWIN = 92,
        //
        // Zusammenfassung:
        //     Applications key (Natural keyboard)
        APPS = 93,
        //
        // Zusammenfassung:
        //     Computer Sleep key
        SLEEP = 95,
        //
        // Zusammenfassung:
        //     Numeric keypad 0 key
        NUMPAD0 = 96,
        //
        // Zusammenfassung:
        //     Numeric keypad 1 key
        NUMPAD1 = 97,
        //
        // Zusammenfassung:
        //     Numeric keypad 2 key
        NUMPAD2 = 98,
        //
        // Zusammenfassung:
        //     Numeric keypad 3 key
        NUMPAD3 = 99,
        //
        // Zusammenfassung:
        //     Numeric keypad 4 key
        NUMPAD4 = 100,
        //
        // Zusammenfassung:
        //     Numeric keypad 5 key
        NUMPAD5 = 101,
        //
        // Zusammenfassung:
        //     Numeric keypad 6 key
        NUMPAD6 = 102,
        //
        // Zusammenfassung:
        //     Numeric keypad 7 key
        NUMPAD7 = 103,
        //
        // Zusammenfassung:
        //     Numeric keypad 8 key
        NUMPAD8 = 104,
        //
        // Zusammenfassung:
        //     Numeric keypad 9 key
        NUMPAD9 = 105,
        //
        // Zusammenfassung:
        //     Multiply key
        MULTIPLY = 106,
        //
        // Zusammenfassung:
        //     Add key
        ADD = 107,
        //
        // Zusammenfassung:
        //     Separator key
        SEPARATOR = 108,
        //
        // Zusammenfassung:
        //     Subtract key
        SUBTRACT = 109,
        //
        // Zusammenfassung:
        //     Decimal key
        DECIMAL = 110,
        //
        // Zusammenfassung:
        //     Divide key
        DIVIDE = 111,
        //
        // Zusammenfassung:
        //     F1 key
        F1 = 112,
        //
        // Zusammenfassung:
        //     F2 key
        F2 = 113,
        //
        // Zusammenfassung:
        //     F3 key
        F3 = 114,
        //
        // Zusammenfassung:
        //     F4 key
        F4 = 115,
        //
        // Zusammenfassung:
        //     F5 key
        F5 = 116,
        //
        // Zusammenfassung:
        //     F6 key
        F6 = 117,
        //
        // Zusammenfassung:
        //     F7 key
        F7 = 118,
        //
        // Zusammenfassung:
        //     F8 key
        F8 = 119,
        //
        // Zusammenfassung:
        //     F9 key
        F9 = 120,
        //
        // Zusammenfassung:
        //     F10 key
        F10 = 121,
        //
        // Zusammenfassung:
        //     F11 key
        F11 = 122,
        //
        // Zusammenfassung:
        //     F12 key
        F12 = 123,
        //
        // Zusammenfassung:
        //     F13 key
        F13 = 124,
        //
        // Zusammenfassung:
        //     F14 key
        F14 = 125,
        //
        // Zusammenfassung:
        //     F15 key
        F15 = 126,
        //
        // Zusammenfassung:
        //     F16 key
        F16 = 0x7F,
        //
        // Zusammenfassung:
        //     F17 key
        F17 = 0x80,
        //
        // Zusammenfassung:
        //     F18 key
        F18 = 129,
        //
        // Zusammenfassung:
        //     F19 key
        F19 = 130,
        //
        // Zusammenfassung:
        //     F20 key
        F20 = 131,
        //
        // Zusammenfassung:
        //     F21 key
        F21 = 132,
        //
        // Zusammenfassung:
        //     F22 key
        F22 = 133,
        //
        // Zusammenfassung:
        //     F23 key
        F23 = 134,
        //
        // Zusammenfassung:
        //     F24 key
        F24 = 135,
        //
        // Zusammenfassung:
        //     NUM LOCK key
        NUMLOCK = 144,
        //
        // Zusammenfassung:
        //     SCROLL LOCK key
        SCROLL = 145,
        //
        // Zusammenfassung:
        //     Left SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        LSHIFT = 160,
        //
        // Zusammenfassung:
        //     Right SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        RSHIFT = 161,
        //
        // Zusammenfassung:
        //     Left CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        LCONTROL = 162,
        //
        // Zusammenfassung:
        //     Right CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        RCONTROL = 163,
        //
        // Zusammenfassung:
        //     Left MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        LMENU = 164,
        //
        // Zusammenfassung:
        //     Right MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        RMENU = 165,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Browser Back key
        BROWSER_BACK = 166,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Browser Forward key
        BROWSER_FORWARD = 167,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Browser Refresh key
        BROWSER_REFRESH = 168,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Browser Stop key
        BROWSER_STOP = 169,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Browser Search key
        BROWSER_SEARCH = 170,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Browser Favorites key
        BROWSER_FAVORITES = 171,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Browser Start and Home key
        BROWSER_HOME = 172,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Volume Mute key
        VOLUME_MUTE = 173,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Volume Down key
        VOLUME_DOWN = 174,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Volume Up key
        VOLUME_UP = 175,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Next Track key
        MEDIA_NEXT_TRACK = 176,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Previous Track key
        MEDIA_PREV_TRACK = 177,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Stop Media key
        MEDIA_STOP = 178,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Play/Pause Media key
        MEDIA_PLAY_PAUSE = 179,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Start Mail key
        LAUNCH_MAIL = 180,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Select Media key
        LAUNCH_MEDIA_SELECT = 181,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Start Application 1 key
        LAUNCH_APP1 = 182,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Start Application 2 key
        LAUNCH_APP2 = 183,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the ';:' key
        OEM_1 = 186,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: For any country/region, the '+' key
        OEM_PLUS = 187,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: For any country/region, the ',' key
        OEM_COMMA = 188,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: For any country/region, the '-' key
        OEM_MINUS = 189,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: For any country/region, the '.' key
        OEM_PERIOD = 190,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '/?' key
        OEM_2 = 191,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '`~' key
        OEM_3 = 192,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '[{' key
        OEM_4 = 219,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '\|' key
        OEM_5 = 220,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the ']}' key
        OEM_6 = 221,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the 'single-quote/double-quote' key
        OEM_7 = 222,
        //
        // Zusammenfassung:
        //     Used for miscellaneous characters; it can vary by keyboard.
        OEM_8 = 223,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Either the angle bracket key or the backslash key on the RT
        //     102-key keyboard
        OEM_102 = 226,
        //
        // Zusammenfassung:
        //     Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
        PROCESSKEY = 229,
        //
        // Zusammenfassung:
        //     Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
        //     The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard
        //     input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN,
        //     and WM_KEYUP
        PACKET = 231,
        //
        // Zusammenfassung:
        //     Attn key
        ATTN = 246,
        //
        // Zusammenfassung:
        //     CrSel key
        CRSEL = 247,
        //
        // Zusammenfassung:
        //     ExSel key
        EXSEL = 248,
        //
        // Zusammenfassung:
        //     Erase EOF key
        EREOF = 249,
        //
        // Zusammenfassung:
        //     Play key
        PLAY = 250,
        //
        // Zusammenfassung:
        //     Zoom key
        ZOOM = 251,
        //
        // Zusammenfassung:
        //     Reserved
        NONAME = 252,
        //
        // Zusammenfassung:
        //     PA1 key
        PA1 = 253,
        //
        // Zusammenfassung:
        //     Clear key
        OEM_CLEAR = 254
    }
}
