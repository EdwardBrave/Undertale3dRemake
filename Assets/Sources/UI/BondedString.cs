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
    public class BondedString : MultiColumnTreeElement
    {
        [SerializeField]
        public string str;
    
        public BondedString()
        {
            cells.Add(new CellData(DrawStringCell, () => str));
        }

        public BondedString(string name, int depth, int id) : base(name, depth, id)
        {
            str = null;
            cells.Add(new CellData(DrawStringCell, () => str));
        }
    
        private void DrawStringCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
            str = EditorGUI.TextField(cellRect, str);
        }

        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers);
            headers.Add(GetDefaultColumn("String", 100, "string for the bonded textfield"));
        }
    }
}