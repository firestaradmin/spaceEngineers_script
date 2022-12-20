using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	public class MyGridTargeting : MyEntityComponentBase
	{
		[Flags]
		private enum RelationFiltering
		{
			None = 0x0,
			Enemy = 0x2,
			Neutral = 0x4,
			Friend = 0x8,
			All = 0xE
		}

		private class Sandbox_Game_EntityComponents_MyGridTargeting_003C_003EActor : IActivator, IActivator<MyGridTargeting>
		{
			private sealed override object CreateInstance()
			{
				return new MyGridTargeting();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGridTargeting CreateInstance()
			{
				return new MyGridTargeting();
			}

			MyGridTargeting IActivator<MyGridTargeting>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyCubeGrid m_grid;

		private BoundingSphere m_queryLocal = new BoundingSphere(Vector3.Zero, float.MinValue);

		private List<MyEntity> m_targetRoots = new List<MyEntity>();

		private MyListDictionary<MyCubeGrid, MyEntity> m_targetBlocks = new MyListDictionary<MyCubeGrid, MyEntity>();

		private List<long> m_ownersB = new List<long>();

		private List<long> m_ownersA = new List<long>();

		private FastResourceLock m_scanLock = new FastResourceLock();

		private int m_lastScan;

		public bool AllowScanning = true;

		private RelationFiltering m_currentRelationFilter;

		public FastResourceLock ScanLock => m_scanLock;

		public List<MyEntity> TargetRoots
		{
			get
			{
				if (AllowScanning && MySession.Static.GameplayFrameCounter - m_lastScan > 100)
				{
					Scan();
				}
				return m_targetRoots;
			}
		}

		public MyListDictionary<MyCubeGrid, MyEntity> TargetBlocks
		{
			get
			{
				if (AllowScanning && MySession.Static.GameplayFrameCounter - m_lastScan > 100)
				{
					Scan();
				}
				return m_targetBlocks;
			}
		}

		public override string ComponentTypeDebugString => "MyGridTargeting";

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_grid = base.Entity as MyCubeGrid;
			m_grid.OnBlockAdded += m_grid_OnBlockAdded;
		}

		private void m_grid_OnBlockAdded(MySlimBlock obj)
		{
			IMyTargetingReceiver myTargetingReceiver = obj.FatBlock as IMyTargetingReceiver;
			if (myTargetingReceiver != null)
			{
<<<<<<< HEAD
				m_queryLocal.Include(new BoundingSphere(obj.FatBlock.PositionComp.LocalMatrixRef.Translation, myTargetingReceiver.SearchRange));
				myTargetingReceiver.AddPropertiesChangedCallback(TurretOnPropertiesChanged);
=======
				m_queryLocal.Include(new BoundingSphere(obj.FatBlock.PositionComp.LocalMatrixRef.Translation, myLargeTurretBase.SearchingRange));
				myLargeTurretBase.PropertiesChanged += TurretOnPropertiesChanged;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void TurretOnPropertiesChanged(MyTerminalBlock obj)
		{
			IMyTargetingReceiver myTargetingReceiver = obj as IMyTargetingReceiver;
			if (myTargetingReceiver != null)
			{
<<<<<<< HEAD
				m_queryLocal.Include(new BoundingSphere(obj.PositionComp.LocalMatrixRef.Translation, myTargetingReceiver.SearchRange));
=======
				m_queryLocal.Include(new BoundingSphere(obj.PositionComp.LocalMatrixRef.Translation, myLargeTurretBase.SearchingRange));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void ProcessGrid(MyCubeGrid grid, float range, ref List<MyEntity> output)
		{
			BoundingSphereD query = new BoundingSphereD(Vector3D.Transform(m_queryLocal.Center, m_grid.WorldMatrix), range);
			m_ownersA.AddRange(m_grid.SmallOwners);
			m_ownersA.AddRange(m_grid.BigOwners);
			ProcessGrid(grid, ref m_ownersA, ref query, ref output);
			m_ownersA.Clear();
		}

		public bool IsValidTarget(long owner, long other)
		{
			if (m_currentRelationFilter == RelationFiltering.All)
			{
<<<<<<< HEAD
				return true;
			}
			if (m_currentRelationFilter == RelationFiltering.None)
			{
				return false;
			}
			MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(owner, other);
			if ((relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.Neutral || relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.NoOwnership) && (m_currentRelationFilter & RelationFiltering.Neutral) != 0)
			{
				return true;
			}
			if ((relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.Owner || relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.FactionShare || relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.Friends) && (m_currentRelationFilter & RelationFiltering.Friend) != 0)
			{
				return true;
			}
			if (relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.Enemies && (m_currentRelationFilter & RelationFiltering.Enemy) != 0)
			{
				return true;
			}
			return false;
		}

		public void ProcessGrid(MyCubeGrid grid, ref List<long> owners, ref BoundingSphereD query, ref List<MyEntity> output)
		{
			if (grid == null || (grid.Physics != null && !grid.Physics.Enabled))
			{
				return;
			}
			bool flag = false;
			if (m_currentRelationFilter == RelationFiltering.All)
			{
				flag = true;
			}
			else if (grid.BigOwners.Count == 0 && grid.SmallOwners.Count == 0)
			{
				foreach (long item in m_ownersA)
				{
					if (MyIDModule.GetRelationPlayerBlock(item, 0L) == MyRelationsBetweenPlayerAndBlock.NoOwnership)
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				m_ownersB.AddRange(grid.BigOwners);
				m_ownersB.AddRange(grid.SmallOwners);
				foreach (long item2 in m_ownersA)
				{
					foreach (long item3 in m_ownersB)
					{
						if (IsValidTarget(item2, item3))
						{
							flag = true;
							break;
=======
				if (!AllowScanning || MySession.Static.GameplayFrameCounter - m_lastScan <= 100)
				{
					return;
				}
				BoundingSphereD sphere = new BoundingSphereD(Vector3D.Transform(m_queryLocal.Center, m_grid.WorldMatrix), m_queryLocal.Radius);
				m_targetRoots.Clear();
				m_targetBlocks.Clear();
				MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_targetRoots);
				MyMissiles.GetAllMissilesInSphere(ref sphere, m_targetRoots);
				int count = m_targetRoots.Count;
				m_ownersA.AddRange(m_grid.SmallOwners);
				m_ownersA.AddRange(m_grid.BigOwners);
				for (int i = 0; i < count; i++)
				{
					MyCubeGrid myCubeGrid = m_targetRoots[i] as MyCubeGrid;
					if (myCubeGrid == null || (myCubeGrid.Physics != null && !myCubeGrid.Physics.Enabled))
					{
						continue;
					}
					bool flag = false;
					if (myCubeGrid.BigOwners.Count == 0 && myCubeGrid.SmallOwners.Count == 0)
					{
						foreach (long item in m_ownersA)
						{
							if (MyIDModule.GetRelationPlayerBlock(item, 0L) == MyRelationsBetweenPlayerAndBlock.NoOwnership)
							{
								flag = true;
								break;
							}
						}
					}
					else
					{
						m_ownersB.AddRange(myCubeGrid.BigOwners);
						m_ownersB.AddRange(myCubeGrid.SmallOwners);
						foreach (long item2 in m_ownersA)
						{
							foreach (long item3 in m_ownersB)
							{
								if (MyIDModule.GetRelationPlayerBlock(item2, item3) == MyRelationsBetweenPlayerAndBlock.Enemies)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
						m_ownersB.Clear();
					}
					if (flag)
					{
						List<MyEntity> orAdd = m_targetBlocks.GetOrAdd(myCubeGrid);
						using (myCubeGrid.Pin())
						{
							if (!myCubeGrid.MarkedForClose)
							{
								myCubeGrid.Hierarchy.QuerySphere(ref sphere, orAdd);
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						continue;
					}
<<<<<<< HEAD
					if (flag)
					{
						break;
					}
				}
				m_ownersB.Clear();
			}
			if (!flag)
			{
				foreach (MyCubeBlock fatBlock in grid.GetFatBlocks())
				{
					IMyComponentOwner<MyIDModule> myComponentOwner = fatBlock as IMyComponentOwner<MyIDModule>;
					if (myComponentOwner == null || !myComponentOwner.GetComponent(out var _))
					{
						continue;
					}
					long ownerId = fatBlock.OwnerId;
					foreach (long item4 in m_ownersA)
					{
						if (IsValidTarget(item4, ownerId))
						{
							flag = true;
							break;
=======
					foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
					{
						IMyComponentOwner<MyIDModule> myComponentOwner = fatBlock as IMyComponentOwner<MyIDModule>;
						if (myComponentOwner == null || !myComponentOwner.GetComponent(out var _))
						{
							continue;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						long ownerId = fatBlock.OwnerId;
						foreach (long item4 in m_ownersA)
						{
							if (MyIDModule.GetRelationPlayerBlock(item4, ownerId) == MyRelationsBetweenPlayerAndBlock.Enemies)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						List<MyEntity> orAdd2 = m_targetBlocks.GetOrAdd(myCubeGrid);
						if (!myCubeGrid.Closed)
						{
							myCubeGrid.Hierarchy.QuerySphere(ref sphere, orAdd2);
						}
					}
				}
				m_ownersA.Clear();
				for (int num = m_targetRoots.Count - 1; num >= 0; num--)
				{
					MyEntity myEntity = m_targetRoots[num];
					if (myEntity is MyDebrisBase || myEntity is MyFloatingObject || (myEntity.Physics != null && !myEntity.Physics.Enabled) || myEntity.GetTopMostParent().Physics == null || !myEntity.GetTopMostParent().Physics.Enabled)
					{
						m_targetRoots.RemoveAtFast(num);
					}
<<<<<<< HEAD
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			using (grid.Pin())
			{
				if (!grid.MarkedForClose)
				{
					grid.Hierarchy.QuerySphere(ref query, output);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_lastScan = MySession.Static.GameplayFrameCounter;
			}
		}

		private void Scan()
		{
			using (m_scanLock.AcquireExclusiveUsing())
			{
				if (!AllowScanning || MySession.Static.GameplayFrameCounter - m_lastScan <= 100)
				{
					return;
				}
				BoundingSphereD sphere = new BoundingSphereD(Vector3D.Transform(m_queryLocal.Center, m_grid.WorldMatrix), m_queryLocal.Radius);
				m_targetRoots.Clear();
				m_targetBlocks.Clear();
				MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_targetRoots);
				MyMissiles.Static.GetAllMissilesInSphere(ref sphere, m_targetRoots);
				int count = m_targetRoots.Count;
				m_ownersA.AddRange(m_grid.SmallOwners);
				m_ownersA.AddRange(m_grid.BigOwners);
				for (int i = 0; i < count; i++)
				{
					MyCubeGrid myCubeGrid = m_targetRoots[i] as MyCubeGrid;
					if (myCubeGrid != null)
					{
						List<MyEntity> output = m_targetBlocks.GetOrAdd(myCubeGrid);
						ProcessGrid(myCubeGrid, ref m_ownersA, ref sphere, ref output);
					}
				}
				m_ownersA.Clear();
				for (int num = m_targetRoots.Count - 1; num >= 0; num--)
				{
					MyEntity myEntity = m_targetRoots[num];
					if (myEntity is MyDebrisBase || myEntity is MyFloatingObject || (myEntity.Physics != null && !myEntity.Physics.Enabled) || myEntity.GetTopMostParent().Physics == null || !myEntity.GetTopMostParent().Physics.Enabled)
					{
						m_targetRoots.RemoveAtFast(num);
					}
				}
				m_lastScan = MySession.Static.GameplayFrameCounter;
			}
		}

		private static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
		{
			if (!potentialDescendant.IsSubclassOf(potentialBase))
			{
				return potentialDescendant == potentialBase;
			}
			return true;
		}

		public void RescanIfNeeded()
		{
			if (AllowScanning && MySession.Static.GameplayFrameCounter - m_lastScan > 100)
			{
				Scan();
			}
		}

		internal void SetRelationFlags(bool targetEnemies, bool targetNeutrals, bool targetFriends)
		{
			m_currentRelationFilter = RelationFiltering.None;
			if (targetEnemies)
			{
				m_currentRelationFilter |= RelationFiltering.Enemy;
			}
			if (targetNeutrals)
			{
				m_currentRelationFilter |= RelationFiltering.Neutral;
			}
			if (targetFriends)
			{
				m_currentRelationFilter |= RelationFiltering.Friend;
			}
		}
	}
}
