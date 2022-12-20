using System;
using System.Collections.Generic;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.LightingStage;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Scene.Components
{
	internal class MyLightComponent : VRage.Render.Scene.Components.MyLightComponent
	{
		private class VRage_Render11_Scene_Components_MyLightComponent_003C_003EActor : IActivator, IActivator<MyLightComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyLightComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLightComponent CreateInstance()
			{
				return new MyLightComponent();
			}

			MyLightComponent IActivator<MyLightComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		internal float ViewerDistanceSquaredFast;

		private MyStreamedTexturePin m_reflectorTexture;

		private MatrixD m_viewProjection;

		private Matrix m_projection;

		private int m_viewProjectionIndex;

		private float m_spotHeight;

		private HashSet<uint> m_ignoredShadowEntities;

		private const float EXTENT = 1f;

		private float m_lastRadius = -1f;

		private float m_lastOffset = -1f;

		private float m_lastQuerySize = -1f;

		private float m_lastSpotHeight = -1f;

<<<<<<< HEAD
=======
		private float m_lastSpotRange = -1f;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool m_aabbChange;

		private bool m_positionChanged;

		private MyChildCullTreeData m_cullData;

		private bool m_cullSpotOn;

		private bool m_cullPointOn;

		public int LastSpotShadowIndex = int.MaxValue;

		internal float ViewerDistanceSquared => base.Owner.CalculateCameraDistanceSquared();

		internal MatrixD SpotlightViewProjection
		{
			get
			{
				CheckViewProjection();
				return m_viewProjection;
			}
		}

		internal Matrix SpotlightProjection
		{
			get
			{
				CheckViewProjection();
				return m_projection;
			}
		}

		internal FlareId FlareId { get; private set; }

		public override Color DebugColor => Color.Yellow;

		public override void Construct()
		{
			base.Construct();
			FlareId = FlareId.NULL;
		}

		private void Reset()
		{
			m_lastRadius = -1f;
			m_lastQuerySize = -1f;
			m_lastSpotHeight = -1f;
			m_aabbChange = true;
			m_positionChanged = true;
		}

		public override void Assign(MyActor owner)
		{
			base.Assign(owner);
			base.Owner.SetVisibility(visibility: false);
			Reset();
		}

		public override void OnRemove(MyActor owner)
		{
			Reset();
			MyFlareRenderer.Remove(FlareId);
			FlareId = FlareId.NULL;
			m_reflectorTexture.Dispose();
			base.OnRemove(owner);
		}

		public override void OnVisibilityChange()
		{
			base.OnVisibilityChange();
			if (!base.IsVisible)
			{
				Reset();
			}
		}

		public override void UpdateData(ref UpdateRenderLightData data)
		{
			m_data = data;
			m_originalData = data;
			m_aabbChange |= m_data.AabbChanged;
			m_positionChanged |= m_data.PositionChanged;
			if (m_data.SpotLightOn && m_data.SpotIntensity < 0.001f)
			{
				m_data.SpotLightOn = false;
				m_data.Glare.Enabled = false;
			}
			if (m_data.PointIntensity < 0.001f)
			{
				m_data.PointLightOn = false;
				if (!m_data.SpotLightOn)
				{
					m_data.Glare.Enabled = false;
				}
			}
			bool flag = m_data.Glare.Enabled || m_data.PointLightOn || m_data.SpotLightOn;
			if (flag)
			{
				if (ShadowRange() == 0f)
				{
					m_data.CastShadows = false;
				}
				m_reflectorTexture.Dispose();
				m_reflectorTexture = MyManagers.Textures.GetPermanentTexture(m_data.ReflectorTexture, MyFileTextureEnum.CUSTOM);
				FlareId = MyFlareRenderer.Set(FlareId, base.Owner, m_data.Glare);
				float num = m_data.PointLight.Falloff / 1.5f;
				m_data.PointLight.Falloff = 1f / (0.001f + num * num * num);
				num = m_data.SpotLight.Light.Falloff / 1.5f;
				m_data.SpotLight.Light.Falloff = 1f / (0.001f + num * num * num);
				if (m_aabbChange)
				{
					if (m_data.SpotLightOn)
					{
						float num2 = (float)Math.Sqrt(1f - m_data.SpotLight.ApertureCos * m_data.SpotLight.ApertureCos) / m_data.SpotLight.ApertureCos;
						m_spotHeight = num2 * m_data.SpotLight.Light.Range;
					}
					if (!base.Owner.HasLocalAabb || (m_data.PointLightOn && (Math.Abs(m_data.PointLight.Range - m_lastRadius) > 1f || Math.Abs(m_data.PointOffset - m_lastOffset) > 1f)) || (FlareId != FlareId.NULL && Math.Abs(m_data.Glare.QuerySize - m_lastQuerySize) > 1f) || (m_data.SpotLightOn && Math.Abs(m_spotHeight - m_lastSpotHeight) > 1f))
					{
						BoundingBox localAabb = BoundingBox.CreateInvalid();
						if (m_data.SpotLightOn)
						{
							m_lastSpotHeight = m_spotHeight;
							float num3 = m_spotHeight + 1f;
<<<<<<< HEAD
							localAabb.Include(new Vector3(0f - num3, 0f - num3, 0f));
							localAabb.Include(new Vector3(num3, num3, 0f - (m_data.SpotLight.Light.Range + 1f)));
=======
							localAabb.Include(new Vector3D(0f - num3, 0f - num3, 0.0));
							localAabb.Include(new Vector3D(num3, num3, 0f - (m_data.SpotLight.Light.Range + 1f)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						else
						{
							m_lastSpotHeight = -1f;
						}
						if (m_data.PointLightOn)
						{
							m_lastRadius = m_data.PointLight.Range;
							m_lastOffset = m_data.PointOffset;
							Vector3 vector = Vector3.Forward * m_data.PointOffset;
							BoundingBox box = new BoundingBox(new Vector3(0f - (m_lastRadius + 1f)) + vector, new Vector3(m_lastRadius + 1f) + vector);
							localAabb.Include(box);
						}
						else
						{
							m_lastRadius = -1f;
						}
						if (FlareId != FlareId.NULL)
						{
							m_lastQuerySize = m_data.Glare.QuerySize;
							localAabb.Include(new BoundingBox(new Vector3(0f - (m_lastQuerySize + 1f)), new Vector3(m_lastQuerySize + 1f)));
						}
						else
						{
							m_lastQuerySize = -1f;
						}
						base.Owner.SetLocalAabb(localAabb);
						m_viewProjectionIndex = -1;
					}
					m_aabbChange = false;
				}
				if (m_positionChanged)
				{
					if (base.Owner.Parent != null)
					{
						Matrix m = Matrix.CreateWorld(m_data.Position, m_data.SpotLight.Direction, m_data.SpotLight.Up);
						base.Owner.SetRelativeTransform(ref m);
					}
					else
					{
						MatrixD matrix = MatrixD.CreateWorld(m_data.Position, m_data.SpotLight.Direction, m_data.SpotLight.Up);
						base.Owner.SetMatrix(ref matrix);
					}
					m_positionChanged = false;
				}
			}
			UpdateCullData();
			base.Owner.SetVisibility(flag);
		}

		private void UpdateCullData()
		{
			if ((m_cullData != null || (!m_data.SpotLightOn && !m_data.PointLightOn)) && m_cullSpotOn == m_data.SpotLightOn && m_cullPointOn == m_data.PointLightOn)
			{
				return;
			}
			m_cullSpotOn = m_data.SpotLightOn;
			m_cullPointOn = m_data.PointLightOn;
			if (m_data.SpotLightOn && m_data.PointLightOn)
			{
				m_cullData = new MyChildCullTreeData
				{
					Add = delegate(MyCullResultsBase x, bool y)
					{
						((MyCullResults)x).SpotLights.Add(this);
						((MyCullResults)x).PointLights.Add(this);
					},
					Remove = delegate(MyCullResultsBase x)
					{
						((MyCullResults)x).SpotLights.Remove(this);
						((MyCullResults)x).PointLights.Remove(this);
					},
					DebugColor = () => DebugColor
				};
			}
			else if (m_data.SpotLightOn)
			{
				m_cullData = new MyChildCullTreeData
				{
					Add = delegate(MyCullResultsBase x, bool y)
					{
						((MyCullResults)x).SpotLights.Add(this);
					},
					Remove = delegate(MyCullResultsBase x)
					{
						((MyCullResults)x).SpotLights.Remove(this);
					},
					DebugColor = () => DebugColor
				};
			}
			else if (m_data.PointLightOn)
			{
				m_cullData = new MyChildCullTreeData
				{
					Add = delegate(MyCullResultsBase x, bool y)
					{
						((MyCullResults)x).PointLights.Add(this);
					},
					Remove = delegate(MyCullResultsBase x)
					{
						((MyCullResults)x).PointLights.Remove(this);
					},
					DebugColor = () => DebugColor
				};
			}
			else
			{
				m_cullData = null;
			}
			base.Owner.InvalidateCullTreeData();
		}

		public override MyChildCullTreeData GetCullTreeData()
		{
			return m_cullData;
		}

		private void CheckViewProjection()
		{
			if (base.Owner.WorldMatrixIndex != m_viewProjectionIndex)
			{
				UpdateViewProjection();
				m_viewProjectionIndex = base.Owner.WorldMatrixIndex;
			}
		}

		private float ShadowRange()
		{
			return MyRender11.Settings.User.ShadowQuality.ReflectorShadowDistance() * m_data.SpotLight.Light.Range;
		}

		private void UpdateViewProjection()
		{
			if (m_data.SpotLightOn)
			{
				float num = 0.5f;
				MatrixD matrixD = MatrixD.CreateLookAt(base.Owner.WorldMatrix.Translation, base.Owner.WorldMatrix.Translation + base.Owner.WorldMatrix.Forward, base.Owner.WorldMatrix.Up);
				m_projection = Matrix.CreatePerspectiveFieldOfView((float)(Math.Acos(m_data.SpotLight.ApertureCos) * 2.0), 1f, num, Math.Max(ShadowRange(), num + 1f));
				m_viewProjection = matrixD * m_projection;
			}
		}

		private Matrix CreateShadowMatrix()
		{
			MatrixD m = MatrixD.CreateTranslation(MyRender11.Environment.Matrices.CameraPosition) * SpotlightViewProjection * MyMatrixHelpers.ClipspaceToTexture;
			return Matrix.Transpose(m);
		}

		internal void WritePointlightConstants(ref MyPointlightConstants data)
		{
			data.Light = m_data.PointLight;
			Vector3D vector3D = base.Owner.WorldMatrix.Translation + m_data.PointOffset * base.Owner.WorldMatrix.Forward;
			data.Light.Position = Vector3.Transform(vector3D - MyRender11.Environment.Matrices.CameraPosition, ref MyRender11.Environment.Matrices.ViewAt0);
		}

		internal ISrvBindable WriteSpotlightConstants(ref SpotlightConstants data)
		{
			data.Spotlight = m_data.SpotLight;
			Vector3 vector = base.Owner.WorldMatrix.Translation - MyRender11.Environment.Matrices.CameraPosition;
			data.Spotlight.Light.Position = vector;
			data.Spotlight.Direction = base.Owner.WorldMatrix.Forward;
			data.Spotlight.Up = base.Owner.WorldMatrix.Up;
			Matrix viewProjectionAt = MyRender11.Environment.Matrices.ViewProjectionAt0;
			if (MyStereoRender.Enable)
			{
				if (MyStereoRender.RenderRegion == MyStereoRegion.LEFT)
				{
					viewProjectionAt = MyStereoRender.EnvMatricesLeftEye.ViewProjectionAt0;
				}
				if (MyStereoRender.RenderRegion == MyStereoRegion.RIGHT)
				{
					viewProjectionAt = MyStereoRender.EnvMatricesRightEye.ViewProjectionAt0;
				}
			}
			Matrix matrix = base.Owner.WorldMatrix;
			matrix.Translation = vector;
			data.ProxyWorldViewProj = Matrix.Transpose(Matrix.CreateScale(2.63999987f * m_spotHeight, 2.63999987f * m_spotHeight, m_data.SpotLight.Light.Range * 1.31999993f) * matrix * viewProjectionAt);
			data.ShadowMatrix = CreateShadowMatrix();
			return m_reflectorTexture.Texture;
		}

		internal void IgnoreShadowForEntity(uint id)
		{
			if (m_ignoredShadowEntities == null)
			{
				m_ignoredShadowEntities = new HashSet<uint>();
			}
			m_ignoredShadowEntities.Add(id);
		}

		internal HashSet<uint> GetEntitiesIgnoringShadow()
		{
			return m_ignoredShadowEntities;
		}

		internal void ClearIgnoredEntities()
		{
			m_ignoredShadowEntities = null;
		}
	}
}
