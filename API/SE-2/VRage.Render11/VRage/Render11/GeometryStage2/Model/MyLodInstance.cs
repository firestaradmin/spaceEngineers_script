using System.Collections.Generic;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.GeometryStage2.Model.Preprocess;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene.Resources;
using VRageRender.Messages;

namespace VRage.Render11.GeometryStage2.Model
{
	internal class MyLodInstance
	{
		public MyRenderMaterialBindings[] GBufferParts;

		public MyRenderMaterialBindings[] ForwardParts;

		public MyRenderMaterialBindings[] DepthParts;

		public MyRenderMaterialBindings[] TransparentParts;

		public MyRenderMaterialBindings[] HighlightParts;

		public int UniqueId { get; }

		public MyLodInstance(MyLod lod, Dictionary<string, MyTextureChange> changes, HashSet<IMySceneResource> usedTextures)
		{
			UniqueId = MyManagers.IDGenerator.Lods.Generate();
			InitParts(lod.PreprocessedParts.GBufferParts, changes, out GBufferParts, usedTextures);
			InitParts(lod.PreprocessedParts.ForwardParts, changes, out ForwardParts, usedTextures);
			InitParts(lod.PreprocessedParts.DepthParts, changes, out DepthParts, usedTextures);
			InitParts(lod.PreprocessedParts.TransparentParts, changes, out TransparentParts, usedTextures);
			InitParts(lod.PreprocessedParts.HighlightParts, changes, out HighlightParts, usedTextures);
		}

		private void InitParts(List<MyPreprocessedPart> preprocessedParts, Dictionary<string, MyTextureChange> changes, out MyRenderMaterialBindings[] instanceParts, HashSet<IMySceneResource> usedTextures)
		{
			if (preprocessedParts == null)
			{
				instanceParts = null;
				return;
			}
			instanceParts = new MyRenderMaterialBindings[preprocessedParts.Count];
			for (int i = 0; i < instanceParts.Length; i++)
			{
				ref MyRenderMaterialBindings reference = ref instanceParts[i];
				MyRenderMaterialBindings binding = preprocessedParts[i].Material.Binding;
				if (changes != null && changes.TryGetValue(preprocessedParts[i].Name, out var value) && !value.IsDefault())
				{
					reference = MyModelMaterials.GetBinding(MyResourceUtils.GetTextureFullPath(value.ColorMetalFileName), MyResourceUtils.GetTextureFullPath(value.NormalGlossFileName), MyResourceUtils.GetTextureFullPath(value.ExtensionsFileName), MyResourceUtils.GetTextureFullPath(value.AlphamaskFileName));
					if (value.ColorMetalFileName == null && binding.Srvs.Length != 0)
					{
						reference.Srvs[0] = binding.Srvs[0];
						reference.TextureHandles[0] = binding.TextureHandles[0];
					}
					if (value.NormalGlossFileName == null && binding.Srvs.Length > 1)
					{
						reference.Srvs[1] = binding.Srvs[1];
						reference.TextureHandles[1] = binding.TextureHandles[1];
					}
					if (value.ExtensionsFileName == null && binding.Srvs.Length > 2)
					{
						reference.Srvs[2] = binding.Srvs[2];
						reference.TextureHandles[2] = binding.TextureHandles[2];
					}
					if (value.AlphamaskFileName == null && binding.Srvs.Length > 3)
					{
						reference.SrvAlphamask = (reference.Srvs[3] = binding.Srvs[3]);
						reference.TextureHandles[3] = binding.TextureHandles[3];
					}
				}
				else
				{
					reference = binding;
				}
				if (reference.TextureHandles != null)
				{
					IMyStreamedTexture[] textureHandles = reference.TextureHandles;
<<<<<<< HEAD
					foreach (IMyStreamedTexture item in textureHandles)
					{
						usedTextures.Add(item);
=======
					foreach (IMyStreamedTexture myStreamedTexture in textureHandles)
					{
						usedTextures.Add((IMySceneResource)myStreamedTexture);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		internal void OnTextureHandleChanged(IMyStreamedTexture texture)
		{
			Update(GBufferParts);
			Update(ForwardParts);
			Update(DepthParts);
			Update(TransparentParts);
			Update(HighlightParts);
			void Update(MyRenderMaterialBindings[] parts)
			{
<<<<<<< HEAD
				if (parts != null)
				{
					for (int i = 0; i < parts.Length; i++)
					{
						ref MyRenderMaterialBindings reference = ref parts[i];
						IMyStreamedTexture[] textureHandles = reference.TextureHandles;
						if (textureHandles != null)
						{
							for (int j = 0; j < textureHandles.Length; j++)
							{
								if (texture == textureHandles[j])
								{
									ITexture texture2 = texture.Texture;
									if (j != 3)
									{
										reference.Srvs[j] = texture2;
									}
									else
									{
										reference.SrvAlphamask = texture2;
									}
=======
				for (int i = 0; i < parts.Length; i++)
				{
					ref MyRenderMaterialBindings reference = ref parts[i];
					IMyStreamedTexture[] textureHandles = reference.TextureHandles;
					if (textureHandles != null)
					{
						for (int j = 0; j < textureHandles.Length; j++)
						{
							if (texture == textureHandles[j])
							{
								ITexture texture2 = texture.Texture;
								if (j != 3)
								{
									reference.Srvs[j] = texture2;
								}
								else
								{
									reference.SrvAlphamask = texture2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								}
							}
						}
					}
				}
			}
		}
	}
}
