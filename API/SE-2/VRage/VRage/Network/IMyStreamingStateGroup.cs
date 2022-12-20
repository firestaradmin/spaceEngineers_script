namespace VRage.Network
{
	public interface IMyStreamingStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		bool HasStreamed(Endpoint endpoint);
	}
}
