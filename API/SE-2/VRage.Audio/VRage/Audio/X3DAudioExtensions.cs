using SharpDX.Mathematics.Interop;
using SharpDX.X3DAudio;
using VRage.Data.Audio;
using VRageMath;

namespace VRage.Audio
{
	public static class X3DAudioExtensions
	{
		internal static void SetDefaultValues(this Emitter emitter)
		{
			emitter.Position = default(RawVector3);
			emitter.Velocity = default(RawVector3);
			emitter.OrientFront = new RawVector3(0f, 0f, 1f);
			emitter.OrientTop = new RawVector3(0f, 1f, 0f);
			emitter.ChannelCount = 1;
			emitter.CurveDistanceScaler = float.MinValue;
			emitter.Cone = null;
		}

		internal static void SetDefaultValues(this Listener listener)
		{
			listener.Position = default(RawVector3);
			listener.Velocity = default(RawVector3);
			listener.OrientFront = new RawVector3(0f, 0f, 1f);
			listener.OrientTop = new RawVector3(0f, 1f, 0f);
		}

		internal static void UpdateValuesOmni(this Emitter emitter, Vector3 position, Vector3 velocity, MySoundData cue, int channelsCount, float? customMaxDistance, float dopplerScaler)
		{
			emitter.Position = new RawVector3(position.X, position.Y, position.Z);
			emitter.Velocity = new RawVector3(velocity.X, velocity.Y, velocity.Z);
			float num = (customMaxDistance.HasValue ? customMaxDistance.Value : cue.MaxDistance);
			emitter.DopplerScaler = dopplerScaler;
			emitter.CurveDistanceScaler = num;
			emitter.VolumeCurve = MyDistanceCurves.Curves[(int)cue.VolumeCurve].Points;
			emitter.InnerRadius = ((channelsCount > 2) ? num : 0f);
			emitter.InnerRadiusAngle = ((channelsCount > 2) ? 0.785398f : 0f);
		}

		internal static void UpdateValuesOmni(this Emitter emitter, Vector3 position, Vector3 velocity, float maxDistance, int channelsCount, MyCurveType volumeCurve, float dopplerScaler)
		{
			emitter.Position = new RawVector3(position.X, position.Y, position.Z);
			emitter.Velocity = new RawVector3(velocity.X, velocity.Y, velocity.Z);
			emitter.DopplerScaler = dopplerScaler;
			emitter.CurveDistanceScaler = maxDistance;
			emitter.VolumeCurve = MyDistanceCurves.Curves[(int)volumeCurve].Points;
			emitter.InnerRadius = ((channelsCount > 2) ? maxDistance : 0f);
			emitter.InnerRadiusAngle = ((channelsCount > 2) ? 0.785398f : 0f);
		}
	}
}
