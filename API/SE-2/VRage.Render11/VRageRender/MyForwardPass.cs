using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.Resources;

namespace VRageRender
{
	[PooledObject(2)]
	internal class MyForwardPass : MyRenderingPass
	{
		internal IDsvBindable Dsv;

		internal ISrvBindable DepthSrv;

		internal readonly RenderTargetView[] Rtvs = new RenderTargetView[2];

		private IConstantBuffer m_cbForward;

		internal sealed override void Begin()
		{
			base.Begin();
			Locals.BindConstantBuffersBatched = true;
			base.RC.SetRtvs(Dsv, Rtvs);
			base.RC.AllShaderStages.SetConstantBuffer(4, MyManagers.Shadows.ShadowCascades.CascadeConstantBufferOld);
			base.RC.PixelShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			base.RC.PixelShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArrayOld);
			m_cbForward = MyCommon.GetForwardConstants(base.RC, Projection);
			base.RC.PixelShader.SetConstantBuffer(7, m_cbForward);
			base.RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestWrite);
		}

		internal override void End()
		{
			MyCommon.ReturnForwardConstants(m_cbForward);
			m_cbForward = null;
			base.RC.ResetTargets();
			base.End();
		}

		protected sealed override void RecordCommandsInternal(MyRenderableProxy proxy)
		{
			Stats.Draws++;
			SetProxyConstants(proxy);
			BindProxyGeometry(proxy);
			MyRenderUtils.BindShaderBundle(base.RC, proxy.ForwardShaders);
			if ((proxy.Flags & MyRenderableProxyFlags.DisableFaceCulling) > MyRenderableProxyFlags.None)
			{
				base.RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			}
			else
			{
				base.RC.SetRasterizerState(null);
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
			if (proxy.InstanceCount == 0 && drawSubmesh.IndexCount > 0)
			{
				base.RC.DrawIndexed(drawSubmesh.IndexCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex);
				Stats.Triangles += drawSubmesh.IndexCount / 3;
			}
			else if (drawSubmesh.IndexCount > 0)
			{
				base.RC.DrawIndexedInstanced(drawSubmesh.IndexCount, proxy.InstanceCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex, proxy.StartInstance);
				Stats.Triangles += proxy.InstanceCount * drawSubmesh.IndexCount / 3;
			}
		}

		protected override void RecordCommandsInternal(ref MyRenderableProxy_2 proxy, int instanceIndex, int sectionIndex)
		{
			MyRenderUtils.SetSrvs(base.RC, ref proxy.ObjectSrvs);
			MyRenderUtils.BindShaderBundle(base.RC, proxy.ForwardShaders.MultiInstance);
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
			Rtvs[0] = (Rtvs[1] = null);
		}

		internal override MyRenderingPass Fork()
		{
			MyForwardPass obj = (MyForwardPass)base.Fork();
			obj.Dsv = Dsv;
			obj.DepthSrv = DepthSrv;
			obj.Rtvs[0] = Rtvs[0];
			obj.Rtvs[1] = Rtvs[1];
			return obj;
		}
	}
}
