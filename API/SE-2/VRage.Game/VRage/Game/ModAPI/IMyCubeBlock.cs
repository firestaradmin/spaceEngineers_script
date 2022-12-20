using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes base block interface (mod interface)
	/// Also known as `fatblock`
	/// </summary>
	/// <see cref="T:VRage.Game.ModAPI.IMySlimBlock" />
	public interface IMyCubeBlock : VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity
	{
		/// <summary>
<<<<<<< HEAD
=======
		/// Whether the grid should call the ConnectionAllowed method for this block 
		///             (ConnectionAllowed checks mount points and other per-block requirements)
		/// </summary>
		new bool CheckConnectionAllowed { get; set; }

		/// <summary>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// Grid in which the block is placed
		/// </summary>
		new IMyCubeGrid CubeGrid { get; }

		/// <summary>
		/// Resource sink (draws power)
		/// </summary>
		/// <remarks>Cast to MyResourceSinkComponent as needed.</remarks>
		MyResourceSinkComponentBase ResourceSink { get; set; }

		/// <summary>
		/// Get all values changed by upgrade modules
		/// Should only be used as read-only
		/// </summary>
		Dictionary<string, float> UpgradeValues { get; }

		/// <summary>
		/// Gets the SlimBlock associated with this block
		/// </summary>
		IMySlimBlock SlimBlock { get; }

		event Action<IMyCubeBlock> IsWorkingChanged;

		/// <summary>
		/// Event called when upgrade values are changed
		/// Either upgrades were built or destroyed, or they become damaged or unpowered
		/// </summary>
		event Action OnUpgradeValuesChanged;

		/// <summary>
		/// Calculates local matrix, and currentModel
		/// </summary>
		/// <param name="localMatrix">Returns local matrix</param>
		/// <param name="currModel">Returns currently used model</param>
		void CalcLocalMatrix(out Matrix localMatrix, out string currModel);

		/// <summary>
		/// Calculates model currently used by block depending on its build progress and other factors
		/// </summary>
		/// <param name="orientation">Model orientation</param>
		/// <returns>Model path</returns>
		string CalculateCurrentModel(out Matrix orientation);

		/// <summary>
		/// Returns block object builder which can be serialized or added to grid
		/// </summary>
		/// <param name="copy">Set if creating a copy of block</param>
		/// <returns>Block object builder</returns>
		MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false);

		/// <summary>
		/// Reloads block model and interactive objects (doors, terminals, etc...)
		/// </summary>
		void Init();

		/// <summary>
		/// Initializes block state from object builder
		/// </summary>
		/// <param name="builder">Object builder of block (should correspond with block type)</param>
		/// <param name="cubeGrid">Owning grid</param>
		void Init(MyObjectBuilder_CubeBlock builder, IMyCubeGrid cubeGrid);

		/// <summary>
		/// Method called when a block has been built (after adding to the grid).
		/// This is called right after placing the block and it doesn't matter whether
		/// it is fully built (creative mode) or is only construction site.
		/// Note that it is not called for blocks which do not create FatBlock at that moment.
		/// </summary>
		void OnBuildSuccess(long builtBy);

		/// <summary>
		/// Method called when a block has been built (after adding to the grid).
		/// This is called right after placing the block and it doesn't matter whether
		/// it is fully built (creative mode) or is only construction site.
		/// Note that it is not called for blocks which do not create FatBlock at that moment.
		/// </summary>
		/// <param name="builtBy">IdentityId of a builder</param>
		/// <param name="instantBuild">Defines if block was instantly built. Used in blocks such piston, rotor, hinge, motor suspension to define if top part should be instantly built</param>
		void OnBuildSuccess(long builtBy, bool instantBuild);

		/// <summary>
		/// Called when block is destroyed before being removed from grid
		/// </summary>
		void OnDestroy();

		/// <summary>
		/// Called when the model referred by the block is changed
		/// </summary>
		void OnModelChange();

		/// <summary>
		/// Called at the end of registration from grid systems (after block has been registered).
		/// </summary>
		void OnRegisteredToGridSystems();

		/// <summary>
		/// Method called when user removes a cube block from grid. Useful when block
		/// has to remove some other attached block (like motors).
		/// </summary>
		void OnRemovedByCubeBuilder();

		/// <summary>
		/// Called at the end of unregistration from grid systems (after block has been unregistered).
		/// </summary>
		void OnUnregisteredFromGridSystems();

		/// <summary>
		/// Gets the name of interactive object intersected by defined line
		/// </summary>
		/// <param name="worldFrom">Line from point in world coordinates</param>
		/// <param name="worldTo">Line to point in world coordinates</param>
		/// <returns>Name of intersected detector (interactive object)</returns>
		string RaycastDetectors(Vector3D worldFrom, Vector3D worldTo);

		/// <summary>
		/// Reloads detectors (interactive objects) in model
		/// </summary>
		/// <param name="refreshNetworks">ie conveyor network</param>
		void ReloadDetectors(bool refreshNetworks = true);

		/// <summary>
		/// Start or stop damage effect on cube block
		/// </summary>
		void SetDamageEffect(bool start);

		/// <summary>
		/// Activate block effect listed in definition
		/// </summary>
		/// <param name="effectName">Name of effect</param>
		/// <param name="stopPrevious">Defines if previous effect should be stopped</param>
		/// <returns><b>true</b> if effect was started; <b>false</b> otherwise</returns>
		bool SetEffect(string effectName, bool stopPrevious = false);

		/// <summary>
		/// Activate block effect with parameters listed in definition
		/// See: Sandbox.Definitions.CubeBlockEffectBase
		/// </summary>
		/// <param name="effectName">Name of effect</param>
		/// <param name="parameter">When this value more than CubeBlockEffectBase.ParameterMin and less than CubeBlockEffectBase.ParameterMax, effect can be added</param>
		/// <param name="stopPrevious">Defines if previous effect should be stopped</param>
		/// <param name="ignoreParameter">When true effect always added</param>
		/// <param name="removeSameNameEffects">When true effect with same name are removed</param>
		/// <returns><b>true</b> if effect was started; <b>false</b> otherwise</returns>
		bool SetEffect(string effectName, float parameter, bool stopPrevious = false, bool ignoreParameter = false, bool removeSameNameEffects = false);

		/// <summary>
		/// Removes active effect set with SetEffect
		/// </summary>
		/// <param name="effectName">Name of effect</param>
		/// <param name="exception">Index of effect that should be ignored. Starting from 0, use -1 to remove all effects with specified name</param>
		/// <returns>The number of effects removed</returns>
		int RemoveEffect(string effectName, int exception = -1);

		/// <summary>
		/// Preferred way of registering a block for upgrades
		/// Adding directly to the dictionary can have unintended consequences
		/// when multiple mods are involved.
		/// </summary>
		/// <param name="upgrade">Name of upgrade. Example: Productivity/PowerEfficiency/Effectiveness</param>
		/// <param name="defaultValue">Default value for this upgrade</param>
		void AddUpgradeValue(string upgrade, float defaultValue);
	}
}
