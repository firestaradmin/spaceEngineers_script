using VRage.Game.Entity;
using VRage.Game.Utils;
using VRageMath;

namespace VRage.Game.ModAPI.Interfaces
{
	/// <summary>
	/// Interface to control game camera (not block) (mods interface)
	/// </summary>
	public interface IMyCameraController
	{
		/// <summary>
		/// Gets or sets if the current camera view is first person.
		/// </summary>
		bool IsInFirstPersonView { get; set; }

<<<<<<< HEAD
		/// <summary>
		/// Gets or sets if player can use only first person view
		/// </summary>
		bool ForceFirstPersonCamera { get; set; }

		/// <summary>
		/// Gets or set if player can use first person view
		/// </summary>
		bool EnableFirstPersonView { get; set; }

		/// <summary>
		/// Gets if player block building enabled
		/// </summary>
		bool AllowCubeBuilding { get; }

		/// <summary>
		/// Gets Entity to which it is attached 
		/// </summary>
=======
		bool ForceFirstPersonCamera { get; set; }

		bool EnableFirstPersonView { get; set; }

		bool AllowCubeBuilding { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		MyEntity Entity { get; }

		/// <summary>
		/// Change camera properties now.
		/// Communication: from controller to camera.
		/// </summary>
		/// <param name="currentCamera"></param>
		void ControlCamera(MyCamera currentCamera);

		/// <summary>
		/// Rotate camera controller.
		/// </summary>
		void Rotate(Vector2 rotationIndicator, float rollIndicator);

		/// <summary>
		/// Rotation of camera controller stopped.
		/// </summary>
		void RotateStopped();

		/// <summary>
		/// Called when it is setted as new main camera controller
		/// </summary>
		/// <param name="previousCameraController">Previous camera controller</param>
		void OnAssumeControl(IMyCameraController previousCameraController);

		/// <summary>
		/// Called when new camera controller setted, and this was main camera controller
		/// </summary>
		/// <param name="newCameraController">New main camera controller</param>
		void OnReleaseControl(IMyCameraController newCameraController);

		/// <summary>
		/// Used to send "use" commands to camera controller
		/// </summary>
		/// <returns>
		/// Return value indicates if the camera controller handled the use button.
		/// If not, it should fall to ControlledObject
		/// </returns>
		bool HandleUse();

		/// <summary>
		/// Does nothing. 
		/// </summary>
		/// <returns>Always returns false</returns>
		bool HandlePickUp();

		/// <summary>
		/// Transformation that should be used for target selection while focusing
		/// </summary>
		/// <returns></returns>
		MatrixD? GetOverridingFocusMatrix();
	}
}
