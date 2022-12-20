using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageUnloadTextures : MyRenderMessageBase
	{
		public List<string> Files;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UnloadTextures;

		public override void Close()
		{
			base.Close();
			Files.Clear();
		}
	}
}
