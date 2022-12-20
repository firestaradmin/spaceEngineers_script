using System.Collections.Generic;
using System.Threading;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.EntityComponents
{
	/// <summary>
	/// Entity component providing terrain height when asked.
	/// </summary>
	internal class MyEntityTerrainHeightProviderComponent : MyEntityComponentBase, IMyTerrainHeightProvider
	{
		private class AsyncQuery
		{
			/// <summary>
			/// Last result height.
			/// </summary>
			public float Height;

			/// <summary>
			/// Last result normal.
			/// </summary>
			public Vector3 Normal = Vector3.Up;

			/// <summary>
			/// Whether this query actually returned results.
			/// </summary>
			public bool HasValue;

			/// <summary>
			/// Whether this task is waiting for a result.
			/// </summary>
			public int Queued;

			public List<MyPhysics.HitInfo> HitList = new List<MyPhysics.HitInfo>(32);

			/// <summary>
			/// Terrain height provider component.
			/// </summary>
			private MyEntityTerrainHeightProviderComponent m_component;

			public AsyncQuery(MyEntityTerrainHeightProviderComponent component)
			{
				m_component = component;
				Height = ((IMyTerrainHeightProvider)m_component).GetReferenceTerrainHeight();
			}

			public void Complete(List<MyPhysics.HitInfo> hits)
			{
				Queued = 0;
			}

			public void ProcessQuery()
			{
				float terrainHeight;
				Vector3 terrainNormal;
				bool flag = (HasValue = m_component.ProcessResult(HitList, out terrainHeight, out terrainNormal));
				Height = terrainHeight;
				Normal = terrainNormal;
				HitList.Clear();
			}
		}

		private class Sandbox_Game_EntityComponents_MyEntityTerrainHeightProviderComponent_003C_003EActor : IActivator, IActivator<MyEntityTerrainHeightProviderComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityTerrainHeightProviderComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityTerrainHeightProviderComponent CreateInstance()
			{
				return new MyEntityTerrainHeightProviderComponent();
			}

			MyEntityTerrainHeightProviderComponent IActivator<MyEntityTerrainHeightProviderComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private List<MyPhysics.HitInfo> m_raycastHits = new List<MyPhysics.HitInfo>(32);

		private MatrixD m_worldMatrix;

		private MatrixD m_worldMatrixInv;

		private float m_bbBottom;

		private Dictionary<int, AsyncQuery> m_cachedTasks = new Dictionary<int, AsyncQuery>();

		public override string ComponentTypeDebugString => "SkinnedEntityTerrainHeightProvider";

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			base.Entity.PositionComp.OnPositionChanged += OnPositionChanged;
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			base.Entity.PositionComp.OnPositionChanged -= OnPositionChanged;
		}

		private void OnPositionChanged(MyPositionComponentBase obj)
		{
			m_worldMatrix = base.Entity.WorldMatrix;
			m_worldMatrixInv = base.Entity.WorldMatrixNormalizedInv;
			m_bbBottom = base.Entity.PositionComp.LocalAABB.Min.Y;
		}

		bool IMyTerrainHeightProvider.GetTerrainHeight(int key, Vector3 bonePosition, Vector3 boneRigPosition, out float terrainHeight, out Vector3 terrainNormal)
		{
			MatrixD matrix = m_worldMatrix;
			Vector3D down = matrix.Down;
			Vector3D vector3D = Vector3D.Transform(new Vector3(bonePosition.X, m_bbBottom, bonePosition.Z), ref matrix);
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS)
			{
				MyRenderProxy.DebugDrawLine3D(vector3D - down, vector3D + down, Color.Red, Color.Yellow, depthRead: false);
			}
			if (key == 0)
			{
				using (MyUtils.ReuseCollection(ref m_raycastHits))
				{
					MyPhysics.CastRay(vector3D - down, vector3D + down, m_raycastHits, 18);
					return ProcessResult(m_raycastHits, out terrainHeight, out terrainNormal);
				}
			}
			if (!m_cachedTasks.TryGetValue(key, out var value))
			{
				value = (m_cachedTasks[key] = new AsyncQuery(this));
			}
			if (Interlocked.Exchange(ref value.Queued, 1) == 0)
			{
				Vector3D from = vector3D - down;
				Vector3D to = vector3D + down;
				value.ProcessQuery();
				MyPhysics.CastRayParallel(ref from, ref to, value.HitList, 18, value.Complete);
			}
			if (value.HasValue)
			{
				terrainHeight = value.Height;
				terrainNormal = value.Normal;
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS)
				{
					Vector3D vector3D2 = Vector3D.Transform(bonePosition + new Vector3(0f, terrainHeight, 0f), matrix);
					MyRenderProxy.DebugDrawSphere(vector3D2, 0.05f, Color.Yellow, 1f, depthRead: false);
					Vector3D direction = Vector3D.TransformNormal(terrainNormal, matrix);
					MyRenderProxy.DebugDrawArrow3DDir(vector3D2, direction, Color.Yellow);
				}
				return true;
			}
			terrainHeight = m_bbBottom;
			terrainNormal = Vector3.Zero;
			return false;
		}

		private bool ProcessResult(List<MyPhysics.HitInfo> hits, out float terrainHeight, out Vector3 terrainNormal)
		{
			foreach (MyPhysics.HitInfo hit in hits)
			{
				if (hit.HkHitInfo.Body == null || hit.HkHitInfo.Body.IsDisposed)
				{
					continue;
				}
				IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
				if (hitEntity != base.Entity && !(hitEntity is MyCharacter))
				{
					if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS)
					{
						MyRenderProxy.DebugDrawSphere(hit.Position, 0.05f, Color.Red, 1f, depthRead: false);
					}
					terrainHeight = (float)Vector3D.Transform(hit.Position, m_worldMatrixInv).Y - m_bbBottom;
					float convexRadius = hit.HkHitInfo.GetConvexRadius();
					terrainHeight -= ((convexRadius < 0.06f) ? convexRadius : 0.06f);
					terrainNormal = Vector3D.Transform(hit.HkHitInfo.Normal, m_worldMatrixInv.GetOrientation());
					return true;
				}
			}
			terrainHeight = 0f;
			terrainNormal = Vector3.Zero;
			return false;
		}

		float IMyTerrainHeightProvider.GetReferenceTerrainHeight()
		{
			return m_bbBottom;
		}
	}
}
