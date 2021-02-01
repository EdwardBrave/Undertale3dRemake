using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI
{
    public class ListLayout : SerializedMonoBehaviour
    {
        [ChildGameObjectsOnly]
        public GameObject prototype;

        [ChildGameObjectsOnly]
        public Transform container;
        
        private List<UIBinder> _items = new List<UIBinder>();

        public UIBinder this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public UIBinder Add(UIData uiData = null)
        {
            var binder = Instantiate(prototype, container ? container : transform).GetComponent<UIBinder>();
            _items.Add(binder);
            if (uiData)
                binder.LoadUIData(uiData);
            return binder;
        }

        public void RemoveAt(int index)
        {
            var item = _items[index];
            _items.RemoveAt(index);
            Destroy(item.gameObject);
        }
        public void Remove(UIBinder item) 
        {
            _items.Remove(item);
            Destroy(item.gameObject);
        } 

        public void AddRange(List<UIData> uiDataList)
        {
            foreach(var uiData in uiDataList)
                Add(uiData);
        }
        
        public void Reload(List<UIData> uiDataList)
        {
            foreach(var item in _items)
                Destroy(item.gameObject);
            _items.Clear();
            AddRange(uiDataList);
        }

        private void Start()
        {
            prototype.SetActive(false);
        }
    }
}
