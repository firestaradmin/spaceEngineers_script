using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
	public interface IMyFactionCollection
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets new dictionary with all factions. As keys used factionId
		/// </summary>
		Dictionary<long, IMyFaction> Factions { get; }

		/// <summary>
		/// Called when faction <see cref="P:VRage.Game.ModAPI.IMyFaction.AutoAcceptMember" /> and <see cref="P:VRage.Game.ModAPI.IMyFaction.AutoAcceptPeace" /> changed
		/// </summary>
=======
		Dictionary<long, IMyFaction> Factions { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		event Action<long, bool, bool> FactionAutoAcceptChanged;

		/// <summary>
		/// Called when faction somehow changes.
		/// </summary>
		event Action<long> FactionEdited;

		/// <summary>
		/// Called when new faction created. FactionId is used as argument
		/// </summary>
		event Action<long> FactionCreated;

		/// <summary>
		/// Called when on of factions changed Arguments:
		/// action,
		/// fromFactionId, faction Id
		/// toFactionId, faction Id, or 0
		/// playerId - IdentityId on whom action was applied, or 0
		/// senderId - IdentityId who triggered state change, or 0
		/// </summary>
		event Action<MyFactionStateChange, long, long, long, long> FactionStateChanged;

		/// <summary>
		/// Gets if faction with provided tag exists
		/// </summary>
		/// <param name="tag">Tag to check</param>
		/// <param name="doNotCheck">Faction to ignore</param>
		/// <returns>True if faction with that tag exists, and it is not ignored</returns>
		bool FactionTagExists(string tag, IMyFaction doNotCheck = null);

		/// <summary>
		/// Gets if faction with provided tag exists
		/// </summary>
		/// <param name="name">Name to check</param>
		/// <param name="doNotCheck">Faction to ignore</param>
		/// <returns>True if faction with that tag exists, and it is not ignored</returns>
		bool FactionNameExists(string name, IMyFaction doNotCheck = null);

		/// <summary>
		/// Tries get faction by faction id
		/// </summary>
		/// <param name="factionId">Id of faction</param>
		/// <returns>Faction or null</returns>
		IMyFaction TryGetFactionById(long factionId);

		/// <summary>
		/// Tries get faction that has member with provided id
		/// </summary>
		/// <param name="playerId">IdentityId of player</param>
		/// <returns>Faction or null</returns>
		IMyFaction TryGetPlayerFaction(long playerId);

		/// <summary>
		/// Tries get faction with provided tag
		/// </summary>
		/// <param name="tag">Tag of faction</param>
		/// <returns>Faction or null</returns>
		IMyFaction TryGetFactionByTag(string tag);

		/// <summary>
		/// Tries get faction with provided name
		/// </summary>
		/// <param name="name">Name of faction</param>
		/// <returns>Faction or null</returns>
		IMyFaction TryGetFactionByName(string name);

		[Obsolete("Use SendJoinRequest instead, this will be removed in future")]
		void AddPlayerToFaction(long playerId, long factionId);

		[Obsolete("Use KickMember instead, this will be removed in future")]
		void KickPlayerFromFaction(long playerId);

		/// <summary>
		/// Gets relation between 2 factions
		/// </summary>
		/// <param name="factionId1">Faction id</param>
		/// <param name="factionId2">Faction id</param>
		/// <returns>Relation enum</returns>
		MyRelationsBetweenFactions GetRelationBetweenFactions(long factionId1, long factionId2);

		/// <summary>
		/// Gets reputation between 2 factions
		/// </summary>
		/// <param name="factionId1">Faction id</param>
		/// <param name="factionId2">Faction id</param>
		/// <returns>Reputation</returns>
		int GetReputationBetweenFactions(long factionId1, long factionId2);

		/// <summary>
		/// Sets reputation between 2 factions
		/// Note: Faction 1 always has same reputation to Faction 2, as Faction 2 to Faction 1
		/// </summary>
		/// <param name="fromFactionId">Faction 1</param>
		/// <param name="toFactionId">Faction 2</param>
		/// <param name="reputation">Desired reputation</param>
		void SetReputation(long fromFactionId, long toFactionId, int reputation);

		/// <summary>
		/// Gets reputation between identity and faction
		/// </summary>
		/// <param name="identityId">Player IdentityId</param>
		/// <param name="factionId">Faction id</param>
		/// <returns>Reputation</returns>
		int GetReputationBetweenPlayerAndFaction(long identityId, long factionId);

		/// <summary>
		/// Sets reputation between player and faction. 
		/// </summary>
		/// <param name="identityId">IdentityId</param>
		/// <param name="factionId">Faction Id</param>
		/// <param name="reputation">Desired reputation</param>
		void SetReputationBetweenPlayerAndFaction(long identityId, long factionId, int reputation);

		/// <summary>
		/// Gets if factions are enemies to each other
		/// </summary>
		/// <param name="factionId1">Faction id 1</param>
		/// <param name="factionId2">Faction id 2</param>
		/// <returns></returns>
		bool AreFactionsEnemies(long factionId1, long factionId2);

		/// <summary>
		/// Gets if there is peace request sent
		/// </summary>
		/// <param name="myFactionId"></param>
		/// <param name="foreignFactionId"></param>
		/// <returns></returns>
		bool IsPeaceRequestStateSent(long myFactionId, long foreignFactionId);

		bool IsPeaceRequestStatePending(long myFactionId, long foreignFactionId);

		/// <summary>
		/// Remove faction by id
		/// </summary>
		/// <param name="factionId">Faction id</param>
		void RemoveFaction(long factionId);

		/// <summary>
		/// Send peace request from one faction to another
		/// </summary>
		/// <param name="fromFactionId">From faction</param>
		/// <param name="toFactionId">To faction</param>
		void SendPeaceRequest(long fromFactionId, long toFactionId);

		/// <summary>
		/// Cancel peace request from one faction to another
		/// </summary>
		/// <param name="fromFactionId">From faction</param>
		/// <param name="toFactionId">To faction</param>
		void CancelPeaceRequest(long fromFactionId, long toFactionId);

		/// <summary>
		/// Accepts peace
		/// </summary>
		/// <param name="fromFactionId">Faction that sent peace request</param>
		/// <param name="toFactionId">Faction that received peace request</param>
		void AcceptPeace(long fromFactionId, long toFactionId);

		/// <summary>
		/// Declare war
		/// </summary>
		/// <param name="fromFactionId">Faction that declares war</param>
		/// <param name="toFactionId">Faction that receive war declaration</param>
		void DeclareWar(long fromFactionId, long toFactionId);

		/// <summary>
		/// Send faction join request
		/// </summary>
		/// <param name="factionId">Faction to join</param>
		/// <param name="playerId">Player Identity Id</param>
		void SendJoinRequest(long factionId, long playerId);

		/// <summary>
		/// Cancels player faction join request
		/// </summary>
		/// <param name="factionId">Faction to join</param>
		/// <param name="playerId">Player that sent join request</param>
		void CancelJoinRequest(long factionId, long playerId);

		/// <summary>
		/// Accepts faction join request
		/// </summary>
		/// <param name="factionId">Faction that accepts join</param>
		/// <param name="playerId">Player id</param>
		void AcceptJoin(long factionId, long playerId);

		/// <summary>
		/// Kicks member from faction
		/// </summary>
		/// <param name="factionId">Faction that has player</param>
		/// <param name="playerId">Player IdentityId to kick</param>
		void KickMember(long factionId, long playerId);

		/// <summary>
		/// Promotes faction member
		/// </summary>
		/// <param name="factionId">Faction that has player</param>
		/// <param name="playerId">Player IdentityId to promote</param>
		void PromoteMember(long factionId, long playerId);

		/// <summary>
		/// Demote faction member
		/// </summary>
		/// <param name="factionId">Faction that has player</param>
		/// <param name="playerId">Player IdentityId to demote</param>
		void DemoteMember(long factionId, long playerId);

		/// <summary>
		/// Forces member to leave
		/// </summary>
		/// <param name="factionId">Faction that has player</param>
		/// <param name="playerId">Player IdentityId to force leave</param>
		void MemberLeaves(long factionId, long playerId);

		/// <summary>
		/// Changes AutoAccept for faction
		/// </summary>
		/// <param name="factionId">Faction Id</param>
		/// <param name="playerId">Player IdentityId</param>
		/// <param name="autoAcceptMember">New value of faction <see cref="P:VRage.Game.ModAPI.IMyFaction.AutoAcceptMember" /></param>
		/// <param name="autoAcceptPeace">New value of faction <see cref="P:VRage.Game.ModAPI.IMyFaction.AutoAcceptPeace" /></param>
		void ChangeAutoAccept(long factionId, long playerId, bool autoAcceptMember, bool autoAcceptPeace);

		/// <summary>
		/// Edits faction
		/// </summary>
		/// <param name="factionId">FactionId that should be changed</param>
		/// <param name="tag">New faction tag</param>
		/// <param name="name">New faction name</param>
		/// <param name="desc">New faction description</param>
		/// <param name="privateInfo">New faction private info</param>
		void EditFaction(long factionId, string tag, string name, string desc, string privateInfo);

		/// <summary>
		/// Creates new faction with faction type <see ref="MyFactionTypes.None" />.
		/// </summary>
		/// <param name="founderId">IdentityId of faction founder</param>
		/// <param name="tag">Faction tag</param>
		/// <param name="name">Faction name</param>
		/// <param name="desc">Faction description</param>
		/// <param name="privateInfo">Faction private info</param>
		/// <remarks>You should use <see cref="M:VRage.Game.ModAPI.IMyFactionCollection.CreateFaction(System.Int64,System.String,System.String,System.String,System.String,VRage.Game.MyFactionTypes)" /> if you want to create a faction with different type (<see ref="MyFactionTypes.PlayerMade" />). </remarks>
		void CreateFaction(long founderId, string tag, string name, string desc, string privateInfo);

		/// <summary>
		/// Creates new faction
		/// </summary>
		/// <param name="founderId">IdentityId of faction founder</param>
		/// <param name="tag">Faction tag</param>
		/// <param name="name">Faction name</param>
		/// <param name="desc">Faction description</param>
		/// <param name="privateInfo">Faction private info</param>
		/// <param name="type">Faction type</param>
		void CreateFaction(long founderId, string tag, string name, string desc, string privateInfo, MyFactionTypes type);

		/// <summary>
		/// Creates new faction with faction type <see cref="F:VRage.Game.MyFactionTypes.None" />
		/// </summary>
		/// <param name="tag">Faction tag</param>
		/// <param name="name">Faction name</param>
		/// <param name="desc">Faction description</param>
		/// <param name="privateInfo">Faction private info</param>
		void CreateNPCFaction(string tag, string name, string desc, string privateInfo);

		/// <summary>
		/// Gets object builder
		/// </summary>
		/// <returns>Object builder</returns>
		MyObjectBuilder_FactionCollection GetObjectBuilder();

		/// <summary>
		/// Adds new NPC to faction. Name example: "SPRT NPC3340"
		/// </summary>
		/// <param name="factionId">Faction Id</param>
		void AddNewNPCToFaction(long factionId);

		/// <summary>
		/// Adds new NPC to faction
		/// </summary>
		/// <param name="factionId">Faction Id</param>
		/// <param name="npcName">Name of NPC</param>
		void AddNewNPCToFaction(long factionId, string npcName);
	}
}
