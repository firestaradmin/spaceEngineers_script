using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.BankingAndCurrency;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof(MyObjectBuilder_BankingSystem), null, false)]
	[StaticEventOwner]
	public class MyBankingSystem : MySessionComponentBase
	{
		public delegate void AccountBalanceChanged(MyAccountInfo oldAccountInfo, MyAccountInfo newAccountInfo);

		protected sealed class CreateAccount_Clients_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long ownerIdentifier, in long startingBalance, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateAccount_Clients(ownerIdentifier, startingBalance);
			}
		}

		protected sealed class RemoveAccount_Clients_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long ownerIdentifier, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveAccount_Clients(ownerIdentifier);
			}
		}

		protected sealed class UnlockAchievementForClient_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string achievement, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UnlockAchievementForClient(achievement);
			}
		}

		protected sealed class ChangeBalanceBroadcastToClients_003C_003ESystem_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identifierId, in long amount, in long finalToBalance, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ChangeBalanceBroadcastToClients(identifierId, amount, finalToBalance);
			}
		}

		protected sealed class RequestTransfer_Server_003C_003ESystem_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long fromIdentifier, in long toIdentifier, in long amount, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestTransfer_Server(fromIdentifier, toIdentifier, amount);
			}
		}

		protected sealed class RequestTransfer_BroadcastToClients_003C_003ESystem_Int64_0023System_Int64_0023System_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, long, long, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long fromIdentifier, in long toIdentifier, in long amount, in long finalFromBalance, in long finalToBalance, in DBNull arg6)
			{
				RequestTransfer_BroadcastToClients(fromIdentifier, toIdentifier, amount, finalFromBalance, finalToBalance);
			}
		}

		private const long ACHIEVEMENT_CURRENCY_THRESHOLD_MILIONAIRE = 1000000L;

		private const string ACHIEVEMENT_KEY_MILIONAIRE = "MillionaireClub";

		/// <summary>
		/// Dictionary of owners to their accounts.
		/// </summary>
		private Dictionary<long, MyAccount> m_accounts = new Dictionary<long, MyAccount>();

		public static MyBankingSystem Static { get; private set; }

		public static MyBankingSystemDefinition BankingSystemDefinition { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// Overall amount of currency in the world.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public long OverallBalance { get; private set; }

		public event AccountBalanceChanged OnAccountBalanceChanged;

		public MyBankingSystem()
		{
			Static = this;
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			BankingSystemDefinition = definition as MyBankingSystemDefinition;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_BankingSystem myObjectBuilder_BankingSystem = sessionComponent as MyObjectBuilder_BankingSystem;
			if (myObjectBuilder_BankingSystem.Accounts == null)
			{
				return;
			}
			foreach (MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry account in myObjectBuilder_BankingSystem.Accounts)
			{
				MyAccount myAccount = new MyAccount();
				myAccount.Init(account.Account);
				m_accounts.Add(account.OwnerIdentifier, myAccount);
			}
			OverallBalance = myObjectBuilder_BankingSystem.OverallBalance;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_BankingSystem myObjectBuilder_BankingSystem = base.GetObjectBuilder() as MyObjectBuilder_BankingSystem;
			if (m_accounts.Count > 0)
			{
				myObjectBuilder_BankingSystem.Accounts = new List<MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry>(m_accounts.Count);
				foreach (KeyValuePair<long, MyAccount> account in m_accounts)
				{
					MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry item = default(MyObjectBuilder_BankingSystem.MyObjectBuilder_AccountEntry);
					item.OwnerIdentifier = account.Key;
					item.Account = account.Value.GetObjectBuilder();
					myObjectBuilder_BankingSystem.Accounts.Add(item);
				}
				myObjectBuilder_BankingSystem.OverallBalance = OverallBalance;
			}
			return myObjectBuilder_BankingSystem;
		}

		/// <summary>
		/// Creates an account associated with owner id with default starting balance. You can provide any id, but ids cannot collide (Only one account per id).
		/// Server side only method.
		/// </summary>
		/// <param name="ownerIdentifier">Identifier to associate the account with.</param>
		public void CreateAccount(long ownerIdentifier)
		{
			CreateAccount(ownerIdentifier, BankingSystemDefinition.StartingBalance);
		}

		/// <summary>
		/// Creates an account associated with owner id. You can provide any id, but ids cannot collide (Only one account per id).
		/// Server side only method.
		/// </summary>
		/// <param name="ownerIdentifier">Identifier to associate the account with.</param>
		/// <param name="startingBalance">Starting balance to be set on the account.</param>
		public void CreateAccount(long ownerIdentifier, long startingBalance)
		{
			if (Sync.IsServer && !m_accounts.ContainsKey(ownerIdentifier))
			{
				MyAccount value = new MyAccount(ownerIdentifier, startingBalance);
				m_accounts.Add(ownerIdentifier, value);
				OverallBalance += startingBalance;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateAccount_Clients, ownerIdentifier, startingBalance);
			}
		}

		[Event(null, 133)]
		[Reliable]
		[Broadcast]
		public static void CreateAccount_Clients(long ownerIdentifier, long startingBalance)
		{
			if (!Static.m_accounts.ContainsKey(ownerIdentifier))
			{
				MyAccount value = new MyAccount(ownerIdentifier, startingBalance);
				Static.m_accounts.Add(ownerIdentifier, value);
			}
		}

		/// <summary>
		/// Removes account associated with owner id.
		/// </summary>
		/// <param name="ownerIdentifier">Identifier to remove account for.</param>
		/// <returns>True if account was removed, otherwise false.</returns>
		public bool RemoveAccount(long ownerIdentifier)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			bool num = m_accounts.ContainsKey(ownerIdentifier) && m_accounts.Remove(ownerIdentifier);
			if (num)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveAccount_Clients, ownerIdentifier);
			}
			return num;
		}

		[Event(null, 169)]
		[Reliable]
		[Broadcast]
		public static void RemoveAccount_Clients(long ownerIdentifier)
		{
			Static.m_accounts.Remove(ownerIdentifier);
		}

		public static long GetBalance(long identifierId)
		{
			if (!Sync.IsServer)
			{
				return -1L;
			}
			if (!Static.TryGetAccountInfo(identifierId, out var account))
			{
				return -1L;
			}
			return account.Balance;
		}

<<<<<<< HEAD
		/// <summary>
		///  Server side method to increase/decrease funds on player's account
		/// </summary>
		/// <param name="identifierId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool ChangeBalance(long identifierId, long amount)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (!Static.ChangeBalanceInternal(identifierId, amount, out var ToAccount))
			{
				return false;
			}
			MyLog.Default.WriteLine($"Balance change of {amount} to account owner {ToAccount.OwnerIdentifier} with new balance of {ToAccount.Balance}");
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ChangeBalanceBroadcastToClients, identifierId, amount, ToAccount.Balance);
			return true;
		}

		private bool ChangeBalanceInternal(long identifierId, long amount, out MyAccount ToAccount)
		{
			ToAccount = null;
			if (!m_accounts.TryGetValue(identifierId, out var value))
			{
				MyLog.Default.Error($"Target Identifier {identifierId} does not contain account.");
				return false;
			}
			if (amount < 0 && value + amount < 0)
			{
				MyLog.Default.Error($"Identifier {identifierId} does contain enough currency to do the subtraction.");
				return false;
			}
			MyAccountInfo accountInfo = value.GetAccountInfo();
			if (accountInfo.Balance + amount < 0)
			{
				amount = 0L;
			}
			if (amount >= 0)
			{
				value.Add(amount);
				if (MySession.Static.IsServer && MySession.Static.Players.TryGetIdentity(value.OwnerIdentifier) != null)
				{
					CheckBalanceIncreaseAchievements(value, amount);
				}
			}
			else
			{
				value.Subtract(-amount);
			}
			ToAccount = value;
			OverallBalance += amount;
			this.OnAccountBalanceChanged?.Invoke(accountInfo, value.GetAccountInfo());
			return true;
		}

		private void CheckBalanceIncreaseAchievements(MyAccount account, long change)
		{
			if (account.Balance < 1000000 || account.Balance - change >= 1000000)
			{
				return;
			}
			ulong num = MySession.Static.Players.TryGetSteamId(account.OwnerIdentifier);
			if (num == 0L)
			{
				return;
			}
			if (MySession.Static.LocalPlayerId == account.OwnerIdentifier)
			{
				UnlockAchievement_Internal("MillionaireClub");
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UnlockAchievementForClient, "MillionaireClub", new EndpointId(num));
		}

		[Event(null, 268)]
		[Reliable]
		[Client]
		private static void UnlockAchievementForClient(string achievement)
		{
			UnlockAchievement_Internal(achievement);
		}

		private static void UnlockAchievement_Internal(string achievement)
		{
			MyGameService.GetAchievement(achievement, null, 0f).Unlock();
		}

		[Event(null, 279)]
		[Reliable]
		[Broadcast]
		public static void ChangeBalanceBroadcastToClients(long identifierId, long amount, long finalToBalance)
		{
			Static.ChangeBalanceInternal(identifierId, amount, out var ToAccount);
			if (finalToBalance != ToAccount.Balance)
			{
				MyLog.Default.Error("Server and client data do not match. Reseting client data");
				ToAccount.ResetBalance(finalToBalance);
			}
		}

		/// <summary>
		/// Request transfer of currency between two identifiers.
		/// </summary>
		/// <param name="fromIdentifier">Identifier of an account to make transfer from.</param>
		/// <param name="toIdentifier">Identifier of an account to make transfer to.</param>
		/// <param name="amount">Amount to transfer.</param>
		public static void RequestTransfer(long fromIdentifier, long toIdentifier, long amount)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestTransfer_Server, fromIdentifier, toIdentifier, amount);
		}

		[Event(null, 302)]
		[Reliable]
		[Server]
		private static void RequestTransfer_Server(long fromIdentifier, long toIdentifier, long amount)
		{
			if (Static.Transfer_Internal(fromIdentifier, toIdentifier, amount, validate: true, out var fromAccount, out var ToAccount))
			{
				MyLog.Default.WriteLine($"Transfer of {amount} from {fromIdentifier} with new balance of {fromAccount.Balance} to {toIdentifier} with new balance of {ToAccount.Balance}");
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestTransfer_BroadcastToClients, fromIdentifier, toIdentifier, amount, fromAccount.Balance, ToAccount.Balance);
			}
		}

		/// <summary>
		/// Transfer amount of currency from one identifier to another. This method can be only called on server. Validation is not done.
		/// </summary>
		/// <param name="fromIdentifier">Identifier of an account to make transfer from.</param>
		/// <param name="toIdentifier">Identifier of an account to make transfer to.</param>
		/// <param name="amount">Amount to transfer.</param>
		internal void Transfer_Server(long fromIdentifier, long toIdentifier, long amount)
		{
			if (Sync.IsServer && Static.Transfer_Internal(fromIdentifier, toIdentifier, amount, validate: false, out var fromAccount, out var ToAccount))
			{
				MyLog.Default.WriteLine($"Transfer of {amount} from {fromIdentifier} with new balance of {fromAccount.Balance} to {toIdentifier} with new balance of {ToAccount.Balance}");
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestTransfer_BroadcastToClients, fromIdentifier, toIdentifier, amount, fromAccount.Balance, ToAccount.Balance);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Broadcast to clients to do the update.
		/// </summary>
		/// <param name="fromIdentifier">Identifier of an account to make transfer from.</param>
		/// <param name="toIdentifier">Identifier of an account to make transfer to.</param>
		/// <param name="amount">Amount to transfer.</param>
		/// <param name="finalFromBalance">Final balance on server (used for validation).</param>
		/// <param name="finalToBalance">Final balance on server (used for validation).</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Event(null, 350)]
		[Reliable]
		[Broadcast]
		public static void RequestTransfer_BroadcastToClients(long fromIdentifier, long toIdentifier, long amount, long finalFromBalance, long finalToBalance)
		{
			Static.Transfer_Internal(fromIdentifier, toIdentifier, amount, validate: false, out var fromAccount, out var ToAccount);
			if (finalFromBalance != fromAccount.Balance || finalToBalance != ToAccount.Balance)
			{
				MyLog.Default.Error("Server and client data do not match. Reseting client data");
				fromAccount.ResetBalance(finalFromBalance);
				ToAccount.ResetBalance(finalToBalance);
			}
		}

		private bool Transfer_Internal(long fromIdentifier, long toIdentifier, long amount, bool validate, out MyAccount fromAccount, out MyAccount ToAccount)
		{
			fromAccount = (ToAccount = null);
			if (validate)
			{
				bool flag = Sync.Players.TryGetIdentity(fromIdentifier) != null;
				if (flag && !CheckIsOnline(fromIdentifier))
				{
					return false;
				}
				bool flag2 = Sync.Players.TryGetIdentity(toIdentifier) != null;
				if (flag2 && !CheckIsOnline(toIdentifier))
				{
					return false;
				}
				if (flag2 && !flag)
				{
					if (!IsFactionValid(toIdentifier, fromIdentifier))
					{
						return false;
					}
				}
				else if (flag)
				{
					Sync.Players.TryGetPlayerId(fromIdentifier, out var result);
					if (result.SteamId != MyEventContext.Current.Sender.Value)
					{
						MyLog.Default.Error("Transfer from player that is not the sender of the message is not allowed!");
						return false;
					}
				}
			}
			if (!Static.m_accounts.TryGetValue(fromIdentifier, out var value))
			{
				MyLog.Default.Error($"Source Identifier {fromIdentifier} does not contain account.");
				return false;
			}
			if (!Static.m_accounts.TryGetValue(toIdentifier, out var value2))
			{
				MyLog.Default.Error($"Target Identifier {toIdentifier} does not contain account.");
				return false;
			}
			if (value - amount < 0)
			{
				MyLog.Default.Error($"Identifier {fromIdentifier} does contain enough currency to do the transfer.");
				return false;
			}
			MyAccountInfo accountInfo = value.GetAccountInfo();
			MyAccountInfo accountInfo2 = value2.GetAccountInfo();
			value.Subtract(amount);
			MyAccountLogEntry myAccountLogEntry = default(MyAccountLogEntry);
			myAccountLogEntry.ChangeIdentifier = toIdentifier;
			myAccountLogEntry.Amount = -amount;
			myAccountLogEntry.DateTime = DateTime.Now;
			MyAccountLogEntry item = myAccountLogEntry;
			if (value.Log.Count > BankingSystemDefinition.AccountLogLen)
			{
				value.Log.RemoveAt(0);
			}
			value.Log.Add(item);
			value2.Add(amount);
			myAccountLogEntry = default(MyAccountLogEntry);
			myAccountLogEntry.ChangeIdentifier = fromIdentifier;
			myAccountLogEntry.Amount = amount;
			myAccountLogEntry.DateTime = DateTime.Now;
			MyAccountLogEntry item2 = myAccountLogEntry;
			if (value2.Log.Count > BankingSystemDefinition.AccountLogLen)
			{
				value2.Log.RemoveAt(0);
			}
			value2.Log.Add(item2);
			if (MySession.Static.IsServer && MySession.Static.Players.TryGetIdentity(value2.OwnerIdentifier) != null)
			{
				CheckBalanceIncreaseAchievements(value2, amount);
			}
			fromAccount = value;
			ToAccount = value2;
			this.OnAccountBalanceChanged?.Invoke(accountInfo, value.GetAccountInfo());
			this.OnAccountBalanceChanged?.Invoke(accountInfo2, value2.GetAccountInfo());
			return true;
		}

		internal void GetPerPlayerBalances(ref Dictionary<long, long> result)
		{
			if (result == null)
			{
				result = new Dictionary<long, long>();
			}
			if (MySession.Static.Players == null)
			{
				return;
			}
			result.Clear();
			foreach (MyIdentity allIdentity in MySession.Static.Players.GetAllIdentities())
			{
				if (TryGetAccountInfo(allIdentity.IdentityId, out var account))
				{
					result.Add(allIdentity.IdentityId, account.Balance);
				}
			}
		}

		internal void GetPerFactionBalances(ref Dictionary<long, long> result)
		{
			if (result == null)
			{
				result = new Dictionary<long, long>();
			}
			if (MySession.Static.Factions == null)
			{
				return;
			}
			result.Clear();
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				if (TryGetAccountInfo(faction.Value.FactionId, out var account))
				{
					result.Add(faction.Value.FactionId, account.Balance);
				}
			}
		}

		/// <summary>
		/// Validates if given player has rights to perform transfer of money from faction to him.
		/// </summary>
		/// <param name="playerIdentity">Player to validate for.</param>
		/// <param name="factionId">Faction to validate for.</param>
		/// <returns></returns>
		private bool IsFactionValid(long playerIdentity, long factionId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction == null)
			{
				MyLog.Default.Error($"Faction {factionId} does not exist. Transfer impossible.");
				return false;
			}
			if (myFaction.IsFounder(playerIdentity) || myFaction.IsLeader(playerIdentity))
			{
				return true;
			}
			MyLog.Default.Error($"Player of identity {playerIdentity} does not have rights to transfer from Faction {myFaction.Name}. Transfer impossible.");
			return false;
		}

		private bool CheckIsOnline(long identifier)
		{
			bool flag = false;
			if (Sync.Players.TryGetPlayerId(identifier, out var result))
			{
				flag = Sync.Players.IsPlayerOnline(ref result);
			}
			if (!flag)
			{
				MyLog.Default.Error($"Identity {identifier} does not have online player. Transfer not possible");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Gets an account for given owner id.
		/// </summary>
		/// <param name="ownerIdentifier">Identifier of the owner of the account to get.</param>
		/// <param name="account">Account associated with the identifier.</param>
		/// <returns>True if account was found, otherwise false.</returns>
		public bool TryGetAccountInfo(long ownerIdentifier, out MyAccountInfo account)
		{
			if (!m_accounts.TryGetValue(ownerIdentifier, out var value))
			{
				account = default(MyAccountInfo);
				return false;
			}
			account = value.GetAccountInfo();
			return true;
		}

		/// <summary>
		/// Gets short version of account balance for provided id in form of 'Balance CurrencyShortName'.
		/// </summary>
		/// <param name="ownerIdentidier">Owner identifier to get balance string for.</param>
		/// <param name="addCurrencyShortName">If true, short name of the currency will be added in format of 'Value CurrencyShortName'.</param>
		/// <returns></returns>
		public string GetBalanceShortString(long ownerIdentidier, bool addCurrencyShortName = true)
		{
			_ = string.Empty;
			if (!m_accounts.TryGetValue(ownerIdentidier, out var value))
			{
				return "Error, Account Not Found";
			}
			return GetFormatedValue(value.Balance, addCurrencyShortName);
		}

		/// <summary>
		/// Gets formated currency with possibility to add currency short name to it.
		/// </summary>
		/// <param name="valueToFormat">Value to format.</param>
		/// <param name="addCurrencyShortName">If true, short name of the currency will be added in format of 'Value CurrencyShortName'.</param>
		/// <returns></returns>
		public static string GetFormatedValue(long valueToFormat, bool addCurrencyShortName = false)
		{
			if (valueToFormat > 1000000000000L || valueToFormat < -1000000000000L)
			{
				valueToFormat /= 1000000000000L;
				if (addCurrencyShortName)
				{
					return valueToFormat.ToString("N0") + " T " + BankingSystemDefinition.CurrencyShortName.ToString();
				}
				return valueToFormat.ToString("N0") + " T";
			}
			if (addCurrencyShortName)
			{
				return valueToFormat.ToString("N0") + " " + BankingSystemDefinition.CurrencyShortName.ToString();
			}
			return valueToFormat.ToString("N0");
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}
	}
}
