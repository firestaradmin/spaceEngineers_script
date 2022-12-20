using System;
using SharpDX;
using SharpDX.Direct3D11;
using VRage.Library;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal struct MyMapping
	{
		private MyRenderContext m_rc;

		private Resource m_resource;

		private int m_bufferSize;

		private DataBox m_dataBox;

		private IntPtr m_dataPointer;

		internal static MyMapping MapDiscard(IBuffer buffer)
		{
			return MapDiscard(MyRender11.RC, buffer);
		}

		internal static MyMapping MapDiscard(MyRenderContext rc, IBuffer buffer)
		{
			return Map(rc, buffer, buffer.Description.SizeInBytes, MapMode.WriteDiscard);
		}

		internal static MyMapping MapDiscard(IResource resource)
		{
			MyMapping result = Map(MyRender11.RC, resource, 0, MapMode.WriteDiscard);
			if (result.m_dataBox.SlicePitch != 0)
			{
				result.m_bufferSize = result.m_dataBox.SlicePitch;
			}
			else
			{
				result.m_bufferSize = result.m_dataBox.RowPitch * resource.Size.Y;
			}
			return result;
		}

		internal static MyMapping MapNoOverwrite(IBuffer buffer)
		{
			return MapNoOverwrite(MyRender11.RC, buffer);
		}

		internal static MyMapping MapNoOverwrite(MyRenderContext rc, IBuffer buffer)
		{
			return Map(rc, buffer, buffer.Description.SizeInBytes, MapMode.WriteNoOverwrite);
		}

		internal static MyMapping MapNoOverwrite(IResource resource)
		{
			MyMapping result = Map(MyRender11.RC, resource, 0, MapMode.WriteNoOverwrite);
			if (result.m_dataBox.SlicePitch != 0)
			{
				result.m_bufferSize = result.m_dataBox.SlicePitch;
			}
			else
			{
				result.m_bufferSize = result.m_dataBox.RowPitch * resource.Size.Y;
			}
			return result;
		}

		internal static MyMapping Map(MyRenderContext rc, IResource resource, int bufferSize, MapMode mapMode)
		{
			MyMapping result = default(MyMapping);
			result.m_rc = rc;
			result.m_resource = resource.Resource;
			result.m_bufferSize = bufferSize;
			result.m_dataBox = rc.MapSubresource(resource, 0, mapMode, MapFlags.None);
			if (result.m_dataBox.IsEmpty)
			{
				throw new MyRenderException("Resource mapping failed!");
			}
			result.m_dataPointer = result.m_dataBox.DataPointer;
			return result;
		}

		internal static MyMapping MapRead(IResource resource)
		{
			MyMapping result = Map(MyRender11.RC, resource, 0, MapMode.Read);
			if (result.m_dataBox.SlicePitch != 0)
			{
				result.m_bufferSize = result.m_dataBox.SlicePitch;
			}
			return result;
		}

		internal static MyMapping MapRead(MyRenderContext rc, IResource resource)
		{
			MyMapping result = Map(rc, resource, 0, MapMode.Read);
			if (result.m_dataBox.SlicePitch != 0)
			{
				result.m_bufferSize = result.m_dataBox.SlicePitch;
			}
			return result;
		}

		internal void ReadAndPosition<T>(ref T data) where T : struct
		{
			m_dataPointer = Utilities.ReadAndPosition(m_dataPointer, ref data);
		}

		internal void ReadAndPosition<T>(T[] data, int count, int offset = 0) where T : struct
		{
			m_dataPointer = Utilities.Read(m_dataPointer, data, offset, count);
		}

		internal void WriteAndPosition<T>(ref T data) where T : struct
		{
			m_dataPointer = Utilities.WriteAndPosition(m_dataPointer, ref data);
		}

		internal void WriteAndPosition<T>(T[] data, int count, int offset = 0) where T : struct
		{
			m_dataPointer = Utilities.Write(m_dataPointer, data, offset, count);
		}

		internal void WriteAndPosition(NativeArray data, int bytesCount, int offset = 0)
		{
			Utilities.CopyMemory(m_dataPointer, data.Ptr + offset, bytesCount);
			m_dataPointer += bytesCount;
		}

		internal void WriteAndPositionByRow<T>(T[] data, int count, int offset = 0) where T : struct
		{
			Utilities.Write(m_dataPointer, data, offset, count);
			m_dataPointer += m_dataBox.RowPitch;
		}

		internal void Offset(int bytes)
		{
			m_dataPointer += bytes;
		}

		internal void Position(int offset)
		{
			m_dataPointer = m_dataBox.DataPointer + offset;
		}

		internal void Unmap()
		{
			m_rc.UnmapSubresource(m_resource, 0);
		}
	}
}
