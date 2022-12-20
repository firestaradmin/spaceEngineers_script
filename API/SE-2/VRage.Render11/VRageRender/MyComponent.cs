using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace VRageRender
{
	internal abstract class MyComponent
	{
		private static int NextIndex(Dictionary<string, int> dict, string name)
		{
			int value = 0;
			if (dict.TryGetValue(name, out value))
			{
				dict[name] = value + 1;
			}
			else
			{
				dict[name] = 1;
			}
			return value;
		}

		public static ShaderMacro[] GetComponentMacros(string declaration, string transferCode, MyVertexInputComponent[] components)
		{
			ShaderMacro shaderMacro = new ShaderMacro("VERTEX_COMPONENTS_DECLARATIONS", declaration);
			ShaderMacro shaderMacro2 = new ShaderMacro("TRANSFER_VERTEX_COMPONENTS", transferCode);
			bool flag = false;
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].Type == MyVertexInputComponentType.TEXINDICES)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return new ShaderMacro[2] { shaderMacro, shaderMacro2 };
			}
			return new ShaderMacro[3]
			{
				shaderMacro,
				shaderMacro2,
				new ShaderMacro("USE_TEXTURE_INDICES", null)
			};
		}

		protected static void AddSingle(string name, string variable, Format format, MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration)
		{
			InputClassification slotClass = ((component.Freq != 0) ? InputClassification.PerInstanceData : InputClassification.PerVertexData);
			int stepRate = ((component.Freq != 0) ? 1 : 0);
			int num = NextIndex(dict, name);
			list.Add(new InputElement(name, num, format, InputElement.AppendAligned, component.Slot, slotClass, stepRate));
			declaration.AppendFormat("{0} : {1}{2};\\\n", variable, name, num);
		}

		internal abstract void AddComponent(MyVertexInputComponent component, List<InputElement> list, Dictionary<string, int> dict, StringBuilder declaration, StringBuilder code);
	}
}
