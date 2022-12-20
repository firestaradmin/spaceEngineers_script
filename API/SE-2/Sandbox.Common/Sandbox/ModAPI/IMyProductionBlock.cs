using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes production blocks : refinery block, assembler block, survival kit block. (mods interface)
	/// </summary>
	public interface IMyProductionBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets inventory responsible for source items
		/// </summary>
		new VRage.Game.ModAPI.IMyInventory InputInventory { get; }

		/// <summary>
		/// Gets inventory responsible for produced items
		/// </summary>
=======
		new VRage.Game.ModAPI.IMyInventory InputInventory { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		new VRage.Game.ModAPI.IMyInventory OutputInventory { get; }

		/// <summary>
		/// Called when production block has started producing items
		/// </summary>
		event Action StartedProducing;

		/// <summary>
		/// Called when production block has stopped producing items (ex: no power, no resources, inventory full)
		/// </summary>
		event Action StoppedProducing;

		/// <summary>
		/// Can this production block produce this blueprint?
		/// </summary>
		/// <param name="blueprint">A MyBlueprintDefinition that defines the blueprint</param>
		/// <returns></returns>
		bool CanUseBlueprint(MyDefinitionBase blueprint);

		/// <summary>
		/// Adds a blueprint to the production queue
		/// </summary>
		/// <param name="blueprint">A MyBlueprintDefinition that defines the blueprint</param>
		/// <param name="amount">Amount of items</param>
		void AddQueueItem(MyDefinitionBase blueprint, MyFixedPoint amount);

		/// <summary>
		/// Inserts a blueprint into the production queue
		/// </summary>
		/// <param name="idx">Index of the item</param>
		/// <param name="blueprint">A MyBlueprintDefinition that defines the blueprint</param>
		/// <param name="amount">Amount of items</param>
		void InsertQueueItem(int idx, MyDefinitionBase blueprint, MyFixedPoint amount);

		/// <summary>
		/// Gets the current production queue (copy)
		/// </summary>
		/// <returns>List of MyProductionQueueItems</returns>
		List<MyProductionQueueItem> GetQueue();
	}
}
