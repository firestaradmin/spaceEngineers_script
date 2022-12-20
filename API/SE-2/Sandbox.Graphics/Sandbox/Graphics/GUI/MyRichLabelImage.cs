using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyRichLabelImage : MyRichLabelPart
	{
		private string m_texture;

		private Vector2 m_size;

		private Vector4 m_imageColor;

		public string Texture
		{
			get
			{
				return m_texture;
			}
			set
			{
				m_texture = value;
			}
		}

<<<<<<< HEAD
=======
		public new Vector4 Color
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public new Vector2 Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
			}
		}

		public MyRichLabelImage(string texture, Vector2 size, Vector4 color)
		{
			m_texture = texture;
			m_size = size;
			m_imageColor = color;
		}

		public override bool Draw(Vector2 position, float alphamask, ref int charactersLeft)
		{
			Vector4 imageColor = m_imageColor;
			imageColor *= alphamask;
			MyGuiManager.DrawSpriteBatch(m_texture, position, m_size, new Color(imageColor), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			charactersLeft--;
			return true;
		}

		public override bool HandleInput(Vector2 position)
		{
			return false;
		}
	}
}
