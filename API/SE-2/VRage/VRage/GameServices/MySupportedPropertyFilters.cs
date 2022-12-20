using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VRage.GameServices
{
	public readonly struct MySupportedPropertyFilters : IReadOnlyList<MySupportedPropertyFilters.Entry>, IEnumerable<MySupportedPropertyFilters.Entry>, IEnumerable, IReadOnlyCollection<MySupportedPropertyFilters.Entry>
	{
		public readonly struct Entry
		{
			public readonly string Property;

			public readonly MySearchConditionFlags SupportedConditions;

			public Entry(string property, MySearchConditionFlags supportedConditions)
			{
				Property = property;
				SupportedConditions = supportedConditions;
			}

			public static implicit operator Entry(in (string Property, MySearchConditionFlags SupportedConditions) tuple)
			{
				return new Entry(tuple.Property, tuple.SupportedConditions);
			}

			public void Deconstruct(out string property, out MySearchConditionFlags supportedConditions)
			{
				property = Property;
				supportedConditions = SupportedConditions;
			}
		}

		public struct Enumerator : IEnumerator<Entry>, IEnumerator, IDisposable
		{
			private readonly Entry[] m_entries;

			private int m_index;

			/// <inheritdoc />
			public Entry Current => m_entries[m_index];

			/// <inheritdoc />
			object IEnumerator.Current => Current;

			/// <inheritdoc />
			internal Enumerator(Entry[] entries)
			{
				m_entries = entries;
				m_index = -1;
			}

			/// <inheritdoc />
			public bool MoveNext()
			{
				if (m_index < m_entries.Length - 1)
				{
					m_index++;
					return true;
				}
				return false;
			}

			/// <inheritdoc />
			public void Reset()
			{
				m_index = -1;
			}

			/// <inheritdoc />
			public void Dispose()
			{
			}
		}

		private readonly Entry[] m_entries;

		public static readonly MySupportedPropertyFilters Empty = new MySupportedPropertyFilters(Array.Empty<Entry>());

		/// <inheritdoc />
		public int Count => m_entries.Length;

		/// <inheritdoc />
		public Entry this[int index] => m_entries[index];

		public MySupportedPropertyFilters(IEnumerable<(string Property, MySearchConditionFlags SupportedConditions)> entries)
		{
<<<<<<< HEAD
			m_entries = entries.Select<(string, MySearchConditionFlags), Entry>((Func<(string, MySearchConditionFlags), Entry>)(((string Property, MySearchConditionFlags SupportedConditions) x) => x)).ToArray();
=======
			m_entries = Enumerable.ToArray<Entry>(Enumerable.Select<(string, MySearchConditionFlags), Entry>(entries, (Func<(string, MySearchConditionFlags), Entry>)(((string Property, MySearchConditionFlags SupportedConditions) x) => x)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public MySupportedPropertyFilters(IEnumerable<Entry> entries)
		{
<<<<<<< HEAD
			m_entries = entries.ToArray();
=======
			m_entries = Enumerable.ToArray<Entry>(entries);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private MySupportedPropertyFilters(Entry[] entries)
		{
			m_entries = entries;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(m_entries);
		}

		/// <inheritdoc />
		IEnumerator<Entry> IEnumerable<Entry>.GetEnumerator()
		{
			return new Enumerator(m_entries);
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
