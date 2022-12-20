using System;
using Epic.OnlineServices;

namespace VRage.EOS
{
	internal static class Extensions
	{
		public static ProductUserId ToProductUserId(this ulong id)
		{
			return new ProductUserId(new IntPtr((long)id));
		}

		public static ulong ToUlong(this ProductUserId id)
		{
			return (ulong)(long)id.InnerHandle;
		}

		public static string GetIdString(this ProductUserId id)
		{
			id.ToString(out var outBuffer);
			return outBuffer;
		}
	}
}
