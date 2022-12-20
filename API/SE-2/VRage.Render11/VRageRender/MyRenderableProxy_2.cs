namespace VRageRender
{
	/// <summary>
	/// Renderable proxies for merge-instancing
	/// </summary>
	internal struct MyRenderableProxy_2
	{
		internal MyMaterialType MaterialType;

		internal MyRenderableProxyFlags RenderFlags;

		internal MyConstantsPack ObjectConstants;

		internal MySrvTable ObjectSrvs;

		internal int InstanceCount;

		internal int StartInstance;

		internal MyMergeInstancingShaderBundle DepthShaders;

		internal MyMergeInstancingShaderBundle Shaders;

		internal MyMergeInstancingShaderBundle HighlightShaders;

		internal MyMergeInstancingShaderBundle ForwardShaders;

		internal MyDrawSubmesh_2[] SubmeshesDepthOnly;

		internal MyDrawSubmesh_2[] Submeshes;

		internal MyDrawSubmesh_2[][] SectionSubmeshes;

		internal static readonly MyRenderableProxy_2[] EmptyList = new MyRenderableProxy_2[0];

		internal static readonly ulong[] EmptyKeyList = new ulong[0];
	}
}
