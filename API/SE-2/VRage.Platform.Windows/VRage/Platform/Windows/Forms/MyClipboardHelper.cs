using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace VRage.Platform.Windows.Forms
{
	public static class MyClipboardHelper
	{
		public static void SetClipboard(string text)
		{
			Thread thread = new Thread(delegate(object arg)
			{
				for (int i = 0; i < 10; i++)
				{
					try
					{
						Clipboard.Clear();
						Clipboard.SetText((string)arg);
						return;
					}
					catch (ExternalException)
					{
					}
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start(text);
			thread.Join();
		}

		public static string GetClipboardText()
		{
			return Clipboard.GetText();
		}
	}
}
