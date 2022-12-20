using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyGenericInstanceComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("TEXCOORD", "float4 matrix_row0", Format.R16G16B16A16_Float, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 matrix_row1", Format.R16G16B16A16_Float, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 matrix_row2", Format.R16G16B16A16_Float, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 colormask", Format.R16G16B16A16_Float, component, list, dict, declaration);
			code.Append("__instance_matrix = construct_matrix_43( input.matrix_row0, input.matrix_row1, input.matrix_row2);\\\n");
			code.Append("__colormask = input.colormask;\\\n");
		}
	}
}
