using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ProtoBuf;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyCachedServerItem
	{
		[ProtoContract]
		public class MyServerData
		{
			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003ESettings_003C_003EAccessor : IMemberAccessor<MyServerData, MyObjectBuilder_SessionSettings>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in MyObjectBuilder_SessionSettings value)
				{
					owner.Settings = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out MyObjectBuilder_SessionSettings value)
				{
					value = owner.Settings;
				}
			}

			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EExperimentalMode_003C_003EAccessor : IMemberAccessor<MyServerData, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in bool value)
				{
					owner.ExperimentalMode = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out bool value)
				{
					value = owner.ExperimentalMode;
				}
			}

			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EMods_003C_003EAccessor : IMemberAccessor<MyServerData, List<WorkshopId>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in List<WorkshopId> value)
				{
					owner.Mods = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out List<WorkshopId> value)
				{
					value = owner.Mods;
				}
			}

			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyServerData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in string value)
				{
					owner.Description = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out string value)
				{
					value = owner.Description;
				}
			}

			protected class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EUsedServices_003C_003EAccessor : IMemberAccessor<MyServerData, List<string>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyServerData owner, in List<string> value)
				{
					owner.UsedServices = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyServerData owner, out List<string> value)
				{
					value = owner.UsedServices;
				}
			}

			private class Sandbox_Game_MyCachedServerItem_003C_003EMyServerData_003C_003EActor : IActivator, IActivator<MyServerData>
			{
				private sealed override object CreateInstance()
				{
					return new MyServerData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyServerData CreateInstance()
				{
					return new MyServerData();
				}

				MyServerData IActivator<MyServerData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public MyObjectBuilder_SessionSettings Settings;

			[ProtoMember(4)]
			public bool ExperimentalMode;

			[ProtoMember(7)]
			public List<WorkshopId> Mods = new List<WorkshopId>();

			[ProtoMember(10)]
			public string Description;

			[ProtoMember(14)]
			public List<string> UsedServices = new List<string>();
		}

		public readonly bool AllowedInGroup;

		public readonly MyGameServerItem Server;

		public Dictionary<string, string> Rules;

		private MyServerData m_data = new MyServerData();

		private const int RULE_LENGTH = 93;

		public MyObjectBuilder_SessionSettings Settings => m_data.Settings;

		public bool ExperimentalMode => m_data.ExperimentalMode;

		public List<string> UsedServices => m_data.UsedServices;

		public string Description => m_data.Description;

		public List<WorkshopId> Mods => m_data.Mods;

		public MyCachedServerItem()
		{
		}

		public MyCachedServerItem(MyGameServerItem server)
		{
			Server = server;
			Rules = null;
			ulong gameTagByPrefixUlong = server.GetGameTagByPrefixUlong("groupId");
			AllowedInGroup = gameTagByPrefixUlong == 0L || MyGameService.IsUserInGroup(gameTagByPrefixUlong);
		}

		private static byte[] GetServerData()
		{
			int num = 1018;
			int num2 = 90;
			int num3 = 10;
			int num4 = 5;
			MyObjectBuilder_SessionSettings myObjectBuilder_SessionSettings = (MyObjectBuilder_SessionSettings)MySession.Static.Settings.Clone();
			List<string> usedServices = MySession.Static.Mods.Select((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedServiceName).Distinct().ToList();
			SerializableDictionary<string, short> serializableDictionary = new SerializableDictionary<string, short>(new Dictionary<string, short>(myObjectBuilder_SessionSettings.BlockTypeLimits.Dictionary));
			List<WorkshopId> list = MySession.Static.Mods.Select((MyObjectBuilder_Checkpoint.ModItem m) => new WorkshopId(m.PublishedFileId, m.PublishedServiceName)).Distinct().ToList();
			string text = ((MySandboxGame.ConfigDedicated == null) ? null : MySandboxGame.ConfigDedicated.ServerDescription);
			if (text != null && text.Length > 128)
			{
				text = text.Substring(0, 128);
			}
			if (list.Count > num2)
			{
				list.RemoveRange(num2, list.Count - num2);
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
<<<<<<< HEAD
				byte[] array;
				while (true)
				{
					MyServerData myServerData = new MyServerData
					{
						Settings = myObjectBuilder_SessionSettings,
						ExperimentalMode = MySession.Static.IsSettingsExperimental(),
						Description = text,
						Mods = list,
						UsedServices = usedServices
					};
					myServerData.Settings.BlockTypeLimits = serializableDictionary;
					Serializer.Serialize(memoryStream, myServerData);
					array = MyCompression.Compress(memoryStream.ToArray());
					if (array.Length < num || (list.Count == 0 && serializableDictionary.Dictionary.Count == 0))
					{
						break;
					}
					memoryStream.SetLength(0L);
					memoryStream.Position = 0L;
					if (list.Count > 0)
					{
						int index = Math.Max(list.Count - num3, 0);
						int count = Math.Min(num3, list.Count);
						list.RemoveRange(index, count);
					}
					else if (serializableDictionary.Dictionary.Count > 0)
					{
						new Dictionary<string, short>(serializableDictionary.Dictionary);
						int num5 = num4;
						while (serializableDictionary.Dictionary.Count > 0 && num5-- > 0)
						{
							KeyValuePair<string, short> keyValuePair = serializableDictionary.Dictionary.First();
							serializableDictionary.Dictionary.Remove(keyValuePair.Key);
						}
					}
				}
				return array;
			}
		}

		public static void SendSettingsToSteam()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated || MyGameService.GameServer == null)
			{
				return;
=======
				SerializableDictionary<string, short> blockTypeLimits = MySession.Static.Settings.BlockTypeLimits;
				MyServerData myServerData = new MyServerData
				{
					Settings = MySession.Static.Settings,
					ExperimentalMode = MySession.Static.IsSettingsExperimental(),
					Mods = Enumerable.ToList<WorkshopId>(Enumerable.Distinct<WorkshopId>(Enumerable.Select<MyObjectBuilder_Checkpoint.ModItem, WorkshopId>(Enumerable.Where<MyObjectBuilder_Checkpoint.ModItem>((IEnumerable<MyObjectBuilder_Checkpoint.ModItem>)MySession.Static.Mods, (Func<MyObjectBuilder_Checkpoint.ModItem, bool>)((MyObjectBuilder_Checkpoint.ModItem m) => !m.IsDependency)), (Func<MyObjectBuilder_Checkpoint.ModItem, WorkshopId>)((MyObjectBuilder_Checkpoint.ModItem m) => new WorkshopId(m.PublishedFileId, m.PublishedServiceName))))),
					Description = ((MySandboxGame.ConfigDedicated == null) ? null : MySandboxGame.ConfigDedicated.ServerDescription)
				};
				myServerData.Settings.BlockTypeLimits = new SerializableDictionary<string, short>();
				Serializer.Serialize((Stream)memoryStream, myServerData);
				array = MyCompression.Compress(memoryStream.ToArray());
				myServerData.Settings.BlockTypeLimits = blockTypeLimits;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			byte[] serverData = GetServerData();
			MyGameService.GameServer.SetKeyValue("sc", serverData.Length.ToString());
			for (int i = 0; (double)i < Math.Ceiling((double)serverData.Length / 93.0); i++)
			{
				byte[] array = new byte[93];
				int num = serverData.Length - 93 * i;
				if (num >= 93)
				{
					Array.Copy(serverData, i * 93, array, 0, 93);
				}
				else
				{
					array = new byte[num];
					Array.Copy(serverData, i * 93, array, 0, num);
				}
				MyGameService.GameServer.SetKeyValue("sc" + i, Convert.ToBase64String(array));
			}
		}

		public void DeserializeSettings()
		{
			string value = null;
			try
			{
				if (Rules.TryGetValue("sc", out value))
				{
					int num = int.Parse(value);
					byte[] array = new byte[num];
					for (int i = 0; (double)i < Math.Ceiling((double)num / 93.0); i++)
					{
						byte[] array2 = Convert.FromBase64String(Rules["sc" + i]);
						Array.Copy(array2, 0, array, i * 93, array2.Length);
					}
					using MemoryStream source = new MemoryStream(MyCompression.Decompress(array));
					m_data = Serializer.Deserialize<MyServerData>(source);
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLineAndConsole("Failed to deserialize session settings for server!");
				MyLog.Default.WriteLineAndConsole(value);
				MyLog.Default.WriteLineAndConsole(ex.ToString());
			}
		}
	}
}
