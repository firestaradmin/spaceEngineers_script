using System.Globalization;

namespace System.Net
{
	public static class IPAddressExtensions
	{
		public static uint ToIPv4NetworkOrder(this IPAddress ip)
		{
			return (uint)IPAddress.HostToNetworkOrder((int)ip.get_Address());
		}

		public static IPAddress FromIPv4NetworkOrder(uint ip)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return new IPAddress((long)(uint)IPAddress.NetworkToHostOrder((int)ip));
		}

		public static IPAddress ParseOrAny(string ip)
		{
<<<<<<< HEAD
			if (!IPAddress.TryParse(ip, out var address))
=======
			IPAddress result = default(IPAddress);
			if (!IPAddress.TryParse(ip, ref result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return IPAddress.Any;
			}
			return result;
		}

		/// <summary>
		/// Parses IP Endpoint from string. Expected format is the same as output by <see cref="M:System.Net.IPEndPoint.ToString" />.
		/// </summary>
		public static bool TryParseEndpoint(string s, out string prefix, out IPEndPoint result, uint defaultPort = 27016u)
		{
<<<<<<< HEAD
=======
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int num = s.IndexOf("://", StringComparison.Ordinal);
			if (num != -1)
			{
				int num2 = num + 3;
				prefix = s.Substring(0, num2);
				s = s.Substring(num2);
			}
			else
			{
				prefix = "";
			}
			int num3 = s.Length;
			int num4 = s.LastIndexOf(':');
			if (num4 > 0)
			{
				if (s[num4 - 1] == ']')
				{
					num3 = num4;
				}
				else if (s.Substring(0, num4).LastIndexOf(':') == -1)
				{
					num3 = num4;
				}
			}
<<<<<<< HEAD
			if (IPAddress.TryParse(s.Substring(0, num3), out var address))
=======
			IPAddress val = default(IPAddress);
			if (IPAddress.TryParse(s.Substring(0, num3), ref val))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				uint result2 = defaultPort;
				if (num3 == s.Length || (uint.TryParse(s.Substring(num3 + 1), NumberStyles.None, CultureInfo.InvariantCulture, out result2) && result2 <= 65535))
				{
<<<<<<< HEAD
					result = new IPEndPoint(address, (int)result2);
=======
					result = new IPEndPoint(val, (int)result2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					return true;
				}
			}
			result = null;
			return false;
		}
	}
}
