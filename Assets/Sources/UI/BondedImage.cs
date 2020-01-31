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
    public class BondedImage : MultiColumnTreeElement
    {
        [SerializeField]
        public Image component;

        public BondedImage()
        {
            cells.Add(new CellData(DrawImageCell, () => component));
        }

        public BondedImage(string name, int depth, int id) : base(name, depth, id)
        {
            component = null;
            cells.Add(new CellData(DrawImageCell, () => component));
        }
    
        private void DrawImageCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
            component = (Image)EditorGUI.ObjectField(cellRect, GUIContent.none, component, typeof(Image), true);
        }

        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers);
            headers.Add(GetDefaultColumn("Image", 100, "bonded image link"));
        }
    }
}

