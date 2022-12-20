using System;
using Epic.OnlineServices;
using VRage.GameServices;

namespace VRage.EOS
{
	internal static class EOSGameServiceExtensions
	{
		public static ExternalAccountType GetServiceKind(this IMyGameService gameService)
		{
			string serviceName = gameService.ServiceName;
			switch (serviceName)
			{
			case "Steam":
				return ExternalAccountType.Steam;
			case "Xbox Live":
				return ExternalAccountType.Xbl;
			case "EOS":
				return ExternalAccountType.Epic;
			default:
			{
				string text = serviceName;
				throw new Exception("Unknown service " + text);
			}
			}
		}
	}
}
