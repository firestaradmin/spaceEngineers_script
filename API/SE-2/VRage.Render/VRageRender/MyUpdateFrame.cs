using System.Collections.Generic;
using VRage.Library.Utils;
using VRage.Network;
using VRageRender.Messages;

namespace VRageRender
{
	/// <summary>
	/// Contains data produced by update frame, sent to render in thread-safe manner
	/// </summary>
	[GenerateActivator]
	public class MyUpdateFrame
	{
		public int Id;

		public bool Processed;

		public bool Cleared;

		public MyTimeSpan UpdateTimestamp;

		public readonly List<MyRenderMessageBase> RenderInput = new List<MyRenderMessageBase>(2048);

		private static int m_idCounter;

		public MyUpdateFrame()
		{
			Id = m_idCounter++;
		}

		public void Enqueue(MyRenderMessageBase message)
		{
			lock (RenderInput)
			{
				RenderInput.Add(message);
			}
		}
	}
}
