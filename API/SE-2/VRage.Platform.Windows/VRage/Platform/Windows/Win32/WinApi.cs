using System;
using System.Runtime.InteropServices;
using VRage.Library.Utils;

namespace VRage.Platform.Windows.Win32
{
	internal static class WinApi
	{
		public enum CtrlType
		{
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT = 1,
			CTRL_CLOSE_EVENT = 2,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT = 6
		}

		[Flags]
		public enum DM
		{
			Orientation = 0x1,
			PaperSize = 0x2,
			PaperLength = 0x4,
			PaperWidth = 0x8,
			Scale = 0x10,
			Position = 0x20,
			NUP = 0x40,
			DisplayOrientation = 0x80,
			Copies = 0x100,
			DefaultSource = 0x200,
			PrintQuality = 0x400,
			Color = 0x800,
			Duplex = 0x1000,
			YResolution = 0x2000,
			TTOption = 0x4000,
			Collate = 0x8000,
			FormName = 0x10000,
			LogPixels = 0x20000,
			BitsPerPixel = 0x40000,
			PelsWidth = 0x80000,
			PelsHeight = 0x100000,
			DisplayFlags = 0x200000,
			DisplayFrequency = 0x400000,
			ICMMethod = 0x800000,
			ICMIntent = 0x1000000,
			MediaType = 0x2000000,
			DitherType = 0x4000000,
			PanningWidth = 0x8000000,
			PanningHeight = 0x10000000,
			DisplayFixedOutput = 0x20000000
		}

		public enum SystemCommands
		{
			SC_SIZE = 61440,
			SC_MOVE = 61456,
			SC_MINIMIZE = 61472,
			SC_MAXIMIZE = 61488,
			SC_MAXIMIZE2 = 61490,
			SC_NEXTWINDOW = 61504,
			SC_PREVWINDOW = 61520,
			SC_CLOSE = 61536,
			SC_VSCROLL = 61552,
			SC_HSCROLL = 61568,
			SC_MOUSEMENU = 61584,
			SC_KEYMENU = 61696,
			SC_ARRANGE = 61712,
			SC_RESTORE = 61728,
			SC_RESTORE2 = 61730,
			SC_TASKLIST = 61744,
			SC_SCREENSAVE = 61760,
			SC_HOTKEY = 61776,
			SC_DEFAULT = 61792,
			SC_MONITORPOWER = 61808,
			SC_CONTEXTHELP = 61824,
			SC_SEPARATOR = 61455
		}

		public enum HookType
		{
			WH_JOURNALRECORD,
			WH_JOURNALPLAYBACK,
			WH_KEYBOARD,
			WH_GETMESSAGE,
			WH_CALLWNDPROC,
			WH_CBT,
			WH_SYSMSGFILTER,
			WH_MOUSE,
			WH_HARDWARE,
			WH_DEBUG,
			WH_SHELL,
			WH_FOREGROUNDIDLE,
			WH_CALLWNDPROCRET,
			WH_KEYBOARD_LL,
			WH_MOUSE_LL
		}

		internal enum MAPVK : uint
		{
			VK_TO_VSC,
			VSC_TO_VK,
			VK_TO_CHAR
		}

		public enum NTSTATUS : uint
		{
			STATUS_SUCCESS = 0u,
			STATUS_TIMER_RESOLUTION_NOT_SET = 3221226053u
		}

		[DontCheck]
		public enum WM
		{
			NULL = 0,
			CREATE = 1,
			DESTROY = 2,
			MOVE = 3,
			SIZE = 5,
			ACTIVATE = 6,
			SETFOCUS = 7,
			KILLFOCUS = 8,
			ENABLE = 10,
			SETREDRAW = 11,
			SETTEXT = 12,
			GETTEXT = 13,
			GETTEXTLENGTH = 14,
			PAINT = 0xF,
			CLOSE = 0x10,
			QUERYENDSESSION = 17,
			QUERYOPEN = 19,
			ENDSESSION = 22,
			QUIT = 18,
			ERASEBKGND = 20,
			SYSCOLORCHANGE = 21,
			SHOWWINDOW = 24,
			WININICHANGE = 26,
			SETTINGCHANGE = 26,
			DEVMODECHANGE = 27,
			ACTIVATEAPP = 28,
			FONTCHANGE = 29,
			TIMECHANGE = 30,
			CANCELMODE = 0x1F,
			SETCURSOR = 0x20,
			MOUSEACTIVATE = 33,
			CHILDACTIVATE = 34,
			QUEUESYNC = 35,
			GETMINMAXINFO = 36,
			PAINTICON = 38,
			ICONERASEBKGND = 39,
			NEXTDLGCTL = 40,
			SPOOLERSTATUS = 42,
			DRAWITEM = 43,
			MEASUREITEM = 44,
			DELETEITEM = 45,
			VKEYTOITEM = 46,
			CHARTOITEM = 47,
			SETFONT = 48,
			GETFONT = 49,
			SETHOTKEY = 50,
			GETHOTKEY = 51,
			QUERYDRAGICON = 55,
			COMPAREITEM = 57,
			GETOBJECT = 61,
			COMPACTING = 65,
			[Obsolete]
			COMMNOTIFY = 68,
			WINDOWPOSCHANGING = 70,
			WINDOWPOSCHANGED = 71,
			[Obsolete]
			POWER = 72,
			COPYDATA = 74,
			CANCELJOURNAL = 75,
			NOTIFY = 78,
			INPUTLANGCHANGEREQUEST = 80,
			INPUTLANGCHANGE = 81,
			TCARD = 82,
			HELP = 83,
			USERCHANGED = 84,
			NOTIFYFORMAT = 85,
			CONTEXTMENU = 123,
			STYLECHANGING = 124,
			STYLECHANGED = 125,
			DISPLAYCHANGE = 126,
			GETICON = 0x7F,
			SETICON = 0x80,
			NCCREATE = 129,
			NCDESTROY = 130,
			NCCALCSIZE = 131,
			NCHITTEST = 132,
			NCPAINT = 133,
			NCACTIVATE = 134,
			GETDLGCODE = 135,
			SYNCPAINT = 136,
			NCMOUSEMOVE = 160,
			NCLBUTTONDOWN = 161,
			NCLBUTTONUP = 162,
			NCLBUTTONDBLCLK = 163,
			NCRBUTTONDOWN = 164,
			NCRBUTTONUP = 165,
			NCRBUTTONDBLCLK = 166,
			NCMBUTTONDOWN = 167,
			NCMBUTTONUP = 168,
			NCMBUTTONDBLCLK = 169,
			NCXBUTTONDOWN = 171,
			NCXBUTTONUP = 172,
			NCXBUTTONDBLCLK = 173,
			INPUT_DEVICE_CHANGE = 254,
			INPUT = 0xFF,
			KEYFIRST = 0x100,
			KEYDOWN = 0x100,
			KEYUP = 257,
			CHAR = 258,
			DEADCHAR = 259,
			SYSKEYDOWN = 260,
			SYSKEYUP = 261,
			SYSCHAR = 262,
			SYSDEADCHAR = 263,
			UNICHAR = 265,
			KEYLAST = 265,
			IME_STARTCOMPOSITION = 269,
			IME_ENDCOMPOSITION = 270,
			IME_COMPOSITION = 271,
			IME_KEYLAST = 271,
			INITDIALOG = 272,
			COMMAND = 273,
			SYSCOMMAND = 274,
			TIMER = 275,
			HSCROLL = 276,
			VSCROLL = 277,
			INITMENU = 278,
			INITMENUPOPUP = 279,
			MENUSELECT = 287,
			MENUCHAR = 288,
			ENTERIDLE = 289,
			MENURBUTTONUP = 290,
			MENUDRAG = 291,
			MENUGETOBJECT = 292,
			UNINITMENUPOPUP = 293,
			MENUCOMMAND = 294,
			CHANGEUISTATE = 295,
			UPDATEUISTATE = 296,
			QUERYUISTATE = 297,
			CTLCOLORMSGBOX = 306,
			CTLCOLOREDIT = 307,
			CTLCOLORLISTBOX = 308,
			CTLCOLORBTN = 309,
			CTLCOLORDLG = 310,
			CTLCOLORSCROLLBAR = 311,
			CTLCOLORSTATIC = 312,
			MOUSEFIRST = 0x200,
			MOUSEMOVE = 0x200,
			LBUTTONDOWN = 513,
			LBUTTONUP = 514,
			LBUTTONDBLCLK = 515,
			RBUTTONDOWN = 516,
			RBUTTONUP = 517,
			RBUTTONDBLCLK = 518,
			MBUTTONDOWN = 519,
			MBUTTONUP = 520,
			MBUTTONDBLCLK = 521,
			MOUSEWHEEL = 522,
			XBUTTONDOWN = 523,
			XBUTTONUP = 524,
			XBUTTONDBLCLK = 525,
			MOUSEHWHEEL = 526,
			MOUSELAST = 526,
			PARENTNOTIFY = 528,
			ENTERMENULOOP = 529,
			EXITMENULOOP = 530,
			NEXTMENU = 531,
			SIZING = 532,
			CAPTURECHANGED = 533,
			MOVING = 534,
			POWERBROADCAST = 536,
			DEVICECHANGE = 537,
			MDICREATE = 544,
			MDIDESTROY = 545,
			MDIACTIVATE = 546,
			MDIRESTORE = 547,
			MDINEXT = 548,
			MDIMAXIMIZE = 549,
			MDITILE = 550,
			MDICASCADE = 551,
			MDIICONARRANGE = 552,
			MDIGETACTIVE = 553,
			MDISETMENU = 560,
			ENTERSIZEMOVE = 561,
			EXITSIZEMOVE = 562,
			DROPFILES = 563,
			MDIREFRESHMENU = 564,
			IME_SETCONTEXT = 641,
			IME_NOTIFY = 642,
			IME_CONTROL = 643,
			IME_COMPOSITIONFULL = 644,
			IME_SELECT = 645,
			IME_CHAR = 646,
			IME_REQUEST = 648,
			IME_KEYDOWN = 656,
			IME_KEYUP = 657,
			MOUSEHOVER = 673,
			MOUSELEAVE = 675,
			NCMOUSEHOVER = 672,
			NCMOUSELEAVE = 674,
			WTSSESSION_CHANGE = 689,
			TABLET_FIRST = 704,
			TABLET_LAST = 735,
			CUT = 768,
			COPY = 769,
			PASTE = 770,
			CLEAR = 771,
			UNDO = 772,
			RENDERFORMAT = 773,
			RENDERALLFORMATS = 774,
			DESTROYCLIPBOARD = 775,
			DRAWCLIPBOARD = 776,
			PAINTCLIPBOARD = 777,
			VSCROLLCLIPBOARD = 778,
			SIZECLIPBOARD = 779,
			ASKCBFORMATNAME = 780,
			CHANGECBCHAIN = 781,
			HSCROLLCLIPBOARD = 782,
			QUERYNEWPALETTE = 783,
			PALETTEISCHANGING = 784,
			PALETTECHANGED = 785,
			HOTKEY = 786,
			PRINT = 791,
			PRINTCLIENT = 792,
			APPCOMMAND = 793,
			THEMECHANGED = 794,
			CLIPBOARDUPDATE = 797,
			DWMCOMPOSITIONCHANGED = 798,
			DWMNCRENDERINGCHANGED = 799,
			DWMCOLORIZATIONCOLORCHANGED = 800,
			DWMWINDOWMAXIMIZEDCHANGE = 801,
			GETTITLEBARINFOEX = 831,
			HANDHELDFIRST = 856,
			HANDHELDLAST = 863,
			AFXFIRST = 864,
			AFXLAST = 895,
			PENWINFIRST = 896,
			PENWINLAST = 911,
			APP = 0x8000,
			USER = 0x400,
			CPL_LAUNCH = 5120,
			CPL_LAUNCHED = 5121,
			SYSTIMER = 280
		}

		public enum GeoTypeEnum
		{
			Nation = 1,
			Latitude,
			Longitude,
			ISO2,
			ISO3,
			Rfc1766,
			Lcid,
			FriendlyName,
			OfficialName,
			Timezones,
			OfficialLanguages,
			ISOUnNumber,
			Parent
		}

		public delegate bool ConsoleEventHandler(CtrlType sig);

		public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		public struct MyCopyData
		{
			public IntPtr Data;

			public int DataSize;

			public IntPtr DataPointer;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct DEVMODE
		{
			public const int CCHDEVICENAME = 32;

			public const int CCHFORMNAME = 32;

			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;

			[FieldOffset(32)]
			public short dmSpecVersion;

			[FieldOffset(34)]
			public short dmDriverVersion;

			[FieldOffset(36)]
			public short dmSize;

			[FieldOffset(38)]
			public short dmDriverExtra;

			[FieldOffset(40)]
			public DM dmFields;

			[FieldOffset(44)]
			private short dmOrientation;

			[FieldOffset(46)]
			private short dmPaperSize;

			[FieldOffset(48)]
			private short dmPaperLength;

			[FieldOffset(50)]
			private short dmPaperWidth;

			[FieldOffset(52)]
			private short dmScale;

			[FieldOffset(54)]
			private short dmCopies;

			[FieldOffset(56)]
			private short dmDefaultSource;

			[FieldOffset(58)]
			private short dmPrintQuality;

			[FieldOffset(44)]
			public POINTL dmPosition;

			[FieldOffset(52)]
			public int dmDisplayOrientation;

			[FieldOffset(56)]
			public int dmDisplayFixedOutput;

			[FieldOffset(60)]
			public short dmColor;

			[FieldOffset(62)]
			public short dmDuplex;

			[FieldOffset(64)]
			public short dmYResolution;

			[FieldOffset(66)]
			public short dmTTOption;

			[FieldOffset(68)]
			public short dmCollate;

			[FieldOffset(72)]
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;

			[FieldOffset(102)]
			public short dmLogPixels;

			[FieldOffset(104)]
			public int dmBitsPerPel;

			[FieldOffset(108)]
			public int dmPelsWidth;

			[FieldOffset(112)]
			public int dmPelsHeight;

			[FieldOffset(116)]
			public int dmDisplayFlags;

			[FieldOffset(116)]
			public int dmNup;

			[FieldOffset(120)]
			public int dmDisplayFrequency;
		}

		public struct POINTL
		{
			public int x;

			public int y;
		}

		public struct POINT
		{
			public int X;

			public int Y;
		}

		public struct MSG
		{
			public IntPtr hwnd;

			public uint message;

			public IntPtr wParam;

			public IntPtr lParam;

			public uint time;

			public POINT pt;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MEMORYSTATUSEX
		{
			public uint dwLength;

			public uint dwMemoryLoad;

			public ulong ullTotalPhys;

			public ulong ullAvailPhys;

			public ulong ullTotalPageFile;

			public ulong ullAvailPageFile;

			public ulong ullTotalVirtual;

			public ulong ullAvailVirtual;

			public ulong ullAvailExtendedVirtual;

			public MEMORYSTATUSEX()
			{
				dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
			}
		}

		public struct DeviceChangeHookStruct
		{
			public int lParam;

			public int wParam;

			public int message;

			public int hwnd;
		}

		public struct KeyboardHookStruct
		{
			public int vkCode;

			public int scanCode;

			public int flags;

			public int time;

			public int dwExtraInfo;
		}

		public const int GW_HWNDFIRST = 0;

		public const int GW_HWNDLAST = 1;

		public const int GW_HWNDNEXT = 2;

		public const int GW_HWNDPREV = 3;

		public const int GW_OWNER = 4;

		public const int GW_CHILD = 5;

		public const int GW_ENABLEDPOPUP = 6;

		public const int ENUM_CURRENT_SETTINGS = -1;

		public const int ENUM_REGISTRY_SETTINGS = -2;

		public const int MF_BYPOSITION = 1024;

		public const int MF_REMOVE = 4096;

		public const int WM_DEVICECHANGE = 537;

		public const int WM_KEYDOWN = 256;

		public const int WM_KEYUP = 257;

		public const int WM_SYSKEYDOWN = 260;

		public const int WM_SYSKEYUP = 261;

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref MyCopyData lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern int ShowCursor(bool bVisible);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AllocConsole();

		[DllImport("Kernel32")]
		public static extern bool SetConsoleCtrlHandler(ConsoleEventHandler handler, bool add);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GlobalMemoryStatusEx([In][Out] MEMORYSTATUSEX lpBuffer);

		[DllImport("user32.dll")]
		public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

		[DllImport("user32.dll")]
		public static extern bool TranslateMessage([In] ref MSG lpMsg);

		[DllImport("user32.dll")]
		public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox(IntPtr hWndle, string text, string caption, int buttons);

		[DllImport("user32.dll")]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr LoadKeyboardLayout(string keyboardLayoutID, uint flags);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool UnloadKeyboardLayout(IntPtr handle);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetKeyboardLayout(IntPtr threadId);

		[DllImport("kernel32")]
		public static extern bool SetProcessWorkingSetSize(IntPtr handle, int minSize, int maxSize);

		[DllImport("kernel32.dll")]
		public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string pName);

		[DllImport("ntdll.dll")]
		public static extern NTSTATUS NtQueryTimerResolution(ref uint MinimumResolution, ref uint MaximumResolution, ref uint CurrentResolution);

		[DllImport("ntdll.dll")]
		public static extern NTSTATUS NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);

		public unsafe static string GetGeoInfo(GeoTypeEnum geoType)
		{
			char* ptr = stackalloc char[256];
			if (GetGeoInfoW(GetUserGeoID(16), (int)geoType, ptr, 256, 0) <= 0)
			{
				return string.Empty;
			}
			return Marshal.PtrToStringUni(new IntPtr(ptr));
		}

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern int GetUserGeoID(int c);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private unsafe static extern int GetGeoInfoW(int location, int geoType, char* str, int strSize, short langId);

		public static IntPtr FindWindowInParent(string parentName, string childName)
		{
			IntPtr intPtr = FindWindow(null, parentName);
			if (intPtr != IntPtr.Zero)
			{
				if (childName == null)
				{
					return intPtr;
				}
				return FindChildWindow(intPtr, childName);
			}
			return IntPtr.Zero;
		}

		public static IntPtr FindChildWindow(IntPtr windowHandle, string childName)
		{
			IntPtr intPtr = FindWindowEx(windowHandle, IntPtr.Zero, null, childName);
			windowHandle.ToInt32();
			if (intPtr != IntPtr.Zero)
			{
				return intPtr;
			}
			IntPtr intPtr2 = IntPtr.Zero;
			intPtr = GetWindow(windowHandle, 5u);
			while (intPtr2 != intPtr && intPtr != IntPtr.Zero)
			{
				if (intPtr2 == IntPtr.Zero)
				{
					intPtr2 = intPtr;
				}
				IntPtr intPtr3 = FindChildWindow(intPtr, childName);
				if (intPtr3 != IntPtr.Zero)
				{
					return intPtr3;
				}
				intPtr = GetWindow(intPtr, 2u);
			}
			return IntPtr.Zero;
		}
	}
}
