using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditor.TreeViewExamples;

[CustomEditor(typeof(UIBinder))]
public class BindingsListEditor : Editor
{
	MultiColumnTreeView<BondedImage> _treeView;
	[SerializeField] MultiColumnHeaderState _multiColumnHeaderState;

	UIBinder Asset => (UIBinder) target;


	void OnEnable ()
	{
		Undo.undoRedoPerformed += OnUndoRedoPerformed;
		var treeViewState = new TreeViewState();
		var headerState = MultiColumnTreeView<BondedImage>.CreateDefaultMultiColumnHeaderState(100);
		if (MultiColumnHeaderState.CanOverwriteSerializedFields(_multiColumnHeaderState, headerState))
			MultiColumnHeaderState.OverwriteSerializedFields(_multiColumnHeaderState, headerState);
		_multiColumnHeaderState = headerState;
		
		if (Asset.bondedImages.Count == 0)
			Asset.bondedImages.Add(new BondedImage("ROOT", -1, 0));
		var multiColumnHeader = new MultiColumnHeader(headerState);
		multiColumnHeader.ResizeToFit();
		
		var treeModel = new TreeModel<BondedImage> (Asset.bondedImages);
		_treeView = new MultiColumnTreeView<BondedImage>(treeViewState, multiColumnHeader, treeModel);
		_treeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
		_treeView.Reload();
	}

	void OnDisable ()
	{
		Undo.undoRedoPerformed -= OnUndoRedoPerformed;
	}

	void OnUndoRedoPerformed ()
	{
		if (_treeView != null)
		{
			_treeView.treeModel.SetData (Asset.bondedImages);
			_treeView.Reload ();
		}
	}

	void OnBeforeDroppingDraggedItems (IList<TreeViewItem> draggedRows)
	{
		Undo.RecordObject (Asset, $"Moving {draggedRows.Count} Item{(draggedRows.Count > 1 ? "s" : "")}");
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.Space (5f);
		_treeView.DrawToolBar(Asset);
		GUILayout.Space (3f);
		
		const float topToolbarHeight = 20f;
		const float spacing = 2f;
		float totalHeight = _treeView.totalHeight + topToolbarHeight + 2 * spacing;
		Rect rect = GUILayoutUtility.GetRect (0, 10000, 0, totalHeight);
		Rect toolbarRect = new Rect (rect.x, rect.y, rect.width, topToolbarHeight);
		Rect multiColumnTreeViewRect = new Rect (rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
		_treeView.DrawSearchBar(toolbarRect);
		_treeView.OnGUI(multiColumnTreeViewRect);
	}
}

