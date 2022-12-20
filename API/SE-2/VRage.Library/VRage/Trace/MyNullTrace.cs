namespace VRage.Trace
{
	internal class MyNullTrace : ITrace
	{
		public bool Enabled { get; set; }

		public void Send(string msg, string comment = null)
		{
		}

		public void Flush()
		{
		}

		public void Watch(string name, object value)
		{
		}
	}
}
