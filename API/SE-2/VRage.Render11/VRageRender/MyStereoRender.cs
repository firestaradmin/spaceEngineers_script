using SharpDX.Mathematics.Interop;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyStereoRender
	{
		internal static MyEnvironmentMatrices EnvMatricesLeftEye = new MyEnvironmentMatrices();

		internal static MyEnvironmentMatrices EnvMatricesRightEye = new MyEnvironmentMatrices();

		internal static MyStereoRegion RenderRegion = MyStereoRegion.FULLSCREEN;

		internal static bool Enable => MyRender11.DeviceSettings.UseStereoRendering;

		internal static bool EnableUsingStencilMask => true;

		public static void PSBindRawCB_FrameConstants(MyRenderContext rc)
		{
			if (RenderRegion == MyStereoRegion.FULLSCREEN)
			{
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
			else if (RenderRegion == MyStereoRegion.LEFT)
			{
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
			}
			else if (RenderRegion == MyStereoRegion.RIGHT)
			{
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
			}
		}

		public static void CSBindRawCB_FrameConstants(MyRenderContext rc)
		{
			if (RenderRegion == MyStereoRegion.FULLSCREEN)
			{
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
			else if (RenderRegion == MyStereoRegion.LEFT)
			{
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
			}
			else if (RenderRegion == MyStereoRegion.RIGHT)
			{
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
			}
		}

		public static void VSBindRawCB_FrameConstants(MyRenderContext rc)
		{
			if (RenderRegion == MyStereoRegion.FULLSCREEN)
			{
				rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
			else if (RenderRegion == MyStereoRegion.LEFT)
			{
				rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
			}
			else if (RenderRegion == MyStereoRegion.RIGHT)
			{
				rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
			}
		}

		public static void BindRawCB_FrameConstants(MyRenderContext rc)
		{
			if (RenderRegion == MyStereoRegion.FULLSCREEN)
			{
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			}
			else if (RenderRegion == MyStereoRegion.LEFT)
			{
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
				rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
			}
			else if (RenderRegion == MyStereoRegion.RIGHT)
			{
				rc.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
				rc.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
				rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
			}
		}

		public static void SetViewport(MyRenderContext rc, MyStereoRegion region)
		{
			RawViewportF rawViewportF = default(RawViewportF);
			rawViewportF.Width = MyRender11.ViewportResolution.X;
			rawViewportF.Height = MyRender11.ViewportResolution.Y;
			RawViewportF viewport = rawViewportF;
			switch (region)
			{
			case MyStereoRegion.LEFT:
				rawViewportF = default(RawViewportF);
				rawViewportF.X = viewport.X;
				rawViewportF.Y = viewport.Y;
				rawViewportF.Width = viewport.Width / 2f;
				rawViewportF.Height = viewport.Height;
				viewport = rawViewportF;
				break;
			case MyStereoRegion.RIGHT:
				rawViewportF = default(RawViewportF);
				rawViewportF.X = viewport.X + viewport.Width / 2f;
				rawViewportF.Y = viewport.Y;
				rawViewportF.Width = viewport.Width / 2f;
				rawViewportF.Height = viewport.Height;
				viewport = rawViewportF;
				break;
			}
			rc.SetViewport(ref viewport);
		}

		public static void SetViewport(MyRenderContext rc)
		{
			SetViewport(rc, RenderRegion);
		}

		private static void BeginDrawGBufferPass(MyRenderContext rc)
		{
			SetViewport(rc, MyStereoRegion.LEFT);
			Matrix data = Matrix.Transpose(EnvMatricesLeftEye.ViewProjectionAt0);
			MyMapping myMapping = MyMapping.MapDiscard(rc, MyCommon.ProjectionConstants);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			rc.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
		}

		private static void SwitchDrawGBufferPass(MyRenderContext rc)
		{
			SetViewport(rc, MyStereoRegion.RIGHT);
			Matrix data = Matrix.Transpose(EnvMatricesRightEye.ViewProjectionAt0);
			MyMapping myMapping = MyMapping.MapDiscard(rc, MyCommon.ProjectionConstants);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			rc.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
		}

		private static void EndDrawGBufferPass(MyRenderContext rc)
		{
			SetViewport(rc, MyStereoRegion.FULLSCREEN);
			Matrix data = Matrix.Transpose(MyRender11.Environment.Matrices.ViewProjectionAt0);
			MyMapping myMapping = MyMapping.MapDiscard(rc, MyCommon.ProjectionConstants);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			rc.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
		}

		internal static void DrawGBufferPass(MyRenderContext rc, int vertexCount, int startVertexLocation)
		{
			BeginDrawGBufferPass(rc);
			rc.Draw(vertexCount, startVertexLocation);
			SwitchDrawGBufferPass(rc);
			rc.Draw(vertexCount, startVertexLocation);
			EndDrawGBufferPass(rc);
		}

		internal static void DrawIndexedGBufferPass(MyRenderContext rc, int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			BeginDrawGBufferPass(rc);
			rc.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
			SwitchDrawGBufferPass(rc);
			rc.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
			EndDrawGBufferPass(rc);
		}

		internal static void DrawInstancedGBufferPass(MyRenderContext rc, int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation)
		{
			BeginDrawGBufferPass(rc);
			rc.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
			SwitchDrawGBufferPass(rc);
			rc.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
			EndDrawGBufferPass(rc);
		}

		internal static void DrawIndexedInstancedGBufferPass(MyRenderContext rc, int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation)
		{
			BeginDrawGBufferPass(rc);
			rc.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
			SwitchDrawGBufferPass(rc);
			rc.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
			EndDrawGBufferPass(rc);
		}

		internal static void DrawIndexedInstancedIndirectGBufferPass(MyRenderContext rc, IBuffer bufferForArgsRef, int alignedByteOffsetForArgs)
		{
			BeginDrawGBufferPass(rc);
			rc.DrawIndexedInstancedIndirect(bufferForArgsRef, alignedByteOffsetForArgs);
			SwitchDrawGBufferPass(rc);
			rc.DrawIndexedInstancedIndirect(bufferForArgsRef, alignedByteOffsetForArgs);
			EndDrawGBufferPass(rc);
		}

		internal static void DrawIndexedBillboards(MyRenderContext rc, int indexCount, int startIndexLocation, int baseVertexLocation)
		{
			SetViewport(rc, MyStereoRegion.LEFT);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
			rc.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
			SetViewport(rc, MyStereoRegion.RIGHT);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
			rc.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
			SetViewport(rc, MyStereoRegion.FULLSCREEN);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
		}

		internal static void DrawIndexedInstancedIndirectGPUParticles(MyRenderContext rc, IBuffer bufferForArgsRef, int alignedByteOffsetForArgs)
		{
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstantsStereoLeftEye);
			SetViewport(rc, MyStereoRegion.LEFT);
			rc.DrawIndexedInstancedIndirect(bufferForArgsRef, alignedByteOffsetForArgs);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstantsStereoRightEye);
			SetViewport(rc, MyStereoRegion.RIGHT);
			rc.DrawIndexedInstancedIndirect(bufferForArgsRef, alignedByteOffsetForArgs);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			SetViewport(rc, MyStereoRegion.FULLSCREEN);
		}
	}
}
