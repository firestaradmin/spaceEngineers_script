using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Model
{
	internal class MyMwmDataPart
	{
		public MyMeshDrawTechnique Technique;

		public string MaterialName;

		public int IndexOffset;

		public int IndicesCount;

		public MyFacingEnum Facing;

		public string ColorMetalFilepath;

		public string NormalGlossFilepath;

		public string ExtensionFilepath;

		public string AlphamaskFilepath;

		public MyMwmDataPart(MyMeshPartInfo partInfo, int indexOffset, int indicesCount, string contentPath)
		{
			Technique = partInfo.Technique;
			MaterialName = partInfo.GetMaterialName();
			IndexOffset = indexOffset;
			IndicesCount = indicesCount;
			if (partInfo.m_MaterialDesc != null)
			{
				Facing = partInfo.m_MaterialDesc.Facing;
				ColorMetalFilepath = MyMwmUtils.GetColorMetalTexture(partInfo, contentPath);
				NormalGlossFilepath = MyMwmUtils.GetNormalGlossTexture(partInfo, contentPath);
				ExtensionFilepath = MyMwmUtils.GetExtensionTexture(partInfo, contentPath);
				AlphamaskFilepath = MyMwmUtils.GetAlphamaskTexture(partInfo, contentPath);
			}
			else
			{
				Facing = MyFacingEnum.None;
				ColorMetalFilepath = null;
				NormalGlossFilepath = null;
				ExtensionFilepath = null;
				AlphamaskFilepath = null;
			}
		}
	}
}
