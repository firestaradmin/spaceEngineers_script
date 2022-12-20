using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes vending machine block (mods interface)
	/// </summary>
	public interface IMyVendingMachine : IMyStoreBlock, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyStoreBlock
	{
		/// <summary>
		/// Select next item.
		/// </summary>
		void SelectNextItem();

		/// <summary>
		/// Select previews item.
		/// </summary>
		void SelectPreviewsItem();

		/// <summary>
		/// Sells the item to the person using the machine.
		/// </summary>
		void Buy();
	}
}
