using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MyUGCAggregator
	{
		private readonly List<IMyUGCService> m_aggregates = new List<IMyUGCService>();

		public MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item)
		{
			return GetAggregate(item.ServiceName).CreateWorkshopPublisher(item);
		}

		public MyWorkshopQuery CreateWorkshopQuery()
		{
			return new MyWorkshopQueryAggregated(this);
		}

		public void SuspendWorkshopDownloads()
		{
			foreach (IMyUGCService aggregate in m_aggregates)
			{
				aggregate.SuspendWorkshopDownloads();
			}
		}

		public void ResumeWorkshopDownloads()
		{
			foreach (IMyUGCService aggregate in m_aggregates)
			{
				aggregate.ResumeWorkshopDownloads();
			}
		}

		public void SetTestEnvironment(bool testEnabled)
		{
			foreach (IMyUGCService aggregate in m_aggregates)
			{
				aggregate.SetTestEnvironment(testEnabled);
			}
		}

		public void Update()
		{
			foreach (IMyUGCService aggregate in m_aggregates)
			{
				aggregate.Update();
			}
		}

		public void AddAggregate(IMyUGCService ugc)
		{
			m_aggregates.Add(ugc);
		}

		public List<IMyUGCService> GetAggregates()
		{
			return m_aggregates;
		}

		public IMyUGCService GetAggregate(string serviceName)
		{
			foreach (IMyUGCService aggregate in m_aggregates)
			{
				if (aggregate.ServiceName == serviceName)
				{
					return aggregate;
				}
			}
			return null;
		}
	}
}
