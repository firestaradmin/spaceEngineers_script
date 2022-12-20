using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
	public static class MyInputExtensions
	{
		public const float MOUSE_ROTATION_INDICATOR_MULTIPLIER = 0.075f;

		public const float ROTATION_INDICATOR_MULTIPLIER = 0.15f;

		public static float GetRoll(this IMyInput self)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			return MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_RIGHT) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_LEFT);
		}

		public static float GetDeveloperRoll(this IMyInput self)
		{
			float num = 0f;
			bool flag = false;
			if (self.IsGameControlPressed(MyControlsSpace.ROLL_LEFT))
			{
				num += -1f;
				flag = true;
			}
			if (self.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT))
			{
				num += 1f;
				flag = true;
			}
			if (!flag)
			{
				MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
				num += MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_RIGHT) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_LEFT);
			}
			return num;
		}

		public static Vector3 GetPositionDelta(this IMyInput self)
		{
			Vector3 result = Vector3.Zero;
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			MyStringId myStringId = MySpaceBindingCreator.CX_SPECTATOR;
			if (!MySession.Static.IsCameraUserControlledSpectator())
			{
				myStringId = controlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			}
<<<<<<< HEAD
			if (MyInput.Static.IsJoystickLastUsed && myStringId == MyControllerHelper.CX_SPACESHIP)
=======
			if (MyInput.Static.IsJoystickLastUsed && myStringId == MySpaceBindingCreator.CX_SPACESHIP)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				result = GetShipMoveIndicator_Gamepad(myStringId);
			}
			else
			{
				result.X = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.STRAFE_RIGHT) - MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.STRAFE_LEFT);
				result.Y = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.JUMP) - MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.CROUCH);
				result.Z = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.BACKWARD) - MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.FORWARD);
			}
			return result;
		}

		private static bool IsControlledEntityCar()
		{
			MyPlayer myPlayer = Sandbox.Game.Multiplayer.Sync.Clients?.LocalClient?.FirstPlayer;
			if (myPlayer == null)
			{
				return false;
			}
			IMyControllableEntity controlledEntity = myPlayer.Controller.ControlledEntity;
			if (controlledEntity == null)
			{
				return false;
			}
<<<<<<< HEAD
			if (controlledEntity.ControlContext.Id != MyControllerHelper.CX_SPACESHIP.Id)
			{
				return false;
			}
			MyShipController myShipController;
			if ((myShipController = controlledEntity as MyShipController) == null)
=======
			if (controlledEntity.ControlContext.Id != MySpaceBindingCreator.CX_SPACESHIP.Id)
			{
				return false;
			}
			MyShipController myShipController = (MyShipController)controlledEntity;
			if (myShipController == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return false;
			}
			if (myShipController.GridWheels == null || myShipController.GridWheels.WheelCount <= 0)
			{
				return false;
			}
			if (myShipController.GetTotalGravity() == Vector3D.Zero)
			{
				return false;
			}
			return true;
		}

		private static Vector3 GetShipMoveIndicator_Gamepad(MyStringId cx)
		{
			Vector3 zero = Vector3.Zero;
			zero.X = MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.STRAFE_RIGHT, gamepadShipControl: true) - MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.STRAFE_LEFT, gamepadShipControl: true);
			zero.Y = MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.JUMP) - MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.CROUCH);
			zero.Z = MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.BACKWARD) - MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.FORWARD);
			return zero;
		}

		public static Vector2 GetRotation(this IMyInput self)
		{
			Vector2 result = Vector2.Zero;
			result = new Vector2(self.GetMouseYForGamePlayF(), self.GetMouseXForGamePlayF()) * 0.075f;
<<<<<<< HEAD
			MyStringId myStringId = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			bool flag = false;
			if ((MyInput.Static.GetJoystickYInversionCharacter() && myStringId == MyControllerHelper.CX_CHARACTER) || (MyInput.Static.GetJoystickYInversionVehicle() && (myStringId == MyControllerHelper.CX_JETPACK || myStringId == MyControllerHelper.CX_SPACESHIP)))
			{
				flag = true;
			}
			float num = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.ROTATION_UP);
			float num2 = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.ROTATION_DOWN);
			if (flag)
			{
				result.X += num;
				result.X -= num2;
			}
			else
			{
				result.X -= num;
				result.X += num2;
			}
			result.Y -= MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.ROTATION_LEFT);
			result.Y += MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.ROTATION_RIGHT);
=======
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			result.X -= MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_UP);
			result.X += MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_DOWN);
			result.Y -= MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_LEFT);
			result.Y += MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_RIGHT);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			result *= 9f;
			return result;
		}

		public static Vector2 GetLookAroundRotation(this IMyInput self)
		{
			Vector2 result = Vector2.Zero;
			result = new Vector2(self.GetMouseYForGamePlayF(), self.GetMouseXForGamePlayF()) * 0.075f;
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			bool flag = false;
			if (controlledEntity != null && controlledEntity.Entity != null && controlledEntity.Entity is IMyShipController && !((IMyShipController)controlledEntity.Entity).CanControlShip)
			{
				flag = true;
			}
<<<<<<< HEAD
			bool flag2 = false;
			MyStringId myStringId;
			if (controlledEntity != null)
			{
				myStringId = controlledEntity.ControlContext;
				if ((MyInput.Static.GetJoystickYInversionCharacter() && myStringId == MyControllerHelper.CX_CHARACTER) || (MyInput.Static.GetJoystickYInversionVehicle() && (myStringId == MyControllerHelper.CX_JETPACK || myStringId == MyControllerHelper.CX_SPACESHIP)))
				{
					flag2 = true;
				}
			}
			else
			{
				myStringId = MySpaceBindingCreator.CX_BASE;
			}
			float num = MyControllerHelper.IsControlAnalog(myStringId, flag ? MyControlsSpace.ROTATION_UP : MyControlsSpace.LOOK_UP);
			float num2 = MyControllerHelper.IsControlAnalog(myStringId, flag ? MyControlsSpace.ROTATION_DOWN : MyControlsSpace.LOOK_DOWN);
			if (flag2)
			{
				result.X += num;
				result.X -= num2;
			}
			else
			{
				result.X -= num;
				result.X += num2;
			}
			result.Y -= MyControllerHelper.IsControlAnalog(myStringId, flag ? MyControlsSpace.ROTATION_LEFT : MyControlsSpace.LOOK_LEFT);
			result.Y += MyControllerHelper.IsControlAnalog(myStringId, flag ? MyControlsSpace.ROTATION_RIGHT : MyControlsSpace.LOOK_RIGHT);
=======
			MyStringId context = controlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			result.X -= MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_UP : MyControlsSpace.LOOK_UP);
			result.X += MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_DOWN : MyControlsSpace.LOOK_DOWN);
			result.Y -= MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_LEFT : MyControlsSpace.LOOK_LEFT);
			result.Y += MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_RIGHT : MyControlsSpace.LOOK_RIGHT);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			result *= 9f;
			return result;
		}

		public static Vector2 GetCursorPositionDelta(this IMyInput self)
		{
			Vector2 one = Vector2.One;
			return new Vector2(self.GetMouseX(), self.GetMouseY()) * one;
		}

		public static float GetRoll(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetRoll();
		}

		public static Vector3 GetPositionDelta(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetPositionDelta();
		}

		public static Vector2 GetRotation(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetRotation();
		}

		public static Vector2 GetCursorPositionDelta(this VRage.ModAPI.IMyInput self)
		{
			return ((IMyInput)self).GetCursorPositionDelta();
		}
	}
}
