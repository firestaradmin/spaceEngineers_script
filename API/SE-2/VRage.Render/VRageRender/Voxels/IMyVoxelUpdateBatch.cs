namespace VRageRender.Voxels
{
	public interface IMyVoxelUpdateBatch
	{
		void Commit(bool allowEmpty = false);
	}
}
