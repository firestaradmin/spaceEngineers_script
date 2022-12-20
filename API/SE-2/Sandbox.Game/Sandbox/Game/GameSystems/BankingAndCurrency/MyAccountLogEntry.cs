using System;

namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
	public struct MyAccountLogEntry
	{
<<<<<<< HEAD
		/// <summary>
		/// Identifier of entity changing the balance. (faction or player, or anything else)
		/// </summary>
		public long ChangeIdentifier { get; set; }

		/// <summary>
		/// Amount by which balance was changed.
		/// </summary>
		public long Amount { get; set; }

		/// <summary>
		/// Date and time of the transaction.
		/// </summary>
=======
		public long ChangeIdentifier { get; set; }

		public long Amount { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public DateTime DateTime { get; set; }
	}
}
