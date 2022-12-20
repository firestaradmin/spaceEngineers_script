using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using VRage.FileSystem;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;
using VRage.Utils;
using VRageMath;

namespace VRage.Render11.Resources
{
	internal class MyTextureAtlas : IDisposable
	{
		internal struct Element
		{
			internal Vector4 UvOffsetScale;

			internal IMyStreamedTexture Texture;

			internal MyStreamedTexturePin TexturePin;
		}

		private Dictionary<string, Element> m_elements;

		internal MyTextureAtlas(string textureDir, string atlasFile)
		{
			ParseAtlasDescription(textureDir, atlasFile, out m_elements);
		}

		internal Element FindElement(string id)
		{
			return m_elements[id];
		}

		private static void ParseAtlasDescription(string textureDir, string atlasFile, out Dictionary<string, Element> atlasDict)
		{
			atlasDict = new Dictionary<string, Element>();
			MyTextureStreamingManager textures = MyManagers.Textures;
			try
			{
<<<<<<< HEAD
				using (Stream stream = MyFileSystem.OpenRead(Path.Combine(MyFileSystem.ContentPath, atlasFile)))
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						while (!streamReader.EndOfStream)
						{
							string text = streamReader.ReadLine();
							if (!text.StartsWith("#") && text.Trim(new char[1] { ' ' }).Length != 0)
							{
								string[] array = text.Split(new char[3] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
								string path = array[0];
								string text2 = array[1];
								Vector4 uvOffsetScale = new Vector4(Convert.ToSingle(array[4], CultureInfo.InvariantCulture), Convert.ToSingle(array[5], CultureInfo.InvariantCulture), Convert.ToSingle(array[7], CultureInfo.InvariantCulture), Convert.ToSingle(array[8], CultureInfo.InvariantCulture));
								path = textureDir + Path.GetFileName(path);
								string name = textureDir + text2;
								IMyStreamedTexture texture = textures.GetTexture(name, new MyTextureStreamingManager.QueryArgs
								{
									TextureType = MyFileTextureEnum.GUI,
									WaitUntilLoaded = true
								});
								if (atlasDict.TryGetValue(path, out var value))
								{
									value.TexturePin.Dispose();
								}
								atlasDict[path] = new Element
								{
									UvOffsetScale = uvOffsetScale,
									Texture = texture
								};
							}
						}
=======
				using Stream stream = MyFileSystem.OpenRead(Path.Combine(MyFileSystem.ContentPath, atlasFile));
				using StreamReader streamReader = new StreamReader(stream);
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					if (!text.StartsWith("#") && text.Trim(new char[1] { ' ' }).Length != 0)
					{
						string[] array = text.Split(new char[3] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
						string path = array[0];
						string text2 = array[1];
						Vector4 uvOffsetScale = new Vector4(Convert.ToSingle(array[4], CultureInfo.InvariantCulture), Convert.ToSingle(array[5], CultureInfo.InvariantCulture), Convert.ToSingle(array[7], CultureInfo.InvariantCulture), Convert.ToSingle(array[8], CultureInfo.InvariantCulture));
						path = textureDir + Path.GetFileName(path);
						string name = textureDir + text2;
						IMyStreamedTexture texture = textures.GetTexture(name, new MyTextureStreamingManager.QueryArgs
						{
							TextureType = MyFileTextureEnum.GUI,
							WaitUntilLoaded = true
						});
						if (atlasDict.TryGetValue(path, out var value))
						{
							value.TexturePin.Dispose();
						}
						atlasDict[path] = new Element
						{
							UvOffsetScale = uvOffsetScale,
							Texture = texture
						};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Warning: " + ex.ToString());
			}
		}

		public void LoadTextures()
		{
<<<<<<< HEAD
			string[] array = m_elements.Keys.ToArray();
=======
			string[] array = Enumerable.ToArray<string>((IEnumerable<string>)m_elements.Keys);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (string key in array)
			{
				Element value = m_elements[key];
				if (value.TexturePin.IsEmpty)
				{
					value.TexturePin = value.Texture.Pin();
					m_elements[key] = value;
				}
			}
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, Element> element2 in m_elements)
			{
				LinqExtensions.Deconstruct(element2, out var _, out var v);
				Element element = v;
				MyStreamedTexturePin texturePin = element.TexturePin;
				texturePin.Dispose();
			}
			m_elements.Clear();
		}
	}
}
