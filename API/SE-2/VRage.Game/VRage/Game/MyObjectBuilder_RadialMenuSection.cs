using System.ComponentModel;
using System.Xml.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	public class MyObjectBuilder_RadialMenuSection
	{
		public MyStringId Label;

		[DefaultValue(null)]
		[XmlArrayItem("Item", typeof(MyAbstractXmlSerializer<MyObjectBuilder_RadialMenuItem>))]
		public MyObjectBuilder_RadialMenuItem[] Items;

		public bool m_IsEnabledCreative = true;

		public bool m_IsEnabledSurvival = true;

		public bool IsEnabledCreative
		{
			get
			{
				return m_IsEnabledCreative;
			}
			set
			{
				m_IsEnabledCreative = value;
			}
		}

		public bool IsEnabledSurvival
		{
			get
			{
				return m_IsEnabledSurvival;
			}
			set
			{
				m_IsEnabledSurvival = value;
			}
		}
	}
}
