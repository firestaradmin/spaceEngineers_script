using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace VRage.Game
{
	public struct CompactSerializedArray<T>
	{
		private T[] m_data;

		private static int Size = Marshal.SizeOf<T>();

		[XmlText]
		public byte[] SerializableData
		{
			get
			{
				if (m_data == null)
				{
					return null;
				}
				byte[] array = new byte[Size * m_data.Length];
				Buffer.BlockCopy(m_data, 0, array, 0, array.Length);
				return array;
			}
			set
			{
				if (value == null)
				{
					m_data = null;
					return;
				}
				m_data = new T[value.Length / Size];
				Buffer.BlockCopy(value, 0, m_data, 0, value.Length);
			}
		}

		private CompactSerializedArray(in T[] array)
		{
			m_data = array;
		}

		public static implicit operator T[](in CompactSerializedArray<T> array)
		{
			return array.m_data;
		}

		public static implicit operator CompactSerializedArray<T>(in T[] array)
		{
			return new CompactSerializedArray<T>(in array);
		}
	}
}
