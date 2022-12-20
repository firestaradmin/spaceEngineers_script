using System.Collections.Generic;
using VRage;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal struct MyInstancingInfo
	{
		internal uint ID;

		internal uint ParentID;

		internal MyRenderInstanceBufferType Type;

		internal VertexLayoutId Layout;

		internal int VisibleCapacity;

		internal int TotalCapacity;

		internal int Stride;

		internal string DebugName;

		internal byte[] Data;

		internal List<uint> Refs;

		internal MyInstanceData[] InstanceData;

		internal Vector3[] Positions;

		internal bool[] VisibilityMask;

		internal int NonVisibleInstanceCount;

		internal bool EnabledSkinning;

		internal void SetVisibility(int index, bool value)
		{
			if (VisibilityMask != null && index < VisibilityMask.Length && value != VisibilityMask[index])
			{
				VisibilityMask[index] = value;
				NonVisibleInstanceCount += ((!value) ? 1 : (-1));
			}
		}

		internal void ResetVisibility()
		{
			for (int i = 0; i < TotalCapacity; i++)
			{
				VisibilityMask[i] = true;
			}
			NonVisibleInstanceCount = 0;
		}
	}
}
