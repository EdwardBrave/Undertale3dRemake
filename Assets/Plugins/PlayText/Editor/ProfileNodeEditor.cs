using UnityEditor;
using UnityEngine;
using XNodeEditor;
using XNode;

namespace GraphSpace
{
    [CustomNodeEditor(typeof(ProfileNode))]
    public class ProfileNodeEditor : NodeEditor
    {
        private DialogueGraph dialogueGraph;

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            int a = dialogueGraph.ProfileUpdate();
            switch (a)
            {
                case 2:
                    EditorGUILayout.HelpBox("There are more than 1 profile Node. ",MessageType.Error);
                    break;
                default:
                    break;
            }
        }
    }
}

