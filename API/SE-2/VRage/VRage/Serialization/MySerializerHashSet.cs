using System;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerHashSet<TItem> : MySerializer<HashSet<TItem>>
	{
		private MySerializer<TItem> m_itemSerializer = MyFactory.GetSerializer<TItem>();

		public override void Clone(ref HashSet<TItem> value)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			HashSet<TItem> val = new HashSet<TItem>();
			Enumerator<TItem> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					TItem value2 = enumerator.get_Current();
					m_itemSerializer.Clone(ref value2);
					val.Add(value2);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			value = val;
		}

		public override bool Equals(ref HashSet<TItem> a, ref HashSet<TItem> b)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (a == b)
			{
				return true;
			}
			if (MySerializer.AnyNull(a, b))
			{
				return false;
			}
			if (a.get_Count() != b.get_Count())
			{
				return false;
			}
			Enumerator<TItem> enumerator = a.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					TItem current = enumerator.get_Current();
					if (!b.Contains(current))
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return true;
		}

		public override void Read(BitStream stream, out HashSet<TItem> value, MySerializeInfo info)
		{
			int num = (int)stream.ReadUInt32Variant();
			value = new HashSet<TItem>();
			for (int i = 0; i < num; i++)
			{
				MySerializationHelpers.CreateAndRead(stream, out var result, m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
				value.Add(result);
			}
		}

		public override void Write(BitStream stream, ref HashSet<TItem> value, MySerializeInfo info)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			int count = value.get_Count();
			stream.WriteVariant((uint)count);
			Enumerator<TItem> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					TItem value2 = enumerator.get_Current();
					MySerializationHelpers.Write(stream, ref value2, m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}
	}
}
