using System;
using VRageMath;

namespace VRageRender
{
	internal struct MyHighlightDesc : IEquatable<MyHighlightDesc>
	{
		internal string SectionName;

		internal Color Color;

		internal float Thickness;

		internal float PulseTimeInSeconds;

		internal int InstanceId;

		public bool Equals(MyHighlightDesc other)
		{
			if (SectionName == other.SectionName && Color.Equals(other.Color) && Thickness == other.Thickness && PulseTimeInSeconds == other.PulseTimeInSeconds)
			{
				return InstanceId == other.InstanceId;
			}
			return false;
		}
	}
}
