namespace VRageRender
{
	internal static class MyMaterials1
	{
		internal static readonly MyFreelist<MyMaterialProxy_2> ProxyPool = new MyFreelist<MyMaterialProxy_2>(512);

		internal static MyMaterialProxyId AllocateProxy()
		{
			MyMaterialProxyId result = default(MyMaterialProxyId);
			result.Index = ProxyPool.Allocate();
			return result;
		}

		internal static void FreeProxy(MyMaterialProxyId id)
		{
			ProxyPool.Free(id.Index);
		}

		internal static void Init()
		{
		}

		internal static void OnSessionEnd()
		{
		}

		internal static void OnDeviceEnd()
		{
			ProxyPool.Clear();
		}
	}
}
