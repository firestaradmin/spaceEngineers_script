using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Security;
using VRage;

namespace VRageRender
{
	internal static class MyVideoFactory
	{
		private static readonly Dictionary<uint, MyVideoPlayer> m_videos = new Dictionary<uint, MyVideoPlayer>();

		public static int Count
		{
			get
			{
				lock (m_videos)
				{
					return m_videos.Count;
				}
			}
		}

		public static MyVideoPlayer Create(uint id)
		{
			lock (m_videos)
			{
				DisposeVideo(id);
				return m_videos[id] = new MyVideoPlayer();
			}
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		public static void Play(uint id, string videoFile, float volume)
		{
			MyVideoPlayer video = GetVideo(id);
			if (video == null)
			{
				return;
			}
			try
			{
				lock (video)
				{
					video.Init(videoFile, MyVRage.Platform.CreateVideoPlayer());
					video.Volume = volume;
				}
			}
			catch (Exception ex)
			{
				MyRender11.Log.WriteLine(ex);
			}
		}

		public static MyVideoPlayer GetVideo(uint id)
		{
			lock (m_videos)
			{
				return m_videos.Get(id);
			}
		}

		public static void Remove(uint GID)
		{
			lock (m_videos)
			{
				DisposeVideo(GID);
			}
		}

		private static bool DisposeVideo(uint id)
		{
			if (m_videos.TryGetValue(id, out var value))
			{
				lock (value)
				{
					value.Stop();
					value.Dispose();
				}
				m_videos.Remove(id);
				return true;
			}
			return false;
		}
	}
}
