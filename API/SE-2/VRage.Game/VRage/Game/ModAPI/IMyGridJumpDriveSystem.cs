using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Interface for MyGridJumpDriveSystem in IMyCubeGrid
	/// </summary>
	public interface IMyGridJumpDriveSystem
	{
		/// <summary>
		/// True if the grid is in the animation to jump
		/// </summary>
		bool IsJumping { get; }

		/// <summary>
		/// True when the grid finally teleports
		/// </summary>
		bool Jumped { get; }

		/// <summary>
		/// Gets the direction vector the jump will occur (includes magnitude)
		/// </summary>
		Vector3D? GetJumpDriveDirection();

		/// <summary>
		/// Gets the normalized direction the jump will occur
		/// </summary>
		Vector3D? GetJumpDriveDirectionNormalized();

		/// <summary>
		/// Gets the target position the jump is attempting to reach
		/// </summary>
		Vector3D? GetJumpDriveTarget();

		/// <summary>
		/// Gets the time until the jump finally occurs
		/// </summary>
		float? GetRemainingJumpTime();

		/// <summary>
		/// Gets the minimum possible jump distance with the current loadout
		/// </summary>
		/// <param name="userId">IdentityId, that using using jump drive</param>
		double GetMinJumpDistance(long userId);

		/// <summary>
		/// Gets the maximum possible jump distance with the current loadout
		/// </summary>
		/// <param name="userId">IdentityId, that using using jump drive</param>
		double GetMaxJumpDistance(long userId);

		/// <summary>
		/// True if the grid could jump (does not check obstacles)
		/// </summary>
		/// <param name="userId">IdentityId, that using using jump drive</param>
		bool IsJumpValid(long userId);

		/// <summary>
		/// Gets a safe position at the desired location (not overlapping stuff)
		/// </summary>
		Vector3D? FindSuitableJumpLocation(Vector3D desiredLocation);

		/// <summary>
		/// Makes the grid begin the jumping sequence (Call on server only!)
		/// </summary>
		/// <param name="jumpTarget">World coordinates of place where you jumping</param>
		/// <param name="userId">IdentityId, that using using jump drive</param>
		/// <param name="jumpDriveDelay">Delay in seconds before jump</param>
		void Jump(Vector3D jumpTarget, long userId, float jumpDriveDelay = 10f);

		/// <summary>
		/// Performs a jump without the delay or animation (Call on server only!)
		/// </summary>
		void PerformJump(Vector3D jumpTarget);

		/// <summary>
		/// Requests the pilot or local player to jump (Prompts the pilot with the jump UI, they can cancel the jump or say yes)
		/// </summary>
		/// <param name="destinationName">Name of place where you jumping</param>
		/// <param name="destination">World coordinates of place where you jumping</param>
		/// <param name="userId">IdentityId, that using using jump drive</param>
		/// <param name="shipBox">Bounding box of ship in world coordinates that would be used in gravity check</param>
		/// <param name="jumpDriveDelay">Delay in seconds before jump</param>
		void RequestJump(string destinationName, Vector3D destination, long userId, BoundingBoxD? shipBox = null, float jumpDriveDelay = 10f);

		/// <summary>
		/// Requests the pilot or local player to jump (Prompts the pilot with the jump UI, they can cancel the jump or say yes)
		/// </summary>
		/// <param name="jumpTarget">World coordinates of place where you jumping</param>
		/// <param name="userId">IdentityId, that using using jump drive</param>
		/// <param name="jumpDriveDelay">Delay in seconds before jump</param>
		void RequestJump(Vector3D jumpTarget, long userId, float jumpDriveDelay = 10f);

		/// <summary>
		/// Ends the jump for the provided reason
		/// 0 = None, 1 = Static, 2 = Locked, 3 = ShortDistance, 4 = AlreadyJumping, 5 = NoLocation, 6 = Other
		/// </summary>
		void AbortJump(int reason = 0);
	}
}
