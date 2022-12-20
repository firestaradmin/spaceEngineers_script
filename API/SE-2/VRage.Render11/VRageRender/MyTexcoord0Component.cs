using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyTexcoord0Component : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("TEXCOORD", "float2 texcoord0", Format.R32G32_Float, component, list, dict, declaration);
			code.Append("__texcoord0 = input.texcoord0;\\\n");
		}
	}
}
