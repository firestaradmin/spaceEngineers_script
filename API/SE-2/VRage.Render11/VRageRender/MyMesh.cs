namespace VRageRender
{
	internal class MyMesh
	{
		protected string m_name;

		internal MyRenderLodInfo[] LODs;

		protected MyAssetLoadingEnum m_loadingStatus;

		internal string Name => m_name;

		internal MyAssetLoadingEnum LoadingStatus => m_loadingStatus;

		internal bool IsAnimated { get; set; }

		internal MyMesh()
		{
			LODs = null;
			m_loadingStatus = MyAssetLoadingEnum.Unassigned;
			IsAnimated = false;
		}

		internal virtual void Release()
		{
			MyRenderLodInfo[] lODs = LODs;
			for (int i = 0; i < lODs.Length; i++)
			{
				lODs[i].m_meshInfo.ReleaseBuffers();
			}
			LODs = null;
			m_loadingStatus = MyAssetLoadingEnum.Unassigned;
		}

		internal virtual int GetSortingID(int lodNum)
		{
			return 0;
		}

		internal void DebugWriteInfo()
		{
			for (int i = 0; i < LODs.Length; i++)
			{
			}
		}
	}
}
