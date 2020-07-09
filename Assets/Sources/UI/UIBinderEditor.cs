#if UNITY_EDITOR
using System.Collections.Generic;
using UI.BondedElements;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UI
{
	[CustomEditor(typeof(UIBinder))]
	public class UIBinderEditor : Editor
	{
		MultiColumnTreeView<BondedUIBinder> _bindsTreeView;

		MultiColumnTreeView<BondedImage> _imagesTreeView;

		MultiColumnTreeView<BondedText> _textsTreeView;

		MultiColumnTreeView<BondedField> _fieldsTreeView;

		UIBinder Asset => (UIBinder) target;


		void OnEnable ()
		{
			Undo.undoRedoPerformed += OnUndoRedoPerformed;
			
			// bondedBinders -------------------------------------------------------------------------------------------
			if (Asset.bondedBinders.Count == 0)
				Asset.bondedBinders.Add(new BondedUIBinder("ROOT", -1, 0));
			_bindsTreeView = TreeViewInit(Asset.bondedBinders);
			
			// BondedImages --------------------------------------------------------------------------------------------
			if (Asset.bondedImages.Count == 0)
				Asset.bondedImages.Add(new BondedImage("ROOT", -1, 0));
			_imagesTreeView = TreeViewInit(Asset.bondedImages);
			
			// BondedTexts ---------------------------------------------------------------------------------------------
			if (Asset.bondedTexts.Count == 0)
				Asset.bondedTexts.Add(new BondedText("ROOT", -1, 0));
			_textsTreeView = TreeViewInit(Asset.bondedTexts);
			
			// BondedTexts ---------------------------------------------------------------------------------------------
			if (Asset.bondedFields.Count == 0)
				Asset.bondedFields.Add(new BondedField("ROOT", -1, 0));
			_fieldsTreeView = TreeViewInit(Asset.bondedFields);
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

		private void OnDisable()
		{
			if (target)
				EditorUtility.SetDirty(target);
			Undo.undoRedoPerformed -= OnUndoRedoPerformed;
		}

		private void OnUndoRedoPerformed()
		{
			Perform(_bindsTreeView, Asset.bondedBinders);
			Perform(_imagesTreeView, Asset.bondedImages);
			Perform(_textsTreeView, Asset.bondedTexts);
			Perform(_fieldsTreeView, Asset.bondedFields);
		}
		
		private static void Perform<T>(MultiColumnTreeView<T> treeView, IList<T> boundedData) where T: MultiColumnTreeElement, new()
		{
			if (treeView == null) return;
			treeView.treeModel.SetData(boundedData);
			treeView.Reload();
		}

		void OnBeforeDroppingDraggedItems(IList<TreeViewItem> draggedRows)
		{
			Undo.RecordObject (Asset, $"Moving {draggedRows.Count} Item{(draggedRows.Count > 1 ? "s" : "")}");
		}

		public override void OnInspectorGUI()
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Load UIData for test"))
					Asset.LoadUIData(Asset.loadedUIData);
				Rect rectUI = GUILayoutUtility.GetRect(0, 10000, 0, 24);
				Asset.loadedUIData = (UIData) EditorGUI.ObjectField(rectUI, GUIContent.none, Asset.loadedUIData,
					typeof(UIData), true);
			}

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField("isAutoSearch");
				Asset.isAutoSearch = EditorGUILayout.Toggle(Asset.isAutoSearch);
			}
			
			if (Asset.isAutoSearch)
				return;
			
			if (GUILayout.Button("Find all potential bindings"))
			{
				Asset.FindBindings();
				if (Asset.bondedBinders.Count > 0)
					Asset.isShowBinders = true;
				if (Asset.bondedImages.Count > 0)
					Asset.isShowImages = true;
				if (Asset.bondedTexts.Count > 0)
					Asset.isShowTexts = true;
				if (Asset.bondedFields.Count > 0)
					Asset.isShowFields = true;
				_bindsTreeView = TreeViewInit(Asset.bondedBinders);
				_imagesTreeView = TreeViewInit(Asset.bondedImages);
				_textsTreeView = TreeViewInit(Asset.bondedTexts);
				_fieldsTreeView = TreeViewInit(Asset.bondedFields);
			}

			GUILayout.Space(10f);
			// bondedBinders -------------------------------------------------------------------------------------------
			Asset.isShowBinders = EditorGUILayout.Foldout(Asset.isShowBinders, "UIBinders bonded links");
			if (Asset.isShowBinders)
				DrawTreeView(_bindsTreeView, Asset);
			// BondedImages --------------------------------------------------------------------------------------------
			Asset.isShowImages = EditorGUILayout.Foldout(Asset.isShowImages, "Images bonded links");
			if (Asset.isShowImages)
				DrawTreeView(_imagesTreeView, Asset);
			// BondedTexts ---------------------------------------------------------------------------------------------
			Asset.isShowTexts = EditorGUILayout.Foldout(Asset.isShowTexts, "Texts bonded links");
			if (Asset.isShowTexts)
				DrawTreeView(_textsTreeView, Asset);
			// BondedFields --------------------------------------------------------------------------------------------
			Asset.isShowFields = EditorGUILayout.Foldout(Asset.isShowFields, "Fields bonded links");
			if (Asset.isShowFields)
				DrawTreeView(_fieldsTreeView, Asset);
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
