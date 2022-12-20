using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal struct MyInstanceMaterial
	{
		private Vector3 m_colorMult;

		private float m_emissivity;

		public static MyInstanceMaterial Default = new MyInstanceMaterial
		{
			ColorMult = Vector3.One,
			Emissivity = 1f
		};

		public Vector3 ColorMult
		{
			get
			{
				return m_colorMult;
			}
			set
			{
				m_colorMult = value;
				PackedColorMultEmissivity = new HalfVector4(new Vector4(m_colorMult, m_emissivity));
			}
		}

		public float Emissivity
		{
			get
			{
				return m_emissivity;
			}
			set
			{
				m_emissivity = value;
				PackedColorMultEmissivity = new HalfVector4(new Vector4(m_colorMult, m_emissivity));
			}
		}

		public HalfVector4 PackedColorMultEmissivity { get; private set; }
	}
}
