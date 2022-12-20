using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
<<<<<<< HEAD
using Sandbox.Engine.Networking;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems.Chat
{
	public static class MyChatCommands
	{
		private static StringBuilder m_tempBuilder = new StringBuilder();

		public static void PreloadCommands(MyChatCommandSystem system)
		{
			AnimationCommands.LoadAnimations(system);
		}

		[ChatCommand("/help", "ChatCommand_Help_Help", "ChatCommand_HelpSimple_Help", MyPromoteLevel.None)]
		private static void CommandHelp(string[] args)
		{
			MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(Sync.MyId);
			if (args == null || args.Length == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(MyTexts.GetString(MyCommonTexts.ChatCommand_AvailableControls));
				stringBuilder.Append("PageUp/Down - " + MyTexts.GetString(MyCommonTexts.ChatCommand_PageUpdown));
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.ChatCommand_AvailableCommands));
				foreach (KeyValuePair<string, IMyChatCommand> chatCommand in MySession.Static.ChatSystem.CommandSystem.ChatCommands)
				{
					if (userPromoteLevel >= chatCommand.Value.VisibleTo)
					{
						stringBuilder2.Append(chatCommand.Key);
						stringBuilder2.Append(", ");
					}
				}
				stringBuilder2.Remove(stringBuilder2.Length - 2, 2);
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), stringBuilder.ToString(), Color.Red);
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), stringBuilder2.ToString(), Color.Red);
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Help), Color.Red);
				return;
			}
			if (!MySession.Static.ChatSystem.CommandSystem.ChatCommands.TryGetValue(args[0], out var value))
			{
				List<IMyChatCommand> list = new List<IMyChatCommand>();
				AnimationCommands.GetAnimationCommands(args[0], string.Empty, list);
				if (list.Count <= 0)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NotFound), args[0]), Color.Red);
					return;
				}
				value = list[0];
			}
			if (userPromoteLevel < value.VisibleTo)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoPermission), Color.Red);
			}
			else
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), value.CommandText + ": " + MyTexts.GetString(MyStringId.GetOrCompute(value.HelpText)), Color.Red);
			}
		}

		[ChatCommand("/gps", "ChatCommand_Help_GPS", "ChatCommand_HelpSimple_GPS", MyPromoteLevel.None)]
		private static void CommandGPS(string[] args)
		{
			if (MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			MyGps gps = new MyGps();
			MySession.Static.Gpss.GetNameForNewCurrent(m_tempBuilder);
			gps.Name = m_tempBuilder.ToString();
			gps.Description = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromCurrent_Desc).ToString();
			gps.Coords = MySession.Static.LocalHumanPlayer.GetPosition();
			gps.ShowOnHud = true;
			gps.DiscardAt = null;
			if (args != null && args.Length != 0)
			{
				if (args[0] == "share")
				{
					MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
					if (MyMultiplayer.Static != null)
					{
						MyMultiplayer.Static.SendChatMessage(gps.ToString(), ChatChannel.Global, 0L);
					}
					else
					{
						MyHud.Chat.ShowMessageScripted(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_GPSRequireOnline));
					}
					return;
				}
				if (args[0] == "faction")
				{
					MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
					MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId);
					if (playerFaction == null)
					{
						MyHud.Chat.ShowMessage(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_GPSRequireFaction));
					}
					else if (MyMultiplayer.Static != null)
					{
						MyMultiplayer.Static.SendChatMessage(gps.ToString(), ChatChannel.Faction, playerFaction.FactionId);
					}
					else
					{
						MyHud.Chat.ShowMessageScripted(MyTexts.GetString(MySpaceTexts.ChatBotName), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_GPSRequireOnline));
					}
					return;
				}
				string text = args[0];
				for (int i = 1; i < args.Length; i++)
				{
					text = text + " " + args[i];
				}
				if (MySession.Static.Gpss.GetGpsByName(MySession.Static.LocalPlayerId, text) != null)
				{
					bool flag = true;
					int num = 1;
					do
					{
						num++;
					}
					while (MySession.Static.Gpss.GetGpsByName(MySession.Static.LocalPlayerId, text + "_" + num) != null);
					text = text + "_" + num;
				}
				gps.Name = text;
				MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
			}
			else
			{
				MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
			}
		}

		[ChatCommand("/g", "ChatCommand_Help_G", "ChatCommand_HelpSimple_G", MyPromoteLevel.None)]
		private static void CommandChannelGlobal(string[] args)
		{
			MySession.Static.ChatSystem.ChangeChatChannel_Global();
			if (args != null && args.Length != 0)
			{
				string text = args[0];
				for (int i = 1; i < args.Length; i++)
				{
					text = text + " " + args[i];
				}
				if (!string.IsNullOrEmpty(text))
				{
					MyGuiScreenChat.SendChatMessage(text);
				}
			}
		}

		[ChatCommand("/f", "ChatCommand_Help_F", "ChatCommand_HelpSimple_F", MyPromoteLevel.None)]
		private static void CommandChannelFaction(string[] args)
		{
			long num = MySession.Static.Players.TryGetIdentityId(Sync.MyId);
			if (num == 0L)
			{
				return;
			}
			if (MySession.Static.Factions.GetPlayerFaction(num) == null)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_FactionChatTarget), Color.Red);
				return;
			}
			MySession.Static.ChatSystem.ChangeChatChannel_Faction();
			if (args != null && args.Length != 0)
			{
				string text = args[0];
				for (int i = 1; i < args.Length; i++)
				{
					text = text + " " + args[i];
				}
				if (!string.IsNullOrEmpty(text))
				{
					MyGuiScreenChat.SendChatMessage(text);
				}
			}
		}

		[ChatCommand("/w", "ChatCommand_Help_W", "ChatCommand_HelpSimple_W", MyPromoteLevel.None)]
		private static void CommandChannelWhisper(string[] args)
		{
			string empty = string.Empty;
			string text = string.Empty;
			if (args == null || Enumerable.Count<string>((IEnumerable<string>)args) < 1)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_WhisperChatTarget));
				return;
			}
			if (args[0].Length > 0 && args[0][0] == '"')
			{
				int i = 0;
				bool flag = false;
				for (; i < args.Length; i++)
				{
					if (args[i][args[i].Length - 1] == '"')
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (i == 0)
					{
						empty = ((args[0].Length > 2) ? args[0].Substring(1, args[0].Length - 2) : string.Empty);
						if (i < args.Length - 1)
						{
							string text2 = args[1];
							for (int j = 2; j < args.Length; j++)
							{
								text2 = text2 + " " + args[j];
							}
							text = text2;
						}
					}
					else
					{
						string text3 = args[0];
						for (int k = 1; k <= i; k++)
						{
							text3 = text3 + " " + args[k];
						}
						empty = ((text3.Length > 2) ? text3.Substring(1, text3.Length - 2) : string.Empty);
						if (i < args.Length - 1)
						{
							string text4 = args[i + 1];
							for (int k = i + 2; k < args.Length; k++)
							{
								text4 = text4 + " " + args[k];
							}
							text = text4;
						}
					}
				}
				else
				{
					empty = args[0];
					if (args.Length > 1)
					{
						string text5 = args[1];
						for (int l = 2; l < args.Length; l++)
						{
							text5 = text5 + " " + args[l];
						}
						text = text5;
					}
				}
			}
			else
			{
				empty = args[0];
				if (args.Length > 1)
				{
					string text6 = args[1];
					for (int m = 2; m < args.Length; m++)
					{
						text6 = text6 + " " + args[m];
					}
					if (string.IsNullOrEmpty(text6))
					{
						return;
					}
					text = text6;
				}
			}
			MyPlayer playerByName = MySession.Static.Players.GetPlayerByName(empty);
			if (playerByName == null)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_WhisperTargetNotFound), empty));
				return;
			}
			MySession.Static.ChatSystem.ChangeChatChannel_Whisper(playerByName.Identity.IdentityId);
			if (!string.IsNullOrEmpty(text))
			{
				MyGuiScreenChat.SendChatMessage(text);
			}
		}

		[ChatCommand("/timestamp", "ChatCommand_Help_Timestamp", "ChatCommand_HelpSimple_Timestamp", MyPromoteLevel.None)]
		private static void CommandTiemstampToggle(string[] args)
		{
			if (args != null && args.Length != 0)
			{
				if (args[0].Equals("on") || args[0].Equals("true"))
				{
					MySandboxGame.Config.ShowChatTimestamp = true;
					MySandboxGame.Config.Save();
				}
				else if (args[0].Equals("off") || args[0].Equals("false"))
				{
					MySandboxGame.Config.ShowChatTimestamp = false;
					MySandboxGame.Config.Save();
				}
			}
		}

		[ChatCommand("/smite", "ChatCommand_Help_Smite", "ChatCommand_HelpSimple_Smite", MyPromoteLevel.Admin)]
		private static void CommandSmite(string[] args)
		{
			MyAPIGateway.Physics.CastRay(MySector.MainCamera.WorldMatrix.Translation, MySector.MainCamera.WorldMatrix.Translation + MySector.MainCamera.WorldMatrix.Forward * 10000.0, out var hitInfo);
			if (hitInfo != null)
			{
				MySession.Static.GetComponent<MySectorWeatherComponent>().RequestLightning(MySector.MainCamera.WorldMatrix.Translation, hitInfo.Position);
			}
		}

		[ChatCommand("/rweather", "ChatCommand_Help_RWeather", "ChatCommand_HelpSimple_RWeather", MyPromoteLevel.Admin)]
		private static void CommandRWeather(string[] args)
		{
			if (MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(MySector.MainCamera.Position, out var weatherEffect))
			{
				MySession.Static.GetComponent<MySectorWeatherComponent>().RemoveWeather(weatherEffect);
			}
			MySession.Static.GetComponent<MySectorWeatherComponent>().CreateRandomWeather(MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position), verbose: true);
		}

		[ChatCommand("/weatherlist", "ChatCommand_Help_Weatherlist", "ChatCommand_HelpSimple_Weatherlist", MyPromoteLevel.Admin)]
		private static void CommandWeatherList(string[] args)
		{
<<<<<<< HEAD
			MyWeatherEffectDefinition[] array = MyDefinitionManager.Static.GetWeatherDefinitions().ToArray();
=======
			MyWeatherEffectDefinition[] array = Enumerable.ToArray<MyWeatherEffectDefinition>((IEnumerable<MyWeatherEffectDefinition>)MyDefinitionManager.Static.GetWeatherDefinitions());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Public)
				{
					text += array[i].Id.SubtypeName;
					if (i < array.Length - 1)
					{
						text += ", ";
					}
				}
			}
			MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_AvailableWeathers).ToString() + text, Color.Red);
		}

		[ChatCommand("/weather", "ChatCommand_Help_Weather", "ChatCommand_HelpSimple_Weather", MyPromoteLevel.Admin)]
		private static void CommandWeather(string[] args)
		{
<<<<<<< HEAD
			if (!Sync.IsServer && !MySession.Static.IsUserSpaceMaster(MyGameService.UserId))
			{
				return;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (args == null || args.Length == 0)
			{
				string weather = MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(MySector.MainCamera.Position);
				if (weather != null)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_CurrentWeather).ToString() + weather, Color.Red);
				}
				else
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_NoWeather).ToString(), Color.Red);
				}
			}
			else if (args.Length == 1 || args.Length == 2)
			{
				int result = 0;
				if (args.Length == 2)
				{
					int.TryParse(args[1], out result);
				}
				MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(args[0], result, null, verbose: true, Vector3.Zero);
			}
		}
	}
}
