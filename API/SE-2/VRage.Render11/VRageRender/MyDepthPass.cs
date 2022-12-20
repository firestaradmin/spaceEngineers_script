using VRage.Render11.Resources;

namespace VRageRender
{
	[PooledObject(2)]
	internal class MyDepthPass : MyRenderingPass
	{
		internal IDsvBindable Dsv;

		internal IRasterizerState DefaultRasterizer;

		internal bool IsCascade;

		internal sealed override void Begin()
		{
			base.Begin();
			Locals.BindConstantBuffersBatched = true;
			base.RC.SetRasterizerState(DefaultRasterizer);
			base.RC.SetRtv(Dsv);
			base.RC.PixelShader.Set(null);
			base.RC.SetDepthStencilState(null);
		}

		protected sealed override void RecordCommandsInternal(MyRenderableProxy proxy)
		{
			Stats.Draws++;
			SetProxyConstants(proxy);
			BindProxyGeometry(proxy);
			MyRenderUtils.BindShaderBundle(base.RC, proxy.DepthShaders);
			if ((proxy.Flags & MyRenderableProxyFlags.DisableFaceCulling) > MyRenderableProxyFlags.None)
			{
				base.RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			}
			else
			{
				base.RC.SetRasterizerState(DefaultRasterizer);
			}
			MyDrawSubmesh drawSubmesh = proxy.DrawSubmesh;
			if (drawSubmesh.MaterialId != Locals.MatTexturesID && (proxy.Flags & MyRenderableProxyFlags.DepthSkipTextures) <= MyRenderableProxyFlags.None)
			{
				Locals.MatTexturesID = drawSubmesh.MaterialId;
				MyMaterialProxy_2 myMaterialProxy_ = MyMaterials1.ProxyPool.Data[drawSubmesh.MaterialId.Index];
				MyRenderUtils.SetConstants(base.RC, ref myMaterialProxy_.MaterialConstants, 3);
				MyRenderUtils.SetSrvs(base.RC, ref myMaterialProxy_.MaterialSrvs);
			}
			if (proxy.InstanceCount == 0)
			{
				base.RC.DrawIndexed(drawSubmesh.IndexCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex);
				Stats.Triangles += drawSubmesh.IndexCount / 3;
			}
			else
			{
				base.RC.DrawIndexedInstanced(drawSubmesh.IndexCount, proxy.InstanceCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex, proxy.StartInstance);
				Stats.Triangles += proxy.InstanceCount * drawSubmesh.IndexCount / 3;
			}
		}

		protected override void RecordCommandsInternal(ref MyRenderableProxy_2 proxy, int instanceIndex, int sectionIndex)
		{
			MyRenderUtils.SetSrvs(base.RC, ref proxy.ObjectSrvs);
			base.RC.SetRasterizerState(DefaultRasterizer);
			MyRenderUtils.BindShaderBundle(base.RC, proxy.DepthShaders.MultiInstance);
			SetProxyConstants(ref proxy);
			for (int i = 0; i < proxy.SubmeshesDepthOnly.Length; i++)
			{
				MyDrawSubmesh_2 myDrawSubmesh_ = proxy.SubmeshesDepthOnly[i];
				if (proxy.InstanceCount == 0)
				{
					switch (myDrawSubmesh_.DrawCommand)
					{
					case MyDrawCommandEnum.DrawIndexed:
						base.RC.DrawIndexed(myDrawSubmesh_.Count, myDrawSubmesh_.Start, myDrawSubmesh_.BaseVertex);
						break;
					case MyDrawCommandEnum.Draw:
						base.RC.Draw(myDrawSubmesh_.Count, myDrawSubmesh_.Start);
						break;
					}
				}
				else
				{
					switch (myDrawSubmesh_.DrawCommand)
					{
					case MyDrawCommandEnum.DrawIndexed:
						base.RC.DrawIndexedInstanced(myDrawSubmesh_.Count, proxy.InstanceCount, myDrawSubmesh_.Start, myDrawSubmesh_.BaseVertex, proxy.StartInstance);
						break;
					case MyDrawCommandEnum.Draw:
						base.RC.DrawInstanced(myDrawSubmesh_.Count, proxy.InstanceCount, myDrawSubmesh_.Start, proxy.StartInstance);
						break;
					}
				}
			}
		}

		internal override void Cleanup()
		{
			base.Cleanup();
			Dsv = null;
			DefaultRasterizer = null;
			IsCascade = false;
		}

		internal override MyRenderingPass Fork()
		{
			MyDepthPass obj = base.Fork() as MyDepthPass;
			obj.Dsv = Dsv;
			obj.DefaultRasterizer = DefaultRasterizer;
			obj.IsCascade = IsCascade;
			return obj;
		}
	}
}
