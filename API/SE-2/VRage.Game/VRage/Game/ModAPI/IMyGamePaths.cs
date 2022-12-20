namespace VRage.Game.ModAPI
{
	/// <summary>
	/// ModAPI interface that, giving you information about most important game paths 
	/// </summary>
	public interface IMyGamePaths
	{
<<<<<<< HEAD
		/// <summary>
		/// Return path, where original SE content folder is located. Example: D:\SteamLibrary\steamapps\common\SpaceEngineers\Content
		/// </summary>
		string ContentPath { get; }

		/// <summary>
		/// Return path, where SE mod folder is located. Example: C:\Users\{USERNAME}\AppData\Roaming\SpaceEngineers\Mods
		/// </summary>
		string ModsPath { get; }

		/// <summary>
		/// Return path, where SE user folder is located. Example: C:\Users\{USERNAME}\AppData\Roaming\SpaceEngineers
		/// </summary>
		string UserDataPath { get; }

		/// <summary>
		/// Return path, where SE user saves path is located. Example: C:\Users\{USERNAME}\AppData\Roaming\SpaceEngineers\Saves
		/// </summary>
=======
		string ContentPath { get; }

		string ModsPath { get; }

		string UserDataPath { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		string SavesPath { get; }

		/// <summary>
		/// Gets the calling mod's assembly ScopeName. This name is used in storage paths (eg. 1234567.sbm_TypeName).
		/// </summary>
		string ModScopeName { get; }
	}
}
