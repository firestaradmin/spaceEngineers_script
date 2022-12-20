using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public abstract class MyGuiControlBase : IMyGuiControlsOwner, IVRageGuiControl
	{
		public class Friend
		{
			protected static void SetOwner(MyGuiControlBase control, IMyGuiControlsOwner owner)
			{
				control.Owner = owner;
			}
		}

		public struct NameChangedArgs
		{
			public string OldName;
		}

		private float m_alpha = 1f;

		private const bool DEBUG_CONTROL_FOCUS = false;

		public static bool DEBUG_CONTROL_BORDERS;

		/// <summary>
		/// Status of mouse over in this update.
		/// </summary>
		private bool m_isMouseOver;

		private bool m_canPlaySoundOnMouseOver = true;

		private bool m_canHaveFocus;

		private Vector2 m_minSize = Vector2.Zero;

		private Vector2 m_maxSize = Vector2.PositiveInfinity;

		private string m_name;

		protected bool m_mouseButtonPressed;

		private int m_showToolTipDelay;

		protected bool m_showToolTip;

		private bool m_showToolTipByFocus;

		public bool CanAutoFocusOnInputHandling = true;

		public bool BlockAutofocusOnHandlingOnce;

		protected internal MyToolTips m_toolTip;

		protected Vector2 m_toolTipPosition;

		public bool m_canFocusChildren;

		public readonly MyGuiControls Elements;

		private Thickness m_margin;

		private Vector2 m_position;

		private Vector2 m_size;

		private string m_gamepadHelpText;

		private Vector4 m_colorMask;

		public MyGuiCompositeTexture BackgroundTexture;

		public Vector4 BorderColor;

		public bool BorderEnabled;

		public bool BorderHighlightEnabled;

		public bool DrawWhilePaused;

		public bool SkipForMouseTest;

		private bool m_enabled;

		public bool ShowTooltipWhenDisabled;

		public bool IsHitTestVisible = true;

		/// <summary>
		/// There are some controls, that cannot receive any handle input(control panel for example), thus disable them with this.
		/// </summary>
		public bool IsActiveControl;

		private MyGuiDrawAlignEnum m_originAlign;

		private bool m_visible;

		public MyGuiControlHighlightType HighlightType;

		private bool m_hasHighlight;

		private int x = 1;

		public Vector2 BorderMargin;

		private bool m_isWithinScissor = true;

		public Action<MyGuiControlBase> EnabledChanged;

<<<<<<< HEAD
		private int m_over;
=======
		private int over;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public List<MyGuiControlBase> VisibleElements => Elements.GetVisibleControls();

		public float Alpha
		{
			get
			{
				return m_alpha;
			}
			set
			{
				m_alpha = value;
			}
		}

		public bool CanFocusChildren
		{
			get
			{
				return m_canFocusChildren;
			}
			set
			{
				m_canFocusChildren = value;
				m_canHaveFocus |= value;
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				if (m_name != value)
				{
					string name = m_name;
					m_name = value;
					this.NameChanged.InvokeIfNotNull(this, new NameChangedArgs
					{
						OldName = name
					});
				}
			}
		}

		public IMyGuiControlsOwner Owner { get; private set; }

		public MyToolTips Tooltips => m_toolTip;

		/// <summary>
		/// Position of control's center (normalized and relative to parent screen center (not left/top corner!!!))
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return m_position;
			}
			set
			{
				if (m_position != value)
				{
					m_position = value;
					OnPositionChanged();
				}
			}
		}

		public Thickness Margin
		{
			get
			{
				return m_margin;
			}
			set
			{
				m_margin = value;
			}
		}

		public float PositionY
		{
			get
			{
				return m_position.Y;
			}
			set
			{
				if (m_position.Y != value)
				{
					m_position.Y = value;
					OnPositionChanged();
				}
			}
		}

		public float PositionX
		{
			get
			{
				return m_position.X;
			}
			set
			{
				if (m_position.X != value)
				{
					m_position.X = value;
					OnPositionChanged();
				}
			}
		}

		/// <summary>
		/// Size of control (normalized).
		/// </summary>
		public Vector2 Size
		{
			get
			{
				return m_size;
			}
			set
			{
				value = Vector2.Clamp(value, MinSize, MaxSize);
				if (m_size != value)
				{
					m_size = value;
					OnSizeChanged();
				}
			}
		}

		public RectangleF Rectangle => new RectangleF(GetPositionAbsoluteTopLeft(), m_size);

		public virtual RectangleF FocusRectangle => Rectangle;

		public MyStringId GamepadHelpTextId { get; set; } = MyStringId.NullOrEmpty;


		public string GamepadHelpText
		{
			get
			{
				return m_gamepadHelpText;
			}
			set
			{
				if (m_gamepadHelpText != value)
				{
					m_gamepadHelpText = value;
<<<<<<< HEAD
					this.GamepadHelpTextChanged?.Invoke(this, EventArgs.Empty);
=======
					if (this.GamepadHelpTextChanged != null)
					{
						this.GamepadHelpTextChanged(this, EventArgs.Empty);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		public Vector2 MinSize
		{
			get
			{
				return m_minSize;
			}
			protected set
			{
				if (m_minSize != value)
				{
					m_minSize = value;
					Size = m_size;
				}
			}
		}

		public Vector2 MaxSize
		{
			get
			{
				return m_maxSize;
			}
			protected set
			{
				if (m_maxSize != value)
				{
					m_maxSize = value;
					Size = m_size;
				}
			}
		}

		public Vector4 ColorMask
		{
			get
			{
				return m_colorMask;
			}
			set
			{
				if (m_colorMask != value)
				{
					m_colorMask = value;
					OnColorMaskChanged();
				}
			}
		}

		public int BorderSize { get; set; }

		/// <summary>
		/// False to disable control, disabled controls are skipped when switching with Tab key etc., look implemented atm. only in MyGuiControlButton.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					m_enabled = value;
					OnEnabledChanged();
				}
			}
		}

		public MyGuiDrawAlignEnum OriginAlign
		{
			get
			{
				return m_originAlign;
			}
			set
			{
				if (m_originAlign != value)
				{
					m_originAlign = value;
					OnOriginAlignChanged();
				}
			}
		}

		/// <summary>
		/// Says whether control is visible. Note that this is not a constant time operation (checks parents, fires events on set).
		/// </summary>
		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				if (m_visible != value)
				{
					m_visible = value;
					OnVisibleChanged();
				}
			}
		}

		/// <summary>
		/// Says whether control is currently highlighted. When control is highlit depends on HighlightType.
		/// </summary>
		public bool HasHighlight
		{
			get
			{
				return m_hasHighlight;
			}
			set
			{
				if (m_hasHighlight != value)
				{
					m_hasHighlight = value;
					OnHasHighlightChanged();
<<<<<<< HEAD
					this.HighlightChanged.InvokeIfNotNull(this);
=======
					if (this.HighlightChanged != null)
					{
						this.HighlightChanged(this);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		public bool HasFocus => MyScreenManager.FocusedControl == this;

		public bool IsMouseOver
		{
			get
			{
				return m_isMouseOver;
			}
			set
			{
				if (m_isMouseOver != value)
				{
<<<<<<< HEAD
=======
					if (value && this is MyGuiControlSliderBase)
					{
						_ = x++ % 100;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_isMouseOver = value;
				}
			}
		}

		public bool CanHaveFocus
		{
			get
			{
				return m_canHaveFocus;
			}
			set
			{
				m_canHaveFocus = value;
			}
		}

		public bool CanPlaySoundOnMouseOver
		{
			get
			{
				return m_canPlaySoundOnMouseOver;
			}
			set
			{
				m_canPlaySoundOnMouseOver = value;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Specific user data for this control.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public object UserData { get; set; }

		public string DebugNamePath => Path.Combine((Owner != null) ? Owner.DebugNamePath : "null", Name);

		public int TooltipDelay { get; set; } = MyGuiConstants.SHOW_CONTROL_TOOLTIP_DELAY;


		public bool IsWithinScissor
		{
			get
			{
				return m_isWithinScissor;
			}
			set
			{
				m_isWithinScissor = value;
				IsWithinDrawScissor = value;
			}
		}

		public bool IsWithinDrawScissor { get; set; } = true;


		public event Action<MyGuiControlBase, NameChangedArgs> NameChanged;

		public event Action<MyGuiControlBase> SizeChanged;

		public event VisibleChangedDelegate VisibleChanged;

		public event Action<MyGuiControlBase> HighlightChanged;

		/// <summary>
		/// Called when the control enters or leaves focus.
		/// </summary>
		public event Action<MyGuiControlBase, bool> FocusChanged;

		public event EventHandler GamepadHelpTextChanged;

		public void SetMaxSize(Vector2 maxSize)
		{
			MaxSize = maxSize;
		}

		public void SetMaxWidth(float maxWidth)
		{
			MaxSize = new Vector2(maxWidth, MaxSize.Y);
		}

		protected MyGuiControlBase(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null, string toolTip = null, MyGuiCompositeTexture backgroundTexture = null, bool isActiveControl = true, bool canHaveFocus = false, MyGuiControlHighlightType highlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
		{
			m_canPlaySoundOnMouseOver = true;
			Name = GetType().Name;
			Visible = true;
			m_enabled = true;
			m_position = position ?? Vector2.Zero;
			m_canHaveFocus = canHaveFocus;
			m_size = size ?? Vector2.One;
			m_colorMask = colorMask ?? Vector4.One;
			BackgroundTexture = backgroundTexture;
			IsActiveControl = isActiveControl;
			HighlightType = highlightType;
			m_originAlign = originAlign;
			BorderSize = 1;
			BorderColor = new Vector4(1f, 1f, 1f, 0.5f);
			BorderEnabled = false;
			BorderHighlightEnabled = false;
			DrawWhilePaused = true;
			Elements = new MyGuiControls(this);
			if (toolTip != null)
			{
				m_toolTip = new MyToolTips(toolTip);
			}
		}

		public virtual void Init(MyObjectBuilder_GuiControlBase builder)
		{
			m_position = builder.Position;
			Size = builder.Size;
			Name = builder.Name;
			if (builder.BackgroundColor != Vector4.One)
			{
				ColorMask = builder.BackgroundColor;
			}
			if (builder.ControlTexture != null)
			{
				BackgroundTexture = new MyGuiCompositeTexture
				{
					Center = new MyGuiSizedTexture
					{
						Texture = builder.ControlTexture
					}
				};
			}
			OriginAlign = builder.OriginAlign;
		}

		public virtual MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlBase myObjectBuilder_GuiControlBase = MyGuiControlsFactory.CreateObjectBuilder(this);
			myObjectBuilder_GuiControlBase.Position = m_position;
			myObjectBuilder_GuiControlBase.Size = Size;
			myObjectBuilder_GuiControlBase.Name = Name;
			myObjectBuilder_GuiControlBase.BackgroundColor = ColorMask;
			myObjectBuilder_GuiControlBase.ControlTexture = BackgroundTexture?.Center.Texture;
			myObjectBuilder_GuiControlBase.OriginAlign = OriginAlign;
			return myObjectBuilder_GuiControlBase;
		}

		public static void ReadIfHasValue<T>(ref T target, T? source) where T : struct
		{
			if (source.HasValue)
			{
				target = source.Value;
			}
		}

		public static void ReadIfHasValue(ref Color target, Vector4? source)
		{
			if (source.HasValue)
			{
				target = new Color(source.Value);
			}
		}

		public virtual void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			DrawBackground(backgroundTransitionAlpha);
			DrawElements(transitionAlpha, backgroundTransitionAlpha);
			DrawBorder(transitionAlpha);
		}

		protected virtual void DrawBackground(float transitionAlpha)
		{
			if (BackgroundTexture != null && ColorMask.W > 0f)
			{
				BackgroundTexture.Draw(GetPositionAbsoluteTopLeft(), Size, ApplyColorMaskModifiers(ColorMask, Enabled, transitionAlpha));
			}
		}

		protected void DrawBorder(float transitionAlpha)
		{
			if (DEBUG_CONTROL_BORDERS)
			{
				float num = (float)(MyGuiManager.TotalTimeInMilliseconds % 5000) / 5000f;
				Color color = new Vector3((PositionY + num) % 1f, PositionX / 2f + 0.5f, 1f).HSVtoColor();
				MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft(), Size, color, 1);
			}
			else if (BorderEnabled || (BorderHighlightEnabled && HasHighlight))
			{
				Color color2 = ApplyColorMaskModifiers(BorderColor * ColorMask, Enabled, transitionAlpha);
				MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + BorderMargin, Size - BorderMargin * 2f, color2, BorderSize);
			}
		}

		public virtual MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			return null;
		}

		public virtual MyGuiControlBase GetExclusiveInputHandler()
		{
			return GetExclusiveInputHandler(Elements);
		}

		public static MyGuiControlBase GetExclusiveInputHandler(MyGuiControls controls)
		{
			foreach (MyGuiControlBase visibleControl in controls.GetVisibleControls())
			{
				MyGuiControlBase exclusiveInputHandler = visibleControl.GetExclusiveInputHandler();
				if (exclusiveInputHandler != null)
				{
					return exclusiveInputHandler;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns first control, which has mouse over.
		/// </summary>
		public virtual MyGuiControlBase GetMouseOverControl()
		{
			if (IsMouseOver)
			{
				return this;
			}
			return null;
		}

		/// <summary>
		/// Method returns true if input was captured by control, so no other controls, nor screen can use input in this update.
		/// </summary>
		public virtual MyGuiControlBase HandleInput()
		{
			bool isMouseOver = IsMouseOver;
			IsMouseOver = CheckMouseOver();
			if (IsActiveControl)
			{
				m_mouseButtonPressed = IsMouseOver && MyInput.Static.IsPrimaryButtonPressed();
				if (IsMouseOver && !isMouseOver && Enabled && CanPlaySoundOnMouseOver)
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
				}
			}
			if ((IsMouseOver && isMouseOver) || (HasFocus && MyInput.Static.IsJoystickLastUsed))
			{
				m_showToolTipByFocus = !IsMouseOver || !isMouseOver;
				if (!m_showToolTip)
				{
					m_showToolTipDelay = MyGuiManager.TotalTimeInMilliseconds + TooltipDelay;
					m_showToolTip = true;
				}
			}
			else if (m_showToolTip)
			{
				m_showToolTip = false;
			}
			return null;
		}

		public virtual void HideToolTip()
		{
			m_showToolTip = false;
		}

		public virtual bool IsMouseOverAnyControl()
		{
			return IsMouseOver;
		}

		public virtual RectangleF? GetScissoringArea()
		{
			if (Owner == null)
			{
				return null;
			}
			return Owner.GetScissoringArea();
		}

		public virtual void GetScissorBounds(ref Vector2 topLeft, ref Vector2 botRight)
		{
			topLeft = GetPositionAbsoluteTopLeft();
			botRight = GetPositionAbsoluteBottomRight();
		}

		public virtual void ShowToolTip()
		{
			foreach (MyGuiControlBase visibleControl in Elements.GetVisibleControls())
			{
				if (visibleControl.IsWithinDrawScissor)
				{
					visibleControl.ShowToolTip();
				}
			}
<<<<<<< HEAD
			if (!m_showToolTip || (!Enabled && !ShowTooltipWhenDisabled) || !IsWithinDrawScissor || MyGuiManager.TotalTimeInMilliseconds <= m_showToolTipDelay || m_toolTip == null || !m_toolTip.HasContent)
=======
			if (!m_showToolTip || (!Enabled && !ShowTooltipWhenDisabled) || !IsWithinScissor || MyGuiManager.TotalTimeInMilliseconds <= m_showToolTipDelay || m_toolTip == null || !m_toolTip.HasContent)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			m_toolTipPosition = GetToolTipPosition();
			if (m_showToolTipByFocus)
			{
				if (HasFocus)
				{
					m_toolTip.Draw(m_toolTipPosition);
				}
				else
				{
					m_showToolTip = false;
				}
			}
			else if (!MyInput.Static.IsJoystickLastUsed)
			{
				if (CheckMouseOver(Size, GetPositionAbsolute(), OriginAlign) || CheckMouseOverInternal())
				{
					m_toolTip.Draw(m_toolTipPosition);
				}
				else
				{
					m_showToolTip = false;
				}
			}
		}

		protected virtual Vector2 GetToolTipPosition()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				RectangleF focusRectangle = FocusRectangle;
				return focusRectangle.Position + focusRectangle.Size / 2f;
			}
			return MyGuiManager.MouseCursorPosition;
		}

		protected virtual bool CheckMouseOverInternal()
		{
			return false;
		}

		public virtual void Update()
		{
			HasHighlight = ShouldHaveHighlight();
			foreach (MyGuiControlBase element in Elements)
			{
				element.Update();
			}
		}

		protected virtual bool ShouldHaveHighlight()
		{
			if (HighlightType == MyGuiControlHighlightType.CUSTOM)
			{
				return HasHighlight;
			}
			if (Enabled && (HighlightType == MyGuiControlHighlightType.WHEN_ACTIVE || HighlightType == MyGuiControlHighlightType.WHEN_CURSOR_OVER) && IsMouseOverOrKeyboardActive())
			{
				if (HighlightType != MyGuiControlHighlightType.WHEN_ACTIVE)
				{
					if (HighlightType == MyGuiControlHighlightType.WHEN_CURSOR_OVER)
					{
						return IsMouseOver;
					}
					return false;
				}
				return true;
			}
			if (Enabled && HighlightType == MyGuiControlHighlightType.WHEN_CURSOR_OVER_OR_FOCUS)
			{
				if (IsMouseOverOrKeyboardActive() || HasFocus)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public virtual bool CheckMouseOver(bool use_IsMouseOverAll = true)
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			RectangleF value = new RectangleF(positionAbsoluteTopLeft, m_size);
			RectangleF result = new RectangleF(0f, 0f, 0f, 0f);
			MyGuiControlBase myGuiControlBase = Owner as MyGuiControlBase;
			bool flag = true;
			while (myGuiControlBase != null && flag)
			{
				if (use_IsMouseOverAll)
				{
					flag &= myGuiControlBase.IsMouseOver;
				}
				Vector2 positionAbsoluteTopLeft2 = myGuiControlBase.GetPositionAbsoluteTopLeft();
				RectangleF value2 = new RectangleF(positionAbsoluteTopLeft2, myGuiControlBase.m_size);
				if (!myGuiControlBase.SkipForMouseTest && (!RectangleF.Intersect(ref value, ref value2, out result) || !IsPointInside(MyGuiManager.MouseCursorPosition, result.Size, result.Position, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)))
				{
					return false;
				}
				myGuiControlBase = myGuiControlBase.Owner as MyGuiControlBase;
			}
			if (flag)
			{
				return CheckMouseOver(Size, GetPositionAbsolute(), OriginAlign);
			}
			return false;
		}

		protected virtual void OnHasHighlightChanged()
		{
			foreach (MyGuiControlBase element in Elements)
			{
				element.HasHighlight = HasHighlight;
			}
		}

		protected virtual void OnPositionChanged()
		{
		}

		protected virtual void OnSizeChanged()
		{
			this.SizeChanged.InvokeIfNotNull(this);
		}

		protected virtual void OnVisibleChanged()
		{
			this.VisibleChanged?.Invoke(this, m_visible);
		}

		protected virtual void OnOriginAlignChanged()
		{
		}

		protected virtual void OnEnabledChanged()
		{
			foreach (MyGuiControlBase element in Elements)
			{
				element.Enabled = m_enabled;
			}
			EnabledChanged.InvokeIfNotNull(this);
		}

		protected virtual void OnColorMaskChanged()
		{
			foreach (MyGuiControlBase element in Elements)
			{
				element.ColorMask = ColorMask;
			}
		}

		public virtual void OnFocusChanged(bool focus)
		{
			this.FocusChanged.InvokeIfNotNull(this, focus);
			Owner?.OnFocusChanged(this, focus);
		}

		/// <summary>
		/// Modifies source color mask using transition alpha and color multiplier in case a control is disabled.
		/// </summary>
		/// <param name="sourceColorMask">Original color mask of the control.</param>
		/// <param name="enabled">Indicates whether disabled color mask should be applied.</param>
		/// <param name="transitionAlpha">Alpha value modified during transition.</param>
		/// <returns></returns>
		public static Color ApplyColorMaskModifiers(Vector4 sourceColorMask, bool enabled, float transitionAlpha)
		{
			Vector4 vector = sourceColorMask;
			if (!enabled)
			{
				vector.X *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.X;
				vector.Y *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.Y;
				vector.Z *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.Z;
				vector.W *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.W;
			}
			vector *= transitionAlpha;
			return new Color(vector);
		}

		public virtual string GetMouseCursorTexture()
		{
			string mouseCursorTexture = MyGuiManager.GetMouseCursorTexture();
			_ = IsMouseOver;
			return mouseCursorTexture;
		}

		public Vector2 GetPositionAbsolute()
		{
			if (Owner != null)
			{
				return Owner.GetPositionAbsoluteCenter() + m_position;
			}
			return m_position;
		}

		public Vector2 GetPositionAbsoluteBottomLeft()
		{
			return GetPositionAbsoluteTopLeft() + new Vector2(0f, Size.Y);
		}

		public Vector2 GetPositionAbsoluteBottomRight()
		{
			return GetPositionAbsoluteTopLeft() + Size;
		}

		public Vector2 GetPositionAbsoluteCenterLeft()
		{
			return GetPositionAbsoluteTopLeft() + new Vector2(0f, Size.Y * 0.5f);
		}

		public Vector2 GetPositionAbsoluteCenterRight()
		{
			return GetPositionAbsoluteTopLeft() + new Vector2(Size.X, Size.Y * 0.5f);
		}

		public Vector2 GetPositionAbsoluteCenter()
		{
			return MyUtils.GetCoordCenterFromAligned(GetPositionAbsolute(), Size, OriginAlign);
		}

		public Vector2 GetPositionAbsoluteTopLeft()
		{
			return MyUtils.GetCoordTopLeftFromAligned(GetPositionAbsolute(), Size, OriginAlign);
		}

		public Vector2 GetPositionAbsoluteTopRight()
		{
			return GetPositionAbsoluteTopLeft() + new Vector2(Size.X, 0f);
		}

		public Vector2? GetSize()
		{
			return Size;
		}

		public virtual void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
		}

		public void SetToolTip(MyToolTips toolTip)
		{
			m_toolTip = toolTip;
		}

		public void SetToolTip(string text)
		{
			SetToolTip(new MyToolTips(text));
		}

		public void SetToolTip(MyStringId text)
		{
			SetToolTip(MyTexts.GetString(text));
		}

		public static bool CheckMouseOver(Vector2 size, Vector2 position, MyGuiDrawAlignEnum originAlign)
		{
			return IsPointInside(MyGuiManager.MouseCursorPosition, size, position, originAlign);
		}

		public static bool IsPointInside(Vector2 queryPoint, Vector2 size, Vector2 position, MyGuiDrawAlignEnum originAlign)
		{
			Vector2 coordCenterFromAligned = MyUtils.GetCoordCenterFromAligned(position, size, originAlign);
			Vector2 vector = coordCenterFromAligned - size / 2f;
			Vector2 vector2 = coordCenterFromAligned + size / 2f;
			if (queryPoint.X >= vector.X && queryPoint.X <= vector2.X && queryPoint.Y >= vector.Y)
			{
				return queryPoint.Y <= vector2.Y;
			}
			return false;
		}

		protected MyGuiScreenBase GetTopMostOwnerScreen()
		{
			IMyGuiControlsOwner owner = Owner;
			while (!(owner is MyGuiScreenBase) && owner != null)
			{
				owner = ((MyGuiControlBase)owner).Owner;
			}
			return owner as MyGuiScreenBase;
		}

		protected bool IsMouseOverOrKeyboardActive()
		{
			MyGuiScreenBase topMostOwnerScreen = GetTopMostOwnerScreen();
			if (topMostOwnerScreen != null)
			{
				MyGuiScreenState state = topMostOwnerScreen.State;
				if ((uint)state <= 1u || state == MyGuiScreenState.UNHIDING)
				{
					if (!IsMouseOver)
					{
						return HasFocus;
					}
					return true;
				}
				return false;
			}
			return false;
		}

		protected virtual void DrawElements(float transitionAlpha, float backgroundTransitionAlpha)
		{
			foreach (MyGuiControlBase visibleControl in Elements.GetVisibleControls())
			{
				if (visibleControl.GetExclusiveInputHandler() != visibleControl)
				{
					visibleControl.Draw(transitionAlpha * visibleControl.Alpha, backgroundTransitionAlpha * visibleControl.Alpha);
				}
			}
		}

		protected MyGuiControlBase HandleInputElements()
		{
			MyGuiControlBase myGuiControlBase = null;
			List<MyGuiControlBase> visibleControls = Elements.GetVisibleControls();
			if (visibleControls == null)
<<<<<<< HEAD
			{
				return null;
			}
			for (int num = visibleControls.Count - 1; num >= 0; num--)
			{
=======
			{
				return null;
			}
			for (int num = visibleControls.Count - 1; num >= 0; num--)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (visibleControls.IsValidIndex(num))
				{
					if (num == 7 && !visibleControls[num].CheckMouseOver())
					{
<<<<<<< HEAD
						m_over++;
						if (m_over % 200 == 0)
=======
						over++;
						if (over % 200 == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							CheckMouseOver(visibleControls[num].Size, visibleControls[num].GetPositionAbsolute(), visibleControls[num].OriginAlign);
						}
					}
					myGuiControlBase = visibleControls[num].HandleInput();
				}
				if (myGuiControlBase != null)
				{
					break;
				}
			}
			return myGuiControlBase;
		}

		protected virtual void ClearEvents()
		{
			this.SizeChanged = null;
			this.VisibleChanged = null;
			this.NameChanged = null;
		}

		/// <summary>
		/// Removes various references and clears event handlers.
		/// </summary>
		public virtual void OnRemoving()
		{
			if (HasFocus)
			{
				GetTopMostOwnerScreen().FocusedControl = null;
			}
			Elements.Clear();
			Owner = null;
			ClearEvents();
		}

		public void GetElementsUnderCursor(Vector2 position, bool visibleOnly, List<MyGuiControlBase> controls)
		{
			if (visibleOnly)
			{
				foreach (MyGuiControlBase visibleControl in Elements.GetVisibleControls())
				{
					if (IsPointInside(position, visibleControl.Size, visibleControl.GetPositionAbsolute(), visibleControl.OriginAlign))
					{
						visibleControl.GetElementsUnderCursor(position, visibleOnly, controls);
						controls.Add(visibleControl);
					}
				}
				return;
			}
			foreach (MyGuiControlBase element in Elements)
			{
				if (IsPointInside(position, element.Size, element.GetPositionAbsolute(), element.OriginAlign))
				{
					element.GetElementsUnderCursor(position, visibleOnly, controls);
					controls.Add(element);
				}
			}
		}

		public MyGuiControlBase GetFocusControl(MyDirection direction, bool page, bool loop)
		{
			if (CanFocusChildren)
			{
				MyGuiControlBase nextFocusControl = GetNextFocusControl(this, direction, page);
				if (nextFocusControl != null)
				{
					return nextFocusControl;
				}
			}
			if (Owner == null)
			{
				return null;
			}
			for (IMyGuiControlsOwner owner = Owner; owner != null; owner = owner.Owner)
			{
				MyGuiControlBase nextFocusControl2 = owner.GetNextFocusControl(this, direction, page);
				if (nextFocusControl2 != null)
				{
					return nextFocusControl2;
				}
			}
			if (loop)
			{
				for (IMyGuiControlsOwner owner = Owner; owner != null; owner = owner.Owner)
				{
					List<MyGuiControlBase> visibleElements = owner.VisibleElements;
					if (visibleElements.Count > 0)
					{
						RectangleF fRect = ((direction != MyDirection.Up && direction != MyDirection.Left) ? new RectangleF(new Vector2(-0.5f, -0.5f), new Vector2(1f, 0.01f)) : new RectangleF(new Vector2(-0.5f, 1.5f), new Vector2(1f, 0.01f)));
						MyGuiControlBase nextFocusControl3 = MyGuiScreenBase.GetNextFocusControl(ref fRect, null, null, visibleElements, null, direction, page);
						if (nextFocusControl3 != null)
						{
							return nextFocusControl3;
						}
					}
				}
			}
			return this;
		}

		public virtual MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			return MyGuiScreenBase.GetNextFocusControl(currentFocusControl, null, Elements.GetVisibleControls(), null, direction, page);
		}

		public virtual void Clear()
		{
		}

		public override string ToString()
		{
			return DebugNamePath;
		}

		public virtual void UpdateMeasure()
		{
		}

		public virtual void UpdateArrange()
		{
		}

		public virtual void CheckIsWithinScissor(RectangleF scissor, bool complete = false)
		{
			Vector2 topLeft = Vector2.Zero;
			Vector2 botRight = Vector2.Zero;
			GetScissorBounds(ref topLeft, ref botRight);
			bool flag = true;
			bool flag2 = true;
			if (complete)
			{
				float num = 0.005f;
				flag2 &= topLeft.X + num > scissor.X && botRight.X - num < scissor.Right;
				flag2 &= topLeft.Y + num > scissor.Y && botRight.Y - num < scissor.Bottom;
			}
			flag &= topLeft.X < scissor.Right && botRight.X > scissor.X;
			flag &= topLeft.Y < scissor.Bottom && botRight.Y > scissor.Y;
			IsWithinScissor = (complete ? flag2 : flag);
			IsWithinDrawScissor = flag;
		}
	}
}
