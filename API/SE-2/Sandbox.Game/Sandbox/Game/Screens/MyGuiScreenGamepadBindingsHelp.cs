using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenGamepadBindingsHelp : MyGuiScreenBase
	{
		private ControlScheme m_scheme;

		private MyGuiControlGamepadBindings m_character;

		private MyGuiControlGamepadBindings m_jetpack;

		private MyGuiControlGamepadBindings m_ship;

		public MyGuiScreenGamepadBindingsHelp(ControlScheme scheme)
		{
			base.CanHideOthers = false;
			m_scheme = scheme;
			m_backgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK.Center.Texture;
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 1f);
			base.EnabledBackgroundFade = true;
			base.Size = new Vector2(1.05f, 0.75f);
			RecreateControls(constructor: true);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y, MyControlStateType.NEW_RELEASED))
			{
				CloseScreen();
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_character = new MyGuiControlGamepadBindings(BindingType.Character, m_scheme, isFullWidth: false);
			m_jetpack = new MyGuiControlGamepadBindings(BindingType.Jetpack, m_scheme, isFullWidth: false);
			m_ship = new MyGuiControlGamepadBindings(BindingType.Ship, m_scheme, isFullWidth: false);
			Vector2 size = m_character.Size;
			m_character.Position = new Vector2(-0.5f * size.X, -0.5f * size.Y);
			m_jetpack.Position = new Vector2(0.5f * size.X, -0.5f * size.Y);
			m_ship.Position = new Vector2(0f, 0.5f * size.Y);
			Controls.Add(m_character);
			Controls.Add(m_jetpack);
			Controls.Add(m_ship);
		}

		public override string GetFriendlyName()
		{
			return "GameBindingsHelp";
		}
	}
}
