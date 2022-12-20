using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ProtoBuf;

namespace VRage.EOS
{
	[Serializable]
	[ProtoContract]
	[XmlSerializerAssembly("VRage.EOS.XmlSerializers")]
	[XmlRoot("LobbyList")]
	public class MySerializedServerList
	{
		[ProtoMember(1)]
		public List<string> Lobbies { get; set; }

		public MySerializedServerList()
		{
			Lobbies = new List<string>();
		}

		public MySerializedServerList(List<string> lobbiesId)
		{
			Lobbies = lobbiesId;
		}
	}
}
