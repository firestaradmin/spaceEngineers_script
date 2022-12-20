using SharpDX.Direct3D;
using VRage;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;

namespace VRageRender
{
	internal sealed class MyFoliageRenderingPass : MyRenderingPass, IManager, IManagerDevice
	{
		private const string FoliageRenderShader = "Foliage/Foliage.hlsl";

		private MyInputLayouts.Id m_inputLayout = MyInputLayouts.Id.NULL;

		private MyVertexShaders.Id m_VS = MyVertexShaders.Id.NULL;

		private readonly MyGeometryShaders.Id[] m_GS = new MyGeometryShaders.Id[2]
		{
			MyGeometryShaders.Id.NULL,
			MyGeometryShaders.Id.NULL
		};

		private readonly MyPixelShaders.Id[] m_PS = new MyPixelShaders.Id[2]
		{
			MyPixelShaders.Id.NULL,
			MyPixelShaders.Id.NULL
		};

		private bool m_consumable;

		internal MyFoliageRenderingPass()
		{
			Cleanup();
			DebugName = "MyFoliageRenderingPass";
		}

		private void InitShaders()
		{
			m_VS = MyVertexShaders.Create("Foliage/Foliage.hlsl");
			m_GS[0] = MyGeometryShaders.Create("Foliage/Foliage.hlsl");
			m_PS[0] = MyPixelShaders.Create("Foliage/Foliage.hlsl");
			ShaderMacro[] macros = new ShaderMacro[1]
			{
				new ShaderMacro("ROCK_FOLIAGE", null)
			};
			m_GS[1] = MyGeometryShaders.Create("Foliage/Foliage.hlsl", macros);
			m_PS[1] = MyPixelShaders.Create("Foliage/Foliage.hlsl", macros);
			m_inputLayout = MyInputLayouts.Create(m_VS.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION3, MyVertexInputComponentType.CUSTOM_UNORM4_0));
		}

		internal override void Begin()
		{
			base.Begin();
			base.RC.SetPrimitiveTopology(PrimitiveTopology.PointList);
			base.RC.SetInputLayout(m_inputLayout);
			base.RC.GeometryShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			base.RC.GeometryShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			base.RC.VertexShader.SetConstantBuffer(4, MyCommon.MaterialFoliageTableConstants);
			base.RC.PixelShader.SetConstantBuffer(4, MyCommon.MaterialFoliageTableConstants);
			base.RC.GeometryShader.SetConstantBuffer(4, MyCommon.MaterialFoliageTableConstants);
			base.RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			base.RC.VertexShader.Set(m_VS);
			base.RC.SetInputLayout(m_inputLayout);
			base.RC.SetRtvs(MyGBuffer.Main, MyDepthStencilAccess.ReadWrite);
			base.RC.SetBlendState(null);
			base.RC.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
		}

		internal override void End()
		{
			CleanupPipeline();
			base.End();
		}

		private void CleanupPipeline()
		{
			base.RC.GeometryShader.Set(null);
			base.RC.SetRasterizerState(null);
		}

		internal void RecordCommands(MyRenderableProxy proxy, ref MyObjectDataCommon cod, IVertexBuffer stream, int voxelMatId)
		{
			if (stream != null)
			{
				MaterialFoliage boxedValue = MyVoxelMaterials.Table[voxelMatId].Foliage.BoxedValue;
				MyFoliageType type = boxedValue.Type;
				IConstantBuffer objectBuffer = proxy.GetObjectBuffer(base.RC);
				MyMapping myMapping = MyMapping.MapDiscard(base.RC, objectBuffer);
				myMapping.WriteAndPosition(ref proxy.NonVoxelObjectData);
				myMapping.WriteAndPosition(ref cod);
				myMapping.Unmap();
				base.RC.VertexShader.SetConstantBuffer(2, objectBuffer);
				base.RC.GeometryShader.SetConstantBuffer(2, objectBuffer);
				base.RC.PixelShader.SetConstantBuffer(2, objectBuffer);
				base.RC.GeometryShader.Set(m_GS[(int)type]);
				base.RC.PixelShader.Set(m_PS[(int)type]);
				base.RC.PixelShader.SetSrv(0, boxedValue.ColorTextureArray);
				base.RC.PixelShader.SetSrv(1, boxedValue.NormalTextureArray);
				base.RC.SetVertexBuffer(0, stream);
				base.RC.DrawAuto();
			}
		}

		internal int Render(MyCullQuery cullQuery)
		{
			MyManagers.FoliageGenerator.Render(cullQuery);
			MyList<MyFoliageComponent> foliage = cullQuery.Results.Foliage;
			if (foliage.Count <= 0)
			{
				return 0;
			}
			ViewProjection = MyRender11.Environment.Matrices.ViewProjectionAt0;
			Projection = MyRender11.Environment.Matrices.Projection;
			Viewport = new MyViewport(MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
			Begin();
			foreach (MyFoliageComponent item in foliage)
			{
				item.Render(this);
			}
			End();
			m_consumable = true;
			return foliage.Count;
		}

		public void Consume()
		{
			if (m_consumable)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("FoliageRenderer", "Consume", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageRenderingPass.cs");
=======
				MyGpuProfiler.IC_BeginBlock("FoliageRenderer", "Consume", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageRenderingPass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ExecuteCommandList(MyRender11.RC);
				Cleanup();
				DebugName = "MyFoliageRenderingPass";
				m_consumable = false;
<<<<<<< HEAD
				MyGpuProfiler.IC_EndBlock(0f, "Consume", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageRenderingPass.cs");
=======
				MyGpuProfiler.IC_EndBlock(0f, "Consume", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageRenderingPass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnDeviceInit()
		{
			InitShaders();
		}

		public void OnDeviceReset()
		{
			OnDeviceInit();
		}

		public void OnDeviceEnd()
		{
		}
	}
}
