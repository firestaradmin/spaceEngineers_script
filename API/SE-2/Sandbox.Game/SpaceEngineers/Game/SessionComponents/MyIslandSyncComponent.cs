<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyIslandSyncComponent : MySessionComponentBase
	{
		public struct IslandData
		{
			public HashSet<IMyEntity> RootEntities;

			public BoundingBoxD AABB;

			public Dictionary<ulong, float> ClientPriority;
		}

		protected static Color[] m_colors = new Color[19]
		{
			new Color(0, 192, 192),
			Color.Orange,
			Color.BlueViolet * 1.5f,
			Color.BurlyWood,
			Color.Chartreuse,
			Color.CornflowerBlue,
			Color.Cyan,
			Color.ForestGreen,
			Color.Fuchsia,
			Color.Gold,
			Color.GreenYellow,
			Color.LightBlue,
			Color.LightGreen,
			Color.LimeGreen,
			Color.Magenta,
			Color.MintCream,
			Color.Orchid,
			Color.PeachPuff,
			Color.Purple
		};

		public static MyIslandSyncComponent Static = null;

		private List<HkRigidBody> m_rigidBodies = new List<HkRigidBody>();

		private List<IslandData> m_rootIslands = new List<IslandData>();

		private Dictionary<IMyEntity, int> m_rootEntityIslandIndex = new Dictionary<IMyEntity, int>();

		public override bool IsRequiredByGame => MyFakes.MP_ISLANDS;

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			MyPositionComponent.SynchronizationEnabled = false;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public override void UpdateAfterSimulation()
		{
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateAfterSimulation();
			m_rootIslands.Clear();
			m_rootEntityIslandIndex.Clear();
			if (!MyPhysics.GetClusterList().HasValue)
			{
				return;
			}
			foreach (HkWorld item2 in MyPhysics.GetClusterList().Value)
			{
				int activeSimulationIslandsCount = item2.GetActiveSimulationIslandsCount();
				for (int i = 0; i < activeSimulationIslandsCount; i++)
				{
					item2.GetActiveSimulationIslandRigidBodies(i, m_rigidBodies);
<<<<<<< HEAD
					HashSet<IMyEntity> hashSet = null;
=======
					HashSet<IMyEntity> val = null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					foreach (HkRigidBody rigidBody in m_rigidBodies)
					{
						List<IMyEntity> allEntities = rigidBody.GetAllEntities();
						foreach (IMyEntity item3 in allEntities)
						{
							IMyEntity topMostParent = item3.GetTopMostParent();
							foreach (IslandData rootIsland in m_rootIslands)
							{
								if (rootIsland.RootEntities.Contains(topMostParent))
								{
<<<<<<< HEAD
									hashSet = rootIsland.RootEntities;
=======
									val = rootIsland.RootEntities;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
									break;
								}
							}
						}
						allEntities.Clear();
					}
<<<<<<< HEAD
					if (hashSet == null)
=======
					if (val == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						IslandData islandData = default(IslandData);
						islandData.AABB = BoundingBoxD.CreateInvalid();
						islandData.RootEntities = new HashSet<IMyEntity>();
						islandData.ClientPriority = new Dictionary<ulong, float>();
						IslandData item = islandData;
<<<<<<< HEAD
						hashSet = item.RootEntities;
=======
						val = item.RootEntities;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						m_rootIslands.Add(item);
					}
					foreach (HkRigidBody rigidBody2 in m_rigidBodies)
					{
						List<IMyEntity> allEntities2 = rigidBody2.GetAllEntities();
						foreach (IMyEntity item4 in allEntities2)
						{
							IMyEntity topMostParent2 = item4.GetTopMostParent();
<<<<<<< HEAD
							hashSet.Add(topMostParent2);
=======
							val.Add(topMostParent2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						allEntities2.Clear();
					}
					m_rigidBodies.Clear();
				}
			}
			for (int j = 0; j < m_rootIslands.Count; j++)
			{
				IslandData value = m_rootIslands[j];
				value.AABB = BoundingBoxD.CreateInvalid();
<<<<<<< HEAD
				foreach (IMyEntity rootEntity in value.RootEntities)
				{
					value.AABB.Include(rootEntity.PositionComp.WorldAABB);
					m_rootEntityIslandIndex[rootEntity] = j;
				}
=======
				Enumerator<IMyEntity> enumerator5 = value.RootEntities.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						IMyEntity current2 = enumerator5.get_Current();
						value.AABB.Include(current2.PositionComp.WorldAABB);
						m_rootEntityIslandIndex[current2] = j;
					}
				}
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_rootIslands[j] = value;
			}
		}

		protected static Color IndexToColor(int index)
		{
			return m_colors[index % m_colors.Length];
		}

		public override void Draw()
		{
			base.Draw();
			int num = 0;
			foreach (IslandData rootIsland in m_rootIslands)
			{
				MyRenderProxy.DebugDrawAABB(rootIsland.AABB, IndexToColor(num));
				string text = "Island " + num + " : " + rootIsland.RootEntities.get_Count() + " root entities. Priorities: ";
				int num2 = 0;
				foreach (KeyValuePair<ulong, float> item in rootIsland.ClientPriority)
				{
					text = text + "Client" + num2 + ": " + item.Value;
					num2++;
				}
				MyRenderProxy.DebugDrawText2D(new Vector2(100f, num * 15), text, IndexToColor(num), 0.7f);
				num++;
			}
		}

		public bool GetIslandAABBForEntity(IMyEntity entity, out BoundingBoxD aabb)
		{
			aabb = BoundingBoxD.CreateInvalid();
			if (m_rootEntityIslandIndex.TryGetValue(entity, out var value))
			{
				aabb = m_rootIslands[value].AABB;
				return true;
			}
			return false;
		}

		public void SetPriorityForIsland(IMyEntity entity, ulong client, float priority)
		{
			if (m_rootEntityIslandIndex.TryGetValue(entity, out var value))
			{
				m_rootIslands[value].ClientPriority[client] = priority;
			}
		}
	}
}
