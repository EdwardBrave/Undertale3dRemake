using System;
using System.Collections.Generic;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class BondedSprite : MultiColumnTreeElement
    {
        [SerializeField]
        public Sprite sprite;

        public BondedSprite()
        {
            cells.Add(new CellData(DrawSpriteCell, () => sprite));
        }

        public BondedSprite(string name, int depth, int id) : base(name, depth, id)
        {
            sprite = null;
            cells.Add(new CellData(DrawSpriteCell, () => sprite));
        }
    
        private void DrawSpriteCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
            sprite = (Sprite)EditorGUI.ObjectField(cellRect, GUIContent.none, sprite, typeof(Sprite), true);
        }

        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers);
            headers.Add(GetDefaultColumn("Sprite", 100, "Sprite for the bonded image"));
        }
    }
}

