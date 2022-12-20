using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using ParallelTasks;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Library.Parallelization;
using VRage.Render11.Common;
using VRage.Render11.Resources.Internal;
using VRage.Render11.Scene.Resources;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Textures
{
	internal class MyTextureStreamingManager : IManagerFrameEnd, IManager
	{
		internal struct BatchTextureInfo
		{
			public int TokenId;

			public int UseCount;
		}

		public struct CompiledBatch : IDisposable
		{
			private BatchTextureInfo[] m_ids;

			private readonly int m_indicesCount;

			private readonly MyTextureStreamingManager m_context;

			public CompiledBatch(MyTextureStreamingManager context, BatchTextureInfo[] ids, int idIndicesCount)
			{
				m_ids = ids;
				m_context = context;
				m_indicesCount = idIndicesCount;
			}

			public void UpdateScenePriority(int priority)
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_context.SyncInProgress.Read("Can't update priority while sync is in progress'"))
					{
						TextureToken[] textureTokens = m_context.m_textureTokens;
						for (int i = 0; i < m_indicesCount; i++)
						{
							int tokenId = m_ids[i].TokenId;
							MyUtils.InterlockedMax(ref textureTokens[tokenId].ScenePriority, priority);
						}
					}
				}
			}

			public void RegisterUsagePriority()
			{
				AlterUsagePriority();
			}

			public void UnregisterUsagePriority()
			{
				AlterUsagePriority(isRemoval: true);
			}

			private void AlterUsagePriority(bool isRemoval = false)
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_context.SyncInProgress.Read("Can't update priority while sync is in progress'"))
					{
						TextureToken[] textureTokens = m_context.m_textureTokens;
						for (int i = 0; i < m_indicesCount; i++)
						{
							int tokenId = m_ids[i].TokenId;
							int useCount = m_ids[i].UseCount;
							Interlocked.Add(ref textureTokens[tokenId].UsagePriority, isRemoval ? (-useCount) : useCount);
						}
					}
				}
			}

			public void Dispose()
			{
				if (m_ids != null)
				{
					m_context.m_indicesPool.Return(m_ids);
					m_ids = null;
				}
			}
		}

		internal class MyStreamedTexture : MyStreamedTextureBase, IMyStreamedTexture, IMyStreamedTextureBase, IMySceneResource
		{
<<<<<<< HEAD
=======
			public new const int NullTokenId = -1;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			private Action<IMyStreamedTexture> m_onTextureHandleChanged;

			private readonly Func<bool, bool, (ITexture Texture, uint Size)> m_handleGetter;

			public ITexture Texture { get; private set; }

			public override uint Size => m_handleGetter(arg1: true, arg2: true).Item2;

			public event Action<IMyStreamedTexture> OnTextureHandleChanged
			{
				add
				{
					if (CanHandleChangeAfterCreation(Texture))
					{
						lock (m_handleGetter)
						{
							m_onTextureHandleChanged = (Action<IMyStreamedTexture>)Delegate.Combine(m_onTextureHandleChanged, value);
						}
					}
				}
				remove
				{
					lock (m_handleGetter)
					{
						m_onTextureHandleChanged = (Action<IMyStreamedTexture>)Delegate.Remove(m_onTextureHandleChanged, value);
					}
				}
			}

			internal MyStreamedTexture(MyTextureStreamingManager context, Func<bool, bool, (ITexture Texture, uint Size)> handleGetter, int decayTime)
				: base(context, decayTime)
			{
				m_handleGetter = handleGetter;
				Texture = m_handleGetter(arg1: true, arg2: true).Item1;
			}

			MyStreamedTexturePin IMyStreamedTexture.Pin()
			{
				return new MyStreamedTexturePin(this);
			}

			internal void Pin()
			{
				using (m_context.TextureUpdatesInProgress.Shared())
				{
					if (Interlocked.Increment(ref m_pinCount) == 1)
					{
						PerformStateChange(load: true);
						if (IsStreamed)
						{
							RegisterForUpdate(4);
						}
					}
				}
			}

			internal void ReleasePin()
			{
				using (m_context.TextureUpdatesInProgress.Shared())
				{
					if (Interlocked.Decrement(ref m_pinCount) == 0)
					{
						RegisterForUpdate(4);
					}
				}
			}

			public void Load(bool fastLoad = false)
			{
				if (m_framesToStateChange > 0)
				{
					return;
				}
				m_framesToStateChange = 10;
				if (fastLoad)
				{
					using (m_context.TextureTokensGuard.Read())
					{
						TextureToken textureToken = m_context.m_textureTokens[base.TextureTokenId];
						if (textureToken.ScenePriority + textureToken.TouchPriority > 500)
						{
							m_framesToStateChange = 2;
						}
					}
				}
				RegisterForUpdate(0);
			}

			public void Unload()
			{
				if (m_framesToStateChange >= 0)
				{
					m_framesToStateChange = -2;
					RegisterForUpdate(0);
				}
			}

			internal void PerformScheduledStateChangesImmediately(bool? load)
			{
				if (m_framesToStateChange != 0)
				{
					bool flag = m_framesToStateChange > 0;
					if (flag == load.GetValueOrDefault(flag))
					{
						PerformStateChange(flag);
						m_framesToStateChange = 0;
					}
				}
			}

			internal override void PerformStateChange(bool load, bool forceSyncLoad = false)
			{
				MyFileTexture myFileTexture;
				if ((myFileTexture = Texture as MyFileTexture) != null)
				{
					FileTextureState targetState = (load ? FileTextureState.Loaded : FileTextureState.Unloaded);
					WorkPriority? priority = ((myFileTexture.TextureType == MyFileTextureEnum.GUI && m_pinCount > 0) ? new WorkPriority?(WorkPriority.VeryHigh) : null);
					MyManagers.FileTextures.ChangeTextureState(myFileTexture, targetState, forceSyncLoad, priority);
				}
			}

			protected override void PerformStreamedStateChange(bool streamed)
			{
				if (streamed)
				{
					m_context.m_texturePrioritizer.AddStreamedTexture(this);
				}
				else
				{
					m_context.m_texturePrioritizer.RemoveStreamedTexture(this);
				}
			}

			internal void RecomputeByteSize()
			{
				using (m_context.TextureTokensGuard.Read())
				{
					int textureTokenId = base.TextureTokenId;
					if (textureTokenId != -1 && Texture != null)
					{
						uint item = m_handleGetter(arg1: true, arg2: false).Item2;
						m_context.m_textureTokens[textureTokenId].Size = item;
					}
				}
			}

			internal void UpdateTextureHandle(bool removed)
			{
				Texture = (removed ? null : m_handleGetter(arg1: false, arg2: false).Item1);
				m_onTextureHandleChanged.InvokeIfNotNull(this);
			}

			public override string ToString()
			{
				MyFileTexture myFileTexture;
				if ((myFileTexture = Texture as MyFileTexture) != null)
				{
					return Path.GetFileName(myFileTexture.Path);
				}
				return Texture.ToString();
			}

			private static bool CanHandleChangeAfterCreation(ITexture texture)
			{
				if (texture != null)
				{
					return texture is IUserGeneratedTexture;
				}
				return true;
			}
		}

		internal class MyStreamedTextureArrayTile : MyStreamedTextureBase, IMyStreamedTextureArrayTile, IMyStreamedTextureBase, IMySceneResource
		{
			private const int UNPLACED_TILE_ID = -1;

			private readonly object m_callbackLock = new object();

			private Action<IMyStreamedTextureArrayTile> m_onTextureTileHandleChanged;

			private MyTextureArrayPrioritizer m_prioritizer;

			private MyStreamedTexturePin m_prefetchTexturePin;

			public IDynamicFileArrayTexture TextureArray { get; }

			public int TileID { get; private set; }

			public string Filepath { get; }

			public bool IsLoaded { get; private set; }

			public bool IsPlaced => TileID != -1;

			public override uint Size => 1u;

			public event Action<IMyStreamedTextureArrayTile> OnTextureTileHandleChanged
			{
				add
				{
					lock (m_callbackLock)
					{
						m_onTextureTileHandleChanged = (Action<IMyStreamedTextureArrayTile>)Delegate.Combine(m_onTextureTileHandleChanged, value);
					}
				}
				remove
				{
					lock (m_callbackLock)
					{
						m_onTextureTileHandleChanged = (Action<IMyStreamedTextureArrayTile>)Delegate.Remove(m_onTextureTileHandleChanged, value);
					}
				}
			}

			internal MyStreamedTextureArrayTile(MyTextureStreamingManager context, MyTextureArrayPrioritizer prioritizer, IDynamicFileArrayTexture textureArray, string filepath, int decayTime)
				: base(context, decayTime)
			{
				TextureArray = textureArray;
				Filepath = filepath;
				m_prioritizer = prioritizer;
				TileID = -1;
				IsLoaded = false;
			}

			internal void Unplace()
			{
				TileID = -1;
				m_prefetchTexturePin.Dispose();
				IsLoaded = false;
				m_onTextureTileHandleChanged.InvokeIfNotNull(this);
			}

			internal void PlaceAt(int tileID)
			{
				TileID = tileID;
				m_prefetchTexturePin = MyManagers.Textures.GetPermanentTexture(Filepath, new QueryArgs
				{
					TextureType = TextureArray.Type,
					IsVoxel = true
				});
			}

			internal override void PerformStateChange(bool load, bool forceSyncLoad = false)
			{
			}

			protected override void PerformStreamedStateChange(bool streamed)
			{
				if (streamed)
				{
					m_prioritizer.AddStreamedTexture(this);
				}
				else
				{
					m_prioritizer.RemoveStreamedTexture(this);
				}
			}

			internal void UpdateLoadedState()
			{
				if (IsPlaced && !IsLoaded && m_prefetchTexturePin.Texture.IsTextureLoaded())
				{
					IsLoaded = true;
					m_onTextureTileHandleChanged.InvokeIfNotNull(this);
					m_prefetchTexturePin.Dispose();
				}
			}
<<<<<<< HEAD

			void IMyStreamedTextureArrayTile.ResetOnTextureTileHandleChangedEvent()
			{
				lock (m_callbackLock)
				{
					m_onTextureTileHandleChanged = null;
				}
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal abstract class MyStreamedTextureBase : IMyStreamedTextureBase, IMySceneResource
		{
			[StructLayout(LayoutKind.Sequential, Size = 1)]
			protected struct Flags
			{
				public const int REGISTERED_FOR_UPDATE = 1;

				public const int CREATE_TEXTURE_TOKEN = 2;

				public const int CHANGE_STREAMING_STATE = 4;

				public const int REMOVE_FROM_STREAMING = 8;

				public const int ALL_STATE_UPDATE_FLAGS = 14;
			}

			public const int NullTokenId = -1;

			protected readonly MyTextureStreamingManager m_context;

			private int m_priorityDecay;

			private readonly int m_decayTime;

			protected int m_pinCount;

			protected int m_framesToStateChange;

			internal bool IsStreamed;

			private AtomicFlags m_flags;

			public int TextureTokenId { get; protected set; }

			public abstract uint Size { get; }

			internal bool NeedsUpdate
			{
				get
				{
					if (!m_flags.IsSet(14) && m_priorityDecay == 0)
					{
						return m_framesToStateChange != 0;
					}
					return true;
				}
			}

			protected MyStreamedTextureBase(MyTextureStreamingManager context, int decayTime)
			{
				m_context = context;
				m_decayTime = decayTime;
				TextureTokenId = -1;
			}

			void IMyStreamedTextureBase.Touch(ushort priority)
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_context.SyncInProgress.Shared("Can't touch texture while syncing"))
					{
						bool flag = TextureTokenId != -1;
						if (!flag || AdjustTouchPriority(priority))
						{
							int num = Interlocked.Add(ref m_priorityDecay, priority);
							if (num < 0)
							{
								Interlocked.Add(ref m_priorityDecay, -priority);
							}
							else if (num - priority == 0 || !flag)
							{
								OnStreamingPriorityChangedFromZero(1);
							}
						}
					}
				}
			}

			internal void Deprioritize()
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_context.SyncInProgress.Shared("Can't release texture while syncing"))
					{
						int num = Interlocked.Exchange(ref m_priorityDecay, 0);
						if (num > 0 && TextureTokenId != -1)
						{
							Interlocked.Add(ref m_context.m_textureTokens[TextureTokenId].TouchPriority, -num);
						}
					}
				}
			}

			internal void RemoveFromStreaming()
			{
				using (m_context.SyncInProgress.Shared())
				{
					Deprioritize();
					if (IsStreamed)
					{
						RegisterForUpdate(8);
					}
				}
			}

			protected void RegisterForUpdate(int flags)
			{
				if (((uint)m_flags.Set(1 | flags) & (true ? 1u : 0u)) != 0)
				{
					List<MyStreamedTextureBase> texturesToUpdate = m_context.m_texturesToUpdate;
					lock (texturesToUpdate)
					{
						texturesToUpdate.Add(this);
					}
				}
			}

			public void OnRemovedFromUpdate()
			{
				m_flags.Clear(1);
			}

			internal void Update()
			{
				int num = m_flags.Clear(14);
				if (((uint)num & 2u) != 0)
				{
					uint size = Size;
					if (size == uint.MaxValue)
					{
						m_flags.Set(num);
						return;
					}
					TextureTokenId = m_context.GetNewToken(this, size, m_priorityDecay);
					_ = m_priorityDecay;
				}
				using (m_context.TextureTokensGuard.Read())
				{
					if (((uint)num & 8u) != 0 && m_pinCount == 0 && IsStreamed)
					{
						PerformStreamedStateChange(streamed: false);
						PerformStateChange(load: false);
					}
					if (((uint)num & 4u) != 0)
					{
						m_framesToStateChange = 0;
						if (m_pinCount == 0)
						{
							if (!IsStreamed)
							{
								if (TextureTokenId != -1)
								{
									PerformStreamedStateChange(streamed: true);
								}
								else
								{
									PerformStateChange(load: false);
								}
							}
						}
						else if (IsStreamed)
						{
							PerformStreamedStateChange(streamed: false);
						}
					}
				}
				int num2 = Volatile.Read(ref m_priorityDecay);
				if (num2 > 0)
				{
					if (num2 > m_decayTime)
					{
						num2 /= m_decayTime;
					}
					Interlocked.Add(ref m_priorityDecay, -num2);
					AdjustTouchPriority(-num2);
				}
				int num3 = Math.Sign(m_framesToStateChange);
				if (num3 != 0)
				{
					m_framesToStateChange -= num3;
					if (m_framesToStateChange == 0)
					{
						PerformStateChange(num3 == 1);
					}
				}
			}

			internal abstract void PerformStateChange(bool load, bool forceSyncLoad = false);

			protected abstract void PerformStreamedStateChange(bool streamed);

			public void OnStreamingPriorityChangedFromZero(int flags = 0)
			{
				using (m_context.SyncInProgress.Shared("Can't adjust priority while sync is in progress'"))
				{
					if (!IsStreamed || m_flags.IsSet(8))
					{
						flags |= 4;
					}
					if (TextureTokenId == -1)
					{
						flags |= 2;
					}
					if (flags != 0)
					{
						RegisterForUpdate(flags);
					}
				}
			}

			private bool AdjustTouchPriority(int diff)
			{
				int textureTokenId = TextureTokenId;
				using (m_context.TextureTokensGuard.Read())
				{
					TextureToken[] textureTokens = m_context.m_textureTokens;
					if (Interlocked.Add(ref textureTokens[textureTokenId].TouchPriority, diff) < 0 && diff > 0)
					{
						Interlocked.Add(ref textureTokens[textureTokenId].TouchPriority, -diff);
						return false;
					}
					return true;
				}
			}
		}

		internal class MyTextureArrayPrioritizer
		{
			private class PriorityCalculator
			{
				private float m_sceneWeight;

				private float m_touchWeight;

				private float m_usageWeight;

				public PriorityCalculator(float sceneWeight = 1f, float touchWeight = 1f, float usageWeight = 1f)
				{
					m_sceneWeight = sceneWeight;
					m_touchWeight = touchWeight;
					m_usageWeight = usageWeight;
				}

				public int ComputePriority(ref TextureToken token)
				{
					return (int)((float)token.ScenePriority * m_sceneWeight + (float)token.TouchPriority * m_touchWeight + (float)token.UsagePriority * m_usageWeight);
				}
			}

			private class TextureComparer : IComparer<MyStreamedTextureArrayTile>
			{
				public TextureToken[] Textures;

				public PriorityCalculator PriorityCalculator;

				int IComparer<MyStreamedTextureArrayTile>.Compare(MyStreamedTextureArrayTile a, MyStreamedTextureArrayTile b)
				{
					return PriorityCalculator.ComputePriority(ref Textures[b.TextureTokenId]).CompareTo(PriorityCalculator.ComputePriority(ref Textures[a.TextureTokenId]));
				}
			}

			private readonly IDynamicFileArrayTexture m_arrayTexture;

			private int m_usedSlices;

			private readonly MyTextureStreamingManager m_context;

			private readonly List<MyStreamedTextureArrayTile> m_streamedTiles = new List<MyStreamedTextureArrayTile>();

			private DataGuard m_streamedTilesGuard;

			private readonly TextureComparer m_textureComparer = new TextureComparer();

			private PriorityCalculator m_priorityCalculator;

			private int m_slicesTotal;

			private float m_priorityHysteresis;

			public MyTextureArrayPrioritizer(MyTextureStreamingManager context, IDynamicFileArrayTexture arrayTexture)
			{
				m_arrayTexture = arrayTexture;
				m_context = context;
				m_priorityCalculator = new PriorityCalculator(25f);
				m_slicesTotal = arrayTexture.MinSlices;
				m_priorityHysteresis = 2f;
			}

			public void UpdateStreamingPriority(bool textureQualityChange)
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_streamedTilesGuard.Exclusive())
					{
						if (textureQualityChange)
						{
							foreach (MyStreamedTextureArrayTile streamedTile in m_streamedTiles)
							{
								if (streamedTile.IsPlaced)
								{
									streamedTile.Unplace();
								}
							}
							m_usedSlices = 0;
							m_slicesTotal = m_arrayTexture.MinSlices;
						}
						int num = 0;
						int num2 = Math.Min(m_streamedTiles.Count, m_slicesTotal);
						m_textureComparer.Textures = m_context.m_textureTokens;
						m_textureComparer.PriorityCalculator = m_priorityCalculator;
						m_streamedTiles.Sort(m_textureComparer);
						int num3 = 0;
						int num4 = m_streamedTiles.Count - 1;
						while (true)
						{
							if (num3 < num2 && m_streamedTiles[num3].IsPlaced)
							{
								num3++;
								continue;
							}
							while (num4 > num2 && !m_streamedTiles[num4].IsPlaced)
							{
								num4--;
							}
							if (num3 == num2)
							{
								break;
							}
							MyStreamedTextureArrayTile myStreamedTextureArrayTile = m_streamedTiles[num3];
							if (m_usedSlices < m_slicesTotal)
							{
								int orAddSlice = m_arrayTexture.GetOrAddSlice(myStreamedTextureArrayTile.Filepath);
								myStreamedTextureArrayTile.PlaceAt(orAddSlice);
								m_usedSlices++;
								continue;
							}
							MyStreamedTextureArrayTile myStreamedTextureArrayTile2 = m_streamedTiles[num4];
							TextureToken token = m_context.m_textureTokens[myStreamedTextureArrayTile.TextureTokenId];
							TextureToken token2 = m_context.m_textureTokens[myStreamedTextureArrayTile2.TextureTokenId];
							int num5 = m_priorityCalculator.ComputePriority(ref token);
							int num6 = m_priorityCalculator.ComputePriority(ref token2);
							if (num5 > (int)((float)num6 * m_priorityHysteresis))
							{
								m_arrayTexture.SwapSlice(myStreamedTextureArrayTile2.TileID, myStreamedTextureArrayTile.Filepath);
								num++;
								myStreamedTextureArrayTile.PlaceAt(myStreamedTextureArrayTile2.TileID);
								myStreamedTextureArrayTile2.Unplace();
								if (!myStreamedTextureArrayTile2.IsStreamed)
								{
									m_streamedTiles.RemoveAtFast(num4);
								}
							}
							else
							{
								m_streamedTiles[num4] = myStreamedTextureArrayTile;
								m_streamedTiles[num3] = myStreamedTextureArrayTile2;
							}
						}
						for (int i = 0; i < m_usedSlices; i++)
						{
							m_streamedTiles[i].UpdateLoadedState();
						}
						m_context.Statistics.LoadedTiles += m_usedSlices;
						m_context.Statistics.StreamedTiles += m_streamedTiles.Count;
						m_context.Statistics.SwapsPerformed += num;
						m_context.Statistics.TotalSwapsPerformed += num;
					}
				}
			}

			public void AddStreamedTexture(MyStreamedTextureArrayTile tile)
			{
				using (m_streamedTilesGuard.Exclusive())
				{
					tile.IsStreamed = true;
					m_streamedTiles.Add(tile);
				}
			}

			public void RemoveStreamedTexture(MyStreamedTextureArrayTile tile)
			{
				using (m_streamedTilesGuard.Exclusive())
				{
					tile.IsStreamed = false;
					if (!tile.IsLoaded)
					{
						m_streamedTiles.Remove(tile);
					}
				}
			}
		}

		internal class MyTexturePrioritizer
		{
			private class TextureComparer : IComparer<int>
			{
				public TextureToken[] Textures;

				int IComparer<int>.Compare(int a, int b)
				{
					return Textures[b].Priority.CompareTo(Textures[a].Priority);
				}
			}

			private MyTextureStreamingManager m_context;

			private int m_loadedTextures;

			private ulong m_totalTexturesSize;

			private ulong m_streamingPool;

			private int m_sortingPressureFrames;

			private int m_streamedTextures;

			private DataGuard m_streamOrderGuard;

			private int[] m_streamedTexturesOrder = Array.Empty<int>();

			private bool[] m_wasLoadedCache;

			private readonly TextureComparer m_textureComparerCache = new TextureComparer();

			public MyTexturePrioritizer(MyTextureStreamingManager context)
			{
				m_context = context;
			}

			public void AddStreamedTexture(MyStreamedTexture texture)
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_streamOrderGuard.Exclusive())
					{
						texture.IsStreamed = true;
						int textureTokenId = texture.TextureTokenId;
						m_totalTexturesSize += m_context.m_textureTokens[textureTokenId].Size;
						ArrayExtensions.EnsureCapacity(ref m_streamedTexturesOrder, Math.Max(m_streamedTextures + 1, 500), 1.5f);
						m_streamedTexturesOrder[m_streamedTextures++] = textureTokenId;
						if (m_totalTexturesSize <= m_streamingPool)
						{
							m_loadedTextures++;
							texture.Load(fastLoad: true);
						}
						else
						{
							texture.Unload();
						}
					}
				}
			}

			public void RemoveStreamedTexture(MyStreamedTexture texture)
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_streamOrderGuard.Exclusive())
					{
						texture.IsStreamed = false;
						int textureTokenId = texture.TextureTokenId;
						uint size = m_context.m_textureTokens[textureTokenId].Size;
						int num = Array.IndexOf(m_streamedTexturesOrder, textureTokenId);
						m_streamedTextures--;
						Array.Copy(m_streamedTexturesOrder, num + 1, m_streamedTexturesOrder, num, m_streamedTextures - num);
						m_totalTexturesSize -= size;
						if (num < m_loadedTextures)
						{
							m_loadedTextures--;
						}
					}
				}
			}

			public void UpdateStreamingPriority(bool forceFullSort)
			{
				using (m_context.TextureTokensGuard.Read())
				{
					using (m_streamOrderGuard.Exclusive())
					{
						if (m_streamingPool != MyRender11.Settings.ResourceStreamingPool)
						{
							forceFullSort = true;
							m_streamingPool = MyRender11.Settings.ResourceStreamingPool;
						}
						if (m_sortingPressureFrames > 10 || forceFullSort)
						{
							PerformFullSort();
							return;
						}
						int num = 0;
						ulong num2 = m_totalTexturesSize;
						for (int num3 = m_streamedTextures - 1; num3 > 0; num3--)
						{
							int num4 = num3 - 1;
							TextureToken textureToken = m_context.m_textureTokens[m_streamedTexturesOrder[num3]];
							TextureToken textureToken2 = m_context.m_textureTokens[m_streamedTexturesOrder[num4]];
							if (textureToken2.Priority < textureToken.Priority)
							{
								MyUtils.Swap(ref m_streamedTexturesOrder[num3], ref m_streamedTexturesOrder[num4]);
								bool flag = num2 - textureToken2.Size <= m_streamingPool;
								if (m_loadedTextures <= num3)
								{
									MyStreamedTexture myStreamedTexture = textureToken.Texture as MyStreamedTexture;
									MyStreamedTexture myStreamedTexture2 = textureToken2.Texture as MyStreamedTexture;
									num++;
									if (m_loadedTextures <= num4)
									{
										if (m_loadedTextures == num4 && flag)
										{
											myStreamedTexture?.Load();
											m_loadedTextures++;
										}
									}
									else
									{
										myStreamedTexture2?.Unload();
										if (flag)
										{
											myStreamedTexture?.Load();
										}
										else
										{
											m_loadedTextures--;
										}
									}
								}
								textureToken = textureToken2;
							}
							num2 -= textureToken.Size;
						}
						if (num > 100)
						{
							m_sortingPressureFrames++;
						}
						else
						{
							m_sortingPressureFrames = 0;
						}
					}
				}
			}

			private void PerformFullSort()
			{
				m_sortingPressureFrames = 0;
				ArrayExtensions.EnsureCapacity(ref m_wasLoadedCache, m_context.m_texturesCount);
				for (int i = 0; i < m_streamedTextures; i++)
				{
					m_wasLoadedCache[m_streamedTexturesOrder[i]] = i < m_loadedTextures;
				}
				m_textureComparerCache.Textures = m_context.m_textureTokens;
				Array.Sort(m_streamedTexturesOrder, 0, m_streamedTextures, m_textureComparerCache);
				ulong num = 0uL;
				for (int j = 0; j < m_streamedTextures; j++)
				{
					int num2 = m_streamedTexturesOrder[j];
					TextureToken textureToken = m_context.m_textureTokens[num2];
					num += textureToken.Size;
					bool flag = m_wasLoadedCache[num2];
					bool flag2 = num <= m_streamingPool;
					if (flag != flag2)
					{
						MyStreamedTexture myStreamedTexture = textureToken.Texture as MyStreamedTexture;
						if (flag)
						{
							m_loadedTextures--;
							myStreamedTexture?.Unload();
						}
						else
						{
							m_loadedTextures++;
							myStreamedTexture?.Load();
						}
					}
				}
				m_totalTexturesSize = num;
			}

			[Conditional("DEBUG")]
			private void AssertStreamingConsistency()
			{
				ulong num = 0uL;
				for (int i = 0; i < m_streamedTextures; i++)
				{
					TextureToken textureToken = m_context.m_textureTokens[m_streamedTexturesOrder[i]];
					num += textureToken.Size;
					_ = m_loadedTextures;
					_ = m_streamingPool;
				}
			}
		}

		public struct QueryArgs
		{
			public bool WaitUntilLoaded;

			public bool SkipQualityReduction;

			public bool IsVoxel;

			public MyFileTextureEnum TextureType;

			public static implicit operator QueryArgs(MyFileTextureEnum textureType)
			{
				QueryArgs result = default(QueryArgs);
				result.TextureType = textureType;
				return result;
			}
		}

		private struct TextureToken
		{
			public int ScenePriority;

			public int TouchPriority;

			public int UsagePriority;

			public uint Size;

			public readonly MyStreamedTextureBase Texture;

			public int Priority => ScenePriority + TouchPriority + UsagePriority;

			public TextureToken(MyStreamedTextureBase texture, uint size, int priority)
			{
				Size = size;
				TouchPriority = priority;
				Texture = texture;
				ScenePriority = -1;
				UsagePriority = 0;
			}

			public override string ToString()
			{
				return $"{Texture}: {Priority}";
			}
		}

		private readonly MyConcurrentArrayBufferPool<BatchTextureInfo> m_indicesPool = new MyConcurrentArrayBufferPool<BatchTextureInfo>("MyTextureStreamingManager::Indices");

		private DataGuard SyncInProgress;

		private DataGuard TextureUpdatesInProgress;

		private DataGuard TextureTokensGuard;

		private int m_texturesCount;

		private TextureToken[] m_textureTokens;

		private bool m_textureQualityChanged;

		private readonly List<MyStreamedTextureBase> m_texturesToUpdate = new List<MyStreamedTextureBase>();

		private readonly MyTexturePrioritizer m_texturePrioritizer;

		private readonly ConcurrentDictionary<string, MyStreamedTexture> m_textures = new ConcurrentDictionary<string, MyStreamedTexture>();

		private readonly Dictionary<MyFileTextureEnum, MyStreamedTexture> m_emptyTextures = new Dictionary<MyFileTextureEnum, MyStreamedTexture>();

		private readonly Dictionary<IDynamicFileArrayTexture, MyTextureArrayPrioritizer> m_textureArrayPrioritizers = new Dictionary<IDynamicFileArrayTexture, MyTextureArrayPrioritizer>();

		private readonly ConcurrentDictionary<(IDynamicFileArrayTexture arrayTexture, string path), MyStreamedTextureArrayTile> m_textureTiles = new ConcurrentDictionary<(IDynamicFileArrayTexture, string), MyStreamedTextureArrayTile>();

		public MyTextureTileStreamingStatistics Statistics;

		public (CompiledBatch Batch, bool HasAllTextures) CompileBatch(Dictionary<IMyStreamedTextureBase, int> textureUsages)
		{
			using (SyncInProgress.Shared("Can't compile batch while sync is in progress'"))
			{
				int num = 0;
				BatchTextureInfo[] array = null;
				if (textureUsages.Count > 0)
				{
					array = m_indicesPool.Get(textureUsages.Count);
					foreach (KeyValuePair<IMyStreamedTextureBase, int> textureUsage in textureUsages)
					{
						IMyStreamedTextureBase key = textureUsage.Key;
						int textureTokenId = textureUsage.Key.TextureTokenId;
						if (textureTokenId == -1)
						{
							key.OnStreamingPriorityChangedFromZero(0);
							continue;
						}
						array[num].TokenId = textureTokenId;
						array[num].UseCount = textureUsage.Value;
						num++;
					}
				}
				return (new CompiledBatch(this, array, num), num == textureUsages.Count);
			}
		}

		public MyTextureStreamingManager()
		{
			m_texturePrioritizer = new MyTexturePrioritizer(this);
		}

		public void InitStreamingArray(IDynamicFileArrayTexture textureArray)
		{
			if (!m_textureArrayPrioritizers.ContainsKey(textureArray))
			{
				m_textureArrayPrioritizers[textureArray] = new MyTextureArrayPrioritizer(this, textureArray);
			}
		}

		void IManagerFrameEnd.OnFrameEnd()
		{
			Statistics.ResetFrame();
			using (SyncInProgress.Exclusive())
			{
				PerformSync();
			}
			void PerformSync()
			{
				using (TextureUpdatesInProgress.Exclusive("MyTextureStreamingManager.PerformSync"))
				{
					lock (m_texturesToUpdate)
					{
						for (int num = m_texturesToUpdate.Count - 1; num >= 0; num--)
						{
							MyStreamedTextureBase myStreamedTextureBase = m_texturesToUpdate[num];
							if (!myStreamedTextureBase.NeedsUpdate)
							{
								myStreamedTextureBase.OnRemovedFromUpdate();
								m_texturesToUpdate.RemoveAtFast(num);
							}
							else
							{
								myStreamedTextureBase.Update();
							}
						}
					}
				}
				string k;
				MyStreamedTexture v;
				if (m_textureQualityChanged)
				{
					foreach (KeyValuePair<string, MyStreamedTexture> texture in m_textures)
					{
						LinqExtensions.Deconstruct(texture, out k, out v);
						v.RecomputeByteSize();
					}
				}
				m_texturePrioritizer.UpdateStreamingPriority(m_textureQualityChanged);
				foreach (KeyValuePair<IDynamicFileArrayTexture, MyTextureArrayPrioritizer> textureArrayPrioritizer in m_textureArrayPrioritizers)
				{
					textureArrayPrioritizer.Value.UpdateStreamingPriority(m_textureQualityChanged);
				}
				using (TextureTokensGuard.Write())
				{
					for (int i = 0; i < m_texturesCount; i++)
					{
						ref int scenePriority = ref m_textureTokens[i].ScenePriority;
						if (scenePriority > 0)
						{
							scenePriority = 0;
						}
					}
				}
				if (m_textureQualityChanged)
				{
					m_textureQualityChanged = false;
					foreach (KeyValuePair<string, MyStreamedTexture> texture2 in m_textures)
					{
						LinqExtensions.Deconstruct(texture2, out k, out v);
						v.PerformScheduledStateChangesImmediately(false);
					}
					MyManagers.DynamicFileArrayTextures.ReloadAll();
					foreach (KeyValuePair<string, MyStreamedTexture> texture3 in m_textures)
					{
						LinqExtensions.Deconstruct(texture3, out k, out v);
						v.PerformScheduledStateChangesImmediately(true);
					}
					MyManagers.FileTextures.ReloadTextures(MyFileTextureManager.MyFileTextureHelper.IsQualityDependentFilter);
				}
			}
		}

		public IMyStreamedTexture GetTexture(string name, QueryArgs args)
		{
			return GetOrMakeTexture(name, args);
		}

		public IMyStreamedTextureArrayTile GetTextureArrayTile(IDynamicFileArrayTexture textureArray, string filePath)
		{
			return GetOrMakeTextureArrayTile(textureArray, filePath);
		}

		public ITexture GetTempTexture(string name, QueryArgs args, ushort priority = 100)
		{
			MyStreamedTexture orMakeTexture = GetOrMakeTexture(name, args);
			((IMyStreamedTextureBase)orMakeTexture).Touch(priority);
			return orMakeTexture.Texture;
		}

		public MyStreamedTexturePin GetPermanentTexture(string name, QueryArgs args)
		{
			return ((IMyStreamedTexture)GetOrMakeTexture(name, args)).Pin();
		}

		public void InvalidateTextureHandle(string texturePath, bool removed)
		{
			using (SyncInProgress.Shared())
			{
				if (!string.IsNullOrEmpty(texturePath))
				{
					string name = texturePath;
					MyResourceUtils.NormalizeFileTextureName(ref name, out var _);
<<<<<<< HEAD
					if (m_textures.TryGetValue(name, out var value))
					{
						value.UpdateTextureHandle(removed);
=======
					MyStreamedTexture myStreamedTexture = default(MyStreamedTexture);
					if (m_textures.TryGetValue(name, ref myStreamedTexture))
					{
						myStreamedTexture.UpdateTextureHandle(removed);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public void UnloadTexture(string texturePath)
		{
			using (SyncInProgress.Shared())
			{
				if (!string.IsNullOrEmpty(texturePath))
				{
					string name = texturePath;
					MyResourceUtils.NormalizeFileTextureName(ref name, out var _);
<<<<<<< HEAD
					if (m_textures.TryGetValue(name, out var value))
					{
						value.RemoveFromStreaming();
=======
					MyStreamedTexture myStreamedTexture = default(MyStreamedTexture);
					if (m_textures.TryGetValue(name, ref myStreamedTexture))
					{
						myStreamedTexture.RemoveFromStreaming();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public void DeprioritizeTexture(string texturePath)
		{
			using (SyncInProgress.Shared())
			{
				if (!string.IsNullOrEmpty(texturePath))
				{
					string name = texturePath;
					MyResourceUtils.NormalizeFileTextureName(ref name, out var _);
<<<<<<< HEAD
					if (m_textures.TryGetValue(name, out var value))
					{
						value.Deprioritize();
=======
					MyStreamedTexture myStreamedTexture = default(MyStreamedTexture);
					if (m_textures.TryGetValue(name, ref myStreamedTexture))
					{
						myStreamedTexture.Deprioritize();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public void OnTextureQualityChanged()
		{
			m_textureQualityChanged = true;
		}

		private MyStreamedTextureArrayTile GetOrMakeTextureArrayTile(IDynamicFileArrayTexture textureArray, string filePath)
		{
			filePath = MyResourceUtils.GetTextureFullPath(filePath);
<<<<<<< HEAD
			if (m_textureTiles.TryGetValue((textureArray, filePath), out var value))
			{
				return value;
=======
			MyStreamedTextureArrayTile result = default(MyStreamedTextureArrayTile);
			if (m_textureTiles.TryGetValue((textureArray, filePath), ref result))
			{
				return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (!m_textureArrayPrioritizers.TryGetValue(textureArray, out var prioritizer))
			{
				throw new Exception("Texture tile streaming prioritizer not initialized.");
			}
<<<<<<< HEAD
			return m_textureTiles.GetOrAdd((textureArray, filePath), delegate((IDynamicFileArrayTexture arrayTexture, string path) key)
=======
			return m_textureTiles.GetOrAdd((textureArray, filePath), (Func<(IDynamicFileArrayTexture, string), MyStreamedTextureArrayTile>)delegate((IDynamicFileArrayTexture arrayTexture, string path) key)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				int decayTime = 20;
				return new MyStreamedTextureArrayTile(this, prioritizer, textureArray, key.path, decayTime);
			});
		}

		private MyStreamedTexture GetOrMakeTexture(string fileName, QueryArgs args)
		{
			MyStreamedTexture value;
			if (string.IsNullOrEmpty(fileName))
			{
				lock (m_emptyTextures)
				{
					if (!m_emptyTextures.TryGetValue(args.TextureType, out value))
					{
						value = GetNewTexture(string.Empty, args);
						m_emptyTextures.Add(args.TextureType, value);
					}
				}
			}
			else
			{
				string name = fileName;
				MyResourceUtils.NormalizeFileTextureName(ref name, out var _);
<<<<<<< HEAD
				if (m_textures.TryGetValue(name, out var value2))
				{
					value = value2;
=======
				MyStreamedTexture myStreamedTexture = default(MyStreamedTexture);
				if (m_textures.TryGetValue(name, ref myStreamedTexture))
				{
					value = myStreamedTexture;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					QueryArgs argsClosure = args;
<<<<<<< HEAD
					value = m_textures.GetOrAdd(name, (string x) => GetNewTexture(x, argsClosure));
=======
					value = m_textures.GetOrAdd(name, (Func<string, MyStreamedTexture>)((string x) => GetNewTexture(x, argsClosure)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			if (args.WaitUntilLoaded && !value.Texture.IsTextureLoaded())
			{
				value.PerformStateChange(load: true, forceSyncLoad: true);
				while (!value.Texture.IsTextureLoaded())
				{
					Thread.Yield();
					value.PerformStateChange(load: true, forceSyncLoad: true);
				}
			}
			return value;
		}

		private MyStreamedTexture GetNewTexture(string textureName, QueryArgs args)
		{
			bool textureParamsPreloaded = false;
			int decayTime = ((args.TextureType == MyFileTextureEnum.GUI) ? 3600 : 20);
			return new MyStreamedTexture(this, new Func<bool, bool, (ITexture, uint)>(GetTextureHandle), decayTime);
			(ITexture, uint) GetTextureHandle(bool needsSize, bool onlyIfCheap)
			{
				ITexture texture = MyManagers.FileTextures.GetTexture(textureName, args.TextureType, args.IsVoxel, waitTillLoaded: false, args.SkipQualityReduction);
				uint item = 1u;
				if (!string.IsNullOrEmpty(textureName) && texture is IFileTexture)
				{
					if (needsSize)
					{
						if (MyFileTextureParamsManager.LoadFromFile(textureName, out var outParams, onlyIfCheap))
						{
							Vector2I xy;
							int mipLevels;
							if (!args.SkipQualityReduction)
							{
								(xy, mipLevels) = MyResourceUtils.GetTextureSizeAfterMipmapSkip(args.TextureType, args.IsVoxel, outParams.Resolution, outParams.MipLevels);
							}
							else
							{
								Vector2I resolution = outParams.Resolution;
								int mipLevels2 = outParams.MipLevels;
								xy = resolution;
								mipLevels = mipLevels2;
							}
							int z = (((args.TextureType & MyFileTextureEnum.CUBEMAP) == 0) ? 1 : 6);
							long num = MyResourceUtils.GetTextureByteSize(new Vector3I(xy, z), mipLevels, outParams.Format);
							if (num >= uint.MaxValue)
							{
								num = 4294967294L;
							}
							item = (uint)num;
							textureParamsPreloaded = true;
						}
						else if (MyFileSystem.FileExists(MyResourceUtils.GetTextureFullPath(textureName)))
						{
							item = uint.MaxValue;
						}
					}
					if (!textureParamsPreloaded)
					{
						textureParamsPreloaded = true;
						Parallel.Start(delegate
						{
							MyFileTextureParamsManager.LoadFromFile(textureName, out var _);
						});
					}
				}
				return (texture, item);
			}
		}

		private int GetNewToken(MyStreamedTextureBase texture, uint size, int priority)
		{
			using (TextureTokensGuard.Write())
			{
				int num = m_texturesCount++;
				ArrayExtensions.EnsureCapacity(ref m_textureTokens, m_texturesCount, 1.5f);
				m_textureTokens[num] = new TextureToken(texture, size, priority);
				return num;
			}
		}
	}
}
