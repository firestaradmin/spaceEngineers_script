namespace VRage.Scripting
{
	public readonly struct Message
	{
		public readonly bool IsError;

		public readonly string Text;

		public Message(bool isError, string text)
		{
			Text = text;
			IsError = isError;
		}

		public override string ToString()
		{
			return (IsError ? "Error" : "Warning") + " " + Text;
		}
	}
}
