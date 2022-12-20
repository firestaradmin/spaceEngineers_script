using System.Collections.Generic;
using System.Text;
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Tools;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal class MyBorrowedRwTextureManager : IManager, IManagerFrameEnd, IManagerUnloadData, IManagerDevice
	{
		private int m_currentFrameNum;

		private readonly MyObjectsPool<MyBorrowedRtvTexture> m_objectPoolRtv = new MyObjectsPool<MyBorrowedRtvTexture>(16);

		private readonly Dictionary<MyBorrowedTextureKey, List<MyBorrowedRtvTexture>> m_dictionaryRtvTextures = new Dictionary<MyBorrowedTextureKey, List<MyBorrowedRtvTexture>>();

		private readonly MyObjectsPool<MyBorrowedUavTexture> m_objectPoolUav = new MyObjectsPool<MyBorrowedUavTexture>(16);

		private readonly Dictionary<MyBorrowedTextureKey, List<MyBorrowedUavTexture>> m_dictionaryUavTextures = new Dictionary<MyBorrowedTextureKey, List<MyBorrowedUavTexture>>();

		private readonly MyObjectsPool<MyBorrowedCustomTexture> m_objectPoolCustom = new MyObjectsPool<MyBorrowedCustomTexture>(16);

		private readonly Dictionary<MyBorrowedTextureKey, List<MyBorrowedCustomTexture>> m_dictionaryCustomTextures = new Dictionary<MyBorrowedTextureKey, List<MyBorrowedCustomTexture>>();

		private readonly MyObjectsPool<MyBorrowedDepthStencilTexture> m_objectPoolDepthStencil = new MyObjectsPool<MyBorrowedDepthStencilTexture>(16);

		private readonly Dictionary<MyBorrowedTextureKey, List<MyBorrowedDepthStencilTexture>> m_dictionaryDepthStencilTextures = new Dictionary<MyBorrowedTextureKey, List<MyBorrowedDepthStencilTexture>>();

		private readonly List<MyBorrowedRtvTexture> m_tmpBorrowedRtvTextures = new List<MyBorrowedRtvTexture>();

		private readonly List<MyBorrowedUavTexture> m_tmpBorrowedUavTextures = new List<MyBorrowedUavTexture>();

		private readonly List<MyBorrowedCustomTexture> m_tmpBorrowedCustomTextures = new List<MyBorrowedCustomTexture>();

		private readonly List<MyBorrowedDepthStencilTexture> m_tmpBorrowedDepthStencilTextures = new List<MyBorrowedDepthStencilTexture>();

		protected void AddNewRtvsList(MyBorrowedTextureKey key)
		{
			lock (m_dictionaryRtvTextures)
			{
				m_dictionaryRtvTextures.Add(key, new List<MyBorrowedRtvTexture>());
			}
		}

		protected MyBorrowedRtvTexture CreateRtv(string debugName, MyBorrowedTextureKey key)
		{
			lock (m_dictionaryRtvTextures)
			{
				m_objectPoolRtv.AllocateOrCreate(out var item);
				item.Create(key, debugName);
				m_dictionaryRtvTextures[key].Add(item);
				return item;
			}
		}

		protected MyBorrowedUavTexture CreateUav(string debugName, MyBorrowedTextureKey key)
		{
			lock (m_dictionaryUavTextures)
			{
				m_objectPoolUav.AllocateOrCreate(out var item);
				item.Create(key, debugName);
				m_dictionaryUavTextures[key].Add(item);
				return item;
			}
		}

		protected MyBorrowedCustomTexture CreateCustom(string debugName, MyBorrowedTextureKey key)
		{
			m_objectPoolCustom.AllocateOrCreate(out var item);
			item.Create(key, debugName);
			m_dictionaryCustomTextures[key].Add(item);
			return item;
		}

		protected MyBorrowedDepthStencilTexture CreateDepthStencil(string debugName, MyBorrowedTextureKey key)
		{
			lock (m_dictionaryDepthStencilTextures)
			{
				m_objectPoolDepthStencil.AllocateOrCreate(out var item);
				item.Create(key, debugName);
				m_dictionaryDepthStencilTextures[key].Add(item);
				return item;
			}
		}

		public IBorrowedRtvTexture BorrowRtv(string debugName, Format format, int samplesCount = 1, int samplesQuality = 0)
		{
			return BorrowRtv(debugName, MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y, format, samplesCount, samplesQuality);
		}

		public IBorrowedRtvTexture BorrowRtv(string debugName, int width, int height, Format format, int samplesCount = 1, int samplesQuality = 0)
		{
			MyBorrowedTextureKey myBorrowedTextureKey = default(MyBorrowedTextureKey);
			myBorrowedTextureKey.Width = width;
			myBorrowedTextureKey.Height = height;
			myBorrowedTextureKey.Format = format;
			myBorrowedTextureKey.SamplesCount = samplesCount;
			myBorrowedTextureKey.SamplesQuality = samplesQuality;
			MyBorrowedTextureKey key = myBorrowedTextureKey;
			lock (m_dictionaryRtvTextures)
			{
				if (!m_dictionaryRtvTextures.ContainsKey(key))
				{
					AddNewRtvsList(key);
				}
				foreach (MyBorrowedRtvTexture item in m_dictionaryRtvTextures[key])
				{
					if (!item.IsBorrowed)
					{
						item.SetBorrowed(debugName, m_currentFrameNum);
						return item;
					}
				}
				MyBorrowedRtvTexture myBorrowedRtvTexture = CreateRtv(debugName, key);
				myBorrowedRtvTexture.SetBorrowed(debugName, m_currentFrameNum);
				return myBorrowedRtvTexture;
			}
		}

		public IBorrowedUavTexture BorrowUav(string debugName, Format format, int samplesCount = 1, int samplesQuality = 0)
		{
			return BorrowUav(debugName, MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y, format, samplesCount, samplesQuality);
		}

		public IBorrowedUavTexture BorrowUav(string debugName, int width, int height, Format format, int samplesCount = 1, int samplesQuality = 0)
		{
			MyBorrowedTextureKey myBorrowedTextureKey = default(MyBorrowedTextureKey);
			myBorrowedTextureKey.Width = width;
			myBorrowedTextureKey.Height = height;
			myBorrowedTextureKey.Format = format;
			myBorrowedTextureKey.SamplesCount = samplesCount;
			myBorrowedTextureKey.SamplesQuality = samplesQuality;
			MyBorrowedTextureKey key = myBorrowedTextureKey;
			lock (m_dictionaryUavTextures)
			{
				if (!m_dictionaryUavTextures.ContainsKey(key))
				{
					m_dictionaryUavTextures.Add(key, new List<MyBorrowedUavTexture>());
				}
				foreach (MyBorrowedUavTexture item in m_dictionaryUavTextures[key])
				{
					if (!item.IsBorrowed)
					{
						item.SetBorrowed(debugName, m_currentFrameNum);
						return item;
					}
				}
				MyBorrowedUavTexture myBorrowedUavTexture = CreateUav(debugName, key);
				myBorrowedUavTexture.SetBorrowed(debugName, m_currentFrameNum);
				return myBorrowedUavTexture;
			}
		}

		public IBorrowedCustomTexture BorrowCustom(string debugName, int samplesCount = 1, int samplesQuality = 0)
		{
			return BorrowCustom(debugName, MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y, samplesCount, samplesQuality);
		}

		public IBorrowedCustomTexture BorrowCustom(string debugName, int width, int height, int samplesCount = 1, int samplesQuality = 0)
		{
			MyBorrowedTextureKey myBorrowedTextureKey = default(MyBorrowedTextureKey);
			myBorrowedTextureKey.Width = width;
			myBorrowedTextureKey.Height = height;
			myBorrowedTextureKey.Format = Format.Unknown;
			myBorrowedTextureKey.SamplesCount = samplesCount;
			myBorrowedTextureKey.SamplesQuality = samplesQuality;
			MyBorrowedTextureKey key = myBorrowedTextureKey;
			if (!m_dictionaryCustomTextures.ContainsKey(key))
			{
				m_dictionaryCustomTextures.Add(key, new List<MyBorrowedCustomTexture>());
			}
			foreach (MyBorrowedCustomTexture item in m_dictionaryCustomTextures[key])
			{
				if (!item.IsBorrowed)
				{
					item.SetBorrowed(debugName, m_currentFrameNum);
					return item;
				}
			}
			MyBorrowedCustomTexture myBorrowedCustomTexture = CreateCustom(debugName, key);
			myBorrowedCustomTexture.SetBorrowed(debugName, m_currentFrameNum);
			return myBorrowedCustomTexture;
		}

		public IBorrowedDepthStencilTexture BorrowDepthStencil(string debugName, int width, int height, bool hqDepth, int samplesCount = 1, int samplesQuality = 0)
		{
			lock (m_dictionaryDepthStencilTextures)
			{
				MyBorrowedTextureKey myBorrowedTextureKey = default(MyBorrowedTextureKey);
				myBorrowedTextureKey.Width = width;
				myBorrowedTextureKey.Height = height;
				myBorrowedTextureKey.Format = Format.Unknown;
				myBorrowedTextureKey.HqDepth = hqDepth;
				myBorrowedTextureKey.SamplesCount = samplesCount;
				myBorrowedTextureKey.SamplesQuality = samplesQuality;
				MyBorrowedTextureKey key = myBorrowedTextureKey;
				if (!m_dictionaryDepthStencilTextures.ContainsKey(key))
				{
					m_dictionaryDepthStencilTextures.Add(key, new List<MyBorrowedDepthStencilTexture>());
				}
				foreach (MyBorrowedDepthStencilTexture item in m_dictionaryDepthStencilTextures[key])
				{
					if (!item.IsBorrowed)
					{
						item.SetBorrowed(debugName, m_currentFrameNum);
						return item;
					}
				}
				MyBorrowedDepthStencilTexture myBorrowedDepthStencilTexture = CreateDepthStencil(debugName, key);
				myBorrowedDepthStencilTexture.SetBorrowed(debugName, m_currentFrameNum);
				return myBorrowedDepthStencilTexture;
			}
		}

		private void DisposeTexture(MyBorrowedRtvTexture rtv)
		{
			lock (m_dictionaryRtvTextures)
			{
				IRtvTexture texture = rtv.RtvTexture;
				MyManagers.RwTextures.DisposeTex(ref texture);
				MyBorrowedTextureKey key = rtv.Key;
				m_dictionaryRtvTextures[key].Remove(rtv);
				m_objectPoolRtv.Deallocate(rtv);
			}
		}

		private void DisposeTexture(MyBorrowedUavTexture uav)
		{
			lock (m_dictionaryUavTextures)
			{
				IUavTexture texture = uav.UavTexture;
				MyManagers.RwTextures.DisposeTex(ref texture);
				MyBorrowedTextureKey key = uav.Key;
				m_dictionaryUavTextures[key].Remove(uav);
				m_objectPoolUav.Deallocate(uav);
			}
		}

		private void DisposeTexture(MyBorrowedCustomTexture custom)
		{
			ICustomTexture texture = custom.CustomTexture;
			MyManagers.CustomTextures.DisposeTex(ref texture);
			MyBorrowedTextureKey key = custom.Key;
			m_dictionaryCustomTextures[key].Remove(custom);
			m_objectPoolCustom.Deallocate(custom);
		}

		private void DisposeTexture(MyBorrowedDepthStencilTexture depthStencil)
		{
			lock (m_dictionaryDepthStencilTextures)
			{
				IDepthStencil tex = depthStencil.DepthStencilTexture;
				MyManagers.DepthStencils.DisposeTex(ref tex);
				MyBorrowedTextureKey key = depthStencil.Key;
				m_dictionaryDepthStencilTextures[key].Remove(depthStencil);
				m_objectPoolDepthStencil.Deallocate(depthStencil);
			}
		}

		public bool IsAnyTextureBorrowed()
		{
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedUavTexture>> dictionaryUavTexture in m_dictionaryUavTextures)
			{
				foreach (MyBorrowedUavTexture item in dictionaryUavTexture.Value)
				{
					if (item.IsBorrowed)
					{
						return true;
					}
				}
			}
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedRtvTexture>> dictionaryRtvTexture in m_dictionaryRtvTextures)
			{
				foreach (MyBorrowedRtvTexture item2 in dictionaryRtvTexture.Value)
				{
					if (item2.IsBorrowed)
					{
						return true;
					}
				}
			}
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedCustomTexture>> dictionaryCustomTexture in m_dictionaryCustomTextures)
			{
				foreach (MyBorrowedCustomTexture item3 in dictionaryCustomTexture.Value)
				{
					if (item3.IsBorrowed)
					{
						return true;
					}
				}
			}
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedDepthStencilTexture>> dictionaryDepthStencilTexture in m_dictionaryDepthStencilTextures)
			{
				foreach (MyBorrowedDepthStencilTexture item4 in dictionaryDepthStencilTexture.Value)
				{
					if (item4.IsBorrowed)
					{
						return true;
					}
				}
			}
			return false;
		}

		protected IEnumerable<MyBorrowedTexture> GetBorrowedTextures()
		{
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedUavTexture>> dictionaryUavTexture in m_dictionaryUavTextures)
			{
				foreach (MyBorrowedUavTexture item in dictionaryUavTexture.Value)
				{
					if (item.IsBorrowed)
					{
						yield return item;
					}
				}
			}
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedRtvTexture>> dictionaryRtvTexture in m_dictionaryRtvTextures)
			{
				foreach (MyBorrowedRtvTexture item2 in dictionaryRtvTexture.Value)
				{
					if (item2.IsBorrowed)
					{
						yield return item2;
					}
				}
			}
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedCustomTexture>> dictionaryCustomTexture in m_dictionaryCustomTextures)
			{
				foreach (MyBorrowedCustomTexture item3 in dictionaryCustomTexture.Value)
				{
					if (item3.IsBorrowed)
					{
						yield return item3;
					}
				}
			}
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedDepthStencilTexture>> dictionaryDepthStencilTexture in m_dictionaryDepthStencilTextures)
			{
				foreach (MyBorrowedDepthStencilTexture item4 in dictionaryDepthStencilTexture.Value)
				{
					if (item4.IsBorrowed)
					{
						yield return item4;
					}
				}
			}
		}

		private void CleanUp(int numFramesToPreserveTexture)
		{
			if (IsAnyTextureBorrowed())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (MyBorrowedTexture borrowedTexture in GetBorrowedTextures())
				{
					stringBuilder.AppendFormat("{0}: {1}x{2} {3};  ", borrowedTexture.Name, borrowedTexture.Key.Width, borrowedTexture.Key.Height, borrowedTexture.Key.Format);
					while (borrowedTexture.IsBorrowed)
					{
						borrowedTexture.Release();
					}
				}
			}
			ClearHelper(m_dictionaryUavTextures, m_tmpBorrowedUavTextures, numFramesToPreserveTexture);
			foreach (MyBorrowedUavTexture tmpBorrowedUavTexture in m_tmpBorrowedUavTextures)
			{
				DisposeTexture(tmpBorrowedUavTexture);
			}
			m_tmpBorrowedUavTextures.Clear();
			ClearHelper(m_dictionaryRtvTextures, m_tmpBorrowedRtvTextures, numFramesToPreserveTexture);
			foreach (MyBorrowedRtvTexture tmpBorrowedRtvTexture in m_tmpBorrowedRtvTextures)
			{
				DisposeTexture(tmpBorrowedRtvTexture);
			}
			m_tmpBorrowedRtvTextures.Clear();
			ClearHelper(m_dictionaryCustomTextures, m_tmpBorrowedCustomTextures, numFramesToPreserveTexture);
			foreach (MyBorrowedCustomTexture tmpBorrowedCustomTexture in m_tmpBorrowedCustomTextures)
			{
				DisposeTexture(tmpBorrowedCustomTexture);
			}
			m_tmpBorrowedCustomTextures.Clear();
			ClearHelper(m_dictionaryDepthStencilTextures, m_tmpBorrowedDepthStencilTextures, numFramesToPreserveTexture);
			foreach (MyBorrowedDepthStencilTexture tmpBorrowedDepthStencilTexture in m_tmpBorrowedDepthStencilTextures)
			{
				DisposeTexture(tmpBorrowedDepthStencilTexture);
			}
			m_tmpBorrowedDepthStencilTextures.Clear();
			m_currentFrameNum++;
		}

		private void ClearHelper<T>(Dictionary<MyBorrowedTextureKey, List<T>> dict, List<T> tmpList, int numFramesToPreserveTexture) where T : MyBorrowedTexture
		{
			foreach (KeyValuePair<MyBorrowedTextureKey, List<T>> item in dict)
			{
				foreach (T item2 in item.Value)
				{
					if (!item2.IsBorrowed && item2.LastUsedInFrameNum + numFramesToPreserveTexture < m_currentFrameNum)
					{
						tmpList.Add(item2);
					}
				}
			}
		}

		public void UpdateStats(string page)
		{
			string group = "Rtv pooling";
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedRtvTexture>> dictionaryRtvTexture in m_dictionaryRtvTextures)
			{
				if (dictionaryRtvTexture.Value.Count > 0)
				{
					MyStatsDisplay.Write(group, dictionaryRtvTexture.Key.ToString(), dictionaryRtvTexture.Value.Count, page);
				}
			}
			group = "Uav pooling";
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedUavTexture>> dictionaryUavTexture in m_dictionaryUavTextures)
			{
				if (dictionaryUavTexture.Value.Count > 0)
				{
					MyStatsDisplay.Write(group, dictionaryUavTexture.Key.ToString(), dictionaryUavTexture.Value.Count, page);
				}
			}
			group = "Custom pooling";
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedCustomTexture>> dictionaryCustomTexture in m_dictionaryCustomTextures)
			{
				MyStatsDisplay.Write(group, dictionaryCustomTexture.Key.ToString(), dictionaryCustomTexture.Value.Count, page);
			}
			group = "DepthStencil pooling";
			foreach (KeyValuePair<MyBorrowedTextureKey, List<MyBorrowedDepthStencilTexture>> dictionaryDepthStencilTexture in m_dictionaryDepthStencilTextures)
			{
				MyStatsDisplay.Write(group, dictionaryDepthStencilTexture.Key.ToString(), dictionaryDepthStencilTexture.Value.Count, page);
			}
		}

		void IManagerFrameEnd.OnFrameEnd()
		{
			CleanUp(MyRender11.Settings.RwTexturePool_FramesToPreserveTextures);
		}

		void IManagerUnloadData.OnUnloadData()
		{
		}

		public void OnDeviceInit()
		{
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
			CleanUp(0);
		}
	}
}
