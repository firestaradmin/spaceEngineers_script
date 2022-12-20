using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes programmable block (PB scripting interface)
	/// </summary>
	public interface IMyProgrammableBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity, IMyTextSurfaceProvider
	{
		/// <summary>
		/// Get whether this programmable block is currently running its program.
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		/// Get default terminal argument.
		/// </summary>
		string TerminalRunArgument { get; }

		/// <summary>
		/// Attempts to run this programmable block using the given argument. An already running
		/// programmable block cannot be run again.
		/// This is equivalent to running <c>block.ApplyAction("Run", argumentsList);</c>
		/// This should be called from an ingame script. Do not use in mods.
		/// </summary>
		/// <param name="argument"></param>
		/// <returns><c>true</c> if the action was applied, <c>false</c> otherwise</returns>
		bool TryRun(string argument);
	}
}
