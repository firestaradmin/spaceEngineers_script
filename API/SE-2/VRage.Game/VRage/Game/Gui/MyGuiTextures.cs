using System.Collections.Generic;
using System.IO;
using ParallelTasks;
using VRage.FileSystem;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game.GUI
{
	public class MyGuiTextures
	{
		private readonly Dictionary<MyStringHash, MyObjectBuilder_GuiTexture> m_textures = new Dictionary<MyStringHash, MyObjectBuilder_GuiTexture>();

		private readonly Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture> m_compositeTextures = new Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture>();

		private List<string> m_texturesToUnload;

		private static MyGuiTextures m_instance;

		public static MyGuiTextures Static => m_instance ?? (m_instance = new MyGuiTextures());

		public void Reload(IEnumerable<string> texturesToPreload = null, bool preload = true)
		{
			m_textures.Clear();
			m_compositeTextures.Clear();
			if (MyRenderProxy.RenderThread == null)
			{
				return;
			}
			IEnumerable<MyGuiTextureAtlasDefinition> allDefinitions = MyDefinitionManagerBase.Static.GetAllDefinitions<MyGuiTextureAtlasDefinition>();
			if (texturesToPreload != null && preload)
			{
				MyRenderProxy.PreloadTextures(texturesToPreload, TextureType.GUI);
			}
			List<string> list = new List<string>();
			if (allDefinitions != null)
			{
				foreach (MyGuiTextureAtlasDefinition item in allDefinitions)
				{
					foreach (KeyValuePair<MyStringHash, MyObjectBuilder_GuiTexture> texture in item.Textures)
					{
						m_textures[texture.Key] = texture.Value;
						list.Add(texture.Value.Path);
					}
					foreach (KeyValuePair<MyStringHash, MyObjectBuilder_CompositeTexture> compositeTexture in item.CompositeTextures)
					{
						m_compositeTextures[compositeTexture.Key] = compositeTexture.Value;
					}
				}
			}
			string path = Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons");
			IEnumerable<string> files = MyFileSystem.GetFiles(path, "*", MySearchOption.TopDirectoryOnly);
			list.AddRange(files);
			if (preload)
			{
				MyRenderProxy.PreloadTextures(list, TextureType.GUI);
			}
			list.Clear();
			List<string> deferredTexturesToLoad = new List<string>();
			Parallel.Start(delegate
			{
				string path4 = Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons\\cubes");
				files = MyFileSystem.GetFiles(path4, "*", MySearchOption.TopDirectoryOnly);
				deferredTexturesToLoad.AddRange(files);
				string path5 = Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons\\component");
				files = MyFileSystem.GetFiles(path5, "*", MySearchOption.TopDirectoryOnly);
				deferredTexturesToLoad.AddRange(files);
			}, delegate
			{
				if (preload)
				{
					MyRenderProxy.PreloadTextures(deferredTexturesToLoad, TextureType.GUI);
				}
			});
			string path2 = Path.Combine(MyFileSystem.ContentPath, "customworlds");
			files = MyFileSystem.GetFiles(path2, "*.jpg", MySearchOption.AllDirectories);
			list.AddRange(files);
			string path3 = Path.Combine(MyFileSystem.ContentPath, "scenarios");
			files = MyFileSystem.GetFiles(path3, "*.png", MySearchOption.AllDirectories);
			list.AddRange(files);
			if (preload)
			{
				MyRenderProxy.PreloadTextures(list, TextureType.GUIWithoutPremultiplyAlpha);
			}
			m_texturesToUnload = list;
		}

		public void Unload()
		{
			MyRenderProxy.DeprioritizeTextures(m_texturesToUnload);
		}

		public MyObjectBuilder_GuiTexture GetTexture(MyStringHash hash)
		{
			MyObjectBuilder_GuiTexture value = null;
			m_textures.TryGetValue(hash, out value);
			return value;
		}

		public MyObjectBuilder_CompositeTexture GetCompositeTexture(MyStringHash hash)
		{
			MyObjectBuilder_CompositeTexture value = null;
			m_compositeTextures.TryGetValue(hash, out value);
			return value;
		}

		public bool TryGetTexture(MyStringHash hash, out MyObjectBuilder_GuiTexture texture)
		{
			return m_textures.TryGetValue(hash, out texture);
		}

		public bool TryGetCompositeTexture(MyStringHash hash, out MyObjectBuilder_CompositeTexture texture)
		{
			return m_compositeTextures.TryGetValue(hash, out texture);
		}
	}
}
