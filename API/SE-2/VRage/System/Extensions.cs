using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Network;

namespace System
{
	public static class Extensions
	{
		[ThreadStatic]
		private static List<IMyStateGroup> m_tmpStateGroupsPerThread;

		private static List<IMyStateGroup> m_tmpStateGroups
		{
			get
			{
				if (m_tmpStateGroupsPerThread == null)
				{
					m_tmpStateGroupsPerThread = new List<IMyStateGroup>();
				}
				return m_tmpStateGroupsPerThread;
			}
		}

		public static NetworkId ReadNetworkId(this BitStream stream)
		{
			return new NetworkId(stream.ReadUInt32Variant());
		}

		public static TypeId ReadTypeId(this BitStream stream)
		{
			return new TypeId(stream.ReadUInt32Variant());
		}

		public static void WriteNetworkId(this BitStream stream, NetworkId networkId)
		{
			stream.WriteVariant(networkId.Value);
		}

		public static void WriteTypeId(this BitStream stream, TypeId typeId)
		{
			stream.WriteVariant(typeId.Value);
		}

		/// <summary>
		/// Finds state group of specified type.
		/// Returns null when group of specified type not found.
		/// </summary>
		public static T FindStateGroup<T>(this IMyReplicable obj) where T : class, IMyStateGroup
		{
			try
			{
				if (obj == null)
				{
					return null;
				}
				obj.GetStateGroups(m_tmpStateGroups);
				foreach (IMyStateGroup tmpStateGroup in m_tmpStateGroups)
				{
					T val = tmpStateGroup as T;
					if (val != null)
					{
						return val;
					}
				}
				return null;
			}
			finally
			{
				m_tmpStateGroups.Clear();
			}
		}
	}
}
