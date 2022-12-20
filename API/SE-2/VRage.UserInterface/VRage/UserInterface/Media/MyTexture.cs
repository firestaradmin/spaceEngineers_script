using System;
using System.Collections.Generic;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.UserInterface.Media
{
	public class MyTexture : TextureBase
	{
		private int m_width;

		private int m_height;

		private string m_textureName;

		private bool m_initialized;

		public override int Height
		{
			get
			{
				if (!m_initialized && !string.IsNullOrEmpty(m_textureName))
				{
					Vector2 textureSize = MyRenderProxy.GetTextureSize(m_textureName);
					m_width = (int)textureSize.X;
					m_height = (int)textureSize.Y;
					if (m_width * m_height != 0)
					{
						m_initialized = true;
					}
				}
				return m_height;
			}
		}

		public override int Width
		{
			get
			{
				if (!m_initialized && !string.IsNullOrEmpty(m_textureName))
				{
					Vector2 textureSize = MyRenderProxy.GetTextureSize(m_textureName);
					m_width = (int)textureSize.X;
					m_height = (int)textureSize.Y;
					if (m_width * m_height != 0)
					{
						m_initialized = true;
					}
				}
				return m_width;
			}
		}

		public override TextureSurfaceFormat Format
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override void GenerateArrow(ArrowDirection direction, int startX, int lineSize)
		{
		}

		public override void GenerateCheckbox()
		{
		}

		public override void GenerateLinearGradient(PointF lineStart, PointF lineEnd, Thickness borderThickness, List<GradientStop> sortedStops, GradientSpreadMethod spread, bool isBorder)
		{
		}

		public override void GenerateOneToOne()
		{
		}

		public override void GenerateSolidColor(Thickness borderThickness, bool isBorder)
		{
			int width = Width;
			int height = Height;
			if (width == 0 || height == 0)
			{
				return;
			}
			m_textureName = $"EKUI_Texture_{width}_{height}_{borderThickness.Left}_{borderThickness.Top}_{borderThickness.Right}_{borderThickness.Bottom}";
			MyAssetManager myAssetManager = Engine.Instance.AssetManager as MyAssetManager;
			if (myAssetManager == null)
			{
				return;
			}
			if (myAssetManager.GeneratedTextures.Contains(m_textureName))
			{
				m_initialized = true;
				return;
			}
			myAssetManager.GeneratedTextures.Add(m_textureName);
			byte[] array = new byte[width * height * 4];
			for (int i = 0; i < array.Length; i++)
			{
				int num = i / (width * 4);
				int num2 = i / 4 - num * width;
				if (borderThickness.Top > (float)num || borderThickness.Bottom >= (float)(height - num) || borderThickness.Left > (float)num2 || borderThickness.Right >= (float)(width - num2))
				{
					array[i] = byte.MaxValue;
				}
				else
				{
					array[i] = 0;
				}
			}
			m_initialized = true;
			MyRenderProxy.CreateGeneratedTexture(m_textureName, width, height, MyGeneratedTextureType.RGBA, 1, array, generateMipmaps: false);
		}

		public override object GetNativeTexture()
		{
			return m_textureName;
		}

		public override void SetColorData(uint[] data)
		{
		}

		public override void Dispose()
		{
			MyAssetManager myAssetManager;
			if ((myAssetManager = Engine.Instance.AssetManager as MyAssetManager) != null && myAssetManager.GeneratedTextures.Contains(m_textureName))
			{
				MyRenderProxy.DestroyGeneratedTexture(m_textureName);
				myAssetManager.GeneratedTextures.Remove(m_textureName);
			}
			else
			{
				MyRenderProxy.UnloadTexture(m_textureName);
			}
		}

		public MyTexture(int width, int height)
			: base(null)
		{
			m_width = width;
			m_height = height;
			if (width == 1 && height == 1)
			{
				m_textureName = "Textures\\Fake.dds";
			}
		}

		public MyTexture(string textureName)
			: base(null)
		{
			m_textureName = textureName;
		}
	}
}
