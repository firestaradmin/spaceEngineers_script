<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using SharpDX.Direct3D11;
using VRage.Collections;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Scene.Components
{
	internal class MyFoliageManager : IManager, IManagerUpdate, IManagerDevice, IManagerUnloadData
	{
		private class FoliageBufferAllocator : IMyElementAllocator<IVertexBuffer>
		{
			public bool ExplicitlyDisposeAllElements => false;

			public unsafe IVertexBuffer Allocate(int bucketId)
			{
				return MyManagers.Buffers.CreateVertexBuffer("MyFoliageStream", bucketId, sizeof(Vector3) + 4, null, ResourceUsage.Default, isStreamOutput: true);
			}

			public void Dispose(IVertexBuffer buffer)
			{
				MyManagers.Buffers.Dispose(buffer);
			}

			public void Init(IVertexBuffer instance)
			{
			}

			public int GetBucketId(IVertexBuffer buffer)
			{
				return buffer.ElementCount;
			}

			public int GetBytes(IVertexBuffer buffer)
			{
				return buffer.ByteSize;
			}
		}

		/// <summary>
		/// The farthest lod that can have rocks
		/// </summary>
		private const int ROCKS_LOD_LIMIT = 0;

		/// <summary>
		/// The farthest lod that can have foliage
		/// </summary>
		private const int GRASS_LOD_LIMIT = 4;

		private HashSet<MyFoliageComponent> m_activeComponents;

		private const float CACHED_DISTANCE = 1.2f;

		public readonly MyConcurrentBucketPool<IVertexBuffer> FoliagePool = new MyConcurrentBucketPool<IVertexBuffer, FoliageBufferAllocator>("FoliagePool");

		internal float GrassDistanceSqr { get; private set; }

		internal float GrassDistanceCachedSqr { get; private set; }

		public void OnDeviceInit()
		{
			m_activeComponents = new HashSet<MyFoliageComponent>();
		}

		public void OnDeviceReset()
		{
			Reset();
		}

		public void OnDeviceEnd()
		{
		}

		public void OnUnloadData()
		{
			Reset();
		}

		internal void RegisterActive(MyFoliageComponent component)
		{
			m_activeComponents.Add(component);
		}

		internal void UnegisterActive(MyFoliageComponent component)
		{
			m_activeComponents.Remove(component);
		}

		public void OnUpdate()
		{
			float grassDrawDistance = MyRender11.Settings.User.GrassDrawDistance;
			GrassDistanceSqr = grassDrawDistance * grassDrawDistance;
			GrassDistanceCachedSqr = GrassDistanceSqr * 1.2f * 1.2f;
		}

		internal void UpdateFoliage(MyActor actor)
		{
			MeshId model = actor.GetRenderable().GetModel();
			int lod = MyMeshes.GetVoxelInfo(model).Lod;
			MyFoliageComponent foliage = actor.GetFoliage();
			bool flag = model.ShouldHaveFoliage(lod > 0) && lod <= 4;
			if (foliage != null)
			{
				if (!flag)
				{
					actor.RemoveComponent<MyFoliageComponent>(foliage);
				}
				else
				{
					foliage.RefreshStreams();
				}
			}
			else if (flag)
			{
				foliage = MyComponentFactory<MyFoliageComponent>.Create();
				actor.AddComponent<MyFoliageComponent>(foliage);
				foliage.RefreshStreams();
			}
		}

		internal void Reset()
		{
<<<<<<< HEAD
			foreach (MyFoliageComponent activeComponent in m_activeComponents)
			{
				activeComponent.RefreshStreams();
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyFoliageComponent> enumerator = m_activeComponents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().RefreshStreams();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_activeComponents.Clear();
			FoliagePool.Clear();
		}
	}
}
