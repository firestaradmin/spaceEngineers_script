using System;
using System.Text;

namespace Sandbox.Graphics.GUI
{
	public struct Thickness : IEquatable<Thickness>
	{
		private static Thickness m_zero = new Thickness(0f);

		public float Left;

		public float Top;

		public float Right;

		public float Bottom;

		public static Thickness Zero => m_zero;

		public Thickness(float uniformLength)
		{
			Left = (Top = (Right = (Bottom = uniformLength)));
		}

		public Thickness(float left, float top, float right, float bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public bool Equals(Thickness other)
		{
			if (Left == other.Left && Top == other.Top && Right == other.Right)
			{
				return Bottom == other.Bottom;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Thickness)
			{
				return this == (Thickness)obj;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0}, {1}, {2}, {3}", Left, Top, Right, Bottom);
			return stringBuilder.ToString();
		}

		public static bool operator ==(Thickness t1, Thickness t2)
		{
			if ((t1.Left == t2.Left || (float.IsNaN(t1.Left) && float.IsNaN(t2.Left))) && (t1.Top == t2.Top || (float.IsNaN(t1.Top) && float.IsNaN(t2.Top))) && (t1.Right == t2.Right || (float.IsNaN(t1.Right) && float.IsNaN(t2.Right))))
			{
				if (t1.Bottom != t2.Bottom)
				{
					if (float.IsNaN(t1.Bottom))
					{
						return float.IsNaN(t2.Bottom);
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool operator !=(Thickness t1, Thickness t2)
		{
			return !(t1 == t2);
		}

		public static Thickness operator +(Thickness t1, Thickness t2)
		{
			return new Thickness(t1.Left + t2.Left, t1.Top + t2.Top, t1.Right + t2.Right, t1.Bottom + t2.Bottom);
		}

		public static Thickness operator -(Thickness t1, Thickness t2)
		{
			return new Thickness(t1.Left - t2.Left, t1.Top - t2.Top, t1.Right - t2.Right, t1.Bottom - t2.Bottom);
		}
	}
}
