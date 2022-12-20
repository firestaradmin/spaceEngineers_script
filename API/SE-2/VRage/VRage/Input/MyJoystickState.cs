using System.Runtime.InteropServices;

namespace VRage.Input
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MyJoystickState
	{
		public int X;

		public int Y;

		public int Z;

		public int RotationX;

		public int RotationY;

		public int RotationZ;

		public unsafe fixed int Sliders[2];

		public unsafe fixed int PointOfViewControllers[4];

		public unsafe fixed byte Buttons[128];

		public int VelocityX;

		public int VelocityY;

		public int VelocityZ;

		public int AngularVelocityX;

		public int AngularVelocityY;

		public int AngularVelocityZ;

		public unsafe fixed int VelocitySliders[2];

		public int AccelerationX;

		public int AccelerationY;

		public int AccelerationZ;

		public int AngularAccelerationX;

		public int AngularAccelerationY;

		public int AngularAccelerationZ;

		public unsafe fixed int AccelerationSliders[2];

		public int ForceX;

		public int ForceY;

		public int ForceZ;

		public int TorqueX;

		public int TorqueY;

		public int TorqueZ;

		public unsafe fixed int ForceSliders[2];

		public int Z_Left;

		public int Z_Right;

		public unsafe void CopyFromJoystickBasicState(MyJoystickState_Basic originalState)
		{
			X = originalState.X;
			Y = originalState.Y;
			Z = originalState.Z;
			RotationX = originalState.RotationX;
			RotationY = originalState.RotationY;
			RotationZ = originalState.RotationZ;
			VelocityX = originalState.VelocityX;
			VelocityY = originalState.VelocityY;
			VelocityZ = originalState.VelocityZ;
			AngularVelocityX = originalState.AngularVelocityX;
			AngularVelocityY = originalState.AngularVelocityY;
			AngularVelocityZ = originalState.AngularVelocityZ;
			AccelerationX = originalState.AccelerationX;
			AccelerationY = originalState.AccelerationY;
			AccelerationZ = originalState.AccelerationZ;
			AngularAccelerationX = originalState.AngularAccelerationX;
			AngularAccelerationY = originalState.AngularAccelerationY;
			AngularAccelerationZ = originalState.AngularAccelerationZ;
			ForceX = originalState.ForceX;
			ForceY = originalState.ForceY;
			ForceZ = originalState.ForceZ;
			TorqueX = originalState.TorqueX;
			TorqueY = originalState.TorqueY;
			TorqueZ = originalState.TorqueZ;
			for (int i = 0; i < 128; i++)
			{
				if (i < 2)
				{
					Sliders[i] = originalState.Sliders[i];
				}
				if (i < 4)
				{
					PointOfViewControllers[i] = originalState.PointOfViewControllers[i];
				}
				if (i < 128)
				{
					Buttons[i] = originalState.Buttons[i];
				}
				if (i < 2)
				{
					VelocitySliders[i] = originalState.VelocitySliders[i];
				}
				if (i < 2)
				{
					AccelerationSliders[i] = originalState.AccelerationSliders[i];
				}
			}
		}
	}
}
