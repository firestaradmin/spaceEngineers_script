using System;

namespace VRage.Library.Net
{
	/// <summary>
	/// Represents a unreliable message-based connection between two peers.
	/// </summary>
	public interface IUnreliableTransportChannel : IDisposable
	{
		/// <summary>
		/// Guaranteed minimum value for MTU over this transport. It should always be possible to send packets of this size between hosts.
		/// </summary>
		/// <remarks>This value should be constant over the lifetime of the connection.</remarks>
		int MinimumMTU { get; }

		/// <summary>
		/// Send a frame of data to the remote host.
		/// </summary>
		/// <param name="frame"></param>
		/// <returns>Number of bytes actually transmitted.</returns>
		int Send(Span<byte> frame);

		/// <summary>
		/// Whether there are any un-processed frames from the remote host.
		/// </summary>
		/// <param name="frameSize"></param>
		/// <remarks>This query should be non-blocking.</remarks>
		bool PeekFrame(out int frameSize);

		/// <summary>
		/// Attempt to read a frame from the remote end.
		/// </summary>
		/// <param name="frame">The memory range where the frame is to be stored. The length of the span will be adjusted to match the actual size of the received frame.</param>
		/// <returns>Whether a valid frame was actually read.</returns>
		/// <remarks>If the frame is larger than the provided buffer any additional data will be lost.</remarks>
		bool TryGetFrame(ref Span<byte> frame);

		/// <summary>
		/// Query the actual Minimum Transmission Unit for the connection. This value can be greater than <see cref="P:VRage.Library.Net.IUnreliableTransportChannel.MinimumMTU" />.
		/// </summary>
		/// <remarks>This method should be non-blocking.</remarks>
		/// <param name="result">Callback invoked when the result is available.</param>
		void QueryMTU(Action<int> result);
	}
}
