using VRage.Utils;
using VRageRender.Import;

namespace VRageRender.Models
{
	public class MyMesh
	{
		public readonly string AssetName;

		public readonly MyMeshMaterial Material;

		public int IndexStart;

		public int TriStart;

		public int TriCount;

		/// <summary>
		/// c-tor - generic way for collecting resources
		/// </summary>
		/// <param name="meshInfo"></param>        
		/// <param name="assetName">just for debug output</param>
		public MyMesh(MyMeshPartInfo meshInfo, string assetName)
		{
			MyMaterialDescriptor materialDesc = meshInfo.m_MaterialDesc;
			if (materialDesc != null)
			{
				materialDesc.Textures.TryGetValue("DiffuseTexture", out var _);
<<<<<<< HEAD
				MyMeshMaterial myMeshMaterial = new MyMeshMaterial();
				myMeshMaterial.Name = meshInfo.m_MaterialDesc.MaterialName;
				myMeshMaterial.Name = MyStringId.GetOrCompute(myMeshMaterial.Name).String;
				myMeshMaterial.Textures = materialDesc.Textures;
				myMeshMaterial.DrawTechnique = meshInfo.Technique;
				myMeshMaterial.GlassCW = meshInfo.m_MaterialDesc.GlassCW;
				myMeshMaterial.GlassCCW = meshInfo.m_MaterialDesc.GlassCCW;
				myMeshMaterial.GlassSmooth = meshInfo.m_MaterialDesc.GlassSmoothNormals;
				Material = myMeshMaterial;
=======
				Material = new MyMeshMaterial
				{
					Name = meshInfo.m_MaterialDesc.MaterialName,
					Textures = materialDesc.Textures,
					DrawTechnique = meshInfo.Technique,
					GlassCW = meshInfo.m_MaterialDesc.GlassCW,
					GlassCCW = meshInfo.m_MaterialDesc.GlassCCW,
					GlassSmooth = meshInfo.m_MaterialDesc.GlassSmoothNormals
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				Material = new MyMeshMaterial();
			}
			AssetName = assetName;
		}

		public MyMesh(MyMeshMaterial material, string assetName)
		{
			Material = material;
			AssetName = assetName;
		}
	}
}
