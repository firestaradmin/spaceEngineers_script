using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyCustomUnorm4_0Component : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("TEXCOORD", "float4 custom_unorm4_0", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
		}
	}
}
