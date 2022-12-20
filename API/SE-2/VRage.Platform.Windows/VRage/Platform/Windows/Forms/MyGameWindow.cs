using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using SharpDX.Windows;
using VRage.FileSystem;
using VRage.Platform.Windows.IME;
using VRage.Platform.Windows.Win32;
using VRageMath;

namespace VRage.Platform.Windows.Forms
{
	internal sealed class MyGameWindow : MyGameForm, IVRageWindow
	{
		private bool m_showCursor = true;

		private int m_isCursorVisible = 1;

		private const int INITIAL_QUEUE_CAPACITY = 64;

		private RenderLoop m_renderLoop;

		private bool m_isActive;

		private readonly Dictionary<uint, ActionRef<MyMessage>> m_messageDictionary;

		private readonly Queue<Message> m_messageQueue;

		private readonly Queue<WinApi.MyCopyData> m_messageCopyDataQueue;

		private readonly List<Message> m_tmpMessages;

		private readonly List<WinApi.MyCopyData> m_tmpCopyData;

		private bool IsCursorVisible
		{
			get
			{
				return m_isCursorVisible > 0;
			}
			set
			{
				if (value)
				{
					if (Interlocked.Exchange(ref m_isCursorVisible, 1) == 0)
					{
						Invoke(new Action(Cursor.Show));
					}
				}
				else if (!value && Interlocked.Exchange(ref m_isCursorVisible, 0) == 1)
				{
					Invoke(new Action(Cursor.Hide));
				}
			}
		}

		public bool IsActive
		{
			get
			{
				if (m_isActive)
				{
					return Form.ActiveForm == this;
				}
				return false;
			}
		}

		public new Vector2I ClientSize => new Vector2I(base.ClientSize.Width, base.ClientSize.Height);

		public override bool ShowCursor
		{
			get
			{
				return m_showCursor;
			}
			set
			{
				if (m_showCursor != value)
				{
					m_showCursor = value;
					IsCursorVisible = value;
				}
			}
		}

		public bool DrawEnabled => base.WindowState != FormWindowState.Minimized;

		public event Action OnExit;

		internal MyGameWindow(string gameName, string gameIcon, Type imeCandidateType)
		{
			Text = gameName;
			m_messageDictionary = new Dictionary<uint, ActionRef<MyMessage>>();
			m_tmpMessages = new List<Message>(64);
			m_tmpCopyData = new List<WinApi.MyCopyData>(64);
			m_messageQueue = new Queue<Message>(64);
			m_messageCopyDataQueue = new Queue<WinApi.MyCopyData>(64);
			Init("VRage", imeCandidateType);
			AddMessageHandler(1054u, OnToolIsGameRunningMessage);
			AddMessageHandler(537u, base.OnDeviceChangedInternal);
			try
			{
				base.Icon = new Icon(Path.Combine(MyFileSystem.ExePath, gameIcon));
			}
			catch (FileNotFoundException)
			{
				base.Icon = null;
			}
			base.FormClosing += OnFormClosing;
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
			}
			this.OnExit.InvokeIfNotNull();
		}

		private void OnToolIsGameRunningMessage(ref MyMessage msg)
		{
			IntPtr wParam = new IntPtr((!MyVRage.Platform.SessionReady) ? 1 : 0);
			WinApi.PostMessage(msg.WParam, 1055u, wParam, IntPtr.Zero);
		}

		protected override void OnActivated(EventArgs e)
		{
			if (!m_isActive)
			{
				m_isActive = true;
				if (!ShowCursor)
				{
					IsCursorVisible = false;
				}
			}
			base.OnActivated(e);
		}

		protected override void OnDeactivate(EventArgs e)
		{
			if (m_isActive)
			{
				m_isActive = false;
				ClearClip();
				if (!IsCursorVisible)
				{
					IsCursorVisible = true;
				}
			}
			base.OnDeactivate(e);
		}

		protected override void OnResizeBegin(EventArgs e)
		{
			ClearClip();
			base.OnResizeBegin(e);
		}

		private void SetClip()
		{
			Cursor.Clip = RectangleToScreen(base.ClientRectangle);
		}

		private static void ClearClip()
		{
			Cursor.Clip = System.Drawing.Rectangle.Empty;
		}

		public override void UpdateClip()
		{
			if (!base.IsDisposed)
			{
				if (IsActive && (base.MouseCapture || !IsCursorVisible))
				{
					SetClip();
				}
				else
				{
					ClearClip();
				}
			}
		}

		public void OnModeChanged(MyWindowModeEnum mode, int width, int height, VRageMath.Rectangle desktopBounds)
		{
			switch (mode)
			{
			case MyWindowModeEnum.Window:
				base.FormBorderStyle = FormBorderStyle.FixedSingle;
				base.TopMost = false;
				base.ClientSize = new Size(width, height);
				base.Location = new System.Drawing.Point(desktopBounds.X + (desktopBounds.Width - base.ClientSize.Width) / 2, desktopBounds.Y + (desktopBounds.Height - base.ClientSize.Height) / 2);
				break;
			case MyWindowModeEnum.FullscreenWindow:
				base.FormBorderStyle = FormBorderStyle.None;
				base.TopMost = true;
				base.SizeGripStyle = SizeGripStyle.Hide;
				base.ClientSize = new Size(desktopBounds.Width, desktopBounds.Height);
				base.Location = new System.Drawing.Point(desktopBounds.Left, desktopBounds.Top);
				break;
			case MyWindowModeEnum.Fullscreen:
				base.TopMost = true;
				base.FormBorderStyle = FormBorderStyle.None;
				break;
			}
			UpdateClip();
		}

		public void ShowAndFocus()
		{
			Invoke((Action)delegate
			{
				Show();
				Focus();
			});
		}

		void IVRageWindow.DoEvents()
		{
			Application.DoEvents();
		}

		void IVRageWindow.Exit()
		{
			if (!base.IsDisposed)
			{
				Invoke(new Action(Dispose));
			}
		}

		bool IVRageWindow.UpdateRenderThread()
		{
			UpdateClip();
			if (m_renderLoop == null)
			{
				m_renderLoop = new RenderLoop(this)
				{
					UseApplicationDoEvents = false
				};
			}
			return m_renderLoop.NextFrame();
		}

		void IVRageWindow.Hide()
		{
			try
			{
				if (!base.IsDisposed)
				{
					Invoke(new Action(SafeHide));
				}
			}
			catch
			{
			}
		}

		private void SafeHide()
		{
			try
			{
				if (!base.IsDisposed)
				{
					Hide();
				}
			}
			catch
			{
			}
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			base.ClientSize = new System.Drawing.Size(284, 262);
			base.Name = "MyGameWindow";
			ResumeLayout(false);
		}

		protected override void WndProc(ref Message m)
		{
			bool flag = true;
			switch (m.Msg)
			{
			case 81:
				if (MyImeProcessor.Instance != null)
				{
					MyImeProcessor.Instance.LanguageChanged();
				}
				break;
			case 269:
			case 270:
			case 271:
			case 641:
			case 642:
			case 643:
			case 644:
			case 645:
			case 646:
			case 648:
			case 656:
			case 657:
				flag = false;
				break;
			}
			if (flag)
			{
				base.WndProc(ref m);
			}
			AddMessage(ref m);
		}

		public unsafe void UpdateMainThread()
		{
			lock (m_messageQueue)
			{
				m_tmpMessages.AddRange(m_messageQueue);
				m_tmpCopyData.AddRange(m_messageCopyDataQueue);
				m_messageQueue.Clear();
				m_messageCopyDataQueue.Clear();
			}
			int num = 0;
			for (int i = 0; i < m_tmpMessages.Count; i++)
			{
				Message message = m_tmpMessages[i];
				if ((long)message.Msg != 74)
				{
					ProcessMessage(ref message);
				}
				else if (num < m_tmpCopyData.Count)
				{
					WinApi.MyCopyData myCopyData = m_tmpCopyData[num++];
					void* ptr = &myCopyData;
					message.LParam = (IntPtr)ptr;
					ProcessMessage(ref message);
					Marshal.FreeHGlobal(myCopyData.DataPointer);
				}
			}
			m_tmpMessages.Clear();
			m_tmpCopyData.Clear();
		}

		public void SetCursor(Stream stream)
		{
			Bitmap bitmap = Image.FromStream(stream) as Bitmap;
			if (bitmap != null)
			{
				Invoke((Action)delegate
				{
					Cursor = new Cursor(bitmap.GetHicon());
				});
			}
		}

		public void AddMessageHandler(uint wmCode, ActionRef<MyMessage> messageHandler)
		{
			if (m_messageDictionary.ContainsKey(wmCode))
			{
				Dictionary<uint, ActionRef<MyMessage>> messageDictionary = m_messageDictionary;
				messageDictionary[wmCode] = (ActionRef<MyMessage>)Delegate.Combine(messageDictionary[wmCode], messageHandler);
			}
			else
			{
				m_messageDictionary.Add(wmCode, messageHandler);
			}
		}

		public void RemoveMessageHandler(uint wmCode, ActionRef<MyMessage> messageHandler)
		{
			if (m_messageDictionary.ContainsKey(wmCode))
			{
				Dictionary<uint, ActionRef<MyMessage>> messageDictionary = m_messageDictionary;
				messageDictionary[wmCode] = (ActionRef<MyMessage>)Delegate.Remove(messageDictionary[wmCode], messageHandler);
			}
		}

		public unsafe void AddMessage(ref Message message)
		{
			lock (m_messageQueue)
			{
				if ((long)message.Msg == 74)
				{
					WinApi.MyCopyData item = (WinApi.MyCopyData)message.GetLParam(typeof(WinApi.MyCopyData));
					IntPtr dataPointer = Marshal.AllocHGlobal(item.DataSize);
					Unsafe.CopyBlockUnaligned(dataPointer.ToPointer(), item.DataPointer.ToPointer(), (uint)item.DataSize);
					item.DataPointer = dataPointer;
					m_messageCopyDataQueue.Enqueue(item);
				}
				m_messageQueue.Enqueue(message);
			}
		}

		public void ClearMessageQueue()
		{
			lock (m_messageQueue)
			{
				m_messageQueue.Clear();
			}
		}

		private void ProcessMessage(ref Message message)
		{
			ActionRef<MyMessage> value = null;
			m_messageDictionary.TryGetValue((uint)message.Msg, out value);
			MyMessage myMessage = default(MyMessage);
			myMessage.Msg = message.Msg;
			myMessage.LParam = message.LParam;
			myMessage.WParam = message.WParam;
			MyMessage item = myMessage;
			value?.Invoke(ref item);
		}
	}
}
