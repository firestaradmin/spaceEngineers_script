using System.Collections.Generic;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlContentButton : MyGuiControlRadioButton
	{
		private struct MyControlImages
		{
			public MyGuiControlImage Normal;

			public MyGuiControlImage Highlight;

			public MyGuiControlImage Focus;

			public MyGuiControlImage Active;
		}

		private readonly MyGuiControlLabel m_titleLabel;

		private MyGuiControlImage m_previewImage;

		private string m_previewImagePath;

		private readonly MyControlImages m_workshopSteamImages;

		private readonly MyControlImages m_workshopModioImages;

		private readonly MyControlImages m_localmodImages;

		private readonly MyControlImages m_cloudmodImages;

		public bool FocusHighlightAlsoSelects;

		private readonly List<MyGuiControlImage> m_dlcIcons;

		private string m_workshopServiceName;

		private readonly MyGuiCompositeTexture m_noThumbnailTexture = new MyGuiCompositeTexture("Textures\\GUI\\Icons\\Blueprints\\NoThumbnailFound.png");

		private readonly Color m_noThumbnailColor = new Color(94, 115, 127);

		private MyBlueprintTypeEnum m_modType = MyBlueprintTypeEnum.DEFAULT;

<<<<<<< HEAD
		/// <summary>
		/// Changes titlebar content.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string Title => m_titleLabel.Text;

		public string PreviewImagePath => m_previewImagePath;

		private MyControlImages? GetControlImages()
		{
<<<<<<< HEAD
			switch (m_modType)
			{
			case MyBlueprintTypeEnum.LOCAL:
				return m_localmodImages;
			case MyBlueprintTypeEnum.CLOUD:
				return m_cloudmodImages;
			case MyBlueprintTypeEnum.WORKSHOP:
				return (m_workshopServiceName == "mod.io") ? m_workshopModioImages : m_workshopSteamImages;
			default:
				return null;
			}
=======
			return m_modType switch
			{
				MyBlueprintTypeEnum.LOCAL => m_localmodImages, 
				MyBlueprintTypeEnum.CLOUD => m_cloudmodImages, 
				MyBlueprintTypeEnum.WORKSHOP => (m_workshopServiceName == "mod.io") ? m_workshopModioImages : m_workshopSteamImages, 
				_ => null, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void SetIconType(MyBlueprintTypeEnum modType, string serviceName)
		{
			MyControlImages? controlImages = GetControlImages();
			if (controlImages?.Normal != null)
			{
				Elements.Remove(controlImages.Value.Normal);
				Elements.Remove(controlImages.Value.Highlight);
				Elements.Remove(controlImages.Value.Focus);
				Elements.Remove(controlImages.Value.Active);
			}
			m_workshopServiceName = serviceName;
			m_modType = modType;
			controlImages = GetControlImages();
			if (controlImages?.Normal != null)
			{
				Elements.Add(base.HasHighlight ? controlImages.Value.Highlight : (base.HasFocus ? controlImages.Value.Focus : (base.Selected ? controlImages.Value.Active : controlImages.Value.Normal)));
			}
		}

		public MyGuiControlContentButton(string title, string imagePath, string dlcIcon = null)
		{
			m_dlcIcons = new List<MyGuiControlImage>();
			base.VisualStyle = MyGuiControlRadioButtonStyleEnum.ScenarioButton;
			base.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_titleLabel = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = title
			};
			m_workshopSteamImages = CreateControlImages(MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM);
			m_workshopModioImages = CreateControlImages(MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_MOD_IO);
			m_localmodImages = CreateControlImages(MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL);
			m_cloudmodImages = CreateControlImages(MyGuiConstants.TEXTURE_ICON_MODS_CLOUD);
			m_previewImagePath = imagePath;
			if (!string.IsNullOrEmpty(dlcIcon))
			{
				AddDlcIcon(dlcIcon);
			}
			CreatePreview(imagePath);
			m_titleLabel.IsAutoScaleEnabled = true;
			m_titleLabel.IsAutoEllipsisEnabled = true;
			m_titleLabel.SetMaxWidth(m_previewImage.Size.X - 0.01f);
			Elements.Add(m_titleLabel);
			base.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_ContentButton;
		}

		public void SetModType(MyBlueprintTypeEnum modType, string serviceName)
		{
			SetIconType(modType, serviceName);
		}

		private MyControlImages CreateControlImages(MyGuiHighlightTexture textures)
		{
			MyControlImages result = default(MyControlImages);
			result.Normal = new MyGuiControlImage(null, null, null, null, new string[1] { textures.Normal })
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = textures.SizeGui
			};
			result.Highlight = new MyGuiControlImage(null, null, null, null, new string[1] { textures.Highlight })
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = textures.SizeGui
			};
			result.Focus = new MyGuiControlImage(null, null, null, null, new string[1] { textures.Focus })
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = textures.SizeGui
			};
			result.Active = new MyGuiControlImage(null, null, null, null, new string[1] { textures.Active })
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Size = textures.SizeGui
			};
			return result;
		}

		protected override void RefreshInternals()
		{
			base.RefreshInternals();
			CheckBorder();
		}

		public void SetPreviewVisibility(bool visible)
		{
			m_previewImage.Visible = visible;
			Vector2 size = new Vector2(242f, 128f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			if (visible)
			{
				m_previewImage.Size = size;
				m_previewImage.BorderEnabled = true;
				m_previewImage.BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
				base.Size = new Vector2(m_previewImage.Size.X, m_titleLabel.Size.Y + m_previewImage.Size.Y);
				int num = 0;
				Vector2 vector = new Vector2(base.Size.X * 0.48f, (0f - base.Size.Y) * 0.48f + m_titleLabel.Size.Y);
				foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
				{
					dlcIcon.Visible = true;
					dlcIcon.Position = vector + new Vector2(0f, (float)num * (dlcIcon.Size.Y + 0.002f));
					num++;
				}
			}
			else
			{
				m_previewImage.Size = new Vector2(0f, 0f);
				m_previewImage.BorderEnabled = true;
				m_previewImage.BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
				base.Size = new Vector2(size.X, m_titleLabel.Size.Y + 0.002f);
			}
			foreach (MyGuiControlImage dlcIcon2 in m_dlcIcons)
			{
				dlcIcon2.Visible = visible;
			}
		}

		public void CreatePreview(string path, bool visible = true)
		{
			if (m_previewImage != null && Elements.Contains(m_previewImage))
			{
				Elements.Remove(m_previewImage);
			}
			m_previewImagePath = path;
			m_previewImage = new MyGuiControlImage(null, null, null, null, new string[1] { path })
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			if (!m_previewImage.IsAnyTextureValid())
			{
				m_previewImage.BackgroundTexture = m_noThumbnailTexture;
				m_previewImage.ColorMask = m_noThumbnailColor;
			}
			Elements.Add(m_previewImage);
			UpdatePositions();
			SetPreviewVisibility(visible: true);
			if (!visible)
			{
				SetPreviewVisibility(visible);
			}
		}

		public void AddDlcIcon(string path)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1] { path })
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Size = new Vector2(32f) / MyGuiConstants.GUI_OPTIMAL_SIZE
			};
			m_dlcIcons.Add(myGuiControlImage);
			Elements.Add(myGuiControlImage);
		}

		public void ClearDlcIcons()
		{
			if (m_dlcIcons == null || m_dlcIcons.Count == 0)
			{
				return;
			}
			foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
			{
				Elements.Remove(dlcIcon);
			}
			m_dlcIcons.Clear();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			UpdatePositions();
		}

		private void UpdatePositions()
		{
			if (m_previewImage.Visible)
			{
				Vector2 vector = new Vector2(base.Size.X * -0.5f, base.Size.Y * -0.52f);
				m_titleLabel.Position = vector + new Vector2(0.003f, 0.002f);
				m_previewImage.Position = vector + new Vector2(0f, m_titleLabel.Size.Y * 1f);
				Vector2 position = base.Size * 0.5f - new Vector2(0.001f, 0.002f);
				UpdateControlImagesPositions(m_workshopSteamImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				UpdateControlImagesPositions(m_workshopModioImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				UpdateControlImagesPositions(m_localmodImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				UpdateControlImagesPositions(m_cloudmodImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				int num = 0;
				vector = new Vector2(base.Size.X * 0.48f, (0f - base.Size.Y) * 0.5f + m_titleLabel.Size.Y);
				foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
				{
					dlcIcon.Visible = true;
					dlcIcon.Position = vector + new Vector2(0f, (float)num * (dlcIcon.Size.Y + 0.002f));
					num++;
				}
				return;
			}
			Vector2 vector2 = new Vector2(base.Size.X * -0.5f, base.Size.Y * -0.61f);
			m_titleLabel.Position = vector2 + new Vector2(0.003f + m_workshopSteamImages.Normal.Size.X * 2f / 3f, 0.002f);
			m_previewImage.Position = vector2 + new Vector2(0f, m_titleLabel.Size.Y * 1f);
			Vector2 position2 = vector2;
			UpdateControlImagesPositions(m_workshopSteamImages, position2, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui * 2f / 3f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			UpdateControlImagesPositions(m_workshopModioImages, position2, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui * 2f / 3f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			UpdateControlImagesPositions(m_localmodImages, position2, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui * 2f / 3f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			UpdateControlImagesPositions(m_cloudmodImages, position2, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui * 2f / 3f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			foreach (MyGuiControlImage dlcIcon2 in m_dlcIcons)
			{
				dlcIcon2.Visible = false;
			}
		}

		private void UpdateControlImagesPositions(MyControlImages images, Vector2 position, Vector2 size, MyGuiDrawAlignEnum align)
		{
			images.Normal.Position = position;
			images.Normal.OriginAlign = align;
			images.Normal.Size = size;
			images.Highlight.Position = position;
			images.Highlight.OriginAlign = align;
			images.Highlight.Size = size;
			images.Focus.Position = position;
			images.Focus.OriginAlign = align;
			images.Focus.Size = size;
			images.Active.Position = position;
			images.Active.OriginAlign = align;
			images.Active.Size = size;
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			CheckBorder();
		}

		public override void OnFocusChanged(bool focus)
		{
			base.OnFocusChanged(focus);
			if (FocusHighlightAlsoSelects)
			{
				base.Selected = true;
			}
			CheckBorder();
		}

		private void SetControlImagesHighlight()
		{
			MyControlImages? controlImages = GetControlImages();
			if (controlImages.HasValue)
			{
				if (Elements.Contains(controlImages.Value.Normal))
				{
					Elements.Remove(controlImages.Value.Normal);
				}
				if (Elements.Contains(controlImages.Value.Focus))
				{
					Elements.Remove(controlImages.Value.Focus);
				}
				if (Elements.Contains(controlImages.Value.Active))
				{
					Elements.Remove(controlImages.Value.Active);
				}
				if (!Elements.Contains(controlImages.Value.Highlight))
				{
					Elements.Add(controlImages.Value.Highlight);
				}
			}
		}

		private void SetControlImagesNormal()
		{
			MyControlImages? controlImages = GetControlImages();
			if (controlImages.HasValue)
			{
				if (Elements.Contains(controlImages.Value.Focus))
				{
					Elements.Remove(controlImages.Value.Focus);
				}
				if (Elements.Contains(controlImages.Value.Active))
				{
					Elements.Remove(controlImages.Value.Active);
				}
				if (Elements.Contains(controlImages.Value.Highlight))
				{
					Elements.Remove(controlImages.Value.Highlight);
				}
				if (!Elements.Contains(controlImages.Value.Normal))
				{
					Elements.Add(controlImages.Value.Normal);
				}
			}
		}

		private void SetControlImagesFocus()
		{
			MyControlImages? controlImages = GetControlImages();
			if (controlImages.HasValue)
			{
				if (Elements.Contains(controlImages.Value.Normal))
				{
					Elements.Remove(controlImages.Value.Normal);
				}
				if (Elements.Contains(controlImages.Value.Active))
				{
					Elements.Remove(controlImages.Value.Active);
				}
				if (Elements.Contains(controlImages.Value.Highlight))
				{
					Elements.Remove(controlImages.Value.Highlight);
				}
				if (!Elements.Contains(controlImages.Value.Focus))
				{
					Elements.Add(controlImages.Value.Focus);
				}
			}
		}

		private void SetControlImagesActive()
		{
			MyControlImages? controlImages = GetControlImages();
			if (controlImages.HasValue)
			{
				if (Elements.Contains(controlImages.Value.Normal))
				{
					Elements.Remove(controlImages.Value.Normal);
				}
				if (Elements.Contains(controlImages.Value.Focus))
				{
					Elements.Remove(controlImages.Value.Focus);
				}
				if (Elements.Contains(controlImages.Value.Highlight))
				{
					Elements.Remove(controlImages.Value.Highlight);
				}
				if (!Elements.Contains(controlImages.Value.Active))
				{
					Elements.Add(controlImages.Value.Active);
				}
			}
		}

		private void CheckBorder()
		{
			if (base.HasHighlight)
			{
				BorderEnabled = true;
				BorderColor = MyGuiConstants.HIGHLIGHT_BACKGROUND_COLOR;
				base.BorderSize = 3;
				m_titleLabel.Font = "White";
				SetControlImagesHighlight();
				return;
			}
			if (base.HasFocus)
			{
				BorderEnabled = true;
				BorderColor = MyGuiConstants.FOCUS_BACKGROUND_COLOR;
				base.BorderSize = 3;
				m_titleLabel.Font = "White";
				SetControlImagesFocus();
				return;
			}
			if (base.Selected)
			{
				BorderEnabled = true;
				BorderColor = MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
				base.BorderSize = 3;
				m_titleLabel.Font = "White";
				SetControlImagesActive();
				return;
			}
			BorderEnabled = false;
			BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
			base.BorderSize = 1;
			if (m_titleLabel != null)
			{
				m_titleLabel.Font = "Blue";
			}
			SetControlImagesNormal();
		}
	}
}
