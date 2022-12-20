using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Profiler;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRageMath;

namespace VRageRender
{
	internal sealed class MyFoliageGeneratingPass : MyRenderingPass, IManager, IManagerDevice
	{
		private const string FOLIAGE_STREAMING_SHADER = "Foliage/FoliageStreaming.hlsl";

		private MyGeometryShaders.Id m_geometryShader;

		private static IConstantBuffer m_foliageConstants;

		private bool m_consumable;

		internal MyFoliageGeneratingPass()
		{
			Cleanup();
			DebugName = "MyFoliageGeneratingPass";
		}

		internal void Consume()
		{
			if (m_consumable)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("FoliageGeneratingPass", "Consume", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageGeneratingPass.cs");
=======
				MyGpuProfiler.IC_BeginBlock("FoliageGeneratingPass", "Consume", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageGeneratingPass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ExecuteCommandList(MyRender11.RC);
				Cleanup();
				m_consumable = false;
				DebugName = "MyFoliageGeneratingPass";
<<<<<<< HEAD
				MyGpuProfiler.IC_EndBlock(0f, "Consume", "E:\\Repo1\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageGeneratingPass.cs");
=======
				MyGpuProfiler.IC_EndBlock(0f, "Consume", "E:\\Repo3\\Sources\\VRage.Render11\\GeometryStage\\RenderingPasses\\MyFoliageGeneratingPass.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		internal override void Begin()
		{
			base.Begin();
			base.RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			base.RC.GeometryShader.Set(m_geometryShader);
			base.RC.PixelShader.Set(null);
			base.RC.SetRtvNull();
		}

		internal override void End()
		{
			base.RC.GeometryShader.Set(null);
			base.RC.ResetStreamTargets();
			base.End();
		}

		internal void RecordCommands(MyRenderableProxy proxy, MyFoliageStream stream, int voxelMatId, VertexShader vertexShader, InputLayout inputLayout, int materialIndex, int indexCount, int startIndex, int baseVertex)
		{
			if (stream.Stream != null)
			{
				MatrixD m = proxy.WorldMatrix;
				MyObjectDataCommon data = proxy.CommonObjectData;
				data.LocalMatrix = m;
				IConstantBuffer objectBuffer = proxy.GetObjectBuffer(base.RC);
				MyMapping myMapping = MyMapping.MapDiscard(base.RC, objectBuffer);
				myMapping.WriteAndPosition(ref proxy.VoxelCommonObjectData);
				myMapping.WriteAndPosition(ref data);
				myMapping.Unmap();
				base.RC.VertexShader.SetConstantBuffer(2, objectBuffer);
				base.RC.GeometryShader.SetConstantBuffer(2, objectBuffer);
				BindProxyGeometry(proxy);
				base.RC.VertexShader.Set(vertexShader);
				base.RC.SetInputLayout(inputLayout);
				int offsets = -1;
				if (!stream.Append)
				{
					offsets = 0;
					stream.Append = true;
				}
				base.RC.SetTarget(stream.Stream, offsets);
				base.RC.VertexShader.SetConstantBuffer(4, m_foliageConstants);
				base.RC.GeometryShader.SetConstantBuffer(4, m_foliageConstants);
				float data2 = MyVoxelMaterials.Table[voxelMatId].Foliage.BoxedValue.Density * MyRender11.Settings.User.GrassDensityFactor * proxy.NonVoxelObjectData.FoliageMultiplier;
				float data3 = 0f;
				myMapping = MyMapping.MapDiscard(base.RC, m_foliageConstants);
				myMapping.WriteAndPosition(ref data2);
				myMapping.WriteAndPosition(ref materialIndex);
				myMapping.WriteAndPosition(ref voxelMatId);
				myMapping.WriteAndPosition(ref data3);
				myMapping.Unmap();
				base.RC.DrawIndexed(indexCount, startIndex, baseVertex);
			}
		}

		public void Render(MyCullQuery cullQuery)
		{
			MyList<MyFoliageComponent> foliage = cullQuery.Results.Foliage;
			if (foliage.Count == 0)
			{
				return;
			}
			Begin();
			Vector3D point = MyRender11.Environment.Matrices.CameraPosition;
			for (int i = 0; i < foliage.Count; i++)
			{
				MyFoliageComponent myFoliageComponent = foliage[i];
				double num = myFoliageComponent.Owner.WorldAabb.DistanceSquared(ref point);
				if (num >= (double)MyManagers.FoliageManager.GrassDistanceSqr)
				{
					if (num >= (double)MyManagers.FoliageManager.GrassDistanceCachedSqr)
					{
						myFoliageComponent.Dispose();
					}
					foliage.RemoveAtFast(i);
					i--;
				}
				else
				{
					myFoliageComponent.FillStreams(this);
				}
			}
			End();
			m_consumable = true;
		}

		public unsafe void OnDeviceInit()
		{
			StreamOutputElement[] array = new StreamOutputElement[2];
			array[0].Stream = 0;
			array[0].SemanticName = "TEXCOORD";
			array[0].SemanticIndex = 0;
			array[0].ComponentCount = 3;
			array[0].OutputSlot = 0;
			array[1].Stream = 0;
			array[1].SemanticName = "TEXCOORD";
			array[1].SemanticIndex = 1;
			array[1].ComponentCount = 1;
			array[1].OutputSlot = 0;
			int[] strides = new int[1] { sizeof(Vector3) + 4 };
			m_geometryShader = MyGeometryShaders.Create("Foliage/FoliageStreaming.hlsl", null, new MyShaderStreamOutputInfo
			{
				Elements = array,
				Strides = strides,
				RasterizerStreams = -1
			});
			m_foliageConstants = MyManagers.Buffers.CreateConstantBuffer("FoliageConstants", sizeof(Vector4), null, ResourceUsage.Dynamic, isGlobal: true);
		}

		public void OnDeviceReset()
		{
			OnDeviceEnd();
			OnDeviceInit();
		}

		public void OnDeviceEnd()
		{
			MyManagers.Buffers.Dispose(m_foliageConstants);
		}
	}
}
