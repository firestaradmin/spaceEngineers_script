using System;
using System.Text;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	public class MySoundPair
	{
		public static MySoundPair Empty = new MySoundPair();

		[ThreadStatic]
		private static StringBuilder m_cache;

		private MyCueId m_arcade;

		private MyCueId m_realistic;

		private static StringBuilder Cache
		{
			get
			{
				if (m_cache == null)
				{
					m_cache = new StringBuilder();
				}
				return m_cache;
			}
		}

		public MyCueId Arcade => m_arcade;

		public MyCueId Realistic => m_realistic;

		public MyCueId SoundId
		{
			get
			{
				if (MySession.Static != null)
				{
					if (!MySession.Static.Settings.RealisticSound || !MyFakes.ENABLE_NEW_SOUNDS)
					{
						return m_arcade;
					}
					return m_realistic;
				}
				return m_arcade;
			}
		}

		public MySoundPair()
		{
			Init(null);
		}

		public MySoundPair(string cueName, bool useLog = true)
		{
			Init(cueName, useLog);
		}

		public void Init(string cueName, bool useLog = true)
		{
			if (string.IsNullOrEmpty(cueName) || Sandbox.Engine.Platform.Game.IsDedicated || MyAudio.Static == null)
			{
				m_arcade = new MyCueId(MyStringHash.NullOrEmpty);
				m_realistic = new MyCueId(MyStringHash.NullOrEmpty);
				return;
			}
			m_arcade = MyAudio.Static.GetCueId(cueName);
			if (m_arcade.Hash != MyStringHash.NullOrEmpty)
			{
				m_realistic = m_arcade;
				return;
			}
			Cache.Clear();
			Cache.Append("Arc").Append(cueName);
			m_arcade = MyAudio.Static.GetCueId(Cache.ToString());
			Cache.Clear();
			Cache.Append("Real").Append(cueName);
			m_realistic = MyAudio.Static.GetCueId(Cache.ToString());
			if (useLog && m_arcade.Hash == MyStringHash.NullOrEmpty && m_realistic.Hash == MyStringHash.NullOrEmpty)
			{
				MySandboxGame.Log.WriteLine($"Could not find any sound for '{cueName}'");
			}
		}

		public void Init(MyCueId cueId)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
				{
					m_realistic = cueId;
					m_arcade = new MyCueId(MyStringHash.NullOrEmpty);
				}
				else
				{
					m_arcade = cueId;
					m_realistic = new MyCueId(MyStringHash.NullOrEmpty);
				}
			}
			else
			{
				m_arcade = new MyCueId(MyStringHash.NullOrEmpty);
				m_realistic = new MyCueId(MyStringHash.NullOrEmpty);
			}
		}

		public override bool Equals(object obj)
		{
			MySoundPair mySoundPair;
			if ((mySoundPair = obj as MySoundPair) != null)
			{
				if (Arcade == mySoundPair.Arcade)
				{
					return Realistic == mySoundPair.Realistic;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Arcade.GetHashCode() ^ Realistic.GetHashCode();
		}

		public override string ToString()
		{
			return SoundId.ToString();
		}

		public static MyCueId GetCueId(string cueName)
		{
			if (string.IsNullOrEmpty(cueName))
			{
				return new MyCueId(MyStringHash.NullOrEmpty);
			}
			MyCueId cueId = MyAudio.Static.GetCueId(cueName);
			if (cueId.Hash != MyStringHash.NullOrEmpty)
			{
				return cueId;
			}
			Cache.Clear();
			if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
			{
				Cache.Append("Real").Append(cueName);
				return MyAudio.Static.GetCueId(Cache.ToString());
			}
			Cache.Append("Arc").Append(cueName);
			return MyAudio.Static.GetCueId(Cache.ToString());
		}
	}
}
