using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageDeprioritizeTextures : MyRenderMessageBase
	{
		public List<string> Files;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DeprioritizeTextures;
	}
}
