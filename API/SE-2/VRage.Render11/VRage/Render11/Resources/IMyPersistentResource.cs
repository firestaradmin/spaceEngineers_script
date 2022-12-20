namespace VRage.Render11.Resources
{
	internal interface IMyPersistentResource<TDescription>
	{
		string Name { get; }

		TDescription Description { get; }

		void ChangeDescription(ref TDescription desc);
	}
}
