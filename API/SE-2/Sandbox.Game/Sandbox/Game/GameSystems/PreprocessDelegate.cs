namespace Sandbox.Game.GameSystems
{
	public delegate ResponseType PreprocessDelegate(string messageText, out string preprocessedText, out string responseId);
}
