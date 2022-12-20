<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.Components
{
	[MyComponentType(typeof(MyEntityStorageComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_EntityStorageComponent), true)]
	public class MyEntityStorageComponent : MyEntityComponentBase
	{
		private class VRage_Game_Components_MyEntityStorageComponent_003C_003EActor : IActivator, IActivator<MyEntityStorageComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityStorageComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityStorageComponent CreateInstance()
			{
				return new MyEntityStorageComponent();
			}

			MyEntityStorageComponent IActivator<MyEntityStorageComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyObjectBuilder_EntityStorageComponent m_objectBuilder = new MyObjectBuilder_EntityStorageComponent();

		public override string ComponentTypeDebugString => "Entity Storage";

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			return m_objectBuilder;
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			MyObjectBuilder_EntityStorageComponent myObjectBuilder_EntityStorageComponent = (MyObjectBuilder_EntityStorageComponent)builder;
			m_objectBuilder = new MyObjectBuilder_EntityStorageComponent
			{
				BoolStorage = myObjectBuilder_EntityStorageComponent.BoolStorage,
				FloatStorage = myObjectBuilder_EntityStorageComponent.FloatStorage,
				StringStorage = myObjectBuilder_EntityStorageComponent.StringStorage,
				IntStorage = myObjectBuilder_EntityStorageComponent.IntStorage,
				Vector3DStorage = myObjectBuilder_EntityStorageComponent.Vector3DStorage,
				LongStorage = myObjectBuilder_EntityStorageComponent.LongStorage,
				BoolListStorage = myObjectBuilder_EntityStorageComponent.BoolListStorage,
				FloatListStorage = myObjectBuilder_EntityStorageComponent.FloatListStorage,
				StringListStorage = myObjectBuilder_EntityStorageComponent.StringListStorage,
				IntListStorage = myObjectBuilder_EntityStorageComponent.IntListStorage,
				Vector3DListStorage = myObjectBuilder_EntityStorageComponent.Vector3DListStorage,
				LongListStorage = myObjectBuilder_EntityStorageComponent.LongListStorage
			};
		}

		public bool Write(string variableName, int value)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			m_objectBuilder.IntStorage.Dictionary[variableName] = value;
			return true;
		}

		public bool Write(string variableName, long value)
		{
			m_objectBuilder.LongStorage.Dictionary[variableName] = value;
			return true;
		}

		public bool Write(string variableName, bool value)
		{
			m_objectBuilder.BoolStorage.Dictionary[variableName] = value;
			return true;
		}

		public bool Write(string variableName, float value)
		{
			m_objectBuilder.FloatStorage.Dictionary[variableName] = value;
			return true;
		}

		public bool Write(string variableName, string value)
		{
			m_objectBuilder.StringStorage.Dictionary[variableName] = value;
			return true;
		}

		public bool Write(string variableName, Vector3D value)
		{
			m_objectBuilder.Vector3DStorage.Dictionary[variableName] = value;
			return true;
		}

		public int ReadInt(string variableName)
		{
			if (m_objectBuilder.IntStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return -1;
		}

		public long ReadLong(string variableName)
		{
			if (m_objectBuilder.LongStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return -1L;
		}

		public float ReadFloat(string variableName)
		{
			if (m_objectBuilder.FloatStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return 0f;
		}

		public string ReadString(string variableName)
		{
			if (m_objectBuilder.StringStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return null;
		}

		public Vector3D ReadVector3D(string variableName)
		{
			if (m_objectBuilder.Vector3DStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return Vector3D.Zero;
		}

		public bool ReadBool(string variableName)
		{
			if (m_objectBuilder.BoolStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return false;
		}

		public bool Write(string variableName, List<int> value)
		{
			m_objectBuilder.IntListStorage.Dictionary[variableName] = new MySerializableList<int>(value);
			return true;
		}

		public bool Write(string variableName, List<long> value)
		{
			m_objectBuilder.LongListStorage.Dictionary[variableName] = new MySerializableList<long>(value);
			return true;
		}

		public bool Write(string variableName, List<bool> value)
		{
			m_objectBuilder.BoolListStorage.Dictionary[variableName] = new MySerializableList<bool>(value);
			return true;
		}

		public bool Write(string variableName, List<float> value)
		{
			m_objectBuilder.FloatListStorage.Dictionary[variableName] = new MySerializableList<float>(value);
			return true;
		}

		public bool Write(string variableName, List<string> value)
		{
			m_objectBuilder.StringListStorage.Dictionary[variableName] = new MySerializableList<string>(value);
			return true;
		}

		public bool Write(string variableName, List<Vector3D> value)
		{
<<<<<<< HEAD
			m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(value.Cast<SerializableVector3D>());
=======
			m_objectBuilder.Vector3DListStorage.Dictionary[variableName] = new MySerializableList<SerializableVector3D>(Enumerable.Cast<SerializableVector3D>((IEnumerable)value));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return true;
		}

		public List<int> ReadIntList(string variableName)
		{
			if (m_objectBuilder.IntListStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return new List<int>();
		}

		public List<long> ReadLongList(string variableName)
		{
			if (m_objectBuilder.LongListStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return new List<long>();
		}

		public List<float> ReadFloatList(string variableName)
		{
			if (m_objectBuilder.FloatListStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return new List<float>();
		}

		public List<string> ReadStringList(string variableName)
		{
			if (m_objectBuilder.StringListStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return new List<string>();
		}

		public List<Vector3D> ReadVector3DList(string variableName)
		{
			if (m_objectBuilder.Vector3DListStorage.Dictionary.TryGetValue(variableName, out var value))
			{
<<<<<<< HEAD
				return value.Cast<Vector3D>().ToList();
=======
				return Enumerable.ToList<Vector3D>(Enumerable.Cast<Vector3D>((IEnumerable)value));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return new List<Vector3D>();
		}

		public List<bool> ReadBoolList(string variableName)
		{
			if (m_objectBuilder.BoolListStorage.Dictionary.TryGetValue(variableName, out var value))
			{
				return value;
			}
			return new List<bool>();
		}

		public SerializableDictionary<string, bool> GetBools()
		{
			return m_objectBuilder.BoolStorage;
		}

		public SerializableDictionary<string, int> GetInts()
		{
			return m_objectBuilder.IntStorage;
		}

		public SerializableDictionary<string, long> GetLongs()
		{
			return m_objectBuilder.LongStorage;
		}

		public SerializableDictionary<string, string> GetStrings()
		{
			return m_objectBuilder.StringStorage;
		}

		public SerializableDictionary<string, float> GetFloats()
		{
			return m_objectBuilder.FloatStorage;
		}

		public SerializableDictionary<string, SerializableVector3D> GetVector3D()
		{
			return m_objectBuilder.Vector3DStorage;
		}

		public Dictionary<string, bool> GetBoolsByRegex(Regex nameRegex)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (KeyValuePair<string, bool> item in m_objectBuilder.BoolStorage.Dictionary)
			{
				if (nameRegex.IsMatch(item.Key))
				{
					dictionary.Add(item.Key, item.Value);
				}
			}
			return dictionary;
		}
	}
}
