using VRageMath;

namespace VRageRender
{
	internal class MyProjectionInfo
	{
		internal MatrixD WorldToProjection;

		internal MatrixD LocalToProjection;

		internal MatrixD LocalToProjectionExtruded;

		internal Vector3D WorldCameraOffsetPosition;

		internal Matrix Projection;

		internal Vector3D ViewOrigin;

		internal MatrixD CurrentLocalToProjection => MatrixD.CreateTranslation(MyRender11.Environment.Matrices.CameraPosition - WorldCameraOffsetPosition) * LocalToProjection;

		internal MatrixD CurrentLocalToProjectionExtruded => MatrixD.CreateTranslation(MyRender11.Environment.Matrices.CameraPosition - WorldCameraOffsetPosition) * LocalToProjectionExtruded;
	}
}
