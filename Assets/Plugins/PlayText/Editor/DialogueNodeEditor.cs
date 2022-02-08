using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace GraphSpace
{
    [CustomNodeEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : NodeEditor
    {
        private DialogueNode dialogueNode;

        private DialogueGraph dialogueGraph;

        public override void OnHeaderGUI()
        {
            GUI.color = Color.white;
            if (dialogueNode == null) dialogueNode = target as DialogueNode;
            if (dialogueGraph == null) dialogueGraph = window.graph as DialogueGraph;
            if (dialogueGraph.current == dialogueNode) GUI.color = Color.blue;
            string temp = dialogueNode.GetBriefDialog();
            dialogueNode.name = temp;
            GUILayout.Label(temp, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            GUI.color = Color.white;
            if (dialogueNode == null) dialogueNode = target as DialogueNode;
            foreach (XNode.NodePort Port in target.Ports)//画出所有出入点
                NodeEditorGUILayout.PortField(Port);
            if (dialogueNode.IsMax)
            {
                serializedObject.Update();
                DialogueProfile pro = dialogueGraph.GetProfile();
                if(pro != null)
                {
                    if(pro.TalkingPerson.Count >= 1)
                    {
                        int index = pro.TalkingPerson.FindIndex(Lan => { return Lan.Name.Equals(dialogueNode.TalkingPerson); });
                        if(index >= 0)
                        {
                            List<string> Names = new List<string>();
                            for (int i = 0; i < pro.TalkingPerson.Count; i++)
                            {
                                Names.Add(pro.TalkingPerson[i].Name);
                            }
                            dialogueNode.TalkingPerson = pro.TalkingPerson[EditorGUILayout.Popup("TalkingPerson", index, Names.ToArray())].Name;


                            int ExpIndex = pro.TalkingPerson[index].Expression.FindIndex(Lan => { return Lan.Equals(dialogueNode.Expression); });
                            if(ExpIndex >= 0)
                            {
                                List<string> Expressions = new List<string>();
                                for (int i = 0; i < pro.TalkingPerson[index].Expression.Count; i++)
                                {
                                    Expressions.Add(pro.TalkingPerson[index].Expression[i]);
                                }
                                dialogueNode.Expression = pro.TalkingPerson[index].Expression[EditorGUILayout.Popup("Expression", ExpIndex, Expressions.ToArray())];
                            }
                            else
                            {
                                List<string> Expressions = new List<string>();
                                for (int i = 0; i < pro.TalkingPerson[index].Expression.Count; i++)
                                {
                                    Expressions.Add(pro.TalkingPerson[index].Expression[i]);
                                }
                                dialogueNode.Expression = pro.TalkingPerson[index].Expression[EditorGUILayout.Popup("Expression", 0, Expressions.ToArray())];
                            }
                        }
                        else
                        {
                            List<string> Names = new List<string>();
                            for (int i = 0; i < pro.TalkingPerson.Count; i++)
                            {
                                Names.Add(pro.TalkingPerson[i].Name);
                            }
                            dialogueNode.TalkingPerson = pro.TalkingPerson[EditorGUILayout.Popup("TalkingPerson", 0, Names.ToArray())].Name;

                            int ExpIndex = pro.TalkingPerson[0].Expression.FindIndex(Lan => { return Lan.Equals(dialogueNode.Expression); });
                            if (ExpIndex >= 0)
                            {
                                List<string> Expressions = new List<string>();
                                for (int i = 0; i < pro.TalkingPerson[0].Expression.Count; i++)
                                {
                                    Expressions.Add(pro.TalkingPerson[0].Expression[i]);
                                }
                                dialogueNode.Expression = pro.TalkingPerson[0].Expression[EditorGUILayout.Popup("Expression", ExpIndex, Expressions.ToArray())];
                            }
                            else
                            {
                                List<string> Expressions = new List<string>();
                                for (int i = 0; i < pro.TalkingPerson[0].Expression.Count; i++)
                                {
                                    Expressions.Add(pro.TalkingPerson[0].Expression[i]);
                                }
                                dialogueNode.Expression = pro.TalkingPerson[0].Expression[EditorGUILayout.Popup("Expression", 0, Expressions.ToArray())];
                            }
                        }
                    }
                }

                //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("讲话人"));
                //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("表情"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Facing"));
                if (dialogueNode.Facing == FacingDirection.FacingPerson)
                {
                    if (pro != null)
                    {
                        if (pro.TalkingPerson.Count >= 1)
                        {
                            int index = pro.TalkingPerson.FindIndex(Lan => { return Lan.Name.Equals(dialogueNode.FacingPerson); });
                            if (index >= 0)
                            {
                                List<string> Names = new List<string>();
                                for (int i = 0; i < pro.TalkingPerson.Count; i++)
                                {
                                    Names.Add(pro.TalkingPerson[i].Name);
                                }
                                dialogueNode.FacingPerson = pro.TalkingPerson[EditorGUILayout.Popup("FacingPerson", index, Names.ToArray())].Name;
                            }
                            else
                            {
                                List<string> Names = new List<string>();
                                for (int i = 0; i < pro.TalkingPerson.Count; i++)
                                {
                                    Names.Add(pro.TalkingPerson[i].Name);
                                }
                                dialogueNode.FacingPerson = pro.TalkingPerson[EditorGUILayout.Popup("FacingPerson", 0, Names.ToArray())].Name;
                            }
                        }
                    }
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowBubble"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("CameraFollow"));
                EditorGUILayout.EndHorizontal();

                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Audio"));
                if(dialogueNode.Audio != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("PlayPerChar"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("PlayTyping"));
                    EditorGUILayout.EndHorizontal();
                }
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Dialogue"));
                if (GUILayout.Button("Minimize", EditorStyles.miniButton))
                    dialogueNode.IsMax = false;
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                if (GUILayout.Button("Maximize", EditorStyles.miniButton))
                    dialogueNode.IsMax = true;
            }
        }
    }
}
