using VRageMath;

namespace VRage.Render11.GeometryStage2.Instancing
{
	internal struct RowMatrix
	{
		public Vector4 Col0;

		public Vector4 Col1;

		public Vector4 Col2;

		public static RowMatrix Create(ref Matrix m)
		{
			RowMatrix result = default(RowMatrix);
			result.Col0 = new Vector4(m.M11, m.M21, m.M31, m.M41);
			result.Col1 = new Vector4(m.M12, m.M22, m.M32, m.M42);
			result.Col2 = new Vector4(m.M13, m.M23, m.M33, m.M43);
			return result;
		}
	}
}
