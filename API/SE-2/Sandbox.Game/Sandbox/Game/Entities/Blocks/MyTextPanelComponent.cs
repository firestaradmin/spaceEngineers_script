using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.GUI.TextPanel;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	public class MyTextPanelComponent : Sandbox.ModAPI.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextSurface
	{
		[Serializable]
		[ProtoContract]
		public struct FontData
		{
			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EFontData_003C_003EName_003C_003EAccessor : IMemberAccessor<FontData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FontData owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FontData owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EFontData_003C_003ETextColor_003C_003EAccessor : IMemberAccessor<FontData, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FontData owner, in Color value)
				{
					owner.TextColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FontData owner, out Color value)
				{
					value = owner.TextColor;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EFontData_003C_003EAlignment_003C_003EAccessor : IMemberAccessor<FontData, TextAlignment>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FontData owner, in TextAlignment value)
				{
					owner.Alignment = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FontData owner, out TextAlignment value)
				{
					value = owner.Alignment;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EFontData_003C_003ESize_003C_003EAccessor : IMemberAccessor<FontData, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FontData owner, in float value)
				{
					owner.Size = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FontData owner, out float value)
				{
					value = owner.Size;
				}
			}

			private class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EFontData_003C_003EActor : IActivator, IActivator<FontData>
			{
				private sealed override object CreateInstance()
				{
					return default(FontData);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override FontData CreateInstance()
				{
					return (FontData)(object)default(FontData);
				}

				FontData IActivator<FontData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[Nullable]
			public string Name;

			[ProtoMember(4)]
			public Color TextColor;

			[ProtoMember(7)]
			public TextAlignment Alignment;

			[ProtoMember(10)]
			public float Size;

			public FontData(string font, Color color, TextAlignment alignment, float size)
			{
				Name = font;
				TextColor = color;
				Alignment = alignment;
				Size = size;
			}
		}

		[Serializable]
		[ProtoContract]
		public struct ContentMetadata
		{
			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EContentMetadata_003C_003EContentType_003C_003EAccessor : IMemberAccessor<ContentMetadata, ContentType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContentMetadata owner, in ContentType value)
				{
					owner.ContentType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContentMetadata owner, out ContentType value)
				{
					value = owner.ContentType;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EContentMetadata_003C_003EBackgroundColor_003C_003EAccessor : IMemberAccessor<ContentMetadata, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContentMetadata owner, in Color value)
				{
					owner.BackgroundColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContentMetadata owner, out Color value)
				{
					value = owner.BackgroundColor;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EContentMetadata_003C_003EChangeInterval_003C_003EAccessor : IMemberAccessor<ContentMetadata, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContentMetadata owner, in float value)
				{
					owner.ChangeInterval = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContentMetadata owner, out float value)
				{
					value = owner.ChangeInterval;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EContentMetadata_003C_003EPreserveAspectRatio_003C_003EAccessor : IMemberAccessor<ContentMetadata, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContentMetadata owner, in bool value)
				{
					owner.PreserveAspectRatio = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContentMetadata owner, out bool value)
				{
					value = owner.PreserveAspectRatio;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EContentMetadata_003C_003ETextPadding_003C_003EAccessor : IMemberAccessor<ContentMetadata, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContentMetadata owner, in float value)
				{
					owner.TextPadding = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContentMetadata owner, out float value)
				{
					value = owner.TextPadding;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EContentMetadata_003C_003EBackgroundAlpha_003C_003EAccessor : IMemberAccessor<ContentMetadata, byte>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContentMetadata owner, in byte value)
				{
					owner.BackgroundAlpha = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContentMetadata owner, out byte value)
				{
					value = owner.BackgroundAlpha;
				}
			}

			private class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EContentMetadata_003C_003EActor : IActivator, IActivator<ContentMetadata>
			{
				private sealed override object CreateInstance()
				{
					return default(ContentMetadata);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ContentMetadata CreateInstance()
				{
					return (ContentMetadata)(object)default(ContentMetadata);
				}

				ContentMetadata IActivator<ContentMetadata>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			/// <summary>
			/// What kinds of data is this panel displaying?
			/// </summary>
			[ProtoMember(1)]
			public ContentType ContentType;

			/// <summary>
			/// Background color of the panel.
			/// </summary>
			[ProtoMember(4)]
			public Color BackgroundColor;

			/// <summary>
			/// The amount of seconds after which the pictures (in m_selectedTexturesToDraw) cycle to the next image.
			/// </summary>
			[ProtoMember(7)]
			public float ChangeInterval;

			/// <summary>
			/// If enabled, image is resized to fit the screen while maintaining its aspect ratio.
			/// </summary>
			[ProtoMember(10)]
			public bool PreserveAspectRatio;

			/// <summary>
			/// Amount of spacing on all sides, as a percentage of size of image.
			/// </summary>
			[ProtoMember(13)]
			public float TextPadding;

			/// <summary>
			/// Value for offscreen texture alpha channel
			/// - for PBR material it is metalness (should be 0)
			/// - for transparent texture it is opacity
			/// </summary>
			[ProtoMember(16)]
			public byte BackgroundAlpha;

			public ContentMetadata(ContentType contentType, Color backgroundColor, float changeInterval, bool preserveAspectRatio, float textPadding, byte backgroundAlpha)
			{
				ContentType = contentType;
				BackgroundColor = backgroundColor;
				ChangeInterval = changeInterval;
				PreserveAspectRatio = preserveAspectRatio;
				TextPadding = textPadding;
				BackgroundAlpha = backgroundAlpha;
			}
		}

		[Serializable]
		[ProtoContract]
		public struct ScriptData
		{
			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EScriptData_003C_003EScript_003C_003EAccessor : IMemberAccessor<ScriptData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ScriptData owner, in string value)
				{
					owner.Script = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ScriptData owner, out string value)
				{
					value = owner.Script;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EScriptData_003C_003ECustomizeScript_003C_003EAccessor : IMemberAccessor<ScriptData, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ScriptData owner, in bool value)
				{
					owner.CustomizeScript = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ScriptData owner, out bool value)
				{
					value = owner.CustomizeScript;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EScriptData_003C_003EBackgroundColor_003C_003EAccessor : IMemberAccessor<ScriptData, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ScriptData owner, in Color value)
				{
					owner.BackgroundColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ScriptData owner, out Color value)
				{
					value = owner.BackgroundColor;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EScriptData_003C_003EForegroundColor_003C_003EAccessor : IMemberAccessor<ScriptData, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ScriptData owner, in Color value)
				{
					owner.ForegroundColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ScriptData owner, out Color value)
				{
					value = owner.ForegroundColor;
				}
			}

			private class Sandbox_Game_Entities_Blocks_MyTextPanelComponent_003C_003EScriptData_003C_003EActor : IActivator, IActivator<ScriptData>
			{
				private sealed override object CreateInstance()
				{
					return default(ScriptData);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ScriptData CreateInstance()
				{
					return (ScriptData)(object)default(ScriptData);
				}

				ScriptData IActivator<ScriptData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string Script;

			[ProtoMember(4)]
			public bool CustomizeScript;

			[ProtoMember(7)]
			public Color BackgroundColor;

			[ProtoMember(10)]
			public Color ForegroundColor;

			public ScriptData(string script, bool customizeScript, Color backgroundColor, Color foregroundColor)
			{
				Script = script;
				CustomizeScript = customizeScript;
				BackgroundColor = backgroundColor;
				ForegroundColor = foregroundColor;
			}
		}

		protected class m_fontData_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType fontData;
				ISyncType result = (fontData = new Sync<FontData, SyncDirection.BothWays>(P_1, P_2));
				((MyTextPanelComponent)P_0).m_fontData = (Sync<FontData, SyncDirection.BothWays>)fontData;
				return result;
			}
		}

		protected class m_contentData_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType contentData;
				ISyncType result = (contentData = new Sync<ContentMetadata, SyncDirection.BothWays>(P_1, P_2));
				((MyTextPanelComponent)P_0).m_contentData = (Sync<ContentMetadata, SyncDirection.BothWays>)contentData;
				return result;
			}
		}

		protected class m_scriptData_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType scriptData;
				ISyncType result = (scriptData = new Sync<ScriptData, SyncDirection.BothWays>(P_1, P_2));
				((MyTextPanelComponent)P_0).m_scriptData = (Sync<ScriptData, SyncDirection.BothWays>)scriptData;
				return result;
			}
		}

		public static readonly MySerializer<MySpriteCollection> SpriteSerializer = MyFactory.GetSerializer<MySpriteCollection>();

		private static readonly List<MySerializableSprite> m_spritesToSend = new List<MySerializableSprite>();

		public const int NUM_DECIMALS = 3;

		public const int MAX_NUMBER_CHARACTERS = 100000;

		public const string DEFAULT_OFFLINE_TEXTURE = "Offline";

		public const string DEFAULT_ONLINE_TEXTURE = "Online";

		public const string DEFAULT_OFFLINE_WIDE_TEXTURE = "Offline_wide";

		public const string DEFAULT_ONLINE_WIDE_TEXTURE = "Online_wide";

		private const int DEFAULT_RESOLUTION = 512;

		private const int MAX_SPRITE_COLLECTION_BYTE_SIZE = 9504;

		private static readonly StringBuilder m_helperSB = new StringBuilder();

		private int m_currentSelectedTexture;

		private long m_previousUpdateTime;

		private string m_previousTextureID;

		private string m_previousScript = string.Empty;

		private bool m_textureGenerated;

		private bool m_staticContent;

		private MySpriteCollection m_spriteQueue;

		private MySpriteCollection m_lastSpriteQueue;

		private readonly List<MyLCDTextureDefinition> m_definitions = new List<MyLCDTextureDefinition>();

		private readonly List<MySprite> m_renderLayers = new List<MySprite>();

		private readonly List<MySprite> m_lastRenderLayers = new List<MySprite>();

		private readonly List<MySprite> m_textAndImageLayers = new List<MySprite>();

		private readonly List<MyLCDTextureDefinition> m_selectedTexturesToDraw = new List<MyLCDTextureDefinition>();

		private readonly List<MyGuiControlListbox.Item> m_selectedTexturesToAdd = new List<MyGuiControlListbox.Item>();

		private readonly List<MyGuiControlListbox.Item> m_selectedTexturesToRemove = new List<MyGuiControlListbox.Item>();

		private MyTerminalBlock m_block;

		private MyRenderComponentScreenAreas m_render;

		private IMyTextSurfaceScript m_script;

		private Sync<FontData, SyncDirection.BothWays> m_fontData;

		private Sync<ContentMetadata, SyncDirection.BothWays> m_contentData;

		private bool m_backgroundChanged;

		private Color m_backgroundColorLast = Color.Black;

		private Sync<ScriptData, SyncDirection.BothWays> m_scriptData;

		private MySerializableSpriteCollection m_externalSprites;

		private string m_name;

		private string m_displayName;

		private StringBuilder m_text;

		private bool m_failedToRenderTexture;

		private bool m_useOnlineTexture = true;

		private bool m_areSpritesDirty;

		private int m_lastUpdate;

		private int m_renderObjectIndex;

		private int m_area;

		private int m_lastAreaIndex;

		private Vector2I m_textureSize = Vector2I.One;

		private Vector2 m_screenAspectRatio = Vector2.One;

		private Action<MyTextPanelComponent, int[]> m_addImagesToSelectionRequest;

		private Action<MyTextPanelComponent, int[]> m_removeImagesFromSelectionRequest;

		private Action<MyTextPanelComponent, string> m_changeTextRequest;

		private Action<MyTextPanelComponent, MySerializableSpriteCollection> m_spriteCollectionUpdate;

		private int m_randomOffset;

		private readonly MySpriteCollection m_spriteQueueError;

		private StringBuilder m_textHelper = new StringBuilder();

		private bool m_textHelperDirty;

		public Vector3D WorldPosition => m_block.PositionComp.WorldMatrixRef.Translation;

		public string Name => m_name;

		public string DisplayName => m_displayName;

		public StringBuilder Text
		{
			get
			{
				return m_text;
			}
			set
			{
				m_text.Clear();
				m_text.Append((object)value);
				UpdateIsStaticContent();
			}
		}

		public int Area => m_area;

		public ContentType ContentType
		{
			get
			{
				return m_contentData.Value.ContentType;
			}
			set
			{
				if (m_contentData.Value.ContentType != value)
				{
					ContentMetadata value2 = m_contentData.Value;
					if (value == ContentType.IMAGE)
					{
						value2.ContentType = ContentType.TEXT_AND_IMAGE;
					}
					else
					{
						value2.ContentType = value;
					}
					m_contentData.Value = value2;
					UpdateIsStaticContent();
				}
			}
		}

		public ShowTextOnScreenFlag ShowTextFlag
		{
			get
			{
				if (m_contentData.Value.ContentType == ContentType.TEXT_AND_IMAGE)
				{
					return ShowTextOnScreenFlag.PUBLIC;
				}
				return ShowTextOnScreenFlag.NONE;
			}
			set
			{
				if (value != ShowTextFlag)
				{
					if (value == ShowTextOnScreenFlag.NONE)
					{
						ContentType = ContentType.NONE;
					}
					else
					{
						ContentType = ContentType.TEXT_AND_IMAGE;
					}
				}
			}
		}

		public bool ShowTextOnScreen => m_contentData.Value.ContentType == ContentType.TEXT_AND_IMAGE;

		public List<MyLCDTextureDefinition> Definitions => m_definitions;

		public int CurrentSelectedTexture
		{
			get
			{
				return m_currentSelectedTexture;
			}
			set
			{
				m_currentSelectedTexture = value;
			}
		}

		internal MyRenderComponentScreenAreas Render => m_render;

		public Vector2 SurfaceSize => m_textureSize * MyRenderComponentScreenAreas.CalcAspectFactor(m_textureSize, m_screenAspectRatio);

		public Vector2 TextureSize => m_textureSize;

		public List<MyLCDTextureDefinition> SelectedTexturesToDraw => m_selectedTexturesToDraw;

		public Color BackgroundColor
		{
			get
			{
				return m_contentData.Value.BackgroundColor;
			}
			set
			{
				if (m_contentData.Value.BackgroundColor != value)
				{
					ContentMetadata value2 = m_contentData.Value;
					value2.BackgroundColor = value;
					m_contentData.Value = value2;
					m_backgroundChanged = true;
					UpdateIsStaticContent();
				}
			}
		}

		public byte BackgroundAlpha
		{
			get
			{
				return m_contentData.Value.BackgroundAlpha;
			}
			set
			{
				if (m_contentData.Value.BackgroundAlpha != value)
				{
					ContentMetadata value2 = m_contentData.Value;
					value2.BackgroundAlpha = value;
					m_contentData.Value = value2;
					m_backgroundChanged = true;
					UpdateIsStaticContent();
				}
			}
		}

		public Color FontColor
		{
			get
			{
				return m_fontData.Value.TextColor;
			}
			set
			{
				if (m_fontData.Value.TextColor != value)
				{
					FontData value2 = m_fontData.Value;
					value2.TextColor = value;
					m_fontData.Value = value2;
				}
			}
		}

		public MyDefinitionId Font
		{
			get
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_FontDefinition), m_fontData.Value.Name);
			}
			set
			{
				if (m_fontData.Value.Name != value.SubtypeName)
				{
					FontData value2 = m_fontData.Value;
					value2.Name = value.SubtypeName;
					m_fontData.Value = value2;
				}
			}
		}

		public float FontSize
		{
			get
			{
				return m_fontData.Value.Size;
			}
			set
			{
				if (m_fontData.Value.Size != value)
				{
					float size = (float)Math.Round(value, 3);
					FontData value2 = m_fontData.Value;
					value2.Size = size;
					m_fontData.Value = value2;
				}
			}
		}

		public TextAlignment Alignment
		{
			get
			{
				return m_fontData.Value.Alignment;
			}
			set
			{
				if (m_fontData.Value.Alignment != value)
				{
					FontData value2 = m_fontData.Value;
					value2.Alignment = value;
					m_fontData.Value = value2;
					m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
			}
		}

		public string Script
		{
			get
			{
				return m_scriptData.Value.Script;
			}
			set
			{
				string text = value ?? string.Empty;
				if (m_scriptData.Value.Script != text)
				{
					ScriptData value2 = m_scriptData.Value;
					value2.Script = text;
					value2.CustomizeScript = false;
					SetScriptData(value2);
					m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
			}
		}

		public bool CustomizeScripts
		{
			get
			{
				return m_scriptData.Value.CustomizeScript;
			}
			set
			{
				if (m_scriptData.Value.CustomizeScript != value)
				{
					ScriptData value2 = m_scriptData.Value;
					value2.CustomizeScript = value;
					SetScriptData(value2);
					m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
			}
		}

		public Color ScriptBackgroundColor
		{
			get
			{
				return m_scriptData.Value.BackgroundColor;
			}
			set
			{
				if (m_scriptData.Value.BackgroundColor != value)
				{
					ScriptData value2 = m_scriptData.Value;
					value2.BackgroundColor = value;
					value2.CustomizeScript = true;
					SetScriptData(value2);
					m_backgroundChanged = true;
				}
			}
		}

		public Color ScriptForegroundColor
		{
			get
			{
				return m_scriptData.Value.ForegroundColor;
			}
			set
			{
				if (m_scriptData.Value.ForegroundColor != value)
				{
					ScriptData value2 = m_scriptData.Value;
					value2.ForegroundColor = value;
					value2.CustomizeScript = true;
					SetScriptData(value2);
					m_backgroundChanged = true;
				}
			}
		}

		public bool FailedToRenderTexture => m_failedToRenderTexture;

		public float ChangeInterval
		{
			get
			{
				return m_contentData.Value.ChangeInterval;
			}
			set
			{
				float num = (float)Math.Round(value, 3);
				if (m_contentData.Value.ChangeInterval != num)
				{
					ContentMetadata value2 = m_contentData.Value;
					value2.ChangeInterval = num;
					m_contentData.Value = value2;
				}
			}
		}

		public bool PreserveAspectRatio
		{
			get
			{
				return m_contentData.Value.PreserveAspectRatio;
			}
			set
			{
				if (m_contentData.Value.PreserveAspectRatio != value)
				{
					ContentMetadata value2 = m_contentData.Value;
					value2.PreserveAspectRatio = value;
					m_contentData.Value = value2;
					UpdateIsStaticContent();
				}
			}
		}

		public float TextPadding
		{
			get
			{
				return m_contentData.Value.TextPadding;
			}
			set
			{
				float num = (float)Math.Round(value, 3);
				if (m_contentData.Value.TextPadding != num)
				{
					ContentMetadata value2 = m_contentData.Value;
					value2.TextPadding = num;
					m_contentData.Value = value2;
				}
			}
		}

		public bool IsStatic => m_staticContent;

		public MySerializableSpriteCollection ExternalSprites => m_lastSpriteQueue;

		public int TextureByteCount => MyRenderComponentScreenAreas.GetTextureByteCount(m_textureSize);

		string Sandbox.ModAPI.Ingame.IMyTextSurface.CurrentlyShownImage
		{
			get
			{
				if (SelectedTexturesToDraw.Count == 0)
				{
					return null;
				}
				if (CurrentSelectedTexture >= SelectedTexturesToDraw.Count)
				{
					return SelectedTexturesToDraw[0].Id.SubtypeName;
				}
				return SelectedTexturesToDraw[CurrentSelectedTexture].Id.SubtypeName;
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Font
		{
			get
			{
				return Font.SubtypeName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value) != null)
				{
					Font = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value).Id;
				}
			}
		}

		TextAlignment Sandbox.ModAPI.Ingame.IMyTextSurface.Alignment
		{
			get
			{
				return Alignment;
			}
			set
			{
				Alignment = value;
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Script
		{
			get
			{
				return Script;
			}
			set
			{
				Script = value;
			}
		}

		ContentType Sandbox.ModAPI.Ingame.IMyTextSurface.ContentType
		{
			get
			{
				return ContentType;
			}
			set
			{
				ContentType = value;
			}
		}

		Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.SurfaceSize => SurfaceSize;

		Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.TextureSize => TextureSize;

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.PreserveAspectRatio
		{
			get
			{
				return PreserveAspectRatio;
			}
			set
			{
				PreserveAspectRatio = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyTextSurface.TextPadding
		{
			get
			{
				return TextPadding;
			}
			set
			{
				TextPadding = value;
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptBackgroundColor
		{
			get
			{
				return ScriptBackgroundColor;
			}
			set
			{
				ScriptBackgroundColor = value;
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptForegroundColor
		{
			get
			{
				return ScriptForegroundColor;
			}
			set
			{
				ScriptForegroundColor = value;
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Name => Name;

		string Sandbox.ModAPI.Ingame.IMyTextSurface.DisplayName => DisplayName;

		public void SetFailedToRenderTexture(int area, bool failed)
		{
			if (failed)
			{
				SetDefaultTexture(isOnline: false);
			}
			m_failedToRenderTexture = failed;
		}

<<<<<<< HEAD
		public void ChangeRenderTexture(int area, string path, bool isForced)
=======
		public void ChangeRenderTexture(int area, string path)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (!(path == m_previousTextureID) || isForced)
			{
				Render.ChangeTexture(area, path);
				m_previousTextureID = path;
			}
		}

		public void RemoveTexture(int area)
		{
			Render.ChangeTexture(area, null);
		}

		public static void CreateTerminalControls<T>() where T : MyTerminalBlock, Sandbox.ModAPI.IMyTextPanel, IMyTextPanelComponentOwner
		{
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("Content", MySpaceTexts.BlockPropertyTitle_PanelContent, MySpaceTexts.Blank)
			{
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					FillContentComboBoxContent(x);
				},
				Getter = (T x) => (long)x.PanelComponent.ContentType,
				Setter = delegate(T x, long y)
				{
					x.PanelComponent.ContentType = (ContentType)y;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<T>
			{
				Visible = (T x) => x.PanelComponent.ContentType != ContentType.NONE
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("Script", MySpaceTexts.BlockPropertyTitle_PanelScript, MySpaceTexts.Blank)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.SCRIPT,
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> lastFocused)
				{
					x.PanelComponent.FillScriptsContent(list1, list2, lastFocused);
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					x.PanelComponent.SelectScriptToDraw(y);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("ScriptForegroundColor", MySpaceTexts.BlockPropertyTitle_FontColor)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.SCRIPT,
				Getter = (T x) => x.PanelComponent.ScriptForegroundColor,
				Setter = delegate(T x, Color v)
				{
					x.PanelComponent.ScriptForegroundColor = v;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("ScriptBackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, isAutoscaleEnabled: true, 0.055f, isAutosEllipsisEnabled: true)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.SCRIPT,
				Getter = (T x) => x.PanelComponent.ScriptBackgroundColor,
				Setter = delegate(T x, Color v)
				{
					x.PanelComponent.ScriptBackgroundColor = v;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("ShowTextPanel", MySpaceTexts.BlockPropertyTitle_TextPanelShowPublicTextPanel, MySpaceTexts.Blank, delegate(T x)
			{
				x.OpenWindow(isEditable: true, sync: true, isPublic: true);
			})
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				Enabled = (T x) => !x.IsTextPanelOpen,
				SupportsMultipleBlocks = false
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("Font", MySpaceTexts.BlockPropertyTitle_Font, MySpaceTexts.Blank)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					FillFontComboBoxContent(x);
				},
				Getter = (T x) => (int)x.PanelComponent.Font.SubtypeId,
				Setter = delegate(T x, long y)
				{
					x.PanelComponent.Font = new MyDefinitionId(typeof(MyObjectBuilder_FontDefinition), MyStringHash.TryGet((int)y));
				}
			});
			MyTerminalControlSlider<T> myTerminalControlSlider = new MyTerminalControlSlider<T>("FontSize", MySpaceTexts.BlockPropertyTitle_LCDScreenTextSize, MySpaceTexts.Blank);
			myTerminalControlSlider.Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE;
			myTerminalControlSlider.SetLimits(0.1f, 10f);
			myTerminalControlSlider.DefaultValue = 1f;
			myTerminalControlSlider.Getter = (T x) => x.PanelComponent.FontSize;
			myTerminalControlSlider.Setter = delegate(T x, float v)
			{
				x.PanelComponent.FontSize = v;
			};
			myTerminalControlSlider.Writer = delegate(T x, StringBuilder result)
			{
				result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.FontSize, 3));
			};
			myTerminalControlSlider.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("FontColor", MySpaceTexts.BlockPropertyTitle_FontColor)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				Getter = (T x) => x.PanelComponent.FontColor,
				Setter = delegate(T x, Color v)
				{
					x.PanelComponent.FontColor = v;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("alignment", MySpaceTexts.BlockPropertyTitle_Alignment, MySpaceTexts.Blank)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					FillAlignmentComboBoxContent(x);
				},
				Getter = (T x) => (long)x.PanelComponent.Alignment,
				Setter = delegate(T x, long y)
				{
					x.PanelComponent.Alignment = (TextAlignment)y;
				}
			});
			MyTerminalControlSlider<T> myTerminalControlSlider2 = new MyTerminalControlSlider<T>("TextPaddingSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenTextPadding, MySpaceTexts.Blank);
			myTerminalControlSlider2.Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE;
			myTerminalControlSlider2.SetLimits(0f, 50f);
			myTerminalControlSlider2.DefaultValue = 0f;
			myTerminalControlSlider2.Getter = (T x) => x.PanelComponent.TextPadding;
			myTerminalControlSlider2.Setter = delegate(T x, float v)
			{
				x.PanelComponent.TextPadding = v;
			};
			myTerminalControlSlider2.Writer = delegate(T x, StringBuilder result)
			{
				result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.TextPadding, 1)).Append("%");
			};
			myTerminalControlSlider2.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<T>
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("BackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, isAutoscaleEnabled: true, 0.055f, isAutosEllipsisEnabled: true)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				Getter = (T x) => x.PanelComponent.BackgroundColor,
				Setter = delegate(T x, Color v)
				{
					x.PanelComponent.BackgroundColor = v;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("ImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenDefinitionsTextures, MySpaceTexts.Blank, multiSelect: true)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> lastFocused)
				{
					x.PanelComponent.FillListContent(list1, list2);
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					x.PanelComponent.SelectImageToDraw(y);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("SelectTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectTextures, MySpaceTexts.Blank, delegate(T x)
			{
				x.PanelComponent.AddImagesToSelection();
			})
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE
			});
			MyTerminalControlSlider<T> myTerminalControlSlider3 = new MyTerminalControlSlider<T>("ChangeIntervalSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenRefreshInterval, MySpaceTexts.Blank, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
			myTerminalControlSlider3.Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE;
			myTerminalControlSlider3.SetLimits(0f, 30f);
			myTerminalControlSlider3.DefaultValue = 0f;
			myTerminalControlSlider3.Getter = (T x) => x.PanelComponent.ChangeInterval;
			myTerminalControlSlider3.Setter = delegate(T x, float v)
			{
				x.PanelComponent.ChangeInterval = v;
			};
			myTerminalControlSlider3.Writer = delegate(T x, StringBuilder result)
			{
				result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.ChangeInterval, 3)).Append(" s");
			};
			myTerminalControlSlider3.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("SelectedImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectedTextures, MySpaceTexts.Blank, multiSelect: true)
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE,
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> lastFocused)
				{
					x.PanelComponent.FillSelectedListContent(list1, list2);
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					x.PanelComponent.SelectImage(y);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("RemoveSelectedTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenRemoveSelectedTextures, MySpaceTexts.Blank, delegate(T x)
			{
				x.PanelComponent.RemoveImagesFromSelection();
			})
			{
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE
			});
			MyTerminalControlCheckbox<T> obj = new MyTerminalControlCheckbox<T>("PreserveAspectRatio", MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio, MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio)
			{
				Getter = (T x) => x.PanelComponent.PreserveAspectRatio,
				Setter = delegate(T x, bool v)
				{
					x.PanelComponent.PreserveAspectRatio = v;
				},
				Visible = (T x) => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE
			};
			obj.EnableAction();
			MyTerminalControlFactory.AddControl(obj);
		}

		public static void FillContentComboBoxContent(List<MyTerminalControlComboBoxItem> items)
		{
			MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MySpaceTexts.BlockPropertyValue_NoContent
			};
			items.Add(item);
			item = new MyTerminalControlComboBoxItem
			{
				Key = 1L,
				Value = MySpaceTexts.BlockPropertyValue_TextAndImageContent
			};
			items.Add(item);
			item = new MyTerminalControlComboBoxItem
			{
				Key = 3L,
				Value = MySpaceTexts.BlockPropertyValue_ScriptContent
			};
			items.Add(item);
		}

		public static void FillFontComboBoxContent(List<MyTerminalControlComboBoxItem> items)
		{
			foreach (MyFontDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyFontDefinition>())
			{
				if (definition.Public)
				{
					items.Add(new MyTerminalControlComboBoxItem
					{
						Key = (int)definition.Id.SubtypeId,
						Value = MyStringId.GetOrCompute(definition.Id.SubtypeName)
					});
				}
			}
		}

		public static void FillAlignmentComboBoxContent(List<MyTerminalControlComboBoxItem> items)
		{
			foreach (object value in Enum.GetValues(typeof(TextAlignmentEnum)))
			{
				items.Add(new MyTerminalControlComboBoxItem
				{
					Key = (int)value,
					Value = MyStringId.GetOrCompute(value.ToString())
				});
			}
		}

		public void FillScriptsContent(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems, ICollection<MyGuiControlListbox.Item> lastFocused)
		{
			MyGuiControlListbox.Item item = null;
			MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(MyTexts.Get(MySpaceTexts.None));
			listBoxContent.Add(item2);
			if (MyTextSurfaceScriptFactory.Instance == null)
			{
				return;
			}
			foreach (KeyValuePair<string, MyTextSurfaceScriptFactory.ScriptInfo> script in MyTextSurfaceScriptFactory.Instance.Scripts)
			{
				MyGuiControlListbox.Item item3 = new MyGuiControlListbox.Item(MyTexts.Get(script.Value.DisplayName), null, null, script.Key);
				listBoxContent.Add(item3);
				if (string.Compare(script.Key, m_scriptData.Value.Script, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					listBoxSelectedItems.Add(item3);
					item = item3;
				}
			}
			if (listBoxSelectedItems.Count == 0)
			{
				listBoxSelectedItems.Add(item2);
			}
			if (item != null)
			{
				lastFocused.Add(item);
			}
		}

		public void FillListContent(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			foreach (MyLCDTextureDefinition definition in m_definitions)
			{
				if (definition.Public && definition.Selectable)
				{
					if (!string.IsNullOrEmpty(definition.LocalizationId))
					{
						m_helperSB.Clear().Append((object)MyTexts.Get(MyStringId.GetOrCompute(definition.LocalizationId)));
					}
					else
					{
						m_helperSB.Clear().Append(definition.Id.SubtypeName);
					}
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(m_helperSB, null, null, definition.Id.SubtypeName);
					listBoxContent.Add(item);
				}
			}
			foreach (MyGuiControlListbox.Item item2 in m_selectedTexturesToAdd)
			{
				listBoxSelectedItems.Add(item2);
			}
		}

		public void FillSelectedListContent(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			foreach (MyLCDTextureDefinition item2 in m_selectedTexturesToDraw)
			{
				if (!string.IsNullOrEmpty(item2.LocalizationId))
				{
					m_helperSB.Clear().Append((object)MyTexts.Get(MyStringId.GetOrCompute(item2.LocalizationId)));
				}
				else
				{
					m_helperSB.Clear().Append(item2.Id.SubtypeName);
				}
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(m_helperSB, null, null, item2.Id.SubtypeName);
				listBoxContent.Add(item);
			}
		}

		public void SelectImage(List<MyGuiControlListbox.Item> imageId)
		{
			m_selectedTexturesToRemove.Clear();
			for (int i = 0; i < imageId.Count; i++)
			{
				m_selectedTexturesToRemove.Add(imageId[i]);
			}
		}

		public void SelectImageToDraw(List<MyGuiControlListbox.Item> imageIds)
		{
			m_selectedTexturesToAdd.Clear();
			for (int i = 0; i < imageIds.Count; i++)
			{
				m_selectedTexturesToAdd.Add(imageIds[i]);
			}
		}

		public void SelectScriptToDraw(List<MyGuiControlListbox.Item> combo)
		{
			if (combo != null && combo.Count != 0)
			{
				Script = (combo[0].UserData as string) ?? string.Empty;
			}
		}

		public MyTextPanelComponent(int area, MyTerminalBlock block, string name, string displayName, int textureResolution, int screenAreaCount, int screenWidth = 1, int screenHeight = 1, bool useOnlineTexture = true)
		{
			m_block = block;
			m_name = name;
			m_displayName = displayName;
			m_text = new StringBuilder();
			m_textureSize = GetTextureResolutionForAspectRatio(screenWidth, screenHeight, textureResolution);
			m_area = area;
			m_lastAreaIndex = screenAreaCount - 1;
			if (screenWidth > screenHeight)
			{
				m_screenAspectRatio = new Vector2(1f, 1f * (float)screenHeight / (float)screenWidth);
			}
			else
			{
				m_screenAspectRatio = new Vector2(1f * (float)screenWidth / (float)screenHeight, 1f);
			}
			m_useOnlineTexture = useOnlineTexture;
			m_definitions.Clear();
			foreach (MyLCDTextureDefinition lCDTexturesDefinition in MyDefinitionManager.Static.GetLCDTexturesDefinitions())
			{
				m_definitions.Add(lCDTexturesDefinition);
			}
			m_spriteQueueError = new MySpriteCollection(new MySprite[2]
			{
				new MySprite(SpriteType.TEXTURE, "Cross", SurfaceSize / 2f, SurfaceSize),
				new MySprite(SpriteType.TEXT, MyTexts.GetString(MyCommonTexts.Scripts_TooManySprites), SurfaceSize / 2f - new Vector2(0f, SurfaceSize.Y / 2f), SurfaceSize, Color.Yellow, "Debug", TextAlignment.CENTER, (float)Math.Min(m_textureSize.X, m_textureSize.Y) / 512f)
			});
		}

		public void Init(MySerializableSpriteCollection initialSprites = default(MySerializableSpriteCollection), string initialScript = null, Action<MyTextPanelComponent, int[]> addImagesRequest = null, Action<MyTextPanelComponent, int[]> removeImagesRequest = null, Action<MyTextPanelComponent, string> changeTextRequest = null, Action<MyTextPanelComponent, MySerializableSpriteCollection> spriteCollectionUpdate = null)
		{
			m_previousTextureID = string.Empty;
			ContentMetadata contentMetadata = default(ContentMetadata);
			contentMetadata.ContentType = ((!string.IsNullOrEmpty(initialScript)) ? ContentType.SCRIPT : ContentType.NONE);
			contentMetadata.BackgroundColor = Color.Black;
			contentMetadata.ChangeInterval = 0f;
			contentMetadata.BackgroundAlpha = 0;
			contentMetadata.PreserveAspectRatio = false;
			contentMetadata.TextPadding = 2f;
			ContentMetadata content = contentMetadata;
			FontData fontData = default(FontData);
			fontData.Name = "Debug";
			fontData.TextColor = Color.White;
			fontData.Alignment = TextAlignment.LEFT;
			fontData.Size = 1f;
			FontData font = fontData;
			ScriptData scriptData = default(ScriptData);
			scriptData.Script = initialScript ?? string.Empty;
			scriptData.CustomizeScript = false;
			scriptData.BackgroundColor = MyTextSurfaceScriptBase.DEFAULT_BACKGROUND_COLOR;
			scriptData.ForegroundColor = MyTextSurfaceScriptBase.DEFAULT_FONT_COLOR;
			ScriptData script = scriptData;
			SetLocalValues(content, font, script);
			m_fontData.ValueChanged += m_fontData_ValueChanged;
			m_contentData.ValueChanged += m_contentData_ValueChanged;
			m_scriptData.ValueChanged += m_scriptData_ValueChanged;
			if (initialSprites.Sprites != null)
			{
				for (int i = 0; i < initialSprites.Sprites.Length; i++)
				{
					initialSprites.Sprites[i].Index = i;
				}
			}
			UpdateSpriteCollection(initialSprites);
			m_addImagesToSelectionRequest = addImagesRequest;
			m_removeImagesFromSelectionRequest = removeImagesRequest;
			m_changeTextRequest = changeTextRequest;
			m_spriteCollectionUpdate = spriteCollectionUpdate;
			m_block.IsWorkingChanged += m_block_IsWorkingChanged;
			m_randomOffset = MyRandom.Instance.Next(10000);
			UpdateIsStaticContent();
		}

		public void SetLocalValues(ContentMetadata content, FontData font, ScriptData script)
		{
			m_contentData.SetLocalValue(content);
			m_fontData.SetLocalValue(font);
			m_scriptData.SetLocalValue(script);
			if (content.BackgroundColor != Color.Black)
			{
				m_backgroundChanged = true;
			}
		}

		public void GetLocalValues(out ContentMetadata content, out FontData font, out ScriptData script)
		{
			content = m_contentData.Value;
			font = m_fontData.Value;
			script = m_scriptData.Value;
		}

		public void Unload()
		{
		}

		public void UpdateIsStaticContent()
		{
			if (ContentType == ContentType.NONE || !m_block.IsWorking)
			{
				m_staticContent = true;
			}
			else if ((m_text == null || m_text.Length == 0) && !PreserveAspectRatio && BackgroundColor == Color.Black && ContentType == ContentType.TEXT_AND_IMAGE)
			{
				m_staticContent = true;
			}
			else
			{
				m_staticContent = false;
			}
		}

		public void UpdateAfterSimulation(bool isWorking, bool isInRange)
		{
			if (Render == null)
			{
				return;
			}
			UpdateModApiText();
			if (Sync.IsServer && m_areSpritesDirty)
			{
				SendSpriteQueue();
			}
			if (!m_block.IsFunctional)
			{
				ReleaseTexture(useEmptyTexture: false);
				return;
			}
			if (!isInRange && !m_staticContent && isWorking)
			{
				ReleaseTexture();
				SetDefaultTexture(isWorking);
				return;
			}
			if (!isWorking)
			{
				ReleaseTexture();
				SetDefaultTexture(isOnline: false);
				return;
			}
			if (m_staticContent)
			{
				ReleaseTexture();
			}
			else
			{
				EnsureGeneratedTexture();
			}
			switch (ContentType)
			{
			case ContentType.NONE:
				SetDefaultTexture(isOnline: true);
				break;
			case ContentType.TEXT_AND_IMAGE:
				UpdateRenderTexture();
				break;
			case ContentType.SCRIPT:
				if (m_script == null && Script != string.Empty)
				{
					SelectScriptToDraw(Script);
				}
				UpdateSpritesTexture();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void SendSpriteQueue()
		{
			m_areSpritesDirty = false;
			MySerializableSpriteCollection delta = GetDelta(m_lastSpriteQueue, m_spriteQueue);
			if (!delta.Sprites.IsNullOrEmpty())
			{
				m_spriteCollectionUpdate?.Invoke(this, delta);
				m_lastSpriteQueue = m_spriteQueue;
			}
			m_spriteQueue = default(MySpriteCollection);
		}

		private MySerializableSpriteCollection GetDelta(MySpriteCollection original, MySpriteCollection current)
		{
			if (original.Sprites == null)
			{
				original.Sprites = new MySprite[0];
			}
			int num = 0;
			if (current.Sprites != null)
			{
				num = current.Sprites.Length;
			}
			m_spritesToSend.Clear();
			for (int i = 0; i < num; i++)
			{
				if (i >= original.Sprites.Length || !original.Sprites[i].Equals(current.Sprites[i]))
				{
					MySerializableSprite item = current.Sprites[i];
					item.Index = i;
					m_spritesToSend.Add(item);
				}
			}
			return new MySerializableSpriteCollection(m_spritesToSend.ToArray(), num);
		}

		private void EnsureGeneratedTexture()
		{
			if (!m_textureGenerated && !Sync.IsDedicated)
			{
				Render.CreateTexture(m_area, m_textureSize);
				m_textureGenerated = true;
				m_backgroundChanged = true;
				m_externalSprites_ValueChanged(null);
			}
		}

		private void SetDefaultTexture(bool isOnline)
		{
<<<<<<< HEAD
			bool isForced = (m_block.SlimBlock.RecolorTextureUpdateFlags & MySlimBlock.MyRecolorTextureUpdateFlags.Default) != 0;
			if (!isOnline)
			{
				ChangeRenderTexture(path: (!(m_screenAspectRatio.X > m_screenAspectRatio.Y * 4f)) ? GetPathForID("Offline") : GetPathForID("Offline_wide"), area: m_area, isForced: isForced);
			}
			else
			{
				ChangeRenderTexture(path: (!m_useOnlineTexture) ? GetPathForID(null) : ((!(m_screenAspectRatio.X > m_screenAspectRatio.Y * 4f)) ? GetPathForID("Online") : GetPathForID("Online_wide")), area: m_area, isForced: isForced);
			}
			if (m_area == m_lastAreaIndex)
			{
				m_block.SlimBlock.RecolorTextureUpdateFlags &= ~MySlimBlock.MyRecolorTextureUpdateFlags.Default;
			}
=======
			if (isOnline)
			{
				ChangeRenderTexture(path: (!m_useOnlineTexture) ? GetPathForID(null) : ((!(m_screenAspectRatio.X > m_screenAspectRatio.Y * 4f)) ? GetPathForID("Online") : GetPathForID("Online_wide")), area: m_area);
				return;
			}
			ChangeRenderTexture(path: (!(m_screenAspectRatio.X > m_screenAspectRatio.Y * 4f)) ? GetPathForID("Offline") : GetPathForID("Offline_wide"), area: m_area);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected bool UpdateSpritesTexture()
		{
			if (Render == null)
			{
				return false;
			}
			if (m_script != null && NeedsUpdate(m_script))
			{
				m_script.Run();
			}
			if (!AreEqual(m_lastRenderLayers, m_renderLayers) || m_backgroundChanged)
			{
				Render.RenderSpritesToTexture(m_area, m_renderLayers, m_textureSize, m_screenAspectRatio, ScriptBackgroundColor, BackgroundAlpha);
				m_lastRenderLayers.Clear();
				m_lastRenderLayers.AddList(m_renderLayers);
				m_backgroundChanged = false;
			}
			bool isForced = (m_block.SlimBlock.RecolorTextureUpdateFlags & MySlimBlock.MyRecolorTextureUpdateFlags.Script) != 0;
			ChangeRenderTexture(m_area, GetRenderTextureName(), isForced);
			SetFailedToRenderTexture(m_area, failed: false);
			if (m_area == m_lastAreaIndex)
			{
				m_block.SlimBlock.RecolorTextureUpdateFlags &= ~MySlimBlock.MyRecolorTextureUpdateFlags.Script;
			}
			return true;
		}

		protected void UpdateTexture()
		{
			if (m_selectedTexturesToDraw.Count <= 0)
			{
				return;
			}
			int num = (int)(ChangeInterval * 1000f);
			if (num > 0)
			{
				long num2 = (long)MySession.Static.ElapsedGameTime.TotalMilliseconds;
				if (num2 - m_previousUpdateTime > num)
				{
					m_currentSelectedTexture++;
					m_previousUpdateTime = num2;
				}
			}
			if (m_currentSelectedTexture >= m_selectedTexturesToDraw.Count)
			{
				m_currentSelectedTexture = 0;
			}
		}

		protected bool UpdateRenderTexture()
		{
			if (Render == null || !Render.IsRenderObjectAssigned(m_renderObjectIndex))
			{
				return false;
			}
			Vector2 vector = MyRenderComponentScreenAreas.CalcAspectFactor(m_textureSize, m_screenAspectRatio);
			MyRenderComponentScreenAreas.CalcShift(m_textureSize, vector);
			m_textAndImageLayers.Clear();
			bool flag = m_textureSize.X == m_textureSize.Y;
			Vector2 vector2 = new Vector2(m_textureSize.X, m_textureSize.Y) * 0.5f;
			Vector2 vector3 = m_textureSize * vector;
			bool isForced = (m_block.SlimBlock.RecolorTextureUpdateFlags & MySlimBlock.MyRecolorTextureUpdateFlags.TextAndImage) != 0;
			if (m_selectedTexturesToDraw.Count > 0)
			{
				UpdateTexture();
				if ((m_text == null || m_text.Length == 0) && !PreserveAspectRatio && BackgroundColor == Color.Black)
				{
					ChangeRenderTexture(m_area, m_selectedTexturesToDraw[m_currentSelectedTexture].TexturePath, isForced);
					return false;
				}
				MySprite item = MySprite.CreateSprite(m_selectedTexturesToDraw[m_currentSelectedTexture].Id.SubtypeName, vector2, vector3);
				if (PreserveAspectRatio)
				{
					item.Size = new Vector2(Math.Min(m_textureSize.X, m_textureSize.Y));
					if (!flag)
					{
						ref Vector2? size = ref item.Size;
						size *= vector;
					}
					else
					{
						ref Vector2? size2 = ref item.Size;
						size2 *= Math.Min(vector.X, vector.Y);
					}
				}
				m_textAndImageLayers.Add(item);
			}
			if (m_text != null && m_text.Length > 0)
			{
				MySprite item2 = MySprite.CreateText(m_text.ToString(), Font.SubtypeName, FontColor, FontSize * (float)Math.Min(m_textureSize.X, m_textureSize.Y) / 512f, Alignment);
				Vector2 vector4 = new Vector2(TextPadding * 0.02f);
				vector3 *= Vector2.One - vector4;
				switch (Alignment)
				{
				case TextAlignment.LEFT:
					item2.Position = vector2 - vector3 * 0.5f;
					break;
				case TextAlignment.RIGHT:
					item2.Position = vector2 + new Vector2(vector3.X * 0.5f, (0f - vector3.Y) * 0.5f);
					break;
				case TextAlignment.CENTER:
					item2.Position = vector2 - new Vector2(0f, vector3.Y * 0.5f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				m_textAndImageLayers.Add(item2);
			}
			else if (m_selectedTexturesToDraw.Count == 0 && BackgroundColor == Color.Black)
			{
<<<<<<< HEAD
				ChangeRenderTexture(m_area, "EMPTY", isForced);
=======
				ChangeRenderTexture(m_area, "EMPTY");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return false;
			}
			if (!AreEqual(m_lastRenderLayers, m_textAndImageLayers) || m_backgroundChanged)
			{
				m_backgroundChanged = false;
				Render.RenderSpritesToTexture(m_area, m_textAndImageLayers, m_textureSize, m_screenAspectRatio, BackgroundColor, BackgroundAlpha);
				m_lastRenderLayers.Clear();
				m_lastRenderLayers.AddRange(m_textAndImageLayers);
			}
			ChangeRenderTexture(m_area, GetRenderTextureName(), isForced);
			SetFailedToRenderTexture(m_area, failed: false);
			if (m_area == m_lastAreaIndex)
			{
				m_block.SlimBlock.RecolorTextureUpdateFlags &= ~MySlimBlock.MyRecolorTextureUpdateFlags.TextAndImage;
			}
			return true;
		}

		private bool NeedsUpdate(IMyTextSurfaceScript script)
		{
			int num = int.MaxValue;
			num = script.NeedsUpdate switch
			{
				ScriptUpdate.Update10 => 10, 
				ScriptUpdate.Update100 => 100, 
				ScriptUpdate.Update1000 => 1000, 
				ScriptUpdate.Update10000 => 10000, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
			int num2 = MySession.Static.GameplayFrameCounter + m_randomOffset / (10000 / num);
			if (m_lastUpdate == 0)
			{
				m_lastUpdate = num2;
				return true;
			}
			bool num3 = num2 >= m_lastUpdate + num;
			if (num3)
			{
				m_lastUpdate = num2;
			}
			return num3;
		}

		private void m_block_IsWorkingChanged(MyCubeBlock obj)
		{
			UpdateIsStaticContent();
		}

		public void GetScripts(List<string> scripts)
		{
			if (scripts != null && MyTextSurfaceScriptFactory.Instance != null)
			{
				scripts.AddRange(MyTextSurfaceScriptFactory.Instance.Scripts.Keys);
			}
		}

		public void SelectScriptToDraw(string id)
		{
			if (m_script != null && id == m_previousScript)
			{
				return;
			}
			if (m_script != null)
			{
				m_script.Dispose();
			}
			m_script = null;
			m_lastUpdate = 0;
			if (Sync.IsDedicated)
			{
				return;
			}
			m_renderLayers.Clear();
			if (!string.IsNullOrEmpty(id))
			{
				m_script = MyTextSurfaceScriptFactory.CreateScript(id, this, m_block, m_textureSize);
			}
			if (m_script == null)
			{
				Script = string.Empty;
				using MySpriteDrawFrame mySpriteDrawFrame = DrawFrame();
				MySprite dEFAULT_BACKGROUND = MyTextSurfaceHelper.DEFAULT_BACKGROUND;
				dEFAULT_BACKGROUND.Color = Color.White;
				mySpriteDrawFrame.Add(dEFAULT_BACKGROUND);
			}
			else
			{
				if (!CustomizeScripts)
				{
					ScriptData scriptData = new ScriptData(id, customizeScript: true, m_script.BackgroundColor, m_script.ForegroundColor);
					SetScriptData(scriptData);
				}
				m_script.Run();
			}
			m_previousScript = id;
		}

		private void SetScriptData(ScriptData scriptData)
		{
			bool flag = m_block.HasPlayerAccess(MySession.Static.LocalPlayerId);
			if (Sync.IsServer || (MySession.Static.LocalCharacter != null && flag))
			{
				m_scriptData.Value = scriptData;
			}
			else
			{
				m_scriptData.SetLocalValue(scriptData);
			}
		}

		public void AddImagesToSelection()
		{
			if (m_selectedTexturesToAdd == null || m_selectedTexturesToAdd.Count == 0)
			{
				return;
			}
			int[] array = new int[m_selectedTexturesToAdd.Count];
			for (int i = 0; i < m_selectedTexturesToAdd.Count; i++)
			{
				for (int j = 0; j < m_definitions.Count; j++)
				{
					if ((string)m_selectedTexturesToAdd[i].UserData == m_definitions[j].Id.SubtypeName)
					{
						array[i] = j;
						break;
					}
				}
			}
			if (m_addImagesToSelectionRequest != null)
			{
				m_addImagesToSelectionRequest(this, array);
			}
		}

		public void RemoveImagesFromSelection()
		{
			if (m_selectedTexturesToRemove == null || m_selectedTexturesToRemove.Count == 0)
			{
				return;
			}
			m_previousTextureID = null;
			int[] array = new int[m_selectedTexturesToRemove.Count];
			for (int i = 0; i < m_selectedTexturesToRemove.Count; i++)
			{
				for (int j = 0; j < m_definitions.Count; j++)
				{
					if ((string)m_selectedTexturesToRemove[i].UserData == m_definitions[j].Id.SubtypeName)
					{
						array[i] = j;
						break;
					}
				}
			}
			if (m_removeImagesFromSelectionRequest != null)
			{
				m_removeImagesFromSelectionRequest(this, array);
			}
		}

		public void SelectItems(int[] selection)
		{
			for (int i = 0; i < selection.Length; i++)
			{
				if (selection[i] < m_definitions.Count)
				{
					m_selectedTexturesToDraw.Add(m_definitions[selection[i]]);
				}
			}
			m_currentSelectedTexture = 0;
			m_block.RaisePropertiesChanged();
			m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			UpdateIsStaticContent();
		}

		public void RemoveItems(int[] selection)
		{
			for (int i = 0; i < selection.Length; i++)
			{
				if (selection[i] < m_definitions.Count)
				{
					m_selectedTexturesToDraw.Remove(m_definitions[selection[i]]);
				}
			}
			m_currentSelectedTexture = 0;
			_ = m_selectedTexturesToDraw.Count;
			m_block.RaisePropertiesChanged();
			m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			UpdateIsStaticContent();
		}

		public void UpdateSpriteCollection(MySerializableSpriteCollection sprites)
		{
			m_externalSprites = sprites;
			if (!Sync.IsDedicated)
			{
				m_externalSprites_ValueChanged(null);
			}
		}

		public void SetRender(MyRenderComponentScreenAreas render)
		{
			if (m_render != null && render == null)
			{
				ReleaseTexture();
			}
			m_render = render;
		}

		public void ReleaseTexture(bool useEmptyTexture = true)
		{
			if (m_render != null && m_textureGenerated)
			{
				m_render.ReleaseTexture(m_area, useEmptyTexture);
				m_textureGenerated = false;
				m_lastRenderLayers.Clear();
			}
		}

		public void SetRenderObjectIndex(int renderObjectIndex)
		{
			m_renderObjectIndex = renderObjectIndex;
		}

		public void RefreshRenderText(int freeResources = int.MaxValue)
		{
		}

		public void Reset(bool setOfflineTexture = false)
		{
			m_previousTextureID = string.Empty;
			m_previousScript = string.Empty;
			m_textureGenerated = false;
			if (m_script != null)
			{
				m_script.Dispose();
				m_script = null;
			}
			if (setOfflineTexture)
			{
				SetDefaultTexture(isOnline: false);
			}
		}

		public MySpriteDrawFrame DrawFrame()
		{
			return new MySpriteDrawFrame(DispatchSprites);
		}

		private void DispatchSprites(MySpriteDrawFrame drawFrame)
		{
			m_renderLayers.Clear();
			drawFrame.AddToList(m_renderLayers);
			if (Sync.IsServer)
			{
				if (Script != string.Empty)
				{
					m_spriteQueue = default(MySpriteCollection);
					return;
				}
				m_spriteQueue = drawFrame.ToCollection();
				m_areSpritesDirty = true;
			}
		}

		public string GetRenderTextureName()
		{
			return m_render.GenerateOffscreenTextureName(m_block.EntityId, m_area);
		}

		public string GetPathForID(string id)
		{
			return MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(id)?.TexturePath;
		}

		private static Vector2I GetTextureResolutionForAspectRatio(int width, int height, int textureSize)
		{
			if (width == height)
			{
				return new Vector2I(textureSize, textureSize);
			}
			if (width > height)
			{
				int num = MathHelper.Pow2(MathHelper.Floor(MathHelper.Log2(width / height)));
				return new Vector2I(textureSize * num, textureSize);
			}
			int num2 = MathHelper.Pow2(MathHelper.Floor(MathHelper.Log2(height / width)));
			return new Vector2I(textureSize, textureSize * num2);
		}

		private static bool AreEqual(List<MySprite> lhs, List<MySprite> rhs)
		{
			if (lhs.Count == 0 && rhs.Count == 0)
			{
				return true;
			}
			if (lhs.Count != rhs.Count)
			{
				return false;
			}
			for (int i = 0; i < lhs.Count; i++)
			{
				if (!lhs[i].Equals(rhs[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			return $"{Name} area:{m_area}";
		}

		private void m_contentData_ValueChanged(SyncBase obj)
		{
			m_block.RaisePropertiesChanged();
			m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (m_backgroundColorLast != m_contentData.Value.BackgroundColor)
			{
				m_backgroundChanged = true;
				m_backgroundColorLast = m_contentData.Value.BackgroundColor;
			}
			UpdateIsStaticContent();
		}

		private void m_fontData_ValueChanged(SyncBase obj)
		{
			m_block.RaisePropertiesChanged();
			m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void m_scriptData_ValueChanged(SyncBase obj)
		{
			m_block.RaisePropertiesChanged();
			SelectScriptToDraw(Script);
		}

		private void m_externalSprites_ValueChanged(SyncBase obj)
		{
			if (m_externalSprites.Sprites.IsNullOrEmpty() || (m_script != null && Script != string.Empty))
			{
				return;
			}
			int length = m_externalSprites.Length;
			int num = m_renderLayers.Count - length;
			if (num > 0)
			{
				while (num-- > 0)
				{
					m_renderLayers.RemoveAt(m_renderLayers.Count - 1);
				}
			}
			else if (num < 0)
			{
				while (num++ < 0)
				{
					m_renderLayers.Add(default(MySerializableSprite));
				}
			}
			MySerializableSprite[] sprites = m_externalSprites.Sprites;
			for (int i = 0; i < sprites.Length; i++)
			{
				MySerializableSprite mySerializableSprite = sprites[i];
				m_renderLayers[mySerializableSprite.Index] = mySerializableSprite;
			}
		}

		public void UpdateModApiText()
		{
			if (m_textHelperDirty)
			{
				m_textHelperDirty = false;
				if (Text != m_textHelper)
				{
					m_changeTextRequest?.Invoke(this, m_textHelper.ToString());
				}
				UpdateIsStaticContent();
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(string value, bool append)
		{
			if (!append)
			{
				m_textHelper.Clear();
			}
			if (value.Length + m_textHelper.Length > 100000)
			{
				value = value.Remove(100000 - m_textHelper.Length);
			}
			m_textHelper.Append(value);
			m_textHelperDirty = true;
			return true;
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.GetText()
		{
			return m_textHelper.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(StringBuilder value, bool append)
		{
			if (!append)
			{
				m_textHelper.Clear();
			}
			int num = value.Length + m_textHelper.Length - 100000;
			if (num > 0)
			{
				m_textHelper.AppendSubstring(value, 0, num);
			}
			else
			{
				m_textHelper.Append((object)value);
			}
			m_textHelperDirty = true;
			return true;
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.ReadText(StringBuilder buffer, bool append)
		{
			if (!append)
			{
				buffer.Clear();
			}
			buffer.Append((object)Text);
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImageToSelection(string id, bool checkExistence)
		{
			if (id == null)
			{
				return;
			}
			for (int i = 0; i < Definitions.Count; i++)
			{
				if (!(Definitions[i].Id.SubtypeName == id))
				{
					continue;
				}
				if (checkExistence)
				{
					for (int j = 0; j < SelectedTexturesToDraw.Count; j++)
					{
						if (SelectedTexturesToDraw[j].Id.SubtypeName == id)
						{
							return;
						}
					}
				}
				if (m_addImagesToSelectionRequest != null)
				{
					m_addImagesToSelectionRequest(this, new int[1] { i });
				}
				break;
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImagesToSelection(List<string> ids, bool checkExistence)
		{
			if (ids == null)
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (string id in ids)
			{
				for (int i = 0; i < Definitions.Count; i++)
				{
					if (!(Definitions[i].Id.SubtypeName == id))
<<<<<<< HEAD
					{
						continue;
					}
					bool flag = false;
					if (checkExistence)
					{
=======
					{
						continue;
					}
					bool flag = false;
					if (checkExistence)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						for (int j = 0; j < SelectedTexturesToDraw.Count; j++)
						{
							if (SelectedTexturesToDraw[j].Id.SubtypeName == id)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						list.Add(i);
					}
					break;
				}
<<<<<<< HEAD
			}
			if (list.Count > 0 && m_addImagesToSelectionRequest != null)
			{
				m_addImagesToSelectionRequest(this, list.ToArray());
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (list.Count > 0 && m_addImagesToSelectionRequest != null)
			{
				m_addImagesToSelectionRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImageFromSelection(string id, bool removeDuplicates)
		{
			if (id == null)
			{
				return;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < Definitions.Count; i++)
			{
				if (!(Definitions[i].Id.SubtypeName == id))
				{
					continue;
				}
				if (removeDuplicates)
				{
					for (int j = 0; j < SelectedTexturesToDraw.Count; j++)
					{
						if (SelectedTexturesToDraw[j].Id.SubtypeName == id)
						{
							list.Add(i);
						}
					}
				}
				else
				{
					list.Add(i);
				}
				break;
			}
			if (list.Count > 0 && m_removeImagesFromSelectionRequest != null)
			{
				m_removeImagesFromSelectionRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImagesFromSelection(List<string> ids, bool removeDuplicates)
		{
			if (ids == null)
<<<<<<< HEAD
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (string id in ids)
			{
				for (int i = 0; i < Definitions.Count; i++)
				{
					if (!(Definitions[i].Id.SubtypeName == id))
					{
						continue;
					}
					if (removeDuplicates)
					{
=======
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (string id in ids)
			{
				for (int i = 0; i < Definitions.Count; i++)
				{
					if (!(Definitions[i].Id.SubtypeName == id))
					{
						continue;
					}
					if (removeDuplicates)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						for (int j = 0; j < SelectedTexturesToDraw.Count; j++)
						{
							if (SelectedTexturesToDraw[j].Id.SubtypeName == id)
							{
								list.Add(i);
							}
						}
					}
					else
					{
						list.Add(i);
					}
					break;
				}
			}
			if (list.Count > 0 && m_removeImagesFromSelectionRequest != null)
			{
				m_removeImagesFromSelectionRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.ClearImagesFromSelection()
		{
			if (SelectedTexturesToDraw.Count == 0)
			{
				return;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < SelectedTexturesToDraw.Count; i++)
			{
				for (int j = 0; j < Definitions.Count; j++)
				{
					if (Definitions[j].Id.SubtypeName == SelectedTexturesToDraw[i].Id.SubtypeName)
					{
						list.Add(j);
						break;
					}
				}
			}
			if (list.Count > 0 && m_removeImagesFromSelectionRequest != null)
			{
				m_removeImagesFromSelectionRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetSelectedImages(List<string> output)
		{
			foreach (MyLCDTextureDefinition item in SelectedTexturesToDraw)
			{
				output.Add(item.Id.SubtypeName);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetFonts(List<string> fonts)
		{
			if (fonts == null)
			{
				return;
			}
			foreach (MyFontDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyFontDefinition>())
			{
				fonts.Add(definition.Id.SubtypeName);
			}
		}

		public void GetSprites(List<string> sprites)
		{
			foreach (MyLCDTextureDefinition definition in m_definitions)
			{
				sprites.Add(definition.Id.SubtypeName);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetScripts(List<string> scripts)
		{
			GetScripts(scripts);
		}

		MySpriteDrawFrame Sandbox.ModAPI.Ingame.IMyTextSurface.DrawFrame()
		{
			return DrawFrame();
		}

		public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
		{
			return MyGuiManager.MeasureStringRaw(font, text, scale);
		}
	}
}
