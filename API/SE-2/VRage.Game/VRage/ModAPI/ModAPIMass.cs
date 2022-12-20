using Havok;
using VRageMath;

namespace VRage.ModAPI
{
	public struct ModAPIMass
	{
		public float Volume;

		public float Mass;

		public Vector3 CenterOfMass;

		public Matrix InertiaTensor;

		public static ModAPIMass FromHkMass(HkMassProperties mass)
		{
			ModAPIMass result = default(ModAPIMass);
			result.Volume = mass.Volume;
			result.Mass = mass.Mass;
			result.CenterOfMass = mass.CenterOfMass;
			result.InertiaTensor = mass.InertiaTensor;
			return result;
		}

		public ModAPIMass(float volume, float mass, Vector3 centerOfMass, Matrix inertiaTensor)
		{
			Volume = volume;
			Mass = mass;
			CenterOfMass = centerOfMass;
			InertiaTensor = inertiaTensor;
		}
	}
}
