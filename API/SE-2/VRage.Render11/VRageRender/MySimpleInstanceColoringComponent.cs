using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MySimpleInstanceColoringComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("TEXCOORD", "float4 instance_keyColorDithering", Format.R16G16B16A16_Float, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 instance_colorMultEmissivity", Format.R16G16B16A16_Float, component, list, dict, declaration);
			code.Append("__instance_keyColor = input.instance_keyColorDithering.xyz;\\\n");
			code.Append("__instance_dithering = input.instance_keyColorDithering.w;\\\n");
			code.Append("__instance_colorMult = input.instance_colorMultEmissivity.xyz;\\\n");
			code.Append("__instance_emissivity = input.instance_colorMultEmissivity.w;\\\n");
		}
	}
}
