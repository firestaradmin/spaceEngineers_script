using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Havok;
<<<<<<< HEAD
using Sandbox.Game.Entities.Cube;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.GameSystems;
using VRage;
using VRage.Collections;
using VRage.Game.ModAPI;
using VRage.Groups;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	public class MyGridPhysicalGroupData : MyGridGroupData<MyGridPhysicalGroupData>
	{
		public struct GroupSharedPxProperties
		{
			public readonly int GridCount;

			public readonly MyCubeGrid ReferenceGrid;

			public readonly HkMassProperties PxProperties;

			/// <summary>
			/// Local to reference-grid
			/// </summary>
			public Matrix InertiaTensor => PxProperties.InertiaTensor;

			public float Mass => PxProperties.Mass;

			public Vector3D CoMWorld
			{
				get
				{
					Vector3 position = PxProperties.CenterOfMass;
					MatrixD matrix = ReferenceGrid.WorldMatrix;
					Vector3D.Transform(ref position, ref matrix, out var result);
					return result;
				}
			}

			public GroupSharedPxProperties(MyCubeGrid referenceGrid, HkMassProperties sharedProperties, int gridCount)
			{
				GridCount = gridCount;
				ReferenceGrid = referenceGrid;
				PxProperties = sharedProperties;
			}

			public Matrix GetInertiaTensorLocalToGrid(MyCubeGrid localGrid)
			{
				Matrix m = InertiaTensor;
				MatrixD m2 = m;
				if (ReferenceGrid != localGrid)
				{
					MatrixD matrix = ReferenceGrid.WorldMatrix * localGrid.PositionComp.WorldMatrixNormalizedInv;
					MatrixD.Multiply(ref m2, ref matrix, out m2);
				}
				return m2;
			}
		}

		private volatile Ref<GroupSharedPxProperties> m_groupPropertiesCache;

		internal readonly MyGroupControlSystem ControlSystem = new MyGroupControlSystem();

		public static GroupSharedPxProperties GetGroupSharedProperties(MyCubeGrid localGrid, bool checkMultithreading = true)
		{
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(localGrid);
			if (group == null)
			{
				HkMassProperties valueOrDefault = GetGridMassProperties(localGrid).GetValueOrDefault();
				return new GroupSharedPxProperties(localGrid, valueOrDefault, 1);
			}
			return group.GroupData.GetSharedPxProperties(localGrid);
		}

		public static void InvalidateSharedMassPropertiesCache(MyCubeGrid groupRepresentative)
		{
			MyCubeGridGroups.Static.Physical.GetGroup(groupRepresentative)?.GroupData.InvalidateCoMCache();
		}

		private GroupSharedPxProperties GetSharedPxProperties(MyCubeGrid referenceGrid)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			Ref<GroupSharedPxProperties> @ref = m_groupPropertiesCache;
			if (@ref == null)
			{
				HashSetReader<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> nodes = m_group.Nodes;
				MatrixD worldMatrixNormalizedInv = referenceGrid.PositionComp.WorldMatrixNormalizedInv;
				int count = nodes.Count;
				HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(count);
				Span<HkMassElement> elements = new Span<HkMassElement>(array, 0, count);
				int length = 0;
				Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
<<<<<<< HEAD
						MatrixD m = nodeData.PositionComp.WorldMatrixRef * worldMatrixNormalizedInv;
						elements[length++] = new HkMassElement
=======
						MyCubeGrid nodeData = enumerator.get_Current().NodeData;
						HkMassProperties? gridMassProperties = GetGridMassProperties(nodeData);
						if (gridMassProperties.HasValue)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							MatrixD m = nodeData.PositionComp.WorldMatrixRef * worldMatrixNormalizedInv;
							elements[length++] = new HkMassElement
							{
								Tranform = m,
								Properties = gridMassProperties.Value
							};
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				elements = elements.Slice(0, length);
				HkInertiaTensorComputer.CombineMassProperties(elements, out var massProperties);
				ArrayPool<HkMassElement>.Shared.Return(array);
				@ref = (m_groupPropertiesCache = Ref.Create(new GroupSharedPxProperties(referenceGrid, massProperties, nodes.Count)));
			}
			return @ref.Value;
		}

		private static void DrawDebugSphere(MyCubeGrid referenceGrid, Color color, Vector3 localPosition, double radius)
		{
			MyRenderProxy.DebugDrawSphere(Vector3D.Transform(localPosition, referenceGrid.PositionComp.WorldMatrixRef), (float)radius, color, 1f, depthRead: false);
		}

		private void InvalidateCoMCache()
		{
			m_groupPropertiesCache = null;
		}

		private static HkMassProperties? GetGridMassProperties(MyCubeGrid grid)
		{
			if (grid.Physics == null)
			{
				return null;
			}
			return grid.Physics.Shape.MassProperties;
		}

		public MyGridPhysicalGroupData()
		{
			base.LinkType = GridLinkTypeEnum.Physical;
		}

		public override void OnNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup)
		{
			InvalidateCoMCache();
			entity.OnAddedToGroup(this);
			base.OnNodeAdded(entity, prevGroup);
		}

		public override void OnNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData prevGroup)
		{
			base.OnNodeRemoved(entity, prevGroup);
			InvalidateCoMCache();
			entity.OnRemovedFromGroup(this);
		}

		internal static bool IsMajorGroup(MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group a, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group b)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f;
			Enumerator<MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> enumerator = a.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current = enumerator.get_Current();
					if (current.NodeData.Physics != null)
					{
						num += current.NodeData.PositionComp.LocalVolume.Radius;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = b.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node current2 = enumerator.get_Current();
					if (current2.NodeData.Physics != null)
					{
						num -= current2.NodeData.PositionComp.LocalVolume.Radius;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return num > 0f;
		}

<<<<<<< HEAD
		public override void OnRelease()
=======
		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			m_group = group as MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group;
		}

		public void OnRelease()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			base.OnRelease();
			m_group = null;
			m_groupPropertiesCache = null;
			ControlSystem.Clear();
		}

		[DebuggerStepThrough]
		[Conditional("DEBUG")]
		private static void AssertThread()
		{
			_ = MySandboxGame.Static.UpdateThread;
			Thread.get_CurrentThread();
		}
	}
}
