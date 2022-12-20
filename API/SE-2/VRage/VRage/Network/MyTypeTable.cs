using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Library.Collections;
using VRage.Utils;

namespace VRage.Network
{
	public class MyTypeTable
	{
		private List<MySynchronizedTypeInfo> m_idToType = new List<MySynchronizedTypeInfo>();

		private Dictionary<Type, MySynchronizedTypeInfo> m_typeLookup = new Dictionary<Type, MySynchronizedTypeInfo>();

		private Dictionary<int, MySynchronizedTypeInfo> m_hashLookup = new Dictionary<int, MySynchronizedTypeInfo>();

		private MyEventTable m_staticEventTable = new MyEventTable(null);

		public MyEventTable StaticEventTable => m_staticEventTable;

		public bool Contains(Type type)
		{
			return m_typeLookup.ContainsKey(type);
		}

		public MySynchronizedTypeInfo Get(TypeId id)
		{
			if (id.Value >= m_idToType.Count)
			{
				MyLog.Default.WriteLine("Invalid replication type ID: " + id.Value);
			}
			return m_idToType[(int)id.Value];
		}

		public MySynchronizedTypeInfo Get(Type type)
		{
			return m_typeLookup[type];
		}

		public bool TryGet(Type type, out MySynchronizedTypeInfo typeInfo)
		{
			return m_typeLookup.TryGetValue(type, out typeInfo);
		}

		public MySynchronizedTypeInfo Register(Type type)
		{
			if (!m_typeLookup.TryGetValue(type, out var value))
			{
				MySynchronizedTypeInfo mySynchronizedTypeInfo = CreateBaseType(type);
				bool flag = IsReplicated(type);
				bool flag2 = HasEvents(type);
				bool flag3 = IsSerializableClass(type);
				if (flag || flag2 || flag3)
				{
					value = new MySynchronizedTypeInfo(type, new TypeId((uint)m_idToType.Count), mySynchronizedTypeInfo, flag);
					m_idToType.Add(value);
					m_hashLookup.Add(value.TypeHash, value);
					m_typeLookup.Add(type, value);
					if (flag || flag2)
					{
						m_staticEventTable.AddStaticEvents(type);
					}
				}
				else if (mySynchronizedTypeInfo != null)
				{
					value = mySynchronizedTypeInfo;
					m_typeLookup.Add(type, value);
				}
				else
				{
					value = null;
				}
			}
			return value;
		}

		public static bool ShouldRegister(Type type)
		{
			if (!IsReplicated(type) && !CanHaveEvents(type))
			{
				return IsSerializableClass(type);
			}
			return true;
		}

		private static bool IsSerializableClass(Type type)
		{
			if ((!type.HasAttribute<SerializableAttribute>() || type.HasAttribute<CompilerGeneratedAttribute>()) && !type.IsEnum)
			{
				return typeof(MulticastDelegate).IsAssignableFrom(type.BaseType);
			}
			return true;
		}

		private static bool IsReplicated(Type type)
		{
			if (!type.IsAbstract && typeof(IMyReplicable).IsAssignableFrom(type))
			{
				return !type.HasAttribute<NotReplicableAttribute>();
			}
			return false;
		}

		private static bool CanHaveEvents(Type type)
		{
			if (!Attribute.IsDefined(type, typeof(StaticEventOwnerAttribute)))
			{
				return typeof(IMyEventOwner).IsAssignableFrom(type);
			}
			return true;
		}

		private static bool HasEvents(Type type)
		{
			return Enumerable.Any<MemberInfo>((IEnumerable<MemberInfo>)type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), (Func<MemberInfo, bool>)((MemberInfo s) => s.HasAttribute<EventAttribute>()));
		}

		private MySynchronizedTypeInfo CreateBaseType(Type type)
		{
			while (type.BaseType != null && type.BaseType != typeof(object))
			{
				if (ShouldRegister(type.BaseType))
				{
					return Register(type.BaseType);
				}
				type = type.BaseType;
			}
			return null;
		}

		/// <summary>
		/// Serializes id to hash list.
		/// Server sends the hashlist to client, client reorders type table to same order as server.
		/// </summary>
		public void Serialize(BitStream stream)
		{
			if (stream.Writing)
			{
				stream.WriteVariant((uint)m_idToType.Count);
				for (int i = 0; i < m_idToType.Count; i++)
				{
					stream.WriteInt32(m_idToType[i].TypeHash);
				}
				return;
			}
			int num = (int)stream.ReadUInt32Variant();
			if (m_idToType.Count != num)
			{
				MyLog.Default.WriteLine($"Bad number of types from server. Recieved {num}, have {m_idToType.Count}");
			}
			m_staticEventTable = new MyEventTable(null);
			for (int j = 0; j < num; j++)
			{
				int num2 = stream.ReadInt32();
				if (!m_hashLookup.ContainsKey(num2))
				{
					MyLog.Default.WriteLine("Type hash not found! Value: " + num2);
				}
				MySynchronizedTypeInfo mySynchronizedTypeInfo = m_hashLookup[num2];
				m_idToType[j] = mySynchronizedTypeInfo;
				m_staticEventTable.AddStaticEvents(mySynchronizedTypeInfo.Type);
			}
		}
	}
}
