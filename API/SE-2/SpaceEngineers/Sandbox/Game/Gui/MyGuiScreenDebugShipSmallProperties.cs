using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Small Ship properties")]
	internal class MyGuiScreenDebugShipSmallProperties : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugShipSmallProperties()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("System small ship properties", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("Front light", Color.Yellow.ToVector4(), 1.2f);
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugShipSmallProperties";
		}
	}
}
