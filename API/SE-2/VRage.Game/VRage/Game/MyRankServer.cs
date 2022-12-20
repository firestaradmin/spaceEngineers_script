using System.Xml.Serialization;

namespace VRage.Game
{
	public class MyRankServer
	{
		private string m_connectionString;

		[XmlAttribute]
		public int Rank { get; set; }

		[XmlAttribute]
		public string Address { get; set; }

		[XmlAttribute]
		public string ServicePrefix { get; set; }

		[XmlIgnore]
		public string ConnectionString => m_connectionString ?? (m_connectionString = ServicePrefix + Address);
	}
}
