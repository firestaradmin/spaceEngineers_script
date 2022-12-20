using VRageMath;

namespace VRageRender
{
	internal class MyEnvironmentMatrices
	{
		internal Vector3D CameraPosition;

		internal Matrix ViewAt0;

		internal Matrix InvViewAt0;

		internal Matrix ViewProjectionAt0;

		internal Matrix InvViewProjectionAt0;

		internal Matrix Projection;

		internal Matrix ProjectionForSkybox;

		internal Matrix InvProjection;

		internal MatrixD ViewD;

		internal MatrixD InvViewD;

		internal Matrix OriginalProjection;

		internal Matrix OriginalProjectionFar;

		internal MatrixD ViewProjectionD;

		internal MatrixD InvViewProjectionD;

		internal BoundingFrustumD ViewFrustumClippedD;

		internal BoundingFrustumD ViewFrustumClippedFarD;

		internal float NearClipping;

		internal float LargeDistanceFarClipping;

		internal float FarClipping;

		/// <summary>Field of view Horizontal</summary>
		internal float FovH;

		/// <summary>Field of view Vertical</summary>
		internal float FovV;

		/// <summary>
		/// Whether the last matrix update was smooth.
		/// </summary>
		internal bool LastUpdateWasSmooth;
	}
}
