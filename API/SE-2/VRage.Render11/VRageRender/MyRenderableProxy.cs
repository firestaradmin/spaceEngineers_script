using System;
using System.Collections.Generic;
using VRage.Render.Scene.Components;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender.Import;

namespace VRageRender
{
	/// <summary>
	/// Contains data needed to render an actor or part of it.
	/// Does not own any data
	/// </summary>
	[PooledObject(2)]
	internal class MyRenderableProxy : IPooledObject
	{
		internal const int IS_DECAL_SORTKEY_BIT = 50;

		internal MyObjectDataCommon CommonObjectData;

		internal MyObjectDataNonVoxel NonVoxelObjectData;

		internal MyObjectDataVoxelCommon VoxelCommonObjectData;

		internal Matrix[] SkinningMatrices;

		internal LodMeshId Mesh;

		internal InstancingId Instancing;

		internal MyMaterialShadersBundleId DepthShaders;

		internal MyMaterialShadersBundleId Shaders;

		internal MyMaterialShadersBundleId HighlightShaders;

		internal MyMaterialShadersBundleId ForwardShaders;

		internal MyMaterialShadersBundleId TransparentDepthShaders;

		internal MyDrawSubmesh DrawSubmesh;

		internal int PerMaterialIndex;

		internal MyDrawSubmesh[] SectionSubmeshes;

		internal int InstanceCount;

		internal int StartInstance;

		internal MyMaterialType Type;

		internal MyRenderableProxyFlags Flags;

		internal int ObjectBufferSize;

		internal int ObjectBufferSizeAligned;

		internal MyActorComponent Parent;

		internal MyMeshMaterialId Material;

		internal HashSet<string> UnusedMaterials;

		public IDepthStencilState GbufferDepthState;

		public IBlendState GbufferBlendState;

		public IRasterizerState GbufferRasterizerState;

		public MyMeshDrawTechnique Technique;

		public int PartIndex;

		public bool TransparentTechnique;

		internal bool InstancingEnabled => Instancing != InstancingId.NULL;

		internal MatrixD WorldMatrix => Parent.Owner.WorldMatrix;

		/// <inheritdoc />
		void IPooledObject.Cleanup()
		{
			Clear();
		}

		private void Clear()
		{
			ObjectBufferSize = 0;
			CommonObjectData = default(MyObjectDataCommon);
			NonVoxelObjectData = MyObjectDataNonVoxel.Invalid;
			VoxelCommonObjectData = MyObjectDataVoxelCommon.Invalid;
			Mesh = LodMeshId.NULL;
			Instancing = InstancingId.NULL;
			DepthShaders = MyMaterialShadersBundleId.NULL;
			Shaders = MyMaterialShadersBundleId.NULL;
			TransparentDepthShaders = MyMaterialShadersBundleId.NULL;
			ForwardShaders = MyMaterialShadersBundleId.NULL;
			DrawSubmesh = default(MyDrawSubmesh);
			PerMaterialIndex = 0;
			SectionSubmeshes = null;
			InstanceCount = 0;
			StartInstance = 0;
			SkinningMatrices = null;
			Type = MyMaterialType.OPAQUE;
			Flags = MyRenderableProxyFlags.None;
			Parent = null;
			TransparentTechnique = false;
			Material = MyMeshMaterialId.NULL;
			UnusedMaterials = UnusedMaterials ?? new HashSet<string>();
			UnusedMaterials.Clear();
			UpdateTechniqueGbuffer();
		}

		private IDepthStencilState GetDepthStencilViewGbuffer(bool readOnly)
		{
			if (readOnly)
			{
				if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
				{
					return MyDepthStencilStateManager.StereoDepthTestReadOnly;
				}
				return MyDepthStencilStateManager.DepthTestReadOnly;
			}
			if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
			{
				return MyDepthStencilStateManager.StereoDepthTestWrite;
			}
			return MyDepthStencilStateManager.DepthTestWrite;
		}

		public void UpdateTechniqueGbuffer()
		{
			MyMeshDrawTechnique myMeshDrawTechnique = MyMeshDrawTechnique.MESH;
			if (Material != MyMeshMaterialId.NULL)
			{
				myMeshDrawTechnique = Material.Info.Technique;
			}
			if ((Flags & MyRenderableProxyFlags.DisableFaceCulling) > MyRenderableProxyFlags.None)
			{
				switch (myMeshDrawTechnique)
				{
				case MyMeshDrawTechnique.DECAL:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: true);
					GbufferBlendState = MyMeshMaterials1.GetMaterialTextureBlendState(Material.Info.TextureTypes, premultipliedAlpha: true);
					GbufferRasterizerState = MyRasterizerStateManager.NocullRasterizerState;
					break;
				case MyMeshDrawTechnique.DECAL_NOPREMULT:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: true);
					GbufferBlendState = MyMeshMaterials1.GetMaterialTextureBlendState(Material.Info.TextureTypes, premultipliedAlpha: false);
					GbufferRasterizerState = MyRasterizerStateManager.NocullRasterizerState;
					break;
				case MyMeshDrawTechnique.DECAL_CUTOUT:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: true);
					GbufferBlendState = null;
					GbufferRasterizerState = MyRasterizerStateManager.NocullRasterizerState;
					break;
				default:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: false);
					GbufferBlendState = null;
					GbufferRasterizerState = MyRasterizerStateManager.NocullRasterizerState;
					break;
				}
			}
			else
			{
				GbufferRasterizerState = null;
				switch (myMeshDrawTechnique)
				{
				case MyMeshDrawTechnique.DECAL:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: true);
					GbufferBlendState = MyMeshMaterials1.GetMaterialTextureBlendState(Material.Info.TextureTypes, premultipliedAlpha: true);
					break;
				case MyMeshDrawTechnique.DECAL_NOPREMULT:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: true);
					GbufferBlendState = MyMeshMaterials1.GetMaterialTextureBlendState(Material.Info.TextureTypes, premultipliedAlpha: false);
					break;
				case MyMeshDrawTechnique.DECAL_CUTOUT:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: true);
					GbufferBlendState = null;
					break;
				default:
					GbufferDepthState = GetDepthStencilViewGbuffer(readOnly: false);
					GbufferBlendState = null;
					break;
				}
			}
		}

		public IConstantBuffer GetObjectBuffer(MyRenderContext rc)
		{
			return rc.GetObjectCB(ObjectBufferSize);
		}

		public IConstantBuffer UpdateObjectBuffer(MyRenderContext rc)
		{
			IConstantBuffer objectCB = rc.GetObjectCB(ObjectBufferSize);
			MyMapping mapping = MyMapping.MapDiscard(rc, objectCB);
			UpdateObjectBuffer(ref mapping);
			mapping.Unmap();
			return objectCB;
		}

		public void UpdateObjectBuffer(ref MyMapping mapping)
		{
			if (NonVoxelObjectData.IsValid)
			{
				mapping.WriteAndPosition(ref NonVoxelObjectData);
			}
			else if (VoxelCommonObjectData.IsValid)
			{
				mapping.WriteAndPosition(ref VoxelCommonObjectData);
			}
			mapping.WriteAndPosition(ref CommonObjectData);
			if (SkinningMatrices == null)
			{
				return;
			}
			if (DrawSubmesh.BonesMapping == null)
			{
				mapping.WriteAndPosition(SkinningMatrices, Math.Min(60, SkinningMatrices.Length));
				return;
			}
			for (int i = 0; i < DrawSubmesh.BonesMapping.Length; i++)
			{
				mapping.WriteAndPosition(ref SkinningMatrices[DrawSubmesh.BonesMapping[i]]);
			}
		}
	}
}
