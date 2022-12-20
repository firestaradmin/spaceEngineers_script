namespace VRageRender
{
	internal class MyRenderLodInfo
	{
		internal int LodNum;

		internal float Distance;

		internal MyRenderMeshInfo m_meshInfo;

		internal MyRenderLodInfo()
		{
			LodNum = 1;
			Distance = 0f;
		}
	}
}
