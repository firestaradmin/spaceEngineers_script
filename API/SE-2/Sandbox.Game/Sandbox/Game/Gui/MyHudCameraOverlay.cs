using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
<<<<<<< HEAD
	public class MyHudCameraOverlay : MyGuiControlBase
=======
	internal class MyHudCameraOverlay : MyGuiControlBase
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		private static string m_textureName;

		private static bool m_enabled;

		public static string TextureName
		{
			get
			{
				return m_textureName;
			}
			set
			{
				m_textureName = value;
			}
		}

		public new static bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					m_enabled = value;
				}
			}
		}

		public MyHudCameraOverlay()
		{
<<<<<<< HEAD
			if (MySession.Static != null)
			{
				Enabled = MySession.Static.CameraController is MyLargeTurretBase || MySession.Static.CameraController is MyCameraBlock;
			}
			else
			{
				Enabled = false;
			}
=======
			Enabled = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			if (Enabled)
			{
				DrawFullScreenSprite();
			}
		}

		private static void DrawFullScreenSprite()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			RectangleF destination = new RectangleF(fullscreenRectangle.X, fullscreenRectangle.Y, fullscreenRectangle.Width, fullscreenRectangle.Height);
			MyRenderProxy.DrawSprite(m_textureName, ref destination, null, Color.White, 0f, ignoreBounds: true, waitTillLoaded: true);
		}
	}
}
