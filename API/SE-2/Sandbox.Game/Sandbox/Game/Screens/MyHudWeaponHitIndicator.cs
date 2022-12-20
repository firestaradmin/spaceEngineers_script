<<<<<<< HEAD
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
=======
using Sandbox.Game.World;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyHudWeaponHitIndicator
	{
		private readonly int ANIMATION_DURATION = 15;

		private readonly int MAX_VISIBLE_TIME = 15;

		private readonly float ANIMATION_SCALE = 1.5f;

<<<<<<< HEAD
		private readonly Vector2 m_center = new Vector2(0.5f);
=======
		private readonly Vector2 m_position = new Vector2(0.5f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private readonly Vector2 m_baseSize = new Vector2(1f, 1.33333337f) * 0.025f;

		private float m_size;

		private Color m_color;

		private int m_visibleTime = 60;

<<<<<<< HEAD
=======
		private bool m_isVisible = true;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGuiControlImage GuiControlImage { get; private set; }

		public MyHudWeaponHitIndicator()
		{
<<<<<<< HEAD
			GuiControlImage = new MyGuiControlImage(m_center, m_baseSize)
=======
			GuiControlImage = new MyGuiControlImage(m_position, m_baseSize)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Visible = false
			};
		}

		private void SetColor(Color hitColor)
		{
			m_color = hitColor;
			GuiControlImage.ColorMask = hitColor;
		}

		public void Hit(MySession.MyHitIndicatorTarget target)
		{
			MyGuiSizedTexture center;
			switch (target)
			{
			case MySession.MyHitIndicatorTarget.Character:
			{
				GuiControlImage.ColorMask = MySandboxGame.Config.HitIndicatorColorCharacter;
				MyGuiControlImage guiControlImage5 = GuiControlImage;
				MyGuiCompositeTexture myGuiCompositeTexture5 = new MyGuiCompositeTexture();
				center = new MyGuiSizedTexture
				{
					Texture = MySandboxGame.Config.HitIndicatorTextureCharacter
				};
				myGuiCompositeTexture5.Center = center;
				guiControlImage5.BackgroundTexture = myGuiCompositeTexture5;
				break;
			}
			case MySession.MyHitIndicatorTarget.Grid:
			{
				GuiControlImage.ColorMask = MySandboxGame.Config.HitIndicatorColorGrid;
				MyGuiControlImage guiControlImage4 = GuiControlImage;
				MyGuiCompositeTexture myGuiCompositeTexture4 = new MyGuiCompositeTexture();
				center = new MyGuiSizedTexture
				{
					Texture = MySandboxGame.Config.HitIndicatorTextureGrid
				};
				myGuiCompositeTexture4.Center = center;
				guiControlImage4.BackgroundTexture = myGuiCompositeTexture4;
				break;
			}
			case MySession.MyHitIndicatorTarget.Friend:
			{
				GuiControlImage.ColorMask = MySandboxGame.Config.HitIndicatorColorFriend;
				MyGuiControlImage guiControlImage3 = GuiControlImage;
				MyGuiCompositeTexture myGuiCompositeTexture3 = new MyGuiCompositeTexture();
				center = new MyGuiSizedTexture
				{
					Texture = MySandboxGame.Config.HitIndicatorTextureFriend
				};
				myGuiCompositeTexture3.Center = center;
				guiControlImage3.BackgroundTexture = myGuiCompositeTexture3;
				break;
			}
			case MySession.MyHitIndicatorTarget.Headshot:
			{
				GuiControlImage.ColorMask = MySandboxGame.Config.HitIndicatorColorHeadshot;
				MyGuiControlImage guiControlImage2 = GuiControlImage;
				MyGuiCompositeTexture myGuiCompositeTexture2 = new MyGuiCompositeTexture();
				center = new MyGuiSizedTexture
				{
					Texture = MySandboxGame.Config.HitIndicatorTextureHeadshot
				};
				myGuiCompositeTexture2.Center = center;
				guiControlImage2.BackgroundTexture = myGuiCompositeTexture2;
				break;
			}
			case MySession.MyHitIndicatorTarget.Kill:
			{
				GuiControlImage.ColorMask = MySandboxGame.Config.HitIndicatorColorKill;
				MyGuiControlImage guiControlImage = GuiControlImage;
				MyGuiCompositeTexture myGuiCompositeTexture = new MyGuiCompositeTexture();
				center = new MyGuiSizedTexture
				{
					Texture = MySandboxGame.Config.HitIndicatorTextureKill
				};
				myGuiCompositeTexture.Center = center;
				guiControlImage.BackgroundTexture = myGuiCompositeTexture;
				break;
			}
			}
			m_visibleTime = 0;
		}

		public void Update()
		{
			bool flag = m_visibleTime < MAX_VISIBLE_TIME;
			GuiControlImage.Visible = flag;
			if (flag)
			{
<<<<<<< HEAD
				GuiControlImage.Position = Convert(MyHud.Crosshair.Position);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_visibleTime++;
				if (m_visibleTime < ANIMATION_DURATION)
				{
					float num = 1f - (float)m_visibleTime / (float)ANIMATION_DURATION;
					m_size = 1f + (ANIMATION_SCALE - 1f) * num * num;
				}
				GuiControlImage.Size = m_size * m_baseSize;
			}
		}
<<<<<<< HEAD

		private Vector2 Convert(Vector2 v)
		{
			float num = (float)MyGuiManager.GetSafeFullscreenRectangle().Width / (float)MyGuiManager.GetSafeGuiRectangle().Width;
			float num2 = 1f / MyGuiManager.GetHudSize().Y;
			return new Vector2((v.X - 0.5f) * num + 0.5f, v.Y * num2);
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
