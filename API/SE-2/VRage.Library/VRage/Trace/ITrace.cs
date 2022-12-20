namespace VRage.Trace
{
	public interface ITrace
	{
		bool Enabled { get; set; }

		void Watch(string name, object value);

		void Send(string msg, string comment = null);

		void Flush();
	}
}
