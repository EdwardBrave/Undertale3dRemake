using System.Collections.Generic;
using UI.TreeDataModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UI
{
	[CustomEditor(typeof(UIBinder))]
	public class BindingsListEditor : Editor
	{
		MultiColumnTreeView<BondedUIBinder> _bindsTreeView;
		[SerializeField] MultiColumnHeaderState _bindsHeaderState;
		
		MultiColumnTreeView<BondedImage> _imagesTreeView;
		[SerializeField] MultiColumnHeaderState _imagesHeaderState;
		
		MultiColumnTreeView<BondedText> _textTreeView;
		[SerializeField] MultiColumnHeaderState _textsHeaderState;

		UIBinder Asset => (UIBinder) target;


		void OnEnable ()
		{
			Undo.undoRedoPerformed += OnUndoRedoPerformed;
			if (Asset.bondedBinders.Count == 0)
				Asset.bondedBinders.Add(new BondedUIBinder("ROOT", -1, 0));
			if (Asset.bondedImages.Count == 0)
				Asset.bondedImages.Add(new BondedImage("ROOT", -1, 0));
			if (Asset.bondedTexts.Count == 0)
				Asset.bondedTexts.Add(new BondedText("ROOT", -1, 0));

			
			// bondedBinders -------------------------------------------------------------------------------------------
			var treeViewState = new TreeViewState();
			var bindsHeaderState = MultiColumnTreeView<BondedUIBinder>.CreateDefaultMultiColumnHeaderState(100);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(_bindsHeaderState, bindsHeaderState))
				MultiColumnHeaderState.OverwriteSerializedFields(_bindsHeaderState, bindsHeaderState);
			_bindsHeaderState = bindsHeaderState;

			var multiColumnHeader = new MultiColumnHeader(bindsHeaderState);
			multiColumnHeader.ResizeToFit();
		
			var bindsTreeModel = new TreeModel<BondedUIBinder> (Asset.bondedBinders);
			_bindsTreeView = new MultiColumnTreeView<BondedUIBinder>(treeViewState, multiColumnHeader, bindsTreeModel);
			_bindsTreeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
			_bindsTreeView.Reload();
			
			// BondedImages --------------------------------------------------------------------------------------------
			treeViewState = new TreeViewState();
			var imgHeaderState = MultiColumnTreeView<BondedImage>.CreateDefaultMultiColumnHeaderState(100);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(_imagesHeaderState, imgHeaderState))
				MultiColumnHeaderState.OverwriteSerializedFields(_imagesHeaderState, imgHeaderState);
			_imagesHeaderState = imgHeaderState;

			multiColumnHeader = new MultiColumnHeader(imgHeaderState);
			multiColumnHeader.ResizeToFit();
		
			var imgTreeModel = new TreeModel<BondedImage> (Asset.bondedImages);
			_imagesTreeView = new MultiColumnTreeView<BondedImage>(treeViewState, multiColumnHeader, imgTreeModel);
			_imagesTreeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
			_imagesTreeView.Reload();
			
			// BondedTexts ---------------------------------------------------------------------------------------------
			treeViewState = new TreeViewState();
			var txtHeaderState = MultiColumnTreeView<BondedText>.CreateDefaultMultiColumnHeaderState(100);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(_textsHeaderState, txtHeaderState))
				MultiColumnHeaderState.OverwriteSerializedFields(_textsHeaderState, txtHeaderState);
			_textsHeaderState = txtHeaderState;

			multiColumnHeader = new MultiColumnHeader(txtHeaderState);
			multiColumnHeader.ResizeToFit();
		
			var txtTreeModel = new TreeModel<BondedText> (Asset.bondedTexts);
			_textTreeView = new MultiColumnTreeView<BondedText>(treeViewState, multiColumnHeader, txtTreeModel);
			_textTreeView.beforeDroppingDraggedItems += OnBeforeDroppingDraggedItems;
			_textTreeView.Reload();
		}

		void OnDisable()
		{
			Undo.undoRedoPerformed -= OnUndoRedoPerformed;
		}

		void OnUndoRedoPerformed()
		{
			if (_bindsTreeView != null)
			{
				_bindsTreeView.treeModel.SetData(Asset.bondedBinders);
				_bindsTreeView.Reload();
			}
			if (_imagesTreeView != null)
			{
				_imagesTreeView.treeModel.SetData(Asset.bondedImages);
				_imagesTreeView.Reload();
			}
			if (_textTreeView != null)
			{
				_textTreeView.treeModel.SetData(Asset.bondedTexts);
				_textTreeView.Reload();
			}
			
		}

		void OnBeforeDroppingDraggedItems(IList<TreeViewItem> draggedRows)
		{
			Undo.RecordObject (Asset, $"Moving {draggedRows.Count} Item{(draggedRows.Count > 1 ? "s" : "")}");
		}
		
		public override void OnInspectorGUI()
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Load UIConfig for test"))
					Asset.LoadTextUIConfig(Asset.testUIConfig);
				Rect rectUI = GUILayoutUtility.GetRect(0, 10000, 0, 24);
				Asset.testUIConfig = (UIConfig) EditorGUI.ObjectField(rectUI, GUIContent.none, Asset.testUIConfig,
					typeof(UIConfig), true);
			}
			
			const float topToolbarHeight = 20f;
			const float spacing = 2f;
			float totalHeight = 0f;
			
			GUILayout.Space(10f);
			
			// bondedBinders -------------------------------------------------------------------------------------------
			GUILayout.Label("UIBinders bonded links");
			_bindsTreeView.DrawToolBar(Asset);
			GUILayout.Space(3f);
			
			totalHeight = _bindsTreeView.totalHeight + topToolbarHeight + 2 * spacing;
			Rect rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
			Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
			Rect multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
			_bindsTreeView.DrawSearchBar(toolbarRect);
			_bindsTreeView.OnGUI(multiColumnTreeViewRect);
			
			GUILayout.Space(10f);
			
			// BondedImages --------------------------------------------------------------------------------------------
			GUILayout.Label("Images bonded links");
			_imagesTreeView.DrawToolBar(Asset);
			GUILayout.Space(3f);
			
			totalHeight = _imagesTreeView.totalHeight + topToolbarHeight + 2 * spacing;
			rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
			toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
			multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
			_imagesTreeView.DrawSearchBar(toolbarRect);
			_imagesTreeView.OnGUI(multiColumnTreeViewRect);
			
			GUILayout.Space(10f);
			
			// BondedTexts ---------------------------------------------------------------------------------------------
			GUILayout.Label("Texts bonded links");
			_textTreeView.DrawToolBar(Asset);
			GUILayout.Space(3f);
			
			totalHeight = _textTreeView.totalHeight + topToolbarHeight + 2 * spacing;
			rect = GUILayoutUtility.GetRect(0, 10000, 0, totalHeight);
			toolbarRect = new Rect(rect.x, rect.y, rect.width, topToolbarHeight);
			multiColumnTreeViewRect = new Rect(rect.x, rect.y + topToolbarHeight + spacing, rect.width, rect.height - topToolbarHeight - 2 * spacing);
			_textTreeView.DrawSearchBar(toolbarRect);
			_textTreeView.OnGUI(multiColumnTreeViewRect);
		}
	}
}

