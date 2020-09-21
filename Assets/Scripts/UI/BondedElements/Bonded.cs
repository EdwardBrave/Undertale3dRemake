using System;
using System.Collections.Generic;
using UI.TreeDataModel;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.IMGUI.Controls;
#endif
using Object = UnityEngine.Object;

namespace UI.BondedElements
{
    [Serializable]
    public abstract class Bonded<T> : MultiColumnTreeElement
    {

        [SerializeField]
        protected T content;

        protected Bonded()
        {
            cells.Add(new CellData(DrawTextCell, () => content));
        }

        protected Bonded(string name, int depth, int id) : base(name, depth, id)
        {
            content = default(T);
            cells.Add(new CellData(DrawTextCell, () => content));
        }

        protected void DrawTextCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
#if UNITY_EDITOR
            if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object))
                content = Multicast(EditorGUI.ObjectField(cellRect, GUIContent.none, (content as Object), typeof(T), true));
            else if (typeof(T) == typeof(string))
                content = Multicast(EditorGUI.TextArea(cellRect, (content as string)) );
            else if (typeof(T) == typeof(int))
                content = Multicast(EditorGUI.IntField(cellRect, Convert.ToInt32(content)) );
            else if (typeof(T) == typeof(float))
                content = Multicast(EditorGUI.FloatField(cellRect, Convert.ToSingle(content)) );
            else 
                throw new NotImplementedException();
#endif
        }

        private T Multicast(object obj)
        {
            if (obj is T result)
                return result;
            return default(T);
        }

#if UNITY_EDITOR
        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers);
            headers.Add(GetDefaultColumn(typeof(T).Name, 100, "bonded link"));
        }
#endif
    }
}