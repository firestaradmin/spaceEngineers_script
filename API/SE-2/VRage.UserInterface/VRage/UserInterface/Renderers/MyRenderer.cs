using System;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Renderers;
using VRage.UserInterface.Media;
using VRageMath;
using VRageRender;

namespace VRage.UserInterface.Renderers
{
	public class MyRenderer : Renderer
	{
		private Rectangle m_scissorRect;

		public override bool IsFullScreen
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override void Begin()
		{
		}

		public override void Begin(EffectBase effect)
		{
		}

		public override void BeginClipped(Rect clipRect)
		{
			m_scissorRect.X = (int)clipRect.X;
			m_scissorRect.Y = (int)clipRect.Y;
			m_scissorRect.Width = (int)clipRect.Width;
			m_scissorRect.Height = (int)clipRect.Height;
			MyRenderProxy.SpriteScissorPush(m_scissorRect);
		}

		public override void BeginClipped(Rect clipRect, EffectBase effect)
		{
			BeginClipped(clipRect);
		}

		public override void End(bool endEffect = false)
		{
		}

		public override void EndClipped(bool endEffect = false)
		{
			MyRenderProxy.SpriteScissorPop();
		}

		public override FontBase CreateFont(object nativeFont)
		{
			return new VRage.UserInterface.Media.MyFont(nativeFont, 0, 1f);
		}

		public override GeometryBuffer CreateGeometryBuffer()
		{
			return new MyGeometryBuffer();
		}

		public override TextureBase CreateTexture(int width, int height, bool mipmap, bool dynamic)
		{
			return new MyTexture(width, height);
		}

		public override EffectBase CreateEffect(object nativeEffect)
		{
			return null;
		}

		public override EffectBase GetSDFFontEffect()
		{
			return null;
		}

		public override TextureBase CreateTexture(object nativeTexture)
		{
			return new MyTexture((string)nativeTexture);
		}

		public override void Draw(TextureBase texture, PointF position, Size renderSize, ColorW color, Rect source, bool centerOrigin)
		{
			RectangleF destination = new RectangleF(position.X, position.Y, renderSize.Width, renderSize.Height);
			Color color2 = new Color(color.R, color.G, color.B, color.A);
			Rectangle? sourceRectangle = new Rectangle((int)source.X, (int)source.Y, (int)source.Width, (int)source.Height);
			if (source.Width * source.Height == 0f)
			{
				sourceRectangle = null;
			}
			if (!string.IsNullOrEmpty((string)texture.GetNativeTexture()))
			{
				MyRenderProxy.DrawSprite((string)texture.GetNativeTexture(), ref destination, sourceRectangle, color2, 0f, ignoreBounds: false, waitTillLoaded: true);
			}
		}

		public override void Draw(TextureBase texture, PointF position, Size renderSize, ColorW color, bool centerOrigin)
		{
			RectangleF destination = new RectangleF(position.X, position.Y, renderSize.Width, renderSize.Height);
			Color color2 = new Color(color.R, color.G, color.B, color.A);
			Rectangle? sourceRectangle = null;
			string text = (string)texture.GetNativeTexture();
			if (!string.IsNullOrEmpty(text))
			{
				MyRenderProxy.DrawSprite(text, ref destination, sourceRectangle, color2, 0f, ignoreBounds: false, waitTillLoaded: true);
			}
		}

		public override void DrawGeometryColor(GeometryBuffer buffer, PointF position, ColorW color, float opacity, float depth)
		{
		}

		public override void DrawGeometryTexture(GeometryBuffer buffer, PointF position, TextureBase texture, float opacity, float depth)
		{
		}

		public override void DrawText(FontBase font, string text, PointF position, Size renderSize, ColorW color, PointF scale, float depth)
		{
			Vector2 screenCoord = new Vector2(position.X, position.Y);
			Color colorMask = new Color(color.R, color.G, color.B, color.A);
			VRage.UserInterface.Media.MyFont myFont = font as VRage.UserInterface.Media.MyFont;
			MyRenderProxy.DrawString((int)font.GetNativeFont(), screenCoord, colorMask, text, VRage.UserInterface.Media.MyFont.GlobalFontScale * myFont.Scale, MyRenderProxy.MainViewport.Width, ignoreBounds: false);
		}

		public override Rect GetViewport()
		{
			return new Rect(MyRenderProxy.MainViewport.OffsetX, MyRenderProxy.MainViewport.OffsetY, MyRenderProxy.MainViewport.Width, MyRenderProxy.MainViewport.Height);
		}

		public override void ResetNativeSize()
		{
		}

		public override bool IsClipped(PointF position, Size renderSize)
		{
			return false;
		}
	}
}
