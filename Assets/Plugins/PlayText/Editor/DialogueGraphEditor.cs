using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace GraphSpace
{
    [CustomNodeGraphEditor(typeof(DialogueGraph))]
    public class DialogueGraphEditor : NodeGraphEditor
    {
        private DialogueGraph dialogueGraph;

        public override void OnOpen()
        {
            base.OnOpen();
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            dialogueGraph.Open();
        }

        public override void OnDropObjects(Object[] objects)
        {
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            dialogueGraph.ProfileUpdate();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            int re = dialogueGraph.ProfileUpdate();
            switch (re)
            {
                case 0:
                    ProfileNode profileNode = CreateNode(typeof(ProfileNode), new Vector2(-504, -136)) as ProfileNode;
                    string[] Profiles = AssetDatabase.FindAssets("t:DialogueProfile");
                    if (Profiles.Length > 0)
                        profileNode.dialogueProfile = AssetDatabase.LoadAssetAtPath<DialogueProfile>(AssetDatabase.GUIDToAssetPath(Profiles[0]));
                    break;
                default:
                    break;
            }
        }

        public override string GetNodeMenuName(System.Type type)
        {
            if (type == typeof(DialogueNode))
            {
                return base.GetNodeMenuName(type);
            }
            else if(type == typeof(OptionNode))
            {
                return base.GetNodeMenuName(type);
            }
            else if (type == typeof(StartNode))
            {
                return base.GetNodeMenuName(type);
            }
            else if (type == typeof(EventNode))
            {
                return base.GetNodeMenuName(type);
            }
            else if (type == typeof(CommentNode))
            {
                return base.GetNodeMenuName(type);
            }
            else if (type == typeof(ReceiveFromCode))
            {
                return base.GetNodeMenuName(type);
            }
            else if (type == typeof(ProfileNode) && dialogueGraph.ProfileUpdate() == 0)
            {
                return base.GetNodeMenuName(type);
            }
            else return null;
        }
    }
}

