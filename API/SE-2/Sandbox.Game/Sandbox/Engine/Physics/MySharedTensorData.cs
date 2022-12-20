using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	internal class MySharedTensorData : IGroupData<MyCubeGrid>
	{
		public MyGroups<MyCubeGrid, MySharedTensorData>.Group m_group { get; set; }

		public void OnNodeAdded<TGroupData>(MyCubeGrid grid, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			MarkDirty();
			MarkGridTensorDirty(grid);
		}

		public void OnPostNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnNodeRemoved<TGroupData>(MyCubeGrid grid, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			MarkDirty();
			MarkGridTensorDirty(grid);
		}

		public void OnPreNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void MarkDirty()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyGroups<MyCubeGrid, MySharedTensorData>.Node> enumerator = m_group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MarkGridTensorDirty(enumerator.get_Current().NodeData);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public static void MarkGridTensorDirty(MyCubeGrid grid)
		{
			grid.Physics?.Shape.MarkSharedTensorDirty();
		}

		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			m_group = group as MyGroups<MyCubeGrid, MySharedTensorData>.Group;
<<<<<<< HEAD
		}

		public void OnPostCreate()
		{
		}

		public void OnPreRelease()
		{
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void OnRelease()
		{
			m_group = null;
		}
	}
}
