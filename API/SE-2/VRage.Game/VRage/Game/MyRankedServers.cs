using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
<<<<<<< HEAD
using System.Net;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Xml.Serialization;
using ParallelTasks;
using VRage.Http;
using VRage.Utils;

namespace VRage.Game
{
	public class MyRankedServers
	{
		private class DownloadWork : WorkData
		{
			public readonly string Url;

			public MyRankedServers Result;

			public readonly Action<MyRankedServers> CompletedCallback;

			/// <inheritdoc />
			public DownloadWork(string url, Action<MyRankedServers> completedCallback)
			{
				Url = url;
				CompletedCallback = completedCallback;
			}
		}
<<<<<<< HEAD

		private static readonly XmlSerializer m_serializer = new XmlSerializer(typeof(MyRankedServers));

		public List<MyRankServer> Servers { get; set; }

		public IEnumerable<MyRankServer> GetByPrefix(string prefix)
		{
			return Servers.Where((MyRankServer x) => x.ServicePrefix == prefix);
=======

		private static readonly XmlSerializer m_serializer = new XmlSerializer(typeof(MyRankedServers));

		public List<MyRankServer> Servers { get; set; }

		public IEnumerable<MyRankServer> GetByPrefix(string prefix)
		{
			return Enumerable.Where<MyRankServer>((IEnumerable<MyRankServer>)Servers, (Func<MyRankServer, bool>)((MyRankServer x) => x.ServicePrefix == prefix));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public MyRankedServers()
		{
			Servers = new List<MyRankServer>();
		}

		public static void LoadAsync(string url, Action<MyRankedServers> completedCallback)
		{
			DownloadWork workData = new DownloadWork(url, completedCallback);
			Parallel.Start(DownloadChangelog, Completion, workData);
		}

		private static void DownloadChangelog(WorkData work)
		{
<<<<<<< HEAD
			DownloadWork downloadWork = (DownloadWork)work;
			try
			{
				if (MyVRage.Platform.Http.SendRequest(downloadWork.Url, null, HttpMethod.GET, out var content) == HttpStatusCode.OK)
=======
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Invalid comparison between Unknown and I4
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			DownloadWork downloadWork = (DownloadWork)work;
			try
			{
				if ((int)MyVRage.Platform.Http.SendRequest(downloadWork.Url, null, HttpMethod.GET, out var content) == 200)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					StringReader val = new StringReader(content);
					try
					{
<<<<<<< HEAD
						downloadWork.Result = m_serializer.Deserialize(textReader) as MyRankedServers;
=======
						downloadWork.Result = m_serializer.Deserialize((TextReader)(object)val) as MyRankedServers;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error while downloading ranked servers: " + ex.ToString());
			}
		}

		private static void Completion(WorkData work)
		{
			DownloadWork downloadWork = (DownloadWork)work;
			downloadWork.CompletedCallback?.Invoke(downloadWork.Result);
		}

		public static void SaveTestData()
		{
			MyRankedServers myRankedServers = new MyRankedServers();
			myRankedServers.Servers.Add(new MyRankServer
			{
				Address = "10.20.0.26:27016",
				Rank = 1,
				ServicePrefix = "steam://"
			});
			using FileStream fileStream = File.OpenWrite("rankedServers.xml");
			m_serializer.Serialize((Stream)fileStream, (object)myRankedServers);
		}
	}
}
