using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyPosition3Component : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("POSITION", "float3 position", Format.R32G32B32_Float, component, list, dict, declaration);
			code.Append("__position_object = float4(input.position, 1);\\\n");
		}
	}
}
