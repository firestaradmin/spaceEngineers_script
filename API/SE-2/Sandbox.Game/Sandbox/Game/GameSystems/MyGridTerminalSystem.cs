using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGridTerminalSystem : Sandbox.ModAPI.IMyGridTerminalSystem, Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
	{
		private readonly MyGridLogicalGroupData m_gridLogicalGroupData;

		private readonly int m_oreDetectorCounterValue = 50;

		private readonly HashSet<MyTerminalBlock> m_blocks = new HashSet<MyTerminalBlock>();

		private readonly List<MyTerminalBlock> m_blockList = new List<MyTerminalBlock>();

		private readonly Dictionary<long, MyTerminalBlock> m_blockTable = new Dictionary<long, MyTerminalBlock>();

		private readonly List<MyBlockGroup> m_blockGroups = new List<MyBlockGroup>();

		private readonly HashSet<MyTerminalBlock> m_blocksForHud = new HashSet<MyTerminalBlock>();

		private List<string> m_debugChanges = new List<string>();

		private int m_lastHudIndex;

		private int m_oreDetectorUpdateCounter;

		private bool m_needsHudUpdate;

		private bool m_scheduled;

		public bool NeedsHudUpdate
		{
			get
			{
				return m_needsHudUpdate;
			}
			set
			{
				if (m_needsHudUpdate != value)
				{
					m_needsHudUpdate = value;
					if (value)
					{
						Schedule();
					}
					else
					{
						DeSchedule();
					}
				}
			}
		}

		public HashSetReader<MyTerminalBlock> Blocks => new HashSetReader<MyTerminalBlock>(m_blocks);

		public HashSetReader<MyTerminalBlock> HudBlocks => new HashSetReader<MyTerminalBlock>(m_blocksForHud);

		public List<MyBlockGroup> BlockGroups => m_blockGroups;

		public event Action<MyTerminalBlock> BlockAdded;

		public event Action<MyTerminalBlock> BlockRemoved;

		public event Action BlockManipulationFinished;

		/// <summary>
		/// Warning, on grid disconnects, you would need to unsubscribe and subscribe to new TerminalSystem 
		/// </summary>
		public event Action<MyBlockGroup> GroupAdded;

		/// <summary>
		/// Warning, on grid disconnects, you would need to unsubscribe and subscribe to new TerminalSystem 
		/// </summary>
		public event Action<MyBlockGroup> GroupRemoved;

<<<<<<< HEAD
		event Action<Sandbox.ModAPI.IMyBlockGroup> Sandbox.ModAPI.IMyGridTerminalSystem.GroupAdded
		{
			add
			{
				GroupAdded += GetDelegate(value);
			}
			remove
			{
				GroupAdded -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyBlockGroup> Sandbox.ModAPI.IMyGridTerminalSystem.GroupRemoved
		{
			add
			{
				GroupRemoved += GetDelegate(value);
			}
			remove
			{
				GroupRemoved -= GetDelegate(value);
			}
		}

		public MyGridTerminalSystem(MyGridLogicalGroupData gridLogicalGroupData)
		{
			m_gridLogicalGroupData = gridLogicalGroupData;
		}

		public void Schedule(MyCubeGrid root = null)
		{
			if (!m_scheduled)
			{
				root = root ?? m_gridLogicalGroupData.Root;
				if (root != null)
				{
					root.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, UpdateHud, 8);
					m_scheduled = true;
				}
			}
		}

		protected void DeSchedule(MyCubeGrid root = null)
		{
=======
		public MyGridTerminalSystem(MyGridLogicalGroupData gridLogicalGroupData)
		{
			m_gridLogicalGroupData = gridLogicalGroupData;
		}

		public void Schedule(MyCubeGrid root = null)
		{
			if (!m_scheduled)
			{
				root = root ?? m_gridLogicalGroupData.Root;
				if (root != null)
				{
					root.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, UpdateHud, 8);
					m_scheduled = true;
				}
			}
		}

		protected void DeSchedule(MyCubeGrid root = null)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			root = root ?? m_gridLogicalGroupData.Root;
			root?.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, UpdateHud);
			m_scheduled = false;
		}

		public void OnRootChanged(MyCubeGrid oldRoot, MyCubeGrid newRoot)
		{
			if (m_scheduled)
			{
				DeSchedule(oldRoot);
				if (newRoot != null)
				{
					Schedule(newRoot);
				}
			}
		}

		public void Add(MyTerminalBlock block)
		{
			if (!block.MarkedForClose && !block.IsBeingRemoved && !MyEntities.IsClosingAll && !m_blockTable.ContainsKey(block.EntityId))
			{
				m_blockTable.Add(block.EntityId, block);
				m_blocks.Add(block);
				m_blockList.Add(block);
				this.BlockAdded?.Invoke(block);
			}
		}

		public void Remove(MyTerminalBlock block)
		{
			if (block.MarkedForClose || MyEntities.IsClosingAll)
			{
				return;
			}
			m_blockTable.Remove(block.EntityId);
			m_blocks.Remove(block);
			m_blockList.Remove(block);
			m_blocksForHud.Remove(block);
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				myBlockGroup.Blocks.Remove(block);
				if (myBlockGroup.Blocks.get_Count() == 0)
				{
					RemoveGroup(myBlockGroup, !block.IsBeingRemoved);
					i--;
				}
			}
			this.BlockRemoved?.Invoke(block);
		}

		public MyBlockGroup AddUpdateGroup(MyBlockGroup gridGroup, bool fireEvent, bool modify = false)
		{
			if (gridGroup.Blocks.get_Count() == 0)
			{
				return null;
			}
			MyBlockGroup myBlockGroup = BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(gridGroup.Name) == 0);
			if (myBlockGroup == null)
			{
				myBlockGroup = new MyBlockGroup();
				myBlockGroup.Name.Clear().AppendStringBuilder(gridGroup.Name);
				BlockGroups.Add(myBlockGroup);
			}
			if (modify)
			{
				myBlockGroup.Blocks.Clear();
			}
			myBlockGroup.Blocks.UnionWith((IEnumerable<MyTerminalBlock>)gridGroup.Blocks);
			if (fireEvent && this.GroupAdded != null)
			{
				this.GroupAdded(gridGroup);
			}
			return gridGroup;
		}

		public void RemoveGroup(MyBlockGroup gridGroup, bool fireEvent)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			MyBlockGroup myBlockGroup = BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(gridGroup.Name) == 0);
			if (myBlockGroup != null)
			{
				List<MyTerminalBlock> list = new List<MyTerminalBlock>();
				Enumerator<MyTerminalBlock> enumerator = gridGroup.Blocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyTerminalBlock current = enumerator.get_Current();
						if (myBlockGroup.Blocks.Contains(current))
						{
							list.Add(current);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				foreach (MyTerminalBlock item in list)
				{
					myBlockGroup.Blocks.Remove(item);
				}
				if (myBlockGroup.Blocks.get_Count() == 0)
				{
					BlockGroups.Remove(myBlockGroup);
				}
			}
			if (fireEvent && this.GroupRemoved != null)
			{
				this.GroupRemoved(gridGroup);
			}
		}

		public void CopyBlocksTo(List<MyTerminalBlock> result)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					result.Add(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void UpdateGridBlocksOwnership(long ownerID)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					current.IsAccessibleForProgrammableBlock = current.HasPlayerAccess(ownerID);
				}
			}
			finally
			{
<<<<<<< HEAD
				block.IsAccessibleForProgrammableBlock = block.HasPlayerAccess(ownerID, MyRelationsBetweenPlayerAndBlock.Enemies);
=======
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void UpdateHud()
		{
			if (!NeedsHudUpdate)
			{
				return;
			}
			if (m_lastHudIndex < m_blocks.get_Count())
			{
				MyTerminalBlock myTerminalBlock = m_blockList[m_lastHudIndex];
				if (MeetsHudConditions(myTerminalBlock))
				{
					m_blocksForHud.Add(myTerminalBlock);
				}
				else
				{
					m_blocksForHud.Remove(myTerminalBlock);
				}
				m_lastHudIndex++;
			}
			else
			{
				m_lastHudIndex = 0;
				NeedsHudUpdate = false;
			}
		}

		private bool MeetsHudConditions(MyTerminalBlock terminalBlock)
		{
			if (terminalBlock is MyRadioAntenna)
<<<<<<< HEAD
=======
			{
				return false;
			}
			if (terminalBlock.HasLocalPlayerAccess() && (terminalBlock.ShowOnHUD || (terminalBlock.IsBeingHacked && terminalBlock.IDModule != null && terminalBlock.IDModule.Owner != 0L) || (terminalBlock is MyCockpit && (terminalBlock as MyCockpit).Pilot != null)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return false;
			}
			if (terminalBlock.HasLocalPlayerAccess() && (terminalBlock.ShowOnHUD || (terminalBlock.IsBeingHacked && terminalBlock.IDModule != null && terminalBlock.IDModule.Owner != 0L) || (terminalBlock is MyCockpit && (terminalBlock as MyCockpit).Pilot != null)))
			{
<<<<<<< HEAD
				return true;
=======
				_ = terminalBlock is IMyComponentOwner<MyOreDetectorComponent>;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}

		internal void BlockManipulationFinishedFunction()
		{
			this.BlockManipulationFinished?.Invoke();
		}

		[Conditional("DEBUG")]
		private void RecordChange(string text)
		{
			m_debugChanges.Add(DateTime.Now.ToLongTimeString() + ": " + text);
			if (m_debugChanges.Count > 10)
			{
				m_debugChanges.RemoveAt(0);
			}
		}

		public void DebugDraw(MyEntity entity)
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS)
			{
				return;
			}
			double num = 6.5 * 0.045;
			Vector3D vector3D = entity.WorldMatrix.Translation;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.GetPhysicalGroupAABB();
				vector3D = myCubeGrid.GetPhysicalGroupAABB().Center;
				if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					vector3D -= new Vector3D(0.0, 5.0, 0.0);
				}
			}
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(vector3D, position);
			float num2 = (float)Math.Atan(6.5 / Math.Max(val, 0.001));
			if (num2 <= 0.27f)
			{
				return;
			}
			MyRenderProxy.DebugDrawText3D(vector3D, entity.ToString(), Color.Yellow, num2, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			int num3 = -1;
<<<<<<< HEAD
			MyRenderProxy.DebugDrawText3D(vector3D + num3 * up * num + right * 0.064999997615814209, $"Blocks: {m_blocks.Count}", Color.LightYellow, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
=======
			MyRenderProxy.DebugDrawText3D(vector3D + num3 * up * num + right * 0.064999997615814209, $"Blocks: {m_blocks.get_Count()}", Color.LightYellow, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			num3--;
			MyRenderProxy.DebugDrawText3D(vector3D + num3 * up * num + right * 0.064999997615814209, $"Groups: {m_blockGroups.Count}", Color.LightYellow, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			num3--;
			MyRenderProxy.DebugDrawText3D(vector3D + num3 * up * num + right * 0.064999997615814209, "Recent group changes:", Color.LightYellow, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			num3--;
			foreach (string debugChange in m_debugChanges)
			{
				MyRenderProxy.DebugDrawText3D(vector3D + num3 * up * num + right * 0.064999997615814209, debugChange, Color.White, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				num3--;
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocks(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			blocks.Clear();
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current.IsAccessibleForProgrammableBlock)
					{
						blocks.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroups(List<Sandbox.ModAPI.Ingame.IMyBlockGroup> blockGroups, Func<Sandbox.ModAPI.Ingame.IMyBlockGroup, bool> collect)
		{
			blockGroups?.Clear();
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				if (collect == null || collect(myBlockGroup))
				{
					blockGroups?.Add(myBlockGroup);
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					T val = current as T;
					if (val != null && current.IsAccessibleForProgrammableBlock && (collect == null || collect(val)))
					{
						blocks?.Add(val);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current as T != null && current.IsAccessibleForProgrammableBlock && (collect == null || collect(current)))
					{
						blocks?.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.SearchBlocksOfName(string name, List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (System.StringExtensions.Contains(block.CustomName.ToString(), name, StringComparison.OrdinalIgnoreCase) && block.IsAccessibleForProgrammableBlock && (collect == null || collect(block)))
=======
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (System.StringExtensions.Contains(current.CustomName.ToString(), name, StringComparison.OrdinalIgnoreCase) && current.IsAccessibleForProgrammableBlock && (collect == null || collect(current)))
					{
						blocks?.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithName(string name)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current.CustomName.CompareTo(name) == 0 && current.IsAccessibleForProgrammableBlock)
					{
						return current;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}

		Sandbox.ModAPI.Ingame.IMyBlockGroup Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroupWithName(string name)
		{
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				if (myBlockGroup.Name.CompareTo(name) == 0)
				{
					return myBlockGroup;
				}
			}
			return null;
		}

		Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithId(long id)
		{
			if (m_blockTable.TryGetValue(id, out var value) && value.IsAccessibleForProgrammableBlock)
			{
				return value;
			}
			return null;
		}

		bool Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.CanAccess(Sandbox.ModAPI.Ingame.IMyTerminalBlock block, MyTerminalAccessScope scope)
		{
			throw new NotSupportedException("This method must be run in a programmable block context and cannot run in a modding context.");
		}

		bool Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.CanAccess(IMyCubeGrid grid, MyTerminalAccessScope scope)
		{
			throw new NotSupportedException("This method must be run in a programmable block context and cannot run in a modding context.");
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlocks(List<Sandbox.ModAPI.IMyTerminalBlock> blocks)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			blocks.Clear();
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					blocks.Add(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockGroups(List<Sandbox.ModAPI.IMyBlockGroup> blockGroups)
		{
			blockGroups.Clear();
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				blockGroups.Add(blockGroup);
			}
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlocksOfType<T>(List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			blocks.Clear();
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current is T && (collect == null || collect(current)))
					{
						blocks.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.SearchBlocksOfName(string name, List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			blocks.Clear();
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (System.StringExtensions.Contains(block.CustomName.ToString(), name, StringComparison.OrdinalIgnoreCase) && (collect == null || collect(block)))
=======
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (System.StringExtensions.Contains(current.CustomName.ToString(), name, StringComparison.OrdinalIgnoreCase) && (collect == null || collect(current)))
					{
						blocks.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		Sandbox.ModAPI.IMyTerminalBlock Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockWithName(string name)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyTerminalBlock> enumerator = m_blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current.CustomName.ToString() == name)
					{
						return current;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}

		Sandbox.ModAPI.IMyBlockGroup Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockGroupWithName(string name)
		{
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				if (blockGroup.Name.ToString() == name)
				{
					return blockGroup;
				}
			}
			return null;
		}

		private Action<MyBlockGroup> GetDelegate(Action<Sandbox.ModAPI.IMyBlockGroup> value)
		{
			return (Action<MyBlockGroup>)Delegate.CreateDelegate(typeof(Action<MyBlockGroup>), value.Target, value.Method);
		}
	}
}
