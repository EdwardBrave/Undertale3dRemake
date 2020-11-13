using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace GraphSpace
{
    [CustomNodeEditor(typeof(CommentNode))]
    public class CommentNodeEditor : NodeEditor
    {
        private CommentNode commentNode;

        public override void OnBodyGUI()
        {
            GUI.color = Color.white;
            if (commentNode == null) commentNode = target as CommentNode;
            serializedObject.Update();
            commentNode.Comment = EditorGUILayout.TextArea(commentNode.Comment);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
