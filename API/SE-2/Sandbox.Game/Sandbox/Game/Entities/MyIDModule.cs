using System;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	/// <summary>
	/// This should be replaced by MyEntityOwnershipComponent
	/// </summary>
	public class MyIDModule
	{
		public long Owner { get; set; }

		public MyOwnershipShareModeEnum ShareMode { get; set; }

		public MyIDModule()
			: this(0L, MyOwnershipShareModeEnum.None)
		{
		}

		public MyIDModule(long owner, MyOwnershipShareModeEnum shareMode)
		{
			Owner = owner;
			ShareMode = shareMode;
		}

		public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long identityId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
		{
			return GetRelationPlayerBlock(Owner, identityId, ShareMode, MyRelationsBetweenPlayerAndBlock.Enemies, MyRelationsBetweenFactions.Enemies, MyRelationsBetweenPlayerAndBlock.FactionShare, defaultNoUser);
		}

		public static MyRelationsBetweenPlayers GetRelationPlayerPlayer(long owner, long user, MyRelationsBetweenFactions defaultFactionRelations = MyRelationsBetweenFactions.Enemies, MyRelationsBetweenPlayers defaultNoFactionRelation = MyRelationsBetweenPlayers.Enemies)
		{
			if (owner == user)
			{
				return MyRelationsBetweenPlayers.Self;
			}
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(user);
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(owner);
			if (myFaction == null && myFaction2 == null)
			{
				return defaultNoFactionRelation;
			}
			if (myFaction == myFaction2)
			{
				return MyRelationsBetweenPlayers.Allies;
			}
			if (myFaction == null)
			{
				return ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(user, myFaction2.FactionId).Item1);
			}
			if (myFaction2 == null)
			{
				return ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(owner, myFaction.FactionId).Item1);
			}
			int item = 0;
			if (MySession.Static != null)
			{
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component != null)
				{
					item = component.TranslateRelationToReputation(defaultFactionRelations);
				}
			}
			bool flag = MySession.Static.Factions.IsNpcFaction(myFaction.Tag);
			bool flag2 = MySession.Static.Factions.IsNpcFaction(myFaction2.Tag);
			bool flag3 = Sync.Players.IdentityIsNpc(user);
			bool flag4 = Sync.Players.IdentityIsNpc(owner);
			bool flag5 = flag != flag2;
			if (flag5 || (flag && flag3 && !flag4) || (flag2 && flag4 && !flag3))
			{
				if (flag && (flag5 || (flag3 && !flag4)))
				{
					return ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(owner, myFaction.FactionId).Item1);
				}
				if (flag2 && (flag5 || (flag4 && !flag3)))
				{
					return ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(user, myFaction2.FactionId).Item1);
				}
			}
			return ConvertToPlayerRelation(MySession.Static.Factions.GetRelationBetweenFactions(myFaction2.FactionId, myFaction.FactionId, new Tuple<MyRelationsBetweenFactions, int>(defaultFactionRelations, item)).Item1);
		}

		public static MyRelationsBetweenPlayerAndBlock GetRelationPlayerBlock(long owner, long user, MyOwnershipShareModeEnum share = MyOwnershipShareModeEnum.None, MyRelationsBetweenPlayerAndBlock noFactionResult = MyRelationsBetweenPlayerAndBlock.Enemies, MyRelationsBetweenFactions defaultFactionRelations = MyRelationsBetweenFactions.Enemies, MyRelationsBetweenPlayerAndBlock defaultShareWithAllRelations = MyRelationsBetweenPlayerAndBlock.FactionShare, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
		{
			if (!MyFakes.SHOW_FACTIONS_GUI)
			{
				return MyRelationsBetweenPlayerAndBlock.NoOwnership;
			}
			if (owner == user)
			{
				return MyRelationsBetweenPlayerAndBlock.Owner;
			}
			if (user == 0L)
			{
				return defaultNoUser;
			}
			if (owner == 0L)
			{
				return MyRelationsBetweenPlayerAndBlock.NoOwnership;
			}
			if (MySession.Static == null || MySession.Static.Factions == null)
			{
				MyLog.Default.Error("Session or factions not ready - GetRelationshipPlayerBlock");
				return MyRelationsBetweenPlayerAndBlock.Enemies;
			}
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(user);
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(owner);
			if (myFaction != null && myFaction == myFaction2 && share == MyOwnershipShareModeEnum.Faction)
			{
				return MyRelationsBetweenPlayerAndBlock.FactionShare;
			}
			if (share == MyOwnershipShareModeEnum.All)
			{
				return defaultShareWithAllRelations;
			}
			if (myFaction == null && myFaction2 == null)
			{
				return noFactionResult;
			}
			return ConvertToPlayerBlockRelation(GetRelationPlayerPlayer(user, owner, defaultFactionRelations, ConvertToPlayerRelation(noFactionResult)));
		}

		private static MyRelationsBetweenPlayerAndBlock ConvertToPlayerBlockRelation(MyRelationsBetweenFactions factionRelation)
		{
			switch (factionRelation)
			{
			case MyRelationsBetweenFactions.Allies:
			case MyRelationsBetweenFactions.Friends:
				return MyRelationsBetweenPlayerAndBlock.Friends;
			case MyRelationsBetweenFactions.Neutral:
				return MyRelationsBetweenPlayerAndBlock.Neutral;
			case MyRelationsBetweenFactions.Enemies:
				return MyRelationsBetweenPlayerAndBlock.Enemies;
			default:
				return MyRelationsBetweenPlayerAndBlock.Enemies;
			}
		}

		private static MyRelationsBetweenPlayerAndBlock ConvertToPlayerBlockRelation(MyRelationsBetweenPlayers playerRelation)
		{
			switch (playerRelation)
			{
			case MyRelationsBetweenPlayers.Self:
			case MyRelationsBetweenPlayers.Allies:
				return MyRelationsBetweenPlayerAndBlock.Friends;
			case MyRelationsBetweenPlayers.Neutral:
				return MyRelationsBetweenPlayerAndBlock.Neutral;
			case MyRelationsBetweenPlayers.Enemies:
				return MyRelationsBetweenPlayerAndBlock.Enemies;
			default:
				return MyRelationsBetweenPlayerAndBlock.Enemies;
			}
		}

		private static MyRelationsBetweenPlayers ConvertToPlayerRelation(MyRelationsBetweenFactions factionRelation)
		{
			switch (factionRelation)
			{
			case MyRelationsBetweenFactions.Allies:
			case MyRelationsBetweenFactions.Friends:
				return MyRelationsBetweenPlayers.Allies;
			case MyRelationsBetweenFactions.Neutral:
				return MyRelationsBetweenPlayers.Neutral;
			case MyRelationsBetweenFactions.Enemies:
				return MyRelationsBetweenPlayers.Enemies;
			default:
				return MyRelationsBetweenPlayers.Enemies;
			}
		}

		private static MyRelationsBetweenPlayers ConvertToPlayerRelation(MyRelationsBetweenPlayerAndBlock blockRelation)
		{
			switch (blockRelation)
			{
			case MyRelationsBetweenPlayerAndBlock.Owner:
			case MyRelationsBetweenPlayerAndBlock.FactionShare:
			case MyRelationsBetweenPlayerAndBlock.Friends:
				return MyRelationsBetweenPlayers.Allies;
			case MyRelationsBetweenPlayerAndBlock.NoOwnership:
			case MyRelationsBetweenPlayerAndBlock.Neutral:
				return MyRelationsBetweenPlayers.Neutral;
			case MyRelationsBetweenPlayerAndBlock.Enemies:
				return MyRelationsBetweenPlayers.Enemies;
			default:
				return MyRelationsBetweenPlayers.Enemies;
			}
		}
	}
}
