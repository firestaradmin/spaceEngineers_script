using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game
{
	/// <summary>
	/// This class is responsible for calculating damage from explosions
	/// It works by recursively raycasting from the point it needs to 
	/// calculate to the explosion center. It does two types of raycast, 
	/// 3D DDA raycasts for traversing grids (ships, stations) and Havok 
	/// raycasts for traversing space between grids. For each block, it builds
	/// a stack of blocks that are between it and the explosion center and then
	/// calculates the damage for all blocks in this stack. 
	/// It early exits if it encounters a block that was already calculated.
	/// </summary>
	public class MyGridExplosion
	{
		public struct MyRaycastDamageInfo
		{
			public float DamageRemaining;

			public float DistanceToExplosion;

			public MyRaycastDamageInfo(float damageRemaining, float distanceToExplosion)
			{
				DamageRemaining = damageRemaining;
				DistanceToExplosion = distanceToExplosion;
			}
		}

		public bool GridWasHit;

		public readonly HashSet<MyCubeGrid> AffectedCubeGrids = new HashSet<MyCubeGrid>();

		public readonly HashSet<MySlimBlock> AffectedCubeBlocks = new HashSet<MySlimBlock>();

		private Dictionary<MySlimBlock, float> m_damagedBlocks = new Dictionary<MySlimBlock, float>();

		private Dictionary<MySlimBlock, MyRaycastDamageInfo> m_damageRemaining = new Dictionary<MySlimBlock, MyRaycastDamageInfo>();

		private Stack<MySlimBlock> m_castBlocks = new Stack<MySlimBlock>();

		private BoundingSphereD m_explosion;

		private float m_explosionDamage;

		private int stackOverflowGuard;

		private const int MAX_PHYSICS_RECURSION_COUNT = 10;

		private List<Vector3I> m_cells = new List<Vector3I>();

		public Dictionary<MySlimBlock, float> DamagedBlocks => m_damagedBlocks;

		public Dictionary<MySlimBlock, MyRaycastDamageInfo> DamageRemaining => m_damageRemaining;

		public float Damage => m_explosionDamage;

		public BoundingSphereD Sphere => m_explosion;

		public void Init(BoundingSphereD explosion, float explosionDamage)
		{
			m_explosion = explosion;
			m_explosionDamage = explosionDamage;
		}

		public void Clear()
		{
			AffectedCubeBlocks.Clear();
			AffectedCubeGrids.Clear();
			m_damageRemaining.Clear();
			m_damagedBlocks.Clear();
			m_castBlocks.Clear();
		}

		/// <summary>
		/// Computes damage for all blocks assigned in constructor
		/// </summary>
		public void ComputeDamagedBlocks()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySlimBlock> enumerator = AffectedCubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
<<<<<<< HEAD
					MySlimBlock mySlimBlock = m_castBlocks.Pop();
					if (mySlimBlock.FatBlock is MyWarhead)
					{
						m_damagedBlocks[mySlimBlock] = 1E+07f;
						continue;
					}
					float num = (float)(mySlimBlock.WorldAABB.Center - m_explosion.Center).Length();
					if (value.DamageRemaining > 0f)
					{
						float num2 = MathHelper.Clamp(1f - (num - value.DistanceToExplosion) / ((float)m_explosion.Radius - value.DistanceToExplosion), 0f, 1f);
						if (num2 > 0f)
=======
					MySlimBlock current = enumerator.get_Current();
					m_castBlocks.Clear();
					MyRaycastDamageInfo value = CastDDA(current);
					while (m_castBlocks.get_Count() > 0)
					{
						MySlimBlock mySlimBlock = m_castBlocks.Pop();
						if (mySlimBlock.FatBlock is MyWarhead)
						{
							m_damagedBlocks[mySlimBlock] = 1E+07f;
							continue;
						}
						float num = (float)(mySlimBlock.WorldAABB.Center - m_explosion.Center).Length();
						if (value.DamageRemaining > 0f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							m_damagedBlocks.Add(mySlimBlock, value.DamageRemaining * num2 * mySlimBlock.DeformationRatio);
							value.DamageRemaining = Math.Max(0f, value.DamageRemaining * num2 - mySlimBlock.Integrity / mySlimBlock.DeformationRatio);
						}
						else
						{
							m_damagedBlocks.Add(mySlimBlock, value.DamageRemaining);
						}
					}
					else
					{
						value.DamageRemaining = 0f;
					}
					value.DistanceToExplosion = Math.Abs(num);
					m_damageRemaining.Add(mySlimBlock, value);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		/// <summary>
		/// Used to calculate damage for entities that are not grids
		/// Can be called even if ComputeDamagedBlocks was not called before, but it doesn't do any caching
		/// </summary>
		/// <param name="worldPosition">World position from where the cast starts (usually the entity position)</param>
		/// <returns></returns>
		public MyRaycastDamageInfo ComputeDamageForEntity(Vector3D worldPosition)
		{
			return new MyRaycastDamageInfo(m_explosionDamage, (float)(worldPosition - m_explosion.Center).Length());
		}

		/// <summary>
		/// Performs a grid raycast (is prone to aliasing effects).
		/// It can be recursive (it calls CastPhysicsRay when exiting the grid or when it hits an empty cell).
		/// </summary>
		/// <param name="cubeBlock">Starting block</param>
		/// <returns>Returns starting damage for current stack</returns>
		private MyRaycastDamageInfo CastDDA(MySlimBlock cubeBlock)
		{
			if (m_damageRemaining.ContainsKey(cubeBlock))
			{
				return m_damageRemaining[cubeBlock];
			}
			stackOverflowGuard = 0;
			m_castBlocks.Push(cubeBlock);
			cubeBlock.ComputeWorldCenter(out var worldCenter);
			m_cells.Clear();
			cubeBlock.CubeGrid.RayCastCells(worldCenter, m_explosion.Center, m_cells);
			(m_explosion.Center - worldCenter).Normalize();
			foreach (Vector3I cell in m_cells)
			{
				Vector3D vector3D = Vector3D.Transform(cell * cubeBlock.CubeGrid.GridSize, cubeBlock.CubeGrid.WorldMatrix);
				_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_DDA_RAYCASTS;
				MySlimBlock cubeBlock2 = cubeBlock.CubeGrid.GetCubeBlock(cell);
				if (cubeBlock2 == null)
				{
					if (IsExplosionInsideCell(cell, cubeBlock.CubeGrid))
					{
						return new MyRaycastDamageInfo(m_explosionDamage, (float)(vector3D - m_explosion.Center).Length());
					}
					return CastPhysicsRay(vector3D);
				}
				if (cubeBlock2 != cubeBlock)
				{
					if (m_damageRemaining.ContainsKey(cubeBlock2))
					{
						return m_damageRemaining[cubeBlock2];
					}
					if (!m_castBlocks.Contains(cubeBlock2))
					{
						m_castBlocks.Push(cubeBlock2);
					}
				}
				else if (IsExplosionInsideCell(cell, cubeBlock.CubeGrid))
				{
					return new MyRaycastDamageInfo(m_explosionDamage, (float)(vector3D - m_explosion.Center).Length());
				}
			}
			return new MyRaycastDamageInfo(m_explosionDamage, (float)(worldCenter - m_explosion.Center).Length());
		}

		private bool IsExplosionInsideCell(Vector3I cell, MyCubeGrid cellGrid)
		{
			return cellGrid.WorldToGridInteger(m_explosion.Center) == cell;
		}

		/// <summary>
		/// Performes a physics raycast
		/// It can be recursive (it calls CastDDA when it hits a grid).
		/// </summary>
		/// <param name="fromWorldPos"></param>
		/// <returns>Returns starting damage for current stack</returns>
		private MyRaycastDamageInfo CastPhysicsRay(Vector3D fromWorldPos)
		{
			Vector3D position = Vector3D.Zero;
			IMyEntity myEntity = null;
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(fromWorldPos, m_explosion.Center, 29);
			if (hitInfo.HasValue)
			{
				myEntity = ((hitInfo.Value.HkHitInfo.Body.UserObject != null) ? ((MyPhysicsBody)hitInfo.Value.HkHitInfo.Body.UserObject).Entity : null);
				position = hitInfo.Value.Position;
			}
			Vector3D normal = m_explosion.Center - fromWorldPos;
			float distanceToExplosion = (float)normal.Normalize();
			MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				MyCubeBlock myCubeBlock = myEntity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeGrid = myCubeBlock.CubeGrid;
				}
			}
			if (myCubeGrid != null)
			{
				Vector3D vector3D = Vector3D.Transform(position, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) * myCubeGrid.GridSizeR;
				Vector3D vector3D2 = Vector3D.TransformNormal(normal, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) * 1.0 / 8.0;
				for (int i = 0; i < 5; i++)
				{
					Vector3I pos = Vector3I.Round(vector3D);
					MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(pos);
					if (cubeBlock != null)
					{
						if (m_castBlocks.Contains(cubeBlock))
						{
							return new MyRaycastDamageInfo(0f, distanceToExplosion);
						}
						return CastDDA(cubeBlock);
					}
					vector3D += vector3D2;
				}
				position = Vector3D.Transform(vector3D * myCubeGrid.GridSize, myCubeGrid.WorldMatrix);
				Vector3D min = Vector3D.Min(fromWorldPos, position);
				Vector3D max = Vector3D.Max(fromWorldPos, position);
				if (new BoundingBoxD(min, max).Contains(m_explosion.Center) == ContainmentType.Contains)
				{
					return new MyRaycastDamageInfo(m_explosionDamage, distanceToExplosion);
				}
				stackOverflowGuard++;
				if (stackOverflowGuard > 10)
				{
					_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS;
					return new MyRaycastDamageInfo(0f, distanceToExplosion);
				}
				_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS;
				return CastPhysicsRay(position);
			}
			if (hitInfo.HasValue)
			{
				_ = MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS;
				return new MyRaycastDamageInfo(0f, distanceToExplosion);
			}
			return new MyRaycastDamageInfo(m_explosionDamage, distanceToExplosion);
		}

		[Conditional("DEBUG")]
		private void DrawRay(Vector3D from, Vector3D to, float damage, bool depthRead = true)
		{
			if (!(damage > 0f))
			{
				_ = Color.Blue;
			}
			else
			{
				Color.Lerp(Color.Green, Color.Red, damage / m_explosionDamage);
			}
		}

		[Conditional("DEBUG")]
		private void DrawRay(Vector3D from, Vector3D to, Color color, bool depthRead = true)
		{
			if (MyAlexDebugInputComponent.Static != null)
			{
				MyAlexDebugInputComponent.Static.AddDebugLine(new MyAlexDebugInputComponent.LineInfo(from, to, color, depthRead: false));
			}
		}
	}
}
