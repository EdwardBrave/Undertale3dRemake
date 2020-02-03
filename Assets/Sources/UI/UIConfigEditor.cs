using System.Collections.Generic;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


namespace UI
{
	[CustomEditor(typeof(UIConfig))]
    public class UIConfigEditor : Editor
    {
	    MultiColumnTreeView<BondedUIConfig> _configsTreeView;
	    [SerializeField] MultiColumnHeaderState configsHeaderState;
	    
	    MultiColumnTreeView<BondedSprite> _spritesTreeView;
	    [SerializeField] MultiColumnHeaderState spritesHeaderState;
	    
        MultiColumnTreeView<BondedString> _stringsTreeView;
		[SerializeField] MultiColumnHeaderState stringsHeaderState;

		UIConfig Asset => (UIConfig) target;

		void OnEnable()
		{
			Undo.undoRedoPerformed += OnUndoRedoPerformed;
			
			// UI Configs
			if (Asset.bondedConfigs.Count == 0)
				Asset.bondedConfigs.Add(new BondedUIConfig("ROOT", -1, 0));
			 
			var configsTreeViewState = new TreeViewState();
			var configsHeaderState = MultiColumnTreeView<BondedUIConfig>.CreateDefaultMultiColumnHeaderState(100);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(stringsHeaderState, configsHeaderState))
				MultiColumnHeaderState.OverwriteSerializedFields(stringsHeaderState, configsHeaderState);
			stringsHeaderState = configsHeaderState;
			
			
			var multiColumnHeader = new MultiColumnHeader(configsHeaderState);
			multiColumnHeader.ResizeToFit();
			
			var configsTreeModel = new TreeModel<BondedUIConfig>(Asset.bondedConfigs);
			_configsTreeView = new MultiColumnTreeView<BondedUIConfig>(configsTreeViewState, multiColumnHeader, configsTreeModel);
			_configsTreeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
			_configsTreeView.Reload();
			
			// Sprites 
			if (Asset.bondedSprites.Count == 0)
				Asset.bondedSprites.Add(new BondedSprite("ROOT", -1, 0));
			
			var treeViewState = new TreeViewState();
			var headerState = MultiColumnTreeView<BondedSprite>.CreateDefaultMultiColumnHeaderState(100);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(spritesHeaderState, headerState))
				MultiColumnHeaderState.OverwriteSerializedFields(spritesHeaderState, headerState);
			spritesHeaderState = headerState;
			
			
			multiColumnHeader = new MultiColumnHeader(headerState);
			multiColumnHeader.ResizeToFit();
			
			var spriteTreeModel = new TreeModel<BondedSprite>(Asset.bondedSprites);
			_spritesTreeView = new MultiColumnTreeView<BondedSprite>(treeViewState, multiColumnHeader, spriteTreeModel);
			_spritesTreeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
			_spritesTreeView.Reload();
			
			// Strings
			if (Asset.bondedStrings.Count == 0)
				Asset.bondedStrings.Add(new BondedString("ROOT", -1, 0));
			 
			var strTreeViewState = new TreeViewState();
			var strHeaderState = MultiColumnTreeView<BondedString>.CreateDefaultMultiColumnHeaderState(100);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(stringsHeaderState, strHeaderState))
				MultiColumnHeaderState.OverwriteSerializedFields(stringsHeaderState, strHeaderState);
			stringsHeaderState = strHeaderState;
			
			
			multiColumnHeader = new MultiColumnHeader(strHeaderState);
			multiColumnHeader.ResizeToFit();
			
			var strTreeModel = new TreeModel<BondedString>(Asset.bondedStrings);
			_stringsTreeView = new MultiColumnTreeView<BondedString>(strTreeViewState, multiColumnHeader, strTreeModel);
			_stringsTreeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
			_stringsTreeView.Reload();
		}

		void OnDisable()
		{
			Undo.undoRedoPerformed -= OnUndoRedoPerformed;
		}

		void OnUndoRedoPerformed()
		{
			if (_configsTreeView != null)
			{
				_configsTreeView.treeModel.SetData(Asset.bondedConfigs);
				_configsTreeView.Reload();
			}
			
			if (_spritesTreeView != null)
			{
				_spritesTreeView.treeModel.SetData(Asset.bondedSprites);
				_spritesTreeView.Reload();
			}
			
			if (_stringsTreeView != null)
			{
				_stringsTreeView.treeModel.SetData(Asset.bondedStrings);
				_stringsTreeView.Reload();
			}
		}

		void OnBeforeDroppingDraggedItems(IList<TreeViewItem> draggedRows)
		{
			Undo.RecordObject(Asset, $"Moving {draggedRows.Count} Item{(draggedRows.Count > 1 ? "s" : "")}");
		}

		public override void OnInspectorGUI()
		{
			const float topToolbarHeight = 20f;
			const float spacing = 2f;
			float totalHeight = 0f;
			
			// Configs ----------------------------------------------------------
			GUILayout.Label("Sprites bindings data");
			_configsTreeView.DrawToolBar(Asset);
			GUILayout.Space(3f);
			
			totalHeight = _configsTreeView.totalHeight + topToolbarHeight + 2 * spacing;
			Rect rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
			Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
			Rect multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
			_configsTreeView.DrawSearchBar(toolbarRect);
			_configsTreeView.OnGUI(multiColumnTreeViewRect);
			
			GUILayout.Space(10f);
			
			// Sprites ----------------------------------------------------------
			GUILayout.Label("Sprites bindings data");
			_spritesTreeView.DrawToolBar(Asset);
			GUILayout.Space(3f);
			
			totalHeight = _spritesTreeView.totalHeight + topToolbarHeight + 2 * spacing;
			rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
			toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
			multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
			_spritesTreeView.DrawSearchBar(toolbarRect);
			_spritesTreeView.OnGUI(multiColumnTreeViewRect);
			
			GUILayout.Space(10f);
			
			// Strings ----------------------------------------------------------
			GUILayout.Label("Strings bindings data");
			_stringsTreeView.DrawToolBar(Asset);
			GUILayout.Space(3f);
			
			totalHeight = _stringsTreeView.totalHeight + topToolbarHeight + 2 * spacing;
			rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
			toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
			multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
			_stringsTreeView.DrawSearchBar(toolbarRect);
			_stringsTreeView.OnGUI(multiColumnTreeViewRect);
		}
    }
}
