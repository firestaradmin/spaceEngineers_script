using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessagePreloadModels : MyRenderMessageBase
	{
		public readonly List<string> Models = new List<string>();

		public bool ForInstancedComponent;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.PreloadModels;

		public override void Close()
		{
			base.Close();
			Models.Clear();
		}
	}
}
