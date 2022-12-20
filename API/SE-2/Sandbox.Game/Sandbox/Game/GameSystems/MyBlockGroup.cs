using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public class MyBlockGroup : Sandbox.ModAPI.IMyBlockGroup, Sandbox.ModAPI.Ingame.IMyBlockGroup
	{
		public StringBuilder Name = new StringBuilder();

		internal readonly HashSet<MyTerminalBlock> Blocks = new HashSet<MyTerminalBlock>();

		string Sandbox.ModAPI.Ingame.IMyBlockGroup.Name => Name.ToString();

		internal MyBlockGroup()
		{
		}

		internal void Init(MyCubeGrid grid, MyObjectBuilder_BlockGroup builder)
		{
			Name.Clear().Append(builder.Name);
			foreach (Vector3I block in builder.Blocks)
			{
				MySlimBlock cubeBlock = grid.GetCubeBlock(block);
				if (cubeBlock != null)
				{
					MyTerminalBlock myTerminalBlock = cubeBlock.FatBlock as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						Blocks.Add(myTerminalBlock);
					}
				}
			}
		}

		internal MyObjectBuilder_BlockGroup GetObjectBuilder()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			MyObjectBuilder_BlockGroup myObjectBuilder_BlockGroup = new MyObjectBuilder_BlockGroup();
			myObjectBuilder_BlockGroup.Name = Name.ToString();
			Enumerator<MyTerminalBlock> enumerator = Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					myObjectBuilder_BlockGroup.Blocks.Add(current.Position);
				}
				return myObjectBuilder_BlockGroup;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public override string ToString()
		{
			return $"{Name} - {Blocks.get_Count()} blocks";
		}

		void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocks(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current.IsAccessibleForProgrammableBlock && (collect == null || collect(current)))
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

		void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocksOfType<T>(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = Blocks.GetEnumerator();
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

		void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = Blocks.GetEnumerator();
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

		void Sandbox.ModAPI.IMyBlockGroup.GetBlocks(List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (collect == null || collect(current))
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

		void Sandbox.ModAPI.IMyBlockGroup.GetBlocksOfType<T>(List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			blocks?.Clear();
			Enumerator<MyTerminalBlock> enumerator = Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current as T != null && (collect == null || collect(current)))
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
	}
}
