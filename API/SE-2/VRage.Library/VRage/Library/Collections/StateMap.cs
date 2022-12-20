using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VRage.Library.Collections
{
	public class StateMap<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : unmanaged, Enum
	{
		private ulong[] m_data;

		private int m_size;

		private readonly T m_default;

		/// <summary>
		/// The number of bits required to fully represent a valid instance of the state element.
		/// </summary>
		public static readonly int ElementBitSize;

		/// <summary>
		/// The number of bytes required to fully represent a valid instance of the state element.
		/// </summary>
		public static readonly int ElementByteSize;

		/// <summary>
		/// Mask that can be used to select all the relevant bits from a valid value.
		/// </summary>
		public static readonly ulong ElementMask;

		/// <summary>
		/// Whether the underlying type of the state enum is signed.
		/// </summary>
		public static readonly bool IsSigned;

		/// <summary>
		/// Whether the element enum is annotated with <see cref="T:System.FlagsAttribute" />
		/// </summary>
		public static bool IsFlags;

		private static HashSet<Type> SignedTypes;

		private static Dictionary<Type, int> BitCount;

		/// <inheritdoc />
		public int Count => m_size;

		/// <inheritdoc />
		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= m_size)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				ComputeCell(index, out var cell, out var offset);
				ulong num = (m_data[cell] >> offset) & ElementMask;
				if (IsSigned && (num & ~(ElementMask >> 1)) != 0L)
				{
					num |= ~ElementMask;
				}
				return Cast(num);
			}
			set
			{
				if (index < 0 || index >= m_size)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				ComputeCell(index, out var cell, out var offset);
				ulong num = (Cast(value) & ElementMask) << offset;
				m_data[cell] &= ~(ElementMask << offset);
				m_data[cell] |= num;
			}
		}

		public StateMap(int count, T defaultState = default(T))
		{
			m_default = defaultState;
			Resize(count);
		}

		public void Resize(int count)
		{
			int size = m_size;
			m_size = count;
			int newSize = count * ElementBitSize + 63 >> 6;
			Array.Resize(ref m_data, newSize);
			if (Cast(m_default) != 0L)
			{
				for (int i = size; i < m_size; i++)
				{
					this[i] = m_default;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ComputeCell(int index, out int cell, out int offset)
		{
			int num = index * ElementBitSize;
			offset = num & 0x3F;
			cell = num >> 6;
		}

		/// <inheritdoc />
		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		unsafe static StateMap()
		{
			HashSet<Type> val = new HashSet<Type>();
			val.Add(typeof(sbyte));
			val.Add(typeof(short));
			val.Add(typeof(int));
			val.Add(typeof(long));
			SignedTypes = val;
			BitCount = new Dictionary<Type, int>
			{
				{
					typeof(sbyte),
					8
				},
				{
					typeof(byte),
					8
				},
				{
					typeof(short),
					16
				},
				{
					typeof(ushort),
					16
				},
				{
					typeof(int),
					32
				},
				{
					typeof(uint),
					32
				},
				{
					typeof(long),
					64
				},
				{
					typeof(ulong),
					64
				}
			};
			Type typeFromHandle = typeof(T);
			IsSigned = SignedTypes.Contains(Enum.GetUnderlyingType(typeFromHandle));
			IsFlags = typeFromHandle.HasAttribute<FlagsAttribute>();
			ElementByteSize = sizeof(T);
			Array values = Enum.GetValues(typeof(T));
			ulong num = 0uL;
			bool flag = false;
			foreach (object item in values)
			{
				ulong num2 = Cast((T)item);
				if (IsSigned && (long)num2 < 0L)
				{
					flag = true;
					if (!IsFlags)
					{
						num2 = 0L - num2;
					}
				}
				num = Math.Max(num, num2);
			}
			if (IsSigned && IsFlags && flag)
			{
				ElementBitSize = ElementByteSize * 8;
			}
			else
			{
				int num3 = 65;
				ulong num4 = ulong.MaxValue;
				while (num4 != 0 && num4 >= num)
				{
					num3--;
					num4 >>= 1;
				}
				ElementBitSize = num3;
				if (flag)
				{
					ElementBitSize++;
				}
			}
			ElementMask = (ulong)((1L << ElementBitSize) - 1);
		}

		private unsafe static T Cast(ulong value)
		{
			return *(T*)(&value);
		}

		private unsafe static ulong Cast(T value)
		{
			switch (ElementByteSize)
			{
			case 1:
				if (IsSigned)
				{
					return (ulong)(*(sbyte*)(&value));
				}
				return *(byte*)(&value);
			case 2:
				if (IsSigned)
				{
					return (ulong)(*(short*)(&value));
				}
				return *(ushort*)(&value);
			case 4:
				if (IsSigned)
				{
					return (ulong)(int)(*(uint*)(&value));
				}
				return *(uint*)(&value);
			case 8:
				_ = IsSigned;
				return *(ulong*)(&value);
			default:
				throw new InvalidBranchException();
			}
		}
	}
}
