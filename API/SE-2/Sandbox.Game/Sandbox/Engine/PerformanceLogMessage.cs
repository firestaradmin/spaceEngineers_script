using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace Sandbox.Engine
{
	[Serializable]
	public class PerformanceLogMessage
	{
		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ETime_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in double value)
			{
				owner.Time = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out double value)
			{
				value = owner.Time;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EReceivedPerSecond_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.ReceivedPerSecond = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.ReceivedPerSecond;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ESentPerSecond_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.SentPerSecond = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.SentPerSecond;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EPeakReceivedPerSecond_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.PeakReceivedPerSecond = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.PeakReceivedPerSecond;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EPeakSentPerSecond_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.PeakSentPerSecond = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.PeakSentPerSecond;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EOverallReceived_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.OverallReceived = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.OverallReceived;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EOverallSent_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.OverallSent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.OverallSent;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ECPULoadSmooth_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.CPULoadSmooth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.CPULoadSmooth;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EThreadLoadSmooth_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.ThreadLoadSmooth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.ThreadLoadSmooth;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EGetOnlinePlayerCount_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in int value)
			{
				owner.GetOnlinePlayerCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out int value)
			{
				value = owner.GetOnlinePlayerCount;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EPing_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.Ping = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.Ping;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EGCMemoryUsed_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.GCMemoryUsed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.GCMemoryUsed;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EProcessMemory_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.ProcessMemory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.ProcessMemory;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EPCUBuilt_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.PCUBuilt = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.PCUBuilt;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EPCU_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.PCU = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.PCU;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EGridsCount_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.GridsCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.GridsCount;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ERenderCPULoadSmooth_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.RenderCPULoadSmooth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.RenderCPULoadSmooth;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ERenderGPULoadSmooth_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.RenderGPULoadSmooth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.RenderGPULoadSmooth;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EHardwareCPULoad_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.HardwareCPULoad = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.HardwareCPULoad;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EHardwareAvailableMemory_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.HardwareAvailableMemory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.HardwareAvailableMemory;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EFrameTime_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.FrameTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.FrameTime;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ELowSimQuality_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.LowSimQuality = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.LowSimQuality;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EFrameTimeLimit_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.FrameTimeLimit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.FrameTimeLimit;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EFrameTimeCPU_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.FrameTimeCPU = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.FrameTimeCPU;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EFrameTimeGPU_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.FrameTimeGPU = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.FrameTimeGPU;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ECPULoadLimit_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.CPULoadLimit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.CPULoadLimit;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ETrackedMemory_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.TrackedMemory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.TrackedMemory;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EGCMemoryAllocated_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in float value)
			{
				owner.GCMemoryAllocated = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out float value)
			{
				value = owner.GCMemoryAllocated;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EGameVersion_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in int value)
			{
				owner.GameVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out int value)
			{
				value = owner.GameVersion;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003EExePath_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in string value)
			{
				owner.ExePath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out string value)
			{
				value = owner.ExePath;
			}
		}

		protected class Sandbox_Engine_PerformanceLogMessage_003C_003ESavePath_003C_003EAccessor : IMemberAccessor<PerformanceLogMessage, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerformanceLogMessage owner, in string value)
			{
				owner.SavePath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerformanceLogMessage owner, out string value)
			{
				value = owner.SavePath;
			}
		}

		public double Time { get; set; }

		public float ReceivedPerSecond { get; set; }

		public float SentPerSecond { get; set; }

		public float PeakReceivedPerSecond { get; set; }

		public float PeakSentPerSecond { get; set; }

		public float OverallReceived { get; set; }

		public float OverallSent { get; set; }

		public float CPULoadSmooth { get; set; }

		public float ThreadLoadSmooth { get; set; }

		public int GetOnlinePlayerCount { get; set; }

		public float Ping { get; set; }

		public float GCMemoryUsed { get; set; }

		public float ProcessMemory { get; set; }

		public float PCUBuilt { get; set; }

		public float PCU { get; set; }

		public float GridsCount { get; set; }

		public float RenderCPULoadSmooth { get; set; }

		public float RenderGPULoadSmooth { get; set; }

		public float HardwareCPULoad { get; set; }

		public float HardwareAvailableMemory { get; set; }

		public float FrameTime { get; set; }

		public float LowSimQuality { get; set; }

		public float FrameTimeLimit { get; set; }

		public float FrameTimeCPU { get; set; }

		public float FrameTimeGPU { get; set; }

		public float CPULoadLimit { get; set; }

		public float TrackedMemory { get; set; }

		public float GCMemoryAllocated { get; set; }

		public int GameVersion { get; set; }

		public string ExePath { get; set; }

		public string SavePath { get; set; }

		public static string GetEndOfTestString()
		{
			return "EndOfTest_WriteResults";
		}

		public static string GetPerfLognamePrefix()
		{
			return "perfLog_";
		}

		public static string SerializeObject<T>(T toSerialize)
		{
<<<<<<< HEAD
			XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
			using (StringWriter stringWriter = new StringWriter())
			{
				xmlSerializer.Serialize(stringWriter, toSerialize);
				stringWriter.ToString();
				return stringWriter.ToString();
=======
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			XmlSerializer val = new XmlSerializer(toSerialize.GetType());
			StringWriter val2 = new StringWriter();
			try
			{
				val.Serialize((TextWriter)(object)val2, (object)toSerialize);
				((object)val2).ToString();
				return ((object)val2).ToString();
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static PerformanceLogMessage DeserializeObject(string str)
		{
<<<<<<< HEAD
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PerformanceLogMessage));
			if (str.StartsWith("<" + typeof(PerformanceLogMessage).Name))
			{
				using (StringReader textReader = new StringReader(str))
				{
					return (PerformanceLogMessage)xmlSerializer.Deserialize(textReader);
=======
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			XmlSerializer val = new XmlSerializer(typeof(PerformanceLogMessage));
			if (str.StartsWith("<" + typeof(PerformanceLogMessage).Name))
			{
				StringReader val2 = new StringReader(str);
				try
				{
					return (PerformanceLogMessage)val.Deserialize((TextReader)(object)val2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			return null;
		}

		public static PerformanceLogMessage DeserializeCSVline(string propertyNames, string values)
		{
			string[] array = values.Split(new char[1] { ',' });
			string[] array2 = propertyNames.Split(new char[1] { ',' });
<<<<<<< HEAD
			if (array.Count() != array2.Count())
=======
			if (Enumerable.Count<string>((IEnumerable<string>)array) != Enumerable.Count<string>((IEnumerable<string>)array2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return null;
			}
			PerformanceLogMessage performanceLogMessage = new PerformanceLogMessage();
			int num = 0;
			string[] array3 = array;
			foreach (string value in array3)
			{
				PropertyInfo property = typeof(PerformanceLogMessage).GetProperty(array2[num]);
				switch (property.PropertyType.Name)
				{
				case "Single":
					property.SetValue(performanceLogMessage, Convert.ToSingle(value), null);
					break;
				case "Float":
					property.SetValue(performanceLogMessage, Convert.ToDecimal(value), null);
					break;
				case "Int32":
					property.SetValue(performanceLogMessage, Convert.ToInt32(value), null);
					break;
				case "Nullable`1":
					property.SetValue(performanceLogMessage, Convert.ToInt32(value), null);
					break;
				case "Double":
					property.SetValue(performanceLogMessage, Convert.ToDouble(value), null);
					break;
				default:
					property.SetValue(performanceLogMessage, value, null);
					break;
				}
				num++;
			}
			return performanceLogMessage;
		}
	}
}
