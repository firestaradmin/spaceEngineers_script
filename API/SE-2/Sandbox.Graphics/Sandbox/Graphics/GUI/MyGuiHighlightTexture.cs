using VRageMath;
using VRageRender;

namespace Sandbox.Graphics.GUI
{
	/// <summary>
	/// Structure describing texture that consists of normal and highlight
	/// version. Also holds information about size of the texture before it was
	/// scaled to power of 2 and this size in GUI normalized coordinates.
	/// </summary>
	public struct MyGuiHighlightTexture
	{
		private string m_normal;

		private string m_highlight;

		private string m_focus;

		private string m_active;

		private Vector2 m_sizePx;

		public Vector2 ActiveSize;

		public Vector2 FocusSize;

		public Vector2 HighlightSize;

		public Vector2 NormalSize;

		public string Normal
		{
			get
			{
				return m_normal;
			}
			set
			{
				m_normal = value;
				NormalSize = MyRenderProxy.GetTextureSize(m_normal);
			}
		}

		public string Highlight
		{
			get
			{
				return m_highlight;
			}
			set
			{
				m_highlight = value;
				HighlightSize = MyRenderProxy.GetTextureSize(m_highlight);
			}
		}

		public string Focus
		{
			get
			{
				return m_focus;
			}
			set
			{
				m_focus = value;
				FocusSize = MyRenderProxy.GetTextureSize(m_focus);
<<<<<<< HEAD
=======
			}
		}

		public string Active
		{
			get
			{
				return m_active;
			}
			set
			{
				m_active = value;
				ActiveSize = MyRenderProxy.GetTextureSize(m_active);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public string Active
		{
			get
			{
				return m_active;
			}
			set
			{
				m_active = value;
				ActiveSize = MyRenderProxy.GetTextureSize(m_active);
			}
		}

		/// <summary>
		/// Size in pixels before texture was scaled to power of 2. This helps
		/// when we have to compute its correct aspect ratio and ideal
		/// resolution for rendering.
		/// </summary>
		public Vector2 SizePx
		{
			get
			{
				return m_sizePx;
			}
			set
			{
				m_sizePx = value;
				SizeGui = m_sizePx / MyGuiConstants.GUI_OPTIMAL_SIZE;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Size in pixels converted to normalized gui coordinates. Can be used
		/// as size when drawing.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Vector2 SizeGui { get; private set; }
	}
}
