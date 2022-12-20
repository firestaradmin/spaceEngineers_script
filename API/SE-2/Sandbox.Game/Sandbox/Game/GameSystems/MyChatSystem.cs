using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.Chat;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 900)]
	public class MyChatSystem : MySessionComponentBase
	{
		protected sealed class OnFactionMessageRequest_003C_003ESandbox_Game_Entities_Character_MyUnifiedChatItem : ICallSite<IMyEventOwner, MyUnifiedChatItem, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyUnifiedChatItem item, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnFactionMessageRequest(item);
			}
		}

		protected sealed class OnFactionMessageSuccess_003C_003ESandbox_Game_Entities_Character_MyUnifiedChatItem : ICallSite<IMyEventOwner, MyUnifiedChatItem, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyUnifiedChatItem item, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnFactionMessageSuccess(item);
			}
		}

		private MyChatCommandSystem m_commandSystem = new MyChatCommandSystem();

		private ChatChannel m_currentChannel;

		private long m_targetPlayerId;

		public MyUnifiedChatHistory ChatHistory = new MyUnifiedChatHistory();

		public MyChatCommandSystem CommandSystem => m_commandSystem;

		public ChatChannel CurrentChannel => m_currentChannel;

		public long CurrentTarget => m_currentChannel switch
		{
			ChatChannel.Global => 0L, 
			ChatChannel.Faction => MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId)?.FactionId ?? 0, 
			ChatChannel.Private => m_targetPlayerId, 
			_ => 0L, 
		};

		public event Action<long> FactionMessageReceived;

		public static void AddFactionChatItem(MyUnifiedChatItem chatItem)
		{
			MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(ref chatItem);
		}

		public void ChangeChatChannel_Global()
		{
			m_currentChannel = ChatChannel.Global;
			m_targetPlayerId = 0L;
		}

		public void ChangeChatChannel_Faction()
		{
			m_currentChannel = ChatChannel.Faction;
			m_targetPlayerId = 0L;
		}

		public void ChangeChatChannel_Whisper(long playerId)
		{
			m_currentChannel = ChatChannel.Private;
			m_targetPlayerId = playerId;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			CommandSystem.Init();
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			CommandSystem.Init();
		}

		protected override void UnloadData()
		{
			CommandSystem.Unload();
			base.UnloadData();
		}

		public void OnNewFactionMessage(ref MyUnifiedChatItem item)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
			if (!Sync.IsDedicated && (myFaction != null || !MySession.Static.IsUserAdmin(Sync.MyId)))
			{
				this.FactionMessageReceived?.Invoke(item.TargetId);
			}
		}

		private void ShowNewMessageHudNotification(MyHudNotification notification)
		{
			MyHud.Notifications.Add(notification);
		}

		public override void UpdateAfterSimulation()
		{
		}

		public void SendNewFactionMessage(MyUnifiedChatItem chatItem)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnFactionMessageRequest, chatItem);
		}

		[Event(null, 142)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		private static void OnFactionMessageRequest(MyUnifiedChatItem item)
		{
			if (item.Text.Length == 0 || item.Text.Length > 200)
			{
				return;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(item.SenderId);
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(item.TargetId);
			if (myIdentity == null || myFaction == null)
			{
				return;
			}
			bool flag = false;
			ulong num = 0uL;
			if (!myFaction.IsMember(item.SenderId) && MySession.Static.Players.TryGetPlayerId(item.SenderId, out var result))
			{
				num = result.SteamId;
				flag |= MySession.Static.IsUserAdmin(num);
			}
			foreach (KeyValuePair<long, MyFactionMember> member in myFaction.Members)
			{
				MySession.Static.Players.TryGetPlayerId(member.Value.PlayerId, out var result2);
				ulong steamId = result2.SteamId;
				if (steamId != 0L)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnFactionMessageSuccess, item, new EndpointId(steamId));
				}
			}
			if (flag)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnFactionMessageSuccess, item, new EndpointId(num));
			}
		}

		[Event(null, 185)]
		[Reliable]
		[Client]
		private static void OnFactionMessageSuccess(MyUnifiedChatItem item)
		{
			long senderId = item.SenderId;
			if (!Sync.IsServer || senderId == MySession.Static.LocalPlayerId)
			{
				AddFactionChatItem(item);
				if (MyMultiplayer.Static != null)
				{
					ulong author = MySession.Static.Players.TryGetSteamId(item.SenderId);
					ChatMsg chatMsg = default(ChatMsg);
					chatMsg.Text = item.Text;
					chatMsg.Author = author;
					chatMsg.Channel = (byte)item.Channel;
					chatMsg.TargetId = item.TargetId;
					chatMsg.CustomAuthorName = string.Empty;
					ChatMsg msg = chatMsg;
					MyMultiplayer.Static.OnChatMessage(ref msg);
				}
			}
			if (senderId != MySession.Static.LocalPlayerId)
			{
				MySession.Static.Gpss.ScanText(item.Text, MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewFromFactionComms));
			}
			MySession.Static.ChatSystem.OnNewFactionMessage(ref item);
		}

		public static Color GetRelationColor(long identityId)
		{
			if (MySession.Static.IsUserAdmin(MySession.Static.Players.TryGetSteamId(identityId)))
			{
				return Color.Purple;
			}
			return MyIDModule.GetRelationPlayerPlayer(MySession.Static.LocalPlayerId, identityId) switch
			{
				MyRelationsBetweenPlayers.Self => Color.CornflowerBlue, 
				MyRelationsBetweenPlayers.Allies => Color.LightGreen, 
				MyRelationsBetweenPlayers.Neutral => Color.PaleGoldenrod, 
				MyRelationsBetweenPlayers.Enemies => Color.Crimson, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}

		public static Color GetChannelColor(ChatChannel channel)
		{
			switch (channel)
			{
			case ChatChannel.Faction:
				return Color.LimeGreen;
			case ChatChannel.Private:
				return Color.Violet;
			case ChatChannel.Global:
			case ChatChannel.GlobalScripted:
			case ChatChannel.ChatBot:
				return Color.White;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
