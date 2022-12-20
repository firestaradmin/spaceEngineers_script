using System.Collections.Generic;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Game.Components;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MySessionComponentArmorHand : MySessionComponentBase
	{
		private MyCubeGrid m_lastCubeGrid;

		private Vector3I? m_lastBone;

		private Vector3I? m_lastCube;

		private Vector3D m_localBonePosition;

		private MyCubeGrid m_movingCubeGrid;

		private Vector3I? m_movingBone;

		public override void Draw()
		{
			base.Draw();
			if (!MyFakes.ENABLE_ARMOR_HAND)
			{
				return;
			}
			Vector3 forwardVector = MySector.MainCamera.ForwardVector;
			Vector3D linePointA = MySector.MainCamera.Position;
			Vector3D linePointB = linePointA + forwardVector * 100f;
			m_lastCubeGrid = null;
			m_lastBone = null;
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(linePointA, linePointB, 29);
			MyCubeGrid myCubeGrid = (hitInfo.HasValue ? ((MyPhysicsBody)hitInfo.Value.HkHitInfo.Body.UserObject).Entity : null) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			m_lastCubeGrid = myCubeGrid;
			double num = double.MaxValue;
			LineD line = new LineD(linePointA, linePointB);
			Vector3I position = default(Vector3I);
			double distanceSquared = double.MaxValue;
			if (m_lastCubeGrid.GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared))
			{
				m_lastCube = position;
			}
			else
			{
				m_lastCube = null;
			}
			foreach (KeyValuePair<Vector3I, Vector3> bone in myCubeGrid.Skeleton.Bones)
			{
				Vector3D point = Vector3D.Transform((Vector3D)(bone.Key / 2f) * (double)myCubeGrid.GridSize + bone.Value - new Vector3D(myCubeGrid.GridSize / 2f), myCubeGrid.PositionComp.WorldMatrixRef);
				Color color = Color.Red;
				if (MyUtils.GetPointLineDistance(ref linePointA, ref linePointB, ref point) < 0.10000000149011612)
				{
					double num2 = (linePointA - point).LengthSquared();
					if (num2 < num)
					{
						num = num2;
						color = Color.Blue;
						m_lastBone = bone.Key;
					}
				}
				MyRenderProxy.DebugDrawSphere(point, 0.05f, color.ToVector3(), 0.5f, depthRead: false, smooth: true);
			}
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (!MyFakes.ENABLE_ARMOR_HAND)
			{
				return;
			}
			if (MyInput.Static.IsNewLeftMousePressed() && m_lastCubeGrid != null && m_lastBone.HasValue)
			{
<<<<<<< HEAD
				Vector3D position = Vector3D.Transform((Vector3D)(m_lastBone / 2f).Value * (double)m_lastCubeGrid.GridSize + m_lastCubeGrid.Skeleton.Bones[m_lastBone.Value] - new Vector3D(m_lastCubeGrid.GridSize / 2f), m_lastCubeGrid.PositionComp.WorldMatrixRef);
				m_localBonePosition = Vector3D.Transform(position, MySession.Static.LocalCharacter.PositionComp.WorldMatrixNormalizedInv);
=======
				Vector3D vector3D = Vector3D.Transform((Vector3D)(m_lastBone / 2f).Value * (double)m_lastCubeGrid.GridSize + m_lastCubeGrid.Skeleton.Bones.get_Item(m_lastBone.Value) - new Vector3D(m_lastCubeGrid.GridSize / 2f), m_lastCubeGrid.PositionComp.WorldMatrixRef);
				m_localBonePosition = Vector3.Transform(vector3D, MySession.Static.LocalCharacter.PositionComp.WorldMatrixNormalizedInv);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_movingCubeGrid = m_lastCubeGrid;
				m_movingBone = m_lastBone;
				m_lastCubeGrid.Skeleton.GetDefinitionOffsetWithNeighbours(m_lastCube.Value, m_movingBone.Value, m_lastCubeGrid);
			}
			if (MyInput.Static.IsLeftMousePressed() && m_movingCubeGrid != null && m_movingBone.HasValue)
			{
				if (MyInput.Static.IsAnyShiftKeyPressed())
				{
					Vector3 boneOnSphere = GetBoneOnSphere(new Vector3I(2, 0, 0), m_movingBone.Value, m_movingCubeGrid);
					m_movingCubeGrid.Skeleton.Bones.set_Item(m_movingBone.Value, boneOnSphere);
				}
				else
				{
<<<<<<< HEAD
					Vector3D vector3D = Vector3D.Transform(m_localBonePosition, MySession.Static.LocalCharacter.PositionComp.WorldMatrixRef);
					Vector3D vector3D2 = Vector3D.Transform(vector3D, m_movingCubeGrid.PositionComp.WorldMatrixInvScaled);
					vector3D2 += new Vector3D(m_movingCubeGrid.GridSize / 2f);
					m_movingCubeGrid.Skeleton.Bones[m_movingBone.Value] = vector3D2 - (Vector3D)(m_movingBone / 2f).Value * (double)m_movingCubeGrid.GridSize;
					Vector3I vector3I = m_movingCubeGrid.WorldToGridInteger(vector3D);
=======
					Vector3D vector3D2 = Vector3D.Transform(m_localBonePosition, MySession.Static.LocalCharacter.PositionComp.WorldMatrixRef);
					Vector3D vector3D3 = Vector3D.Transform(vector3D2, m_movingCubeGrid.PositionComp.WorldMatrixInvScaled);
					vector3D3 += new Vector3D(m_movingCubeGrid.GridSize / 2f);
					m_movingCubeGrid.Skeleton.Bones.set_Item(m_movingBone.Value, (Vector3)(vector3D3 - (Vector3D)(m_movingBone / 2f).Value * (double)m_movingCubeGrid.GridSize));
					Vector3I vector3I = m_movingCubeGrid.WorldToGridInteger(vector3D2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							for (int k = -1; k <= 1; k++)
							{
								m_movingCubeGrid.SetCubeDirty(new Vector3I(i, j, k) + vector3I);
							}
						}
					}
				}
			}
			if (MyInput.Static.IsNewLeftMouseReleased())
			{
				m_movingCubeGrid = null;
				m_movingBone = null;
			}
		}

		private Vector3D BoneToWorld(Vector3I bone, Vector3 offset, MyCubeGrid grid)
		{
			return Vector3D.Transform((Vector3D)(bone / 2f) * (double)grid.GridSize + offset - new Vector3D(grid.GridSize / 2f), grid.PositionComp.WorldMatrixRef);
		}

		private Vector3 GetBoneOnSphere(Vector3I center, Vector3I bonePos, MyCubeGrid grid)
		{
			Vector3D vector3D = BoneToWorld(center, Vector3.Zero, grid);
			Vector3D vector3D2 = BoneToWorld(bonePos, Vector3.Zero, grid);
			BoundingSphereD boundingSphereD = new BoundingSphereD(vector3D, grid.GridSize);
			Vector3D vector3D3 = vector3D - vector3D2;
			vector3D3.Normalize();
			RayD ray = new RayD(vector3D2, vector3D3);
			if (boundingSphereD.IntersectRaySphere(ray, out var tmin, out var _))
			{
				return Vector3D.Transform(vector3D2 + vector3D3 * tmin, grid.PositionComp.WorldMatrixInvScaled) + new Vector3D(grid.GridSize / 2f) - (Vector3D)(bonePos / 2f) * (double)grid.GridSize;
			}
			return Vector3.Zero;
		}
	}
}
