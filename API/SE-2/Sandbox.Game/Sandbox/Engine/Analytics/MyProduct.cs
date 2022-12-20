using System;

namespace Sandbox.Engine.Analytics
{
	[SupportedType]
	public class MyProduct
	{
		public string ProductName { get; set; }

		public string ProductID { get; set; }

		public string PackageID { get; set; }

		public string StoreID { get; set; }

		public DateTime PurchaseTimestamp { get; set; }
	}
}
