using System;
using Sandbox.ModAPI.Ingame;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// The interface for the grid program provides extra access for the game and for mods. See <see cref="T:Sandbox.ModAPI.Ingame.MyGridProgram" /> for the class the scripts
	/// actually derive from.
	/// </summary>
	public interface IMyGridProgram
	{
<<<<<<< HEAD
=======
		Func<IMyIntergridCommunicationSystem> IGC_ContextGetter { set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// <summary>
		/// Sets provider for <see cref="T:Sandbox.ModAPI.Ingame.IMyIntergridCommunicationSystem" />
		/// </summary>
<<<<<<< HEAD
		Func<IMyIntergridCommunicationSystem> IGC_ContextGetter { set; }
=======
		Sandbox.ModAPI.Ingame.IMyGridTerminalSystem GridTerminalSystem { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Gets or sets the GridTerminalSystem available for the grid programs.
		/// </summary>
<<<<<<< HEAD
		Sandbox.ModAPI.Ingame.IMyGridTerminalSystem GridTerminalSystem { get; set; }
=======
		Sandbox.ModAPI.Ingame.IMyProgrammableBlock Me { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Gets or sets the programmable block which is currently running this grid program.
		/// </summary>
<<<<<<< HEAD
		Sandbox.ModAPI.Ingame.IMyProgrammableBlock Me { get; set; }
=======
		[Obsolete("Use Runtime.TimeSinceLastRun instead")]
		TimeSpan ElapsedTime { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Gets or sets the storage string for this grid program.
		/// </summary>
		string Storage { get; set; }

		/// <summary>
		/// Gets or sets the object used to provide runtime information for the running grid program.
		/// </summary>
		IMyGridProgramRuntimeInfo Runtime { get; set; }

		/// <summary>
		/// Gets or sets the action which prints out text onto the currently running programmable block's detail info area.
		/// </summary>
		Action<string> Echo { get; set; }

		/// <summary>
		/// Determines whether this grid program has a valid Main method.
		/// </summary>
		bool HasMainMethod { get; }

		/// <summary>
		/// Determines whether this grid program has a valid Save method.
		/// </summary>
		bool HasSaveMethod { get; }

		/// <summary>
		/// Invokes this grid program.
		/// </summary>
		/// <param name="argument"></param>
		[Obsolete("Use overload Main(String, UpdateType)")]
		void Main(string argument);

		/// <summary>
		/// Invokes this grid program with the given update source.
		/// </summary>
		/// <param name="argument"></param>
		/// <param name="updateSource"></param>
		void Main(string argument, UpdateType updateSource);

		/// <summary>
		/// If this grid program has state saving capability, calling this method
		/// will invoke it.
		/// </summary>
		void Save();
	}
}
