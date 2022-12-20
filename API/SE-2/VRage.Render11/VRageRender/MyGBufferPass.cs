using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRageRender
{
	[PooledObject(2)]
	internal class MyGBufferPass : MyRenderingPass
	{
		internal MyGBuffer GBuffer;

		internal sealed override void Begin()
		{
			base.Begin();
			Locals.BindConstantBuffersBatched = true;
			base.RC.SetRtvs(GBuffer, MyDepthStencilAccess.ReadWrite);
		}

		protected sealed override void RecordCommandsInternal(MyRenderableProxy proxy)
		{
			SetProxyConstants(proxy);
			BindProxyGeometry(proxy);
			MyRenderUtils.BindShaderBundle(base.RC, proxy.Shaders);
			if (MyRender11.Settings.Wireframe)
			{
				SetDepthStencilView(readOnly: false);
				base.RC.SetBlendState(null);
				if ((proxy.Flags & MyRenderableProxyFlags.DisableFaceCulling) > MyRenderableProxyFlags.None)
				{
					base.RC.SetRasterizerState(MyRasterizerStateManager.NocullWireframeRasterizerState);
				}
				else
				{
					base.RC.SetRasterizerState(MyRasterizerStateManager.WireframeRasterizerState);
				}
			}
			else
			{
				base.RC.SetDepthStencilState(proxy.GbufferDepthState);
				base.RC.SetBlendState(proxy.GbufferBlendState);
				base.RC.SetRasterizerState(proxy.GbufferRasterizerState);
			}
			MyDrawSubmesh drawSubmesh = proxy.DrawSubmesh;
			if (drawSubmesh.MaterialId != Locals.MatTexturesID)
			{
				Locals.MatTexturesID = drawSubmesh.MaterialId;
				if (drawSubmesh.MaterialId != MyMaterialProxyId.NULL)
				{
					MyMaterialProxy_2 myMaterialProxy_ = MyMaterials1.ProxyPool.Data[drawSubmesh.MaterialId.Index];
					MyRenderUtils.SetConstants(base.RC, ref myMaterialProxy_.MaterialConstants, 3);
					MyRenderUtils.SetSrvs(base.RC, ref myMaterialProxy_.MaterialSrvs);
				}
			}
			if (proxy.InstanceCount == 0)
			{
				if (!MyStereoRender.Enable)
				{
					base.RC.DrawIndexed(drawSubmesh.IndexCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex);
				}
				else
				{
					MyStereoRender.DrawIndexedGBufferPass(base.RC, drawSubmesh.IndexCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex);
				}
				Stats.Triangles += drawSubmesh.IndexCount / 3;
			}
			else
			{
				if (!MyStereoRender.Enable)
				{
					base.RC.DrawIndexedInstanced(drawSubmesh.IndexCount, proxy.InstanceCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex, proxy.StartInstance);
				}
				else
				{
					MyStereoRender.DrawIndexedInstancedGBufferPass(base.RC, drawSubmesh.IndexCount, proxy.InstanceCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex, proxy.StartInstance);
				}
				Stats.Triangles += proxy.InstanceCount * drawSubmesh.IndexCount / 3;
			}
			Stats.Draws++;
		}

		protected override void RecordCommandsInternal(ref MyRenderableProxy_2 proxy, int instanceIndex, int sectionIndex)
		{
			MyRenderUtils.SetSrvs(base.RC, ref proxy.ObjectSrvs);
			MyRenderUtils.BindShaderBundle(base.RC, proxy.Shaders.MultiInstance);
			SetDepthStencilView(readOnly: false);
			SetProxyConstants(ref proxy);
			for (int i = 0; i < proxy.Submeshes.Length; i++)
			{
				MyDrawSubmesh_2 myDrawSubmesh_ = proxy.Submeshes[i];
				MyMaterialProxy_2 myMaterialProxy_ = MyMaterials1.ProxyPool.Data[myDrawSubmesh_.MaterialId.Index];
				MyRenderUtils.SetConstants(base.RC, ref myMaterialProxy_.MaterialConstants, 3);
				MyRenderUtils.SetSrvs(base.RC, ref myMaterialProxy_.MaterialSrvs);
				if (proxy.InstanceCount == 0)
				{
					switch (myDrawSubmesh_.DrawCommand)
					{
					case MyDrawCommandEnum.DrawIndexed:
						if (!MyStereoRender.Enable)
						{
							base.RC.DrawIndexed(myDrawSubmesh_.Count, myDrawSubmesh_.Start, myDrawSubmesh_.BaseVertex);
						}
						else
						{
							MyStereoRender.DrawIndexedGBufferPass(base.RC, myDrawSubmesh_.Count, myDrawSubmesh_.Start, myDrawSubmesh_.BaseVertex);
						}
						break;
					case MyDrawCommandEnum.Draw:
						if (!MyStereoRender.Enable)
						{
							base.RC.Draw(myDrawSubmesh_.Count, myDrawSubmesh_.Start);
						}
						else
						{
							MyStereoRender.DrawGBufferPass(base.RC, myDrawSubmesh_.Count, myDrawSubmesh_.Start);
						}
						break;
					}
					continue;
				}
				switch (myDrawSubmesh_.DrawCommand)
				{
				case MyDrawCommandEnum.DrawIndexed:
					if (!MyStereoRender.Enable)
					{
						base.RC.DrawIndexedInstanced(myDrawSubmesh_.Count, proxy.InstanceCount, myDrawSubmesh_.Start, myDrawSubmesh_.BaseVertex, proxy.StartInstance);
					}
					else
					{
						MyStereoRender.DrawIndexedInstancedGBufferPass(base.RC, myDrawSubmesh_.Count, proxy.InstanceCount, myDrawSubmesh_.Start, myDrawSubmesh_.BaseVertex, proxy.StartInstance);
					}
					break;
				case MyDrawCommandEnum.Draw:
					if (!MyStereoRender.Enable)
					{
						base.RC.DrawInstanced(myDrawSubmesh_.Count, proxy.InstanceCount, myDrawSubmesh_.Start, proxy.StartInstance);
					}
					else
					{
						MyStereoRender.DrawInstancedGBufferPass(base.RC, myDrawSubmesh_.Count, proxy.InstanceCount, myDrawSubmesh_.Start, proxy.StartInstance);
					}
					break;
				}
			}
		}

		private void SetDepthStencilView(bool readOnly)
		{
			if (readOnly)
			{
				if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
				{
					base.RC.SetDepthStencilState(MyDepthStencilStateManager.StereoDepthTestReadOnly);
				}
				else
				{
					base.RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestReadOnly);
				}
			}
			else if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
			{
				base.RC.SetDepthStencilState(MyDepthStencilStateManager.StereoDepthTestWrite);
			}
			else
			{
				base.RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestWrite);
			}
		}

		internal override void Cleanup()
		{
			base.Cleanup();
			GBuffer = null;
		}

		internal override MyRenderingPass Fork()
		{
			MyGBufferPass obj = base.Fork() as MyGBufferPass;
			obj.GBuffer = GBuffer;
			return obj;
		}
	}
}
