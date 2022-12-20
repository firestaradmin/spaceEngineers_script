using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D;

namespace VRageRender
{
	internal static class MyShadersDefines
	{
		internal const string ShadersFolderName = "Shaders";

		internal const string ContentCacheDir = "ShaderCache";

		internal const string UserDataCacheDir = "ShaderCache2";

		internal const string UserDataPdbDir = "ShaderCachePdb";

		internal const string UserDataPreprocessedDir = "ShaderCachePreprocessed";

		internal static string GetString(this IEnumerable<ShaderMacro> macros)
		{
			if (macros == null)
			{
				return string.Empty;
			}
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ShaderMacro macro in macros)
			{
				if (num > 0)
				{
					stringBuilder.AppendFormat(";");
				}
				if (macro.Definition != null)
				{
					stringBuilder.AppendFormat("{0}={1}", macro.Name, macro.Definition);
				}
				else
				{
					stringBuilder.Append(macro.Name);
				}
				num++;
			}
			return stringBuilder.ToString();
		}

		internal static string GetString(this IEnumerable<MyVertexInputComponent> components)
		{
			if (components == null)
			{
				return string.Empty;
			}
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MyVertexInputComponent component in components)
			{
				if (num > 0)
				{
					stringBuilder.AppendFormat(";");
				}
				stringBuilder.Append(component.Type);
				num++;
			}
			return stringBuilder.ToString();
		}
	}
}
