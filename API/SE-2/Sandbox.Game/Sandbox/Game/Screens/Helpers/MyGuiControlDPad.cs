using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlDPad : MyGuiControlBase, IDisposable
	{
		private enum MyDPadVisualLayouts
		{
			Classic,
			ColorPicker
		}

		private class MyBlinkAnimator
		{
			public MyGuiControlImageRotatable Control;

			public int Duration = -1;

			public bool Highlighted;
		}

		private static readonly string DEFAULT_SKIN = "Textures\\GUI\\Icons\\Skins\\Armor\\DefaultArmor.DDS";

		private MyObjectBuilder_DPadControlVisualStyle m_style;

		private MyStringId m_lastContext;

		private MyGuiControlImage m_upImage;

		private MyGuiControlImage m_leftImage;

		private MyGuiControlImage m_rightImage;

		private MyGuiControlImage m_downImage;

		private MyGuiControlImage m_upImageTop;

		private MyGuiControlImage m_leftImageTop;

		private MyGuiControlImage m_rightImageTop;

		private MyGuiControlImage m_downImageTop;

		private MyGuiControlImage m_arrows;

		private MyGuiControlLabel m_bottomHintLeft;

		private MyGuiControlLabel m_bottomHintRight;

		private Vector2 m_subiconOffset;

		private MyGuiControlImageRotatable m_upBackground;

		private MyGuiControlImageRotatable m_upMidground;

		private MyGuiControlImageRotatable m_leftBackground;

		private MyGuiControlImageRotatable m_rightBackground;

		private MyGuiControlImageRotatable m_downBackground;

		private List<MyGuiControlImage> m_images;

		private List<MyGuiControlImageRotatable> m_backgrounds;

		private List<MyGuiControlImageRotatable> m_midgrounds;

		private MyDPadVisualLayouts m_visuals;

		private MyGuiControlImageRotatable m_upBackgroundColor;

		private MyGuiControlImageRotatable m_leftBackgroundColor;

		private MyGuiControlImageRotatable m_rightBackgroundColor;

		private MyGuiControlImageRotatable m_downBackgroundColor;

		private MyGuiControlImage m_centerImageInner;

		private MyGuiControlImage m_centerImageOuter;

		private List<MyGuiControlImage> m_imagesColor;

		private List<MyGuiControlImageRotatable> m_backgroundsColor;

		private MyGuiControlLabel m_upLabel;

		private MyGuiControlLabel m_leftLabel;

		private MyGuiControlLabel m_rightLabel;

		private MyGuiControlLabel m_downLabel;

		private Func<string> m_upFunc;

		private Func<string> m_leftFunc;

		private Func<string> m_rightFunc;

		private Func<string> m_downFunc;

		private MyToolSwitcher m_toolSwitcher;

		private MyDefinitionBase m_handWeaponDefinition;

		private bool m_keepHandWeaponAmmoCount;

		private bool m_preloadSkins = true;

		private List<MyBlinkAnimator> m_activeAnimators = new List<MyBlinkAnimator>(4);

		private static int BLINK_DURATION = 5;

		private bool[] m_canBlink = new bool[4];

		private MyDPadVisualLayouts Visuals
		{
			get
			{
				return m_visuals;
			}
			set
			{
				m_visuals = value;
			}
		}

		public MyGuiControlDPad(MyObjectBuilder_DPadControlVisualStyle style)
		{
			m_style = style;
			if (m_style.VisibleCondition != null)
			{
				InitStatConditions(m_style.VisibleCondition);
			}
			m_images = new List<MyGuiControlImage>();
			m_backgrounds = new List<MyGuiControlImageRotatable>();
			m_midgrounds = new List<MyGuiControlImageRotatable>();
			m_imagesColor = new List<MyGuiControlImage>();
			m_backgroundsColor = new List<MyGuiControlImageRotatable>();
			m_backgrounds.Add(m_upBackground = new MyGuiControlImageRotatable());
			m_midgrounds.Add(m_upMidground = new MyGuiControlImageRotatable());
			m_backgrounds.Add(m_leftBackground = new MyGuiControlImageRotatable());
			m_backgrounds.Add(m_rightBackground = new MyGuiControlImageRotatable());
			m_backgrounds.Add(m_downBackground = new MyGuiControlImageRotatable());
			Vector2 vector = new Vector2(200f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_upBackground.Size = (m_upMidground.Size = (m_leftBackground.Size = (m_rightBackground.Size = (m_downBackground.Size = vector))));
			SetBackgroundTexture(ref m_upBackground);
			SetBackgroundTexture(ref m_leftBackground);
			SetBackgroundTexture(ref m_rightBackground);
			SetBackgroundTexture(ref m_downBackground);
			m_leftBackground.Rotation = (float)Math.E * -449f / 777f;
			m_rightBackground.Rotation = (float)Math.E * 449f / 777f;
			m_downBackground.Rotation = (float)Math.PI;
			Vector2 vector6 = new Vector2(48f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_subiconOffset = new Vector2(vector6.X * 0.33f, vector6.Y * -0.33f);
			m_images.Add(m_upImage = new MyGuiControlImage(null, vector6));
			m_images.Add(m_leftImage = new MyGuiControlImage(null, vector6));
			m_images.Add(m_rightImage = new MyGuiControlImage(null, vector6));
			m_images.Add(m_downImage = new MyGuiControlImage(null, vector6));
			m_images.Add(m_upImageTop = new MyGuiControlImage(null, vector6 / 3f));
			m_images.Add(m_leftImageTop = new MyGuiControlImage(null, vector6 / 3f));
			m_images.Add(m_rightImageTop = new MyGuiControlImage(null, vector6 / 3f));
			m_images.Add(m_downImageTop = new MyGuiControlImage(null, vector6 / 3f));
			Vector2 value = new Vector2(200f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_arrows = new MyGuiControlImage(null, value);
			m_arrows.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector_arrows.png");
			m_bottomHintLeft = new MyGuiControlLabel(Vector2.Zero, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_bottomHintRight = new MyGuiControlLabel(Vector2.Zero, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_backgroundsColor.Add(m_upBackgroundColor = new MyGuiControlImageRotatable());
			m_backgroundsColor.Add(m_downBackgroundColor = new MyGuiControlImageRotatable());
			m_backgroundsColor.Add(m_leftBackgroundColor = new MyGuiControlImageRotatable());
			m_backgroundsColor.Add(m_rightBackgroundColor = new MyGuiControlImageRotatable());
			m_upBackgroundColor.Size = (m_leftBackgroundColor.Size = (m_rightBackgroundColor.Size = (m_downBackgroundColor.Size = vector)));
			m_leftBackgroundColor.Rotation = (float)Math.E * -449f / 777f;
			m_rightBackgroundColor.Rotation = (float)Math.E * 449f / 777f;
			m_downBackgroundColor.Rotation = (float)Math.PI;
			m_imagesColor.Add(m_centerImageInner = new MyGuiControlImage(null, vector));
			m_imagesColor.Add(m_centerImageOuter = new MyGuiControlImage(null, vector));
			m_upBackgroundColor.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_downBackgroundColor.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_centerImageOuter.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTMiddleCircle.dds");
			m_activeAnimators.Add(new MyBlinkAnimator
			{
				Control = m_upBackground,
				Duration = -1,
				Highlighted = false
			});
			m_activeAnimators.Add(new MyBlinkAnimator
			{
				Control = m_leftBackground,
				Duration = -1,
				Highlighted = false
			});
			m_activeAnimators.Add(new MyBlinkAnimator
			{
				Control = m_rightBackground,
				Duration = -1,
				Highlighted = false
			});
			m_activeAnimators.Add(new MyBlinkAnimator
			{
				Control = m_downBackground,
				Duration = -1,
				Highlighted = false
			});
			float textScale = 0.45f;
			m_upLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_leftLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_rightLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_downLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			MyCubeBuilder.Static.OnActivated += RefreshIcons;
			MyCubeBuilder.Static.OnDeactivated += RefreshIcons;
			MyCubeBuilder.Static.OnBlockVariantChanged += RefreshIcons;
			MyCubeBuilder.Static.OnSymmetrySetupModeChanged += RefreshIcons;
			MyCockpit.OnPilotAttached += RefreshIcons;
			MySession.Static.OnLocalPlayerSkinOrColorChanged += RefreshIcons;
			MyCubeBuilder.Static.OnToolTypeChanged += RefreshIcons;
			MySessionComponentVoxelHand.Static.OnEnabledChanged += RefreshIcons;
			MySessionComponentVoxelHand.Static.OnBrushChanged += RefreshIcons;
			m_toolSwitcher = MySession.Static.GetComponent<MyToolSwitcher>();
			m_toolSwitcher.ToolsRefreshed += RefreshIcons;
			MySession.Static.GetComponent<MyEmoteSwitcher>().OnActiveStateChanged += RefreshIcons;
			MySession.Static.GetComponent<MyEmoteSwitcher>().OnPageChanged += RefreshIcons;
			MyGuiControlRadialMenuBlock.OnSelectionConfirmed += RefreshIconsRadialBlock;
			if (MyGuiScreenGamePlay.Static != null)
			{
				MyGuiScreenGamePlay.Static.OnHelmetChanged += RefreshIcons;
				MyGuiScreenGamePlay.Static.OnHeadlightChanged += RefreshIcons;
			}
		}

		internal void UnregisterEvents()
		{
			MyCockpit.OnPilotAttached -= RefreshIcons;
			MyGuiControlRadialMenuBlock.OnSelectionConfirmed -= RefreshIconsRadialBlock;
		}

		public void RefreshIcons()
		{
			CleanUp(full: false);
			if (MySession.Static.GetComponent<MyEmoteSwitcher>().IsActive)
			{
				RefreshEmoteIcons();
				return;
			}
			MyStringId myStringId = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyControlStateType.PRESSED))
			{
				if (myStringId == MySpaceBindingCreator.AX_TOOLS)
				{
					RefreshToolShortcuts();
				}
				else if (myStringId == MySpaceBindingCreator.AX_ACTIONS)
				{
					RefreshShipToolbarShortcuts();
				}
				else if (myStringId == MySpaceBindingCreator.AX_BUILD)
				{
					RefreshBuildingShortcutIcons();
				}
				else if (myStringId == MySpaceBindingCreator.AX_VOXEL)
				{
					RefreshVoxelShortcutIcons();
				}
				else if (myStringId == MySpaceBindingCreator.AX_COLOR_PICKER)
				{
					RefreshColorShortcutIcons();
				}
				else if (myStringId == MySpaceBindingCreator.AX_CLIPBOARD)
				{
					RefreshClipboardShortcutIcons();
				}
				else
				{
					RefreshEmptyIcons();
				}
			}
			else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyControlStateType.PRESSED))
			{
				if ((MySession.Static.ControlledEntity?.ControlContext ?? MyStringId.NullOrEmpty) == MySpaceBindingCreator.CX_SPACESHIP)
				{
					RefreshShipShortcutIcons();
				}
				else
				{
					RefreshCharacterShortcutIcons();
				}
			}
			else if (myStringId == MySpaceBindingCreator.AX_TOOLS)
			{
				RefreshToolIcons();
			}
			else if (myStringId == MySpaceBindingCreator.AX_ACTIONS)
			{
				RefreshShipToolbarIcons();
			}
			else if (myStringId == MySpaceBindingCreator.AX_BUILD)
			{
				RefreshBuildingIcons();
			}
			else if (myStringId == MySpaceBindingCreator.AX_SYMMETRY)
			{
				RefreshSymmetryIcons();
			}
			else if (myStringId == MySpaceBindingCreator.AX_COLOR_PICKER)
			{
				RefreshColorIcons();
			}
			else if (myStringId == MySpaceBindingCreator.AX_CLIPBOARD)
			{
				RefreshClipboardIcons();
			}
			else if (myStringId == MySpaceBindingCreator.AX_VOXEL)
			{
				RefreshVoxelIcons();
			}
		}

		private void CleanUp(bool full)
		{
			m_upImage.ColorMask = Vector4.One;
			m_leftImage.ColorMask = Vector4.One;
			m_rightImage.ColorMask = Vector4.One;
			m_downImage.ColorMask = Vector4.One;
			m_upMidground.SetTexture(string.Empty);
			m_upImageTop.SetTexture(string.Empty);
			m_leftImageTop.SetTexture(string.Empty);
			m_rightImageTop.SetTexture(string.Empty);
			m_downImageTop.SetTexture(string.Empty);
			m_upImageTop.ColorMask = Vector4.One;
			m_leftImageTop.ColorMask = Vector4.One;
			m_rightImageTop.ColorMask = Vector4.One;
			m_downImageTop.ColorMask = Vector4.One;
			SetBackgroundTexture(ref m_activeAnimators[0].Control, m_activeAnimators[0].Highlighted);
			SetBackgroundTexture(ref m_activeAnimators[1].Control, m_activeAnimators[1].Highlighted);
			SetBackgroundTexture(ref m_activeAnimators[2].Control, m_activeAnimators[2].Highlighted);
			SetBackgroundTexture(ref m_activeAnimators[3].Control, m_activeAnimators[3].Highlighted);
			m_bottomHintLeft.Text = string.Empty;
			m_bottomHintRight.Text = string.Empty;
			Visuals = MyDPadVisualLayouts.Classic;
			if (full)
			{
				m_upLabel.Text = string.Empty;
				m_leftLabel.Text = string.Empty;
				m_rightLabel.Text = string.Empty;
				m_downLabel.Text = string.Empty;
				m_upLabel.ColorMask = Vector4.One;
				m_leftLabel.ColorMask = Vector4.One;
				m_rightLabel.ColorMask = Vector4.One;
				m_downLabel.ColorMask = Vector4.One;
				m_upFunc = null;
				m_leftFunc = null;
				m_rightFunc = null;
				m_downFunc = null;
				m_handWeaponDefinition = null;
				m_keepHandWeaponAmmoCount = false;
			}
		}

		private void RefreshShipShortcutIcons()
		{
			Vector4 colorMask = Vector4.One;
<<<<<<< HEAD
=======
			_ = MySession.Static.LocalCharacter;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MySession.Static.ControlledEntity != null && !MySession.Static.ControlledEntity.EnabledLights)
			{
				colorMask = new Vector4(0.5f);
			}
<<<<<<< HEAD
			Vector4 colorMask2 = (((MySession.Static.ControlledEntity as MyShipController)?.CubeGrid?.IsPowered ?? true) ? Vector4.One : new Vector4(1f, 0f, 0f, 1f));
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchCamera.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\LightCenter.png");
			m_rightImage.ColorMask = colorMask;
<<<<<<< HEAD
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleConnectedGrid.png");
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\GridPowerOnCenter.png");
			m_leftImage.ColorMask = colorMask2;
			m_downImage.ColorMask = colorMask2;
			m_leftLabel.Text = string.Empty;
		}

		private void RefreshCharacterShortcutIcons()
		{
			Vector4 colorMask = Vector4.One;
			string texture = "Textures\\GUI\\Icons\\HUD 2017\\PlayerHelmetOn.png";
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.OxygenComponent != null && !localCharacter.OxygenComponent.HelmetEnabled)
			{
				colorMask = new Vector4(0.5f);
				texture = "Textures\\GUI\\Icons\\HUD 2017\\PlayerHelmetOff.png";
			}
			Vector4 colorMask2 = Vector4.One;
			if (localCharacter != null && !localCharacter.LightEnabled)
			{
				colorMask2 = new Vector4(0.5f);
			}
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchCamera.png");
			m_leftImage.SetTexture(texture);
			m_leftImage.ColorMask = colorMask;
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\LightCenter.png");
			m_rightImage.ColorMask = colorMask2;
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ColorPicker.png");
		}

		private void RefreshToolIcons()
		{
			string[] nextToolImage = GetNextToolImage(MyToolSwitcher.ToolType.Drill);
			m_upImage.SetTextures(nextToolImage);
			string[] nextToolImage2 = GetNextToolImage(MyToolSwitcher.ToolType.Welder);
			m_rightImage.SetTextures(nextToolImage2);
			string[] nextToolImage3 = GetNextToolImage(MyToolSwitcher.ToolType.Grinder);
			m_leftImage.SetTextures(nextToolImage3);
			string[] nextToolImage4 = GetNextToolImage(MyToolSwitcher.ToolType.Weapon);
			m_downImage.SetTextures(nextToolImage4);
			m_upLabel.Text = string.Empty;
			m_leftLabel.Text = string.Empty;
			m_rightLabel.Text = string.Empty;
			m_upFunc = null;
			m_leftFunc = null;
			m_rightFunc = null;
			m_keepHandWeaponAmmoCount = m_handWeaponDefinition != null;
			m_handWeaponDefinition = GetWeaponDefinition();
			if (m_handWeaponDefinition != null)
			{
				m_downFunc = GetAmmoCount;
			}
			SetBackgroundTexture(ref m_upBackground, m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Drill));
			SetBackgroundTexture(ref m_leftBackground, m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Grinder));
			SetBackgroundTexture(ref m_rightBackground, m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Welder));
			SetBackgroundTexture(ref m_downBackground, m_toolSwitcher.IsEquipped(MyToolSwitcher.ToolType.Weapon));
		}

		private void SetBackgroundTexture(ref MyGuiControlImageRotatable background, bool isHighlighted = false)
		{
			if (isHighlighted)
			{
				background.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSectorOn.png");
			}
			else
			{
				background.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector.png");
			}
		}

		private void RefreshIconsRadialBlock(MyGuiControlRadialMenuBlock menu)
		{
			RefreshIcons();
		}

		private string GetAmmoCount()
		{
			if (m_handWeaponDefinition == null)
			{
				return null;
			}
			bool flag = false;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && (localCharacter.FindWeaponItemByDefinition(m_handWeaponDefinition.Id).HasValue || !localCharacter.WeaponTakesBuilderFromInventory(m_handWeaponDefinition.Id)))
			{
				IMyHandheldGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
				if (currentWeapon != null)
				{
					flag = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id == m_handWeaponDefinition.Id;
				}
				if (localCharacter.LeftHandItem != null)
				{
					flag |= m_handWeaponDefinition == localCharacter.LeftHandItem.PhysicalItemDefinition;
				}
				if (flag && currentWeapon != null)
				{
					MyWeaponItemDefinition myWeaponItemDefinition = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId) as MyWeaponItemDefinition;
					if (myWeaponItemDefinition != null && myWeaponItemDefinition.ShowAmmoCount)
					{
						return $"{localCharacter.CurrentWeapon.GetAmmunitionAmount().ToString()} • {localCharacter.CurrentWeapon.GetMagazineAmount()}";
					}
				}
			}
			if (m_keepHandWeaponAmmoCount)
			{
				return null;
			}
			return "0 • 0";
		}

		private void RefreshBuildingShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleSymmetry.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Autorotate.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
		}

		private void RefreshShipToolbarShortcuts()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Contract.png");
			m_leftImage.SetTexture(string.Empty);
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleHud.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Chat.png");
		}

		private void RefreshToolShortcuts()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Contract.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ProgressionTree.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleHud.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Chat.png");
		}

		private void RefreshBuildingIcons()
		{
			MyCubeBlockDefinition currentBlockDefinition = MyCubeBuilder.Static.CurrentBlockDefinition;
			if (currentBlockDefinition != null)
			{
				m_upImage.SetTextures(currentBlockDefinition.Icons);
			}
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateCounterClockwise.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
		}

		private void RefreshSymmetryIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\CloseSymmetrySetup.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RemoveSymmetryPlane.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\PlaceSymmetryPlane.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchSymmetryAxis.png");
		}

		private void RefreshEmptyIcons()
		{
			m_upImage.SetTexture(string.Empty);
			m_leftImage.SetTexture(string.Empty);
			m_rightImage.SetTexture(string.Empty);
			m_downImage.SetTexture(string.Empty);
		}

		private void RefreshVoxelShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
			Vector4 vector3 = (m_leftImage.ColorMask = (m_rightImage.ColorMask = (MySessionComponentVoxelHand.Static.IsBrushRotationEnabled() ? Vector4.One : new Vector4(0.5f))));
		}

		private void RefreshVoxelIcons()
		{
			m_upImage.SetTexture(string.Empty);
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ScaleDown.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ScaleUp.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SetupVoxelHand.png");
			string currentMaterialTextureName = MySessionComponentVoxelHand.Static.CurrentMaterialTextureName;
			m_upMidground.SetTexture(currentMaterialTextureName, "Textures\\GUI\\Icons\\HUD 2017\\RadialSelectorVoxelCurrent.png");
		}

		private void RefreshColorShortcutIcons()
		{
			m_upImage.SetTexture(string.Empty);
			m_leftImage.SetTexture(string.Empty);
			m_rightImage.SetTexture(string.Empty);
			m_downImage.SetTexture(string.Empty);
		}

		private void RefreshColorIcons()
		{
			GetColors(out var colPrev, out var colCur, out var colNext);
			GetSkins(out var skinPrev, out var skinCur, out var skinNext);
			m_upBackgroundColor.ColorMask = colNext;
			m_centerImageOuter.ColorMask = colCur;
			m_downBackgroundColor.ColorMask = colPrev;
			m_leftBackgroundColor.SetTexture(skinPrev, "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_rightBackgroundColor.SetTexture(skinNext, "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_centerImageInner.SetTexture(skinCur, "Textures\\GUI\\Icons\\HUD 2017\\BCTCentralCircle.dds");
			Visuals = MyDPadVisualLayouts.ColorPicker;
		}

		private void RefreshClipboardShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Autorotate.png");
			m_rightImage.SetTexture(string.Empty);
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
		}

		private void RefreshClipboardIcons()
		{
			m_upImage.SetTexture(string.Empty);
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateCounterClockwise.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
		}

		private void GetSkins(out string skinPrev, out string skinCur, out string skinNext)
		{
			skinPrev = (skinCur = (skinNext = string.Empty));
			List<MyAssetModifierDefinition> list = new List<MyAssetModifierDefinition>();
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
=======
			HashSet<string> val = new HashSet<string>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
			{
				if (inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor)
				{
					MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), inventoryItem.ItemDefinition.AssetModifierId));
<<<<<<< HEAD
					if (assetModifierDefinition != null && !hashSet.Contains(inventoryItem.ItemDefinition.AssetModifierId))
					{
						list.Add(assetModifierDefinition);
						hashSet.Add(inventoryItem.ItemDefinition.AssetModifierId);
=======
					if (assetModifierDefinition != null && !val.Contains(inventoryItem.ItemDefinition.AssetModifierId))
					{
						list.Add(assetModifierDefinition);
						val.Add(inventoryItem.ItemDefinition.AssetModifierId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			string buildArmorSkin = MySession.Static.LocalHumanPlayer.BuildArmorSkin;
			int num = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Id.SubtypeName == buildArmorSkin)
				{
					num = i;
					break;
				}
			}
			int num2 = list.Count + 1;
			int num3 = (num + num2) % num2 - 1;
			int num4 = (num + 2) % num2 - 1;
			skinPrev = ((num3 == -1) ? DEFAULT_SKIN : list[num3].Icons[0]);
			skinCur = ((num == -1) ? DEFAULT_SKIN : list[num].Icons[0]);
			skinNext = ((num4 == -1) ? DEFAULT_SKIN : list[num4].Icons[0]);
			if (m_preloadSkins)
			{
				m_preloadSkins = false;
<<<<<<< HEAD
				MyRenderProxy.PreloadTextures(list.SelectMany((MyAssetModifierDefinition x) => x.Icons), TextureType.GUI);
=======
				MyRenderProxy.PreloadTextures(Enumerable.SelectMany<MyAssetModifierDefinition, string>((IEnumerable<MyAssetModifierDefinition>)list, (Func<MyAssetModifierDefinition, IEnumerable<string>>)((MyAssetModifierDefinition x) => x.Icons)), TextureType.GUI);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void GetColors(out Vector4 colPrev, out Vector4 colCur, out Vector4 colNext)
		{
			MySession.Static.LocalHumanPlayer.GetColorPreviousCurrentNext(out var prev, out var cur, out var next);
			colPrev = MyColorPickerConstants.HSVOffsetToHSV(prev).HSVtoColor();
			colCur = MyColorPickerConstants.HSVOffsetToHSV(cur).HSVtoColor();
			colNext = MyColorPickerConstants.HSVOffsetToHSV(next).HSVtoColor();
		}

		private void RefreshShipToolbarIcons()
		{
			m_upImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(0));
			m_leftImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(1));
			m_rightImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(2));
			m_downImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(3));
			MyGuiControlLabel upLabel = m_upLabel;
			MyGuiControlImage upImageTop = m_upImageTop;
			Vector4 vector = (m_upImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(0));
			Vector4 vector4 = (upLabel.ColorMask = (upImageTop.ColorMask = vector));
			MyGuiControlLabel leftLabel = m_leftLabel;
			MyGuiControlImage leftImageTop = m_leftImageTop;
			vector = (m_leftImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(1));
			vector4 = (leftLabel.ColorMask = (leftImageTop.ColorMask = vector));
			MyGuiControlLabel rightLabel = m_rightLabel;
			MyGuiControlImage rightImageTop = m_rightImageTop;
			vector = (m_rightImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(2));
			vector4 = (rightLabel.ColorMask = (rightImageTop.ColorMask = vector));
			MyGuiControlLabel downLabel = m_downLabel;
			MyGuiControlImage downImageTop = m_downImageTop;
			vector = (m_downImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(3));
			vector4 = (downLabel.ColorMask = (downImageTop.ColorMask = vector));
			m_upImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(0));
			m_leftImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(1));
			m_rightImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(2));
			m_downImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(3));
			m_bottomHintLeft.Text = "\ue009+\ue001";
			m_bottomHintRight.Text = "\ue009+\ue003";
			Visuals = MyDPadVisualLayouts.Classic;
			m_upLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(0);
			m_leftLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(1);
			m_rightLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(2);
			m_downLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(3);
			m_upFunc = null;
			m_leftFunc = null;
			m_rightFunc = null;
			m_downFunc = null;
			m_handWeaponDefinition = null;
			m_keepHandWeaponAmmoCount = false;
		}

		private void RefreshEmoteIcons()
		{
			MyEmoteSwitcher component = MySession.Static.GetComponent<MyEmoteSwitcher>();
			if (component != null)
			{
				m_upImage.SetTexture(component.GetIconUp());
				m_leftImage.SetTexture(component.GetIconLeft());
				m_rightImage.SetTexture(component.GetIconRight());
				m_downImage.SetTexture(component.GetIconDown());
				Vector4 iconUpMask = component.GetIconUpMask();
				Vector4 iconLeftMask = component.GetIconLeftMask();
				Vector4 iconRightMask = component.GetIconRightMask();
				Vector4 iconDownMask = component.GetIconDownMask();
				m_upImage.ColorMask = iconUpMask;
				m_leftImage.ColorMask = iconLeftMask;
				m_rightImage.ColorMask = iconRightMask;
				m_downImage.ColorMask = iconDownMask;
				m_upImageTop.SetTexture(component.GetSubIconUp());
				m_leftImageTop.SetTexture(component.GetSubIconLeft());
				m_rightImageTop.SetTexture(component.GetSubIconRight());
				m_downImageTop.SetTexture(component.GetSubIconDown());
				m_upImageTop.ColorMask = iconUpMask;
				m_leftImageTop.ColorMask = iconLeftMask;
				m_rightImageTop.ColorMask = iconRightMask;
				m_downImageTop.ColorMask = iconDownMask;
				m_bottomHintLeft.Text = '\ue001'.ToString();
				m_bottomHintRight.Text = '\ue003'.ToString();
				Visuals = MyDPadVisualLayouts.Classic;
				m_upLabel.Text = string.Empty;
				m_leftLabel.Text = string.Empty;
				m_rightLabel.Text = string.Empty;
				m_downLabel.Text = string.Empty;
				m_upFunc = null;
				m_leftFunc = null;
				m_rightFunc = null;
				m_downFunc = null;
				m_handWeaponDefinition = null;
				m_keepHandWeaponAmmoCount = false;
			}
		}

		private string[] GetNextToolImage(MyToolSwitcher.ToolType type)
		{
			MyDefinitionId? currentOrNextTool = MySession.Static.GetComponent<MyToolSwitcher>().GetCurrentOrNextTool(type);
			if (!currentOrNextTool.HasValue)
			{
				return null;
			}
			if (type != MyToolSwitcher.ToolType.Weapon)
			{
				return MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentOrNextTool.Value).Icons;
			}
			return MyDefinitionManager.Static.GetPhysicalItemDefinition(currentOrNextTool.Value).Icons;
		}

		private MyDefinitionBase GetWeaponDefinition()
		{
			MyDefinitionId? currentOrNextTool = MySession.Static.GetComponent<MyToolSwitcher>().GetCurrentOrNextTool(MyToolSwitcher.ToolType.Weapon);
			if (!currentOrNextTool.HasValue)
			{
				return null;
			}
			return MyDefinitionManager.Static.GetPhysicalItemDefinition(currentOrNextTool.Value);
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			Vector2 positionAbsoluteCenter = GetPositionAbsoluteCenter();
			float num = 0.65f;
			MyGuiControlImage arrows = m_arrows;
			MyGuiControlImageRotatable upBackground = m_upBackground;
			MyGuiControlImageRotatable upMidground = m_upMidground;
			MyGuiControlImageRotatable leftBackground = m_leftBackground;
			MyGuiControlImageRotatable rightBackground = m_rightBackground;
			MyGuiControlImageRotatable downBackground = m_downBackground;
			MyGuiControlImageRotatable upBackgroundColor = m_upBackgroundColor;
			MyGuiControlImageRotatable leftBackgroundColor = m_leftBackgroundColor;
			MyGuiControlImageRotatable rightBackgroundColor = m_rightBackgroundColor;
			MyGuiControlImageRotatable downBackgroundColor = m_downBackgroundColor;
			MyGuiControlImage centerImageInner = m_centerImageInner;
			Vector2 vector2 = (m_centerImageOuter.Position = positionAbsoluteCenter);
			Vector2 vector4 = (centerImageInner.Position = vector2);
			Vector2 vector6 = (downBackgroundColor.Position = vector4);
			Vector2 vector8 = (rightBackgroundColor.Position = vector6);
			Vector2 vector10 = (leftBackgroundColor.Position = vector8);
			Vector2 vector12 = (upBackgroundColor.Position = vector10);
			Vector2 vector14 = (downBackground.Position = vector12);
			Vector2 vector16 = (rightBackground.Position = vector14);
			Vector2 vector18 = (leftBackground.Position = vector16);
			Vector2 vector20 = (upMidground.Position = vector18);
			Vector2 vector23 = (arrows.Position = (upBackground.Position = vector20));
			MyGuiControlImage upImageTop = m_upImageTop;
			vector23 = (m_upImage.Position = positionAbsoluteCenter + new Vector2(0f, -65f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num);
			upImageTop.Position = vector23 + m_subiconOffset;
			MyGuiControlImage leftImageTop = m_leftImageTop;
			vector23 = (m_leftImage.Position = positionAbsoluteCenter + new Vector2(-65f, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num);
			leftImageTop.Position = vector23 + m_subiconOffset;
			MyGuiControlImage rightImageTop = m_rightImageTop;
			vector23 = (m_rightImage.Position = positionAbsoluteCenter + new Vector2(65f, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num);
			rightImageTop.Position = vector23 + m_subiconOffset;
			MyGuiControlImage downImageTop = m_downImageTop;
			vector23 = (m_downImage.Position = positionAbsoluteCenter + new Vector2(0f, 65f) / MyGuiConstants.GUI_OPTIMAL_SIZE * num);
			downImageTop.Position = vector23 + m_subiconOffset;
			float num2 = 80f;
			m_bottomHintLeft.Position = positionAbsoluteCenter + new Vector2(-0.0035f, -0.001f) + new Vector2(0f - num2, num2) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_bottomHintRight.Position = positionAbsoluteCenter + new Vector2(-0.0035f, -0.001f) + new Vector2(num2, num2) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector28 = new Vector2(25f) / MyGuiConstants.GUI_OPTIMAL_SIZE * new Vector2(0.3f, 1.3f);
			m_upLabel.Position = m_upImageTop.Position + vector28;
			m_leftLabel.Position = m_leftImageTop.Position + vector28;
			m_rightLabel.Position = m_rightImageTop.Position + vector28;
			m_downLabel.Position = m_downImageTop.Position + vector28;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (!base.Visible || (m_style.VisibleCondition != null && !m_style.VisibleCondition.Eval()))
			{
				return;
			}
			if (m_upFunc != null)
			{
				string text = m_upFunc();
				if (!string.IsNullOrEmpty(text))
				{
					m_upLabel.Text = text;
				}
			}
			if (m_leftFunc != null)
			{
				string text2 = m_leftFunc();
				if (!string.IsNullOrEmpty(text2))
				{
					m_upLabel.Text = text2;
				}
			}
			if (m_rightFunc != null)
			{
				string text3 = m_rightFunc();
				if (!string.IsNullOrEmpty(text3))
				{
					m_rightLabel.Text = text3;
				}
			}
			if (m_downFunc != null)
			{
				string text4 = m_downFunc();
				if (!string.IsNullOrEmpty(text4) && m_downLabel.Text != text4)
				{
					m_downLabel.Text = text4;
				}
			}
			switch (m_visuals)
			{
			case MyDPadVisualLayouts.Classic:
				foreach (MyGuiControlImageRotatable background in m_backgrounds)
				{
					background.Draw(transitionAlpha * MySandboxGame.Config.HUDBkOpacity, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
				}
				foreach (MyGuiControlImageRotatable midground in m_midgrounds)
				{
					midground.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
				}
				foreach (MyGuiControlImage image in m_images)
				{
					image.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
				}
				break;
			case MyDPadVisualLayouts.ColorPicker:
				foreach (MyGuiControlImageRotatable item in m_backgroundsColor)
				{
					item.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
				}
				foreach (MyGuiControlImage item2 in m_imagesColor)
				{
					item2.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
				}
				break;
			}
			m_arrows.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
			m_bottomHintLeft.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_bottomHintRight.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_upLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_leftLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_rightLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_downLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		private void InitStatConditions(ConditionBase conditionBase)
		{
			ConditionBase[] terms = (conditionBase as Condition).Terms;
			for (int i = 0; i < terms.Length; i++)
			{
				StatCondition statCondition = terms[i] as StatCondition;
				if (statCondition != null)
				{
					IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
					statCondition.SetStat(stat);
				}
			}
		}

		public override void Update()
		{
			MyStringId myStringId = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (myStringId != m_lastContext)
			{
				RefreshIcons();
				m_lastContext = myStringId;
			}
			else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyControlStateType.NEW_RELEASED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyControlStateType.NEW_RELEASED))
			{
				RefreshIcons();
			}
			base.Update();
			foreach (MyBlinkAnimator activeAnimator in m_activeAnimators)
			{
				if (activeAnimator.Duration >= 0)
				{
					activeAnimator.Duration--;
				}
				if (activeAnimator.Duration == 0)
				{
					activeAnimator.Highlighted = !activeAnimator.Highlighted;
					SetBackgroundTexture(ref activeAnimator.Control, activeAnimator.Highlighted);
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_UP))
			{
				Blink(0);
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_DOWN))
			{
				Blink(3);
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_LEFT))
			{
				Blink(1);
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.FAKE_RIGHT))
			{
				Blink(2);
			}
		}

		private void Blink(int idx)
		{
			if (idx >= 0 && idx <= 3 && !m_canBlink[idx])
			{
				m_activeAnimators[idx].Duration = BLINK_DURATION;
				m_activeAnimators[idx].Highlighted = true;
				SetBackgroundTexture(ref m_activeAnimators[idx].Control, isHighlighted: true);
			}
		}

		public void Dispose()
		{
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.OnActivated -= RefreshIcons;
				MyCubeBuilder.Static.OnDeactivated -= RefreshIcons;
				MyCubeBuilder.Static.OnBlockVariantChanged -= RefreshIcons;
				MyCubeBuilder.Static.OnSymmetrySetupModeChanged -= RefreshIcons;
				MyCubeBuilder.Static.OnToolTypeChanged -= RefreshIcons;
			}
			if (MySession.Static != null)
			{
				MySession.Static.OnLocalPlayerSkinOrColorChanged -= RefreshIcons;
				MySession.Static.GetComponent<MyEmoteSwitcher>().OnActiveStateChanged -= RefreshIcons;
				MySession.Static.GetComponent<MyEmoteSwitcher>().OnPageChanged -= RefreshIcons;
			}
			if (MySessionComponentVoxelHand.Static != null)
			{
				MySessionComponentVoxelHand.Static.OnEnabledChanged -= RefreshIcons;
				MySessionComponentVoxelHand.Static.OnBrushChanged -= RefreshIcons;
			}
			if (m_toolSwitcher != null)
			{
				m_toolSwitcher.ToolsRefreshed -= RefreshIcons;
			}
			if (MyGuiScreenGamePlay.Static != null)
			{
				MyGuiScreenGamePlay.Static.OnHelmetChanged -= RefreshIcons;
				MyGuiScreenGamePlay.Static.OnHeadlightChanged -= RefreshIcons;
			}
			MyCockpit.OnPilotAttached -= RefreshIcons;
			MyGuiControlRadialMenuBlock.OnSelectionConfirmed -= RefreshIconsRadialBlock;
		}
	}
}
