using System;
using System.Windows.Forms;

namespace VRage.Platform.Windows.IME
{
	public class MyGuiControlIme : TextBox
	{
		private delegate void SwitchIme();

		private SwitchIme activateInner;

		private SwitchIme deactivateInner;

		public bool AutoFocusing { get; set; }

		public MyGuiControlIme()
		{
			activateInner = ActivateDelegate;
			deactivateInner = DeactivateDelegate;
			base.KeyDown += MyGuiControlIme_KeyDown;
			AutoFocusing = false;
		}

		private void MyGuiControlIme_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				e.SuppressKeyPress = true;
				return;
			}
			Keys keyCode = e.KeyCode;
			if (keyCode == Keys.Tab || keyCode == Keys.Return || keyCode == Keys.Escape)
			{
				e.SuppressKeyPress = true;
			}
		}

		public void ActivateInputListening()
		{
			if (MyImeProcessor.Instance != null)
			{
				MyImeProcessor.Instance.GuiImeControl = this;
			}
		}

		private void ActivateDelegate()
		{
			base.ImeMode = ImeMode.On;
		}

		private void DeactivateDelegate()
		{
			base.ImeMode = ImeMode.Disable;
		}

		public void ActivateIme()
		{
			Invoke(activateInner);
		}

		public void DeactivateIme()
		{
			Invoke(deactivateInner);
		}

		protected override void WndProc(ref Message m)
		{
			if (!Focused && base.CanFocus && AutoFocusing && base.Parent.ContainsFocus)
			{
				Focus();
			}
			if (m.Msg != 260 && MyImeProcessor.Instance != null && MyImeProcessor.Instance.WndProc(ref m))
			{
				base.WndProc(ref m);
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			Text = string.Empty;
		}
	}
}
