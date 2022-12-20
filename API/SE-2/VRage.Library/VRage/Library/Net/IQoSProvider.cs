using System;

namespace VRage.Library.Net
{
	/// <summary>
	/// Interface representing an object that can implement message reliability over a unreliable connection.
	/// </summary>
	/// <remarks></remarks>
	public interface IQoSProvider : IDisposable
	{
		/// <summary>
		/// Whether there are messages available from the remote host.
		/// </summary>
		bool MessagesAvailable { get; }

		/// <summary>
		/// Whether this provider has outstanding messages that are awaiting delivery.
		/// </summary>
		bool HasPendingDeliveries { get; }

		/// <summary>
		/// Maximum size for a message that is guaranteed to be non-blocking if the message queue is not full.
		/// </summary>
		int MaxNonBlockingMessageSize { get; }

		/// <summary>
		/// Send a message to the remote host.
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <param name="channel">The channel to send the message through.</param>
		/// <param name="flags"></param>
		/// <returns>The result of the operation.</returns>
		MessageTransferResult SendMessage(Span<byte> message, byte channel, SendMessageFlags flags = (SendMessageFlags)0);

		/// <summary>
		/// Attempt to receive a message from the remote host.
		/// </summary>
		/// <remarks>This call may block.</remarks>
		/// <param name="message">The message data</param>
		/// <param name="channel">The message channel.</param>
		/// <returns><code>true</code> if a valid message could be read. Otherwise <code>false</code></returns>
		bool TryReceiveMessage(ref Span<byte> message, out byte channel);

		/// <summary>
		/// Peek the channel and size of the next available message.
		/// </summary>
		/// <remarks>This call may block.</remarks>
		/// <param name="size">The message size.</param>
		/// <param name="channel">The message channel.</param>
		/// <returns>Whether there was a message to peek.</returns>
		bool TryPeekMessage(out int size, out byte channel);

		/// <summary>
		/// Process all pending write queues.
		/// </summary>
		/// <remarks>
		/// For implementations that buffer messages this will require them to be flushed immediately.
		/// </remarks>
		void ProcessWriteQueues();

		/// <summary>
		/// Pool for received frames.
		/// </summary>
		void ProcessReadQueues();
	}
}
