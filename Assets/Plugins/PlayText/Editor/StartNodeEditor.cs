using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace GraphSpace
{
    [CustomNodeEditor(typeof(StartNode))]
    public class StartNodeEditor : NodeEditor
    {
        private DialogueGraph dialogueGraph;
        private StartNode startNode;

        public override void OnHeaderGUI()
        {
            GUI.color = Color.white;
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            if (startNode == null) startNode = target as StartNode;
            if (dialogueGraph.current == startNode) GUI.color = Color.blue;
            GUILayout.Label("Start: " + startNode.Language, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
            startNode.name = "Start: " + startNode.Language;
        }

        public override void OnBodyGUI()
        {
            GUI.color = Color.white;
            foreach (XNode.NodePort Port in target.Ports)//画出所有出入点
                NodeEditorGUILayout.PortField(Port);
            serializedObject.Update();
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            if (startNode == null) startNode = target as StartNode;
            if (dialogueGraph.current != null)
            {
                GUILayout.Label("Current: " + dialogueGraph.current.name.ToString(), EditorStyles.label);
            }
            DialogueProfile pro = dialogueGraph.GetProfile();
            if (pro != null)
            {
                if (pro.Language.Count >= 1)
                {
                    int index = pro.Language.FindIndex(Lan => { return Lan.Equals(startNode.Language); });
                    List<string> LanStr = new List<string>();
                    foreach (var str in pro.Language)
                    {
                        LanStr.Add(str);
                    }
                    if (index >= 0)
                    {
                        startNode.Language = pro.Language[EditorGUILayout.Popup("Language", index, LanStr.ToArray())];
                    }
                    else
                    {
                        startNode.Language = pro.Language[EditorGUILayout.Popup("Language", 0, LanStr.ToArray())];
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
