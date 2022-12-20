using System.Threading;
using EmptyKeys.UserInterface.Mvvm;
using VRage;

namespace Sandbox.Graphics
{
	public class MyClipboardService : IClipboardService
	{
		private string m_clipboardText;

		public string GetText()
		{
<<<<<<< HEAD
			Thread thread = new Thread(PasteFromClipboard);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
=======
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Thread val = new Thread((ThreadStart)PasteFromClipboard);
			val.set_ApartmentState(ApartmentState.STA);
			val.Start();
			val.Join();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return m_clipboardText;
		}

		private void PasteFromClipboard()
		{
			m_clipboardText = MyVRage.Platform.System.Clipboard;
		}

		public void SetText(string text)
		{
			MyVRage.Platform.System.Clipboard = text;
		}
	}
}
