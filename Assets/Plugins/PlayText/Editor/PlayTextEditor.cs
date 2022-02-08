using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(PlayText))]
public class PlayTextEditor : Editor
{
    PlayText playText;

    void OnEnable()
    {
        playText = target as PlayText;
    }

    public override void OnInspectorGUI()
    {
        playText = target as PlayText;
        serializedObject.Update();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Profile");
        playText.dialogueProfile = EditorGUILayout.ObjectField(playText.dialogueProfile, typeof(DialogueProfile), false) as DialogueProfile;
        if (playText.dialogueProfile != null)
        {
            if (playText.dialogueProfile.Language.Count >= 1)
            {
                int index = playText.dialogueProfile.Language.FindIndex(Lan => { return Lan.Equals(playText.Language); });
                List<string> LanStr = new List<string>();
                foreach (var str in playText.dialogueProfile.Language)
                {
                    LanStr.Add(str);
                }
                if (index >= 0)
                {
                    playText.Language = playText.dialogueProfile.Language[EditorGUILayout.Popup("Language", index, LanStr.ToArray())];
                }
                else
                {
                    playText.Language = playText.dialogueProfile.Language[EditorGUILayout.Popup("Language", 0, LanStr.ToArray())];
                }
            }
            
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TypingSpeed"));
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("canvas"));
        playText.Root = EditorGUILayout.ObjectField(new GUIContent { text = "Dialogue Root" }, playText.Root, typeof(RectTransform), true) as RectTransform;
        playText.Bubble = EditorGUILayout.ObjectField(new GUIContent { text = "Bubble" }, playText.Bubble, typeof(Image), true) as Image;
        playText.DisplayText = EditorGUILayout.ObjectField(new GUIContent { text = "Display Text" }, playText.DisplayText, typeof(TMP_Text), true) as TMP_Text;
        playText.DisplayPanel = EditorGUILayout.ObjectField(new GUIContent { text = "Display Panel" }, playText.DisplayPanel, typeof(RectTransform), true) as RectTransform;
        playText.BubblePointer = EditorGUILayout.ObjectField(new GUIContent { text = "Bubble Pointer" }, playText.BubblePointer, typeof(Image), true) as Image;
        playText.OptionPanel = EditorGUILayout.ObjectField(new GUIContent { text = "Option Panel" }, playText.OptionPanel, typeof(RectTransform), true) as RectTransform;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NextIcon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultWidth"));
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OptionObject"));
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BubblePositionCurve"));
        EditorGUILayout.Space(10);
        playText.BubbleRange = EditorGUILayout.Toggle("Enable Bubble Range", playText.BubbleRange);
        if (playText.BubbleRange)
        {
            playText.BubbleRangePosition = EditorGUILayout.Vector2Field("Bubble Range Postion", playText.BubbleRangePosition);
            playText.BubbleRangeSize = EditorGUILayout.Vector2Field("Bubble Range Scale", playText.BubbleRangeSize);
            CanvasScaler sc = playText.canvas.gameObject.GetComponent<CanvasScaler>();
            if (sc != null)
            {
                if (sc.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    EditorGUILayout.HelpBox("Please choose 'Scale with Screen Size' in Canvas Scaler to enable this function. ", MessageType.Error);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Please add 'Canvas Scaler' component to your Canvas", MessageType.Error);
            }
        }
        EditorGUILayout.Space(10);
        playText.IsBubbleFollow = EditorGUILayout.Toggle("Is Bubble Follow", playText.IsBubbleFollow);
        playText.AllowCameraFollow = EditorGUILayout.Toggle("Allow Camera Follow", playText.AllowCameraFollow);
        EditorGUILayout.Space(10);
        playText.BubblePositionOffset = EditorGUILayout.Vector2Field("Bubble Position Offset", playText.BubblePositionOffset);
        playText.BubbleSizeOffset = EditorGUILayout.Vector2Field("Bubble Size Offset", playText.BubbleSizeOffset);
        playText.PointerOffset = EditorGUILayout.Vector2Field("Pointer Offset", playText.PointerOffset);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PointerLeftOffset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PointerRightOffset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PointerUpOffset"));
        EditorGUILayout.Space(10);
        playText.OptionColor = EditorGUILayout.ColorField("Option Text Color", playText.OptionColor);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ConvertHalfToFull"));
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("State: " + playText.state.ToString());
        EditorGUILayout.LabelField("Current: " + playText.current.ToString());

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
