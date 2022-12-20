using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes ship tool block (drill/grinder/welder) (PB scripting interface)
	/// </summary>
	public interface IMyShipToolBase : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets if block should push/pull items from conveyor system 
		/// </summary>
		bool UseConveyorSystem { get; set; }

		/// <summary>
		/// True if the tool is activated by mouse click or toggling via terminal.
		/// </summary>
		bool IsActivated { get; }
=======
		bool UseConveyorSystem { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
