using System;

namespace Sandbox.Game.GameSystems
{
	public delegate void ChatBotResponseDelegate(string originalQuestion, string responseId, ResponseType responseType, Action<string> responseAction);
}
