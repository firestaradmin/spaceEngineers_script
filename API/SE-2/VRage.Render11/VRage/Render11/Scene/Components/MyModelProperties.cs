using VRageMath;
using VRageRender.Messages;

namespace VRage.Render11.Scene.Components
{
	internal class MyModelProperties
	{
		internal static readonly float DefaultEmissivity = 1f;

		internal static readonly Vector3 DefaultColorMul = Vector3.One;

		internal float Emissivity = DefaultEmissivity;

		internal Vector3 ColorMul = DefaultColorMul;

		internal MyTextureChange? TextureChange;
	}
}
