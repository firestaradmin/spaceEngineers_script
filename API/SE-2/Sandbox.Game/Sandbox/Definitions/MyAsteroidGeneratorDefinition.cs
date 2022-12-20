using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_AsteroidGeneratorDefinition), null)]
	public class MyAsteroidGeneratorDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyAsteroidGeneratorDefinition_003C_003EActor : IActivator, IActivator<MyAsteroidGeneratorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAsteroidGeneratorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAsteroidGeneratorDefinition CreateInstance()
			{
				return new MyAsteroidGeneratorDefinition();
			}

			MyAsteroidGeneratorDefinition IActivator<MyAsteroidGeneratorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int Version;

		/// <summary>
		/// Minimal size of lone asteroids
		/// </summary>
		public int ObjectSizeMin;

		/// <summary>
		/// Maximal size of lone asteroids
		/// </summary>
		public int ObjectSizeMax;

		public int SubcellSize;

		public int SubCells;

		/// <summary>
		/// Maximal number of asteroids per cluster
		/// </summary>
		public int ObjectMaxInCluster;

		/// <summary>
		/// Controls positional dispersion of cluster objects.
		/// Behavior is controlled by <see cref="F:Sandbox.Definitions.MyAsteroidGeneratorDefinition.ClusterDispersionAbsolute" />
		/// </summary>
		public int ObjectMinDistanceInCluster;

		public int ObjectMaxDistanceInClusterMin;

		public int ObjectMaxDistanceInClusterMax;

		/// <summary>
		/// Minimal size of individual cluster asteroids
		/// </summary>
		public int ObjectSizeMinCluster;

		/// <summary>
		/// Maximal size of individual cluster asteroids
		/// </summary>
		public int ObjectSizeMaxCluster;

		/// <summary>
		/// Probability that generated object in cluster will be used
		/// </summary>
		public double ObjectDensityCluster;

		/// <summary>
		/// Enables absolute positioning in cluster
		/// </summary>
		public bool ClusterDispersionAbsolute;

		/// <summary>
		/// Allows objects in clusters to partially overlap (in terms of their AABBs)
		/// </summary>
		public bool AllowPartialClusterObjectOverlap;

		/// <summary>
		/// Backwards comp for incorrect cluster object size
		/// </summary>
		public bool UseClusterDefAsAsteroid;

		/// <summary>
		/// Enable asteroid rotation
		/// </summary>
		public bool RotateAsteroids;

		public bool UseLinearPowOfTwoSizeDistribution;

		public bool UseGeneratorSeed;

		public bool UseClusterVariableSize;

		public DictionaryReader<MyObjectSeedType, double> SeedTypeProbability;

		public DictionaryReader<MyObjectSeedType, double> SeedClusterTypeProbability;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AsteroidGeneratorDefinition myObjectBuilder_AsteroidGeneratorDefinition = (MyObjectBuilder_AsteroidGeneratorDefinition)builder;
			Version = int.Parse(myObjectBuilder_AsteroidGeneratorDefinition.Id.SubtypeId);
			SubCells = myObjectBuilder_AsteroidGeneratorDefinition.SubCells;
			ObjectSizeMax = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMax;
			SubcellSize = 4096 + myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMax * 2;
			ObjectSizeMin = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMin;
			RotateAsteroids = myObjectBuilder_AsteroidGeneratorDefinition.RotateAsteroids;
			UseGeneratorSeed = myObjectBuilder_AsteroidGeneratorDefinition.UseGeneratorSeed;
			ObjectMaxInCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMaxInCluster;
			ObjectDensityCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectDensityCluster;
			ObjectSizeMaxCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMaxCluster;
			ObjectSizeMinCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectSizeMinCluster;
			UseClusterVariableSize = myObjectBuilder_AsteroidGeneratorDefinition.UseClusterVariableSize;
			UseClusterDefAsAsteroid = myObjectBuilder_AsteroidGeneratorDefinition.UseClusterDefAsAsteroid;
			ClusterDispersionAbsolute = myObjectBuilder_AsteroidGeneratorDefinition.ClusterDispersionAbsolute;
			ObjectMinDistanceInCluster = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMinDistanceInCluster;
			ObjectMaxDistanceInClusterMax = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMaxDistanceInClusterMax;
			ObjectMaxDistanceInClusterMin = myObjectBuilder_AsteroidGeneratorDefinition.ObjectMaxDistanceInClusterMin;
			AllowPartialClusterObjectOverlap = myObjectBuilder_AsteroidGeneratorDefinition.AllowPartialClusterObjectOverlap;
			UseLinearPowOfTwoSizeDistribution = myObjectBuilder_AsteroidGeneratorDefinition.UseLinearPowOfTwoSizeDistribution;
			SeedTypeProbability = myObjectBuilder_AsteroidGeneratorDefinition.SeedTypeProbability.Dictionary;
			SeedClusterTypeProbability = myObjectBuilder_AsteroidGeneratorDefinition.SeedClusterTypeProbability.Dictionary;
		}
	}
}
