using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using MColumn = UnityEditor.IMGUI.Controls.MultiColumnHeaderState.Column;

namespace UI.TreeDataModel
{
	[Serializable]
	public class MultiColumnTreeElement : TreeElement
	{
		public readonly List<CellData> cells = new List<CellData>();

		public List<MColumn> ColumnHeaders
		{
			get
			{
				var headers = new List<MColumn>();
				InitHeaders(headers);
				return headers;
			}
		}

		public MultiColumnTreeElement()
		{
			cells.Add(new CellData(DrawNameCell, () => name));
		}

		public MultiColumnTreeElement(string name, int depth, int id) : base(name, depth, id)
		{
			this.id = id;
			this.name = name;
			this.depth = depth;
			cells.Add(new CellData(DrawNameCell, () => name));
		}

		void DrawNameCell(Rect cellRect, float offset)
		{
			cellRect.x += offset;
			cellRect.width -= offset;
			name = EditorGUI.TextField(cellRect, name);
		}

		protected virtual void InitHeaders(List<MColumn> headers)
		{
			headers.Add(GetDefaultColumn("Name", 100, "The name of bonded image"));
		}

		protected MColumn GetDefaultColumn(string colName, float width, string tooltip = "")
		{
			return new MColumn()
			{
				headerContent = new GUIContent(colName, tooltip),
				headerTextAlignment = TextAlignment.Center,
				sortedAscending = true,
				sortingArrowAlignment = TextAlignment.Left,
				width = width,
				minWidth = 60,
				autoResize = true,
				allowToggleVisibility = true
			};
		}
	}

	public class CellData
	{
		public readonly Func<object> selector;

		public object Value => selector.Invoke();

		public readonly Action<Rect, float> painter;

		public CellData(Action<Rect, float> painter, Func<object> selector)
		{
			this.painter = painter;
			this.selector = selector;
		}

	}
}