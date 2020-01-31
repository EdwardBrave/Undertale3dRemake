using System.Collections.Generic;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BondedText : MultiColumnTreeElement
    {
        public Text text;
    
        public BondedText()
        {
            cells.Add(new CellData(DrawImageCell, () => text));
        }

        public BondedText(string name, int depth, int id) : base(name, depth, id)
        {
            text = null;
            cells.Add(new CellData(DrawImageCell, () => text));
        }
    
        private void DrawImageCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
            text = (Text)EditorGUI.ObjectField(cellRect, GUIContent.none, text, typeof(Image), true);
        }

        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers);
            headers.Add(GetDefaultColumn("Image", 100, "bonded image link"));
        }
    }
}