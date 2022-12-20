using System;
using System.Collections.Generic;
using Sandbox.Game.Gui;

namespace Sandbox.Game.Entities.Character
{
	public class MyUnifiedChatHistory
	{
		protected Queue<MyUnifiedChatItem> m_chat = new Queue<MyUnifiedChatItem>();

		public void EnqueueMessage(string text, ChatChannel channel, long senderId, long targetId = 0L, DateTime? timestamp = null, string authorFont = "Blue")
		{
			DateTime timestamp2 = ((!timestamp.HasValue || !timestamp.HasValue) ? DateTime.UtcNow : timestamp.Value);
			MyUnifiedChatItem item;
			switch (channel)
			{
			case ChatChannel.Global:
			case ChatChannel.GlobalScripted:
				item = MyUnifiedChatItem.CreateGlobalMessage(text, timestamp2, senderId, authorFont);
				break;
			case ChatChannel.Faction:
				item = MyUnifiedChatItem.CreateFactionMessage(text, timestamp2, senderId, targetId, authorFont);
				break;
			case ChatChannel.Private:
				item = MyUnifiedChatItem.CreatePrivateMessage(text, timestamp2, senderId, targetId, authorFont);
				break;
			case ChatChannel.ChatBot:
				item = MyUnifiedChatItem.CreateChatbotMessage(text, timestamp2, senderId, targetId, null, authorFont);
				break;
			default:
				item = null;
				break;
			}
			if (item != null)
			{
				EnqueueMessage(ref item);
			}
		}

		public void EnqueueMessageScripted(string text, string customAuthor, string authorFont = "Blue")
		{
			MyUnifiedChatItem item = MyUnifiedChatItem.CreateScriptedMessage(text, DateTime.UtcNow, customAuthor, authorFont);
			EnqueueMessage(ref item);
		}

		public void EnqueueMessage(ref MyUnifiedChatItem item)
		{
			m_chat.Enqueue(item);
		}

		public void GetCompleteHistory(ref List<MyUnifiedChatItem> list)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyUnifiedChatItem> enumerator = m_chat.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyUnifiedChatItem current = enumerator.get_Current();
					list.Add(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void GetGeneralHistory(ref List<MyUnifiedChatItem> list)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyUnifiedChatItem> enumerator = m_chat.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyUnifiedChatItem current = enumerator.get_Current();
					if (current.Channel == ChatChannel.Global || current.Channel == ChatChannel.GlobalScripted)
					{
						list.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void GetFactionHistory(ref List<MyUnifiedChatItem> list, long factionId)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyUnifiedChatItem> enumerator = m_chat.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyUnifiedChatItem current = enumerator.get_Current();
					if (current.Channel == ChatChannel.Faction && current.TargetId == factionId)
					{
						list.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void GetPrivateHistory(ref List<MyUnifiedChatItem> list, long playerId)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyUnifiedChatItem> enumerator = m_chat.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyUnifiedChatItem current = enumerator.get_Current();
					if (current.Channel == ChatChannel.Private && (current.TargetId == playerId || current.SenderId == playerId))
					{
						list.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void GetChatbotHistory(ref List<MyUnifiedChatItem> list)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyUnifiedChatItem> enumerator = m_chat.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyUnifiedChatItem current = enumerator.get_Current();
					if (current.Channel == ChatChannel.ChatBot)
					{
						list.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void ClearNonGlobalHistory()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Queue<MyUnifiedChatItem> val = new Queue<MyUnifiedChatItem>();
			Enumerator<MyUnifiedChatItem> enumerator = m_chat.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyUnifiedChatItem current = enumerator.get_Current();
					switch (current.Channel)
					{
					case ChatChannel.Global:
					case ChatChannel.GlobalScripted:
					case ChatChannel.ChatBot:
						val.Enqueue(current);
						break;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_chat = val;
		}
	}
}
