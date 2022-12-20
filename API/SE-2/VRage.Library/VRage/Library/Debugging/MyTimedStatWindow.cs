using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;

namespace VRage.Library.Debugging
{
	public class MyTimedStatWindow
	{
		public interface IStatArithmetic<TStats>
		{
			void Add(in TStats lhs, in TStats rhs, out TStats result);

			void Subtract(in TStats lhs, in TStats rhs, out TStats result);
		}

		public class IntArithmeticImpl : IStatArithmetic<int>
		{
			/// <inheritdoc />
			public void Add(in int lhs, in int rhs, out int result)
			{
				result = lhs + rhs;
			}

			/// <inheritdoc />
			public void Subtract(in int lhs, in int rhs, out int result)
			{
				result = lhs - rhs;
			}

			void IStatArithmetic<int>.Add(in int lhs, in int rhs, out int result)
			{
				Add(in lhs, in rhs, out result);
			}

			void IStatArithmetic<int>.Subtract(in int lhs, in int rhs, out int result)
			{
				Subtract(in lhs, in rhs, out result);
			}
		}

		public class FloatArithmeticImpl : IStatArithmetic<float>
		{
			/// <inheritdoc />
			public void Add(in float lhs, in float rhs, out float result)
			{
				result = lhs + rhs;
			}

			/// <inheritdoc />
			public void Subtract(in float lhs, in float rhs, out float result)
			{
				result = lhs - rhs;
			}

			void IStatArithmetic<float>.Add(in float lhs, in float rhs, out float result)
			{
				Add(in lhs, in rhs, out result);
			}

			void IStatArithmetic<float>.Subtract(in float lhs, in float rhs, out float result)
			{
				Subtract(in lhs, in rhs, out result);
			}
		}

		public static readonly IStatArithmetic<int> IntArithmetic = new IntArithmeticImpl();

		public static readonly IStatArithmetic<float> FloatArithmetic = new FloatArithmeticImpl();
	}
	/// <summary>
	/// Fixed time sliding window. Useful for debug statistics.
	/// </summary>
	/// <typeparam name="TStats"></typeparam>
	public class MyTimedStatWindow<TStats> : MyTimedStatWindow, IEnumerable<TStats>, IEnumerable
	{
		private struct Frame
		{
			public TimeSpan Time;

			public TStats Data;
		}

		private class FuncArithmetic : IStatArithmetic<TStats>
		{
			private readonly Func<TStats, TStats, TStats> m_accumulator;

			private readonly Func<TStats, TStats, TStats> m_subtractor;

			public FuncArithmetic(Func<TStats, TStats, TStats> accumulator, Func<TStats, TStats, TStats> subtractor)
			{
				m_accumulator = accumulator;
				m_subtractor = subtractor;
			}

			/// <inheritdoc />
			public void Add(in TStats lhs, in TStats rhs, out TStats result)
			{
				result = m_accumulator(lhs, rhs);
			}

			/// <inheritdoc />
			public void Subtract(in TStats lhs, in TStats rhs, out TStats result)
			{
				result = m_subtractor(lhs, rhs);
			}

			void IStatArithmetic<TStats>.Add(in TStats lhs, in TStats rhs, out TStats result)
			{
				Add(in lhs, in rhs, out result);
			}

			void IStatArithmetic<TStats>.Subtract(in TStats lhs, in TStats rhs, out TStats result)
			{
				Subtract(in lhs, in rhs, out result);
			}
		}

		private readonly MyQueue<Frame> m_frames;

		private readonly Stopwatch m_timer;

		private readonly IStatArithmetic<TStats> m_arithmetic;

		private TStats m_currentTotal;

		public readonly TimeSpan MaxTime;

		public TStats Total
		{
			get
			{
				m_arithmetic.Add(in m_currentTotal, in Current, out var result);
				return result;
			}
		}

		public ref TStats Current => ref m_frames[m_frames.Count - 1].Data;

		/// <inheritdoc />
		public MyTimedStatWindow(TimeSpan maxTime, Func<TStats, TStats, TStats> accumulator, Func<TStats, TStats, TStats> subtractor)
			: this(maxTime, (IStatArithmetic<TStats>)new FuncArithmetic(accumulator, subtractor))
		{
		}

		public MyTimedStatWindow(TimeSpan maxTime, IStatArithmetic<TStats> arithmetic)
		{
			MaxTime = maxTime;
			m_arithmetic = arithmetic;
			m_frames = new MyQueue<Frame>();
			m_timer = Stopwatch.StartNew();
			m_frames.Enqueue(new Frame
			{
<<<<<<< HEAD
				Time = m_timer.Elapsed
=======
				Time = m_timer.get_Elapsed()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			});
		}

		/// <summary>
		///
		/// </summary>
		public void Advance()
		{
<<<<<<< HEAD
			TimeSpan elapsed = m_timer.Elapsed;
=======
			TimeSpan elapsed = m_timer.get_Elapsed();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			while (m_frames.Count > 0 && elapsed - m_frames[0].Time > MaxTime)
			{
				TStats rhs = m_frames.Dequeue().Data;
				if (m_frames.Count > 0)
				{
					m_arithmetic.Subtract(in m_currentTotal, in rhs, out m_currentTotal);
				}
			}
			if (m_frames.Count > 0)
			{
				m_arithmetic.Add(in m_currentTotal, in Current, out m_currentTotal);
			}
			m_frames.Enqueue(new Frame
			{
				Time = elapsed
			});
		}

		/// <inheritdoc />
		public IEnumerator<TStats> GetEnumerator()
		{
			foreach (Frame frame in m_frames)
			{
				yield return frame.Data;
			}
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
