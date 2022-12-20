using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
<<<<<<< HEAD
=======
using System.Security.Cryptography;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Threading;
using VRage.Trace;

namespace VRage.Library.Net
{
	public class FrameIntegrityFilter : IUnreliableTransportChannel, IDisposable
	{
		public delegate void ViolationListener(uint receivedCrc, uint computedCrc, FrameIntegrityFilter instance);

		private readonly IUnreliableTransportChannel m_channel;

		private readonly byte[] m_frameBuffer;

		private readonly Crc32 m_crc;

		private static readonly ITrace m_trace = MyTrace.GetTrace(TraceWindow.NetworkQoS);

		private static int m_idCtr;

		private string m_id;

		private int m_sendCount;

		private int m_receiveCount;

		private const string TraceCondition = "__UNDEFINED_SYMBOL__";

		/// <inheritdoc />
		public int MinimumMTU => m_channel.MinimumMTU - 4;

		public event ViolationListener ValidationFailure;

		public FrameIntegrityFilter(IUnreliableTransportChannel channel)
		{
			m_channel = channel;
			m_frameBuffer = new byte[channel.MinimumMTU];
			m_crc = new Crc32();
		}

		/// <inheritdoc />
		public void Dispose()
		{
			m_channel.Dispose();
		}

		/// <inheritdoc />
		public int Send(Span<byte> frame)
		{
			frame = frame.Slice(0, Math.Min(m_frameBuffer.Length - 4, frame.Length));
			frame.CopyTo(MemoryExtensions.AsSpan(m_frameBuffer, 4));
<<<<<<< HEAD
			m_crc.Initialize();
			m_crc.ComputeHash(m_frameBuffer, 4, frame.Length);
=======
			((HashAlgorithm)m_crc).Initialize();
			((HashAlgorithm)m_crc).ComputeHash(m_frameBuffer, 4, frame.Length);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Unsafe.As<byte, uint>(ref m_frameBuffer[0]) = m_crc.CrcValue;
			m_channel.Send(MemoryExtensions.AsSpan(m_frameBuffer, 0, frame.Length + 4));
			return frame.Length;
		}

		/// <inheritdoc />
		public bool PeekFrame(out int frameSize)
		{
			if (m_channel.PeekFrame(out var frameSize2))
			{
				frameSize = Math.Min(frameSize2, m_frameBuffer.Length - 4);
				return true;
			}
			frameSize = 0;
			return false;
		}

		/// <inheritdoc />
		public bool TryGetFrame(ref Span<byte> frame)
		{
			Span<byte> frame2 = MemoryExtensions.AsSpan(m_frameBuffer);
			if (!m_channel.TryGetFrame(ref frame2))
			{
				return false;
			}
<<<<<<< HEAD
			m_crc.Initialize();
			m_crc.ComputeHash(m_frameBuffer, 4, frame2.Length - 4);
=======
			((HashAlgorithm)m_crc).Initialize();
			((HashAlgorithm)m_crc).ComputeHash(m_frameBuffer, 4, frame2.Length - 4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			uint num = Unsafe.As<byte, uint>(ref m_frameBuffer[0]);
			if (num != m_crc.CrcValue)
			{
				this.ValidationFailure?.Invoke(num, m_crc.CrcValue, this);
				return false;
			}
			frame2.Slice(4).CopyTo(frame);
			frame = frame.Slice(0, frame2.Length - 4);
			return true;
		}

		/// <inheritdoc />
		public void QueryMTU(Action<int> result)
		{
			result(m_frameBuffer.Length - 4);
		}

		[Conditional("__UNDEFINED_SYMBOL__")]
		private void TraceInit()
		{
			string arg = Assembly.GetEntryAssembly()?.GetName().Name ?? Assembly.GetExecutingAssembly().GetName().Name;
			m_id = $"IntegrityFilter[{arg}::{Interlocked.Increment(ref m_idCtr)}]";
		}

		[Conditional("__UNDEFINED_SYMBOL__")]
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
	}
}
