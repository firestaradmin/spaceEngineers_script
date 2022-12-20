namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
	public struct MyAccountInfo
	{
		/// <summary>
		/// Identifier of the owner (ex. Player IdentityId, FactionId)
		/// </summary>
		public long OwnerIdentifier;

		/// <summary>
		/// Balance of the account at the time of query.
		/// </summary>
		public long Balance;

		/// <summary>
		/// Log of all account transfers.
		/// </summary>
		public MyAccountLogEntry[] Log;
	}
}
