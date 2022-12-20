using SharpDX.Direct3D;
using VRage.Utils;

namespace VRageRender
{
	internal struct MyShaderCompilationInfo
	{
		internal MyStringId File;

		internal MyShaderProfile Profile;

		internal ShaderMacro[] Macros;
	}
}
