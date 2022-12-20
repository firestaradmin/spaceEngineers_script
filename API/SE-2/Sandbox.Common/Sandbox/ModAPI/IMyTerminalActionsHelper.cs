using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Interfaces;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// API class for interactions with terminal actions and properties
	/// </summary>
	public interface IMyTerminalActionsHelper
	{
		/// <summary>
		/// Gets available <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalAction" /> for a block with specified type 
		/// </summary>
		/// <param name="blockType">Block type, that should have actions</param>
		/// <param name="resultList">Preallocated list, where results would be added</param>
		/// <param name="collect">Filter function, if it returns true, item would be added to list</param>
		void GetActions(Type blockType, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null);

		/// <summary>
		/// Gets available <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalAction" /> for a block with specified type and name
		/// </summary>
		/// <param name="name">Action should contain or be equal to this argument</param>
		/// <param name="blockType">Block type, that should have actions</param>
		/// <param name="resultList">Preallocated list, where results would be added</param>
		/// <param name="collect">Filter function, if it returns true, item would be added to list</param>
		void SearchActionsOfName(string name, Type blockType, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null);

		/// <summary>
		/// Gets first available <see cref="T:Sandbox.ModAPI.Interfaces.ITerminalAction" /> for a block with specified type and name
		/// </summary>
		/// <param name="actionId">action.Id should be equal to this argument</param>
		/// <param name="blockType">Block type, that should have action</param>
		/// <returns>Terminal action or null</returns>
		ITerminalAction GetActionWithName(string actionId, Type blockType);

		/// <summary>
		/// Gets property by id
		/// </summary>
		/// <param name="id">Terminal property id should be equal this argument</param>
		/// <param name="blockType">Block type, that should have property</param>
		/// <returns></returns>
		ITerminalProperty GetProperty(string id, Type blockType);

		/// <summary>
		/// Gets all properties that belongs to specific block
		/// </summary>
		/// <param name="blockType">Block type, that should have properties</param>
		/// <param name="resultList">Preallocated list, where results would be added</param>
		/// <param name="collect">Filter function, if it returns true, item would be added to list</param>
		void GetProperties(Type blockType, List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null);

		/// <summary>
		/// Gets <see cref="T:Sandbox.ModAPI.IMyGridTerminalSystem" /> for grid
		/// </summary>
		/// <remarks>Connected grids with <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Logical" /> linking has 1 same for all <see cref="T:Sandbox.ModAPI.IMyGridTerminalSystem" />. You can pass any grid belonging to that grid-group.</remarks>
		/// <param name="grid">For which you want to <see cref="T:Sandbox.ModAPI.IMyGridTerminalSystem" /></param>
		/// <returns><see cref="T:Sandbox.ModAPI.IMyGridTerminalSystem" /> or null, if called too early (on MyCubeGrid.InitInternal).</returns>
		IMyGridTerminalSystem GetTerminalSystemForGrid(IMyCubeGrid grid);
	}
}
