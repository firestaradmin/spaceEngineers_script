using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VRage.Library.Net
{
	/// <summary>
	/// Allows easy mapping of a circular buffer onto a linear range.
	/// </summary>
	/// <returns>
	/// The buffer defines an <i>Active Segment</i>, a range of values delimited by <see cref="F:VRage.Library.Net.CircularMapping.Head" /> and <see cref="F:VRage.Library.Net.CircularMapping.Tail" />.
	/// this active segment can 
	/// </returns>
	public struct CircularMapping
	{
<<<<<<< HEAD
		/// <summary>
		/// Represents an operator that can be used to copy data from one buffer to another.
		/// </summary>
		/// <typeparam name="TBuffer">The type of the buffer</typeparam>
		/// <param name="source">The source buffer.</param>
		/// <param name="sourceIndex">The starting index in the source buffer.</param>
		/// <param name="destinationBuffer">The destination buffer.</param>
		/// <param name="destinationIndex">The starting index in the destination buffer.</param>
		/// <param name="length">The length of range to copy from source to destination.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public delegate void Copy<TBuffer>(TBuffer source, int sourceIndex, TBuffer destinationBuffer, int destinationIndex, int length);

		public readonly struct Enumerable : IEnumerable<int>, IEnumerable
		{
			private readonly int m_head;

			private readonly int m_tail;

			private readonly int m_length;

			private readonly bool m_empty;

			public Enumerable(int head, int tail, int length, bool empty)
			{
				m_head = head;
				m_tail = tail;
				m_length = length;
				m_empty = empty;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(m_head, m_tail, m_length, m_empty);
			}

			/// <inheritdoc />
			IEnumerator<int> IEnumerable<int>.GetEnumerator()
			{
				return GetEnumerator();
			}

			/// <inheritdoc />
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		/// <summary>
		/// Enumerate the indices belonging to a range in a circular buffer.
		/// </summary>
		public struct Enumerator : IEnumerator<int>, IEnumerator, IDisposable
		{
			private readonly int m_head;

			private readonly int m_tail;

			private int m_index;

			private int m_length;

			private bool m_first;

			/// <summary>
			/// Double negative ensured default(RingEnumerator) is empty.
			/// </summary>
			private readonly bool m_notEmpty;

			public int Current => m_index;

			/// <inheritdoc />
			object IEnumerator.Current => Current;

			public Enumerator(int head, int tail, int length, bool empty)
			{
				m_index = 0;
				m_first = true;
				m_head = head;
				m_tail = tail;
				m_length = length;
				m_notEmpty = !empty;
				Reset();
			}

			public bool MoveNext()
			{
				if (!m_notEmpty)
				{
					return false;
				}
				m_index++;
				if (m_index == m_length)
				{
					m_index = 0;
				}
				if (m_first)
				{
					m_first = false;
				}
				else if (m_index == m_tail)
				{
					return false;
				}
				return true;
			}

			public void Reset()
			{
				m_first = true;
				m_index = m_head - 1;
			}

			/// <inheritdoc />
			public void Dispose()
			{
				m_length = 0;
			}
		}

		public int Head;

		public int Tail;

		public int Capacity;

		public int ActiveLength;

		/// <summary>
		/// Create a new circular buffer representation with a given length.
		/// </summary>
		/// <param name="capacity"></param>
		public CircularMapping(int capacity)
		{
			Head = (Tail = 0);
			Capacity = capacity;
			ActiveLength = 0;
		}

		/// <summary>
		/// Create a new circular buffer representation with a given length.
		/// </summary>
		/// <param name="capacity"></param>
<<<<<<< HEAD
		/// <param name="head"></param>
		/// <param name="tail"></param>
		/// <param name="empty"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public CircularMapping(int capacity, int head, int tail, bool empty = false)
		{
			if (empty && head != tail)
			{
				throw new InvalidOperationException("Empty flag should only be used to disambiguate state when head and tail are the same.");
			}
			Head = head;
			Tail = tail;
			Capacity = capacity;
			ActiveLength = ((!empty) ? Distance(Head, Tail, Capacity) : 0);
		}

		/// <summary>
		/// Resize this mapping and copy user data the data to a resized buffer to match.
		/// </summary>
		/// <typeparam name="TBuffer">The type of the user buffer.</typeparam>
		/// <param name="newLength">The new length of the circular mapping.</param>
		/// <param name="original">The original buffer.</param>
		/// <param name="resized">The new buffer that is correctly sized.</param>
<<<<<<< HEAD
		/// <param name="copyFunction"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void ResizeBuffer<TBuffer>(int newLength, TBuffer original, TBuffer resized, Copy<TBuffer> copyFunction)
		{
			if (newLength < ActiveLength)
			{
				throw new InvalidOperationException("New length would be too small to encompass the active segment.");
			}
			if (ActiveLength > 0)
			{
				if (Head < Tail)
				{
					copyFunction(original, Head, resized, 0, ActiveLength);
				}
				else
				{
					copyFunction(original, Head, resized, 0, Capacity - Head);
					copyFunction(original, 0, resized, Capacity - Head, Tail);
				}
			}
			Head = 0;
			Tail = ActiveLength;
			Capacity = newLength;
		}

		/// <summary>
		/// Resize the virtual length of the circular mapping.
		/// </summary>
		/// <param name="newLength"></param>
		public void Resize(int newLength)
		{
			if (newLength < ActiveLength)
			{
				throw new InvalidOperationException("New length would be too small to encompass the active segment.");
			}
			Head = 0;
			Tail = ActiveLength;
			Capacity = newLength;
		}

		/// <summary>
		/// Advance the head, shrinking the active segment.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AdvanceHead(int amount = 1)
		{
			if (amount < 0 || amount > ActiveLength)
			{
				throw new ArgumentOutOfRangeException("amount");
			}
			Head = Advance(Head, amount);
			ActiveLength -= amount;
		}

		/// <summary>
		/// Advance the tail, extending the active segment.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AdvanceTail(int amount = 1)
		{
			if (amount < 0 || ActiveLength + amount > Capacity)
			{
				throw new ArgumentOutOfRangeException("amount");
			}
			Tail = Advance(Tail, amount);
			ActiveLength += amount;
		}

		/// <summary>
		/// Advance the provided index to the next valid position in the circular buffer.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Advance(int index)
		{
			index++;
			if (index == Capacity)
			{
				index = 0;
			}
			return index;
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
			if (amount > Capacity)
			{
				amount %= Capacity;
			}
			index += amount;
			if (index >= Capacity)
			{
				index -= Capacity;
			}
			return index;
		}

		/// <summary>
		/// Retract the provided index to a previous valid position in the circular buffer.
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
			if (amount > Capacity)
			{
				amount %= Capacity;
			}
			index -= amount;
			if (index < 0)
			{
				index += Capacity;
			}
			return index;
		}

		/// <summary>
		/// Advance the provided index to the next valid position in the circular buffer.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Retract(int index)
		{
			index--;
			if (index < 0)
			{
				index = Capacity - 1;
			}
			return index;
		}

		/// <summary>
		/// Calculate the number of entries between the two positions in the circular buffer.
		/// </summary>
		/// <remarks>This method will prefer returning zero over <see cref="F:VRage.Library.Net.CircularMapping.Capacity" /> if the positions are equal.</remarks>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns>The distance between the positions.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Distance(int from, int to)
		{
			return Distance(from, to, Capacity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int Distance(int from, int to, int capacity)
		{
			if (to >= from)
			{
				return to - from;
			}
			return to + capacity - from;
		}

		/// <summary>
		/// Whether <paramref name="index" /> is within the bounds of the active range in the circular buffer.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsInRange(int index)
		{
			if (index < 0 || index >= Capacity)
			{
				return false;
			}
			if (ActiveLength == Capacity)
			{
				return true;
			}
			return IsInRange(Head, Tail, index);
		}

		/// <summary>
		/// Whether a given index is contained in the provided range on the circular buffer.
		/// </summary>
		/// <remarks>This method assumes that the index is valid, and therefore it does not need to know the size of the list.
		/// It also assumes the list does not envelop the entire buffer.</remarks>
		/// <param name="start">The start index.</param>
		/// <param name="end">The end index.</param>
		/// <param name="position">The index to query/</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInRange(int start, int end, int position)
		{
			if (end < start)
			{
				if (position < start)
				{
					return position < end;
				}
				return true;
			}
			if (position >= start)
			{
				return position < end;
			}
			return false;
		}

		/// <summary>
		/// Enumerate all frames in the occupied section of the sliding window.
		/// </summary>
		/// <returns></returns>
		public Enumerable EnumerateActiveSegment()
		{
			return new Enumerable(Head, Tail, Capacity, ActiveLength == 0);
		}

		/// <summary>
		/// Enumerate all frames in the user provided range.
		/// </summary>
		/// <remarks>If <paramref name="start" /> == <paramref name="end" /> the enumeration yields no results.
		/// To enumerate a full circle see <see cref="M:VRage.Library.Net.CircularMapping.EnumerateFullRange(System.Int32)" />.</remarks>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public Enumerable EnumerateRange(int start, int end)
		{
			return new Enumerable(start, end, Capacity, start == end);
		}

		/// <summary>
		/// Enumerate all frames in the mapping, starting at <paramref name="start" />.
		/// </summary>
<<<<<<< HEAD
		/// <param name="start"></param>        
=======
		/// <param name="start"></param>
		/// <param name="end"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <returns></returns>
		public Enumerable EnumerateFullRange(int start)
		{
			return new Enumerable(start, start, Capacity, empty: false);
		}
	}
}
