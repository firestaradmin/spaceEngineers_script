using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace VRage.Platform.Windows.Forms
{
	internal class ModCrashedTheGameMesageBox : Form
	{
		private IContainer components;

		private Panel pLogo;

		private Button CloseBtn;

		private LinkLabel LogLink;

		private ModCrashedTheGameMesageBox(ref MyModCrashScreenTexts texts)
		{
			Application.EnableVisualStyles();
			InitializeComponent();
			CloseBtn.Text = texts.Close;
			Text = texts.Text;
			string text = string.Format(texts.Info, texts.ModName, texts.ModServiceName, texts.ModId, "log");
			LogLink.Text = text;
			LogLink.Links.Add(text.Length - 3, texts.LogPath.Length, texts.LogPath);
		}

		private void btnYes_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void linklblLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(e.Link.LinkData.ToString());
		}

		public static void ShowDialog(ref MyModCrashScreenTexts texts)
		{
			new ModCrashedTheGameMesageBox(ref texts).ShowDialog();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VRage.Platform.Windows.Forms.ModCrashedTheGameMesageBox));
			pLogo = new System.Windows.Forms.Panel();
			CloseBtn = new System.Windows.Forms.Button();
			LogLink = new System.Windows.Forms.LinkLabel();
			SuspendLayout();
			pLogo.AutoSize = true;
			pLogo.BackgroundImage = (System.Drawing.Image)resources.GetObject("pLogo.BackgroundImage");
			pLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			pLogo.Location = new System.Drawing.Point(38, 9);
			pLogo.Margin = new System.Windows.Forms.Padding(0);
			pLogo.MinimumSize = new System.Drawing.Size(340, 89);
			pLogo.Name = "pLogo";
			pLogo.Size = new System.Drawing.Size(425, 89);
			pLogo.TabIndex = 1;
			CloseBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			CloseBtn.Location = new System.Drawing.Point(412, 262);
			CloseBtn.Name = "CloseBtn";
			CloseBtn.Size = new System.Drawing.Size(87, 27);
			CloseBtn.TabIndex = 2;
			CloseBtn.Text = "Close";
			CloseBtn.UseVisualStyleBackColor = true;
			CloseBtn.Click += new System.EventHandler(btnYes_Click);
			LogLink.AutoSize = true;
			LogLink.Location = new System.Drawing.Point(35, 113);
			LogLink.Name = "LogLink";
			LogLink.Size = new System.Drawing.Size(141, 75);
			LogLink.TabIndex = 3;
			LogLink.TabStop = true;
			LogLink.Text = "Mod crashed the game\r\nPlease contact the author\r\n\r\nMore info in log:\r\n\r\n";
			LogLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linklblLog_LinkClicked);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(511, 301);
			base.Controls.Add(CloseBtn);
			base.Controls.Add(pLogo);
			base.Controls.Add(LogLink);
			Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ModCrashedTheGameMesageBox";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Mod crashed the Game!";
			base.TopMost = true;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
