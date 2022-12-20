using System;
using System.Collections.Generic;
using VRage.Library.Extensions;
using VRage.Network;
using VRage.Render;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Culling;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene.Resources;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace VRage.Render11.Scene.Components
{
	internal class MyRenderableComponent : MyActorComponent, IMySceneResourceOwner, IMyAlphaTransitionProxy
	{
		public struct MyDebrisData
		{
			public int VoxelMaterial;

			public Vector3 ColorMultiplier;
		}

		private class VRage_Render11_Scene_Components_MyRenderableComponent_003C_003EActor : IActivator, IActivator<MyRenderableComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderableComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderableComponent CreateInstance()
			{
				return new MyRenderableComponent();
			}

			MyRenderableComponent IActivator<MyRenderableComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		internal static readonly Dictionary<uint, MyDebrisData> DebrisEntityData = new Dictionary<uint, MyDebrisData>();

		private readonly MyChildCullTreeData m_cullData;

		private long m_cullUpdatedFrame;

		private bool m_renderProxyDirty;

		protected MeshId m_mesh;

		private MyCullProxy[] m_renderableProxiesForLodTransition;

		protected InstancingId m_instancing;

		protected int m_instanceCount;

		protected int m_startInstance;

		private bool m_isGenericInstance;

		protected MyRenderLod[] m_lods;

		private int m_nextLod;

		private float m_lodTransitionState;

		private float m_lodTransitionStartDistance;

		private bool m_lodBorder;

		private bool m_skipLodTransition;

		private float m_lodDistanceMultiplierInv = 1f;

		private float m_lastLodDistanceSqr;

		private const float CUBE_SKINNING_LOD_MULTIPLIER = 0.25f;

		/// <summary>
		/// Is used in merge-instancing to indicate whether the owning actor has been merged.
		/// </summary>
		private bool m_isRenderedStandalone;

		internal MyRenderableProxyFlags AdditionalFlags;

		internal MyDrawSubmesh.MySubmeshFlags DrawFlags = MyDrawSubmesh.MySubmeshFlags.All;

		private Vector3 m_keyColor;

		private float m_objectDithering;

		private MyAlphaMode m_objectDitheringMode;

		private bool m_colorEmissivityDirty;

		internal Dictionary<MyEntityMaterialKey, MyModelProperties> ModelProperties;

		private bool m_fadeIn;

		private float m_fadeValue;

		private int m_depthBias;

		private bool m_fadeOut;

		public override Color DebugColor
		{
			get
			{
				if (!(m_instancing != InstancingId.NULL))
				{
					return Color.Blue;
				}
				return Color.GreenYellow;
			}
		}

		internal bool IsRendered
		{
			get
			{
				if (m_isRenderedStandalone)
				{
					return base.Owner.IsVisible;
				}
				return false;
			}
		}

		internal bool IsRenderedStandAlone
		{
			get
			{
				return m_isRenderedStandalone;
			}
			set
			{
				SetStandaloneRendering(value);
			}
		}

		internal MyCullProxy CullProxy { get; private set; }

		internal bool IsCulled { get; private set; }

		internal MeshId Mesh => m_mesh;

		internal int CurrentLod { get; private set; }

		internal MyRenderLod[] Lods => m_lods;

		private bool IsLodTransitionInProgress => m_lodTransitionState != 0f;

		private int LodTransitionProxyIndex
		{
			get
			{
				if (m_nextLod >= CurrentLod)
				{
					return CurrentLod;
				}
				return Math.Max(CurrentLod - 1, 0);
			}
		}

		internal int DepthBias
		{
			get
			{
				return m_depthBias;
			}
			set
			{
				if (value != m_depthBias)
				{
					m_depthBias = value;
					UpdateProxiesObjectData();
				}
			}
		}

		public event Action OnResourcesChanged;

		public MyRenderableComponent()
		{
			m_cullData = new MyChildCullTreeData
			{
				Add = delegate(MyCullResultsBase q, bool y)
				{
					MyCullResults obj2 = (MyCullResults)q;
					obj2.CullProxies.Add(CullProxy);
					obj2.CullProxiesContained.Add(y);
				},
				Remove = delegate(MyCullResultsBase q)
				{
					MyCullResults obj = (MyCullResults)q;
					int index = obj.CullProxies.IndexOf(CullProxy);
					obj.CullProxies.RemoveAt(index);
					obj.CullProxiesContained.RemoveAt(index);
				},
				DebugColor = () => DebugColor
			};
		}

		public override void Construct()
		{
			base.Construct();
			m_fadeOut = false;
			m_fadeIn = false;
			DeallocateLodProxies();
			if (CullProxy != null)
			{
				MyObjectPoolManager.Deallocate(CullProxy);
				CullProxy = null;
			}
			CullProxy = MyObjectPoolManager.Allocate<MyCullProxy>();
			m_mesh = MeshId.NULL;
			m_instancing = InstancingId.NULL;
			m_isGenericInstance = false;
			m_instanceCount = 0;
			m_startInstance = 0;
			m_isRenderedStandalone = true;
			m_keyColor = Vector3.One;
			m_objectDitheringMode = MyAlphaMode.None;
			m_objectDithering = 1f;
			if (m_renderableProxiesForLodTransition != null)
			{
				MyCullProxy[] renderableProxiesForLodTransition = m_renderableProxiesForLodTransition;
				for (int i = 0; i < renderableProxiesForLodTransition.Length; i++)
				{
					MyObjectPoolManager.Deallocate(renderableProxiesForLodTransition[i]);
				}
			}
			m_renderableProxiesForLodTransition = null;
			m_lodTransitionState = 0f;
			m_nextLod = 0;
			CurrentLod = 0;
			m_depthBias = 0;
			AdditionalFlags = MyRenderableProxyFlags.None;
			ModelProperties = new Dictionary<MyEntityMaterialKey, MyModelProperties>(MyEntityMaterialKey.Comparer);
		}

		public override void Destruct()
		{
			if (m_instancing != InstancingId.NULL)
			{
				MyInstancing.Remove(m_instancing.Info.ID, base.Owner.ID);
			}
			if (CullProxy != null)
			{
				MyObjectPoolManager.Deallocate(CullProxy);
				CullProxy = null;
			}
			DeallocateLodProxies();
			ModelProperties.Clear();
			base.Destruct();
		}

		public override void Assign(MyActor owner)
		{
			base.Assign(owner);
			MarkDirty();
			base.Owner.GetSceneResourcePrioritizationComponent().RegisterResourceOwner(this);
		}

		internal void SetLocalAabbToModelLod(int lod)
		{
			BoundingBox? boundingBox = m_mesh.GetBoundingBox(lod);
			if (boundingBox.HasValue)
			{
				base.Owner.SetLocalAabb(boundingBox.Value);
			}
		}

		internal void SetModel(MeshId mesh)
		{
			m_mesh = mesh;
			SetLocalAabbToModelLod(0);
			MarkDirty();
		}

		internal void SetKeyColor(Vector3 keyColor)
		{
			m_keyColor = keyColor;
			UpdateProxiesObjectData();
		}

		public virtual void SetAlpha(MyAlphaMode mode, float value)
		{
			m_objectDithering = value;
			m_objectDitheringMode = mode;
			UpdateProxiesCustomAlpha();
		}

		internal void SetDithering(float val)
		{
			if (val == 0f)
			{
				m_objectDithering = 1f;
				m_objectDitheringMode = MyAlphaMode.None;
			}
			else if (val < 0f)
			{
				m_objectDithering = 0f - val;
				m_objectDitheringMode = MyAlphaMode.Hologram;
			}
			else
			{
				m_objectDithering = 1f - val;
				m_objectDitheringMode = MyAlphaMode.DitherIn;
			}
			UpdateProxiesCustomAlpha();
		}

		private void UpdateProxiesCustomAlpha()
		{
			if (m_lods != null)
			{
				float y = 0f;
				switch (m_objectDitheringMode)
				{
				case MyAlphaMode.DitherIn:
					y = 1f - m_objectDithering;
					break;
				case MyAlphaMode.DitherOut:
					y = 3f - m_objectDithering;
					break;
				case MyAlphaMode.Hologram:
					y = 0f - m_objectDithering;
					break;
				}
				Vector2 currentLodValue = new Vector2(m_lodTransitionState, y);
				Vector2 otherLodvalue = new Vector2(3f - m_lodTransitionState, y);
				UpdateProxiesCustomAlpha(currentLodValue, otherLodvalue);
			}
		}

		private void UpdateProxiesCustomAlpha(Vector2 currentLodValue, Vector2 otherLodvalue)
		{
			for (int i = 0; i < m_lods.Length; i++)
			{
				MyRenderLod myRenderLod = m_lods[i];
				if (myRenderLod.RenderableProxies != null)
				{
					Vector2 customAlpha = ((i == CurrentLod) ? currentLodValue : otherLodvalue);
					MyRenderableProxy[] renderableProxies = myRenderLod.RenderableProxies;
					for (int j = 0; j < renderableProxies.Length; j++)
					{
						renderableProxies[j].CommonObjectData.CustomAlpha = customAlpha;
					}
				}
			}
		}

		private void UpdateProxiesObjectData()
		{
			if (m_lods == null)
			{
				return;
			}
			for (int i = 0; i < m_lods.Length; i++)
			{
				MyRenderLod myRenderLod = m_lods[i];
				if (myRenderLod.RenderableProxies != null)
				{
					MyRenderableProxy[] renderableProxies = myRenderLod.RenderableProxies;
					foreach (MyRenderableProxy myRenderableProxy in renderableProxies)
					{
						UpdateKeyColor(ref myRenderableProxy.CommonObjectData, m_keyColor);
						myRenderableProxy.CommonObjectData.DepthBias = m_depthBias;
						myRenderableProxy.CommonObjectData.LOD = (uint)i;
					}
				}
			}
		}

		private void UpdateKeyColor(ref MyObjectDataCommon data, Vector3 keyColor)
		{
			data.KeyColor = keyColor;
			if (keyColor.X.IsZero() && keyColor.Y.IsEqual(-1f) && keyColor.Z.IsEqual(-1f))
			{
				data.MaterialFlags |= MyMaterialFlags.NO_KEYCOLOR;
			}
			else
			{
				data.MaterialFlags &= ~MyMaterialFlags.NO_KEYCOLOR;
			}
		}

		public override void OnVisibilityChange()
		{
			base.OnVisibilityChange();
			if (IsRendered)
			{
				MarkDirty();
			}
		}

		internal void SetInstancing(InstancingId instancing)
		{
			if (m_instancing != instancing)
			{
				if (m_instancing != InstancingId.NULL)
				{
					MyInstancing.Remove(m_instancing.Info.ID, base.Owner.ID);
				}
				m_instancing = instancing;
				MyInstancing.AddRef(m_instancing.Info.ID, base.Owner.ID);
				MarkDirty();
				m_isGenericInstance = m_instancing != InstancingId.NULL && instancing.Info.Type == MyRenderInstanceBufferType.Generic;
			}
			m_lodDistanceMultiplierInv = 1f;
			if (m_instancing != InstancingId.NULL && !m_isGenericInstance && instancing.Info.EnabledSkinning)
			{
				m_lodDistanceMultiplierInv = 0.25f;
			}
		}

		internal void SetInstancingCounters(int instanceCount, int startInstance)
		{
			m_instanceCount = instanceCount;
			m_startInstance = startInstance;
			if (m_lods == null)
			{
				return;
			}
			for (int i = 0; i < m_lods.Length; i++)
			{
				for (int j = 0; j < m_lods[i].RenderableProxies.Length; j++)
				{
					m_lods[i].RenderableProxies[j].InstanceCount = instanceCount;
					m_lods[i].RenderableProxies[j].StartInstance = startInstance;
				}
			}
		}

		internal MeshId GetModel()
		{
			return m_mesh;
		}

		private void SetLodPartShaders(int lodNum, int proxyIndex, MyShaderUnifiedFlags appendedFlags)
		{
			MyRenderLod myRenderLod = m_lods[lodNum];
			MyRenderableProxy myRenderableProxy = myRenderLod.RenderableProxies[proxyIndex];
			MeshPartId meshPart = MyMeshes.GetMeshPart(m_mesh, lodNum, myRenderableProxy.PartIndex);
			MyMeshDrawTechnique technique = meshPart.Info.Material.Info.Technique;
			MyShaderUnifiedFlags myShaderUnifiedFlags = appendedFlags;
			if (meshPart.Info.Material.Info.Facing == MyFacingEnum.Impostor)
			{
				myShaderUnifiedFlags |= MyShaderUnifiedFlags.ALPHA_MASK_ARRAY;
				if (MyRender11.Settings.User.AlphaMaskedShaderQuality == MyRenderQualityEnum.LOW)
				{
					myShaderUnifiedFlags |= MyShaderUnifiedFlags.LQ;
				}
				else if (MyRender11.Settings.User.AlphaMaskedShaderQuality == MyRenderQualityEnum.NORMAL)
				{
					myShaderUnifiedFlags |= MyShaderUnifiedFlags.MQ;
				}
			}
			if (meshPart.Info.Material.Info.GeometryTextureRef.IsUsed && MyRender11.Settings.UseGeometryArrayTextures)
			{
				myShaderUnifiedFlags |= MyShaderUnifiedFlags.USE_TEXTURE_INDICES;
			}
			switch (meshPart.Info.Material.Info.Technique)
			{
			case MyMeshDrawTechnique.DECAL:
			case MyMeshDrawTechnique.DECAL_NOPREMULT:
				myShaderUnifiedFlags |= MyShaderUnifiedFlags.STATIC_DECAL;
				break;
			case MyMeshDrawTechnique.DECAL_CUTOUT:
				myShaderUnifiedFlags |= MyShaderUnifiedFlags.STATIC_DECAL_CUTOUT;
				break;
			}
			if (DebrisEntityData.ContainsKey(base.Owner.ID))
			{
				technique = MyMeshDrawTechnique.VOXELS_DEBRIS;
			}
			MyStringId shaderMaterial = MyMaterialShaders.MapTechniqueToShaderMaterial(technique);
			AssignShadersToProxy(myRenderableProxy, shaderMaterial, myRenderLod.VertexLayout1, myRenderLod.VertexShaderFlags | MapTechniqueToShaderMaterialFlags(technique) | myShaderUnifiedFlags, MyStringId.NullOrEmpty);
		}

		protected void AssignShadersToProxy(MyRenderableProxy renderableProxy, MyStringId shaderMaterial, VertexLayoutId vertexLayoutId, MyShaderUnifiedFlags shaderFlags, MyStringId forwardMaterial)
		{
			MyFileTextureEnum textureTypes = MyFileTextureEnum.UNSPECIFIED;
			MyMeshDrawTechnique technique = MyMeshDrawTechnique.MESH;
			if (renderableProxy.Material != MyMeshMaterialId.NULL)
			{
				textureTypes = renderableProxy.Material.Info.TextureTypes;
				technique = renderableProxy.Material.Info.Technique;
			}
			renderableProxy.Shaders = MyMaterialShaders.Get(shaderMaterial, MyMaterialShaders.MapTechniqueToDefaultPass(technique), vertexLayoutId, shaderFlags, textureTypes);
			renderableProxy.DepthShaders = MyMaterialShaders.Get(shaderMaterial, MyMaterialShaders.DEPTH_PASS_ID, vertexLayoutId, shaderFlags | MyShaderUnifiedFlags.DEPTH_ONLY, textureTypes);
			renderableProxy.HighlightShaders = MyMaterialShaders.Get(shaderMaterial, MyMaterialShaders.HIGHLIGHT_PASS_ID, vertexLayoutId, shaderFlags, textureTypes);
			renderableProxy.ForwardShaders = MyMaterialShaders.Get((forwardMaterial == MyStringId.NullOrEmpty) ? shaderMaterial : forwardMaterial, MyMaterialShaders.FORWARD_PASS_ID, vertexLayoutId, (shaderFlags | MyShaderUnifiedFlags.LQ) & ~MyShaderUnifiedFlags.MQ, textureTypes);
			renderableProxy.TransparentDepthShaders = MyMaterialShaders.Get(shaderMaterial, MyMaterialShaders.MapTechniqueToDefaultPass(technique), vertexLayoutId, shaderFlags, textureTypes);
		}

		private Vector4 GetUvScaleOffset(Vector2I uvTiles, Vector2I tileIndex)
		{
			return new Vector4(uvTiles.X, uvTiles.Y, 1f / (float)uvTiles.X * (float)tileIndex.X, 1f / (float)uvTiles.Y * (float)tileIndex.Y);
		}

		protected virtual bool RebuildLodProxy(int lodNum, bool skinningEnabled, MySkinningComponent skinning, MyShaderUnifiedFlags appendFlags)
		{
			LodMeshId lodMesh = MyMeshes.GetLodMesh(m_mesh, lodNum);
			MyLodMeshInfo info = lodMesh.Info;
			MyObjectPoolManager.Init(ref m_lods[lodNum]);
			MyRenderLod myRenderLod = m_lods[lodNum];
			myRenderLod.Distance = lodMesh.Info.LodDistance;
			m_lastLodDistanceSqr = Math.Max(myRenderLod.Distance * myRenderLod.Distance, m_lastLodDistanceSqr);
			Matrix[] skinningMatrices = null;
			MyShaderUnifiedFlags myShaderUnifiedFlags = MyShaderUnifiedFlags.NONE;
			if (skinningEnabled)
			{
				skinningMatrices = skinning.SkinMatrices;
				myShaderUnifiedFlags |= MyShaderUnifiedFlags.USE_SKINNING;
			}
			if (m_instancing != InstancingId.NULL && !info.NullLodMesh)
			{
				myRenderLod.VertexLayout1 = MyVertexLayouts.GetLayout(lodMesh.VertexLayout, m_instancing.Info.Layout);
				if (m_instancing.Info.Type == MyRenderInstanceBufferType.Cube)
				{
					myShaderUnifiedFlags = ((!lodMesh.VertexLayout.HasBonesInfo) ? (myShaderUnifiedFlags | MyShaderUnifiedFlags.USE_CUBE_INSTANCING) : (myShaderUnifiedFlags | MyShaderUnifiedFlags.USE_DEFORMED_CUBE_INSTANCING));
				}
				else if (m_instancing.Info.Type == MyRenderInstanceBufferType.Generic)
				{
					myShaderUnifiedFlags |= MyShaderUnifiedFlags.USE_GENERIC_INSTANCING;
				}
			}
			else
			{
				myRenderLod.VertexLayout1 = lodMesh.VertexLayout;
			}
			myRenderLod.VertexShaderFlags = myShaderUnifiedFlags;
			if (lodMesh.Buffers.IB == null || lodMesh.Buffers == MyMeshBuffers.Empty)
			{
				myRenderLod.AllocateProxies(0);
				return true;
			}
			int num = info.PartsNum;
			for (int i = 0; i < num; i++)
			{
				if (!MyMeshes.GetMeshPart(m_mesh, lodNum, i).Info.IsValid())
				{
					num--;
				}
			}
			myRenderLod.AllocateProxies(num);
			int num2 = 0;
			for (int j = 0; j < info.PartsNum; j++)
			{
				if (CreateRenderableProxyForPart(lodNum, j, num2, GetConstantBufferSize(myRenderLod, skinningEnabled), skinningMatrices, appendFlags))
				{
					num2++;
				}
			}
			return true;
		}

		private bool CreateRenderableProxyForPart(int lodIndex, int fetchPartIndex, int partIndex, int objectConstantsSize, Matrix[] skinningMatrices, MyShaderUnifiedFlags appendFlags)
		{
			MyRenderLod myRenderLod = m_lods[lodIndex];
			LodMeshId lodMesh = MyMeshes.GetLodMesh(m_mesh, lodIndex);
			MeshPartId meshPart = MyMeshes.GetMeshPart(m_mesh, lodIndex, fetchPartIndex);
			MyMeshDrawTechnique technique = meshPart.Info.Material.Info.Technique;
			if (!meshPart.Info.IsValid())
			{
				return false;
			}
			int num = -1;
			Vector3 colorMul = MyModelProperties.DefaultColorMul;
			if (DebrisEntityData.ContainsKey(base.Owner.ID))
			{
				technique = MyMeshDrawTechnique.VOXEL_MAP_SINGLE;
				num = DebrisEntityData[base.Owner.ID].VoxelMaterial;
				colorMul = DebrisEntityData[base.Owner.ID].ColorMultiplier;
			}
			MyRenderableProxy myRenderableProxy = myRenderLod.RenderableProxies[partIndex];
			myRenderableProxy.PartIndex = fetchPartIndex;
			myRenderableProxy.CommonObjectData.Emissive = MyModelProperties.DefaultEmissivity;
			myRenderableProxy.CommonObjectData.ColorMul = colorMul;
			myRenderableProxy.CommonObjectData.DepthBias = m_depthBias;
			myRenderableProxy.CommonObjectData.LOD = (uint)lodIndex;
			myRenderableProxy.NonVoxelObjectData.Facing = (int)meshPart.Info.Material.Info.Facing;
			myRenderableProxy.NonVoxelObjectData.WindScaleAndFreq = meshPart.Info.Material.Info.WindScaleAndFreq;
			myRenderableProxy.Technique = technique;
			myRenderableProxy.VoxelCommonObjectData = MyObjectDataVoxelCommon.Invalid;
			if (meshPart.Info.Material.Info.Facing == MyFacingEnum.Full || meshPart.Info.Material.Info.Facing == MyFacingEnum.Impostor)
			{
				myRenderableProxy.NonVoxelObjectData.CenterOffset = meshPart.Info.CenterOffset;
			}
			else
			{
				myRenderableProxy.NonVoxelObjectData.CenterOffset = Vector3.Zero;
			}
			MyMeshPartInfo1 info = meshPart.Info;
			myRenderableProxy.Mesh = lodMesh;
			myRenderableProxy.Material = info.Material;
			myRenderableProxy.TransparentTechnique = myRenderableProxy.Material.Info.Technique.IsTransparent();
			myRenderableProxy.UnusedMaterials = info.UnusedMaterials ?? new HashSet<string>();
			SetLodPartShaders(lodIndex, partIndex, appendFlags);
			MyDrawSubmesh myDrawSubmesh = default(MyDrawSubmesh);
			myDrawSubmesh.BaseVertex = info.BaseVertex;
			myDrawSubmesh.StartIndex = info.StartIndex;
			myDrawSubmesh.IndexCount = info.IndexCount;
			myDrawSubmesh.BonesMapping = info.BonesMapping;
			myDrawSubmesh.MaterialId = MyMeshMaterials1.GetProxyId(info.Material);
			myDrawSubmesh.Flags = DrawFlags;
			MyDrawSubmesh drawSubmesh = myDrawSubmesh;
			if (num != -1)
			{
				drawSubmesh.MaterialId = MyVoxelMaterials.GetMaterialProxyId(num);
			}
			myRenderableProxy.DrawSubmesh = drawSubmesh;
			MyDrawSubmesh[] array = new MyDrawSubmesh[meshPart.Info.SectionSubmeshCount];
			int num2 = 0;
			string[] sectionNames = lodMesh.Info.SectionNames;
			foreach (string section in sectionNames)
			{
				MyMeshSectionPartInfo1[] meshes = MyMeshes.GetMeshSection(m_mesh, lodIndex, section).Info.Meshes;
				for (int j = 0; j < meshes.Length; j++)
				{
					if (meshes[j].PartIndex == fetchPartIndex)
					{
						int num3 = num2;
						myDrawSubmesh = new MyDrawSubmesh
						{
							BaseVertex = meshes[j].BaseVertex,
							StartIndex = meshes[j].StartIndex,
							IndexCount = meshes[j].IndexCount,
							BonesMapping = drawSubmesh.BonesMapping,
							MaterialId = drawSubmesh.MaterialId,
							Flags = drawSubmesh.Flags
						};
						array[num3] = myDrawSubmesh;
						num2++;
					}
				}
			}
			myRenderableProxy.SectionSubmeshes = array;
			if (drawSubmesh.BonesMapping != null && skinningMatrices != null)
			{
				for (int k = 0; k < drawSubmesh.BonesMapping.Length; k++)
				{
					if (drawSubmesh.BonesMapping[k] >= skinningMatrices.Length)
					{
						drawSubmesh.BonesMapping[k] = skinningMatrices.Length - 1;
					}
				}
			}
			myRenderableProxy.SkinningMatrices = skinningMatrices;
			myRenderableProxy.ObjectBufferSize = objectConstantsSize;
			myRenderableProxy.ObjectBufferSizeAligned = MathHelper.Align(objectConstantsSize, 256);
			myRenderableProxy.InstanceCount = m_instanceCount;
			myRenderableProxy.StartInstance = m_startInstance;
			myRenderableProxy.Flags = MapTechniqueToRenderableFlags(technique) | GetRenderAdditionalFlags();
			myRenderableProxy.Type = MapTechniqueToMaterialType(technique);
			myRenderableProxy.Parent = this;
			myRenderableProxy.Instancing = m_instancing;
			MyPerMaterialData perMaterialData = default(MyPerMaterialData);
			perMaterialData.Type = MyMaterialTypeEnum.STANDARD;
			FillPerMaterialData(ref perMaterialData, technique);
			myRenderableProxy.PerMaterialIndex = (int)perMaterialData.Type;
			myRenderableProxy.UpdateTechniqueGbuffer();
			ulong key = 0uL;
			My64BitValueHelper.SetBits(ref key, 50, 1, (ulong)(int)(MyMaterialShaders.IsDecal(technique) ? 1u : 0u));
			My64BitValueHelper.SetBits(ref key, 45, 5, (ulong)MyMaterialShaders.MapTechniqueToSortKey(technique));
			My64BitValueHelper.SetBits(ref key, 20, 25, (ulong)myRenderLod.VertexShaderFlags);
			My64BitValueHelper.SetBits(ref key, 14, 6, (ulong)myRenderLod.VertexLayout1.Index);
			My64BitValueHelper.SetBits(ref key, 7, 7, (ulong)myRenderableProxy.Mesh.Index);
			My64BitValueHelper.SetBits(ref key, 0, 7, (ulong)myRenderableProxy.Material.GetHashCode());
			myRenderLod.SortingKeys[partIndex] = key;
			return true;
		}

		protected MyRenderableProxyFlags GetRenderAdditionalFlags()
		{
			if (MyRender11.Settings.User.ShadowQuality == MyShadowsQuality.LOW && (AdditionalFlags & MyRenderableProxyFlags.CastShadowsOnLow) == 0)
			{
				return AdditionalFlags & ~MyRenderableProxyFlags.CastShadows;
			}
			return AdditionalFlags;
		}

		protected unsafe static int GetConstantBufferSize(MyRenderLod lod, bool skinningEnabled)
		{
			int num = sizeof(MyObjectDataCommon);
			num = (((lod.VertexShaderFlags & MyShaderUnifiedFlags.USE_VOXEL_DATA) == MyShaderUnifiedFlags.USE_VOXEL_DATA) ? (num + sizeof(MyObjectDataVoxelCommon)) : (num + sizeof(MyObjectDataNonVoxel)));
			if (skinningEnabled)
			{
				num += sizeof(Matrix) * 60;
			}
			return num;
		}

		public override void OnUpdateBeforeDraw()
		{
			base.OnUpdateBeforeDraw();
			if (m_renderProxyDirty)
			{
				RebuildRenderProxies();
				this.OnResourcesChanged.InvokeIfNotNull();
			}
		}

		public virtual IEnumerable<ResourceInfo> GetResources()
		{
			if (m_lods == null)
			{
				yield break;
			}
			int lodCount = m_mesh.Info.LodsNum;
			int lodNum = 0;
			ResourceInfo resourceInfo;
			while (lodNum < lodCount)
			{
				MyRenderableProxy[] renderableProxies = m_lods[lodNum].RenderableProxies;
				for (int i = 0; i < renderableProxies.Length; i++)
				{
					MyMaterialProxyId materialId = renderableProxies[i].DrawSubmesh.MaterialId;
					if (!(materialId != MyMaterialProxyId.NULL))
					{
						continue;
					}
					IMyStreamedTexture[] textureHandles = materialId.Info.MaterialSrvs.TextureHandles;
					if (textureHandles != null)
					{
						IMyStreamedTexture[] array = textureHandles;
						foreach (IMyStreamedTexture resource in array)
						{
							resourceInfo = new ResourceInfo
							{
								Resource = resource,
								UseCount = 1
							};
							yield return resourceInfo;
						}
					}
				}
				int num = lodNum + 1;
				lodNum = num;
			}
			if (!DebrisEntityData.TryGetValue(base.Owner.ID, out var value))
			{
				yield break;
			}
			IEnumerable<IMyStreamedTextureArrayTile> materialTiles = MyVoxelMaterials.GetMaterialTiles(value.VoxelMaterial);
			if (materialTiles == null)
			{
				yield break;
			}
			foreach (IMyStreamedTextureArrayTile item in materialTiles)
			{
				resourceInfo = new ResourceInfo
				{
					Resource = item,
					UseCount = 1
				};
				yield return resourceInfo;
			}
		}

		private void RebuildRenderProxies()
		{
			if (!base.Owner.HasLocalAabb)
			{
				SetLocalAabbToModelLod(0);
			}
			MySkinningComponent skinning = base.Owner.GetSkinning();
			bool skinningEnabled = skinning != null && skinning.SkinMatrices != null;
			DeallocateLodProxies();
			MyArrayHelpers.InitOrReserve(ref m_lods, m_mesh.Info.LodsNum);
			m_lastLodDistanceSqr = 0f;
			MyShaderUnifiedFlags appendFlags = (((AdditionalFlags & MyRenderableProxyFlags.DistanceFade) > MyRenderableProxyFlags.None) ? MyShaderUnifiedFlags.DISTANCE_FADE : MyShaderUnifiedFlags.NONE) | (((AdditionalFlags & MyRenderableProxyFlags.MetalnessColorable) > MyRenderableProxyFlags.None) ? MyShaderUnifiedFlags.METALNESS_COLORABLE : MyShaderUnifiedFlags.NONE);
			for (int i = 0; i < m_lods.Length; i++)
			{
				if (!RebuildLodProxy(i, skinningEnabled, skinning, appendFlags))
				{
					CullProxy.Parent = null;
					MarkClean();
					DeallocateLodProxies();
					base.Owner.InvalidateCullTreeData();
					return;
				}
			}
			CullProxy.ResetMatrixIndex();
			CullProxy.Parent = this;
			CullProxy.RenderFlags = GetRenderAdditionalFlags();
			CurrentLod = (m_nextLod = 0);
			RebuildProxiesForLodTransition();
			base.Owner.InvalidateCullTreeData();
			if (m_lods.Length != 0)
			{
				float distanceFromCamera = CalculateViewerDistance(CalculateViewerDistanceSquared());
				CurrentLod = FindLod(distanceFromCamera);
				m_nextLod = CurrentLod;
				m_lodTransitionState = 0f;
				CullProxy.RenderableProxies = m_lods[CurrentLod].RenderableProxies;
				CullProxy.SortingKeys = m_lods[CurrentLod].SortingKeys;
				UpdateProxiesObjectData();
			}
			else
			{
				CullProxy.Clear();
			}
			UpdateProxiesCustomAlpha();
			UpdateAfterCull();
			if (MyScene11.EntityMaterialRenderFlagChanges.TryGetValue(base.Owner.ID, out var value))
			{
				foreach (KeyValuePair<MyEntityMaterialKey, RenderableFlagsChange> item in value)
				{
					for (int j = 0; j < m_lods.Length; j++)
					{
						MyRenderLod myRenderLod = m_lods[j];
						for (int k = 0; k < myRenderLod.RenderableProxies.Length; k++)
						{
							MyRenderableProxy myRenderableProxy = myRenderLod.RenderableProxies[k];
							if (MyMeshes.GetMeshPart(m_mesh, j, myRenderableProxy.PartIndex).Info.Material.Info.Name == item.Key.Material)
							{
								myRenderableProxy.Flags |= item.Value.Add;
								myRenderableProxy.Flags &= ~item.Value.Remove;
							}
						}
					}
				}
			}
			int lodsNum = m_mesh.Info.LodsNum;
			foreach (KeyValuePair<MyEntityMaterialKey, MyModelProperties> modelProperty in ModelProperties)
			{
				for (int l = 0; l < lodsNum; l++)
				{
					MyRenderLod myRenderLod2 = m_lods[l];
					for (int m = 0; m < myRenderLod2.RenderableProxies.Length; m++)
					{
						MyRenderableProxy myRenderableProxy2 = myRenderLod2.RenderableProxies[m];
						MeshPartId meshPart = MyMeshes.GetMeshPart(m_mesh, l, myRenderableProxy2.PartIndex);
						if (!(meshPart.Info.Material.Info.Name == modelProperty.Key.Material))
						{
							continue;
						}
						myRenderableProxy2.CommonObjectData.Emissive = modelProperty.Value.Emissivity;
						myRenderableProxy2.CommonObjectData.ColorMul = modelProperty.Value.ColorMul;
						if (modelProperty.Value.TextureChange.HasValue)
						{
							MyMeshMaterialInfo desc = meshPart.Info.Material.Info;
							_ = desc.GeometryTextureRef.IsUsed;
							MyTextureChange value2 = modelProperty.Value.TextureChange.Value;
							if (value2.ColorMetalFileName != null)
							{
								desc.ColorMetal_Texture = MyResourceUtils.GetTextureFullPath(value2.ColorMetalFileName);
							}
							if (value2.NormalGlossFileName != null)
							{
								desc.NormalGloss_Texture = MyResourceUtils.GetTextureFullPath(value2.NormalGlossFileName);
							}
							if (value2.ExtensionsFileName != null)
							{
								desc.Extensions_Texture = MyResourceUtils.GetTextureFullPath(value2.ExtensionsFileName);
							}
							if (value2.AlphamaskFileName != null)
							{
								desc.Alphamask_Texture = MyResourceUtils.GetTextureFullPath(value2.AlphamaskFileName);
							}
							MyMaterialProxyId proxyId = MyMeshMaterials1.GetProxyId(MyMeshMaterials1.GetMaterialId(ref desc));
							myRenderableProxy2.DrawSubmesh.MaterialId = proxyId;
						}
					}
				}
			}
			if (m_colorEmissivityDirty && IsRendered && m_lods != null)
			{
				m_colorEmissivityDirty = false;
				MyRenderLod[] lods = m_lods;
				for (int n = 0; n < lods.Length; n++)
				{
					MyRenderableProxy[] renderableProxies = lods[n].RenderableProxies;
					foreach (MyRenderableProxy obj in renderableProxies)
					{
						obj.CommonObjectData.Emissive = MyModelProperties.DefaultEmissivity;
						obj.CommonObjectData.ColorMul = MyModelProperties.DefaultColorMul;
					}
				}
				foreach (KeyValuePair<MyEntityMaterialKey, MyModelProperties> modelProperty2 in ModelProperties)
				{
					lods = m_lods;
					for (int n = 0; n < lods.Length; n++)
					{
						MyRenderableProxy[] renderableProxies = lods[n].RenderableProxies;
						foreach (MyRenderableProxy myRenderableProxy3 in renderableProxies)
						{
							if (myRenderableProxy3.UnusedMaterials != null)
							{
								myRenderableProxy3.UnusedMaterials.Contains(modelProperty2.Key.Material.String);
							}
							if (myRenderableProxy3.Material.Info.Name == modelProperty2.Key.Material)
							{
								myRenderableProxy3.CommonObjectData.Emissive = modelProperty2.Value.Emissivity;
								myRenderableProxy3.CommonObjectData.ColorMul = modelProperty2.Value.ColorMul;
							}
						}
					}
				}
			}
			m_colorEmissivityDirty = false;
			MarkClean();
		}

		private void RebuildProxiesForLodTransition()
		{
			if (m_lods.Length != 0)
			{
				if (m_renderableProxiesForLodTransition != null)
				{
					for (int i = 0; i < m_renderableProxiesForLodTransition.Length; i++)
					{
						MyObjectPoolManager.Deallocate(m_renderableProxiesForLodTransition[i]);
						m_renderableProxiesForLodTransition[i] = null;
					}
				}
				Array.Resize(ref m_renderableProxiesForLodTransition, m_lods.Length);
			}
			for (int j = 0; j < m_lods.Length; j++)
			{
				m_renderableProxiesForLodTransition[j] = MyObjectPoolManager.Allocate<MyCullProxy>();
				MyCullProxy myCullProxy = m_renderableProxiesForLodTransition[j];
				bool num = j + 1 < m_lods.Length;
				int num2 = m_lods[j].RenderableProxies.Length;
				int num3 = 0;
				int num4 = num2;
				if (num)
				{
					num3 = m_lods[j + 1].RenderableProxies.Length;
					num4 += num3;
				}
				Array.Resize(ref myCullProxy.RenderableProxies, num4);
				Array.Copy(m_lods[j].RenderableProxies, myCullProxy.RenderableProxies, num2);
				Array.Resize(ref myCullProxy.SortingKeys, num4);
				Array.Copy(m_lods[j].SortingKeys, myCullProxy.SortingKeys, Math.Min(num2, myCullProxy.SortingKeys.Length));
				if (num)
				{
					Array.Copy(m_lods[j + 1].RenderableProxies, 0, myCullProxy.RenderableProxies, num2, num3);
					if (num2 < myCullProxy.SortingKeys.Length)
					{
						Array.Copy(m_lods[j + 1].SortingKeys, 0, myCullProxy.SortingKeys, num2, Math.Min(num3, myCullProxy.SortingKeys.Length - num2));
					}
				}
			}
		}

		private int FindLod(float distanceFromCamera)
		{
			int num = 0;
			if (distanceFromCamera > 0f)
			{
				num = 1000;
				for (int i = 0; i < m_lods.Length; i++)
				{
					if (m_lods[i].Distance <= distanceFromCamera && (i == m_lods.Length - 1 || distanceFromCamera < m_lods[i + 1].Distance))
					{
						num = i;
					}
				}
			}
			num = Math.Max(MyCommon.LoddingSettings.GBuffer.MinLod, num + MyCommon.LoddingSettings.GBuffer.LodShift);
			return Math.Min(m_lods.Length - 1, num);
		}

		private float CalculateViewerDistance(float distanceSqr)
		{
			return ((float)Math.Sqrt(distanceSqr) * MyCommon.LODCoefficient + MyCommon.LoddingSettings.Global.ObjectDistanceAdd) * MyCommon.LoddingSettings.Global.ObjectDistanceMult * m_lodDistanceMultiplierInv;
		}

		public void UpdateColorEmissivity(int lod, string materialName, Color diffuse, float emissivity)
		{
			MyEntityMaterialKey key = new MyEntityMaterialKey(materialName);
			if (!ModelProperties.TryGetValue(key, out var value))
			{
				value = new MyModelProperties();
				ModelProperties[key] = value;
			}
			value.Emissivity = emissivity;
			value.ColorMul = diffuse;
			m_colorEmissivityDirty = true;
			MarkDirty();
		}

		private void DeallocateLodProxies()
		{
			if (m_lods == null)
			{
				return;
			}
			MyRenderLod[] lods = m_lods;
			foreach (MyRenderLod myRenderLod in lods)
			{
				if (myRenderLod != null)
				{
					MyObjectPoolManager.Deallocate(myRenderLod);
				}
			}
			m_lods = null;
		}

		internal void SetProxiesForCurrentLod()
		{
			if (IsLodTransitionInProgress)
			{
				int lodTransitionProxyIndex = LodTransitionProxyIndex;
				CullProxy.RenderableProxies = m_renderableProxiesForLodTransition[lodTransitionProxyIndex].RenderableProxies;
				CullProxy.SortingKeys = m_renderableProxiesForLodTransition[lodTransitionProxyIndex].SortingKeys;
			}
			else
			{
				CullProxy.RenderableProxies = m_lods[CurrentLod].RenderableProxies;
				CullProxy.SortingKeys = m_lods[CurrentLod].SortingKeys;
			}
			CullProxy.ResetMatrixIndex();
		}

		public void SkipNextLodTransition()
		{
			m_skipLodTransition = true;
		}

		private void UpdateLodState(float distanceFromCameraSqr)
		{
			if (m_fadeIn || m_fadeOut)
			{
				float num = MyCommon.GetLastFrameDelta() / MyCommon.LoddingSettings.Global.MaxTransitionInSeconds;
				if (m_fadeIn)
				{
					m_fadeValue += num;
					if (m_fadeValue >= 1f)
					{
						m_fadeIn = false;
						SetAlpha(MyAlphaMode.None, 1f);
					}
					else
					{
						SetAlpha(MyAlphaMode.DitherIn, m_fadeValue);
					}
				}
				else
				{
					m_fadeValue -= num;
					if (m_fadeValue < 0f)
					{
						m_fadeValue = 0f;
					}
					SetAlpha(MyAlphaMode.DitherOut, m_fadeValue);
				}
			}
			if (m_lods.Length <= 1 || !MyCommon.LoddingSettings.Global.IsUpdateEnabled)
			{
				return;
			}
			if (MyCommon.LoddingSettings.Global.EnableLodSelection)
			{
				m_lodTransitionState = 0f;
				CurrentLod = (m_nextLod = Math.Min(MyCommon.LoddingSettings.Global.LodSelection, m_lods.Length - 1));
				SetProxiesForCurrentLod();
				UpdateProxiesCustomAlpha();
				return;
			}
			float num2 = (float)Math.Sqrt(distanceFromCameraSqr);
			num2 *= MyCommon.LODCoefficient;
			num2 += MyCommon.LoddingSettings.Global.ObjectDistanceAdd;
			num2 *= MyCommon.LoddingSettings.Global.ObjectDistanceMult;
			num2 *= m_lodDistanceMultiplierInv;
			if (IsLodTransitionInProgress)
			{
				float transitionDelta = MyLodUtils.GetTransitionDelta(Math.Abs(num2 - m_lodTransitionStartDistance), m_lodTransitionState, m_nextLod);
				m_lodTransitionState += transitionDelta;
				if (m_lodTransitionState >= 1f)
				{
					CurrentLod = m_nextLod;
					m_lodTransitionState = 0f;
					SetProxiesForCurrentLod();
				}
				UpdateProxiesCustomAlpha();
				return;
			}
			if (m_lodBorder)
			{
				if (Math.Abs(num2 - m_lods[CurrentLod].Distance) > m_lods[CurrentLod].Distance * 0.1f)
				{
					m_lodBorder = false;
				}
				return;
			}
			int num3 = FindLod(num2);
			if (num3 != CurrentLod)
			{
				if (m_skipLodTransition)
				{
					m_lodTransitionState = 0f;
					CurrentLod = (m_nextLod = num3);
					SetProxiesForCurrentLod();
				}
				else
				{
					m_nextLod = num3;
					m_lodTransitionState = 0.001f;
					m_lodTransitionStartDistance = num2;
					m_lodBorder = true;
					SetProxiesForCurrentLod();
				}
				UpdateProxiesCustomAlpha();
			}
			m_skipLodTransition = false;
		}

		private bool CheckDistanceCulling(float distanceSqr)
		{
			bool result = false;
			if (distanceSqr > m_lastLodDistanceSqr)
			{
				Vector3 position = new Vector3(base.Owner.VolumeExtent, base.Owner.VolumeExtent, 0.0 - Math.Sqrt(distanceSqr));
				Vector3.TransformProjection(ref position, ref MyRender11.Environment.Matrices.Projection, out var result2);
				Vector2 vector = new Vector2(result2.X, result2.Y) * MyRender11.ResolutionF / 2f;
				if (Math.Abs(vector.X * vector.Y) < 20f)
				{
					result = true;
				}
			}
			return result;
		}

		internal virtual void UpdateAfterCull()
		{
			m_cullUpdatedFrame = MyCommon.FrameCounter;
			float num = CalculateViewerDistanceSquared();
			IsCulled = CheckDistanceCulling(num);
			if (!IsCulled)
			{
				UpdateLodState(num);
			}
		}

		public override void OnRemove(MyActor owner)
		{
			if (GetModel().Info.Dynamic)
			{
				MyMeshes.RemoveMesh(GetModel());
			}
			DebrisEntityData.Remove(owner.ID);
			DeallocateLodProxies();
			base.Owner.GetSceneResourcePrioritizationComponent().UnregisterResourceOwner(this);
			base.OnRemove(owner);
		}

		public override MyChildCullTreeData GetCullTreeData()
		{
			if (IsRendered && CullProxy.Parent != null)
			{
				return m_cullData;
			}
			return null;
		}

		private void SetStandaloneRendering(bool val)
		{
			if (m_isRenderedStandalone != val)
			{
				if (val)
				{
					MarkDirty();
				}
				else
				{
					base.Owner.InvalidateCullTreeData();
				}
			}
			m_isRenderedStandalone = val;
		}

		private float CalculateViewerDistanceSquared()
		{
			return base.Owner.CalculateCameraDistanceSquared();
		}

		private static void FillPerMaterialData(ref MyPerMaterialData perMaterialData, MyMeshDrawTechnique technique)
		{
			perMaterialData.Type = MyMaterialTypeEnum.STANDARD;
			if (technique == MyMeshDrawTechnique.FOLIAGE)
			{
				perMaterialData.Type = MyMaterialTypeEnum.FOLIAGE;
			}
		}

		internal static MyShaderUnifiedFlags MapTechniqueToShaderMaterialFlags(MyMeshDrawTechnique technique)
		{
			MyShaderUnifiedFlags myShaderUnifiedFlags = MyShaderUnifiedFlags.USE_SHADOW_CASCADES;
			if (technique - 3 <= MyMeshDrawTechnique.VOXEL_MAP)
			{
				return myShaderUnifiedFlags | MyShaderUnifiedFlags.ALPHA_MASKED;
			}
			return myShaderUnifiedFlags | MyShaderUnifiedFlags.NONE;
		}

		internal static MyRenderableProxyFlags MapTechniqueToRenderableFlags(MyMeshDrawTechnique technique)
		{
			switch (technique)
			{
			case MyMeshDrawTechnique.ALPHA_MASKED:
			case MyMeshDrawTechnique.FOLIAGE:
			case MyMeshDrawTechnique.CLOUD_LAYER:
				return MyRenderableProxyFlags.DisableFaceCulling;
			case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
				return MyRenderableProxyFlags.None;
			default:
				return MyRenderableProxyFlags.DepthSkipTextures;
			}
		}

		internal static MyMaterialType MapTechniqueToMaterialType(MyMeshDrawTechnique technique)
		{
			if (technique - 3 <= MyMeshDrawTechnique.VOXEL_MAP)
			{
				return MyMaterialType.ALPHA_MASKED;
			}
			return MyMaterialType.OPAQUE;
		}

		internal static void MarkAllDirty()
		{
			foreach (MyRenderableComponent item in MyComponentFactory<MyRenderableComponent>.GetAll())
			{
				item.MarkDirty();
			}
		}

		public override void OnParentSet()
		{
			MarkDirty();
		}

		internal void MarkDirty()
		{
			if (!base.Owner.IsDestroyed && !(m_mesh == MeshId.NULL))
			{
				base.Owner.Scene.Updater.AddToNextUpdate(base.Owner);
				m_renderProxyDirty = true;
			}
		}

		private void MarkClean()
		{
			m_renderProxyDirty = false;
		}

		private void AddTextureChange(string materialName, MyTextureChange textureChange)
		{
			MyEntityMaterialKey key = new MyEntityMaterialKey(materialName);
			if (!ModelProperties.TryGetValue(key, out var value))
			{
				value = new MyModelProperties();
				ModelProperties[key] = value;
			}
			if (textureChange.IsDefault())
			{
				value.TextureChange = null;
			}
			else
			{
				value.TextureChange = textureChange;
			}
			MarkDirty();
		}

		public void AddTextureChanges(IEnumerable<KeyValuePair<string, MyTextureChange>> changes)
		{
			foreach (KeyValuePair<string, MyTextureChange> change in changes)
			{
				AddTextureChange(change.Key, change.Value);
			}
		}

		public void ClearTextureChanges()
		{
			foreach (KeyValuePair<MyEntityMaterialKey, MyModelProperties> modelProperty in ModelProperties)
			{
				modelProperty.Value.TextureChange = null;
			}
			MarkDirty();
		}

		public MyTextureChange? GetTextureChange(string materialName)
		{
			MyEntityMaterialKey key = new MyEntityMaterialKey(materialName);
			if (ModelProperties.TryGetValue(key, out var value))
			{
				return value.TextureChange;
			}
			return null;
		}

		public virtual void TransitionStart(MyAlphaTransitionDirection direction)
		{
		}

		public virtual void TransitionComplete(MyAlphaTransitionDirection visible, Action<uint> transitionFinishedCallback = null)
		{
		}

		public override bool StartFadeOut()
		{
			if (m_lods == null || m_lods.Length == 0 || CurrentLod == -1)
			{
				return true;
			}
			if (!m_fadeIn)
			{
				m_fadeValue = 1f;
			}
			m_fadeIn = false;
			m_fadeOut = true;
			SetAlpha(MyAlphaMode.DitherOut, m_fadeValue);
			return false;
		}

		public void StartFadeIn()
		{
			if (!m_fadeOut)
			{
				m_fadeIn = true;
				m_fadeValue = 0f;
				SetAlpha(MyAlphaMode.DitherIn, m_fadeValue);
			}
		}

		public void StopFadeIn()
		{
			if (m_fadeIn)
			{
				m_fadeIn = false;
				SetAlpha(MyAlphaMode.None, 1f);
			}
		}

		public void SetModelProperties(MyEntityMaterialKey key, float? emissivity, Color? color)
		{
			if ((emissivity.HasValue || color.HasValue) && !ModelProperties.ContainsKey(key))
			{
				ModelProperties[key] = new MyModelProperties();
			}
			if (emissivity.HasValue)
			{
				ModelProperties[key].Emissivity = emissivity.Value;
			}
			if (color.HasValue)
			{
				ModelProperties[key].ColorMul = color.Value;
			}
			MarkDirty();
		}
	}
}
