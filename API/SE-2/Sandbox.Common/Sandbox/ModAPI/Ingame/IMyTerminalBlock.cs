using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Interfaces;
<<<<<<< HEAD
using VRage.Game;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes terminal block (PB scripting interface)
	/// </summary>
	public interface IMyTerminalBlock : IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets how block is named in Terminal menu
		/// </summary>
		string CustomName { get; set; }

		/// <summary>
		/// Gets or sets how block is named in Terminal menu. Work only for Cockpit, LaserAntenna RadioAntenna, SpaceBall, Beacon
		/// </summary>
		string CustomNameWithFaction { get; }

		/// <summary>
		/// Gets information about block status. In Control panel bottom right text
		/// </summary>
		string DetailedInfo { get; }

		/// <summary>
		/// Gets information about block status (available from mods) <see cref="E:Sandbox.ModAPI.IMyTerminalBlock.AppendingCustomInfo" /> <see cref="M:Sandbox.ModAPI.IMyTerminalBlock.RefreshCustomInfo" />. 
		/// </summary>
=======
		string CustomName { get; set; }

		string CustomNameWithFaction { get; }

		string DetailedInfo { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		string CustomInfo { get; }

		/// <summary>
		/// Gets or sets the Custom Data string.
		/// NOTE: Only use this for user input. For storing large mod configs, create your own MyModStorageComponent
		/// </summary>
		string CustomData { get; set; }

<<<<<<< HEAD
		/// <summary>
		/// Represent terminal gui toggle `Show On HUD`. Gets or sets its value
		/// </summary>
		bool ShowOnHUD { get; set; }

		/// <summary>
		/// Represent terminal gui toggle `Show block in terminal`. Gets or sets its value
		/// </summary>
		bool ShowInTerminal { get; set; }

		/// <summary>
		/// Represent terminal gui toggle `Show in toolbar config`. Gets or sets its value
		/// </summary>
		bool ShowInToolbarConfig { get; set; }
=======
		bool ShowOnHUD { get; set; }

		bool ShowInTerminal { get; set; }

		bool ShowInToolbarConfig { get; set; }

		bool ShowInInventory { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Represent terminal gui toggle `Show block in Inventory Screen`. Gets or sets its value
		/// </summary>
		bool ShowInInventory { get; set; }

		/// <summary>
		/// Returns if local player can use block. Executes <see cref="M:Sandbox.ModAPI.Ingame.IMyTerminalBlock.HasPlayerAccess(System.Int64,VRage.Game.MyRelationsBetweenPlayerAndBlock)" /> with local player identityId.
		/// On Dedicated Server as identityId it is using 0 as playerId
		/// </summary>
		/// <returns>Can player access this block or not. (Result of <see cref="M:Sandbox.ModAPI.Ingame.IMyTerminalBlock.HasPlayerAccess(System.Int64,VRage.Game.MyRelationsBetweenPlayerAndBlock)" /> function call)</returns>
		bool HasLocalPlayerAccess();

		/// <summary>
		/// Returns if local player can use block.
		/// It is also checking for admin access.
		/// </summary>
		/// <param name="playerId">PlayerId which you want check</param>
		/// <param name="defaultNoUser"></param>
		/// <returns>Can player access block or not</returns>
		/// <seealso cref="T:VRage.Game.MyRelationsBetweenPlayerAndBlock" />
		bool HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership);

		[Obsolete("Use the setter of Customname")]
		void SetCustomName(string text);

		[Obsolete("Use the setter of Customname")]
		void SetCustomName(StringBuilder text);

		/// <summary>
		/// Get all terminal actions available for block
		/// </summary>
		/// <param name="resultList">Buffer list, results would be added here. Can be null if <param ref="collect" /> always returns false</param>
		/// <param name="collect">Filter function, if function is null or returns true, terminal action would be added to <param ref="resultList" /></param>
		/// <seealso cref="T:Sandbox.ModAPI.Interfaces.ITerminalAction" />
		void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null);

		/// <summary>
		/// Get all terminal actions available for block.
		/// NOTE: First called `<param ref="collect" />` and then `<param ref="name" />` check
		/// </summary>
		/// <param name="name">Searches for terminal action with this name</param>
		/// <param name="resultList">Buffer list, results would be added here. Can be null if <param ref="collect" /> always returns false</param>
		/// <param name="collect">Filter function, if function is null or returns true, terminal action would be added to <param ref="resultList" /></param>
		/// <seealso cref="T:Sandbox.ModAPI.Interfaces.ITerminalAction" />
		void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null);

		/// <summary>
		/// Get first found terminal action with name
		/// </summary>
		/// <param name="name">Searches for terminal action with this name</param>
		/// <returns>Found terminal action or null</returns>
		/// <seealso cref="T:Sandbox.ModAPI.Interfaces.ITerminalAction" />
		ITerminalAction GetActionWithName(string name);

		/// <summary>
		/// Finds terminal property with provided id
		/// </summary>
		/// <param name="id">Terminal id</param>
		/// <returns>Found terminal property or null</returns>
		/// <seealso cref="T:Sandbox.ModAPI.Interfaces.ITerminalProperty" />
		ITerminalProperty GetProperty(string id);

		/// <summary>
		/// Get all terminal actions available for block.
		/// </summary>
		/// <param name="resultList">Buffer list, results would be added here. Can be null if <param ref="collect" /> always returns false</param>
		/// <param name="collect">Filter function, if function is null or returns true, terminal property would be added to <param ref="resultList" /></param>
		/// <seealso cref="T:Sandbox.ModAPI.Interfaces.ITerminalAction" />
		void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null);

		/// <summary>
		/// <para>
		/// Determines whether this block is <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Mechanical" /> connected to the other. This is any block connected
		/// with rotors or pistons or other mechanical devices, but not things like connectors. This will in most
		/// cases constitute your complete construct.
		/// </para>
		/// <para>
		/// Be aware that using merge blocks combines grids into one, so this function will not filter out grids
		/// connected that way. Also be aware that detaching the heads of pistons and rotors will cause this
		/// connection to change.
		/// </para>
		/// </summary>
		/// <param name="other">Other block</param>
		/// <returns>True if blocks are on same grid, or has <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Mechanical" /> linking</returns>
		bool IsSameConstructAs(IMyTerminalBlock other);
	}
}
