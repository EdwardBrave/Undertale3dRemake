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
    public class BondedUIBinder : MultiColumnTreeElement
    {
        [SerializeField]
        public UIBinder component;

        public BondedUIBinder()
        {
            cells.Add(new CellData(DrawBinderCell, () => component));
        }

        public BondedUIBinder(string name, int depth, int id) : base(name, depth, id)
        {
            component = null;
            cells.Add(new CellData(DrawBinderCell, () => component));
        }
    
        private void DrawBinderCell(Rect cellRect, float offset)
        {
            cellRect.xMin += 5f;
            component = (UIBinder)EditorGUI.ObjectField(cellRect, GUIContent.none, component, typeof(UIBinder), true);
        }

        protected override void InitHeaders(List<MultiColumnHeaderState.Column> headers)
        {
            base.InitHeaders(headers);
            headers.Add(GetDefaultColumn("UIBinder", 100, "child UIBinders in hierarchy with its bonded items"));
        }
    }
}

