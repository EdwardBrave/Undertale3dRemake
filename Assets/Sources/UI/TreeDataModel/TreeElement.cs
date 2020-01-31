using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.TreeDataModel
{

	[Serializable]
	public class TreeElement
	{
		[SerializeField] public int id;
		[SerializeField] public string name;
		[SerializeField] public int depth;
		[NonSerialized] public TreeElement parent;
		[NonSerialized] public List<TreeElement> children;

		public bool HasChildren => children != null && children.Count > 0;

		public TreeElement()
		{
		}

		public TreeElement(string name, int depth, int id)
		{
			this.id = id;
			this.name = name;
			this.depth = depth;
		}

	}
}