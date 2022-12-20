using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes connector block (PB scripting interface)
	/// </summary>
	public interface IMyShipConnector : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets or sets whether this connector should throw out anything placed in its inventory.
		/// </summary>
		bool ThrowOut { get; set; }

		/// <summary>
		/// Gets or sets whether this connector should be pulling items into its inventory.
		/// </summary>
		bool CollectAll { get; set; }

		/// <summary>
		/// Gets or sets the currently configured strength of the pull when the connector is within
		/// range of another.
		/// </summary>
		float PullStrength { get; set; }

		[Obsolete("Use the Status property")]
		bool IsLocked { get; }

		[Obsolete("Use the Status property")]
		bool IsConnected { get; }

		/// <summary>
		/// Determines the current status of the connector.
		/// </summary>
		MyShipConnectorStatus Status { get; }

		/// <summary>
		/// Gets the connector this one is connected to when <see cref="P:Sandbox.ModAPI.Ingame.IMyShipConnector.Status" /> is <see cref="F:Sandbox.ModAPI.Ingame.MyShipConnectorStatus.Connected" />.
		/// </summary>
		IMyShipConnector OtherConnector { get; }

		/// <summary>
		/// Is this block triggered by the parking signal?
		/// </summary>
		bool IsParkingEnabled { get; set; }

		/// <summary>
		/// Attempts to connect. If <see cref="P:Sandbox.ModAPI.Ingame.IMyShipConnector.Status" /> is anything else but <see cref="F:Sandbox.ModAPI.Ingame.MyShipConnectorStatus.Connectable" />, this method does nothing.
		/// </summary>
		void Connect();

		/// <summary>
		/// Disconnects this connector.
		/// </summary>
		void Disconnect();

		/// <summary>
		/// Toggles between <see cref="F:Sandbox.ModAPI.Ingame.MyShipConnectorStatus.Connected" /> and <see cref="F:Sandbox.ModAPI.Ingame.MyShipConnectorStatus.Unconnected" />, depending on the current status.
		/// Another connector must be in range for this method to have any effect.
		/// </summary>
		void ToggleConnect();
	}
}
