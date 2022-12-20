using System;
using VRageMath;

namespace VRage.ModAPI
{
	/// <summary>
	/// Describes camera (mods interface)
	/// </summary>
	public interface IMyCamera
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets camera position
		/// </summary>
		Vector3D Position { get; }

		/// <summary>
		/// Gets camera previous position
		/// </summary>
		Vector3D PreviousPosition { get; }

		/// <summary>
		/// Gets camera viewport offset
		/// </summary>
		Vector2 ViewportOffset { get; }

		/// <summary>
		/// Gets camera viewport size
		/// </summary>
		Vector2 ViewportSize { get; }

		/// <summary>
		/// Gets view matrix when camera in real position
		/// </summary>
		MatrixD ViewMatrix { get; }

		/// <summary>
		/// Gets camera world matrix
		/// </summary>
		MatrixD WorldMatrix { get; }

		/// <summary>
		/// Gets projection matrix
		/// </summary>
		MatrixD ProjectionMatrix { get; }

		/// <summary>
		/// Gets near plane distance
		/// </summary>
		float NearPlaneDistance { get; }

		/// <summary>
		/// Gets farplane is set by MyObjectBuilder_SessionSettings.ViewDistance
		/// </summary>
		float FarPlaneDistance { get; }

		/// <summary>
		/// Gets field of view angle in degrees
		/// </summary>
		float FieldOfViewAngle { get; }

		/// <summary>
		/// Gets field of view with zoom
		/// </summary>
=======
		Vector3D Position { get; }

		Vector3D PreviousPosition { get; }

		Vector2 ViewportOffset { get; }

		Vector2 ViewportSize { get; }

		MatrixD ViewMatrix { get; }

		MatrixD WorldMatrix { get; }

		MatrixD ProjectionMatrix { get; }

		float NearPlaneDistance { get; }

		float FarPlaneDistance { get; }

		float FieldOfViewAngle { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float FovWithZoom { get; }

		[Obsolete]
		MatrixD ProjectionMatrixForNearObjects { get; }

		[Obsolete]
		float FieldOfViewAngleForNearObjects { get; }

		[Obsolete]
		float FovWithZoomForNearObjects { get; }

		/// <summary>
		/// Gets distance from point. Equals to <see cref="M:VRageMath.Vector3D.Distance(VRageMath.Vector3D,VRageMath.Vector3D)" />
		/// </summary>
		/// <param name="position">Another point</param>
		/// <returns>Distance in meters</returns>
		double GetDistanceWithFOV(Vector3D position);

		/// <summary>
		/// Checks if specified bounding box is in actual bounding frustum
		/// IMPORTANT: If you observe bad result of this test, check how you transform your bounding box.
		/// Don't use BoundingBox.Transform. Instead transform box manualy and then create new box.
		/// </summary>
		/// <param name="boundingBox">Bounding box to check</param>
		/// <returns>Whether specified bounding box is in actual bounding frustum</returns>
		bool IsInFrustum(ref BoundingBoxD boundingBox);

		/// <summary>
		/// Checks if specified bounding sphere is in actual bounding frustum
		/// IMPORTANT: If you observe bad result of this test, check how you transform your bounding sphere.
		/// Don't use BoundingSphere.Transform. Instead transform sphere center manualy and then create new sphere.
		/// </summary>
		/// <param name="boundingSphere"></param>
		/// <returns>Whether specified bounding box is in actual bounding frustum</returns>
		bool IsInFrustum(ref BoundingSphereD boundingSphere);

		/// <summary>
		/// Checks if specified bounding box is in actual bounding frustum
		/// IMPORTANT: If you observe bad result of this test, check how you transform your bounding box.
		/// Don't use BoundingBox.Transform. Instead transform box manualy and then create new box.
		/// </summary>
		/// <param name="boundingBox">Bounding box to check</param>
		/// <returns>Whether specified bounding box is in actual bounding frustum</returns>
		bool IsInFrustum(BoundingBoxD boundingBox);

		/// <summary>
		/// Gets screen coordinates of 3d world pos in 0 - 1 distance where 1.0 is screen width(for X) or height(for Y).
		/// WARNING: Y is from bottom to top.
		/// </summary>
		/// <param name="worldPos">World position.</param>
		/// <returns>Screen coordinate in 0-1 distance.</returns>
		Vector3D WorldToScreen(ref Vector3D worldPos);

		/// <summary>
		/// Gets normalized world space line from screen space coordinates.
		/// </summary>
		/// <param name="screenCoords"></param>
		/// <returns>Gets normalized world space line</returns>
		LineD WorldLineFromScreen(Vector2 screenCoords);
	}
}
