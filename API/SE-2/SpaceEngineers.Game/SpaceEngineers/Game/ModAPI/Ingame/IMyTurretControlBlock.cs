using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
	/// <summary>
	/// Describes Turret Control block (PB scripting interface)
	/// </summary>
	public interface IMyTurretControlBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets whether this block is locally or remotely controlled.
		/// </summary>
		bool IsUnderControl { get; }

		/// <summary>
		/// Gets whether this block is aimed at the target
		/// </summary>
		bool IsAimed { get; }

		/// <summary>
		/// Sets or Gets shooting range of the turret
		/// </summary>
		float Range { get; set; }

		/// <summary>
		/// Gets whether the turret is locked onto a target
		/// </summary>
		bool HasTarget { get; }

		/// <summary>
		/// Gets whether this block has AI enabled for turret
		/// </summary>
		bool AIEnabled { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target meteors.
		/// </summary>
		bool TargetMeteors { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target missiles.
		/// </summary>
		bool TargetMissiles { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target small grids.
		/// </summary>
		bool TargetSmallGrids { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target large grids.
		/// </summary>
		bool TargetLargeGrids { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target characters.
		/// </summary>
		bool TargetCharacters { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target stations.
		/// </summary>
		bool TargetStations { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target neutrals.
		/// </summary>
		bool TargetNeutrals { get; set; }

		/// <summary>
		/// Gets or Sets if the turret should target friends.
		/// </summary>
		bool TargetFriends { get; set; }

		/// <summary>
		/// Gets or Sets rotor for the azimuth angle
		/// </summary>
		IMyMotorStator AzimuthRotor { get; set; }

		/// <summary>
		/// Gets or Sets rotor for the elevation angle
		/// </summary>
		IMyMotorStator ElevationRotor { get; set; }

		/// <summary>
		/// Gets or Sets camera for the block
		/// </summary>
		IMyCameraBlock Camera { get; set; }

		/// <summary>
		/// Gets or Sets velocity multiplier for azimuth [rpm]
		/// </summary>
		float VelocityMultiplierAzimuthRpm { get; set; }

		/// <summary>
		/// Gets or Sets velocity multiplier for elevation [rpm]
		/// </summary>
		float VelocityMultiplierElevationRpm { get; set; }

		/// <summary>
		/// Gets or Sets angle deviation
		/// </summary>
		float AngleDeviation { get; set; }

		/// <summary>
		/// Gets rotation indicator
		/// </summary>
		Vector2 RotationIndicator { get; }

		/// <summary>
		/// Gets movement indicator
		/// </summary>
		Vector3 MoveIndicator { get; }

		/// <summary>
		/// Gets roll indicator
		/// </summary>
		float RollIndicator { get; }

		/// <summary>
		/// Gets all available targeting groups 
		/// </summary>
		/// <returns>list of names of targeting groups</returns>
		List<string> GetTargetingGroups();

		/// <summary>
		/// Gets current targeting group
		/// </summary>
		/// <returns>current targeting group</returns>
		string GetTargetingGroup();

		/// <summary>
		/// Sets current targeting group
		/// </summary>
		/// <param name="groupSubtypeId">group subtype id</param>
		void SetTargetingGroup(string groupSubtypeId);

		/// <summary>
		/// Gets the turret's current detected entity, if any
		/// </summary>
		/// <returns>empty info if target does not exist</returns>
		MyDetectedEntityInfo GetTargetedEntity();

		/// <summary>
		/// Gets tools for the block
		/// </summary>
		/// <param name="tools">tools collection (weapons/tools blocks) which player can set in UI</param>
		void GetTools(List<IMyFunctionalBlock> tools);

		/// <summary>
		/// Adds tools for the block
		/// </summary>
		/// <param name="tools">tools collection (weapons/tools blocks) which player can set in UI</param>
		void AddTools(List<IMyFunctionalBlock> tools);

		/// <summary>
		/// Removes tools for the block
		/// </summary>
		/// <param name="tool">tools collection (weapons/tools blocks) which player can set in UI</param>
		void RemoveTools(List<IMyFunctionalBlock> tool);

		/// <summary>
		/// Adds the tool for the block
		/// </summary>
		/// <param name="tool">tool (weapons/tools blocks) which player can set in UI</param>
		void AddTool(IMyFunctionalBlock tool);

		/// <summary>
		/// Removes the tool for the block
		/// </summary>
		/// <param name="tool">tool (weapons/tools blocks) which player can set in UI</param>
		void RemoveTool(IMyFunctionalBlock tool);

		/// <summary>
		/// Clears tools
		/// </summary>
		void ClearTools();

		/// <summary>
		/// Get direction of shooting.
		/// </summary>
		/// <returns></returns>
		Vector3 GetShootDirection();

		/// <summary>
		/// Get block that provides direction of shooting
		/// </summary>
		/// <returns></returns>
		IMyTerminalBlock GetDirectionSource();
	}
}
