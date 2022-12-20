using System;
using VRageMath;

namespace VRage
{
	public interface IVRageWindows
	{
		IVRageWindow Window { get; }

		void CreateWindow(string gameName, string gameIcon, Type imeCandidateType);

		void CreateToolWindow(IntPtr windowHandle);

		MessageBoxResult MessageBox(string text, string caption, MessageBoxOptions options);

		void ShowSplashScreen(string image, Vector2 scale);

		void HideSplashScreen();

		IntPtr FindWindowInParent(string parent, string child);

		void PostMessage(IntPtr handle, uint wm, IntPtr wParam, IntPtr lParam);
	}
}
