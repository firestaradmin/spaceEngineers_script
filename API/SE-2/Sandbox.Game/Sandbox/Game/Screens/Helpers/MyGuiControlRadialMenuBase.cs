using System;
using System.Collections.Generic;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.Helpers
{
	public abstract class MyGuiControlRadialMenuBase : MyGuiScreenBase
	{
		protected static readonly TimeSpan MOVEMENT_SUPPRESSION = TimeSpan.FromSeconds(2.0);

		protected static readonly float RADIUS = 0.11f;

		protected static readonly Dictionary<MyDefinitionId, int> m_lastSelectedSection = new Dictionary<MyDefinitionId, int>();

		private const int ITEMS_COUNT = 8;

		protected List<MyGuiControlImageRotatable> m_buttons;

		private List<MyGuiControlImage> m_tabs;

		private List<MyGuiControlLabel> m_tabLabels;

		protected List<MyGuiControlImage> m_icons;

		protected MyRadialMenu m_data;

		protected MyGuiControlLabel m_tooltipName;

		protected MyGuiControlLabel m_tooltipState;

		protected MyGuiControlLabel m_tooltipShortcut;

		protected MyGuiControlImage m_cancelButton;

		protected int m_selectedButton = -1;

		private MyGuiControlLabel m_leftButtonHint;

		private MyGuiControlLabel m_rightButtonHint;

		private readonly Func<bool> m_handleInputCallback;

		private readonly MyStringId m_closingControl;

		protected int m_currentSection;

		private Vector2 m_tabSize = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * 1.25f;

		private Vector2 m_tabSizeSmall = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * new Vector2(0.04f, 1f);

		protected const float HINTS_POS_Y = 0.365f;

		private float CATEGORY_POS_Y = -0.5f + MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.Y / 2f + 0.08f;

		private float m_hintYPos = 0.365f;

		private float m_hintIconYPosOffset = 0.005f;

		private MyGuiControlImageRotatable m_buttonHighlight;

		/// <summary>
		/// Index of the currently selected tab.
		/// </summary>
		public int CurrentTabIndex => m_currentSection;

		protected MyGuiControlRadialMenuBase(MyRadialMenu data, MyStringId closingControl, Func<bool> handleInputCallback)
			: base(null, null, null, isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_isTopMostScreen = true;
			base.DrawMouseCursor = false;
			m_closeOnEsc = true;
			m_closingControl = closingControl;
			m_handleInputCallback = handleInputCallback;
			MyCharacter.OnCharacterDied += MyCharacter_OnCharacterDied;
			m_buttons = new List<MyGuiControlImageRotatable>();
			m_tabs = new List<MyGuiControlImage>();
			m_tabLabels = new List<MyGuiControlLabel>();
			m_icons = new List<MyGuiControlImage>();
			m_data = data;
			m_tooltipName = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				Font = "Blue",
				UseTextShadow = true,
				Visible = false
			};
			m_tooltipName.TextScale *= 1.2f;
			m_tooltipName.RecalculateSize();
			m_tooltipState = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				Font = "Blue",
				UseTextShadow = true,
				Visible = false
			};
			m_tooltipShortcut = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				Font = "Blue",
				UseTextShadow = true,
				Visible = false
			};
			m_tooltipShortcut.TextScale = 1f;
			float num = 0.005f;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(backgroundColor: new Vector4(1f, 1f, 1f, 0.8f), position: new Vector2(-0.19f, 0.365f), size: MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui, backgroundTexture: null, textures: new string[1] { "Textures\\GUI\\Controls\\button_default_outlineless.dds" });
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(myGuiControlImage.Position, null, string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintClose), MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL).ToString()));
			myGuiControlLabel.PositionY += myGuiControlLabel.Size.Y / 2f + num;
			myGuiControlLabel.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			AddControl(myGuiControlImage);
			AddControl(myGuiControlLabel);
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(backgroundColor: new Vector4(1f, 1f, 1f, 0.8f), position: new Vector2(0.19f, 0.365f), size: MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui, backgroundTexture: null, textures: new string[1] { "Textures\\GUI\\Controls\\button_default_outlineless.dds" });
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(myGuiControlImage2.Position, null, string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintConfirm), MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT).ToString()), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			myGuiControlLabel2.PositionY += myGuiControlLabel2.Size.Y / 2f + num;
			myGuiControlLabel2.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			AddControl(myGuiControlImage2);
			AddControl(myGuiControlLabel2);
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
			myGuiControlImage3.SetTexture("Textures\\GUI\\Controls\\RadialMenuBackground.dds");
			myGuiControlImage3.Size = new Vector2(884f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			AddControl(myGuiControlImage3);
			MyGuiControlImage myGuiControlImage4 = new MyGuiControlImage();
			myGuiControlImage4.SetTexture("Textures\\GUI\\Controls\\RadialOuterCircle.dds");
			myGuiControlImage4.Size = new Vector2(632f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			AddControl(myGuiControlImage4);
			MyGuiControlImage myGuiControlImage5 = new MyGuiControlImage();
			myGuiControlImage5.SetTexture("Textures\\GUI\\Controls\\RadialBrackets.dds");
			myGuiControlImage5.Size = new Vector2(674f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			AddControl(myGuiControlImage5);
			foreach (MyRadialMenuSection currentSection in m_data.CurrentSections)
			{
				MyGuiControlImage myGuiControlImage6 = new MyGuiControlImage(null, null, new Vector4(1f, 1f, 1f, 0.8f));
				myGuiControlImage6.SetTexture("Textures\\GUI\\Controls\\button_default_outlineless.dds");
				m_tabs.Add(myGuiControlImage6);
				AddControl(myGuiControlImage6);
				MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER
				};
				myGuiControlLabel3.Text = MyTexts.GetString(currentSection.Label);
				myGuiControlLabel3.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_tabLabels.Add(myGuiControlLabel3);
				AddControl(myGuiControlLabel3);
			}
			for (int i = 0; i < 8; i++)
			{
				MyGuiControlImageRotatable myGuiControlImageRotatable = new MyGuiControlImageRotatable();
				myGuiControlImageRotatable.SetTexture("Textures\\GUI\\Controls\\RadialSectorUnSelected.dds");
				float num3 = (myGuiControlImageRotatable.Rotation = (float)Math.PI / 4f * (float)i);
				myGuiControlImageRotatable.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				myGuiControlImageRotatable.Position = new Vector2((float)Math.Cos(num3 - (float)Math.E * 449f / 777f), (float)Math.Sin(num3 - (float)Math.E * 449f / 777f)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
				m_buttons.Add(myGuiControlImageRotatable);
				AddControl(myGuiControlImageRotatable);
			}
			GenerateIcons(8);
			m_buttonHighlight = new MyGuiControlImageRotatable();
			m_buttonHighlight.SetTexture("Textures\\GUI\\Controls\\RadialSectorSelected.dds");
			m_buttonHighlight.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonHighlight.Visible = false;
			AddControl(m_buttonHighlight);
			AddControl(m_tooltipName);
			AddControl(m_tooltipState);
			AddControl(m_tooltipShortcut);
			m_cancelButton = new MyGuiControlImage();
			AddControl(m_cancelButton);
			m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircle.dds");
			m_cancelButton.Size = new Vector2(126f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			MyGuiControlImage myGuiControlImage7 = new MyGuiControlImage();
			AddControl(myGuiControlImage7);
			myGuiControlImage7.SetTexture("Textures\\GUI\\Icons\\HideWeapon.dds");
			myGuiControlImage7.Size = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			AddControl(m_leftButtonHint = new MyGuiControlLabel(null, null, MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT).ToString(), null, 1f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER)
			{
				Position = new Vector2(-0.045f - MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.X / 2f, CATEGORY_POS_Y)
			});
			AddControl(m_rightButtonHint = new MyGuiControlLabel(null, null, MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT).ToString(), null, 1f)
			{
				Position = new Vector2(0.04f + MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.X / 2f, CATEGORY_POS_Y)
			});
			m_hintYPos = CATEGORY_POS_Y + (m_tabSizeSmall.Y - m_leftButtonHint.Size.Y) / 2f;
			m_leftButtonHint.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_rightButtonHint.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_leftButtonHint.PositionY = m_hintYPos + m_leftButtonHint.Size.Y / 2f - m_hintIconYPosOffset;
			m_rightButtonHint.PositionY = m_hintYPos + m_leftButtonHint.Size.Y / 2f - m_hintIconYPosOffset;
			UpdateHighlight(-1, -1);
			MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
			GetTexturesForPreload(hashSet);
			MyRenderProxy.PreloadTextures(hashSet, TextureType.GUI);
=======
			HashSet<string> val = new HashSet<string>();
			GetTexturesForPreload(val);
			MyRenderProxy.PreloadTextures((IEnumerable<string>)val, TextureType.GUI);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void MyCharacter_OnCharacterDied(MyCharacter obj)
		{
			CloseScreenNow();
		}

		protected virtual void GenerateIcons(int maxSize)
		{
			for (int i = 0; i < maxSize; i++)
			{
				MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
				myGuiControlImage.Size = new Vector2(65f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				m_icons.Add(myGuiControlImage);
				AddControl(myGuiControlImage);
				float num = (float)Math.PI * 2f / (float)maxSize * (float)i - (float)Math.E * 449f / 777f;
				myGuiControlImage.Position = new Vector2(1f, 1.33333337f) * new Vector2((float)Math.Cos(num), (float)Math.Sin(num)) * RADIUS;
			}
		}

		protected bool Cancel()
		{
			if (MySession.Static.LocalCharacter == null)
			{
				return false;
			}
			(MySession.Static.ControlledEntity as MyCharacter)?.SwitchToWeapon((MyToolbarItemWeapon)null);
			MySessionComponentVoxelHand.Static.Enabled = false;
			MyClipboardComponent.Static?.HandleEscapeInternal();
			CloseScreen();
			return true;
		}

		protected override void OnClosed()
		{
			MyToolSwitcher component = MySession.Static.GetComponent<MyToolSwitcher>();
			component.SwitchingEnabled = false;
			component.ToolSwitched -= CloseScreenNow;
			MyGuiScreenGamePlay.Static.SuppressMovement = MySession.Static.ElapsedGameTime + MOVEMENT_SUPPRESSION;
			base.OnClosed();
		}

		protected bool ButtonAction(int section, int itemIndex)
		{
			List<MyRadialMenuItem> items = m_data.CurrentSections[section].Items;
			if (items.Count <= itemIndex)
			{
				return false;
			}
			MyRadialMenuItem myRadialMenuItem = items[itemIndex];
			if (!myRadialMenuItem.CanBeActivated)
			{
				return false;
			}
			ActivateItem(myRadialMenuItem);
			if (myRadialMenuItem.CloseMenu)
			{
				CloseScreen();
			}
			else
			{
				UpdateTooltip();
				UpdateIcon();
			}
			return true;
		}

		protected virtual void UpdateIcon()
		{
			MyRadialMenuSection myRadialMenuSection = m_data.CurrentSections[m_currentSection];
			for (int i = 0; i < myRadialMenuSection.Items.Count; i++)
			{
				m_icons[i].SetTexture(myRadialMenuSection.Items[i].GetIcon());
			}
		}

		protected virtual void ActivateItem(MyRadialMenuItem item)
		{
			item.Activate();
		}

		protected void SwitchSection(int index)
		{
			List<MyRadialMenuSection> currentSections = m_data.CurrentSections;
			int num = (m_currentSection = ((index >= 0) ? ((index >= currentSections.Count) ? (currentSections.Count - 1) : index) : 0));
			m_lastSelectedSection[m_data.Id] = num;
			float cATEGORY_POS_Y = CATEGORY_POS_Y;
			MyRadialMenuSection iconTextures = m_data.CurrentSections[num];
			for (int i = 0; i < m_tabs.Count; i++)
			{
				if (i == num)
				{
					Vector2 vector3 = (m_tabs[i].Position = (m_tabLabels[i].Position = new Vector2(0f, cATEGORY_POS_Y)));
					m_tabs[i].Size = m_tabSize;
					m_tabs[i].SetTexture("Textures\\GUI\\Controls\\button_default_outlineless_active.dds");
					m_tabLabels[i].IsAutoScaleEnabled = true;
					m_tabLabels[i].IsAutoEllipsisEnabled = true;
					m_tabLabels[i].SetMaxWidth(m_tabs[i].Size.X);
					m_tabLabels[i].DoEllipsisAndScaleAdjust(RecalculateSize: true, 0.8f, resetEllipsis: true);
					m_tabLabels[i].PositionY = m_hintYPos;
					m_tabLabels[i].DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				}
				else
				{
					if (Math.Abs(i - num) == 1)
					{
						m_tabs[i].Position = new Vector2((float)(i - num) * 0.21f, cATEGORY_POS_Y);
						m_tabs[i].Size = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui;
						m_tabLabels[i].IsAutoScaleEnabled = true;
						m_tabLabels[i].IsAutoEllipsisEnabled = true;
						m_tabLabels[i].SetMaxWidth(m_tabs[i].Size.X - m_leftButtonHint.Size.X - 0.025f);
						m_tabLabels[i].DoEllipsisAndScaleAdjust(RecalculateSize: true, 0.8f, resetEllipsis: true);
						m_tabLabels[i].Position = new Vector2((float)(i - num) * 0.22f, m_hintYPos);
						m_tabLabels[i].DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
					}
					else
					{
						m_tabs[i].Position = new Vector2((float)Math.Sign(i - num) * 0.285f + (float)(i - num) * 0.01f, cATEGORY_POS_Y);
						m_tabs[i].Size = m_tabSizeSmall;
						m_tabLabels[i].IsAutoScaleEnabled = true;
						m_tabLabels[i].IsAutoEllipsisEnabled = true;
						m_tabLabels[i].SetMaxWidth(m_tabs[i].Size.X);
						m_tabLabels[i].DoEllipsisAndScaleAdjust(RecalculateSize: true, 0.8f, resetEllipsis: true);
						m_tabLabels[i].PositionY = m_hintYPos;
						m_tabLabels[i].DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
					}
					m_tabs[i].SetTexture("Textures\\GUI\\Controls\\button_default_outlineless.dds");
				}
				m_tabLabels[i].Visible = Math.Abs(i - num) <= 1;
			}
			SetIconTextures(iconTextures);
			m_tooltipName.Visible = false;
			m_tooltipState.Visible = false;
			m_tooltipShortcut.Visible = false;
			m_leftButtonHint.Visible = num != 0;
			m_rightButtonHint.Visible = num != m_data.CurrentSections.Count - 1;
			UpdateTooltip();
			RegenerateBlockHints();
		}

		protected virtual void SetIconTextures(MyRadialMenuSection selectedSection)
		{
			for (int i = 0; i < m_buttons.Count; i++)
			{
				MyGuiControlImageRotatable myGuiControlImageRotatable = m_buttons[i];
				MyGuiControlImage myGuiControlImage = m_icons[i];
				if (i < selectedSection.Items.Count)
				{
					bool visible = (myGuiControlImage.Visible = true);
					myGuiControlImageRotatable.Visible = visible;
					MyRadialMenuItem myRadialMenuItem = selectedSection.Items[i];
					myGuiControlImage.SetTexture(myRadialMenuItem.GetIcon());
					myGuiControlImage.ColorMask = (myRadialMenuItem.Enabled() ? Color.White : Color.Gray);
				}
				else
				{
					myGuiControlImage.Visible = false;
				}
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (!MyInput.Static.IsJoystickLastUsed)
			{
				CloseScreen();
				return base.Update(hasFocus);
			}
			Vector3 joystickPositionForGameplay = MyInput.Static.GetJoystickPositionForGameplay(RequestedJoystickAxis.NoZ);
			Vector3 joystickRotationForGameplay = MyInput.Static.GetJoystickRotationForGameplay(RequestedJoystickAxis.NoZ);
			Vector3 value = (Vector3.IsZero(joystickPositionForGameplay) ? joystickRotationForGameplay : joystickPositionForGameplay);
			if (!Vector3.IsZero(value))
			{
				int num = (int)Math.Round((6.2831854820251465 + (1.570796012878418 + Math.Atan2(value.Y, value.X))) % 6.2831854820251465 / (double)((float)Math.PI * 2f / (float)m_buttons.Count)) % m_buttons.Count;
				if (num != m_selectedButton)
				{
					UpdateHighlight(m_selectedButton, num);
					m_selectedButton = num;
					UpdateTooltip();
					RegenerateBlockHints();
					MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
				}
			}
			else if (m_cancelButton != null && m_selectedButton != -1)
			{
				UpdateHighlight(m_selectedButton, -1);
				m_selectedButton = -1;
				UpdateTooltip();
				RegenerateBlockHints();
				MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
			}
			return base.Update(hasFocus);
		}

		protected virtual void RegenerateBlockHints()
		{
		}

		protected abstract void UpdateTooltip();

		protected virtual void UpdateHighlight(int oldIndex, int newIndex)
		{
			if (oldIndex == -1)
			{
				m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircle.dds");
			}
			if (newIndex == -1)
			{
				m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircleSelected.dds");
				m_buttonHighlight.Visible = false;
				return;
			}
			float num = (float)Math.PI / 4f * (float)newIndex;
			m_buttonHighlight.Rotation = num;
			m_buttonHighlight.Position = new Vector2((float)Math.Cos(num - (float)Math.E * 449f / 777f), (float)Math.Sin(num - (float)Math.E * 449f / 777f)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonHighlight.Visible = true;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) && m_currentSection > 0)
			{
				SwitchSection(m_currentSection - 1);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT) && m_data.CurrentSections.Count > m_currentSection + 1)
			{
				SwitchSection(m_currentSection + 1);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT))
			{
				if ((m_selectedButton == -1) ? Cancel() : ButtonAction(m_currentSection, m_selectedButton))
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
				else
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				}
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, m_closingControl) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL))
			{
				CloseScreen();
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			else if (m_handleInputCallback?.Invoke() ?? false)
			{
				CloseScreen();
			}
		}

		public virtual void GetTexturesForPreload(HashSet<string> textures)
		{
			foreach (MyGuiControlBase control in Controls)
			{
				MyGuiControlImage myGuiControlImage;
				if ((myGuiControlImage = control as MyGuiControlImage) != null && myGuiControlImage.Textures != null)
				{
					MyGuiControlImage.MyDrawTexture[] textures2 = myGuiControlImage.Textures;
					for (int i = 0; i < textures2.Length; i++)
					{
						MyGuiControlImage.MyDrawTexture myDrawTexture = textures2[i];
						Add(myDrawTexture.Texture);
						Add(myDrawTexture.MaskTexture);
					}
				}
			}
			foreach (MyRadialMenuSection item in m_data.SectionsComplete)
			{
				foreach (MyRadialMenuItem item2 in item.Items)
				{
					Add(item2.GetIcon());
				}
			}
			void Add(string texture)
			{
				if (!string.IsNullOrEmpty(texture))
				{
					textures.Add(texture);
				}
			}
		}

		public override string GetFriendlyName()
		{
			return "RadialMenu";
		}
	}
}
