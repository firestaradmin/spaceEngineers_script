using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MySimpleInstanceComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("TEXCOORD", "float4 matrix_row0", Format.R32G32B32A32_Float, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 matrix_row1", Format.R32G32B32A32_Float, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 matrix_row2", Format.R32G32B32A32_Float, component, list, dict, declaration);
			code.Append("__instance_matrix = construct_matrix_43(input.matrix_row0, input.matrix_row1, input.matrix_row2);\\\n");
		}
	}
}
