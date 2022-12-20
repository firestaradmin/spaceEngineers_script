using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlInfoProgressBar : MyGuiControlParent
	{
		protected MyGuiControlImage m_Icon;

		protected MyGuiControlLabel m_LeftLabel;

		protected MyGuiControlLabel m_RightLabel;

		protected MyGuiControlImage m_BarInnerLine;

		protected MyGuiControlImage m_BarBackground;

		protected Vector2 m_barSize;

		public MyGuiControlInfoProgressBar(float width, Vector2? position = null, string icon = null, string textLeft = null)
			: base(position)
		{
			m_barSize = new Vector2(width, 0.007f);
			base.Size = new Vector2(m_barSize.X, 0.039f);
			Vector2 vector = -base.Size / 2f;
			m_LeftLabel = new MyGuiControlLabel
			{
				Position = vector + new Vector2(0.03f, 0.002f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = textLeft
			};
			m_RightLabel = new MyGuiControlLabel
			{
				Position = vector + new Vector2(base.Size.X - 0.005f, 0.002f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			if (icon != null)
			{
				m_Icon = new MyGuiControlImage(vector, new Vector2(0.022f, 0.029f), null, null, new string[1] { icon });
				m_Icon.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				base.Controls.Add(m_Icon);
			}
			else
			{
				m_LeftLabel.PositionX = vector.X;
			}
			m_BarBackground = new MyGuiControlImage(vector + new Vector2(0f, base.Size.Y), m_barSize, null, null, new string[1] { "Textures\\GUI\\Icons\\HUD 2017\\DrillBarBackground.png" });
			m_BarBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			base.Controls.Add(m_BarBackground);
			m_BarInnerLine = new MyGuiControlImage(vector + new Vector2(0f, base.Size.Y), m_barSize, null, null, new string[1] { "Textures\\GUI\\Icons\\HUD 2017\\DrillBarProgress.png" });
			m_BarInnerLine.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			base.Controls.Add(m_BarInnerLine);
			base.Controls.Add(m_LeftLabel);
			base.Controls.Add(m_RightLabel);
		}
	}
}
