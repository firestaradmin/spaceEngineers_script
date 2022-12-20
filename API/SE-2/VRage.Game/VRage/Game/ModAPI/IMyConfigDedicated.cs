using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes dedicated server configuration (mods interface)
	/// </summary>
	public interface IMyConfigDedicated
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets administrators
		/// It may contain ids in 2 different formats:
		/// First - steamId.ToString()
		/// Second - starts with `STEAM_0:`
		/// </summary>
		List<string> Administrators { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		int AsteroidAmount { get; set; }

		/// <summary>
		/// Gets or sets Banned players. SteamId and Xbox live ids
		/// </summary>
		List<ulong> Banned { get; set; }

		/// <summary>
		/// Gets or sets reserved players (player can join server even if it is full). SteamId and Xbox live ids
		/// </summary>
		List<ulong> Reserved { get; set; }

		/// <summary>
		/// Steam group id, that blocking access to player not from this group. 
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		ulong GroupID { get; set; }

		/// <summary>
		/// Setting that is used server start. When it is true, it should not load previous server launch world  
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		bool IgnoreLastSession { get; set; }

		/// <summary>
		/// Gets or sets server IP
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		string IP { get; set; }

		/// <summary>
		/// Gets current world load path or sets next server start load path
		/// </summary>
		string LoadWorld { get; set; }

		/// <summary>
		/// Gets or sets world platform : Steam / XBox 
		/// </summary>
		string WorldPlatform { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		bool VerboseNetworkLogging { get; set; }

		/// <summary>
		/// Pause game when zero players on servers
		/// </summary>
		bool PauseGameWhenEmpty { get; set; }

		/// <summary>
		/// Shows Gui Popup for players 
		/// </summary>
		string MessageOfTheDay { get; set; }

		/// <summary>
		/// Shows Gui Popup for players but in web browser
		/// </summary>
		string MessageOfTheDayUrl { get; set; }

		/// <summary>
		/// Gets or sets whether auto restart is enabled
		/// </summary>
		bool AutoRestartEnabled { get; set; }

		/// <summary>
		/// Gets or sets auto restart time in minutes
		/// </summary>
		int AutoRestatTimeInMin { get; set; }

		/// <summary>
		/// Gets or sets if game auto update enabled
		/// </summary>
		bool AutoUpdateEnabled { get; set; }

		/// <summary>
		/// Gets or sets how often game checks if new version is available
		/// </summary>
		int AutoUpdateCheckIntervalInMin { get; set; }

		/// <summary>
		/// Gets or sets time before restart after new update found
		/// </summary>
		int AutoUpdateRestartDelayInMin { get; set; }

		/// <summary>
		/// Gets or sets if game should save on server stop 
		/// </summary>
		bool RestartSave { get; set; }

		/// <summary>
		/// Gets or sets name of steam version branch 
		/// </summary>
		string AutoUpdateSteamBranch { get; set; }

		/// <summary>
		/// Gets or sets password of steam version branch 
		/// </summary>
		string AutoUpdateBranchPassword { get; set; }

		/// <summary>
		/// Gets or sets server name
		/// </summary>
		string ServerName { get; set; }

		/// <summary>
		/// Gets or sets server connection port 27016 - default
		/// </summary>
		int ServerPort { get; set; }

		/// <summary>
		/// Gets or sets (but that doesn't change anything) session settings
		/// </summary>
		MyObjectBuilder_SessionSettings SessionSettings { get; set; }

		/// <summary>
		/// Gets or sets steam port
		/// </summary>
		int SteamPort { get; set; }

		/// <summary>
		/// Gets or sets world name.
		/// Doesn't change world name in client find server gui when setted. 
		/// </summary>
		string WorldName { get; set; }

		/// <summary>
		/// When <see cref="P:VRage.Game.ModAPI.IMyConfigDedicated.IgnoreLastSession" /> is true and <see cref="P:VRage.Game.ModAPI.IMyConfigDedicated.LoadWorld" /> is null or empty, or failed - game would be start new world from <see cref="P:VRage.Game.ModAPI.IMyConfigDedicated.PremadeCheckpointPath" /> 
		/// </summary>
		string PremadeCheckpointPath { get; set; }

		/// <summary>
		/// Gets or sets server description
		/// </summary>
		string ServerDescription { get; set; }

		/// <summary>
		/// Gets or sets server password hash 
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		string ServerPasswordHash { get; set; }

		/// <summary>
		/// Gets or sets server password hash
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		string ServerPasswordSalt { get; set; }

		/// <summary>
		/// Gets or sets if remote api enabled
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		bool RemoteApiEnabled { get; set; }

		/// <summary>
		/// Gets or sets remote api password
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		string RemoteSecurityKey { get; set; }

		/// <summary>
		/// Gets or sets remote api port
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		int RemoteApiPort { get; set; }

		/// <summary>
		/// Gets or sets server plugins. List contains file paths to dlls 
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		List<string> Plugins { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		float WatcherInterval { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		float WatcherSimulationSpeedMinimum { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		int ManualActionDelay { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		string ManualActionChatMessage { get; set; }

		/// <summary>
		/// Gets or sets if game should automatically add dependency mods in mods list 
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		bool AutodetectDependencies { get; set; }

		/// <summary>
		/// Gets or sets if game should automatically add dependency mods in mods list 
		/// </summary>
		bool SaveChatToLog { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		string NetworkType { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		List<string> NetworkParameters { get; set; }

		/// <summary>
		/// Not used
		/// </summary>
		bool ConsoleCompatibility { get; set; }

		/// <summary>
		/// Gets where settings file is located
		/// </summary>
		/// <returns>File path</returns>
=======
		List<string> Administrators { get; set; }

		int AsteroidAmount { get; set; }

		List<ulong> Banned { get; set; }

		List<ulong> Reserved { get; set; }

		ulong GroupID { get; set; }

		bool IgnoreLastSession { get; set; }

		string IP { get; set; }

		string LoadWorld { get; set; }

		string WorldPlatform { get; set; }

		bool VerboseNetworkLogging { get; set; }

		bool PauseGameWhenEmpty { get; set; }

		string MessageOfTheDay { get; set; }

		string MessageOfTheDayUrl { get; set; }

		bool AutoRestartEnabled { get; set; }

		int AutoRestatTimeInMin { get; set; }

		bool AutoUpdateEnabled { get; set; }

		int AutoUpdateCheckIntervalInMin { get; set; }

		int AutoUpdateRestartDelayInMin { get; set; }

		bool AutoRestartSave { get; set; }

		string AutoUpdateSteamBranch { get; set; }

		string AutoUpdateBranchPassword { get; set; }

		string ServerName { get; set; }

		int ServerPort { get; set; }

		MyObjectBuilder_SessionSettings SessionSettings { get; set; }

		int SteamPort { get; set; }

		string WorldName { get; set; }

		string PremadeCheckpointPath { get; set; }

		string ServerDescription { get; set; }

		string ServerPasswordHash { get; set; }

		string ServerPasswordSalt { get; set; }

		bool RemoteApiEnabled { get; set; }

		string RemoteSecurityKey { get; set; }

		int RemoteApiPort { get; set; }

		List<string> Plugins { get; set; }

		float WatcherInterval { get; set; }

		float WatcherSimulationSpeedMinimum { get; set; }

		int ManualActionDelay { get; set; }

		string ManualActionChatMessage { get; set; }

		bool AutodetectDependencies { get; set; }

		bool SaveChatToLog { get; set; }

		string NetworkType { get; set; }

		List<string> NetworkParameters { get; set; }

		bool ConsoleCompatibility { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		string GetFilePath();

		/// <summary>
		/// Load settings from file
		/// </summary>
		void Load(string path = null);

		/// <summary>
		/// Saves game.
		/// </summary>
		/// <param name="path">When not null will save to provided folder</param>
		void Save(string path = null);

		/// <summary>
		/// Used to set new password for server
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		void SetPassword(string password);

		/// <summary>
		/// Gets remote api password
		/// </summary>
		/// <remarks>You need save and restart server to apply changes</remarks>
		void GenerateRemoteSecurityKey();
	}
}
