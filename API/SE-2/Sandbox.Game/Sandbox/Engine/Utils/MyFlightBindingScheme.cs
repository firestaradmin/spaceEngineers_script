using Sandbox.Game;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Engine.Utils
{
	internal class MyFlightBindingScheme : IMyControlsBindingScheme
	{
		public void CreateBinding(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
			CreateForBase();
			CreateForCharacter(invertYAxisCharacter, invertYAxisJetpackVehicle);
			CreateForJetpack(invertYAxisCharacter, invertYAxisJetpackVehicle);
			CreateForSpaceship(invertYAxisCharacter, invertYAxisJetpackVehicle);
			CreateForSpectator(invertYAxisCharacter, invertYAxisJetpackVehicle);
		}

		private void CreateForBase()
		{
			MyStringId cX_BASE = MySpaceBindingCreator.CX_BASE;
			MyControllerHelper.AddContext(cX_BASE);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyJoystickButtonsEnum.J05);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyJoystickButtonsEnum.J06);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.INVENTORY, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, pressed: false);
			MyControllerHelper.AddControl(cX_BASE, MyControlsGUI.MAIN_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J08, pressed: false);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FORWARD, MyJoystickAxesEnum.Yneg);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.BACKWARD, MyJoystickAxesEnum.Ypos);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.LOOKAROUND, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, pressed: true);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.ROLL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, pressed: false);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.CAMERA_ZOOM_IN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Yneg, pressed: true);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.CAMERA_ZOOM_OUT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Ypos, pressed: true);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_LS, '\ue009'.ToString());
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_RS, '\ue00a'.ToString());
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM, "\ue005+\ue006+\ue025".ToString());
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.USE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.DAMPING, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.DAMPING_RELATIVE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04, pressed: true);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.HEADLIGHTS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.CAMERA_MODE, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.SYSTEM_RADIAL_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J10, pressed: false);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.CUTSCENE_SKIPPER, MyJoystickButtonsEnum.J07);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.WARNING_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J08, pressed: true);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_UP, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_DOWN, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_LEFT, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(cX_BASE, MyControlsSpace.FAKE_RIGHT, MyJoystickButtonsEnum.JDRight);
		}

		private void CreateForCharacter(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
<<<<<<< HEAD
			MyStringId cX_CHARACTER = MyControllerHelper.CX_CHARACTER;
=======
			MyStringId cX_CHARACTER = MySpaceBindingCreator.CX_CHARACTER;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyControllerHelper.AddContext(cX_CHARACTER, MySpaceBindingCreator.CX_BASE);
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.CROUCH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02, pressed: false);
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.STRAFE_LEFT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xneg, pressed: false);
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.STRAFE_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xpos, pressed: false);
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.ROTATION_LEFT, MyJoystickAxesEnum.RotationXneg);
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.ROTATION_RIGHT, MyJoystickAxesEnum.RotationXpos);
			if (invertYAxisCharacter)
			{
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYneg);
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYpos);
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, pressed: true);
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, pressed: true);
			}
			else
			{
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYneg);
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYpos);
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, pressed: true);
				MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, pressed: true);
			}
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.LOOK_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg, pressed: true);
			MyControllerHelper.AddControl(cX_CHARACTER, MyControlsSpace.LOOK_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos, pressed: true);
			CreateCommonForCharacterAndJetpack(cX_CHARACTER);
		}

		private void CreateCommonForCharacterAndJetpack(MyStringId context)
		{
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.THRUSTS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.HELMET, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(context, MyControlsSpace.COLOR_PICKER, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(context, MyControlsSpace.CONSUME_HEALTH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, pressed: true);
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: true);
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07);
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J07);
			MyControllerHelper.AddControl(context, MyControlsSpace.COLOR_TOOL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(context, MyControlsSpace.TERMINAL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, pressed: true);
			MyControllerHelper.AddControl(context, MyControlsSpace.RELOAD, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, pressed: true);
		}

		private void CreateForJetpack(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
<<<<<<< HEAD
			MyStringId cX_JETPACK = MyControllerHelper.CX_JETPACK;
=======
			MyStringId cX_JETPACK = MySpaceBindingCreator.CX_JETPACK;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyControllerHelper.AddContext(cX_JETPACK, MySpaceBindingCreator.CX_BASE);
			CreateCommonForCharacterAndJetpack(cX_JETPACK);
			CreateCommonForJetpackAndShip(cX_JETPACK, invertYAxisCharacter, invertYAxisJetpackVehicle);
		}

		private void CreateForSpaceship(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
<<<<<<< HEAD
			MyStringId cX_SPACESHIP = MyControllerHelper.CX_SPACESHIP;
=======
			MyStringId cX_SPACESHIP = MySpaceBindingCreator.CX_SPACESHIP;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyControllerHelper.AddContext(cX_SPACESHIP, MySpaceBindingCreator.CX_BASE);
			CreateCommonForJetpackAndShip(cX_SPACESHIP, invertYAxisCharacter, invertYAxisJetpackVehicle);
			MyControllerHelper.AddControl(cX_SPACESHIP, MyControlsSpace.LANDING_GEAR, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, pressed: false);
			MyControllerHelper.AddControl(cX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
<<<<<<< HEAD
			MyControllerHelper.AddControl(cX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS_ALL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDLeft);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyControllerHelper.AddControl(cX_SPACESHIP, MyControlsSpace.WHEEL_JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03);
			MyControllerHelper.AddControl(cX_SPACESHIP, MyControlsSpace.TERMINAL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, pressed: true);
			MyControllerHelper.AddControl(cX_SPACESHIP, MyControlsSpace.FAKE_LS, "\ue005+\ue006+\ue009");
			MyControllerHelper.AddControl(cX_SPACESHIP, MyControlsSpace.FAKE_RS, "\ue005+\ue006+\ue00a");
		}

		private void CreateCommonForJetpackAndShip(MyStringId context, bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
			MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXneg);
			MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXpos);
			if (invertYAxisJetpackVehicle)
			{
				MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYneg, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYpos, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, pressed: true);
				MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, pressed: true);
			}
			else
			{
				MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYneg, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYpos, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, pressed: true);
				MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, pressed: true);
			}
			MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_LEFT, MyJoystickAxesEnum.RotationXneg, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_RIGHT, MyJoystickAxesEnum.RotationXpos, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(context, MyControlsSpace.STRAFE_LEFT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xneg, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.STRAFE_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xpos, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.CROUCH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J02, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg, pressed: true);
			MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos, pressed: true);
		}

		private static void CreateForSpectator(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
		{
			MyStringId cX_SPECTATOR = MySpaceBindingCreator.CX_SPECTATOR;
			MyControllerHelper.AddContext(cX_SPECTATOR, MySpaceBindingCreator.CX_BASE);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_FOCUS_PLAYER, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_PLAYER_CONTROL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, pressed: false);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_LOCK, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_TELEPORT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, pressed: false);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_SPEED_BOOST, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J10);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_SPEED_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_SPEED_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.COLOR_PICKER, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.COLOR_TOOL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.FAKE_LS, '\ue009'.ToString());
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.FAKE_RS, '\ue00a'.ToString());
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.FAKE_LS_PRESS, '\ue00b'.ToString());
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.FAKE_RS_PRESS, '\ue00c'.ToString());
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.FAKE_LB_RB_LS, "\ue005+\ue006+" + '\ue009');
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.FAKE_LB_RB_RS, "\ue005+\ue006+" + '\ue00a');
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.FAKE_LB_ROTATION_H, "\ue005+" + '\ue024');
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROLL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXneg);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROLL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXpos);
			if (invertYAxisJetpackVehicle)
			{
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROTATION_DOWN, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYneg, pressed: false, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROTATION_UP, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYpos, pressed: false, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, pressed: true);
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, pressed: true);
			}
			else
			{
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROTATION_UP, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYneg, pressed: false, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROTATION_DOWN, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYpos, pressed: false, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, pressed: true);
				MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, pressed: true);
			}
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROTATION_LEFT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXneg, pressed: false, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.ROTATION_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXpos, pressed: false, () => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.STRAFE_LEFT, MyJoystickAxesEnum.Xneg);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.STRAFE_RIGHT, MyJoystickAxesEnum.Xpos);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.CROUCH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J02, pressed: false);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.LOOK_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg, pressed: true);
			MyControllerHelper.AddControl(cX_SPECTATOR, MyControlsSpace.LOOK_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos, pressed: true);
		}
	}
}
