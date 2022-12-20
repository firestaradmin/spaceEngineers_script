using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ParallelTasks;
using VRage.Library.Debugging;
using VRage.Library.Memory;
using VRage.Network;
using VRage.Trace;

namespace VRage.Library.Net
{
	/// <summary>
	/// Default implementation of a Quality of Service Provider.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This implementation supports:
	/// <ul>
	///     <li>Reliability for undelivered, out of order and duplicate messages.</li>
	///     <li>Sliding window to maintain bandwidth when latency is high.</li>
	///     <li>Message coalescing to reduce the number of transmissions when messages are sufficiently smaller than the MTU size.</li>
	///     <li>Message fragmentation to allow for streaming messages larger then the MTU size.</li>
	/// </ul>
	/// It does not, however, support:
	/// <ul>
	///     <li>Response to variable MTU size.</li>
	///     <li>Encryption.</li>
	///     <li>Reliability for tampered or incomplete messages.</li>
	/// </ul>
	/// </para>
	/// <h3>Terminology</h3>
	/// <para>
	/// To reduce confusion we use the following terminology:
	/// <ul>
	///     <li>Frame: A complete packet send to underlying unreliable transport.</li>
	///     <li>Message: An individual message as provided by the user.</li>
	/// </ul>
	/// <b>Importantly</b>: One frame may contain multiple small messages and a large message may span multiple frames.
	/// </para>
	/// <h3>Queuing</h3>
	/// <para>
	/// When a frame is ready for sending but the sliding window is full it gets queued. The same happens if we the
	/// sliding window for received frames advances but not all messages from the processed frames are read.
	/// </para><para>
	/// These queues are in the same memory block as the sliding window itself and advance with it.
	/// </para><para>
	/// If the queue get's full we increase it's size up to the user provided maximum. After that we will no longer
	/// process remote message frames when the receive queue is full, and will block the caller until the window
	/// advances on the sending queue.
	/// </para>
	/// <h3>Message Formats</h3>
	/// <ul>
	///     <li>Message: Message frames start with a non-zero sequence number, after that comes a sequence of messages.
	/// Each message has a header containing the channel and size. Messages may span multiple frames, in that case the
	/// following frame will not contain a message header, just the remainder of the data. This repeats for as many
	/// frames as required to send the entire message.</li>
	///     <li>Ack: An ack frame starts with a zero sequence number, followed by a frame type corresponding to the Ack
	/// frame, and by a bitmap representing the state of the remote sliding window.</li>
	///
	/// </ul>
	/// </remarks>
	public class DefaultQoSProvider : IQoSProvider, IDisposable
	{
		/// <summary>
		/// Combined queue and sliding window buffers.
		/// </summary>
		/// <typeparam name="THeader">The type of the metadata header.</typeparam>
		[DebuggerDisplay("Head({Head})-Tail({Tail}) Active({UsedLength})")]
		[DebuggerTypeProxy(typeof(CombinedBuffer<>.CombinedBufferDebugProxy))]
		private struct CombinedBuffer<THeader> where THeader : unmanaged, IHeader
		{
			/// <summary>
			/// Helper struct for buffer reallocation.
			/// </summary>
			private struct BufferSet
			{
				public THeader[] Headers;

				public NativeArray FrameData;

				public int FrameSize;

				public BufferSet(THeader[] headers, NativeArray frameData, int frameSize)
				{
					Headers = headers;
					FrameData = frameData;
					FrameSize = frameSize;
				}
			}

			[DebuggerDisplay("Head({m_instance.Head})-Tail({m_instance.Tail}) Active({m_instance.UsedLength})")]
			private class CombinedBufferDebugProxy
			{
				[DebuggerDisplay("{m_debugString}")]
				public struct Entry
				{
					public THeader Header;

					public uint Sequence;

					private string m_debugString;

					public Entry(THeader header, uint sequence, string debugString)
					{
						Header = header;
						Sequence = sequence;
						m_debugString = debugString;
					}
				}

				private CombinedBuffer<THeader> m_instance;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public unsafe string[] Keys
				{
					get
					{
						THeader[] headers = m_instance.Headers;
						string[] array = new string[headers.Length];
						for (int i = 0; i < headers.Length; i++)
						{
							FrameHeader header = *(FrameHeader*)((byte*)m_instance.Buffer.Ptr.ToPointer() + i * m_instance.FrameSize);
							if (m_instance.Mapping.IsInRange(i) || !headers[i].IsUnused)
							{
								array[i] = headers[i].FormatDebug(header);
							}
							else
							{
								array[i] = "Unused";
							}
							string text = "";
							if (m_instance.Head == m_instance.Tail && i == m_instance.Head)
							{
								text = ((m_instance.UsedLength > 0) ? ">< " : "<> ");
							}
							else if (m_instance.Head == i)
							{
								text = "> ";
							}
							else if (m_instance.Tail == i)
							{
								text = "< ";
							}
							else if (m_instance.Mapping.IsInRange(i))
							{
								text = "- ";
							}
							array[i] = text + array[i];
						}
						return array;
					}
				}

				public CombinedBufferDebugProxy(CombinedBuffer<THeader> instance)
				{
					m_instance = instance;
				}
			}

			public CircularMapping Mapping;

			public readonly int FrameSize;

			private static readonly CircularMapping.Copy<BufferSet> m_copyDelegate = Copy;

			public THeader[] Headers { get; private set; }

			public NativeArray Buffer { get; private set; }

			public int Head => Mapping.Head;

			public int Tail => Mapping.Tail;

			public int UsedLength => Mapping.ActiveLength;

			/// <summary>
			/// Size of the window portion of the buffer.
			/// </summary>
			public int WindowCapacity { get; private set; }

			/// <summary>
			/// Size of the send/receive queue portion of the buffer.
			/// </summary>
			public int QueueCapacity { get; private set; }

			/// <summary>
			/// Total capacity of this sliding window.
			/// </summary>
			public int TotalCapacity => Mapping.Capacity;

			/// <summary>
			/// Get the hea
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public ref THeader this[int index] => ref Headers[index];

			public CombinedBuffer(int windowCapacity, int queueCapacity, int frameSize, bool clear)
			{
				int num = windowCapacity + queueCapacity;
				Headers = new THeader[num];
				Buffer = NativeArrayAllocator.Allocate(num * frameSize);
				if (clear)
				{
					Buffer.AsSpan().Clear();
				}
				Mapping = new CircularMapping(num);
				FrameSize = frameSize;
				WindowCapacity = windowCapacity;
				QueueCapacity = queueCapacity;
			}

			/// <summary>
			/// Adjust the size of the sliding window versus the queue size while maintaining the total size constant.
			/// </summary>
			/// <param name="newWindowSize">new size ofr the window portion of the buffer.</param>
			public void Rescale(int newWindowSize)
			{
				if (newWindowSize > TotalCapacity)
				{
					throw new InvalidOperationException("the new sliding window size is too large.");
				}
				WindowCapacity = newWindowSize;
				QueueCapacity = TotalCapacity - newWindowSize;
			}

			/// <summary>
			/// Resize this sliding window.
			/// </summary>
			/// <param name="windowSize">The new maximum window size.</param>
			/// <param name="queueSize">The new maximum queue size.</param>
<<<<<<< HEAD
			/// <param name="clear"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void Resize(int windowSize, int queueSize, bool clear)
			{
				int num = windowSize + queueSize;
				if (num < UsedLength)
				{
					throw new InvalidOperationException("New window size is not large enough to hold the current number of frames.");
				}
				THeader[] headers = new THeader[num];
				int size = num * FrameSize;
				NativeArray nativeArray = NativeArrayAllocator.Allocate(size);
				if (clear)
				{
					nativeArray.AsSpan().Clear();
				}
				BufferSet original = new BufferSet(Headers, Buffer, FrameSize);
				BufferSet resized = new BufferSet(headers, nativeArray, FrameSize);
				Mapping.ResizeBuffer(num, original, resized, m_copyDelegate);
				Headers = headers;
				Buffer.Dispose();
				Buffer = nativeArray;
				WindowCapacity = windowSize;
				QueueCapacity = queueSize;
			}

			private static void Copy(BufferSet src, int srcIndex, BufferSet dst, int dstIndex, int length)
			{
				Array.Copy(src.Headers, srcIndex, dst.Headers, dstIndex, length);
				int start = srcIndex * src.FrameSize;
				int start2 = dstIndex * src.FrameSize;
				length *= src.FrameSize;
				Span<byte> span = src.FrameData.AsSpan();
				span = span.Slice(start, length);
				span.CopyTo(dst.FrameData.AsSpan().Slice(start2));
			}

			public void CopyFrame(int sourcePosition, int destinationPosition)
			{
				Span<byte> frame = GetFrame(sourcePosition);
				Span<byte> frame2 = GetFrame(destinationPosition);
				frame.CopyTo(frame2);
			}

			/// <summary>
			/// Advance the head to the next frame, making it valid for processing.
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AdvanceHead(bool clear, int amount = 1)
			{
				int head = Mapping.Head;
				Mapping.AdvanceHead(amount);
				if (!clear)
				{
					return;
				}
				int head2 = Mapping.Head;
				foreach (int item in EnumerateFrames(head, head2))
				{
					GetFrame(item).Clear();
				}
			}

			/// <summary>
			/// Advance the tail to the next frame, making it ready for re-use.
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AdvanceTail(int amount = 1)
			{
				Mapping.AdvanceTail(amount);
			}

			/// <summary>
			/// Advance the provided index to the next valid position in the circular buffer.
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Advance(int index)
			{
				return Mapping.Advance(index);
			}

			/// <summary>
			/// Advance the provided index to the next valid position in the circular buffer.
			/// </summary>
			/// <param name="index"></param>
<<<<<<< HEAD
			/// <param name="amount"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Advance(int index, int amount)
			{
				return Mapping.Advance(index, amount);
			}

			/// <summary>
			/// Advance the provided index to the next valid position in the circular buffer.
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Retract(int index)
			{
				return Mapping.Retract(index);
			}

			/// <summary>
			/// Advance the provided index to the next valid position in the circular buffer.
			/// </summary>
			/// <param name="index"></param>
<<<<<<< HEAD
			/// <param name="amount"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Retract(int index, int amount)
			{
				return Mapping.Retract(index, amount);
			}

			/// <summary>
			/// Advance the provided index to the next valid position in the circular buffer.
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public WindowPointer Advance(WindowPointer index)
			{
				return new WindowPointer(index.Sequence + 1, Advance(index.Position));
			}

			/// <summary>
			/// Advance the provided index to the next valid position in the circular buffer.
			/// </summary>
			/// <param name="index"></param>
<<<<<<< HEAD
			/// <param name="amount"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public WindowPointer Advance(WindowPointer index, int amount)
			{
				return new WindowPointer(index.Sequence + (uint)amount, Advance(index.Position, amount));
			}

			/// <summary>
			/// Calculate the number of frames between the two positions in the circular buffer.
			/// </summary>
			/// <param name="from"></param>
			/// <param name="to"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Distance(int from, int to)
			{
				return Mapping.Distance(from, to);
			}

			/// <summary>
			/// Calculate the number of frames between the two positions in the circular buffer.
			/// </summary>
			/// <param name="from"></param>
			/// <param name="to"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Distance(in WindowPointer from, int to)
			{
				return Distance(from.Position, to);
			}

			/// <summary>
			/// Calculate the number of frames between the two positions in the circular buffer.
			/// </summary>
			/// <param name="from"></param>
			/// <param name="to"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Distance(int from, in WindowPointer to)
			{
				return Distance(from, to.Position);
			}

			/// <summary>
			/// Calculate the number of frames between the two positions in the circular buffer.
			/// </summary>
			/// <param name="from"></param>
			/// <param name="to"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int Distance(in WindowPointer from, in WindowPointer to)
			{
				return (int)(to.Sequence - from.Sequence);
			}

			/// <summary>
			/// Given a start reference pointer and a position in the buffer calculate the sequence number.
			/// </summary>
			/// <param name="reference">Point of reference.</param>
			/// <param name="position">The position to calculate the sequence number for.</param>
			/// <returns></returns>
			public uint CalculateSequence(in WindowPointer reference, int position)
			{
				return reference.Sequence + (uint)Distance(reference.Position, position);
			}

			/// <summary>
			/// Calculate the position of a frame given a point of reference and it's sequence number.
			/// </summary>
			/// <param name="reference"></param>
			/// <param name="sequenceNumber"></param>
			/// <returns></returns>
			public int CalculatePosition(in WindowPointer reference, uint sequenceNumber)
			{
				return reference.Position + (int)(sequenceNumber - reference.Sequence);
			}

			/// <summary>
			/// Get a span representing the memory range for a given frame.
			/// </summary>
			/// <param name="frameIndex">The index of the frame.</param>
			/// <returns>The memory range where the frame is stored.</returns>
			public Span<byte> GetFrame(int frameIndex)
			{
				if (frameIndex < 0 || frameIndex >= TotalCapacity)
				{
					throw new ArgumentOutOfRangeException("frameIndex");
				}
				return Buffer.AsSpan().Slice(frameIndex * FrameSize, FrameSize);
			}

			/// <summary>
			/// Enumerate all frames in the user provided range.
			/// </summary>
			/// <remarks>This method assumes start and end may never be the same.</remarks>
			/// <param name="start"></param>
			/// <param name="end"></param>
			/// <returns></returns>
			public CircularMapping.Enumerable EnumerateFrames(int start, int end)
			{
				return new CircularMapping.Enumerable(start, end, TotalCapacity, start == end);
			}

			public void Dispose()
			{
				Buffer.Dispose();
				this = default(CombinedBuffer<THeader>);
			}
		}

		private interface IHeader
		{
			bool IsUnused { get; }

			string FormatDebug(FrameHeader header);
		}

		/// <summary>
		/// Pointer to a frame in the sliding window.
		/// </summary>
		private readonly struct WindowPointer
		{
			/// <summary>
			/// Frame sequence number.
			/// </summary>
			public readonly uint Sequence;

			/// <summary>
			/// Position in the window.
			/// </summary>
			public readonly int Position;

			public WindowPointer(uint sequence, int position)
			{
				Sequence = sequence;
				Position = position;
			}

			public void Deconstruct(out uint sequence, out int queuePosition)
			{
				sequence = Sequence;
				queuePosition = Position;
			}
		}

		/// <summary>
		/// Settings for a sliding window in a user friendly format.
		/// </summary>
		public struct InitSettings
		{
			/// <summary>
			/// Minimum number of bytes to hold in the sliding window.
			/// </summary>
			public int MinimumWindowSize;

			/// <summary>
			/// Maximum number of bytes to hold in the sliding window.
			/// </summary>
			public int MaximumWindowSize;

			/// <summary>
			/// Minimum size of the send/receive queues.
			/// </summary>
			public int MinimumQueueSize;

			/// <summary>
			/// Maximum size of the send/receive queues.
			/// </summary>
			public int MaximumQueueSize;

			/// <summary>
			/// Maximum size for a oversized message.
			/// </summary>
			public int MaximumMessageSize;

			/// <summary>
			/// Initial value for the sequence number.
			/// </summary>
			/// <remarks>Use this with care. Make sure the remote end chooses the same number, otherwise they will not be able to communicate.</remarks>
			public uint InitialSequenceNumber;

			/// <summary>
			/// Time to wait without a successful transmission before throwing an exception.
			/// </summary>
			/// <returns>This will happen when the send queue is full but we cannot get an ack for the frames in the active sliding window.</returns>
			public TimeSpan DisconnectTimeout;

			/// <summary>
			/// Clear buffers after every use.
			/// </summary>
			public bool ClearBuffers;

			/// <summary>
			/// Factor applied to the estimated RTT to arrive at the frame timeout time.
			/// </summary>
			/// <remarks>This value must be greater than or equal to 1 and must be chosen very carefully.</remarks>
			public float FrameTimeoutSlipFactor;

			/// <summary>
			/// Factor used to grow the sliding window by over time.
			/// </summary>
			/// <remarks>This value must be greater than 1 and must be chosen very carefully.</remarks>
			public float WindowGrowthFactor;

			/// <summary>
			/// Maximum acceptable packet loss before the window starts to shrink.
			/// </summary>
			public float FrameLossTolerance;

			/// <summary>
			/// Minimum number of frames to consider before adjusting the sliding window size.
			/// </summary>
			/// <remarks>
			/// The window will still adjust only as often as the frame timeout, this setting
			/// simply adds a second limit so that the window does not try to adjust when only
			/// a handful of messages are being transferred per second.
			/// </remarks>
			public int WindowAdjustmentThreshold;

			/// <summary>
			/// Default sliding window settings.
			/// </summary>
			public static readonly InitSettings Default = new InitSettings
			{
				MinimumWindowSize = 8192,
				MaximumWindowSize = 65536,
				MinimumQueueSize = 8192,
				MaximumQueueSize = 262144,
				DisconnectTimeout = TimeSpan.FromSeconds(30.0),
				MaximumMessageSize = int.MaxValue,
				FrameTimeoutSlipFactor = 1.05f,
				WindowGrowthFactor = 2f,
				FrameLossTolerance = 0.05f,
				WindowAdjustmentThreshold = 20
			};
		}

		/// <summary>
		/// Represents the provider setting in a ready to use format.
		/// </summary>
		public struct ProcessedSettings
		{
			/// <summary>
			/// Minimum number of frames to hold in the sliding window.
			/// </summary>
			public int MinimumWindowCapacity;

			/// <summary>
			/// Maximum number of frames to hold in the sliding window.
			/// </summary>
			public int MaximumWindowCapacity;

			/// <summary>
			/// Minimum size of the send/receive queues.
			/// </summary>
			public int MinimumQueueCapacity;

			/// <summary>
			/// Maximum size of the send/receive queues.
			/// </summary>
			public int MaximumQueueCapacity;

			/// <summary>
			/// Maximum size in bytes of a message that could be sent by the QoS provider without blocking.
			/// </summary>
			public int MaximumNonBlockingMessageSize;

			/// <summary>
			/// Time to wait without a successful transmission before throwing an exception.
			/// </summary>
			/// <returns>This will happen when the send queue is full but we cannot get an ack for the frames in the active sliding window.</returns>
			public TimeSpan DisconnectTimeout;

			/// <summary>
			/// Clear buffers after every use.
			/// </summary>
			public bool ClearBuffers;

			/// <summary>
			/// Factor applied to the estimated RTT to arrive at the frame timeout time.
			/// </summary>
			/// <remarks>This value must be greater than or equal to 1 and must be chosen very carefully.</remarks>
			public float FrameTimeoutSlipFactor;

			/// <summary>
			/// Minimum number of frames to consider before adjusting the sliding window size.
			/// </summary>
			public int WindowAdjustmentThreshold;

			/// <summary>
			/// Factor used to grow the sliding window by over time.
			/// </summary>
			/// <remarks>This value must be greater than 1 and must be chosen very carefully.</remarks>
			public float WindowGrowthFactor;

			/// <summary>
			/// Maximum acceptable packet loss before the window starts to shrink.
			/// </summary>
			public float FrameLossTolerance;

			/// <summary>
			/// Create a set of processed setting form a raw instance and transport.
			/// </summary>
			/// <param name="settings"></param>
			/// <param name="transport"></param>
			public static ProcessedSettings Create(in InitSettings settings, IUnreliableTransportChannel transport)
			{
				int mtu = transport.MinimumMTU;
				int num = mtu - AckFrameHeader.Size << 3;
				int bytes2 = Math.Min(settings.MaximumWindowSize, num * mtu);
				ProcessedSettings result = default(ProcessedSettings);
				result.MinimumWindowCapacity = ToMtu(settings.MinimumWindowSize);
				result.MaximumWindowCapacity = ToMtu(bytes2);
				result.MinimumQueueCapacity = Math.Max(ToMtu(settings.MinimumQueueSize), 2);
				result.MaximumQueueCapacity = Math.Max(ToMtu(settings.MaximumQueueSize), 2);
				result.DisconnectTimeout = settings.DisconnectTimeout;
				int num2 = mtu - FrameHeader.Size;
				result.MaximumNonBlockingMessageSize = num2 * (result.MaximumQueueCapacity - 1) - 6;
				result.ClearBuffers = settings.ClearBuffers;
				result.FrameTimeoutSlipFactor = Math.Max(settings.FrameTimeoutSlipFactor, 1f);
				result.WindowGrowthFactor = Math.Max(settings.WindowGrowthFactor, 1.01f);
				if (settings.FrameLossTolerance < 0f || settings.FrameLossTolerance > 1f)
				{
					throw new ArgumentException("Frame loss tolerance out of bounds.");
				}
				result.FrameLossTolerance = settings.FrameLossTolerance;
				result.WindowAdjustmentThreshold = settings.WindowAdjustmentThreshold;
				return result;
				int ToMtu(int bytes)
				{
					return (bytes + mtu - 1) / mtu;
				}
			}
		}

		private struct ReceivedFrame : IHeader
		{
			public ReceiveState State;

			public int Length;

			/// <inheritdoc />
			public bool IsUnused
			{
				get
				{
					if (Length == 0)
					{
						return State == ReceiveState.Missing;
					}
					return false;
				}
			}

			public void SetReceived(int length)
			{
				State = ReceiveState.Received;
				Length = length;
			}

			public void SetMissing()
			{
				State = ReceiveState.Missing;
				Length = 0;
			}

			/// <inheritdoc />
			public string FormatDebug(FrameHeader header)
			{
<<<<<<< HEAD
				switch (State)
				{
				case ReceiveState.Missing:
					return $"{header.Sequence:D5}:Missing";
				case ReceiveState.Received:
					return $"{header.Sequence:D5}:Received({Length})";
				case ReceiveState.Unprocessed:
					return $"{header.Sequence:D5}:Unprocessed({Length})";
				default:
					throw new ArgumentOutOfRangeException();
				}
=======
				return State switch
				{
					ReceiveState.Missing => $"{header.Sequence:D5}:Missing", 
					ReceiveState.Received => $"{header.Sequence:D5}:Received({Length})", 
					ReceiveState.Unprocessed => $"{header.Sequence:D5}:Unprocessed({Length})", 
					_ => throw new ArgumentOutOfRangeException(), 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private enum ReceiveState : byte
		{
			Missing,
			Received,
			Unprocessed
		}

		/// <summary>
		/// Represents a frame we will be sending.
		/// </summary>
		[DebuggerDisplay("{State}:{Length}")]
		private struct SendingFrame : IHeader
		{
			/// <summary>
			/// State of the frame.
			/// </summary>
			public SendState State;

			/// <summary>
			/// Last time we sent this frame.
			/// </summary>
			public TimeSpan LastSentTime;

			/// <summary>
			/// Last time we sent this frame.
			/// </summary>
			public TimeSpan EnqueueTime;

			/// <summary>
			/// The length of the frame.
			/// </summary>
			public int Length;

			/// <inheritdoc />
			public bool IsUnused
			{
				get
				{
					if (State == SendState.Acknowledged)
					{
						return Length == 0;
					}
					return false;
				}
			}

			/// <summary>
			/// Move the frame to the Acknowledged state.
			/// </summary>
			public void SetAcknowledged()
			{
				State = SendState.Acknowledged;
				LastSentTime = TimeSpan.Zero;
				EnqueueTime = TimeSpan.Zero;
				Length = 0;
			}

			/// <inheritdoc />
			public string FormatDebug(FrameHeader header)
			{
<<<<<<< HEAD
				switch (State)
				{
				case SendState.Sent:
					return $"{header.Sequence:D5}:Sent({Length} bytes, first sent {EnqueueTime}, last {LastSentTime.TotalMilliseconds})";
				case SendState.Queued:
					return $"{header.Sequence:D5}:Queue({Length} bytes)";
				default:
					return $"{header.Sequence:D5}:{State}";
				}
=======
				return State switch
				{
					SendState.Sent => $"{header.Sequence:D5}:Sent({Length} bytes, first sent {EnqueueTime}, last {LastSentTime.TotalMilliseconds})", 
					SendState.Queued => $"{header.Sequence:D5}:Queue({Length} bytes)", 
					_ => $"{header.Sequence:D5}:{State}", 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		/// State for a frame on the sending sliding window.
		/// </summary>
		private enum SendState : byte
		{
			/// <summary>Frame has been acknowledged/is not used.</summary>
			Acknowledged,
			/// <summary>Frame is being written and is not finished.</summary>
			Unfinished,
			/// <summary>Frame was put on the sending queue.</summary>
			Queued,
			/// <summary>Frame was sent to the remote host.</summary>
			Sent
		}

		/// <summary>
		/// Represents all the logical information about a message frame.
		/// </summary>
		private readonly ref struct FrameInfo
		{
			/// <summary>
			/// Index of the frame in the sliding window.
			/// </summary>
			public readonly int FramePosition;

			/// <summary>
			/// Sequence number for the frame.
			/// </summary>
			public readonly uint Sequence;

			/// <summary>
			/// The frame's raw data.
			/// </summary>
			public readonly Span<byte> Data;

			public FrameInfo(int framePosition, uint sequence, Span<byte> data)
			{
				FramePosition = framePosition;
				Sequence = sequence;
				Data = data;
			}
		}

		/// <summary>
		/// Enum describing the types of control frames.
		/// </summary>
		private enum FrameType : byte
		{
			/// <summary>This frame contains one or more messages.</summary>
			Message,
			/// <summary>This frame Acknowledges a given control message.</summary>
			AckControl,
			/// <summary>This frame contains a sequence of bits that represents which frames the remote currently has received.</summary>
			Ack,
			/// <summary>Ping frames are used to calculate RTT.</summary>
			Ping
		}

		/// <summary>
		/// Basic frame header.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private readonly struct FrameHeader
		{
			/// <summary>
			/// Number of bytes occupied by this struct.
			/// </summary>
			public unsafe static readonly int Size = sizeof(FrameHeader);

			/// <summary>
			/// Sequence number.
			/// </summary>
			public readonly uint Sequence;

			/// <summary>
			/// The type of the frame.
			/// </summary>
			public readonly FrameType Type;

			public FrameHeader(uint sequence, FrameType type)
			{
				Sequence = sequence;
				Type = type;
			}

			public static FrameHeader Message(uint sequence)
			{
				return new FrameHeader(sequence, FrameType.Message);
			}
		}

		/// <summary>
		/// Header for the ack frame.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct AckFrameHeader
		{
			/// <summary>
			/// Number of bytes occupied by this struct.
			/// </summary>
			public unsafe static readonly int Size = sizeof(AckFrameHeader);

			/// <summary>
			/// Base Header.
			/// </summary>
			public FrameHeader FrameHeader;

			/// <summary>
			/// Sequence number for the first frame the remote knows it has not received.
			/// </summary>
			public uint FirstMissingSequence;

			public AckFrameHeader(uint sequence, uint firstMissingSequence)
			{
				FrameHeader = new FrameHeader(sequence, FrameType.Ack);
				FirstMissingSequence = firstMissingSequence;
			}
		}

		/// <summary>
		/// Layout of the ping frame.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct PingFrame
		{
			/// <summary>
			/// Number of bytes occupied by this struct.
			/// </summary>
			public unsafe static readonly int Size = sizeof(PingFrame);

			/// <summary>
			/// Base Header.
			/// </summary>
			public FrameHeader FrameHeader;

			/// <summary>
			/// Timestamp for the original sending of this message.
			/// </summary>
			public TimeSpan Timestamp;

			public PingFrame(uint sequence, TimeSpan timestamp)
			{
				FrameHeader = new FrameHeader(sequence, FrameType.Ping);
				Timestamp = timestamp;
			}
		}

		/// <summary>
		/// Struct representing a message header.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private readonly struct MessageHeader
		{
			/// <summary>
			/// The channel the message is sent through.
			/// </summary>
			public readonly byte Channel;

			/// <summary>
			/// Sequence number.
			/// </summary>
			public readonly uint Length;

			/// <summary>
			/// Maximum size for a message header.
			/// </summary>
			public const int MaxSize = 6;

			public MessageHeader(byte channel, uint length)
			{
				Length = length;
				Channel = channel;
			}

			public MessageHeader(byte channel, Span<byte> message)
			{
				Length = (uint)message.Length;
				Channel = channel;
			}

			/// <summary>
			/// Whether this message header fits in the available space.
			/// </summary>
			/// <param name="space"></param>
			/// <returns></returns>
			public bool Fits(int space)
			{
				if (space < 6 && Variant.CalcEncodedLength(Length) + 1 > space)
				{
					return false;
				}
				return true;
			}

			/// <summary>
			/// Calculate the size of this message header.
			/// </summary>
			public int Size()
			{
				return Variant.CalcEncodedLength(Length) + 1;
			}

			/// <summary>
			/// Read a message header from a memory range.
			/// </summary>
			/// <param name="frame">The range where the memory is stored.</param>
			/// <param name="headerLength">The length the header occupied.</param>
			/// <returns></returns>
			public static MessageHeader Read(Span<byte> frame, out int headerLength)
			{
				byte channel = frame[0];
				headerLength = 1 + Variant.ReadVariant(frame.Slice(1), out var value);
				return new MessageHeader(channel, value);
			}
		}

		/// <summary>
		/// Utilities for encoding numbers in a variable length format.
		/// </summary>
		private static class Variant
		{
			/// <summary>
			/// Maximum length for a valid Variant-encoded Uint32.
			/// </summary>
			public const int Uint32MaximumEncodedLength = 5;

			/// <summary>
			/// Read a variant from the provided source range and store it in <paramref name="value" />.
			/// </summary>
			/// <param name="source">The memory range containing the variant to be decoded.</param>
			/// <param name="value">The decoded value.</param>
			/// <returns>The number of bytes the encoded representation occupied.</returns>
			/// <exception cref="T:System.OverflowException">If the encoded value would overflow the capacity of a <c>uint</c>.</exception>
			public static int ReadVariant(Span<byte> source, out uint value)
			{
				value = source[0];
				if ((value & 0x80) == 0)
				{
					return 1;
				}
				value &= 127u;
				uint num = source[1];
				value |= (num & 0x7F) << 7;
				if ((num & 0x80) == 0)
				{
					return 2;
				}
				num = source[2];
				value |= (num & 0x7F) << 14;
				if ((num & 0x80) == 0)
				{
					return 3;
				}
				num = source[3];
				value |= (num & 0x7F) << 21;
				if ((num & 0x80) == 0)
				{
					return 4;
				}
				num = source[4];
				value |= num << 28;
				if ((num & 0xF0) == 0)
				{
					return 5;
				}
				throw new OverflowException("The encoded variant would overflow the capacity of System.Uint32.");
			}

			/// <summary>
			/// Writes a variant to the provided memory chunk.
			/// </summary>
			/// <param name="value">The value to write as a variant</param>
			/// <param name="destination">The memory range where to store the result.</param>
			/// <returns>The number of bytes written.</returns>
			public static int WriteVariant(uint value, Span<byte> destination)
			{
				int num = 0;
				do
				{
					destination[num++] = (byte)(value | 0x80u);
				}
				while ((value >>= 7) != 0);
				destination[num - 1] &= 127;
				return num;
			}

			/// <summary>
			/// Calculate how many bytes it would take to write a given unsigned value as a variant.
			/// </summary>
			/// <param name="value"></param>
			/// <returns></returns>
			public static int CalcEncodedLength(uint value)
			{
				int num = 1;
				while ((value >>= 7) != 0)
				{
					num++;
				}
				return num;
			}
		}

		/// <summary>
		/// Statistics tracker used by the Default QoS provider.
		/// </summary>
		/// <remarks>Statistics trackers can be shared between multiple instances.</remarks>
		public abstract class Statistics
		{
			[Serializable]
			public struct SentTraffic
			{
				/// <summary>
				/// Statistics for message acknowledgement times.
				/// </summary>
				public struct Acknowledgement
				{
					public uint Count;

					public TimeSpan TotalTime;

					public TimeSpan Average
					{
						get
						{
							if (Count == 0)
							{
								return TimeSpan.Zero;
							}
							return new TimeSpan(TotalTime.Ticks / (long)Count);
						}
					}

					public static Acknowledgement operator +(in Acknowledgement lhs, in Acknowledgement rhs)
					{
						Acknowledgement result = default(Acknowledgement);
						result.Count = lhs.Count + rhs.Count;
						result.TotalTime = lhs.TotalTime + rhs.TotalTime;
						return result;
					}

					public static Acknowledgement operator -(in Acknowledgement lhs, in Acknowledgement rhs)
					{
						Acknowledgement result = default(Acknowledgement);
						result.Count = lhs.Count - rhs.Count;
						result.TotalTime = lhs.TotalTime - rhs.TotalTime;
						return result;
					}

					public void Add(TimeSpan ackDelay)
					{
						try
						{
							TotalTime += ackDelay;
							Count++;
						}
						catch (OverflowException)
						{
						}
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ESentTraffic_003C_003EMessage_003C_003EAccessor : IMemberAccessor<SentTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref SentTraffic owner, in Traffic value)
					{
						owner.Message = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref SentTraffic owner, out Traffic value)
					{
						value = owner.Message;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ESentTraffic_003C_003EMessageResend_003C_003EAccessor : IMemberAccessor<SentTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref SentTraffic owner, in Traffic value)
					{
						owner.MessageResend = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref SentTraffic owner, out Traffic value)
					{
						value = owner.MessageResend;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ESentTraffic_003C_003EAcknowledged_003C_003EAccessor : IMemberAccessor<SentTraffic, Acknowledgement>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref SentTraffic owner, in Acknowledgement value)
					{
						owner.Acknowledged = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref SentTraffic owner, out Acknowledgement value)
					{
						value = owner.Acknowledged;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ESentTraffic_003C_003EAck_003C_003EAccessor : IMemberAccessor<SentTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref SentTraffic owner, in Traffic value)
					{
						owner.Ack = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref SentTraffic owner, out Traffic value)
					{
						value = owner.Ack;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ESentTraffic_003C_003EPing_003C_003EAccessor : IMemberAccessor<SentTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref SentTraffic owner, in Traffic value)
					{
						owner.Ping = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref SentTraffic owner, out Traffic value)
					{
						value = owner.Ping;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ESentTraffic_003C_003EAckControl_003C_003EAccessor : IMemberAccessor<SentTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref SentTraffic owner, in Traffic value)
					{
						owner.AckControl = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref SentTraffic owner, out Traffic value)
					{
						value = owner.AckControl;
					}
				}

				public Traffic Message;

				public Traffic MessageResend;

				public Acknowledgement Acknowledged;

				public Traffic Ack;

				public Traffic Ping;

				public Traffic AckControl;

				public Traffic TotalControl
				{
					get
					{
						Traffic lhs = Ack + Ping;
						return lhs + AckControl;
					}
				}

				public Traffic TotalMessage => Message + MessageResend;

				public Traffic Total
				{
					get
					{
						Traffic lhs = TotalMessage;
						Traffic rhs = TotalControl;
						return lhs + rhs;
					}
				}

				/// <inheritdoc />
				public static SentTraffic operator +(in SentTraffic lhs, in SentTraffic rhs)
				{
					SentTraffic result = default(SentTraffic);
					result.Message = lhs.Message + rhs.Message;
					result.MessageResend = lhs.MessageResend + rhs.MessageResend;
					result.Acknowledged = lhs.Acknowledged + rhs.Acknowledged;
					result.Ack = lhs.Ack + rhs.Ack;
					result.Ping = lhs.Ping + rhs.Ping;
					result.AckControl = lhs.AckControl + rhs.AckControl;
					return result;
				}

				/// <inheritdoc />
				public static SentTraffic operator -(in SentTraffic lhs, in SentTraffic rhs)
				{
					SentTraffic result = default(SentTraffic);
					result.Message = lhs.Message - rhs.Message;
					result.MessageResend = lhs.MessageResend - rhs.MessageResend;
					result.Acknowledged = lhs.Acknowledged - rhs.Acknowledged;
					result.Ack = lhs.Ack - rhs.Ack;
					result.Ping = lhs.Ping - rhs.Ping;
					result.AckControl = lhs.AckControl - rhs.AckControl;
					return result;
				}
			}

			[Serializable]
			public struct ReceivedTraffic
			{
				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EReceivedTraffic_003C_003EMessage_003C_003EAccessor : IMemberAccessor<ReceivedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref ReceivedTraffic owner, in Traffic value)
					{
						owner.Message = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref ReceivedTraffic owner, out Traffic value)
					{
						value = owner.Message;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EReceivedTraffic_003C_003EAck_003C_003EAccessor : IMemberAccessor<ReceivedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref ReceivedTraffic owner, in Traffic value)
					{
						owner.Ack = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref ReceivedTraffic owner, out Traffic value)
					{
						value = owner.Ack;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EReceivedTraffic_003C_003EPing_003C_003EAccessor : IMemberAccessor<ReceivedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref ReceivedTraffic owner, in Traffic value)
					{
						owner.Ping = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref ReceivedTraffic owner, out Traffic value)
					{
						value = owner.Ping;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EReceivedTraffic_003C_003EAckControl_003C_003EAccessor : IMemberAccessor<ReceivedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref ReceivedTraffic owner, in Traffic value)
					{
						owner.AckControl = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref ReceivedTraffic owner, out Traffic value)
					{
						value = owner.AckControl;
					}
				}

				public Traffic Message;

				public Traffic Ack;

				public Traffic Ping;

				public Traffic AckControl;

				public Traffic TotalControl
				{
					get
					{
						Traffic lhs = Ack + Ping;
						return lhs + AckControl;
					}
				}

				public Traffic Total
				{
					get
					{
						ref Traffic message = ref Message;
						Traffic rhs = TotalControl;
						return message + rhs;
					}
				}

				/// <inheritdoc />
				public static ReceivedTraffic operator +(in ReceivedTraffic lhs, in ReceivedTraffic rhs)
				{
					ReceivedTraffic result = default(ReceivedTraffic);
					result.Message = lhs.Message + rhs.Message;
					result.Ack = lhs.Ack + rhs.Ack;
					result.Ping = lhs.Ping + rhs.Ping;
					result.AckControl = lhs.AckControl + rhs.AckControl;
					return result;
				}

				/// <inheritdoc />
				public static ReceivedTraffic operator -(in ReceivedTraffic lhs, in ReceivedTraffic rhs)
				{
					ReceivedTraffic result = default(ReceivedTraffic);
					result.Message = lhs.Message - rhs.Message;
					result.Ack = lhs.Ack - rhs.Ack;
					result.Ping = lhs.Ping - rhs.Ping;
					result.AckControl = lhs.AckControl - rhs.AckControl;
					return result;
				}
			}

			[Serializable]
			public struct DroppedTraffic
			{
				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EDroppedTraffic_003C_003EMessageOutOfOrder_003C_003EAccessor : IMemberAccessor<DroppedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref DroppedTraffic owner, in Traffic value)
					{
						owner.MessageOutOfOrder = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref DroppedTraffic owner, out Traffic value)
					{
						value = owner.MessageOutOfOrder;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EDroppedTraffic_003C_003EMessageQueueFull_003C_003EAccessor : IMemberAccessor<DroppedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref DroppedTraffic owner, in Traffic value)
					{
						owner.MessageQueueFull = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref DroppedTraffic owner, out Traffic value)
					{
						value = owner.MessageQueueFull;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EDroppedTraffic_003C_003EMessageWindowFull_003C_003EAccessor : IMemberAccessor<DroppedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref DroppedTraffic owner, in Traffic value)
					{
						owner.MessageWindowFull = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref DroppedTraffic owner, out Traffic value)
					{
						value = owner.MessageWindowFull;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003EDroppedTraffic_003C_003EControl_003C_003EAccessor : IMemberAccessor<DroppedTraffic, Traffic>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref DroppedTraffic owner, in Traffic value)
					{
						owner.Control = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref DroppedTraffic owner, out Traffic value)
					{
						value = owner.Control;
					}
				}

				public Traffic MessageOutOfOrder;

				public Traffic MessageQueueFull;

				public Traffic MessageWindowFull;

				public Traffic Control;

				public Traffic TotalMessage
				{
					get
					{
						Traffic lhs = MessageOutOfOrder + MessageQueueFull;
						return lhs + MessageWindowFull;
					}
				}

				public Traffic Total
				{
					get
					{
						ref Traffic control = ref Control;
						Traffic rhs = TotalMessage;
						return control + rhs;
					}
				}

				/// <inheritdoc />
				public static DroppedTraffic operator +(in DroppedTraffic lhs, in DroppedTraffic rhs)
				{
					DroppedTraffic result = default(DroppedTraffic);
					result.MessageOutOfOrder = lhs.MessageOutOfOrder + rhs.MessageOutOfOrder;
					result.MessageQueueFull = lhs.MessageQueueFull + rhs.MessageQueueFull;
					result.MessageWindowFull = lhs.MessageWindowFull + rhs.MessageWindowFull;
					result.Control = lhs.Control + rhs.Control;
					return result;
				}

				/// <inheritdoc />
				public static DroppedTraffic operator -(in DroppedTraffic lhs, in DroppedTraffic rhs)
				{
					DroppedTraffic result = default(DroppedTraffic);
					result.MessageOutOfOrder = lhs.MessageOutOfOrder - rhs.MessageOutOfOrder;
					result.MessageQueueFull = lhs.MessageQueueFull - rhs.MessageQueueFull;
					result.MessageWindowFull = lhs.MessageWindowFull - rhs.MessageWindowFull;
					result.Control = lhs.Control - rhs.Control;
					return result;
				}
			}

			public enum DroppedFrameType
			{
				OutOfOrder,
				WindowFull,
				QueueFull
			}

			/// <summary>
			/// Sample of network traffic.
			/// </summary>
			[Serializable]
			public struct Traffic
			{
				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ETraffic_003C_003EFrames_003C_003EAccessor : IMemberAccessor<Traffic, uint>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Traffic owner, in uint value)
					{
						owner.Frames = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Traffic owner, out uint value)
					{
						value = owner.Frames;
					}
				}

				protected class VRage_Library_Net_DefaultQoSProvider_003C_003EStatistics_003C_003ETraffic_003C_003EBytes_003C_003EAccessor : IMemberAccessor<Traffic, ulong>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Traffic owner, in ulong value)
					{
						owner.Bytes = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Traffic owner, out ulong value)
					{
						value = owner.Bytes;
					}
				}

				public uint Frames;

				public ulong Bytes;

				public Traffic(uint frames, ulong bytes)
				{
					Frames = frames;
					Bytes = bytes;
				}

				public void AddFrame(int length)
				{
					Frames++;
					Bytes += (uint)length;
				}

				public static Traffic operator +(in Traffic lhs, in Traffic rhs)
				{
					return new Traffic(lhs.Frames + rhs.Frames, lhs.Bytes + rhs.Bytes);
				}

				public static Traffic operator -(in Traffic lhs, in Traffic rhs)
				{
					return new Traffic(lhs.Frames - rhs.Frames, lhs.Bytes - rhs.Bytes);
				}

				public static TrafficF operator /(Traffic lhs, float length)
				{
					return new TrafficF((float)lhs.Frames / length, (double)lhs.Bytes / (double)length);
				}

				public bool Equals(Traffic other)
				{
					if (Frames == other.Frames)
					{
						return Bytes == other.Bytes;
					}
					return false;
				}

				/// <inheritdoc />
				public override bool Equals(object obj)
				{
					object obj2;
					if ((obj2 = obj) is Traffic)
					{
						Traffic other = (Traffic)obj2;
						return Equals(other);
					}
					return false;
				}

				/// <inheritdoc />
				public override int GetHashCode()
				{
					return (int)(Frames * 397) ^ Bytes.GetHashCode();
				}

				public static bool operator ==(Traffic left, Traffic right)
				{
					return left.Equals(right);
				}

				public static bool operator !=(Traffic left, Traffic right)
				{
					return !left.Equals(right);
				}

				/// <inheritdoc />
				public override string ToString()
				{
					return $"{Frames}, {Bytes}";
				}
			}

			/// <summary>
			/// Sample of network traffic.
			/// </summary>
			public struct TrafficF
			{
				public float Frames;

				public double Bytes;

				public TrafficF(float frames, double bytes)
				{
					Frames = frames;
					Bytes = bytes;
				}

				/// <inheritdoc />
				public override string ToString()
				{
					return $"{Frames}, {Bytes}";
				}
			}

			public abstract SentTraffic Sent { get; }

			public abstract ReceivedTraffic Received { get; }

			public abstract DroppedTraffic Dropped { get; }

			internal Statistics()
			{
			}

			/// <summary>
			/// Add all of the values of this instance into another.
			/// </summary>
			/// <param name="instance"></param>
			public void AddTo(Statistics instance)
			{
				AbsoluteStatistics absoluteStatistics;
				if ((absoluteStatistics = instance as AbsoluteStatistics) != null)
				{
					absoluteStatistics.AddFrom(this);
					return;
				}
				throw new InvalidOperationException("Accumulator instance is not absolute.");
			}

			public virtual void Tick()
			{
			}

			/// <summary>
			/// Create a statistics object that tracks an absolute total.
			/// </summary>
			/// <returns></returns>
			public static Statistics CreateAbsolute()
			{
				return new AbsoluteStatistics();
			}

			/// <summary>
			/// Create a statistics object that tracks the total traffic during a moving window of time with provided length.
			/// </summary>
			/// <param name="windowLength">Length of time to collect traffic for.</param>
			public static Statistics CreateMovingWindow(TimeSpan windowLength)
			{
				return new WindowedStatistics(windowLength);
			}

			public override string ToString()
			{
<<<<<<< HEAD
				return string.Join("\n", from x in EnumerateFields()
					select $"{x.Name}: {x.Stat}");
=======
				return string.Join("\n", Enumerable.Select<(string, object), string>(EnumerateFields(), (Func<(string, object), string>)(((string Name, object Stat) x) => $"{x.Name}: {x.Stat}")));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			public IEnumerable<(string Name, object Stat)> EnumerateFields()
			{
				SentTraffic sent = Sent;
				ReceivedTraffic received = Received;
				DroppedTraffic dropped = Dropped;
				yield return ("Sent", sent.Total);
				yield return ("\tMessage", sent.Message);
				yield return ("\t\tRe-Sent", sent.MessageResend);
				yield return ("\t\tAcknowledged", sent.Acknowledged);
				yield return ("\tControl", sent.TotalControl);
				yield return ("\t\tAck", sent.Ack);
				yield return ("\t\tAckControl", sent.AckControl);
				yield return ("\t\tPing", sent.Ping);
				yield return ("Received", received.Total);
				yield return ("\tMessage", received.Message);
				yield return ("\tControl", received.TotalControl);
				yield return ("\t\tAck", received.Ack);
				yield return ("\t\tAckControl", received.AckControl);
				yield return ("\t\tPing", received.Ping);
				yield return ("Dropped", dropped.Total);
				yield return ("\tMessage", dropped.TotalMessage);
				yield return ("\t\tQueue Full", dropped.MessageQueueFull);
				yield return ("\t\tWindow Full", dropped.MessageWindowFull);
				yield return ("\t\tOut of Order", dropped.MessageOutOfOrder);
				yield return ("\tControl", dropped.Control);
			}
		}

		private abstract class StatisticsInternal : Statistics
		{
			protected void AddFrameSent(ref SentTraffic sentTraffic, int length, FrameType type, bool resend)
			{
				switch (type)
				{
				case FrameType.Message:
					if (resend)
					{
						sentTraffic.MessageResend.AddFrame(length);
					}
					else
					{
						sentTraffic.Message.AddFrame(length);
					}
					break;
				case FrameType.AckControl:
					sentTraffic.AckControl.AddFrame(length);
					break;
				case FrameType.Ack:
					sentTraffic.Ack.AddFrame(length);
					break;
				case FrameType.Ping:
					sentTraffic.Ping.AddFrame(length);
					break;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
				}
			}

			protected void AddFrameReceived(ref ReceivedTraffic received, int length, FrameType type)
			{
				switch (type)
				{
				case FrameType.Message:
					received.Message.AddFrame(length);
					break;
				case FrameType.AckControl:
					received.AckControl.AddFrame(length);
					break;
				case FrameType.Ack:
					received.Ack.AddFrame(length);
					break;
				case FrameType.Ping:
					received.Ping.AddFrame(length);
					break;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
				}
			}

			protected void AddFrameAcknowledged(ref SentTraffic received, TimeSpan ackDelay)
			{
				received.Acknowledged.Add(ackDelay);
			}

			protected void DropFrame(ref DroppedTraffic dropped, int length, bool message, DroppedFrameType type)
			{
				if (message)
				{
					switch (type)
					{
					case DroppedFrameType.OutOfOrder:
						dropped.MessageOutOfOrder.AddFrame(length);
						break;
					case DroppedFrameType.WindowFull:
						dropped.MessageWindowFull.AddFrame(length);
						break;
					case DroppedFrameType.QueueFull:
						dropped.MessageQueueFull.AddFrame(length);
						break;
					default:
						throw new ArgumentOutOfRangeException("type", type, null);
					}
				}
				else
				{
					dropped.Control.AddFrame(length);
				}
			}

			public abstract void AddFrameSent(int length, FrameType type, bool resend = false);

			public abstract void AddFrameAcknowledged(TimeSpan ackDelay);

			public abstract void AddFrameReceived(int length, FrameType type);

			public abstract void DropFrame(int length, bool message, DroppedFrameType type);
		}

		private class AbsoluteStatistics : StatisticsInternal
		{
			private SentTraffic m_sent;

			private ReceivedTraffic m_received;

			private DroppedTraffic m_dropped;

			/// <inheritdoc />
			public override SentTraffic Sent => m_sent;

			/// <inheritdoc />
			public override ReceivedTraffic Received => m_received;

			/// <inheritdoc />
			public override DroppedTraffic Dropped => m_dropped;

			public void AddFrom(Statistics stats)
			{
				ref SentTraffic sent = ref m_sent;
				SentTraffic rhs = stats.Sent;
				m_sent = sent + rhs;
				ref ReceivedTraffic received = ref m_received;
				ReceivedTraffic rhs2 = stats.Received;
				m_received = received + rhs2;
				ref DroppedTraffic dropped = ref m_dropped;
				DroppedTraffic rhs3 = stats.Dropped;
				m_dropped = dropped + rhs3;
			}

			/// <inheritdoc />
			public override void AddFrameSent(int length, FrameType type, bool resend = false)
			{
				AddFrameSent(ref m_sent, length, type, resend);
			}

			/// <inheritdoc />
			public override void AddFrameAcknowledged(TimeSpan ackDelay)
			{
				AddFrameAcknowledged(ref m_sent, ackDelay);
			}

			/// <inheritdoc />
			public override void AddFrameReceived(int length, FrameType type)
			{
				AddFrameReceived(ref m_received, length, type);
			}

			/// <inheritdoc />
			public override void DropFrame(int length, bool message, DroppedFrameType type)
			{
				DropFrame(ref m_dropped, length, message, type);
			}
		}

		private class WindowedStatistics : StatisticsInternal
		{
			private struct StatFrame : MyTimedStatWindow.IStatArithmetic<StatFrame>
			{
				public SentTraffic Sent;

				public ReceivedTraffic Received;

				public DroppedTraffic Dropped;

				/// <inheritdoc />
				public void Add(in StatFrame lhs, in StatFrame rhs, out StatFrame result)
				{
					result.Sent = lhs.Sent + rhs.Sent;
					result.Received = lhs.Received + rhs.Received;
					result.Dropped = lhs.Dropped + rhs.Dropped;
				}

				/// <inheritdoc />
				public void Subtract(in StatFrame lhs, in StatFrame rhs, out StatFrame result)
				{
					result.Sent = lhs.Sent - rhs.Sent;
					result.Received = lhs.Received - rhs.Received;
					result.Dropped = lhs.Dropped - rhs.Dropped;
				}

				void MyTimedStatWindow.IStatArithmetic<StatFrame>.Add(in StatFrame lhs, in StatFrame rhs, out StatFrame result)
				{
					Add(in lhs, in rhs, out result);
				}

				void MyTimedStatWindow.IStatArithmetic<StatFrame>.Subtract(in StatFrame lhs, in StatFrame rhs, out StatFrame result)
				{
					Subtract(in lhs, in rhs, out result);
				}
			}

			private MyTimedStatWindow<StatFrame> m_window;

			/// <inheritdoc />
			public override SentTraffic Sent => m_window.Total.Sent;

			/// <inheritdoc />
			public override ReceivedTraffic Received => m_window.Total.Received;

			/// <inheritdoc />
			public override DroppedTraffic Dropped => m_window.Total.Dropped;

			private ref StatFrame Frame => ref m_window.Current;

			/// <inheritdoc />
			public WindowedStatistics(TimeSpan maxTime)
			{
				m_window = new MyTimedStatWindow<StatFrame>(maxTime, default(StatFrame));
			}

			/// <inheritdoc />
			public override void AddFrameSent(int length, FrameType type, bool resend = false)
			{
				AddFrameSent(ref Frame.Sent, length, type, resend);
			}

			/// <inheritdoc />
			public override void AddFrameAcknowledged(TimeSpan ackDelay)
			{
				AddFrameAcknowledged(ref Frame.Sent, ackDelay);
			}

			/// <inheritdoc />
			public override void AddFrameReceived(int length, FrameType type)
			{
				AddFrameReceived(ref Frame.Received, length, type);
			}

			/// <inheritdoc />
			public override void DropFrame(int length, bool message, DroppedFrameType type)
			{
				DropFrame(ref Frame.Dropped, length, message, type);
			}

			/// <inheritdoc />
			public override void Tick()
			{
				StatFrame total = m_window.Total;
<<<<<<< HEAD
				StatFrame statFrame = m_window.Aggregate(delegate(StatFrame lhs, StatFrame rhs)
=======
				StatFrame statFrame = Enumerable.Aggregate<StatFrame>((IEnumerable<StatFrame>)m_window, (Func<StatFrame, StatFrame, StatFrame>)delegate(StatFrame lhs, StatFrame rhs)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					StatFrame result2 = default(StatFrame);
					result2.Sent = lhs.Sent + rhs.Sent;
					result2.Received = lhs.Received + rhs.Received;
					result2.Dropped = lhs.Dropped + rhs.Dropped;
					return result2;
				});
				m_window.Advance();
				total = m_window.Total;
<<<<<<< HEAD
				statFrame = m_window.Aggregate(delegate(StatFrame lhs, StatFrame rhs)
=======
				statFrame = Enumerable.Aggregate<StatFrame>((IEnumerable<StatFrame>)m_window, (Func<StatFrame, StatFrame, StatFrame>)delegate(StatFrame lhs, StatFrame rhs)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					StatFrame result = default(StatFrame);
					result.Sent = lhs.Sent + rhs.Sent;
					result.Received = lhs.Received + rhs.Received;
					result.Dropped = lhs.Dropped + rhs.Dropped;
					return result;
				});
			}
		}

		/// <summary>
		/// Use a separate sequence number for control frames.
		/// </summary>
		private uint m_lastSentControlSequence;

		/// <summary>
		/// Last sent sequence number for a Ping frame.
		/// </summary>
		private uint m_lastReceivedControlSequence;

		/// <summary>
		/// Whether to include ping messages in the trace.
		/// </summary>
		public static bool TraceControlFrames = false;

		/// <summary>
		/// Calculated by an exponential moving average of the last few samples.
		/// </summary>
		private TimeSpan m_averageEstimateRtt = TimeSpan.Zero;

		/// <summary>
		/// Calculated by an exponential moving average of the last few samples.
		/// </summary>
		private TimeSpan m_averageEstimateRttVariation = TimeSpan.Zero;

		/// <summary>
		/// Timestamp of the last sent ping.
		/// </summary>
		private TimeSpan m_lastSentPingTime = -MinPingInterval;

		/// <summary>
		/// Timestamp of the last sent ping.
		/// </summary>
		private TimeSpan m_lastReceivedPingTime = TimeSpan.Zero;

		/// <summary>
		/// Minimum interval between pings.
		/// </summary>
		private static readonly TimeSpan MinPingInterval = TimeSpan.FromMilliseconds(100.0);

		/// <summary>
		/// Minimum ping value we accept to use.
		/// </summary>
		/// <remarks>This allows us to protect against explosive packet transfers on the same network or machine.</remarks>
		private static readonly TimeSpan MinPing = TimeSpan.FromMilliseconds(1.0);

		/// <summary>
		/// Allocator for all the native arrays.
		/// </summary>
		private static readonly NativeArrayAllocator NativeArrayAllocator = new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("VRage.Net.DefaultQoSProvider"));

		/// <summary>
		/// Settings for the current sliding window.
		/// </summary>
		private ProcessedSettings m_settings;

		/// <summary>
		/// Buffer used to read and write control messages.
		/// </summary>
		private readonly NativeArray m_controlBuffer;

		/// <summary>
		/// The underlying transport.
		/// </summary>
		private readonly IUnreliableTransportChannel m_transport;

		/// <summary>
		/// Our time counter.
		/// </summary>
		private readonly Stopwatch m_time;

		/// <summary>
		/// Sliding window of frames received from the remote host.
		/// </summary>
		private CombinedBuffer<ReceivedFrame> m_receivingBuffer;

		/// <summary>
		/// Pointer to the first frame in the sliding window that is missing.
		/// </summary>
		private WindowPointer m_receivingWindowHead;

		/// <summary>
		/// Cursor into the oldest unprocessed message frame.
		/// </summary>
		private int m_receivedFrameCursor;

		/// <summary>
		/// Last Ack packet send time.
		/// </summary>
		private TimeSpan m_lastAckSent = TimeSpan.Zero;

		/// <summary>
		/// Timeout to use for frame resending.
		/// </summary>
		/// <remarks>While RTT is still not calculated we will use the minimum ping interval.</remarks>
		private TimeSpan FrameTimeout = MinPingInterval;

		/// <summary>
		/// Sliding window for frames destined to the remote host.
		/// </summary>
		private CombinedBuffer<SendingFrame> m_sendingBuffer;

		/// <summary>
		/// Write cursor into the last unsent frame.
		/// </summary>
		private int m_unsentFrameCursor;

		/// <summary>
		/// Index of the next frame we'll be resending once it times out.
		/// </summary>
		private int m_nextResendFrame;

		/// <summary>
		/// Pointer to the oldest frame in the sliding window.
		/// </summary>
		private WindowPointer m_sendingHead;

		/// <summary>
		/// Position of the first frame in the send queue.
		/// </summary>
		private int m_firstSendQueueFrame;

		private TimeSpan m_averageAckTime = TimeSpan.Zero;

		/// <summary>
		/// Total number of frames sent (successfully or not) since the last window resize.
		/// </summary>
		private int m_framesSent;

		/// <summary>
		/// Number of frames lost since the last window resize.
		/// </summary>
		private int m_framesLost;

		/// <summary>
		/// Maximum window length since the last adjustment.
		/// </summary>
		/// <remarks>This is used to prevent idle connections from reaching maximum window size unnecessarily.</remarks>
		private int m_maxWindowSize;

		/// <summary>
		/// The target size for the sending window capacity.
		/// </summary>
		private float m_targetSendingWindowCapacity;

		/// <summary>
		/// Growth factor for the sliding window. Adjusted based on packet loss.
		/// </summary>
		private float m_windowGrowthFactor;

		/// <summary>
		/// Time since we last attempted to adjust the window size.
		/// </summary>
		private TimeSpan m_timeSinceLastWindowAdjustment = TimeSpan.Zero;

		/// <summary>
		/// Stats instance.
		/// </summary>
		private StatisticsInternal m_statistics;

		private static readonly ITrace m_trace = MyTrace.GetTrace(TraceWindow.NetworkQoS);

		private static int m_idCtr;

		private string m_id;

		/// <summary>
		/// Estimated Round-Trip-Time.
		/// </summary>
		public TimeSpan EstimateRTT => m_averageEstimateRtt;

		/// <summary>
		/// Variation in the Estimated Round-Trip-Time.
		/// </summary>
		public TimeSpan EstimateRTTVariation => m_averageEstimateRttVariation;

		/// <inheritdoc />
		public int MaxNonBlockingMessageSize => m_settings.MaximumNonBlockingMessageSize;

		/// <summary>
		/// Get a copy of the settings used by this provider.
		/// </summary>
		public ProcessedSettings Settings => m_settings;

		/// <inheritdoc />
		public bool MessagesAvailable
		{
			get
			{
				CheckDisposed();
				return SeekToMessage();
			}
		}

		/// <inheritdoc />
		public bool HasPendingDeliveries
		{
			get
			{
				CheckDisposed();
				return m_sendingBuffer.UsedLength > 0;
			}
		}

		/// <summary>
		/// Average time between a message being sent and it being acknowledged.
		/// </summary>
		public TimeSpan AverageAckTime => m_averageAckTime;

		/// <summary>
		/// Ratio of frames lost.
		/// </summary>
		public float FrameLoss
		{
			get
			{
				if (m_framesSent != 0)
				{
					return (float)m_framesLost / (float)m_framesSent;
				}
				return 0f;
			}
		}

		/// <summary>
		/// Current size of the sending sliding window.
		/// </summary>
		public int SlidingWindowSize => (int)m_targetSendingWindowCapacity;

		/// <summary>
		/// Current size of the sending queue window.
		/// </summary>
		public int QueueSize => m_sendingBuffer.Distance(m_firstSendQueueFrame, m_sendingBuffer.Tail);

		/// <summary>
		/// Start a new control frame of given type.
		/// </summary>
		/// <remarks>Control frames are recorded using <see cref="F:VRage.Library.Net.DefaultQoSProvider.m_controlBuffer" />, make sure to never start two control frames simultaneously.</remarks>
		/// <param name="type">The type of the control frame.</param>
		/// <param name="frameData">The frame data.</param>
		private void GetNewControlFrame(FrameType type, out Span<byte> frameData)
		{
			uint sequence = ++m_lastSentControlSequence;
			frameData = m_controlBuffer.AsSpan();
			Span<byte> destination = frameData;
			FrameHeader data = new FrameHeader(sequence, type);
			Write(destination, in data);
		}

		/// <summary>
		/// Process a control frame received from the remote.
		/// </summary>
		/// <param name="header">The header for the frame.</param>
		/// <param name="frameData">The frame data including the header.</param>
		private void ProcessControlFrame(in FrameHeader header, Span<byte> frameData)
		{
			if (header.Sequence > m_lastReceivedControlSequence || (ulong)((long)header.Sequence + 4294967295L - m_lastReceivedControlSequence) < 10000uL)
			{
				m_lastReceivedControlSequence = header.Sequence;
				switch (header.Type)
				{
				case FrameType.Message:
					throw new ArgumentException("Messages frames should not be handled here.", "header");
				case FrameType.AckControl:
					ProcessAckControl(frameData);
					break;
				case FrameType.Ack:
					ProcessAcks(frameData);
					break;
				case FrameType.Ping:
					ProcessPing(frameData);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				m_statistics?.AddFrameReceived(frameData.Length, header.Type);
			}
			else
			{
				m_statistics?.DropFrame(frameData.Length, message: false, Statistics.DroppedFrameType.OutOfOrder);
			}
		}

		/// <summary>
		/// Send a message frame to the remote.
		/// </summary>
		/// <param name="frame"></param>
		/// <param name="type"></param>
		private void SendControlFrame(Span<byte> frame, FrameType type)
		{
			m_statistics?.AddFrameSent(frame.Length, type);
			m_transport.Send(frame);
		}

		/// <summary>
		/// Process an ack control frame.
		/// </summary>
		/// <param name="frameData"></param>
		private void ProcessAckControl(Span<byte> frameData)
		{
			int size = FrameHeader.Size;
			switch (Access<FrameHeader>(frameData.Slice(size)).Type)
			{
			case FrameType.Ping:
				ProcessAckPing(frameData.Slice(size));
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case FrameType.Message:
			case FrameType.AckControl:
			case FrameType.Ack:
				break;
			}
		}

		/// <summary>
		/// Send a frame acknowledging a given received control frame.
		/// </summary>
		/// <param name="originalControlFrame"></param>
		private void SendAckControl(Span<byte> originalControlFrame)
		{
			_ = TraceControlFrames;
			GetNewControlFrame(FrameType.AckControl, out var frameData);
			originalControlFrame.CopyTo(frameData.Slice(FrameHeader.Size));
			frameData = frameData.Slice(0, FrameHeader.Size + originalControlFrame.Length);
			SendControlFrame(frameData, FrameType.AckControl);
		}

		/// <summary>
		/// Check whether it's time to send a ping, and if so send it.
		/// </summary>
		private void CheckAndSendPing()
		{
<<<<<<< HEAD
			TimeSpan timeSpan;
			if (m_averageEstimateRtt == TimeSpan.Zero)
			{
				timeSpan = MinPingInterval;
			}
			else
			{
				long ticks = m_averageEstimateRtt.Ticks;
				TimeSpan minPingInterval = MinPingInterval;
				timeSpan = new TimeSpan(Math.Max(ticks, minPingInterval.Ticks));
			}
=======
			TimeSpan timeSpan = ((!(m_averageEstimateRtt == TimeSpan.Zero)) ? new TimeSpan(Math.Max(m_averageEstimateRtt.Ticks, MinPingInterval.Ticks)) : MinPingInterval);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TimeSpan time = GetTime();
			if (!(time - m_lastSentPingTime > timeSpan))
			{
				return;
			}
			if (m_lastSentPingTime == TimeSpan.Zero)
			{
				TimeSpan elapsed = time - m_lastReceivedPingTime;
				if (m_averageEstimateRtt < new TimeSpan((long)((double)elapsed.Ticks * 1.5)))
				{
					_ = TraceControlFrames;
					UpdateRtt(elapsed);
				}
			}
			SendPing();
		}

		/// <summary>
		/// Send a ping frame to the remote.
		/// </summary>
		private void SendPing()
		{
			GetNewControlFrame(FrameType.Ping, out var frameData);
			m_lastSentPingTime = (Access<PingFrame>(frameData).Timestamp = GetTime());
			frameData = frameData.Slice(0, PingFrame.Size);
			SendControlFrame(frameData, FrameType.Ping);
			_ = TraceControlFrames;
		}

		/// <summary>
		/// Process a received ping frame.
		/// </summary>
		/// <param name="frameData"></param>
		private void ProcessPing(Span<byte> frameData)
		{
			SendAckControl(frameData);
		}

		/// <summary>
		/// Process a bounced back ping frame.
		/// </summary>
		/// <param name="pingFrameData"></param>
		private void ProcessAckPing(Span<byte> pingFrameData)
		{
			ref PingFrame reference = ref Access<PingFrame>(pingFrameData);
			TimeSpan time = GetTime();
			TimeSpan elapsed = time - reference.Timestamp;
			UpdateRtt(elapsed);
			m_lastReceivedPingTime = time;
		}

		/// <summary>
		/// Calculate a new updated average RTT based on a new estimate.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateRtt(TimeSpan elapsed)
		{
			TimeSpan averageEstimateRtt;
			TimeSpan averageEstimateRttVariation;
			if (m_averageEstimateRtt == TimeSpan.Zero)
			{
				averageEstimateRtt = elapsed;
				averageEstimateRttVariation = TimeSpan.Zero;
			}
			else
			{
				averageEstimateRttVariation = ((!(m_averageEstimateRttVariation == TimeSpan.Zero)) ? UpdateAverage(m_averageEstimateRttVariation, (m_averageEstimateRtt - elapsed).Duration()) : (m_averageEstimateRtt - elapsed).Duration());
				averageEstimateRtt = Max(UpdateAverage(m_averageEstimateRtt, elapsed), MinPing);
			}
			_ = TraceControlFrames;
			m_averageEstimateRtt = averageEstimateRtt;
			m_averageEstimateRttVariation = averageEstimateRttVariation;
			FrameTimeout = new TimeSpan((long)((float)m_averageEstimateRtt.Ticks * m_settings.FrameTimeoutSlipFactor));
		}

		public DefaultQoSProvider(IUnreliableTransportChannel transport, InitSettings? settings = null)
		{
			if (!BitConverter.IsLittleEndian)
			{
				throw new NotImplementedException("The default QoS provider is not prepared to handle operation in BigEndian systems.");
			}
			m_transport = transport;
			int minimumMTU = transport.MinimumMTU;
			settings = settings ?? InitSettings.Default;
			InitSettings settings2 = settings.Value;
			m_settings = ProcessedSettings.Create(in settings2, transport);
			int minimumWindowCapacity = m_settings.MinimumWindowCapacity;
			int minimumQueueCapacity = m_settings.MinimumQueueCapacity;
			m_receivingBuffer = new CombinedBuffer<ReceivedFrame>(minimumWindowCapacity, minimumQueueCapacity, minimumMTU, m_settings.ClearBuffers);
			m_sendingBuffer = new CombinedBuffer<SendingFrame>(minimumWindowCapacity, minimumQueueCapacity, minimumMTU, m_settings.ClearBuffers);
			m_sendingHead = (m_receivingWindowHead = new WindowPointer(settings.Value.InitialSequenceNumber, 0));
			m_controlBuffer = NativeArrayAllocator.Allocate(minimumMTU);
			m_targetSendingWindowCapacity = m_settings.MinimumWindowCapacity;
			m_windowGrowthFactor = m_settings.WindowGrowthFactor;
			m_time = Stopwatch.StartNew();
		}

		/// <inheritdoc />
		public void ProcessReadQueues()
		{
			CheckDisposed();
			PollAvailableFrames();
		}

		/// <inheritdoc />
		public void ProcessWriteQueues()
		{
			if (m_unsentFrameCursor > 0)
			{
				GetNextFrame(out var frame);
				FinishFrame(in frame);
			}
			ProcessSendQueue();
		}

		/// <summary>
		/// Get the current relative time.
		/// </summary>
		/// <remarks>This is only ever used to time RTT and to determine packet re-send timeouts, therefore we don't need universal time.</remarks>
		/// <returns></returns>
		private TimeSpan GetTime()
		{
<<<<<<< HEAD
			return m_time.Elapsed;
=======
			return m_time.get_Elapsed();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// We waited too long and the remote host has not replied or sending a particular packet has failed too many times.
		/// </summary>
		private void TimedOut()
		{
			throw new TimeoutException("Communication with the remote host has exceeded the user defined number of retries/time.");
		}

		/// <summary>
		/// Compute the ideal new size for an object given the current, desired, and maximum values.
		/// </summary>
		/// <returns>
		/// This method assumes the desired size is not larger than the maximum size.
		/// It also attempts to emulate the amortized constant time, quadratic scaling method commonly used for vectors/lists.
		/// </returns>
		/// <param name="current">Current size of the object.</param>
		/// <param name="max">Maximum size of the object.</param>
		/// <param name="desired">Desired size.</param>
		/// <returns>The computed size.</returns>
		private int ComputeResize(int current, int max, int desired)
		{
			while (current < desired)
			{
				current <<= 1;
			}
			return Math.Min(current, max);
		}

		/// <summary>
		/// Update an exponential moving average with a new value.
		/// </summary>
		/// <param name="average"></param>
		/// <param name="newValue"></param>
<<<<<<< HEAD
		/// <param name="factor"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		private TimeSpan UpdateAverage(TimeSpan average, TimeSpan newValue, float factor = 0.1f)
		{
			return TimeSpan.FromTicks((long)((float)average.Ticks + (float)(newValue.Ticks - average.Ticks) * factor));
		}

		/// <summary>
		/// Return the timespan with the highest tick count between the arguments.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		private TimeSpan Max(TimeSpan lhs, TimeSpan rhs)
		{
			return new TimeSpan(Math.Max(lhs.Ticks, rhs.Ticks));
		}

		private void CheckDisposed()
		{
			if (m_controlBuffer.IsDisposed)
			{
				throw new ObjectDisposedException("DefaultQoSProvider");
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			CheckDisposed();
			m_controlBuffer.Dispose();
			m_sendingBuffer.Dispose();
			m_receivingBuffer.Dispose();
		}

		/// <inheritdoc />
		public bool TryReceiveMessage(ref Span<byte> message, out byte channel)
		{
			CheckDisposed();
			if (!SeekToMessage())
			{
				channel = 0;
				return false;
			}
			Span<byte> headFrame = GetHeadFrame();
			int headerLength;
			MessageHeader messageHeader = MessageHeader.Read(headFrame.Slice(m_receivedFrameCursor), out headerLength);
			if (!IsComplete(m_receivedFrameCursor, messageHeader.Length, headerLength))
			{
				channel = 0;
				return false;
			}
			bool flag = messageHeader.Length > Settings.MaximumNonBlockingMessageSize;
<<<<<<< HEAD
=======
			(uint, int) tuple = (Access<FrameHeader>(headFrame).Sequence, m_receivedFrameCursor);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_receivedFrameCursor += headerLength;
			uint length = messageHeader.Length;
			int num = 0;
			int num2 = 0;
			do
			{
				headFrame = GetHeadFrame();
				int num3 = (int)Math.Min(length - num, headFrame.Length - m_receivedFrameCursor);
				int num4 = Math.Min(num3, message.Length - num);
				if (num4 > 0)
				{
					headFrame.Slice(m_receivedFrameCursor, num4).CopyTo(message.Slice(num));
				}
				m_receivedFrameCursor += num3;
				num += num3;
				num2 += num4;
<<<<<<< HEAD
=======
				(uint, int) tuple2 = (Access<FrameHeader>(headFrame).Sequence, m_receivedFrameCursor);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (headFrame.Length == m_receivedFrameCursor)
				{
					FinishReadFrame();
					m_receivedFrameCursor = FrameHeader.Size;
				}
				if (flag && m_receivingBuffer.Head == m_receivingWindowHead.Position && num < length)
				{
					SendAcks();
					WaitForNewFrame();
				}
			}
			while (num < length);
			message = message.Slice(0, num2);
			channel = messageHeader.Channel;
			return true;
		}

		private void FinishReadFrame()
		{
			m_receivingBuffer[m_receivingBuffer.Head].SetMissing();
			m_receivingBuffer.AdvanceHead(m_settings.ClearBuffers);
		}

		/// <inheritdoc />
		public bool TryPeekMessage(out int size, out byte channel)
		{
			CheckDisposed();
			if (!SeekToMessage())
			{
				size = 0;
				channel = 0;
				return false;
			}
			int headerLength;
			MessageHeader messageHeader = MessageHeader.Read(GetHeadFrame().Slice(m_receivedFrameCursor), out headerLength);
			bool flag = messageHeader.Length > Settings.MaximumNonBlockingMessageSize;
			if (!IsComplete(m_receivedFrameCursor, messageHeader.Length, headerLength) && !flag)
			{
				channel = 0;
				size = 0;
				return false;
			}
			size = (int)messageHeader.Length;
			channel = messageHeader.Channel;
			return true;
		}

		private Span<byte> GetHeadFrame()
		{
			return m_receivingBuffer.GetFrame(m_receivingBuffer.Head).Slice(0, m_receivingBuffer[m_receivingBuffer.Head].Length);
		}

		/// <summary>
		/// Whether a given message is fully available given the current set of available frames.
		/// </summary>
		/// <param name="messageStartOffset">Offset to the start of the message contents.</param>
		/// <param name="messageSize">The size of the message.</param>
		/// <param name="headerLength">The number of bytes occupied by the message header./</param>
		/// <returns></returns>
		private bool IsComplete(int messageStartOffset, uint messageSize, int headerLength)
		{
			if (messageSize > m_settings.MaximumNonBlockingMessageSize)
			{
				return true;
			}
			int num = m_receivingBuffer.FrameSize - messageStartOffset - headerLength;
			int num2 = m_receivingBuffer.FrameSize - FrameHeader.Size;
			long num3 = (messageSize - num + num2 - 1) / num2 + 1;
			return m_receivingBuffer.Distance(m_receivingBuffer.Head, in m_receivingWindowHead) >= num3;
		}

		private bool SeekToMessage()
		{
			PollAvailableFrames();
			if (m_receivingWindowHead.Position == m_receivingBuffer.Head)
			{
				return false;
			}
			if (m_receivedFrameCursor == 0)
			{
				m_receivedFrameCursor = FrameHeader.Size;
			}
			return true;
		}

		/// <summary>
		/// Pulls the waiting frames out of the unreliable network and stores it properly.
		/// </summary>
		private bool PollAvailableFrames()
		{
			bool flag = false;
			bool flag2 = false;
			int frameSize;
			while (m_transport.PeekFrame(out frameSize) && !flag2)
			{
				Span<byte> frame = m_receivingBuffer.GetFrame(m_receivingWindowHead.Position);
				if (!m_transport.TryGetFrame(ref frame))
				{
					continue;
				}
				FrameHeader header = Access<FrameHeader>(frame);
				if (header.Type == FrameType.Message)
				{
					flag2 = ReceiveMessageFrame(header, frame);
					if (GetTime() - m_lastAckSent > FrameTimeout)
					{
						SendAcks();
						flag = false;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					ProcessControlFrame(in header, frame);
				}
			}
			if (flag)
			{
				SendAcks();
			}
			CheckAndSendPing();
			return flag2;
		}

		private void WaitForNewFrame()
		{
			int position = m_receivingWindowHead.Position;
			while (m_receivingWindowHead.Position == position)
			{
				PollAvailableFrames();
			}
		}

		/// <summary>
		/// Process a received message frame.
		/// </summary>
		/// <param name="header"></param>
<<<<<<< HEAD
		/// <param name="frame"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool ReceiveMessageFrame(FrameHeader header, Span<byte> frame)
		{
			if (m_receivingWindowHead.Position == m_receivingBuffer.Tail)
			{
				m_receivingBuffer.AdvanceTail();
			}
			if (m_receivingBuffer.Distance(m_receivingBuffer.Head, in m_receivingWindowHead) >= m_receivingBuffer.QueueCapacity)
			{
				if (m_receivingBuffer.QueueCapacity == m_settings.MaximumQueueCapacity)
				{
					m_statistics?.DropFrame(frame.Length, message: true, Statistics.DroppedFrameType.QueueFull);
					return true;
				}
				int queueCapacity = ComputeResize(m_receivingBuffer.QueueCapacity, m_settings.MaximumQueueCapacity, m_receivingBuffer.QueueCapacity + 1);
				ResizeReceivingBuffer(m_receivingBuffer.WindowCapacity, queueCapacity);
				frame = m_receivingBuffer.GetFrame(m_receivingWindowHead.Position).Slice(0, frame.Length);
				header = Access<FrameHeader>(frame);
			}
			uint num = header.Sequence + (uint)(int)(0L - (long)m_receivingWindowHead.Sequence);
			if (num > m_settings.MaximumWindowCapacity)
			{
				if (m_statistics != null)
				{
					if ((int)num > m_settings.MaximumWindowCapacity)
					{
						m_statistics.DropFrame(frame.Length, message: true, Statistics.DroppedFrameType.WindowFull);
					}
					else
					{
						m_statistics.DropFrame(frame.Length, message: true, Statistics.DroppedFrameType.OutOfOrder);
					}
				}
				return false;
			}
			if (header.Sequence != m_receivingWindowHead.Sequence)
			{
				if (num > m_receivingBuffer.WindowCapacity)
				{
					int windowCapacity = ComputeResize(m_receivingBuffer.WindowCapacity, m_settings.MaximumWindowCapacity, (int)num);
					ResizeReceivingBuffer(windowCapacity, m_receivingBuffer.QueueCapacity);
				}
				int num2 = m_receivingBuffer.Advance(m_receivingWindowHead.Position, (int)num);
				if (!CircularMapping.IsInRange(m_receivingWindowHead.Position, m_receivingBuffer.Tail, num2))
				{
					m_receivingBuffer.AdvanceTail(m_receivingBuffer.Distance(m_receivingBuffer.Tail, num2 + 1));
				}
				m_receivingBuffer.CopyFrame(m_receivingWindowHead.Position, num2);
				m_receivingBuffer[num2].SetReceived(frame.Length);
			}
			else
			{
				m_receivingBuffer[m_receivingWindowHead.Position].SetReceived(frame.Length);
				int num3 = 0;
				foreach (int item in m_receivingBuffer.EnumerateFrames(m_receivingWindowHead.Position, m_receivingBuffer.Tail))
				{
					if (m_receivingBuffer[item].State != ReceiveState.Received)
					{
						break;
					}
					m_receivingBuffer[item].State = ReceiveState.Unprocessed;
					num3++;
				}
				m_receivingWindowHead = m_receivingBuffer.Advance(m_receivingWindowHead, num3);
			}
			foreach (int item2 in m_receivingBuffer.EnumerateFrames(m_receivingBuffer.Head, m_receivingWindowHead.Position))
			{
			}
			m_statistics?.AddFrameReceived(frame.Length, FrameType.Message);
			return false;
		}

		/// <summary>
		/// Resize the receiving buffer and update pointers.
		/// </summary>
		/// <remarks>All pointers into the buffer become invalid after this call.</remarks>
		/// <param name="windowCapacity"></param>
		/// <param name="queueCapacity"></param>
		private void ResizeReceivingBuffer(int windowCapacity, int queueCapacity)
		{
			int position = m_receivingBuffer.Distance(m_receivingBuffer.Head, in m_receivingWindowHead);
			m_receivingBuffer.Resize(windowCapacity, queueCapacity, m_settings.ClearBuffers);
			m_receivingWindowHead = new WindowPointer(m_receivingWindowHead.Sequence, position);
		}

		private void SendAcks()
		{
			GetNewControlFrame(FrameType.Ack, out var frameData);
			WriteAckFrame(ref frameData);
			_ = m_trace.Enabled;
			SendControlFrame(frameData, FrameType.Ack);
			m_lastAckSent = GetTime();
		}

		/// <summary>
		/// Write an ack frame.
		/// </summary>
<<<<<<< HEAD
		/// <param name="frame"></param>        
=======
		/// <param name="frame"></param>
		/// <param name="frameWriteHead"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void WriteAckFrame(ref Span<byte> frame)
		{
			Access<AckFrameHeader>(frame).FirstMissingSequence = m_receivingWindowHead.Sequence;
			int num = AckFrameHeader.Size;
			if (m_receivingBuffer.Distance(m_receivingWindowHead.Position, m_receivingBuffer.Tail) > 1)
			{
				byte b = 0;
				int num2 = 0;
				foreach (int item in m_receivingBuffer.EnumerateFrames(m_receivingWindowHead.Position, m_receivingBuffer.Tail))
				{
					bool flag = m_receivingBuffer[item].State == ReceiveState.Received;
					b = (byte)(b | (byte)((flag ? 1 : 0) << num2));
					num2++;
					if (num2 == 8)
					{
						frame[num] = b;
						num++;
						num2 = 0;
						b = 0;
					}
				}
				if (num2 > 0)
				{
					frame[num++] = b;
				}
			}
			frame = frame.Slice(0, num);
		}

		/// <inheritdoc />
		public MessageTransferResult SendMessage(Span<byte> message, byte channel, SendMessageFlags flags = (SendMessageFlags)0)
		{
			CheckDisposed();
			MessageHeader header = new MessageHeader(channel, message);
			int space = m_sendingBuffer.FrameSize - m_unsentFrameCursor;
			FrameInfo frame;
			if (!header.Fits(space))
			{
				GetNextFrame(out frame);
				FinishFrame(in frame);
			}
			bool flag = message.Length > Settings.MaximumNonBlockingMessageSize;
			if ((flags & SendMessageFlags.NonBlocking) != 0)
			{
				if (flag)
				{
					return MessageTransferResult.OversizedMessage;
				}
				int num = Math.Max((int)m_targetSendingWindowCapacity - m_sendingBuffer.Distance(in m_sendingHead, m_firstSendQueueFrame), 0);
				int num2 = m_settings.MaximumQueueCapacity - m_sendingBuffer.Distance(m_firstSendQueueFrame, m_sendingBuffer.Tail);
				int num3 = num + num2;
				int num4 = ((m_unsentFrameCursor > 0) ? m_unsentFrameCursor : FrameHeader.Size);
				int num5 = m_sendingBuffer.FrameSize - num4 - header.Size();
				int num6 = m_sendingBuffer.FrameSize - FrameHeader.Size;
				int num7 = (message.Length - num5 + num6 - 1) / num6 + 1;
				if (num3 < num7)
				{
					return MessageTransferResult.QueueFull;
				}
			}
			if (m_unsentFrameCursor == 0)
			{
				GetNewFrame(out frame);
			}
			else
			{
				GetNextFrame(out frame);
			}
<<<<<<< HEAD
=======
			(uint, int) tuple = (frame.Sequence, m_unsentFrameCursor);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int num8 = WriteMessage(in header, frame.Data, ref m_unsentFrameCursor, message);
			while (num8 < message.Length)
			{
				FinishFrame(in frame);
				GetNewFrame(out frame, !flag);
				Span<byte> span = message.Slice(num8);
				Span<byte> destination = frame.Data.Slice(FrameHeader.Size);
				int num9 = Math.Min(span.Length, destination.Length);
				span.Slice(0, num9).CopyTo(destination);
				num8 += num9;
				m_unsentFrameCursor += num9;
			}
			if ((flags & SendMessageFlags.HighPriority) != 0 || m_unsentFrameCursor == m_sendingBuffer.FrameSize || flag)
			{
				FinishFrame(in frame);
			}
			PollAvailableFrames();
			ProcessSendQueue();
			return MessageTransferResult.QueuedSuccessfully;
		}

		/// <summary>
		/// Get the data for the message frame that is under construction and is the next to be sent.
		/// </summary>
		/// <param name="frame"></param>
		private void GetNextFrame(out FrameInfo frame)
		{
			int tail = m_sendingBuffer.Tail;
			Span<byte> frame2 = m_sendingBuffer.GetFrame(tail);
			uint sequence = m_sendingBuffer.CalculateSequence(in m_sendingHead, tail);
			frame = new FrameInfo(tail, sequence, frame2);
		}

		/// <summary>
		/// Get a new message frame to start assembling.
		/// </summary>
		/// <param name="frame"></param>
<<<<<<< HEAD
		/// <param name="shouldResize"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void GetNewFrame(out FrameInfo frame, bool shouldResize = true)
		{
			int num = m_sendingBuffer.Distance(m_firstSendQueueFrame, m_sendingBuffer.Tail);
			if (num >= m_sendingBuffer.QueueCapacity)
			{
				if (shouldResize && num < m_settings.MaximumQueueCapacity)
				{
					int queueCapacity = ComputeResize(m_sendingBuffer.QueueCapacity, m_settings.MaximumQueueCapacity, m_sendingBuffer.QueueCapacity + 1);
					ResizeSendingBuffer(m_sendingBuffer.WindowCapacity, queueCapacity);
				}
				else
				{
					ProcessSendQueueBlocking();
				}
			}
			int tail = m_sendingBuffer.Tail;
			Span<byte> frame2 = m_sendingBuffer.GetFrame(tail);
			uint sequence = m_sendingBuffer.CalculateSequence(in m_sendingHead, tail);
			FrameHeader data = FrameHeader.Message(sequence);
			m_unsentFrameCursor = Write(frame2, in data);
			m_sendingBuffer[tail].State = SendState.Unfinished;
			frame = new FrameInfo(tail, sequence, frame2);
		}

		/// <summary>
		/// Finish and send a frame to the remote client.
		/// </summary>
		/// <param name="frame"></param>
		private void FinishFrame(in FrameInfo frame)
		{
			ref SendingFrame reference = ref m_sendingBuffer[frame.FramePosition];
			reference.Length = m_unsentFrameCursor;
			reference.State = SendState.Queued;
			m_sendingBuffer.AdvanceTail();
			m_unsentFrameCursor = 0;
		}

		/// <summary>
		/// Send a finished frame to the remote host.
		/// </summary>
		/// <param name="framePosition">The index of the frame to send.</param>
		private void SendFrame(int framePosition)
		{
			ref SendingFrame reference = ref m_sendingBuffer[framePosition];
			Span<byte> frame = m_sendingBuffer.GetFrame(framePosition);
			m_transport.Send(frame.Slice(0, reference.Length));
			m_framesSent++;
			reference.LastSentTime = GetTime();
			if (reference.State == SendState.Queued)
			{
				reference.EnqueueTime = reference.LastSentTime;
			}
			m_statistics?.AddFrameSent(reference.Length, FrameType.Message, reference.State != SendState.Queued);
			reference.State = SendState.Sent;
			if (framePosition == m_firstSendQueueFrame)
			{
				m_firstSendQueueFrame = m_sendingBuffer.Advance(m_firstSendQueueFrame);
			}
		}

		/// <summary>
		/// Resize the receiving buffer and update pointers.
		/// </summary>
		/// <remarks>All pointers into the buffer become invalid after this call.</remarks>
		/// <param name="windowCapacity"></param>
		/// <param name="queueCapacity"></param>
		private void ResizeSendingBuffer(int windowCapacity, int queueCapacity)
		{
			if (m_unsentFrameCursor > 0)
			{
				GetNextFrame(out var frame);
				FinishFrame(in frame);
			}
			int firstSendQueueFrame = m_sendingBuffer.Distance(m_sendingBuffer.Head, m_firstSendQueueFrame);
			int nextResendFrame = m_sendingBuffer.Distance(m_sendingBuffer.Head, m_nextResendFrame);
			m_sendingBuffer.Resize(windowCapacity, queueCapacity, m_settings.ClearBuffers);
			m_firstSendQueueFrame = firstSendQueueFrame;
			m_nextResendFrame = nextResendFrame;
			m_sendingHead = new WindowPointer(m_sendingHead.Sequence, 0);
		}

		/// <summary>
		/// Work on the send queue until at least one frame becomes available.
		/// </summary>
		private void ProcessSendQueueBlocking()
		{
			int num;
			do
			{
				PollAvailableFrames();
				ProcessSendQueue(blocking: true);
				num = m_sendingBuffer.Distance(m_firstSendQueueFrame, m_sendingBuffer.Tail);
			}
			while (num >= m_sendingBuffer.QueueCapacity);
		}

		/// <summary>
		/// Process the send queue for frames that might need to be sent for the first time or re-sent due to timing out.
		/// </summary>
		private void ProcessSendQueue(bool blocking = false)
		{
			CheckAndSendPing();
			TryAdjustingWindowCapacity();
			int num = m_sendingBuffer.Distance(in m_sendingHead, m_firstSendQueueFrame);
			bool flag = false;
			if (m_sendingBuffer.UsedLength - num > 0)
			{
				int num2 = Math.Min(m_sendingBuffer.WindowCapacity, (int)m_targetSendingWindowCapacity);
				int val = num2 - m_sendingBuffer.Distance(in m_sendingHead, m_firstSendQueueFrame);
				int num3 = Math.Min(val, m_sendingBuffer.Distance(m_firstSendQueueFrame, m_sendingBuffer.Tail));
				for (int i = 0; i < num3; i++)
				{
					SendFrame(m_firstSendQueueFrame);
				}
				flag = flag || num3 > 0;
				m_maxWindowSize = Math.Max(m_maxWindowSize, num + num3);
			}
			if (num <= 0)
			{
				return;
			}
			if (m_sendingBuffer[m_nextResendFrame].State == SendState.Acknowledged)
			{
				FindNextResendFrame();
			}
			int nextResendFrame = m_nextResendFrame;
			TimeSpan waitTime;
			while (ShouldResend(in m_sendingBuffer[m_nextResendFrame], out waitTime))
			{
				flag = true;
				if (!HasRetriedTooManyTimes(in m_sendingBuffer[m_nextResendFrame]))
				{
					if (m_averageEstimateRtt > TimeSpan.Zero && -waitTime > m_averageEstimateRttVariation)
					{
						m_framesLost++;
					}
					SendFrame(m_nextResendFrame);
				}
				FindNextResendFrame();
				if (nextResendFrame == m_nextResendFrame)
				{
					break;
				}
			}
			if (blocking && waitTime > TimeSpan.Zero)
			{
				Thread.Sleep(waitTime);
			}
		}

		/// <summary>
		/// Whether the frame identified by the provided header should be resent.
		/// </summary>
		/// <param name="header"></param>
<<<<<<< HEAD
		/// <param name="waitTime"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		private bool ShouldResend(in SendingFrame header, out TimeSpan waitTime)
		{
			TimeSpan timeSpan = GetTime() - header.LastSentTime;
			waitTime = FrameTimeout - timeSpan;
			return timeSpan > FrameTimeout;
		}

		/// <summary>
		/// Whether a frame was re-tried too many times.
		/// </summary>
		/// <param name="header">The frame header.</param>
		/// <returns>True if the frame was retired too many times. False otherwise.</returns>
		private bool HasRetriedTooManyTimes(in SendingFrame header)
		{
			if (m_settings.DisconnectTimeout < TimeSpan.Zero)
			{
				return false;
			}
			TimeSpan timeSpan = GetTime() - header.EnqueueTime;
			if (timeSpan > m_settings.DisconnectTimeout)
			{
				TimedOut();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Find the index for the next frame that is missing in the sliding window
<<<<<<< HEAD
		/// </summary>        
=======
		/// </summary>
		/// <param name="missingFramePosition"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		private void FindNextResendFrame()
		{
			int capacity = Math.Min(m_sendingBuffer.Distance(m_sendingBuffer.Head, m_firstSendQueueFrame), (int)m_targetSendingWindowCapacity);
			int index = m_sendingBuffer.Distance(m_sendingBuffer.Head, m_nextResendFrame);
			CircularMapping circularMapping = new CircularMapping(capacity);
			foreach (int item in circularMapping.EnumerateFullRange(circularMapping.Advance(index)))
			{
				int num = m_sendingBuffer.Advance(m_sendingBuffer.Head, item);
				if (m_sendingBuffer[num].State == SendState.Sent)
				{
					m_nextResendFrame = num;
					break;
				}
			}
		}

		/// <summary>
		/// Process an ack frame.
		/// </summary>
		/// <param name="ackMessage"></param>
		private void ProcessAcks(Span<byte> ackMessage)
		{
			ref AckFrameHeader reference = ref Unsafe.As<byte, AckFrameHeader>(ref ackMessage[0]);
			_ = m_trace.Enabled;
			int position = m_sendingHead.Position;
			int num = (int)(reference.FirstMissingSequence - m_sendingHead.Sequence);
			long num2 = m_sendingHead.Sequence + m_sendingBuffer.Distance(in m_sendingHead, m_firstSendQueueFrame);
			if (reference.FirstMissingSequence > num2)
			{
				return;
			}
			int num3 = m_sendingBuffer.Advance(position, num);
			TimeSpan now = GetTime();
			foreach (int item in m_sendingBuffer.EnumerateFrames(position, num3))
			{
				AckFrame(item);
			}
			int position2 = m_sendingHead.Position;
			m_sendingBuffer.AdvanceHead(m_settings.ClearBuffers, num);
			m_sendingHead = new WindowPointer(m_sendingHead.Sequence + (uint)num, m_sendingBuffer.Head);
			int num4 = AckFrameHeader.Size;
			if (CircularMapping.IsInRange(position2, num3, m_nextResendFrame))
			{
				m_nextResendFrame = num3;
			}
			if (num4 == ackMessage.Length)
			{
				return;
			}
			int amount = Math.Min(ackMessage.Length - num4 << 3, m_sendingBuffer.Distance(num3, m_firstSendQueueFrame));
			int end = m_sendingBuffer.Advance(num3, amount);
			byte b = ackMessage[num4];
			int num5 = 0;
			foreach (int item2 in m_sendingBuffer.EnumerateFrames(num3, end))
			{
				if (num5 == 0)
				{
					num5 = 8;
					b = ackMessage[num4];
					num4++;
				}
				if (((uint)b & (true ? 1u : 0u)) != 0)
				{
					AckFrame(item2);
				}
				b = (byte)(b >> 1);
				num5--;
			}
			void AckFrame(int index)
			{
				if (m_sendingBuffer[index].State != 0)
				{
					TimeSpan timeSpan = now - m_sendingBuffer[index].EnqueueTime;
					UpdateAckAverage(timeSpan);
					m_statistics?.AddFrameAcknowledged(timeSpan);
					m_sendingBuffer[index].SetAcknowledged();
				}
			}
		}

		private void UpdateAckAverage(TimeSpan ackTime)
		{
			if (m_averageAckTime == TimeSpan.Zero)
			{
				m_averageAckTime = ackTime;
			}
			else
			{
				m_averageAckTime = UpdateAverage(m_averageEstimateRtt, ackTime, 0.01f);
			}
		}

		private void TryAdjustingWindowCapacity()
		{
			if (m_averageEstimateRtt == TimeSpan.Zero || !(GetTime() - m_timeSinceLastWindowAdjustment > FrameTimeout) || m_framesSent <= m_settings.WindowAdjustmentThreshold)
			{
				return;
			}
			if (FrameLoss < m_settings.FrameLossTolerance)
			{
				if (m_maxWindowSize >= (int)m_targetSendingWindowCapacity)
				{
					m_targetSendingWindowCapacity = Math.Min(m_targetSendingWindowCapacity * m_windowGrowthFactor, m_settings.MaximumWindowCapacity);
					if (m_windowGrowthFactor < m_settings.WindowGrowthFactor)
					{
						m_windowGrowthFactor = m_settings.WindowGrowthFactor * 0.1f + m_windowGrowthFactor * 0.9f;
					}
				}
			}
			else
			{
				m_targetSendingWindowCapacity = (int)Math.Max(m_targetSendingWindowCapacity * (1f - FrameLoss), m_settings.MinimumWindowCapacity);
				if (FrameLoss > 0.2f)
				{
					m_windowGrowthFactor = 1f + (m_windowGrowthFactor - 1f) * 0.75f;
				}
			}
			int num = (int)m_targetSendingWindowCapacity;
			if (m_sendingBuffer.WindowCapacity != num)
			{
				int num2 = m_sendingBuffer.Distance(m_sendingBuffer.Head, m_firstSendQueueFrame);
				if (num > num2)
				{
					ResizeSendingBuffer(num, m_sendingBuffer.QueueCapacity);
				}
				else
				{
					int num3 = m_sendingBuffer.Distance(m_sendingBuffer.Head, m_nextResendFrame);
					if (num3 >= num)
					{
						m_nextResendFrame = m_sendingHead.Position;
					}
				}
			}
			m_framesSent = (m_framesLost = 0);
			m_timeSinceLastWindowAdjustment = GetTime();
			m_maxWindowSize = 0;
		}

		/// <summary>
		/// Write a given struct to the provided memory range.
		/// </summary>
		/// <typeparam name="T"></typeparam>
<<<<<<< HEAD
		/// <param name="destination"></param>
		/// <param name="data"></param>        
=======
		/// <param name="data"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		private unsafe int Write<T>(Span<byte> destination, in T data) where T : unmanaged
		{
			fixed (byte* ptr = destination)
			{
				*(T*)ptr = data;
			}
			return sizeof(T);
		}

		/// <summary>
		/// Write a message to the given frame.
		/// </summary>
		/// <remarks>
		/// This method will only write the number of bytes that fit into the current frame.
		/// </remarks>
		/// <param name="header"></param>
		/// <param name="frame">The frame to write the message to.</param>
		/// <param name="frameWriteHead">The current frame write head.
		/// Upon returning this will be the new position on the first byte after the message ends.</param>
		/// <param name="message">The message to write.</param>
		/// <returns>The number of message bytes written. This can be zero if the only the header fits the current frame.</returns>
		private int WriteMessage(in MessageHeader header, Span<byte> frame, ref int frameWriteHead, Span<byte> message)
		{
			frame[frameWriteHead] = header.Channel;
			frameWriteHead++;
			frameWriteHead += Variant.WriteVariant(header.Length, frame.Slice(frameWriteHead));
			int val = frame.Length - frameWriteHead;
			int num = Math.Min(message.Length, val);
			if (num > 0)
			{
				message.Slice(0, num).CopyTo(frame.Slice(frameWriteHead));
			}
			frameWriteHead += num;
			return num;
		}

		/// <summary>
		/// Get a reference to the given range in a span interpreted as an instance of <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">The type to reinterpret the memory range as.</typeparam>
		/// <param name="dataRange">The range of memory containing the type instance.</param>
		/// <returns>A reference to the reinterpreted memory range.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ref T Access<T>(Span<byte> dataRange) where T : unmanaged
		{
			return ref Unsafe.As<byte, T>(ref dataRange[0]);
		}

		/// <summary>
		/// Get the current statistics tracker, if any.
		/// </summary>
		/// <returns></returns>
		public Statistics GetStatistics()
		{
			return m_statistics;
		}

		/// <summary>
		/// Se the statistics tracker for this instance.
		/// </summary>
		/// <param name="instance"></param>
		public void SetStatisticsTracker(Statistics instance)
		{
			m_statistics = (StatisticsInternal)instance;
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private void TraceInit()
		{
			string arg = Assembly.GetEntryAssembly()?.GetName().Name ?? Assembly.GetExecutingAssembly().GetName().Name;
			m_id = $"QoS[{arg}::{Interlocked.Increment(ref m_idCtr)}]";
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private void TraceMessage(string label = null, string content = null)
		{
			if (m_trace.Enabled)
			{
				if (label == null)
				{
					m_trace.Send(m_id ?? "", content);
				}
				else
				{
					m_trace.Send(m_id + ": " + label, content);
				}
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private void TraceSend(string label = null, string content = null)
		{
			if (m_trace.Enabled)
			{
				if (label == null)
				{
					m_trace.Send(m_id + " Snd", content);
				}
				else
				{
					m_trace.Send(m_id + " Snd: " + label, content);
				}
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private void TraceReceive(string label = null, string content = null)
		{
			if (m_trace.Enabled)
			{
				if (label == null)
				{
					m_trace.Send(m_id + " Rcv", content);
				}
				else
				{
					m_trace.Send(m_id + " Rcv: " + label, content);
				}
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private void TraceSendingBuffer()
		{
			_ = m_trace.Enabled;
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private void TraceReceivingBuffer()
		{
			_ = m_trace.Enabled;
		}

		private string FormatSendAcks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"First missing: {m_receivingWindowHead.Sequence}");
			if (m_receivingBuffer.Distance(m_receivingWindowHead.Position, m_receivingBuffer.Tail) == 1)
			{
				return stringBuilder.ToString();
			}
			bool flag = true;
			uint num = m_receivingWindowHead.Sequence;
			foreach (int item in m_receivingBuffer.EnumerateFrames(m_receivingWindowHead.Position, m_receivingBuffer.Tail))
			{
				if (m_receivingBuffer[item].State == ReceiveState.Received)
				{
					if (!flag)
					{
						stringBuilder.Append(", ");
					}
					else
					{
						flag = false;
					}
					uint value = m_receivingBuffer.CalculateSequence(in m_receivingWindowHead, item);
					stringBuilder.Append(value);
				}
				num++;
			}
			return stringBuilder.ToString();
		}

		private string FormatReceivedAcks(in AckFrameHeader header, Span<byte> frame)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"First missing: {header.FirstMissingSequence}");
			int num = AckFrameHeader.Size;
			if (frame.Length > num)
			{
				bool flag = true;
				uint num2 = header.FirstMissingSequence;
				int num3 = 0;
				while (num < frame.Length)
				{
					int num4 = frame[num] >> num3;
					if ((num4 & 1) == 1)
					{
						if (!flag)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							flag = false;
						}
						stringBuilder.Append(num2);
					}
					num3++;
					num2++;
					if (num3 == 8)
					{
						num3 = 0;
						num++;
					}
				}
			}
			return stringBuilder.ToString();
		}

		private string FormatSendingSide()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Q{m_sendingBuffer.QueueCapacity} W{m_sendingBuffer.WindowCapacity}");
			stringBuilder.AppendLine($"NextResendFrame: {m_nextResendFrame} SendingHead: ({m_sendingHead.Position},#{m_sendingHead.Sequence}) FirstSendQueueFrame: {m_firstSendQueueFrame} Loss: {100f * FrameLoss:F2}% ");
			FormatSendingFrames(stringBuilder);
			return stringBuilder.ToString();
		}

		private void FormatSendingFrames(StringBuilder builder)
		{
<<<<<<< HEAD
			FormatCombinedBuffer(builder, in m_sendingBuffer, m_firstSendQueueFrame, delegate(SendingFrame x)
			{
				switch (x.State)
				{
				case SendState.Acknowledged:
					return 'A';
				case SendState.Unfinished:
					return 'U';
				case SendState.Queued:
					return 'Q';
				case SendState.Sent:
					return 'S';
				default:
					throw new ArgumentOutOfRangeException();
				}
=======
			FormatCombinedBuffer(builder, in m_sendingBuffer, m_firstSendQueueFrame, (SendingFrame x) => x.State switch
			{
				SendState.Acknowledged => 'A', 
				SendState.Unfinished => 'U', 
				SendState.Queued => 'Q', 
				SendState.Sent => 'S', 
				_ => throw new ArgumentOutOfRangeException(), 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			});
		}

		private string FormatReceivingSide()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Q{m_receivingBuffer.QueueCapacity} W{m_receivingBuffer.WindowCapacity}");
			stringBuilder.AppendLine($"LastReceivedControlSequence: {m_lastReceivedControlSequence} ReceivingHead: ({m_receivingWindowHead.Position},#{m_receivingWindowHead.Sequence}) ReceivedFrameCursor: {m_receivedFrameCursor}");
			FormatReceivingFrames(stringBuilder);
			return stringBuilder.ToString();
		}

		private void FormatReceivingFrames(StringBuilder builder)
		{
<<<<<<< HEAD
			FormatCombinedBuffer(builder, in m_receivingBuffer, m_receivingWindowHead.Position, delegate(ReceivedFrame x)
			{
				switch (x.State)
				{
				case ReceiveState.Missing:
					return 'M';
				case ReceiveState.Received:
					return 'R';
				case ReceiveState.Unprocessed:
					return 'U';
				default:
					throw new ArgumentOutOfRangeException();
				}
=======
			FormatCombinedBuffer(builder, in m_receivingBuffer, m_receivingWindowHead.Position, (ReceivedFrame x) => x.State switch
			{
				ReceiveState.Missing => 'M', 
				ReceiveState.Received => 'R', 
				ReceiveState.Unprocessed => 'U', 
				_ => throw new ArgumentOutOfRangeException(), 
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			});
		}

		private void FormatCombinedBuffer<THeader>(StringBuilder sb, in CombinedBuffer<THeader> cb, int splitIndex, Func<THeader, char> headerState) where THeader : unmanaged, IHeader
		{
			sb.Append('⎡');
			CircularMapping mapping = cb.Mapping;
			for (int i = 0; i < mapping.Capacity; i++)
			{
				char value = ' ';
				if (i == mapping.Head)
				{
					value = ((i != mapping.Tail) ? '<' : ((mapping.ActiveLength > 0) ? '╳' : '◇'));
				}
				else if (i == mapping.Tail)
				{
					value = '>';
				}
				else if (i == splitIndex)
				{
					value = '⊢';
				}
				else if (mapping.IsInRange(i))
				{
					value = '-';
				}
				sb.Append(value);
			}
			sb.AppendLine("⎤");
			sb.Append('⎣');
			for (int j = 0; j < cb.TotalCapacity; j++)
			{
				sb.Append(mapping.IsInRange(j) ? headerState(cb[j]) : ' ');
			}
			sb.AppendLine("⎦");
		}
	}
}
