using UnityEngine;
using XNodeEditor;

namespace GraphSpace
{
    [CustomNodeEditor(typeof(ReceiveFromCode))]
    public class ReceiveFromCodeEditor : NodeEditor
    {
        private ReceiveFromCode receiveNode; //TODO: ChangeName
        private DialogueGraph dialogueGraph;

        public override void OnHeaderGUI()
        {
            GUI.color = Color.white;
            if (receiveNode == null) receiveNode = target as ReceiveFromCode;
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            if (dialogueGraph.current == receiveNode) GUI.color = Color.blue;
            GUILayout.Label("Receive: " + receiveNode.GetBriefEvent(), NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
            receiveNode.name = "Receive: " + receiveNode.GetBriefEvent();
        }
    }
}

