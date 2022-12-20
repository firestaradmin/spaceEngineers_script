using ParallelTasks;
using VRage.Library.Memory;
using VRage.Render11.Ansel;
using VRage.Render11.Culling;
using VRage.Render11.Culling.Occlusion;
using VRage.Render11.GeometryStage;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.GeometryStage2.StaticGroup;
using VRage.Render11.LightingStage.EnvironmentProbe;
using VRage.Render11.Particles;
using VRage.Render11.Render;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene.Components;
using VRage.Render11.Sprites;
using VRageRender;

namespace VRage.Render11.Common
{
	internal static class MyManagers
	{
		public static MyDeferredRenderContextManager DeferredRCs;

		public static MyBlendStateManager BlendStates;

		public static MyDepthStencilStateManager DepthStencilStates;

		public static MyRasterizerStateManager RasterizerStates;

		public static MySamplerStateManager SamplerStates;

		public static MyGeneratedTextureManager GeneratedTextures;

		public static MyTextureStreamingManager Textures;

		public static MyFileTextureManager FileTextures;

		public static MyFileArrayTextureManager FileArrayTextures;

		public static MyRwTextureManager RwTextures;

		public static MyCustomTextureManager CustomTextures;

		public static MyDepthStencilManager DepthStencils;

		public static MyArrayTextureManager ArrayTextures;

		public static MyBorrowedRwTextureManager RwTexturesPool;

		public static MyDynamicFileArrayTextureManager DynamicFileArrayTextures;

		public static MyRwTextureCatalog RwTexturesCatalog;

		public static MyGlobalResources GlobalResources;

		public static MyBufferManager Buffers;

		public static MyShadows Shadows;

		public static MyEnvironmentProbe EnvironmentProbe;

		public static MyGeometryTextureSystem GeometryTextureSystem;

		public static MyFoliageGeneratingPass FoliageGenerator;

		public static MyFoliageRenderingPass FoliageRenderer;

		public static MyShaderBundleManager ShaderBundles;

		public static MyIDGeneratorManager IDGenerator;

		public static MyModelFactory ModelFactory;

		public static MyInstanceManager Instances;

		public static MyStaticGroupManager StaticGroups;

		public static MyCullManager Cull;

		public static MyFlareOcclusionRenderer FlareOcclusionRenderer;

		public static MyActorOcclusionRenderer ActorOcclusionRenderer;

		public static MyGeometryRendererOld GeometryRendererOld;

		public static MyGeometryRenderer GeometryRenderer;

		public static MyPostponedUpdateManager PostponedUpdate;

		public static MyAnselRenderManager Ansel;

		private static readonly MyGeneralManager m_generalManager;

		public static MyTextureChangeManager TextureChangeManager;

		public static MyFoliageManager FoliageManager;

		public static MyRenderScheduler RenderScheduler;

		public static MySpritesManager SpritesManager;

		public static MyRenderParticlesManager ParticleEffectsManager;

		public static MyMemorySystem MemoryTracker { get; }

		public static MyMemorySystem TexturesMemoryTracker { get; }

		static MyManagers()
		{
			MemoryTracker = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Dx11Render");
			TexturesMemoryTracker = MemoryTracker.RegisterSubsystem("Textures");
			DeferredRCs = new MyDeferredRenderContextManager();
			BlendStates = new MyBlendStateManager();
			DepthStencilStates = new MyDepthStencilStateManager();
			RasterizerStates = new MyRasterizerStateManager();
			SamplerStates = new MySamplerStateManager();
			GeneratedTextures = new MyGeneratedTextureManager();
			Textures = new MyTextureStreamingManager();
			FileTextures = new MyFileTextureManager();
			FileArrayTextures = new MyFileArrayTextureManager();
			RwTextures = new MyRwTextureManager();
			CustomTextures = new MyCustomTextureManager();
			DepthStencils = new MyDepthStencilManager();
			ArrayTextures = new MyArrayTextureManager();
			RwTexturesPool = new MyBorrowedRwTextureManager();
			DynamicFileArrayTextures = new MyDynamicFileArrayTextureManager();
			RwTexturesCatalog = new MyRwTextureCatalog();
			GlobalResources = new MyGlobalResources();
			Buffers = new MyBufferManager();
			Shadows = new MyShadows();
			EnvironmentProbe = new MyEnvironmentProbe();
			GeometryTextureSystem = new MyGeometryTextureSystem();
			FoliageGenerator = new MyFoliageGeneratingPass();
			FoliageRenderer = new MyFoliageRenderingPass();
			ShaderBundles = new MyShaderBundleManager();
			IDGenerator = new MyIDGeneratorManager();
			ModelFactory = new MyModelFactory();
			Instances = new MyInstanceManager();
			StaticGroups = new MyStaticGroupManager();
			Cull = new MyCullManager(Shadows);
			FlareOcclusionRenderer = new MyFlareOcclusionRenderer();
			ActorOcclusionRenderer = new MyActorOcclusionRenderer();
			GeometryRendererOld = new MyGeometryRendererOld();
			GeometryRenderer = new MyGeometryRenderer();
			PostponedUpdate = new MyPostponedUpdateManager();
			Ansel = new MyAnselRenderManager();
			m_generalManager = new MyGeneralManager();
			TextureChangeManager = new MyTextureChangeManager();
			FoliageManager = new MyFoliageManager();
			RenderScheduler = new MyRenderScheduler();
			SpritesManager = new MySpritesManager();
			ParticleEffectsManager = new MyRenderParticlesManager();
			m_generalManager.RegisterManager(Buffers);
			m_generalManager.RegisterManager(DeferredRCs);
			m_generalManager.RegisterManager(BlendStates);
			m_generalManager.RegisterManager(DepthStencilStates);
			m_generalManager.RegisterManager(RasterizerStates);
			m_generalManager.RegisterManager(SamplerStates);
			m_generalManager.RegisterManager(GeneratedTextures);
			m_generalManager.RegisterManager(FileTextures);
			m_generalManager.RegisterManager(Textures);
			m_generalManager.RegisterManager(RwTextures);
			m_generalManager.RegisterManager(CustomTextures);
			m_generalManager.RegisterManager(DepthStencils);
			m_generalManager.RegisterManager(FileArrayTextures);
			m_generalManager.RegisterManager(DynamicFileArrayTextures);
			m_generalManager.RegisterManager(ArrayTextures);
			m_generalManager.RegisterManager(RwTexturesCatalog);
			m_generalManager.RegisterManager(RwTexturesPool);
			m_generalManager.RegisterManager(GlobalResources);
			m_generalManager.RegisterManager(Shadows);
			m_generalManager.RegisterManager(EnvironmentProbe);
			m_generalManager.RegisterManager(GeometryTextureSystem);
			m_generalManager.RegisterManager(ShaderBundles);
			m_generalManager.RegisterManager(IDGenerator);
			m_generalManager.RegisterManager(ModelFactory);
			m_generalManager.RegisterManager(Instances);
			m_generalManager.RegisterManager(StaticGroups);
			m_generalManager.RegisterManager(RenderScheduler);
			m_generalManager.RegisterManager(GeometryRenderer);
			m_generalManager.RegisterManager(PostponedUpdate);
			m_generalManager.RegisterManager(Ansel);
			m_generalManager.RegisterManager(TextureChangeManager);
			m_generalManager.RegisterManager(FoliageManager);
			m_generalManager.RegisterManager(GeometryRendererOld);
			m_generalManager.RegisterManager(Cull);
			m_generalManager.RegisterManager(FlareOcclusionRenderer);
			m_generalManager.RegisterManager(ActorOcclusionRenderer);
			m_generalManager.RegisterManager(FoliageGenerator);
			m_generalManager.RegisterManager(FoliageRenderer);
			m_generalManager.RegisterManager(SpritesManager);
		}

		public static void OnDeviceInit()
		{
			m_generalManager.OnDeviceInit();
		}

		public static void OnDeviceReset()
		{
			m_generalManager.OnDeviceReset();
		}

		public static void OnDeviceEnd()
		{
			m_generalManager.OnDeviceEnd();
		}

		public static void OnUnloadData()
		{
			m_generalManager.OnUnloadData();
		}

		public static void OnFrameEnd()
		{
			m_generalManager.OnFrameEnd();
		}

		public static void OnUpdate()
		{
			m_generalManager.OnUpdate();
		}
	}
}
