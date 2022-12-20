using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes conveyor sorter block (PB scripting interface)
	/// </summary>
	public interface IMyConveyorSorter : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Determines whether the sorter should drain any inventories connected to it and push them to the other side - as long
		/// as the items passes the filtering as defined by the filter list (<see cref="M:Sandbox.ModAPI.Ingame.IMyConveyorSorter.GetFilterList(System.Collections.Generic.List{Sandbox.ModAPI.Ingame.MyInventoryItemFilter})" />) and <see cref="P:Sandbox.ModAPI.Ingame.IMyConveyorSorter.Mode" />.
		/// </summary>
		bool DrainAll { get; set; }

		/// <summary>
		/// Determines the current mode of this sorter.
		/// </summary>
		MyConveyorSorterMode Mode { get; }

		/// <summary>
		/// Gets the items currently being allowed through or rejected, depending on the <see cref="P:Sandbox.ModAPI.Ingame.IMyConveyorSorter.Mode" />.
		/// </summary>
		/// <param name="items">Buffer array, that would be cleared, and receive block filter settings</param>
		void GetFilterList(List<MyInventoryItemFilter> items);

		/// <summary>
		/// Adds a single item to the filter list. See <see cref="M:Sandbox.ModAPI.Ingame.IMyConveyorSorter.SetFilter(Sandbox.ModAPI.Ingame.MyConveyorSorterMode,System.Collections.Generic.List{Sandbox.ModAPI.Ingame.MyInventoryItemFilter})" /> to change the filter mode and/or fill
		/// the entire list in one go.
		/// </summary>
		/// <param name="item">Item filter settings</param>
		void AddItem(MyInventoryItemFilter item);

		/// <summary>
		/// Removes a single item from the filter list. See <see cref="M:Sandbox.ModAPI.Ingame.IMyConveyorSorter.SetFilter(Sandbox.ModAPI.Ingame.MyConveyorSorterMode,System.Collections.Generic.List{Sandbox.ModAPI.Ingame.MyInventoryItemFilter})" /> to change the filter mode and/or clear
		/// the entire list in one go.
		/// </summary>
		/// <param name="item">Item filter settings</param>
		void RemoveItem(MyInventoryItemFilter item);

		/// <summary>
		/// Determines whether a given item type is allowed go through the sorter, depending on the filter list (<see cref="M:Sandbox.ModAPI.Ingame.IMyConveyorSorter.GetFilterList(System.Collections.Generic.List{Sandbox.ModAPI.Ingame.MyInventoryItemFilter})" />) and <see cref="P:Sandbox.ModAPI.Ingame.IMyConveyorSorter.Mode" />.
		/// </summary>
		/// <param name="id">Item id</param>
		/// <returns>Whether a given item type is allowed go through the sorter</returns>
		bool IsAllowed(MyDefinitionId id);

		/// <summary>
		/// Changes the sorter to desired mode and filters the provided items. You can pass in <c>null</c> to empty the list.
		/// </summary>
		/// <param name="mode">New work mod</param>
		/// <param name="items">Replaces previous filter settings with this one</param>
		void SetFilter(MyConveyorSorterMode mode, List<MyInventoryItemFilter> items);
	}
}
