using VRageMath;

namespace VRageRender
{
	public static class MyRenderProxyUtils
	{
		public static Vector2I GetFixedWindowResolution(Vector2I inResolution, MyAdapterInfo adapterInfo)
		{
			Vector2I vector2I = new Vector2I(150);
			Vector2I v = new Vector2I(adapterInfo.DesktopBounds.Width, adapterInfo.DesktopBounds.Height) - vector2I;
			v = Vector2I.Min(inResolution, v);
			if (inResolution == v)
			{
				return v;
			}
			Vector2I vector2I2 = Vector2I.Zero;
			MyDisplayMode[] supportedDisplayModes = adapterInfo.SupportedDisplayModes;
			for (int i = 0; i < supportedDisplayModes.Length; i++)
			{
				MyDisplayMode myDisplayMode = supportedDisplayModes[i];
				Vector2I vector2I3 = new Vector2I(myDisplayMode.Width, myDisplayMode.Height);
				if (vector2I3.X <= v.X && vector2I3.Y <= v.Y)
				{
					float num = vector2I3.X * vector2I3.Y;
					float num2 = vector2I2.X * vector2I2.Y;
					if (num > num2)
					{
						vector2I2 = vector2I3;
					}
				}
			}
			if (vector2I2 != Vector2I.Zero)
			{
				v = vector2I2;
			}
			return v;
		}
	}
}
