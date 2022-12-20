using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MyWorkshopQueryAggregated : MyWorkshopQuery
	{
		private readonly List<MyWorkshopQuery> m_queries = new List<MyWorkshopQuery>();

		private int m_queryCounter;

		public override uint ItemsPerPage
		{
			get
			{
				uint num = 0u;
				foreach (MyWorkshopQuery query in m_queries)
				{
					num = Math.Max(num, query.ItemsPerPage);
				}
				return num;
			}
			set
			{
				foreach (MyWorkshopQuery query in m_queries)
				{
					query.ItemsPerPage = value;
				}
			}
		}

		public override bool IsRunning
		{
			get
			{
				foreach (MyWorkshopQuery query in m_queries)
				{
					if (query.IsRunning)
					{
						return true;
					}
				}
				return false;
			}
		}

		public MyWorkshopQueryAggregated(MyUGCAggregator aggregator)
		{
			foreach (IMyUGCService aggregate in aggregator.GetAggregates())
			{
				MyWorkshopQuery query = aggregate.CreateWorkshopQuery();
				query.QueryCompleted += delegate(MyGameServiceCallResult x)
				{
					AggregateQueryCompleted(query, x);
				};
				m_queries.Add(query);
			}
		}

		private void AggregateQueryCompleted(MyWorkshopQuery query, MyGameServiceCallResult result)
		{
			if (base.Items == null)
			{
				base.Items = new List<MyWorkshopItem>();
			}
			base.Items.AddRange(query.Items);
			base.TotalResults += query.TotalResults;
			m_queryCounter--;
			if (m_queryCounter == 0)
			{
				OnQueryCompleted(result);
			}
		}

		public override void Run()
		{
			Run(1u);
		}

		public override void Run(uint startingPage)
		{
			base.Items?.Clear();
			base.TotalResults = 0u;
			m_queryCounter = m_queries.Count;
			foreach (MyWorkshopQuery query in m_queries)
			{
				query.SearchString = base.SearchString;
				query.ItemType = base.ItemType;
				query.ListType = base.ListType;
				query.QueryType = base.QueryType;
				query.RequiredTags = base.RequiredTags;
				query.RequireAllTags = base.RequireAllTags;
				query.ExcludedTags = base.ExcludedTags;
				query.UserId = base.UserId;
				query.ItemIds = base.ItemIds;
				query.Run(startingPage);
			}
		}

		public override void Stop()
		{
			foreach (MyWorkshopQuery query in m_queries)
			{
				query.Stop();
			}
		}

		public override void Dispose()
		{
			foreach (MyWorkshopQuery query in m_queries)
			{
				query.Dispose();
			}
			m_queries.Clear();
		}
	}
}
