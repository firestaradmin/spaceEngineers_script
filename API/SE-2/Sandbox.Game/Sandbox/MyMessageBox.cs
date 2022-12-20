using VRage;

namespace Sandbox
{
	public static class MyMessageBox
	{
		public static MessageBoxResult Show(string text, string caption, MessageBoxOptions options)
		{
			return MyVRage.Platform.Windows.MessageBox(text, caption, options);
		}
	}
}
