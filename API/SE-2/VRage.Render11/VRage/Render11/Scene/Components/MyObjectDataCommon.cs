using System.Runtime.InteropServices;
using VRageMath;

namespace VRage.Render11.Scene.Components
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MyObjectDataCommon
	{
		internal Vector4 m_row0;

		internal Vector4 m_row1;

		internal Vector4 m_row2;

		internal Vector3 KeyColor;

		internal uint LOD;

		internal Vector3 ColorMul;

		internal float Emissive;

		internal Vector2 CustomAlpha;

		internal int DepthBias;

		internal MyMaterialFlags MaterialFlags;

		internal Matrix LocalMatrix
		{
			set
			{
				m_row0 = new Vector4(value.M11, value.M21, value.M31, value.M41);
				m_row1 = new Vector4(value.M12, value.M22, value.M32, value.M42);
				m_row2 = new Vector4(value.M13, value.M23, value.M33, value.M43);
			}
		}

		internal Vector3 LocalMatrixTranslation
		{
			get
			{
				return new Vector3(m_row0.W, m_row1.W, m_row2.W);
			}
			set
			{
				m_row0.W = value.X;
				m_row1.W = value.Y;
				m_row2.W = value.Z;
			}
		}
	}
}
