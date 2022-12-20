using System;

namespace VRage.Render.Particles
{
	[Flags]
	public enum GPUEmitterFlags : uint
	{
		Streaks = 0x1u,
		Collide = 0x2u,
		SleepState = 0x4u,
		Dead = 0x8u,
		Light = 0x10u,
		VolumetricLight = 0x20u,
		FreezeSimulate = 0x80u,
		FreezeEmit = 0x100u,
		RandomRotationEnabled = 0x200u,
		LocalRotation = 0x400u,
		LocalAndCameraRotation = 0x800u,
		UseAlphaAnisotropy = 0x2000u
	}
}
