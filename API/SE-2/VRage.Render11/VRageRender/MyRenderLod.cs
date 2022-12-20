namespace VRageRender
{
	[PooledObject(8)]
	internal class MyRenderLod : IPooledObject
	{
		internal MyRenderableProxy[] RenderableProxies;

		internal ulong[] SortingKeys;

		internal VertexLayoutId VertexLayout1;

		internal MyShaderUnifiedFlags VertexShaderFlags;

		internal float Distance;

		internal void AllocateProxies(int allocationSize)
		{
			if (RenderableProxies == null || allocationSize != RenderableProxies.Length)
			{
				DeallocateProxies();
				RenderableProxies = new MyRenderableProxy[allocationSize];
				SortingKeys = new ulong[allocationSize];
			}
			else if (RenderableProxies != null)
			{
				for (int i = 0; i < RenderableProxies.Length; i++)
				{
					MyObjectPoolManager.Deallocate(RenderableProxies[i]);
				}
			}
			for (int j = 0; j < allocationSize; j++)
			{
				RenderableProxies[j] = MyObjectPoolManager.Allocate<MyRenderableProxy>();
			}
		}

		private void DeallocateProxies()
		{
			if (RenderableProxies != null)
			{
				MyRenderableProxy[] renderableProxies = RenderableProxies;
				for (int i = 0; i < renderableProxies.Length; i++)
				{
					MyObjectPoolManager.Deallocate(renderableProxies[i]);
				}
				RenderableProxies = null;
			}
		}

		private void Clear()
		{
			DeallocateProxies();
			SortingKeys = null;
			VertexLayout1 = VertexLayoutId.NULL;
			VertexShaderFlags = MyShaderUnifiedFlags.NONE;
			Distance = 0f;
		}

		/// <inheritdoc />
		void IPooledObject.Cleanup()
		{
			Clear();
		}
	}
}
