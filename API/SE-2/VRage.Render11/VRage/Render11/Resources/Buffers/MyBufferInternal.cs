using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Library.Memory;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Buffers
{
	internal abstract class MyBufferInternal : IBuffer, IResource
	{
		internal bool IsReleased;

		private int m_elementCount;

		private SharpDX.Direct3D11.Buffer m_buffer;

		protected BufferDescription m_description;

		private MyMemorySystem.AllocationRecord? m_allocationRecord;

<<<<<<< HEAD
=======
		private int m_globalCount;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string Name
		{
			get
			{
				if (m_buffer != null)
				{
					return m_buffer.DebugName;
				}
				return "<Undefined>";
			}
		}

		public Vector3I Size3 => new Vector3I(m_elementCount, 1, 1);

		public Vector2I Size => new Vector2I(m_elementCount, 1);

		public SharpDX.Direct3D11.Resource Resource => m_buffer;

		public int ByteSize => m_elementCount * Math.Max(1, Description.StructureByteStride);

		public int ElementCount => m_elementCount;

		public BufferDescription Description => m_description;

		public SharpDX.Direct3D11.Buffer Buffer => m_buffer;

		public bool IsGlobal { get; protected set; }

		private void LogCreateBuffer(string name, ref BufferDescription description, Exception e)
		{
			MyLog.Default.Error("Error creating buffer: {0}\nName: {1}\nDebug name: {2}\nBufferDescription: [\n", e.ToString(), GetType().Name, Name);
			MyLog.Default.IncreaseIndent();
			MyLog.Default.Error("BindFlags = {0}", description.BindFlags);
			MyLog.Default.Error("CpuAccessFlags = {0}", description.CpuAccessFlags);
			MyLog.Default.Error("OptionFlags = {0}", description.OptionFlags);
			MyLog.Default.Error("SizeInBytes = {0}", description.SizeInBytes);
			MyLog.Default.Error("StructureByteStride = {0}", description.StructureByteStride);
			MyLog.Default.Error("SizeInBytes = {0}", description.SizeInBytes);
			MyLog.Default.Error("Usage = {0}", description.Usage);
			MyLog.Default.DecreaseIndent();
			MyLog.Default.Error("]");
			MyLog.Default.Flush();
			MyRender11.Log.Log(MyLogSeverity.Error, "Error creating buffer: {0}\nName: {1}\nDebug name: {2}\nBufferDescription: [\n", e.ToString(), GetType().Name, Name);
			MyRender11.Log.IncreaseIndent();
			MyRender11.Log.Log(MyLogSeverity.Error, "BindFlags = {0}", description.BindFlags);
			MyRender11.Log.Log(MyLogSeverity.Error, "CpuAccessFlags = {0}", description.CpuAccessFlags);
			MyRender11.Log.Log(MyLogSeverity.Error, "OptionFlags = {0}", description.OptionFlags);
			MyRender11.Log.Log(MyLogSeverity.Error, "SizeInBytes = {0}", description.SizeInBytes);
			MyRender11.Log.Log(MyLogSeverity.Error, "StructureByteStride = {0}", description.StructureByteStride);
			MyRender11.Log.Log(MyLogSeverity.Error, "SizeInBytes = {0}", description.SizeInBytes);
			MyRender11.Log.Log(MyLogSeverity.Error, "Usage = {0}", description.Usage);
			MyRender11.Log.DecreaseIndent();
			MyRender11.Log.Log(MyLogSeverity.Error, "]");
			MyRender11.Log.Flush();
		}

		internal void Init(string name, ref BufferDescription description, IntPtr? initData, MyMemorySystem memorySystem, bool isGlobal = false)
		{
			m_description = description;
			m_elementCount = description.SizeInBytes / Math.Max(1, Description.StructureByteStride);
			IsGlobal = isGlobal;
			try
			{
				m_buffer = new SharpDX.Direct3D11.Buffer(MyRender11.DeviceInstance, initData ?? ((IntPtr)0), description)
				{
					DebugName = name
				};
			}
			catch (SharpDXException e)
			{
				if (description.SizeInBytes == 0)
				{
					MyRenderProxy.Log.WriteLine("Error requesting for buffer with zero size");
				}
				MyRenderProxy.Log.WriteLine("Error during allocation of a directX buffer!");
				LogStuff(e);
				LogCreateBuffer(name, ref description, e);
				throw;
			}
			catch (Exception e2)
			{
				LogCreateBuffer(name, ref description, e2);
				throw;
			}
			try
			{
				AfterBufferInit();
			}
			catch (SharpDXException e3)
			{
				MyRenderProxy.Log.WriteLine("Error during creating a view or an unordered access to a directX buffer!");
				LogStuff(e3);
				throw;
			}
			RegisterToMemorySystem(memorySystem);
			IsReleased = false;
		}

		private void RegisterToMemorySystem(MyMemorySystem memorySystem)
		{
			m_allocationRecord = memorySystem.RegisterAllocation(Name, ByteSize);
		}

		protected virtual void AfterBufferInit()
		{
		}

		public virtual void DisposeInternal()
		{
			IsReleased = true;
			m_elementCount = 0;
			m_description = default(BufferDescription);
			m_buffer?.Dispose();
			m_buffer = null;
			m_allocationRecord?.Dispose();
			m_allocationRecord = null;
		}

		private void LogStuff(SharpDXException e)
		{
			if (MyRenderProxy.Log != null)
			{
				MyRenderProxy.Log.WriteLine("Reason: " + e.Message.Trim());
				if (e.Descriptor == SharpDX.DXGI.ResultCode.DeviceRemoved)
				{
					MyRenderProxy.Log.WriteLine("Reason: " + MyRender11.DeviceInstance.DeviceRemovedReason);
				}
				MyRenderProxy.Log.WriteMemoryUsage("");
				MyRenderProxy.Log.WriteLine("Buffer type name: " + GetType().Name);
				MyRenderProxy.Log.WriteLine("Buffer debug name: " + Name);
				MyRenderProxy.Log.WriteLine("Buffer description:\n" + BufferDescriptionToString());
				MyRenderProxy.Log.WriteLine("Exception stack trace: " + e.StackTrace);
			}
		}

		private string BufferDescriptionToString()
		{
			return $"ByteSize:\t{Description.SizeInBytes}\nByteStride:\t{Description.StructureByteStride}\nUsage:\t{Description.Usage}\nBindFlags:\t{Description.BindFlags}\nCpuAccesFlage:\t{Description.CpuAccessFlags}\nOptionsFlags:\t{Description.OptionFlags}";
		}
	}
}
