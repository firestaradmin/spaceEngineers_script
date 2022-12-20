using System;
using System.Threading;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;
using VRageMath;

namespace VRage.Render11.Resources.Textures
{
	[GenerateActivator]
	internal abstract class MyBorrowedTexture : IBorrowedSrvTexture, ISrvTexture, ISrvBindable, IResource, ITexture
	{
		private int m_numRefs;

		public MyBorrowedTextureKey Key { get; private set; }

		public string LastUsedDebugName { get; private set; }

		public int LastUsedInFrameNum { get; private set; }

		public bool IsBorrowed => m_numRefs > 0;

		public string Name => LastUsedDebugName;

		public abstract ShaderResourceView Srv { get; }

		public abstract SharpDX.Direct3D11.Resource Resource { get; }

		public abstract Vector3I Size3 { get; }

		public abstract Vector2I Size { get; }

		public Format Format => Key.Format;

		public int MipLevels => 1;

		public event Action<ITexture> OnFormatChanged;

		public void AddRef()
		{
			Interlocked.Increment(ref m_numRefs);
		}

		public void Release()
		{
			Interlocked.Decrement(ref m_numRefs);
		}

		public void Create(MyBorrowedTextureKey key, string debugName)
		{
			m_numRefs = 0;
			CreateTextureInternal(ref key, debugName);
			LastUsedDebugName = "None";
			LastUsedInFrameNum = 0;
			Key = key;
		}

		public void SetBorrowed(string name, int currentFrameNum)
		{
			LastUsedDebugName = name;
			LastUsedInFrameNum = currentFrameNum;
			m_numRefs = 1;
		}

		protected abstract void CreateTextureInternal(ref MyBorrowedTextureKey key, string debugName);
	}
}
