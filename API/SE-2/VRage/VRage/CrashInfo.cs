using System.IO;

namespace VRage
{
	public struct CrashInfo
	{
		public const string StartMark = "================================== CRASH INFO ==================================";

		public const string EndMark = "================================== OFNI HSARC ==================================";

		public string AppVersion;

		public string GameName;

		public string AnalyticId;

		public bool IsOutOfMemory;

		public bool IsGPU;

		public bool IsNative;

		public bool IsTask;

		public bool IsHang;

		public bool IsExperimental;

		public long ProcessRunTime;

		public int PCUCount;

		public long GCMemory;

		public long GCMemoryAllocated;

		public long HWAvailableMemory;

		public long ProcessPrivateMemory;

		public CrashInfo(string appVersion, string gameName, string analyticId)
		{
			this = default(CrashInfo);
			AppVersion = appVersion;
			GameName = gameName;
			AnalyticId = analyticId;
		}

		public void Write(TextWriter writer)
		{
			writer.WriteLine("================================== CRASH INFO ==================================");
			writer.WriteLine("AppVersion: " + AppVersion);
			writer.WriteLine("GameName: " + GameName);
			writer.WriteLine($"IsOutOfMemory: {IsOutOfMemory}");
			writer.WriteLine($"IsGPU: {IsGPU}");
			writer.WriteLine($"IsNative: {IsNative}");
			writer.WriteLine($"IsTask: {IsTask}");
			writer.WriteLine($"IsExperimental: {IsExperimental}");
			writer.WriteLine($"ProcessRunTime: {ProcessRunTime}");
			writer.WriteLine($"PCUCount: {PCUCount}");
			writer.WriteLine($"IsHang: {IsHang}");
			writer.WriteLine($"GCMemory: {GCMemory}");
			writer.WriteLine($"GCMemoryAllocated: {GCMemoryAllocated}");
			writer.WriteLine($"HWAvailableMemory: {HWAvailableMemory}");
			writer.WriteLine($"ProcessPrivateMemory: {ProcessPrivateMemory}");
			writer.WriteLine("AnalyticId: " + AnalyticId);
			writer.WriteLine("================================== OFNI HSARC ==================================");
		}

		public static CrashInfo Read(TextReader reader)
		{
			string text;
			while ((text = reader.ReadLine()) != "================================== CRASH INFO ==================================")
			{
				if (text == null)
				{
					return default(CrashInfo);
				}
			}
			CrashInfo result = default(CrashInfo);
			while ((text = reader.ReadLine()) != "================================== OFNI HSARC ==================================" && text != null)
			{
				int num = text.IndexOf(':');
				string text2 = text.Substring(0, num);
				string text3 = text.Substring(num + 2);
				switch (text2)
				{
				case "AppVersion":
					result.AppVersion = text3;
					break;
				case "GameName":
					result.GameName = text3;
					break;
				case "IsOutOfMemory":
					result.IsOutOfMemory = bool.Parse(text3);
					break;
				case "IsGPU":
					result.IsGPU = bool.Parse(text3);
					break;
				case "IsNative":
					result.IsNative = bool.Parse(text3);
					break;
				case "IsTask":
					result.IsTask = bool.Parse(text3);
					break;
				case "ProcessRunTime":
					result.ProcessRunTime = long.Parse(text3);
					break;
				case "PCUCount":
					result.PCUCount = int.Parse(text3);
					break;
				case "IsExperimental":
					result.IsExperimental = bool.Parse(text3);
					break;
				case "IsHang":
					result.IsHang = bool.Parse(text3);
					break;
				case "GCMemory":
					result.GCMemory = long.Parse(text3);
					break;
				case "GCMemoryAllocated":
					result.GCMemoryAllocated = long.Parse(text3);
					break;
				case "HWAvailableMemory":
					result.HWAvailableMemory = long.Parse(text3);
					break;
				case "ProcessPrivateMemory":
					result.ProcessPrivateMemory = long.Parse(text3);
					break;
				case "AnalyticId":
					result.AnalyticId = text3;
					break;
				}
			}
			return result;
		}

		/// <inheritdoc />
		public override string ToString()
		{
<<<<<<< HEAD
			StringWriter stringWriter = new StringWriter();
			Write(stringWriter);
			return stringWriter.ToString();
=======
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			StringWriter val = new StringWriter();
			Write((TextWriter)(object)val);
			return ((object)val).ToString();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
