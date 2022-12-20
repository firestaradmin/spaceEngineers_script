using System;
using System.Collections.Generic;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty2D<T, V, W> : MyAnimatedProperty<T>, IMyAnimatedProperty2D<T, V, W>, IMyAnimatedProperty2D, IMyAnimatedProperty, IMyConstProperty where T : MyAnimatedProperty<V>, new()
	{
		public override bool Is2D => true;

		bool IMyConstProperty.IsDefault
		{
			get
			{
				if (GetKeysCount() != m_defaultKeys.Length)
				{
					return false;
				}
				for (int i = 0; i < GetKeysCount(); i++)
				{
					GetKey(i, out var _, out var time, out var value);
					if (time != m_defaultKeys[i].Time)
					{
						return false;
					}
					if (value.GetKeysCount() != m_defaultKeys[i].Value.GetKeysCount())
					{
						return false;
					}
					for (int j = 0; j < value.GetKeysCount(); j++)
					{
						value.GetKey(j, out var _, out var time2, out var value2);
						m_defaultKeys[i].Value.GetKey(j, out var _, out var time3, out var value3);
						if (time2 != time3)
						{
							return false;
						}
						if (!EqualityComparer<V>.Default.Equals(value2, value3))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public MyAnimatedProperty2D()
		{
		}

		public MyAnimatedProperty2D(string name, string description)
			: base(name, description, interpolateAfterEnd: false)
		{
		}

		public X GetInterpolatedValue<X>(float overallTime, float time) where X : V
		{
			GetPreviousValue(overallTime, out var previousValue, out var previousTime);
			GetNextValue(overallTime, out var nextValue, out var _, out var difference);
			previousValue.GetInterpolatedValue(time, out var value);
			nextValue.GetInterpolatedValue(time, out var value2);
			previousValue.Interpolate(in value, in value2, (overallTime - previousTime) * difference, out var value3);
			return (X)(object)value3;
		}

		void IMyAnimatedProperty2D.GetInterpolatedKeys(float overallTime, float multiplier, IMyAnimatedProperty interpolatedKeys)
		{
			GetInterpolatedKeys(overallTime, default(W), multiplier, (T)interpolatedKeys);
		}

		public void GetInterpolatedKeys(float overallTime, float multiplier, T interpolatedKeys)
		{
			GetInterpolatedKeys(overallTime, default(W), multiplier, interpolatedKeys);
		}

		void IMyAnimatedProperty2D<T, V, W>.GetInterpolatedKeys(float overallTime, W variance, float multiplier, IMyAnimatedProperty interpolatedKeysOb)
		{
			GetInterpolatedKeys(overallTime, variance, multiplier, (T)interpolatedKeysOb);
		}

		public void GetInterpolatedKeys(float overallTime, W variance, float multiplier, T interpolatedKeys)
		{
			GetInterval(overallTime, out var values, out var times, out var difference);
			interpolatedKeys.ClearKeys();
			if (values.Item1 == null)
			{
				return;
			}
			for (int i = 0; i < values.Item1.GetKeysCount(); i++)
			{
				values.Item1.GetKey(i, out var time, out var value);
				V value2 = value;
				if (times.Item1 != times.Item2)
				{
					values.Item2.GetInterpolatedValue(time, out var value3);
					interpolatedKeys.Interpolate(in value, in value3, (overallTime - times.Item1) * difference, out value2);
				}
				ApplyVariance(ref value2, ref variance, multiplier, out value2);
				interpolatedKeys.AddKey(time, value2);
			}
		}

		public virtual void ApplyVariance(ref V interpolatedValue, ref W variance, float multiplier, out V value)
		{
			value = default(V);
		}

		public IMyAnimatedProperty CreateEmptyKeys()
		{
			return new T();
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			(value as IMyAnimatedProperty).Serialize(writer);
		}

		internal override void Interpolate(in T val1, in T val2, float time, out T value)
		{
			throw new InvalidOperationException("This is not implemented.");
		}

		public override IMyConstProperty Duplicate()
		{
			return null;
		}

		void IMyConstProperty.SetDefaultValue(object val)
		{
		}

		object IMyConstProperty.GetDefaultValue()
		{
			return null;
		}

		public override GenerationProperty SerializeToObjectBuilder()
		{
			GenerationProperty generationProperty = new GenerationProperty();
			generationProperty.Keys = new List<AnimationKey>();
			generationProperty.Name = base.Name;
			generationProperty.Type = ValueType;
			_ = base.Name == "Emitter size";
			generationProperty.AnimationType = PropertyAnimationType.Animated2D;
			for (int i = 0; i < GetKeysCount(); i++)
			{
				GetKey(i, out var _, out var time, out var value);
				AnimationKey item = default(AnimationKey);
				item.Time = time;
				item.Value2D = default(Generation2DProperty);
				item.Value2D.Keys = value.SerializeKeys(ValueType);
				generationProperty.Keys.Add(item);
			}
			return generationProperty;
		}

		public override void DeserializeFromObjectBuilder(GenerationProperty property)
		{
			base.Name = property.Name;
			MyAnimatedProperty<T>.KeyTypeName = property.Type;
			foreach (AnimationKey key in property.Keys)
			{
				T val = new T();
				val.DeserializeFromObjectBuilder_Animation(key.Value2D, property.Type);
				AddKey(key.Time, val);
			}
		}
	}
}
