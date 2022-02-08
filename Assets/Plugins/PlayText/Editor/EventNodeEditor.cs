using UnityEngine;
using XNodeEditor;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace GraphSpace
{
    [CustomNodeEditor(typeof(EventNode))]
    public class EventNodeEditor : NodeEditor
    {
        private EventNode eventNode;
        private DialogueGraph dialogueGraph;

        private ReorderableList EventValue;

        private string[] TypeList = new string[]
        {
            "int",
            "float",
            "string",
            "bool"
        };

        public override void OnCreate()
        {
            base.OnCreate();
            if (eventNode == null) eventNode = target as EventNode;
            EventValue = new ReorderableList(serializedObject, serializedObject.FindProperty("EventValue"), true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    GUI.Label(rect, "Event Value List");
                }
            };

            EventValue.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
            {
                EventValue.elementHeight = 2 * EditorGUIUtility.singleLineHeight + 2;
                EventValueClass evtv = eventNode.EventValue[index];
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y += 2;
                evtv.ValueType = EditorGUI.Popup(rect, "ValueType", evtv.ValueType, TypeList);

                rect.y += EditorGUIUtility.singleLineHeight;
                switch (evtv.ValueType)
                {
                    case 0://int
                        evtv.intValue = EditorGUI.IntField(rect, "Value", evtv.intValue);
                        break;
                    case 1://float
                        evtv.floatValue = EditorGUI.FloatField(rect, "Value", evtv.floatValue);
                        break;
                    case 2://string
                        evtv.stringValue = EditorGUI.TextField(rect, "Value", evtv.stringValue);
                        break;
                    case 3://bool
                        evtv.boolValue = EditorGUI.Toggle(rect, "Value", evtv.boolValue);
                        break;
                    default:
                        break;
                }
                //EventValue.elementHeight = 2 * EditorGUIUtility.singleLineHeight + 2;
                //EventValueClass evtv = eventNode.EventValue[index];
                //rect.height = EditorGUIUtility.singleLineHeight;
                //rect.y += 2;
                //eventNode.EventValue[index].@object = EditorGUI.ObjectField(rect, "Object", eventNode.EventValue[index].@object, typeof(Object), true);
                //if(eventNode != null)
                //{
                //    List<string> componentString = new List<string>();
                //    GameObject obj = eventNode.EventValue[index].@object as GameObject;
                //    rect.y += EditorGUIUtility.singleLineHeight;
                //    if(obj != null)
                //    {
                //        Component[] com =  obj.GetComponents<Component>();
                //        for (int i = 0; i < com.Length; i++)
                //        {
                //            string ComTemp = com[i].ToString();
                //            ComTemp = ComTemp.Remove(0, com[i].name.Length + 2);
                //            ComTemp = ComTemp.Remove(ComTemp.Length - 1, 1);
                //            System.Reflection.MemberInfo[] ComMember = com[i].GetType().GetMembers();
                //            System.Reflection.PropertyInfo[] ComProperty = com[i].GetType().GetProperties();
                //            System.Reflection.MethodInfo[] ComMethod = com[i].GetType().GetMethods();
                //            System.Reflection.ConstructorInfo[] ComConstr = com[i].GetType().GetConstructors();
                //            List<string> ComMemberString = new List<string>();
                //            //取得所有Member的名字，Members包含Property Method 和 Var
                //            for (int a = 0; a < ComMember.Length; a++)
                //            {
                //                ComMemberString.Add(ComMember[a].Name);
                //            }
                //            //去掉所有Property的名字
                //            for (int a = 0; a < ComProperty.Length; a++)
                //            {
                //                if(ComMemberString.Contains(ComProperty[a].Name) )
                //                {
                //                    ComMemberString.Remove(ComProperty[a].Name);
                //                }
                //            }
                //            //去掉所有Method的名字
                //            for (int a = 0; a < ComMethod.Length; a++)
                //            {
                //                if(ComMemberString.Contains(ComMethod[a].Name))
                //                {
                //                    ComMemberString.Remove(ComMethod[a].Name);
                //                }
                //            }
                //            //去掉所有Constructor的名字
                //            for (int a = 0; a < ComConstr.Length; a++)
                //            {
                //                if (ComMemberString.Contains(ComConstr[a].Name))
                //                {
                //                    ComMemberString.Remove(ComConstr[a].Name);
                //                }
                //            }
                //           for (int a = 0; a < ComMemberString.Count; a++)
                //            {
                //                if(ComMemberString[a] != string.Empty)
                //                {
                //                    componentString.Add(ComTemp + "/" + ComMemberString[a]);
                //                }
                //            }
                //        }
                //        int FindedIndex = componentString.FindIndex(In => { return In == eventNode.EventValue[index].ComponentName + "/" + eventNode.EventValue[index].ValueName; } );
                //        if(FindedIndex >= 0)
                //        {
                //            int result = EditorGUI.Popup(rect, "Value", FindedIndex, componentString.ToArray());
                //            eventNode.EventValue[index].ComponentName = Regex.Match(componentString[result], @".+(?=/)").Value;
                //            eventNode.EventValue[index].ValueName = Regex.Match(componentString[result], @"(?<=/).+").Value;
                //        }
                //        else
                //        {
                //            int result = EditorGUI.Popup(rect, "Value", 0, componentString.ToArray());
                //            if(componentString.Count >= 1)
                //            {
                //                eventNode.EventValue[index].ComponentName = Regex.Match(componentString[result], @".+(?=/)").Value;
                //                eventNode.EventValue[index].ValueName = Regex.Match(componentString[result], @"(?<=/).+").Value;
                //            }
                //        }
                //    }
                //}
            };
        }

        public override void OnHeaderGUI()
        {
            GUI.color = Color.white;
            if (eventNode == null) eventNode = target as EventNode;
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            if (dialogueGraph.current == eventNode) GUI.color = Color.blue;
            GUILayout.Label("Event: " + eventNode.GetBriefEvent(), NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
            eventNode.name = "Event: " + eventNode.GetBriefEvent();
        }

        public override void OnBodyGUI()
        {
            GUI.color = Color.white;
            serializedObject.Update();
            if (eventNode == null) eventNode = target as EventNode;
            foreach (XNode.NodePort Port in target.Ports)
                NodeEditorGUILayout.PortField(Port);
            
            if(eventNode.IsMax == true)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("EventName"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("WaitCallback"));
                EventValue.DoLayoutList();
                if (GUILayout.Button("Minimize", EditorStyles.miniButton))
                    eventNode.IsMax = false;
            }
            else
            {
                if (GUILayout.Button("Maximize", EditorStyles.miniButton))
                    eventNode.IsMax = true;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

