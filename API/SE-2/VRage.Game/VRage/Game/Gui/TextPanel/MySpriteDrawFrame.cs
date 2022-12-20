using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace VRage.Game.GUI.TextPanel
{
	public struct MySpriteDrawFrame : IDisposable
	{
		public struct ClearClipToken : IDisposable
		{
			private MySpriteDrawFrame m_frame;

			public ClearClipToken(MySpriteDrawFrame frame)
			{
				m_frame = frame;
			}

			public void Dispose()
			{
				m_frame.Add(MySprite.CreateClearClipRect());
			}
		}

		private MyList<MySprite> m_sprites;

		private Action<MySpriteDrawFrame> m_submitFrameCallback;

		public MySpriteDrawFrame(Action<MySpriteDrawFrame> submitFrameCallback)
		{
			m_sprites = null;
			m_submitFrameCallback = submitFrameCallback;
			if (submitFrameCallback != null)
			{
				m_sprites = PoolManager.Get<MyList<MySprite>>();
			}
		}

		public ClearClipToken Clip(int x, int y, int width, int height)
		{
			Add(MySprite.CreateClipRect(new Rectangle(x, y, width, height)));
			return new ClearClipToken(this);
		}

		public void Add(MySprite sprite)
		{
			m_sprites?.Add(sprite);
		}

		public void AddRange(IEnumerable<MySprite> sprites)
		{
			m_sprites?.AddRange(sprites);
		}

		public MySpriteCollection ToCollection()
		{
			if (m_sprites == null || m_sprites.Count == 0)
			{
				return default(MySpriteCollection);
			}
			MySprite[] array = new MySprite[m_sprites.Count];
			for (int i = 0; i < m_sprites.Count; i++)
			{
				array[i] = m_sprites[i];
			}
			return new MySpriteCollection(array);
		}

		public void AddToList(List<MySprite> list)
		{
			if (list != null && m_sprites != null)
			{
				list.AddRange(m_sprites);
			}
		}

		public void Dispose()
		{
			if (m_submitFrameCallback != null)
			{
				m_submitFrameCallback.InvokeIfNotNull(this);
				m_submitFrameCallback = null;
				m_sprites.ClearFast();
				PoolManager.Return(ref m_sprites);
			}
		}
	}
}
