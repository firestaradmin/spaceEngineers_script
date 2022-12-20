using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty
	{
		protected static int GlobalKeyCounter;
	}
	public abstract class MyAnimatedProperty<T> : MyAnimatedProperty, IMyAnimatedProperty<T>, IMyAnimatedProperty, IMyConstProperty
	{
		public struct ValueHolder
		{
			public int ID;

			public float PrecomputedDiff;

			public float Time;

			public T Value;

			public ValueHolder(int id, float time, T value, float diff)
			{
				ID = id;
				Time = time;
				Value = value;
				PrecomputedDiff = diff;
			}

			public ValueHolder Duplicate()
			{
				ValueHolder valueHolder = default(ValueHolder);
				valueHolder.Time = Time;
				valueHolder.PrecomputedDiff = PrecomputedDiff;
				valueHolder.ID = ID;
				ValueHolder result = valueHolder;
				if (Value is IMyConstProperty)
				{
					result.Value = (T)((IMyConstProperty)(object)Value).Duplicate();
				}
				else
				{
					result.Value = Value;
				}
				return result;
			}
		}

		private class MyKeysComparer : IComparer<ValueHolder>
		{
			public int Compare(ValueHolder x, ValueHolder y)
			{
				return x.Time.CompareTo(y.Time).CompareTo(x.ID.CompareTo(y.ID));
			}
		}

		private string m_name;

		private string m_description;

		protected ValueHolder[] m_defaultKeys = Array.Empty<ValueHolder>();

		private ValueHolder[] m_keys = Array.Empty<ValueHolder>();

		private byte m_keyCount;

		private readonly bool m_interpolateAfterEnd;

		/// <summary>
		/// Name of this key's type.
		/// </summary>
		protected static string KeyTypeName;

		/// <summary>
		/// Whether the key type is blittable and can be left uninitialized or uncleared.
		/// </summary>
		protected static bool IsBlittable;

		/// <summary>
		/// Comparer used when sorting the array elements.
		/// </summary>
		private static readonly MyKeysComparer KeysComparer;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public string Description => m_description;

		public virtual string ValueType => KeyTypeName;

		public virtual string BaseValueType => ValueType;

		public virtual bool Animated => true;

		public virtual bool Is2D => false;

		bool IMyConstProperty.IsDefault
		{
			get
			{
				if (m_keyCount != m_defaultKeys.Length)
				{
					return false;
				}
				for (int i = 0; i < m_keyCount; i++)
				{
					if (m_defaultKeys[i].Time != m_keys[i].Time || !EqualityComparer<T>.Default.Equals(m_defaultKeys[i].Value, m_keys[i].Value))
					{
						return false;
					}
				}
				return true;
			}
		}

		static MyAnimatedProperty()
		{
			KeysComparer = new MyKeysComparer();
			KeyTypeName = typeof(T).Name;
			try
			{
				if (default(T) != null)
				{
					GCHandle.Alloc(default(T), GCHandleType.Pinned).Free();
					IsBlittable = true;
				}
			}
			catch
			{
			}
		}

		public MyAnimatedProperty()
		{
			Init();
		}

		public MyAnimatedProperty(string name, string description, bool interpolateAfterEnd)
			: this()
		{
			m_name = name;
			m_description = description;
			m_interpolateAfterEnd = interpolateAfterEnd;
		}

		protected virtual void Init()
		{
		}

		public void SetValue(object val)
		{
		}

		public void SetDefaultValue(object val = null)
		{
			Array.Resize(ref m_defaultKeys, m_keys.Length);
			for (int i = 0; i < m_keyCount; i++)
			{
				if (m_keys[i].Value is IMyConstProperty)
				{
					((IMyConstProperty)(object)m_keys[i].Value).SetDefaultValue((IMyConstProperty)(object)m_keys[i].Value);
				}
				m_defaultKeys[i] = m_keys[i].Duplicate();
			}
		}

		public void SetValue(T val)
		{
		}

		object IMyConstProperty.GetValue()
		{
			return null;
		}

		object IMyConstProperty.GetDefaultValue()
		{
			return null;
		}

		public U GetValue<U>()
		{
			return default(U);
		}

		void IMyAnimatedProperty.GetInterpolatedValue(float time, out object value)
		{
			GetInterpolatedValue(time, out var value2);
			value = value2;
		}

		public void GetInterpolatedValue(float time, out T value)
		{
			if (m_keyCount == 0)
			{
				value = default(T);
			}
			else if (m_keyCount == 1)
			{
				value = m_keys[0].Value;
			}
			else if (time > m_keys[m_keyCount - 1].Time)
			{
				if (m_interpolateAfterEnd)
				{
					GetPreviousValue(m_keys[m_keyCount - 1].Time, out var previousValue, out var previousTime);
					GetNextValue(time, out var nextValue, out var _, out var difference);
					Interpolate(in previousValue, in nextValue, (time - previousTime) * difference, out value);
				}
				else
				{
					value = m_keys[m_keyCount - 1].Value;
				}
			}
			else
			{
				GetInterval(time, out var values, out var times, out var difference2);
				if (times.Item1 == time)
				{
					(value, _) = values;
				}
				else
				{
					Interpolate(in values.Item1, in values.Item2, (time - times.Item1) * difference2, out value);
				}
			}
		}

		public void GetPreviousValue(float time, out T previousValue, out float previousTime)
		{
			int num = Math.Max(GetUpperBoundary(time) - 1, 0);
			ref ValueHolder reference = ref m_keys[num];
			previousValue = reference.Value;
			previousTime = reference.Time;
		}

		public void GetNextValue(float time, out T nextValue, out float nextTime, out float difference)
		{
			int num = Math.Min(GetUpperBoundary(time), m_keyCount - 1);
			ref ValueHolder reference = ref m_keys[num];
			nextValue = reference.Value;
			nextTime = reference.Time;
			difference = reference.PrecomputedDiff;
		}

		public void GetInterval(float time, out (T, T) values, out (float, float) times, out float difference)
		{
			int upperBoundary = GetUpperBoundary(time);
			int num = Math.Max(upperBoundary - 1, 0);
			int num2 = Math.Max(Math.Min(upperBoundary, m_keyCount - 1), 0);
			values = (m_keys[num].Value, m_keys[num2].Value);
			times = (m_keys[num].Time, m_keys[num2].Time);
			difference = m_keys[num2].PrecomputedDiff;
		}

		/// <summary>
		/// Binary search for the index of the first key greater than the provided time index.
		/// </summary>
		/// <remarks>If the time value is greater than all keys in this property the length of the key list is returned.</remarks>
		/// <param name="time"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetUpperBoundary(float time)
		{
			if (m_keyCount == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = m_keyCount;
			while (num2 - num > 1)
			{
				int num3 = (num + num2) / 2;
				if (time >= m_keys[num3].Time)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			int result = num;
			if (time >= m_keys[num].Time)
			{
				result = num2;
			}
			return result;
		}

		void IMyAnimatedProperty.GetKey(int index, out float time, out object value)
		{
			GetKey(index, out time, out var value2);
			value = value2;
		}

		void IMyAnimatedProperty.GetKey(int index, out int id, out float time, out object value)
		{
			GetKey(index, out id, out time, out var value2);
			value = value2;
		}

		void IMyAnimatedProperty.GetKeyByID(int id, out float time, out object value)
		{
			GetKeyByID(id, out time, out var value2);
			value = value2;
		}

		void IMyAnimatedProperty.SetKey(int index, float time, object value)
		{
			ValueHolder valueHolder = m_keys[index];
			valueHolder.Time = time;
			valueHolder.Value = (T)value;
			m_keys[index] = valueHolder;
			UpdateDiff(index - 1);
			UpdateDiff(index);
			UpdateDiff(index + 1);
			Array.Sort(m_keys, KeysComparer);
		}

		void IMyAnimatedProperty.SetKey(int index, float time)
		{
			ValueHolder valueHolder = m_keys[index];
			valueHolder.Time = time;
			m_keys[index] = valueHolder;
			UpdateDiff(index - 1);
			UpdateDiff(index);
			UpdateDiff(index + 1);
			Array.Sort(m_keys, KeysComparer);
		}

		void IMyAnimatedProperty.SetKeyByID(int id, float time, object value)
		{
			int num = -1;
			num = GetKeyIndex(id);
			if (num == -1)
			{
				num = m_keyCount;
				int index = num;
				ValueHolder value2 = new ValueHolder(id, time, (T)value, 0f);
				InsertKey(index, in value2);
			}
			else
			{
				m_keys[num].Time = time;
				m_keys[num].Value = (T)value;
			}
			UpdateDiff(num - 1);
			UpdateDiff(num);
			UpdateDiff(num + 1);
			Array.Sort(m_keys, KeysComparer);
		}

		void IMyAnimatedProperty.SetKeyByID(int id, float time)
		{
			int keyIndex = GetKeyIndex(id);
			m_keys[keyIndex].Time = time;
			UpdateDiff(keyIndex - 1);
			UpdateDiff(keyIndex);
			UpdateDiff(keyIndex + 1);
			Array.Sort(m_keys, KeysComparer);
		}

		public void CopyKeys(MyAnimatedProperty<T> source)
		{
			byte keyCount = source.m_keyCount;
			if (m_keys.Length < source.m_keys.Length)
			{
				Array.Resize(ref m_keys, source.m_keys.Length);
			}
			for (int i = 0; i < m_keys.Length; i++)
			{
				m_keys[i] = source.m_keys[i].Duplicate();
			}
			m_keyCount = keyCount;
			Array.Resize(ref m_defaultKeys, source.m_defaultKeys.Length);
			for (int j = 0; j < source.m_defaultKeys.Length; j++)
			{
				m_defaultKeys[j] = source.m_defaultKeys[j].Duplicate();
			}
		}

		public int AddKey(float time, T val)
		{
			int num;
			if (m_keyCount > 0 && m_keys[m_keyCount - 1].Time <= time)
			{
				num = GetUpperBoundary(time);
				if (num <= m_keyCount && m_keys[num - 1].Time == time)
				{
					m_keys[num - 1].Value = val;
					return m_keys[num - 1].ID;
				}
			}
			else
			{
				num = m_keyCount;
			}
			ValueHolder value = new ValueHolder(++MyAnimatedProperty.GlobalKeyCounter, time, val, 0f);
			InsertKey(num, in value);
			if (num > 0)
			{
				UpdateDiff(num);
			}
			return value.ID;
		}

		private void InsertKey(int index, in ValueHolder value)
		{
			if (m_keyCount == m_keys.Length)
			{
				int newSize = ((m_keyCount >= 4) ? (m_keyCount * 2) : 4);
				Array.Resize(ref m_keys, newSize);
			}
			if (index < m_keyCount)
			{
				Array.Copy(m_keys, index, m_keys, index + 1, m_keyCount - index);
			}
			m_keys[index] = value;
			m_keyCount++;
		}

		private void UpdateDiff(int index)
		{
			if (index >= 1 && index < m_keyCount)
			{
				float time = m_keys[index].Time;
				float time2 = m_keys[index - 1].Time;
				m_keys[index] = new ValueHolder(m_keys[index].ID, time, m_keys[index].Value, 1f / (time - time2));
			}
		}

		int IMyAnimatedProperty.AddKey(float time, object val)
		{
			return AddKey(time, (T)val);
		}

		public void RemoveKey(float time)
		{
			for (int i = 0; i < m_keyCount; i++)
			{
				if (m_keys[i].Time == time)
				{
					RemoveKey(i);
					break;
				}
			}
		}

		void IMyAnimatedProperty.RemoveKey(int index)
		{
			RemoveKey(index);
		}

		void IMyAnimatedProperty.RemoveKeyByID(int id)
		{
			int keyIndex = GetKeyIndex(id);
			if (keyIndex != -1)
			{
				RemoveKey(keyIndex);
			}
		}

		private void RemoveKey(int index)
		{
			m_keyCount--;
			if (index < m_keyCount)
			{
				Array.Copy(m_keys, index + 1, m_keys, index, m_keyCount - index);
			}
			if (IsBlittable)
			{
				m_keys[m_keyCount] = default(ValueHolder);
			}
			UpdateDiff(index);
		}

		public void ClearKeys()
		{
			if (!IsBlittable)
			{
				Array.Clear(m_keys, 0, m_keyCount);
			}
			m_keyCount = 0;
		}

		public void GetKey(int index, out float time, out T value)
		{
			time = m_keys[index].Time;
			value = m_keys[index].Value;
		}

		public void GetKey(int index, out int id, out float time, out T value)
		{
			id = m_keys[index].ID;
			time = m_keys[index].Time;
			value = m_keys[index].Value;
		}

		public void GetKeyByID(int id, out float time, out T value)
		{
			int keyIndex = GetKeyIndex(id);
			if (keyIndex >= 0)
			{
				time = m_keys[keyIndex].Time;
				value = m_keys[keyIndex].Value;
			}
			else
			{
				time = 0f;
				value = default(T);
			}
		}

		/// <summary>
		/// Get the index of a key by it's id.
		/// </summary>
		/// <param name="keyId">The id of the key.</param>
		/// <returns></returns>
		public int GetKeyIndex(int keyId)
		{
			for (int i = 0; i < m_keyCount; i++)
			{
				if (m_keys[i].ID == keyId)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetKeysCount()
		{
			return m_keyCount;
		}

		public virtual IMyConstProperty Duplicate()
		{
			return null;
		}

		protected virtual void Duplicate(IMyConstProperty targetProp)
		{
			(targetProp as MyAnimatedProperty<T>).CopyKeys(this);
		}

		Type IMyConstProperty.GetValueType()
		{
			return typeof(T);
		}

		public virtual void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Keys");
			ValueHolder[] keys = m_keys;
			for (int i = 0; i < keys.Length; i++)
			{
				ValueHolder valueHolder = keys[i];
				writer.WriteStartElement("Key");
				float time = valueHolder.Time;
				writer.WriteElementString("Time", time.ToString(CultureInfo.InvariantCulture));
				if (Is2D)
				{
					writer.WriteStartElement("Value2D");
				}
				else
				{
					writer.WriteStartElement("Value" + ValueType);
				}
				SerializeValue(writer, valueHolder.Value);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		public virtual void Deserialize(XmlReader reader)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Invalid comparison between Unknown and I4
			m_name = reader.GetAttribute("name");
			reader.ReadStartElement();
			ClearKeys();
<<<<<<< HEAD
			bool isEmptyElement = reader.IsEmptyElement;
=======
			bool isEmptyElement = reader.get_IsEmptyElement();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			reader.ReadStartElement();
			while ((int)reader.get_NodeType() != 15)
			{
				reader.ReadStartElement();
				float time = reader.ReadElementContentAsFloat();
				reader.ReadStartElement();
				DeserializeValue(reader, out var value);
				reader.ReadEndElement();
				AddKey(time, (T)value);
				reader.ReadEndElement();
			}
			if (!isEmptyElement)
			{
				reader.ReadEndElement();
			}
			reader.ReadEndElement();
		}

		public virtual GenerationProperty SerializeToObjectBuilder()
		{
			GenerationProperty generationProperty = new GenerationProperty();
			generationProperty.Name = m_name;
			generationProperty.Type = ValueType;
			generationProperty.AnimationType = PropertyAnimationType.Animated;
			string type = "Int";
			switch (typeof(T).Name)
			{
			case "Single":
				type = "Float";
				break;
			case "Vector3":
				type = "Vector3";
				break;
			case "Vector4":
				type = "Vector4";
				break;
			case "Bool":
				type = "Bool";
				break;
			case "MyTransparentMaterial":
				type = "MyTransparentMaterial";
				break;
			case "String":
				type = "String";
				break;
			}
			generationProperty.Keys = SerializeKeys(type);
			return generationProperty;
		}

		public List<AnimationKey> SerializeKeys(string type)
		{
			List<AnimationKey> list = new List<AnimationKey>();
			AnimationKey item = default(AnimationKey);
			for (int i = 0; i < GetKeysCount(); i++)
			{
				GetKey(i, out var _, out var time, out var value);
				item.Time = time;
				item.ValueType = type;
				switch (type)
				{
				case "Float":
					item.ValueFloat = (float)(object)value;
					break;
				case "Vector3":
					item.ValueVector3 = (Vector3)(object)value;
					break;
				case "Vector4":
					item.ValueVector4 = (Vector4)(object)value;
					break;
				default:
					item.ValueInt = (int)(object)value;
					break;
				case "Bool":
					item.ValueBool = (bool)(object)value;
					break;
				case "MyTransparentMaterial":
					item.ValueString = ((MyTransparentMaterial)(object)value).Id.String;
					break;
				case "String":
					item.ValueString = (string)(object)value;
					break;
				}
				list.Add(item);
			}
			return list;
		}

		public void DeserializeFromObjectBuilder_Animation(Generation2DProperty property, string type)
		{
			DeserializeKeys(property.Keys, type);
		}

		public virtual void DeserializeFromObjectBuilder(GenerationProperty property)
		{
			m_name = property.Name;
			KeyTypeName = property.Type;
			DeserializeKeys(property.Keys, property.Type);
		}

		public void DeserializeKeys(List<AnimationKey> keys, string type)
		{
			ClearKeys();
			foreach (AnimationKey key in keys)
			{
				object obj;
				switch (type)
				{
				case "Float":
				case "Single":
					obj = key.ValueFloat;
					break;
				case "Vector3":
					obj = key.ValueVector3;
					break;
				case "Vector4":
					obj = key.ValueVector4;
					break;
				default:
					obj = key.ValueInt;
					break;
				case "Bool":
					obj = key.ValueBool;
					break;
				case "MyTransparentMaterial":
					obj = MyTransparentMaterials.GetMaterial(MyStringId.GetOrCompute(key.ValueString));
					break;
				case "String":
					obj = key.ValueString ?? string.Empty;
					break;
				}
				AddKey(key.Time, (T)obj);
			}
		}

		private void RemoveRedundantKeys()
		{
			int num = 0;
			bool flag = true;
			while (num < m_keyCount - 1)
			{
				object value = m_keys[num].Value;
				object value2 = m_keys[num + 1].Value;
				bool flag2 = EqualsValues(value, value2);
				if (flag2 && !flag)
				{
					RemoveKey(num);
					continue;
				}
				flag = !flag2;
				num++;
			}
			if (m_keyCount == 2)
			{
				object value3 = m_keys[0].Value;
				object value4 = m_keys[1].Value;
				if (EqualsValues(value3, value4))
				{
					RemoveKey(num);
				}
			}
		}

		public virtual void SerializeValue(XmlWriter writer, object value)
		{
		}

		public virtual void DeserializeValue(XmlReader reader, out object value)
		{
			value = reader.get_Value();
			reader.Read();
		}

		protected virtual bool EqualsValues(object value1, object value2)
		{
			return false;
		}

		internal abstract void Interpolate(in T val1, in T val2, float time, out T value);
	}
}
