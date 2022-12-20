using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Utils;
using VRage.Input;
using VRage.Plugins;
using VRage.UserInterface;
using VRage.UserInterface.Media;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Graphics
{
	public static class MyGuiManager
	{
		public class MyGuiTextureScreen
		{
			private string m_filename;

			private float m_aspectRatio;

			private MyGuiTextureScreen()
			{
			}

			public MyGuiTextureScreen(string filename, int width, int height)
			{
				m_filename = filename;
				m_aspectRatio = (float)width / (float)height;
			}

			public string GetFilename()
			{
				return m_filename;
			}

			public float GetAspectRatio()
			{
				return m_aspectRatio;
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct SpriteScissorToken : IDisposable
		{
			public void Dispose()
			{
				MyRenderProxy.SpriteScissorPop();
			}
		}

		private const float MAXIMUM_UI_RATIO = 1.2f;

		private const float DEFAULT_UI_DPI = 96f;

		private static Rectangle? m_nullRectangle;

		public static int TotalTimeInMilliseconds;

		public const int FAREST_TIME_IN_PAST = -60000;

		public static readonly float MIN_TEXT_SCALE;

		private static MyCamera m_camera;

		private static Rectangle m_safeGuiRectangle;

		private static Rectangle m_safeFullscreenRectangle;

		private static float m_safeScreenScale;

		private static Rectangle m_fullscreenRectangle;

		private static bool m_debugScreensEnabled;

		private static Vector2 m_hudSize;

		private static Vector2 m_hudSizeHalf;

		private static Vector2 m_minMouseCoord;

		private static Vector2 m_maxMouseCoord;

		private static Vector2 m_minMouseCoordFullscreenHud;

		private static Vector2 m_maxMouseCoordFullscreenHud;

		private static string m_mouseCursorTexture;

		private static List<MyGuiTextureScreen> m_backgroundScreenTextures;

		private static bool m_fullScreenHudEnabled;

		private static MyLanguagesEnum m_currentLanguage;

		private static Vector2 m_mouseCursorPosition;

		public static MatrixD Camera => m_camera.WorldMatrix;

		public static MatrixD CameraView => m_camera.ViewMatrix;

		public static bool FullscreenHudEnabled
		{
			get
			{
				return m_fullScreenHudEnabled;
			}
			set
			{
				m_fullScreenHudEnabled = value;
			}
		}

		public static float UIScale { get; set; }

		public static MyLanguagesEnum CurrentLanguage
		{
			get
			{
				return m_currentLanguage;
			}
			set
			{
				if (m_currentLanguage != value)
				{
					m_currentLanguage = value;
					InitFonts();
				}
			}
		}

		public static Vector2 MouseCursorPosition
		{
			get
			{
				if (!MyInput.Static.IsJoystickLastUsed)
				{
					return m_mouseCursorPosition;
				}
				return Vector2.Zero;
			}
			set
			{
				m_mouseCursorPosition = value;
			}
		}

		public static float LanguageTextScale { get; set; }

		public static void SetCamera(MyCamera camera)
		{
			m_camera = camera;
		}

		public static bool IsDebugScreenEnabled()
		{
			return m_debugScreensEnabled;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object" /> class.
		/// </summary>
		static MyGuiManager()
		{
<<<<<<< HEAD
			m_nullRectangle = null;
=======
			vector2Zero = Vector2.Zero;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MIN_TEXT_SCALE = 0.7f;
			m_safeFullscreenRectangle = new Rectangle(0, 0, 640, 480);
			m_debugScreensEnabled = true;
			m_hudSize = new Vector2(1f, 0.8f);
			m_fullScreenHudEnabled = false;
			MyGuiControlsFactory.RegisterDescriptorsFromAssembly(typeof(MyGuiManager).Assembly);
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		public static void LoadData()
		{
			MyGuiControlsFactory.RegisterDescriptorsFromAssembly(MyPlugins.SandboxGameAssembly);
		}

		public static void LoadContent()
		{
<<<<<<< HEAD
			MyDebug.AssertRelease(MyRenderProxy.Log != null, "MyGuiManager.LoadContent() - MyRenderProxy.Log is NULL", "E:\\Repo1\\Sources\\Sandbox.Graphics\\MyGuiManager.cs", 167);
			MyRenderProxy.Log.WriteLine("MyGuiManager2.LoadContent() - START");
			MyRenderProxy.Log.IncreaseIndent();
			string path = Path.Combine(MyFileSystem.ContentPath, Path.Combine("Textures", "GUI", "MouseCursorHW.png"));
			MyDebug.AssertRelease(MyVRage.Platform?.Windows?.Window != null, "MyVRage.Platform.Windows.Window is NULL", "E:\\Repo1\\Sources\\Sandbox.Graphics\\MyGuiManager.cs", 172);
=======
			MyDebug.AssertRelease(MyRenderProxy.Log != null, "MyGuiManager.LoadContent() - MyRenderProxy.Log is NULL", "E:\\Repo3\\Sources\\Sandbox.Graphics\\MyGuiManager.cs", 203);
			MyRenderProxy.Log.WriteLine("MyGuiManager2.LoadContent() - START");
			MyRenderProxy.Log.IncreaseIndent();
			string path = Path.Combine(MyFileSystem.ContentPath, Path.Combine("Textures", "GUI", "MouseCursorHW.png"));
			MyDebug.AssertRelease(MyVRage.Platform?.Windows?.Window != null, "MyVRage.Platform.Windows.Window is NULL", "E:\\Repo3\\Sources\\Sandbox.Graphics\\MyGuiManager.cs", 208);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			using (Stream cursor = MyFileSystem.OpenRead(path))
			{
				MyVRage.Platform.Windows.Window.SetCursor(cursor);
			}
			SetMouseCursorTexture("Textures\\GUI\\MouseCursor.dds");
			m_backgroundScreenTextures = new List<MyGuiTextureScreen>
			{
				new MyGuiTextureScreen(MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture, (int)MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.SizePx.X, (int)MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.SizePx.Y)
			};
			InitFonts();
			MouseCursorPosition = new Vector2(0.5f, 0.5f);
			MyRenderProxy.Log.DecreaseIndent();
			MyRenderProxy.Log.WriteLine("MyGuiManager2.LoadContent() - END");
			ServiceManager.Instance.AddService((ILocalizationService)new MyLocalizationService());
			ServiceManager.Instance.AddService((IClipboardService)new MyClipboardService());
			ToolTipService.InitialShowDelayProperty.DefaultMetadata.DefaultValue = 250;
			ToolTipService.ShowDurationProperty.DefaultMetadata.DefaultValue = 15000;
			DragDrop.Instance.AllowRightMouseButton = true;
			LoadTexturesToImageManager(MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Textures\\GUI\\Icons"), "*", MySearchOption.AllDirectories));
			LoadTexturesToImageManager(MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Textures\\FactionLogo"), "*", MySearchOption.AllDirectories));
			LoadTexturesToImageManager(MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Textures\\GUI\\Prefabs"), "*", MySearchOption.AllDirectories));
		}

		private static void LoadTexturesToImageManager(IEnumerable<string> files)
		{
			foreach (string file in files)
			{
				string text = file;
				if (text.Contains(MyFileSystem.ContentPath))
				{
					text = text.Remove(0, MyFileSystem.ContentPath.Length);
					text = text.TrimStart(new char[1] { Path.DirectorySeparatorChar });
				}
				ImageManager.Instance.AddImage(text);
			}
		}

		private static Vector2 CalculateHudSize()
		{
			return new Vector2(1f, (float)m_safeFullscreenRectangle.Height / (float)m_safeFullscreenRectangle.Width);
		}

		public static float GetSafeScreenScale()
		{
			return m_safeScreenScale;
		}

		public static Rectangle GetSafeGuiRectangle()
		{
			return m_safeGuiRectangle;
		}

		public static Rectangle GetSafeFullscreenRectangle()
		{
			return m_safeFullscreenRectangle;
		}

		public static Rectangle GetFullscreenRectangle()
		{
			return m_fullscreenRectangle;
		}

		/// <summary>
		/// Computes aligned coordinate for screens without size (Size == null) with optional pixel offset from given origin.
		/// </summary>
		public static Vector2 ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum align, int pixelOffsetX = 54, int pixelOffsetY = 54)
		{
			float x = (float)pixelOffsetX * m_safeScreenScale;
			float y = (float)pixelOffsetY * m_safeScreenScale;
			Vector2 normalizedCoordinateFromScreenCoordinate_FULLSCREEN = GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(x, y));
<<<<<<< HEAD
			switch (align)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return normalizedCoordinateFromScreenCoordinate_FULLSCREEN;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return new Vector2(normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return new Vector2(normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return new Vector2(0.5f, normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return new Vector2(0.5f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return new Vector2(0.5f, 1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return new Vector2(1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return new Vector2(1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return new Vector2(1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y);
			default:
				return normalizedCoordinateFromScreenCoordinate_FULLSCREEN;
			}
=======
			return align switch
			{
				MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP => normalizedCoordinateFromScreenCoordinate_FULLSCREEN, 
				MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER => new Vector2(normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 0.5f), 
				MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM => new Vector2(normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y), 
				MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP => new Vector2(0.5f, normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y), 
				MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER => new Vector2(0.5f, 0.5f), 
				MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM => new Vector2(0.5f, 1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y), 
				MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP => new Vector2(1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y), 
				MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER => new Vector2(1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 0.5f), 
				MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM => new Vector2(1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.X, 1f - normalizedCoordinateFromScreenCoordinate_FULLSCREEN.Y), 
				_ => normalizedCoordinateFromScreenCoordinate_FULLSCREEN, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static float GetPixelsFromFloat(float size)
		{
			return GetScreenSizeFromNormalizedSize(new Vector2(1f, 1f)).X * size;
		}

		public static void DrawString(string font, string text, Vector2 normalizedCoord, float scale, Color? colorMask = null, MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, bool useFullClientArea = false, float maxTextWidth = float.PositiveInfinity, bool ignoreBounds = false)
		{
			Vector2 normalizedCoord2;
			if (drawAlign != 0)
			{
				Vector2 size = MeasureString(font, text, scale);
				size.X = Math.Min(maxTextWidth, size.X);
				normalizedCoord2 = MyUtils.GetCoordTopLeftFromAligned(normalizedCoord, size, drawAlign);
			}
			else
			{
				normalizedCoord2 = normalizedCoord;
			}
			Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(normalizedCoord2, useFullClientArea);
			float screenScale = scale * m_safeScreenScale;
			float x = GetScreenSizeFromNormalizedSize(new Vector2(maxTextWidth, 0f)).X;
			if (screenCoordinateFromNormalizedCoordinate.X.IsValid() && screenCoordinateFromNormalizedCoordinate.Y.IsValid())
			{
				screenCoordinateFromNormalizedCoordinate.X = (int)Math.Ceiling(screenCoordinateFromNormalizedCoordinate.X);
				screenCoordinateFromNormalizedCoordinate.Y = (int)Math.Ceiling(screenCoordinateFromNormalizedCoordinate.Y);
				MyRenderProxy.DrawString((int)MyStringHash.Get(font), screenCoordinateFromNormalizedCoordinate, colorMask ?? new Color(MyGuiConstants.LABEL_TEXT_COLOR), text, screenScale, x, ignoreBounds);
			}
		}

		[Obsolete("Allocates memory. Use with string param variant", false)]
		public static void DrawString(string font, StringBuilder text, Vector2 normalizedCoord, float scale, Color? colorMask = null, MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, bool useFullClientArea = false, float maxTextWidth = float.PositiveInfinity, bool ignoreBounds = false)
		{
			DrawString(font, text.ToString(), normalizedCoord, scale, colorMask, drawAlign, useFullClientArea, maxTextWidth, ignoreBounds);
		}

		public static void InitFonts()
		{
			InitFonts(MyDefinitionManagerBase.Static.GetDefinitions<MyFontDefinition>());
			FontManager.DefaultFont = new VRage.UserInterface.Media.MyFont(GetFont(MyStringHash.GetOrCompute("White")), (int)MyStringHash.Get("White"), 0.8f);
			FontManager.Instance.ClearCache();
			string text = "InventorySmall";
			VRage.UserInterface.Media.MyFont value = new VRage.UserInterface.Media.MyFont(GetFont(MyStringHash.GetOrCompute("White")), (int)MyStringHash.Get("White"), 0.6f);
			FontManager.Instance.AddFont(text, 10f, FontStyle.Regular, text);
			string text2 = "LargeFont";
			VRage.UserInterface.Media.MyFont value2 = new VRage.UserInterface.Media.MyFont(GetFont(MyStringHash.GetOrCompute("White")), (int)MyStringHash.Get("White"), 1f);
			FontManager.Instance.AddFont(text2, 16.6f, FontStyle.Regular, text2);
			MyAssetManager obj = EmptyKeys.UserInterface.Engine.Instance.AssetManager as MyAssetManager;
			obj.Fonts.Clear();
			obj.Fonts.Add(text, value);
			obj.Fonts.Add(text2, value2);
			FontManager.Instance.LoadFonts(null);
		}

		public static void InitFonts(IEnumerable<MyFontDefinition> fontDefinitions)
		{
			fontDefinitions.ForEach(InitFont);
		}

		public static void InitFont(MyFontDefinition fontDefinition)
		{
			fontDefinition.UseLanguage(m_currentLanguage.ToString());
			if (!string.IsNullOrEmpty(fontDefinition.CompatibilityPath))
			{
				MyRenderProxy.CreateFont((int)fontDefinition.Id.SubtypeId, fontDefinition.CompatibilityPath, fontDefinition.Default, null, fontDefinition.ColorMask);
				return;
			}
			MyObjectBuilder_FontData myObjectBuilder_FontData = Enumerable.FirstOrDefault<MyObjectBuilder_FontData>(fontDefinition.Resources);
			MyRenderProxy.CreateFont((int)fontDefinition.Id.SubtypeId, myObjectBuilder_FontData.Path, fontDefinition.Default, null, fontDefinition.ColorMask);
		}

		public static Vector2 MeasureStringRaw(string font, string text, float scale)
		{
			return MyFontDefinition.MeasureStringRaw(font, text, scale);
		}

		public static Vector2 MeasureStringRaw(string font, StringBuilder text, float scale)
		{
			return MyFontDefinition.MeasureStringRaw(font, text, scale);
		}

		public static Vector2 MeasureString(string font, StringBuilder text, float scale)
		{
			float scale2 = scale * m_safeScreenScale;
			return GetNormalizedSizeFromScreenSize(MyFontDefinition.MeasureStringRaw(font, text, scale2));
		}

		public static Vector2 MeasureString(string font, string text, float scale)
		{
			float scale2 = scale * m_safeScreenScale;
			return GetNormalizedSizeFromScreenSize(MyFontDefinition.MeasureStringRaw(font, text, scale2));
		}

		public static float MeasureScaleNeeded(string font, string text, float width)
		{
<<<<<<< HEAD
			float x = MyFontDefinition.MeasureStringRaw(font, text, 1f).X;
			return GetScreenSizeFromNormalizedSize(new Vector2(1f, 1f)).X * width / x / m_safeScreenScale;
=======
			float num = MyFontDefinition.MeasureStringRaw(font, text, 1f).X;
			return GetScreenSizeFromNormalizedSize(new Vector2(1f, 1f)).X * width / num / m_safeScreenScale;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static float MeasureScaleNeeded(string font, StringBuilder text, float width)
		{
<<<<<<< HEAD
			float x = MyFontDefinition.MeasureStringRaw(font, text, 1f).X;
			return GetScreenSizeFromNormalizedSize(new Vector2(1f, 1f)).X * width / x / m_safeScreenScale;
=======
			float num = MyFontDefinition.MeasureStringRaw(font, text, 1f).X;
			return GetScreenSizeFromNormalizedSize(new Vector2(1f, 1f)).X * width / num / m_safeScreenScale;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static int ComputeNumCharsThatFit(string font, StringBuilder text, float scale, float maxTextWidth)
		{
			float scale2 = scale * m_safeScreenScale;
			float x = GetScreenSizeFromNormalizedSize(new Vector2(maxTextWidth, 0f)).X;
			return MyFontDefinition.GetFont(MyStringHash.GetOrCompute(font)).ComputeCharsThatFit(text, scale2, x);
		}

		public static VRageRender.MyFont GetFont(MyStringHash fontId)
		{
			return MyFontDefinition.GetFont(fontId);
		}

		public static float GetFontHeight(string font, float scale)
		{
			float num = scale * m_safeScreenScale * (144f / 185f);
			return GetNormalizedSizeFromScreenSize(new Vector2(0f, num * (float)MyFontDefinition.GetFont(MyStringHash.GetOrCompute(font)).LineHeight)).Y;
		}

		public static void DrawSprite(string texture, Rectangle rectangle, Color color, bool ignoreBounds, bool waitTillLoaded, string maskTexture = null, float rotation = 0f, float rotSpeed = 0f)
		{
			RectangleF destination = new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
<<<<<<< HEAD
			DrawSprite(texture, ref destination, m_nullRectangle, color, rotation, ignoreBounds, waitTillLoaded, maskTexture, rotSpeed);
=======
			DrawSprite(texture, ref destination, nullRectangle, color, rotation, ignoreBounds, waitTillLoaded, maskTexture, rotSpeed);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void DrawSpriteBatch(string texture, int x, int y, int width, int height, Color color, bool waitTillLoaded = true)
		{
			DrawSprite(texture, new Rectangle(x, y, width, height), color, ignoreBounds: false, waitTillLoaded);
		}

		public static void DrawSpriteBatch(string texture, Rectangle destinationRectangle, Color color, bool ignoreBounds, bool waitTillLoaded)
		{
			if (!string.IsNullOrEmpty(texture))
			{
				RectangleF destination = new RectangleF(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height);
<<<<<<< HEAD
				DrawSprite(texture, ref destination, m_nullRectangle, color, 0f, ignoreBounds, waitTillLoaded);
=======
				DrawSprite(texture, ref destination, nullRectangle, color, 0f, ignoreBounds, waitTillLoaded);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static void DrawSpriteBatch(string texture, Vector2 pos, Color color, bool waitTillLoaded = true)
		{
			if (!string.IsNullOrEmpty(texture))
			{
				DrawSprite(texture, pos, color, ignoreBounds: false, waitTillLoaded);
			}
		}

		public static void DrawSprite(string texture, Vector2 position, Color color, bool ignoreBounds, bool waitTillLoaded)
		{
			RectangleF destination = new RectangleF(position.X, position.Y, 1f, 1f);
<<<<<<< HEAD
			DrawSprite(texture, ref destination, m_nullRectangle, color, 0f, ignoreBounds, waitTillLoaded);
=======
			DrawSprite(texture, ref destination, nullRectangle, color, 0f, ignoreBounds, waitTillLoaded);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void DrawSprite(string texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 scale, bool ignoreBounds, bool waitTillLoaded)
		{
			RectangleF destination = new RectangleF(position.X, position.Y, scale.X, scale.Y);
			DrawSprite(texture, ref destination, sourceRectangle, color, rotation, ignoreBounds, waitTillLoaded);
		}

		private static void DrawSprite(string texture, ref RectangleF destination, Rectangle? sourceRectangle, Color color, float rotation, bool ignoreBounds, bool waitTillLoaded, string maskTexture = null, float rotSpeed = 0f)
		{
			MyRenderProxy.DrawSprite(texture, ref destination, sourceRectangle, color, rotation, ignoreBounds, waitTillLoaded, null, maskTexture, rotSpeed);
		}

		public static void DrawSpriteBatch(string texture, Vector2 normalizedCoord, int screenWidth, float normalizedHeight, Color color, MyGuiDrawAlignEnum drawAlign, bool waitTillLoaded = true)
		{
			if (!string.IsNullOrEmpty(texture))
			{
				Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(normalizedCoord);
				Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(new Vector2(0f, normalizedHeight));
				screenSizeFromNormalizedSize.X = screenWidth;
				screenCoordinateFromNormalizedCoordinate = MyUtils.GetCoordAligned(screenCoordinateFromNormalizedCoordinate, screenSizeFromNormalizedSize, drawAlign);
				DrawSprite(texture, new Rectangle((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y, (int)screenSizeFromNormalizedSize.X, (int)screenSizeFromNormalizedSize.Y), color, ignoreBounds: false, waitTillLoaded);
			}
		}

		public static void DrawSpriteBatch(string texture, Vector2 normalizedCoord, float normalizedWidth, int screenHeight, Color color, MyGuiDrawAlignEnum drawAlign, bool waitTillLoaded = true)
		{
			if (!string.IsNullOrEmpty(texture))
			{
				Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(normalizedCoord);
				Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(new Vector2(normalizedWidth, 0f));
				screenSizeFromNormalizedSize.Y = screenHeight;
				screenCoordinateFromNormalizedCoordinate = MyUtils.GetCoordAligned(screenCoordinateFromNormalizedCoordinate, screenSizeFromNormalizedSize, drawAlign);
				DrawSprite(texture, new Rectangle((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y, (int)screenSizeFromNormalizedSize.X, (int)screenSizeFromNormalizedSize.Y), color, ignoreBounds: false, waitTillLoaded);
			}
		}

		public static void DrawSpriteBatch(string texture, Vector2 normalizedCoord, Vector2 normalizedSize, Color color, MyGuiDrawAlignEnum drawAlign, bool useFullClientArea = false, bool waitTillLoaded = true, string maskTexture = null, float rotation = 0f, float rotSpeed = 0f, bool ignoreBounds = false)
		{
			if (!string.IsNullOrEmpty(texture))
			{
				Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(normalizedCoord, useFullClientArea);
				Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(normalizedSize, useFullClientArea);
				screenCoordinateFromNormalizedCoordinate = MyUtils.GetCoordAligned(screenCoordinateFromNormalizedCoordinate, screenSizeFromNormalizedSize, drawAlign);
				Rectangle rectangle = new Rectangle((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y, (int)screenSizeFromNormalizedSize.X, (int)screenSizeFromNormalizedSize.Y);
				DrawSprite(texture, rectangle, color, ignoreBounds, waitTillLoaded, maskTexture, rotation, rotSpeed);
			}
		}

		public static void DrawSpriteBatchRoundUp(string texture, Vector2 normalizedCoord, Vector2 normalizedSize, Color color, MyGuiDrawAlignEnum drawAlign, bool waitTillLoaded = true)
		{
			if (!string.IsNullOrEmpty(texture))
			{
				Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(normalizedCoord);
				Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(normalizedSize);
				screenCoordinateFromNormalizedCoordinate = MyUtils.GetCoordAligned(screenCoordinateFromNormalizedCoordinate, screenSizeFromNormalizedSize, drawAlign);
				DrawSprite(texture, new Rectangle((int)Math.Floor(screenCoordinateFromNormalizedCoordinate.X), (int)Math.Floor(screenCoordinateFromNormalizedCoordinate.Y), (int)Math.Ceiling(screenSizeFromNormalizedSize.X), (int)Math.Ceiling(screenSizeFromNormalizedSize.Y)), color, ignoreBounds: false, waitTillLoaded);
			}
		}

		public static string GetBackgroundTextureFilenameByAspectRatio(Vector2 normalizedSize)
		{
			Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(normalizedSize);
			float num = screenSizeFromNormalizedSize.X / screenSizeFromNormalizedSize.Y;
			float num2 = float.MaxValue;
			string result = null;
			foreach (MyGuiTextureScreen backgroundScreenTexture in m_backgroundScreenTextures)
			{
				float num3 = Math.Abs(num - backgroundScreenTexture.GetAspectRatio());
				if (num3 < num2)
				{
					num2 = num3;
					result = backgroundScreenTexture.GetFilename();
				}
			}
			return result;
		}

		public static Vector2 GetNormalizedSize(Vector2 size, float scale)
		{
			return GetNormalizedSizeFromScreenSize(size * scale * m_safeScreenScale);
		}

		/// <summary>Convertes normalized size [0,1] to screen size (pixels)</summary>
		/// <param name="useFullClientArea">True uses full client rectangle. False limits to GUI rectangle</param>
		public static Vector2 GetScreenSizeFromNormalizedSize(Vector2 normalizedSize, bool useFullClientArea = false)
		{
			if (useFullClientArea)
			{
				return new Vector2((float)(m_safeFullscreenRectangle.Width + 1) * normalizedSize.X, (float)m_safeFullscreenRectangle.Height * normalizedSize.Y);
			}
			return new Vector2((float)(m_safeGuiRectangle.Width + 1) * normalizedSize.X, (float)m_safeGuiRectangle.Height * normalizedSize.Y);
		}

		/// <summary>Convertes normalized coodrinate [0,1] to screen coordinate (pixels)</summary>
		/// <param name="useFullClientArea">True uses full client rectangle. False limits to GUI rectangle</param>
		public static Vector2 GetScreenCoordinateFromNormalizedCoordinate(Vector2 normalizedCoord, bool useFullClientArea = false)
		{
			if (useFullClientArea)
			{
				return new Vector2((float)m_safeFullscreenRectangle.Left + (float)m_safeFullscreenRectangle.Width * normalizedCoord.X, (float)m_safeFullscreenRectangle.Top + (float)m_safeFullscreenRectangle.Height * normalizedCoord.Y);
			}
			return new Vector2((float)m_safeGuiRectangle.Left + (float)m_safeGuiRectangle.Width * normalizedCoord.X, (float)m_safeGuiRectangle.Top + (float)m_safeGuiRectangle.Height * normalizedCoord.Y);
		}

		public static Vector2 GetNormalizedCoordinateFromScreenCoordinate(Vector2 screenCoord)
		{
			return new Vector2((screenCoord.X - (float)m_safeGuiRectangle.Left) / (float)m_safeGuiRectangle.Width, (screenCoord.Y - (float)m_safeGuiRectangle.Top) / (float)m_safeGuiRectangle.Height);
		}

		public static Vector2 GetNormalizedMousePosition(Vector2 mousePosition, Vector2 mouseAreaSize)
		{
			Vector2 screenCoord = default(Vector2);
			screenCoord.X = mousePosition.X * ((float)m_fullscreenRectangle.Width / mouseAreaSize.X);
			screenCoord.Y = mousePosition.Y * ((float)m_fullscreenRectangle.Height / mouseAreaSize.Y);
			return GetNormalizedCoordinateFromScreenCoordinate(screenCoord);
		}

		public static Vector2 GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(Vector2 fullScreenCoord)
		{
			return GetNormalizedCoordinateFromScreenCoordinate(new Vector2((float)m_safeFullscreenRectangle.Left + fullScreenCoord.X, (float)m_safeFullscreenRectangle.Top + fullScreenCoord.Y));
		}

		public static Vector2 GetNormalizedSizeFromScreenSize(Vector2 screenSize)
		{
			float x = ((m_safeGuiRectangle.Width != 0) ? (screenSize.X / (float)m_safeGuiRectangle.Width) : 0f);
			float y = ((m_safeGuiRectangle.Height != 0) ? (screenSize.Y / (float)m_safeGuiRectangle.Height) : 0f);
			return new Vector2(x, y);
		}

		public static Vector2 GetHudNormalizedCoordFromPixelCoord(Vector2 pixelCoord)
		{
			return new Vector2((pixelCoord.X - (float)m_safeFullscreenRectangle.Left) / (float)m_safeFullscreenRectangle.Width, (pixelCoord.Y - (float)m_safeFullscreenRectangle.Top) / (float)m_safeFullscreenRectangle.Height * m_hudSize.Y);
		}

		public static Vector2 GetHudNormalizedSizeFromPixelSize(Vector2 pixelSize)
		{
			return new Vector2(pixelSize.X / (float)m_safeFullscreenRectangle.Width, pixelSize.Y / (float)m_safeFullscreenRectangle.Height * m_hudSize.Y);
		}

		public static Vector2 GetHudPixelCoordFromNormalizedCoord(Vector2 normalizedCoord)
		{
			return new Vector2(normalizedCoord.X * (float)m_safeFullscreenRectangle.Width, normalizedCoord.Y * (float)m_safeFullscreenRectangle.Height);
		}

		public static Vector2 GetMinMouseCoord()
		{
			if (!FullscreenHudEnabled)
			{
				return m_minMouseCoord;
			}
			return m_minMouseCoordFullscreenHud;
		}

		public static Vector2 GetMaxMouseCoord()
		{
			if (!FullscreenHudEnabled)
			{
				return m_maxMouseCoord;
			}
			return m_maxMouseCoordFullscreenHud;
		}

		public static void GetSafeHeightFullScreenPictureSize(Vector2I originalSize, out Rectangle outRect)
		{
			GetSafeHeightPictureSize(originalSize, m_safeFullscreenRectangle, out outRect);
		}

		public static void GetSafeAspectRatioFullScreenPictureSize(Vector2I originalSize, out Rectangle outRect)
		{
			GetSafeAspectRatioPictureSize(originalSize, m_safeFullscreenRectangle, out outRect);
		}

		private static void GetSafeHeightPictureSize(Vector2I originalSize, Rectangle boundingArea, out Rectangle outRect)
		{
			outRect.Height = boundingArea.Height;
			outRect.Width = (int)((float)outRect.Height / (float)originalSize.Y * (float)originalSize.X);
			outRect.X = boundingArea.Left + (boundingArea.Width - outRect.Width) / 2;
			outRect.Y = boundingArea.Top + (boundingArea.Height - outRect.Height) / 2;
		}

		private static void GetSafeAspectRatioPictureSize(Vector2I originalSize, Rectangle boundingArea, out Rectangle outRect)
		{
			outRect.Width = boundingArea.Width;
			outRect.Height = (int)((float)outRect.Width / (float)originalSize.X * (float)originalSize.Y);
			if (outRect.Height > boundingArea.Height)
			{
				outRect.Height = boundingArea.Height;
				outRect.Width = (int)((float)outRect.Height * ((float)originalSize.X / (float)originalSize.Y));
			}
			outRect.X = boundingArea.Left + (boundingArea.Width - outRect.Width) / 2;
			outRect.Y = boundingArea.Top + (boundingArea.Height - outRect.Height) / 2;
		}

		public static Vector2 GetScreenTextRightTopPosition()
		{
			float num = 25f * GetSafeScreenScale();
			return GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float)GetSafeFullscreenRectangle().Width - num, num));
		}

		public static Vector2 GetScreenTextRightBottomPosition()
		{
			float num = 25f * GetSafeScreenScale();
			GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float)GetSafeFullscreenRectangle().Width - num, num));
			return GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float)GetSafeFullscreenRectangle().Width - num, (float)GetSafeFullscreenRectangle().Height - 2f * num));
		}

		public static Vector2 GetScreenTextLeftBottomPosition()
		{
			float num = 25f * GetSafeScreenScale();
			return GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(num, (float)GetSafeFullscreenRectangle().Height - 2f * num));
		}

		public static Vector2 GetScreenTextLeftTopPosition()
		{
			float num = 25f * GetSafeScreenScale();
			return GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(num, num));
		}

		public static bool UpdateScreenSize(Vector2I screenSize, Vector2I screenSizeHalf, bool isTriple)
		{
			bool result = m_fullscreenRectangle != new Rectangle(0, 0, screenSize.X, screenSize.Y);
			int y = screenSize.Y;
			int num = Math.Min(screenSize.X, (int)((float)y * 1.33333337f));
			int num2 = screenSize.X;
			int y2 = screenSize.Y;
			m_fullscreenRectangle = new Rectangle(0, 0, screenSize.X, screenSize.Y);
			if (isTriple)
			{
				num2 /= 3;
			}
			m_safeGuiRectangle = new Rectangle(screenSize.X / 2 - num / 2, 0, num, y);
			m_safeFullscreenRectangle = new Rectangle(screenSize.X / 2 - num2 / 2, 0, num2, y2);
			m_safeScreenScale = (float)y / 1080f;
			VRage.UserInterface.Media.MyFont.GlobalFontScale = GetSafeScreenScale();
			float num3 = Math.Min((float)screenSize.X / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1.2f);
			if (screenSize.Y >= 1440)
			{
				num3 = (float)screenSize.X / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			}
			UIRoot.DpiX = (int)(num3 * 96f);
			UIRoot.DpiY = UIRoot.DpiX;
			m_minMouseCoord = GetNormalizedCoordinateFromScreenCoordinate(new Vector2(m_safeFullscreenRectangle.Left, m_safeFullscreenRectangle.Top));
			m_maxMouseCoord = GetNormalizedCoordinateFromScreenCoordinate(new Vector2(m_safeFullscreenRectangle.Left + m_safeFullscreenRectangle.Width, m_safeFullscreenRectangle.Top + m_safeFullscreenRectangle.Height));
			m_minMouseCoordFullscreenHud = GetNormalizedCoordinateFromScreenCoordinate(new Vector2(m_fullscreenRectangle.Left, m_fullscreenRectangle.Top));
			m_maxMouseCoordFullscreenHud = GetNormalizedCoordinateFromScreenCoordinate(new Vector2(m_fullscreenRectangle.Left + m_fullscreenRectangle.Width, m_fullscreenRectangle.Top + m_fullscreenRectangle.Height));
			m_hudSize = CalculateHudSize();
			m_hudSizeHalf = m_hudSize / 2f;
			return result;
		}

		public static void Update(int totalTimeInMS)
		{
			TotalTimeInMilliseconds = totalTimeInMS;
		}

		public static Vector2 GetHudSize()
		{
			return m_hudSize;
		}

		public static Vector2 GetHudSizeHalf()
		{
			return m_hudSizeHalf;
		}

		public static string GetMouseCursorTexture()
		{
			return m_mouseCursorTexture;
		}

		public static void SetMouseCursorTexture(string texture)
		{
			m_mouseCursorTexture = texture;
		}

		public static void DrawBorders(Vector2 topLeftPosition, Vector2 size, Color color, int borderSize)
		{
			Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(size);
			screenSizeFromNormalizedSize = new Vector2((int)screenSizeFromNormalizedSize.X, (int)screenSizeFromNormalizedSize.Y);
			Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(topLeftPosition);
			screenCoordinateFromNormalizedCoordinate = new Vector2((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y);
			Vector2 vector = screenCoordinateFromNormalizedCoordinate + new Vector2(screenSizeFromNormalizedSize.X, 0f);
			Vector2 vector2 = screenCoordinateFromNormalizedCoordinate + new Vector2(0f, screenSizeFromNormalizedSize.Y);
			DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y, (int)screenSizeFromNormalizedSize.X, borderSize, color);
			DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)vector.X - borderSize, (int)vector.Y + borderSize, borderSize, (int)screenSizeFromNormalizedSize.Y - borderSize * 2, color);
			DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)vector2.X, (int)vector2.Y - borderSize, (int)screenSizeFromNormalizedSize.X, borderSize, color);
			DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y + borderSize, borderSize, (int)screenSizeFromNormalizedSize.Y - borderSize * 2, color);
		}

		public static void DrawRectangle(Vector2 topLeftPosition, Vector2 size, Color color)
		{
			Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(size);
			screenSizeFromNormalizedSize = new Vector2((int)screenSizeFromNormalizedSize.X, (int)screenSizeFromNormalizedSize.Y);
			Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(topLeftPosition);
			screenCoordinateFromNormalizedCoordinate = new Vector2((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y);
			DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y, (int)screenSizeFromNormalizedSize.X, (int)screenSizeFromNormalizedSize.Y, color);
		}

		public static SpriteScissorToken UsingScissorRectangle(ref RectangleF normalizedRectangle)
		{
			Vector2 screenSizeFromNormalizedSize = GetScreenSizeFromNormalizedSize(normalizedRectangle.Size);
			Vector2 screenCoordinateFromNormalizedCoordinate = GetScreenCoordinateFromNormalizedCoordinate(normalizedRectangle.Position);
			MyRenderProxy.SpriteScissorPush(new Rectangle((int)Math.Round(screenCoordinateFromNormalizedCoordinate.X, MidpointRounding.AwayFromZero), (int)Math.Round(screenCoordinateFromNormalizedCoordinate.Y, MidpointRounding.AwayFromZero), (int)Math.Round(screenSizeFromNormalizedSize.X, MidpointRounding.AwayFromZero), (int)Math.Round(screenSizeFromNormalizedSize.Y, MidpointRounding.AwayFromZero)));
			return default(SpriteScissorToken);
		}
	}
}
