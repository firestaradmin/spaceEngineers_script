namespace VRageRender
{
	internal struct MyPassLocals
	{
		internal int BoundMeshIndex;

		internal MyMaterialProxyId MatTexturesID;

		internal bool BindConstantBuffersBatched;

		internal void Clear()
		{
			BindConstantBuffersBatched = false;
			MatTexturesID = MyMaterialProxyId.NULL;
			BoundMeshIndex = MyMeshMaterialId.NULL.Index;
		}
	}
}
