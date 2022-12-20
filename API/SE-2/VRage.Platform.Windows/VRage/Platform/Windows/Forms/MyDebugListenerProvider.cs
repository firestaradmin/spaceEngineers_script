using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VRage.Utils;

namespace VRage.Platform.Windows.Forms
{
	public class MyDebugListenerProvider
	{
		private class MyTraceListener : DefaultTraceListener, IAdvancedDebugListener
		{
			private readonly HashSet<string> m_ignoredMsgs = new HashSet<string>();

			private readonly FastResourceLock m_ignoredLock = new FastResourceLock();

			public MyTraceListener()
			{
				Name = "VRage Listener";
			}

			[DllImport("User32.dll")]
			private static extern short GetAsyncKeyState(Keys vKey);

			[DebuggerHidden]
			public override void Fail(string message)
			{
				Fail(message, null, null, -1);
			}

			[DebuggerHidden]
			public override void Fail(string message, string detailMessage)
			{
				Fail(message, detailMessage, null, -1);
			}

			public void Fail(string message, string detail, string file, int line)
			{
				StringBuilder stringBuilder = new StringBuilder(message);
				if (file != null)
				{
					stringBuilder.AppendFormat("\n{0}:{1}", file, line);
				}
				if (!string.IsNullOrEmpty(detail))
				{
					stringBuilder.Append('\n', 2).Append("Detail: ").Append(detail);
				}
				AddStackTrace(stringBuilder);
				TraceMessage(stringBuilder.ToString(), showMessageBox: true);
			}

			private void AddStackTrace(StringBuilder sb)
			{
				string stackTrace = Environment.StackTrace;
				bool flag = false;
				int num = 0;
				while (true)
				{
					int num2 = stackTrace.IndexOf('\n', num);
					if (num2 < 0)
					{
						break;
					}
					ReadOnlySpan<char> span = stackTrace.AsSpan(num, num2 - num);
					if (!flag)
					{
						if (span.Contains("at VRage.Platform.Windows.Forms.MyDebugListenerProvider.MyTraceListener.AddStackTrace".AsSpan(), StringComparison.Ordinal))
						{
							flag = true;
						}
					}
					else if (!span.Contains(" VRage.MyDebug".AsSpan(), StringComparison.Ordinal) && !span.Contains(" System.Diagnostics.Debug".AsSpan(), StringComparison.Ordinal) && !span.Contains(" VRage.Platform.Windows.Forms.MyDebugListenerProvider".AsSpan(), StringComparison.Ordinal))
					{
						break;
					}
					num = num2 + 1;
				}
				sb.Append('\n', 2).Append(stackTrace, num, stackTrace.Length - num);
			}

			[DebuggerHidden]
			private void TraceMessage(string msg, bool showMessageBox, bool ignoreAll = false)
			{
				using (m_ignoredLock.AcquireSharedUsing())
				{
					if (m_ignoredMsgs.Contains(msg))
					{
						return;
					}
				}
				MyLog.Default.AppendToClosedLog("ASSERT: " + msg);
				DialogResult dialogResult = DialogResult.Ignore;
				if (showMessageBox)
				{
					do
					{
						dialogResult = MessageBox.Show(msg, "Assertion", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button3, System.Windows.Forms.MessageBoxOptions.ServiceNotification);
					}
					while (dialogResult == DialogResult.Abort && Convert.ToInt32(GetAsyncKeyState(Keys.A).ToString()) < 0);
					ignoreAll = (Control.ModifierKeys & Keys.Control) != 0;
				}
				switch (dialogResult)
				{
				case DialogResult.Abort:
					Environment.Exit(1);
					return;
				case DialogResult.Retry:
					Debugger.Break();
					return;
				}
				if (dialogResult == DialogResult.Ignore && ignoreAll)
				{
					using (m_ignoredLock.AcquireExclusiveUsing())
					{
						m_ignoredMsgs.Add(msg);
					}
				}
			}

			public override void WriteLine(string message)
			{
				WriteLine(message, null, -1);
			}

			public void WriteLine(string message, string file, int line)
			{
				StringBuilder stringBuilder = new StringBuilder(message);
				if (file != null)
				{
					stringBuilder.AppendFormat("\n{0}:{1}", file, line);
				}
				AddStackTrace(stringBuilder);
				TraceMessage(stringBuilder.ToString(), showMessageBox: false, ignoreAll: true);
			}
		}

		private static TraceListener[] m_storedListeners;

		public static void Register()
		{
			m_storedListeners = new TraceListener[MyDebug.Listeners.Count];
			MyDebug.Listeners.CopyTo(m_storedListeners, 0);
			MyDebug.Listeners.Clear();
			MyDebug.Listeners.Add(new MyTraceListener());
		}

		public static void Unregister()
		{
			MyDebug.Listeners.Clear();
			MyDebug.Listeners.AddRange(m_storedListeners);
		}
	}
}
