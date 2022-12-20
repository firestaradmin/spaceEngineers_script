using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyTangentBitanSgnComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("TANGENT", "float4 tangent4", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			code.Append("__tangent = unpack_tangent_sign(input.tangent4);\\\n");
		}
	}
}
