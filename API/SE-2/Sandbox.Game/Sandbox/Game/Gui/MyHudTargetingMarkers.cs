using Sandbox.Definitions.GUI;
using Sandbox.Game.GUI;
using Sandbox.Graphics;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudTargetingMarkers
	{
		protected MyObjectBuilder_TargetingMarkersStyle m_style;

		protected MyStatControls m_statControls;

		private Vector2 m_position;

		public MyStatControlTargetingProgressBar OffscreenTargetCircle { get; private set; }

		public MyStatControlTargetingProgressBar TargetingCircle { get; private set; }

		public Vector2 Position => m_position;

		public static Vector2 ScreenCenter => new Vector2(0.5f, MyGuiManager.GetHudSizeHalf().Y);

		public void Draw()
		{
			if (m_statControls != null && m_style != null)
			{
				Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
				new Vector2(fullscreenRectangle.Width, fullscreenRectangle.Height);
				m_statControls.Draw(1f, 1f);
			}
		}

		public void RecreateControls(bool constructor)
		{
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			m_style = hudDefinition.TargetingMarkers;
			if (m_style != null)
			{
				InitStatControls();
			}
		}

		private void InitStatControls()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			Vector2 vector = new Vector2(fullscreenRectangle.Width, fullscreenRectangle.Height);
			float uiScale = MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier;
			m_statControls = new MyStatControls(m_style, uiScale);
			Vector2 coordScreen = m_style.Position * vector;
			m_statControls.Position = (Position - ScreenCenter) / MyGuiManager.GetHudSize() * vector + MyUtils.AlignCoord(coordScreen, vector, m_style.OriginAlign);
			OffscreenTargetCircle = m_statControls.OffscreenTargetCircle;
			TargetingCircle = m_statControls.TargetingCircle;
		}
	}
}
