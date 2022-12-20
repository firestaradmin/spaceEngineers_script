using System.Collections.Generic;
using System.Threading;
using SharpDX.Direct3D11;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;

namespace VRageRender
{
	internal class MyGeometryRendererOld : IManager, IManagerUnloadData
	{
		internal class MyConstantBufferAllocator : IMyElementAllocator<IConstantBuffer>
		{
			public bool ExplicitlyDisposeAllElements => false;

			public IConstantBuffer Allocate(int size)
			{
				return MyManagers.Buffers.CreateConstantBuffer("PooledCBInstance" + size, size, null, ResourceUsage.Dynamic);
			}

			public void Dispose(IConstantBuffer instance)
			{
				MyManagers.Buffers.Dispose(instance);
			}

			public void Init(IConstantBuffer instance)
			{
			}

			public int GetBytes(IConstantBuffer instance)
			{
				return instance.ByteSize;
			}

			public int GetBucketId(IConstantBuffer instance)
			{
				return instance.ByteSize;
			}
		}

		private readonly List<MyRenderingWork> m_workList = new List<MyRenderingWork>();

		private static readonly MyConcurrentPool<MyRenderingWork> m_workPool = new MyConcurrentPool<MyRenderingWork>();

		private static readonly MyConcurrentPool<MyList<MyRenderCullResultFlat>> m_resultListPool = new MyConcurrentPool<MyList<MyRenderCullResultFlat>>(8);

		private static readonly MyConcurrentPool<MyList<MyRenderCullResultBatch>> m_resultBatchListPool = new MyConcurrentPool<MyList<MyRenderCullResultBatch>>(8);

		public readonly MyConcurrentBufferPool<IConstantBuffer, MyConstantBufferAllocator> ConstantBufferPool = new MyConcurrentBufferPool<IConstantBuffer, MyConstantBufferAllocator>("PooledCB");

		internal void InitFrame()
		{
			for (int i = m_workList.Count; i < 19; i++)
			{
				m_workList.Add(null);
			}
		}

		internal int ProcessCullProxies(MyCullQuery cullQuery)
		{
			UpdateCullProxies(cullQuery);
			TrimCullProxies(cullQuery);
			return cullQuery.Results.CullProxies.Count;
		}

		internal void UpdateCullProxies(MyCullQuery cullQuery)
		{
			foreach (MyCullProxy cullProxy in cullQuery.Results.CullProxies)
			{
				if (Interlocked.CompareExchange(ref cullProxy.Updated, 0, 0) == 1)
				{
					continue;
				}
				lock (cullProxy)
				{
					if (cullProxy.Updated == 0)
					{
						MyRenderableComponent parent = cullProxy.Parent;
						parent.UpdateAfterCull();
						if (!parent.IsCulled)
						{
							cullProxy.UpdateWorldMatrix();
						}
						Interlocked.Exchange(ref cullProxy.Updated, 1);
					}
				}
			}
			foreach (MyCullProxy_2 item in cullQuery.Results.CullProxies2)
			{
				item.FirstFullyContainingCascadeIndex = 0;
			}
		}

		internal void TrimCullProxies(MyCullQuery cullQuery)
		{
			MyList<MyCullProxy> cullProxies = cullQuery.Results.CullProxies;
			MyList<bool> cullProxiesContained = cullQuery.Results.CullProxiesContained;
			switch (cullQuery.ViewType)
			{
			default:
			{
				for (int l = 0; l < cullProxies.Count; l++)
				{
					MyCullProxy myCullProxy3 = cullProxies[l];
					if (myCullProxy3.Parent.IsCulled)
					{
						myCullProxy3.Updated = 0;
						cullProxies.RemoveAtFast(l);
						cullProxiesContained.RemoveAtFast(l);
						l--;
					}
				}
				break;
			}
			case MyViewType.ShadowProjection:
			{
				for (int k = 0; k < cullProxies.Count; k++)
				{
					MyCullProxy myCullProxy2 = cullProxies[k];
					if (myCullProxy2.Parent.IsCulled || (myCullProxy2.RenderFlags & MyRenderableProxyFlags.CastShadows) == 0)
					{
						myCullProxy2.Updated = 0;
						cullProxies.RemoveAtFast(k);
						cullProxiesContained.RemoveAtFast(k);
						k--;
					}
				}
				break;
			}
			case MyViewType.ShadowCascade:
			{
				bool flag = cullQuery.ViewIndex < 4;
				for (int i = 0; i < cullProxies.Count; i++)
				{
					MyCullProxy myCullProxy = cullProxies[i];
					if (myCullProxy.Parent.IsCulled || (myCullProxy.RenderFlags & MyRenderableProxyFlags.CastShadows) == 0 || (!flag && (myCullProxy.RenderFlags & MyRenderableProxyFlags.DrawOutsideViewDistance) == 0))
					{
						myCullProxy.Updated = 0;
						cullProxies.RemoveAtFast(i);
						cullProxiesContained.RemoveAtFast(i);
						i--;
					}
					else if (cullProxiesContained[i])
					{
						myCullProxy.FirstFullyContainingCascadeIndex = cullQuery.ViewIndex;
					}
				}
				MyList<MyCullProxy_2> cullProxies2 = cullQuery.Results.CullProxies2;
				MyList<bool> cullProxies2Contained = cullQuery.Results.CullProxies2Contained;
				if (cullProxies2.Count <= 0)
				{
					break;
				}
				for (int j = 0; j < cullProxies2.Count; j++)
				{
					MyCullProxy_2 myCullProxy_ = cullProxies2[j];
					if (myCullProxy_.FirstFullyContainingCascadeIndex < cullQuery.ViewIndex)
					{
						cullProxies2.RemoveAtFast(j);
						cullProxies2Contained.RemoveAtFast(j);
						j--;
					}
					else if (cullProxies2Contained[j])
					{
						myCullProxy_.FirstFullyContainingCascadeIndex = cullQuery.ViewIndex;
					}
				}
				break;
			}
			}
<<<<<<< HEAD
			if (cullQuery.Ignored == null || cullQuery.Ignored.Count <= 0)
=======
			if (cullQuery.Ignored == null || cullQuery.Ignored.get_Count() <= 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			for (int m = 0; m < cullProxies.Count; m++)
			{
				MyCullProxy myCullProxy4 = cullProxies[m];
				if (cullQuery.Ignored.Contains(myCullProxy4.Parent.Owner.ID))
				{
					cullProxies.RemoveAtFast(m);
					cullProxiesContained.RemoveAtFast(m);
					m--;
				}
			}
		}

		private MyList<MyRenderCullResultFlat> PrepareFilter(MyViewType viewType, MyList<MyCullProxy> cullProxies)
		{
			MyDrawSubmesh.MySubmeshFlags mySubmeshFlags;
			MyRenderableProxyFlags myRenderableProxyFlags;
			switch (viewType)
			{
			case MyViewType.ShadowCascade:
				mySubmeshFlags = MyDrawSubmesh.MySubmeshFlags.Depth;
				myRenderableProxyFlags = MyRenderableProxyFlags.SkipInDepth;
				break;
			case MyViewType.ShadowProjection:
				mySubmeshFlags = MyDrawSubmesh.MySubmeshFlags.Depth;
				myRenderableProxyFlags = MyRenderableProxyFlags.SkipInDepth;
				break;
			case MyViewType.EnvironmentProbe:
				mySubmeshFlags = MyDrawSubmesh.MySubmeshFlags.Forward;
				myRenderableProxyFlags = MyRenderableProxyFlags.SkipInForward;
				break;
			default:
				mySubmeshFlags = MyDrawSubmesh.MySubmeshFlags.Gbuffer;
				myRenderableProxyFlags = MyRenderableProxyFlags.SkipInMainView;
				break;
			}
			MyList<MyRenderCullResultFlat> myList = m_resultListPool.Get();
			int num = 0;
			foreach (MyCullProxy cullProxy in cullProxies)
			{
				if (cullProxy.Parent.IsCulled)
				{
					continue;
				}
				MyRenderableProxy[] renderableProxies = cullProxy.RenderableProxies;
				ulong[] sortingKeys = cullProxy.SortingKeys;
				if (renderableProxies.Length != sortingKeys.Length)
				{
					cullProxy.Parent.SetProxiesForCurrentLod();
					renderableProxies = cullProxy.RenderableProxies;
					sortingKeys = cullProxy.SortingKeys;
					if (renderableProxies.Length != sortingKeys.Length)
					{
						continue;
					}
				}
				for (int i = 0; i < renderableProxies.Length; i++)
				{
					MyRenderableProxy myRenderableProxy = renderableProxies[i];
					if ((myRenderableProxy.DrawSubmesh.Flags & mySubmeshFlags) == 0 || (myRenderableProxy.Flags & myRenderableProxyFlags) > MyRenderableProxyFlags.None)
					{
						continue;
					}
					ulong sortKey = sortingKeys[i];
					MyRenderCullResultFlat myRenderCullResultFlat = default(MyRenderCullResultFlat);
					myRenderCullResultFlat.SortKey = sortKey;
					myRenderCullResultFlat.RenderProxy = myRenderableProxy;
					myRenderCullResultFlat.ObjectBufferSize = myRenderableProxy.ObjectBufferSizeAligned;
					MyRenderCullResultFlat item = myRenderCullResultFlat;
					if (myRenderableProxy.TransparentTechnique)
					{
						if (viewType == MyViewType.Main)
						{
							MyTransparentModelRenderer.Renderables.Add(item);
						}
					}
					else
					{
						myList.Add(item);
					}
				}
				num++;
			}
			return myList;
		}

		internal int Prepare(MyCullQuery cullQuery)
		{
			MyList<MyRenderCullResultFlat> myList = PrepareFilter(cullQuery.ViewType, cullQuery.Results.CullProxies);
			MyList<MyRenderCullResultBatch> batches = m_resultBatchListPool.Get();
			MyRenderCullResultBatch decalsBatch = default(MyRenderCullResultBatch);
			MyRenderCullResultBatch currentBatch;
			int batchSize;
			if (myList.Count > 0)
			{
				myList.Sort((MyRenderCullResultFlat s1, MyRenderCullResultFlat s2) => s1.SortKey.CompareTo(s2.SortKey));
				currentBatch = default(MyRenderCullResultBatch);
				batchSize = 0;
				int oldPipelineBatchSizeAligned = MyRender11.Settings.OldPipelineBatchSizeAligned;
				int i;
				for (i = 0; i < myList.Count; i++)
				{
					MyRenderCullResultFlat myRenderCullResultFlat = myList[i];
					if (IsDecal(myRenderCullResultFlat.SortKey))
					{
						break;
					}
					int num = batchSize + myRenderCullResultFlat.ObjectBufferSize;
					if (num > oldPipelineBatchSizeAligned)
					{
						CommitBatch(i);
						batchSize = myRenderCullResultFlat.ObjectBufferSize;
					}
					else
					{
						batchSize = num;
					}
				}
				CommitBatch(i);
				if (cullQuery.ViewType == MyViewType.Main && i < myList.Count)
				{
					MyRenderCullResultBatch myRenderCullResultBatch = default(MyRenderCullResultBatch);
					myRenderCullResultBatch.Begin = i;
					myRenderCullResultBatch.Count = myList.Count - i;
					decalsBatch = myRenderCullResultBatch;
					batchSize = 0;
					for (; i < myList.Count; i++)
					{
						batchSize += myList[i].ObjectBufferSize;
					}
					decalsBatch.CBSize = batchSize;
				}
			}
			MyRenderableProxy_2[] array = PrepareFilterMergeInstancing();
			if (myList.Count != 0 || array != null)
			{
				m_workList[cullQuery.ViewId] = CreateWork(new MyRenderingWork.MyItem
				{
					Pass = cullQuery.RenderPass,
					Renderables = myList,
					List2 = array,
					Batches = batches,
					DecalsBatch = decalsBatch
				});
			}
			else
			{
				m_resultListPool.Return(myList);
				m_resultBatchListPool.Return(batches);
			}
			return myList.Count;
			void CommitBatch(int endIndex)
			{
				currentBatch.Count = endIndex - currentBatch.Begin;
				currentBatch.CBSize = batchSize;
				if (currentBatch.Count > 0)
				{
					batches.Add(currentBatch);
					currentBatch.Begin = endIndex;
				}
			}
<<<<<<< HEAD
			bool IsDecal(ulong key)
=======
			static bool IsDecal(ulong key)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return (key & 0x4000000000000L) != 0;
			}
		}

		private MyRenderableProxy_2[] PrepareFilterMergeInstancing()
		{
			return null;
		}

		private MyRenderingWork CreateWork(MyRenderingWork.MyItem workItem)
		{
			MyRenderingWork myRenderingWork = m_workPool.Get();
			myRenderingWork.Setup(workItem);
			return myRenderingWork;
		}

		private void ReturnWork(MyRenderingWork work)
		{
			work.WorkItem.Batches.SetSize(0);
			work.WorkItem.Renderables.SetSize(0);
			m_resultListPool.Return(work.WorkItem.Renderables);
			m_resultBatchListPool.Return(work.WorkItem.Batches);
			work.Cleanup();
			m_workPool.Return(work);
		}

		internal bool HasWork(MyCullQuery cullQuery)
		{
			if (m_workList[cullQuery.ViewId] != null)
			{
				return !m_workList[cullQuery.ViewId].WorkDone();
			}
			return false;
		}

		internal int Render(MyCullQuery cullQuery)
		{
			if (HasWork(cullQuery))
			{
				return m_workList[cullQuery.ViewId].DoWork();
			}
			return 0;
		}

		internal void DoneFrame()
		{
			for (int i = 0; i < m_workList.Count; i++)
			{
				MyRenderingWork myRenderingWork = m_workList[i];
				if (myRenderingWork != null)
				{
					myRenderingWork.ConsumeWork();
					ReturnWork(myRenderingWork);
					m_workList[i] = null;
				}
			}
		}

		public void OnUnloadData()
		{
			ConstantBufferPool.Clear();
		}
	}
}
