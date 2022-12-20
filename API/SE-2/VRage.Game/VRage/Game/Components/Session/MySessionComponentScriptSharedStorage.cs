using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VRage.Game.ObjectBuilders.Components;
using VRage.ObjectBuilder;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.Components.Session
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof(MyObjectBuilder_SharedStorageComponent), null, false)]
	public class MySessionComponentScriptSharedStorage : MySessionComponentBase
	{
		private MyObjectBuilder_SharedStorageComponent m_objectBuilder = new MyObjectBuilder_SharedStorageComponent();

		private static MySessionComponentScriptSharedStorage m_instance;

		public static MySessionComponentScriptSharedStorage Instance => m_instance;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_SharedStorageComponent myObjectBuilder_SharedStorageComponent = (m_objectBuilder = sessionComponent as MyObjectBuilder_SharedStorageComponent);
			m_instance = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_instance = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			return m_objectBuilder;
		}

		public bool Write(string variableName, string secondaryKey, int value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.IntStorage.Dictionary[variableName] = value;
			}
			else
			{
				if (!m_objectBuilder.IntStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.IntStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, int>());
				}
				m_objectBuilder.IntStorageSecondary.Dictionary[variableName][secondaryKey] = value;
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, long value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.LongStorage.Dictionary[variableName] = value;
			}
			else
			{
				if (!m_objectBuilder.LongStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.LongStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, long>());
				}
				m_objectBuilder.LongStorageSecondary.Dictionary[variableName][secondaryKey] = value;
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, ulong value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.ULongStorage.Dictionary[variableName] = value;
			}
			else
			{
				if (!m_objectBuilder.ULongStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.ULongStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, ulong>());
				}
				m_objectBuilder.ULongStorageSecondary.Dictionary[variableName][secondaryKey] = value;
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, bool value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.BoolStorage.Dictionary[variableName] = value;
			}
			else
			{
				if (!m_objectBuilder.BoolStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.BoolStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, bool>());
				}
				m_objectBuilder.BoolStorageSecondary.Dictionary[variableName][secondaryKey] = value;
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, float value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.FloatStorage.Dictionary[variableName] = value;
			}
			else
			{
				if (!m_objectBuilder.FloatStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.FloatStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, float>());
				}
				m_objectBuilder.FloatStorageSecondary.Dictionary[variableName][secondaryKey] = value;
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, string value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.StringStorage.Dictionary[variableName] = value;
			}
			else
			{
				if (!m_objectBuilder.StringStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.StringStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, string>());
				}
				m_objectBuilder.StringStorageSecondary.Dictionary[variableName][secondaryKey] = value;
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, Vector3D value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.Vector3DStorage.Dictionary[variableName] = value;
			}
			else
			{
				if (!m_objectBuilder.Vector3DStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.Vector3DStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, SerializableVector3D>());
				}
				m_objectBuilder.Vector3DStorageSecondary.Dictionary[variableName][secondaryKey] = value;
			}
			return true;
		}

		public int ReadInt(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return -1;
			}
			SerializableDictionary<string, int> value2;
			int value3;
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.IntStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
			}
			else if (m_objectBuilder.IntStorageSecondary.Dictionary.TryGetValue(variableName, out value2) && value2.Dictionary.TryGetValue(secondaryKey, out value3))
			{
				return value3;
			}
			return -1;
		}

		public long ReadLong(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return -1L;
			}
			SerializableDictionary<string, long> value2;
			long value3;
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.LongStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
			}
			else if (m_objectBuilder.LongStorageSecondary.Dictionary.TryGetValue(variableName, out value2) && value2.Dictionary.TryGetValue(secondaryKey, out value3))
			{
				return value3;
			}
			return -1L;
		}

		public ulong ReadULong(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return 0uL;
			}
			SerializableDictionary<string, ulong> value2;
			ulong value3;
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.ULongStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
			}
			else if (m_objectBuilder.ULongStorageSecondary.Dictionary.TryGetValue(variableName, out value2) && value2.Dictionary.TryGetValue(secondaryKey, out value3))
			{
				return value3;
			}
			return 0uL;
		}

		public float ReadFloat(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return 0f;
			}
			SerializableDictionary<string, float> value2;
			float value3;
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.FloatStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
			}
			else if (m_objectBuilder.FloatStorageSecondary.Dictionary.TryGetValue(variableName, out value2) && value2.Dictionary.TryGetValue(secondaryKey, out value3))
			{
				return value3;
			}
			return 0f;
		}

		public string ReadString(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return "";
			}
			SerializableDictionary<string, string> value2;
			string value3;
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.StringStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
			}
			else if (m_objectBuilder.StringStorageSecondary.Dictionary.TryGetValue(variableName, out value2) && value2.Dictionary.TryGetValue(secondaryKey, out value3))
			{
				return value3;
			}
			return "";
		}

		public Vector3D ReadVector3D(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return Vector3D.Zero;
			}
			SerializableDictionary<string, SerializableVector3D> value2;
			SerializableVector3D value3;
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.Vector3DStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
			}
			else if (m_objectBuilder.Vector3DStorageSecondary.Dictionary.TryGetValue(variableName, out value2) && value2.Dictionary.TryGetValue(secondaryKey, out value3))
			{
				return value3;
			}
			return Vector3D.Zero;
		}

		public bool ReadBool(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			SerializableDictionary<string, bool> value2;
			bool value3;
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.BoolStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
			}
			else if (m_objectBuilder.BoolStorageSecondary.Dictionary.TryGetValue(variableName, out value2) && value2.Dictionary.TryGetValue(secondaryKey, out value3))
			{
				return value3;
			}
			return false;
		}

		public bool Write(string variableName, string secondaryKey, List<int> value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.IntListStorage.Dictionary[variableName] = new MySerializableList<int>(value);
			}
			else
			{
				if (!m_objectBuilder.IntListStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.IntListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<int>>());
				}
				m_objectBuilder.IntListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<int>(value);
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, List<long> value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.LongListStorage.Dictionary[variableName] = new MySerializableList<long>(value);
			}
			else
			{
				if (!m_objectBuilder.LongListStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.LongListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<long>>());
				}
				m_objectBuilder.LongListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<long>(value);
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, List<ulong> value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.ULongListStorage.Dictionary[variableName] = new MySerializableList<ulong>(value);
			}
			else
			{
				if (!m_objectBuilder.ULongListStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.ULongListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<ulong>>());
				}
				m_objectBuilder.ULongListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<ulong>(value);
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, List<bool> value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.BoolListStorage.Dictionary[variableName] = new MySerializableList<bool>(value);
			}
			else
			{
				if (!m_objectBuilder.BoolListStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.BoolListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<bool>>());
				}
				m_objectBuilder.BoolListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<bool>(value);
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, List<float> value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.FloatListStorage.Dictionary[variableName] = new MySerializableList<float>(value);
			}
			else
			{
				if (!m_objectBuilder.FloatListStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.FloatListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<float>>());
				}
				m_objectBuilder.FloatListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<float>(value);
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, List<string> value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				m_objectBuilder.StringListStorage.Dictionary[variableName] = new MySerializableList<string>(value);
			}
			else
			{
				if (!m_objectBuilder.StringListStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.StringListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<string>>());
				}
				m_objectBuilder.StringListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<string>(value);
			}
			return true;
		}

		public bool Write(string variableName, string secondaryKey, List<Vector3D> value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
<<<<<<< HEAD
				m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(value.Cast<SerializableVector3D>());
=======
				m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(Enumerable.Cast<SerializableVector3D>((IEnumerable)value));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				if (!m_objectBuilder.Vector3DListStorageSecondary.Dictionary.ContainsKey(variableName))
				{
					m_objectBuilder.Vector3DListStorageSecondary.Dictionary.Add(variableName, new SerializableDictionary<string, MySerializableList<SerializableVector3D>>());
				}
<<<<<<< HEAD
				m_objectBuilder.Vector3DListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<SerializableVector3D>(value.Cast<SerializableVector3D>());
=======
				m_objectBuilder.Vector3DListStorageSecondary.Dictionary[variableName][secondaryKey] = new MySerializableList<SerializableVector3D>(Enumerable.Cast<SerializableVector3D>((IEnumerable)value));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return true;
		}

		public List<int> ReadIntList(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return new List<int>();
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.IntListStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
				MySerializableList<int> mySerializableList = new MySerializableList<int>();
				m_objectBuilder.IntListStorage.Dictionary[variableName] = mySerializableList;
				return mySerializableList;
			}
			if (m_objectBuilder.IntListStorageSecondary.Dictionary.TryGetValue(variableName, out var value2) && value2.Dictionary.TryGetValue(secondaryKey, out var value3))
			{
				return value3;
			}
			return new List<int>();
		}

		public List<long> ReadLongList(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return new List<long>();
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.LongListStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
				MySerializableList<long> mySerializableList = new MySerializableList<long>();
				m_objectBuilder.LongListStorage.Dictionary[variableName] = mySerializableList;
				return mySerializableList;
			}
			if (m_objectBuilder.LongListStorageSecondary.Dictionary.TryGetValue(variableName, out var value2) && value2.Dictionary.TryGetValue(secondaryKey, out var value3))
			{
				return value3;
			}
			return new List<long>();
		}

		public List<ulong> ReadULongList(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return new List<ulong>();
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.ULongListStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
				MySerializableList<ulong> mySerializableList = new MySerializableList<ulong>();
				m_objectBuilder.ULongListStorage.Dictionary[variableName] = mySerializableList;
				return mySerializableList;
			}
			if (m_objectBuilder.ULongListStorageSecondary.Dictionary.TryGetValue(variableName, out var value2) && value2.Dictionary.TryGetValue(secondaryKey, out var value3))
			{
				return value3;
			}
			return new List<ulong>();
		}

		public List<float> ReadFloatList(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return new List<float>();
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.FloatListStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
				MySerializableList<float> mySerializableList = new MySerializableList<float>();
				m_objectBuilder.FloatListStorage.Dictionary[variableName] = mySerializableList;
				return mySerializableList;
			}
			if (m_objectBuilder.FloatListStorageSecondary.Dictionary.TryGetValue(variableName, out var value2) && value2.Dictionary.TryGetValue(secondaryKey, out var value3))
			{
				return value3;
			}
			return new List<float>();
		}

		public List<string> ReadStringList(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return new List<string>();
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.StringListStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
				MySerializableList<string> mySerializableList = new MySerializableList<string>();
				m_objectBuilder.StringListStorage.Dictionary[variableName] = mySerializableList;
				return mySerializableList;
			}
			if (m_objectBuilder.StringListStorageSecondary.Dictionary.TryGetValue(variableName, out var value2) && value2.Dictionary.TryGetValue(secondaryKey, out var value3))
			{
				return value3;
			}
			return new List<string>();
		}

		public List<Vector3D> ReadVector3DList(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return new List<Vector3D>();
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.Vector3DListStorage.Dictionary.TryGetValue(variableName, out var value))
				{
<<<<<<< HEAD
					return value.Cast<Vector3D>().ToList();
				}
				List<Vector3D> list = new List<Vector3D>();
				m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(list.Cast<SerializableVector3D>());
=======
					return Enumerable.ToList<Vector3D>(Enumerable.Cast<Vector3D>((IEnumerable)value));
				}
				List<Vector3D> list = new List<Vector3D>();
				m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(Enumerable.Cast<SerializableVector3D>((IEnumerable)list));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return list;
			}
			if (m_objectBuilder.Vector3DListStorageSecondary.Dictionary.TryGetValue(variableName, out var value2) && value2.Dictionary.TryGetValue(secondaryKey, out var value3))
			{
<<<<<<< HEAD
				return value3.Cast<Vector3D>().ToList();
=======
				return Enumerable.ToList<Vector3D>(Enumerable.Cast<Vector3D>((IEnumerable)value3));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return new List<Vector3D>();
		}

		public List<bool> ReadBoolList(string variableName, string secondaryKey)
		{
			if (m_objectBuilder == null)
			{
				return new List<bool>();
			}
			if (string.IsNullOrEmpty(secondaryKey))
			{
				if (m_objectBuilder.BoolListStorage.Dictionary.TryGetValue(variableName, out var value))
				{
					return value;
				}
				MySerializableList<bool> mySerializableList = new MySerializableList<bool>();
				m_objectBuilder.BoolListStorage.Dictionary[variableName] = mySerializableList;
				return mySerializableList;
			}
			if (m_objectBuilder.BoolListStorageSecondary.Dictionary.TryGetValue(variableName, out var value2) && value2.Dictionary.TryGetValue(secondaryKey, out var value3))
			{
				return value3;
			}
			return new List<bool>();
		}

		public SerializableDictionary<string, bool> GetBools(string secondaryKey = null)
		{
			if (string.IsNullOrEmpty(secondaryKey))
			{
				return m_objectBuilder.BoolStorage;
			}
			if (m_objectBuilder.BoolStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				return value;
			}
			return new SerializableDictionary<string, bool>();
		}

		public SerializableDictionary<string, int> GetInts(string secondaryKey = null)
		{
			if (string.IsNullOrEmpty(secondaryKey))
			{
				return m_objectBuilder.IntStorage;
			}
			if (m_objectBuilder.IntStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				return value;
			}
			return new SerializableDictionary<string, int>();
		}

		public SerializableDictionary<string, long> GetLongs(string secondaryKey = null)
		{
			if (string.IsNullOrEmpty(secondaryKey))
			{
				return m_objectBuilder.LongStorage;
			}
			if (m_objectBuilder.LongStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				return value;
			}
			return new SerializableDictionary<string, long>();
		}

		public SerializableDictionary<string, ulong> GetULongs(string secondaryKey = null)
		{
			if (string.IsNullOrEmpty(secondaryKey))
			{
				return m_objectBuilder.ULongStorage;
			}
			if (m_objectBuilder.ULongStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				return value;
			}
			return new SerializableDictionary<string, ulong>();
		}

		public SerializableDictionary<string, string> GetStrings(string secondaryKey = null)
		{
			if (string.IsNullOrEmpty(secondaryKey))
			{
				return m_objectBuilder.StringStorage;
			}
			if (m_objectBuilder.StringStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				return value;
			}
			return new SerializableDictionary<string, string>();
		}

		public SerializableDictionary<string, float> GetFloats(string secondaryKey = null)
		{
			if (string.IsNullOrEmpty(secondaryKey))
			{
				return m_objectBuilder.FloatStorage;
			}
			if (m_objectBuilder.FloatStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				return value;
			}
			return new SerializableDictionary<string, float>();
		}

		public SerializableDictionary<string, SerializableVector3D> GetVector3D(string secondaryKey = null)
		{
			if (string.IsNullOrEmpty(secondaryKey))
			{
				return m_objectBuilder.Vector3DStorage;
			}
			if (m_objectBuilder.Vector3DStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				return value;
			}
			return new SerializableDictionary<string, SerializableVector3D>();
		}

		public Dictionary<string, bool> GetBoolsByRegex(Regex nameRegex, string secondaryKey = null)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			if (string.IsNullOrEmpty(secondaryKey))
			{
				foreach (KeyValuePair<string, bool> item in m_objectBuilder.BoolStorage.Dictionary)
				{
					if (nameRegex.IsMatch(item.Key))
					{
						dictionary.Add(item.Key, item.Value);
					}
				}
				return dictionary;
			}
			if (m_objectBuilder.BoolStorageSecondary.Dictionary.TryGetValue(secondaryKey, out var value))
			{
				foreach (KeyValuePair<string, bool> item2 in value.Dictionary)
				{
					if (nameRegex.IsMatch(item2.Key))
					{
						dictionary.Add(item2.Key, item2.Value);
					}
				}
				return dictionary;
			}
			return dictionary;
		}
	}
}
