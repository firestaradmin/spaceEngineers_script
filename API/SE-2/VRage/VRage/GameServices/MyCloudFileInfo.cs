namespace VRage.GameServices
{
	public struct MyCloudFileInfo
	{
		public readonly string Name;

		public readonly string ContainerName;

		public readonly int Size;

		public readonly long Timestamp;

		public MyCloudFileInfo(string name, string containerName, int size, long timestamp)
		{
			Name = name;
			ContainerName = containerName;
			Size = size;
			Timestamp = timestamp;
		}
	}
}
