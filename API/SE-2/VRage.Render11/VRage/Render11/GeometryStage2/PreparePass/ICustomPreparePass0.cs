using System.Collections.Generic;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Model;
using VRageRender;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	internal interface ICustomPreparePass0
	{
		int ElementSize { get; }

		void InitInstanceElements(int elementsCount);

		void AddInstanceIntoInstanceElements(int bufferOffset, MyInstance instance, int instanceMaterialOffsetInData, float stateData);

		List<int> GetInstanceMaterialOffsetsForThePass(MyLod lod);

		int GetInstanceMaterialsCount(MyLod lod);

		void WriteData(ref MyMapping vb);
	}
}
