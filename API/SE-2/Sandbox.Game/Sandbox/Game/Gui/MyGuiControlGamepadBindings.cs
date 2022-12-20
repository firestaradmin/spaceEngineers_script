using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiControlGamepadBindings : MyGuiControlParent
	{
		private const float VIEWPORT_H = 762f;

		private const float VIEWPORT_W = 1320f;

		private const float CONTROL_LEFT_ALIGN = 0.340151519f;

		private const float CONTROL_RIGHT_ALIGN = 0.659848452f;

		private const int LEFT_CONTROLS = 8;

		private readonly float[] CONTROL_VERTICAL_ALIGNS = new float[19]
		{
			120f, 225f, 330f, 434f, 538f, 510f, 560f, 642f, 120f, 187f,
			245f, 307f, 372f, 437f, 502f, 565f, 632f, 630f, 680f
		};

		private readonly char[] ICONS = new char[19]
		{
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ZPOS", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J05", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J07", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J09", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_MOTION", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_MOTION_X", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_MOTION_Y", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ZNEG", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J06", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J08", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J04", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J02", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J01", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J03", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J10", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION_X", null)[0],
			MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION_Y", null)[0]
		};

		private const string GAMEPAD_IMAGE = "Textures\\GUI\\HelpScreen\\ControllerSchema.png";

		private const string GAMEPAD_IMAGE_BACKGROUND = "Textures\\GUI\\Screens\\image_background.dds";

		private BindingType m_currentType;

		private ControlScheme m_currentScheme;

		public MyGuiControlGamepadBindings(BindingType type, ControlScheme scheme, bool isFullWidth = true)
		{
			BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
			base.ColorMask = new Vector4(1f, 0.3f, 0.3f, 1f);
			base.Size = new Vector2(isFullWidth ? 0.553f : 0.5f, 0.35f);
			m_currentType = type;
			m_currentScheme = scheme;
			Recreate();
		}

		private void Recreate()
		{
			base.Controls.Clear();
			switch (m_currentScheme)
			{
			case ControlScheme.Default:
				switch (m_currentType)
				{
				case BindingType.Character:
					RecreateCharacter();
					break;
				case BindingType.Jetpack:
					RecreateJetpack();
					break;
				case BindingType.Ship:
					RecreateShip();
					break;
				}
				break;
			case ControlScheme.Alternative:
				switch (m_currentType)
				{
				case BindingType.Character:
					RecreateAltCharacter();
					break;
				case BindingType.Jetpack:
					RecreateAltJetpack();
					break;
				case BindingType.Ship:
					RecreateAltShip();
					break;
				}
				break;
			}
		}

		private void RecreateCharacter()
		{
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerCharacterControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOn, MySpaceTexts.ControlName_Crouch, MySpaceTexts.ControlName_Jump, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));
		}

		private void RecreateJetpack()
		{
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerJetpackControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOff, MySpaceTexts.ControlName_Down, MySpaceTexts.ControlName_Up, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));
		}

		private void RecreateShip()
		{
<<<<<<< HEAD
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerShipControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerShipActions, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.BlockActionTitle_Park, MySpaceTexts.HelpScreen_ControllerFlyDown, MySpaceTexts.HelpScreen_ControllerFlyUp, MySpaceTexts.HelpScreen_ControllerLeaveControl, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));
=======
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerShipControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerShipActions, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.BlockActionTitle_Lock, MySpaceTexts.HelpScreen_ControllerFlyDown, MySpaceTexts.HelpScreen_ControllerFlyUp, MySpaceTexts.HelpScreen_ControllerLeaveControl, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void RecreateAltCharacter()
		{
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerCharacterControl, false, false, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOn, MySpaceTexts.ControlName_Crouch, MySpaceTexts.ControlName_Jump, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty));
		}

		private void RecreateAltJetpack()
		{
<<<<<<< HEAD
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerJetpackControl, true, true, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Yaw, MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOff, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Roll, MySpaceTexts.HelpScreen_ControllerRotation_Pitch));
=======
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerJetpackControl, true, true, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward, MySpaceTexts.HelpScreen_ControllerRotation_Yaw, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.ControlName_JetpackOff, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.HelpScreen_ControllerSystemMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Pitch, MySpaceTexts.HelpScreen_ControllerRotation_Roll));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void RecreateAltShip()
		{
<<<<<<< HEAD
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerShipControl, true, true, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Yaw, MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward, MySpaceTexts.HelpScreen_ControllerShipActions, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.BlockActionTitle_Park, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerLeaveControl, MySpaceTexts.HelpScreen_ControllerSystemMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Roll, MySpaceTexts.HelpScreen_ControllerRotation_Pitch));
=======
			base.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerShipControl, true, true, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward, MySpaceTexts.HelpScreen_ControllerRotation_Yaw, MySpaceTexts.HelpScreen_ControllerShipActions, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.BlockActionTitle_Lock, MyStringId.NullOrEmpty, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerLeaveControl, MySpaceTexts.HelpScreen_ControllerSystemMenu, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerRotation_Pitch, MySpaceTexts.HelpScreen_ControllerRotation_Roll));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private MyGuiControlParent AddImagePanel(string imagePath)
		{
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			myGuiControlParent.Size = base.Size;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Size = myGuiControlParent.Size,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				Position = Vector2.Zero,
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage
			{
				Size = myGuiControlImage.Size,
				OriginAlign = myGuiControlImage.OriginAlign,
				Position = myGuiControlImage.Position,
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage2.SetTexture(imagePath);
			myGuiControlParent.Controls.Add(myGuiControlImage);
			myGuiControlParent.Controls.Add(myGuiControlImage2);
			return myGuiControlParent;
		}

		private MyGuiControlParent AddControllerSchema(MyStringId title, bool splitLS, bool splitRS, params MyStringId[] controls)
		{
			MyGuiControlParent panel = AddImagePanel("Textures\\GUI\\HelpScreen\\ControllerSchema.png");
			for (int i = 0; i < CONTROL_VERTICAL_ALIGNS.Length; i++)
			{
				bool num = i < 8;
				float y2 = CONTROL_VERTICAL_ALIGNS[i] / 762f;
				string text = ((!(controls[i] == MyStringId.NullOrEmpty)) ? MyTexts.GetString(controls[i]) : "—");
<<<<<<< HEAD
				float x2;
=======
				float num2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyGuiDrawAlignEnum align2;
				if (num)
				{
					text = $"{text}  {ICONS[i]}";
<<<<<<< HEAD
					x2 = 0.340151519f;
=======
					num2 = 0.340151519f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					align2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				}
				else
				{
					text = $"{ICONS[i]}  {text}";
<<<<<<< HEAD
					x2 = 0.659848452f;
=======
					num2 = 0.659848452f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					align2 = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				}
				if ((!splitLS || i != 4) && (splitLS || (i != 5 && i != 6)) && (!splitRS || i != 16) && (splitRS || (i != 17 && i != 18)))
				{
<<<<<<< HEAD
					Add(x2, y2, align2, new MyGuiControlLabel(null, null, text, null, 0.9f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: true, 0.18f, isAutoScaleEnabled: true));
=======
					Add(num2, y2, align2, new MyGuiControlLabel(null, null, text, null, 0.9f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: true, 0.18f, isAutoScaleEnabled: true));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			Add(0.5f, 0.05f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, new MyGuiControlLabel
			{
				TextEnum = title,
				TextScale = 1.2f
			});
			return panel;
			void Add(float x, float y, MyGuiDrawAlignEnum align, MyGuiControlBase control)
			{
				control.OriginAlign = align;
				control.Position = panel.Size * new Vector2(x * 2f - 1f, y * 2f - 1f) * new Vector2(0.5f);
				panel.Controls.Add(control);
			}
		}
	}
}
