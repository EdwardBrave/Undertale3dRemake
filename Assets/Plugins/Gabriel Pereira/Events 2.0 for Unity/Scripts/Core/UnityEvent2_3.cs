﻿using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Events
{
	/// <summary>
	/// Events 2.0 for Unity with three generic types
	/// </summary>
	[System.Serializable]
	public abstract class UnityEvent2<T0, T1, T2> : UnityEventBase2
	{
		private readonly object[] m_InvokeArray = new object[3];

		/// <summary>
		///   <para>Constructor.</para>
		/// </summary>
		public UnityEvent2() { }

		/// <summary>
		/// Add a non persistent listener to the UnityEvent.
		/// </summary>
		/// <param name="call">Callback function.</param>
		public void AddListener(UnityAction<T0, T1, T2> call)
		{
			AddCall(GetDelegate(call));
		}

		/// <summary>
		/// Remove a non persistent listener from the UnityEvent.
		/// </summary>
		/// <param name="call">Callback function.</param>
		public void RemoveListener(UnityAction<T0, T1, T2> call)
		{
			RemoveListener(call.Target, call.Method);
		}

		protected override MethodInfo FindMethod_Impl(object targetObj, string name)
		{
			return GetValidMethodInfo(targetObj, name, new System.Type[3] { typeof(T0), typeof(T1), typeof(T2) });
		}

		internal override BaseInvokableCall2 GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall2<T0, T1, T2>(target, theFunction);
		}

		private static BaseInvokableCall2 GetDelegate(UnityAction<T0, T1, T2> action)
		{
			return new InvokableCall2<T0, T1, T2>(action);
		}

		/// <summary>
		/// Invoke all registered callbacks (runtime and persistent).
		/// </summary>
		/// <param name="arg1">Dynamic argument 1</param>
		/// <param name="arg2">Dynamic argument 2</param>
		/// <param name="arg3">Dynamic argument 3</param>
		public void Invoke(T0 arg1, T1 arg2, T2 arg3)
		{
			List<BaseInvokableCall2> calls = PrepareInvoke(arg1, arg2, arg3);
			for (var i = 0; i < calls.Count; i++)
			{
				var curCall = calls[i] as InvokableCall2<T0, T1, T2>;
				if (curCall != null)
					curCall.Invoke(arg1, arg2, arg3);
				else
				{
					var staticCurCall = calls[i] as InvokableCall2;
					if (staticCurCall != null)
						staticCurCall.Invoke();
					else
					{
						m_InvokeArray[0] = arg1;
						m_InvokeArray[1] = arg2;
						m_InvokeArray[2] = arg3;
						calls[i].Invoke(m_InvokeArray);
					}
				}
			}
		}

		internal void AddPersistentListener(UnityAction<T0, T1, T2> call)
		{
			AddPersistentListener(call, UnityEventCallState.RuntimeOnly);
		}

		internal void AddPersistentListener(UnityAction<T0, T1, T2> call, UnityEventCallState callState)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterPersistentListener(persistentEventCount, call);
			SetPersistentListenerState(persistentEventCount, callState);
		}

		internal void RegisterPersistentListener(int index, UnityAction<T0, T1, T2> call)
		{
			if (call == null)
				Debug.LogWarning("Registering a Listener requires an action");
			else
				RegisterPersistentListener(index, call.Target, call.Method);
		}
	}
}