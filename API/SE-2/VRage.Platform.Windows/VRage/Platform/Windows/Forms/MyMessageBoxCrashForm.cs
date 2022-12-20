using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VRage.FileSystem;

namespace VRage.Platform.Windows.Forms
{
	internal class MyMessageBoxCrashForm : Form
	{
		private IContainer components;

		private Button btnYes;

		private FlowLayoutPanel flMainPanel;

		private Panel pLogo;

		private Label lblMainText;

		private LinkLabel linklblLog;

		private Label lblEmailText;

		private FlowLayoutPanel flowLayoutPanel2;

		private Label lblEmail;

		private TextBox tbEmail;

		private Label lblDetails;

		private RichTextBox rtbDetails;

		private string Email => tbEmail.Text;

		private string Message => rtbDetails.Text;

		private MyMessageBoxCrashForm(ref MyCrashScreenTexts texts)
		{
			Application.EnableVisualStyles();
			InitializeComponent();
			Text = $"{texts.GameName} has crashed!";
			linklblLog.Text = texts.LogName;
			linklblLog.Links.Add(0, texts.LogName.Length, texts.LogName);
			if (!Directory.Exists(Path.Combine(new FileInfo(MyFileSystem.ExePath).Directory.FullName, "Content")))
			{
				MessageBox.Show("The content folder \"Content\" containing game assets is completely missing. Please verify integrity of game files using Steam. \n\n That is most likely the reason of the crash. As game cannot run without it.", "Content is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			lblMainText.Text = texts.MainText;
			linklblLog.Text = texts.Log;
			lblEmailText.Text = texts.EmailText;
			lblEmail.Text = texts.Email;
			lblDetails.Text = texts.Detail;
			btnYes.Text = texts.Yes;
		}

		private void linklblLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(e.Link.LinkData.ToString());
		}

		private void btnYes_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Yes;
			Close();
		}

		private void tbEmail_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				rtbDetails.Focus();
			}
		}

		public static bool ShowDialog(ref MyCrashScreenTexts texts, out string message, out string email)
		{
			MyMessageBoxCrashForm myMessageBoxCrashForm = new MyMessageBoxCrashForm(ref texts);
			DialogResult num = myMessageBoxCrashForm.ShowDialog();
			message = myMessageBoxCrashForm.Message;
			email = myMessageBoxCrashForm.Email;
			return num == DialogResult.Yes;
		}

		private void MyMessageBoxCrashForm_Shown(object sender, EventArgs e)
		{
			btnYes.Focus();
			linklblLog.TabStop = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VRage.Platform.Windows.Forms.MyMessageBoxCrashForm));
			btnYes = new System.Windows.Forms.Button();
			flMainPanel = new System.Windows.Forms.FlowLayoutPanel();
			lblMainText = new System.Windows.Forms.Label();
			linklblLog = new System.Windows.Forms.LinkLabel();
			lblEmailText = new System.Windows.Forms.Label();
			flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			lblEmail = new System.Windows.Forms.Label();
			tbEmail = new System.Windows.Forms.TextBox();
			lblDetails = new System.Windows.Forms.Label();
			rtbDetails = new System.Windows.Forms.RichTextBox();
			pLogo = new System.Windows.Forms.Panel();
			flMainPanel.SuspendLayout();
			flowLayoutPanel2.SuspendLayout();
			SuspendLayout();
			btnYes.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnYes.Location = new System.Drawing.Point(572, 494);
			btnYes.Name = "btnYes";
			btnYes.Size = new System.Drawing.Size(87, 27);
			btnYes.TabIndex = 0;
			btnYes.Text = "Send Log";
			btnYes.UseVisualStyleBackColor = true;
			btnYes.Click += new System.EventHandler(btnYes_Click);
			flMainPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			flMainPanel.AutoScroll = true;
			flMainPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			flMainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			flMainPanel.Controls.Add(lblMainText);
			flMainPanel.Controls.Add(linklblLog);
			flMainPanel.Controls.Add(lblEmailText);
			flMainPanel.Controls.Add(flowLayoutPanel2);
			flMainPanel.Controls.Add(lblDetails);
			flMainPanel.Controls.Add(rtbDetails);
			flMainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flMainPanel.Location = new System.Drawing.Point(9, 93);
			flMainPanel.Name = "flMainPanel";
			flMainPanel.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
			flMainPanel.Size = new System.Drawing.Size(650, 395);
			flMainPanel.TabIndex = 1;
			flMainPanel.WrapContents = false;
			lblMainText.AutoSize = true;
			lblMainText.Location = new System.Drawing.Point(12, 0);
			lblMainText.Margin = new System.Windows.Forms.Padding(0);
			lblMainText.Name = "lblMainText";
			lblMainText.Size = new System.Drawing.Size(415, 75);
			lblMainText.TabIndex = 1;
			lblMainText.Text = resources.GetString("lblMainText.Text");
			linklblLog.AutoSize = true;
			linklblLog.Location = new System.Drawing.Point(12, 75);
			linklblLog.Margin = new System.Windows.Forms.Padding(0);
			linklblLog.Name = "linklblLog";
			linklblLog.Size = new System.Drawing.Size(24, 15);
			linklblLog.TabIndex = 1;
			linklblLog.TabStop = true;
			linklblLog.Text = "log";
			linklblLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linklblLog_LinkClicked);
			lblEmailText.AutoSize = true;
			lblEmailText.Location = new System.Drawing.Point(12, 90);
			lblEmailText.Margin = new System.Windows.Forms.Padding(0);
			lblEmailText.Name = "lblEmailText";
			lblEmailText.Size = new System.Drawing.Size(624, 180);
			lblEmailText.TabIndex = 3;
			lblEmailText.Text = resources.GetString("lblEmailText.Text");
			flowLayoutPanel2.Controls.Add(lblEmail);
			flowLayoutPanel2.Controls.Add(tbEmail);
			flowLayoutPanel2.Location = new System.Drawing.Point(12, 270);
			flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			flowLayoutPanel2.Name = "flowLayoutPanel2";
			flowLayoutPanel2.Size = new System.Drawing.Size(425, 25);
			flowLayoutPanel2.TabIndex = 4;
			lblEmail.AutoSize = true;
			lblEmail.Location = new System.Drawing.Point(0, 3);
			lblEmail.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			lblEmail.Name = "lblEmail";
			lblEmail.Size = new System.Drawing.Size(91, 15);
			lblEmail.TabIndex = 0;
			lblEmail.Text = "Email (optional)";
			tbEmail.AcceptsReturn = true;
			tbEmail.Location = new System.Drawing.Point(95, 0);
			tbEmail.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			tbEmail.Name = "tbEmail";
			tbEmail.Size = new System.Drawing.Size(215, 23);
			tbEmail.TabIndex = 0;
			tbEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(tbEmail_KeyDown);
			lblDetails.AutoSize = true;
			lblDetails.Location = new System.Drawing.Point(12, 295);
			lblDetails.Margin = new System.Windows.Forms.Padding(0);
			lblDetails.Name = "lblDetails";
			lblDetails.Size = new System.Drawing.Size(600, 30);
			lblDetails.TabIndex = 5;
			lblDetails.Text = "\r\nTo help us resolve the problem, please provide a description of what you were doing when it occurred (optional):";
			rtbDetails.AcceptsTab = true;
			rtbDetails.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			rtbDetails.Location = new System.Drawing.Point(15, 328);
			rtbDetails.Name = "rtbDetails";
			rtbDetails.Size = new System.Drawing.Size(618, 42);
			rtbDetails.TabIndex = 0;
			rtbDetails.Text = "";
			pLogo.AutoSize = true;
			pLogo.BackgroundImage = (System.Drawing.Image)resources.GetObject("pLogo.BackgroundImage");
			pLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			pLogo.Location = new System.Drawing.Point(9, 1);
			pLogo.Margin = new System.Windows.Forms.Padding(0);
			pLogo.MinimumSize = new System.Drawing.Size(340, 89);
			pLogo.Name = "pLogo";
			pLogo.Size = new System.Drawing.Size(650, 89);
			pLogo.TabIndex = 0;
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(668, 525);
			base.Controls.Add(pLogo);
			base.Controls.Add(flMainPanel);
			base.Controls.Add(btnYes);
			Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MyMessageBoxCrashForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.Tag = "";
			Text = "CLANG!";
			base.TopMost = true;
			base.Shown += new System.EventHandler(MyMessageBoxCrashForm_Shown);
			flMainPanel.ResumeLayout(false);
			flMainPanel.PerformLayout();
			flowLayoutPanel2.ResumeLayout(false);
			flowLayoutPanel2.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
