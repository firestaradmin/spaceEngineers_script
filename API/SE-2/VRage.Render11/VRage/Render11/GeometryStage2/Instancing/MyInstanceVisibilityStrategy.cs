using VRage.Render11.GeometryStage2.Common;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal struct MyInstanceVisibilityStrategy
	{
		private bool m_visibility;

		private MyVisibilityExtFlags m_visibilityExt;

		private bool m_cachedGbuffer;

		private bool m_cachedDepth;

		private bool m_cachedForward;

		public bool Visibility
		{
			get
			{
				return m_visibility;
			}
			set
			{
				m_visibility = value;
				UpdateCache();
			}
		}

		public MyVisibilityExtFlags VisibilityExt
		{
			get
			{
				return m_visibilityExt;
			}
			private set
			{
				m_visibilityExt = value;
				UpdateCache();
			}
		}

		public bool GBufferVisibility => m_cachedGbuffer;

		public bool DepthVisibility => m_cachedDepth;

		public bool ForwardVisibility => m_cachedForward;

		public void Init(bool isVisible, MyVisibilityExtFlags visibilityExt)
		{
			VisibilityExt = visibilityExt;
			Visibility = isVisible;
		}

		private void UpdateCache()
		{
			if (m_visibility)
			{
				m_cachedGbuffer = (m_visibilityExt & MyVisibilityExtFlags.Gbuffer) == MyVisibilityExtFlags.Gbuffer;
				m_cachedDepth = (m_visibilityExt & MyVisibilityExtFlags.Depth) == MyVisibilityExtFlags.Depth;
				m_cachedForward = (m_visibilityExt & MyVisibilityExtFlags.Forward) == MyVisibilityExtFlags.Forward;
			}
			else
			{
				m_cachedGbuffer = false;
				m_cachedDepth = false;
				m_cachedForward = false;
			}
		}
	}
}
