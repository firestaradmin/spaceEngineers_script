using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.Definitions;
using VRage.ObjectBuilders;

namespace VRage.Game
{
	/// <summary>
	/// Simple definition manager class that allows loading of definitions from files
	/// and support type overrides (e.g. for loading subset of EnvironmentDefinition)
	/// </summary>
	public class MyDefinitionManagerSimple : MyDefinitionManagerBase
	{
		private Dictionary<string, string> m_overrideMap = new Dictionary<string, string>();

		/// <summary>
		///
		/// </summary>
		/// <param name="overridingType"></param>
		/// <param name="typeOverride">The xst:type attribute overridden</param>
		public void AddDefinitionOverride(Type overridingType, string typeOverride)
		{
			MyDefinitionTypeAttribute myDefinitionTypeAttribute = CustomAttributeExtensions.GetCustomAttribute(overridingType, typeof(MyDefinitionTypeAttribute), inherit: false) as MyDefinitionTypeAttribute;
			if (myDefinitionTypeAttribute == null)
			{
				throw new Exception("Missing type attribute in definition");
			}
<<<<<<< HEAD
			XmlTypeAttribute xmlTypeAttribute = CustomAttributeExtensions.GetCustomAttribute(myDefinitionTypeAttribute.ObjectBuilderType, typeof(XmlTypeAttribute), inherit: false) as XmlTypeAttribute;
			string value = ((xmlTypeAttribute != null) ? xmlTypeAttribute.TypeName : myDefinitionTypeAttribute.ObjectBuilderType.Name);
=======
			Attribute customAttribute = myDefinitionTypeAttribute.ObjectBuilderType.GetCustomAttribute(typeof(XmlTypeAttribute), inherit: false);
			XmlTypeAttribute val = (XmlTypeAttribute)(object)((customAttribute is XmlTypeAttribute) ? customAttribute : null);
			string value = ((val != null) ? val.get_TypeName() : myDefinitionTypeAttribute.ObjectBuilderType.Name);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_overrideMap[typeOverride] = value;
		}

		public void LoadDefinitions(string path)
		{
			bool flag = false;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = null;
			using (Stream stream = MyFileSystem.OpenRead(path))
			{
				if (stream != null)
				{
					using Stream stream2 = stream.UnwrapGZip();
					if (stream2 != null)
					{
<<<<<<< HEAD
						if (stream2 != null)
						{
							flag = MyObjectBuilderSerializer.DeserializeXML(stream2, out var objectBuilder, typeof(MyObjectBuilder_Definitions), m_overrideMap);
							myObjectBuilder_Definitions = objectBuilder as MyObjectBuilder_Definitions;
						}
=======
						flag = MyObjectBuilderSerializer.DeserializeXML(stream2, out var objectBuilder, typeof(MyObjectBuilder_Definitions), m_overrideMap);
						myObjectBuilder_Definitions = objectBuilder as MyObjectBuilder_Definitions;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			if (!flag)
			{
				throw new Exception("Error while reading \"" + path + "\"");
			}
			if (myObjectBuilder_Definitions.Definitions != null)
			{
				MyObjectBuilder_DefinitionBase[] definitions = myObjectBuilder_Definitions.Definitions;
				foreach (MyObjectBuilder_DefinitionBase myObjectBuilder_DefinitionBase in definitions)
				{
					MyObjectBuilderType.RemapType(ref myObjectBuilder_DefinitionBase.Id, m_overrideMap);
					MyDefinitionBase myDefinitionBase = MyDefinitionManagerBase.GetObjectFactory().CreateInstance(myObjectBuilder_DefinitionBase.TypeId);
					myDefinitionBase.Init(myObjectBuilder_Definitions.Definitions[0], new MyModContext());
					m_definitions.AddDefinition(myDefinitionBase);
				}
			}
		}

		public override MyDefinitionSet GetLoadingSet()
		{
			throw new NotImplementedException();
		}
	}
}
