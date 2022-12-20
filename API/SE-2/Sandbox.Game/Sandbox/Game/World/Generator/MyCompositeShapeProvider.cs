using System;
using System.IO;
using System.Linq;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using VRage.Game;
using VRage.Noise;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
	[MyStorageDataProvider(10002)]
	internal sealed class MyCompositeShapeProvider : MyCompositeShapeProviderBase, IMyStorageDataProvider
	{
		public class MyCombinedCompositeInfoProvider : MyProceduralCompositeInfoProvider, IMyCompositionInfoProvider
		{
			private new readonly IMyCompositeShape[] m_filledShapes;

			private new readonly IMyCompositeShape[] m_removedShapes;

			IMyCompositeShape[] IMyCompositionInfoProvider.FilledShapes => m_filledShapes;

			IMyCompositeShape[] IMyCompositionInfoProvider.RemovedShapes => m_removedShapes;

			public MyCombinedCompositeInfoProvider(ref ConstructionData data, IMyCompositeShape[] filledShapes, IMyCompositeShape[] removedShapes)
				: base(ref data)
			{
				m_filledShapes = Enumerable.ToArray<IMyCompositeShape>(Enumerable.Concat<IMyCompositeShape>((IEnumerable<IMyCompositeShape>)base.m_filledShapes, (IEnumerable<IMyCompositeShape>)filledShapes));
				m_removedShapes = Enumerable.ToArray<IMyCompositeShape>(Enumerable.Concat<IMyCompositeShape>((IEnumerable<IMyCompositeShape>)base.m_removedShapes, (IEnumerable<IMyCompositeShape>)removedShapes));
			}

			public new void UpdateMaterials(MyVoxelMaterialDefinition defaultMaterial, MyCompositeShapeOreDeposit[] deposits)
			{
				base.UpdateMaterials(defaultMaterial, deposits);
			}
		}

		private struct State
		{
			public uint Version;

			public int Generator;

			public int Seed;

			public float Size;

			public uint UnusedCompat;

			public int GeneratorSeed;
		}

		public class MyProceduralCompositeInfoProvider : IMyCompositionInfoProvider
		{
			public struct ConstructionData
			{
				public IMyModule MacroModule;

				public IMyModule DetailModule;

				public MyCsgShapeBase[] FilledShapes;

				public MyCsgShapeBase[] RemovedShapes;

				public MyCompositeShapeOreDeposit[] Deposits;

				public MyVoxelMaterialDefinition DefaultMaterial;
			}

			protected class ProceduralCompositeShape : IMyCompositeShape
			{
				private MyCsgShapeBase m_shape;

				private MyProceduralCompositeInfoProvider m_context;

				public ProceduralCompositeShape(MyProceduralCompositeInfoProvider context, MyCsgShapeBase shape)
				{
					m_shape = shape;
					m_context = context;
				}

				public ContainmentType Contains(ref BoundingBox queryBox, ref BoundingSphere querySphere, int lodVoxelSize)
				{
					return m_shape.Contains(ref queryBox, ref querySphere, lodVoxelSize);
				}

				public float SignedDistance(ref Vector3 localPos, int lodVoxelSize)
				{
					return m_shape.SignedDistance(ref localPos, lodVoxelSize, m_context.MacroModule, m_context.DetailModule);
				}

				public unsafe void ComputeContent(MyStorageData target, int lodIndex, Vector3I minInLod, Vector3I maxInLod, int lodVoxelSize)
				{
					Vector3I vector3I = minInLod;
					Vector3I vector3I2 = vector3I * lodVoxelSize;
					Vector3I vector3I3 = vector3I2;
					fixed (byte* ptr = target[MyStorageDataTypeEnum.Content])
					{
						byte* ptr2 = ptr;
						_ = target.SizeLinear;
						vector3I.Z = minInLod.Z;
						while (vector3I.Z <= maxInLod.Z)
						{
							vector3I.Y = minInLod.Y;
							while (vector3I.Y <= maxInLod.Y)
							{
								vector3I.X = minInLod.X;
								while (vector3I.X <= maxInLod.X)
								{
									Vector3 localPos = new Vector3(vector3I2);
									float signedDistance = SignedDistance(ref localPos, lodVoxelSize);
									*ptr2 = MyCompositeShapeProviderBase.SignedDistanceToContent(signedDistance);
									ptr2 += target.StepLinear;
									vector3I2.X += lodVoxelSize;
									vector3I.X++;
								}
								vector3I2.Y += lodVoxelSize;
								vector3I2.X = vector3I3.X;
								vector3I.Y++;
							}
							vector3I2.Z += lodVoxelSize;
							vector3I2.Y = vector3I3.Y;
							vector3I.Z++;
						}
					}
				}

				public virtual void DebugDraw(ref MatrixD worldMatrix, Color color)
				{
					m_shape.DebugDraw(ref worldMatrix, color);
				}

				public void Close()
				{
				}
			}

			protected class ProceduralCompositeOreDeposit : ProceduralCompositeShape, IMyCompositeDeposit, IMyCompositeShape
			{
				private readonly MyCompositeShapeOreDeposit m_deposit;

				public ProceduralCompositeOreDeposit(MyProceduralCompositeInfoProvider context, MyCompositeShapeOreDeposit deposit)
					: base(context, deposit.Shape)
				{
					m_deposit = deposit;
				}

				public MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 localPos, float lodVoxelSize)
				{
					return m_deposit.GetMaterialForPosition(ref localPos, lodVoxelSize);
				}

				public override void DebugDraw(ref MatrixD worldMatrix, Color color)
				{
					m_deposit.DebugDraw(ref worldMatrix, color);
				}
			}

			public readonly IMyModule MacroModule;

			public readonly IMyModule DetailModule;

			protected ProceduralCompositeOreDeposit[] m_deposits;

			protected MyVoxelMaterialDefinition m_defaultMaterial;

			protected readonly ProceduralCompositeShape[] m_filledShapes;

			protected readonly ProceduralCompositeShape[] m_removedShapes;

			IMyCompositeDeposit[] IMyCompositionInfoProvider.Deposits => m_deposits;

			IMyCompositeShape[] IMyCompositionInfoProvider.FilledShapes => m_filledShapes;

			IMyCompositeShape[] IMyCompositionInfoProvider.RemovedShapes => m_removedShapes;

			MyVoxelMaterialDefinition IMyCompositionInfoProvider.DefaultMaterial => m_defaultMaterial;

			public MyProceduralCompositeInfoProvider(ref ConstructionData data)
			{
				MacroModule = data.MacroModule;
				DetailModule = data.DetailModule;
				m_defaultMaterial = data.DefaultMaterial;
				m_deposits = Enumerable.ToArray<ProceduralCompositeOreDeposit>(Enumerable.Select<MyCompositeShapeOreDeposit, ProceduralCompositeOreDeposit>((IEnumerable<MyCompositeShapeOreDeposit>)data.Deposits, (Func<MyCompositeShapeOreDeposit, ProceduralCompositeOreDeposit>)((MyCompositeShapeOreDeposit x) => new ProceduralCompositeOreDeposit(this, x))));
				m_filledShapes = Enumerable.ToArray<ProceduralCompositeShape>(Enumerable.Select<MyCsgShapeBase, ProceduralCompositeShape>((IEnumerable<MyCsgShapeBase>)data.FilledShapes, (Func<MyCsgShapeBase, ProceduralCompositeShape>)((MyCsgShapeBase x) => new ProceduralCompositeShape(this, x))));
				m_removedShapes = Enumerable.ToArray<ProceduralCompositeShape>(Enumerable.Select<MyCsgShapeBase, ProceduralCompositeShape>((IEnumerable<MyCsgShapeBase>)data.RemovedShapes, (Func<MyCsgShapeBase, ProceduralCompositeShape>)((MyCsgShapeBase x) => new ProceduralCompositeShape(this, x))));
			}

			void IMyCompositionInfoProvider.Close()
			{
			}

			protected void UpdateMaterials(MyVoxelMaterialDefinition defaultMaterial, MyCompositeShapeOreDeposit[] deposits)
			{
				m_defaultMaterial = defaultMaterial;
				m_deposits = Enumerable.ToArray<ProceduralCompositeOreDeposit>(Enumerable.Select<MyCompositeShapeOreDeposit, ProceduralCompositeOreDeposit>((IEnumerable<MyCompositeShapeOreDeposit>)deposits, (Func<MyCompositeShapeOreDeposit, ProceduralCompositeOreDeposit>)((MyCompositeShapeOreDeposit x) => new ProceduralCompositeOreDeposit(this, x))));
			}
		}

		private const uint CURRENT_VERSION = 3u;

		private const uint VERSION_WITHOUT_PLANETS = 1u;

		private const uint VERSION_WITHOUT_GENERATOR_SEED = 2u;

		private State m_state;

		public unsafe override int SerializedSize => sizeof(State);

		private void InitFromState(State state)
		{
			m_state = state;
			MyCompositeShapeGeneratorDelegate myCompositeShapeGeneratorDelegate = MyCompositeShapes.AsteroidGenerators[state.Generator];
			m_infoProvider = myCompositeShapeGeneratorDelegate(state.GeneratorSeed, state.Seed, state.Size);
		}

		public override void WriteTo(Stream stream)
		{
			stream.WriteNoAlloc(m_state.Version);
			stream.WriteNoAlloc(m_state.Generator);
			stream.WriteNoAlloc(m_state.Seed);
			stream.WriteNoAlloc(m_state.Size);
			stream.WriteNoAlloc(m_state.UnusedCompat);
			stream.WriteNoAlloc(m_state.GeneratorSeed);
		}

		public override void ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat)
		{
			State state = default(State);
			state.Version = stream.ReadUInt32();
			if (state.Version != 3)
			{
				isOldFormat = true;
			}
			state.Generator = stream.ReadInt32();
			state.Seed = stream.ReadInt32();
			state.Size = stream.ReadFloat();
			if (state.Version == 1)
			{
				state.UnusedCompat = 0u;
				state.GeneratorSeed = 0;
			}
			else
			{
				state.UnusedCompat = stream.ReadUInt32();
				if (state.UnusedCompat == 1)
				{
					throw new InvalidBranchException();
				}
				if (state.Version <= 2)
				{
					isOldFormat = true;
					state.GeneratorSeed = 0;
				}
				else
				{
					state.GeneratorSeed = stream.ReadInt32();
				}
			}
			InitFromState(state);
			m_state.Version = 3u;
		}

<<<<<<< HEAD
		public override void DebugDraw(ref MatrixD worldMatrix)
		{
			base.DebugDraw(ref worldMatrix);
			if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_SEEDS)
			{
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "Size: " + m_state.Size + Environment.NewLine + "Seed: " + m_state.Seed + Environment.NewLine + "GeneratorSeed: " + m_state.GeneratorSeed, Color.Red, 0.7f, depthRead: false);
=======
		private new static void SetupReading(int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, out int lodVoxelSize, out BoundingBox queryBox, out BoundingSphere querySphere)
		{
			float num = 0.5f * (float)(1 << lodIndex);
			lodVoxelSize = (int)(num * 2f);
			Vector3I voxelCoord = minInLod << lodIndex;
			Vector3I voxelCoord2 = maxInLod << lodIndex;
			MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord, out var localPosition);
			Vector3 min = localPosition;
			MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord2, out localPosition);
			Vector3 max = localPosition;
			min -= num;
			max += num;
			queryBox = new BoundingBox(min, max);
			BoundingSphere.CreateFromBoundingBox(ref queryBox, out querySphere);
		}

		public override void DebugDraw(ref MatrixD worldMatrix)
		{
			base.DebugDraw(ref worldMatrix);
			if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_SEEDS)
			{
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "Size: " + m_state.Size + Environment.get_NewLine() + "Seed: " + m_state.Seed + Environment.get_NewLine() + "GeneratorSeed: " + m_state.GeneratorSeed, Color.Red, 0.7f, depthRead: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static MyCompositeShapeProvider CreateAsteroidShape(int seed, float size, int generatorSeed = 0, int? generator = null)
		{
			State state = default(State);
			state.Version = 3u;
			state.Generator = generator.GetValueOrDefault(MySession.Static.Settings.VoxelGeneratorVersion);
			state.Seed = seed;
			state.Size = size;
			state.UnusedCompat = 0u;
			state.GeneratorSeed = generatorSeed;
			MyCompositeShapeProvider myCompositeShapeProvider = new MyCompositeShapeProvider();
			myCompositeShapeProvider.InitFromState(state);
			return myCompositeShapeProvider;
		}
	}
}
