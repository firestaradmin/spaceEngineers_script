using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
<<<<<<< HEAD
=======
using Sandbox.Engine.Voxels;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Engine.Voxels.Planet;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Render.Image;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyHeightMapLoadingSystem : MySessionComponentBase
	{
		private ConcurrentDictionary<string, MyHeightCubemap> m_heightMaps;

		private ConcurrentDictionary<string, MyCubemap[]> m_planetMaps;

		private ConcurrentDictionary<string, MyTileTexture<byte>> m_ditherTilesets;

		private Dictionary<string, int> m_heightMapCounter;

		private Dictionary<string, int> m_planetMapCounter;

		private Dictionary<string, int> m_tilesetMapCounter;

		public static MyHeightMapLoadingSystem Static;

		public override void LoadData()
		{
			base.LoadData();
			m_heightMaps = new ConcurrentDictionary<string, MyHeightCubemap>();
			m_planetMaps = new ConcurrentDictionary<string, MyCubemap[]>();
			m_ditherTilesets = new ConcurrentDictionary<string, MyTileTexture<byte>>();
			m_heightMapCounter = new Dictionary<string, int>();
			m_planetMapCounter = new Dictionary<string, int>();
			m_tilesetMapCounter = new Dictionary<string, int>();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			foreach (MyHeightCubemap value in m_heightMaps.get_Values())
			{
				value.Dispose();
			}
			foreach (MyCubemap[] value2 in m_planetMaps.get_Values())
			{
				for (int i = 0; i < value2.Length; i++)
				{
					value2[i]?.Dispose();
				}
			}
			foreach (MyTileTexture<byte> value3 in m_ditherTilesets.get_Values())
			{
				value3.Dispose();
			}
			m_heightMaps = null;
			m_planetMaps = null;
			m_ditherTilesets = null;
			Static = null;
		}

		private void Retain(string path, Dictionary<string, int> counter)
		{
			lock (counter)
			{
				counter.TryGetValue(path, out var value);
				value = (counter[path] = value + 1);
			}
		}

		private void Release<T>(string path, Dictionary<string, int> counter, ConcurrentDictionary<string, T> maps, Action<T> dispose)
		{
			lock (counter)
			{
				counter.TryGetValue(path, out var value);
				if ((counter[path] = value - 1) == 0)
				{
					counter.Remove(path);
<<<<<<< HEAD
					maps.TryRemove(path, out var value2);
					if (value2 != null)
					{
						dispose(value2);
=======
					T val = default(T);
					maps.TryRemove(path, ref val);
					if (val != null)
					{
						dispose(val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public bool TryGet(string path, out MyHeightCubemap heightmap)
		{
			return m_heightMaps.TryGetValue(path, ref heightmap);
		}

		public void RetainHeightMap(string path)
		{
			Retain(path, m_heightMapCounter);
		}

		public void ReleaseHeightMap(string path)
		{
			Release(path, m_heightMapCounter, m_heightMaps, delegate(MyHeightCubemap m)
			{
				m.Dispose();
			});
		}

		public void RetainHeightMap(string path)
		{
			Retain(path, m_heightMapCounter);
		}

		public void ReleaseHeightMap(string path)
		{
			Release(path, m_heightMapCounter, m_heightMaps, delegate(MyHeightCubemap m)
			{
				m.Dispose();
			});
		}

		public bool TryGet(string path, out MyCubemap[] materialMaps)
		{
			return m_planetMaps.TryGetValue(path, ref materialMaps);
		}

		public void RetainPlanetMap(string path)
		{
			Retain(path, m_planetMapCounter);
		}

		public void ReleasePlanetMap(string path)
		{
			Release(path, m_planetMapCounter, m_planetMaps, delegate(MyCubemap[] mm)
			{
				for (int i = 0; i < mm.Length; i++)
				{
					mm[i]?.Dispose();
				}
			});
		}

		public void RetainPlanetMap(string path)
		{
			Retain(path, m_planetMapCounter);
		}

		public void ReleasePlanetMap(string path)
		{
			Release(path, m_planetMapCounter, m_planetMaps, delegate(MyCubemap[] mm)
			{
				for (int i = 0; i < mm.Length; i++)
				{
					mm[i]?.Dispose();
				}
			});
		}

		public bool TryGet(string path, out MyTileTexture<byte> tilemap)
		{
			return m_ditherTilesets.TryGetValue(path, ref tilemap);
		}

		public void RetainTilesetMap(string path)
		{
			Retain(path, m_tilesetMapCounter);
		}

		public void ReleaseTilesetMap(string path)
		{
			Release(path, m_tilesetMapCounter, m_ditherTilesets, delegate(MyTileTexture<byte> m)
			{
				m.Dispose();
			});
		}

		public void RetainTilesetMap(string path)
		{
			Retain(path, m_tilesetMapCounter);
		}

		public void ReleaseTilesetMap(string path)
		{
			Release(path, m_tilesetMapCounter, m_ditherTilesets, delegate(MyTileTexture<byte> m)
			{
				m.Dispose();
			});
		}

		public void Cache(string path, ref MyHeightCubemap heightmap)
		{
			MyHeightCubemap orAdd = m_heightMaps.GetOrAdd(path, heightmap);
			if (orAdd != heightmap)
			{
				heightmap.Dispose();
				heightmap = orAdd;
			}
		}

		public void Cache(string path, ref MyCubemap[] materialMaps)
		{
			MyCubemap[] orAdd = m_planetMaps.GetOrAdd(path, materialMaps);
			if (orAdd != materialMaps)
			{
				MyCubemap[] array = materialMaps;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]?.Dispose();
				}
				materialMaps = orAdd;
			}
		}

		private void Cache(string path, ref MyTileTexture<byte> tilemap)
		{
			MyTileTexture<byte> orAdd = m_ditherTilesets.GetOrAdd(path, tilemap);
			if (orAdd != tilemap)
			{
				tilemap.Dispose();
				tilemap = orAdd;
			}
		}

		public MyTileTexture<byte> GetTerrainBlendTexture(MyPlanetMaterialBlendSettings settings)
		{
			string texture = settings.Texture;
			int cellSize = settings.CellSize;
			if (!TryGet(texture, out MyTileTexture<byte> tilemap))
			{
				string path = Path.Combine(MyFileSystem.ContentPath, texture) + ".png";
				IMyImage myImage = null;
				try
				{
					myImage = MyImage.Load(path, oneChannel: true);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex.Message);
				}
				if (myImage == null || myImage.BitsPerPixel != 8)
				{
					MyLog.Default.WriteLine("Only 8bit texture supported for terrain");
					return MyTileTexture<byte>.Default;
				}
				tilemap = new MyTileTexture<byte>(myImage.Size, myImage.Stride, ((IMyImage<byte>)myImage).Data, cellSize);
				Cache(texture, ref tilemap);
			}
			return tilemap;
		}
	}
}
