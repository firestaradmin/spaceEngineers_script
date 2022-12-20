using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Engine.Physics
{
	/// <summary>
	/// Simple helper class that prepares a cache with all bones and their matching
	/// animation variable names. 
	/// Reads animation controller variable storage and looks for weight adjustments
	/// made by the animation state machines.
	///
	/// Designers can utilize this tool by creating a variable assignment in 
	/// AnimationControllerPlugin for VRageEditor on per node basis. This lets them 
	/// control the weight and blending time of animation and ragdoll layer.
	///
	/// USAGE: 
	///     rd_weight_BONE_NAME : bone specific weight setting.
	///     rd_blend_time_BONE_NAME : bone specific blend time setting.
	///
	///     rd_weight_LAYER_NAME : sets weights for all non masked bones inside the layer.
	///     rd_blend_time_LAYER_NAME : sets blend times for all non masked bones inside the layer.
	///
	///     rd_default_blend_time : sets the default blend time value.
	///
	///     The specific setting is used when provided instead of the layer setting.
	///     Any variable with value lesser than 0 will be ignored.
	/// </summary>
	public class MyRagdollAnimWeightBlendingHelper
	{
		private struct BoneData
		{
			public MyStringId WeightId;

			public MyStringId BlendTimeId;

			public double BlendTimeMs;

			public double StartedMs;

			public float StartingWeight;

			public float TargetWeight;

			public float PrevWeight;

			public LayerData[] Layers;
		}

		private struct LayerData
		{
			public MyStringId LayerId;

			public MyStringId LayerBlendTimeId;
		}

		private const string RAGDOLL_WEIGHT_VARIABLE_PREFIX = "rd_weight_";

		private const string RAGDOLL_BLEND_TIME_VARIABLE_PREFIX = "rd_blend_time_";

		private const string RAGDOLL_DEFAULT_BLEND_TIME_VARIABLE_NAME = "rd_default_blend_time";

		private const float DEFAULT_BLEND_TIME = 2.5f;

		private static readonly MyGameTimer TIMER = new MyGameTimer();

		private BoneData[] m_boneIndexToData;

		private float m_defaultBlendTime = 0.8f;

		private MyStringId m_defautlBlendTimeId;

		public bool Initialized { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// Call to initialize the internal structure of the helper. 
		/// Calling twice will result in remap.
		/// </summary>
		/// <param name="bones">Initialized and final list of bones of the character.</param>
		/// <param name="controller">Initialized animation controller for the character.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool Init(MyCharacterBone[] bones, MyAnimationController controller)
		{
			List<MyAnimationStateMachine> list = new List<MyAnimationStateMachine>(controller.GetLayerCount());
			for (int i = 0; i < controller.GetLayerCount(); i++)
			{
				MyAnimationStateMachine layerByIndex = controller.GetLayerByIndex(i);
				if (layerByIndex.BoneMask == null)
				{
					return false;
				}
				list.Add(layerByIndex);
			}
			m_boneIndexToData = new BoneData[bones.Length];
			foreach (MyCharacterBone bone in bones)
			{
				m_boneIndexToData[bone.Index] = new BoneData
				{
					WeightId = MyStringId.GetOrCompute("rd_weight_" + bone.Name),
					BlendTimeId = MyStringId.GetOrCompute("rd_blend_time_" + bone.Name),
					BlendTimeMs = -1.0,
					StartingWeight = 0f,
					TargetWeight = 0f,
					PrevWeight = 0f,
					Layers = Enumerable.ToArray<LayerData>(Enumerable.Select<MyAnimationStateMachine, LayerData>(Enumerable.Where<MyAnimationStateMachine>((IEnumerable<MyAnimationStateMachine>)list, (Func<MyAnimationStateMachine, bool>)((MyAnimationStateMachine layer) => layer.BoneMask[bone.Index])), (Func<MyAnimationStateMachine, LayerData>)delegate(MyAnimationStateMachine layer)
					{
						LayerData result = default(LayerData);
						result.LayerId = MyStringId.GetOrCompute("rd_weight_" + layer.Name);
						result.LayerBlendTimeId = MyStringId.GetOrCompute("rd_blend_time_" + layer.Name);
						return result;
					}))
				};
			}
			m_defautlBlendTimeId = MyStringId.GetOrCompute("rd_default_blend_time");
			Initialized = true;
			return true;
		}

		public void Prepare(IMyVariableStorage<float> controllerVariables)
		{
			if (!controllerVariables.GetValue(m_defautlBlendTimeId, out m_defaultBlendTime))
			{
				m_defaultBlendTime = 2.5f;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Corrects the weight according to blend variables from variable storage.
		/// </summary>
		/// <param name="weight">Current considered weight.</param>
		/// <param name="bone">Blended bone.</param>
		/// <param name="controllerVariables">Variables storage of a animation controller animating the bone.</param>        
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void BlendWeight(ref float weight, MyCharacterBone bone, MyAnimationVariableStorage controllerVariables)
		{
			if (m_boneIndexToData.Length <= bone.Index)
			{
				return;
			}
			ref BoneData reference = ref m_boneIndexToData[bone.Index];
			if (!controllerVariables.GetValue(reference.WeightId, out var value) || value < 0f)
			{
				value = -1f;
			}
			if (!controllerVariables.GetValue(reference.BlendTimeId, out var value2) || value2 < 0f)
			{
				value2 = -1f;
			}
			if (value < 0f || value2 < 0f)
			{
				float num = float.MaxValue;
				float num2 = float.MaxValue;
				LayerData[] layers = reference.Layers;
				for (int i = 0; i < layers.Length; i++)
				{
					LayerData layerData = layers[i];
					if (controllerVariables.GetValue(layerData.LayerId, out var value3))
					{
						num = Math.Min(num, value3);
					}
					if (controllerVariables.GetValue(layerData.LayerBlendTimeId, out var value4))
					{
						num2 = Math.Min(num2, value4);
					}
				}
				if (value < 0f)
				{
					if (num == float.MaxValue)
					{
						return;
					}
					value = num;
				}
				if (value2 < 0f)
				{
					value2 = ((num2 == float.MaxValue) ? m_defaultBlendTime : num2);
				}
			}
			double totalMilliseconds = TIMER.ElapsedTimeSpan.TotalMilliseconds;
			reference.BlendTimeMs = value2 * 1000f;
			if (value != reference.TargetWeight)
			{
				reference.StartedMs = totalMilliseconds;
				reference.StartingWeight = ((reference.PrevWeight == -1f) ? weight : reference.PrevWeight);
				reference.TargetWeight = value;
			}
			double amount = MathHelper.Clamp((totalMilliseconds - reference.StartedMs) / reference.BlendTimeMs, 0.0, 1.0);
			weight = (float)MathHelper.Lerp(reference.StartingWeight, reference.TargetWeight, amount);
			reference.PrevWeight = weight;
		}

		public void ResetWeights()
		{
			if (m_boneIndexToData != null)
			{
				for (int i = 0; i < m_boneIndexToData.Length; i++)
				{
					m_boneIndexToData[i].PrevWeight = 0f;
					m_boneIndexToData[i].TargetWeight = 0f;
				}
			}
		}
	}
}
