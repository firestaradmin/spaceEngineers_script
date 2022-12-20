using System.Diagnostics;

namespace VRageRender
{
	[DebuggerDisplay("Name = {Name}, MeshCount = {Meshes.Length}")]
	internal struct MyMeshSectionInfo1
	{
		internal string Name;

		internal MyMeshSectionPartInfo1[] Meshes;
	}
}
