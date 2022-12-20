using System;

namespace VRage.Game.ModAPI
{
	public abstract class MyGridGroupsDefaultEventHandler
	{
		private static int MAX_ID;

		private int m_id;

		protected IMyGridGroupData GridGroup { get; private set; }

		public bool IsClosed { get; private set; }

		protected abstract Guid GetGuid();

		protected MyGridGroupsDefaultEventHandler(IMyGridGroupData obj)
		{
			m_id = ++MAX_ID;
			GridGroup = obj;
			obj.SetVariable(GetGuid(), this);
			obj.OnGridAdded += OnGridAdd;
			obj.OnGridRemoved += OnGridRemove;
			obj.OnReleased += OnRelease;
		}

		protected virtual void OnGridMerged(IMyCubeGrid baseGrid, IMyCubeGrid merged)
		{
		}

		protected virtual void OnGridSplited(IMyCubeGrid basegrid, IMyCubeGrid removedGrid)
		{
		}

		protected virtual void OnReleased()
		{
		}

		protected virtual void OnGridRemoved(IMyCubeGrid arg2, IMyGridGroupData nextGroup)
		{
		}

		protected virtual void OnGridAdded(IMyCubeGrid arg2, IMyGridGroupData prevGroup)
		{
		}

		private void OnGridMerge(IMyCubeGrid arg1, IMyCubeGrid arg2)
		{
			OnGridMerged(arg1, arg2);
		}

		private void OnGridSplit(IMyCubeGrid arg1, IMyCubeGrid arg2)
		{
			OnGridSplited(arg1, arg2);
		}

		private void OnRelease(IMyGridGroupData arg1)
		{
			IsClosed = true;
			arg1.OnGridAdded -= OnGridAdd;
			arg1.OnGridRemoved -= OnGridRemove;
			arg1.OnReleased -= OnRelease;
			OnReleased();
		}

		private void OnGridRemove(IMyGridGroupData arg1, IMyCubeGrid arg2, IMyGridGroupData arg3)
		{
			arg2.OnGridSplit -= OnGridSplit;
			arg2.OnGridMerge -= OnGridMerge;
			OnGridRemoved(arg2, arg3);
		}

		private void OnGridAdd(IMyGridGroupData arg1, IMyCubeGrid arg2, IMyGridGroupData arg3)
		{
			arg2.OnGridSplit -= OnGridSplit;
			arg2.OnGridMerge -= OnGridMerge;
			arg2.OnGridSplit += OnGridSplit;
			arg2.OnGridMerge += OnGridMerge;
			OnGridAdded(arg2, arg3);
		}

		public override string ToString()
		{
			return $"GridGroup#{m_id} isClosed={IsClosed}";
		}
	}
}
