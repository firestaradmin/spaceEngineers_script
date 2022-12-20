using System;

namespace VRageRender
{
	public class MyDeviceErrorException : Exception
	{
		public MyDeviceErrorException(string message)
			: base(message)
		{
		}
	}
}
