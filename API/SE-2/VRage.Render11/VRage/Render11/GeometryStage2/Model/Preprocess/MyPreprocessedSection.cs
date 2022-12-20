using System.Collections.Generic;
using VRage.Render11.GeometryStage2.Common;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Model.Preprocess
{
	internal class MyPreprocessedSection
	{
		public string DebugSectionName { get; private set; }

		public List<MyPreprocessedPart> Parts { get; private set; }

		private static MyMwmDataPart FindPartInfo(string partName, List<MyMwmDataPart> partInfos)
		{
			foreach (MyMwmDataPart partInfo in partInfos)
			{
				if (partInfo.MaterialName == partName)
				{
					return partInfo;
				}
			}
			return null;
		}

		private static bool FindPart(List<MyPreprocessedPart> parts, string name, out MyPreprocessedPart result)
		{
			foreach (MyPreprocessedPart part in parts)
			{
				if (part.Name == name)
				{
					result = part;
					return true;
				}
			}
			result = default(MyPreprocessedPart);
			return false;
		}

		public void Init(MyRenderPassType passType, MyLod lod, string sectionName, MyMeshSectionInfo sectionInfo, List<MyMwmDataPart> partInfos, List<MyPreprocessedPart> parts)
		{
			Parts = new List<MyPreprocessedPart>();
			DebugSectionName = sectionName;
			foreach (MyMeshSectionMeshInfo mesh in sectionInfo.Meshes)
			{
				string materialName = mesh.MaterialName;
				MyMwmDataPart myMwmDataPart = FindPartInfo(materialName, partInfos);
				if (FindPart(parts, materialName, out var result) && myMwmDataPart != null)
				{
					int indexStart = result.IndexStart + mesh.StartIndex;
					int indexCount = mesh.IndexCount;
					MyPreprocessedPart item = default(MyPreprocessedPart);
					item.Init(materialName, lod, indexStart, indexCount, result.Material, passType);
					Parts.Add(item);
				}
			}
		}
	}
}
