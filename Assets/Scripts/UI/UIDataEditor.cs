#if UNITY_EDITOR
using System.Collections.Generic;
using UI.BondedElements;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UI
{
	[CustomEditor(typeof(UIData))]
    public class UIDataEditor : Editor
    {
	    MultiColumnTreeView<BondedUIData> _dataTreeView;

	    MultiColumnTreeView<BondedSprite> _spritesTreeView;

	    MultiColumnTreeView<BondedString> _stringsTreeView;

	    MultiColumnTreeView<BondedString> _settingsTreeView;

	    UIData Asset => (UIData) target;

		void OnEnable()
		{
			//EditorUtility.SetDirty(Asset);
			Undo.undoRedoPerformed += OnUndoRedoPerformed;
			// UI Configs
			if (Asset.bondedData.Count == 0)
				Asset.bondedData.Add(new BondedUIData("ROOT", -1, 0));
			_dataTreeView = TreeViewInit(Asset.bondedData);
			
			// Sprites 
			if (Asset.bondedSprites.Count == 0)
				Asset.bondedSprites.Add(new BondedSprite("ROOT", -1, 0));
			_spritesTreeView = TreeViewInit(Asset.bondedSprites);
			
			// Strings
			if (Asset.bondedStrings.Count == 0)
				Asset.bondedStrings.Add(new BondedString("ROOT", -1, 0));
			_stringsTreeView = TreeViewInit(Asset.bondedStrings);
			
			// Settings
			if (Asset.settings.Count == 0)
				Asset.settings.Add(new BondedString("ROOT", -1, 0));
			_settingsTreeView = TreeViewInit(Asset.settings);
		}

		private MultiColumnTreeView<T> TreeViewInit<T>(IList<T> boundedData) where T: MultiColumnTreeElement, new()
		{
			
			var treeViewState = new TreeViewState();
			var newHeaderState = MultiColumnTreeView<T>.CreateDefaultMultiColumnHeaderState(100);

			var multiColumnHeader = new MultiColumnHeader(newHeaderState);
			multiColumnHeader.ResizeToFit();
			
			var treeModel = new TreeModel<T>(boundedData);
			var treeView = new MultiColumnTreeView<T>(treeViewState, multiColumnHeader, treeModel);
			treeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
			treeView.Reload();
			return treeView;
		}

		void OnDisable()
		{
			if (target)
				EditorUtility.SetDirty(target);
			Undo.undoRedoPerformed -= OnUndoRedoPerformed;
		}

		void OnUndoRedoPerformed()
		{
			Perform(_dataTreeView, Asset.bondedData);
			Perform(_spritesTreeView, Asset.bondedSprites);
			Perform(_stringsTreeView, Asset.bondedStrings);
			Perform(_settingsTreeView, Asset.settings);
		}

		private static void Perform<T>(MultiColumnTreeView<T> treeView, IList<T> boundedData) where T: MultiColumnTreeElement, new()
		{
			if (treeView == null) return;
			treeView.treeModel.SetData(boundedData);
			treeView.Reload();
		}

		void OnBeforeDroppingDraggedItems(IList<TreeViewItem> draggedRows)
		{
			Undo.RecordObject(Asset, $"Moving {draggedRows.Count} Item{(draggedRows.Count > 1 ? "s" : "")}");
		}

		public override void OnInspectorGUI()
		{
			// Configs ----------------------------------------------------------
			Asset.isShowConfigs = EditorGUILayout.Foldout(Asset.isShowConfigs, "Data files bindings data");
			if (Asset.isShowConfigs)
				DrawTreeView(_dataTreeView, Asset);

			// Sprites ----------------------------------------------------------
			Asset.isShowSprites = EditorGUILayout.Foldout(Asset.isShowSprites, "Sprites bindings data");
			if (Asset.isShowSprites)
				DrawTreeView(_spritesTreeView, Asset);

			// Strings ----------------------------------------------------------
			Asset.isShowStrings = EditorGUILayout.Foldout(Asset.isShowStrings, "Strings bindings data");
			if (Asset.isShowStrings)
				DrawTreeView(_stringsTreeView, Asset);

			// Settings ----------------------------------------------------------
			Asset.isShowSettings = EditorGUILayout.Foldout(Asset.isShowSettings, "Additional settings");
			if (Asset.isShowSettings)
				DrawTreeView(_settingsTreeView, Asset);
		}
		
		
		private static void DrawTreeView<T>(MultiColumnTreeView<T> treeView, Object asset) where T:  MultiColumnTreeElement, new()
		{
			const float topToolbarHeight = 20f;
			const float spacing = 2f; 
			
			treeView.DrawToolBar(asset);
			GUILayout.Space(3f);
			
			float totalHeight = treeView.totalHeight + topToolbarHeight + 2 * spacing;
			Rect rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
			Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
			Rect multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
			treeView.DrawSearchBar(toolbarRect);
			treeView.OnGUI(multiColumnTreeViewRect);
			EditorGUILayout.TextArea("",GUI.skin.horizontalSlider);
			GUILayout.Space(10f);
		}
    }
}
#endif
