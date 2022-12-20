using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Render.Particles;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal static class MyGPUEmitters
	{
		private class MyTextureArrayIndex
		{
			internal uint Index;

			internal uint Counter;
		}

		internal const int MAX_LIVE_EMITTERS = 1024;

		internal const int MAX_PARTICLES = 409600;

		public const int ATLAS_INDEX_BITS = 12;

		public const int ATLAS_DIMENSION_BITS = 6;

		public const int ATLAS_TEXTURE_BITS = 8;

		public const int MAX_ATLAS_DIMENSION = 64;

		public const int MAX_ATLAS_INDEX = 4096;

		private static Stack<int> m_freeBufferIndices;

		private static readonly Dictionary<string, MyTextureArrayIndex> m_textureArrayIndices;

		private static IFileArrayTexture m_textureArray;

		private static IFileArrayTexture m_emissiveArray;

		private static bool m_textureArrayDirty;

		private static readonly List<MyGPUEmitter> m_emitters;

		private static readonly List<MyGPUEmitter> m_emittersToDelete;

		private static readonly Dictionary<uint, MyGravity> m_gravities;

		public static float ParticleCountMultiplier { get; private set; }

		public static int Count => m_emitters.Count;

		public static Vector3D CameraPosition { get; private set; }

		static MyGPUEmitters()
		{
			m_textureArrayIndices = new Dictionary<string, MyTextureArrayIndex>();
			m_emitters = new List<MyGPUEmitter>();
			m_emittersToDelete = new List<MyGPUEmitter>();
			m_gravities = new Dictionary<uint, MyGravity>();
			UpdateParticleSettings();
		}

		public static void UpdateParticleSettings()
		{
			switch (MyRender11.Settings.User.ParticleQuality)
			{
			case MyRenderQualityEnum.LOW:
				ParticleCountMultiplier = 0.25f;
				break;
			case MyRenderQualityEnum.NORMAL:
				ParticleCountMultiplier = 1f;
				break;
			case MyRenderQualityEnum.HIGH:
				ParticleCountMultiplier = 1f;
				break;
			case MyRenderQualityEnum.EXTREME:
				ParticleCountMultiplier = 1f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		internal static void Init()
		{
			m_emitters.Clear();
			m_freeBufferIndices = new Stack<int>();
			for (int i = 0; i < 1024; i++)
			{
				m_freeBufferIndices.Push(1024 - i - 1);
			}
		}

		internal static void OnDeviceReset()
		{
			DoneDevice();
		}

		private static void DoneDevice()
		{
			m_textureArrayIndices.Clear();
			MyFileArrayTextureManager fileArrayTextures = MyManagers.FileArrayTextures;
			fileArrayTextures.DisposeTex(ref m_textureArray);
			fileArrayTextures.DisposeTex(ref m_emissiveArray);
			m_textureArrayDirty = false;
		}

		private static bool AllEmittersDead()
		{
			foreach (MyGPUEmitter emitter in m_emitters)
			{
				if (!emitter.EmitterData.Data.Flags.HasFlag(GPUEmitterFlags.Dead))
				{
					return false;
				}
			}
			return true;
		}

		internal static void OnDeviceEnd()
		{
			DoneDevice();
		}

		internal static void OnSessionEnd()
		{
			int num = 0;
			while (num < m_emitters.Count)
			{
				MyGPUEmitter myGPUEmitter = m_emitters[num];
				if ((myGPUEmitter.EmitterData.Data.Flags & GPUEmitterFlags.Dead) == 0)
				{
					myGPUEmitter.TryRemove(instant: true);
				}
				else
				{
					num++;
				}
			}
			m_gravities.Clear();
			Init();
		}

		internal static MyGPUEmitter Create(string debugName)
		{
			MyGPUEmitter myGPUEmitter = new MyGPUEmitter(debugName);
			m_emitters.Add(myGPUEmitter);
			return myGPUEmitter;
		}

		public static void Remove(MyGPUEmitter emitter)
		{
			m_emitters.Remove(emitter);
			MarkTextureUnused(emitter.EmitterData.AtlasTexture);
		}

		internal static void UpdateTexture(string newTexture, string originalTexture)
		{
			MarkTextureUnused(originalTexture);
			AddTexture(newTexture);
		}

		internal static void ReloadTextures()
		{
			m_textureArrayDirty = true;
		}

		private static void AddTexture(string tex)
		{
			if (tex != null)
			{
				if (m_textureArrayIndices.ContainsKey(tex))
				{
					m_textureArrayIndices[tex].Counter++;
					return;
				}
				m_textureArrayIndices.Add(tex, new MyTextureArrayIndex
				{
					Index = (uint)m_textureArrayIndices.Count,
					Counter = 1u
				});
				m_textureArrayDirty = true;
			}
		}

		private static void CleanUpTextures()
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, MyTextureArrayIndex> textureArrayIndex in m_textureArrayIndices)
			{
				if (textureArrayIndex.Value.Counter == 0)
				{
					list.Add(textureArrayIndex.Key);
				}
			}
			foreach (string item in list)
			{
				RemoveTexture(item);
			}
		}

		private static void RemoveTexture(string tex)
		{
			MyTextureArrayIndex myTextureArrayIndex = m_textureArrayIndices[tex];
			m_textureArrayIndices.Remove(tex);
			foreach (KeyValuePair<string, MyTextureArrayIndex> textureArrayIndex in m_textureArrayIndices)
			{
				if (textureArrayIndex.Value.Index > myTextureArrayIndex.Index)
				{
					m_textureArrayIndices[textureArrayIndex.Key].Index--;
				}
			}
		}

		private static void MarkTextureUnused(string tex)
		{
			if (!string.IsNullOrEmpty(tex))
			{
				m_textureArrayIndices[tex].Counter--;
			}
		}

		private static string[] GetTextureArrayFileList()
		{
			string[] array = new string[m_textureArrayIndices.Count];
			foreach (KeyValuePair<string, MyTextureArrayIndex> textureArrayIndex in m_textureArrayIndices)
			{
				array[textureArrayIndex.Value.Index] = textureArrayIndex.Key;
			}
			return array;
		}

		public static void AddToParticleTextureArray(HashSet<string> textures)
		{
<<<<<<< HEAD
			foreach (string texture in textures)
			{
				AddTexture(texture);
=======
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string> enumerator = textures.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AddTexture(enumerator.get_Current());
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void UpdateTextureArray()
		{
			if (!m_textureArrayDirty)
			{
				return;
			}
			MyFileArrayTextureManager fileArrayTextures = MyManagers.FileArrayTextures;
			fileArrayTextures.DisposeTex(ref m_textureArray);
			fileArrayTextures.DisposeTex(ref m_emissiveArray);
			string[] textureArrayFileList = GetTextureArrayFileList();
			if (textureArrayFileList.Length != 0)
			{
				if (CheckConsistency(textureArrayFileList, MyFileTextureEnum.GPUPARTICLES))
				{
					CleanUpTextures();
					textureArrayFileList = GetTextureArrayFileList();
				}
				if (textureArrayFileList.Length != 0)
				{
					m_textureArray = fileArrayTextures.InitFromFilesAsync("gpuParticles", textureArrayFileList, MyFileTextureEnum.GPUPARTICLES, MyGeneratedTexturePatterns.ColorMetal_BC7_SRgb, Format.BC7_UNorm_SRgb, 0);
					for (int i = 0; i < textureArrayFileList.Length; i++)
					{
						int num = textureArrayFileList[i].LastIndexOf(".");
						if (num == -1)
						{
							num = textureArrayFileList[i].Length;
						}
						textureArrayFileList[i] = textureArrayFileList[i].Substring(0, num) + "_em" + textureArrayFileList[i].Substring(num, textureArrayFileList[i].Length - num);
					}
				}
			}
			m_textureArrayDirty = false;
		}

		private static bool CheckConsistency(string[] inputFiles, MyFileTextureEnum type)
		{
			MyFileTextureManager fileTextures = MyManagers.FileTextures;
			Texture2D texture2D = fileTextures.GetTexture(inputFiles[0], type, isVoxel: true, waitTillLoaded: true, skipQualityReduction: false, temporary: true).Resource as Texture2D;
			if (texture2D == null)
			{
				return false;
			}
			for (int i = 1; i < inputFiles.Length; i++)
			{
				Texture2D texture2D2 = fileTextures.GetTexture(inputFiles[i], type, isVoxel: true, waitTillLoaded: true, skipQualityReduction: false, temporary: true).Resource as Texture2D;
				if (texture2D2 == null)
				{
					return false;
				}
				if (!MyResourceUtils.CheckTexturesConsistency(texture2D.Description, texture2D2.Description))
				{
					return false;
				}
			}
			return true;
		}

		internal static int Gather(MyGPUEmitterLayoutData[] data, out ISrvBindable textureArraySrv, out ISrvBindable emissiveArraySrv)
		{
			for (int i = 0; i < 1024; i++)
			{
				data[i].NumParticlesToEmitThisFrame = 0;
			}
			CameraPosition = MyRender11.Environment.Matrices.CameraPosition;
			if (m_emitters.Count > 1024)
			{
				m_emitters.Sort();
			}
			int num = -1;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = -1;
			int num6 = -1;
			for (int j = 0; j < m_emitters.Count; j++)
			{
				MyGPUEmitter myGPUEmitter = m_emitters[j];
				if (myGPUEmitter.BufferIndex == -1)
				{
<<<<<<< HEAD
					if (m_freeBufferIndices.Count > 0)
=======
					if (m_freeBufferIndices.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						if ((myGPUEmitter.EmitterData.Data.Flags & GPUEmitterFlags.Dead) == 0)
						{
							myGPUEmitter.BufferIndex = m_freeBufferIndices.Pop();
						}
					}
					else
					{
						num2++;
						if (num5 == -1)
						{
							num5 = j;
						}
					}
				}
				else
				{
					num3 = num2;
					num6 = j;
					if (num2 > 0)
					{
						num4++;
					}
				}
				if (myGPUEmitter.BufferIndex > num)
				{
					num = myGPUEmitter.BufferIndex;
				}
				myGPUEmitter.Update(ref data[(myGPUEmitter.BufferIndex != -1) ? myGPUEmitter.BufferIndex : 0]);
			}
			UpdateTextureArray();
			textureArraySrv = m_textureArray;
			emissiveArraySrv = m_emissiveArray;
			if (num3 > 0 && num4 > 0)
			{
				int num7 = num5;
				int num8 = num6;
				while (num7 < num8)
				{
					MyGPUEmitter myGPUEmitter2 = m_emitters[num8];
					data[myGPUEmitter2.BufferIndex].Flags |= GPUEmitterFlags.Dead;
					data[myGPUEmitter2.BufferIndex].NumParticlesToEmitThisFrame = 0;
					m_freeBufferIndices.Push(myGPUEmitter2.BufferIndex);
					myGPUEmitter2.BufferIndex = -1;
					do
					{
						num8--;
					}
					while (num8 > 0 && m_emitters[num8].BufferIndex == -1);
					do
					{
						num7++;
					}
					while (num7 < m_emitters.Count && m_emitters[num7].BufferIndex != -1);
				}
			}
			foreach (MyGPUEmitter emitter in m_emitters)
			{
				if ((emitter.EmitterData.Data.Flags & GPUEmitterFlags.Dead) != 0)
				{
					m_emittersToDelete.Add(emitter);
				}
			}
			foreach (MyGPUEmitter item in m_emittersToDelete)
			{
				item.Remove();
			}
			m_emittersToDelete.Clear();
			return num + 1;
		}

		public static MyGravity GetGravity(uint id)
		{
			if (id == uint.MaxValue)
			{
				return new MyGravity(uint.MaxValue);
			}
			if (!m_gravities.TryGetValue(id, out var value))
			{
				m_gravities.Add(id, value = new MyGravity(id));
			}
			return value;
		}

		public static uint GetTextureIndex(string atlasTexture)
		{
			return (m_textureArrayIndices.ContainsKey(atlasTexture) ? m_textureArrayIndices[atlasTexture].Index : 0) << 24;
		}

		public static void FreeBufferIndex(int bufferIndex)
		{
			if (bufferIndex != -1)
			{
				m_freeBufferIndices.Push(bufferIndex);
			}
		}
	}
}
