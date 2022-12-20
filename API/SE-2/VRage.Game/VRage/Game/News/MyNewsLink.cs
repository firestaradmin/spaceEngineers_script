using System.Xml.Serialization;

namespace VRage.Game.News
{
	public class MyNewsLink
	{
		[XmlAttribute(AttributeName = "ref")]
		public string Link;

		[XmlText]
		public string Text;
	}
}
