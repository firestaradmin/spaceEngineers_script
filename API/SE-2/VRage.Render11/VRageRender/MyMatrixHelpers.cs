using VRageMath;

namespace VRageRender
{
	public static class MyMatrixHelpers
	{
		public static Matrix ClipspaceToTexture => new Matrix(0.5f, 0f, 0f, 0f, 0f, -0.5f, 0f, 0f, 0f, 0f, 1f, 0f, 0.5f, 0.5f, 0f, 1f);
	}
}
