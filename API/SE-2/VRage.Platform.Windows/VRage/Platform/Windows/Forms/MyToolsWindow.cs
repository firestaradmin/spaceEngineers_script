using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SharpDX.Windows;
using VRageMath;

namespace VRage.Platform.Windows.Forms
{
	internal class MyToolsWindow : IVRageWindow, IVRageInput
	{
		private readonly Control m_control;

		public readonly Form TopLevelForm;

		private RenderLoop m_renderLoop;

		private readonly FastResourceLock m_bufferedCharsLock = new FastResourceLock();

		private List<char> m_bufferedChars = new List<char>();

		private bool m_mouseCapture;

		public bool DrawEnabled => true;

		public IntPtr Handle => m_control.Handle;

		public bool IsActive { get; }

		public Vector2I ClientSize => new Vector2I(m_control.ClientSize.Width, m_control.ClientSize.Height);

		Vector2 IVRageInput.MousePosition
		{
			get
			{
				System.Drawing.Point point = m_control.PointToClient(Cursor.Position);
				return new Vector2(point.X, point.Y);
			}
			set
			{
			}
		}

		Vector2 IVRageInput.MouseAreaSize => new Vector2(m_control.ClientSize.Width, m_control.ClientSize.Height);

		public bool MouseCapture
		{
			get
			{
				return m_mouseCapture;
			}
			set
			{
				m_mouseCapture = value;
				SetMouseCapture(m_mouseCapture);
			}
		}

		public bool ShowCursor { get; set; }

		public int KeyboardDelay => SystemInformation.KeyboardDelay;

		public int KeyboardSpeed => SystemInformation.KeyboardSpeed;

		public event Action OnExit;

		public MyToolsWindow(IntPtr windowHandle)
		{
			m_control = Control.FromHandle(windowHandle);
			TopLevelForm = (Form)m_control.TopLevelControl;
			TopLevelForm.Tag = this;
			TopLevelForm.KeyPress += TopLevelForm_KeyPress;
			TopLevelForm.FormClosed += OnFormClosed;
			TopLevelForm.Activated += TopLevelForm_Activated;
		}

		private void TopLevelForm_Activated(object sender, EventArgs e)
		{
			SetMouseClip(m_mouseCapture);
		}

		private static void TopLevelForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			((MyToolsWindow)((Form)sender).Tag).AddChar(e.KeyChar);
		}

		public void AddChar(char ch)
		{
			m_bufferedChars.Add(ch);
		}

		private void SetMouseClip(bool clip)
		{
			if (clip)
			{
				Cursor.Clip = m_control.RectangleToScreen(m_control.ClientRectangle);
			}
			else
			{
				Cursor.Clip = System.Drawing.Rectangle.Empty;
			}
		}

		public void SetMouseCapture(bool capture)
		{
			SetMouseClip(capture);
			if (capture)
			{
				Cursor.Clip = m_control.RectangleToScreen(m_control.ClientRectangle);
				Cursor.Hide();
			}
			else
			{
				Cursor.Show();
			}
		}

		public void OnModeChanged(MyWindowModeEnum mode, int width, int height, VRageMath.Rectangle desktopBounds)
		{
		}

		public void Show()
		{
		}

		void IVRageWindow.DoEvents()
		{
			Application.DoEvents();
		}

		public void Exit()
		{
		}

		bool IVRageWindow.UpdateRenderThread()
		{
			if (m_renderLoop == null)
			{
				m_renderLoop = new RenderLoop
				{
					UseApplicationDoEvents = false
				};
			}
			return m_renderLoop.NextFrame();
		}

		public void UpdateMainThread()
		{
		}

		public void SetCursor(Stream stream)
		{
		}

		public void AddMessageHandler(uint wm, ActionRef<MyMessage> action)
		{
		}

		public void RemoveMessageHandler(uint wm, ActionRef<MyMessage> action)
		{
		}

		public void ShowAndFocus()
		{
		}

		public void Hide()
		{
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

		void IVRageInput.AddChar(char ch)
		{
			AddChar(ch);
		}

		public void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			this.OnExit.InvokeIfNotNull();
		}
	}
}
