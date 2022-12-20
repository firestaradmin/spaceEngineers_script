using System;

namespace Sandbox.Game.GameSystems
{
	public interface IMyChatBotResponder
	{
		ChatBotResponseDelegate OnResponse { get; set; }

		void LoadData();

		void SendMessage(string originalQuestion, string preprocessedQuestion, string potentialResponseId, Action<string> responseAction);

		void PerformDebugTest(PreprocessDelegate preprocess);
	}
}
