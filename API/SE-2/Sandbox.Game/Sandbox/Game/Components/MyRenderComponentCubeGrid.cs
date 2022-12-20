using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentCubeGrid : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentCubeGrid_003C_003EActor : IActivator, IActivator<MyRenderComponentCubeGrid>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentCubeGrid();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentCubeGrid CreateInstance()
			{
				return new MyRenderComponentCubeGrid();
			}

			MyRenderComponentCubeGrid IActivator<MyRenderComponentCubeGrid>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyStringId ID_RED_DOT_IGNORE_DEPTH = MyStringId.GetOrCompute("RedDotIgnoreDepth");

		private static readonly MyStringId ID_WEAPON_LASER_IGNORE_DEPTH = MyStringId.GetOrCompute("WeaponLaserIgnoreDepth");

		private static readonly List<MyPhysics.HitInfo> m_tmpHitList = new List<MyPhysics.HitInfo>();

		private MyCubeGrid m_grid;

		private bool m_deferRenderRelease;

		private bool m_shouldReleaseRenderObjects;

		private MyCubeGridRenderData m_renderData;

		private MyParticleEffect m_atmosphericEffect;

		private const float m_atmosphericEffectMinSpeed = 75f;

		private const float m_atmosphericEffectMinFade = 0.85f;

		private const int m_atmosphericEffectVoxelContactDelay = 5000;

		private int m_lastVoxelContactTime;

		private static List<Vector3> m_tmpCornerList = new List<Vector3>();

		public MyCubeGrid CubeGrid => m_grid;

		public bool DeferRenderRelease
		{
			get
			{
				return m_deferRenderRelease;
			}
			set
			{
				m_deferRenderRelease = value;
				if (!value && m_shouldReleaseRenderObjects)
				{
					RemoveRenderObjects();
				}
			}
		}

		public MyCubeGridRenderData RenderData => m_renderData;

		public MyCubeSize GridSizeEnum => m_grid.GridSizeEnum;

		public float GridSize => m_grid.GridSize;

		public bool IsStatic => m_grid.IsStatic;

		public MyRenderComponentCubeGrid()
		{
			m_renderData = new MyCubeGridRenderData(this);
		}

		public void RebuildDirtyCells()
		{
			m_renderData.RebuildDirtyCells(GetRenderFlags());
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_grid = base.Container.Entity as MyCubeGrid;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (m_atmosphericEffect != null)
			{
				MyParticlesManager.RemoveParticleEffect(m_atmosphericEffect);
				m_atmosphericEffect = null;
			}
		}

		public override void Draw()
		{
			base.Draw();
			DrawBlocks();
			if (MyCubeGrid.ShowCenterOfMass && !IsStatic && base.Container.Entity.Physics != null && base.Container.Entity.Physics.HasRigidBody)
			{
				DrawCenterOfMass();
			}
			if (MyCubeGrid.ShowGridPivot)
			{
				DrawGridPivot();
			}
			if (m_grid.MarkedAsTrash)
			{
				DrawMarkedAsTrash();
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrawMarkedAsTrash()
		{
			BoundingBoxD localbox = m_grid.PositionComp.LocalAABB;
			localbox.Max += 0.2f;
			localbox.Min -= 0.20000000298023224;
			MatrixD worldMatrix = m_grid.PositionComp.WorldMatrixRef;
			Color color = Color.Red;
			color.A = (byte)(100.0 * (Math.Sin((float)m_grid.TrashHighlightCounter / 10f) + 1.0) / 2.0 + 100.0);
			color.R = (byte)(200.0 * (Math.Sin((float)m_grid.TrashHighlightCounter / 10f) + 1.0) / 2.0 + 50.0);
			MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 1, 0.008f, null, null, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrawGridPivot()
		{
			MatrixD worldMatrix = base.Container.Entity.WorldMatrix;
			Vector3D translation = worldMatrix.Translation;
			Vector3D position = MySector.MainCamera.Position;
			float num = Vector3.Distance(position, translation);
			bool flag = false;
			if (num < 30f)
			{
				flag = true;
			}
			else if (num < 200f)
			{
				flag = true;
				MyPhysics.CastRay(position, translation, m_tmpHitList, 16);
				foreach (MyPhysics.HitInfo tmpHit in m_tmpHitList)
				{
					if (tmpHit.HkHitInfo.GetHitEntity() != this)
					{
						flag = false;
						break;
					}
				}
				m_tmpHitList.Clear();
			}
			if (flag)
			{
				float num2 = MathHelper.Lerp(1f, 9f, num / 200f);
				MyStringId iD_WEAPON_LASER_IGNORE_DEPTH = ID_WEAPON_LASER_IGNORE_DEPTH;
				float thickness = 0.02f * num2;
				Vector4 color = Color.Green.ToVector4();
				MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix.Up * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness);
				color = Color.Blue.ToVector4();
				MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix.Forward * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness);
				color = Color.Red.ToVector4();
				MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix.Right * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness);
				MyTransparentGeometry.AddBillboardOriented(ID_RED_DOT_IGNORE_DEPTH, Color.White.ToVector4(), translation, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, 0.1f * num2);
				MyRenderProxy.DebugDrawAxis(worldMatrix, 0.5f, depthRead: false);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrawCenterOfMass()
		{
			MatrixD worldMatrix = base.Container.Entity.Physics.GetWorldMatrix();
			Vector3D centerOfMassWorld = base.Container.Entity.Physics.CenterOfMassWorld;
			Vector3D position = MySector.MainCamera.Position;
			float num = Vector3.Distance(position, centerOfMassWorld);
			bool flag = false;
			if (num < 30f)
			{
				flag = true;
			}
			else if (num < 200f)
			{
				flag = true;
				MyPhysics.CastRay(position, centerOfMassWorld, m_tmpHitList, 16);
				foreach (MyPhysics.HitInfo tmpHit in m_tmpHitList)
				{
					if (tmpHit.HkHitInfo.GetHitEntity() != this)
					{
						flag = false;
						break;
					}
				}
				m_tmpHitList.Clear();
			}
			if (flag)
			{
				float num2 = MathHelper.Lerp(1f, 9f, num / 200f);
				MyStringId iD_WEAPON_LASER_IGNORE_DEPTH = ID_WEAPON_LASER_IGNORE_DEPTH;
				Vector4 color = Color.Yellow.ToVector4();
				float thickness = 0.02f * num2;
				MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Up * 0.5 * num2, centerOfMassWorld + worldMatrix.Up * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
				MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Forward * 0.5 * num2, centerOfMassWorld + worldMatrix.Forward * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
				MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Right * 0.5 * num2, centerOfMassWorld + worldMatrix.Right * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
				MyTransparentGeometry.AddBillboardOriented(ID_RED_DOT_IGNORE_DEPTH, Color.White.ToVector4(), centerOfMassWorld, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, 0.1f * num2, MyBillboard.BlendTypeEnum.AdditiveTop);
<<<<<<< HEAD
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrawBlocks()
		{
			m_grid.BlocksForDraw.ApplyChanges();
			foreach (MyCubeBlock item in m_grid.BlocksForDraw)
			{
				if (MyRenderProxy.VisibleObjectsRead.Contains(item.Render.RenderObjectIDs[0]))
				{
					item.Render.Draw();
				}
=======
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrawBlocks()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			m_grid.BlocksForDraw.ApplyChanges();
			Enumerator<MyCubeBlock> enumerator = m_grid.BlocksForDraw.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCubeBlock current = enumerator.get_Current();
					if (MyRenderProxy.VisibleObjectsRead.Contains(current.Render.RenderObjectIDs[0]))
					{
						current.Render.Draw();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void ResetLastVoxelContactTimer()
		{
			m_lastVoxelContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public override void AddRenderObjects()
		{
			MyCubeGrid myCubeGrid = base.Container.Entity as MyCubeGrid;
			if (m_renderObjectIDs[0] == uint.MaxValue && (myCubeGrid.IsDirty() || m_renderData.HasDirtyCells))
			{
				myCubeGrid.UpdateInstanceData();
			}
		}

		public override void RemoveRenderObjects()
		{
			if (m_deferRenderRelease)
			{
				m_shouldReleaseRenderObjects = true;
				return;
			}
			m_shouldReleaseRenderObjects = false;
			m_renderData.OnRemovedFromRender();
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (m_renderObjectIDs[i] != uint.MaxValue)
				{
					ReleaseRenderObjectID(i);
				}
			}
		}

		protected override void UpdateRenderObjectVisibility(bool visible)
		{
			base.UpdateRenderObjectVisibility(visible);
		}

		public void UpdateRenderObjectMatrices(Matrix matrix)
		{
			MatrixD worldMatrix = matrix;
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (m_renderObjectIDs[i] != uint.MaxValue)
				{
					MyRenderProxy.UpdateRenderObject(base.RenderObjectIDs[i], in worldMatrix, in BoundingBox.Invalid, hasLocalAabb: false, LastMomentUpdateIndex);
				}
			}
		}
	}
}
