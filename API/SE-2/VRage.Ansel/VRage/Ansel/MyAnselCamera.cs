using System;
using VRageMath;

namespace VRage.Ansel
{
	internal struct MyAnselCamera
	{
		private Quaternion m_initQuaternion;

		private Quaternion m_anselQuaterion;

		public Matrix ProjectionMatrix;

		public Matrix ProjectionFarMatrix;

		public float FOV;

		public float NearPlane;

		public float FarPlane;

		public Vector2 ProjectionOffset;

		private Vector3D m_initPosition;

		private Vector3 m_anselPosition;

		public float FarFarPlane;

		public Vector3D Position => m_initPosition + Vector3D.Transform(m_anselPosition, Quaternion.Inverse(m_initQuaternion));

		public MatrixD ViewMatrix => MatrixD.CreateTranslation(-Position) * MatrixD.CreateFromQuaternion(Quaternion.Inverse(m_anselQuaterion) * m_initQuaternion);

		private Quat QuaterionToNvQuat(Quaternion quaternion)
		{
			Quat result = default(Quat);
			result.x = quaternion.X;
			result.y = quaternion.Y;
			result.z = quaternion.Z;
			result.w = quaternion.W;
			return result;
		}

		private Quaternion NvQuatToQuaterion(Quat quat)
		{
			return new Quaternion(quat.x, quat.y, quat.z, quat.w);
		}

		public MyAnselCamera(MatrixD viewMatrix, float fov, float aspectRatio, float nearPlane, float farPlane, float farFarPlane, Vector3D position, float projectionOffsetX, float projectionOffsetY)
		{
			m_initQuaternion = Quaternion.CreateFromRotationMatrix(in viewMatrix);
			m_anselQuaterion = Quaternion.Identity;
			MatrixD m = MatrixD.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);
			ProjectionMatrix = m;
			m = MatrixD.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farFarPlane);
			ProjectionFarMatrix = m;
			FOV = fov;
			NearPlane = nearPlane;
			FarPlane = farPlane;
			FarFarPlane = farFarPlane;
			m_initPosition = position;
			m_anselPosition = Vector3.Zero;
			ProjectionOffset = new Vector2(projectionOffsetX, projectionOffsetY);
		}

		public void Update(bool spectator)
		{
			Camera camera = default(Camera);
			camera.rotation = QuaterionToNvQuat(m_anselQuaterion);
			camera.fov = FOV / (float)Math.PI * 180f;
			camera.position = new Vec3(m_anselPosition.X, m_anselPosition.Y, m_anselPosition.Z);
			Camera camera2 = camera;
			NativeMethods.updateCamera(ref camera2);
			m_anselQuaterion = NvQuatToQuaterion(camera2.rotation);
			if (spectator)
			{
				m_anselPosition = new Vector3(camera2.position.x, camera2.position.y, camera2.position.z);
			}
			ProjectionOffset = new Vector2(camera2.projectionOffsetX, camera2.projectionOffsetY);
			ProjectionMatrix.M31 = ProjectionOffset.X;
			ProjectionMatrix.M32 = ProjectionOffset.Y;
			FOV = camera2.fov * (float)Math.PI / 180f;
		}
	}
}
