using System.Security.Cryptography;

namespace VRage.Cryptography
{
	public static class MySHA256
	{
		private static bool m_supportsFips = true;

		private static SHA256 CreateInternal()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (!m_supportsFips)
			{
				return (SHA256)new SHA256Managed();
			}
			return (SHA256)new SHA256CryptoServiceProvider();
		}

		/// <summary>
		/// Creates FIPS compliant crypto provider if available, otherwise pure managed implementation.
		/// </summary>
		public static SHA256 Create()
		{
			try
			{
				return CreateInternal();
			}
			catch
			{
				m_supportsFips = false;
				return CreateInternal();
			}
		}
	}
}
