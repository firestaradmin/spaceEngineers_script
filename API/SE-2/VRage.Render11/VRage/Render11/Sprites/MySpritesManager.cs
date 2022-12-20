using System.Collections.Generic;
using SharpDX.Direct3D;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Sprites
{
	internal class MySpritesManager : IManager, IManagerDevice
	{
		public const string DEFAULT_TEXTURE_TARGET = "DefaultOffscreenTarget";

		public const string DEBUG_TEXTURE_TARGET = "DEBUG_TARGET";

		private readonly Dictionary<string, MySpriteMessageData> m_drawQueue = new Dictionary<string, MySpriteMessageData>();

		private readonly MyConcurrentPool<MySpritesBatch> m_batchPool = new MyConcurrentPool<MySpritesBatch>(1, delegate(MySpritesBatch x)
		{
			x.Clear();
		});

		private readonly MyConcurrentPool<MySpritesRenderer> m_rendererPool = new MyConcurrentPool<MySpritesRenderer>(0, delegate(MySpritesRenderer x)
		{
			x.Clear();
		});

		private MyVertexShaders.Id m_vs;

		private MyPixelShaders.Id m_ps;

		private MyPixelShaders.Id m_psPM;

		private MyInputLayouts.Id m_inputLayout = MyInputLayouts.Id.NULL;

		public MySpriteMessageData DebugDrawMessages { get; private set; }

		public void OnDeviceInit()
		{
			m_vs = MyVertexShaders.Create("Primitives/Sprites.hlsl");
			m_ps = MyPixelShaders.Create("Primitives/Sprites.hlsl");
			m_psPM = MyPixelShaders.Create("Primitives/Sprites.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("PREMULTIPLY_ALPHA", null)
			});
			m_inputLayout = MyInputLayouts.Create(m_vs.InfoId, MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.CUSTOM_HALF4_0, MyVertexInputComponentFreq.PER_INSTANCE), new MyVertexInputComponent(MyVertexInputComponentType.CUSTOM_HALF4_1, MyVertexInputComponentFreq.PER_INSTANCE), new MyVertexInputComponent(MyVertexInputComponentType.CUSTOM_HALF4_2, MyVertexInputComponentFreq.PER_INSTANCE), new MyVertexInputComponent(MyVertexInputComponentType.COLOR4, MyVertexInputComponentFreq.PER_INSTANCE)));
		}

		public void AddMessage(MyRenderMessageBase message, int frameId)
		{
			MySpriteDrawRenderMessage mySpriteDrawRenderMessage = (MySpriteDrawRenderMessage)message;
			string key = mySpriteDrawRenderMessage.TargetTexture ?? "DefaultOffscreenTarget";
			if (!m_drawQueue.TryGetValue(key, out var value))
			{
				value = MyObjectPoolManager.Allocate<MySpriteMessageData>();
				value.FrameId = frameId;
				m_drawQueue.Add(key, value);
			}
			if (value.FrameId != frameId)
			{
				foreach (MySpriteDrawRenderMessage message2 in value.Messages)
				{
					message2.Dispose();
				}
				value.Messages.SetSize(0);
				value.FrameId = frameId;
			}
			mySpriteDrawRenderMessage.AddRef();
			value.Messages.Add(mySpriteDrawRenderMessage);
		}

		internal MySpriteMessageData AcquireDrawMessages(string textureName)
		{
			if (m_drawQueue.TryGetValue(textureName, out var value))
			{
				m_drawQueue.Remove(textureName);
				return value;
			}
			return null;
		}

		internal void DisposeDrawMessages(MySpriteMessageData messages)
		{
			foreach (MySpriteDrawRenderMessage message in messages.Messages)
			{
				message.Dispose();
			}
			MyObjectPoolManager.Deallocate(messages);
		}

		public MySpriteMessageData CloseDebugDrawMessages()
		{
			MySpriteMessageData debugDrawMessages = DebugDrawMessages;
			DebugDrawMessages = MyObjectPoolManager.Allocate<MySpriteMessageData>();
			return debugDrawMessages;
		}

		public void FrameEnd()
		{
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
		}

		public void Render(MyList<MySpritesBatch> batches, IVertexBuffer vb, MyRenderContext rc, IRtvBindable rtv, ref MyViewport viewportRtvBound, ref MyViewport viewportRtvFull, ref Vector2 viewportSizeWrittenIntoShaders, MyViewport? targetRegion, IBlendState blendState)
		{
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleStrip);
			rc.SetInputLayout(m_inputLayout);
			rc.VertexShader.Set(m_vs);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.AllShaderStages.SetConstantBuffer(1, rc.GetObjectCB(64));
			rc.PixelShader.Set(m_ps);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rc.SetRtv(rtv);
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			rc.SetVertexBuffer(0, vb);
			rc.SetBlendState(blendState ?? MyBlendStateManager.BlendAlphaPremult);
			Vector4 data = new Vector4(-1f, -1f, 2f, 2f);
			if (targetRegion.HasValue)
			{
				data = new Vector4(targetRegion.Value.OffsetX, targetRegion.Value.OffsetY, targetRegion.Value.Width, targetRegion.Value.Height);
			}
			Vector2 data2 = 1f / viewportSizeWrittenIntoShaders;
			MyMapping myMapping = MyMapping.MapDiscard(rc, rc.GetObjectCB(64));
			myMapping.WriteAndPosition(ref data);
			myMapping.WriteAndPosition(ref data2);
			myMapping.Unmap();
			bool flag = true;
			Vector2 vector = Vector2.Zero;
			Vector2 vector2 = Vector2.Zero;
			bool flag2 = batches.Count <= 0 || !batches[0].IgnoreBounds;
			foreach (MySpritesBatch batch in batches)
			{
				if (flag2 != batch.IgnoreBounds)
				{
					if (batch.IgnoreBounds)
					{
						rc.SetViewport(ref viewportRtvFull);
						vector2 = new Vector2(viewportRtvFull.OffsetX, viewportRtvFull.OffsetY);
						vector = new Vector2(viewportRtvFull.Width, viewportRtvFull.Height) * data2;
					}
					else
					{
						rc.SetViewport(ref viewportRtvBound);
						vector2 = new Vector2(viewportRtvBound.OffsetX, viewportRtvBound.OffsetY);
						vector = new Vector2(viewportRtvBound.Width, viewportRtvBound.Height) * data2;
					}
					flag2 = batch.IgnoreBounds;
				}
				if (batch.PremultipliedAlpha != flag)
				{
					flag = batch.PremultipliedAlpha;
					rc.PixelShader.Set(batch.PremultipliedAlpha ? m_ps : m_psPM);
				}
				if (batch.ScissorRectangle.HasValue)
				{
					rc.SetRasterizerState(MyRasterizerStateManager.ScissorTestRasterizerState);
					RectangleF value = batch.ScissorRectangle.Value;
					value.X = value.X * vector.X + vector2.X;
					value.Y = value.Y * vector.Y + vector2.Y;
					value.Width *= vector.X;
					value.Height *= vector.Y;
					rc.SetScissorRectangle((int)value.X, (int)value.Y, (int)(value.X + value.Width), (int)(value.Y + value.Height));
				}
				else
				{
					rc.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
				}
				rc.PixelShader.SetSrv(0, batch.Texture);
				rc.PixelShader.SetSrv(1, batch.MaskTexture ?? MyGeneratedTextureManager.ReleaseMissingAlphamaskTex);
				rc.DrawInstanced(4, batch.Count, 0, batch.Start);
				m_batchPool.Return(batch);
			}
			rc.SetBlendState(null);
			rc.SetRasterizerState(null);
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			rc.SetRtvNull();
			rc.PixelShader.SetSrv(0, null);
		}

		public MySpritesRenderer GetSpritesRenderer()
		{
			return m_rendererPool.Get();
		}

		public void Return(MySpritesRenderer renderer)
		{
			m_rendererPool.Return(renderer);
		}

		public MySpritesBatch GetBatch()
		{
			return m_batchPool.Get();
		}
	}
}
