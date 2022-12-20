using SharpDX.Direct3D;
using SharpDX.DXGI;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Tools;
using VRageMath;

namespace VRageRender
{
	internal class MyBloom : MyImmediateRC
	{
		private const int MAX_GAUSSIAN_SAMPLES = 11;

		private static MyComputeShaders.Id m_bloomShader;

		private static MyComputeShaders.Id m_downscale2Shader;

		private static MyComputeShaders.Id m_downscale4Shader;

		private static MyComputeShaders.Id[] m_blurH;

		private static MyComputeShaders.Id[] m_blurV;

		private const int m_numthreads = 8;

		private const int BLOOM_TARGET_SIZE_DIVIDER = 4;

		internal static void Init()
		{
			ShaderMacro[] macros = new ShaderMacro[1]
			{
				new ShaderMacro("NUMTHREADS", 8)
			};
			m_bloomShader = MyComputeShaders.Create("Postprocess/Bloom/Init.hlsl", macros);
			m_downscale2Shader = MyComputeShaders.Create("Postprocess/Bloom/Downscale2.hlsl", macros);
			m_downscale4Shader = MyComputeShaders.Create("Postprocess/Bloom/Downscale4.hlsl", macros);
			m_blurH = new MyComputeShaders.Id[11];
			m_blurV = new MyComputeShaders.Id[11];
			for (int i = 0; i < 11; i++)
			{
				ShaderMacro[] macros2 = new ShaderMacro[3]
				{
					new ShaderMacro("HORIZONTAL", null),
					new ShaderMacro("NUMTHREADS", 8),
					new ShaderMacro("NUM_GAUSSIAN_SAMPLES", (i % 2 > 0) ? (i + 1) : i)
				};
				m_blurH[i] = MyComputeShaders.Create("Postprocess/Bloom/Blur.hlsl", macros2);
				macros2 = new ShaderMacro[2]
				{
					new ShaderMacro("NUMTHREADS", 8),
					new ShaderMacro("NUM_GAUSSIAN_SAMPLES", (i % 2 > 0) ? (i + 1) : i)
				};
				m_blurV[i] = MyComputeShaders.Create("Postprocess/Bloom/Blur.hlsl", macros2);
			}
		}

		internal static IConstantBuffer GetCB_blur(MyStereoRegion region, Vector2I uavSize)
		{
			int data = 0;
			int data2 = uavSize.X - 1;
			switch (region)
			{
			case MyStereoRegion.LEFT:
				data2 = uavSize.X / 2 - 1;
				break;
			case MyStereoRegion.RIGHT:
				data = uavSize.X / 2;
				data2 = uavSize.X / 2 - 1;
				break;
			}
			IConstantBuffer objectCB = MyImmediateRC.RC.GetObjectCB(16);
			MyMapping myMapping = MyMapping.MapDiscard(objectCB);
			myMapping.WriteAndPosition(ref data);
			myMapping.WriteAndPosition(ref data2);
			Vector2 data3 = new Vector2(uavSize.X, uavSize.Y);
			myMapping.WriteAndPosition(ref data3);
			myMapping.Unmap();
			return objectCB;
		}

		internal static IConstantBuffer GetCBSize(float width, float height)
		{
			IConstantBuffer objectCB = MyImmediateRC.RC.GetObjectCB(8);
			MyMapping myMapping = MyMapping.MapDiscard(objectCB);
			myMapping.WriteAndPosition(ref width);
			myMapping.WriteAndPosition(ref height);
			myMapping.Unmap();
			return objectCB;
		}

		internal static IBorrowedUavTexture Run(ISrvBindable src, ISrvBindable srcGBuffer2, ISrvBindable srcDepth, ISrvBindable exposure)
		{
			MyImmediateRC.RC.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			MyImmediateRC.RC.ComputeShader.SetSampler(0, MySamplerStateManager.Default);
			int x = MyRender11.ResolutionI.X;
			int y = MyRender11.ResolutionI.Y;
			Format lBufferFormat = MyGBuffer.LBufferFormat;
			MyBorrowedRwTextureManager rwTexturesPool = MyManagers.RwTexturesPool;
			IBorrowedUavTexture borrowedUavTexture = rwTexturesPool.BorrowUav("MyBloom.UavHalfScreen", x / 2, y / 2, lBufferFormat);
			IBorrowedUavTexture borrowedUavTexture2 = rwTexturesPool.BorrowUav("MyBloom.UavBlurScreen", x / 4, y / 4, lBufferFormat);
			IBorrowedUavTexture borrowedUavTexture3 = rwTexturesPool.BorrowUav("MyBloom.UavBlurScreenHelper", x / 4, y / 4, lBufferFormat);
			MyImmediateRC.RC.ComputeShader.SetUav(0, borrowedUavTexture);
			MyImmediateRC.RC.ComputeShader.SetSrv(0, src);
			MyImmediateRC.RC.ComputeShader.SetSrv(1, srcGBuffer2);
			MyImmediateRC.RC.ComputeShader.SetSrv(2, srcDepth);
			MyImmediateRC.RC.ComputeShader.SetSrv(3, exposure);
			MyImmediateRC.RC.ComputeShader.Set(m_bloomShader);
			Vector2I size = borrowedUavTexture.Size;
			Vector2I vector2I = new Vector2I((size.X + 8 - 1) / 8, (size.Y + 8 - 1) / 8);
			MyImmediateRC.RC.Dispatch(vector2I.X, vector2I.Y, 1);
			bool flag = false;
			MyImmediateRC.RC.ComputeShader.Set(m_downscale2Shader);
			size = borrowedUavTexture2.Size;
			vector2I = new Vector2I((size.X + 8 - 1) / 8, (size.Y + 8 - 1) / 8);
			if (!flag)
			{
				MyImmediateRC.RC.ComputeShader.SetConstantBuffer(1, GetCBSize(borrowedUavTexture.Size.X, borrowedUavTexture.Size.Y));
				MyImmediateRC.RC.ComputeShader.SetUav(0, borrowedUavTexture2);
				MyImmediateRC.RC.ComputeShader.SetSrv(0, borrowedUavTexture);
				MyImmediateRC.RC.Dispatch(vector2I.X, vector2I.Y, 1);
			}
			MyImmediateRC.RC.ComputeShader.SetConstantBuffer(1, GetCB_blur(MyStereoRegion.FULLSCREEN, size));
			MyImmediateRC.RC.ComputeShader.Set(m_blurV[MyRender11.Postprocess.BloomSize]);
			MyImmediateRC.RC.ComputeShader.SetUav(0, borrowedUavTexture3);
			MyImmediateRC.RC.ComputeShader.SetSrv(0, borrowedUavTexture2);
			MyImmediateRC.RC.Dispatch(vector2I.X, vector2I.Y, 1);
			MyImmediateRC.RC.ComputeShader.SetSrv(0, null);
			MyImmediateRC.RC.ComputeShader.SetUav(0, null);
			MyImmediateRC.RC.ComputeShader.Set(m_blurH[MyRender11.Postprocess.BloomSize]);
			MyImmediateRC.RC.ComputeShader.SetUav(0, borrowedUavTexture2);
			MyImmediateRC.RC.ComputeShader.SetSrv(0, borrowedUavTexture3);
			int num = 1;
			if (MyStereoRender.Enable)
			{
				vector2I.X /= 2;
				num = 2;
			}
			for (int i = 0; i < num; i++)
			{
				MyStereoRegion region = MyStereoRegion.FULLSCREEN;
				if (MyStereoRender.Enable)
				{
					region = ((i == 0) ? MyStereoRegion.LEFT : MyStereoRegion.RIGHT);
				}
				MyImmediateRC.RC.ComputeShader.SetConstantBuffer(1, GetCB_blur(region, size));
				MyImmediateRC.RC.Dispatch(vector2I.X, vector2I.Y, 1);
			}
			if (MyRender11.Settings.DisplayBloomFilter)
			{
				MyDebugTextureDisplay.Select(borrowedUavTexture);
			}
			else if (MyRender11.Settings.DisplayBloomMin)
			{
				MyDebugTextureDisplay.Select(borrowedUavTexture2);
			}
			MyImmediateRC.RC.ComputeShader.SetUav(0, null);
			MyImmediateRC.RC.ComputeShader.SetSrv(0, null);
			borrowedUavTexture.Release();
			borrowedUavTexture3.Release();
			return borrowedUavTexture2;
		}
	}
}
