using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpDX;
using VRage.Input;
using VRage.Platform.Windows.IME;
using VRage.Platform.Windows.Render;
using VRage.Platform.Windows.Win32;
using VRageMath;
using VRageRender;

namespace VRage.Platform.Windows.Forms
{
	internal abstract class MyGameForm : Form, IMessageFilter, IVRageInput
	{
		private bool allowUserResizing;

		private MyGuiControlIme m_ImeControl;

		private MouseEventArgs m_emptyMouseEventArgs = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);

		private FastResourceLock m_bufferedCharsLock = new FastResourceLock();

		private List<char> m_bufferedChars = new List<char>();

		private Vector2 m_mousePosition;

		private bool m_mouseCapture;

		public HashSet<int> BypassedMessages { get; private set; }

		internal bool AllowUserResizing
		{
			get
			{
				return allowUserResizing;
			}
			set
			{
				if (allowUserResizing != value)
				{
					allowUserResizing = value;
					base.MaximizeBox = allowUserResizing;
					base.FormBorderStyle = ((!allowUserResizing) ? FormBorderStyle.FixedSingle : FormBorderStyle.Sizable);
				}
			}
		}

		public abstract bool ShowCursor { get; set; }

		public int KeyboardDelay => SystemInformation.KeyboardDelay;

		public int KeyboardSpeed => SystemInformation.KeyboardSpeed;

		Vector2 IVRageInput.MousePosition
		{
			get
			{
				return m_mousePosition;
			}
			set
			{
				Invoke((Action)delegate
				{
					Cursor.Position = PointToScreen(new System.Drawing.Point((int)value.X, (int)value.Y));
				});
				m_mousePosition = value;
			}
		}

		Vector2 IVRageInput.MouseAreaSize => new Vector2(base.ClientSize.Width, base.ClientSize.Height);

		public bool MouseCapture
		{
			get
			{
				return m_mouseCapture;
			}
			set
			{
				m_mouseCapture = value;
				Invoke(new Action(UpdateClip));
			}
		}

		protected void Init(string text, Type imeCandidateType)
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, value: true);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			BypassedMessages = new HashSet<int>();
			if (imeCandidateType != null)
			{
				InitializeIME(imeCandidateType);
			}
			else
			{
				BypassedMessages.Add(642);
				BypassedMessages.Add(6);
			}
			BypassedMessages.Add(256);
			BypassedMessages.Add(257);
			BypassedMessages.Add(258);
			BypassedMessages.Add(259);
			BypassedMessages.Add(260);
			BypassedMessages.Add(261);
			BypassedMessages.Add(262);
			BypassedMessages.Add(263);
			BypassedMessages.Add(522);
			BypassedMessages.Add(512);
			BypassedMessages.Add(513);
			BypassedMessages.Add(514);
			BypassedMessages.Add(515);
			BypassedMessages.Add(516);
			BypassedMessages.Add(517);
			BypassedMessages.Add(518);
			BypassedMessages.Add(519);
			BypassedMessages.Add(520);
			BypassedMessages.Add(521);
			BypassedMessages.Add(525);
			BypassedMessages.Add(523);
			BypassedMessages.Add(524);
			BypassedMessages.Add(20);
			BypassedMessages.Add(24);
			BypassedMessages.Add(7);
			BypassedMessages.Add(8);
		}

		public void OnDeviceChangedInternal(ref MyMessage msg)
		{
			MyInput.Static.DeviceChangeCallback();
		}

		private void InitializeIME(Type imeCandidateType)
		{
			MyImeProcessor.CreateInstance(imeCandidateType);
			base.ImeMode = ImeMode.On;
			m_ImeControl = new MyGuiControlIme();
			base.Controls.Add(m_ImeControl);
			m_ImeControl.ActivateInputListening();
			m_ImeControl.Size = new Size(0, 10);
			m_ImeControl.AutoFocusing = true;
		}

		private void FocusIme()
		{
			if (m_ImeControl != null)
			{
				base.ActiveControl = m_ImeControl;
				Action method = delegate
				{
					m_ImeControl.Focus();
				};
				m_ImeControl.Invoke(method);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			MessageFilterHook.AddMessageFilter(base.Handle, this);
			base.OnLoad(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			MessageFilterHook.RemoveMessageFilter(base.Handle, this);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 260)
			{
				return;
			}
			if (m.Msg == 258)
			{
				char item = (char)(int)m.WParam;
				using (m_bufferedCharsLock.AcquireExclusiveUsing())
				{
					m_bufferedChars.Add(item);
				}
				return;
			}
			if (m.Msg == 512)
			{
				m_mousePosition.X = (short)(long)m.LParam;
				m_mousePosition.Y = (short)((long)m.LParam >> 16);
			}
			if (m.Msg == 675)
			{
				m_mousePosition = -Vector2.One;
			}
			base.WndProc(ref m);
		}

		void IVRageInput.AddChar(char ch)
		{
			using (m_bufferedCharsLock.AcquireExclusiveUsing())
			{
				m_bufferedChars.Add(ch);
			}
		}

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == 512)
			{
				return false;
			}
			if (m.Msg == 258)
			{
				return false;
			}
			if (m.Msg == 260)
			{
				return true;
			}
			if (m.Msg == 261)
			{
				return true;
			}
			if (m.Msg == 262)
			{
				return true;
			}
			if (m.Msg == 263)
			{
				return true;
			}
			if (m.Msg == 257)
			{
				return true;
			}
			if (m.Msg == 256)
			{
				return true;
			}
			if (m.Msg == 164)
			{
				return true;
			}
			if (m.Msg == 6)
			{
				if (m.WParam == (IntPtr)0)
				{
					base.TopMost = false;
				}
				if (MyRenderProxy.RenderThread.CurrentSettings.WindowMode == MyWindowModeEnum.Fullscreen)
				{
					if (m.WParam == (IntPtr)0)
					{
						base.WindowState = FormWindowState.Minimized;
					}
					else
					{
						System.Drawing.Rectangle workingArea = Screen.FromControl(this).WorkingArea;
						base.Location = new System.Drawing.Point
						{
							X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - base.Width) / 2),
							Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - base.Height) / 2)
						};
					}
				}
			}
			if (m.Msg == 7)
			{
				MyPlatformRender.HandleFocusMessage(MyWindowFocusMessage.SetFocus);
			}
			if (BypassedMessages.Contains(m.Msg))
			{
				if (m.Msg == 6)
				{
					if (m.WParam == IntPtr.Zero)
					{
						OnDeactivate(EventArgs.Empty);
					}
					else
					{
						OnActivated(EventArgs.Empty);
					}
				}
				if (m.Msg == 512)
				{
					OnMouseMove(m_emptyMouseEventArgs);
				}
				m.Result = WinApi.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
				return true;
			}
			return false;
		}

		public void GetBufferedTextInput(ref List<char> swappedBuffer)
		{
			swappedBuffer.Clear();
			using (m_bufferedCharsLock.AcquireExclusiveUsing())
			{
				List<char> bufferedChars = swappedBuffer;
				swappedBuffer = m_bufferedChars;
				m_bufferedChars = bufferedChars;
			}
		}

		public abstract void UpdateClip();
	}
}
