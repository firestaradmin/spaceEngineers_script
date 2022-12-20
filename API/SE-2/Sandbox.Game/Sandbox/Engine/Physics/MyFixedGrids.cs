<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Threading;
using Sandbox.Game.Entities;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyFixedGrids : MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>, IMySceneComponent
	{
		public class MyFixedGridsGroupData : IGroupData<MyCubeGrid>
		{
			private Group m_group;

			private int m_rootedGrids;

			public bool IsRooted => m_rootedGrids > 0;

			public void OnNodeAdded<TGroupData>(MyCubeGrid grid, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
			{
				bool flag = false;
				if (Static.m_roots.Contains(grid))
				{
					OnRootAdded();
					flag = true;
				}
				if (flag | (m_rootedGrids != 0))
				{
					ConvertGrid(grid, @static: true);
				}
			}

			public void OnPostNodeAdded<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
			{
			}

			public void OnNodeRemoved<TGroupData>(MyCubeGrid grid, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
			{
				if (Static.m_roots.Contains(grid))
				{
					OnRootRemoved();
				}
				else if (m_rootedGrids != 0)
				{
					ConvertGrid(grid, @static: false);
				}
			}

			public void OnPreNodeRemoved<TGroupData>(MyCubeGrid entity, TGroupData prevGroup) where TGroupData : IGroupData<MyCubeGrid>, new()
			{
			}

			public void OnRootAdded()
			{
				if (m_rootedGrids++ == 0)
				{
					Convert(@static: true);
				}
			}

			public void OnRootRemoved()
			{
				if (--m_rootedGrids == 0)
				{
					Convert(@static: false);
				}
			}

			private void Convert(bool @static)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				Enumerator<Node> enumerator = m_group.Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ConvertGrid(enumerator.get_Current().NodeData, @static);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}

			public static void ConvertGrid(MyCubeGrid grid, bool @static)
			{
				grid.IsMarkedForEarlyDeactivation = @static;
			}

			public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
			{
				m_group = group as Group;
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

			public static bool MajorSelector(Group major, Group minor)
			{
				bool num = major.GroupData.m_rootedGrids > 0;
				bool flag = minor.GroupData.m_rootedGrids > 0;
				if (num)
				{
					if (!flag)
					{
						return true;
					}
				}
				else if (flag)
				{
					return false;
				}
				return major.Nodes.Count >= minor.Nodes.Count;
			}
		}

		private static MyFixedGrids m_static;

		private HashSet<MyCubeGrid> m_roots = new HashSet<MyCubeGrid>();

		private static MyFixedGrids Static => m_static;

		public MyFixedGrids()
			: base(supportOphrans: false, (MajorGroupComparer)MyFixedGridsGroupData.MajorSelector)
		{
			base.SupportsChildToChild = true;
		}

		public void Load()
		{
			m_static = this;
		}

		public void Unload()
		{
			m_static = null;
		}

		private static void AssertThread()
		{
			_ = MySandboxGame.Static.UpdateThread;
			Thread.get_CurrentThread();
		}

		public static void MarkGridRoot(MyCubeGrid grid)
		{
			AssertThread();
			if (Static.m_roots.Add(grid))
			{
				Group group = Static.GetGroup(grid);
				if (group == null)
				{
					MyFixedGridsGroupData.ConvertGrid(grid, @static: true);
				}
				else
				{
					group.GroupData.OnRootAdded();
				}
			}
		}

		public static void UnmarkGridRoot(MyCubeGrid grid)
		{
			AssertThread();
			if (Static.m_roots.Remove(grid))
			{
				Group group = Static.GetGroup(grid);
				if (group == null)
				{
					MyFixedGridsGroupData.ConvertGrid(grid, @static: false);
				}
				else
				{
					group.GroupData.OnRootRemoved();
				}
			}
		}

		public static void Link(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
		{
			AssertThread();
			Static.CreateLink(linkingBlock.EntityId, parent, child);
		}

		public static void BreakLink(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
		{
			AssertThread();
			Static.BreakLink(linkingBlock.EntityId, parent, child);
		}

		public static bool IsRooted(MyCubeGrid grid)
		{
			if (Static.m_roots.Contains(grid))
			{
				return true;
			}
			return Static.GetGroup(grid)?.GroupData.IsRooted ?? false;
		}
	}
}
