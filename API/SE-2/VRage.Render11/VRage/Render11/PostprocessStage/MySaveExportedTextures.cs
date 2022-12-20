using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Render.Image;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.PostprocessStage
{
	internal class MySaveExportedTextures : MyImmediateRC
	{
		private static MyPixelShaders.Id m_ps = MyPixelShaders.Id.NULL;

		private static void EnsureInitialized()
		{
			if (m_ps == MyPixelShaders.Id.NULL)
			{
				m_ps = MyPixelShaders.Create("Postprocess\\PostprocessColorizeExportedTexture.hlsl");
			}
		}

		internal unsafe static void RenderColoredTextures(List<renderColoredTextureProperties> texturesToRender)
		{
			if (texturesToRender.Count == 0)
			{
				return;
			}
			EnsureInitialized();
			MyImmediateRC.RC.SetBlendState(null);
			MyImmediateRC.RC.SetInputLayout(null);
			MyImmediateRC.RC.PixelShader.Set(m_ps);
			MyImmediateRC.RC.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			IConstantBuffer constantBuffer = MyManagers.Buffers.CreateConstantBuffer("ExportedTexturesColor", sizeof(Vector4), null, ResourceUsage.Dynamic);
			IBorrowedRtvTexture renderTarget = null;
			Dictionary<string, MyStreamedTexturePin> dictionary = new Dictionary<string, MyStreamedTexturePin>();
			try
			{
				MyImmediateRC.RC.AllShaderStages.SetConstantBuffer(1, constantBuffer);
				foreach (renderColoredTextureProperties item in texturesToRender)
				{
					ISrvBindable orAllocateTexture = GetOrAllocateTexture(item.TextureAddMaps, MyFileTextureEnum.EXTENSIONS, dictionary);
					ISrvBindable orAllocateTexture2 = GetOrAllocateTexture(item.TextureAplhaMask, MyFileTextureEnum.ALPHAMASK, dictionary);
					ISrvBindable orAllocateTexture3 = GetOrAllocateTexture(item.TextureColorMetal, MyFileTextureEnum.COLOR_METAL, dictionary);
					Vector2I acceptableViewport = GetAcceptableViewport(orAllocateTexture3);
					PrepareRenderTarget(ref renderTarget, acceptableViewport);
					MyImmediateRC.RC.SetRtv(renderTarget);
					MyMapping myMapping = MyMapping.MapDiscard(constantBuffer);
					Vector4 data = new Vector4(item.ColorMaskHSV, 1f);
					myMapping.WriteAndPosition(ref data);
					myMapping.Unmap();
					MyImmediateRC.RC.PixelShader.SetSrv(0, orAllocateTexture3);
					MyImmediateRC.RC.PixelShader.SetSrv(1, orAllocateTexture);
					MyImmediateRC.RC.PixelShader.SetSrv(2, orAllocateTexture2);
					MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC, new MyViewport(acceptableViewport));
					MyTextureData.ToFile(renderTarget, item.PathToSave_ColorAlpha, MyImage.FileFormat.Png);
				}
				foreach (renderColoredTextureProperties item2 in texturesToRender)
				{
					if (item2.TextureNormalGloss != null)
					{
						ISrvBindable orAllocateTexture4 = GetOrAllocateTexture(item2.TextureNormalGloss, MyFileTextureEnum.NORMALMAP_GLOSS, dictionary);
						PrepareRenderTarget(ref renderTarget, orAllocateTexture4.Size);
						MyCopyToRT.Run(renderTarget, orAllocateTexture4);
						MyTextureData.ToFile(renderTarget, item2.PathToSave_NormalGloss, MyImage.FileFormat.Png);
					}
				}
				texturesToRender.Clear();
			}
			finally
			{
				MyImmediateRC.RC.SetRtvNull();
				MyImmediateRC.RC.PixelShader.SetSrvs(0, MyGBuffer.Main);
				renderTarget?.Release();
				foreach (var (_, myStreamedTexturePin2) in dictionary)
				{
					myStreamedTexturePin2.Dispose();
				}
				MyImmediateRC.RC.AllShaderStages.SetConstantBuffer(1, null);
				MyManagers.Buffers.Dispose(constantBuffer);
			}
		}

		/// <summary>
		/// Exporting game's native 4K x 4K (or even larger in some cases) textures may come really expensive for large modes.
		/// This method scales excessivelly large textures under acceptable dimensions.
		/// </summary>
		private static Vector2I GetAcceptableViewport(ISrvBindable texture)
		{
			Vector2I result = texture.Size;
			int num = Math.Max(result.X, result.Y);
			if (num > 1024)
			{
				float num2 = 1024f / (float)num;
				result = new Vector2I((int)((float)result.X * num2), (int)((float)result.Y * num2));
			}
			return result;
		}

		private static ISrvBindable GetOrAllocateTexture(string name, MyFileTextureEnum type, Dictionary<string, MyStreamedTexturePin> usedTextures)
		{
			MyTextureStreamingManager textures = MyManagers.Textures;
			MyTextureStreamingManager.QueryArgs queryArgs = default(MyTextureStreamingManager.QueryArgs);
			queryArgs.TextureType = type;
			queryArgs.WaitUntilLoaded = true;
			MyTextureStreamingManager.QueryArgs args = queryArgs;
			if (string.IsNullOrEmpty(name))
			{
				return textures.GetTexture(name, args).Texture;
			}
			if (!usedTextures.TryGetValue(name, out var value))
			{
				value = textures.GetPermanentTexture(name, args);
				usedTextures.Add(name, value);
			}
			return value.Texture;
		}

		private static void PrepareRenderTarget(ref IBorrowedRtvTexture renderTarget, Vector2I viewport)
		{
			if (renderTarget == null || !(renderTarget.Size == viewport))
			{
				if (renderTarget != null)
				{
					renderTarget.Release();
				}
				renderTarget = MyManagers.RwTexturesPool.BorrowRtv("MySaveExportedTextures.RenderColoredTextures", viewport.X, viewport.Y, Format.R8G8B8A8_UNorm_SRgb);
			}
		}
	}
}
