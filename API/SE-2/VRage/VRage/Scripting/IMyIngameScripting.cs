namespace VRage.Scripting
{
<<<<<<< HEAD
	/// <summary>
	/// Allows mods change programmable block script settings
	/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public interface IMyIngameScripting
	{
		/// <summary>
		/// Provides the ability for mods to add and remove items from a type and member blacklist,
		/// giving the ability to remove even more API for scripts. Intended for server admins to
		/// restrict what people are able to do with scripts to keep their simspeed up.
		/// </summary>
		IMyScriptBlacklist ScriptBlacklist { get; }

<<<<<<< HEAD
		/// <summary>
		/// Clears all <see cref="P:VRage.Scripting.IMyIngameScripting.ScriptBlacklist" /> changes 
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void Clean();
	}
}
