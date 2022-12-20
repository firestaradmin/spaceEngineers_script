using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using LitJson;
using Sandbox.Engine.Utils;
using VRage;
using VRage.Http;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	public class MyChatBotGoodAIResponder : IMyChatBotResponder
	{
		private struct ChatBotResponse
		{
			public string intent { get; set; }

			public string error { get; set; }
		}

		private const string CHATBOT_URL = "https://chatbot.keenswh.com:8011/";

		private const string CHATBOT_DEV_URL = "https://chatbot2.keenswh.com:8011/";

		private const string OUPTUT_FILE = "c:\\x\\stats_out.csv";

		private const string INPUT_FILE = "c:\\x\\stats.csv";

		public ChatBotResponseDelegate OnResponse { get; set; }

		public void LoadData()
		{
		}

		private HttpData[] CreateChatbotRequest(string preprocessedQuestion)
		{
			string value = DateTime.UtcNow.ToString("r", CultureInfo.InvariantCulture);
			string value2 = $"{{\"state\": \"DEFAULT\", \"utterance\": \"{preprocessedQuestion}\"}}";
			return new HttpData[3]
			{
				new HttpData("Date", value, HttpDataType.HttpHeader),
				new HttpData("Content-Type", "application/json", HttpDataType.HttpHeader),
				new HttpData("application/json", value2, HttpDataType.RequestBody)
			};
		}

		private void SendRequest(string preprocessedQuestion, Action<HttpStatusCode, string> onDone)
		{
			HttpData[] parameters = CreateChatbotRequest(preprocessedQuestion);
			MyVRage.Platform.Http.SendRequestAsync((MyFakes.USE_GOODBOT_DEV_SERVER ? "https://chatbot2.keenswh.com:8011/" : "https://chatbot.keenswh.com:8011/") + "intent", parameters, HttpMethod.POST, onDone);
		}

		public void SendMessage(string originalQuestion, string preprocessedQuestion, string potentialResponseId, Action<string> responseAction)
		{
			SendRequest(preprocessedQuestion, delegate(HttpStatusCode x, string y)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				OnResponseHttp(x, y, responseAction, potentialResponseId, originalQuestion);
			});
		}

		private void OnResponseHttp(HttpStatusCode code, string content, Action<string> responseAction, string potentialResponseId, string question)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			string responseId;
			ResponseType responseType = Postprocess(code, content, potentialResponseId, out responseId);
			OnResponse?.Invoke(question, responseId, responseType, responseAction);
		}

		private ResponseType Postprocess(HttpStatusCode code, string content, string potentialResponseId, out string responseId)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			responseId = MyChatBot.UNAVAILABLE_TEXTID;
			ResponseType result = ResponseType.ChatBot;
			if ((int)code >= 200 && (int)code <= 299)
			{
				ChatBotResponse chatBotResponse;
				try
				{
					chatBotResponse = JsonMapper.ToObject<ChatBotResponse>(content);
				}
				catch (Exception arg)
				{
					MyLog.Default.WriteLine($"Chatbot reponse error: {arg}\n{content}");
					throw;
				}
				if (chatBotResponse.error == null)
				{
					if (chatBotResponse.intent == null)
					{
						if (potentialResponseId == null)
						{
							responseId = MyChatBot.GetMisunderstandingTextId();
							result = ResponseType.Misunderstanding;
						}
						else
						{
							responseId = potentialResponseId;
							result = ResponseType.SmallTalk;
						}
					}
					else
					{
						responseId = chatBotResponse.intent;
					}
				}
				else
				{
					result = ResponseType.Error;
				}
			}
			else
			{
				result = ResponseType.Unavailable;
			}
			return result;
		}

		public void PerformDebugTest(PreprocessDelegate preprocess)
		{
			File.Delete("c:\\x\\stats_out.csv");
			List<string>[] array = new List<string>[12];
			int[][] array2 = new int[6][];
			for (int i = 0; i < 6; i++)
			{
				array[i] = new List<string>();
				array[i + 6] = new List<string>();
				array2[i] = new int[6];
			}
			using StreamWriter streamWriter = new StreamWriter("c:\\x\\stats_out.csv", append: false);
			using (StreamReader reader = new StreamReader("c:\\x\\stats.csv"))
			{
				streamWriter.WriteLine("No change: ");
				int num = 0;
				foreach (IList<string> item in CsvParser.Parse(reader, ';', '"'))
				{
					_ = item.Count;
					_ = 3;
					if (item[0] != "")
					{
						if (!Enum.TryParse<ResponseType>(item[0], out var result))
						{
							result = ResponseType.Misunderstanding;
						}
						string text = item[1];
						string text2 = item[2];
						string preprocessedText;
						string responseId;
						ResponseType responseType = preprocess(text, out preprocessedText, out responseId);
						if (responseType == ResponseType.ChatBot)
						{
							bool done = false;
							SendRequest(preprocessedText, delegate(HttpStatusCode code, string content)
							{
								//IL_001d: Unknown result type (might be due to invalid IL or missing references)
								string potentialResponseId = responseId;
								responseType = Postprocess(code, content, potentialResponseId, out responseId);
								done = true;
							});
							while (!done)
							{
								Thread.Sleep(0);
							}
						}
						array2[(int)result][(int)responseType]++;
						string text3 = $"{responseType};\"{text}\";{responseId};{text2}";
						if (result == responseType && responseId == text2)
						{
							streamWriter.WriteLine(text3);
						}
						else
						{
							array[(int)(result + ((result == responseType) ? 6 : 0))].Add(text3);
						}
					}
					num++;
					_ = num % 100;
				}
			}
			streamWriter.WriteLine("---");
			for (int j = 0; j < 6; j++)
			{
				streamWriter.WriteLine(string.Concat((ResponseType)j, ": "));
				for (int k = 0; k < 2; k++)
				{
					foreach (string item2 in array[j + k * 6])
					{
						streamWriter.WriteLine(item2);
					}
					streamWriter.WriteLine("---");
				}
			}
			for (int l = 0; l < 6; l++)
			{
				string text4 = string.Concat((ResponseType)l, ": ");
				for (int m = 0; m < 6; m++)
				{
					text4 = text4 + array2[l][m] + " ";
				}
			}
		}
	}
}
