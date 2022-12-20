using System.Collections.Generic;
using VRage.GameServices;

namespace VRage.EOS
{
	internal class MyEOSLobby : IMyLobby
	{
		public ulong LobbyId { get; }

		public bool IsValid { get; }

		public ulong OwnerId { get; }

		public MyLobbyType LobbyType { get; set; }

		public ConnectionStrategy ConnectionStrategy { get; }

		public int MemberCount { get; }

		public int MemberLimit { get; set; }

		public IEnumerable<ulong> MemberList { get; }

		public bool IsChatAvailable => false;

		public event KickedDelegate OnKicked;

		public event MyLobbyDataUpdated OnDataReceived;

		public event MyLobbyChatUpdated OnChatUpdated;

		public event MessageReceivedDelegate OnChatReceived;

		public string GetMemberName(ulong userId)
		{
			return null;
		}

		public void Leave()
		{
		}

		public bool RequestData()
		{
			return false;
		}

		public string GetData(string key)
		{
			return null;
		}

		public bool SetData(string key, string value, bool important)
		{
			return false;
		}

		public bool DeleteData(string key)
		{
			return false;
		}

		public bool SendChatMessage(string text, byte channel, long targetId, string customAuthor)
		{
			return false;
		}
	}
}
