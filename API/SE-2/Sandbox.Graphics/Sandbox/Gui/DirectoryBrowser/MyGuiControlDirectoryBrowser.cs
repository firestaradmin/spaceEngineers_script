using System;
using System.IO;
using System.Text;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Gui.DirectoryBrowser
{
	public class MyGuiControlDirectoryBrowser : MyGuiControlMultiSelectTable
	{
		protected DirectoryInfo m_topMostDir;

		protected DirectoryInfo m_currentDir;

		protected Row m_backRow;

		public MyGuiHighlightTexture FolderCellIconTexture { get; set; }

		public MyGuiDrawAlignEnum FolderCellIconAlign { get; set; }

		public MyGuiHighlightTexture FileCellIconTexture { get; set; }

		public MyGuiDrawAlignEnum FileCellIconAlign { get; set; }

		public string CurrentDirectory
		{
			get
			{
				return ((FileSystemInfo)m_currentDir).get_FullName();
			}
			set
			{
				TraverseToDirectory(value);
			}
		}

		public event Action<MyGuiControlTable, EventArgs> BrowserItemConfirmed;

		public MyGuiControlDirectoryBrowser(string topMostDirectory = null, string initialDirectory = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			if (!string.IsNullOrEmpty(topMostDirectory))
			{
				m_topMostDir = new DirectoryInfo(topMostDirectory);
			}
			if (!string.IsNullOrEmpty(initialDirectory))
			{
				m_currentDir = new DirectoryInfo(initialDirectory);
			}
			else
			{
				m_currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
			}
			FolderCellIconTexture = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL;
			FolderCellIconAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			base.ItemDoubleClicked += OnItemDoubleClicked;
			base.ItemConfirmed += OnItemDoubleClicked;
			base.ColumnsCount = 3;
			SetCustomColumnWidths(new float[3] { 0.56f, 0.27f, 0.17f });
			SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));
			SetColumnName(1, MyTexts.Get(MyCommonTexts.Created));
			SetColumnName(2, MyTexts.Get(MyCommonTexts.Size));
			SetColumnComparison(0, (Cell cellA, Cell cellB) => cellA.Text.CompareToIgnoreCase(cellB.Text));
			SetColumnComparison(1, (Cell cellA, Cell cellB) => cellB.Text.CompareToIgnoreCase(cellA.Text));
			SetColumnComparison(2, (Cell cellA, Cell cellB) => cellB.Text.CompareToIgnoreCase(cellA.Text));
			Refresh();
		}

		protected string GetLocalPath()
		{
<<<<<<< HEAD
			return m_currentDir.FullName.Replace(m_topMostDir.FullName, "");
=======
			return ((FileSystemInfo)m_currentDir).get_FullName().Replace(((FileSystemInfo)m_topMostDir).get_FullName(), "");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public virtual void Refresh()
		{
			Clear();
			DirectoryInfo[] directories = m_currentDir.GetDirectories();
<<<<<<< HEAD
			if (!m_topMostDir.FullName.TrimEnd(new char[1] { Path.DirectorySeparatorChar }).Equals(m_currentDir.FullName, StringComparison.OrdinalIgnoreCase))
=======
			if (!((FileSystemInfo)m_topMostDir).get_FullName().TrimEnd(new char[1] { Path.DirectorySeparatorChar }).Equals(((FileSystemInfo)m_currentDir).get_FullName(), StringComparison.OrdinalIgnoreCase))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				AddBackRow();
			}
			DirectoryInfo[] array = directories;
			foreach (DirectoryInfo dir in array)
			{
				AddFolderRow(dir);
			}
			ScrollToSelection();
		}

		protected virtual void AddFolderRow(DirectoryInfo dir)
		{
			Row row = new Row(dir);
			row.AddCell(new Cell(((FileSystemInfo)dir).get_Name(), dir, null, null, FolderCellIconTexture, FolderCellIconAlign));
			row.AddCell(new Cell(string.Empty));
			row.AddCell(new Cell(string.Empty));
			row.AddCell(new Cell(string.Empty));
			Add(row);
		}

		protected virtual void AddBackRow()
		{
			if (m_backRow == null)
			{
				m_backRow = new Row();
				m_backRow.AddCell(new Cell("..", null, null, null, MyGuiConstants.TEXTURE_BLUEPRINTS_ARROW, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			Add(m_backRow);
			base.IgnoreFirstRowForSort = true;
		}

		private void TraverseToDirectory(string path)
		{
<<<<<<< HEAD
			if (!(path == m_currentDir.FullName) && (m_topMostDir == null || m_topMostDir.IsParentOf(path)))
=======
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			if (!(path == ((FileSystemInfo)m_currentDir).get_FullName()) && (m_topMostDir == null || m_topMostDir.IsParentOf(path)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_currentDir = new DirectoryInfo(path);
				Refresh();
			}
		}

		private void OnItemDoubleClicked(MyGuiControlTable myGuiControlTable, EventArgs eventArgs)
		{
			if (eventArgs.RowIndex < 0 || eventArgs.RowIndex >= base.RowsCount)
			{
				return;
			}
			Row row = GetRow(eventArgs.RowIndex);
			if (row == null)
			{
				return;
			}
			if (row == m_backRow)
			{
				OnBackDoubleclicked();
				return;
			}
			object userData = row.UserData;
			DirectoryInfo val = (DirectoryInfo)((userData is DirectoryInfo) ? userData : null);
			if (val != null)
			{
<<<<<<< HEAD
				OnDirectoryDoubleclicked(directoryInfo);
=======
				OnDirectoryDoubleclicked(val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				this.BrowserItemConfirmed.InvokeIfNotNull(myGuiControlTable, eventArgs);
			}
		}

		protected virtual void OnDirectoryDoubleclicked(DirectoryInfo info)
		{
<<<<<<< HEAD
			TraverseToDirectory(info.FullName);
=======
			TraverseToDirectory(((FileSystemInfo)info).get_FullName());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected virtual void OnBackDoubleclicked()
		{
			if (m_currentDir.get_Parent() != null)
			{
<<<<<<< HEAD
				string fullName = m_currentDir.Parent.FullName;
=======
				string fullName = ((FileSystemInfo)m_currentDir.get_Parent()).get_FullName();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				TraverseToDirectory(fullName);
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			if (MyInput.Static.IsNewXButton1MousePressed() || MyInput.Static.IsNewKeyPressed(MyKeys.Back))
			{
				OnBackDoubleclicked();
			}
			return base.HandleInput();
		}

		public bool SetTopMostAndCurrentDir(string directory)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			DirectoryInfo val = new DirectoryInfo(directory);
			if (((FileSystemInfo)val).get_Exists())
			{
				m_topMostDir = val;
				m_currentDir = val;
				return true;
			}
			return false;
		}
	}
}
