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
    public class BondedUIConfig : MultiColumnTreeElement
    {
        [SerializeField]
        public UIConfig config;

        public BondedUIConfig()
        {
            cells.Add(new CellData(DrawImageCell, () => config));
        }

        public BondedUIConfig(string name, int depth, int id) : base(name, depth, id)
        {
            config = null;
            cells.Add(new CellData(DrawImageCell, () => config));
        }
    
        private void DrawImageCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
            config = (UIConfig)EditorGUI.ObjectField(cellRect, GUIContent.none, config, typeof(UIConfig), true);
        }

        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers);
            headers.Add(GetDefaultColumn("UI Config", 100, "UI configuration of items for the bonded game objects"));
        }
    }
}