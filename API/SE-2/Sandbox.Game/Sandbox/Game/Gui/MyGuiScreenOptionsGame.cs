using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenOptionsGame : MyGuiScreenBase
	{
		private struct OptionsGameSettings
		{
			public MyLanguagesEnum Language;

			public MyCubeBuilder.BuildingModeEnum BuildingMode;

			public MyStringId SkinId;

			public bool ExperimentalMode;

			public bool ControlHints;

			public bool GoodBotHints;

			public bool EnableNewNewGameScreen;

			public bool RotationHints;

			public MyConfig.CrosshairSwitch ShowCrosshair;

			public IronSightSwitchStateType IronSight;

			public bool EnableTrading;

			public bool AreaInteraction;

			public bool EnableSteamCloud;

			public bool EnablePrediction;

			public bool ShowPlayerNamesOnHud;

			public bool EnablePerformanceWarnings;

			public float UIOpacity;

			public float UIBkOpacity;

			public float HUDBkOpacity;

			public float SpriteMainViewportScale;

			public bool? GDPR;

			public Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings> HitIndicatorSettings;
		}

		private struct HitIndicatorSettings
		{
			public string Texture;

			public Color Color;
		}

		private enum PageEnum
		{
			General,
			Crosshair
		}

		private class ColorPreviewControl : MyGuiControlPanel
		{
			protected override void DrawBackground(float transitionAlpha)
			{
				if (BackgroundTexture != null && base.ColorMask.W > 0f)
				{
					BackgroundTexture.Draw(GetPositionAbsoluteTopLeft(), base.Size, base.ColorMask);
				}
			}
		}

		private class ColorPickerControlWrap
		{
			private static readonly Regex HEX_REGEX = new Regex("^(#{0,1})([0-9A-Fa-f]{6})([0-9A-Fa-f]{2})?$");

			private const float m_fixTextboxHeight = 0.209302321f;

			private readonly StringBuilder m_hexSb = new StringBuilder();

			private MyGuiControlSlider m_hueSlider;

			private MyGuiControlSlider m_saturationSlider;

			private MyGuiControlSlider m_valueSlider;

			private MyGuiControlSlider m_transparencySlider;

			private MyGuiControlTextbox m_hexTextbox;

			private MyGuiControlLabel m_hexLabel;

			public Color Color
			{
				get
				{
					return GetCurrentColor();
				}
				set
				{
					if (!(value == GetCurrentColor()))
					{
						SetColorInSlider(value);
						UpdateHex();
						this.ColorChanged?.Invoke(value);
					}
				}
			}

			public bool HexVisible
			{
				get
				{
					return m_hexTextbox.Visible;
				}
				set
				{
					m_hexTextbox.Visible = value;
					m_hexLabel.Visible = value;
				}
			}

			public event Action<Color> ColorChanged;

			private void SetColorInSlider(Color c)
			{
				Vector3 vector = c.ColorToHSV();
				m_hueSlider.Value = vector.X * 360f;
				m_saturationSlider.Value = vector.Y;
				m_valueSlider.Value = vector.Z;
				m_transparencySlider.Value = (float)(int)c.A / 255f;
			}

			public void Init(MyGuiScreenOptionsGame parent, Vector2 start, Vector2 end, float hitPreviewWidth, float buttonWidth)
			{
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, "H:");
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, "S:");
				MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(null, null, "V:");
				MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_CrosshairTransparency));
				m_hexLabel = new MyGuiControlLabel(null, null, "Hex:");
				m_hueSlider = new MyGuiControlSlider(null, 0f, 360f, 0.29f, null, null, string.Empty, 0, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Hue);
				MyGuiControlSlider hueSlider = m_hueSlider;
				hueSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(hueSlider.ValueChanged, new Action<MyGuiControlSlider>(OnSliderValueChange));
				m_saturationSlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, 0f, null, string.Empty);
				MyGuiControlSlider saturationSlider = m_saturationSlider;
				saturationSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(saturationSlider.ValueChanged, new Action<MyGuiControlSlider>(OnSliderValueChange));
				m_valueSlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, 0f, null, string.Empty);
				MyGuiControlSlider valueSlider = m_valueSlider;
				valueSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(valueSlider.ValueChanged, new Action<MyGuiControlSlider>(OnSliderValueChange));
				m_transparencySlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, 0f, null, string.Empty);
				MyGuiControlSlider transparencySlider = m_transparencySlider;
				transparencySlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(transparencySlider.ValueChanged, new Action<MyGuiControlSlider>(OnSliderValueChange));
				float num = Math.Abs(end.X - start.X) / 15f;
				Vector2 margin = new Vector2(num / 8f, 0f);
				m_hexTextbox = new MyGuiControlTextbox();
				m_hexTextbox.MaxLength = 9;
				m_hexTextbox.EnterPressed += HexTextboxOnEnterPressed;
				m_hexTextbox.FocusChanged += HexTextboxOnFocusChanged;
				m_saturationSlider.Size = new Vector2(hitPreviewWidth, m_saturationSlider.Size.X);
				m_saturationSlider.PositionX = 0f - hitPreviewWidth / 2f;
				m_saturationSlider.PositionY = start.Y;
				m_saturationSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_valueSlider.PositionX = 0f + hitPreviewWidth / 2f + buttonWidth;
				m_valueSlider.Size = new Vector2(end.X - m_valueSlider.PositionX, m_saturationSlider.Size.Y);
				m_valueSlider.PositionY = start.Y;
				m_valueSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_transparencySlider.Size = m_saturationSlider.Size;
				m_transparencySlider.PositionX = m_saturationSlider.PositionX;
				m_transparencySlider.PositionY = m_saturationSlider.PositionY + m_saturationSlider.Size.Y;
				m_transparencySlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_hueSlider.PositionX = start.X + myGuiControlLabel.Size.X + margin.X;
				m_hueSlider.PositionY = start.Y;
				m_hueSlider.Size = new Vector2(m_valueSlider.Size.X * 0.95f, m_hueSlider.Size.Y);
				m_hueSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_hexTextbox.PositionX = m_valueSlider.PositionX;
				m_hexTextbox.PositionY = m_valueSlider.PositionY + m_valueSlider.Size.Y;
				m_hexTextbox.Size = new Vector2(m_valueSlider.Size.X, m_valueSlider.Size.Y);
				m_hexTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				AttachToLeftCenterOf(myGuiControlLabel, m_hueSlider, margin);
				AttachToLeftCenterOf(myGuiControlLabel2, m_saturationSlider, margin);
				AttachToLeftCenterOf(myGuiControlLabel3, m_valueSlider, margin);
				AttachToLeftCenterOf(myGuiControlLabel4, m_transparencySlider, margin);
				AttachToLeftCenterOf(m_hexLabel, m_hexTextbox, margin);
				myGuiControlLabel4.PositionX = start.X + myGuiControlLabel4.Size.X;
				parent.Controls.Add(myGuiControlLabel);
				parent.Controls.Add(myGuiControlLabel2);
				parent.Controls.Add(myGuiControlLabel3);
				parent.Controls.Add(myGuiControlLabel4);
				parent.Controls.Add(m_hexLabel);
				parent.Controls.Add(m_hueSlider);
				parent.Controls.Add(m_saturationSlider);
				parent.Controls.Add(m_valueSlider);
				parent.Controls.Add(m_transparencySlider);
				parent.Controls.Add(m_hexTextbox);
				m_hexLabel.Position = new Vector2(m_hexLabel.Position.X, myGuiControlLabel4.Position.Y);
				m_hexTextbox.Position += new Vector2(0f, m_hexTextbox.Size.Y * 0.209302321f / 2f);
				Color = Color.Red;
			}

			private void UpdateHex()
			{
				Color currentColor = GetCurrentColor();
				m_hexSb.Clear();
				m_hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}{3:X2}", currentColor.R, currentColor.G, currentColor.B, currentColor.A);
				m_hexTextbox.SetText(m_hexSb);
			}

			private void HexTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
			{
				MyGuiControlTextbox obj2;
				if (!state && (obj2 = obj as MyGuiControlTextbox) != null)
				{
					HexTextboxOnEnterPressed(obj2);
				}
			}

			private void HexTextboxOnEnterPressed(MyGuiControlTextbox obj)
			{
				m_hexSb.Clear();
				obj.GetText(m_hexSb);
<<<<<<< HEAD
				string input = Regex.Replace(m_hexSb.ToString(), "\\s+", "");
				Match match = HEX_REGEX.Match(input);
				if (!match.Success || match.Length == 0)
=======
				string text = Regex.Replace(m_hexSb.ToString(), "\\s+", "");
				Match val = HEX_REGEX.Match(text);
				if (!((Group)val).get_Success() || ((Capture)val).get_Length() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					UpdateHex();
					return;
				}
<<<<<<< HEAD
				string text = match.Value;
				if (text.StartsWith("#"))
				{
					text = text.Substring(1);
				}
				byte r = byte.Parse(text.Substring(0, 2), NumberStyles.HexNumber);
				byte g = byte.Parse(text.Substring(2, 2), NumberStyles.HexNumber);
				byte b = byte.Parse(text.Substring(4, 2), NumberStyles.HexNumber);
				int a = 255;
				if (text.Length == 8)
				{
					a = byte.Parse(text.Substring(6, 2), NumberStyles.HexNumber);
=======
				string text2 = ((Capture)val).get_Value();
				if (text2.StartsWith("#"))
				{
					text2 = text2.Substring(1);
				}
				byte r = byte.Parse(text2.Substring(0, 2), NumberStyles.HexNumber);
				byte g = byte.Parse(text2.Substring(2, 2), NumberStyles.HexNumber);
				byte b = byte.Parse(text2.Substring(4, 2), NumberStyles.HexNumber);
				int a = 255;
				if (text2.Length == 8)
				{
					a = byte.Parse(text2.Substring(6, 2), NumberStyles.HexNumber);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				Color = new Color(r, g, b, a);
			}

			private void OnSliderValueChange(MyGuiControlSlider sender)
			{
				UpdateHex();
				this.ColorChanged?.Invoke(GetCurrentColor());
			}

			private Color GetCurrentColor()
			{
				Color result = new Vector3(m_hueSlider.Value / 360f, m_saturationSlider.Value, m_valueSlider.Value).HSVtoColor();
				result.A = (byte)(m_transparencySlider.Value * 255f);
				return result;
			}

			public static void ShowInColumns(MyGuiScreenOptionsGame parent, Vector2 start, Vector2 end, int columns, Vector4 marginsInside, Vector2 marginsBetween, float rowHeight, params MyGuiControlBase[] controls)
			{
				float num = (Math.Abs(end.X - start.X) - marginsBetween.X * (float)(columns - 1)) / (float)columns;
				for (int i = 0; i < controls.Length; i++)
				{
					if (controls[i] != null)
					{
						int num2 = i % columns;
						int num3 = i / columns;
						Vector2 vector = start + ((float)num2 * (num + marginsBetween.X) + marginsInside.X) * Vector2.UnitX + ((float)num3 * (marginsBetween.Y + rowHeight) + marginsInside.Y) * Vector2.UnitY;
						Vector2 vector2 = vector + (num - marginsInside.X - marginsInside.Z) * Vector2.UnitX + (rowHeight - marginsInside.W - marginsInside.Y) * Vector2.UnitY;
						controls[i].Size = new Vector2(Math.Abs(vector2.X - vector.X), Math.Abs(vector2.Y - vector.Y));
						controls[i].Position = vector;
						controls[i].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
					}
				}
			}

			public static void AttachToLeftCenterOf(MyGuiControlBase leftView, MyGuiControlBase rightView, Vector2 margin)
			{
				Vector2 vector2 = (leftView.Position = rightView.GetPositionAbsoluteCenterLeft() - margin);
				leftView.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			}

			public static void AttachToRightCenterOf(MyGuiControlBase leftView, MyGuiControlBase rightView, Vector2 margin)
			{
				Vector2 vector2 = (leftView.Position = rightView.GetPositionAbsoluteCenterRight() + margin);
				leftView.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			}

			public void FitInLine(MyGuiControlParent parent, Vector2 start, Vector2 end, params MyGuiControlBase[] controls)
			{
				controls[0].Position = start;
				controls[0].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				controls[3].Position = end;
				controls[3].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				controls[2].Position = end - 9f / 86f * controls[2].Size * Vector2.UnitY;
				controls[2].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				end -= controls[2].Size * new Vector2(1.1f, 0f);
				controls[1].Size = new Vector2((end - start).X, controls[1].Size.Y);
				controls[1].Position = end;
				controls[1].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				parent.Controls.Add(controls[0]);
				parent.Controls.Add(controls[1]);
				parent.Controls.Add(controls[2]);
				parent.Controls.Add(controls[3]);
			}
		}

		private MyGuiControlCombobox m_languageCombobox;

		private MyGuiControlCombobox m_buildingModeCombobox;

		private MyGuiControlCheckbox m_experimentalCheckbox;

		private MyGuiControlCheckbox m_controlHintsCheckbox;

		private MyGuiControlCheckbox m_goodbotHintsCheckbox;

		private MyGuiControlCombobox m_ironSightCombobox;

		private MyGuiControlCheckbox m_enableNewNewGameScreen;

		private MyGuiControlButton m_goodBotResetButton;

		private MyGuiControlButton m_tab1GeneralButton;

		private MyGuiControlButton m_tab2CrosshairButton;

		private MyGuiControlCheckbox m_rotationHintsCheckbox;

		private MyGuiControlCombobox m_crosshairCombobox;

		private MyGuiControlCheckbox m_cloudCheckbox;

		private MyGuiControlCheckbox m_GDPRConsentCheckbox;

		private MyGuiControlCheckbox m_enableTradingCheckbox;

		private MyGuiControlCheckbox m_areaInteractionCheckbox;

		private MyGuiControlSlider m_spriteMainViewportScaleSlider;

		private MyGuiControlSlider m_UIOpacitySlider;

		private MyGuiControlSlider m_UIBkOpacitySlider;

		private MyGuiControlSlider m_HUDBkOpacitySlider;

		private MyGuiControlPanel m_colorPreview;

		private ColorPickerControlWrap m_colorPicker;

		private MyGuiControlButton m_localizationWebButton;

		private MyGuiControlLabel m_localizationWarningLabel;

		private OptionsGameSettings m_settings = new OptionsGameSettings
		{
			SpriteMainViewportScale = 1f,
			UIOpacity = 1f,
			UIBkOpacity = 1f,
			HUDBkOpacity = 0.6f
		};

		private MyGuiControlElementGroup m_elementGroup;
<<<<<<< HEAD
=======

		private MyGuiControlCombobox m_hitCombobox;

		private MyGuiControlButton m_crosshairLookNextButton;

		private MyGuiControlButton m_crosshairLookPrevButton;

		private MyGuiControlButton m_buttonOk;

		private MyGuiControlButton m_buttonCancel;

		private MyGuiControlImage m_crosshairLookImage;

		private MyGuiControlImage m_crosshairLookPrevImage;

		private MyGuiControlImage m_crosshairLookPrev2Image;

		private MyGuiControlImage m_crosshairLookNextImage;

		private MyGuiControlImage m_crosshairLookNext2Image;

		private List<string> m_crosshairFiles = new List<string>();

		private bool m_isIniting;

		private bool m_ignoreUpdateForFrame;

		private bool m_languangeChanged;

		private bool m_isLimitedMenu;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyGuiControlCombobox m_hitCombobox;

		private MyGuiControlButton m_crosshairLookNextButton;

		private MyGuiControlButton m_crosshairLookPrevButton;

		private MyGuiControlButton m_buttonOk;

		private MyGuiControlButton m_buttonCancel;

		private MyGuiControlImage m_crosshairLookImage;

		private MyGuiControlImage m_crosshairLookPrevImage;

		private MyGuiControlImage m_crosshairLookPrev2Image;

		private MyGuiControlImage m_crosshairLookNextImage;

		private MyGuiControlImage m_crosshairLookNext2Image;

		private List<string> m_crosshairFiles = new List<string>();

		private bool m_isIniting;

		private bool m_ignoreUpdateForFrame;

		private bool m_isLimitedMenu;

		private MyLanguagesEnum m_loadedLanguage;

		private PageEnum m_currentPage;

		private PageEnum CurrentPage
		{
			get
			{
				return m_currentPage;
			}
			set
			{
				if (m_currentPage != value)
				{
					m_currentPage = value;
					RecreateControls(constructor: false);
				}
			}
		}

		public MyGuiScreenOptionsGame(bool isLimitedMenu = false)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, isLimitedMenu ? new Vector2(183f / 280f, 225f / 262f) : new Vector2(183f / 280f, 0.9379771f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_isIniting = true;
			SaveOriginalSettings();
			m_isLimitedMenu = isLimitedMenu;
			base.EnabledBackgroundFade = true;
			InitCrosshairIndicators();
			RecreateControls(constructor: true);
			m_isIniting = false;
		}

		private void InitCrosshairIndicators()
		{
			Path.Combine(MyFileSystem.ContentPath);
			foreach (string item in Directory.EnumerateFiles(Path.Combine(MyFileSystem.ContentPath, "Textures\\GUI\\Indicators")))
			{
				if (item.Contains("HitIndicator"))
				{
					m_crosshairFiles.Add(item);
				}
			}
		}

		private void OnResetGoodbotPressed(MyGuiControlButton obj)
		{
			if (MySession.Static != null)
			{
				MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.Reset();
			}
			else
			{
				MySandboxGame.Config.TutorialsFinished.Clear();
				MySandboxGame.Config.Save();
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaption_GoodBotHintsReset), messageText: new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxText_GoodBotHintReset))));
		}

		private void checkboxChanged(MyGuiControlCheckbox obj)
		{
			if (obj == m_experimentalCheckbox)
			{
				if (m_isIniting)
				{
					return;
				}
				if (obj.IsChecked)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxTextConfirmExperimental)), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
					{
						if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							m_settings.ExperimentalMode = obj.IsChecked;
						}
						else
						{
							m_settings.ExperimentalMode = false;
							obj.IsChecked = false;
						}
					}, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
				}
				else
				{
					m_settings.ExperimentalMode = false;
				}
			}
			else if (obj == m_controlHintsCheckbox)
			{
				m_settings.ControlHints = obj.IsChecked;
			}
			else if (obj == m_rotationHintsCheckbox)
			{
				m_settings.RotationHints = obj.IsChecked;
			}
			else if (obj == m_enableTradingCheckbox)
			{
				m_settings.EnableTrading = obj.IsChecked;
			}
			else if (obj == m_areaInteractionCheckbox)
			{
				m_settings.AreaInteraction = obj.IsChecked;
			}
			else if (obj == m_cloudCheckbox)
			{
				m_settings.EnableSteamCloud = obj.IsChecked;
			}
			else if (obj == m_goodbotHintsCheckbox)
			{
				ref OptionsGameSettings settings = ref m_settings;
				bool goodBotHints = (m_goodBotResetButton.Enabled = obj.IsChecked);
				settings.GoodBotHints = goodBotHints;
			}
			else if (obj == m_enableNewNewGameScreen)
			{
				m_settings.EnableNewNewGameScreen = obj.IsChecked;
			}
			else if (obj == m_GDPRConsentCheckbox)
			{
				m_settings.GDPR = obj.IsChecked;
			}
		}

		private void sliderChanged(MyGuiControlSlider obj)
		{
			if (obj == m_UIOpacitySlider)
			{
				m_settings.UIOpacity = obj.Value;
				m_guiTransition = obj.Value;
			}
			else if (obj == m_UIBkOpacitySlider)
			{
				m_settings.UIBkOpacity = obj.Value;
				m_backgroundTransition = obj.Value;
			}
			else if (obj == m_HUDBkOpacitySlider)
			{
				m_settings.HUDBkOpacity = obj.Value;
			}
			else if (obj == m_spriteMainViewportScaleSlider)
			{
				m_settings.SpriteMainViewportScale = obj.Value;
			}
		}

		private void m_buildingModeCombobox_ItemSelected()
		{
			m_settings.BuildingMode = (MyCubeBuilder.BuildingModeEnum)m_buildingModeCombobox.GetSelectedKey();
		}

		private void LocalizationWebButtonClicked(MyGuiControlButton obj)
		{
			MyGuiSandbox.OpenUrl(MyPerGameSettings.LocalizationWebUrl, UrlOpenMode.SteamOrExternalWithConfirm);
		}

		private void m_languageCombobox_ItemSelected()
		{
			m_settings.Language = (MyLanguagesEnum)m_languageCombobox.GetSelectedKey();
			if (m_localizationWarningLabel != null)
			{
				if (MyTexts.Languages[m_settings.Language].IsCommunityLocalized)
				{
					m_localizationWarningLabel.ColorMask = Color.Red.ToVector4();
				}
				else
				{
					m_localizationWarningLabel.ColorMask = Color.White.ToVector4();
				}
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsGame";
		}

		private void UpdateControls(bool constructor)
		{
			switch (CurrentPage)
			{
			case PageEnum.General:
				Tab1UpdateControl(constructor);
				break;
			case PageEnum.Crosshair:
				Tab2UpdateControls(constructor);
				break;
			}
		}

		public void Tab1UpdateControl(bool constructor)
		{
			m_languageCombobox.SelectItemByKey((int)m_settings.Language);
			if (constructor)
			{
				m_loadedLanguage = (MyLanguagesEnum)m_languageCombobox.GetSelectedKey();
			}
			m_buildingModeCombobox.SelectItemByKey((long)m_settings.BuildingMode);
			m_controlHintsCheckbox.IsChecked = m_settings.ControlHints;
			m_goodbotHintsCheckbox.IsChecked = m_settings.GoodBotHints;
			if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
			{
				m_enableNewNewGameScreen.IsChecked = m_settings.EnableNewNewGameScreen;
			}
			m_goodBotResetButton.Enabled = m_goodbotHintsCheckbox.IsChecked;
			if (m_experimentalCheckbox != null)
			{
				m_experimentalCheckbox.IsChecked = m_settings.ExperimentalMode;
			}
			if (m_rotationHintsCheckbox != null)
			{
				m_rotationHintsCheckbox.IsChecked = m_settings.RotationHints;
			}
			m_enableTradingCheckbox.IsChecked = m_settings.EnableTrading;
			if (m_cloudCheckbox != null)
			{
				m_cloudCheckbox.IsChecked = m_settings.EnableSteamCloud;
			}
			if (m_areaInteractionCheckbox != null)
			{
				m_areaInteractionCheckbox.IsChecked = m_settings.AreaInteraction;
			}
		}

		private void SaveOriginalSettings()
		{
			m_settings.Language = MySandboxGame.Config.Language;
			m_settings.BuildingMode = MyCubeBuilder.BuildingMode;
			m_settings.ControlHints = MySandboxGame.Config.ControlsHints;
			m_settings.EnableNewNewGameScreen = MySandboxGame.Config.EnableNewNewGameScreen;
			m_settings.GoodBotHints = MySandboxGame.Config.GoodBotHints;
			m_settings.ExperimentalMode = MySandboxGame.Config.ExperimentalMode;
			m_settings.RotationHints = MySandboxGame.Config.RotationHints;
			m_settings.EnableTrading = MySandboxGame.Config.EnableTrading;
			m_settings.EnableSteamCloud = MySandboxGame.Config.EnableSteamCloud;
			m_settings.SpriteMainViewportScale = MySandboxGame.Config.SpriteMainViewportScale * 100f;
			m_settings.UIOpacity = MySandboxGame.Config.UIOpacity;
			m_settings.UIBkOpacity = MySandboxGame.Config.UIBkOpacity;
			m_settings.HUDBkOpacity = MySandboxGame.Config.HUDBkOpacity;
			m_settings.GDPR = MySandboxGame.Config.GDPRConsent;
			m_settings.AreaInteraction = MySandboxGame.Config.AreaInteraction;
			m_settings.ShowCrosshair = MySandboxGame.Config.ShowCrosshair;
			m_settings.IronSight = MySandboxGame.Config.IronSightSwitchState;
			m_settings.HitIndicatorSettings = new Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings>();
			string path = Path.Combine(MyFileSystem.ContentPath);
			Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings> hitIndicatorSettings = m_settings.HitIndicatorSettings;
			HitIndicatorSettings value = new HitIndicatorSettings
			{
				Texture = Path.Combine(path, MySandboxGame.Config.HitIndicatorTextureCharacter),
				Color = MySandboxGame.Config.HitIndicatorColorCharacter
			};
			hitIndicatorSettings[MySession.MyHitIndicatorTarget.Character] = value;
			Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings> hitIndicatorSettings2 = m_settings.HitIndicatorSettings;
			value = new HitIndicatorSettings
			{
				Texture = Path.Combine(path, MySandboxGame.Config.HitIndicatorTextureFriend),
				Color = MySandboxGame.Config.HitIndicatorColorFriend
			};
			hitIndicatorSettings2[MySession.MyHitIndicatorTarget.Friend] = value;
			Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings> hitIndicatorSettings3 = m_settings.HitIndicatorSettings;
			value = new HitIndicatorSettings
			{
				Texture = Path.Combine(path, MySandboxGame.Config.HitIndicatorTextureGrid),
				Color = MySandboxGame.Config.HitIndicatorColorGrid
			};
			hitIndicatorSettings3[MySession.MyHitIndicatorTarget.Grid] = value;
			Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings> hitIndicatorSettings4 = m_settings.HitIndicatorSettings;
			value = new HitIndicatorSettings
			{
				Texture = Path.Combine(path, MySandboxGame.Config.HitIndicatorTextureHeadshot),
				Color = MySandboxGame.Config.HitIndicatorColorHeadshot
			};
			hitIndicatorSettings4[MySession.MyHitIndicatorTarget.Headshot] = value;
			Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings> hitIndicatorSettings5 = m_settings.HitIndicatorSettings;
			value = new HitIndicatorSettings
			{
				Texture = Path.Combine(path, MySandboxGame.Config.HitIndicatorTextureKill),
				Color = MySandboxGame.Config.HitIndicatorColorKill
			};
			hitIndicatorSettings5[MySession.MyHitIndicatorTarget.Kill] = value;
		}

		private void DoChanges()
		{
			Tab1DoChanges();
			Tab2DoChanges();
			MySandboxGame.Config.Save();
		}

		public void Tab1DoChanges()
		{
			MySandboxGame.Config.ExperimentalMode = m_settings.ExperimentalMode;
			if (m_settings.GDPR != MySandboxGame.Config.GDPRConsent)
			{
				MySandboxGame.Config.GDPRConsent = m_settings.GDPR;
				ConsentSenderGDPR.TrySendConsent();
			}
			MySandboxGame.Config.EnableTrading = m_settings.EnableTrading;
			MySandboxGame.Config.EnableSteamCloud = m_settings.EnableSteamCloud;
			MyLanguage.CurrentLanguage = m_settings.Language;
			if (m_loadedLanguage != MyLanguage.CurrentLanguage)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextRestartNeededAfterLanguageSwitch), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
				MyScreenManager.RecreateControls();
			}
			MyCubeBuilder.BuildingMode = m_settings.BuildingMode;
			MySandboxGame.Config.ControlsHints = m_settings.ControlHints;
			if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
			{
				MySandboxGame.Config.EnableNewNewGameScreen = m_settings.EnableNewNewGameScreen;
			}
			MySandboxGame.Config.GoodBotHints = m_settings.GoodBotHints;
			MySandboxGame.Config.RotationHints = m_settings.RotationHints;
			if (MyGuiScreenHudSpace.Static != null)
			{
				MyGuiScreenHudSpace.Static.RegisterAlphaMultiplier(VisualStyleCategory.Background, m_settings.HUDBkOpacity);
			}
			MySandboxGame.Config.AreaInteraction = m_settings.AreaInteraction;
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		public void OnOkClick(MyGuiControlButton sender)
		{
			DoChanges();
			CloseScreen();
		}

		private void OnWorkshopConsentClick(MyGuiControlButton sender)
		{
			MyModIoConsentViewModel viewModel = new MyModIoConsentViewModel(SetIgnoreUpdateForOneFrame, SetIgnoreUpdateForOneFrame);
			ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
		}

		private void SetIgnoreUpdateForOneFrame()
		{
			m_ignoreUpdateForFrame = true;
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (m_ignoreUpdateForFrame)
			{
				m_ignoreUpdateForFrame = false;
				return result;
			}
			if (hasFocus)
			{
				if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y).IsNewPressed())
				{
					OnResetGoodbotPressed(null);
				}
				if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X).IsNewPressed())
				{
					OnOkClick(null);
				}
				m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
				if (m_goodBotResetButton != null)
				{
					m_goodBotResetButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				}
				if (m_colorPicker != null)
				{
					m_colorPicker.HexVisible = !MyInput.Static.IsJoystickLastUsed;
				}
			}
			return result;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_isIniting = true;
			CreateTabs();
			switch (CurrentPage)
			{
			case PageEnum.General:
				CreateTab1Menu(constructor);
				break;
			case PageEnum.Crosshair:
				CreateTab2Menu(constructor);
				break;
			}
			UpdateControls(constructor);
			m_isIniting = false;
		}

		private void SelectTab(PageEnum tab)
		{
			CurrentPage = tab;
			switch (tab)
			{
			case PageEnum.General:
				m_tab1GeneralButton.Selected = true;
				m_tab1GeneralButton.Checked = true;
				m_tab2CrosshairButton.Selected = false;
				m_tab2CrosshairButton.Checked = false;
				base.FocusedControl = m_tab1GeneralButton;
				base.GamepadHelpTextId = MySpaceTexts.GameOptions_Help_Screen_TabGeneral;
				break;
			case PageEnum.Crosshair:
				m_tab1GeneralButton.Selected = false;
				m_tab1GeneralButton.Checked = false;
				m_tab2CrosshairButton.Selected = true;
				m_tab2CrosshairButton.Checked = true;
				base.FocusedControl = m_tab2CrosshairButton;
				base.GamepadHelpTextId = MySpaceTexts.GameOptions_Help_Screen_TabCrosshair;
				break;
			}
		}

		public override bool Draw()
		{
			base.Draw();
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				Vector2 positionAbsoluteTopLeft = m_tab1GeneralButton.GetPositionAbsoluteTopLeft();
				Vector2 size = m_tab1GeneralButton.Size;
				Vector2 normalizedCoord = positionAbsoluteTopLeft;
				normalizedCoord.Y += size.Y / 2f;
				normalizedCoord.X -= size.X / 6f;
				Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
				normalizedCoord2.Y = normalizedCoord.Y;
				int num = 2;
				Color value = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, enabled: true, m_transitionAlpha);
				normalizedCoord2.X += (float)num * size.X + size.X / 6f;
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), normalizedCoord, 1f, value, drawAlign);
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), normalizedCoord2, 1f, value, drawAlign);
			}
			return true;
		}

		private void CreateTabs()
		{
			m_elementGroup = new MyGuiControlElementGroup();
			Vector2 vector = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float num2 = 25f;
			float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
			float num3 = 0.0015f;
			new Vector2(0f, 0.042f);
			new Vector2(0f, 0.008f);
			Vector2 vector3 = (m_size.Value / 2f - vector) * new Vector2(-1f, -1f) + new Vector2(0f, y);
			Vector2 vector4 = (m_size.Value / 2f - vector) * new Vector2(1f, -1f) + new Vector2(0f, y);
			Vector2 vector5 = (m_size.Value / 2f - vector2) * new Vector2(0f, 1f);
			new Vector2(vector4.X - (num + num3), vector4.Y);
			_ = (vector3 + vector4) / 2f;
			float num4 = 0.02f;
			AddCaption(MyCommonTexts.ScreenCaptionGameOptions, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.15f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList3);
			Vector2 vector6 = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f - 0.003f, m_size.Value.Y / 2f - 0.095f);
			m_tab1GeneralButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenOptionsGame_GeneralTab), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnTabSwitchClick);
			m_tab1GeneralButton.Position = vector6 + m_tab1GeneralButton.Size / 2f * Vector2.UnitX;
			m_tab1GeneralButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			m_tab1GeneralButton.UserData = PageEnum.General;
			m_tab1GeneralButton.Selected = CurrentPage == PageEnum.General;
			vector6.X += m_tab1GeneralButton.Size.X + num4 / 3.6f;
			m_tab2CrosshairButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenOptionsGame_UITab), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnTabSwitchClick);
			m_tab2CrosshairButton.Position = vector6 + m_tab2CrosshairButton.Size / 2f * Vector2.UnitX;
			m_tab2CrosshairButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			m_tab2CrosshairButton.UserData = PageEnum.Crosshair;
			m_tab2CrosshairButton.Selected = CurrentPage == PageEnum.Crosshair;
			m_buttonOk = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkClick);
			m_buttonOk.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			m_buttonOk.Position = vector5 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonOk.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_buttonCancel = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnCancelClick);
			m_buttonCancel.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_buttonCancel.Position = vector5 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonCancel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_tab1GeneralButton);
			Controls.Add(m_tab2CrosshairButton);
			Controls.Add(m_buttonOk);
			Controls.Add(m_buttonCancel);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(vector3.X, m_buttonOk.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.GameOptions_Help_Screen_TabGeneral;
		}

		private void CreateTab1Menu(bool constructor)
		{
<<<<<<< HEAD
=======
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			MyGuiDrawAlignEnum originAlign2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			Vector2 vector = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
			float num2 = 0.0015f;
			Vector2 vector3 = new Vector2(0f, 0.042f);
			float num3 = 0f;
			Vector2 vector4 = new Vector2(0f, 0.008f);
			Vector2 vector5 = (m_size.Value / 2f - vector) * new Vector2(-1f, -1f) + new Vector2(0f, y);
			Vector2 vector6 = (m_size.Value / 2f - vector) * new Vector2(1f, -1f) + new Vector2(0f, y);
			_ = (m_size.Value / 2f - vector2) * new Vector2(0f, 1f);
			Vector2 vector7 = new Vector2(vector6.X - (num + num2), vector6.Y);
			num3 += 2f;
			MyGuiControlLabel control = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.Language))
			{
				Position = vector5 + num3 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_languageCombobox = new MyGuiControlCombobox
			{
				Position = vector6 + num3 * vector3,
				OriginAlign = originAlign2
			};
			m_languageCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_Language));
			Enumerator<MyLanguagesEnum> enumerator = MyLanguage.SupportedLanguages.GetEnumerator();
			try
			{
<<<<<<< HEAD
				MyTexts.MyLanguageDescription myLanguageDescription = MyTexts.Languages[supportedLanguage];
				string text = myLanguageDescription.Name;
				if (!myLanguageDescription.IsCommunityLocalized || MyPlatformGameSettings.SUPPORT_COMMUNITY_TRANSLATIONS)
				{
					if (myLanguageDescription.IsCommunityLocalized)
					{
						text += " *";
					}
					m_languageCombobox.AddItem((int)supportedLanguage, text);
				}
=======
				while (enumerator.MoveNext())
				{
					MyLanguagesEnum current = enumerator.get_Current();
					MyTexts.MyLanguageDescription myLanguageDescription = MyTexts.Languages[current];
					string text = myLanguageDescription.Name;
					if (!myLanguageDescription.IsCommunityLocalized || MyPlatformGameSettings.SUPPORT_COMMUNITY_TRANSLATIONS)
					{
						if (myLanguageDescription.IsCommunityLocalized)
						{
							text += " *";
						}
						m_languageCombobox.AddItem((int)current, text);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_languageCombobox.CustomSortItems(delegate(MyGuiControlCombobox.Item a, MyGuiControlCombobox.Item b)
			{
				long key = a.Key;
				return key.CompareTo(b.Key);
			});
			m_languageCombobox.ItemSelected += m_languageCombobox_ItemSelected;
			num3 += 1f;
			if (!m_isLimitedMenu)
			{
				m_localizationWarningLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_LocalizationWarning), null, 0.578f)
				{
					Position = vector6 + num3 * vector3 - new Vector2(num - 0.005f, 0f),
					OriginAlign = originAlign
				};
				Vector2? position = vector6 + num3 * vector3 - new Vector2(num - 0.008f - m_localizationWarningLabel.Size.X, 0f);
				StringBuilder text2 = MyTexts.Get(MyCommonTexts.ScreenOptionsGame_MoreInfo);
				Action<MyGuiControlButton> onButtonClick = LocalizationWebButtonClicked;
				m_localizationWebButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, originAlign, null, text2, 0.6f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick);
				m_localizationWebButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
				num3 += 0.83f;
			}
			MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_BuildingMode))
			{
				Position = vector5 + num3 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_buildingModeCombobox = new MyGuiControlCombobox
			{
				Position = vector6 + num3 * vector3,
				OriginAlign = originAlign2
			};
			m_buildingModeCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_BuildingMode));
			m_buildingModeCombobox.AddItem(0L, MyCommonTexts.ScreenOptionsGame_SingleBlock);
			m_buildingModeCombobox.AddItem(1L, MyCommonTexts.ScreenOptionsGame_Line);
			m_buildingModeCombobox.AddItem(2L, MyCommonTexts.ScreenOptionsGame_Plane);
			m_buildingModeCombobox.ItemSelected += m_buildingModeCombobox_ItemSelected;
			num3 += 1.26f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ExperimentalMode))
			{
				Position = vector5 + num3 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_experimentalCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsExperimentalMode))
			{
				Position = vector7 + num3 * vector3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox experimentalCheckbox = m_experimentalCheckbox;
			bool enabled = (myGuiControlLabel.Enabled = MyGuiScreenGamePlay.Static == null);
			experimentalCheckbox.Enabled = enabled;
			num3 += 1f;
			MyGuiControlLabel control3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowControlsHints))
			{
				Position = vector5 + num3 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_controlHintsCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowControlsHints))
			{
				Position = vector7 + num3 * vector3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox controlHintsCheckbox = m_controlHintsCheckbox;
			controlHintsCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(controlHintsCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			num3 += 1f;
			MyGuiControlLabel control4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowGoodBotHints))
			{
				Position = vector5 + num3 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_goodbotHintsCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowGoodBotHints))
			{
				Position = vector7 + num3 * vector3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox goodbotHintsCheckbox = m_goodbotHintsCheckbox;
			goodbotHintsCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(goodbotHintsCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			m_goodBotResetButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, onButtonClick: OnResetGoodbotPressed, toolTip: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_ResetGoodbotHints_TTIP))
			{
				Position = new Vector2(base.Size.Value.X * 0.5f - vector.X, m_goodbotHintsCheckbox.Position.Y),
				OriginAlign = originAlign2,
				Text = MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_ResetGoodbotHints),
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			m_goodBotResetButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			MyGuiControlLabel control5 = null;
			if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
			{
				num3 += 1f;
				control5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.EnableNewNewGameScreen))
				{
					Position = vector5 + num3 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_enableNewNewGameScreen = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnableNewNewGameScreen))
				{
					Position = vector7 + num3 * vector3,
					OriginAlign = originAlign
				};
				MyGuiControlCheckbox enableNewNewGameScreen = m_enableNewNewGameScreen;
				enableNewNewGameScreen.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(enableNewNewGameScreen.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			}
			MyGuiControlLabel control6 = null;
			if (MyFakes.ENABLE_ROTATION_HINTS && !MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				num3 += 1f;
				control6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowRotationHints))
				{
					Position = vector5 + num3 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_rotationHintsCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowRotationHints))
				{
					Position = vector7 + num3 * vector3,
					OriginAlign = originAlign
				};
				MyGuiControlCheckbox rotationHintsCheckbox = m_rotationHintsCheckbox;
				rotationHintsCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(rotationHintsCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			}
			MyGuiControlLabel control7 = null;
			if (!m_isLimitedMenu)
			{
				num3 += 1f;
				control7 = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MyCommonTexts.EnableSteamCloud), MyGameService.Service.ServiceName))
				{
					Position = vector5 + num3 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_cloudCheckbox = new MyGuiControlCheckbox(null, null, string.Format(MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnableSteamCloud), MyGameService.Service.ServiceName))
				{
					Position = vector7 + num3 * vector3,
					OriginAlign = originAlign
				};
				MyGuiControlCheckbox cloudCheckbox = m_cloudCheckbox;
				cloudCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(cloudCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			}
			num3 += 1f;
			MyGuiControlLabel control8 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.GameOptions_EnableTrading))
			{
				Position = vector5 + num3 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_enableTradingCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.GameOptions_EnableTrading_TTIP))
			{
				Position = vector7 + num3 * vector3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox enableTradingCheckbox = m_enableTradingCheckbox;
			enableTradingCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(enableTradingCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			num3 += 1f;
			MyGuiControlLabel control9 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.GDPR_Caption))
			{
				Position = vector5 + num3 * vector3 + vector4,
				OriginAlign = originAlign
			};
			m_GDPRConsentCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_GDPRConsent))
			{
				Position = vector7 + num3 * vector3,
				OriginAlign = originAlign
			};
			m_GDPRConsentCheckbox.IsChecked = MySandboxGame.Config.GDPRConsent == true || !MySandboxGame.Config.GDPRConsent.HasValue;
			MyGuiControlCheckbox gDPRConsentCheckbox = m_GDPRConsentCheckbox;
			gDPRConsentCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(gDPRConsentCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			MyGuiControlButton control10 = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnWorkshopConsentClick)
			{
				Position = new Vector2(base.Size.Value.X * 0.5f - vector.X, m_GDPRConsentCheckbox.Position.Y),
				OriginAlign = originAlign2,
				Text = "Mod.Io " + MyTexts.GetString(MySpaceTexts.Consent),
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			if (MyFakes.ENABLE_AREA_INTERACTIONS)
			{
				num3 += 1.08f;
				MyGuiControlLabel control11 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.AreaInteration_Label))
				{
					Position = vector5 + num3 * vector3 + vector4,
					OriginAlign = originAlign
				};
				m_areaInteractionCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_AreaInteraction))
				{
					Position = vector7 + num3 * vector3,
					OriginAlign = originAlign
				};
				m_areaInteractionCheckbox.IsChecked = MySandboxGame.Config.AreaInteraction;
				MyGuiControlCheckbox areaInteractionCheckbox = m_areaInteractionCheckbox;
				areaInteractionCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(areaInteractionCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
				Controls.Add(control11);
				Controls.Add(m_areaInteractionCheckbox);
			}
			num3 += 1.35f;
			Controls.Add(control);
			Controls.Add(m_languageCombobox);
			if (m_localizationWebButton != null)
			{
				Controls.Add(m_localizationWebButton);
			}
			if (m_localizationWebButton != null)
			{
				Controls.Add(m_localizationWarningLabel);
			}
			Controls.Add(control2);
			Controls.Add(m_buildingModeCombobox);
			Controls.Add(myGuiControlLabel);
			Controls.Add(m_experimentalCheckbox);
			Controls.Add(control3);
			Controls.Add(control4);
			Controls.Add(m_controlHintsCheckbox);
			Controls.Add(m_goodbotHintsCheckbox);
			if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
			{
				Controls.Add(control5);
				Controls.Add(m_enableNewNewGameScreen);
			}
			Controls.Add(m_goodBotResetButton);
			if (MyFakes.ENABLE_ROTATION_HINTS && !MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				Controls.Add(control6);
				Controls.Add(m_rotationHintsCheckbox);
			}
			if (!m_isLimitedMenu)
			{
				Controls.Add(control7);
				Controls.Add(m_cloudCheckbox);
			}
			Controls.Add(control8);
			Controls.Add(m_enableTradingCheckbox);
			Controls.Add(control9);
			Controls.Add(m_GDPRConsentCheckbox);
			Controls.Add(control10);
			base.FocusedControl = m_buttonOk;
			base.CloseButtonEnabled = true;
			MyGuiControlCheckbox experimentalCheckbox2 = m_experimentalCheckbox;
			experimentalCheckbox2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(experimentalCheckbox2.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
		}

		private void CreateTab2Menu(bool constructor)
		{
			float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			Vector2 vector = new Vector2(0f, 0.042f);
			float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
			Vector2 vector2 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector3 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float num2 = 0f;
			Vector2 vector4 = (m_size.Value / 2f - vector2) * new Vector2(-1f, -1f) + new Vector2(0f, y);
			Vector2 vector5 = (m_size.Value / 2f - vector2) * new Vector2(1f, -1f) + new Vector2(0f, y);
			_ = (m_size.Value / 2f - vector3) * new Vector2(0f, 1f);
			MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			MyGuiDrawAlignEnum originAlign2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			Vector2 vector6 = new Vector2(0f, 0.008f);
			float num3 = 0.0015f;
			new Vector2(vector5.X - (num + num3), vector5.Y);
			num2 += 2f;
			MyGuiControlLabel control = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowCrosshair))
			{
				Position = vector4 + num2 * vector + vector6,
				OriginAlign = originAlign
			};
			m_crosshairCombobox = new MyGuiControlCombobox
			{
				Position = vector5 + num2 * vector,
				OriginAlign = originAlign2
			};
			m_crosshairCombobox.SetTooltip(MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowCrosshair));
			m_crosshairCombobox.AddItem(1L, MyCommonTexts.Crosshair_AlwaysVisible);
			m_crosshairCombobox.AddItem(0L, MyCommonTexts.Crosshair_VisibleWithHUD);
			m_crosshairCombobox.AddItem(2L, MyCommonTexts.Crosshair_AlwaysHidden);
			m_crosshairCombobox.ItemSelected += crosshairCombobox_ItemSelected;
			num2 += 1f;
			MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.IronSightSwitch))
			{
				Position = vector4 + num2 * vector + vector6,
				OriginAlign = originAlign
			};
			m_ironSightCombobox = new MyGuiControlCombobox
			{
				Position = vector5 + num2 * vector,
				OriginAlign = originAlign2
			};
			m_ironSightCombobox.SetTooltip(MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsIronsightSwitchType));
			m_ironSightCombobox.AddItem(1L, MyCommonTexts.IronSight_Toggle);
			m_ironSightCombobox.AddItem(0L, MyCommonTexts.IronSight_Hold);
			m_ironSightCombobox.ItemSelected += ironSightCombobox_ItemSelected;
			num2 += 1f;
			float hitPreviewWidth = 0.16f;
			float buttonWidth = 0.0396522f;
			MyGuiControlLabel control3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_HitIndicatorLabel))
			{
				Position = vector4 + num2 * vector + vector6,
				OriginAlign = originAlign
			};
			m_hitCombobox = new MyGuiControlCombobox
			{
				Position = vector5 + num2 * vector,
				OriginAlign = originAlign2
			};
			m_hitCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_HitIndicatorTooltip));
			m_hitCombobox.AddItem(0L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Character);
			m_hitCombobox.AddItem(5L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Friendly);
			m_hitCombobox.AddItem(1L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Headshot);
			m_hitCombobox.AddItem(2L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Kill);
			m_hitCombobox.AddItem(3L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Grid);
			m_hitCombobox.ItemSelected += HitCombobox_ItemSelected;
			m_hitCombobox.SelectItemByIndex(0);
			Controls.Add(control3);
			Controls.Add(m_hitCombobox);
			num2 += 1f;
			if (!MyFakes.HIDE_CROSSHAIR_OPTIONS)
			{
				m_crosshairLookImage = new MyGuiControlImage();
				m_crosshairLookImage.Size = new Vector2(0.16f, 0.16f);
				m_crosshairLookImage.BorderColor = Color.Gray;
				m_crosshairLookImage.BorderSize = 1;
				m_crosshairLookImage.BorderEnabled = true;
				m_crosshairLookImage.UserData = 0;
				MyGuiControlImage crosshairLookImage = m_crosshairLookImage;
				MyGuiControlImage.MyDrawTexture[] array = new MyGuiControlImage.MyDrawTexture[1];
				MyGuiControlImage.MyDrawTexture myDrawTexture = new MyGuiControlImage.MyDrawTexture
				{
					ColorMask = Color.Red,
					Texture = ""
				};
				array[0] = myDrawTexture;
				crosshairLookImage.SetTextures(array);
				float num4 = (vector5.X - vector4.X - m_crosshairLookImage.Size.X) / 4f;
				m_crosshairLookNextImage = new MyGuiControlImage();
				m_crosshairLookNextImage.UserData = 1;
				m_crosshairLookNextImage.Size = new Vector2(num4, num4);
				MyGuiControlImage crosshairLookNextImage = m_crosshairLookNextImage;
				MyGuiControlImage.MyDrawTexture[] array2 = new MyGuiControlImage.MyDrawTexture[1];
				myDrawTexture = new MyGuiControlImage.MyDrawTexture
				{
					ColorMask = Color.Magenta,
					Texture = ""
				};
				array2[0] = myDrawTexture;
				crosshairLookNextImage.SetTextures(array2);
				m_crosshairLookPrevImage = new MyGuiControlImage();
				m_crosshairLookPrevImage.UserData = -1;
				m_crosshairLookPrevImage.Size = new Vector2(num4, num4);
				MyGuiControlImage crosshairLookPrevImage = m_crosshairLookPrevImage;
				MyGuiControlImage.MyDrawTexture[] array3 = new MyGuiControlImage.MyDrawTexture[1];
				myDrawTexture = new MyGuiControlImage.MyDrawTexture
				{
					ColorMask = Color.Yellow,
					Texture = ""
				};
				array3[0] = myDrawTexture;
				crosshairLookPrevImage.SetTextures(array3);
				m_crosshairLookNext2Image = new MyGuiControlImage();
				m_crosshairLookNext2Image.UserData = 2;
				m_crosshairLookNext2Image.Size = new Vector2(num4, num4);
				MyGuiControlImage crosshairLookNext2Image = m_crosshairLookNext2Image;
				MyGuiControlImage.MyDrawTexture[] array4 = new MyGuiControlImage.MyDrawTexture[1];
				myDrawTexture = new MyGuiControlImage.MyDrawTexture
				{
					ColorMask = Color.Blue,
					Texture = ""
				};
				array4[0] = myDrawTexture;
				crosshairLookNext2Image.SetTextures(array4);
				m_crosshairLookPrev2Image = new MyGuiControlImage();
				m_crosshairLookPrev2Image.UserData = -2;
				m_crosshairLookPrev2Image.Size = new Vector2(num4, num4);
				MyGuiControlImage crosshairLookPrev2Image = m_crosshairLookPrev2Image;
				MyGuiControlImage.MyDrawTexture[] array5 = new MyGuiControlImage.MyDrawTexture[1];
				myDrawTexture = new MyGuiControlImage.MyDrawTexture
				{
					ColorMask = Color.Green,
					Texture = ""
				};
				array5[0] = myDrawTexture;
				crosshairLookPrev2Image.SetTextures(array5);
				m_crosshairLookImage.Position = vector4 * Vector2.UnitY + num2 * vector;
				m_crosshairLookImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
				ColorPickerControlWrap.AttachToRightCenterOf(m_crosshairLookNextImage, m_crosshairLookImage, Vector2.Zero);
				ColorPickerControlWrap.AttachToRightCenterOf(m_crosshairLookNext2Image, m_crosshairLookNextImage, Vector2.Zero);
				ColorPickerControlWrap.AttachToLeftCenterOf(m_crosshairLookPrevImage, m_crosshairLookImage, Vector2.Zero);
				ColorPickerControlWrap.AttachToLeftCenterOf(m_crosshairLookPrev2Image, m_crosshairLookPrevImage, Vector2.Zero);
				m_crosshairLookNextButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Square, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, ChangeLook);
				m_crosshairLookNextButton.Icon = MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE;
				m_crosshairLookNextButton.IconRotation = 3.141593f;
				m_crosshairLookNextButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				m_crosshairLookNextButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
				m_crosshairLookNextButton.Position = m_crosshairLookImage.Position - m_crosshairLookImage.Size * new Vector2(0.5f, -1f) - m_crosshairLookNextButton.Size;
				m_crosshairLookNextButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_crosshairLookPrevButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Square, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, ChangeLook);
				m_crosshairLookPrevButton.Icon = MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE;
				m_crosshairLookPrevButton.IconRotation = 0f;
				m_crosshairLookPrevButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				m_crosshairLookPrevButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
				m_crosshairLookPrevButton.Position = m_crosshairLookImage.Position + m_crosshairLookImage.Size * new Vector2(0.5f, 1f) - m_crosshairLookNextButton.Size * Vector2.UnitY + m_crosshairLookNextButton.Size * Vector2.UnitX;
				m_crosshairLookPrevButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				Controls.Add(m_crosshairLookNextButton);
				Controls.Add(m_crosshairLookPrevButton);
				num2 += 3.99600029f;
				Controls.Add(m_crosshairLookImage);
				Controls.Add(m_crosshairLookNextImage);
				Controls.Add(m_crosshairLookNext2Image);
				Controls.Add(m_crosshairLookPrevImage);
				Controls.Add(m_crosshairLookPrev2Image);
			}
			else
			{
				MyGuiControlLabel control4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_CrosshairColor))
				{
					Position = vector4 + num2 * vector + vector6,
					OriginAlign = originAlign
				};
				Controls.Add(control4);
				m_colorPreview = new ColorPreviewControl
				{
					Position = vector5 + num2 * vector,
					Size = m_ironSightCombobox.Size,
					OriginAlign = originAlign2
				};
				m_colorPreview.ColorMask = Color.Transparent;
				m_colorPreview.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
				m_colorPreview.CanHaveFocus = false;
				Controls.Add(m_colorPreview);
				num2 += 1f;
			}
			m_colorPicker = new ColorPickerControlWrap();
			Vector2 end = vector5 + num2 * vector;
			Vector2 start = vector4 + num2 * vector;
			m_colorPicker.Init(this, start, end, hitPreviewWidth, buttonWidth);
			m_colorPicker.ColorChanged += CrosshairColorChanged;
			num2 += 2.37600017f;
			Controls.Add(control);
			Controls.Add(m_crosshairCombobox);
			Controls.Add(control2);
			Controls.Add(m_ironSightCombobox);
			MyGuiControlLabel control5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.LabelOptionsDisplay_SpriteMainViewportScale))
			{
				Position = vector4 + num2 * vector + vector6,
				OriginAlign = originAlign
			};
			m_spriteMainViewportScaleSlider = new MyGuiControlSlider(null, 70f, 100f, 0.29f, labelText: new StringBuilder("{0}%").ToString(), defaultValue: MySandboxGame.Config.SpriteMainViewportScale * 100f, color: null, labelDecimalPlaces: 1, labelScale: 0.8f, labelSpaceWidth: 0.07f, labelFont: "Blue", toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsDisplay_SpriteMainViewportScale), visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: false, showLabel: true)
			{
				Position = vector5 + num2 * vector,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			MyGuiControlSlider spriteMainViewportScaleSlider = m_spriteMainViewportScaleSlider;
			spriteMainViewportScaleSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(spriteMainViewportScaleSlider.ValueChanged, new Action<MyGuiControlSlider>(sliderChanged));
			num2 += 1.08f;
			MyGuiControlLabel control6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_UIOpacity))
			{
				Position = vector4 + num2 * vector + vector6,
				OriginAlign = originAlign
			};
			m_UIOpacitySlider = new MyGuiControlSlider(null, 0.1f, 1f, 0.29f, toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsUIOpacity), defaultValue: 1f)
			{
				Position = vector5 + num2 * vector,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			MyGuiControlSlider uIOpacitySlider = m_UIOpacitySlider;
			uIOpacitySlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(uIOpacitySlider.ValueChanged, new Action<MyGuiControlSlider>(sliderChanged));
			num2 += 1.08f;
			MyGuiControlLabel control7 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_UIBkOpacity))
			{
				Position = vector4 + num2 * vector + vector6,
				OriginAlign = originAlign
			};
			m_UIBkOpacitySlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsUIBkOpacity), defaultValue: 1f)
			{
				Position = vector5 + num2 * vector,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			MyGuiControlSlider uIBkOpacitySlider = m_UIBkOpacitySlider;
			uIBkOpacitySlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(uIBkOpacitySlider.ValueChanged, new Action<MyGuiControlSlider>(sliderChanged));
			num2 += 1.08f;
			MyGuiControlLabel control8 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_HUDBkOpacity))
			{
				Position = vector4 + num2 * vector + vector6,
				OriginAlign = originAlign
			};
			m_HUDBkOpacitySlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsHUDBkOpacity), defaultValue: 1f)
			{
				Position = vector5 + num2 * vector,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			MyGuiControlSlider hUDBkOpacitySlider = m_HUDBkOpacitySlider;
			hUDBkOpacitySlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(hUDBkOpacitySlider.ValueChanged, new Action<MyGuiControlSlider>(sliderChanged));
			Controls.Add(control5);
			Controls.Add(m_spriteMainViewportScaleSlider);
			Controls.Add(control6);
			Controls.Add(m_UIOpacitySlider);
			Controls.Add(control7);
			Controls.Add(m_UIBkOpacitySlider);
			Controls.Add(control8);
			Controls.Add(m_HUDBkOpacitySlider);
		}

		private void crosshairCombobox_ItemSelected()
		{
			m_settings.ShowCrosshair = (MyConfig.CrosshairSwitch)m_crosshairCombobox.GetSelectedKey();
		}

		private void ironSightCombobox_ItemSelected()
		{
			m_settings.IronSight = (IronSightSwitchStateType)m_ironSightCombobox.GetSelectedKey();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				int currentPage = (int)CurrentPage;
				currentPage = ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT)) ? (currentPage - 1) : (currentPage + 1));
				if (currentPage < 0)
				{
					currentPage = 1;
				}
				else if (currentPage > 1)
				{
					currentPage = 0;
				}
				SelectTab((PageEnum)currentPage);
			}
		}

		public void Tab2UpdateControls(bool constructor)
		{
			ChangeLook(m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Character].Texture);
			m_colorPicker.Color = m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Character].Color;
			m_crosshairCombobox.SelectItemByKey((long)m_settings.ShowCrosshair);
			m_ironSightCombobox.SelectItemByKey((long)m_settings.IronSight);
			m_spriteMainViewportScaleSlider.Value = m_settings.SpriteMainViewportScale;
			m_UIOpacitySlider.Value = m_settings.UIOpacity;
			m_UIBkOpacitySlider.Value = m_settings.UIBkOpacity;
			m_HUDBkOpacitySlider.Value = m_settings.HUDBkOpacity;
		}

		public void Tab2DoChanges()
		{
			Dictionary<MySession.MyHitIndicatorTarget, HitIndicatorSettings> hitIndicatorSettings = m_settings.HitIndicatorSettings;
			string fromPath = Path.Combine(MyFileSystem.ContentPath, "Textures");
			if (hitIndicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Character))
			{
				MySandboxGame.Config.HitIndicatorColorCharacter = hitIndicatorSettings[MySession.MyHitIndicatorTarget.Character].Color;
				MySandboxGame.Config.HitIndicatorTextureCharacter = MyFileSystem.MakeRelativePath(fromPath, hitIndicatorSettings[MySession.MyHitIndicatorTarget.Character].Texture);
			}
			if (hitIndicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Friend))
			{
				MySandboxGame.Config.HitIndicatorColorFriend = hitIndicatorSettings[MySession.MyHitIndicatorTarget.Friend].Color;
				MySandboxGame.Config.HitIndicatorTextureFriend = MyFileSystem.MakeRelativePath(fromPath, hitIndicatorSettings[MySession.MyHitIndicatorTarget.Friend].Texture);
			}
			if (hitIndicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Grid))
			{
				MySandboxGame.Config.HitIndicatorColorGrid = hitIndicatorSettings[MySession.MyHitIndicatorTarget.Grid].Color;
				MySandboxGame.Config.HitIndicatorTextureGrid = MyFileSystem.MakeRelativePath(fromPath, hitIndicatorSettings[MySession.MyHitIndicatorTarget.Grid].Texture);
			}
			if (hitIndicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Headshot))
			{
				MySandboxGame.Config.HitIndicatorColorHeadshot = hitIndicatorSettings[MySession.MyHitIndicatorTarget.Headshot].Color;
				MySandboxGame.Config.HitIndicatorTextureHeadshot = MyFileSystem.MakeRelativePath(fromPath, hitIndicatorSettings[MySession.MyHitIndicatorTarget.Headshot].Texture);
			}
			if (hitIndicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Kill))
			{
				MySandboxGame.Config.HitIndicatorColorKill = hitIndicatorSettings[MySession.MyHitIndicatorTarget.Kill].Color;
				MySandboxGame.Config.HitIndicatorTextureKill = MyFileSystem.MakeRelativePath(fromPath, hitIndicatorSettings[MySession.MyHitIndicatorTarget.Kill].Texture);
			}
			MySandboxGame.Config.ShowCrosshair = m_settings.ShowCrosshair;
			MySandboxGame.Config.IronSightSwitchState = m_settings.IronSight;
			MySandboxGame.Config.SpriteMainViewportScale = m_settings.SpriteMainViewportScale / 100f;
			MySandboxGame.Static.UpdateUIScale();
			MySandboxGame.Config.UIOpacity = m_settings.UIOpacity;
			MySandboxGame.Config.UIBkOpacity = m_settings.UIBkOpacity;
			MySandboxGame.Config.HUDBkOpacity = m_settings.HUDBkOpacity;
		}

		private void CrosshairColorChanged(Color obj)
		{
			if (!MyFakes.HIDE_CROSSHAIR_OPTIONS)
			{
				m_crosshairLookImage.Textures[0].ColorMask = obj;
				m_crosshairLookNextImage.Textures[0].ColorMask = obj;
				m_crosshairLookPrevImage.Textures[0].ColorMask = obj;
				m_crosshairLookNext2Image.Textures[0].ColorMask = obj;
				m_crosshairLookPrev2Image.Textures[0].ColorMask = obj;
			}
			else
			{
				m_colorPreview.ColorMask = obj;
			}
			RecordTextureOrColorChange();
		}

		private void HitCombobox_ItemSelected()
		{
			if (!m_isIniting)
			{
				MySession.MyHitIndicatorTarget key = (MySession.MyHitIndicatorTarget)m_hitCombobox.GetSelectedKey();
				if (m_settings.HitIndicatorSettings != null && m_settings.HitIndicatorSettings.ContainsKey(key))
				{
					HitIndicatorSettings hitIndicatorSettings = m_settings.HitIndicatorSettings[key];
					m_colorPicker.Color = hitIndicatorSettings.Color;
					ChangeLook(hitIndicatorSettings.Texture);
				}
			}
		}

		private void RecordTextureOrColorChange()
		{
			if (!m_isIniting)
			{
				MySession.MyHitIndicatorTarget key = (MySession.MyHitIndicatorTarget)m_hitCombobox.GetSelectedKey();
				m_settings.HitIndicatorSettings[key] = new HitIndicatorSettings
				{
					Texture = m_settings.HitIndicatorSettings[key].Texture,
					Color = m_colorPicker.Color
				};
			}
		}

		public void ChangeLook(string texture)
		{
			int index = m_crosshairFiles.IndexOf(texture);
			ChangeLook(index);
		}

		public void ChangeLook(MyGuiControlButton sender)
		{
			int num = m_crosshairFiles.IndexOf(m_crosshairLookImage.Textures[0].Texture);
			if (num == -1)
			{
				num = 0;
			}
			num = ((sender != m_crosshairLookNextButton) ? (num - 1) : (num + 1));
			ChangeLook(num);
		}

		private void ChangeLook(int index)
		{
			if (!MyFakes.HIDE_CROSSHAIR_OPTIONS)
			{
				m_crosshairLookImage.Textures[0].Texture = m_crosshairFiles[Loop(index)];
				m_crosshairLookNextImage.Textures[0].Texture = m_crosshairFiles[Loop(index - 1)];
				m_crosshairLookPrevImage.Textures[0].Texture = m_crosshairFiles[Loop(index + 1)];
				m_crosshairLookNext2Image.Textures[0].Texture = m_crosshairFiles[Loop(index - 2)];
				m_crosshairLookPrev2Image.Textures[0].Texture = m_crosshairFiles[Loop(index + 2)];
			}
			RecordTextureOrColorChange();
			int Loop(int i)
			{
				if (i >= m_crosshairFiles.Count)
				{
					i -= m_crosshairFiles.Count;
				}
				else if (i < 0)
				{
					i += m_crosshairFiles.Count;
				}
				return i;
			}
		}

		public void OnTabSwitchClick(MyGuiControlButton sender)
		{
			SelectTab((PageEnum)sender.UserData);
		}
	}
}
