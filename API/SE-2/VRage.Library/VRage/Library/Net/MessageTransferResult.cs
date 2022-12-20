namespace VRage.Library.Net
{
	/// <summary>
	/// Possible result codes when transferring a message.
	/// </summary>
	public enum MessageTransferResult
	{
		/// <summary>
		/// Message was added to queue.
		/// </summary>
		QueuedSuccessfully,
		/// <summary>
		/// The message queue is full.
		/// </summary>
		QueueFull,
		/// <summary>
		/// The message is oversized.
		/// </summary>
		OversizedMessage
	}
}
