using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyBlendIndicesComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("BLENDINDICES", "uint4 blend_indices", Format.R8G8B8A8_UInt, component, list, dict, declaration);
			code.Append("__blend_indices = input.blend_indices;\\\n");
		}
	}
}
