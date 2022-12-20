using VRage.Library.Collections;
using VRage.Network;
using VRage.Render11.Resources;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Vertex;

namespace VRage.Render11.Sprites
{
	[GenerateActivator]
	internal class MySpritesBatch
	{
		private class VRage_Render11_Sprites_MySpritesBatch_003C_003EActor : IActivator, IActivator<MySpritesBatch>
		{
			private sealed override object CreateInstance()
			{
				return new MySpritesBatch();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySpritesBatch CreateInstance()
			{
				return new MySpritesBatch();
			}

			MySpritesBatch IActivator<MySpritesBatch>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		internal int Count;

		internal int Start;

		internal RectangleF? ScissorRectangle;

		internal ISrvBindable Texture;

		internal ISrvBindable MaskTexture;

		internal bool PremultipliedAlpha;

		public MyList<MyVertexFormatSpritePositionTextureRotationColor> Instances;

		public bool IgnoreBounds;

		internal void AddSprite(Vector2 clipOffset, Vector2 clipScale, Vector2 texOffset, Vector2 texScale, Vector2 origin, Vector2 tangent, Byte4 color)
		{
			Instances.Add(new MyVertexFormatSpritePositionTextureRotationColor(new HalfVector4(clipOffset.X, clipOffset.Y, clipScale.X, clipScale.Y), new HalfVector4(texOffset.X, texOffset.Y, texScale.X, texScale.Y), new HalfVector4(origin.X, origin.Y, tangent.X, tangent.Y), color));
		}

		internal void AddSprite(MyVertexFormatSpritePositionTextureRotationColor sprite)
		{
			Instances.Add(sprite);
			Count++;
		}

		public void Clear()
		{
			Count = (Start = 0);
			ScissorRectangle = null;
			Texture = null;
			PremultipliedAlpha = false;
			Instances = null;
			IgnoreBounds = false;
		}
	}
}
