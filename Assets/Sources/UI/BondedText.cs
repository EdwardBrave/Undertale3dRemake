using System;
using System.Collections.Generic;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class BondedText : MultiColumnTreeElement
    {
        [SerializeField]
        public Text component;
    
        public BondedText()
        {
            cells.Add(new CellData(DrawTextCell, () => component));
        }

        public BondedText(string name, int depth, int id) : base(name, depth, id)
        {
            component = null;
            cells.Add(new CellData(DrawTextCell, () => component));
        }
    
        private void DrawTextCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
            component = (Text)EditorGUI.ObjectField(cellRect, GUIContent.none, component, typeof(Text), true);
        }

        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers); 
            headers.Add(GetDefaultColumn("Component", 100, "bonded image link"));
        }
    }
}