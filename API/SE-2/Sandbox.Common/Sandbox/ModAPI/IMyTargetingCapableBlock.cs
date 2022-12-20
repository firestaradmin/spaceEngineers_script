using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes block that can target (cockpits and turrets) (mods interface)
	/// </summary>
	public interface IMyTargetingCapableBlock
	{
		/// <summary>
		/// Gets if target locking is enabled 
		/// </summary>
		/// <returns>True if enabled</returns>
		bool IsTargetLockingEnabled();

		/// <summary>
		/// Sets locked target grid
		/// </summary>
		/// <remarks>Should be called only for <see cref="T:Sandbox.ModAPI.IMyShipController" />. Other implementations has mock logic inside</remarks>        
		/// <param name="target">Target grid</param>
		void SetLockedTarget(IMyCubeGrid target);

		/// <summary>
		/// Gets barrel or block world matrix  
		/// </summary>
		/// <returns>Barrel or block world matrix</returns>
		MatrixD GetWorldMatrix();

		/// <summary>
		/// Get whether toolbar selected tool can shoot
		/// </summary>
		/// <remarks>Should be called only for <see cref="T:Sandbox.ModAPI.IMyShipController" />. Other implementations has mock logic inside</remarks>
		/// <returns>Whether can shoot</returns>
		bool CanActiveToolShoot();

		/// <summary>
		/// Get whether toolbar selected item is ship tool (drill, grinder, welder)
		/// </summary>
		/// <remarks>Should be called only for <see cref="T:Sandbox.ModAPI.IMyShipController" />. Other implementations has mock logic inside</remarks>
		/// <returns>Whether tool is selected</returns>
		bool IsShipToolSelected();

		/// <summary>
		/// Gets average position 
		/// </summary>
		/// <returns></returns>
		/// <remarks>Should be called only for <see cref="T:Sandbox.ModAPI.IMyShipController" />. Other implementations has mock logic inside</remarks>
		Vector3D GetActiveToolPosition();
	}
}
