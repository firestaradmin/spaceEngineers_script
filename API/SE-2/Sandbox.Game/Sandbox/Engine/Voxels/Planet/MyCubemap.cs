namespace Sandbox.Engine.Voxels.Planet
{
	public class MyCubemap : MyWrappedCubemap<MyCubemapData<byte>>
	{
		public MyCubemap(MyCubemapData<byte>[] faces)
			: base((string)null, faces[0].Resolution, faces)
		{
		}
	}
}
