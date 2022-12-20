using System;
using VRage.Library.Utils;
using VRage.Render11.Common;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Ansel
{
	internal class MyAnselRenderManager : IManager
	{
		private MyTimeSpan m_updateTime;

		private Vector2 m_tilesPerScreenshot;

		public ulong FramesSinceStartOfCapturing;

		private IAnsel m_ansel;

		private readonly MyRenderMessageSetCameraViewMatrix m_cameraSetup = new MyRenderMessageSetCameraViewMatrix();

		public bool IsMultiresCapturing => m_ansel.IsMultiresCapturing;

		public bool Is360Capturing => m_ansel.Is360Capturing;

		public bool IsSessionRunning => m_ansel.IsSessionRunning;

		public bool IsCaptureRunning => m_ansel.IsCaptureRunning;

		public MyAnselRenderManager()
		{
			m_ansel = MyVRage.Platform.Ansel;
			m_ansel.StartCaptureDelegate += StartCapture;
			m_ansel.StopCaptureDelegate += StopCapture;
		}

		public void SetCurrentCamera(MatrixD viewMatrix, float fov, float aspectRatio, float nearPlane, float farPlane, float farFarPlane, Vector3D position)
		{
			if (!m_ansel.IsSessionRunning && m_ansel.IsInitializedSuccessfuly)
			{
				MyCameraSetup myCameraSetup = default(MyCameraSetup);
				myCameraSetup.AspectRatio = aspectRatio;
				myCameraSetup.FarPlane = farFarPlane;
				myCameraSetup.NearPlane = nearPlane;
				myCameraSetup.FOV = fov;
				myCameraSetup.Position = position;
				myCameraSetup.ViewMatrix = viewMatrix;
				MyCameraSetup cameraSetup = myCameraSetup;
				m_ansel.SetCamera(ref cameraSetup);
			}
		}

		public void SetUpdateTime(MyTimeSpan updateTime)
		{
			m_updateTime = updateTime;
		}

		private MyRenderMessageSetCameraViewMatrix UpdateCameraViewMatrixMessage()
		{
			m_ansel.GetCamera(out var cameraSetup);
			m_cameraSetup.ViewMatrix = cameraSetup.ViewMatrix;
			m_cameraSetup.CameraPosition = cameraSetup.Position;
			m_cameraSetup.FarPlane = cameraSetup.FarPlane;
			m_cameraSetup.FarFarPlane = cameraSetup.FarPlane;
			m_cameraSetup.FOV = cameraSetup.FOV;
			m_cameraSetup.FOVForSkybox = cameraSetup.FOV;
			m_cameraSetup.LastMomentUpdateIndex = -1;
			m_cameraSetup.NearPlane = cameraSetup.NearPlane;
			m_cameraSetup.ProjectionMatrix = cameraSetup.ProjectionMatrix;
			m_cameraSetup.ProjectionFarMatrix = cameraSetup.ProjectionMatrix;
			m_cameraSetup.UpdateTime = m_updateTime;
			m_cameraSetup.ProjectionOffsetX = cameraSetup.ProjectionOffsetX;
			m_cameraSetup.ProjectionOffsetY = cameraSetup.ProjectionOffsetY;
			return m_cameraSetup;
		}

		public void BeginDrawScene()
		{
			if (FramesSinceStartOfCapturing != 0L)
			{
				UpdateCameraViewMatrixMessage();
				MyRender11.SetupCameraMatrices(m_cameraSetup);
			}
			MyCommon.UpdateFrameConstants();
			if (m_ansel.Is360Capturing && FramesSinceStartOfCapturing == 0L)
			{
				MyBillboardRenderer.ClearBillboardsOnce();
			}
		}

		public void EndDrawScene()
		{
			FramesSinceStartOfCapturing++;
		}

		public void StartCapture(int captureType)
		{
			FramesSinceStartOfCapturing = 0uL;
		}

		public void StopCapture()
		{
		}

		public MyViewport GetSpriteViewport()
		{
			Vector2 vector = new Vector2(m_cameraSetup.ProjectionOffsetX, m_cameraSetup.ProjectionOffsetY);
			if (m_ansel.IsCaptureRunning)
			{
				if (FramesSinceStartOfCapturing == 1)
				{
					m_tilesPerScreenshot = new Vector2(Math.Abs(m_cameraSetup.ProjectionOffsetX) + 1f, Math.Abs(m_cameraSetup.ProjectionOffsetY) + 1f);
				}
			}
			else
			{
				m_tilesPerScreenshot = Vector2.One;
			}
			return new MyViewport(0f - vector.X - m_tilesPerScreenshot.X, vector.Y - m_tilesPerScreenshot.Y, m_tilesPerScreenshot.X * 2f, m_tilesPerScreenshot.Y * 2f);
		}

		public void EnableAnsel()
		{
			m_ansel.Enable();
		}

		public void MarkHdrBufferBind()
		{
			m_ansel.MarkHdrBufferBind();
		}

		public void MarkHdrBufferFinished()
		{
			m_ansel.MarkHdrBufferFinished();
		}
	}
}
