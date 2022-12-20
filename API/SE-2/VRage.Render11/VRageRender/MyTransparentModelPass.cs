using System;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.RenderPass;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal sealed class MyTransparentModelPass : MyRenderingPass
	{
		internal static readonly MyTransparentModelPass Instance = new MyTransparentModelPass();

		private MyTransparentModelPass()
		{
			Init();
		}

		public void Init()
		{
			Cleanup();
			DebugName = "MyTransparentModelPass";
		}

		public void BeginDepthOnly()
		{
			base.Begin();
			if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
			{
				base.RC.SetDepthStencilState(MyDepthStencilStateManager.StereoDefaultDepthState);
			}
			else
			{
				base.RC.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			}
			base.RC.SetBlendState(null);
			base.RC.SetRasterizerState(null);
		}

		internal override void Begin()
		{
			base.Begin();
			Locals.BindConstantBuffersBatched = true;
			if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
			{
				base.RC.SetDepthStencilState(MyDepthStencilStateManager.StereoDepthTestReadOnly);
			}
			else
			{
				base.RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestReadOnly);
			}
			base.RC.SetRasterizerState(MyRender11.Settings.Wireframe ? MyRasterizerStateManager.WireframeRasterizerState : null);
			IRtvArrayTexture closeCubemapFinal = MyManagers.EnvironmentProbe.CloseCubemapFinal;
			IRtvArrayTexture farCubemapFinal = MyManagers.EnvironmentProbe.FarCubemapFinal;
			base.RC.PixelShader.SetSrv(11, closeCubemapFinal);
			base.RC.PixelShader.SetSrv(17, farCubemapFinal);
			base.RC.PixelShader.SetSrv(16, MyCommon.GetAmbientBrdfLut());
			base.RC.PixelShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray);
		}

		internal override void End()
		{
			base.RC.PixelShader.SetSrv(1, null);
			base.RC.PixelShader.SetSrv(4, null);
			base.End();
		}

		protected override void RecordCommandsInternal(MyRenderableProxy proxy)
		{
			if (!proxy.Material.Info.GeometryTextureRef.IsUsed && MyTransparentMaterials.TryGetMaterial(proxy.Material.Info.Name, out var material))
			{
				Stats.Draws++;
				MyRenderUtils.BindShaderBundle(base.RC, proxy.Shaders);
				SetProxyConstants(proxy);
				BindProxyGeometry(proxy);
				ISrvBindable[] srvs = new ISrvBindable[2]
				{
					MyManagers.Textures.GetTempTexture(material.Texture, MyFileTextureEnum.COLOR_METAL, 100),
					MyManagers.Textures.GetTempTexture(material.GlossTexture, MyFileTextureEnum.NORMALMAP_GLOSS, 100)
				};
				base.RC.PixelShader.SetSrvs(0, srvs);
				IConstantBuffer transparentMaterialCB = MyRenderPassUtils.GetTransparentMaterialCB(base.RC, material, proxy.Mesh.Info.BoundingBox.Value.HalfExtents.X);
				base.RC.PixelShader.SetConstantBuffer(3, transparentMaterialCB);
				MyDrawSubmesh drawSubmesh = proxy.DrawSubmesh;
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
		}

		public void RecordCommandsDepthOnly(MyRenderableProxy proxy)
		{
			Stats.Draws++;
			MyRenderUtils.BindShaderBundle(base.RC, proxy.TransparentDepthShaders);
			base.RC.PixelShader.Set(null);
			SetProxyConstants(proxy);
			BindProxyGeometry(proxy);
			MyDrawSubmesh drawSubmesh = proxy.DrawSubmesh;
			if (proxy.InstanceCount == 0)
			{
				base.RC.DrawIndexed(drawSubmesh.IndexCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex);
			}
			else
			{
				base.RC.DrawIndexedInstanced(drawSubmesh.IndexCount, proxy.InstanceCount, drawSubmesh.StartIndex, drawSubmesh.BaseVertex, proxy.StartInstance);
			}
		}

		protected override void RecordCommandsInternal(ref MyRenderableProxy_2 proxy, int instanceIndex, int sectionIndex)
		{
			throw new Exception();
		}
	}
}
