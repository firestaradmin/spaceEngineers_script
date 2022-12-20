using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace VRageRender
{
	internal struct MyVertexLayoutInfo
	{
		internal MyVertexInputComponent[] Components;

		internal InputElement[] Elements;

		internal ShaderMacro[] Macros;

		internal bool HasBonesInfo;
	}
}
