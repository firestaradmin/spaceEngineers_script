namespace VRage.GameServices
{
	public struct MyCloudFile
	{
		public readonly string FileName;

		public readonly bool Compress;

		public MyCloudFile(string fileName, bool compress = false)
		{
			FileName = fileName;
			Compress = compress;
		}
	}
}
