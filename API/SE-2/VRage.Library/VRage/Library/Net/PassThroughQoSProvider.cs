using System;

namespace VRage.Library.Net
{
	/// <summary>
	/// Placeholder implementation <see cref="T:VRage.Library.Net.IQoSProvider" /> interface without actually providing any quality of service.
	/// </summary>
	/// <remarks>This class is only useful for testing.</remarks>
	public class PassThroughQoSProvider : IQoSProvider, IDisposable
	{
		private readonly IUnreliableTransportChannel m_channel;

		/// <inheritdoc />
		public bool MessagesAvailable
		{
			get
			{
				int frameSize;
				return m_channel.PeekFrame(out frameSize);
			}
		}

		/// <inheritdoc />
		public bool HasPendingDeliveries => false;

		/// <inheritdoc />
		public int MaxNonBlockingMessageSize => m_channel.MinimumMTU;

		public PassThroughQoSProvider(IUnreliableTransportChannel transport)
		{
			m_channel = transport;
		}

		/// <inheritdoc />
		public MessageTransferResult SendMessage(Span<byte> message, byte channel, SendMessageFlags flags = (SendMessageFlags)0)
		{
			m_channel.Send(message);
			return MessageTransferResult.QueuedSuccessfully;
		}

		/// <inheritdoc />
		public bool TryReceiveMessage(ref Span<byte> message, out byte channel)
		{
			channel = 0;
			return m_channel.TryGetFrame(ref message);
		}

		/// <inheritdoc />
		public bool TryPeekMessage(out int size, out byte channel)
		{
			channel = 0;
			return m_channel.PeekFrame(out size);
		}

		/// <inheritdoc />
		public void ProcessWriteQueues()
		{
		}

		/// <inheritdoc />
		public void ProcessReadQueues()
		{
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}
	}
}
