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

        public override void OnCreate()
        {
            base.OnCreate();
            //if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            //DialogueProfile profile = dialogueGraph.GetProfile();
            //for (int i = 0; i < profile.Language.Count; i++)
            //{
            //    StartNode startNode = dialogueGraph.AddNode(typeof(StartNode)) as StartNode;
            //    startNode.position = new Vector2(-504, -56 - (-112 * i));
            //    startNode.Language = profileNode.dialogueProfile.Language[i];
            //}
            //dialogueGraph.RemoveNode
        }

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

