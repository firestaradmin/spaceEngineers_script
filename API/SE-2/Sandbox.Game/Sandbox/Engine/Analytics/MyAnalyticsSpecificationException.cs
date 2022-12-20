using System;

namespace Sandbox.Engine.Analytics
{
	internal class MyAnalyticsSpecificationException : Exception
	{
		public MyAnalyticsSpecificationException()
		{
		}

		public MyAnalyticsSpecificationException(string message)
			: base(message)
		{
		}

		public MyAnalyticsSpecificationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
