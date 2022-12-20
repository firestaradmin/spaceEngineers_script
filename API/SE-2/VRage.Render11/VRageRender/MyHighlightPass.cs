using VRage.Render11.Resources;

namespace VRageRender
{
	internal sealed class MyHighlightPass : MyRenderingPass
	{
		internal static readonly MyHighlightPass Instance = new MyHighlightPass();

		private MyHighlightPass()
		{
			Cleanup();
			SetImmediate(isImmediate: true);
			DebugName = "MyHighlightPass";
		}

		internal override void Begin()
		{
			base.Begin();
			base.RC.SetDepthStencilState(MyDepthStencilStateManager.WriteHighlightStencil, 64);
			base.RC.SetBlendState(null);
			base.RC.PixelShader.SetConstantBuffer(4, MyCommon.HighlightConstants);
		}

		public void RecordCommands(MyRenderableProxy proxy, int sectionmesh, int inctanceId)
		{
			if (proxy.Mesh.Buffers.IB == null || proxy.DrawSubmesh.IndexCount == 0)
			{
				return;
			}
			Stats.Draws++;
			SetProxyConstants(proxy);
			BindProxyGeometry(proxy);
			if ((proxy.Flags & MyRenderableProxyFlags.DisableFaceCulling) > MyRenderableProxyFlags.None)
			{
				base.RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			}
			else
			{
				base.RC.SetRasterizerState(null);
			}
			MyDrawSubmesh myDrawSubmesh = ((sectionmesh >= 0 && sectionmesh < proxy.SectionSubmeshes.Length) ? proxy.SectionSubmeshes[sectionmesh] : proxy.DrawSubmesh);
			if (myDrawSubmesh.MaterialId != Locals.MatTexturesID && myDrawSubmesh.MaterialId.Index < MyMaterials1.ProxyPool.Data.Length)
			{
				Locals.MatTexturesID = myDrawSubmesh.MaterialId;
				MyMaterialProxy_2 myMaterialProxy_ = MyMaterials1.ProxyPool.Data[myDrawSubmesh.MaterialId.Index];
				MyRenderUtils.SetConstants(base.RC, ref myMaterialProxy_.MaterialConstants, 3);
				MyRenderUtils.SetSrvs(base.RC, ref myMaterialProxy_.MaterialSrvs);
			}
			if (proxy.InstanceCount == 0 && myDrawSubmesh.IndexCount > 0)
			{
				base.RC.DrawIndexed(myDrawSubmesh.IndexCount, myDrawSubmesh.StartIndex, myDrawSubmesh.BaseVertex);
				Stats.Triangles += myDrawSubmesh.IndexCount / 3;
			}
			else if (myDrawSubmesh.IndexCount > 0)
			{
				if (inctanceId >= 0)
				{
					base.RC.DrawIndexedInstanced(myDrawSubmesh.IndexCount, 1, myDrawSubmesh.StartIndex, myDrawSubmesh.BaseVertex, inctanceId);
				}
				else
				{
					base.RC.DrawIndexedInstanced(myDrawSubmesh.IndexCount, proxy.InstanceCount, myDrawSubmesh.StartIndex, myDrawSubmesh.BaseVertex, proxy.StartInstance);
				}
				Stats.Triangles += proxy.InstanceCount * myDrawSubmesh.IndexCount / 3;
			}
		}

		protected override void RecordCommandsInternal(ref MyRenderableProxy_2 proxy, int instanceIndex, int sectionIndex)
		{
			MyRenderUtils.SetSrvs(base.RC, ref proxy.ObjectSrvs);
			Stats.Draws++;
			if (instanceIndex == -1)
			{
				MyRenderUtils.BindShaderBundle(base.RC, proxy.HighlightShaders.MultiInstance);
				for (int i = 0; i < proxy.Submeshes.Length; i++)
				{
					MyDrawSubmesh_2 submesh = proxy.Submeshes[i];
					DrawSubmesh(ref proxy, ref submesh, sectionIndex);
				}
			}
			else
			{
				MyRenderUtils.BindShaderBundle(base.RC, proxy.HighlightShaders.SingleInstance);
				MyDrawSubmesh_2 submesh2 = ((sectionIndex != -1) ? proxy.SectionSubmeshes[instanceIndex][sectionIndex] : proxy.Submeshes[instanceIndex]);
				DrawSubmesh(ref proxy, ref submesh2, instanceIndex);
			}
		}

		private void DrawSubmesh(ref MyRenderableProxy_2 proxy, ref MyDrawSubmesh_2 submesh, int instanceIndex)
		{
			MyMaterialProxy_2 myMaterialProxy_ = MyMaterials1.ProxyPool.Data[submesh.MaterialId.Index];
			MyRenderUtils.SetConstants(base.RC, ref myMaterialProxy_.MaterialConstants, 3);
			MyRenderUtils.SetSrvs(base.RC, ref myMaterialProxy_.MaterialSrvs);
			MyMergeInstancingConstants value = default(MyMergeInstancingConstants);
			value.InstanceIndex = instanceIndex;
			value.StartIndex = submesh.Start;
			SetProxyConstants(ref proxy, value);
			if (proxy.InstanceCount == 0)
			{
				switch (submesh.DrawCommand)
				{
				case MyDrawCommandEnum.DrawIndexed:
					base.RC.DrawIndexed(submesh.Count, submesh.Start, submesh.BaseVertex);
					break;
				case MyDrawCommandEnum.Draw:
					base.RC.Draw(submesh.Count, submesh.Start);
					break;
				}
			}
			else
			{
				switch (submesh.DrawCommand)
				{
				case MyDrawCommandEnum.DrawIndexed:
					base.RC.DrawIndexedInstanced(submesh.Count, proxy.InstanceCount, submesh.Start, submesh.BaseVertex, proxy.StartInstance);
					break;
				case MyDrawCommandEnum.Draw:
					base.RC.DrawInstanced(submesh.Count, proxy.InstanceCount, submesh.Start, proxy.StartInstance);
					break;
				}
			}
		}
	}
}
