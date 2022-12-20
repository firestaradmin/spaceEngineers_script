using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyPosition4Component : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("POSITION", "float4 position", Format.R32G32B32A32_Float, component, list, dict, declaration);
			code.Append("__position_object = input.position;\\\n");
		}
	}
}
