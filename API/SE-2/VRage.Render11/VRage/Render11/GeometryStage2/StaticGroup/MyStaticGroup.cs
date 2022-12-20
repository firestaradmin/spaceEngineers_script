using System;
using System.Collections.Generic;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Lodding;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.GeometryStage2.PreparePass;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.Resources;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;

namespace VRage.Render11.GeometryStage2.StaticGroup
{
	internal class MyStaticGroup
	{
		private MyModel m_model;

		private int m_instancesCount;

		private IVertexBuffer m_vbColorInstanceBuffer;

		private IVertexBuffer m_vbDepthInstanceBuffer;

<<<<<<< HEAD
=======
		private int m_cullProxyId;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsVisible;

		private Vector3D Translation;

		public MyLodStrategy LodStrategy;

		private MyRenderData[] m_renderData;

		public Vector3 CameraTranslation => Translation - MyRender11.Environment.Matrices.CameraPosition;

		public MyStaticGroup()
		{
			m_renderData = new MyRenderData[19];
			for (int i = 0; i < m_renderData.Length; i++)
			{
				m_renderData[i] = new MyRenderData();
			}
			IsVisible = true;
		}

		private BoundingBoxD GetBoundingBox(MyModel model, Vector3D translation, List<Matrix> matrices)
		{
			BoundingBox boundingBox = model.BoundingBox;
			Vector3D vector3D = Vector3D.PositiveInfinity;
			Vector3D vector3D2 = Vector3D.NegativeInfinity;
			foreach (Matrix matrix in matrices)
			{
				BoundingBox boundingBox2 = boundingBox.Transform(matrix);
				vector3D = Vector3D.Min(boundingBox2.Min + translation, vector3D);
				vector3D2 = Vector3D.Max(boundingBox2.Max + translation, vector3D2);
			}
			return new BoundingBoxD(vector3D, vector3D2);
		}

		public unsafe void Create(MyModel model, Vector3D translation, List<Matrix> matrices, MyActor root)
		{
			m_model = model;
			m_instancesCount = matrices.Count;
			Translation = translation;
			_ = (BoundingBox)GetBoundingBox(model, Vector3D.Zero, matrices);
			MyColorPreparePass0.MyVbConstantElement[] array = new MyColorPreparePass0.MyVbConstantElement[matrices.Count];
			MyDepthPreparePass0.MyVbConstantElement[] array2 = new MyDepthPreparePass0.MyVbConstantElement[matrices.Count];
			for (int i = 0; i < matrices.Count; i++)
			{
				Matrix m = matrices[i];
				RowMatrix worldMatrix = RowMatrix.Create(ref m);
				array[i] = new MyColorPreparePass0.MyVbConstantElement
				{
					KeyColorDithering = new HalfVector4(0f, 0f, 0f, 0f),
					ColorMultEmissivity = new HalfVector4(1f, 1f, 1f, 0f),
					WorldMatrix = worldMatrix
				};
				array2[i] = new MyDepthPreparePass0.MyVbConstantElement
				{
					WorldMatrix = worldMatrix
				};
			}
			fixed (MyColorPreparePass0.MyVbConstantElement* ptr = array)
			{
				void* value = ptr;
				m_vbColorInstanceBuffer = MyManagers.Buffers.CreateVertexBuffer("StaticGroup.ColorInstances", array.Length, sizeof(MyColorPreparePass0.MyVbConstantElement), new IntPtr(value));
			}
			fixed (MyDepthPreparePass0.MyVbConstantElement* ptr2 = array2)
			{
				void* value2 = ptr2;
				m_vbDepthInstanceBuffer = MyManagers.Buffers.CreateVertexBuffer("StaticGroup.DepthInstances", array2.Length, sizeof(MyDepthPreparePass0.MyVbConstantElement), new IntPtr(value2));
			}
			LodStrategy.Init(m_model.GetLodStrategyInfo());
		}

		public MyRenderData GetRenderData(Matrix viewProj, Matrix proj, int passId)
		{
			MyRenderData myRenderData = m_renderData[passId];
			Matrix viewProj2 = Matrix.CreateTranslation(CameraTranslation) * viewProj;
			myRenderData.Init(viewProj2, proj);
			myRenderData.InstanceLodGroups.Clear();
			for (int i = 0; i < 1; i++)
			{
				int currentLod = LodStrategy.CurrentLod;
				MyInstanceLodGroup myInstanceLodGroup = default(MyInstanceLodGroup);
				myInstanceLodGroup.Lod = m_model.GetLod(currentLod);
				myInstanceLodGroup.InstancesCount = m_instancesCount;
				myInstanceLodGroup.InstancesIncrement = m_instancesCount;
				MyInstanceLodGroup item = myInstanceLodGroup;
				myRenderData.InstanceLodGroups.Add(item);
			}
			if (MyViewIds.IsMainId(passId))
			{
				myRenderData.VBInstanceBuffer = m_vbColorInstanceBuffer;
			}
			else if (MyViewIds.IsShadowId(passId))
			{
				myRenderData.VBInstanceBuffer = m_vbDepthInstanceBuffer;
			}
			else
			{
				MyRenderProxy.Error("Unknown pass type");
			}
			return myRenderData;
		}

		public void Destroy(MyActor root)
		{
<<<<<<< HEAD
=======
			m_cullProxyId = 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_vbColorInstanceBuffer != null)
			{
				MyManagers.Buffers.Dispose(m_vbColorInstanceBuffer);
				m_vbColorInstanceBuffer = null;
			}
			if (m_vbDepthInstanceBuffer != null)
			{
				MyManagers.Buffers.Dispose(m_vbDepthInstanceBuffer);
				m_vbDepthInstanceBuffer = null;
			}
		}

		public bool IsGBufferVisible()
		{
			return IsVisible;
		}

		public bool IsDepthVisible()
		{
			return IsVisible;
		}
	}
}
