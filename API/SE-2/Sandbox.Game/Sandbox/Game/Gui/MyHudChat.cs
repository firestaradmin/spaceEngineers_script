using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudChat
	{
		private static readonly int MAX_MESSAGES_IN_CHAT_DEFAULT = 10;

		private static readonly int MAX_MESSAGE_TIME_DEFAULT = 15000;

		public static int MaxMessageTime = MAX_MESSAGE_TIME_DEFAULT;

		public static int MaxMessageCount = MAX_MESSAGES_IN_CHAT_DEFAULT;

		public Queue<MyChatItem> MessagesQueue = new Queue<MyChatItem>();

		public List<MyChatItem> MessageHistory = new List<MyChatItem>();

		private int m_lastUpdateTime = int.MaxValue;

		private int m_lastScreenUpdateTime = int.MaxValue;

		public MyHudControlChat ChatControl;

		private bool m_chatScreenOpen;

		public int Timestamp { get; private set; }

		public int LastUpdateTime => m_lastUpdateTime;

		public int TimeSinceLastUpdate => MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastScreenUpdateTime;

		public MyHudChat()
		{
			Timestamp = 0;
		}

		public void RegisterChat(MyMultiplayerBase multiplayer)
		{
			if (multiplayer != null)
			{
				multiplayer.ChatMessageReceived += OnMultiplayer_ChatMessageReceived;
				multiplayer.ScriptedChatMessageReceived += multiplayer_ScriptedChatMessageReceived;
			}
		}

		public void UnregisterChat(MyMultiplayerBase multiplayer)
		{
			if (multiplayer != null)
			{
				multiplayer.ChatMessageReceived -= OnMultiplayer_ChatMessageReceived;
				multiplayer.ScriptedChatMessageReceived -= multiplayer_ScriptedChatMessageReceived;
				MessagesQueue.Clear();
				UpdateTimestamp();
			}
		}

		public void ShowMessageScripted(string sender, string messageText)
		{
			Color paleGoldenrod = Color.PaleGoldenrod;
			Color white = Color.White;
			ShowMessage(sender, messageText, paleGoldenrod, white);
		}

		public void ShowMessage(string sender, string messageText, Color color, string font = "Blue")
		{
			MyChatItem myChatItem = new MyChatItem(sender, messageText, font, color);
			MessagesQueue.Enqueue(myChatItem);
			MessageHistory.Add(myChatItem);
			m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (MessagesQueue.get_Count() > MaxMessageCount)
			{
				MessagesQueue.Dequeue();
			}
			UpdateTimestamp();
		}

		public void ShowMessage(string sender, string messageText, string font = "Blue")
		{
			ShowMessage(sender, messageText, Color.White, font);
		}

		public void ShowMessageColoredSP(string text, ChatChannel channel, long targetId = 0L, string customAuthorName = null)
		{
			string empty = string.Empty;
			if (channel == ChatChannel.Private)
			{
				if (targetId == MySession.Static.LocalPlayerId)
				{
					empty = MySession.Static.LocalHumanPlayer.DisplayName;
					empty = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_From), empty);
				}
				else
				{
					empty = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_To), targetId);
				}
			}
			else
			{
				empty = MySession.Static.LocalHumanPlayer.DisplayName;
			}
			long num = MySession.Static.Players.TryGetIdentityId(Sync.MyId);
			Color relationColor = MyChatSystem.GetRelationColor(num);
			Color channelColor = MyChatSystem.GetChannelColor(channel);
			ShowMessage(string.IsNullOrEmpty(customAuthorName) ? empty : customAuthorName, text, relationColor, channelColor);
			if (channel == ChatChannel.GlobalScripted)
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(text, string.IsNullOrEmpty(customAuthorName) ? MyTexts.GetString(MySpaceTexts.ChatBotName) : customAuthorName);
			}
			else
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(text, channel, num, targetId);
			}
		}

		public void ShowMessage(string sender, string message, Color senderColor)
		{
			ShowMessage(sender, message, senderColor, Color.White);
		}

		public void ShowMessage(string sender, string message, Color senderColor, Color messageColor)
		{
			MyChatItem myChatItem = new MyChatItem(sender, message, "White", senderColor, messageColor);
			MessagesQueue.Enqueue(myChatItem);
			MessageHistory.Add(myChatItem);
			m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (MessagesQueue.get_Count() > MaxMessageCount)
			{
				MessagesQueue.Dequeue();
			}
			UpdateTimestamp();
		}

		private void OnMultiplayer_ChatMessageReceived(ulong steamUserId, string messageText, ChatChannel channel, long targetId, string customAuthorName = null)
		{
			if (!MyGameService.IsActive || MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			long num = MySession.Static.Players.TryGetIdentityId(steamUserId);
<<<<<<< HEAD
			string text = MyMultiplayer.Static.GetMemberName(steamUserId);
			switch (channel)
=======
			string text = string.Empty;
			if (channel == ChatChannel.Private)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
			case ChatChannel.Private:
				if (targetId == MySession.Static.LocalPlayerId)
				{
					text = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_From), text);
					break;
				}
				if (num == MySession.Static.LocalPlayerId)
				{
<<<<<<< HEAD
					ulong num2 = MySession.Static.Players.TryGetSteamId(targetId);
					if (num2 != 0L)
					{
=======
					if (num != MySession.Static.LocalPlayerId)
					{
						return;
					}
					ulong num2 = MySession.Static.Players.TryGetSteamId(targetId);
					if (num2 != 0L)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						text = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_To), MyMultiplayer.Static.GetMemberName(num2));
					}
					break;
				}
				return;
			case ChatChannel.Faction:
				foreach (MyPlayer.PlayerId allPlayer in MySession.Static.Players.GetAllPlayers())
				{
					if (allPlayer.SteamId == steamUserId)
					{
						MyPlayer playerById = MySession.Static.Players.GetPlayerById(allPlayer);
						if (playerById != null)
						{
							IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerById.Identity.IdentityId);
							IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalHumanPlayer.Identity.IdentityId);
							if (myFaction != null && myFaction2 != null && myFaction.FactionId == myFaction2.FactionId)
							{
								break;
							}
							return;
						}
						break;
					}
				}
				break;
			}
<<<<<<< HEAD
=======
			else
			{
				text = MyMultiplayer.Static.GetMemberName(steamUserId);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Color relationColor = MyChatSystem.GetRelationColor(num);
			Color channelColor = MyChatSystem.GetChannelColor(channel);
			ShowMessage(string.IsNullOrEmpty(customAuthorName) ? text : customAuthorName, messageText, relationColor, channelColor);
			if (channel == ChatChannel.GlobalScripted)
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(messageText, string.IsNullOrEmpty(customAuthorName) ? MyTexts.GetString(MySpaceTexts.ChatBotName) : customAuthorName);
			}
			else
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(messageText, channel, num, targetId);
			}
		}

		public void multiplayer_ScriptedChatMessageReceived(string message, string author, string font, Color color)
		{
			ShowMessage(author, message, color, font);
			MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(message, author, font);
		}

		private void UpdateTimestamp()
		{
			Timestamp++;
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public void Update()
		{
			if (m_chatScreenOpen)
			{
				m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime > MaxMessageTime && MessagesQueue.get_Count() > 0)
			{
				MessagesQueue.Dequeue();
				UpdateTimestamp();
			}
		}

		public static void ResetChatSettings()
		{
			MaxMessageTime = MAX_MESSAGE_TIME_DEFAULT;
			MaxMessageCount = MAX_MESSAGES_IN_CHAT_DEFAULT;
		}

		public void ChatOpened()
		{
			m_chatScreenOpen = true;
		}

		public void ChatClosed()
		{
			m_chatScreenOpen = false;
		}
	}
}
