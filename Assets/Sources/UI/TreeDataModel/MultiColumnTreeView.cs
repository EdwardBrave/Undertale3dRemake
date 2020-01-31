using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UI.TreeDataModel
{
	public class MultiColumnTreeView<T> : TreeViewWithTreeModel<T> where T : MultiColumnTreeElement, new()
	{
		const float kRowHeights = 20f;
		const float kToggleWidth = 18f;

		public static void TreeToList(TreeViewItem root, IList<TreeViewItem> result)
		{
			if (root == null)
				throw new NullReferenceException("root");
			if (result == null)
				throw new NullReferenceException("result");

			result.Clear();

			if (root.children == null)
				return;

			Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
			for (int i = root.children.Count - 1; i >= 0; i--)
				stack.Push(root.children[i]);

			while (stack.Count > 0)
			{
				TreeViewItem current = stack.Pop();
				result.Add(current);

				if (current.hasChildren && current.children[0] != null)
				{
					for (int i = current.children.Count - 1; i >= 0; i--)
					{
						stack.Push(current.children[i]);
					}
				}
			}
		}

		public MultiColumnTreeView(TreeViewState state, MultiColumnHeader multicolumnHeader, TreeModel<T> model) : base(
			state, multicolumnHeader, model)
		{
			// Custom setup
			isRecursive = false;
			rowHeight = kRowHeights;
			columnIndexForTreeFoldouts = 1;
			showAlternatingRowBackgrounds = true;
			showBorder = true;
			customFoldoutYOffset =
				(kRowHeights - EditorGUIUtility.singleLineHeight) *
				0.5f; // center foldout in the row since we also center content. See RowGUI
			extraSpaceBeforeIconAndLabel = kToggleWidth;
			multicolumnHeader.sortingChanged += OnSortingChanged;

			Reload();
		}


		// Note we We only build the visible rows, only the backend has the full tree information. 
		// The treeview only creates info for the row list.
		protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
		{
			var rows = base.BuildRows(root);
			SortIfNeeded(root, rows);
			return rows;
		}

		void OnSortingChanged(MultiColumnHeader multiColumnHeader)
		{
			SortIfNeeded(rootItem, GetRows());
		}

		void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
		{
			if (rows.Count <= 1)
				return;

			if (multiColumnHeader.sortedColumnIndex == -1)
			{
				return; // No column to sort for (just use the order the data are in)
			}

			// Sort the roots of the existing tree items
			SortByMultipleColumns();
			TreeToList(root, rows);
			Repaint();
		}

		void SortByMultipleColumns()
		{
			var sortedColumns = multiColumnHeader.state.sortedColumns;

			if (sortedColumns.Length == 0)
				return;

			var myTypes = rootItem.children.Cast<TreeViewItem<T>>();
			var orderedQuery = InitialOrder(myTypes, sortedColumns);
			for (int i = 1; i < sortedColumns.Length; i++)
			{
				if (multiColumnHeader.IsSortedAscending(sortedColumns[i]))
					orderedQuery = orderedQuery.ThenBy(l => l.Data.cells[sortedColumns[i - 1]].Value);
				else
					orderedQuery = orderedQuery.ThenByDescending(l => l.Data.cells[sortedColumns[i - 1]].Value);
			}

			rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
		}

		IOrderedEnumerable<TreeViewItem<T>> InitialOrder(IEnumerable<TreeViewItem<T>> myTypes, int[] history)
		{
			if (multiColumnHeader.IsSortedAscending(history[0]))
				return myTypes.OrderBy(l => l.Data.cells[history[0]].Value);
			return myTypes.OrderByDescending(l => l.Data.cells[history[0]].Value);
		}


		protected override void RowGUI(RowGUIArgs args)
		{
			var item = (TreeViewItem<T>) args.item;

			for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
			{
				Rect cellRect = args.GetCellRect(i);
				CenterRectUsingSingleLineHeight(ref cellRect);
				float offset = isRecursive ? GetContentIndent(item) : 0f;
				item.Data.cells[args.GetColumn(i)].painter.Invoke(cellRect, offset);
			}
		}

		// Misc
		//--------

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return true;
		}

		public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
		{
			var item = new T();
			var state = new MultiColumnHeaderState(item.ColumnHeaders.ToArray());
			return state;
		}
	}
}