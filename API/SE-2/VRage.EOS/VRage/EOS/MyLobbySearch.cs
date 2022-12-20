using System.Collections.Generic;
using System.Text;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using VRage.GameServices;

namespace VRage.EOS
{
	internal class MyLobbySearch<TResult>
	{
		public delegate void SearchResultCallback(Result result, (string ConnectingString, TResult Item)[] servers);

		private const int MaxQueryLength = 1000;

		private readonly MyEOSNetworking m_networking;

		private readonly LobbyInterface m_lobbies;

		private readonly MakeLobbyItem<TResult> m_makeResult;

		private bool m_inProgress;

		private readonly Dictionary<string, TResult> m_resultCache = new Dictionary<string, TResult>();

		private readonly StringBuilder m_queryBuilder = new StringBuilder(1000);

		private MySessionSearchFilter m_filter;

		private string[] m_targets;

		private int m_nextTarget;

		private SearchResultCallback m_resultsCallback;

		public MyLobbySearch(MyEOSNetworking networking, MakeLobbyItem<TResult> makeResult)
		{
			m_networking = networking;
			m_makeResult = makeResult;
			m_lobbies = m_networking.Platform.GetLobbyInterface();
		}

		public void Search(string[] connectionStrings, MySessionSearchFilter filter, SearchResultCallback onResults)
		{
			if (m_inProgress)
			{
				m_networking.Log("Search already in progress.");
				return;
			}
			m_filter = filter;
			m_targets = connectionStrings;
			m_nextTarget = 0;
			m_resultsCallback = onResults;
			SearchSegment();
		}

		private void SearchSegment()
		{
			int num = 40;
			int num2 = 0;
			int num3 = 0;
			while (m_nextTarget < m_targets.Length && num2 < num && m_queryBuilder.Length + m_targets[m_nextTarget].Length + num3 <= 1000)
			{
				if (num3 > 0)
				{
					m_queryBuilder.Append(';');
				}
				num3 = 1;
				m_targets[m_nextTarget] = m_targets[m_nextTarget].Replace("eos://", string.Empty);
				m_queryBuilder.Append(m_targets[m_nextTarget]);
				m_nextTarget++;
				num2++;
			}
			string text = m_queryBuilder.ToString();
			m_queryBuilder.Clear();
			LobbySearch outLobbySearchHandle;
			Result result = m_lobbies.CreateLobbySearch(new CreateLobbySearchOptions
			{
				MaxResults = (uint)(5 * num2)
			}, out outLobbySearchHandle);
			if (result != 0)
			{
				Finish(result);
			}
			m_networking.ApplySearchFilter(outLobbySearchHandle, m_filter);
			outLobbySearchHandle.SetParameter("mincurrentmembers", ComparisonOp.Equal, 1L);
			outLobbySearchHandle.SetParameter("OWNER_EOS_ID", ComparisonOp.Anyof, text);
			outLobbySearchHandle.Find(new LobbySearchFindOptions
			{
				LocalUserId = m_networking.Users.ProductUserId
			}, outLobbySearchHandle, OnSearchResults);
		}

		private void OnSearchResults(LobbySearchFindCallbackInfo info)
		{
			LobbySearch lobbySearch = (LobbySearch)info.ClientData;
			if (info.ResultCode != 0)
			{
				Finish(info.ResultCode);
			}
			else
			{
				uint searchResultCount = lobbySearch.GetSearchResultCount(new LobbySearchGetSearchResultCountOptions());
				for (int i = 0; i < searchResultCount; i++)
				{
					if (lobbySearch.CopySearchResultByIndex(new LobbySearchCopySearchResultByIndexOptions
					{
						LobbyIndex = (uint)i
					}, out var outLobbyDetailsHandle) != 0)
					{
						outLobbyDetailsHandle.Release();
						continue;
					}
					outLobbyDetailsHandle.CopyInfo(new LobbyDetailsCopyInfoOptions(), out var outLobbyDetailsInfo);
					if (outLobbyDetailsInfo.LobbyOwnerUserId == null || !outLobbyDetailsInfo.LobbyOwnerUserId.IsValid())
					{
						outLobbyDetailsHandle.Release();
						continue;
					}
					string idString = outLobbyDetailsInfo.LobbyOwnerUserId.GetIdString();
					if (outLobbyDetailsHandle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
					{
						AttrKey = "OWNER_EOS_ID"
					}, out var outAttribute) != 0 || outAttribute.Data.Value.AsUtf8 != idString)
					{
						outLobbyDetailsHandle.Release();
					}
					else
					{
						m_resultCache[idString] = m_makeResult(outLobbyDetailsHandle, outLobbyDetailsInfo);
					}
				}
				if (m_nextTarget == m_targets.Length)
				{
					Finish(Result.Success);
				}
				else
				{
					SearchSegment();
				}
			}
			lobbySearch.Release();
		}

		private void Finish(Result result)
		{
			(string, TResult)[] array = new(string, TResult)[m_targets.Length];
			for (int i = 0; i < array.Length; i++)
			{
				m_resultCache.TryGetValue(m_targets[i], out var value);
				array[i] = (m_targets[i], value);
			}
			m_resultsCallback(result, array);
			m_inProgress = false;
			m_resultCache.Clear();
		}
	}
}
