using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VRage.Ansel;
using VRage.Platform.Windows.Win32;
using VRageMath;

namespace VRage.Platform.Windows.Forms
{
	internal class MyWindowsWindows : IVRageWindows
	{
		[Flags]
		private enum MessageBoxOptionsInternal
		{
			DefButton2 = 0x100,
			DefButton3 = 0x200,
			DefButton4 = 0x300,
			SystemModal = 0x1000,
			TaskModal = 0x2000,
			Help = 0x4000,
			NoFocus = 0x8000,
			SetForeground = 0x10000,
			DefaultDesktopOnly = 0x20000,
			Topmost = 0x40000,
			Right = 0x80000,
			RTLReading = 0x100000
		}

		private MySplashScreen m_splashScreen;

		private Form m_form;

		private readonly MyVRagePlatform m_platform;

		public IVRageWindow Window { get; private set; }

		public MyGameWindow GameWindow { get; private set; }

		public IntPtr WindowHandle { get; private set; }

		public MyWindowsWindows(MyVRagePlatform platform)
		{
			m_platform = platform;
		}

		public void CreateWindow(string gameName, string gameIcon, Type imeCandidateType)
		{
			GameWindow = new MyGameWindow(gameName, gameIcon, imeCandidateType);
			m_form = GameWindow;
			Window = GameWindow;
			m_platform.Input = GameWindow;
			IntPtr intPtr = ((m_platform.Ansel as MyAnsel).WindowHandle = (WindowHandle = m_form.Handle));
		}

		public void CreateToolWindow(IntPtr windowHandle)
		{
			MyToolsWindow myToolsWindow = (MyToolsWindow)(Window = new MyToolsWindow(windowHandle));
			m_platform.Input = myToolsWindow;
			m_form = myToolsWindow.TopLevelForm;
			IntPtr intPtr = ((m_platform.Ansel as MyAnsel).WindowHandle = (WindowHandle = myToolsWindow.Handle));
		}

		public MessageBoxResult MessageBox(string text, string caption, MessageBoxOptions buttons)
		{
			return (MessageBoxResult)MessageBox(WindowHandle, text, caption, (int)(buttons | (MessageBoxOptions)69632));
		}

		public void ShowSplashScreen(string image, Vector2 scale)
		{
			HideSplashScreen();
			m_splashScreen = new MySplashScreen(image, scale);
		}

		public void HideSplashScreen()
		{
			if (m_splashScreen != null)
			{
				m_splashScreen.Close();
				m_splashScreen = null;
			}
		}

		public IntPtr FindWindowInParent(string parent, string child)
		{
			return WinApi.FindWindowInParent(parent, child);
		}

		public void PostMessage(IntPtr handle, uint wm, IntPtr wParam, IntPtr lParam)
		{
			WinApi.PostMessage(handle, wm, wParam, lParam);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern uint MessageBox(IntPtr hWndle, string text, string caption, int buttons);
	}
}
