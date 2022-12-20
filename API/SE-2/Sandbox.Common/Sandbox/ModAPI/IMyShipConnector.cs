using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes connector block (mods interface)
	/// </summary>
	public interface IMyShipConnector : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipConnector
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets the connector this one is connected to when <see cref="P:Sandbox.ModAPI.Ingame.IMyShipConnector.Status" /> is <see cref="F:Sandbox.ModAPI.Ingame.MyShipConnectorStatus.Connected" />.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		new IMyShipConnector OtherConnector { get; }
	}
}
