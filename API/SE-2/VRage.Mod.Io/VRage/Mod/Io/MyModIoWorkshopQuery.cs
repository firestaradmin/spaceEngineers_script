using System.Collections.Generic;
using VRage.GameServices;
using VRage.Mod.Io.Data;

namespace VRage.Mod.Io
{
	internal class MyModIoWorkshopQuery : MyWorkshopQuery
	{
		private MyModIoServiceInternal m_service;

		private bool m_queryActive;

		private int m_resultsReturned;

		private uint m_page = 1u;

		private bool m_cancelQuery;

		private List<ulong> m_tmpList;

		private HashSet<int> m_tmpSubscriptions;

		public override bool IsRunning => m_queryActive;

		public MyModIoWorkshopQuery(MyModIoServiceInternal service)
		{
			m_service = service;
			ItemsPerPage = 50u;
		}

		public override void Run()
		{
			Run(1u);
		}

		public override void Run(uint startingPage)
		{
			if (!m_queryActive)
			{
				QueryCleanup();
				m_cancelQuery = false;
				m_page = startingPage;
				base.TotalResults = 0u;
				if (base.Items == null)
				{
					base.Items = new List<MyWorkshopItem>();
				}
				base.Items.Clear();
				RunQuery();
			}
		}

		public override void Stop()
		{
			base.Stop();
			m_cancelQuery = true;
		}

		public override void Dispose()
		{
			base.Dispose();
			QueryCleanup();
			m_service = null;
			base.ItemIds?.Clear();
			base.RequiredTags?.Clear();
			base.ExcludedTags?.Clear();
			base.Items = null;
		}

		private void QueryCleanup()
		{
			m_resultsReturned = 0;
<<<<<<< HEAD
=======
			m_queryActive = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_page = 1u;
		}

		private void RunQuery()
		{
			m_queryActive = true;
			if ((base.ItemIds != null && base.ItemIds.Count > 0) || base.UserId == 0L)
			{
				MyModIo.GetMods(Response, GetSortOrdering(), base.SearchString, base.ItemIds, base.RequiredTags, base.ExcludedTags, m_page - 1, ItemsPerPage);
				return;
			}
			MyModIo.GetMySubscriptions(delegate(RequestPage<ModProfile> x, MyGameServiceCallResult y)
			{
				ResponseWithSubscriptions(x, y, null);
			}, GetSortOrdering(), base.SearchString, base.ItemIds, base.RequiredTags, base.ExcludedTags, m_page - 1, ItemsPerPage);
		}

		private MyModIo.Sort GetSortOrdering()
		{
<<<<<<< HEAD
			switch (base.QueryType)
			{
			case MyWorkshopQueryType.SortByVotes:
				return MyModIo.Sort.Rating;
			case MyWorkshopQueryType.SortByPublicationDate:
				return MyModIo.Sort.DateUpdated;
			case MyWorkshopQueryType.SortByTotalUniqueSubscriptions:
				return MyModIo.Sort.Subscribers;
			default:
				return MyModIo.Sort.Name;
			}
=======
			return base.QueryType switch
			{
				MyWorkshopQueryType.SortByVotes => MyModIo.Sort.Rating, 
				MyWorkshopQueryType.SortByPublicationDate => MyModIo.Sort.DateUpdated, 
				MyWorkshopQueryType.SortByTotalUniqueSubscriptions => MyModIo.Sort.Subscribers, 
				_ => MyModIo.Sort.Name, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void Response(RequestPage<ModProfile> mods, MyGameServiceCallResult result)
		{
			if (!m_queryActive || result != MyGameServiceCallResult.OK)
			{
				OnQueryCompleted((!m_queryActive) ? MyGameServiceCallResult.Cancelled : result);
				QueryCleanup();
				return;
			}
			int num = mods.data.Length;
			if (num > 0 && MyModIo.ServiceUserId != 0L)
			{
				if (m_tmpList == null)
				{
					m_tmpList = new List<ulong>();
				}
				else
				{
					m_tmpList.Clear();
				}
				for (uint num2 = 0u; num2 < num; num2++)
				{
					m_tmpList.Add((ulong)mods.data[num2].id);
				}
				MyModIo.GetMySubscriptions(delegate(RequestPage<ModProfile> x, MyGameServiceCallResult y)
				{
					ProcessSubscriptions(mods, x, y);
				}, MyModIo.Sort.Name, null, m_tmpList, null, null, 0u, (uint)num);
			}
			else
			{
				ResponseWithSubscriptions(mods, result, null);
			}
		}

		private void ProcessSubscriptions(RequestPage<ModProfile> mods, RequestPage<ModProfile> subscribedMods, MyGameServiceCallResult result)
		{
			if (m_tmpSubscriptions == null)
			{
				m_tmpSubscriptions = new HashSet<int>();
			}
			else
			{
				m_tmpSubscriptions.Clear();
			}
			if (m_queryActive && result == MyGameServiceCallResult.OK)
			{
				ModProfile[] data = subscribedMods.data;
				foreach (ModProfile modProfile in data)
				{
					m_tmpSubscriptions.Add(modProfile.id);
				}
			}
			ResponseWithSubscriptions(mods, result, m_tmpSubscriptions);
		}

		private void ResponseWithSubscriptions(RequestPage<ModProfile> mods, MyGameServiceCallResult result, HashSet<int> subscriptions)
		{
<<<<<<< HEAD
			if (!m_queryActive || mods == null || result != MyGameServiceCallResult.OK || base.Items == null)
=======
			if (!m_queryActive || mods == null || result != MyGameServiceCallResult.OK)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				OnQueryCompleted((!m_queryActive) ? MyGameServiceCallResult.Cancelled : result);
				QueryCleanup();
				return;
			}
			m_queryActive = false;
			base.TotalResults = (uint)mods.result_total;
			int num = mods.data.Length;
			m_resultsReturned += num;
			for (uint num2 = 0u; num2 < num; num2++)
			{
				int id = mods.data[num2].id;
				MyModIoWorkshopItem item = new MyModIoWorkshopItem(m_service, mods.data[num2], subscriptions?.Contains(id) ?? true);
				base.Items.Add(item);
			}
<<<<<<< HEAD
			OnPageQueryCompleted(result, m_page);
			if (m_resultsReturned == base.TotalResults || m_cancelQuery)
=======
			if (m_resultsReturned == base.TotalResults)
			{
				OnPageQueryCompleted(result, m_page);
				OnQueryCompleted(MyGameServiceCallResult.OK);
				QueryCleanup();
				return;
			}
			OnPageQueryCompleted(result, m_page);
			m_page++;
			if (m_cancelQuery)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				OnQueryCompleted(MyGameServiceCallResult.OK);
				QueryCleanup();
			}
			else
			{
<<<<<<< HEAD
				m_page++;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				RunQuery();
			}
		}
	}
}
