using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Lights;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.EntityComponents
{
	public class MyLightingLogic
	{
		public class LightLocalData
		{
			public Matrix LocalMatrix;

			public MyEntitySubpart Subpart;
		}

		private const int NUM_DECIMALS = 1;

		private readonly float m_lightTurningOnSpeed = 0.05f;

		private Color m_bulbColor = Color.Black;

		private float m_currentLightPower;

		private bool m_blinkOn = true;

		private float m_radius;

		private float m_reflectorRadius;

		private Color m_color;

		private float m_falloff;

		private readonly MyFunctionalBlock m_block;

		private string m_model;

		private string m_pointLightEmissiveMaterial;

		private string m_spotLightEmissiveMaterial;

		private IMyLightingLogicSync m_sync;

		private string m_lightDummyName;

		private bool m_isSearchlight;

		public bool HasSubPartLights { get; private set; }

		public bool IsReflector { get; set; }

		public bool IsPositionDirty { get; set; } = true;


		public bool IsEmissiveMaterialDirty { get; set; }

		public bool NeedsRecreateLights { get; set; }

		public List<MyLight> Lights { get; } = new List<MyLight>();


		public List<LightLocalData> LightLocalDatas { get; } = new List<LightLocalData>();


		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				if (m_color != value)
				{
					m_color = value;
					BulbColor = ComputeBulbColor();
					UpdateEmissivity(force: true);
					UpdateLightProperties();
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public float Radius
		{
			get
			{
				return m_radius;
			}
			set
			{
				if (m_radius != value)
				{
					m_radius = value;
					UpdateLightProperties();
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public float ReflectorRadius
		{
			get
			{
				return m_reflectorRadius;
			}
			set
			{
				if (m_reflectorRadius != value)
				{
					m_reflectorRadius = value;
					UpdateLightProperties();
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public float BlinkIntervalSeconds
		{
			get
			{
				return m_sync.BlinkIntervalSecondsSync;
			}
			set
			{
				if ((float)m_sync.BlinkIntervalSecondsSync != value)
				{
					if (value > (float)m_sync.BlinkIntervalSecondsSync)
					{
						m_sync.BlinkIntervalSecondsSync.Value = (float)Math.Round(value + 0.04999f, 1);
					}
					else
					{
						m_sync.BlinkIntervalSecondsSync.Value = (float)Math.Round(value - 0.04999f, 1);
					}
					if ((float)m_sync.BlinkIntervalSecondsSync == 0f && m_block.Enabled)
					{
						UpdateEnabled();
					}
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public virtual float Falloff
		{
			get
			{
				return m_falloff;
			}
			set
			{
				if (m_falloff != value)
				{
					m_falloff = value;
					UpdateIntensity();
					UpdateLightProperties();
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public float Intensity
		{
			get
			{
				return m_sync.IntensitySync;
			}
			set
			{
				if ((float)m_sync.IntensitySync != value)
				{
					m_sync.IntensitySync.Value = value;
					UpdateIntensity();
					UpdateLightProperties();
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public float Offset
		{
			get
			{
				return m_sync.LightOffsetSync.Value;
			}
			set
			{
				if ((float)m_sync.LightOffsetSync != value)
				{
					m_sync.LightOffsetSync.Value = value;
					UpdateLightProperties();
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public float CurrentLightPower
		{
			get
			{
				return m_currentLightPower;
			}
			set
			{
				if (m_currentLightPower != value)
				{
					m_currentLightPower = value;
					IsEmissiveMaterialDirty = true;
				}
			}
		}

		public Color BulbColor
		{
			get
			{
				return m_bulbColor;
			}
			set
			{
				if (m_bulbColor != value)
				{
					m_bulbColor = value;
					IsEmissiveMaterialDirty = true;
				}
			}
		}

		public float BlinkLength
		{
			get
			{
				return m_sync.BlinkLengthSync;
			}
			set
			{
				if ((float)m_sync.BlinkLengthSync != value)
				{
					m_sync.BlinkLengthSync.Value = (float)Math.Round(value, 1);
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public float BlinkOffset
		{
			get
			{
				return m_sync.BlinkOffsetSync;
			}
			set
			{
				if ((float)m_sync.BlinkOffsetSync != value)
				{
					m_sync.BlinkOffsetSync.Value = (float)Math.Round(value, 1);
					this.OnPropertiesChanged.InvokeIfNotNull();
				}
			}
		}

		public MyBounds BlinkIntervalSecondsBounds { get; }

		public MyBounds BlinkLengthBounds { get; }

		public MyBounds BlinkOffsetBounds { get; }

		public MyBounds FalloffBounds { get; }

		public MyBounds OffsetBounds { get; }

		public MyBounds RadiusBounds { get; }

		public MyBounds ReflectorRadiusBounds { get; }

		public MyBounds IntensityBounds { get; }

		public float ReflectorConeDegrees { get; }

		public bool NeedPerFrameUpdate => false | m_isSearchlight | ((float)m_sync.BlinkIntervalSecondsSync > 0f && m_block.IsWorking) | (GetNewLightPower() != CurrentLightPower) | HasSubPartLights;

		public event Action OnPropertiesChanged;

		public event Action OnIntensityUpdated;

		public event Action<bool> OnUpdateEnabled;

		public event Action<MyLight, Vector4, float, float> OnInitLight;

		public event Action<bool> OnEmissivityUpdated;

		public event Action<float> OnRadiusUpdated;

		private void UpdateIntensity()
		{
			this.OnIntensityUpdated.InvokeIfNotNull();
		}

		private void UpdateEnabled(bool state)
		{
			this.OnUpdateEnabled.InvokeIfNotNull(state);
		}

		private void InitLight(MyLight light, Vector4 color, float radius, float falloff)
		{
			this.OnInitLight.InvokeIfNotNull(light, color, radius, falloff);
		}

		private void UpdateEmissivity(bool force = false)
		{
			this.OnEmissivityUpdated.InvokeIfNotNull(force);
		}

		public MyLightingLogic(MyFunctionalBlock block, MyLightingBlockDefinition blockDefinition, IMyLightingLogicSync sync)
		{
			m_block = block;
			m_model = blockDefinition.Model;
			m_pointLightEmissiveMaterial = blockDefinition.PointLightEmissiveMaterial;
			m_spotLightEmissiveMaterial = blockDefinition.SpotLightEmissiveMaterial;
			m_sync = sync;
			m_lightDummyName = blockDefinition.LightDummyName;
			BlinkIntervalSecondsBounds = blockDefinition.BlinkIntervalSeconds;
			BlinkLengthBounds = blockDefinition.BlinkLenght;
			BlinkOffsetBounds = blockDefinition.BlinkOffset;
			FalloffBounds = blockDefinition.LightFalloff;
			OffsetBounds = blockDefinition.LightOffset;
			RadiusBounds = blockDefinition.LightRadius;
			IntensityBounds = blockDefinition.LightIntensity;
			ReflectorRadiusBounds = blockDefinition.LightReflectorRadius;
			ReflectorConeDegrees = blockDefinition.ReflectorConeDegrees;
			m_sync.IntensitySync.ValueChanged += delegate
			{
				UpdateIntensity();
				UpdateLightProperties();
			};
			m_sync.LightColorSync.ValueChanged += delegate
			{
				Color = m_sync.LightColorSync.Value;
			};
			m_sync.LightRadiusSync.ValueChanged += delegate
			{
				UpdateRadius(m_sync.LightRadiusSync.Value);
			};
			m_sync.LightFalloffSync.ValueChanged += delegate
			{
				Falloff = m_sync.LightFalloffSync.Value;
			};
			m_sync.LightOffsetSync.ValueChanged += delegate
			{
				UpdateLightProperties();
			};
		}

		public MyLightingLogic(MyFunctionalBlock block, MyHeatVentBlockDefinition blockDefinition, IMyLightingLogicSync sync)
		{
			m_block = block;
			m_model = blockDefinition.Model;
			m_pointLightEmissiveMaterial = blockDefinition.EmissiveMaterialName;
			m_sync = sync;
			m_lightDummyName = blockDefinition.LightDummyName;
			BlinkIntervalSecondsBounds = new MyBounds(0f, 0f, 0f);
			BlinkLengthBounds = new MyBounds(0f, 0f, 0f);
			BlinkOffsetBounds = new MyBounds(0f, 0f, 0f);
			FalloffBounds = blockDefinition.LightFalloffBounds;
			OffsetBounds = blockDefinition.LightOffsetBounds;
			RadiusBounds = blockDefinition.LightRadiusBounds;
			IntensityBounds = blockDefinition.LightIntensityBounds;
			ReflectorRadius = blockDefinition.LightOffsetBounds.Default;
			ReflectorConeDegrees = blockDefinition.ReflectorConeDegrees;
			m_sync.IntensitySync.ValueChanged += delegate
			{
				UpdateIntensity();
				UpdateLightProperties();
			};
			m_sync.LightColorSync.ValueChanged += delegate
			{
				Color = m_sync.LightColorSync.Value;
			};
			m_sync.LightRadiusSync.ValueChanged += delegate
			{
				UpdateRadius(m_sync.LightRadiusSync.Value);
			};
			m_sync.LightFalloffSync.ValueChanged += delegate
			{
				Falloff = m_sync.LightFalloffSync.Value;
			};
			m_sync.LightOffsetSync.ValueChanged += delegate
			{
				UpdateLightProperties();
			};
		}

		public MyLightingLogic(MyFunctionalBlock block, MySearchlightDefinition blockDefinition, IMyLightingLogicSync sync)
		{
			m_block = block;
			m_model = blockDefinition.Model;
			m_pointLightEmissiveMaterial = blockDefinition.PointLightEmissiveMaterial;
			m_spotLightEmissiveMaterial = blockDefinition.SpotLightEmissiveMaterial;
			m_sync = sync;
			m_lightDummyName = blockDefinition.LightDummyName;
			m_isSearchlight = true;
			BlinkIntervalSecondsBounds = blockDefinition.BlinkIntervalSeconds;
			BlinkLengthBounds = blockDefinition.BlinkLenght;
			BlinkOffsetBounds = blockDefinition.BlinkOffset;
			FalloffBounds = blockDefinition.LightFalloff;
			OffsetBounds = blockDefinition.LightOffset;
			RadiusBounds = blockDefinition.LightRadius;
			IntensityBounds = blockDefinition.LightIntensity;
			ReflectorRadiusBounds = blockDefinition.LightReflectorRadius;
			ReflectorConeDegrees = blockDefinition.ReflectorConeDegrees;
			m_sync.IntensitySync.ValueChanged += delegate
			{
				UpdateIntensity();
				UpdateLightProperties();
			};
			m_sync.LightColorSync.ValueChanged += delegate
			{
				Color = m_sync.LightColorSync.Value;
			};
			m_sync.LightRadiusSync.ValueChanged += delegate
			{
				UpdateRadius(m_sync.LightRadiusSync.Value);
			};
			m_sync.LightFalloffSync.ValueChanged += delegate
			{
				Falloff = m_sync.LightFalloffSync.Value;
			};
			m_sync.LightOffsetSync.ValueChanged += delegate
			{
				UpdateLightProperties();
			};
		}

		public void Initialize()
		{
			UpdateLightData();
			IsPositionDirty = true;
			CreateLights();
			UpdateIntensity();
			UpdateLightPosition();
			UpdateLightBlink();
			UpdateEnabled();
		}

		public Color ComputeBulbColor()
		{
			float num = IntensityBounds.Normalize(Intensity);
			float num2 = 0.125f + num * 0.25f;
			float num3 = (float)(int)Color.R * 0.5f + num2;
			float num4 = (float)(int)Color.G * 0.5f + num2;
			float num5 = (float)(int)Color.B * 0.5f + num2;
			return new Color((int)num3, (int)num4, (int)num5);
		}

		public void OnAddedToScene()
		{
			uint parentCullObject = m_block.CubeGrid.Render.RenderData.GetOrAddCell(m_block.Position * m_block.CubeGrid.GridSize).ParentCullObject;
			foreach (MyLight light in Lights)
			{
				light.ParentID = parentCullObject;
			}
			UpdateLightPosition();
			UpdateLightProperties();
			UpdateEmissivity(force: true);
		}

		public void OnModelChange()
		{
			UpdateLightData();
			NeedsRecreateLights = true;
			IsEmissiveMaterialDirty = true;
		}

		public void UpdateVisual()
		{
			UpdateParents();
			IsPositionDirty = true;
			IsEmissiveMaterialDirty = true;
			UpdateLightPosition();
			UpdateIntensity();
			UpdateLightBlink();
			UpdateEnabled();
		}

		public void UpdateOnceBeforeFrame()
		{
			if (NeedsRecreateLights)
			{
				RecreateLights();
			}
			UpdateParents();
			UpdateLightProperties();
			UpdateEmissiveMaterial();
		}

		public void UpdateAfterSimulation(Vector3 lightPosition, MatrixD lightRotation)
		{
			uint parentCullObject = m_block.CubeGrid.Render.RenderData.GetOrAddCell(m_block.Position * m_block.CubeGrid.GridSize).ParentCullObject;
			foreach (MyLight light in Lights)
			{
				light.ParentID = parentCullObject;
			}
			float newLightPower = GetNewLightPower();
			if (newLightPower != CurrentLightPower)
			{
				CurrentLightPower = newLightPower;
				UpdateIntensity();
			}
			UpdateLightBlink();
			UpdateEnabled();
			UpdateLightPosition(lightPosition, lightRotation);
			UpdateLightProperties();
			UpdateEmissivity();
			UpdateEmissiveMaterial();
		}

		public void UpdateAfterSimulation()
		{
			UpdateAfterSimulation(Vector3.PositiveInfinity, MatrixD.Zero);
		}

		public void UpdateLightProperties()
		{
			foreach (MyLight light in Lights)
			{
				_ = Offset;
				light.Range = m_radius;
				light.ReflectorRange = m_reflectorRadius;
				light.Color = m_color;
				light.ReflectorColor = m_color;
				light.Falloff = m_falloff;
				light.PointLightOffset = Offset;
				light.UpdateLight();
			}
		}

		public void UpdateLightPosition()
		{
			UpdateLightPosition(Vector3.PositiveInfinity, MatrixD.Zero);
		}

		public void UpdateLightPosition(Vector3 lightPosition, MatrixD lightRotation)
		{
			if (Lights == null || Lights.Count == 0 || !IsPositionDirty)
			{
				return;
			}
			IsPositionDirty = false;
			for (int i = 0; i < LightLocalDatas.Count; i++)
			{
				MatrixD matrixD = m_block.PositionComp.LocalMatrixRef;
				MyLight myLight = Lights[i];
				if (lightPosition == Vector3.PositiveInfinity)
				{
					if (LightLocalDatas[i].Subpart != null)
					{
						matrixD = LightLocalDatas[i].Subpart.PositionComp.LocalMatrixRef * matrixD;
					}
					myLight.Position = Vector3D.Transform(LightLocalDatas[i].LocalMatrix.Translation, matrixD);
				}
				else
				{
					myLight.Position = Vector3D.Transform(Vector3D.Transform(lightPosition, m_block.PositionComp.WorldMatrixNormalizedInv), matrixD);
				}
				if (lightRotation == MatrixD.Zero)
				{
					myLight.ReflectorDirection = Vector3D.TransformNormal(LightLocalDatas[i].LocalMatrix.Forward, matrixD);
				}
				else
				{
					myLight.ReflectorDirection = (lightRotation * m_block.PositionComp.LocalMatrixRef).Forward;
				}
				myLight.ReflectorUp = Vector3D.TransformNormal(LightLocalDatas[i].LocalMatrix.Right, matrixD);
			}
		}

		public void UpdateEnabled()
		{
			UpdateEnabled(CurrentLightPower * Intensity > 0f && m_blinkOn && !m_block.IsPreview && !m_block.CubeGrid.IsPreview);
		}

		private void UpdateRadius(float value)
		{
			if (IsReflector)
			{
				ReflectorRadius = value;
			}
			else
			{
				Radius = value;
			}
			this.OnRadiusUpdated.InvokeIfNotNull(value);
		}

		public void CreateLights()
		{
			CloseLights();
			foreach (LightLocalData lightLocalData in LightLocalDatas)
			{
				_ = lightLocalData;
				MyLight myLight = MyLights.AddLight();
				if (myLight != null)
				{
					Lights.Add(myLight);
					InitLight(myLight, m_color, m_radius, m_falloff);
					myLight.ReflectorColor = m_color;
					myLight.ReflectorRange = m_reflectorRadius;
					myLight.Range = m_radius;
					myLight.ReflectorConeDegrees = ReflectorConeDegrees;
					UpdateRadius(IsReflector ? m_reflectorRadius : m_radius);
				}
			}
			IsPositionDirty = true;
		}

		public void CloseLights()
		{
			foreach (MyLight light in Lights)
			{
				MyLights.RemoveLight(light);
			}
			Lights.Clear();
		}

		public void UpdateLightData()
		{
			LightLocalDatas.Clear();
			HasSubPartLights = false;
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(m_model).Dummies)
			{
				string text = dummy.Key.ToLower();
				if (!text.Contains("subpart") && text.Contains(m_lightDummyName))
				{
					LightLocalDatas.Add(new LightLocalData
					{
						LocalMatrix = Matrix.Normalize(dummy.Value.Matrix),
						Subpart = null
					});
				}
			}
			GetSubpartLightDatas(m_block);
		}

		private void GetSubpartLightDatas(MyEntity entity)
		{
			foreach (KeyValuePair<string, MyEntitySubpart> subpart in entity.Subparts)
			{
				MyEntitySubpart value = subpart.Value;
				if (value.Model == null)
				{
					continue;
				}
				foreach (KeyValuePair<string, MyModelDummy> dummy in value.Model.Dummies)
				{
					if (dummy.Key.ToLower().Contains(m_lightDummyName))
					{
						LightLocalDatas.Add(new LightLocalData
						{
							LocalMatrix = Matrix.Normalize(dummy.Value.Matrix),
							Subpart = subpart.Value
						});
						HasSubPartLights = true;
					}
				}
				GetSubpartLightDatas(value);
			}
		}

		public void RecreateLights()
		{
			NeedsRecreateLights = false;
			CreateLights();
			UpdateLightPosition();
			UpdateIntensity();
			UpdateLightBlink();
			UpdateEnabled();
		}

		public void UpdateParents()
		{
			uint parentCullObject = m_block.CubeGrid.Render.RenderData.GetOrAddCell(m_block.Position * m_block.CubeGrid.GridSize).ParentCullObject;
			foreach (MyLight light in Lights)
			{
				light.ParentID = parentCullObject;
			}
		}

		public void UpdateLightBlink()
		{
			if ((float)m_sync.BlinkIntervalSecondsSync > 0.00099f)
			{
				ulong num = (ulong)((float)m_sync.BlinkIntervalSecondsSync * 1000f);
				float num2 = (float)num * (float)m_sync.BlinkOffsetSync * 0.01f;
				ulong num3 = (ulong)(MySession.Static.ElapsedGameTime.TotalMilliseconds - (double)num2) % num;
				ulong num4 = (ulong)((float)num * (float)m_sync.BlinkLengthSync * 0.01f);
				m_blinkOn = num4 > num3;
			}
			else
			{
				m_blinkOn = true;
			}
		}

		public void UpdateEmissiveMaterial()
		{
			if (!IsEmissiveMaterialDirty)
			{
				return;
			}
			uint[] renderObjectIDs = m_block.Render.RenderObjectIDs;
			foreach (uint renderId in renderObjectIDs)
			{
				UpdateEmissiveMaterial(renderId);
			}
			foreach (LightLocalData lightLocalData in LightLocalDatas)
			{
				if (lightLocalData.Subpart != null && lightLocalData.Subpart.Render != null)
				{
					renderObjectIDs = lightLocalData.Subpart.Render.RenderObjectIDs;
					foreach (uint renderId2 in renderObjectIDs)
					{
						UpdateEmissiveMaterial(renderId2);
					}
				}
			}
			IsEmissiveMaterialDirty = false;
		}

		private void UpdateEmissiveMaterial(uint renderId)
		{
			if (renderId != uint.MaxValue)
			{
				MyRenderProxy.UpdateModelProperties(renderId, m_pointLightEmissiveMaterial, (RenderFlags)0, (RenderFlags)0, BulbColor, CurrentLightPower);
				if (!string.IsNullOrEmpty(m_spotLightEmissiveMaterial))
				{
					MyRenderProxy.UpdateModelProperties(renderId, m_spotLightEmissiveMaterial, (RenderFlags)0, (RenderFlags)0, BulbColor, CurrentLightPower);
				}
			}
		}

		private float GetNewLightPower()
		{
			return MathHelper.Clamp(CurrentLightPower + (float)(m_block.IsWorking ? 1 : (-1)) * m_lightTurningOnSpeed, 0f, 1f);
		}
	}
}
