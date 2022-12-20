using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using ProtoBuf;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public class MyConfigBase
	{
		[Serializable]
		[ProtoContract]
		[XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
		public class MyObjectBuilder_ConfigData
		{
			[Serializable]
			[ProtoContract]
			[XmlInclude(typeof(List<string>))]
			[XmlInclude(typeof(MyConfig.MyDebugInputData))]
			[XmlInclude(typeof(MyObjectBuilder_ServerFilterOptions))]
			[XmlInclude(typeof(SerializableDictionary<string, string>))]
			[XmlInclude(typeof(SerializableDictionary<string, MyConfig.MyDebugInputData>))]
			[XmlInclude(typeof(SerializableDictionary<string, SerializableDictionary<string, string>>))]
			public class InnerData
			{
				protected class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EInnerData_003C_003EValue_003C_003EAccessor : IMemberAccessor<InnerData, object>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref InnerData owner, in object value)
					{
						owner.Value = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref InnerData owner, out object value)
					{
						value = owner.Value;
					}
				}

				private class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EInnerData_003C_003EActor : IActivator, IActivator<InnerData>
				{
					private sealed override object CreateInstance()
					{
						return new InnerData();
					}

					object IActivator.CreateInstance()
					{
						//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
						return this.CreateInstance();
					}

					private sealed override InnerData CreateInstance()
					{
						return new InnerData();
					}

					InnerData IActivator<InnerData>.CreateInstance()
					{
						//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
						return this.CreateInstance();
					}
				}

				[ProtoMember(1)]
				public object Value;
			}

			protected class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConfigData, SerializableDictionary<string, InnerData>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyObjectBuilder_ConfigData owner, in SerializableDictionary<string, InnerData> value)
				{
					owner.Data = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyObjectBuilder_ConfigData owner, out SerializableDictionary<string, InnerData> value)
				{
					value = owner.Data;
				}
			}

			private class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ConfigData>
			{
				private sealed override object CreateInstance()
				{
					return new MyObjectBuilder_ConfigData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyObjectBuilder_ConfigData CreateInstance()
				{
					return new MyObjectBuilder_ConfigData();
				}

				MyObjectBuilder_ConfigData IActivator<MyObjectBuilder_ConfigData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(4)]
			public SerializableDictionary<string, InnerData> Data = new SerializableDictionary<string, InnerData>();
		}

		public class ConfigData
		{
			public Dictionary<string, object> Dictionary { get; private set; } = new Dictionary<string, object>();


			public void Init(MyObjectBuilder_ConfigData ob)
			{
<<<<<<< HEAD
				Dictionary = ob?.Data?.Dictionary.ToDictionary((KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData> x) => x.Key, (KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData> x) => x.Value.Value) ?? new Dictionary<string, object>();
=======
				object obj;
				if (ob == null)
				{
					obj = null;
				}
				else
				{
					SerializableDictionary<string, MyObjectBuilder_ConfigData.InnerData> data = ob.Data;
					obj = ((data != null) ? Enumerable.ToDictionary<KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData>, string, object>((IEnumerable<KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData>>)data.Dictionary, (Func<KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData>, string>)((KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData> x) => x.Key), (Func<KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData>, object>)((KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData> x) => x.Value.Value)) : null);
				}
				if (obj == null)
				{
					obj = new Dictionary<string, object>();
				}
				Dictionary = (Dictionary<string, object>)obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			public void InitBackCompatibility(Dictionary<string, object> dictionary)
			{
				Dictionary = dictionary;
			}

			public MyObjectBuilder_ConfigData GetObjectBuilder()
			{
				return new MyObjectBuilder_ConfigData
				{
					Data = new SerializableDictionary<string, MyObjectBuilder_ConfigData.InnerData>(Enumerable.ToDictionary<KeyValuePair<string, object>, string, MyObjectBuilder_ConfigData.InnerData>((IEnumerable<KeyValuePair<string, object>>)Dictionary, (Func<KeyValuePair<string, object>, string>)((KeyValuePair<string, object> x) => x.Key), (Func<KeyValuePair<string, object>, MyObjectBuilder_ConfigData.InnerData>)((KeyValuePair<string, object> x) => new MyObjectBuilder_ConfigData.InnerData
					{
						Value = x.Value
					})))
				};
			}
		}

		protected readonly HashSet<string> RedactedProperties = new HashSet<string>();

		protected readonly ConfigData m_values = new ConfigData();

		private readonly string m_path;

		private readonly string m_fileName;

		private XmlSerializer m_serializer;

		private bool m_isLoaded;

		private XmlSerializer Serializer
		{
			get
			{
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Expected O, but got Unknown
				if (m_serializer == null)
				{
					Attribute customAttribute = Attribute.GetCustomAttribute(typeof(MyObjectBuilder_ConfigData), typeof(XmlSerializerAssemblyAttribute));
					Type type = Assembly.Load(((XmlSerializerAssemblyAttribute)((customAttribute is XmlSerializerAssemblyAttribute) ? customAttribute : null)).get_AssemblyName()).GetType("Microsoft.Xml.Serialization.GeneratedAssembly." + typeof(MyObjectBuilder_ConfigData).Name + "Serializer");
					m_serializer = (XmlSerializer)Activator.CreateInstance(type);
				}
				return m_serializer;
			}
		}

		public event Action OnSettingChanged;

		public MyConfigBase(string fileName)
		{
			m_fileName = fileName;
			m_path = Path.Combine(MyFileSystem.UserDataPath, fileName);
		}

		protected string GetParameterValue(string parameterName)
		{
			try
			{
				if (!m_values.Dictionary.TryGetValue(parameterName, out var value))
				{
					return "";
				}
				return (string)value;
			}
			catch
			{
				return "";
			}
		}

		protected SerializableDictionary<string, TValue> GetParameterValueDictionary<TValue>(string parameterName)
		{
			m_values.Dictionary.TryGetValue(parameterName, out var value);
			SerializableDictionary<string, TValue> result;
			if ((result = value as SerializableDictionary<string, TValue>) != null)
			{
				return result;
			}
			result = new SerializableDictionary<string, TValue>();
			SerializableDictionary<string, object> serializableDictionary;
			if ((serializableDictionary = value as SerializableDictionary<string, object>) != null)
			{
				foreach (KeyValuePair<string, object> item in serializableDictionary.Dictionary)
				{
					object value2;
					object obj;
					if ((value2 = item.Value) is TValue)
					{
						TValue val = (TValue)value2;
						obj = val;
					}
					else
					{
						obj = default(TValue);
					}
					TValue value3 = (TValue)obj;
					result.Dictionary.Add(item.Key, value3);
				}
			}
			m_values.Dictionary[parameterName] = result;
			return result;
		}

		protected T GetParameterValueT<T>(string parameterName)
		{
			try
			{
				if (!m_values.Dictionary.TryGetValue(parameterName, out var value))
				{
					return default(T);
				}
				return (T)value;
			}
			catch
			{
				return default(T);
			}
		}

		private void SetValue(string parameterName, object value)
		{
			if (!m_values.Dictionary.TryGetValue(parameterName, out var value2) || (value != value2 && (value2 == null || !value2.Equals(value))))
			{
				m_values.Dictionary[parameterName] = value;
				this.OnSettingChanged.InvokeIfNotNull();
			}
		}

		protected void SetParameterValue(string parameterName, string value)
		{
			SetValue(parameterName, value);
		}

		protected void SetParameterValue(string parameterName, float value)
		{
			SetValue(parameterName, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		protected void SetParameterValue(string parameterName, bool? value)
		{
			SetValue(parameterName, value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture.NumberFormat) : "");
		}

		protected void SetParameterValue(string parameterName, int value)
		{
			SetValue(parameterName, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		protected void SetParameterValue(string parameterName, int? value)
		{
			SetValue(parameterName, (!value.HasValue) ? "" : value.Value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		protected void SetParameterValue(string parameterName, uint value)
		{
			SetValue(parameterName, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		protected void SetParameterValue(string parameterName, Vector3I value)
		{
			SetParameterValue(parameterName, $"{value.X}, {value.Y}, {value.Z}");
		}

		protected void RemoveParameterValue(string parameterName)
		{
			if (m_values.Dictionary.Remove(parameterName))
			{
				this.OnSettingChanged.InvokeIfNotNull();
			}
		}

		protected T? GetOptionalEnum<T>(string name) where T : struct, IComparable, IFormattable, IConvertible
		{
			int? intFromString = MyUtils.GetIntFromString(GetParameterValue(name));
			if (intFromString.HasValue && Enum.IsDefined(typeof(T), intFromString.Value))
			{
				return (T)(object)intFromString.Value;
			}
			return null;
		}

		protected void SetOptionalEnum<T>(string name, T? value) where T : struct, IComparable, IFormattable, IConvertible
		{
			if (value.HasValue)
			{
				SetParameterValue(name, (int)(object)value.Value);
			}
			else
			{
				RemoveParameterValue(name);
			}
		}

		public void Save()
		{
<<<<<<< HEAD
=======
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Sandbox.Engine.Platform.Game.IsDedicated || !m_isLoaded)
			{
				return;
			}
			MySandboxGame.Log.WriteLine("MyConfig.Save() - START");
			MySandboxGame.Log.IncreaseIndent();
			try
			{
				MySandboxGame.Log.WriteLine("Path: " + m_path, LoggingOptions.CONFIG_ACCESS);
				try
				{
<<<<<<< HEAD
					using (MemoryStream memoryStream = new MemoryStream())
					{
						XmlWriterSettings settings = new XmlWriterSettings
						{
							Indent = true,
							NewLineHandling = NewLineHandling.None
						};
						using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
						{
							Serializer.Serialize(xmlWriter, m_values.GetObjectBuilder());
						}
						byte[] array = memoryStream.ToArray();
						if (MyPlatformGameSettings.GAME_CONFIG_TO_CLOUD)
						{
							CloudResult cloudResult = MyGameService.SaveToCloud("Config/cloud/" + m_fileName, array);
							if (cloudResult != 0)
							{
								MySandboxGame.Log.WriteLine(string.Concat("SaveToCloud failed: ", cloudResult, ", UserId: ", EndpointId.Format(MyGameService.UserId)));
							}
=======
					using MemoryStream memoryStream = new MemoryStream();
					XmlWriterSettings val = new XmlWriterSettings();
					val.set_Indent(true);
					val.set_NewLineHandling((NewLineHandling)2);
					XmlWriterSettings val2 = val;
					XmlWriter val3 = XmlWriter.Create((Stream)memoryStream, val2);
					try
					{
						Serializer.Serialize(val3, (object)m_values.GetObjectBuilder());
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
					byte[] array = memoryStream.ToArray();
					if (MyPlatformGameSettings.GAME_CONFIG_TO_CLOUD)
					{
						CloudResult cloudResult = MyGameService.SaveToCloud("Config/cloud/" + m_fileName, array);
						if (cloudResult != 0)
						{
							MySandboxGame.Log.WriteLine(string.Concat("SaveToCloud failed: ", cloudResult, ", UserId: ", EndpointId.Format(MyGameService.UserId)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						File.WriteAllBytes(m_path, array);
					}
<<<<<<< HEAD
=======
					File.WriteAllBytes(m_path, array);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				catch (Exception ex)
				{
					MySandboxGame.Log.WriteLine("Exception occured, but application is continuing. Exception: " + ex);
				}
			}
			finally
			{
				MySandboxGame.Log.DecreaseIndent();
				MySandboxGame.Log.WriteLine("MyConfig.Save() - END");
			}
		}

		protected virtual void NewConfigWasStarted()
		{
		}

		public void LoadFromCloud(bool syncLoad, Action onDone)
		{
			try
			{
				MySandboxGame.Log.WriteLine("MyConfig.LoadFromCloud()");
				string text = "Config/cloud/" + m_fileName;
				MySandboxGame.Log.WriteLine("Cloud Config Path: " + text);
				if (syncLoad)
				{
					OnDataReceived(MyGameService.LoadFromCloud(text));
				}
				else
				{
					MyGameService.LoadFromCloudAsync(text, OnDataReceived);
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine(ex);
			}
			void OnDataReceived(byte[] data)
			{
				try
				{
					MySandboxGame.Log.WriteLine("Cloud Config received: " + (data != null));
					if (data != null)
<<<<<<< HEAD
					{
						File.WriteAllBytes(m_path, data);
					}
					else
					{
						File.Delete(m_path);
					}
=======
					{
						File.WriteAllBytes(m_path, data);
					}
					else
					{
						File.Delete(m_path);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Load();
				}
				catch (Exception ex2)
				{
					MySandboxGame.Log.WriteLine(ex2);
				}
				onDone.InvokeIfNotNull();
			}
		}

		public void Clear()
		{
			m_values.Dictionary.Clear();
			m_isLoaded = false;
		}

		public void Load()
		{
<<<<<<< HEAD
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			MySandboxGame.Log.WriteLine("MyConfig.Load() - START");
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.CONFIG_ACCESS))
			{
=======
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			MySandboxGame.Log.WriteLine("MyConfig.Load() - START");
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.CONFIG_ACCESS))
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MySandboxGame.Log.WriteLine("Path: " + m_path, LoggingOptions.CONFIG_ACCESS);
				string msg = "";
				try
				{
					Clear();
					if (!File.Exists(m_path))
<<<<<<< HEAD
					{
						MySandboxGame.Log.WriteLine("Config file not found! " + m_path);
						NewConfigWasStarted();
					}
					else
					{
						using (Stream input = MyFileSystem.OpenRead(m_path))
						{
							using (XmlReader xmlReader = XmlReader.Create(input))
							{
								try
								{
									m_values.Init((MyObjectBuilder_ConfigData)Serializer.Deserialize(xmlReader));
								}
								catch (InvalidOperationException)
								{
									SerializableDictionary<string, object> serializableDictionary = (SerializableDictionary<string, object>)new XmlSerializer(typeof(SerializableDictionary<string, object>), new Type[5]
									{
										typeof(SerializableDictionary<string, string>),
										typeof(List<string>),
										typeof(SerializableDictionary<string, MyConfig.MyDebugInputData>),
										typeof(MyConfig.MyDebugInputData),
										typeof(MyObjectBuilder_ServerFilterOptions)
									}).Deserialize(xmlReader);
									m_values.InitBackCompatibility(serializableDictionary.Dictionary);
								}
							}
=======
					{
						MySandboxGame.Log.WriteLine("Config file not found! " + m_path);
						NewConfigWasStarted();
					}
					else
					{
						using Stream stream = MyFileSystem.OpenRead(m_path);
						XmlReader val = XmlReader.Create(stream);
						try
						{
							m_values.Init((MyObjectBuilder_ConfigData)Serializer.Deserialize(val));
						}
						catch (InvalidOperationException)
						{
							SerializableDictionary<string, object> serializableDictionary = (SerializableDictionary<string, object>)new XmlSerializer(typeof(SerializableDictionary<string, object>), new Type[5]
							{
								typeof(SerializableDictionary<string, string>),
								typeof(List<string>),
								typeof(SerializableDictionary<string, MyConfig.MyDebugInputData>),
								typeof(MyConfig.MyDebugInputData),
								typeof(MyObjectBuilder_ServerFilterOptions)
							}).Deserialize(val);
							m_values.InitBackCompatibility(serializableDictionary.Dictionary);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
					}
				}
				catch (Exception ex2)
				{
					MySandboxGame.Log.WriteLine("Exception occurred, but application is continuing. Exception: " + ex2);
					MySandboxGame.Log.WriteLine("Config:");
					MySandboxGame.Log.WriteLine(msg);
				}
				foreach (KeyValuePair<string, object> item in m_values.Dictionary)
				{
					if (item.Value == null)
					{
						MySandboxGame.Log.WriteLine("ERROR: " + item.Key + " is null!", LoggingOptions.CONFIG_ACCESS);
					}
<<<<<<< HEAD
				}
				catch (Exception ex2)
				{
					MySandboxGame.Log.WriteLine("Exception occurred, but application is continuing. Exception: " + ex2);
					MySandboxGame.Log.WriteLine("Config:");
					MySandboxGame.Log.WriteLine(msg);
				}
				foreach (KeyValuePair<string, object> item in m_values.Dictionary)
				{
					if (item.Value == null)
					{
						MySandboxGame.Log.WriteLine("ERROR: " + item.Key + " is null!", LoggingOptions.CONFIG_ACCESS);
					}
					else if (RedactedProperties.Contains(item.Key))
					{
						MySandboxGame.Log.WriteLine(item.Key + ": [REDACTED]", LoggingOptions.CONFIG_ACCESS);
					}
					else
					{
=======
					else if (RedactedProperties.Contains(item.Key))
					{
						MySandboxGame.Log.WriteLine(item.Key + ": [REDACTED]", LoggingOptions.CONFIG_ACCESS);
					}
					else
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						MySandboxGame.Log.WriteLine(item.Key + ": " + item.Value, LoggingOptions.CONFIG_ACCESS);
					}
				}
			}
			MySandboxGame.Log.WriteLine("MyConfig.Load() - END");
			m_isLoaded = true;
			this.OnSettingChanged.InvokeIfNotNull();
		}
	}
}
