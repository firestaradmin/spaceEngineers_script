<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Multiplayer;
using VRage.Game.ModAPI;

namespace Sandbox.Game.World
{
	/// <summary>
	/// This class identifies the steam client (basically a computer) on the network.
	/// </summary>
	public class MyNetworkClient : IMyNetworkClient
	{
		private readonly ulong m_steamUserId;

		/// <summary>
		/// When player sends input, ClientTime is set on server
		/// Later when server sends position updates, it includes client time
		/// It's used for input prediction interpolation on client
		/// </summary>
		public ushort ClientFrameId;

		private int m_controlledPlayerSerialId;

		public ulong SteamUserId => m_steamUserId;

		public bool IsLocal { get; private set; }

		public string DisplayName { get; private set; }

		public int ControlledPlayerSerialId
		{
			private get
			{
				return m_controlledPlayerSerialId;
			}
			set
			{
				if (ControlledPlayerSerialId != value)
				{
					FirstPlayer.ReleaseControls();
					m_controlledPlayerSerialId = value;
					FirstPlayer.AcquireControls();
				}
			}
		}

		public MyPlayer FirstPlayer => GetPlayer(ControlledPlayerSerialId);

<<<<<<< HEAD
=======
		public event Action ClientLeft;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyNetworkClient(ulong steamId, string senderName)
		{
			m_steamUserId = steamId;
			IsLocal = Sync.MyId == steamId;
			DisplayName = senderName;
		}

		public MyPlayer GetPlayer(int serialId)
		{
			MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
			playerId.SteamId = m_steamUserId;
			playerId.SerialId = serialId;
			MyPlayer.PlayerId id = playerId;
			return Sync.Players.GetPlayerById(id);
		}

		public MyPlayer GetPlayer(ulong steamId, int serialId)
		{
			MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
			playerId.SteamId = steamId;
			playerId.SerialId = serialId;
			MyPlayer.PlayerId id = playerId;
			return Sync.Players.GetPlayerById(id);
		}
	}
}
