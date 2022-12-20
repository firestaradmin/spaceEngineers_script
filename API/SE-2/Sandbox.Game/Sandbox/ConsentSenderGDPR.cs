using System;
using System.Net;
using Sandbox.Engine.Networking;
using Sandbox.Game;
using VRage;
using VRage.Http;
using VRage.Utils;

namespace Sandbox
{
	public static class ConsentSenderGDPR
	{
		internal static void TrySendConsent()
		{
			try
			{
				ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
				HttpData[] parameters = new HttpData[5]
				{
					new HttpData("Content-Type", "application/x-www-form-urlencoded", HttpDataType.HttpHeader),
					new HttpData("User-Agent", "Space Engineers Client", HttpDataType.HttpHeader),
					new HttpData("lcvbex", MyPerGameSettings.BasicGameInfo.GameAcronym, HttpDataType.GetOrPost),
					new HttpData("qudfgh", MyGameService.UserId, HttpDataType.GetOrPost),
					new HttpData("praqnf", MySandboxGame.Config.GDPRConsent.Value ? "agree" : "disagree", HttpDataType.GetOrPost)
				};
				MyVRage.Platform.Http.SendRequestAsync("https://gdpr.keenswh.com/consent.php", parameters, HttpMethod.POST, HandleConsentResponse);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Cannot confirm GDPR consent: " + ex);
			}
		}

		private static void HandleConsentResponse(HttpStatusCode statusCode, string content)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			bool consent = false;
			try
			{
				if ((int)statusCode == 200)
				{
					content = content.Replace("\r", "");
					content = content.Replace("\n", "");
					consent = content == "OK";
				}
			}
			catch
			{
			}
			MySandboxGame.Static.Invoke(delegate
			{
				MySandboxGame.Config.GDPRConsentSent = consent;
				MySandboxGame.Config.Save();
			}, "HandleConsentResponse");
		}
	}
}
