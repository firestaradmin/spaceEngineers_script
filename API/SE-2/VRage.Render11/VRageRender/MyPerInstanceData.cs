using VRageMath;

namespace VRageRender
{
	internal struct MyPerInstanceData
	{
		internal Vector4 Row0;

		internal Vector4 Row1;

		internal Vector4 Row2;

		internal int DepthBias;

		internal Vector3 __padding;

		internal static MyPerInstanceData FromWorldMatrix(ref MatrixD matrix, int depthBias)
		{
			Matrix matrix2 = matrix;
			MyPerInstanceData result = default(MyPerInstanceData);
			result.Row0 = new Vector4(matrix2.M11, matrix2.M21, matrix2.M31, matrix2.M41);
			result.Row1 = new Vector4(matrix2.M12, matrix2.M22, matrix2.M32, matrix2.M42);
			result.Row2 = new Vector4(matrix2.M13, matrix2.M23, matrix2.M33, matrix2.M43);
			result.DepthBias = depthBias;
			return result;
		}
	}
}
