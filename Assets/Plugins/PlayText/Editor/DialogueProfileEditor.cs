using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DialogueProfile))]
public class DialogueProfileEditor : Editor
{
    DialogueProfile dialogueProfile;
    ReorderableList LanguageList;

    bool LanRender;
    bool RVRender;

    void OnEnable()
    {
        dialogueProfile = target as DialogueProfile;

        LanguageList = new ReorderableList(serializedObject, serializedObject.FindProperty("Language"), false, false, true, true);

        //自定义绘制列表元素
        LanguageList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
        {
            //根据index获取对应元素 
            SerializedProperty item = LanguageList.serializedProperty.GetArrayElementAtIndex(index);
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 2;
            EditorGUI.PropertyField(rect, item, new GUIContent("Element " + index));
        };

        LanguageList.onAddCallback = (ReorderableList list) =>
        {
            for (int i = 0; i < dialogueProfile.ReplaceVariable.Count; i++)
            {
                if(list.index >= 0)
                {
                    dialogueProfile.ReplaceVariable[i].Value.Insert(list.index, string.Empty);
                }
                else
                {
                    dialogueProfile.ReplaceVariable[i].Value.Add(string.Empty);
                }
            }
            ReorderableList.defaultBehaviours.DoAddButton(list);
        };

        LanguageList.onRemoveCallback = (ReorderableList list) =>
        {
            for (int i = 0; i < dialogueProfile.ReplaceVariable.Count; i++)
            {
                dialogueProfile.ReplaceVariable[i].Value.RemoveAt(list.index);
            }
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        };

    }

    public override void OnInspectorGUI()
    {
        dialogueProfile = target as DialogueProfile;
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TalkingPerson"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultVoice"));

        LanRender = EditorGUILayout.Foldout(LanRender, "Language");
        if (LanRender)
            LanguageList.DoLayoutList();

        RVRender = EditorGUILayout.Foldout(RVRender, "Replace Variable");
        if(RVRender)
        {
            for (int i = 0; i < dialogueProfile.ReplaceVariable.Count; i++)
            {
                dialogueProfile.ReplaceVariable[i].showing = EditorGUILayout.Foldout(dialogueProfile.ReplaceVariable[i].showing, dialogueProfile.ReplaceVariable[i].Variable);
                if (dialogueProfile.ReplaceVariable[i].showing)
                {
                    dialogueProfile.ReplaceVariable[i].Variable = EditorGUILayout.TextField("Variable", dialogueProfile.ReplaceVariable[i].Variable);
                    while (dialogueProfile.ReplaceVariable[i].Value.Count < dialogueProfile.Language.Count)
                    {
                        dialogueProfile.ReplaceVariable[i].Value.Add(string.Empty);
                    }
                    while (dialogueProfile.ReplaceVariable[i].Value.Count > dialogueProfile.Language.Count)
                    {
                        dialogueProfile.ReplaceVariable[i].Value.RemoveAt(dialogueProfile.ReplaceVariable[i].Value.Count - 1);
                    }
                    EditorGUILayout.Separator();
                    for (int a = 0; a < dialogueProfile.Language.Count; a++)
                    {
                        dialogueProfile.ReplaceVariable[i].Value[a] = EditorGUILayout.TextField(dialogueProfile.Language[a], dialogueProfile.ReplaceVariable[i].Value[a]);
                    }
                    if (GUILayout.Button("Remove Variable"))
                    {
                        dialogueProfile.ReplaceVariable.RemoveAt(i);
                    }
                }
            }
            if (GUILayout.Button("Add Variable"))
            {
                dialogueProfile.ReplaceVariable.Add(new ReplaceClass());
                dialogueProfile.ReplaceVariable[dialogueProfile.ReplaceVariable.Count - 1].showing = true;
            }
        }

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
