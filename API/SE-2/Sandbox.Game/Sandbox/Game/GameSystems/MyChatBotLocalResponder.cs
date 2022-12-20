using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using VRage.Collections;

namespace Sandbox.Game.GameSystems
{
	public class MyChatBotLocalResponder : IMyChatBotResponder
	{
		private struct ChatbotQuestion
		{
			public string OriginalQuestion;

			public string PreprocessedQuestion;

			public string PotentialResponseId;

			public Action<string> ResponseAction;
		}

		private class ScoredResponse : IEquatable<ScoredResponse>, IComparable<ScoredResponse>
		{
			public int[] ResponseSubscores;

			public MyChatBotResponseDefinition Definition;

			public int Score => Enumerable.Sum((IEnumerable<int>)ResponseSubscores);

			public override string ToString()
			{
				return Definition.Id.SubtypeName + " (" + Score + ")";
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				ScoredResponse scoredResponse = obj as ScoredResponse;
				if (scoredResponse == null)
				{
					return false;
				}
				return Equals(scoredResponse);
			}

			public int CompareTo(ScoredResponse compareScoredResponse)
			{
				return compareScoredResponse?.Score.CompareTo(Score) ?? 1;
			}

			public override int GetHashCode()
			{
				return Score;
			}

			public bool Equals(ScoredResponse other)
			{
				if (other == null)
				{
					return false;
				}
				return Score.Equals(other.Score);
			}
		}

		private List<ScoredResponse> m_responseScores = new List<ScoredResponse>(300);

		public ChatBotResponseDelegate OnResponse { get; set; }

		public void LoadData()
		{
			ListReader<MyChatBotResponseDefinition> chatBotResponseDefinitions = MyDefinitionManager.Static.GetChatBotResponseDefinitions();
			for (int i = 0; i < chatBotResponseDefinitions.Count; i++)
			{
				ScoredResponse scoredResponse = new ScoredResponse();
				scoredResponse.Definition = chatBotResponseDefinitions[i];
				scoredResponse.ResponseSubscores = new int[scoredResponse.Definition.Questions.Length];
				m_responseScores.Add(scoredResponse);
			}
			ResetScore();
		}

		private void ResetScore()
		{
			for (int i = 0; i < m_responseScores.Count; i++)
			{
				for (int j = 0; j < m_responseScores[i].ResponseSubscores.Length; j++)
				{
					m_responseScores[i].ResponseSubscores[j] = 0;
				}
			}
		}

		public void SendMessage(string originalQuestion, string preprocessedQuestion, string potentialResponseId, Action<string> responseAction)
		{
			ResponseType responseType = ResponseType.Unavailable;
			string response = GetResponse(originalQuestion, out responseType);
			OnResponse?.Invoke(originalQuestion, response, responseType, responseAction);
		}

		public void PerformDebugTest(PreprocessDelegate preprocess)
		{
		}

		private string GetResponse(string question, out ResponseType responseType)
		{
			ResetScore();
			ScoreQuestion(question);
			m_responseScores.Sort();
			if (m_responseScores[0].Score == 0)
			{
				responseType = ResponseType.Misunderstanding;
				return MyChatBot.GetMisunderstandingTextId();
			}
			responseType = ResponseType.ChatBot;
			return m_responseScores[0].Definition.Response;
		}

		private void ScoreQuestion(string question)
		{
			string[] array = question.Split(new char[4] { ' ', ',', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < m_responseScores.Count; i++)
			{
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (text.Length >= 3)
					{
						for (int k = 0; k < m_responseScores[i].Definition.Questions.Length; k++)
						{
							m_responseScores[i].ResponseSubscores[k] += ScoreQuestionWord(text, m_responseScores[i].Definition.Questions[k]);
						}
					}
				}
			}
		}

		private int ScoreQuestionWord(string word, string question)
		{
			if (question.IndexOf(word, StringComparison.InvariantCultureIgnoreCase) == -1)
			{
				return 0;
			}
			return word.Length;
		}
	}
}
