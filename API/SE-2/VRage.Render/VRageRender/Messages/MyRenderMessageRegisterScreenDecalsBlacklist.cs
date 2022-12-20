using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageRegisterScreenDecalsBlacklist : MyRenderMessageBase
	{
		public List<string> targetBlacklist;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.SetDecalsBlacklist;
	}
}
