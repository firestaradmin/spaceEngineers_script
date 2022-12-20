using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
	/// <summary>
	/// Id of network endpoint, opaque struct, internal value should not be accessed outside VRage.Network.
	/// EndpointId is not guid and can change when client reconnects to server.
	/// Internally it's SteamId or RakNetGUID.
	/// </summary>
	[Serializable]
	public struct EndpointId
	{
		protected class VRage_Network_EndpointId_003C_003EValue_003C_003EAccessor : IMemberAccessor<EndpointId, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EndpointId owner, in ulong value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EndpointId owner, out ulong value)
			{
				value = owner.Value;
			}
		}

		public readonly ulong Value;

		public static EndpointId Null = new EndpointId(0uL);

		public bool IsNull => Value == 0;

		public bool IsValid => !IsNull;

		public EndpointId(ulong value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Format(Value);
		}

		public static bool operator ==(EndpointId a, EndpointId b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(EndpointId a, EndpointId b)
		{
			return a.Value != b.Value;
		}

		public bool Equals(EndpointId other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj is EndpointId)
			{
				return Equals((EndpointId)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			ulong value = Value;
			return value.GetHashCode();
		}

		public static string Format(in EndpointId id)
		{
			return Format(id.Value);
		}

		public static string Format(ulong endpointId)
		{
			if (endpointId == 0L)
			{
				return "[Null]";
			}
			return $"[{endpointId / 10000000000000000uL:000}...{endpointId % 1000uL:000}]";
		}

		public static string Format(in EndpointId id)
		{
			return Format(id.Value);
		}

		public static string Format(ulong endpointId)
		{
			if (endpointId == 0L)
			{
				return "[Null]";
			}
			return $"[{endpointId / 10000000000000000uL:000}...{endpointId % 1000uL:000}]";
		}
	}
}
