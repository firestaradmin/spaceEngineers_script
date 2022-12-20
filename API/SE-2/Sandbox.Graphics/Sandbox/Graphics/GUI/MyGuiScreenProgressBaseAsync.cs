using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Utils;

namespace Sandbox.Graphics.GUI
{
	internal abstract class MyGuiScreenProgressBaseAsync<T> : MyGuiScreenProgressBase
	{
		private struct ProgressAction
		{
			public IAsyncResult AsyncResult;

			public ActionDoneHandler<T> ActionDoneHandler;

			public ErrorHandler<T> ErrorHandler;
		}

		private LinkedList<ProgressAction> m_actions = (LinkedList<ProgressAction>)(object)new LinkedList<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>();

		private string m_constructorStackTrace;

		protected MyGuiScreenProgressBaseAsync(MyStringId progressText, MyStringId? cancelText = null)
			: base(progressText, cancelText)
		{
			if (Debugger.IsAttached)
			{
				m_constructorStackTrace = Environment.get_StackTrace();
			}
		}

		protected void AddAction(IAsyncResult asyncResult, ErrorHandler<T> errorHandler = null)
		{
			AddAction(asyncResult, OnActionCompleted, errorHandler);
		}

		protected void AddAction(IAsyncResult asyncResult, ActionDoneHandler<T> doneHandler, ErrorHandler<T> errorHandler = null)
		{
			((LinkedList<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)m_actions).AddFirst((MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction)new ProgressAction
			{
				AsyncResult = asyncResult,
				ActionDoneHandler = doneHandler,
				ErrorHandler = (errorHandler ?? new ErrorHandler<T>(OnError))
			});
		}

		protected void CancelAll()
		{
			((LinkedList<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)m_actions).Clear();
		}

		protected override void OnCancelClick(MyGuiControlButton sender)
		{
			CancelAll();
			base.OnCancelClick(sender);
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			LinkedListNode<ProgressAction> val = ((LinkedList<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)m_actions).get_First();
			while (val != null)
			{
				if (((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Value().AsyncResult.IsCompleted)
				{
					try
					{
						((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Value().ActionDoneHandler(((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Value().AsyncResult, (T)((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Value().AsyncResult.AsyncState);
					}
					catch (Exception exception)
					{
						((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Value().ErrorHandler(exception, (T)((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Value().AsyncResult.AsyncState);
					}
					LinkedListNode<ProgressAction> val2 = val;
					val = ((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Next();
					((LinkedList<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)m_actions).Remove((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val2);
				}
				else
				{
					val = ((LinkedListNode<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)val).get_Next();
				}
			}
			return base.State == MyGuiScreenState.OPENED;
		}

		protected virtual void OnActionCompleted(IAsyncResult asyncResult, T asyncState)
		{
		}

		protected virtual void OnError(Exception exception, T asyncState)
		{
			MyLog.Default.WriteLine(exception);
			throw exception;
		}

		protected void Retry()
		{
			((LinkedList<MyGuiScreenProgressBaseAsync<ProgressAction>.ProgressAction>)(object)m_actions).Clear();
			ProgressStart();
		}
	}
}
