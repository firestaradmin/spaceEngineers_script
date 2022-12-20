using VRageMath;

namespace VRage.ModAPI
{
	public struct ModAPIMassElement
	{
		public ModAPIMass Properties;

		public Matrix Tranform;

		public ModAPIMassElement(ref ModAPIMass properties, ref Matrix tranform)
		{
			Properties = properties;
			Tranform = tranform;
		}
	}
}
