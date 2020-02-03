using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UI.TreeDataModel
{

	// The TreeModel is a utility class working on a list of serializable TreeElements where the order and the depth of each TreeElement define
	// the tree structure. Note that the TreeModel itself is not serializable (in Unity we are currently limited to serializing lists/arrays) but the 
	// input list is.
	// The tree representation (parent and children references) are then build internally using TreeElementUtility.ListToTree (using depth 
	// values of the elements). 
	// The first element of the input list is required to have depth == -1 (the hidden root) and the rest to have
	// depth >= 0 (otherwise an exception will be thrown)

	public class TreeModel<T> where T : TreeElement
	{
		IList<T> _data;
		T _root;
		int _maxId;

		public T root
		{
			get => _root;
			set => _root = value;
		}

		public event Action ModelChanged;

		public int NumberOfDataElements => _data.Count;

		public TreeModel(IList<T> data)
		{
			SetData(data);
		}

		public T Find(int id)
		{
			return _data.FirstOrDefault(element => element.id == id);
		}

		public void SetData(IList<T> data)
		{
			Init(data);
		}

		void Init(IList<T> data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data), "Input data is null. Ensure input is a non-null list.");

			_data = data;
			_maxId = 0;
			if (_data.Count > 0)
			{
				_root = TreeElementUtility.ListToTree(data);
				_maxId = _data.Max(e => e.id);
			}
		}

		public int GenerateUniqueId()
		{
			return ++_maxId;
		}

		public IList<int> GetAncestors(int id)
		{
			var parents = new List<int>();
			TreeElement T = Find(id);
			if (T != null)
			{
				while (T.parent != null)
				{
					parents.Add(T.parent.id);
					T = T.parent;
				}
			}

			return parents;
		}

		public IList<int> GetDescendantsThatHaveChildren(int id)
		{
			T searchFromThis = Find(id);
			if (searchFromThis != null)
			{
				return GetParentsBelowStackBased(searchFromThis);
			}

			return new List<int>();
		}

		IList<int> GetParentsBelowStackBased(TreeElement searchFromThis)
		{
			Stack<TreeElement> stack = new Stack<TreeElement>();
			stack.Push(searchFromThis);

			var parentsBelow = new List<int>();
			while (stack.Count > 0)
			{
				TreeElement current = stack.Pop();
				if (current.HasChildren)
				{
					parentsBelow.Add(current.id);
					foreach (var T in current.children)
					{
						stack.Push(T);
					}
				}
			}
			return parentsBelow;
		}

		public void RemoveElements(IList<int> elementIDs)
		{
			IList<T> elements = _data.Where(element => elementIDs.Contains(element.id)).ToArray();
			RemoveElements(elements);
		}

		public void RemoveElements(IList<T> elements)
		{
			foreach (var element in elements)
				if (element == _root)
					throw new ArgumentException("It is not allowed to remove the root element");

			var commonAncestors = TreeElementUtility.FindCommonAncestorsWithinList(elements);

			foreach (var element in commonAncestors)
			{
				element.parent.children.Remove(element);
				element.parent = null;
			}

			TreeElementUtility.TreeToList(_root, _data);

			Changed();
		}

		public void AddElements(IList<T> elements, TreeElement parent, int insertPosition)
		{
			if (elements == null)
				throw new ArgumentNullException(nameof(elements), "elements is null");
			if (elements.Count == 0)
				throw new ArgumentNullException(nameof(elements), "elements Count is 0: nothing to add");
			if (parent == null)
				throw new ArgumentNullException(nameof(parent), "parent is null");

			if (parent.children == null)
				parent.children = new List<TreeElement>();

			parent.children.InsertRange(insertPosition, elements);
			foreach (var element in elements)
			{
				element.parent = parent;
				element.depth = parent.depth + 1;
				TreeElementUtility.UpdateDepthValues(element);
			}

			TreeElementUtility.TreeToList(_root, _data);

			Changed();
		}

		public void AddRoot(T root)
		{
			if (root == null)
				throw new ArgumentNullException(nameof(root), "root is null");

			if (_data == null)
				throw new InvalidOperationException("Internal Error: data list is null");

			if (_data.Count != 0)
				throw new InvalidOperationException("AddRoot is only allowed on empty data list");

			root.id = GenerateUniqueId();
			root.depth = -1;
			_data.Add(root);
		}

		public void AddElement(T element, TreeElement parent, int insertPosition)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element), "element is null");
			if (parent == null)
				throw new ArgumentNullException(nameof(parent), "parent is null");

			if (parent.children == null)
				parent.children = new List<TreeElement>();

			parent.children.Insert(insertPosition, element);
			element.parent = parent;

			TreeElementUtility.UpdateDepthValues(parent);
			TreeElementUtility.TreeToList(_root, _data);

			Changed();
		}

		public void MoveElements(TreeElement parentElement, int insertionIndex, List<TreeElement> elements)
		{
			if (insertionIndex < 0)
				throw new ArgumentException(
					"Invalid input: insertionIndex is -1, client needs to decide what index elements should be reparented at");

			// Invalid reparenting input
			if (parentElement == null)
				return;

			// We are moving items so we adjust the insertion index to accomodate that any items above the insertion index is removed before inserting
			if (insertionIndex > 0)
				insertionIndex -= parentElement.children.GetRange(0, insertionIndex).Count(elements.Contains);

			// Remove draggedItems from their parents
			foreach (var draggedItem in elements)
			{
				draggedItem.parent.children.Remove(draggedItem); // remove from old parent
				draggedItem.parent = parentElement; // set new parent
			}

			if (parentElement.children == null)
				parentElement.children = new List<TreeElement>();

			// Insert dragged items under new parent
			parentElement.children.InsertRange(insertionIndex, elements);

			TreeElementUtility.UpdateDepthValues(root);
			TreeElementUtility.TreeToList(_root, _data);

			Changed();
		}

		void Changed()
		{
			ModelChanged?.Invoke();
		}
	}

}