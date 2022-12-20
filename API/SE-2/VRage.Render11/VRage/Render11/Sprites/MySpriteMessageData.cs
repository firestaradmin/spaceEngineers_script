using VRage.Library.Collections;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Sprites
{
	[PooledObject(2)]
	internal class MySpriteMessageData : IPooledObject
	{
		public int FrameId;

		public readonly MyList<MySpriteDrawRenderMessage> Messages = new MyList<MySpriteDrawRenderMessage>();

		/// <inheritdoc />
		void IPooledObject.Cleanup()
		{
			Messages.SetSize(0);
		}
	}
}
