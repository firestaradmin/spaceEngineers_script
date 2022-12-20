namespace VRage
{
	public interface IAdvancedDebugListener
	{
		void Fail(string message, string detail, string file, int line);

		void WriteLine(string message, string file, int line);
	}
}
