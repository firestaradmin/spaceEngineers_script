using System.Collections.Generic;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal class MyRenderData
	{
		public readonly List<MyInstanceLodGroup> InstanceLodGroups;

		public IVertexBuffer VBInstanceBuffer;

		public Matrix ViewProjMatrix;

		public Matrix ProjectionMatrix;

		public MyRenderData()
		{
			InstanceLodGroups = new List<MyInstanceLodGroup>();
		}

		public MyRenderData(List<MyInstanceLodGroup> list)
		{
			InstanceLodGroups = list;
		}

		public void Init(Matrix viewProj, Matrix proj)
		{
			ViewProjMatrix = viewProj;
			ProjectionMatrix = proj;
		}

		public MyRenderData ShallowClone()
		{
			return new MyRenderData(InstanceLodGroups)
			{
				VBInstanceBuffer = VBInstanceBuffer,
				ViewProjMatrix = ViewProjMatrix,
				ProjectionMatrix = ProjectionMatrix
			};
		}

		public void Dispose()
		{
			InstanceLodGroups.Clear();
			if (VBInstanceBuffer != null)
			{
				MyManagers.Buffers.Dispose(VBInstanceBuffer);
			}
			VBInstanceBuffer = null;
		}
	}
}
