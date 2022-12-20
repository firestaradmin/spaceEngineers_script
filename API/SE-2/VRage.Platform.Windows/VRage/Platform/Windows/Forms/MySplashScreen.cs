using System;
using System.Drawing;
using System.Windows.Forms;
using VRageMath;

namespace VRage.Platform.Windows.Forms
{
	internal class MySplashScreen : Form
	{
		private readonly Graphics m_graphics;

		private string m_imageFile;

		private readonly Image m_image;

		private readonly PointF m_scale;

		public MySplashScreen(string image, Vector2 scale)
		{
			try
			{
				m_image = Image.FromFile(image);
				m_scale = new PointF(scale.X, scale.Y);
			}
			catch (Exception)
			{
				m_image = null;
				return;
			}
			InitializeComponent();
			m_graphics = CreateGraphics();
			m_imageFile = image;
			Draw();
		}

		public new void Close()
		{
			Hide();
			Dispose();
		}

		private void Draw()
		{
			if (m_image != null)
			{
				Show();
				System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0f, 0f, (float)m_image.Width * m_scale.X, (float)m_image.Height * m_scale.Y);
				m_graphics.DrawImage(m_image, rect);
			}
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			float num = (float)m_image.Width * m_scale.X;
			float num2 = (float)m_image.Height * m_scale.Y;
			base.ClientSize = new System.Drawing.Size((int)num, (int)num2);
			base.Name = "SplashScreen";
			ResumeLayout(false);
			base.TopMost = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			CenterToScreen();
		}
	}
}
