using System;
using SharpDX.Direct3D11;
using VRage.Library.Collections;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Messages;
using VRageRender.Vertex;

namespace VRage.Render11.Sprites
{
	internal class MySpritesRenderer
	{
		private readonly SpriteScissorStack m_scissorStack = new SpriteScissorStack();

		private readonly MyList<MyVertexFormatSpritePositionTextureRotationColor> m_instances = new MyList<MyVertexFormatSpritePositionTextureRotationColor>();

		private readonly MyList<MySpritesBatch> m_batches = new MyList<MySpritesBatch>();

		private MySpritesBatch m_internalBatch;

		private int m_currentBufferSize;

		private IVertexBuffer m_vb;

		public MySpritesRenderer()
		{
			m_internalBatch = MyManagers.SpritesManager.GetBatch();
			m_internalBatch.Instances = m_instances;
		}

		internal void Dispose()
		{
			MyManagers.Buffers.Dispose(m_vb);
		}

		private unsafe void CheckBufferSize(int requiredSize)
		{
			if (m_vb == null)
			{
				m_currentBufferSize = Math.Max(m_currentBufferSize, requiredSize * 2);
				m_vb = MyManagers.Buffers.CreateVertexBuffer("MySpritesRenderer", m_currentBufferSize, sizeof(MyVertexFormatSpritePositionTextureRotationColor), null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
			}
			else if (m_currentBufferSize < requiredSize)
			{
				m_currentBufferSize = (int)((float)requiredSize * 1.33f);
				MyManagers.Buffers.Resize(m_vb, m_currentBufferSize);
			}
		}

		private void AddSingleSprite(ISrvBindable textureSrv, MyVertexFormatSpritePositionTextureRotationColor sprite, bool ignoreBounds, bool premultipliedAlpha, ISrvBindable maskTextureSrv = null)
		{
			MySpritesBatch internalBatch = m_internalBatch;
			if (internalBatch.Texture != textureSrv || internalBatch.MaskTexture != maskTextureSrv || internalBatch.PremultipliedAlpha != premultipliedAlpha || internalBatch.IgnoreBounds != ignoreBounds)
			{
				Flush();
				internalBatch = m_internalBatch;
				internalBatch.Texture = textureSrv;
				internalBatch.MaskTexture = maskTextureSrv;
				internalBatch.PremultipliedAlpha = premultipliedAlpha;
				internalBatch.IgnoreBounds = ignoreBounds;
			}
			internalBatch.AddSprite(sprite);
		}

		internal void AddSingleSprite(ISrvBindable textureSrv, Color color, Vector2 origin, Vector2 tangent, Rectangle? sourceRect, RectangleF destinationRect, bool ignoreBounds, bool premultipliedAlpha, ISrvBindable maskTextureSrv = null)
		{
			AddSingleSprite(textureSrv, textureSrv.Size, color, origin, tangent, sourceRect, destinationRect, ignoreBounds, premultipliedAlpha, maskTextureSrv);
		}

		internal void AddSingleSprite(ISrvBindable textureSrv, Vector2 textureSize, Color color, Vector2 origin, Vector2 tangent, Rectangle? sourceRect, RectangleF destinationRect, bool ignoreBounds, bool premultipliedAlpha, ISrvBindable maskTextureSrv = null)
		{
			if (m_internalBatch.ScissorRectangle.HasValue)
			{
				RectangleF value = m_internalBatch.ScissorRectangle.Value;
				if (!RectangleF.Intersect(ref value, ref destinationRect, out var _))
				{
					return;
				}
			}
			Vector2 vector = Vector2.Zero;
			Vector2 vector2 = Vector2.One;
			if (sourceRect.HasValue)
			{
				Vector2 vector3 = new Vector2(sourceRect.Value.Left, sourceRect.Value.Top);
				Vector2 vector4 = new Vector2(sourceRect.Value.Width, sourceRect.Value.Height);
				vector = vector3 / textureSize;
				vector2 = vector4 / textureSize;
			}
			AddSingleSprite(textureSrv, new MyVertexFormatSpritePositionTextureRotationColor(new HalfVector4(destinationRect.Size.X, destinationRect.Size.Y, destinationRect.Position.X, destinationRect.Position.Y), new HalfVector4(vector.X, vector.Y, vector2.X, vector2.Y), new HalfVector4(origin.X, origin.Y, tangent.X, tangent.Y), new Byte4((int)color.R, (int)color.G, (int)color.B, (int)color.A)), ignoreBounds, premultipliedAlpha, maskTextureSrv);
		}

		internal void ScissorStackPush(Rectangle rect)
		{
			m_scissorStack.Push(rect);
			Flush();
			m_internalBatch.ScissorRectangle = m_scissorStack.Peek();
		}

		internal void ScissorStackPop()
		{
			m_scissorStack.Pop();
			Flush();
			m_internalBatch.ScissorRectangle = m_scissorStack.Peek();
		}

		internal void Draw(MyRenderContext rc, IRtvBindable rtv, ref MyViewport viewportRtvBound, ref MyViewport viewportRtvFull, ref Vector2 viewportSizeWrittenIntoShaders, MyViewport? targetRegion = null, IBlendState blendState = null)
		{
			Flush();
			if (m_instances.Count != 0)
			{
				CheckBufferSize(m_instances.Count);
				MyMapping myMapping = MyMapping.MapDiscard(rc, m_vb);
				for (int i = 0; i < m_instances.Count; i++)
				{
					MyVertexFormatSpritePositionTextureRotationColor data = m_instances[i];
					myMapping.WriteAndPosition(ref data);
				}
				myMapping.Unmap();
				MyManagers.SpritesManager.Render(m_batches, m_vb, rc, rtv, ref viewportRtvBound, ref viewportRtvFull, ref viewportSizeWrittenIntoShaders, targetRegion, blendState);
			}
			m_batches.Clear();
			Clear();
		}

		private void Flush()
		{
			if (m_internalBatch != null)
			{
				if (m_internalBatch.Texture != null && m_internalBatch.Count > 0)
				{
					m_batches.Add(m_internalBatch);
					m_internalBatch = MyManagers.SpritesManager.GetBatch();
				}
				else
				{
					m_internalBatch.Clear();
				}
			}
			else
			{
				m_internalBatch = MyManagers.SpritesManager.GetBatch();
			}
			m_internalBatch.Instances = m_instances;
			m_internalBatch.ScissorRectangle = m_scissorStack.Peek();
			m_internalBatch.Start = m_instances.Count;
		}

		public void Clear()
		{
			m_instances.SetSize(0);
			m_batches.SetSize(0);
			m_internalBatch.Clear();
			m_internalBatch.Instances = m_instances;
		}

		public void ProcessDrawMessage(MyRenderMessageBase drawMessage, bool touchTextures)
		{
			switch (drawMessage.MessageType)
			{
			case MyRenderMessageEnum.SpriteScissorPush:
			{
				MyRenderMessageSpriteScissorPush myRenderMessageSpriteScissorPush = (MyRenderMessageSpriteScissorPush)drawMessage;
				ScissorStackPush(myRenderMessageSpriteScissorPush.ScreenRectangle);
				break;
			}
			case MyRenderMessageEnum.SpriteScissorPop:
				ScissorStackPop();
				break;
			case MyRenderMessageEnum.DrawSprite:
			{
				MyRenderMessageDrawSprite myRenderMessageDrawSprite = (MyRenderMessageDrawSprite)drawMessage;
				Vector2 center = myRenderMessageDrawSprite.DestinationRectangle.Center;
				Vector2 tangent = Vector2.UnitX;
				float num = myRenderMessageDrawSprite.Rotation;
				if (myRenderMessageDrawSprite.RotationSpeed != 0f)
				{
					num += myRenderMessageDrawSprite.RotationSpeed * (float)MyCommon.FrameTime.Seconds;
				}
				if (num != 0f)
				{
					tangent = new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
				}
				AddSingleSprite(GetTexture(myRenderMessageDrawSprite.Texture, myRenderMessageDrawSprite.WaitTillLoaded), myRenderMessageDrawSprite.Color, center, tangent, myRenderMessageDrawSprite.SourceRectangle, myRenderMessageDrawSprite.DestinationRectangle, myRenderMessageDrawSprite.IgnoreBounds, premultipliedAlpha: true, GetTexture(myRenderMessageDrawSprite.MaskTexture, myRenderMessageDrawSprite.WaitTillLoaded));
				break;
			}
			case MyRenderMessageEnum.DrawSpriteExt:
			{
				MyRenderMessageDrawSpriteExt myRenderMessageDrawSpriteExt = (MyRenderMessageDrawSpriteExt)drawMessage;
				AddSingleSprite(GetTexture(myRenderMessageDrawSpriteExt.Texture, myRenderMessageDrawSpriteExt.WaitTillLoaded), myRenderMessageDrawSpriteExt.Color, myRenderMessageDrawSpriteExt.Origin, myRenderMessageDrawSpriteExt.RightVector, myRenderMessageDrawSpriteExt.SourceRectangle, myRenderMessageDrawSpriteExt.DestinationRectangle, myRenderMessageDrawSpriteExt.IgnoreBounds, premultipliedAlpha: true, GetTexture(myRenderMessageDrawSpriteExt.MaskTexture, myRenderMessageDrawSpriteExt.WaitTillLoaded));
				break;
			}
			case MyRenderMessageEnum.DrawSpriteAtlas:
			{
				MyRenderMessageDrawSpriteAtlas myRenderMessageDrawSpriteAtlas = (MyRenderMessageDrawSpriteAtlas)drawMessage;
				ITexture texture2 = GetTexture(myRenderMessageDrawSpriteAtlas.Texture, waitTillLoaded: true) ?? MyGeneratedTextureManager.MissingColorMetal;
				Vector2I size = texture2.Size;
				Rectangle? sourceRect = new Rectangle((int)((float)size.X * myRenderMessageDrawSpriteAtlas.TextureOffset.X), (int)((float)size.Y * myRenderMessageDrawSpriteAtlas.TextureOffset.Y), (int)((float)size.X * myRenderMessageDrawSpriteAtlas.TextureSize.X), (int)((float)size.Y * myRenderMessageDrawSpriteAtlas.TextureSize.Y));
				float num2 = myRenderMessageDrawSpriteAtlas.HalfSize.X * myRenderMessageDrawSpriteAtlas.Scale.X;
				float num3 = myRenderMessageDrawSpriteAtlas.HalfSize.Y * myRenderMessageDrawSpriteAtlas.Scale.Y;
				RectangleF destinationRect = new RectangleF(myRenderMessageDrawSpriteAtlas.Position.X * myRenderMessageDrawSpriteAtlas.Scale.X - myRenderMessageDrawSpriteAtlas.HalfSize.X * myRenderMessageDrawSpriteAtlas.Scale.X, myRenderMessageDrawSpriteAtlas.Position.Y * myRenderMessageDrawSpriteAtlas.Scale.Y - myRenderMessageDrawSpriteAtlas.HalfSize.Y * myRenderMessageDrawSpriteAtlas.Scale.Y, num2 * 2f, num3 * 2f);
				AddSingleSprite(origin: new Vector2(destinationRect.X + num2, destinationRect.Y + num3), textureSrv: texture2, color: myRenderMessageDrawSpriteAtlas.Color, tangent: myRenderMessageDrawSpriteAtlas.RightVector, sourceRect: sourceRect, destinationRect: destinationRect, ignoreBounds: myRenderMessageDrawSpriteAtlas.IgnoreBounds, premultipliedAlpha: true);
				break;
			}
			case MyRenderMessageEnum.DrawString:
			{
				MyRenderMessageDrawString myRenderMessageDrawString = (MyRenderMessageDrawString)drawMessage;
				MyRender11.GetFont(myRenderMessageDrawString.FontIndex).DrawString(this, myRenderMessageDrawString.ScreenCoord, myRenderMessageDrawString.ColorMask, myRenderMessageDrawString.Text, myRenderMessageDrawString.ScreenScale, myRenderMessageDrawString.IgnoreBounds, myRenderMessageDrawString.ScreenMaxWidth);
				break;
			}
			case MyRenderMessageEnum.DrawStringAligned:
			{
				MyRenderMessageDrawStringAligned myRenderMessageDrawStringAligned = (MyRenderMessageDrawStringAligned)drawMessage;
				MyRender11.GetFont(myRenderMessageDrawStringAligned.FontIndex).DrawStringAligned(this, myRenderMessageDrawStringAligned.ScreenCoord, myRenderMessageDrawStringAligned.ColorMask, myRenderMessageDrawStringAligned.Text, myRenderMessageDrawStringAligned.ScreenScale, myRenderMessageDrawStringAligned.IgnoreBounds, myRenderMessageDrawStringAligned.ScreenMaxWidth, myRenderMessageDrawStringAligned.TextureWidthInPx, myRenderMessageDrawStringAligned.Alignment);
				break;
			}
			case MyRenderMessageEnum.DrawVideo:
			{
				MyRenderMessageDrawVideo myRenderMessageDrawVideo = (MyRenderMessageDrawVideo)drawMessage;
				MyVideoFactory.GetVideo(myRenderMessageDrawVideo.ID)?.Draw(this, myRenderMessageDrawVideo.Rectangle, myRenderMessageDrawVideo.Color, myRenderMessageDrawVideo.FitMode, myRenderMessageDrawVideo.IgnoreBounds);
				break;
			}
			default:
				throw new Exception($"Unhandled message {drawMessage.MessageType}");
			}
			ITexture GetTexture(string texture, bool waitTillLoaded)
			{
				if (texture == null)
				{
					return null;
				}
				IMyStreamedTexture texture3 = MyManagers.Textures.GetTexture(texture, new MyTextureStreamingManager.QueryArgs
				{
					WaitUntilLoaded = waitTillLoaded,
					TextureType = MyFileTextureEnum.GUI
				});
				if (touchTextures)
				{
					texture3.Touch(10000);
				}
				return texture3.Texture;
			}
		}

		public bool ProcessDrawSpritesQueue(MySpriteMessageData messages, bool touchTextures)
		{
			if (messages == null)
			{
				return false;
			}
			MySpriteDrawRenderMessage mySpriteDrawRenderMessage = null;
			try
			{
				foreach (MySpriteDrawRenderMessage message in messages.Messages)
				{
					mySpriteDrawRenderMessage = message;
					ProcessDrawMessage(mySpriteDrawRenderMessage, touchTextures);
				}
			}
			catch (Exception)
			{
				if (mySpriteDrawRenderMessage != null)
				{
					MyRender11.Log.WriteLine(string.Concat("Error in ProcessDrawSpritesQueue: ", mySpriteDrawRenderMessage.MessageClass, ", ", mySpriteDrawRenderMessage.MessageType, ", ", mySpriteDrawRenderMessage.TargetTexture));
				}
				throw;
			}
			return true;
		}
	}
}
