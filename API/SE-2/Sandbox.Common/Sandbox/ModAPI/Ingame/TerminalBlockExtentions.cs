using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Class having some sugar extensions.
	/// Written by Kalvin Osborne, AKA Night Lone. 
	/// </summary>
	public static class TerminalBlockExtentions
	{
		/// <summary>
		/// Get EntityId of block
		/// </summary>
		/// <param name="block">Target block</param>
		/// <returns>EntityId</returns>
		public static long GetId(this IMyTerminalBlock block)
		{
			return block.EntityId;
		}

		/// <summary>
		/// Finds action by <see cref="M:Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActionWithName(System.String)" /> and calls <see cref="M:Sandbox.ModAPI.Interfaces.ITerminalAction.Apply(VRage.Game.ModAPI.Ingame.IMyCubeBlock)" />
		/// </summary>
		/// <param name="block">To apply action on</param>
		/// <param name="actionName">Name of action</param>
		/// <exception cref="T:System.NullReferenceException">If action not found</exception>
		public static void ApplyAction(this IMyTerminalBlock block, string actionName)
		{
			block.GetActionWithName(actionName).Apply(block);
		}

		/// <summary>
		/// Finds action by <see cref="M:Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActionWithName(System.String)" /> and calls <see cref="M:Sandbox.ModAPI.Interfaces.ITerminalAction.Apply(VRage.Game.ModAPI.Ingame.IMyCubeBlock,VRage.Collections.ListReader{Sandbox.ModAPI.Ingame.TerminalActionParameter})" />
		/// </summary>
		/// <param name="block">To apply action on</param>
		/// <param name="actionName">Name of action</param>
		/// <param name="parameters">Parameters for terminal action function call</param>
		/// <exception cref="T:System.NullReferenceException">If action not found</exception>
		public static void ApplyAction(this IMyTerminalBlock block, string actionName, List<TerminalActionParameter> parameters)
		{
			block.GetActionWithName(actionName).Apply(block, parameters);
		}

		/// <summary>
		/// Searches for terminal action with name
		/// </summary>
		/// <param name="block">Terminal block which should have action</param>
		/// <param name="actionName">Name of action</param>
		/// <returns>True if terminal action found</returns>
		public static bool HasAction(this IMyTerminalBlock block, string actionName)
		{
			return block.GetActionWithName(actionName) != null;
		}

		[Obsolete("Use the HasInventory property.")]
		public static bool HasInventory(this IMyTerminalBlock block)
		{
			MyEntity myEntity = block as MyEntity;
			if (myEntity == null)
			{
				return false;
			}
			if (!(block is IMyInventoryOwner))
			{
				return false;
			}
			return myEntity.HasInventory;
		}

		[Obsolete("Use the GetInventoryBase method.")]
		public static IMyInventory GetInventory(this IMyTerminalBlock block, int index)
		{
			MyEntity myEntity = block as MyEntity;
			if (myEntity == null)
			{
				return null;
			}
			if (!myEntity.HasInventory)
			{
				return null;
			}
			return myEntity.GetInventoryBase(index) as IMyInventory;
		}

		[Obsolete("Use the InventoryCount property.")]
		public static int GetInventoryCount(this IMyTerminalBlock block)
		{
			return (block as MyEntity)?.InventoryCount ?? 0;
		}

		[Obsolete("Use the blocks themselves, this method is no longer reliable")]
		public static bool GetUseConveyorSystem(this IMyTerminalBlock block)
		{
			if (block is IMyInventoryOwner)
			{
				return ((IMyInventoryOwner)block).UseConveyorSystem;
			}
			return false;
		}

		[Obsolete("Use the blocks themselves, this method is no longer reliable")]
		public static void SetUseConveyorSystem(this IMyTerminalBlock block, bool use)
		{
			if (block is IMyInventoryOwner)
			{
				((IMyInventoryOwner)block).UseConveyorSystem = use;
			}
		}
	}
}
