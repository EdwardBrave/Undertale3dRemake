﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace UnityEditorInternal
{
	/// <summary>
	/// Responsible for drawing an 'Event 2.0 for Unity' attribute in Inspector
	/// </summary>
	[CustomPropertyDrawer(typeof(UnityEventBase2), true)]
	public class UnityEvent2Drawer : PropertyDrawer
	{
		class State
		{
			internal ReorderableList m_ReorderableList;
			public int lastSelectedIndex;
		}

		private const string kNoFunctionString = "No Function";

		// Events Path
		internal const string kInstancePath = "m_Target";
		internal const string kMethodNamePath = "m_MethodName";
		internal const string kCallStatePath = "m_CallState";
		internal const string kArgumentsPath = "m_Arguments";
		internal const string kModesPath = "m_Modes";
		internal const string kModePath = "m_Mode";

		//ArgumentCache paths
		internal const string kObjectArgument = "m_ObjectArgument";
		internal const string kIntArgument = "m_IntArgument";
		internal const string kFloatArgument = "m_FloatArgument";
		internal const string kStringArgument = "m_StringArgument";
		internal const string kBoolArgument = "m_BoolArgument";
		internal const string kEnumArgument = "m_EnumArgument";
		internal const string kVector2Argument = "m_Vector2Argument";
		internal const string kVector2IntArgument = "m_Vector2IntArgument";
		internal const string kVector3Argument = "m_Vector3Argument";
		internal const string kVector3IntArgument = "m_Vector3IntArgument";
		internal const string kVector4Argument = "m_Vector4Argument";
		internal const string kLayerMaskArgument = "m_LayerMaskArgument";
		internal const string kColorArgument = "m_ColorArgument";
		internal const string kQuaternionArgument = "m_QuaternionArgument";
		internal const string kObjectArrayArgument = "m_ObjectArrayArgument";
		internal const string kIntArrayArgument = "m_IntArrayArgument";
		internal const string kFloatArrayArgument = "m_FloatArrayArgument";
		internal const string kStringArrayArgument = "m_StringArrayArgument";
		internal const string kBoolArrayArgument = "m_BoolArrayArgument";
		internal const string kEnumArrayArgument = "m_EnumArrayArgument";
		internal const string kVector2ArrayArgument = "m_Vector2ArrayArgument";
		internal const string kVector2IntArrayArgument = "m_Vector2IntArrayArgument";
		internal const string kVector3ArrayArgument = "m_Vector3ArrayArgument";
		internal const string kVector3IntArrayArgument = "m_Vector3IntArrayArgument";
		internal const string kVector4ArrayArgument = "m_Vector4ArrayArgument";
		internal const string kLayerMaskArrayArgument = "m_LayerMaskArrayArgument";
		internal const string kColorArrayArgument = "m_ColorArrayArgument";
		internal const string kQuaternionArrayArgument = "m_QuaternionArrayArgument";

		internal const string kObjectArgumentAssemblyTypeName = "m_ObjectArgumentAssemblyTypeName";

		private const float kSpacing = 5;
		private const int kExtraSpacing = 9;

		private UnityEventBase2 m_DummyEvent;
		private SerializedProperty m_Prop;
		private SerializedProperty m_ListenersArray;
		private string m_Text;

		private ReorderableList m_ReorderableList;
		private int m_LastSelectedIndex;

		private readonly Dictionary<string, State> m_States = new Dictionary<string, State>();

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			RestoreState(property);

			return m_ReorderableList == null ? 0f : m_ReorderableList.GetHeight();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			m_Prop = property;
			m_Text = label.text;

			State state = RestoreState(property);

			OnGUI(position);
			state.lastSelectedIndex = m_LastSelectedIndex;
		}

		private void OnGUI(Rect position)
		{
			if (m_ListenersArray == null || !m_ListenersArray.isArray)
				return;

			m_DummyEvent = GetDummyEvent(m_Prop);
			if (m_DummyEvent == null)
				return;

			if (m_ReorderableList != null)
			{
				var oldIdentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				m_ReorderableList.DoList(position);
				EditorGUI.indentLevel = oldIdentLevel;
			}
		}

		private State RestoreState(SerializedProperty property)
		{
			State state = GetState(property);

			m_ListenersArray = state.m_ReorderableList.serializedProperty;
			m_ReorderableList = state.m_ReorderableList;
			m_LastSelectedIndex = state.lastSelectedIndex;
			m_ReorderableList.index = m_LastSelectedIndex;

			return state;
		}

		private State GetState(SerializedProperty property)
		{
			string key = property.propertyPath;
			State state = null;
			if (m_States.ContainsKey(key))
				state = m_States[key];
			// ensure the cached SerializedProperty is synchronized (case 974069)
			if (state == null || state.m_ReorderableList.serializedProperty.serializedObject != property.serializedObject)
			{
				if (state == null)
					state = new State();

				SerializedProperty listenersArray = property.FindPropertyRelative("m_PersistentCalls.m_Calls");
				state.m_ReorderableList =
					new ReorderableList(property.serializedObject, listenersArray, true, true, true, true)
					{
						drawHeaderCallback = DrawEventHeader,
						drawElementCallback = DrawEventListener,
						elementHeightCallback = GetElementHeight,
						onSelectCallback = OnSelectEvent,
						onReorderCallback = OnReorderEvent,
						onAddCallback = OnAddEvent,
						onRemoveCallback = OnRemoveEvent
					};

				SetupReorderableList(state.m_ReorderableList);

				m_States[key] = state;
			}
			return state;
		}

		private void DrawEventHeader(Rect rect)
		{
			string label = (string.IsNullOrEmpty(m_Text) ? "Event" : m_Text) + GetEventParams(m_DummyEvent);
			float buttonWidth = 15f;

			rect.width -= buttonWidth;
			rect.height = EditorGUIUtility.singleLineHeight;

			Rect trashButtonRect = new Rect(rect.x + rect.width, rect.y, buttonWidth, rect.height);
			GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
			trashButton.tooltip = "Delete all events";

			var tooltipAttr = GetAttribute<TooltipAttribute>(fieldInfo, typeof(TooltipAttribute));

			EditorGUI.LabelField(rect, new GUIContent(label, tooltipAttr != null ? tooltipAttr.tooltip : ""));
			if (GUI.Button(trashButtonRect, trashButton, ReorderableList.defaultBehaviours.preButton))
				RemoveAllEvents();
		}

		private void DrawEventListener(Rect rect, int index, bool isActive, bool isFocused)
		{
			var pListener = m_ListenersArray.GetArrayElementAtIndex(index);

			rect.y++;
			Rect[] subRects = GetRowRects(rect);
			Rect enabledRect = subRects[0];
			Rect goRect = subRects[1];
			Rect functionRect = subRects[2];
			Rect argNameRect = subRects[3];
			Rect argRect = subRects[4];

			// find the current event target...
			var listenerTarget = pListener.FindPropertyRelative(kInstancePath);
			var callState = pListener.FindPropertyRelative(kCallStatePath);
			var methodName = pListener.FindPropertyRelative(kMethodNamePath);
			var arguments = pListener.FindPropertyRelative(kArgumentsPath);
			var modes = pListener.FindPropertyRelative(kModesPath);

			Color c = GUI.backgroundColor;
			GUI.backgroundColor = Color.white;

			EditorGUI.PropertyField(enabledRect, callState, GUIContent.none);

			EditorGUI.BeginChangeCheck();
			{
				GUI.Box(goRect, GUIContent.none);
				EditorGUI.PropertyField(goRect, listenerTarget, GUIContent.none);
				if (EditorGUI.EndChangeCheck())
				{
					methodName.stringValue = null;
					arguments.arraySize = 0;
					modes.arraySize = 0;
				}
			}

			BuildArgumentsField(pListener, argNameRect, argRect, m_DummyEvent);

			var desiredTypes = new Type[arguments.arraySize];

			for (int i = 0; i < arguments.arraySize; i++)
			{
				var argument = arguments.GetArrayElementAtIndex(i);
				var objArgument = argument.FindPropertyRelative(kObjectArgumentAssemblyTypeName);

				if (objArgument != null)
				{
					var desiredArgTypeName = objArgument.stringValue;
					if (!string.IsNullOrEmpty(desiredArgTypeName))
						desiredTypes[i] = Type.GetType(desiredArgTypeName, false) ?? typeof(Object);
				}
				else
					desiredTypes[i] = typeof(Object);
			}

			using (new EditorGUI.DisabledScope(listenerTarget.objectReferenceValue == null))
			{
				EditorGUI.BeginProperty(functionRect, GUIContent.none, methodName);
				{
					GUIContent buttonContent;
					if (EditorGUI.showMixedValue)
					{
						buttonContent = new GUIContent("\u2014", "Mixed Values");
					}
					else
					{
						var buttonLabel = new StringBuilder();
						if (listenerTarget.objectReferenceValue == null || string.IsNullOrEmpty(methodName.stringValue))
						{
							buttonLabel.Append(kNoFunctionString);
						}
						else if (!IsListenerValid(listenerTarget.objectReferenceValue, methodName.stringValue, desiredTypes))
						{
							var instanceString = "UnknownComponent";
							var instance = listenerTarget.objectReferenceValue;
							if (instance != null)
								instanceString = instance.GetType().Name;

							buttonLabel.Append(string.Format("<Missing {0}.{1}>", instanceString, methodName.stringValue));
						}
						else
						{
							buttonLabel.Append(listenerTarget.objectReferenceValue.GetType().Name);

							if (!string.IsNullOrEmpty(methodName.stringValue))
							{
								buttonLabel.Append(".");
								if (methodName.stringValue.StartsWith("set_"))
									buttonLabel.Append(methodName.stringValue.Substring(4));
								else
									buttonLabel.Append(methodName.stringValue);
							}
						}
						buttonContent = new GUIContent(buttonLabel.ToString());
					}

					if (GUI.Button(functionRect, buttonContent, EditorStyles.popup))
						BuildPopupList(listenerTarget.objectReferenceValue, m_DummyEvent, pListener).DropDown(functionRect);
				}
				EditorGUI.EndProperty();
			}

			GUI.backgroundColor = c;
		}

		private float GetElementHeight(int index)
		{
			var pListener = m_ListenersArray.GetArrayElementAtIndex(index);
			var arguments = pListener.FindPropertyRelative(kArgumentsPath);
			var modes = pListener.FindPropertyRelative(kModesPath);

			var height = m_ReorderableList.elementHeight;
			var arrayArgumentsHeight = 0f;
			for (int i = 0; i < modes.arraySize; i++)
			{
				if (modes.GetArrayElementAtIndex(i).enumValueIndex == (int)PersistentListenerMode2.Array)
				{
					var argument = arguments.GetArrayElementAtIndex(i);
					var assembly = argument.FindPropertyRelative(kObjectArgumentAssemblyTypeName);

					var desiredArgTypeName = assembly == null ? string.Empty : assembly.stringValue;
					var desiredType = typeof(List<Object>);

					if (!string.IsNullOrEmpty(desiredArgTypeName))
						desiredType = Type.GetType(desiredArgTypeName, false) ?? typeof(List<Object>);

					argument = GetArrayPropertyFromType(argument, desiredType);

					if (argument.isExpanded && argument.arraySize > 0)
						arrayArgumentsHeight += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * argument.arraySize;
				}
			}

			if (arguments.arraySize > 0)
			{
				height += EditorGUIUtility.singleLineHeight;

				if (arguments.arraySize > 1)
				{
					var spacing = kExtraSpacing + (arguments.arraySize - 1) * 2;

					height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight * (arguments.arraySize + 1) + EditorGUIUtility.standardVerticalSpacing + spacing;
				}
			}

			return height + arrayArgumentsHeight;
		}

		private void OnSelectEvent(ReorderableList list)
		{
			m_LastSelectedIndex = list.index;
		}

		private void OnReorderEvent(ReorderableList list)
		{
			if (m_LastSelectedIndex != list.index)
			{
				int from = m_LastSelectedIndex;
				int to = list.index;

				if (from < to)
				{
					while (from < to)
					{
						m_ListenersArray.MoveArrayElement(from, from++);
					}
				}
				else
				{
					while (from > to)
					{
						m_ListenersArray.MoveArrayElement(from, from--);
					}
				}
			}

			m_LastSelectedIndex = list.index;
		}

		private void OnAddEvent(ReorderableList list)
		{
			if (m_ListenersArray.hasMultipleDifferentValues)
			{
				//When increasing a multi-selection array using Serialized Property
				//Data can be overwritten if there is mixed values.
				//The Serialization system applies the Serialized data of one object, to all other objects in the selection.
				//We handle this case here, by creating a SerializedObject for each object.
				//Case 639025.
				foreach (var targetObject in m_ListenersArray.serializedObject.targetObjects)
				{
					var tempSerializedObject = new SerializedObject(targetObject);
					var listenerArrayProperty = tempSerializedObject.FindProperty(m_ListenersArray.propertyPath);
					listenerArrayProperty.arraySize += 1;
					tempSerializedObject.ApplyModifiedProperties();
				}
				m_ListenersArray.serializedObject.SetIsDifferentCacheDirty();
				m_ListenersArray.serializedObject.Update();
				list.index = list.serializedProperty.arraySize - 1;
			}
			else
			{
				ReorderableList.defaultBehaviours.DoAddButton(list);
			}

			m_LastSelectedIndex = list.index;

			var pListener = m_ListenersArray.GetArrayElementAtIndex(list.index);

			var callState = pListener.FindPropertyRelative(kCallStatePath);
			var listenerTarget = pListener.FindPropertyRelative(kInstancePath);
			var methodName = pListener.FindPropertyRelative(kMethodNamePath);
			var modes = pListener.FindPropertyRelative(kModesPath);
			var arguments = pListener.FindPropertyRelative(kArgumentsPath);

			callState.enumValueIndex = (int)UnityEventCallState.RuntimeOnly;
			listenerTarget.objectReferenceValue = null;
			methodName.stringValue = null;
			modes.arraySize = 0;
			arguments.arraySize = 0;
		}

		private void OnRemoveEvent(ReorderableList list)
		{
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
			m_LastSelectedIndex = list.index;
		}

		private void RemoveAllEvents()
		{
			if (m_ListenersArray == null || !m_ListenersArray.isArray)
				return;

			m_ListenersArray.arraySize = 0;
		}

		private UnityEventBase2 GetDummyEvent(SerializedProperty prop)
		{
			//Use the SerializedProperty path to iterate through the fields of the inspected targetObject
			Object tgtobj = prop.serializedObject.targetObject;
			if (tgtobj == null)
				return new UnityEvent2();

			string propPath = prop.propertyPath;
			Type ft = tgtobj.GetType();
			while (propPath.Length != 0)
			{
				//we could have a leftover '.' if the previous iteration handled an array element
				if (propPath.StartsWith("."))
					propPath = propPath.Substring(1);

				var splits = propPath.Split(new[] { '.' }, 2);
				var newField = ft.GetField(splits[0], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (newField == null)
					break;

				ft = newField.FieldType;

				if (ft.IsArray)
					ft = ft.GetElementType();

				if (ft.IsGenericType && typeof(List<>) == ft.GetGenericTypeDefinition())
					ft = ft.GetGenericArguments()[0];

				//the last item in the property path could have been an array element
				//bail early in that case
				if (splits.Length == 1)
					break;

				propPath = splits[1];
				if (propPath.StartsWith("Array.data["))
					propPath = propPath.Split(new[] { ']' }, 2)[1];
			}

			if (ft.IsSubclassOf(typeof(UnityEventBase2)))
				return Activator.CreateInstance(ft) as UnityEventBase2;

			return new UnityEvent2();
		}

		internal string GetEventParams(UnityEventBase2 evt)
		{
			var methodInfo = evt.FindMethod(evt, "Invoke", PersistentListenerMode2.EventDefined);
			var types = methodInfo.GetParameters().Select(x => x.ParameterType).ToArray();

			var sb = new StringBuilder();

			sb.Append(" (");
			for (int i = 0; i < types.Length; i++)
			{
				sb.Append(GetTypeName(types[i]));
				if (i < types.Length - 1)
				{
					sb.Append(", ");
				}
			}
			sb.Append(")");

			return sb.ToString();
		}

		private void SetupReorderableList(ReorderableList list)
		{
			// Two standard lines with standard spacing between and extra spacing below to better separate items visually.
			list.elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + kExtraSpacing;
		}

		private Rect[] GetRowRects(Rect rect)
		{
			Rect[] rects = new Rect[5];

			rect.height = EditorGUIUtility.singleLineHeight;
			rect.y += 2;

			Rect enabledRect = rect;
			enabledRect.width *= 0.3f;

			Rect gameObjectRect = enabledRect;
			gameObjectRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			Rect functionRect = rect;
			functionRect.xMin = gameObjectRect.xMax + kSpacing;

			Rect argNameRect = gameObjectRect;
			argNameRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			Rect argRect = functionRect;
			argRect.y += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;

			rects[0] = enabledRect;
			rects[1] = gameObjectRect;
			rects[2] = functionRect;
			rects[3] = argNameRect;
			rects[4] = argRect;

			return rects;
		}

		private void BuildArgumentsField(SerializedProperty listener, Rect argNameRect, Rect argRect, UnityEventBase2 dummyEvent)
		{
			// figure out the signature of this delegate...
			// The property at this stage points to the 'container' and has the field name
			Type delegateType = dummyEvent.GetType();

			// check out the signature of invoke as this is the callback!
			MethodInfo delegateMethod = delegateType.GetMethod("Invoke");
			var delegateArgumentsTypes = delegateMethod.GetParameters().Select(x => x.ParameterType).ToArray();

			var listenerTarget = listener.FindPropertyRelative(kInstancePath);
			var methodName = listener.FindPropertyRelative(kMethodNamePath);
			var arguments = listener.FindPropertyRelative(kArgumentsPath);
			var modes = listener.FindPropertyRelative(kModesPath);

			if (arguments.arraySize == 0
				|| modes.arraySize == 0)
				return;

			MethodInfo method = null;
			if (listenerTarget.objectReferenceValue != null)
				method = GetMethodInfo(listenerTarget, methodName, arguments);

			if (method != null)
			{
				var customInspectorAttr = GetAttribute<CustomInspectorAttribute>(method, typeof(CustomInspectorAttribute));

				if (customInspectorAttr != null)
				{
					var methodBuilder = UnityEventBase2.GetValidMethodInfo(listenerTarget.objectReferenceValue, customInspectorAttr.methodName, new Type[] { typeof(SerializedProperty), typeof(Rect), typeof(Rect) });

					methodBuilder.Invoke(listenerTarget.objectReferenceValue, new object[] { arguments, argNameRect, argRect });
				}
				else
				{
					ParameterInfo[] parameters = method.GetParameters();

					var hasLayerAttr = GetAttribute<LayerAttribute>(method, typeof(LayerAttribute)) != null;
					var methodSliderAttr = GetAttribute<SliderAttribute>(method, typeof(SliderAttribute));
					var methodIntSliderAttr = GetAttribute<IntSliderAttribute>(method, typeof(IntSliderAttribute));

					for (int i = 0; i < arguments.arraySize; i++)
					{
						var argument = arguments.GetArrayElementAtIndex(i);
						var modeEnum = GetMode(modes.GetArrayElementAtIndex(i));
						var assembly = argument.FindPropertyRelative(kObjectArgumentAssemblyTypeName);

						SliderAttribute paramSliderAttr = null;
						IntSliderAttribute paramIntSliderAttr = null;

						//only allow argument if we have a valid target / method
						if (listenerTarget.objectReferenceValue == null || string.IsNullOrEmpty(methodName.stringValue))
							modeEnum = PersistentListenerMode2.Void;
						else if (!hasLayerAttr && i < parameters.Length)
							hasLayerAttr = parameters[i].IsDefined(typeof(LayerAttribute), false);

						if (methodSliderAttr == null || methodIntSliderAttr == null)
						{
							var attrs = parameters[i].GetCustomAttributes(false);
							foreach (var attr in attrs)
							{
								if (methodSliderAttr == null && attr is SliderAttribute)
									paramSliderAttr = (SliderAttribute)attr;

								if (methodIntSliderAttr == null && attr is IntSliderAttribute)
									paramIntSliderAttr = (IntSliderAttribute)attr;

								if (attr is CustomInspectorAttribute)
									customInspectorAttr = (CustomInspectorAttribute)attr;
							}
						}

						switch (modeEnum)
						{
							case PersistentListenerMode2.Object:
								argument = argument.FindPropertyRelative(kObjectArgument);
								break;
							case PersistentListenerMode2.Int:
								argument = argument.FindPropertyRelative(kIntArgument);
								break;
							case PersistentListenerMode2.Float:
								argument = argument.FindPropertyRelative(kFloatArgument);
								break;
							case PersistentListenerMode2.String:
								argument = argument.FindPropertyRelative(kStringArgument);
								break;
							case PersistentListenerMode2.Bool:
								argument = argument.FindPropertyRelative(kBoolArgument);
								break;
							case PersistentListenerMode2.Vector2:
								argument = argument.FindPropertyRelative(kVector2Argument);
								break;
							case PersistentListenerMode2.Vector2Int:
								argument = argument.FindPropertyRelative(kVector2IntArgument);
								break;
							case PersistentListenerMode2.Vector3:
								argument = argument.FindPropertyRelative(kVector3Argument);
								break;
							case PersistentListenerMode2.Vector3Int:
								argument = argument.FindPropertyRelative(kVector3IntArgument);
								break;
							case PersistentListenerMode2.Vector4:
								argument = argument.FindPropertyRelative(kVector4Argument);
								break;
							case PersistentListenerMode2.LayerMask:
								argument = argument.FindPropertyRelative(kLayerMaskArgument);
								break;
							case PersistentListenerMode2.Color:
								argument = argument.FindPropertyRelative(kColorArgument);
								break;
							case PersistentListenerMode2.Quaternion:
								argument = argument.FindPropertyRelative(kQuaternionArgument);
								break;
							case PersistentListenerMode2.Array:
								var desiredArgTypeName = assembly == null ? string.Empty : assembly.stringValue;
								var desiredType = typeof(List<Object>);

								if (!string.IsNullOrEmpty(desiredArgTypeName))
									desiredType = Type.GetType(desiredArgTypeName, false) ?? typeof(List<Object>);

								argument = GetArrayPropertyFromType(argument, desiredType);

								break;
							default:
								argument = argument.FindPropertyRelative(kIntArgument);
								break;
						}

						if (argument == null)
							continue;

						if (customInspectorAttr != null)
						{
							var methodBuilder = UnityEventBase2.GetValidMethodInfo(listenerTarget.objectReferenceValue, customInspectorAttr.methodName, new Type[] { typeof(SerializedProperty), typeof(Rect), typeof(Rect) });

							methodBuilder.Invoke(listenerTarget.objectReferenceValue, new object[] { argument, argNameRect, argRect });
						}
						else
						{
							if (i < parameters.Length)
								EditorGUI.LabelField(argNameRect, new GUIContent(string.Format("{0}:", parameters[i].Name)));

							if (modeEnum == PersistentListenerMode2.EventDefined)
							{
								//if (i < delegateArgumentsTypes.Length && arguments.arraySize > delegateArgumentsTypes.Length)
								if (i < delegateArgumentsTypes.Length)
								{
									var type = Type.GetType(assembly.stringValue, false) ?? typeof(Object);

									if (type == delegateArgumentsTypes[i])
										EditorGUI.LabelField(argRect, "Dynamic " + GetTypeName(type));
								}
							}
							else if (modeEnum == PersistentListenerMode2.Enum)
							{
								Type enumType = Type.GetType(assembly.stringValue, false);
								string[] names = Enum.GetNames(enumType);

								argument.intValue = EditorGUI.Popup(argRect, argument.intValue, names);
							}
							else if (modeEnum == PersistentListenerMode2.Int)
							{
								if (hasLayerAttr)
									argument.intValue = EditorGUI.LayerField(argRect, argument.intValue);
								else if (methodIntSliderAttr != null)
									EditorGUI.IntSlider(argRect, argument, methodIntSliderAttr.minValue, methodIntSliderAttr.maxValue, GUIContent.none);
								else if (paramIntSliderAttr != null)
									EditorGUI.IntSlider(argRect, argument, paramIntSliderAttr.minValue, paramIntSliderAttr.maxValue, GUIContent.none);
								else
									EditorGUI.PropertyField(argRect, argument, GUIContent.none);
							}
							else if (modeEnum == PersistentListenerMode2.Float && (methodSliderAttr != null || paramSliderAttr != null))
							{
								if (methodSliderAttr != null)
									EditorGUI.Slider(argRect, argument, methodSliderAttr.minValue, methodSliderAttr.maxValue, GUIContent.none);
								else if (paramSliderAttr != null)
									EditorGUI.Slider(argRect, argument, paramSliderAttr.minValue, paramSliderAttr.maxValue, GUIContent.none);
							}
							else if (modeEnum == PersistentListenerMode2.Vector2)
							{
								EditorGUI.BeginChangeCheck();
								var result = EditorGUI.Vector2Field(argRect, GUIContent.none, argument.vector2Value);
								if (EditorGUI.EndChangeCheck())
									argument.vector2Value = result;
							}
#if UNITY_2017_2_OR_NEWER
						else if (modeEnum == PersistentListenerMode2.Vector2Int)
						{
							EditorGUI.BeginChangeCheck();
							var result = EditorGUI.Vector2IntField(argRect, GUIContent.none, argument.vector2IntValue);
							if (EditorGUI.EndChangeCheck())
								argument.vector2IntValue = result;
						}
#endif
							else if (modeEnum == PersistentListenerMode2.Vector3)
							{
								EditorGUI.BeginChangeCheck();
								var result = EditorGUI.Vector3Field(argRect, GUIContent.none, argument.vector3Value);
								if (EditorGUI.EndChangeCheck())
									argument.vector3Value = result;
							}
#if UNITY_2017_2_OR_NEWER
						else if (modeEnum == PersistentListenerMode2.Vector3Int)
						{
							EditorGUI.BeginChangeCheck();
							var result = EditorGUI.Vector3IntField(argRect, GUIContent.none, argument.vector3IntValue);
							if (EditorGUI.EndChangeCheck())
								argument.vector3IntValue = result;
						}
#endif
							else if (modeEnum == PersistentListenerMode2.Vector4)
							{
								EditorGUI.BeginChangeCheck();
								var result = EditorGUI.Vector4Field(argRect, GUIContent.none, argument.vector4Value);
								if (EditorGUI.EndChangeCheck())
									argument.vector4Value = result;
							}
							else if (modeEnum == PersistentListenerMode2.Quaternion)
							{
								EditorGUI.BeginChangeCheck();
								var result = EditorGUI.Vector4Field(argRect, GUIContent.none, new Vector4(argument.quaternionValue.x, argument.quaternionValue.y, argument.quaternionValue.z, argument.quaternionValue.w));
								if (EditorGUI.EndChangeCheck())
									argument.quaternionValue = new Quaternion(result.x, result.y, result.z, result.w);
							}
							else if (modeEnum == PersistentListenerMode2.Array)
							{
								var desiredArgTypeName = assembly == null ? string.Empty : assembly.stringValue;
								var desiredType = typeof(List<Object>);

								if (!string.IsNullOrEmpty(desiredArgTypeName))
									desiredType = Type.GetType(desiredArgTypeName, false) ?? typeof(List<Object>);

								var foldoutArgRect = argRect;
								foldoutArgRect.x += 10f;
								foldoutArgRect.width = 10f;

								var arraySizeArgRect = argRect;
								arraySizeArgRect.x += 10f;
								arraySizeArgRect.width -= 10f;

								var itemArgRect = arraySizeArgRect;
								itemArgRect.x += 10f;
								itemArgRect.width -= 10f;
								itemArgRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

								argument.isExpanded = EditorGUI.Foldout(foldoutArgRect, argument.isExpanded, GUIContent.none);

								argument.arraySize = EditorGUI.IntField(arraySizeArgRect, argument.arraySize);

								if (argument.isExpanded)
								{
									for (int j = 0; j < argument.arraySize; j++)
									{
										if (desiredType.IsArray && desiredType.GetElementType().IsEnum)
										{
											string[] names = Enum.GetNames(desiredType.GetElementType());

											argument.GetArrayElementAtIndex(j).intValue = EditorGUI.Popup(itemArgRect, argument.GetArrayElementAtIndex(j).intValue, names);
										}
										else if (desiredType.GetGenericArguments().Length > 0 && desiredType.GetGenericArguments()[0].IsEnum)
										{
											string[] names = Enum.GetNames(desiredType.GetGenericArguments()[0]);

											argument.GetArrayElementAtIndex(j).intValue = EditorGUI.Popup(itemArgRect, argument.GetArrayElementAtIndex(j).intValue, names);
										}
										else if (desiredType == typeof(List<int>) || desiredType == typeof(int[])
											|| desiredType == typeof(List<float>) || desiredType == typeof(float[])
											|| desiredType == typeof(List<string>) || desiredType == typeof(string[])
											|| desiredType == typeof(List<bool>) || desiredType == typeof(bool[])
											|| desiredType == typeof(List<Vector2>) || desiredType == typeof(Vector2[])
											|| desiredType == typeof(List<Vector3>) || desiredType == typeof(Vector3[])
#if UNITY_2017_2_OR_NEWER
											|| desiredType == typeof(List<Vector2Int>) || desiredType == typeof(Vector2Int[])
											|| desiredType == typeof(List<Vector3Int>) || desiredType == typeof(Vector3Int[])
#endif
											|| desiredType == typeof(List<LayerMask>) || desiredType == typeof(LayerMask[])
											|| desiredType == typeof(List<Color>) || desiredType == typeof(Color[]))
										{
											EditorGUI.PropertyField(itemArgRect, argument.GetArrayElementAtIndex(j), GUIContent.none);
										}
										else if (desiredType == typeof(List<Vector4>) || desiredType == typeof(Vector4[]))
										{
											EditorGUI.BeginChangeCheck();
											var result = EditorGUI.Vector4Field(itemArgRect, GUIContent.none, argument.GetArrayElementAtIndex(j).vector4Value);
											if (EditorGUI.EndChangeCheck())
												argument.GetArrayElementAtIndex(j).vector4Value = result;
										}
										else if (desiredType == typeof(List<Quaternion>) || desiredType == typeof(Quaternion[]))
										{
											var quaternionValue = argument.GetArrayElementAtIndex(j).quaternionValue;
											EditorGUI.BeginChangeCheck();
											var result = EditorGUI.Vector4Field(itemArgRect, GUIContent.none, new Vector4(quaternionValue.x, quaternionValue.y, quaternionValue.z, quaternionValue.w));
											if (EditorGUI.EndChangeCheck())
												argument.GetArrayElementAtIndex(j).quaternionValue = new Quaternion(result.x, result.y, result.z, result.w);
										}
										else if (IsValidListType(desiredType) || typeof(Object[]).IsAssignableFrom(desiredType))
										{
											Type type = desiredType.IsArray ? desiredType.GetElementType() : desiredType.GetGenericArguments()[0];

											EditorGUI.BeginChangeCheck();
											var result = EditorGUI.ObjectField(itemArgRect, GUIContent.none, argument.GetArrayElementAtIndex(j).objectReferenceValue, type, true);
											if (EditorGUI.EndChangeCheck())
												argument.GetArrayElementAtIndex(j).objectReferenceValue = result;
										}

										itemArgRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
										argNameRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
										argRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
									}
								}
							}
							else if (modeEnum == PersistentListenerMode2.Object)
							{
								var desiredArgTypeName = assembly == null ? string.Empty : assembly.stringValue;
								var desiredType = typeof(Object);

								if (!string.IsNullOrEmpty(desiredArgTypeName))
									desiredType = Type.GetType(desiredArgTypeName, false) ?? typeof(Object);

								EditorGUI.BeginChangeCheck();
								var result = EditorGUI.ObjectField(argRect, GUIContent.none, argument.objectReferenceValue, desiredType, true);
								if (EditorGUI.EndChangeCheck())
									argument.objectReferenceValue = result;
							}
							else if (modeEnum != PersistentListenerMode2.Void)
								EditorGUI.PropertyField(argRect, argument, GUIContent.none);
						}

						customInspectorAttr = null;

						argNameRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
						argRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					}
				}
			}
		}

		private GenericMenu BuildPopupList(Object target, UnityEventBase2 dummyEvent, SerializedProperty listener)
		{
			//special case for components... we want all the game objects targets there!
			var targetToUse = target;
			if (targetToUse is Component)
				targetToUse = (target as Component).gameObject;

			// find the current event target...
			var methodName = listener.FindPropertyRelative(kMethodNamePath);

			var menu = new GenericMenu();

			menu.AddItem(new GUIContent(kNoFunctionString),
				string.IsNullOrEmpty(methodName.stringValue),
				ClearEventFunction,
				new UnityEventFunction(listener, null, null, null));

			if (targetToUse == null)
				return menu;

			menu.AddSeparator("");

			// figure out the signature of this delegate...
			// The property at this stage points to the 'container' and has the field name
			Type delegateType = dummyEvent.GetType();

			// check out the signature of invoke as this is the callback!
			MethodInfo delegateMethod = delegateType.GetMethod("Invoke");
			var delegateArgumentsTypes = delegateMethod.GetParameters().Select(x => x.ParameterType).ToArray();
			GeneratePopUpForType(menu, targetToUse, false, listener, delegateArgumentsTypes);

			if (targetToUse is GameObject)
			{
				Component[] comps = (targetToUse as GameObject).GetComponents<Component>();
				var duplicateNames = comps.Where(c => c != null).Select(c => c.GetType().Name).GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
				foreach (Component comp in comps)
				{
					if (comp == null)
						continue;

					GeneratePopUpForType(menu, comp, duplicateNames.Contains(comp.GetType().Name), listener, delegateArgumentsTypes);
				}
			}

			return menu;
		}

		private void GeneratePopUpForType(GenericMenu menu, Object target, bool useFullTargetName, SerializedProperty listener, Type[] delegateArgumentsTypes)
		{
			var methods = new List<ValidMethodMap>();
			string targetName = useFullTargetName ? target.GetType().FullName : target.GetType().Name;

			bool didAddDynamic = false;

			// skip 'void' event defined on the GUI as we have a void prebuilt type!
			if (delegateArgumentsTypes.Length != 0)
			{
				GetMethodsForTargetAndMode(target, methods, true, delegateArgumentsTypes);
				if (methods.Count > 0)
				{
					menu.AddDisabledItem(new GUIContent(targetName + "/Dynamic " + string.Join(", ", delegateArgumentsTypes.Select(e => GetTypeName(e)).ToArray())));
					AddMethodsToMenu(menu, listener, methods, targetName, delegateArgumentsTypes);
					didAddDynamic = true;
				}
			}

			methods.Clear();

			GetMethodsForTargetAndMode(target, methods);

			if (methods.Count > 0)
			{
				if (didAddDynamic)
					// AddSeperator doesn't seem to work for sub-menus, so we have to use this workaround instead of a proper separator for now.
					menu.AddItem(new GUIContent(targetName + "/ "), false, null);
				if (delegateArgumentsTypes.Length != 0)
					menu.AddDisabledItem(new GUIContent(targetName + "/Static Parameters"));

				AddMethodsToMenu(menu, listener, methods, targetName);
			}
		}

		private void GetMethodsForTargetAndMode(Object target, List<ValidMethodMap> methods, bool isDynamic = false, Type[] delegateArgumentsTypes = null)
		{
			IEnumerable<ValidMethodMap> newMethods = CalculateMethodMap(target, delegateArgumentsTypes, isDynamic);

			methods.AddRange(newMethods);
		}

		private IEnumerable<ValidMethodMap> CalculateMethodMap(Object target, Type[] types, bool isDynamic)
		{
			var validMethods = new List<ValidMethodMap>();
			if (target == null || (isDynamic && types == null))
				return validMethods;

			// find the methods on the behaviour that match the signature
			Type componentType = target.GetType();
			var componentMethods = componentType.GetMethods().Where(x => !x.IsSpecialName).ToList();

			var wantedProperties = componentType.GetProperties().AsEnumerable();
			wantedProperties = wantedProperties.Where(x => x.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length == 0 && x.GetSetMethod() != null);
			componentMethods.AddRange(wantedProperties.Select(x => x.GetSetMethod()));

			foreach (var componentMethod in componentMethods)
			{
				var componentParameters = componentMethod.GetParameters();

				// Don't show obsolete methods.
				if (componentMethod.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
					continue;

				if (componentMethod.ReturnType != typeof(void))
					continue;

				if (isDynamic)
				{
					if (types.Length > componentParameters.Length)
						continue;

					var modes = new PersistentListenerMode2[componentParameters.Length];

					// if the argument types do not match, no match
					bool parametersMatch = true;
					for (int i = 0; i < types.Length; i++)
					{
						if (componentParameters[i].ParameterType.IsAssignableFrom(types[i]))
							modes[i] = PersistentListenerMode2.EventDefined;
						else
						{
							modes[i] = GetMode(componentParameters[i].ParameterType);
							parametersMatch = false;
						}
					}

					for (int i = types.Length; i < componentParameters.Length; i++)
					{
						modes[i] = GetMode(componentParameters[i].ParameterType);
					}

					// valid dynamic method
					if (parametersMatch)
					{
						var vmm = new ValidMethodMap
						{
							target = target,
							methodInfo = componentMethod,
							modes = modes,
							parameters = componentParameters,
							isDynamic = isDynamic
						};
						validMethods.Add(vmm);
					}
				}
				else
				{
					if (HasInvalidParameter(componentParameters))
						continue;

					var vmm = new ValidMethodMap
					{
						target = target,
						methodInfo = componentMethod,
						modes = GetModes(componentParameters),
						parameters = componentParameters,
						isDynamic = isDynamic
					};
					validMethods.Add(vmm);
				}
			}

			return validMethods;
		}

		public bool IsListenerValid(Object uObject, string methodName, Type[] argumentTypes)
		{
			if (uObject == null || string.IsNullOrEmpty(methodName))
				return false;

			return UnityEventBase2.GetValidMethodInfo(uObject, methodName, argumentTypes) != null;
		}

		private bool HasInvalidParameter(ParameterInfo[] parameters)
		{
			foreach (var parameter in parameters)
				if (!IsValidParameter(parameter))
					return true;

			return false;
		}

		private bool IsValidParameter(ParameterInfo parameter)
		{
			if (parameter.IsOut)
				return false;

			return IsValidType(parameter.ParameterType);
		}

		private bool IsValidType(Type type)
		{
			if (type.IsPrimitive
				|| type.IsEnum
				|| type.IsValueType
				|| type == typeof(string)
				|| type == typeof(GameObject)
				|| typeof(Component).IsAssignableFrom(type))
			{
				return true;
			}

			if (type.IsArray || IsValidListType(type))
				return true;

			if (type.IsGenericTypeDefinition
				|| type.IsGenericParameter
				|| type.IsInterface
				|| type == typeof(object)
				|| type == typeof(Object)
				|| type == typeof(IEnumerator)
				|| type == typeof(Coroutine)
				|| type == typeof(UnityAction)
				|| typeof(IList).IsAssignableFrom(type)
				|| !typeof(Object).IsAssignableFrom(type)
				)
			{
				return false;
			}

			return true;
		}

		private bool IsValidListParameter(ParameterInfo parameter)
		{
			return IsValidListType(parameter.ParameterType);
		}

		private bool IsValidListType(Type type)
		{
			var genericArguments = type.GetGenericArguments();

			return type.IsGenericType
					&& genericArguments.Length == 1
					&& typeof(List<>).MakeGenericType(genericArguments[0]).IsAssignableFrom(type);
		}

		private Type GetListType(Type type)
		{
			return typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]);
		}

		private void AddMethodsToMenu(GenericMenu menu, SerializedProperty listener, List<ValidMethodMap> methods, string targetName, Type[] delegateArgumentsTypes = null)
		{
			// Note: sorting by a bool in OrderBy doesn't seem to work for some reason, so using numbers explicitly.
			IEnumerable<ValidMethodMap> orderedMethods = methods.OrderBy(e => e.methodInfo.Name.StartsWith("set_") ? 0 : 1).ThenBy(e => e.methodInfo.Name);
			foreach (var validMethod in orderedMethods)
				AddFunctionsForScript(menu, listener, validMethod, targetName, delegateArgumentsTypes);
		}

		private void AddFunctionsForScript(GenericMenu menu, SerializedProperty listener, ValidMethodMap method, string targetName, Type[] delegateArgumentsTypes)
		{
			// find the current event target...
			var listenerTarget = listener.FindPropertyRelative(kInstancePath).objectReferenceValue;
			var methodName = listener.FindPropertyRelative(kMethodNamePath).stringValue;
			var modes = listener.FindPropertyRelative(kModesPath);

			var args = new StringBuilder();
			var parameters = method.methodInfo.GetParameters();
			var count = parameters.Length;
			for (int index = 0; index < count; index++)
			{
				var methodArg = parameters[index];
				args.Append(GetTypeName(methodArg.ParameterType));

				if (index < count - 1)
					args.Append(", ");
			}

			var isCurrentlySet = listenerTarget == method.target
				&& methodName == method.methodInfo.Name
				&& CompareModes(modes, method.modes);

			string path = GetFormattedMethodName(targetName, method.methodInfo, args.ToString(), method.isDynamic, delegateArgumentsTypes);

			menu.AddItem(new GUIContent(path),
				isCurrentlySet,
				SetEventFunction,
				new UnityEventFunction(listener, method.target, method.methodInfo, method.modes));
		}

		private bool CompareModes(SerializedProperty modes1, PersistentListenerMode2[] modes2)
		{
			if (modes1 == null || modes2 == null)
				return false;

			if (modes1.arraySize == 1 && modes2.Length == 0)
				if (modes1.GetArrayElementAtIndex(0).enumValueIndex == (int)PersistentListenerMode2.Void)
					return true;

			if (modes1.arraySize != modes2.Length)
				return false;

			for (int i = 0; i < modes1.arraySize; i++)
			{
				if (modes1.GetArrayElementAtIndex(i).enumValueIndex != (int)modes2[i])
					return false;
			}

			return true;
		}

		private PersistentListenerMode2 GetMode(SerializedProperty mode)
		{
			switch (mode.type)
			{
				case "bool":
					return PersistentListenerMode2.Bool;
				case "float":
					return PersistentListenerMode2.Float;
				case "int":
					return PersistentListenerMode2.Int;
				case "string":
					return PersistentListenerMode2.String;
				case "Enum":
					return (PersistentListenerMode2)mode.enumValueIndex;
				default:
					return PersistentListenerMode2.Void;
			}
		}

		private PersistentListenerMode2 GetMode(Type type)
		{
			if (type == typeof(int))
				return PersistentListenerMode2.Int;
			else if (type == typeof(float))
				return PersistentListenerMode2.Float;
			else if (type == typeof(string))
				return PersistentListenerMode2.String;
			else if (type == typeof(bool))
				return PersistentListenerMode2.Bool;
			else if (type.IsEnum)
				return PersistentListenerMode2.Enum;
			else if (type == typeof(Vector2))
				return PersistentListenerMode2.Vector2;
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(Vector2Int))
				return PersistentListenerMode2.Vector2Int;
#endif
			else if (type == typeof(Vector3))
				return PersistentListenerMode2.Vector3;
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(Vector3Int))
				return PersistentListenerMode2.Vector3Int;
#endif
			else if (type == typeof(Vector4))
				return PersistentListenerMode2.Vector4;
			else if (type == typeof(LayerMask))
				return PersistentListenerMode2.LayerMask;
			else if (type == typeof(Color))
				return PersistentListenerMode2.Color;
			else if (type == typeof(Quaternion))
				return PersistentListenerMode2.Quaternion;
			else if (type.IsArray || IsValidListType(type))
				return PersistentListenerMode2.Array;
			else
				return PersistentListenerMode2.Object;
		}

		private PersistentListenerMode2[] GetModes(ParameterInfo[] parameters)
		{
			if (parameters == null)
				return new PersistentListenerMode2[0];

			PersistentListenerMode2[] modes = new PersistentListenerMode2[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
				modes[i] = GetMode(parameters[i].ParameterType);

			return modes;
		}

		private string GetTypeName(Type type)
		{
			if (type == typeof(int))
				return "int";
			if (type == typeof(float))
				return "float";
			if (type == typeof(string))
				return "string";
			if (type == typeof(bool))
				return "bool";
			if (type.IsArray)
				return GetArrayTypeName(type);
			if (IsValidListType(type))
				return GetListTypeName(type);
			return type.Name;
		}

		private string GetArrayTypeName(Type type)
		{
			if (type == typeof(int[]))
				return "int[]";
			else if (type == typeof(float[]))
				return "float[]";
			else if (type == typeof(string[]))
				return "string[]";
			else if (type == typeof(bool[]))
				return "bool[]";
			else if (type == typeof(Vector2[]))
				return "Vector2[]";
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(Vector2Int[]))
				return "Vector2Int[]";
#endif
			else if (type == typeof(Vector3[]))
				return "Vector3[]";
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(Vector3Int[]))
				return "Vector3Int[]";
#endif
			else if (type == typeof(Vector4[]))
				return "Vector4[]";
			else if (type == typeof(LayerMask[]))
				return "LayerMask[]";
			else if (type == typeof(Color[]))
				return "Color[]";
			else if (type == typeof(Quaternion[]))
				return "Quaternion[]";
			else
				return string.Format("{0}[]", type.GetElementType().Name);
		}

		private string GetListTypeName(Type type)
		{
			if (type == typeof(List<int>))
				return "List<int>";
			else if (type == typeof(List<float>))
				return "List<float>";
			else if (type == typeof(List<string>))
				return "List<string>";
			else if (type == typeof(List<bool>))
				return "List<bool>";
			else if (type == typeof(List<Vector2>))
				return "List<Vector2>";
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(List<Vector2Int>))
				return "List<Vector2Int>";
#endif
			else if (type == typeof(List<Vector3>))
				return "List<Vector3>";
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(List<Vector3Int>))
				return "List<Vector3Int>";
#endif
			else if (type == typeof(List<Vector4>))
				return "List<Vector4>";
			else if (type == typeof(List<LayerMask>))
				return "List<LayerMask>";
			else if (type == typeof(List<Color>))
				return "List<Color>";
			else if (type == typeof(List<Quaternion>))
				return "List<Quaternion>";
			else
				return string.Format("List<{0}>", type.GetGenericArguments()[0].Name);
		}

		private string GetFormattedMethodName(string targetName, MethodInfo method, string args, bool dynamic, Type[] delegateArgumentsTypes)
		{
			if (dynamic)
			{
				if (method.Name.StartsWith("set_"))
					return string.Format("{0}/{1}", targetName, method.Name.Substring(4));
				else
				{
					var builder = new StringBuilder();

					var count = delegateArgumentsTypes.Length;
					for (int i = 0; i < count; i++)
					{
						builder.Append("[dynamic]");

						if (i < count - 1)
							builder.Append(", ");
					}

					var parameters = method.GetParameters();
					count = parameters.Length;

					if (count - delegateArgumentsTypes.Length > 0)
						builder.Append(", ");

					for (int i = delegateArgumentsTypes.Length; i < count; i++)
					{
						builder.Append(GetTypeName(parameters[i].ParameterType));

						if (i < count - 1)
							builder.Append(", ");
					}

					return string.Format("{0}/{1} ({2})", targetName, method.Name, builder.ToString());
				}
			}
			else
			{
				if (method.Name.StartsWith("set_"))
					return string.Format("{0}/{2} {1}", targetName, method.Name.Substring(4), args);
				else
					return string.Format("{0}/{1} ({2})", targetName, method.Name, args);
			}
		}

		private MethodInfo GetMethodInfo(SerializedProperty listenerTarget, SerializedProperty methodName, SerializedProperty arguments)
		{
			var desiredTypes = new Type[arguments.arraySize];
			for (int i = 0; i < arguments.arraySize; i++)
			{
				var argument = arguments.GetArrayElementAtIndex(i);
				var objArgument = argument.FindPropertyRelative(kObjectArgumentAssemblyTypeName);

				if (objArgument != null)
				{
					var desiredArgTypeName = objArgument.stringValue;

					if (!string.IsNullOrEmpty(desiredArgTypeName))
						desiredTypes[i] = Type.GetType(desiredArgTypeName, false) ?? typeof(Object);
				}
				else
					desiredTypes[i] = typeof(Object);
			}

			return UnityEventBase2.GetValidMethodInfo(listenerTarget.objectReferenceValue, methodName.stringValue, desiredTypes);
		}

		private static T GetAttribute<T>(MemberInfo memberInfo, Type attributeType, bool inherit = false)
		{
			if (memberInfo != null)
			{
				var attrs = memberInfo.GetCustomAttributes(attributeType, inherit);

				if (attrs.Length > 0)
					return (T)attrs[0];
			}

			return default(T);
		}

		private static T GetAttribute<T>(ParameterInfo parameterInfo, Type attributeType, bool inherit)
		{
			if (parameterInfo != null)
			{
				var attrs = parameterInfo.GetCustomAttributes(attributeType, inherit);

				if (attrs.Length > 0)
					return (T)attrs[0];
			}

			return default(T);
		}

		private SerializedProperty GetArrayPropertyFromType(SerializedProperty argument, Type type)
		{
			if (type == typeof(List<int>) || type == typeof(int[])
				|| type.IsArray && type.GetElementType().IsEnum
				|| type.GetGenericArguments().Length > 0 && type.GetGenericArguments()[0].IsEnum)
				return argument.FindPropertyRelative(kIntArrayArgument);
			else if (type == typeof(List<float>) || type == typeof(float[]))
				return argument.FindPropertyRelative(kFloatArrayArgument);
			else if (type == typeof(List<string>) || type == typeof(string[]))
				return argument.FindPropertyRelative(kStringArrayArgument);
			else if (type == typeof(List<bool>) || type == typeof(bool[]))
				return argument.FindPropertyRelative(kBoolArrayArgument);
			else if (type == typeof(List<Vector2>) || type == typeof(Vector2[]))
				return argument.FindPropertyRelative(kVector2ArrayArgument);
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(List<Vector2Int>) || type == typeof(Vector2Int[]))
				return argument.FindPropertyRelative(kVector2IntArrayArgument);
#endif
			else if (type == typeof(List<Vector3>) || type == typeof(Vector3[]))
				return argument.FindPropertyRelative(kVector3ArrayArgument);
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(List<Vector3Int>) || type == typeof(Vector3Int[]))
				return argument.FindPropertyRelative(kVector3IntArrayArgument);
#endif
			else if (type == typeof(List<Vector4>) || type == typeof(Vector4[]))
				return argument.FindPropertyRelative(kVector4ArrayArgument);
			else if (type == typeof(List<LayerMask>) || type == typeof(LayerMask[]))
				return argument.FindPropertyRelative(kLayerMaskArrayArgument);
			else if (type == typeof(List<Color>) || type == typeof(Color[]))
				return argument.FindPropertyRelative(kColorArrayArgument);
			else if (type == typeof(List<Quaternion>) || type == typeof(Quaternion[]))
				return argument.FindPropertyRelative(kQuaternionArrayArgument);
			else if (IsValidListType(type) || typeof(Object[]).IsAssignableFrom(type))
				return argument.FindPropertyRelative(kObjectArrayArgument);

			return null;
		}

		private void SetEventFunction(object source)
		{
			((UnityEventFunction)source).Assign();
		}

		private void ClearEventFunction(object source)
		{
			((UnityEventFunction)source).Clear();
		}

		struct ValidMethodMap
		{
			public Object target;
			public MethodInfo methodInfo;
			public PersistentListenerMode2[] modes;
			public ParameterInfo[] parameters;
			public bool isDynamic;
		}

		struct UnityEventFunction
		{
			public readonly SerializedProperty m_Listener;
			public readonly Object m_Target;
			public readonly MethodInfo m_Method;
			public readonly PersistentListenerMode2[] m_Modes;

			public UnityEventFunction(SerializedProperty listener, Object target, MethodInfo method, PersistentListenerMode2[] modes)
			{
				m_Listener = listener;
				m_Target = target;
				m_Method = method;
				m_Modes = modes;
			}

			public void Assign()
			{
				// find the current event target...
				var listenerTarget = m_Listener.FindPropertyRelative(kInstancePath);
				var methodName = m_Listener.FindPropertyRelative(kMethodNamePath);
				var arguments = m_Listener.FindPropertyRelative(kArgumentsPath);
				var modes = m_Listener.FindPropertyRelative(kModesPath);

				listenerTarget.objectReferenceValue = m_Target;
				methodName.stringValue = m_Method.Name;

				var argParams = m_Method.GetParameters();

				arguments.arraySize = argParams.Length;
				modes.arraySize = argParams.Length;

				if (modes.arraySize == 0)
				{
					modes.InsertArrayElementAtIndex(0);
					modes.GetArrayElementAtIndex(0).enumValueIndex = (int)PersistentListenerMode2.Void;
				}

				int defaultValue = 0;
				var methodLayerAttr = GetAttribute<LayerAttribute>(m_Method, typeof(LayerAttribute));
				if (methodLayerAttr != null)
					defaultValue = methodLayerAttr.defaultValue;

				for (int i = 0; i < argParams.Length; i++)
				{
					modes.GetArrayElementAtIndex(i).enumValueIndex = (int)m_Modes[i];

					var argument = arguments.GetArrayElementAtIndex(i);

					var fullArgumentType = argument.FindPropertyRelative(kObjectArgumentAssemblyTypeName);

					fullArgumentType.stringValue = argParams[i].ParameterType.AssemblyQualifiedName;

					var paramLayerAttr = GetAttribute<LayerAttribute>(argParams[i], typeof(LayerAttribute), false);
					if (paramLayerAttr != null)
						defaultValue = paramLayerAttr.defaultValue;

					switch (m_Modes[i])
					{
						case PersistentListenerMode2.Object:
							argument.FindPropertyRelative(kObjectArgument).objectReferenceValue = null;
							break;
						case PersistentListenerMode2.Int:
							argument.FindPropertyRelative(kIntArgument).intValue = defaultValue;
							break;
						case PersistentListenerMode2.Float:
							argument.FindPropertyRelative(kFloatArgument).floatValue = 0f;
							break;
						case PersistentListenerMode2.String:
							argument.FindPropertyRelative(kStringArgument).stringValue = null;
							break;
						case PersistentListenerMode2.Bool:
							argument.FindPropertyRelative(kBoolArgument).boolValue = false;
							break;
						case PersistentListenerMode2.Enum:
							argument.FindPropertyRelative(kIntArgument).intValue = 0;
							break;
						case PersistentListenerMode2.Vector2:
							argument.FindPropertyRelative(kVector2Argument).vector2Value = Vector2.zero;
							break;
#if UNITY_2017_2_OR_NEWER
						case PersistentListenerMode2.Vector2Int:
							argument.FindPropertyRelative(kVector2IntArgument).vector2IntValue = Vector2Int.zero;
							break;
#endif
						case PersistentListenerMode2.Vector3:
							argument.FindPropertyRelative(kVector3Argument).vector3Value = Vector3.zero;
							break;
#if UNITY_2017_2_OR_NEWER
						case PersistentListenerMode2.Vector3Int:
							argument.FindPropertyRelative(kVector3IntArgument).vector3IntValue = Vector3Int.zero;
							break;
#endif
						case PersistentListenerMode2.Vector4:
							argument.FindPropertyRelative(kVector4Argument).vector4Value = Vector4.zero;
							break;
						case PersistentListenerMode2.LayerMask:
							argument.FindPropertyRelative(kLayerMaskArgument).intValue = 0;
							break;
						case PersistentListenerMode2.Color:
							argument.FindPropertyRelative(kColorArgument).colorValue = Color.clear;
							break;
						case PersistentListenerMode2.Quaternion:
							argument.FindPropertyRelative(kQuaternionArgument).quaternionValue = Quaternion.Euler(Vector3.zero);
							break;
						case PersistentListenerMode2.Array:
							argument.FindPropertyRelative(kObjectArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kIntArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kFloatArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kStringArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kBoolArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kVector2ArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kVector3ArrayArgument).arraySize = 0;
#if UNITY_2017_2_OR_NEWER
							argument.FindPropertyRelative(kVector2IntArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kVector3IntArrayArgument).arraySize = 0;
#endif
							argument.FindPropertyRelative(kVector4ArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kLayerMaskArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kColorArrayArgument).arraySize = 0;
							argument.FindPropertyRelative(kQuaternionArrayArgument).arraySize = 0;
							break;
						default:
							break;
					}
				}

				m_Listener.serializedObject.ApplyModifiedProperties();
			}

			public void Clear()
			{
				// find the current event target...
				m_Listener.FindPropertyRelative(kMethodNamePath).stringValue = null;
				m_Listener.FindPropertyRelative(kArgumentsPath).arraySize = 0;
				m_Listener.FindPropertyRelative(kModesPath).arraySize = 0;

				m_Listener.serializedObject.ApplyModifiedProperties();
			}
		}
	}
}