using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty2DVector4 : MyAnimatedProperty2D<MyAnimatedPropertyVector4, Vector4, float>
	{
		public override string ValueType => "Vector4";

		public MyAnimatedProperty2DVector4(string name, string description)
			: base(name, description)
		{
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyAnimatedPropertyVector4 myAnimatedPropertyVector = new MyAnimatedPropertyVector4(base.Name, base.Description);
			myAnimatedPropertyVector.Deserialize(reader);
			value = myAnimatedPropertyVector;
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedProperty2DVector4 myAnimatedProperty2DVector = new MyAnimatedProperty2DVector4(base.Name, base.Description);
			Duplicate(myAnimatedProperty2DVector);
			return myAnimatedProperty2DVector;
		}

		public override void ApplyVariance(ref Vector4 interpolatedValue, ref float variance, float multiplier, out Vector4 value)
		{
			float randomFloat = MyUtils.GetRandomFloat(1f - variance, 1f + variance);
			value.X = interpolatedValue.X * randomFloat;
			value.Y = interpolatedValue.Y * randomFloat;
			value.Z = interpolatedValue.Z * randomFloat;
			value.W = interpolatedValue.W;
			value.X = MathHelper.Clamp(value.X, 0f, 1f);
			value.Y = MathHelper.Clamp(value.Y, 0f, 1f);
			value.Z = MathHelper.Clamp(value.Z, 0f, 1f);
		}
	}
}
