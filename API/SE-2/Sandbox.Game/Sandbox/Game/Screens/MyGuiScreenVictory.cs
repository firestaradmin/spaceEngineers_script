using System;
using System.IO;
using System.Text;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenVictory : MyGuiScreenBase
	{
		private string m_factionTag = string.Empty;

		private MyFaction m_faction;

		private float m_duration;

		private MyGuiControlImage m_image;

		private MyGuiControlLabel m_caption;

		private TimeSpan m_appearedAt;

		public MyGuiScreenVictory()
			: base(new Vector2(0.5f, 0.25f), 0.35f * Color.Yellow.ToVector4(), new Vector2(0.6f, 0.35f), isTopMostScreen: true)
		{
			m_appearedAt = MySession.Static.ElapsedGameTime;
			m_closeOnEsc = true;
			m_drawEvenWithoutFocus = true;
			m_isTopMostScreen = false;
			m_isTopScreen = true;
			base.CanHaveFocus = false;
			base.CanBeHidden = false;
			base.CanHideOthers = false;
			base.BackgroundColor = Color.Transparent;
			base.EnabledBackgroundFade = false;
			base.CloseButtonEnabled = false;
			RecreateControls(constructor: true);
		}

		public void SetWinner(string factionTag)
		{
			m_factionTag = factionTag;
			m_faction = MySession.Static.Factions.TryGetFactionByTag(factionTag);
			if (m_faction != null)
			{
				MyStringId? factionIcon = m_faction.FactionIcon;
				_ = m_faction.IconColor;
				Vector3 customColor = m_faction.CustomColor;
				string name = m_faction.Name;
				m_caption.Text = string.Format(MyTexts.Get(MySpaceTexts.ScreenVictory_Title).ToString(), name);
				m_caption.TextScale = MyGuiControlAutoScaleText.GetScale(m_caption.Font, new StringBuilder(m_caption.Text), m_size.Value * 0.8f, m_caption.TextScale, 0f);
				m_image.Textures[0].Texture = Path.Combine(MyFileSystem.ContentPath, factionIcon.Value.String);
				m_image.Textures[0].ColorMask = MyColorPickerConstants.HSVOffsetToHSV(customColor).HSVtoColor();
			}
		}

		public void SetDuration(float timeInSec)
		{
			m_duration = timeInSec;
		}

		public override bool RegisterClicks()
		{
			return false;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenVictory";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_caption = AddCaption("", Color.White);
			m_caption.TextScale = 3f;
			float num = m_size.Value.Y * 0.5f;
			m_image = new MyGuiControlImage
			{
				Size = new Vector2(num, num * MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y)
			};
			m_image.SetTextures(new MyGuiControlImage.MyDrawTexture[1]
			{
				new MyGuiControlImage.MyDrawTexture
				{
					ColorMask = Vector4.One,
					Texture = ""
				}
			});
			m_image.Position = 0.1f * m_size.Value.Y * Vector2.UnitY;
			m_image.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			Elements.Add(m_image);
		}

		public static void AttachToBottomCenterOf(MyGuiControlBase leftView, MyGuiControlBase rightView, Vector2 margin)
		{
			Vector2 vector2 = (leftView.Position = rightView.GetPositionAbsoluteCenter() + margin);
			leftView.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
		}

		public override bool Draw()
		{
			MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER.Draw(m_position - base.Size.Value / 2f, base.Size.Value, Color.White * 0.45f);
			return base.Draw();
		}

		public override bool Update(bool hasFocus)
		{
			base.Update(hasFocus);
			if ((MySession.Static.ElapsedGameTime - m_appearedAt).TotalSeconds > (double)m_duration || m_faction == null)
			{
				CloseScreenNow();
			}
			return false;
		}
	}
}
