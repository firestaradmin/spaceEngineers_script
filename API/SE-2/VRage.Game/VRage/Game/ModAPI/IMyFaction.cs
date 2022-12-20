using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes faction (mods interface)
	/// </summary>
	public interface IMyFaction
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets faction id
		/// </summary>
		long FactionId { get; }

		/// <summary>
		/// Gets faction tag
		/// </summary>
		string Tag { get; }

		/// <summary>
		/// Gets faction name
		/// </summary>
		string Name { get; }
=======
		long FactionId { get; }

		string Tag { get; }

		string Name { get; }

		string Description { get; }

		string PrivateInfo { get; }

		int Score { get; set; }

		float ObjectivePercentageCompleted { get; set; }

		MyStringId? FactionIcon { get; }

		bool AutoAcceptMember { get; }

		bool AutoAcceptPeace { get; }

		bool AcceptHumans { get; }

		long FounderId { get; }

		Vector3 CustomColor { get; }

		Vector3 IconColor { get; }

		DictionaryReader<long, MyFactionMember> Members { get; }

		DictionaryReader<long, MyFactionMember> JoinRequests { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Gets faction description
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Gets faction private info
		/// </summary>
		string PrivateInfo { get; }

		/// <summary>
		/// Gets or sets score of faction. Used in Uranium Heist scenario
		/// </summary>
		int Score { get; set; }

		/// <summary>
		/// Gets or sets percentage of completed objective. Used in Uranium Heist scenario
		/// </summary>
		float ObjectivePercentageCompleted { get; set; }

		/// <summary>
		/// Gets faction icon
		/// </summary>
		MyStringId? FactionIcon { get; }

		/// <summary>
		/// Gets if faction automatically accept new members
		/// </summary>
		bool AutoAcceptMember { get; }

		/// <summary>
		/// Gets if faction automatically accept peace from other factions
		/// </summary>
		bool AutoAcceptPeace { get; }

		/// <summary>
		/// Gets if faction accepts players
		/// </summary>
		bool AcceptHumans { get; }

		/// <summary>
		/// IdentityId of founder of faction
		/// </summary>
		long FounderId { get; }

		/// <summary>
		/// Gets faction icon background color
		/// </summary>
		Vector3 CustomColor { get; }

		/// <summary>
		/// Gets faction icon color
		/// </summary>
		Vector3 IconColor { get; }

		/// <summary>
		/// Gets all members (founder, leaders also) of faction. 
		/// </summary>
		DictionaryReader<long, MyFactionMember> Members { get; }

		/// <summary>
		/// Gets all faction join requests. 
		/// </summary>
		DictionaryReader<long, MyFactionMember> JoinRequests { get; }

		/// <summary>
		/// Returns if player with provided playerId is founder of faction 
		/// </summary>
		/// <param name="playerId">Player IdentityId</param>
		/// <returns>True if player is founder of faction</returns>
		bool IsFounder(long playerId);

		/// <summary>
		/// Returns if player with provided playerId is faction leader
		/// </summary>
		/// <param name="playerId">Player IdentityId</param>
		/// <returns>True if player is faction leader</returns>
		bool IsLeader(long playerId);

		/// <summary>
		/// Returns if player with provided playerId is faction member
		/// </summary>
		/// <param name="playerId">Player IdentityId</param>
		/// <returns>True if player is faction leader</returns>
		bool IsMember(long playerId);

		/// <summary>
		/// Returns if player with provided playerId is neutral to faction
		/// </summary>
		/// <param name="playerId">Player IdentityId</param>
		/// <returns>True if player is neutral to faction</returns>
		bool IsNeutral(long playerId);

		/// <summary>
		/// Returns if player with provided playerId is enemy to faction
		/// </summary>
		/// <param name="playerId">Player IdentityId</param>
		/// <returns>True if player is enemy to faction</returns>
		bool IsEnemy(long playerId);

		/// <summary>
		/// Returns if player with provided playerId is friendly to faction
		/// </summary>
		/// <param name="playerId">Player IdentityId</param>
		/// <returns>True if player is friendly to faction</returns>
		bool IsFriendly(long playerId);

		/// <summary>
		/// Returns if faction has no humans
		/// </summary>
		/// <returns>True if faction has no humans</returns>
		bool IsEveryoneNpc();

		/// <summary>
		/// Gets balance of an account associated with faction.
		/// </summary>
		/// <param name="balance">Returns current balance of the account. (If called on client, can return delayed value, as changes to balace have to be synchronized first)</param>
		/// <returns>True if account was found. Otherwise false.</returns>
		bool TryGetBalanceInfo(out long balance);

		/// <summary>
		/// Gets balance of an account associated with faction. Format is 'BALANCE CURRENCYSHORTNAME'.
		/// </summary>
		/// <returns>Current balance of the account in form of formatted string. If Banking System does not exist method returns null.</returns>
		string GetBalanceShortString();

		/// <summary>
		/// Changes the balance of the account of this faction by given amount. Sends a message to server with the request.
		/// </summary>
		/// <param name="amount">Amount by which to change te balance.</param>
		void RequestChangeBalance(long amount);
	}
}
