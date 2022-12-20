using System.Xml.Serialization;

namespace VRage.Game.News
{
	public class MyNewsEntry
	{
		[XmlAttribute(AttributeName = "title")]
		public string Title;

		[XmlAttribute(AttributeName = "date")]
		public string Date;

		[XmlAttribute(AttributeName = "version")]
		public string Version;

		[XmlAttribute(AttributeName = "public")]
		public bool Public = true;

		[XmlAttribute(AttributeName = "dev")]
		public bool Dev;

		[XmlArrayItem("Link")]
		public MyNewsLink[] Links;

		[XmlText]
		public string Text;
	}
}
