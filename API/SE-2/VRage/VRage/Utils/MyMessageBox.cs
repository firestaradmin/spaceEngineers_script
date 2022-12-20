namespace VRage.Utils
{
	/// <summary>
	/// Custom message box
	/// </summary>
	public static class MyMessageBox
	{
		public static void Show(string caption, string text)
		{
			MyVRage.Platform.Windows.MessageBox(text, caption, MessageBoxOptions.OkOnly);
		}
	}
}
