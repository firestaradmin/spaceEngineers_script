using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using VRage.Collections;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentScreenAreas : MyRenderComponentCubeBlock
	{
		private class PanelScreenArea
		{
			public uint[] RenderObjectIDs;

			public string Material;

			public string OffscreenTextureName;
<<<<<<< HEAD

			public string Path;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private class Sandbox_Game_Components_MyRenderComponentScreenAreas_003C_003EActor
		{
		}

		private readonly MyEntity m_entity;

		private readonly List<PanelScreenArea> m_screenAreas = new List<PanelScreenArea>();
<<<<<<< HEAD

		public bool IsUpdateModelPropertiesEnabled { get; set; }
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public MyRenderComponentScreenAreas(MyEntity entity)
		{
			m_entity = entity;
<<<<<<< HEAD
			IsUpdateModelPropertiesEnabled = true;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void UpdateModelProperties()
		{
			foreach (PanelScreenArea screenArea in m_screenAreas)
			{
				uint[] renderObjectIDs = screenArea.RenderObjectIDs;
				foreach (uint num in renderObjectIDs)
				{
					if (num != uint.MaxValue)
					{
						MyRenderProxy.UpdateModelProperties(num, screenArea.Material, (RenderFlags)0, (RenderFlags)0, null, null);
					}
				}
			}
		}

		public void ChangeTexture(int area, string path)
		{
			if (area >= m_screenAreas.Count)
			{
				return;
			}
			if (IsUpdateModelPropertiesEnabled)
			{
				for (int i = 0; i < m_screenAreas[area].RenderObjectIDs.Length; i++)
				{
					if (m_screenAreas[area].RenderObjectIDs[i] == uint.MaxValue)
					{
						continue;
					}
					if (string.IsNullOrEmpty(path))
					{
						MyRenderProxy.ChangeMaterialTexture(m_screenAreas[area].RenderObjectIDs[i], m_screenAreas[area].Material);
						if (!System.StringExtensions.Contains(m_screenAreas[area].Material, "TransparentScreenArea", StringComparison.Ordinal))
						{
							MyRenderProxy.UpdateModelProperties(m_screenAreas[area].RenderObjectIDs[i], m_screenAreas[area].Material, (RenderFlags)0, RenderFlags.Visible, null, null);
						}
<<<<<<< HEAD
					}
					else
					{
						MyRenderProxy.ChangeMaterialTexture(m_screenAreas[area].RenderObjectIDs[i], m_screenAreas[area].Material, path);
						if (!System.StringExtensions.Contains(m_screenAreas[area].Material, "TransparentScreenArea", StringComparison.Ordinal))
						{
							MyRenderProxy.UpdateModelProperties(m_screenAreas[area].RenderObjectIDs[i], m_screenAreas[area].Material, RenderFlags.Visible, (RenderFlags)0, null, null);
						}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				return;
			}
			for (int j = 0; j < m_screenAreas.Count; j++)
			{
				PanelScreenArea panelScreenArea = m_screenAreas[j];
				for (int k = 0; k < panelScreenArea.RenderObjectIDs.Length; k++)
				{
<<<<<<< HEAD
					uint num = panelScreenArea.RenderObjectIDs[k];
					if (num == uint.MaxValue)
					{
						continue;
					}
					if (j == area)
					{
						if (string.IsNullOrEmpty(path))
						{
							MyRenderProxy.ChangeMaterialTexture(num, panelScreenArea.Material);
							panelScreenArea.Path = null;
						}
						else
						{
							MyRenderProxy.ChangeMaterialTexture(num, panelScreenArea.Material, path);
							panelScreenArea.Path = path;
						}
					}
					else if (panelScreenArea.Path == null)
					{
						MyRenderProxy.ChangeMaterialTexture(num, panelScreenArea.Material);
					}
					else
=======
					MyRenderProxy.ChangeMaterialTexture(m_screenAreas[area].RenderObjectIDs[j], m_screenAreas[area].Material, path);
					if (!System.StringExtensions.Contains(m_screenAreas[area].Material, "TransparentScreenArea", StringComparison.Ordinal))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyRenderProxy.ChangeMaterialTexture(num, panelScreenArea.Material, panelScreenArea.Path);
					}
				}
			}
		}

		public string GenerateOffscreenTextureName(long entityId, int area)
		{
			if (entityId != base.Entity.EntityId)
			{
				return $"LCDOffscreenTexture_{entityId}_{m_screenAreas[area].Material}";
			}
			return m_screenAreas[area].OffscreenTextureName;
		}

		internal static Vector2 CalcAspectFactor(Vector2I textureSize, Vector2 aspectRatio)
		{
			Vector2 vector = ((textureSize.X > textureSize.Y) ? new Vector2(1f, textureSize.X / textureSize.Y) : new Vector2(textureSize.Y / textureSize.X, 1f));
			return aspectRatio * vector;
		}

		internal static Vector2 CalcShift(Vector2I textureSize, Vector2 aspectFactor)
		{
			return textureSize * (aspectFactor - Vector2.One) / 2f;
		}

		public void RenderSpritesToTexture(int area, ListReader<MySprite> sprites, Vector2I textureSize, Vector2 aspectRatio, Color backgroundColor, byte backgroundAlpha)
		{
			string text = GenerateOffscreenTextureName(m_entity.EntityId, area);
			Vector2 vector = CalcAspectFactor(textureSize, aspectRatio);
			Vector2 vector2 = CalcShift(textureSize, vector);
			bool flag = false;
			for (int i = 0; i < sprites.Count; i++)
			{
				MySprite mySprite = sprites[i];
				Vector2 vector3 = mySprite.Size ?? ((Vector2)textureSize);
				Vector2 vector4 = mySprite.Position ?? ((Vector2)(textureSize / 2));
				Color color = mySprite.Color ?? Color.White;
				vector4 += vector2;
				switch (mySprite.Type)
				{
				case SpriteType.TEXTURE:
				{
					MyLCDTextureDefinition definition2 = MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(MyStringHash.GetOrCompute(mySprite.Data));
					if ((definition2?.SpritePath ?? definition2?.TexturePath) != null)
					{
						switch (mySprite.Alignment)
						{
						case TextAlignment.LEFT:
							vector4 += new Vector2(vector3.X * 0.5f, 0f);
							break;
						case TextAlignment.RIGHT:
							vector4 -= new Vector2(vector3.X * 0.5f, 0f);
							break;
						}
						Vector2 rightVector = new Vector2(1f, 0f);
						if (Math.Abs(mySprite.RotationOrScale) > 1E-05f)
						{
							rightVector = new Vector2((float)Math.Cos(mySprite.RotationOrScale), (float)Math.Sin(mySprite.RotationOrScale));
						}
						MyRenderProxy.DrawSpriteAtlas(definition2.SpritePath ?? definition2.TexturePath, vector4, Vector2.Zero, Vector2.One, rightVector, Vector2.One, color, vector3 / 2f, ignoreBounds: false, text);
					}
					break;
				}
				case SpriteType.TEXT:
				{
					switch (mySprite.Alignment)
					{
					case TextAlignment.RIGHT:
						vector4 -= new Vector2(vector3.X, 0f);
						break;
					case TextAlignment.CENTER:
						vector4 -= new Vector2(vector3.X * 0.5f, 0f);
						break;
					}
					MyFontDefinition definition = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(MyStringHash.GetOrCompute(mySprite.FontId));
					int textureWidthinPx = (int)Math.Round(vector3.X);
					MyRenderProxy.DrawStringAligned((int)(definition?.Id.SubtypeId ?? MyStringHash.GetOrCompute("Debug")), vector4, color, mySprite.Data ?? string.Empty, mySprite.RotationOrScale, float.PositiveInfinity, ignoreBounds: false, text, textureWidthinPx, (MyRenderTextAlignmentEnum)mySprite.Alignment);
					break;
				}
				case SpriteType.CLIP_RECT:
					if (mySprite.Position.HasValue && mySprite.Size.HasValue)
					{
						if (flag)
						{
							MyRenderProxy.SpriteScissorPop(text);
						}
						else
						{
							flag = true;
						}
						MyRenderProxy.SpriteScissorPush(new Rectangle((int)vector4.X, (int)vector4.Y, (int)vector3.X, (int)vector3.Y), text);
					}
					else if (flag)
					{
						MyRenderProxy.SpriteScissorPop(text);
						flag = false;
					}
					break;
				}
			}
			if (flag)
			{
				MyRenderProxy.SpriteScissorPop(text);
			}
			backgroundColor.A = backgroundAlpha;
			uint[] renderObjectIDs = m_screenAreas[area].RenderObjectIDs;
			int j;
			for (j = 0; j < renderObjectIDs.Length && renderObjectIDs[j] == uint.MaxValue; j++)
			{
			}
			if (j < renderObjectIDs.Length)
			{
				MyRenderProxy.RenderOffscreenTexture(text, vector, backgroundColor);
			}
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			UpdateRenderAreas();
		}

		protected void UpdateRenderAreas()
		{
			for (int i = 0; i < base.RenderObjectIDs.Length; i++)
			{
				for (int j = 0; j < m_screenAreas.Count; j++)
				{
					m_screenAreas[j].RenderObjectIDs[i] = base.RenderObjectIDs[i];
				}
			}
		}

		public override void ReleaseRenderObjectID(int index)
		{
			base.ReleaseRenderObjectID(index);
			for (int i = 0; i < m_screenAreas.Count; i++)
			{
				m_screenAreas[i].RenderObjectIDs[index] = uint.MaxValue;
			}
		}

		public void AddScreenArea(uint[] renderObjectIDs, string materialName)
		{
			m_screenAreas.Add(new PanelScreenArea
			{
<<<<<<< HEAD
				RenderObjectIDs = renderObjectIDs.ToArray(),
=======
				RenderObjectIDs = Enumerable.ToArray<uint>((IEnumerable<uint>)renderObjectIDs),
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Material = materialName,
				OffscreenTextureName = $"LCDOffscreenTexture_{base.Entity.EntityId}_{materialName}"
			});
		}

		public static int GetTextureByteCount(Vector2I textureSize)
		{
			return (int)((float)(textureSize.X * textureSize.Y) * 4f * 1.33333325f);
		}

		public void CreateTexture(int area, Vector2I textureSize)
		{
			MyRenderProxy.CreateGeneratedTexture(GenerateOffscreenTextureName(m_entity.EntityId, area), textureSize.X, textureSize.Y, MyGeneratedTextureType.RGBA, 1, null, generateMipmaps: true, immediatelyReady: false);
		}

		public void ReleaseTexture(int area, bool useEmptyTexture = true)
		{
			if (useEmptyTexture)
			{
				ChangeTexture(area, "EMPTY");
			}
			MyRenderProxy.DestroyGeneratedTexture(GenerateOffscreenTextureName(m_entity.EntityId, area));
		}
	}
}
