using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyBlendWeightsComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("BLENDWEIGHT", "float4 blend_weights", Format.R16G16B16A16_Float, component, list, dict, declaration);
			code.Append("__blend_weights = input.blend_weights;\\\n");
		}
	}
}
