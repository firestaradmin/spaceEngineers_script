using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal sealed class MyVoxelPositionMaterialComponent : MyComponent
	{
		internal override void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code)
		{
			MyComponent.AddSingle("POSITION", "float3 position", Format.R32G32B32_Float, component, list, dict, declaration);
			MyComponent.AddSingle("MATERIAL", "int4 material", Format.R8G8B8A8_SInt, component, list, dict, declaration);
			code.Append("__position_object = unpack_voxel_position(input.position);\\\n");
			code.Append("__material_weights = unpack_voxel_weights(input.material.w);\\\n");
			code.Append("__triplanar_mat_info = input.material;\\\n");
		}
	}
}
