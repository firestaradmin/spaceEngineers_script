using System;
using VRage.Render11.Common;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal interface IDeviceWriteBuffer : IDisposable
	{
		(MyMapping, IBuffer, MyQuery, int Offset) Alloc(int bytes);
	}
}
