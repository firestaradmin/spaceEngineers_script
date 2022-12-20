using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyVoxelNormalComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("NORMAL", "uint2 normal", Format.R16G16_UInt, component, list, dict, declaration);
			MyComponent.AddSingle("COLORSHIFT", "int4 colorshift", Format.R8G8B8A8_SInt, component, list, dict, declaration);
			code.Append("__normal = unpack_normal(input.normal);\\\n");
			code.Append("__colormask = unpack_color_shift_hsv(input.colorshift);\\\n");
		}
	}
}
