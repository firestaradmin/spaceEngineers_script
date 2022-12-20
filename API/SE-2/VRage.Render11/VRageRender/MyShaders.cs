namespace VRageRender
{
	internal static class MyShaders
	{
		private struct MyShaderInfo
		{
			internal MyShaderCompilationInfo Info;

			internal byte[] ByteCode;
		}

		private static readonly MyFreelist<MyShaderInfo> m_shaderInfos = new MyFreelist<MyShaderInfo>(512);

		internal static void Init()
		{
		}

		internal static byte[] GetByteCode(ShaderInfoId id)
		{
			return m_shaderInfos.Data[id.Index].ByteCode;
		}

		internal static MyShaderCompilationInfo GetCompilationInfo(ShaderInfoId id)
		{
			return m_shaderInfos.Data[id.Index].Info;
		}

		internal static ShaderInfoId CreateInfo(byte[] byteCode, ref MyShaderCompilationInfo compileInfo)
		{
			ShaderInfoId result;
			lock (m_shaderInfos)
			{
				ShaderInfoId shaderInfoId = default(ShaderInfoId);
				shaderInfoId.Index = m_shaderInfos.Allocate();
				result = shaderInfoId;
			}
			m_shaderInfos.Data[result.Index] = new MyShaderInfo
			{
				ByteCode = byteCode,
				Info = compileInfo
			};
			return result;
		}

		public static void UpdateByteCode(ShaderInfoId id, byte[] byteCode)
		{
			MyShaderInfo myShaderInfo = m_shaderInfos.Data[id.Index];
			myShaderInfo.ByteCode = byteCode;
			m_shaderInfos.Data[id.Index] = myShaderInfo;
		}

		internal static void Recompile()
		{
			MyVertexShaders.Recompile();
			MyPixelShaders.Recompile();
			MyComputeShaders.Recompile();
			MyGeometryShaders.Recompile();
			MyInputLayouts.Recompile();
		}

		internal static void OnDeviceEnd()
		{
			MyVertexShaders.OnDeviceEnd();
			MyPixelShaders.OnDeviceEnd();
			MyComputeShaders.OnDeviceEnd();
			MyGeometryShaders.OnDeviceEnd();
			MyInputLayouts.OnDeviceEnd();
		}

		internal static void OnDeviceReset()
		{
			OnDeviceEnd();
			Recompile();
		}
	}
}
