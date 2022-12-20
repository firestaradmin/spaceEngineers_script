using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.Multimedia;
using VRage.Data.Audio;
using VRage.FileSystem;

namespace VRage.Audio
{
	public class MyWaveBank : IDisposable
	{
		private readonly Dictionary<string, MyInMemoryWave> m_waves = new Dictionary<string, MyInMemoryWave>();

		internal readonly Dictionary<string, MyInMemoryWave> LoadedStreamedWaves = new Dictionary<string, MyInMemoryWave>();

		public int Count => m_waves.Count;

		private static bool FindAudioFile(MySoundData cue, string fileName, out string fsPath)
		{
			fsPath = (Path.IsPathRooted(fileName) ? fileName : Path.Combine(MyFileSystem.ContentPath, "Audio", fileName));
			bool flag = MyFileSystem.FileExists(fsPath);
			if (!flag)
			{
				string text = Path.Combine(Path.GetDirectoryName(fsPath), Path.GetFileNameWithoutExtension(fsPath) + ".wav");
				flag = MyFileSystem.FileExists(text);
				if (flag)
				{
					fsPath = text;
				}
			}
			if (!flag)
			{
				MyAudio.OnSoundError?.Invoke(cue, "Unable to find audio file: '" + cue.SubtypeId.ToString() + "', '" + fileName + "'");
			}
			return flag;
		}

		private static bool CheckWaveErrors(MySoundData cue, MyInMemoryWave wave, ref WaveFormatEncoding encoding, MySoundDimensions dim, string waveFileName)
		{
			bool result = false;
			if (encoding == WaveFormatEncoding.Unknown)
			{
				encoding = wave.WaveFormat.Encoding;
			}
			if (wave.WaveFormat.Encoding == WaveFormatEncoding.Unknown)
			{
				result = true;
				MyAudio.OnSoundError?.Invoke(cue, "Unknown audio encoding '" + cue.SubtypeId.ToString() + "', '" + waveFileName + "'");
			}
			if (dim == MySoundDimensions.D3 && wave.WaveFormat.Channels != 1)
			{
				result = true;
				MyAudio.OnSoundError?.Invoke(cue, $"3D sound '{cue.SubtypeId.ToString()}', '{waveFileName}' must be in mono, got {wave.WaveFormat.Channels} channels");
			}
			if (wave.WaveFormat.Encoding != encoding)
			{
				result = true;
				MyAudio.OnSoundError?.Invoke(cue, $"Inconsistent sound encoding in '{cue.SubtypeId.ToString()}', '{waveFileName}', got '{wave.WaveFormat.Encoding}', expected '{encoding}'");
			}
			return result;
		}

		public void Add(MySoundData cue, MyAudioWave cueWave, bool cacheLoaded)
		{
			MySoundDimensions type = cueWave.Type;
			string[] obj = new string[3] { cueWave.Start, cueWave.Loop, cueWave.End };
			WaveFormatEncoding encoding = WaveFormatEncoding.Unknown;
			int num = 0;
			string[] array = obj;
			foreach (string text in array)
			{
				num++;
				if (string.IsNullOrEmpty(text) || m_waves.ContainsKey(text) || !FindAudioFile(cue, text, out var fsPath))
				{
					continue;
				}
				if (cue.StreamSound)
				{
					break;
				}
				try
				{
					MyInMemoryWave myInMemoryWave = new MyInMemoryWave(cue, fsPath, this, streamed: false, cacheLoaded);
					if (num != 2)
					{
						myInMemoryWave.Buffer.LoopCount = 0;
						myInMemoryWave.Buffer.LoopBegin = 0;
						myInMemoryWave.Buffer.LoopLength = 0;
					}
					m_waves[text] = myInMemoryWave;
					CheckWaveErrors(cue, myInMemoryWave, ref encoding, type, fsPath);
				}
				catch (Exception arg)
				{
					MyAudio.OnSoundError?.Invoke(cue, $"Unable to load audio file: '{cue.SubtypeId.ToString()}', '{text}': {arg}");
				}
			}
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, MyInMemoryWave> wave in m_waves)
			{
				wave.Value.Dispose();
			}
			m_waves.Clear();
		}

		public MyInMemoryWave GetWave(string filename)
		{
			if (string.IsNullOrEmpty(filename) || !m_waves.ContainsKey(filename))
			{
				return null;
			}
			return m_waves[filename];
		}

		public MyInMemoryWave GetStreamedWave(string waveFileName, MySoundData cue, MySoundDimensions dim = MySoundDimensions.D2)
		{
			if (string.IsNullOrEmpty(waveFileName))
			{
				return null;
			}
			if (FindAudioFile(cue, waveFileName, out var fsPath))
			{
				try
				{
					if (!LoadedStreamedWaves.TryGetValue(fsPath, out var value))
					{
						value = new MyInMemoryWave(cue, fsPath, this, streamed: true);
						LoadedStreamedWaves[fsPath] = value;
					}
					else
					{
						value.Reference();
					}
					WaveFormatEncoding encoding = WaveFormatEncoding.Unknown;
					if (CheckWaveErrors(cue, value, ref encoding, dim, waveFileName))
					{
						value.Dereference();
						value = null;
					}
					return value;
				}
				catch (Exception arg)
				{
					MyAudio.OnSoundError?.Invoke(cue, $"Unable to load audio file: '{cue.SubtypeId.ToString()}', '{waveFileName}': {arg}");
				}
			}
			return null;
		}
	}
}
