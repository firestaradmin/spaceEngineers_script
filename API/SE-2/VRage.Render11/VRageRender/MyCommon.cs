using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;
using VRage.Library.Utils;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage.Materials;
using VRage.Render11.LightingStage;
using VRage.Render11.LightingStage.EnvironmentProbe;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageMath;

namespace VRageRender
{
	internal static class MyCommon
	{
		internal struct ForwardConstants
		{
			public Matrix ProjectionMatrix;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct MyEnvironmentLayout
		{
			internal Matrix View;

			internal Matrix Projection;

			internal Matrix ProjectionForSkybox;

			internal Matrix ViewProjection;

			internal Matrix InvView;

			internal Matrix InvProjection;

			internal Matrix InvViewProjection;

			internal Matrix BackgroundOrientation;

			internal Vector4 WorldOffset;

			internal Vector3 EyeOffsetInWorld;

			internal readonly float __pad0;

			internal Vector3 CameraPositionDelta;

			internal readonly float __pad1;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct MyScreenLayout
		{
			internal Vector2I Offset;

			internal Vector2 Resolution;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct MyFoliageLayout
		{
			internal Vector4 ClippingScaling;

			internal Vector3 WindVector;

			private readonly float __pad;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct MyFogLayout
		{
			internal Vector3 Color;

			internal float Density;

			internal float Mult;

			internal float SkyFogIntensity;

			internal float AtmoFogIntensity;

			private readonly float __pad0;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct MyVoxelLayout
		{
			internal readonly Vector4 LodRange0;

			internal readonly Vector4 LodRange1;

			internal readonly Vector4 LodRange2;

			internal readonly Vector4 LodRange3;

			internal readonly Vector4 MassiveLodRange0;

			internal readonly Vector4 MassiveLodRange1;

			internal readonly Vector4 MassiveLodRange2;

			internal readonly Vector4 MassiveLodRange3;

			internal readonly Vector4 MassiveLodRange4;

			internal readonly Vector4 MassiveLodRange5;

			internal readonly Vector4 MassiveLodRange6;

			internal readonly Vector4 MassiveLodRange7;

			internal float DebugVoxelLod;

			private readonly Vector3 __pad;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct MyFrameConstantsLayout
		{
			internal MyEnvironmentLayout Environment;

			internal MyScreenLayout Screen;

			internal MyFoliageLayout Foliage;

			internal MyEnvironmentLightData EnvironmentLight;

			internal MyPostprocessSettings.Layout Postprocess;

			internal MyFogLayout Fog;

			internal MyVoxelLayout Voxel;

			internal MyTextureDebugMultipliers TextureDebugMultipliers;

			internal float FrameTime;

			internal float FrameTimeDelta;

			internal float RandomSeed;

			internal float DistanceFade;
		}

		internal const int FRAME_SLOT = 0;

		internal const int PROJECTION_SLOT = 1;

		internal const int OBJECT_SLOT = 2;

		internal const int MATERIAL_SLOT = 3;

		internal const int FOLIAGE_SLOT = 4;

		internal const int ALPHAMASK_VIEWS_SLOT = 5;

		internal const int VOXELS_MATERIALS_LUT_SLOT = 6;

		internal const int FORWARD_SLOT = 7;

		internal const int BIG_TABLE_INDICES = 10;

		internal const int BIG_TABLE_VERTEX_POSITION = 11;

		internal const int BIG_TABLE_VERTEX = 12;

		internal const int INSTANCE_INDIRECTION = 13;

		internal const int INSTANCE_DATA = 14;

		internal const int VOXEL_TEXTURE_ARRAYS = 10;

		internal const int SKYBOX_SLOT = 10;

		internal const int SKYBOX_IBL_CLOSE_SLOT = 11;

		internal const int SKYBOX_IBL_FAR_SLOT = 17;

		internal const int AO_SLOT = 12;

		internal const int POINTLIGHT_SLOT = 13;

		internal const int TILE_LIGHT_INDICES_SLOT = 14;

		internal const int CASCADES_SM_SLOT = 15;

		internal const int AMBIENT_BRDF_LUT_SLOT = 16;

		internal const int SHADOW_SLOT = 19;

		internal const int MATERIAL_BUFFER_SLOT = 20;

		internal const int DITHER_8X8_SLOT = 28;

		internal const int RANDOM_SLOT = 29;

		internal const int LBUFFER_SLOT = 18;

		internal const int DEPTHBUFFER_SLOT = 21;

		internal const int SHADOW_SAMPLER_SLOT = 15;

		private const float MAX_FRAMETIME = 66f;

		private const int DEFAULT_SEED = 1248819489;

		internal static readonly int SCREEN_LAYOUT_SIZE;

		private static readonly MyRandom m_random;

		private static float m_constFrameTimeDelta;

		private static float m_lastFrameTimeDelta;

		private static float m_frameTime;

		private static float m_lastFrameTimer;

		private static readonly Stopwatch m_timer;

		private static Vector3D m_lastCameraPosition;

		private static bool m_paused;

		internal static MyFrameConstantsLayout FrameConstantsData;

		private static readonly string[] s_viewVectorData;

		private static readonly ConcurrentBag<IConstantBuffer> m_forwardConstants;

		internal static IConstantBuffer FrameConstantsStereoLeftEye { get; set; }

		internal static IConstantBuffer FrameConstantsStereoRightEye { get; set; }

		internal static IConstantBuffer FrameConstants { get; set; }

		internal static IConstantBuffer ProjectionConstants { get; set; }

		internal static IConstantBuffer ObjectConstants { get; set; }

		internal static IConstantBuffer MaterialFoliageTableConstants { get; set; }

		internal static IConstantBuffer HighlightConstants { get; set; }

		internal static IConstantBuffer AlphamaskViewsConstants { get; set; }

		internal static MyVoxelMaterialsConstantBuffer VoxelMaterialsConstants { get; set; }

		public static bool IsPaused
		{
			get
			{
				return m_paused;
			}
			set
			{
				m_paused = value;
			}
		}

<<<<<<< HEAD
		private static float TimerMs => (float)((double)m_timer.ElapsedTicks / (double)Stopwatch.Frequency * 1000.0);
=======
		private static float TimerMs => (float)((double)m_timer.get_ElapsedTicks() / (double)Stopwatch.Frequency * 1000.0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		internal static MyTimeSpan FrameTime => MyTimeSpan.FromSeconds(m_frameTime);

		internal static MyTimeSpan UpdateTime { get; set; }

		internal static MyRandom Random => m_random;

		public static float LODCoefficient { get; private set; }

		public static MyNewLoddingSettings LoddingSettings { get; set; }

		public static long FrameCounter => MyScene.FrameCounter;

		unsafe static MyCommon()
		{
<<<<<<< HEAD
=======
			//IL_0745: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			SCREEN_LAYOUT_SIZE = sizeof(MyScreenLayout);
			m_random = new MyRandom();
			m_lastFrameTimeDelta = 0f;
			m_frameTime = 0f;
			m_lastFrameTimer = 0f;
			m_paused = false;
			s_viewVectorData = new string[181]
			{
				"-0.707107,-0.707107,0.000000,0.000000,-0.000000,1.000000,0.707107,-0.707107,-0.000000", "-0.613941,-0.789352,0.000000,0.000000,-0.000000,1.000000,0.789352,-0.613941,-0.000000", "-0.707107,-0.707107,0.000000,0.105993,-0.105993,0.850104,0.601114,-0.601114,-0.149896", "-0.789352,-0.613940,0.000000,0.000000,-0.000000,1.000000,0.613940,-0.789352,-0.000000", "-0.485643,-0.874157,0.000000,0.000000,-0.000000,1.000000,0.874157,-0.485643,-0.000000", "-0.600000,-0.800000,0.000000,0.119917,-0.089938,0.850104,0.680083,-0.510062,-0.149896", "-0.707107,-0.707107,0.000000,0.188689,-0.188689,0.733154,0.518418,-0.518418,-0.266846", "-0.800000,-0.600000,0.000000,0.089938,-0.119917,0.850104,0.510062,-0.680083,-0.149896", "-0.874157,-0.485643,0.000000,0.000000,-0.000000,1.000000,0.485643,-0.874157,-0.000000", "-0.316228,-0.948683,0.000000,0.000000,-0.000000,1.000000,0.948683,-0.316228,-0.000000",
				"-0.447214,-0.894427,0.000000,0.134071,-0.067036,0.850104,0.760356,-0.380178,-0.149896", "-0.581238,-0.813734,0.000000,0.217142,-0.155101,0.733154,0.596592,-0.426137,-0.266846", "-0.707107,-0.707107,0.000000,0.258819,-0.258819,0.633975,0.448288,-0.448288,-0.366025", "-0.813734,-0.581238,0.000000,0.155101,-0.217142,0.733154,0.426137,-0.596592,-0.266846", "-0.894427,-0.447214,0.000000,0.067036,-0.134071,0.850104,0.380178,-0.760356,-0.149896", "-0.948683,-0.316228,0.000000,0.000000,-0.000000,1.000000,0.316228,-0.948683,-0.000000", "-0.110431,-0.993884,0.000000,0.000000,-0.000000,1.000000,0.993884,-0.110431,-0.000000", "-0.242536,-0.970143,0.000000,0.145421,-0.036355,0.850104,0.824722,-0.206180,-0.149896", "-0.393919,-0.919145,0.000000,0.245270,-0.105116,0.733154,0.673875,-0.288803,-0.266846", "-0.554700,-0.832050,0.000000,0.304552,-0.203034,0.633975,0.527499,-0.351666,-0.366025",
				"-0.707107,-0.707107,0.000000,0.322621,-0.322621,0.543744,0.384485,-0.384485,-0.456256", "-0.832050,-0.554700,0.000000,0.203034,-0.304552,0.633975,0.351666,-0.527499,-0.366025", "-0.919145,-0.393919,0.000000,0.105116,-0.245270,0.733154,0.288803,-0.673875,-0.266846", "-0.970142,-0.242536,0.000000,0.036355,-0.145421,0.850104,0.206181,-0.824722,-0.149896", "-0.993884,-0.110432,0.000000,0.000000,-0.000000,1.000000,0.110432,-0.993884,-0.000000", "0.110431,-0.993884,0.000000,0.000000,0.000000,1.000000,0.993884,0.110431,-0.000000", "-0.000000,-1.000000,0.000000,0.149896,-0.000000,0.850104,0.850104,-0.000000,-0.149896", "-0.141422,-0.989949,0.000000,0.264164,-0.037738,0.733154,0.725785,-0.103684,-0.266846", "-0.316228,-0.948683,0.000000,0.347242,-0.115747,0.633975,0.601441,-0.200480,-0.366025", "-0.514496,-0.857493,0.000000,0.391236,-0.234742,0.543744,0.466257,-0.279754,-0.456256",
				"-0.707107,-0.707107,0.000000,0.384485,-0.384485,0.456256,0.322621,-0.322621,-0.543744", "-0.857493,-0.514496,0.000000,0.234742,-0.391236,0.543744,0.279754,-0.466257,-0.456256", "-0.948683,-0.316228,0.000000,0.115747,-0.347242,0.633975,0.200480,-0.601441,-0.366025", "-0.989950,-0.141421,0.000000,0.037738,-0.264164,0.733154,0.103684,-0.725785,-0.266846", "-1.000000,0.000000,0.000000,-0.000000,-0.149896,0.850104,-0.000000,-0.850104,-0.149896", "-0.993884,0.110431,0.000000,-0.000000,-0.000000,1.000000,-0.110431,-0.993884,-0.000000", "0.316228,-0.948683,0.000000,0.000000,0.000000,1.000000,0.948683,0.316228,-0.000000", "0.242536,-0.970143,0.000000,0.145421,0.036355,0.850104,0.824722,0.206180,-0.149896", "0.141422,-0.989949,0.000000,0.264164,0.037738,0.733154,0.725785,0.103684,-0.266846", "-0.000000,-1.000000,0.000000,0.366025,-0.000000,0.633975,0.633975,-0.000000,-0.366025",
				"-0.196116,-0.980581,0.000000,0.447395,-0.089479,0.543744,0.533185,-0.106637,-0.456256", "-0.447214,-0.894427,0.000000,0.486340,-0.243170,0.456256,0.408087,-0.204044,-0.543744", "-0.707107,-0.707107,0.000000,0.448288,-0.448288,0.366025,0.258819,-0.258819,-0.633975", "-0.894427,-0.447214,0.000000,0.243170,-0.486340,0.456256,0.204044,-0.408087,-0.543744", "-0.980581,-0.196116,0.000000,0.089479,-0.447395,0.543744,0.106637,-0.533185,-0.456256", "-1.000000,0.000000,0.000000,-0.000000,-0.366025,0.633975,-0.000000,-0.633975,-0.366025", "-0.989950,0.141421,0.000000,-0.037738,-0.264164,0.733154,-0.103684,-0.725785,-0.266846", "-0.970143,0.242536,0.000000,-0.036355,-0.145421,0.850104,-0.206180,-0.824722,-0.149896", "-0.948683,0.316228,0.000000,-0.000000,-0.000000,1.000000,-0.316228,-0.948683,-0.000000", "0.485643,-0.874157,0.000000,0.000000,0.000000,1.000000,0.874157,0.485643,-0.000000",
				"0.447214,-0.894427,0.000000,0.134071,0.067036,0.850104,0.760356,0.380178,-0.149896", "0.393919,-0.919145,0.000000,0.245270,0.105116,0.733154,0.673875,0.288803,-0.266846", "0.316228,-0.948683,0.000000,0.347242,0.115747,0.633975,0.601441,0.200480,-0.366025", "0.196116,-0.980581,0.000000,0.447395,0.089479,0.543744,0.533185,0.106637,-0.456256", "-0.000000,-1.000000,0.000000,0.543744,-0.000000,0.456256,0.456256,-0.000000,-0.543744", "-0.316228,-0.948683,0.000000,0.601441,-0.200480,0.366025,0.347242,-0.115747,-0.633975", "-0.707107,-0.707107,0.000000,0.518418,-0.518418,0.266846,0.188689,-0.188689,-0.733154", "-0.948683,-0.316228,0.000000,0.200480,-0.601441,0.366025,0.115747,-0.347242,-0.633975", "-1.000000,0.000000,0.000000,-0.000000,-0.543744,0.456256,-0.000000,-0.456256,-0.543744", "-0.980581,0.196116,0.000000,-0.089479,-0.447395,0.543744,-0.106637,-0.533185,-0.456256",
				"-0.948683,0.316228,0.000000,-0.115747,-0.347242,0.633975,-0.200480,-0.601441,-0.366025", "-0.919145,0.393919,0.000000,-0.105116,-0.245270,0.733154,-0.288803,-0.673875,-0.266846", "-0.894427,0.447214,0.000000,-0.067036,-0.134071,0.850104,-0.380178,-0.760356,-0.149896", "-0.874157,0.485643,0.000000,-0.000000,-0.000000,1.000000,-0.485643,-0.874157,-0.000000", "0.613941,-0.789352,0.000000,0.000000,0.000000,1.000000,0.789352,0.613941,-0.000000", "0.600000,-0.800000,0.000000,0.119917,0.089938,0.850104,0.680083,0.510062,-0.149896", "0.581238,-0.813734,0.000000,0.217142,0.155101,0.733154,0.596592,0.426137,-0.266846", "0.554700,-0.832050,0.000000,0.304552,0.203034,0.633975,0.527499,0.351666,-0.366025", "0.514496,-0.857493,0.000000,0.391236,0.234742,0.543744,0.466257,0.279754,-0.456256", "0.447214,-0.894427,0.000000,0.486340,0.243170,0.456256,0.408087,0.204044,-0.543744",
				"0.316228,-0.948683,0.000000,0.601441,0.200480,0.366025,0.347242,0.115747,-0.633975", "-0.000000,-1.000000,0.000000,0.733154,-0.000000,0.266846,0.266846,-0.000000,-0.733154", "-0.707107,-0.707107,0.000000,0.601114,-0.601114,0.149896,0.105993,-0.105993,-0.850104", "-1.000000,0.000000,0.000000,-0.000000,-0.733154,0.266846,-0.000000,-0.266846,-0.733154", "-0.948683,0.316228,0.000000,-0.200480,-0.601441,0.366025,-0.115747,-0.347242,-0.633975", "-0.894427,0.447214,0.000000,-0.243170,-0.486340,0.456256,-0.204044,-0.408087,-0.543744", "-0.857493,0.514496,0.000000,-0.234742,-0.391236,0.543744,-0.279754,-0.466257,-0.456256", "-0.832050,0.554700,0.000000,-0.203034,-0.304552,0.633975,-0.351666,-0.527499,-0.366025", "-0.813733,0.581238,0.000000,-0.155101,-0.217142,0.733154,-0.426137,-0.596592,-0.266846", "-0.800000,0.600000,0.000000,-0.089938,-0.119917,0.850104,-0.510062,-0.680083,-0.149896",
				"-0.789352,0.613941,0.000000,-0.000000,-0.000000,1.000000,-0.613941,-0.789352,-0.000000", "0.707107,-0.707107,0.000000,0.000000,0.000000,1.000000,0.707107,0.707107,-0.000000", "0.707107,-0.707107,0.000000,0.105993,0.105993,0.850104,0.601114,0.601114,-0.149896", "0.707107,-0.707107,0.000000,0.188689,0.188689,0.733154,0.518418,0.518418,-0.266846", "0.707107,-0.707107,0.000000,0.258819,0.258819,0.633975,0.448288,0.448288,-0.366025", "0.707107,-0.707107,0.000000,0.322621,0.322621,0.543744,0.384485,0.384485,-0.456256", "0.707107,-0.707107,0.000000,0.384485,0.384485,0.456256,0.322621,0.322621,-0.543744", "0.707107,-0.707107,0.000000,0.448288,0.448288,0.366025,0.258819,0.258819,-0.633975", "0.707107,-0.707107,0.000000,0.518418,0.518418,0.266846,0.188689,0.188689,-0.733154", "0.707107,-0.707107,0.000000,0.601114,0.601114,0.149896,0.105993,0.105993,-0.850104",
				"0.000000,1.000000,0.000000,-1.000000,0.000000,0.000000,0.000000,0.000000,-1.000000", "-0.707107,0.707107,0.000000,-0.601114,-0.601114,0.149896,-0.105993,-0.105993,-0.850104", "-0.707107,0.707107,0.000000,-0.518418,-0.518418,0.266846,-0.188689,-0.188689,-0.733154", "-0.707107,0.707107,0.000000,-0.448288,-0.448288,0.366025,-0.258819,-0.258819,-0.633975", "-0.707107,0.707107,0.000000,-0.384485,-0.384485,0.456256,-0.322621,-0.322621,-0.543744", "-0.707107,0.707107,0.000000,-0.322621,-0.322621,0.543744,-0.384485,-0.384485,-0.456256", "-0.707107,0.707107,0.000000,-0.258819,-0.258819,0.633975,-0.448288,-0.448288,-0.366025", "-0.707107,0.707107,0.000000,-0.188689,-0.188689,0.733154,-0.518418,-0.518418,-0.266846", "-0.707107,0.707107,0.000000,-0.105993,-0.105993,0.850104,-0.601114,-0.601114,-0.149896", "-0.707107,0.707107,0.000000,-0.000000,-0.000000,1.000000,-0.707107,-0.707107,-0.000000",
				"0.789352,-0.613941,0.000000,0.000000,0.000000,1.000000,0.613941,0.789352,-0.000000", "0.800000,-0.600000,0.000000,0.089938,0.119917,0.850104,0.510062,0.680083,-0.149896", "0.813734,-0.581238,0.000000,0.155101,0.217142,0.733154,0.426137,0.596592,-0.266846", "0.832050,-0.554700,0.000000,0.203034,0.304551,0.633975,0.351666,0.527499,-0.366025", "0.857493,-0.514496,0.000000,0.234742,0.391236,0.543744,0.279754,0.466257,-0.456256", "0.894427,-0.447214,0.000000,0.243170,0.486340,0.456256,0.204044,0.408087,-0.543744", "0.948683,-0.316228,0.000000,0.200480,0.601441,0.366025,0.115747,0.347242,-0.633975", "1.000000,0.000000,0.000000,0.000000,0.733154,0.266846,0.000000,0.266846,-0.733154", "0.707107,0.707107,0.000000,-0.601114,0.601114,0.149896,-0.105993,0.105993,-0.850104", "0.000000,1.000000,0.000000,-0.733154,0.000000,0.266846,-0.266846,0.000000,-0.733154",
				"-0.316228,0.948683,0.000000,-0.601441,-0.200480,0.366025,-0.347242,-0.115747,-0.633975", "-0.447214,0.894427,0.000000,-0.486340,-0.243170,0.456256,-0.408087,-0.204044,-0.543744", "-0.514496,0.857493,0.000000,-0.391236,-0.234742,0.543744,-0.466257,-0.279754,-0.456256", "-0.554700,0.832050,0.000000,-0.304552,-0.203034,0.633975,-0.527499,-0.351666,-0.366025", "-0.581238,0.813734,0.000000,-0.217142,-0.155101,0.733154,-0.596592,-0.426137,-0.266846", "-0.600000,0.800000,0.000000,-0.119917,-0.089938,0.850104,-0.680083,-0.510062,-0.149896", "-0.613941,0.789352,0.000000,-0.000000,-0.000000,1.000000,-0.789352,-0.613941,-0.000000", "0.874157,-0.485643,0.000000,0.000000,0.000000,1.000000,0.485643,0.874157,-0.000000", "0.894427,-0.447214,0.000000,0.067036,0.134071,0.850104,0.380178,0.760356,-0.149896", "0.919145,-0.393919,0.000000,0.105116,0.245270,0.733154,0.288803,0.673875,-0.266846",
				"0.948683,-0.316228,0.000000,0.115747,0.347242,0.633975,0.200480,0.601441,-0.366025", "0.980581,-0.196116,0.000000,0.089479,0.447395,0.543744,0.106637,0.533185,-0.456256", "1.000000,0.000000,0.000000,0.000000,0.543744,0.456256,0.000000,0.456256,-0.543744", "0.948683,0.316228,0.000000,-0.200480,0.601441,0.366025,-0.115747,0.347242,-0.633975", "0.707107,0.707107,0.000000,-0.518418,0.518418,0.266846,-0.188689,0.188689,-0.733154", "0.316228,0.948683,0.000000,-0.601441,0.200480,0.366025,-0.347242,0.115747,-0.633975", "0.000000,1.000000,0.000000,-0.543744,0.000000,0.456256,-0.456256,0.000000,-0.543744", "-0.196116,0.980581,0.000000,-0.447395,-0.089479,0.543744,-0.533185,-0.106637,-0.456256", "-0.316228,0.948683,0.000000,-0.347242,-0.115747,0.633975,-0.601441,-0.200480,-0.366025", "-0.393919,0.919145,0.000000,-0.245270,-0.105116,0.733154,-0.673875,-0.288803,-0.266846",
				"-0.447214,0.894427,0.000000,-0.134071,-0.067036,0.850104,-0.760356,-0.380178,-0.149896", "-0.485643,0.874157,0.000000,-0.000000,-0.000000,1.000000,-0.874157,-0.485643,-0.000000", "0.948683,-0.316228,0.000000,0.000000,0.000000,1.000000,0.316228,0.948683,-0.000000", "0.970142,-0.242536,0.000000,0.036355,0.145421,0.850104,0.206180,0.824722,-0.149896", "0.989950,-0.141421,0.000000,0.037738,0.264164,0.733154,0.103684,0.725785,-0.266846", "1.000000,0.000000,0.000000,0.000000,0.366025,0.633975,0.000000,0.633975,-0.366025", "0.980581,0.196116,0.000000,-0.089479,0.447395,0.543744,-0.106637,0.533185,-0.456256", "0.894427,0.447214,0.000000,-0.243170,0.486340,0.456256,-0.204044,0.408087,-0.543744", "0.707107,0.707107,0.000000,-0.448288,0.448288,0.366025,-0.258819,0.258819,-0.633975", "0.447214,0.894427,0.000000,-0.486340,0.243170,0.456256,-0.408087,0.204044,-0.543744",
				"0.196116,0.980581,0.000000,-0.447395,0.089479,0.543744,-0.533185,0.106637,-0.456256", "0.000000,1.000000,0.000000,-0.366025,0.000000,0.633975,-0.633975,0.000000,-0.366025", "-0.141421,0.989949,0.000000,-0.264164,-0.037738,0.733154,-0.725785,-0.103684,-0.266846", "-0.242536,0.970143,0.000000,-0.145421,-0.036355,0.850104,-0.824722,-0.206180,-0.149896", "-0.316228,0.948683,0.000000,-0.000000,-0.000000,1.000000,-0.948683,-0.316228,-0.000000", "0.993884,-0.110432,0.000000,0.000000,0.000000,1.000000,0.110432,0.993884,-0.000000", "1.000000,0.000000,0.000000,0.000000,0.149896,0.850104,0.000000,0.850104,-0.149896", "0.989949,0.141421,0.000000,-0.037738,0.264164,0.733154,-0.103684,0.725785,-0.266846", "0.948683,0.316228,0.000000,-0.115747,0.347242,0.633975,-0.200480,0.601441,-0.366025", "0.857493,0.514496,0.000000,-0.234742,0.391236,0.543744,-0.279754,0.466257,-0.456256",
				"0.707107,0.707107,0.000000,-0.384485,0.384485,0.456256,-0.322621,0.322621,-0.543744", "0.514496,0.857493,0.000000,-0.391236,0.234742,0.543744,-0.466257,0.279754,-0.456256", "0.316228,0.948683,0.000000,-0.347242,0.115747,0.633975,-0.601441,0.200480,-0.366025", "0.141421,0.989949,0.000000,-0.264164,0.037738,0.733154,-0.725785,0.103684,-0.266846", "0.000000,1.000000,0.000000,-0.149896,0.000000,0.850104,-0.850104,0.000000,-0.149896", "-0.110432,0.993884,0.000000,-0.000000,-0.000000,1.000000,-0.993884,-0.110432,-0.000000", "0.993884,0.110431,0.000000,-0.000000,0.000000,1.000000,-0.110431,0.993884,-0.000000", "0.970143,0.242536,0.000000,-0.036355,0.145421,0.850104,-0.206180,0.824722,-0.149896", "0.919145,0.393919,0.000000,-0.105116,0.245270,0.733154,-0.288803,0.673875,-0.266846", "0.832050,0.554700,0.000000,-0.203034,0.304552,0.633975,-0.351666,0.527499,-0.366025",
				"0.707107,0.707107,0.000000,-0.322621,0.322621,0.543744,-0.384485,0.384485,-0.456256", "0.554700,0.832050,0.000000,-0.304552,0.203034,0.633975,-0.527499,0.351666,-0.366025", "0.393919,0.919145,0.000000,-0.245270,0.105116,0.733154,-0.673875,0.288803,-0.266846", "0.242536,0.970143,0.000000,-0.145421,0.036355,0.850104,-0.824722,0.206180,-0.149896", "0.110432,0.993884,0.000000,-0.000000,0.000000,1.000000,-0.993884,0.110432,-0.000000", "0.948683,0.316228,0.000000,-0.000000,0.000000,1.000000,-0.316228,0.948683,-0.000000", "0.894427,0.447214,0.000000,-0.067036,0.134071,0.850104,-0.380178,0.760356,-0.149896", "0.813733,0.581238,0.000000,-0.155101,0.217142,0.733154,-0.426137,0.596592,-0.266846", "0.707107,0.707107,0.000000,-0.258819,0.258819,0.633975,-0.448288,0.448288,-0.366025", "0.581238,0.813733,0.000000,-0.217142,0.155101,0.733154,-0.596592,0.426137,-0.266846",
				"0.447214,0.894427,0.000000,-0.134071,0.067036,0.850104,-0.760356,0.380178,-0.149896", "0.316228,0.948683,0.000000,-0.000000,0.000000,1.000000,-0.948683,0.316228,-0.000000", "0.874157,0.485643,0.000000,-0.000000,0.000000,1.000000,-0.485643,0.874157,-0.000000", "0.800000,0.600000,0.000000,-0.089938,0.119917,0.850104,-0.510062,0.680083,-0.149896", "0.707107,0.707107,0.000000,-0.188689,0.188689,0.733154,-0.518418,0.518418,-0.266846", "0.600000,0.800000,0.000000,-0.119917,0.089938,0.850104,-0.680083,0.510062,-0.149896", "0.485643,0.874157,0.000000,-0.000000,0.000000,1.000000,-0.874157,0.485643,-0.000000", "0.789352,0.613941,0.000000,-0.000000,0.000000,1.000000,-0.613941,0.789352,-0.000000", "0.707107,0.707107,0.000000,-0.105993,0.105993,0.850104,-0.601114,0.601114,-0.149896", "0.613941,0.789352,0.000000,-0.000000,0.000000,1.000000,-0.789352,0.613941,-0.000000",
				"0.707107,0.707107,0.000000,-0.000000,0.000000,1.000000,-0.707107,0.707107,-0.000000"
			};
			m_forwardConstants = new ConcurrentBag<IConstantBuffer>();
			m_timer = new Stopwatch();
			m_timer.Start();
		}

		internal unsafe static void Init()
		{
			FrameConstantsStereoLeftEye = MyManagers.Buffers.CreateConstantBuffer("FrameConstantsStereoLeftEye", sizeof(MyFrameConstantsLayout), null, ResourceUsage.Dynamic, isGlobal: true);
			FrameConstantsStereoRightEye = MyManagers.Buffers.CreateConstantBuffer("FrameConstantsStereoRightEye", sizeof(MyFrameConstantsLayout), null, ResourceUsage.Dynamic, isGlobal: true);
			FrameConstants = MyManagers.Buffers.CreateConstantBuffer("FrameConstants", sizeof(MyFrameConstantsLayout), null, ResourceUsage.Dynamic, isGlobal: true);
			ProjectionConstants = MyManagers.Buffers.CreateConstantBuffer("ProjectionConstants", sizeof(Matrix), null, ResourceUsage.Dynamic, isGlobal: true);
			ObjectConstants = MyManagers.Buffers.CreateConstantBuffer("ObjectConstants", sizeof(Matrix), null, ResourceUsage.Dynamic, isGlobal: true);
			MaterialFoliageTableConstants = MyManagers.Buffers.CreateConstantBuffer("MaterialFoliageTableConstants", sizeof(Vector4) * 256, null, ResourceUsage.Dynamic, isGlobal: true);
			HighlightConstants = MyManagers.Buffers.CreateConstantBuffer("HighlightConstants", sizeof(HighlightConstantsLayout), null, ResourceUsage.Dynamic, isGlobal: true);
			AlphamaskViewsConstants = MyManagers.Buffers.CreateConstantBuffer("AlphamaskViewsConstants", sizeof(Matrix) * 181, null, ResourceUsage.Dynamic, isGlobal: true);
			VoxelMaterialsConstants = new MyVoxelMaterialsConstantBuffer();
			LoddingSettings = new MyNewLoddingSettings();
			UpdateAlphamaskViewsConstants();
		}

		internal static ISrvBindable GetAmbientBrdfLut()
		{
			return MyManagers.Textures.GetTempTexture("Textures/Miscellaneous/ambient_brdf.dds", new MyTextureStreamingManager.QueryArgs
			{
				WaitUntilLoaded = true,
				TextureType = MyFileTextureEnum.CUSTOM
			}, 1000);
		}

		public static void UnloadData()
		{
<<<<<<< HEAD
			IConstantBuffer result;
			while (m_forwardConstants.TryTake(out result))
			{
				MyManagers.Buffers.Dispose(result);
=======
			IConstantBuffer constantBuffer = default(IConstantBuffer);
			while (m_forwardConstants.TryTake(ref constantBuffer))
			{
				MyManagers.Buffers.Dispose(constantBuffer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		internal static void MoveToNextFrame()
		{
			MyScene.FrameCounter++;
			float num = (float)(Math.Tan(MyRender11.Environment.Matrices.FovH / 2f) / Math.Tan(MathHelper.ToRadians(70f) / 2f));
			float num2 = 1080f / (float)MyRender11.ViewportResolution.Y;
			LODCoefficient = num * num2;
		}

		public static float GetLastFrameDelta()
		{
			return m_lastFrameTimeDelta;
		}

		public static void SetConstFrameTimeDelta(float timestepInSeconds)
		{
			if (timestepInSeconds == 0f && m_constFrameTimeDelta > 0f)
			{
				m_lastFrameTimer = TimerMs - m_constFrameTimeDelta;
			}
			m_constFrameTimeDelta = timestepInSeconds;
		}

		public static void SetRandomSeed(int? value)
		{
			int num = value ?? 1248819489;
			m_random.SetSeed(num);
			MyManagers.GeneratedTextures.InitializeRandomTexture(num);
			MyHBAO.InitializeConstantBuffer(MyImmediateRC.RC, num);
		}

		private static void UpdateFrameConstantsInternal(MyEnvironmentMatrices envMatrices, ref MyFrameConstantsLayout constants, MyStereoRegion typeofFC)
		{
			constants.Environment.View = Matrix.Transpose(envMatrices.ViewAt0);
			constants.Environment.Projection = Matrix.Transpose(envMatrices.Projection);
			constants.Environment.ProjectionForSkybox = Matrix.Transpose(envMatrices.ProjectionForSkybox);
			constants.Environment.ViewProjection = Matrix.Transpose(envMatrices.ViewProjectionAt0);
			constants.Environment.InvView = Matrix.Transpose(envMatrices.InvViewAt0);
			constants.Environment.InvProjection = Matrix.Transpose(envMatrices.InvProjection);
			constants.Environment.InvViewProjection = Matrix.Transpose(envMatrices.InvViewProjectionAt0);
			constants.Environment.WorldOffset = new Vector4(envMatrices.CameraPosition, 0f);
			constants.Screen.Resolution = MyRender11.ResolutionF;
			if (typeofFC != 0)
			{
				constants.Screen.Resolution.X /= 2f;
				Vector3 eyeOffsetInWorld = Vector3.Transform(new Vector3(envMatrices.ViewAt0.M41, envMatrices.ViewAt0.M42, envMatrices.ViewAt0.M43), Matrix.Transpose(MyRender11.Environment.Matrices.ViewAt0));
				constants.Environment.EyeOffsetInWorld = eyeOffsetInWorld;
			}
			constants.Screen.Offset = new Vector2I(0, 0);
			if (typeofFC == MyStereoRegion.RIGHT)
			{
				constants.Screen.Offset.X = MyRender11.ResolutionI.X / 2;
			}
		}

		internal static void UpdateFrameConstants()
		{
			FrameConstantsData = default(MyFrameConstantsLayout);
			UpdateFrameConstantsInternal(MyRender11.Environment.Matrices, ref FrameConstantsData, MyStereoRegion.FULLSCREEN);
			FrameConstantsData.Environment.CameraPositionDelta = MyRender11.Environment.Matrices.CameraPosition - m_lastCameraPosition;
			m_lastCameraPosition = MyRender11.Environment.Matrices.CameraPosition;
			FrameConstantsData.Environment.BackgroundOrientation = Matrix.CreateFromQuaternion(MyRender11.Environment.Data.SkyboxOrientation);
			FrameConstantsData.Foliage.ClippingScaling = new Vector4(MyRender11.Settings.User.GrassDrawDistance, MyRender11.Settings.GrassGeometryScalingNearDistance * 0.4f, MyRender11.Settings.GrassGeometryScalingFarDistance * 0.4f, MyRender11.Settings.GrassGeometryDistanceScalingFactor);
			FrameConstantsData.Foliage.WindVector = new Vector3((float)Math.Cos((double)MyRender11.Settings.WindAzimuth * Math.PI / 180.0), 0f, (float)Math.Sin((double)MyRender11.Settings.WindAzimuth * Math.PI / 180.0)) * MyRender11.Settings.WindStrength;
			FrameConstantsData.DistanceFade = MyRender11.Settings.User.DistanceFade;
			FrameConstantsData.Postprocess = MyRender11.Postprocess.GetProcessedData();
			FrameConstantsData.EnvironmentLight = MyRender11.Environment.Data.EnvironmentLight;
			if (!MyRender11.DebugOverrides.Sun)
			{
				FrameConstantsData.EnvironmentLight.SunColorRaw = new Vector3(0f, 0f, 0f);
			}
			FrameConstantsData.EnvironmentLight.AmbientForwardPass += MyEnvironmentProbe.LastAmbient;
			FrameConstantsData.EnvironmentLight.SkipIBLevels = 0;
			MyRender11.Environment.Data.EnvironmentLight.SkipIBLevels = FrameConstantsData.EnvironmentLight.SkipIBLevels;
			FrameConstantsData.EnvironmentLight.TilesNum = (uint)MyLightsRendering.GetTilesNum();
			FrameConstantsData.EnvironmentLight.TilesX = (uint)MyLightsRendering.GetTilesX();
			FrameConstantsData.Fog.Density = MyRender11.Environment.Fog.FogDensity;
			FrameConstantsData.Fog.Mult = MyRender11.Environment.Fog.FogMultiplier;
			FrameConstantsData.Fog.Color = MyRender11.Environment.Fog.FogColor.ToVector3().ToLinearRGB();
			FrameConstantsData.Fog.SkyFogIntensity = MyRender11.Environment.Fog.FogSkybox;
			FrameConstantsData.Fog.AtmoFogIntensity = MyRender11.Environment.Fog.FogAtmo;
			FrameConstantsData.Voxel.DebugVoxelLod = (MyRender11.Settings.DebugTextureLodColor ? 2f : 0f);
			FrameConstantsData.TextureDebugMultipliers = MyRender11.Environment.Data.TextureMultipliers;
			FrameConstantsData.FrameTimeDelta = m_lastFrameTimeDelta;
			FrameConstantsData.FrameTime = m_frameTime;
			FrameConstantsData.RandomSeed = m_random.NextFloat();
			MyMapping myMapping = MyMapping.MapDiscard(FrameConstants);
			myMapping.WriteAndPosition(ref FrameConstantsData);
			myMapping.Unmap();
			if (MyStereoRender.Enable)
			{
				UpdateFrameConstantsInternal(MyStereoRender.EnvMatricesLeftEye, ref FrameConstantsData, MyStereoRegion.LEFT);
				myMapping = MyMapping.MapDiscard(FrameConstantsStereoLeftEye);
				myMapping.WriteAndPosition(ref FrameConstantsData);
				myMapping.Unmap();
				UpdateFrameConstantsInternal(MyStereoRender.EnvMatricesRightEye, ref FrameConstantsData, MyStereoRegion.RIGHT);
				myMapping = MyMapping.MapDiscard(FrameConstantsStereoRightEye);
				myMapping.WriteAndPosition(ref FrameConstantsData);
				myMapping.Unmap();
			}
		}

		internal static void UpdateTimers()
		{
			if (!m_paused)
			{
				if (m_constFrameTimeDelta > 0f)
				{
					m_frameTime += m_constFrameTimeDelta;
					m_lastFrameTimeDelta = m_constFrameTimeDelta;
				}
				else if (m_constFrameTimeDelta == 0f)
				{
					float timerMs = TimerMs;
					float num = Math.Min(timerMs - m_lastFrameTimer, 66f) / 1000f;
					m_frameTime += num;
					m_lastFrameTimeDelta = num;
					m_lastFrameTimer = timerMs;
				}
			}
			else
			{
				m_lastFrameTimeDelta = 0f;
				m_lastFrameTimer = TimerMs;
			}
		}

		internal unsafe static void UpdateAlphamaskViewsConstants()
		{
			Matrix* ptr = stackalloc Matrix[s_viewVectorData.Length];
			for (int i = 0; i < s_viewVectorData.Length; i++)
			{
				Matrix matrix = Matrix.Identity;
				string[] array = s_viewVectorData[i].Split(new char[1] { ',' });
				matrix.M11 = Convert.ToSingle(array[0], CultureInfo.InvariantCulture);
				matrix.M12 = Convert.ToSingle(array[1], CultureInfo.InvariantCulture);
				matrix.M13 = Convert.ToSingle(array[2], CultureInfo.InvariantCulture);
				matrix.M21 = Convert.ToSingle(array[3], CultureInfo.InvariantCulture);
				matrix.M22 = Convert.ToSingle(array[4], CultureInfo.InvariantCulture);
				matrix.M23 = Convert.ToSingle(array[5], CultureInfo.InvariantCulture);
				matrix.M31 = Convert.ToSingle(array[6], CultureInfo.InvariantCulture);
				matrix.M32 = Convert.ToSingle(array[7], CultureInfo.InvariantCulture);
				matrix.M33 = Convert.ToSingle(array[8], CultureInfo.InvariantCulture);
				matrix = Matrix.Normalize(matrix);
				matrix *= Matrix.CreateRotationX((float)Math.E * 449f / 777f);
				matrix.Up = -matrix.Up;
				ptr[i] = matrix;
			}
			MyMapping myMapping = MyMapping.MapDiscard(AlphamaskViewsConstants);
			for (int j = 0; j < s_viewVectorData.Length; j++)
			{
				myMapping.WriteAndPosition(ref ptr[j]);
			}
			myMapping.Unmap();
		}

		internal unsafe static IConstantBuffer GetForwardConstants(MyRenderContext RC, Matrix proj)
		{
<<<<<<< HEAD
			if (!m_forwardConstants.TryTake(out var result))
			{
				result = MyManagers.Buffers.CreateConstantBuffer("ForwardBuffer", sizeof(ForwardConstants), null, ResourceUsage.Dynamic);
=======
			IConstantBuffer constantBuffer = default(IConstantBuffer);
			if (!m_forwardConstants.TryTake(ref constantBuffer))
			{
				constantBuffer = MyManagers.Buffers.CreateConstantBuffer("ForwardBuffer", sizeof(ForwardConstants), null, ResourceUsage.Dynamic);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			ForwardConstants forwardConstants = default(ForwardConstants);
			forwardConstants.ProjectionMatrix = Matrix.Transpose(proj);
			ForwardConstants data = forwardConstants;
<<<<<<< HEAD
			MyMapping myMapping = MyMapping.MapDiscard(RC, result);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			return result;
=======
			MyMapping myMapping = MyMapping.MapDiscard(RC, constantBuffer);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			return constantBuffer;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static void ReturnForwardConstants(IConstantBuffer buffer)
		{
			m_forwardConstants.Add(buffer);
		}
	}
}
