using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyCubeInstanceComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone0", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone1", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone2", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone3", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone4", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone5", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone6", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 packed_bone7", Format.R8G8B8A8_UNorm, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 cube_transformation", Format.R32G32B32A32_Float, component, list, dict, declaration);
			MyComponent.AddSingle("TEXCOORD", "float4 colormask", Format.R32G32B32A32_Float, component, list, dict, declaration);
			code.Append("__packed_bone0 = input.packed_bone0;\\\n");
			code.Append("__packed_bone1 = input.packed_bone1;\\\n");
			code.Append("__packed_bone2 = input.packed_bone2;\\\n");
			code.Append("__packed_bone3 = input.packed_bone3;\\\n");
			code.Append("__packed_bone4 = input.packed_bone4;\\\n");
			code.Append("__packed_bone5 = input.packed_bone5;\\\n");
			code.Append("__packed_bone6 = input.packed_bone6;\\\n");
			code.Append("__packed_bone7 = input.packed_bone7;\\\n");
			code.Append("__cube_transformation = input.cube_transformation;\\\n");
			code.Append("__colormask = input.colormask;\\\n");
		}
	}
}
