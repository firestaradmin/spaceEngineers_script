using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;

namespace VRageRender
{
	internal class MyShaderPass
	{
		internal string m_vertexStageSrc;

		internal string m_pixelStageSrc;

		private static Dictionary<string, MyShaderPass> m_cached = new Dictionary<string, MyShaderPass>();

		internal static void ClearCache()
		{
			m_cached.Clear();
		}

		internal static MyShaderPass GetOrCreate(string tag)
		{
			bool flag = false;
			if (!m_cached.TryGetValue(tag, out var value))
			{
				value = new MyShaderPass();
				flag = true;
				m_cached[tag] = value;
			}
			if (flag)
			{
				using (Stream stream = MyFileSystem.OpenRead(Path.Combine(MyFileSystem.ContentPath, "Shaders/passes", tag), "vertex_stage.hlsl"))
				{
					value.m_vertexStageSrc = new StreamReader(stream).ReadToEnd();
				}
<<<<<<< HEAD
				using (Stream stream2 = MyFileSystem.OpenRead(Path.Combine(MyFileSystem.ContentPath, "Shaders/passes", tag), "pixel_stage.hlsl"))
				{
					value.m_pixelStageSrc = new StreamReader(stream2).ReadToEnd();
					return value;
				}
=======
				using Stream stream2 = MyFileSystem.OpenRead(Path.Combine(MyFileSystem.ContentPath, "Shaders/passes", tag), "pixel_stage.hlsl");
				value.m_pixelStageSrc = new StreamReader(stream2).ReadToEnd();
				return value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return value;
		}

		internal static string VertexStageSrc(string tag)
		{
			return GetOrCreate(tag)?.m_vertexStageSrc;
		}

		internal static string PixelStageSrc(string tag)
		{
			return GetOrCreate(tag)?.m_pixelStageSrc;
		}
	}
}
