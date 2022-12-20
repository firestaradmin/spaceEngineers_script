using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sandbox.Engine.Analytics;
using Sandbox.Game.Localization;
using VRage;
using VRage.Game.Components;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 900)]
	public class MyChatBot : MySessionComponentBase
	{
		private struct Substitute
		{
			public Regex Source;

			public MyStringId Dest;
		}

		private static readonly char[] m_separators = new char[3] { ' ', '\r', '\n' };

		private static readonly string[] m_nicks = new string[5] { "+bot", "/bot", "+?", "/?", "?" };

		private const string MISUNDERSTANDING_TEXTID = "ChatBotMisunderstanding";

		public static readonly string UNAVAILABLE_TEXTID = "ChatBotUnavailable";

		private static readonly MyStringId[] m_smallTalk = new MyStringId[10]
		{
			MySpaceTexts.ChatBot_Rude,
			MySpaceTexts.ChatBot_ThankYou,
			MySpaceTexts.ChatBot_Generic,
			MySpaceTexts.ChatBot_HowAreYou,
			MySpaceTexts.Description_FAQ_Objective,
			MySpaceTexts.Description_FAQ_GoodBot,
			MySpaceTexts.Description_FAQ_Begin,
			MySpaceTexts.Description_FAQ_Bug,
			MySpaceTexts.Description_FAQ_Test,
			MySpaceTexts.Description_FAQ_Clang
		};

		private static readonly Regex[] m_smallTalkRegex = (Regex[])(object)new Regex[m_smallTalk.Length];

		private const int MAX_MISUNDERSTANDING = 1;

		private readonly List<Substitute> m_substitutes = new List<Substitute>();

		private Regex m_stripSymbols;

		private IMyChatBotResponder m_chatbotResponder = new MyChatBotLocalResponder();

		public IMyChatBotResponder ChatBotResponder => m_chatbotResponder;

		public override bool IsRequiredByGame => true;

		public MyChatBot()
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			int num = 0;
			while (true)
			{
				MyStringId orCompute = MyStringId.GetOrCompute("ChatBot_Substitute" + num + "_S");
				MyStringId orCompute2 = MyStringId.GetOrCompute("ChatBot_Substitute" + num + "_D");
				if (!MyTexts.Exists(orCompute) || !MyTexts.Exists(orCompute2))
				{
					break;
				}
				m_substitutes.Add(new Substitute
				{
					Source = new Regex(MyTexts.GetString(orCompute) + "(?:[ ,.?;\\-()*]|$)", (RegexOptions)9),
					Dest = orCompute2
				});
				num++;
			}
			for (num = 0; num < m_smallTalk.Length; num++)
			{
				int num2 = 0;
				string text2 = "";
				while (true)
				{
					MyStringId orCompute3 = MyStringId.GetOrCompute(string.Concat(m_smallTalk[num], "_Q", num2));
					if (!MyTexts.Exists(orCompute3))
					{
						break;
					}
					if (num2 != 0)
					{
						text2 += "(?:[ ,.?!;\\-()*]|$)|";
					}
					text2 += MyTexts.GetString(orCompute3);
					num2++;
				}
				text2 += "(?:[ ,.?!;\\-()*]|$)";
				m_smallTalkRegex[num] = new Regex(text2, (RegexOptions)9);
			}
			m_stripSymbols = new Regex("(?:[^a-z0-9 ])", (RegexOptions)9);
			IMyChatBotResponder chatbotResponder = m_chatbotResponder;
			chatbotResponder.OnResponse = (ChatBotResponseDelegate)Delegate.Combine(chatbotResponder.OnResponse, (ChatBotResponseDelegate)delegate(string question, string text, ResponseType responseType, Action<string> responseAction)
			{
				Respond(question, text, responseType, responseAction);
			});
		}

		public override void LoadData()
		{
			base.LoadData();
			m_chatbotResponder.LoadData();
		}

		public bool FilterMessage(string message, Action<string> responseAction)
		{
			string[] array = message.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length > 1)
			{
				string[] nicks = m_nicks;
				for (int i = 0; i < nicks.Length; i++)
				{
					if (nicks[i] == array[0].ToLower())
					{
						string text = "";
						for (int j = 1; j < array.Length; j++)
						{
							text = text + array[j] + " ";
						}
						text = text.Trim();
						string preprocessedText;
						string responseId;
						ResponseType responseType = Preprocess(text, out preprocessedText, out responseId);
						if (responseType == ResponseType.ChatBot)
						{
							SendMessage(text, preprocessedText, responseId, responseAction);
						}
						else
						{
							Respond(text, responseId, responseType, responseAction);
						}
						return true;
					}
				}
			}
			return false;
		}

		private ResponseType Preprocess(string messageText, out string preprocessedText, out string responseId)
		{
			preprocessedText = messageText;
			responseId = GetMisunderstandingTextId();
			ResponseType result = ResponseType.Garbage;
			string text = m_stripSymbols.Replace(messageText, "").Trim();
			if (text.Length != 0)
			{
				result = ResponseType.SmallTalk;
				string text2 = ExtractPhrases(text, out responseId);
				if (text2 != null)
				{
					preprocessedText = ApplySubstitutions(text2);
					result = ResponseType.ChatBot;
				}
			}
			return result;
		}

		private string ApplySubstitutions(string text)
		{
			foreach (Substitute substitute in m_substitutes)
			{
				text = substitute.Source.Replace(text, MyTexts.GetString(substitute.Dest));
			}
			return text;
		}

		private string ExtractPhrases(string messageText, out string potentialResponseId)
		{
			potentialResponseId = null;
			for (int i = 0; i < m_smallTalkRegex.Length; i++)
			{
				string text = m_smallTalkRegex[i].Replace(messageText, "");
				if (text.Length != messageText.Length)
				{
					potentialResponseId = m_smallTalk[i].ToString();
					if (text.Trim().Length < 4)
					{
						return null;
					}
					return text;
				}
			}
			return messageText;
		}

		private void SendMessage(string originalQuestion, string preprocessedQuestion, string potentialResponseId, Action<string> responseAction)
		{
			m_chatbotResponder.SendMessage(originalQuestion, preprocessedQuestion, potentialResponseId, responseAction);
		}

		private void Respond(string question, string responseId, ResponseType responseType, Action<string> responseAction)
		{
			MySpaceAnalytics.Instance.ReportGoodBotQuestion(responseType, question, responseId);
			string text = MyTexts.GetString(responseId);
			if (responseAction != null)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					responseAction(text);
				}, "OnChatBotResponse");
			}
		}

		public static string GetMisunderstandingTextId()
		{
			return "ChatBotMisunderstanding" + MyRandom.Instance.Next(0, 1);
		}
	}
}
