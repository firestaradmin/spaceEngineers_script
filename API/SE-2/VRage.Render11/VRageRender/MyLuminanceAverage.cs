using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyLuminanceAverage : MyImmediateRC
	{
		private static MyComputeShaders.Id m_initialShader;

		private static MyComputeShaders.Id m_sumShader;

		private static MyComputeShaders.Id m_finalShader;

		private static MyComputeShaders.Id m_skipShader;

		private static IUavTexture m_prevLum;

		private const int NUM_THREADS = 8;

		internal static void Init()
		{
			ShaderMacro[] macros = new ShaderMacro[1]
			{
				new ShaderMacro("NUMTHREADS", 8)
			};
			m_initialShader = MyComputeShaders.Create("Postprocess/LuminanceReduction/Init.hlsl", macros);
			m_sumShader = MyComputeShaders.Create("Postprocess/LuminanceReduction/Sum.hlsl", macros);
			macros = new ShaderMacro[2]
			{
				new ShaderMacro("NUMTHREADS", 8),
				new ShaderMacro("_FINAL", null)
			};
			m_finalShader = MyComputeShaders.Create("Postprocess/LuminanceReduction/Sum.hlsl", macros);
			m_skipShader = MyComputeShaders.Create("Postprocess/LuminanceReduction/Skip.hlsl");
			m_prevLum = MyManagers.RwTextures.CreateUav("MyLuminanceAverage.PrevLum", 1, 1, Format.R32G32_Float);
		}

		internal static void Reset()
		{
			MyRender11.RC.ClearUav(m_prevLum, default(RawInt4));
		}

		internal static IBorrowedUavTexture Skip()
		{
			IBorrowedUavTexture borrowedUavTexture = MyManagers.RwTexturesPool.BorrowUav("MyLuminanceAverage.Skip", Format.R32G32_Float);
			MyImmediateRC.RC.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			MyImmediateRC.RC.ComputeShader.SetUav(0, borrowedUavTexture);
			MyImmediateRC.RC.ComputeShader.Set(m_skipShader);
			MyImmediateRC.RC.Dispatch(1, 1, 1);
			MyImmediateRC.RC.ComputeShader.SetUav(0, null);
			return borrowedUavTexture;
		}

		internal static IBorrowedUavTexture Run(ISrvBindable src)
		{
			IBorrowedUavTexture borrowedUavTexture = MyManagers.RwTexturesPool.BorrowUav("MyLuminanceAverage.Uav0", Format.R32G32_Float);
			IBorrowedUavTexture borrowedUavTexture2 = MyManagers.RwTexturesPool.BorrowUav("MyLuminanceAverage.Uav1", Format.R32G32_Float);
			Vector2I size = src.Size;
			int data = size.X * size.Y;
			uint data2 = (uint)size.X;
			uint data3 = (uint)size.Y;
			MyMapping myMapping = MyMapping.MapDiscard(MyImmediateRC.RC.GetObjectCB(16));
			myMapping.WriteAndPosition(ref data2);
			myMapping.WriteAndPosition(ref data3);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			MyImmediateRC.RC.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			MyImmediateRC.RC.ComputeShader.SetConstantBuffer(1, MyImmediateRC.RC.GetObjectCB(16));
			MyImmediateRC.RC.ComputeShader.Set(m_initialShader);
			IBorrowedUavTexture uavBindable = borrowedUavTexture;
			MyImmediateRC.RC.ComputeShader.SetUav(0, uavBindable);
			MyImmediateRC.RC.ComputeShader.SetSrv(0, src);
			int num = ComputeGroupCount(size.X);
			int num2 = ComputeGroupCount(size.Y);
			MyImmediateRC.RC.Dispatch(num, num2, 1);
			MyImmediateRC.RC.ComputeShader.Set(m_sumShader);
			int num3 = 0;
			IBorrowedUavTexture srvBind;
			while (true)
			{
				size.X = num;
				size.Y = num2;
				if (size.X <= 8 && size.Y <= 8)
				{
					break;
				}
				uavBindable = ((num3 % 2 == 0) ? borrowedUavTexture2 : borrowedUavTexture);
				srvBind = ((num3 % 2 == 0) ? borrowedUavTexture : borrowedUavTexture2);
				MyImmediateRC.RC.ComputeShader.SetSrv(0, null);
				MyImmediateRC.RC.ComputeShader.SetUav(0, uavBindable);
				MyImmediateRC.RC.ComputeShader.SetSrv(0, srvBind);
				num = ComputeGroupCount(size.X);
				num2 = ComputeGroupCount(size.Y);
				MyImmediateRC.RC.Dispatch(num, num2, 1);
				num3++;
			}
			MyImmediateRC.RC.ComputeShader.Set(m_finalShader);
			uavBindable = ((num3 % 2 == 0) ? borrowedUavTexture2 : borrowedUavTexture);
			srvBind = ((num3 % 2 == 0) ? borrowedUavTexture : borrowedUavTexture2);
			MyImmediateRC.RC.ComputeShader.SetSrv(0, null);
			MyImmediateRC.RC.ComputeShader.SetUav(0, uavBindable);
			MyImmediateRC.RC.ComputeShader.SetSrvs(0, srvBind, m_prevLum);
			num = ComputeGroupCount(size.X);
			num2 = ComputeGroupCount(size.Y);
			MyImmediateRC.RC.Dispatch(num, num2, 1);
			MyImmediateRC.RC.ComputeShader.Set(null);
			MyImmediateRC.RC.ComputeShader.SetUav(0, null);
			MyImmediateRC.RC.CopySubresourceRegion(uavBindable, 0, new ResourceRegion(0, 0, 0, 1, 1, 1), m_prevLum, 0);
			srvBind.Release();
			return uavBindable;
		}

		private static int ComputeGroupCount(int dim)
		{
			return (dim + 8 - 1) / 8;
		}
	}
}
