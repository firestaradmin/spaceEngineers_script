using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlMultiSelectTable : MyGuiControlTable
	{
		private List<int> m_selectedRowsIndexes;

		public List<int> SelectedRowsIndexes
		{
			get
			{
				if (m_selectedRowsIndexes == null)
				{
					m_selectedRowsIndexes = new List<int>();
				}
				return m_selectedRowsIndexes;
			}
			set
			{
				m_selectedRowsIndexes = value;
				if (m_selectedRowsIndexes != null && m_selectedRowsIndexes.Count > 0)
				{
					base.SelectedRowIndex = m_selectedRowsIndexes[0];
				}
			}
		}

		public List<Row> SelectedRows
		{
			get
			{
				List<Row> list = new List<Row>();
				foreach (int selectedRowsIndex in SelectedRowsIndexes)
				{
					if (IsValidRowIndex(selectedRowsIndex))
					{
						list.Add(m_rows[selectedRowsIndex]);
					}
				}
				return list;
			}
			set
			{
				m_selectedRowsIndexes.Clear();
				foreach (Row item in value)
				{
					int num = m_rows.IndexOf(item);
					if (num >= 0)
					{
						SelectedRowsIndexes.Add(num);
					}
				}
			}
		}

		public MyGuiControlMultiSelectTable()
		{
			m_drawSingleSelectRows = false;
			m_mouseOverRowIndex = 0;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			DrawRows(transitionAlpha);
		}

		private void DrawRows(float transitionAlpha)
		{
			Vector2 vector = GetPositionAbsoluteTopLeft() + m_rowsArea.Position;
			Vector2 normalizedSizeFromScreenSize = MyGuiManager.GetNormalizedSizeFromScreenSize(new Vector2(1f, 1f));
			for (int i = 0; i < base.VisibleRowsCount; i++)
			{
				int num = i + m_visibleRowIndexOffset;
				if (num >= m_rows.Count)
				{
					break;
				}
				if (num < 0)
				{
					continue;
				}
				bool flag = m_mouseOverRowIndex.HasValue && m_mouseOverRowIndex.Value == num;
				bool flag2 = SelectedRowsIndexes != null && SelectedRowsIndexes.Contains(num);
				string text = m_styleDef.RowFontNormal;
				if (flag || flag2)
				{
					Vector2 normalizedCoord = vector;
					normalizedCoord.X += normalizedSizeFromScreenSize.X;
					MyGuiManager.DrawSpriteBatch(flag ? m_styleDef.RowTextureHighlight : (base.HasFocus ? m_styleDef.RowTextureFocus : m_styleDef.RowTextureActive), normalizedCoord, new Vector2(m_rowsArea.Size.X - normalizedSizeFromScreenSize.X, base.RowHeight), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					text = m_styleDef.RowFontHighlight;
				}
				Row row = m_rows[num];
				if (row != null)
				{
					Vector2 vector2 = vector;
					for (int j = 0; j < base.ColumnsCount && j < row.Cells.Count; j++)
					{
						Cell cell = row.Cells[j];
						ColumnMetaData columnMetaData = m_columnsMetaData[j];
						if (!columnMetaData.Visible)
						{
							continue;
						}
						Vector2 vector3 = new Vector2(columnMetaData.VisibleWidth * m_rowsArea.Size.X, base.RowHeight);
						if (cell != null && cell.Control != null)
						{
							MyUtils.GetCoordAlignedFromTopLeft(vector2, vector3, cell.IconOriginAlign);
							cell.Control.Position = vector2 + vector3 * 0.5f;
							cell.Control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
							cell.Control.Draw(transitionAlpha, transitionAlpha);
						}
						else if (cell != null && cell.Text != null)
						{
							float num2 = 0f;
							float num3 = columnMetaData.Margin.Left + cell.Margin.Left;
							if (cell.Icon.HasValue)
							{
								Vector2 coordAlignedFromTopLeft = MyUtils.GetCoordAlignedFromTopLeft(vector2, vector3, cell.IconOriginAlign);
								MyGuiHighlightTexture value = cell.Icon.Value;
								Vector2 vector4 = Vector2.Min(value.SizeGui, vector3) / value.SizeGui;
								float num4 = Math.Min(vector4.X, vector4.Y);
								num2 = value.SizeGui.X;
								MyGuiManager.DrawSpriteBatch(base.HasHighlight ? value.Highlight : ((base.HasFocus && value.Focus != null) ? value.Focus : value.Normal), coordAlignedFromTopLeft + new Vector2(num3, 0f), value.SizeGui * num4, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), cell.IconOriginAlign);
								if (num4.IsValid())
								{
									num3 *= 2f;
								}
							}
							Vector2 vector5 = default(Vector2);
							if (columnMetaData.TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER || columnMetaData.TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP || columnMetaData.TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM)
							{
								Vector2 vector6 = MyGuiManager.MeasureString(text, cell.Text, base.TextScaleWithLanguage);
								vector5 = MyUtils.GetCoordAlignedFromCenter(vector2 + 0.5f * vector3 + new Vector2(num3, 0f) - new Vector2(vector6.X / 2f, 0f), vector3, columnMetaData.TextAlign);
							}
							else
							{
								vector5 = ((columnMetaData.TextAlign != MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER && columnMetaData.TextAlign != MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP && columnMetaData.TextAlign != MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM) ? MyUtils.GetCoordAlignedFromCenter(vector2 + 0.5f * vector3 + new Vector2(num3, 0f), vector3, columnMetaData.TextAlign) : MyUtils.GetCoordAlignedFromCenter(vector2 + 0.5f * vector3 - new Vector2(num3, 0f), vector3, columnMetaData.TextAlign));
							}
							Vector4 vector7 = Vector4.One;
							if (flag && m_styleDef.TextColorHighlight.HasValue)
							{
								vector7 = m_styleDef.TextColorHighlight.Value;
							}
							if (flag2)
							{
								if (base.HasFocus && m_styleDef.TextColorFocus.HasValue)
								{
									vector7 = m_styleDef.TextColorFocus.Value;
								}
								if (!base.HasFocus && m_styleDef.TextColorActive.HasValue)
								{
									vector7 = m_styleDef.TextColorActive.Value;
								}
							}
							float textScale = base.TextScaleWithLanguage;
							if (cell.IsAutoScaleEnabled)
							{
								DoEllipsisAndScaleAdjust(cell, text, ref textScale, vector3);
							}
							vector5.X += num2;
							string font = text;
							string text2 = cell.Text.ToString();
							Vector2 normalizedCoord2 = vector5;
							float scale = textScale;
							Color value2 = vector7;
							Color? obj = ((!cell.TextColor.HasValue) ? new Color?(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha)) : (cell.TextColor * transitionAlpha));
							MyGuiManager.DrawString(font, text2, normalizedCoord2, scale, value2 * obj, columnMetaData.TextAlign, useFullClientArea: false, vector3.X - num3 - columnMetaData.Margin.Right - cell.Margin.Right);
						}
						vector2.X += vector3.X;
					}
				}
				vector.Y += base.RowHeight;
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			if (SelectedRowsIndexes != null && SelectedRowsIndexes.Count > 1 && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_R, MyControlStateType.PRESSED) && MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))))
			{
				SelectedRowsIndexes = new List<int> { SelectedRowsIndexes[SelectedRowsIndexes.Count - 1] };
				return this;
			}
			if (MyInput.Static.IsJoystickLastUsed && m_mouseOverRowIndex.HasValue && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_R, MyControlStateType.PRESSED) && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_A))
			{
				ToggleSelect(m_mouseOverRowIndex.Value);
				return this;
			}
			MyGuiControlBase captureInput = HandleBaseInput();
			if (captureInput != null)
			{
				return captureInput;
			}
			if (!base.Enabled)
			{
				return null;
			}
			if (m_scrollBar != null && m_scrollBar.HandleInput())
			{
				captureInput = this;
			}
			HandleMouseOver();
			HandleNewMousePress(ref captureInput);
			using (List<MyGuiControlBase>.Enumerator enumerator = base.Controls.GetVisibleControls().GetEnumerator())
			{
				while (enumerator.MoveNext() && enumerator.Current.HandleInput() == null)
				{
				}
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
			}
			if (!base.HasFocus)
			{
				return captureInput;
			}
			if (base.HasItemConfirmedEvent && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)) && (m_mouseOverRowIndex.HasValue || (SelectedRowsIndexes != null && SelectedRowsIndexes.Count == 1)))
			{
				captureInput = this;
				base.SelectedRowIndex = 0;
				if (SelectedRowsIndexes != null && SelectedRowsIndexes.Count == 1)
				{
					base.SelectedRowIndex = SelectedRowsIndexes[0];
				}
				else if (m_mouseOverRowIndex.HasValue)
				{
					base.SelectedRowIndex = m_mouseOverRowIndex.Value;
				}
				RaiseItemConfirmed(this, new EventArgs
				{
					RowIndex = base.SelectedRowIndex.Value
				});
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				if (!m_gamepadSortIndex.HasValue)
				{
					m_gamepadSortIndex = 0;
				}
				int num = 0;
				bool flag = true;
				do
				{
					m_gamepadSortIndex++;
					num++;
					if (num == m_columnsMetaData.Count)
					{
						flag = false;
						break;
					}
					if (m_gamepadSortIndex >= m_columnsMetaData.Count)
					{
						m_gamepadSortIndex = 0;
					}
				}
				while (m_columnsMetaData[m_gamepadSortIndex.Value].AscendingComparison == null);
				if (flag)
				{
					SortByColumn(m_gamepadSortIndex.Value);
				}
			}
			return captureInput;
		}

		private void HandleNewMousePress(ref MyGuiControlBase captureInput)
		{
			bool flag = m_rowsArea.Contains(MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft());
			MyMouseButtonsEnum mouseButton = MyMouseButtonsEnum.None;
			if (MyInput.Static.IsNewPrimaryButtonPressed())
			{
				mouseButton = MyMouseButtonsEnum.Left;
			}
			else if (MyInput.Static.IsNewSecondaryButtonPressed())
			{
				mouseButton = MyMouseButtonsEnum.Right;
			}
			else if (MyInput.Static.IsNewMiddleMousePressed())
			{
				mouseButton = MyMouseButtonsEnum.Middle;
			}
			else if (MyInput.Static.IsNewXButton1MousePressed())
			{
				mouseButton = MyMouseButtonsEnum.XButton1;
			}
			else if (MyInput.Static.IsNewXButton2MousePressed())
			{
				mouseButton = MyMouseButtonsEnum.XButton2;
			}
			int num = ComputeRowIndexFromPosition(MyGuiManager.MouseCursorPosition);
			EventArgs args;
			if (MyInput.Static.IsAnyNewMouseOrJoystickPressed() && flag)
			{
				captureInput = this;
				if (base.SelectedRowIndex.HasValue && base.HasItemSelectedEvent)
				{
					args = new EventArgs
					{
						RowIndex = base.SelectedRowIndex.Value,
						MouseButton = mouseButton
					};
					RaiseItemSelected(this, args);
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
			}
			if (!MyInput.Static.IsNewPrimaryButtonPressed())
			{
				return;
			}
			if (m_mouseOverHeader)
			{
				SortByColumn(m_mouseOverColumnIndex.Value);
				if (base.HasColumnClickedEvent)
				{
					RaiseColumnClicked(this, m_mouseOverColumnIndex.Value);
				}
			}
			else
			{
				if (!flag)
				{
					return;
				}
				if (!m_doubleClickStarted.HasValue)
				{
					m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					m_doubleClickFirstPosition = MyGuiManager.MouseCursorPosition;
				}
				else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f && (m_doubleClickFirstPosition - MyGuiManager.MouseCursorPosition).Length() <= 0.005f)
				{
					if (base.HasItemDoubleClickedEvent && base.SelectedRowIndex.HasValue)
					{
						args = new EventArgs
						{
							RowIndex = base.SelectedRowIndex.Value,
							MouseButton = mouseButton
						};
						RaiseItemDoubleClicked(this, args);
					}
					m_doubleClickStarted = null;
					captureInput = this;
					return;
				}
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					ToggleSelect(num);
				}
				else if (MyInput.Static.IsAnyShiftKeyPressed())
				{
					base.SelectedRowIndex = num;
					args = new EventArgs
					{
						RowIndex = base.SelectedRowIndex.Value,
						MouseButton = MyMouseButtonsEnum.Left
					};
					RaiseItemSelected(this, args);
					if (!SelectedRowsIndexes.Contains(num))
					{
						SelectedRowsIndexes.Add(num);
					}
				}
				else if (MyInput.Static.IsAnyNewMouseOrJoystickPressed() && flag)
				{
					SelectedRowsIndexes = new List<int> { num };
					captureInput = this;
					if (base.HasItemSelectedEvent)
					{
						args = new EventArgs
						{
							RowIndex = base.SelectedRowIndex.Value,
							MouseButton = mouseButton
						};
						RaiseItemSelected(this, args);
						MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					}
				}
			}
		}

		private void ToggleSelect(int rowIndex)
		{
			EventArgs args;
			if (SelectedRowsIndexes.Contains(rowIndex))
			{
				SelectedRowsIndexes.Remove(rowIndex);
				if (m_selectedRowsIndexes != null && m_selectedRowsIndexes.Count > 0)
				{
					base.SelectedRowIndex = rowIndex;
				}
				else
				{
					base.SelectedRowIndex = -1;
				}
				args = new EventArgs
				{
					RowIndex = base.SelectedRowIndex.Value,
					MouseButton = MyMouseButtonsEnum.Left
				};
				RaiseItemSelected(this, args);
			}
			else
			{
				SelectedRowsIndexes.Add(rowIndex);
				base.SelectedRowIndex = rowIndex;
				args = new EventArgs
				{
					RowIndex = base.SelectedRowIndex.Value,
					MouseButton = MyMouseButtonsEnum.Left
				};
				RaiseItemSelected(this, args);
			}
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (currentFocusControl == this)
			{
				if (page)
				{
					return null;
				}
				int num = 0;
				if (MyInput.Static.IsJoystickLastUsed && m_mouseOverRowIndex.HasValue)
				{
					num = m_mouseOverRowIndex.Value;
				}
				else if (base.SelectedRowIndex.HasValue)
				{
					num = base.SelectedRowIndex.Value;
				}
				else
				{
					base.SelectedRowIndex = 0;
				}
				switch (direction)
				{
				case MyDirection.Down:
					num++;
					break;
				case MyDirection.Up:
					num--;
					break;
				case MyDirection.Right:
					return null;
				case MyDirection.Left:
					return null;
				}
				if (num == -1 || num == m_rows.Count)
				{
					return null;
				}
				if (num < 0)
				{
					num = 0;
				}
				if (num > m_rows.Count)
				{
					num = m_rows.Count;
				}
				EventArgs args;
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					if (SelectedRowsIndexes.Contains(num))
					{
						SelectedRowsIndexes.Remove(num);
						if (m_selectedRowsIndexes != null && m_selectedRowsIndexes.Count > 0)
						{
							base.SelectedRowIndex = num;
						}
						else
						{
							base.SelectedRowIndex = -1;
						}
						args = new EventArgs
						{
							RowIndex = base.SelectedRowIndex.Value,
							MouseButton = MyMouseButtonsEnum.Left
						};
						RaiseItemSelected(this, args);
					}
					else
					{
						SelectedRowsIndexes.Add(num);
						base.SelectedRowIndex = num;
						args = new EventArgs
						{
							RowIndex = base.SelectedRowIndex.Value,
							MouseButton = MyMouseButtonsEnum.Left
						};
						RaiseItemSelected(this, args);
					}
				}
				else if (MyInput.Static.IsAnyShiftKeyPressed())
				{
					base.SelectedRowIndex = num;
					args = new EventArgs
					{
						RowIndex = base.SelectedRowIndex.Value,
						MouseButton = MyMouseButtonsEnum.Left
					};
					RaiseItemSelected(this, args);
					if (!SelectedRowsIndexes.Contains(num))
					{
						SelectedRowsIndexes.Add(num);
					}
				}
				else if ((!MyInput.Static.IsJoystickLastUsed && base.SelectedRowIndex.HasValue && base.SelectedRowIndex != num) || (MyInput.Static.IsJoystickLastUsed && num != -1 && m_mouseOverRowIndex != num))
				{
					if (!MyInput.Static.IsJoystickLastUsed)
					{
						SelectedRowsIndexes = new List<int> { num };
					}
					else
					{
						m_mouseOverRowIndex = num;
					}
					args = new EventArgs
					{
						RowIndex = num,
						MouseButton = MyMouseButtonsEnum.Left
					};
					RaiseItemSelected(this, args);
					if (base.Owner == null)
					{
						return null;
					}
					ScrollToSelection(num);
				}
			}
			else
			{
				m_entryPoint = currentFocusControl.FocusRectangle.Center;
				m_entryDirection = direction;
			}
			return this;
		}
	}
}
