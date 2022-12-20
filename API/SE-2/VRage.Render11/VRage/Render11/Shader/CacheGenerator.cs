using System;
using System.Xml.Serialization;
using VRageRender;

namespace VRage.Render11.Shader
{
	[Serializable]
	public class CacheGenerator
	{
		[Serializable]
		public class ComboGroup
		{
			[XmlAttribute]
			public string Material;

			[XmlAttribute]
			public string Pass;

			public Combo[] ComboList1;

			public Combo[] ComboList2;
		}

		[Serializable]
		public class Material
		{
			[XmlAttribute]
			public string Id;

			[XmlAttribute]
			public string FlagNames;

			[XmlAttribute]
			public string UnsupportedFlagNames;

			[XmlAttribute]
			public string UnsupportedPasses;
		}

		[Serializable]
		public class Pass
		{
			[XmlAttribute]
			public string Id;

			[XmlAttribute]
			public string FlagNames;

			[XmlAttribute]
			public string UnsupportedFlagNames;
		}

		[Serializable]
		public class Combo
		{
			[XmlAttribute]
			public MyVertexInputComponentType[] VertexInput;

			[XmlAttribute]
			public int[] VertexInputOrder;

			[XmlAttribute]
			public string FlagNames;
		}

		[XmlArrayItem("Ignore")]
		public string[] Ignores;

		public Material[] Materials;

		public Pass[] Passes;

		public ComboGroup[] Combos;
	}
}
