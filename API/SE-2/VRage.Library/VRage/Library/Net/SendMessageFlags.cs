using System;

namespace VRage.Library.Net
{
	/// <summary>
	/// Flags altering how message sending should be performed..
	/// </summary>
	[Flags]
	public enum SendMessageFlags
	{
		/// <summary>
		/// Causes the message queue to be flushed immediately upon sending the message.
		/// </summary>
		HighPriority = 0x1,
		/// <summary>
		/// Causes the send operation to fail if it could block the caller.
		/// </summary>
		NonBlocking = 0x2
	}
}
