using System.Collections.Generic;

namespace VRageRender.Messages
{
	/// <summary>
	/// Render message representing a repeatable command list.
	/// </summary>
	public class MyRenderMessageDrawCommands : MyRenderMessageBase
	{
		public List<MyRenderMessageBase> Messages;

		public bool DisposeAfterDraw;

		/// <inheritdoc />
		public override MyRenderMessageType MessageClass => MyRenderMessageType.Draw;

		/// <inheritdoc />
		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DrawCommands;

		/// <inheritdoc />
		public override void Close()
		{
			base.Close();
			if (DisposeAfterDraw)
			{
				MyRenderProxy.DisposeMessages(Messages);
			}
			Messages = null;
		}
	}
}
